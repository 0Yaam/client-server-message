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
        public string Password { get; set; }       // mật khẩu
        public UserRole Role { get; set; }         // phân quyền
        public string DisplayName { get; set; }    // tên hiển thị
        public string Avatar { get; set; }         // đường dẫn avatar

        public Account() { }

        public Account(string username, string password, UserRole role)
        {
            Username = username;
            Password = password;
            Role = role;
            DisplayName = username; // Mặc định displayname = username
            Avatar = "default.png"; // Avatar mặc định
        }
    }
}
