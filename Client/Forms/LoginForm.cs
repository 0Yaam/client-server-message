using Client.Services;
using Guna.UI2.WinForms;
using Shared.OL;
using System;
using System.Windows.Forms;
using Client.Forms; 


namespace Client
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            Account acc;
            if (AccountJsonService.Login(username, password, out acc))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo");

                this.Hide();
                var chat = new ChatForm();
                chat.Show();

            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignUpForm form = new SignUpForm();
            form.Show();
            this.Hide();
        }
    }
}
