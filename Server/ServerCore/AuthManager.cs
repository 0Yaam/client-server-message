using Newtonsoft.Json;
using Shared.OL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
    new Account("admin", "123", "", UserRole.Admin, "Quản trị"),
    new Account("user",  "123", "", UserRole.User,  "Thành viên")
};

                File.WriteAllText(_filePath, JsonConvert.SerializeObject(seed, Formatting.Indented));
            }
            var json = File.ReadAllText(_filePath);
            _users = JsonConvert.DeserializeObject<List<Account>>(json) ?? new List<Account>();
        }

        public static bool UsernameExists(string username)
        {
            lock (_lock)
                return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public static bool CreateUser(string username, string password, string displayName, out Account acc)
        {
            acc = null;
            lock (_lock)
            {
                if (UsernameExists(username)) return false;

                var saltBytes = new byte[16];
                using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(saltBytes);
                var saltHex = BitConverter.ToString(saltBytes).Replace("-", "").ToLowerInvariant();
                var hash = Sha256Hex(saltHex + password);

                acc = new Account(username, hash, saltHex, UserRole.User, displayName); // ← set displayName
                _users.Add(acc);
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(_users, Formatting.Indented));
                return true;
            }
        }
        public static string GetDisplayName(string username)
        {
            lock (_lock)
            {
                var u = _users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                return u?.DisplayName ?? username;
            }
        }


        public static bool Validate(string username, string password, out Account acc)
        {
            lock (_lock)
            {
                acc = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (acc == null) return false;

                // nếu acc.Salt rỗng thì là seed cũ → cho pass thẳng để demo
                if (string.IsNullOrEmpty(acc.Salt))
                    return acc.PasswordHash == password;

                var check = Sha256Hex(acc.Salt + password);
                return string.Equals(acc.PasswordHash, check, StringComparison.OrdinalIgnoreCase);
            }
        }

        private static string Sha256Hex(string s)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }    
            
        }

    }
}
