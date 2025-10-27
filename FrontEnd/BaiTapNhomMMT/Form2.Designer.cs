namespace BaiTapNhomMMT
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2Panel pnlLeft;
        private System.Windows.Forms.ListBox lstContacts;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2Button btnLogout;
        private System.Windows.Forms.Label lblHeader;
        private Guna.UI2.WinForms.Guna2Panel pnlChat;
        private System.Windows.Forms.FlowLayoutPanel flowMessages;
        private Guna.UI2.WinForms.Guna2Panel pnlInput;
        private Guna.UI2.WinForms.Guna2TextBox txtMessage;
        private Guna.UI2.WinForms.Guna2Button btnSend;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.pnlLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnMenu = new Guna.UI2.WinForms.Guna2Button();
            this.lstContacts = new System.Windows.Forms.ListBox();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pnlChat = new Guna.UI2.WinForms.Guna2Panel();
            this.flowMessages = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlInput = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSend = new Guna.UI2.WinForms.Guna2Button();
            this.txtMessage = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnLogout = new Guna.UI2.WinForms.Guna2Button();
            this.Menu = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            this.ItemTaoNhom = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemThemBan = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemXoaBan = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLeft.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlChat.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlLeft.Controls.Add(this.lstContacts);
            this.pnlLeft.Controls.Add(this.guna2Panel1);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.FillColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(284, 481);
            this.pnlLeft.TabIndex = 0;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.guna2Panel1.Controls.Add(this.txtSearch);
            this.guna2Panel1.Controls.Add(this.btnMenu);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(284, 40);
            this.guna2Panel1.TabIndex = 3;
            this.guna2Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel1_Paint);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtSearch.BorderRadius = 10;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(46, 0);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(238, 40);
            this.txtSearch.TabIndex = 0;
            // 
            // btnMenu
            // 
            this.btnMenu.BorderRadius = 6;
            this.btnMenu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMenu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMenu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMenu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMenu.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnMenu.ForeColor = System.Drawing.Color.White;
            this.btnMenu.Location = new System.Drawing.Point(0, 0);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(46, 40);
            this.btnMenu.TabIndex = 2;
            this.btnMenu.Text = "☰";
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // lstContacts
            // 
            this.lstContacts.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lstContacts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstContacts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstContacts.ItemHeight = 23;
            this.lstContacts.Location = new System.Drawing.Point(0, 40);
            this.lstContacts.Name = "lstContacts";
            this.lstContacts.Size = new System.Drawing.Size(284, 441);
            this.lstContacts.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.FillColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlHeader.Location = new System.Drawing.Point(284, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(635, 40);
            this.pnlHeader.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblHeader.Location = new System.Drawing.Point(6, 6);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(261, 28);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Chưa chọn cuộc trò chuyện";
            this.lblHeader.Click += new System.EventHandler(this.lblHeader_Click);
            // 
            // pnlChat
            // 
            this.pnlChat.Controls.Add(this.flowMessages);
            this.pnlChat.Controls.Add(this.pnlInput);
            this.pnlChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.pnlChat.Location = new System.Drawing.Point(284, 40);
            this.pnlChat.Name = "pnlChat";
            this.pnlChat.Size = new System.Drawing.Size(635, 441);
            this.pnlChat.TabIndex = 2;
            // 
            // flowMessages
            // 
            this.flowMessages.AutoScroll = true;
            this.flowMessages.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.flowMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMessages.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowMessages.Location = new System.Drawing.Point(0, 0);
            this.flowMessages.Margin = new System.Windows.Forms.Padding(10);
            this.flowMessages.Name = "flowMessages";
            this.flowMessages.Padding = new System.Windows.Forms.Padding(15);
            this.flowMessages.Size = new System.Drawing.Size(635, 377);
            this.flowMessages.TabIndex = 0;
            this.flowMessages.WrapContents = false;
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.btnSend);
            this.pnlInput.Controls.Add(this.txtMessage);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlInput.FillColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlInput.Location = new System.Drawing.Point(0, 377);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Padding = new System.Windows.Forms.Padding(10);
            this.pnlInput.Size = new System.Drawing.Size(635, 64);
            this.pnlInput.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSend.BorderRadius = 10;
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSend.Location = new System.Drawing.Point(526, 10);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(99, 44);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Gửi";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txtMessage.BorderRadius = 10;
            this.txtMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMessage.DefaultText = "";
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.FillColor = System.Drawing.Color.GhostWhite;
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMessage.Location = new System.Drawing.Point(10, 10);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.PlaceholderText = "Nhập tin nhắn...";
            this.txtMessage.SelectedText = "";
            this.txtMessage.Size = new System.Drawing.Size(615, 44);
            this.txtMessage.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BorderRadius = 8;
            this.btnLogout.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(135)))), ((int)(((byte)(255)))));
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(720, 5);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 30);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // Menu
            // 
            this.Menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemTaoNhom,
            this.ItemThemBan,
            this.ItemXoaBan,
            this.ItemDangXuat});
            this.Menu.Name = "Menu";
            this.Menu.RenderStyle.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(143)))), ((int)(((byte)(255)))));
            this.Menu.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
            this.Menu.RenderStyle.ColorTable = null;
            this.Menu.RenderStyle.RoundedEdges = true;
            this.Menu.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
            this.Menu.RenderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.Menu.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.Menu.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
            this.Menu.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.Menu.Size = new System.Drawing.Size(147, 100);
            this.Menu.Text = "☰";
            // 
            // ItemTaoNhom
            // 
            this.ItemTaoNhom.Name = "ItemTaoNhom";
            this.ItemTaoNhom.Size = new System.Drawing.Size(210, 24);
            this.ItemTaoNhom.Text = "Tạo nhóm";
            // 
            // ItemThemBan
            // 
            this.ItemThemBan.Name = "ItemThemBan";
            this.ItemThemBan.Size = new System.Drawing.Size(210, 24);
            this.ItemThemBan.Text = "Thêm bạn";
            // 
            // ItemXoaBan
            // 
            this.ItemXoaBan.Name = "ItemXoaBan";
            this.ItemXoaBan.Size = new System.Drawing.Size(210, 24);
            this.ItemXoaBan.Text = "Xóa Bạn";
            // 
            // ItemDangXuat
            // 
            this.ItemDangXuat.Name = "ItemDangXuat";
            this.ItemDangXuat.Size = new System.Drawing.Size(210, 24);
            this.ItemDangXuat.Text = "Đăng xuất";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 481);
            this.Controls.Add(this.pnlChat);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlLeft);
            this.Name = "Form2";
            this.Text = "Telegram - Guna2 UI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.pnlLeft.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlChat.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.Menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2ContextMenuStrip Menu;
        private Guna.UI2.WinForms.Guna2Button btnMenu;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.ToolStripMenuItem ItemTaoNhom;
        private System.Windows.Forms.ToolStripMenuItem ItemThemBan;
        private System.Windows.Forms.ToolStripMenuItem ItemXoaBan;
        private System.Windows.Forms.ToolStripMenuItem ItemDangXuat;
    }
}
