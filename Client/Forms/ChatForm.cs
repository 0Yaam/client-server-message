using Client.Models;
using Client.Services;
using Newtonsoft.Json;
using Shared.OL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class ChatForm : Form
    {
        private readonly Account _me;
        private readonly TcpService _tcp;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private string _currentPeer = null;

        // Timer refresh LIST (giảm còn 4s cho nhẹ)
        private readonly System.Windows.Forms.Timer _listTimer = new System.Windows.Forms.Timer { Interval = 4000 };

        // Cờ đang vẽ list để tránh re-entrancy / click khi đang render
        private bool _renderingUsers = false;

        public ChatForm(Account me, TcpService tcp)
        {
            InitializeComponent();

            _me = me ?? throw new ArgumentNullException(nameof(me));
            _tcp = tcp ?? throw new ArgumentNullException(nameof(tcp));

            // Panel tin nhắn
            flpMessages.WrapContents = false;
            flpMessages.AutoScroll = true;
            flpMessages.FlowDirection = FlowDirection.TopDown;

            // Enter để gửi
            txtMessage.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && !e.Shift)
                {
                    e.SuppressKeyPress = true;
                    btnSend.PerformClick();
                }
            };

            // Gửi LIST một lần khi form Load (WinForms là "Load")
            this.Load += async (s, e) =>
            {
                try { await _tcp.SendAsync(new { type = "LIST" }); } catch { }
            };

            // Gửi LIST định kỳ; ListenLoop là nơi duy nhất đọc stream
            _listTimer.Tick += async (s, e) =>
            {
                if (_renderingUsers) return; // đang render list thì bỏ 1 nhịp
                try { await _tcp.SendAsync(new { type = "LIST" }); } catch { }
            };
            _listTimer.Start();

            // Lắng nghe tất cả phản hồi từ server tại một chỗ duy nhất
            _ = Task.Run(ListenLoop);
        }

        // KHÔNG dùng ctor rỗng cho runtime (để nguyên cho Designer)
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
                        try
                        {
                            var jarr = (Newtonsoft.Json.Linq.JArray)msg.users;

                            var list = new System.Collections.Generic.List<Client.Models.UserListItem>();

                            if (jarr.Count == 0 || jarr[0].Type == Newtonsoft.Json.Linq.JTokenType.String)
                            {
                                // Server trả string[]
                                var arr = jarr.ToObject<string[]>();
                                foreach (var uname in arr)
                                {
                                    list.Add(new Client.Models.UserListItem
                                    {
                                        Username = uname,
                                        DisplayName = uname,
                                        LastMessage = "Nhấn để chat",
                                        Time = null
                                    });
                                }
                            }
                            else
                            {
                                // Server trả object { username, displayName, lastMessage, time }
                                foreach (var it in jarr)
                                {
                                    var uname = (string)it["username"];
                                    var dname = (string)(it["displayName"] ?? uname);
                                    var last = (string)it["lastMessage"];
                                    DateTime? tm = null;
                                    if (it["time"] != null && it["time"].Type != Newtonsoft.Json.Linq.JTokenType.Null)
                                        tm = it["time"].ToObject<DateTime>();

                                    list.Add(new Client.Models.UserListItem
                                    {
                                        Username = uname,
                                        DisplayName = string.IsNullOrEmpty(dname) ? uname : dname,
                                        LastMessage = last,
                                        Time = tm
                                    });
                                }
                            }

                            BeginInvoke(new Action(() =>
                            {
                                RenderUserList(list);
                                if (list.Count > 0 && string.IsNullOrEmpty(_currentPeer))
                                    SelectPeer(list[0].Username);
                            }));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("LIST_OK parse error: " + ex.Message);
                        }
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

        // Chọn người nhận từ panel trái
        private void SelectPeer(string username)
        {
            // Nếu click lại người đang chọn thì không clear khung chat
            if (!string.IsNullOrEmpty(_currentPeer) &&
                string.Equals(_currentPeer, username, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _currentPeer = username;

            if (lblHeader != null)
                lblHeader.Text = "Chat với: " + username;

            // Demo: khi đổi người thì clear khung tin nhắn
            flpMessages.SuspendLayout();
            flpMessages.Controls.Clear();
            flpMessages.ResumeLayout();
        }

        // VẼ LẠI DANH SÁCH USER (string[] usernames)
        private void RenderUserList(List<UserListItem> users)
        {
            if (_renderingUsers) return;
            _renderingUsers = true;

            try
            {
                string selected = _currentPeer;

                flpUsers.SuspendLayout();
                flpUsers.Controls.Clear();

                foreach (var u in users)
                {
                    var item = new Controls.ChatListItemControl
                    {
                        Width = flpUsers.ClientSize.Width - 6,
                        Margin = new Padding(3, 0, 3, 2)
                    };

                    // 👇 Dùng đúng thuộc tính của u
                    item.Bind(
                        username: u.Username,
                        displayName: string.IsNullOrEmpty(u.DisplayName) ? u.Username : u.DisplayName,
                        lastMessage: string.IsNullOrEmpty(u.LastMessage) ? "Nhấn để chat" : u.LastMessage,
                        time: u.Time.HasValue ? u.Time.Value : DateTime.Now
                    );

                    item.ItemClicked += (s, e) =>
                    {
                        if (_renderingUsers) return;

                        foreach (Control c in flpUsers.Controls)
                        {
                            var it = c as Controls.ChatListItemControl;
                            if (it != null) it.SetSelected(false);
                        }

                        item.SetSelected(true);
                        BeginInvoke(new Action(() => SelectPeer(item.Username)));
                    };

                    if (!string.IsNullOrEmpty(selected) &&
                        string.Equals(u.Username, selected, StringComparison.OrdinalIgnoreCase))
                    {
                        item.SetSelected(true);
                    }

                    flpUsers.Controls.Add(item);
                }

                flpUsers.ResumeLayout();

                if (string.IsNullOrEmpty(_currentPeer) && flpUsers.Controls.Count > 0)
                {
                    var first = flpUsers.Controls[0] as Controls.ChatListItemControl;
                    if (first != null)
                    {
                        first.SetSelected(true);
                        _currentPeer = first.Username;
                        if (lblHeader != null) lblHeader.Text = "Chat với: " + _currentPeer;
                    }
                }
            }
            finally
            {
                _renderingUsers = false;
            }
        }

        private void UpdateListItemLastMsg(string username, string text)
        {
            foreach (Control c in flpUsers.Controls)
            {
                var item = c as Controls.ChatListItemControl;
                if (item != null && string.Equals(item.Username, username, StringComparison.OrdinalIgnoreCase))
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
                MessageText = "[" + from + "] " + text,
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
