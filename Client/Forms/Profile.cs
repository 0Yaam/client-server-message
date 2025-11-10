using Client.Services;
using Shared.OL;
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
            _tcp = tcp; // may be null in some flows

            // Map controls
            lblDisplayName.Text = _me.DisplayName ?? _me.Username;
            lblUserName.Text = _me.Username;
            lblRole.Text = _me.Role.ToString();

            // Load avatar if exists (use stream to avoid locking file)
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

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // Close profile and return to login
            try
            {
                // Attempt to close TCP connection if present
                try { _tcp?.CloseAsync().Wait(500); } catch { }
            }
            catch { }

            var login = new FormLogin();
            login.Show();

            // Close the main chat form if it's open (owner)
            if (this.Owner != null)
            {
                try { this.Owner.Close(); } catch { }
            }

            this.Close();
        }

        private async void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // Copy to app data folder for persistence (optional)
                        var destDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Avatars");
                        Directory.CreateDirectory(destDir);
                        var dest = Path.Combine(destDir, Path.GetFileName(dlg.FileName));
                        File.Copy(dlg.FileName, dest, true);

                        // Update local account avatar path
                        _me.Avatar = dest;

                        // Update UI using stream to avoid locking
                        using (var fs = File.OpenRead(dest))
                        {
                            var img = Image.FromStream(fs);
                            // clone image to keep after stream closed
                            guna2PictureBox1.Image = new Bitmap(img);
                        }

                        // Send avatar to server so it can save and broadcast
                        try
                        {
                            if (_tcp != null)
                            {
                                byte[] data = File.ReadAllBytes(dest);
                                string b64 = Convert.ToBase64String(data);
                                string ext = Path.GetExtension(dest);
                                await _tcp.SendAsync(new { type = "AVATAR_UPLOAD", image = b64, ext = ext });

                                // We expect server to broadcast AVATAR_UPDATED to all clients; ChatForm listens to that.
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

        // Change password immediately locally and persist to client Data/users.json; optionally notify server without waiting response
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
                // Verify old password against local account data if possible
                bool oldMatches = false;
                if (string.IsNullOrEmpty(_me.Salt))
                {
                    oldMatches = _me.PasswordHash == oldPass;
                }
                else
                {
                    var hashedOld = HashPassword(oldPass, _me.Salt);
                    oldMatches = _me.PasswordHash == hashedOld;
                }

                if (!oldMatches)
                {
                    // If local verification fails, still allow change but warn user
                    var res = MessageBox.Show("Mật khẩu cũ không trùng với dữ liệu cục bộ. Bạn có muốn tiếp tục và ghi đè?","Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res != DialogResult.Yes) return;
                }

                // Generate new salt/hash and update local account
                var newSalt = GenerateSalt();
                var newHash = HashPassword(newPass, newSalt);
                _me.Salt = newSalt;
                _me.PasswordHash = newHash;

                // Persist to client Data/users.json if exists
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
                            found.PasswordHash = _me.PasswordHash;
                            found.Salt = _me.Salt;
                        }
                        else
                        {
                            // add local record
                            arr.Add(_me);
                        }

                        File.WriteAllText(file, JsonConvert.SerializeObject(arr, Formatting.Indented));
                    }
                    else
                    {
                        // create new file with this account
                        var arr = new Account[] { _me };
                        File.WriteAllText(file, JsonConvert.SerializeObject(arr, Formatting.Indented));
                    }
                }
                catch { /* ignore persistence errors */ }

                // Try notify server but do not wait for response
                try
                {
                    if (_tcp != null)
                    {
                        // fire-and-forget
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

        private static string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[32];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string saltedPassword = salt + password;
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
