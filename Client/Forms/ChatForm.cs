using Client.Services;
using Newtonsoft.Json;
using Shared.OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Client.Forms
{
    public partial class ChatForm : Form
    {
        private readonly Account _me;
        private readonly TcpService _tcp;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private string _currentPeer = null;
        private readonly System.Windows.Forms.Timer _listTimer = new System.Windows.Forms.Timer { Interval = 2000 };

        // Conversation storage so UI can be rebuilt without losing history
        private class ConversationMessage
        {
            public bool IsOutgoing { get; set; }
            public string Sender { get; set; }    // null for outgoing, sender username for incoming
            public string Text { get; set; }
            public DateTime Timestamp { get; set; }
        }
        private readonly Dictionary<string, List<ConversationMessage>> _conversations = new Dictionary<string, List<ConversationMessage>>();

        public ChatForm(Account me, TcpService tcp)
        {
            InitializeComponent();
            _me = me ?? throw new ArgumentNullException(nameof(me));
            _tcp = tcp ?? throw new ArgumentNullException(nameof(tcp));

            flpMessages.WrapContents = false;
            flpMessages.AutoScroll = true;
            flpMessages.FlowDirection = FlowDirection.TopDown;

            txtMessage.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && !e.Shift)
                {
                    e.SuppressKeyPress = true;
                    btnSend.PerformClick();
                }
            };

            this.Load += async (s, e) => { try { await _tcp.SendAsync(new { type = "LIST" }); } catch { } };

            _listTimer.Tick += async (s, e) => { try { await _tcp.SendAsync(new { type = "LIST" }); } catch { } };
            _listTimer.Start();

            _ = Task.Run(ListenLoop);
        }
        public ChatForm() : this(new Account("demo", "", "", UserRole.User), null) { }

        private async Task ListenLoop()
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    var line = await _tcp.ReadLineAsync(_cts.Token);
                    if (line == null) break;

                    System.Diagnostics.Debug.WriteLine("RECV: " + line);

                    dynamic msg = JsonConvert.DeserializeObject(line);
                    string type = (string)msg.type;

                    if (type == "LIST_OK")
                    {
                        var arr = ((Newtonsoft.Json.Linq.JArray)msg.users).ToObject<string[]>();
                        BeginInvoke(new Action(() =>
                        {
                            RenderUserList(arr);
                            if (arr.Length > 0 && string.IsNullOrEmpty(_currentPeer))
                                SelectPeer(arr[0]);
                        }));
                    }
                    else if (type == "MSG_RECV")
                    {
                        string from = (string)msg.from;
                        string text = (string)msg.message;
                        BeginInvoke(new Action(() => AppendIncoming(from, text)));
                    }
                    else if (type == "MSG_SENT")
                    {
                        string text = (string)msg.message;
                        BeginInvoke(new Action(() => AppendOutgoing(text)));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ListenLoop error: " + ex.Message);
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            var text = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            if (string.IsNullOrEmpty(_currentPeer))
            {
                MessageBox.Show("Nothing happend");
                return;
            }

            try
            {
                await _tcp.SendAsync(new
                {
                    type = "MSG_TO",
                    to = _currentPeer,
                    message = text
                });
                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch
            {
                MessageBox.Show("Không gửi được tin, kiểm tra kết nối.");
            }
        }

        private void SelectPeer(string username)
        {
            // If same user selected again, do nothing (preserve view)
            if (string.Equals(_currentPeer, username, StringComparison.Ordinal))
                return;

            _currentPeer = username;
            if (lblHeader != null) lblHeader.Text = "Chat với: " + username;

            // Rebuild message view from stored conversation (if any)
            flpMessages.Controls.Clear();
            if (!string.IsNullOrEmpty(username) && _conversations.TryGetValue(username, out var list))
            {
                foreach (var m in list)
                {
                    var bubble = new Controls.MessageBubbleControl
                    {
                        IsOutgoing = m.IsOutgoing,
                        MessageText = m.IsOutgoing ? m.Text : (string.IsNullOrEmpty(m.Sender) ? m.Text : $"[{m.Sender}] {m.Text}"),
                        Timestamp = m.Timestamp
                    };
                    flpMessages.Controls.Add(bubble);
                    bubble.UpdateLayoutBubble();
                }

                if (flpMessages.Controls.Count > 0)
                    flpMessages.ScrollControlIntoView(flpMessages.Controls[flpMessages.Controls.Count - 1]);
            }
        }

        private void RenderUserList(string[] users)
        {
            // tránh layout thrash
            flpUsers.SuspendLayout();
            try
            {
                flpUsers.Controls.Clear();

                foreach (var u in users)
                {
                    // compute preview from stored conversation if present
                    string preview = "Nhấn để chat";
                    DateTime time = DateTime.Now;
                    if (_conversations.TryGetValue(u, out var conv) && conv.Count > 0)
                    {
                        var last = conv[conv.Count - 1];
                        // preview text: prefix when outgoing
                        preview = last.IsOutgoing ? $"Bạn: {last.Text}" : last.Text;
                        time = last.Timestamp;
                    }

                    var item = new Controls.ChatListItemControl
                    {
                        Width = flpUsers.ClientSize.Width - 6,
                        Margin = new Padding(3, 0, 3, 2)
                    };
                    item.Bind(username: u, displayName: u, lastMessage: preview, time: time);

                    item.ItemClicked += (s, e) =>
                    {
                        foreach (Control c in flpUsers.Controls)
                            if (c is Controls.ChatListItemControl it) it.SetSelected(false);

                        item.SetSelected(true);
                        SelectPeer(item.Username);
                    };

                    flpUsers.Controls.Add(item);
                }
            }
            finally
            {
                flpUsers.ResumeLayout();
            }
        }

        private void UpdateListItemLastMsg(string username, string text)
        {
            foreach (Control c in flpUsers.Controls)
            {
                var item = c as Controls.ChatListItemControl;
                if (item != null && item.Username == username)
                {
                    item.LastMessage = text;
                    item.Time = DateTime.Now;
                    break;
                }
            }
        }

        private void AddMessageToConversation(string username, bool isOutgoing, string text, DateTime ts, string sender = null)
        {
            if (string.IsNullOrEmpty(username)) return;
            if (!_conversations.TryGetValue(username, out var list))
            {
                list = new List<ConversationMessage>();
                _conversations[username] = list;
            }
            list.Add(new ConversationMessage
            {
                IsOutgoing = isOutgoing,
                Sender = sender,
                Text = text,
                Timestamp = ts
            });

            // update the list preview
            UpdateListItemLastMsg(username, isOutgoing ? $"Bạn: {text}" : text);
        }

        private void AppendIncoming(string from, string text)
        {
            var now = DateTime.Now;
            // Store conversation
            AddMessageToConversation(from, false, text, now, sender: from);

            // If currently viewing this peer, show bubble immediately
            if (string.Equals(_currentPeer, from, StringComparison.Ordinal))
            {
                var bubble = new Controls.MessageBubbleControl
                {
                    IsOutgoing = false,
                    MessageText = $"[{from}] {text}",
                    Timestamp = now
                };
                flpMessages.Controls.Add(bubble);
                bubble.UpdateLayoutBubble();
                flpMessages.ScrollControlIntoView(bubble);
            }

            // Ensure the user list preview updated (AddMessageToConversation already updated it)
        }

        private void AppendOutgoing(string text)
        {
            var now = DateTime.Now;
            // Add to conversation storage for current peer
            if (!string.IsNullOrEmpty(_currentPeer))
            {
                AddMessageToConversation(_currentPeer, true, text, now, sender: null);
            }

            // Show bubble if viewing the peer
            if (!string.IsNullOrEmpty(_currentPeer))
            {
                var bubble = new Controls.MessageBubbleControl
                {
                    IsOutgoing = true,
                    MessageText = text,
                    Timestamp = now
                };
                flpMessages.Controls.Add(bubble);
                bubble.UpdateLayoutBubble();
                flpMessages.ScrollControlIntoView(bubble);
            }
        }


        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            try { _listTimer.Stop(); } catch { }
            _cts.Cancel();
            try { await _tcp?.CloseAsync(); } catch { }
            base.OnFormClosing(e);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
