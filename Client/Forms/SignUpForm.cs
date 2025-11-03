using Client.Services;
using Newtonsoft.Json;
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

namespace Client
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Hide();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirm = txtConfirm.Text;
            string display = txtDisplayName.Text.Trim();

            // validate cơ bản
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đủ Username và Password.");
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu tối thiểu 6 ký tự.");
                return;
            }
            if (password != confirm)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp.");
                return;
            }
            if (string.IsNullOrWhiteSpace(display))
                display = username;

            try
            {
                var tcp = new TcpService();
                await tcp.ConnectAsync("127.0.0.1", 9000);

                await tcp.SendAsync(new
                {
                    type = "REGISTER",
                    username = username,
                    password = password,
                    displayName = display
                });

                var line = await tcp.ReadLineAsync(CancellationToken.None);
                if (line == null)
                {
                    MessageBox.Show("Mất kết nối server.");
                    return;
                }

                dynamic resp = JsonConvert.DeserializeObject(line);
                string type = (string)resp.type;

                if (type == "REGISTER_OK")
                {
                    MessageBox.Show("Đăng ký thành công. Mời bạn đăng nhập.");
                    // quay về FormLogin
                    var login = new FormLogin();
                    login.Show();
                    this.Close();
                }
                else
                {
                    string reason = resp.reason != null ? (string)resp.reason : "unknown";
                    string msg;
                    if (reason == "username_taken")
                        msg = "Tên đăng nhập đã tồn tại.";
                    else if (reason == "invalid_input")
                        msg = "Dữ liệu không hợp lệ.";
                    else
                        msg = "Đăng ký thất bại.";

                    MessageBox.Show(msg);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtConfirm_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenHienThi_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
