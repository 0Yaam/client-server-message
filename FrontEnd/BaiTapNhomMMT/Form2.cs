using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapNhomMMT
{
    public partial class Form2 : Form
    {
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); 
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Menu.ItemClicked += Menu_ItemClicked;
            lstContacts.Items.Add("Nguyễn An");
            lstContacts.Items.Add("Lê Bình");
            lstContacts.Items.Add("Phạm Cường");
            lstContacts.Items.Add("Bạn demo");
            lstContacts.SelectedIndexChanged += (s, ev) =>
            {
                lblHeader.Text = lstContacts.SelectedItem?.ToString();
                flowMessages.Controls.Clear();
            };
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text)) return;

            var msgBubble = new Guna.UI2.WinForms.Guna2Panel()
            {
                AutoSize = true,
                BorderRadius = 10,
                FillColor = System.Drawing.Color.FromArgb(15, 135, 255),
                Padding = new Padding(10),
                MaximumSize = new Size(400, 0),
                Margin = new Padding(5, 5, 10, 5) 
            };

            
            var lblMsg = new Label()
            {
                Text = txtMessage.Text,
                AutoSize = true,
                BackColor = System.Drawing.Color.FromArgb(15, 135, 255),
                ForeColor = System.Drawing.Color.White,
                Font = new Font("Segoe UI", 10),
                MaximumSize = new Size(380, 0)
            };

            msgBubble.Controls.Add(lblMsg);
            flowMessages.Controls.Add(msgBubble);
            txtMessage.Clear();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Quay lại form đăng nhập
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
            this.Hide();
        }

        private void lblHeader_Click(object sender, EventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Point location = new Point(btnMenu.Left, btnMenu.Bottom);
            Menu.Show(this, location);
        }
        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string selected = e.ClickedItem.Text;

            switch (selected)
            {
                case "Tạo nhóm":
                    MessageBox.Show("Chức năng tạo nhóm đang phát triển!");
                    break;
                case "Thêm bạn":
                    MessageBox.Show("Chức năng thêm bạn đang phát triển!");
                    break;
                case "Xóa bạn":
                    MessageBox.Show("Chức năng xóa bạn đang phát triển!");
                    break;
                case "Đăng xuất":
                    FormLogin f = new FormLogin();
                    f.Show();
                    this.Hide();
                    break;
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
