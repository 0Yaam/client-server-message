using System;
using System.Collections.Generic;

namespace Shared.OL
{
    public class Conversation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        // Nếu là nhóm → GroupId có giá trị
        // Nếu 1-1 → GroupId = ""
        public string GroupId { get; set; } = "";

        public List<string> MemberUsernames { get; set; } = new List<string>();

        public Conversation() { }

        // Chat 1-1
        public Conversation(string userA, string userB)
        {
            MemberUsernames.Add(userA);
            MemberUsernames.Add(userB);
        }

        public Conversation(string groupId)
        {
            GroupId = groupId;
        }
    }
}
