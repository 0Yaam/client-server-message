using System;

namespace Shared.OL
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string ConversationId { get; set; }
        public string SenderUsername { get; set; }
        public string Text { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;

        public Message() { }

        public Message(string conversationId, string senderUsername, string text)
        {
            ConversationId = conversationId;
            SenderUsername = senderUsername;
            Text = text;
            SentTime = DateTime.Now;
        }
    }
}
