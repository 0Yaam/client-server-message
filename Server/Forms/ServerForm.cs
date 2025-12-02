using Server.ServerCore;
using Shared.OL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

// new usings for client integration
using Client.Services;
using Client.Forms;

namespace Server
{
    public partial class ServerForm : Form
    {
        private ChatServer _server;
        private CancellationTokenSource _cts;
        private System.Windows.Forms.Timer _refreshTimer;

        public ServerForm()
        {
            InitializeComponent();
            InitializeListView();
            InitializeRefreshTimer();

            // Hook double-click
            lvListUser.DoubleClick -= LvListUser_DoubleClick;
            lvListUser.DoubleClick += LvListUser_DoubleClick;

            // Hook menu and buttons
            msPrivateChat.Click -= MsPrivateChat_Click;
            msPrivateChat.Click += MsPrivateChat_Click;

            btnServerSend.Click -= BtnServerSend_Click;
            btnServerSend.Click += BtnServerSend_Click;

            chkSelectAll.CheckedChanged -= ChkSelectAll_CheckedChanged;
            chkSelectAll.CheckedChanged += ChkSelectAll_CheckedChanged;

            // Hook search controls safely
            var txtSearch = this.Controls.Find("txtServerSearch", true).FirstOrDefault() as TextBox;
            if (txtSearch != null)
            {
                txtSearch.TextChanged -= TxtServerSearch_TextChanged;
                txtSearch.TextChanged += TxtServerSearch_TextChanged;
            }

            var rdDisplay = this.Controls.Find("radioButton1", true).FirstOrDefault() as RadioButton;
            if (rdDisplay != null)
            {
                rdDisplay.CheckedChanged -= SearchOptionChanged;
                rdDisplay.CheckedChanged += SearchOptionChanged;
            }

            var rdUser = this.Controls.Find("radioButton3", true).FirstOrDefault() as RadioButton;
            if (rdUser != null)
            {
                rdUser.CheckedChanged -= SearchOptionChanged;
                rdUser.CheckedChanged += SearchOptionChanged;
            }

            var cbb = this.Controls.Find("cbbRole", true).FirstOrDefault() as ComboBox;
            if (cbb != null)
            {
                cbb.SelectedIndexChanged -= CbbRole_SelectedIndexChanged;
                cbb.SelectedIndexChanged += CbbRole_SelectedIndexChanged;
            }
        }

        private void CbbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshUsersList();
        }

        private void SearchOptionChanged(object sender, EventArgs e)
        {
            RefreshUsersList();
        }

