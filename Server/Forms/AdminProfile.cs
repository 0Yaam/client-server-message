using Server.ServerCore;
using System;
using System.IO;
using System.Windows.Forms;

namespace Server
{
    public partial class AdminProfile : Form
    {
        private readonly string _username;
        private readonly string _displayName;
        private string _password;

        public AdminProfile(string username, string displayName, string password)
        {
            InitializeComponent();
            _username = username;
            _displayName = displayName;
            _password = password;

            txtUsername.Text = _username;
            txtDisplayName.Text = _displayName;
            txtPassword.Text = _password;
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            var newPass = txtPasswordPlain.Text;
            if (string.IsNullOrWhiteSpace(newPass) || newPass.Length < 6)
            {
                MessageBox.Show("M?t kh?u m?i ph?i có ít nh?t 6 ký t?", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AuthManager.SetPassword(_username, newPass, out string err))
            {
                MessageBox.Show("??t m?t kh?u thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Không th? ??t m?t kh?u: " + err, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
