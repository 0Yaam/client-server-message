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
                MessageBox.Show("Chọn người nhận ở panel trái trước đã.");
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
            }
            catch
            {
                MessageBox.Show("Không gửi được tin, kiểm tra kết nối.");
            }
        }

        private void SelectPeer(string username)
        {
            _currentPeer = username;
            if (lblHeader != null) lblHeader.Text = "Chat với: " + username;
            flpMessages.Controls.Clear();  
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
                    var item = new Controls.ChatListItemControl
                    {
                        Width = flpUsers.ClientSize.Width - 6,
                        Margin = new Padding(3, 0, 3, 2)
                    };
                    item.Bind(username: u, displayName: u, lastMessage: "Nhấn để chat", time: DateTime.Now);

                    item.ItemClicked += (s, e) =>
                    {
                        // unselect hết
                        foreach (Control c in flpUsers.Controls)
                            if (c is Controls.ChatListItemControl it) it.SetSelected(false);

                        // select item này
                        item.SetSelected(true);
                        SelectPeer(item.Username); // set _currentPeer + clear messages (demo)
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

        private void AppendIncoming(string from, string text)
        {
            var bubble = new Controls.MessageBubbleControl
            {
                IsOutgoing = false,
                MessageText = $"[{from}] {text}",
                Timestamp = DateTime.Now
            };
            flpMessages.Controls.Add(bubble);
            bubble.UpdateLayoutBubble();
            flpMessages.ScrollControlIntoView(bubble);

            UpdateListItemLastMsg(from, text);
        }

        private void AppendOutgoing(string text)
        {
            var bubble = new Controls.MessageBubbleControl
            {
                IsOutgoing = true,
                MessageText = text,
                Timestamp = DateTime.Now
            };
            flpMessages.Controls.Add(bubble);
            bubble.UpdateLayoutBubble();
            flpMessages.ScrollControlIntoView(bubble);

            if (!string.IsNullOrEmpty(_currentPeer))
                UpdateListItemLastMsg(_currentPeer, text);
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
    }
}
