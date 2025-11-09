using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class CreateGroup : Form
    {
        public class GroupCreatedEventArgs : EventArgs
        {
            public string GroupName { get; }
            public string[] Members { get; }

            public GroupCreatedEventArgs(string groupName, string[] members)
            {
                GroupName = groupName;
                Members = members;
            }
        }

        /// <summary>
        /// Raised when user clicks Create and the input is valid.
        /// </summary>
        public event EventHandler<GroupCreatedEventArgs> GroupCreated;

        private readonly string[] _availableUsers;

        public CreateGroup(IEnumerable<string> users)
        {
            InitializeComponent();
            _availableUsers = (users ?? Array.Empty<string>()).ToArray();

            // Configure ListView for simple display
            lvList.View = View.Details;
            lvList.Columns.Clear();
            lvList.Columns.Add("User", lvList.ClientSize.Width - 4, HorizontalAlignment.Left);
            lvList.HeaderStyle = ColumnHeaderStyle.None;
            lvList.FullRowSelect = true;
            lvList.CheckBoxes = true;
            lvList.HideSelection = false;
            lvList.MultiSelect = false;
            lvList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            PopulateList();

            btnCreate.Click -= BtnCreate_Click;
            btnCreate.Click += BtnCreate_Click;
        }

        private void PopulateList()
        {
            lvList.Items.Clear();
            foreach (var u in _availableUsers)
            {
                var li = new ListViewItem(u) { Checked = false };
                lvList.Items.Add(li);
            }
            // ensure column width fits
            if (lvList.Columns.Count > 0)
                lvList.Columns[0].Width = -2; // autosize
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var groupName = txtNameGroup.Text?.Trim();
            if (string.IsNullOrEmpty(groupName))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm.", "Thiếu tên nhóm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameGroup.Focus();
                return;
            }

            var selected = lvList.CheckedItems.Cast<ListViewItem>().Select(i => i.Text).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray();
            if (selected.Length == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một thành viên.", "Chưa chọn thành viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Raise event to caller (ChatForm) so it can send to server / update UI
            GroupCreated?.Invoke(this, new GroupCreatedEventArgs(groupName, selected));

            // Close dialog
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
