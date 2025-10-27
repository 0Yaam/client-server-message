using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhomMMT.Backend.OL
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Text { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;

        public Message(string convId, string senderId, string text)
        {
            ConversationId = convId;
            SenderId = senderId;
            Text = text;
        }
    }

}
