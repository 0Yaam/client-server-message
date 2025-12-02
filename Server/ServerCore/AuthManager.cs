using Newtonsoft.Json;
using Shared.OL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Server.ServerCore
{
    public static class AuthManager
    {
        private static readonly object _lock = new object();
        private static List<Account> _users = new List<Account>();
        private static string _filePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json");

        public static void Init()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            if (!File.Exists(_filePath))
            {
                var seed = new List<Account>
                {
                    new Account("admin", "123", "", UserRole.Admin),
                    new Account("user",  "123", "", UserRole.User)
                };
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(seed, Formatting.Indented));
            }
            var json = File.ReadAllText(_filePath);
            _users = JsonConvert.DeserializeObject<List<Account>>(json) ?? new List<Account>();
        }

        public static bool Validate(string username, string password, out Account acc)
        {
            lock (_lock)
            {
                acc = _users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (acc == null) return false;

                if (string.IsNullOrEmpty(acc.Salt))
                {
                    return acc.PasswordHash == password;
                }

                string hashedPassword = HashPassword(password, acc.Salt);
                return acc.PasswordHash == hashedPassword;
            }
        }

        public static bool Register(string username, string displayName, string password, out string errorMessage)
        {
            lock (_lock)
            {
                errorMessage = "";

                // Kiểm tra username đã tồn tại chưa
                if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessage = "Tên đăng nhập đã tồn tại";
                    return false;
                }

                // Kiểm tra độ dài username
                if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
                {
                    errorMessage = "Tên đăng nhập phải có ít nhất 3 ký tự";
                    return false;
                }

                // Kiểm tra độ dài password
                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                {
                    errorMessage = "Mật khẩu phải có ít nhất 6 ký tự";
                    return false;
                }

                // Kiểm tra displayName
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    errorMessage = "Tên hiển thị không được để trống";
                    return false;
                }

                try
                {
                    // Tạo salt và hash password
                    string salt = GenerateSalt();
                    string passwordHash = HashPassword(password, salt);

                    // Tạo account mới
                    var newAccount = new Account(username, passwordHash, salt, UserRole.User)
                    {
                        DisplayName = displayName,
                        Avatar = string.Empty
                    };

                    // Thêm vào danh sách
                    _users.Add(newAccount);

                    // Lưu vào file
                    SaveUsers();

                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = "Lỗi khi tạo tài khoản: " + ex.Message;
                    return false;
                }
            }
        }

        public static bool ChangePassword(string username, string oldPassword, string newPassword, out string errorMessage)
        {
            lock (_lock)
            {
                errorMessage = string.Empty;

                var acc = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (acc == null)
                {
                    errorMessage = "Tài khoản không tồn tại";
                    return false;
                }

                // Validate old password
                if (!Validate(username, oldPassword, out var _))
                {
                    errorMessage = "Mật khẩu cũ không đúng";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    errorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự";
                    return false;
                }

                try
                {
                    var salt = GenerateSalt();
                    var hash = HashPassword(newPassword, salt);
                    acc.Salt = salt;
                    acc.PasswordHash = hash;
                    SaveUsers();
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = "Lỗi khi đổi mật khẩu: " + ex.Message;
                    return false;
                }
            }
        }

        // Admin forced set password without old password
        public static bool SetPassword(string username, string newPassword, out string errorMessage)
        {
            lock (_lock)
            {
                errorMessage = string.Empty;
                var acc = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (acc == null)
                {
                    errorMessage = "Tài khoản không tồn tại";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    errorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự";
                    return false;
                }

                try
                {
                    var salt = GenerateSalt();
                    var hash = HashPassword(newPassword, salt);
                    acc.Salt = salt;
                    acc.PasswordHash = hash;
                    SaveUsers();
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = "Lỗi khi đặt mật khẩu: " + ex.Message;
                    return false;
                }
            }
        }

        public static bool UpdateAvatar(string username, byte[] imageData, string ext, out string savedPath, out string errorMessage)
        {
            lock (_lock)
            {
                savedPath = string.Empty;
                errorMessage = string.Empty;

                var acc = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (acc == null)
                {
                    errorMessage = "Tài khoản không tồn tại";
                    return false;
                }

                try
                {
                    var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Avatars");
                    Directory.CreateDirectory(dir);
                    var fileName = username + (string.IsNullOrEmpty(ext) ? ".png" : ext.StartsWith(".") ? ext : "." + ext);
                    var path = Path.Combine(dir, fileName);
                    File.WriteAllBytes(path, imageData);

                    acc.Avatar = path;
                    SaveUsers();

                    savedPath = path;
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = "Lỗi lưu avatar: " + ex.Message;
                    return false;
                }
            }
        }

        private static string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[32];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string saltedPassword = salt + password;
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static void SaveUsers()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_users, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        public static Account[] GetAllUsers()
        {
            lock (_lock)
            {
                return _users.ToArray();
            }
        }

        public static Account GetUser(string username)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static bool UpdateUser(string originalUsername, string newUsername, string displayName, UserRole role, out string errorMessage)
        {
            lock (_lock)
            {
                errorMessage = string.Empty;
                var acc = _users.FirstOrDefault(u => u.Username.Equals(originalUsername, StringComparison.OrdinalIgnoreCase));
                if (acc == null)
                {
                    errorMessage = "Tài khoản không tồn tại";
                    return false;
                }

                // If changing username, ensure new username not already taken (unless same as original)
                if (!string.Equals(originalUsername, newUsername, StringComparison.OrdinalIgnoreCase))
                {
                    if (_users.Any(u => u.Username.Equals(newUsername, StringComparison.OrdinalIgnoreCase)))
                    {
                        errorMessage = "Tên đăng nhập mới đã tồn tại";
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(newUsername) || newUsername.Length < 3)
                    {
                        errorMessage = "Tên đăng nhập phải có ít nhất 3 ký tự";
                        return false;
                    }
                }

                if (string.IsNullOrWhiteSpace(displayName))
                {
                    errorMessage = "Tên hiển thị không được để trống";
                    return false;
                }

                try
                {
                    // apply changes
                    acc.DisplayName = displayName;
                    acc.Role = role;

                    if (!string.Equals(originalUsername, newUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        acc.Username = newUsername;
                    }

                    SaveUsers();
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = "Lỗi khi cập nhật tài khoản: " + ex.Message;
                    return false;
                }
            }
        }
    }
}
