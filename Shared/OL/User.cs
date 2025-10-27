using System;

namespace Shared.OL
{
    public class User
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public UserRole Role { get; set; }

        public User() { }

        public User(string username, string displayName, UserRole role)
        {
            Username = username;
            DisplayName = displayName;
            Role = role;
        }
    }
}
