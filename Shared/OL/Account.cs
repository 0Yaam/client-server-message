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
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public UserRole Role { get; set; }
        public string DisplayName { get; set; }   // ← thêm dòng này
        public Account() { }

        public Account(string username, string passwordHash, string salt, UserRole role, string displayName = null)
        {
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
            Role = role;
            DisplayName = displayName ?? username;
        }
    }
}
