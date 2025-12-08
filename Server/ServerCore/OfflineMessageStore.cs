using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.ServerCore
{
    public class OfflineMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string GroupId { get; set; }     }

    public static class OfflineMessageStore
    {
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, List<OfflineMessage>> _pendingMessages = 
            new Dictionary<string, List<OfflineMessage>>(StringComparer.OrdinalIgnoreCase);

        public static void Store(string toUsername, string fromUsername, string message, string groupId = null)
        {
            if (string.IsNullOrEmpty(toUsername) || string.IsNullOrEmpty(message)) return;

            lock (_lock)
            {
                if (!_pendingMessages.TryGetValue(toUsername, out var list))
                {
                    list = new List<OfflineMessage>();
                    _pendingMessages[toUsername] = list;
                }

                list.Add(new OfflineMessage
                {
                    From = fromUsername,
                    To = toUsername,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    GroupId = groupId
                });
            }
        }

        public static List<OfflineMessage> GetAndClear(string username)
        {
            if (string.IsNullOrEmpty(username)) return new List<OfflineMessage>();

            lock (_lock)
            {
                if (_pendingMessages.TryGetValue(username, out var list))
                {
                    _pendingMessages.Remove(username);
                    return list.OrderBy(m => m.Timestamp).ToList();
                }
                return new List<OfflineMessage>();
            }
        }
        public static bool HasPendingMessages(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;

            lock (_lock)
            {
                return _pendingMessages.TryGetValue(username, out var list) && list.Count > 0;
            }
        }

        public static int GetPendingCount(string username)
        {
            if (string.IsNullOrEmpty(username)) return 0;

            lock (_lock)
            {
                if (_pendingMessages.TryGetValue(username, out var list))
                {
                    return list.Count;
                }
                return 0;
            }
        }
    }
}
