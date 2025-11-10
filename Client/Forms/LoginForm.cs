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
        private const string SERVER_HOST = "127.0.0.1";
        private const int SERVER_PORT = 9000;

        public FormLogin()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
            txtUsername.Focus();

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                await _tcp.ConnectAsync("127.0.0.1", 9000);
                await _tcp.SendAsync(new { type = "AUTH", username, password });

                var line = await _tcp.ReadLineAsync(CancellationToken.None);
                if (line == null) { MessageBox.Show("Mất kết nối server"); return; }

                dynamic resp = JsonConvert.DeserializeObject(line);
                if ((string)resp.type == "AUTH_OK")
                {
                    var roleStr = (string)resp.role;
                    var role = roleStr == "Admin" ? UserRole.Admin : UserRole.User;

                    var acc = new Account(username, password, "", role);

                    Hide();
                    var chat = new Client.Forms.ChatForm(acc, _tcp);
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
