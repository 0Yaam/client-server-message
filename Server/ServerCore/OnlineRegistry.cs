using Server.ServerCore;
using System.Collections.Generic;

namespace Server.ServerCore
{
    public static class OnlineRegistry
    {
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, ClientSession> _map = new Dictionary<string, ClientSession>();

        public static void Add(ClientSession s)
        {
            if (string.IsNullOrEmpty(s?.Username)) return;
            lock (_lock) _map[s.Username] = s;
        }

        public static void Remove(string username)
        {
            if (string.IsNullOrEmpty(username)) return;
            lock (_lock) _map.Remove(username);
        }

        public static string[] ListUsernames()
        {
            lock (_lock) { var arr = new string[_map.Count]; _map.Keys.CopyTo(arr, 0); return arr; }
        }

        public static ClientSession Get(string username)
        {
            lock (_lock) return _map.TryGetValue(username, out var s) ? s : null;
        }
    }
}
