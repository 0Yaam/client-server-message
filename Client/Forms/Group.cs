using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Client.Forms.Controls
{
    public partial class Group : Form
    {
        // Danh sách tất cả user đầu vào
        private readonly List<string> _candidates = new List<string>();
        // Đường dẫn ảnh được chọn (nếu có)
        private string _avatarPath;

        // Event trả về cho form gọi
        public class GroupCreatedEventArgs : EventArgs
        {
            public string GroupName { get; set; }
            public string[] Members { get; set; }
            public string AvatarPath { get; set; } // có thể null
        }
        public event EventHandler<GroupCreatedEventArgs> GroupCreated;

        public Group()
        {
            InitializeComponent();
            InitRuntime();
        }

        public Group(IEnumerable<string> candidates) : this()
        {
            if (candidates != null)
            {
                _candidates.AddRange(candidates.Where(x => !string.IsNullOrWhiteSpace(x)));
                RefreshListView();
            }
        }

        private void InitRuntime()
        {
            try
            {
                // Không thay đổi design: chỉ cấu hình tối thiểu khi runtime
                lvList.View = View.List; // hiển thị đơn giản tên user
            }
            catch { }
        }

        private void RefreshListView(string filter = null)
        {
            lvList.BeginUpdate();
            try
            {
                lvList.Items.Clear();
                IEnumerable<string> data = _candidates;
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filter = filter.Trim();
                    data = data.Where(u => u.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                foreach (var u in data)
                {
                    var item = new ListViewItem(u) { Checked = false };
                    lvList.Items.Add(item);
                }
            }
            finally
            {
                lvList.EndUpdate();
            }
        }

        private void pbAvatar_Click(object sender, EventArgs e)
        {
            // Cho phép click avatar để chọn lại
            BrowseAvatar();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void txtGroupName_TextChanged(object sender, EventArgs e)
        {
            // Có thể thêm validate realtime nếu cần
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshListView(txtSearch.Text);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            BrowseAvatar();
        }

        private void BrowseAvatar()
        {
            try
            {
                using (var dlg = new OpenFileDialog())
                {
                    dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    _avatarPath = dlg.FileName;
                    try
                    {
                        using (var fs = File.OpenRead(_avatarPath))
                        {
                            var img = Image.FromStream(fs);
                            pbAvatar.Image = new Bitmap(img);
                            pbAvatar.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chọn ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string groupName = txtGroupName.Text.Trim();
                if (string.IsNullOrEmpty(groupName))
                {
                    MessageBox.Show("Vui lòng nhập tên nhóm", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGroupName.Focus();
                    return;
                }

                var members = lvList.Items.Cast<ListViewItem>()
                    .Where(i => i.Checked)
                    .Select(i => i.Text)
                    .Distinct(StringComparer.Ordinal)
                    .ToList();

                if (members.Count < 1)
                {
                    MessageBox.Show("Chọn ít nhất 1 thành viên", "Thiếu thành viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Raise event
                GroupCreated?.Invoke(this, new GroupCreatedEventArgs
                {
                    GroupName = groupName,
                    Members = members.ToArray(),
                    AvatarPath = _avatarPath
                });

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tạo nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Không cần xử lý thêm
        }
    }
}
