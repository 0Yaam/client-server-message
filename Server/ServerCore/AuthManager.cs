using Newtonsoft.Json;
using Shared.OL;
using System;
using System.Collections.Generic;
using System.IO;

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
                acc = _users.Find(u => u.Username == username && u.PasswordHash == password);
                return acc != null;
            }
        }
    }
}
