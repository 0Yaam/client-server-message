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
using System.IO;
using System.Drawing;

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
            public string Sender { get; set; }
            public string Text { get; set; }
            public DateTime Timestamp { get; set; }
        }
        private readonly Dictionary<string, List<ConversationMessage>> _conversations = new Dictionary<string, List<ConversationMessage>>();
        private readonly Dictionary<string, string[]> _groupMembers = new Dictionary<string, string[]>();
        private string[] _latestUsers = Array.Empty<string>();

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

        private void EnsureUserInList(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username)) return;
                // If it's a group id, skip
                if (_groupMembers.ContainsKey(username)) return;

                bool exists = flpUsers.Controls.OfType<Controls.ChatListItemControl>().Any(i => i.Username == username);
                if (!exists)
                {
                    var item = new Controls.ChatListItemControl
                    {
                        Width = flpUsers.ClientSize.Width - 6,
                        Margin = new Padding(3, 0, 3, 2)
                    };
                    item.Bind(username: username, displayName: username, lastMessage: "Nhấn để chat", time: DateTime.Now);

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
            catch { }
        }

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
                        var arr = ((JArray)msg.users).ToObject<string[]>();
                        _latestUsers = arr ?? Array.Empty<string>();
                        BeginInvoke(new Action(() =>
                        {
                            RenderUserList(arr);
                            if (arr.Length > 0 && string.IsNullOrEmpty(_currentPeer))
                                SelectPeer(arr[0]);
                        }));
                    }
                    else if (type == "PASS_CHANGE_OK")
                    {
                        string message = (string)msg.message ?? "Đổi mật khẩu thành công";
                        BeginInvoke(new Action(() => MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    }
                    else if (type == "PASS_CHANGE_FAIL")
                    {
                        string reason = (string)msg.reason ?? "Đổi mật khẩu thất bại";
                        BeginInvoke(new Action(() => MessageBox.Show(reason, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    }
                    else if (type == "MSG_RECV")
                    {
                        string from = (string)msg.from;
                        string text = (string)msg.message;

                        // Ensure sender exists in user list (so admin/system senders like Zola appear)
                        BeginInvoke(new Action(() => EnsureUserInList(from)));

                        // Kiểm tra có phải tin nhắn nhóm không
                        string groupId = msg.groupId; // có thể null nếu là 1:1

                        BeginInvoke(new Action(() =>
                        {
                            if (!string.IsNullOrEmpty(groupId))
                            {
                                // Tin nhắn nhóm
                                AppendIncomingGroup(groupId, from, text);
                            }
                            else
                            {
                                // Tin nhắn 1:1
                                AppendIncoming(from, text);
                            }
                        }));
                    }
                    else if (type == "MSG_SENT")
                    {
                        string text = (string)msg.message;
                        string to = (string)msg.to;

                        BeginInvoke(new Action(() =>
                        {
                            // Kiểm tra xem có phải gửi tới nhóm không
                            if (_groupMembers.ContainsKey(to))
                            {
                                // Gửi tới nhóm
                                AppendOutgoingGroup(to, text);
                            }
                            else
                            {
                                // Gửi 1:1
                                AppendOutgoing(text);
                            }
                        }));
                    }
                    else if (type == "GROUP_CREATED")
                    {
                        string groupId = (string)msg.groupId;
                        string name = (string)msg.name;
                        var members = ((JArray)msg.members).ToObject<string[]>();
                        BeginInvoke(new Action(() =>
                        {
                            AddGroupToUserList(groupId, name, members);
                        }));
                    }
                    else if (type == "AVATAR_UPDATED")
                    {
                        // Receive avatar update broadcast from server
                        string username = (string)msg.username;
                        string b64 = (string)msg.image;
                        string ext = (string)msg.ext; // may include dot

                        try
                        {
                            var data = Convert.FromBase64String(b64);
                            var saved = SaveAvatarLocal(username, data, ext);

                            // update local account if matches
                            if (string.Equals(_me?.Username, username, StringComparison.OrdinalIgnoreCase))
                            {
                                _me.Avatar = saved;
                            }

                            // Update UI
                            BeginInvoke(new Action(() =>
                            {
                                foreach (Control c in flpUsers.Controls)
                                {
                                    if (c is Controls.ChatListItemControl it && it.Username == username)
                                    {
                                        try
                                        {
                                            using (var ms = new MemoryStream(data))
                                            {
                                                var img = Image.FromStream(ms);
                                                it.SetAvatar(new Bitmap(img));
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("AVATAR_UPDATED error: " + ex.Message);
                        }
                    }
                    else if (type == "MSG_RECV_IMAGE")
                    {
                        string from = (string)msg.from;
                        string b64 = (string)msg.image;
                        string ext = (string)msg.ext;

                        // Ensure sender exists in user list
                        BeginInvoke(new Action(() => EnsureUserInList(from)));

                        try
                        {
                            var data = Convert.FromBase64String(b64);
                            var now = DateTime.Now;
                            AddMessageToConversation(from, false, "[Image]", now, sender: from);

                            BeginInvoke(new Action(() =>
                            {
                                if (string.Equals(_currentPeer, from, StringComparison.Ordinal))
                                {
                                    try
                                    {
                                        using (var ms = new MemoryStream(data))
                                        {
                                            var img = Image.FromStream(ms);
                                            var bubble = new Controls.MessageBubbleControl
                                            {
                                                IsOutgoing = false,
                                                ImageContent = new Bitmap(img),
                                                Timestamp = now
                                            };
                                            flpMessages.Controls.Add(bubble);
                                            bubble.UpdateLayoutBubble();
                                            flpMessages.ScrollControlIntoView(bubble);
                                        }
                                    }
                                    catch { }
                                }
                            }));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("MSG_RECV_IMAGE error: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ListenLoop error: " + ex.Message);
            }
        }

        private string SaveAvatarLocal(string username, byte[] data, string ext)
        {
            try
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Avatars");
                Directory.CreateDirectory(dir);
                if (string.IsNullOrEmpty(ext)) ext = ".png";
                if (!ext.StartsWith(".")) ext = "." + ext;
                var file = Path.Combine(dir, username + ext);
                File.WriteAllBytes(file, data);
                return file;
            }
            catch { return string.Empty; }
        }

        private string FindLocalAvatar(string username)
        {
            try
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Avatars");
                if (!Directory.Exists(dir)) return null;
                var files = Directory.GetFiles(dir, username + ".*");
                if (files.Length == 0) return null;
                return files[0];
            }
            catch { return null; }
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

        // === SelectPeer: FIX để tránh clear khi click lại vào conversation hiện tại ===
        private void SelectPeer(string username)
        {
            // NẾU đã đang xem conversation này rồi, KHÔNG làm gì (tránh clear)
            if (string.Equals(_currentPeer, username, StringComparison.Ordinal))
                return;

            _currentPeer = username;
            bool isGroup = _groupMembers.ContainsKey(username);

            if (lblHeader != null)
            {
                if (isGroup)
                {
                    var item = flpUsers.Controls.OfType<Controls.ChatListItemControl>()
                        .FirstOrDefault(i => i.Username == username);
                    lblHeader.Text = item != null ? $"Nhóm: {item.DisplayName}" : $"Nhóm: {username}";
                }
                else
                {
                    lblHeader.Text = "Chat với: " + username;
                }
            }

            // Rebuild message view với format phù hợp
            flpMessages.Controls.Clear();
            if (!string.IsNullOrEmpty(username) && _conversations.TryGetValue(username, out var list))
            {
                foreach (var m in list)
                {
                    var bubble = new Controls.MessageBubbleControl
                    {
                        IsOutgoing = m.IsOutgoing,
                        Timestamp = m.Timestamp
                    };

                    // Format message dựa trên loại chat
                    if (isGroup)
                    {
                        // Tin nhắn nhóm
                        if (m.IsOutgoing)
                        {
                            bubble.MessageText = m.Text; // Tin nhắn của mình
                        }
                        else
                        {
                            bubble.MessageText = $"[{m.Sender}] {m.Text}"; // Tin nhắn từ người khác
                        }
                    }
                    else
                    {
                        // Tin nhắn 1:1
                        bubble.MessageText = m.Text;
                    }

                    flpMessages.Controls.Add(bubble);
                    bubble.UpdateLayoutBubble();
                }

                if (flpMessages.Controls.Count > 0)
                    flpMessages.ScrollControlIntoView(flpMessages.Controls[flpMessages.Controls.Count - 1]);
            }
        }

        private void RenderUserList(string[] users)
        {
            flpUsers.SuspendLayout();
            try
            {
                // Keep existing groups, only update users
                var existingGroups = flpUsers.Controls.OfType<Controls.ChatListItemControl>()
                    .Where(i => _groupMembers.ContainsKey(i.Username))
                    .ToList();

                flpUsers.Controls.Clear();

                // Re-add groups first
                foreach (var grp in existingGroups)
                {
                    flpUsers.Controls.Add(grp);
                }

                // Add users
                foreach (var u in users)
                {
                    string preview = "Nhấn để chat";
                    DateTime time = DateTime.Now;
                    if (_conversations.TryGetValue(u, out var conv) && conv.Count > 0)
                    {
                        var last = conv[conv.Count - 1];
                        preview = last.IsOutgoing ? $"Bạn: {last.Text}" : last.Text;
                        time = last.Timestamp;
                    }

                    var item = new Controls.ChatListItemControl
                    {
                        Width = flpUsers.ClientSize.Width - 6,
                        Margin = new Padding(3, 0, 3, 2)
                    };
                    item.Bind(username: u, displayName: u, lastMessage: preview, time: time);

                    // Try to load local avatar
                    var avatarPath = FindLocalAvatar(u);
                    if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
                    {
                        try
                        {
                            using (var fs = File.OpenRead(avatarPath))
                            {
                                var img = Image.FromStream(fs);
                                item.SetAvatar(new Bitmap(img));
                            }
                        }
                        catch { }
                    }

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

        // === Helper: thêm group vào user list (không duplicate) ===
        private void AddGroupToUserList(string groupId, string groupName, string[] members)
        {
            if (string.IsNullOrEmpty(groupId)) return;

            // Kiểm tra đã tồn tại chưa (tránh duplicate)
            foreach (Control c in flpUsers.Controls)
            {
                if (c is Controls.ChatListItemControl existing && existing.Username == groupId)
                {
                    // đã có rồi, chỉ cập nhật nếu cần
                    existing.DisplayName = groupName;
                    return;
                }
            }

            // Lưu members để client biết ai trong group
            _groupMembers[groupId] = members ?? new string[0];

            var item = new Controls.ChatListItemControl
            {
                Width = flpUsers.ClientSize.Width - 6,
                Margin = new Padding(3, 0, 3, 2)
            };
            item.Bind(username: groupId, displayName: groupName, lastMessage: "Nhóm chat mới", time: DateTime.Now);

            item.ItemClicked += (senderItem, args) =>
            {
                // Deselect all items
                foreach (Control c in flpUsers.Controls)
                    if (c is Controls.ChatListItemControl it) it.SetSelected(false);

                item.SetSelected(true);
                SelectPeer(item.Username);
            };

            flpUsers.Controls.Add(item);

            // Tạo conversation storage rỗng cho group
            if (!_conversations.ContainsKey(groupId))
                _conversations[groupId] = new List<ConversationMessage>();
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

            // Update preview trong ChatListItemControl
            string preview;
            bool isGroup = _groupMembers.ContainsKey(username);

            if (isGroup)
            {
                // Nhóm: hiển thị [Sender] hoặc "Bạn:"
                if (isOutgoing)
                {
                    preview = $"Bạn: {text}";
                }
                else
                {
                    preview = $"[{sender}] {text}";
                }
            }
            else
            {
                // 1:1: hiển thị "Bạn:" hoặc text trực tiếp
                preview = isOutgoing ? $"Bạn: {text}" : text;
            }

            UpdateListItemLastMsg(username, preview);
        }

        private void AppendIncoming(string from, string text)
        {
            var now = DateTime.Now;
            AddMessageToConversation(from, false, text, now, sender: from);

            if (string.Equals(_currentPeer, from, StringComparison.Ordinal))
            {
                var bubble = new Controls.MessageBubbleControl
                {
                    IsOutgoing = false,
                    MessageText = text,  // 1:1 không cần [Sender] vì đã biết ai gửi
                    Timestamp = now
                };
                flpMessages.Controls.Add(bubble);
                bubble.UpdateLayoutBubble();
                flpMessages.ScrollControlIntoView(bubble);
            }
        }

        private void AppendOutgoing(string text)
        {
            var now = DateTime.Now;
            if (!string.IsNullOrEmpty(_currentPeer) && !_groupMembers.ContainsKey(_currentPeer))
            {
                AddMessageToConversation(_currentPeer, true, text, now, sender: null);

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

        // Tin nhắn nhóm đến - hiển thị với format [Sender] message
        private void AppendIncomingGroup(string groupId, string from, string text)
        {
            var now = DateTime.Now;
            AddMessageToConversation(groupId, false, text, now, sender: from);

            // Nếu đang xem nhóm này, hiển thị bubble
            if (string.Equals(_currentPeer, groupId, StringComparison.Ordinal))
            {
                var bubble = new Controls.MessageBubbleControl
                {
                    IsOutgoing = false,
                    MessageText = $"[{from}] {text}",  // Format nhóm: [Sender] message
                    Timestamp = now
                };
                flpMessages.Controls.Add(bubble);
                bubble.UpdateLayoutBubble();
                flpMessages.ScrollControlIntoView(bubble);
            }
        }

        // Tin nhắn nhóm gửi đi - hiển thị như tin nhắn thường nhưng lưu vào conversation của nhóm
        private void AppendOutgoingGroup(string groupId, string text)
        {
            var now = DateTime.Now;
            AddMessageToConversation(groupId, true, text, now, sender: null);

            // Nếu đang xem nhóm này, hiển thị bubble
            if (string.Equals(_currentPeer, groupId, StringComparison.Ordinal))
            {
                var bubble = new Controls.MessageBubbleControl
                {
                    IsOutgoing = true,
                    MessageText = text,  // Tin nhắn của mình không cần [Sender]
                    Timestamp = now
                };
                flpMessages.Controls.Add(bubble);
                bubble.UpdateLayoutBubble();
                flpMessages.ScrollControlIntoView(bubble);
            }
        }

        // === cmsTaoNhom_Click: GỬI tới server, KHÔNG thêm local placeholder ===
        private void cmsTaoNhom_Click(object sender, EventArgs e)
        {
            try
            {
                var candidates = _latestUsers.Where(x => !string.Equals(x, _me?.Username, StringComparison.Ordinal)).ToArray();
                using (var dlg = new CreateGroup(candidates))
                {
                    dlg.GroupCreated += async (s, ev) =>
                    {
                        try
                        {
                            // Include creator as member
                            var members = new List<string>(ev.Members);
                            if (!members.Contains(_me.Username)) members.Add(_me.Username);

                            // GỬI tới server - server sẽ broadcast GROUP_CREATED
                            await _tcp.SendAsync(new
                            {
                                type = "GROUP_CREATE",
                                name = ev.GroupName,
                                members = members.ToArray()
                            });

                            // KHÔNG thêm item local ở đây - chờ server gửi GROUP_CREATED
                        }
                        catch (Exception ex)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                MessageBox.Show("Không thể tạo nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                    };

                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở form tạo nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var profile = new Profile(_me, _tcp)
                {
                    Owner = this
                };

                // When profile closed, try update avatar in chat list items
                profile.FormClosed += (s, ev) =>
                {
                    try
                    {
                        // If user changed avatar, update any ChatListItemControl that matches username
                        if (!string.IsNullOrEmpty(_me.Avatar))
                        {
                            foreach (Control c in flpUsers.Controls)
                            {
                                if (c is Controls.ChatListItemControl it && it.Username == _me.Username)
                                {
                                    try
                                    {
                                        var img = System.Drawing.Image.FromFile(_me.Avatar);
                                        it.SetAvatar(img);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    catch { }
                };

                profile.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở Profile: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAttach_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPeer))
            {
                MessageBox.Show("Chọn một người để gửi ảnh.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var path = dlg.FileName;
                try
                {
                    byte[] data = File.ReadAllBytes(path);
                    string b64 = Convert.ToBase64String(data);
                    string ext = Path.GetExtension(path);

                    // Send a image message to server. Server will relay to recipient as MSG_RECV_IMAGE
                    await _tcp.SendAsync(new
                    {
                        type = "MSG_TO_IMAGE",
                        to = _currentPeer,
                        image = b64,
                        ext = ext
                    });

                    // Locally append outgoing image bubble
                    var now = DateTime.Now;
                    AddMessageToConversation(_currentPeer, true, "[Image]", now, sender: null);

                    var bubble = new Controls.MessageBubbleControl
                    {
                        IsOutgoing = true,
                        ImageContent = Image.FromFile(path),
                        Timestamp = now
                    };
                    flpMessages.Controls.Add(bubble);
                    bubble.UpdateLayoutBubble();
                    flpMessages.ScrollControlIntoView(bubble);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể gửi ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
