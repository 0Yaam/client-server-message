using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Forms.Controls
{
    public partial class ChatListItemControl : UserControl
    {
        public string DisplayName { get; set; }
        public string LastMessage { get; set; }
        public DateTime Time { get; set; }
        public string ConversationId { get; set; }

        public ChatListItemControl()
        {
            InitializeComponent();
        }
    }
}
