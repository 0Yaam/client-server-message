using Newtonsoft.Json;
using Shared.OL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Client.Services
{
    public static class AccountJsonService
    {
        private static List<Account> _accounts = new List<Account>();
        private static bool loaded = false;

        private static string JsonPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Data", "users.json");

        private static void Load()
        {
            if (loaded) return;

            if (!File.Exists(JsonPath))
            {
                throw new FileNotFoundException("File users.json không tồn tại!");
            }

            string json = File.ReadAllText(JsonPath, Encoding.UTF8);
            _accounts = JsonConvert.DeserializeObject<List<Account>>(json);
            loaded = true;
        }

        public static bool Login(string username, string password, out Account acc)
        {
            Load();
            acc = null;

            foreach (var a in _accounts)
            {
                if (a.Username == username && a.PasswordHash == password)
                {
                    acc = a;
                    return true;
                }
            }
            return false;
        }
    }
}
