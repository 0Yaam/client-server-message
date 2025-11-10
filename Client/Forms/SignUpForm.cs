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
        private readonly TcpService _tcp = new TcpService();
        private const string SERVER_HOST = "127.0.0.1";
        private const int SERVER_PORT = 9000;

        public SignUpForm()
        {
            InitializeComponent();
            this.AcceptButton = btnRegister;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Hide();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string displayName = txtDisplayName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirm.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(displayName))
            {
                MessageBox.Show("Vui lòng nhập tên hiển thị!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDisplayName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (username.Length < 3)
            {
                MessageBox.Show("Tên đăng nhập phải có ít nhất 3 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirm.Focus();
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "Đang đăng ký...";

            try
            {
                await _tcp.ConnectAsync(SERVER_HOST, SERVER_PORT);
                await _tcp.SendAsync(new
                {
                    type = "REGISTER",
                    username = username,
                    displayName = displayName,
                    password = password
                });

                var line = await _tcp.ReadLineAsync(CancellationToken.None);
                if (line == null)
                {
                    MessageBox.Show("Mất kết nối server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dynamic resp = JsonConvert.DeserializeObject(line);
                string responseType = (string)resp.type;

                if (responseType == "REGISTER_OK")
                {
                    MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Quay về màn hình đăng nhập
                    FormLogin login = new FormLogin();
                    login.Show();
                    this.Hide();
                }
                else if (responseType == "REGISTER_FAIL")
                {
                    string reason = (string)resp.reason;
                    MessageBox.Show("Đăng ký thất bại: " + reason, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Phản hồi không xác định từ server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "Đăng ký";
                
                try
                {
                    await _tcp.CloseAsync();
                }
                catch { }
            }
        }
    }
}
