using Server.ServerCore;
using Shared.OL;
using System;
using System.IO;
using System.Windows.Forms;

namespace Server.Forms
{
    public partial class UpdateForm : Form
    {
        private readonly string _originalUsername;
        private byte[] _newAvatarData = null;
        private string _newAvatarExt = null;

        public UpdateForm()
        {
            InitializeComponent();
        }

        public UpdateForm(string username)
        {
            InitializeComponent();
            _originalUsername = username;

            this.Load += UpdateForm_Load;

            btnExit.Click -= BtnExit_Click;
            btnExit.Click += BtnExit_Click;

            txtBrowse.Click -= TxtBrowse_Click;
            txtBrowse.Click += TxtBrowse_Click;

            btnUpdate.Click -= BtnUpdate_Click;
            btnUpdate.Click += BtnUpdate_Click;

            // populate role combo
            cbbRole.Items.Clear();
            cbbRole.Items.Add(UserRole.Admin.ToString());
            cbbRole.Items.Add(UserRole.User.ToString());
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void LoadAccount()
        {
            var acc = AuthManager.GetUser(_originalUsername);
            if (acc == null)
            {
                MessageBox.Show("Tài khoản không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            txtDisplayName.Text = acc.DisplayName;
            txtUserName.Text = acc.Username;
            // Salt readonly text box is guna2TextBox1
            guna2TextBox1.Text = acc.Salt ?? string.Empty;

            // Set role
            cbbRole.SelectedItem = acc.Role.ToString();

            // Load avatar if exists
            try
            {
                if (!string.IsNullOrEmpty(acc.Avatar) && File.Exists(acc.Avatar))
                {
                    pbAvatar.Image = System.Drawing.Image.FromFile(acc.Avatar);
                }
            }
            catch { }
        }

        private void TxtBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var path = dlg.FileName;
                try
                {
                    _newAvatarData = File.ReadAllBytes(path);
                    _newAvatarExt = Path.GetExtension(path);
                    using (var ms = new MemoryStream(_newAvatarData))
                    {
                        pbAvatar.Image = System.Drawing.Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể đọc ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var newDisplay = txtDisplayName.Text?.Trim() ?? string.Empty;
            var newUsername = txtUserName.Text?.Trim() ?? string.Empty;
            var newPassword = txtOldPassword.Text ?? string.Empty; // reused field for new password
            var roleStr = cbbRole.SelectedItem as string ?? UserRole.User.ToString();

            if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newDisplay))
            {
                MessageBox.Show("Tên đăng nhập và tên hiển thị không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Enum.TryParse<UserRole>(roleStr, out var role)) role = UserRole.User;

            // Update user (may change username)
            if (!AuthManager.UpdateUser(_originalUsername, newUsername, newDisplay, role, out var err))
            {
                MessageBox.Show("Cập nhật thất bại: " + err, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If password provided, set it
            if (!string.IsNullOrEmpty(newPassword))
            {
                if (!AuthManager.SetPassword(newUsername, newPassword, out var perr))
                {
                    MessageBox.Show("Không thể cập nhật mật khẩu: " + perr, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // If avatar changed, save it
            if (_newAvatarData != null)
            {
                if (!AuthManager.UpdateAvatar(newUsername, _newAvatarData, _newAvatarExt, out var savedPath, out var aerr))
                {
                    MessageBox.Show("Không thể cập nhật avatar: " + aerr, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("Cập nhật thành công", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