        private void TxtServerSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshUsersList();
        }

        private void InitializeListView()
        {
            // Cấu hình ListView để hiển thị thông tin users
            lvListUser.View = View.Details;
            lvListUser.FullRowSelect = true;
            lvListUser.GridLines = true;

            // Thêm các cột
            lvListUser.Columns.Add("Tên đăng nhập", 120);
            lvListUser.Columns.Add("Tên hiển thị", 150);
            lvListUser.Columns.Add("Vai trò", 80);
            lvListUser.Columns.Add("Trạng thái", 80);
            lvListUser.Columns.Add("Mật khẩu", 200);
        }

        private void LvListUser_DoubleClick(object sender, EventArgs e)
        {
            if (lvListUser.SelectedItems.Count == 0) return;
            var it = lvListUser.SelectedItems[0];
            var username = it.SubItems[0].Text;
            var displayName = it.SubItems[1].Text;
            var passwordHash = it.SubItems.Count > 4 ? it.SubItems[4].Text : string.Empty;

            using (var dlg = new Server.Forms.UpdateForm(username))
            {
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    RefreshUsersList();
                }
            }
        }

        private void InitializeRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 2000; // Refresh every 2 seconds
            _refreshTimer.Tick += RefreshTimer_Tick;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                RefreshUsersList();
                RefreshOnlineList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Refresh error: " + ex.Message);
            }
        }

        private void RefreshUsersList()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshUsersList));
                return;
            }

            try
            {
                var users = AuthManager.GetAllUsers().ToList();
                var onlineUsers = OnlineRegistry.ListUsernames();

                string roleFilter = cbbRole.SelectedItem as string;
                if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "Tất cả")
                {
                    users = users.Where(u => u.Role.ToString().Equals(roleFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Apply text search
                var q = txtServerSearch.Text?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(q))
                {
                    var rdDisplay = this.Controls.Find("radioButton1", true).FirstOrDefault() as RadioButton;
                    var rdUser = this.Controls.Find("radioButton3", true).FirstOrDefault() as RadioButton;

                    if (rdDisplay != null && rdDisplay.Checked)
                    {
                        // display name
                        users = users.Where(u => (!string.IsNullOrEmpty(u.DisplayName) && u.DisplayName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                    }
                    else if (rdUser != null && rdUser.Checked)
                    {
                        // username
                        users = users.Where(u => (!string.IsNullOrEmpty(u.Username) && u.Username.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                    }
                    else
                    {
                        // fallback: search both
                        users = users.Where(u => (u.Username != null && u.Username.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) || (u.DisplayName != null && u.DisplayName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
                    }
                }

                // Preserve checked usernames before clearing
                var checkedSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                try
                {
                    foreach (ListViewItem it in lvListUser.Items)
                    {
                        if (it.Checked && it.SubItems.Count > 0)
                        {
                            checkedSet.Add(it.SubItems[0].Text);
                        }
                    }
                }
                catch { }

                lvListUser.BeginUpdate();
                lvListUser.Items.Clear();

                foreach (var user in users)
                {
                    var item = new ListViewItem(user.Username);
                    item.SubItems.Add(user.DisplayName ?? user.Username);
                    item.SubItems.Add(user.Role.ToString());

                    bool isOnline = onlineUsers.Contains(user.Username);
                    item.SubItems.Add(isOnline ? "Online" : "Offline");

                    item.SubItems.Add(user.PasswordHash ?? string.Empty);

                    // Color coding for online/offline
                    if (isOnline)
                    {
                        item.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        item.BackColor = Color.White;
                    }

                    // restore checked state or honor chkSelectAll
                    try
                    {
                        if (chkSelectAll.Checked)
                        {
                            item.Checked = true;
                        }
                        else
                        {
                            item.Checked = checkedSet.Contains(user.Username);
                        }
                    }
                    catch { }

                    lvListUser.Items.Add(item);
                }
            }
            finally
            {
                lvListUser.EndUpdate();
            }
        }

        private void RefreshOnlineList()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshOnlineList));
                return;
            }

            var list = OnlineRegistry.ListUsernames();
            listBoxOnline.Items.Clear();
            listBoxOnline.Items.AddRange(list);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            try
            {
                AuthManager.Init();

                _server = new ChatServer(9000);
                _server.Start();

                _cts = new CancellationTokenSource();
                _ = _server.AcceptLoopAsync(_cts.Token); 
                // Start refresh timer
                _refreshTimer.Start();

                // Initial refresh
                // populate role combo
                if (cbbRole.Items.Count == 0)
                {
                    cbbRole.Items.Add("Tất cả");
                    cbbRole.Items.Add("Admin");
                    cbbRole.Items.Add("User");
                    cbbRole.SelectedIndex = 0;
                }

                RefreshUsersList();
                RefreshOnlineList();

                MessageBox.Show("Server started on port 9000");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting server: " + ex.Message);
            }
            finally
            {
                btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            _refreshTimer?.Stop();
            _cts?.Cancel();
            _server?.Stop();
            MessageBox.Show("Server stopped");
            btnStart.Enabled = true;

            // Clear lists
            lvListUser.Items.Clear();
            listBoxOnline.Items.Clear();
        }

        private void msThongTin_Click(object sender, EventArgs e)
        {
            try
            {
                var users = AuthManager.GetAllUsers();
                var onlineCount = OnlineRegistry.ListUsernames().Length;

                string info = $"Tổng số tài khoản đã đăng ký: {users.Length}\n";
                info += $"Số người đang online: {onlineCount}\n";
                info += $"Server đang chạy: {(_server != null ? "Có" : "Không")}";

                MessageBox.Show(info, "Thông tin Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            _cts?.Cancel();
            _server?.Stop();
            base.OnFormClosing(e);
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            txtIP.PlaceholderText = "127.0.0.1";
            txtPort.PlaceholderText = "9000";
            txtServerSearch.PlaceholderText = "Search...";
            txtServerMessage.PlaceholderText = "Message send to all...";
        }

        private async void MsPrivateChat_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure server is running
                if (_server == null)
                {
                    MessageBox.Show("Server chưa chạy. Bắt đầu server trước khi mở private chat.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Determine host and port from UI (use defaults if empty)
                string host = "127.0.0.1";
                int port = 9000;
                try
                {
                    if (!string.IsNullOrWhiteSpace(txtIP.Text)) host = txtIP.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(txtPort.Text) && int.TryParse(txtPort.Text.Trim(), out var p)) port = p;
                }
                catch { }

                // Create a TcpService as admin client connecting to specified host/port
                var tcp = new TcpService();
                try
                {
                    await tcp.ConnectAsync(host, port);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể kết nối tới server (kiểm tra IP/Port hoặc server chưa chạy).", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use admin credentials by default
                string adminUser = "admin";
                string adminPass = "123";

                await tcp.SendAsync(new { type = "AUTH", username = adminUser, password = adminPass });
                var line = await tcp.ReadLineAsync(CancellationToken.None);
                if (line == null)
                {
                    MessageBox.Show("Không nhận được phản hồi từ server khi AUTH", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    await tcp.CloseAsync();
                    return;
                }

                dynamic resp = Newtonsoft.Json.JsonConvert.DeserializeObject(line);
                if ((string)resp.type == "AUTH_OK")
                {
                    // Construct Account using info from server
                    string roleStr = (string)resp.role;
                    var role = roleStr == "Admin" ? UserRole.Admin : UserRole.User;
                    var acc = new Account(adminUser, adminPass, "", role)
                    {
                        DisplayName = "Zola"
                    };

                    // Open client ChatForm inside server process
                    var chat = new Client.Forms.ChatForm(acc, tcp);
                    chat.Text = "Admin Chat - Zola";
                    chat.Show();
                }
                else
                {
                    MessageBox.Show("AUTH thất bại cho admin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    await tcp.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở private chat: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool check = chkSelectAll.Checked;
            for (int i = 0; i < lvListUser.Items.Count; i++)
            {
                lvListUser.Items[i].Checked = check;
            }
        }

        private async void BtnServerSend_Click(object sender, EventArgs e)
        {
            var text = txtServerMessage.Text?.Trim();
            if (string.IsNullOrEmpty(text)) return;

            var targets = new List<string>();
            foreach (ListViewItem item in lvListUser.Items)
            {
                if (item.Checked)
                {
                    var uname = item.SubItems[0].Text;
                    targets.Add(uname);
                }
            }

            if (targets.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn người nhận (checkbox)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            foreach (var uname in targets)
            {
                var session = OnlineRegistry.Get(uname);
                if (session != null)
                {
                    try
                    {
                        await session.SendObjectAsync(new
                        {
                            type = "MSG_RECV",
                            from = "Zola",
                            message = text,
                            time = DateTime.UtcNow
                        });
                    }
                    catch { }
                }
            }

            MessageBox.Show("Đã gửi tới " + targets.Count + " người.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
