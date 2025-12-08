using Client.Services;
using Shared.OL;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Linq;

namespace Client.Forms
{
    public partial class Profile : Form
    {
        private readonly Account _me;
        private readonly TcpService _tcp;

        public Profile(Account me, TcpService tcp)
        {
            InitializeComponent();
            _me = me ?? throw new ArgumentNullException(nameof(me));
            _tcp = tcp;

            // Hiển thị thông tin tài khoản
            lblDisplayName.Text = _me.DisplayName ?? _me.Username;
            lblUserName.Text = _me.Username;
            lblRole.Text = _me.Role.ToString();

            // Tải avatar hiện tại nếu có
            try
            {
                if (!string.IsNullOrEmpty(_me.Avatar) && File.Exists(_me.Avatar))
                {
                    using (var fs = File.OpenRead(_me.Avatar))
                    {
                        guna2PictureBox1.Image = Image.FromStream(fs);
                    }
                }
            }
            catch { }

            btnBrowse.Click += BtnBrowse_Click;
            btnLogout.Click += BtnLogout_Click;
            btnChangePass.Click += btnChangePass_Click;
        }

        // Đăng xuất ra màn hình đăng nhập
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                try { _tcp?.CloseAsync().Wait(500); } catch { }
            }
            catch { }

            var login = new FormLogin();
            login.Show();

            if (this.Owner != null)
            {
                try { this.Owner.Close(); } catch { }
            }

            this.Close();
        }

        // Chọn và upload avatar mới
        private async void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // Lưu avatar vào thư mục Data/Avatars
                        var destDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Avatars");
                        Directory.CreateDirectory(destDir);
                        var dest = Path.Combine(destDir, Path.GetFileName(dlg.FileName));
                        File.Copy(dlg.FileName, dest, true);

                        // Cập nhật đường dẫn avatar cục bộ
                        _me.Avatar = dest;

                        // Hiển thị avatar mới
                        using (var fs = File.OpenRead(dest))
                        {
                            var img = Image.FromStream(fs);
                            guna2PictureBox1.Image = new Bitmap(img);
                        }

                        // Upload avatar lên server
                        try
                        {
                            if (_tcp != null)
                            {
                                byte[] data = File.ReadAllBytes(dest);
                                string b64 = Convert.ToBase64String(data);
                                string ext = Path.GetExtension(dest);
                                await _tcp.SendAsync(new { type = "AVATAR_UPLOAD", image = b64, ext = ext });
                            }
                        }
                        catch { }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tải avatar: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Đổi mật khẩu cục bộ và gửi yêu cầu lên server
        private async void btnChangePass_Click(object sender, EventArgs e)
        {
            var oldPass = txtOldPassword.Text;
            var newPass = txtNewPasswỏd.Text;

            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng điền mật khẩu cũ và mật khẩu mới", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra khớp mật khẩu cũ với dữ liệu cục bộ
                bool oldMatches = _me.Password == oldPass;

                if (!oldMatches)
                {
                    var res = MessageBox.Show("Mật khẩu cũ không trùng với dữ liệu cục bộ. Bạn có muốn tiếp tục và ghi đè?","Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res != DialogResult.Yes) return;
                }

                // Lưu mật khẩu mới cục bộ
                _me.Password = newPass;

                // Cập nhật file Data/users.json cục bộ
                try
                {
                    var dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                    Directory.CreateDirectory(dataDir);
                    var file = Path.Combine(dataDir, "users.json");
                    if (File.Exists(file))
                    {
                        var json = File.ReadAllText(file);
                        var list = JsonConvert.DeserializeObject<Account[]>(json) ?? new Account[0];
                        var arr = list.ToList();
                        var found = arr.FirstOrDefault(a => a.Username.Equals(_me.Username, StringComparison.OrdinalIgnoreCase));
                        if (found != null)
                        {
                            found.Password = _me.Password;
                        }
                        else
                        {
                            arr.Add(_me);
                        }

                        File.WriteAllText(file, JsonConvert.SerializeObject(arr, Formatting.Indented));
                    }
                    else
                    {
                        var arr = new Account[] { _me };
                        File.WriteAllText(file, JsonConvert.SerializeObject(arr, Formatting.Indented));
                    }
                }
                catch {  }

                // Gửi yêu cầu đổi mật khẩu tới server
                try
                {
                    if (_tcp != null)
                    {
                        _ = _tcp.SendAsync(new { type = "PASS_CHANGE", oldPassword = oldPass, newPassword = newPass });
                    }
                }
                catch { }

                MessageBox.Show("Mật khẩu đã được thay đổi cục bộ.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thay đổi mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
