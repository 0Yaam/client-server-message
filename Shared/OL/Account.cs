using System;

namespace Shared.OL
{
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }

    public class Account
    {
        public string Username { get; set; }       // tên đăng nhập
        public string PasswordHash { get; set; }   // sha256 salt + pass
        public string Salt { get; set; }           // chuỗi salt lưu dạng hex
        public UserRole Role { get; set; }         // phân quyền
        public string DisplayName { get; set; }    // tên hiển thị
        public string Avatar { get; set; }         // đường dẫn avatar

        public Account() { }

        public Account(string username, string passwordHash, string salt, UserRole role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
            Role = role;
            DisplayName = username; // Mặc định displayname = username
            Avatar = "default.png"; // Avatar mặc định
        }
    }
}
