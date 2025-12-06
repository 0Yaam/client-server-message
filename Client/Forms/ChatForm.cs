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
using System.Reflection;

namespace Client.Forms
{
    public partial class ChatForm : Form
    {
        private readonly Account _me;
        private readonly TcpService _tcp;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private string _currentPeer = null;
        private readonly System.Windows.Forms.Timer _listTimer = new System.Windows.Forms.Timer { Interval = 2000 };

        // Local users preserved across LIST refresh
        private readonly HashSet<string> _localSpecialUsers = new HashSet<string>(StringComparer.Ordinal);

        // Pending group avatars by group name
        private readonly Dictionary<string, string> _pendingGroupAvatarByName = new Dictionary<string, string>(StringComparer.Ordinal);

        // Conversation history store
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
        private string[] _allUsers = Array.Empty<string>();

        // Hàng đợi tin nhắn offline theo người nhận
        private readonly Dictionary<string, List<string>> _pendingOutgoingByUser = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        public ChatForm(Account me, TcpService tcp)
        {
            InitializeComponent();
            _me = me ?? throw new ArgumentNullException(nameof(me));
            _tcp = tcp ?? throw new ArgumentNullException(nameof(tcp));

            flpMessages.WrapContents = false;
            flpMessages.AutoScroll = true;
            flpMessages.FlowDirection = FlowDirection.TopDown;

            // Ensure user list lays out vertically without wrapping
            try
            {
                flpUsers.WrapContents = false;
                flpUsers.AutoScroll = true;
                flpUsers.FlowDirection = FlowDirection.TopDown;
            }
            catch { }

            // Enable double-buffering to reduce flicker
            try
            {
                typeof(FlowLayoutPanel).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(flpUsers, true);
                typeof(FlowLayoutPanel).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(flpMessages, true);
            }
            catch { }

            txtMessage.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && !e.Shift)
                {
                    e.SuppressKeyPress = true;
                    btnSend.PerformClick();
                }
            };

            // Request initial user list on load
            this.Load += async (s, e) =>
            {
                try
                {
                    LoadAllUsersLocal();
                    RenderUserList(_latestUsers);
                    await _tcp.SendAsync(new { type = "LIST" });
                }
                catch { }
            };

            // Periodically refresh user list
            _listTimer.Tick += async (s, e) => { try { await _tcp.SendAsync(new { type = "LIST" }); } catch { } };
            _listTimer.Start();

