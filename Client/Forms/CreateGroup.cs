using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Client.Forms
{
    public partial class CreateGroup : Form
    {
        public class GroupCreatedEventArgs : EventArgs
        {
            public string GroupName { get; }
            public string[] Members { get; }
            public byte[] AvatarData { get; }
            public string AvatarExt { get; }

            public GroupCreatedEventArgs(string groupName, string[] members, byte[] avatarData = null, string avatarExt = null)
            {
                GroupName = groupName;
                Members = members;
                AvatarData = avatarData;
                AvatarExt = avatarExt;
            }
        }



        public event EventHandler<GroupCreatedEventArgs> GroupCreated;

        private readonly string[] _availableUsers;
        private byte[] _groupAvatarData = null;
        private string _groupAvatarExt = null;

        public CreateGroup(IEnumerable<string> users)
        {
            InitializeComponent();
            _availableUsers = (users ?? Array.Empty<string>()).ToArray();


            lvList.View = View.Details;
            lvList.Columns.Clear();
            lvList.Columns.Add("Thành viên", lvList.ClientSize.Width - 4, HorizontalAlignment.Left);
            lvList.HeaderStyle = ColumnHeaderStyle.None;
            lvList.FullRowSelect = true;
            lvList.CheckBoxes = true;
            lvList.HideSelection = false;
            lvList.MultiSelect = false;
            lvList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            PopulateList();

            btnCreate.Click -= BtnCreate_Click;
            btnCreate.Click += BtnCreate_Click;

            // wire search box
            txtSearch.TextChanged -= TxtSearch_TextChanged;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // wire browse and exit
            btnBrowse.Click -= BtnBrowse_Click;
            btnBrowse.Click += BtnBrowse_Click;

            btnExit.Click -= BtnExit_Click;
            btnExit.Click += BtnExit_Click;
        }

        private void PopulateList(string filter = null)
        {
            lvList.Items.Clear();
            var items = _availableUsers;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var q = filter.Trim();
                items = items.Where(u => u.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
            }

            foreach (var u in items)
            {
                var li = new ListViewItem(u) { Checked = false };
                lvList.Items.Add(li);
            }

            if (lvList.Columns.Count > 0)
                lvList.Columns[0].Width = -2;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var groupName = txtGroupName.Text?.Trim();
            if (string.IsNullOrEmpty(groupName))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm!", "Thiếu tên nhóm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGroupName.Focus();
                return;
            }

            var selected = lvList.CheckedItems.Cast<ListViewItem>().Select(i => i.Text).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray();
            if (selected.Length == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một thành viên!", "Chưa chọn thành viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GroupCreated?.Invoke(this, new GroupCreatedEventArgs(groupName, selected, _groupAvatarData, _groupAvatarExt));
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var text = txtSearch.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                PopulateList(null);
                return;
            }

            PopulateList(text);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var path = dlg.FileName;
                try
                {
                    _groupAvatarData = File.ReadAllBytes(path);
                    _groupAvatarExt = Path.GetExtension(path);
                    using (var ms = new MemoryStream(_groupAvatarData))
                    {
                        using (var src = System.Drawing.Image.FromStream(ms))
                        {
                            // create a copy so the stream can be closed safely
                            var bmp = new System.Drawing.Bitmap(src);
                            // dispose previous image
                            try { var old = pbAvatar.Image; if (old != null && !object.ReferenceEquals(old, bmp)) old.Dispose(); } catch { }
                            pbAvatar.Image = bmp;
                        }
                    }

                    // ensure visible and properly sized
                    try { pbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; } catch { }
                    try { pbAvatar.BringToFront(); } catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể đọc ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
