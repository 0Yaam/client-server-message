using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.ServerCore
{
    public static class MessageStore
    {
        private static readonly object _lock = new object();
        private static readonly List<Msg> _msgs = new List<Msg>();

        private class Msg
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Text { get; set; }
            public DateTime Time { get; set; }
        }

        // model trả về phù hợp .NET 4.7 + C# 7.3
        public class LastMsg
        {
            public string Text { get; set; }
            public DateTime? Time { get; set; }
        }

        public static void Add(string from, string to, string text)
        {
            lock (_lock)
            {
                _msgs.Add(new Msg { From = from, To = to, Text = text, Time = DateTime.UtcNow });

                if (_msgs.Count > 5000)
                    _msgs.RemoveRange(0, 1000);
            }
        }

        public static LastMsg GetLastBetween(string a, string b)
        {
            lock (_lock)
            {
                var m = _msgs.Where(x =>
                        (x.From == a && x.To == b) ||
                        (x.From == b && x.To == a))
                    .OrderByDescending(x => x.Time)
                    .FirstOrDefault();

                if (m == null) return new LastMsg { Text = null, Time = null };
                return new LastMsg { Text = m.Text, Time = m.Time };
            }
        }
    }
}