            // Start background listener
            _ = Task.Run(ListenLoop);
        }
        public ChatForm() : this(new Account("demo", "", "", UserRole.User), null) { }

        private void LoadAllUsersLocal()
        {
            try
            {
                var accs = AccountJsonService_LoginSafe();
                if (accs != null)
                {
                    _allUsers = accs.Where(a => !string.Equals(a.Username, _me.Username, StringComparison.OrdinalIgnoreCase))
                                    .Select(a => a.Username)
                                    .Distinct(StringComparer.Ordinal)
                                    .ToArray();
                }
            }
            catch { _allUsers = Array.Empty<string>(); }
        }

        // Đọc danh sách tài khoản từ file JSON (an toàn)
        private List<Account> AccountJsonService_LoginSafe()
        {
            try
            {
                // Ưu tiên Server/bin/Debug/Data/users.json (danh sách chính)
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string path = null;

                try
                {
                    var dir = new DirectoryInfo(baseDir);
                    for (int i = 0; i < 6 && dir != null; i++)
                    {
                        var candidates = new[]
                        {
                            Path.Combine(dir.FullName, "Server", "bin", "Debug", "Data", "users.json"),
                            Path.Combine(dir.FullName, "Server", "bin", "Release", "Data", "users.json"),
                            Path.Combine(dir.FullName, "Server", "Data", "users.json")
                        };
                        foreach (var c in candidates)
                        {
                            if (File.Exists(c)) { path = c; break; }
                        }
                        if (path != null) break;
                        dir = dir.Parent;
                    }
                }
                catch { }

                // Fallback sang Client/Data nếu không tìm thấy Server
                if (path == null || !File.Exists(path))
                {
                    var clientPath = Path.Combine(baseDir, "Data", "users.json");
                    if (File.Exists(clientPath)) path = clientPath;
                }

                if (!File.Exists(path)) return null;
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<Account>>(json) ?? new List<Account>();
            }
            catch { return null; }
        }

        private void EnsureUserInList(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username)) return;

                // Skip groups
                if (_groupMembers.ContainsKey(username)) return;

                // Preserve as local special user
                _localSpecialUsers.Add(username);

                bool exists = flpUsers.Controls.OfType<Controls.ChatListItemControl>().Any(i => i.Username == username);
                if (!exists)
                {
                    // Use parent panel width for reliable sizing
                    int panelWidth = pnlLeft?.ClientSize.Width ?? 265;
                    int itemWidth = Math.Max(240, panelWidth - 20);
                    var item = new Controls.ChatListItemControl
                    {
                        Width = itemWidth,
                        Height = 72,
                        MinimumSize = new Size(itemWidth, 72),
                        MaximumSize = new Size(itemWidth, 72),
                        Margin = new Padding(10, 4, 10, 4),
                        Anchor = AnchorStyles.Left | AnchorStyles.Top
                    };
                    item.Bind(username: username, displayName: username, lastMessage: "Nhấn để chat", time: DateTime.Now);

                    item.ItemClicked += (s, e) =>
                    {
                        foreach (Control c in flpUsers.Controls)
                            if (c is Controls.ChatListItemControl it) it.SetSelected(false);

                        item.SetSelected(true);
                        SelectPeer(item.Username);
                    };

                    // Mặc định làm mờ nếu offline
                    bool isOnline = _latestUsers?.Contains(username) == true;
                    ApplyOnlineVisual(item, isOnline);

                    // Force item to take full width using parent panel width
                    int panelW = pnlLeft?.ClientSize.Width ?? 265;
                    int availWidth = Math.Max(240, panelW - 20);
                    item.Width = availWidth;
                    item.MinimumSize = new Size(availWidth, 72);
                    item.MaximumSize = new Size(availWidth, 72);

                    flpUsers.Controls.Add(item);
                    flpUsers.SetFlowBreak(item, true);
                }
            }
            catch { }
        }

        private void ApplyOnlineVisual(Controls.ChatListItemControl item, bool isOnline)
        {
            try
            {
                item.Enabled = true; // luôn tương tác được
                var fore = isOnline ? SystemColors.ControlText : Color.Gray;
                item.ForeColor = fore;
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
                            RenderUserList(_latestUsers);
                            // Flush pending messages in background to avoid UI blocking
                            _ = FlushPendingToOnlineAsync(_latestUsers);
                            if (_latestUsers.Length > 0 && string.IsNullOrEmpty(_currentPeer))
                                SelectPeer(_latestUsers[0]);
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

                        // Ensure sender appears in list
                        BeginInvoke(new Action(() => EnsureUserInList(from)));

                        // Check if message is for a group
                        string groupId = msg.groupId; // may be null for 1:1

                        BeginInvoke(new Action(() =>
                        {
                            if (!string.IsNullOrEmpty(groupId))
                            {
                                // Group message
                                AppendIncomingGroup(groupId, from, text);
                            }
                            else
                            {
                                // Direct message
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
                            // Decide group vs direct
                            if (_groupMembers.ContainsKey(to))
                            {
                                AppendOutgoingGroup(to, text);
                            }
                            else
                            {
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
                        // Handle avatar update broadcast
                        string username = (string)msg.username;
                        string b64 = (string)msg.image;
                        string ext = (string)msg.ext; // may include dot

                        try
                        {
                            var data = Convert.FromBase64String(b64);
                            var saved = SaveAvatarLocal(username, data, ext);

                            // Update local account avatar path if current user
                            if (string.Equals(_me?.Username, username, StringComparison.OrdinalIgnoreCase))
                            {
                                _me.Avatar = saved;
                            }

                            // Update UI avatar for matching list items
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

                        // Ensure sender appears in list
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
                // Tìm avatar ở Client/Data/Avatars, nếu không có thì fallback Server/Data/Avatars (tìm ngược nhiều cấp)
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                // 1) Client/Data/Avatars
                var clientAvt = Path.Combine(baseDir, "Data", "Avatars");
                if (Directory.Exists(clientAvt))
                {
                    var files = Directory.GetFiles(clientAvt, username + ".*");
                    if (files.Length > 0) return files[0];
                }

                // 2) Server/bin/{Debug|Release}/Data/Avatars or Server/Data/Avatars by walking up
                try
                {
                    var dir = new DirectoryInfo(baseDir);
                    for (int i = 0; i < 6 && dir != null; i++)
                    {
                        var candidates = new[]
                        {
                            Path.Combine(dir.FullName, "Server", "bin", "Debug", "Data", "Avatars"),
                            Path.Combine(dir.FullName, "Server", "bin", "Release", "Data", "Avatars"),
                            Path.Combine(dir.FullName, "Server", "Data", "Avatars"),
                        };
                        foreach (var serverAvt in candidates)
                        {
                            if (Directory.Exists(serverAvt))
                            {
                                var files = Directory.GetFiles(serverAvt, username + ".*");
                                if (files.Length > 0) return files[0];
                            }
                        }
                        dir = dir.Parent;
                    }
                }
                catch { }
                return null;
            }
            catch { return null; }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            var text = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            if (string.IsNullOrEmpty(_currentPeer))
            {
                MessageBox.Show("Chưa chọn người nhận");
                return;
            }

            try
            {
                // Gửi trực tiếp nếu là nhóm hoặc người đang online
                bool isGroup = _groupMembers.ContainsKey(_currentPeer);
                bool isOnline = _latestUsers?.Contains(_currentPeer) == true;
                if (isGroup || isOnline)
                {
                    await _tcp.SendAsync(new
                    {
                        type = "MSG_TO",
                        to = _currentPeer,
                        message = text
                    });
                }
                else
                {
                    // Lưu vào hàng đợi offline để gửi khi người nhận online
                    QueueOfflineMessage(_currentPeer, text);
                }

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch
            {
                MessageBox.Show("Không gửi được tin, kiểm tra kết nối.");
            }
        }

        private void QueueOfflineMessage(string to, string text)
        {
            if (string.IsNullOrEmpty(to) || string.IsNullOrEmpty(text)) return;
            if (!_pendingOutgoingByUser.TryGetValue(to, out var list))
            {
                list = new List<string>();
                _pendingOutgoingByUser[to] = list;
            }
            list.Add(text);

            // Hiển thị ngay tin nhắn đã xếp hàng
            var now = DateTime.Now;
            AddMessageToConversation(to, true, text, now, sender: null);
            if (string.Equals(_currentPeer, to, StringComparison.Ordinal))
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

        private async Task FlushPendingToOnlineAsync(string[] onlineUsers)
        {
            try
            {
                if (onlineUsers == null || onlineUsers.Length == 0) return;
                foreach (var u in onlineUsers)
                {
                    if (_pendingOutgoingByUser.TryGetValue(u, out var list) && list.Count > 0)
                    {
                        // copy to avoid mutation during send
                        var snapshot = list.ToArray();
                        foreach (var msg in snapshot)
                        {
                            try
                            {
                                await _tcp.SendAsync(new { type = "MSG_TO", to = u, message = msg });
                                list.Remove(msg);
                            }
                            catch { }
                        }
                        if (list.Count == 0) _pendingOutgoingByUser.Remove(u);
                    }
                }
            }
            catch { }
        }

        private void SelectPeer(string username)
        {

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


                    if (isGroup)
                    {

                        if (m.IsOutgoing)
                        {
                            bubble.MessageText = m.Text;
                        }
                        else
                        {
                            bubble.MessageText = $"[{m.Sender}] {m.Text}";
                        }
                    }
                    else
                    {

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
            // Update user list with minimal redraw
            flpUsers.SuspendLayout();
            try
            {
                // Calculate item width once at the start
                int panelW = pnlLeft?.ClientSize.Width ?? 265;
                int itemWidth = Math.Max(240, panelW - 20);
                
                var currentItems = flpUsers.Controls.OfType<Controls.ChatListItemControl>().ToList();
                var currentUserItems = currentItems.Where(i => !_groupMembers.ContainsKey(i.Username)).ToList();
                var groupItems = currentItems.Where(i => _groupMembers.ContainsKey(i.Username)).ToList();

                // Merge server users with local special users and all users from local store
                var finalUsersSet = new HashSet<string>(StringComparer.Ordinal);
                if (_allUsers != null) foreach (var u in _allUsers) finalUsersSet.Add(u);
                if (users != null) foreach (var u in users) finalUsersSet.Add(u);
                foreach (var su in _localSpecialUsers) finalUsersSet.Add(su);

                var finalUsers = finalUsersSet.Where(u => !string.Equals(u, _me.Username, StringComparison.OrdinalIgnoreCase)).ToList();

                // Sort: online first then offline, then by name
                finalUsers = finalUsers
                    .OrderByDescending(u => users != null && users.Contains(u))
                    .ThenBy(u => u, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // Map existing items by username
                var currentMap = currentUserItems.ToDictionary(i => i.Username, StringComparer.Ordinal);

                // Remove stale items
                var toRemove = currentUserItems.Where(i => !finalUsers.Contains(i.Username)).ToList();
                foreach (var rm in toRemove)
                {
                    flpUsers.Controls.Remove(rm);
                    try { rm.Dispose(); } catch { }
                }

                // Save selection before rebuild
                var previouslySelected = currentItems.FirstOrDefault(i => i.Selected)?.Username;

                // Rebuild controls - Add groups first, then users
                // Keep track of all items to add
                var itemsToAdd = new List<Controls.ChatListItemControl>();
                
                // Add groups first
                itemsToAdd.AddRange(groupItems);

                // Add or reuse user items
                foreach (var u in finalUsers)
                {
                    if (_groupMembers.ContainsKey(u)) continue;

                    string preview = "Nhấn để chat";
                    DateTime time = DateTime.Now;
                    if (_conversations.TryGetValue(u, out var conv) && conv.Count > 0)
                    {
                        var last = conv[conv.Count - 1];
                        preview = last.IsOutgoing ? $"Bạn: {last.Text}" : last.Text;
                        time = last.Timestamp;
                    }

                    if (currentMap.TryGetValue(u, out var existing))
                    {
                        if (existing.DisplayName != u) existing.DisplayName = u;
                        if (existing.LastMessage != preview) existing.LastMessage = preview;
                        if (existing.Time != time) existing.Time = time;

                        bool isOnline = users != null && users.Contains(u);
                        ApplyOnlineVisual(existing, isOnline);

                        itemsToAdd.Add(existing);
                    }
                    else
                    {
                        // Use calculated item width
                        var item = new Controls.ChatListItemControl
                        {
                            Width = itemWidth,
                            Height = 72,
                            MinimumSize = new Size(itemWidth, 72),
                            MaximumSize = new Size(itemWidth, 72),
                            Margin = new Padding(10, 4, 10, 4),
                            Anchor = AnchorStyles.Left | AnchorStyles.Top
                        };
                        item.Bind(username: u, displayName: u, lastMessage: preview, time: time);

                        // Load local avatar when present
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

                        bool isOnline = users != null && users.Contains(u);
                        ApplyOnlineVisual(item, isOnline);

                        itemsToAdd.Add(item);
                    }
                }

                // Now clear and add all items at once with proper sizing
                flpUsers.Controls.Clear();
                
                foreach (var item in itemsToAdd)
                {
                    // Force each item to take full width so they stack vertically
                    item.Width = itemWidth;
                    item.MinimumSize = new Size(itemWidth, 72);
                    item.MaximumSize = new Size(itemWidth, 72);
                    
                    flpUsers.Controls.Add(item);
                    flpUsers.SetFlowBreak(item, true);
                }

                // Restore selection if possible
                if (!string.IsNullOrEmpty(previouslySelected))
                {
                    foreach (Control c in flpUsers.Controls)
                        if (c is Controls.ChatListItemControl it) it.SetSelected(it.Username == previouslySelected);
                }
            }
            finally
            {
                flpUsers.ResumeLayout();
            }
        }

        private void AddGroupToUserList(string groupId, string groupName, string[] members)
        {
            if (string.IsNullOrEmpty(groupId)) return;

            foreach (Control c in flpUsers.Controls)
            {
                if (c is Controls.ChatListItemControl existing && existing.Username == groupId)
                {
                    existing.DisplayName = groupName;
                    return;
                }
            }

            _groupMembers[groupId] = members ?? new string[0];

            // Use parent panel width for reliable sizing
            int panelWidth = pnlLeft?.ClientSize.Width ?? 265;
            int itemWidth = Math.Max(240, panelWidth - 20);
            var item = new Controls.ChatListItemControl
            {
                Width = itemWidth,
                Height = 72,
                MinimumSize = new Size(itemWidth, 72),
                MaximumSize = new Size(itemWidth, 72),
                Margin = new Padding(10, 4, 10, 4),
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };
            item.Bind(username: groupId, displayName: groupName, lastMessage: "Nhóm chat mới", time: DateTime.Now);

            // Apply cached group avatar if present
            try
            {
                string avatarPath;
                if (!string.IsNullOrEmpty(groupName) && _pendingGroupAvatarByName.TryGetValue(groupName, out avatarPath))
                {
                    if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
                    {
                        using (var fs = File.OpenRead(avatarPath))
                        {
                            var img = Image.FromStream(fs);
                            item.SetAvatar(new Bitmap(img));
                        }
                    }
                }
            }
            catch { }

            item.ItemClicked += (senderItem, args) =>
            {
                foreach (Control c in flpUsers.Controls)
                    if (c is Controls.ChatListItemControl it) it.SetSelected(false);

                item.SetSelected(true);
                SelectPeer(item.Username);
            };

            // Force item to take full width using parent panel width
            int pnlW = pnlLeft?.ClientSize.Width ?? 265;
            int availW = Math.Max(240, pnlW - 20);
            item.Width = availW;
            item.MinimumSize = new Size(availW, 72);
            item.MaximumSize = new Size(availW, 72);

            flpUsers.Controls.Add(item);
            flpUsers.SetFlowBreak(item, true);

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


            string preview;
            bool isGroup = _groupMembers.ContainsKey(username);

            if (isGroup)
            {

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
                    MessageText = text,
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


        private void AppendIncomingGroup(string groupId, string from, string text)
        {
            var now = DateTime.Now;
            AddMessageToConversation(groupId, false, text, now, sender: from);


            if (string.Equals(_currentPeer, groupId, StringComparison.Ordinal))
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
        }


        private void AppendOutgoingGroup(string groupId, string text)
        {
            var now = DateTime.Now;
            AddMessageToConversation(groupId, true, text, now, sender: null);


            if (string.Equals(_currentPeer, groupId, StringComparison.Ordinal))
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


        private void cmsTaoNhom_Click(object sender, EventArgs e)
        {
            try
            {
                var candidates = _allUsers.Length > 0 ? _allUsers : _latestUsers;
                candidates = candidates.Where(x => !string.Equals(x, _me?.Username, StringComparison.OrdinalIgnoreCase)).ToArray();
                using (var dlg = new Controls.Group(candidates))
                {
                    dlg.GroupCreated += async (s, ev) =>
                    {
                        try
                        {
                            // Cache avatar for applying when server returns GROUP_CREATED
                            if (!string.IsNullOrEmpty(ev.GroupName) && !string.IsNullOrEmpty(ev.AvatarPath))
                            {
                                _pendingGroupAvatarByName[ev.GroupName] = ev.AvatarPath;
                            }

                            var members = new List<string>(ev.Members);
                            if (!members.Contains(_me.Username)) members.Add(_me.Username);

                            await _tcp.SendAsync(new
                            {
                                type = "GROUP_CREATE",
                                name = ev.GroupName,
                                members = members.ToArray()
                            });
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


                profile.FormClosed += (s, ev) =>
                {
                    try
                    {

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


                    await _tcp.SendAsync(new
                    {
                        type = "MSG_TO_IMAGE",
                        to = _currentPeer,
                        image = b64,
                        ext = ext
                    });


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

        private void cmsXoaBan_Click(object sender, EventArgs e)
        {
            try
            {
                // Prefer currently selected item
                var selected = flpUsers.Controls
                    .OfType<Controls.ChatListItemControl>()
                    .FirstOrDefault(i => i != null && i.Selected);

                // If none selected, pick control under mouse
                if (selected == null)
                {
                    var pt = flpUsers.PointToClient(Cursor.Position);
                    selected = flpUsers.Controls
                        .OfType<Controls.ChatListItemControl>()
                        .FirstOrDefault(c => c.Bounds.Contains(pt));
                }

                if (selected == null) return;

                // Do not remove groups
                if (_groupMembers.ContainsKey(selected.Username)) return;

                flpUsers.Controls.Remove(selected);
                try { selected.Dispose(); } catch { }

                if (string.Equals(_currentPeer, selected.Username, StringComparison.Ordinal))
                {
                    _currentPeer = null;
                    if (lblHeader != null) lblHeader.Text = string.Empty;
                    flpMessages.Controls.Clear();
                }
            }
            catch { }
        }
    }
}
