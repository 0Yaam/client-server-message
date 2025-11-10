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
                var users = AuthManager.GetAllUsers();
                var onlineUsers = OnlineRegistry.ListUsernames();

                lvListUser.BeginUpdate();
                lvListUser.Items.Clear();

                foreach (var user in users)
                {
                    var item = new ListViewItem(user.Username);
                    item.SubItems.Add(user.DisplayName ?? user.Username);
                    item.SubItems.Add(user.Role.ToString());
                    
                    // Check if user is online
                    bool isOnline = onlineUsers.Contains(user.Username);
                    item.SubItems.Add(isOnline ? "Online" : "Offline");
                    
                    // Color coding for online/offline
                    if (isOnline)
                    {
                        item.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        item.BackColor = Color.White;
                    }

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
                _ = _server.AcceptLoopAsync(_cts.Token); // không chặn UI

                // Start refresh timer
                _refreshTimer.Start();

                // Initial refresh
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
                info += $"Server đang chạy: {(_server != null ? "Có" : "Không")}\"";
                
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
    }
}
