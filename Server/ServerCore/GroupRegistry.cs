using System;
using System.Collections.Generic;

namespace Server.ServerCore
{
    public class GroupInfo
    {
        public string GroupId { get; }
        public string Name { get; }
        public string[] Members { get; }

        public GroupInfo(string groupId, string name, string[] members)
        {
            GroupId = groupId;
            Name = name;
            Members = members ?? new string[0];
        }
    }

    public static class GroupRegistry
    {
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, GroupInfo> _map = new Dictionary<string, GroupInfo>();

        public static void Add(string groupId, string name, string[] members)
        {
            if (string.IsNullOrEmpty(groupId)) return;
            lock (_lock) _map[groupId] = new GroupInfo(groupId, name, members);
        }

        public static bool Contains(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) return false;
            lock (_lock) return _map.ContainsKey(groupId);
        }

        public static bool TryGet(string groupId, out GroupInfo info)
        {
            info = null;
            if (string.IsNullOrEmpty(groupId)) return false;
            lock (_lock) return _map.TryGetValue(groupId, out info);
        }

        public static bool Remove(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) return false;
            lock (_lock) return _map.Remove(groupId);
        }
    }
}