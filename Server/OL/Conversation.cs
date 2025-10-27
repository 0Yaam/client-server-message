using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapNhomMMT.Backend.OL
{
    public class Conversation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        // Nếu không phải nhóm thì = ""
        public string GroupId { get; set; } = "";

        public List<string> MemberIds { get; set; } = new List<string>();

        public string LastMessagePreview { get; set; } = "";
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Chat 1-1
        public Conversation(string userA, string userB)
        {
            MemberIds.Add(userA);
            MemberIds.Add(userB);
        }

        // Chat nhóm
        public Conversation(string groupId)
        {
            GroupId = groupId;
        }
    }

}
