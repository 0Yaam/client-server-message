using Client.Services;
using Guna.UI2.WinForms;
using Newtonsoft.Json;
using Shared.OL;
using System;
using System.Threading;
using System.Windows.Forms;
using Client.Forms;

namespace Client
{
    public partial class FormLogin : Form
    {
        private readonly TcpService _tcp = new TcpService();
        // cấu hình nhanh: IP/port của server
        private const string SERVER_HOST = "127.0.0.1";
        private const int SERVER_PORT = 9000;

        public FormLogin()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                // 1) Kết nối server
                await _tcp.ConnectAsync(SERVER_HOST, SERVER_PORT);

                // 2) Gửi AUTH
                await _tcp.SendAsync(new { type = "AUTH", username, password });

                // 3) Đọc phản hồi
                var line = await _tcp.ReadLineAsync(CancellationToken.None);
                if (line == null)
                {
                    MessageBox.Show("Mất kết nối server.");
                    return;
                }

                dynamic resp = JsonConvert.DeserializeObject(line);
                string type = (string)resp.type;

                if (type == "AUTH_OK")
                {
                    string roleStr = (string)resp.role; // "Admin" hoặc "User"
                    UserRole role = roleStr == "Admin" ? UserRole.Admin : UserRole.User;

                    // Nếu cần truyền thông tin người dùng sang ChatForm:
                    var acc = new Account(username, password, "", role);

                    MessageBox.Show("Đăng nhập thành công!", "Thông báo");
                    Hide();

                    var chat = new ChatForm(); // hoặc new ChatForm(acc, _tcp) nếu bạn muốn giữ kết nối
                    chat.Show();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được server: " + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = new SignUpForm();
            form.Show();
            Hide();
        }
    }
}
