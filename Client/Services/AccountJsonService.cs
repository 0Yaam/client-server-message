using Newtonsoft.Json;
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

            string path = null;
            // Ưu tiên Server/bin/Debug/Data/users.json (danh sách chính)
            try
            {
                var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                for (int i = 0; i < 6 && dir != null; i++)
                {
                    var candidates = new[]
                    {
                        Path.Combine(dir.FullName, "Server", "bin", "Debug", "Data", "users.json"),
                        Path.Combine(dir.FullName, "Server", "bin", "Release", "Data", "users.json"),
                    };
                    foreach (var c in candidates)
                    {
                        if (File.Exists(c)) { path = c; break; }
                    }
                    if (path != null) break;
                    dir = dir.Parent;
                }
            }
            catch { }

            // Fallback sang Client/Data nếu không tìm thấy Server
            if (path == null || !File.Exists(path))
            {
                path = JsonPath;
            }

            if (!File.Exists(path))
            {
                _accounts = new List<Account>();
                loaded = true;
                return;
            }

            string json = File.ReadAllText(path, Encoding.UTF8);
            _accounts = JsonConvert.DeserializeObject<List<Account>>(json) ?? new List<Account>();
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
