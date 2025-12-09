namespace Server
{
    partial class ServerForm
    {



        private System.ComponentModel.IContainer components = null;





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
            this.btnStart = new Guna.UI2.WinForms.Guna2Button();
            this.listBoxOnline = new System.Windows.Forms.ListBox();
            this.btnStop = new Guna.UI2.WinForms.Guna2Button();
            this.txtServerSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lvListUser = new System.Windows.Forms.ListView();
            this.txtServerMessage = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnServerSend = new Guna.UI2.WinForms.Guna2Button();
            this.txtIP = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtPort = new Guna.UI2.WinForms.Guna2TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msThongTin = new System.Windows.Forms.ToolStripMenuItem();
            this.msPrivateChat = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLogout = new Guna.UI2.WinForms.Guna2Button();
            this.rdDisplayName = new System.Windows.Forms.RadioButton();
            this.rdUserName = new System.Windows.Forms.RadioButton();
            this.cbbRole = new System.Windows.Forms.ComboBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BorderRadius = 8;
            this.btnStart.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStart.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStart.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStart.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(414, 28);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 34);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // listBoxOnline
            // 
            this.listBoxOnline.FormattingEnabled = true;
            this.listBoxOnline.Location = new System.Drawing.Point(6, 19);
            this.listBoxOnline.Name = "listBoxOnline";
            this.listBoxOnline.Size = new System.Drawing.Size(149, 251);
            this.listBoxOnline.TabIndex = 1;
            // 
            // btnStop
            // 
            this.btnStop.BorderRadius = 8;
            this.btnStop.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStop.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStop.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStop.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(545, 28);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 34);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtServerSearch
            // 
            this.txtServerSearch.BorderRadius = 5;
            this.txtServerSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtServerSearch.DefaultText = "";
            this.txtServerSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtServerSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtServerSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtServerSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerSearch.Location = new System.Drawing.Point(6, 16);
            this.txtServerSearch.Name = "txtServerSearch";
            this.txtServerSearch.PlaceholderText = "";
            this.txtServerSearch.SelectedText = "";
            this.txtServerSearch.Size = new System.Drawing.Size(200, 30);
            this.txtServerSearch.TabIndex = 3;
            // 
            // lvListUser
            // 
            this.lvListUser.CheckBoxes = true;
            this.lvListUser.HideSelection = false;
            this.lvListUser.Location = new System.Drawing.Point(12, 125);
            this.lvListUser.Name = "lvListUser";
            this.lvListUser.Size = new System.Drawing.Size(502, 265);
            this.lvListUser.TabIndex = 5;
            this.lvListUser.UseCompatibleStateImageBehavior = false;
            // 
            // txtServerMessage
            // 
            this.txtServerMessage.BorderRadius = 5;
            this.txtServerMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtServerMessage.DefaultText = "";
            this.txtServerMessage.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtServerMessage.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtServerMessage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerMessage.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerMessage.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtServerMessage.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerMessage.Location = new System.Drawing.Point(12, 401);
            this.txtServerMessage.Name = "txtServerMessage";
            this.txtServerMessage.PlaceholderText = "";
            this.txtServerMessage.SelectedText = "";
            this.txtServerMessage.Size = new System.Drawing.Size(200, 34);
            this.txtServerMessage.TabIndex = 6;
            // 
            // btnServerSend
            // 
            this.btnServerSend.BorderRadius = 8;
            this.btnServerSend.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnServerSend.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnServerSend.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnServerSend.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnServerSend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnServerSend.ForeColor = System.Drawing.Color.White;
            this.btnServerSend.Location = new System.Drawing.Point(293, 401);
            this.btnServerSend.Name = "btnServerSend";
            this.btnServerSend.Size = new System.Drawing.Size(100, 34);
            this.btnServerSend.TabIndex = 7;
            this.btnServerSend.Text = "Send";
            this.btnServerSend.Click += new System.EventHandler(this.btnServerSend_Click_1);
            // 
            // txtIP
            // 
            this.txtIP.BorderRadius = 5;
            this.txtIP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIP.DefaultText = "";
            this.txtIP.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtIP.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtIP.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtIP.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtIP.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtIP.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtIP.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtIP.Location = new System.Drawing.Point(12, 28);
            this.txtIP.Name = "txtIP";
            this.txtIP.PlaceholderText = "";
            this.txtIP.SelectedText = "";
            this.txtIP.Size = new System.Drawing.Size(200, 34);
            this.txtIP.TabIndex = 11;
            // 
            // txtPort
            // 
            this.txtPort.BorderRadius = 5;
            this.txtPort.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPort.DefaultText = "";
            this.txtPort.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPort.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPort.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPort.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPort.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPort.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPort.Location = new System.Drawing.Point(249, 28);
            this.txtPort.Name = "txtPort";
            this.txtPort.PlaceholderText = "";
            this.txtPort.SelectedText = "";
            this.txtPort.Size = new System.Drawing.Size(122, 34);
            this.txtPort.TabIndex = 12;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msThongTin,
            this.msPrivateChat});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(711, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msThongTin
            // 
            this.msThongTin.Name = "msThongTin";
            this.msThongTin.Size = new System.Drawing.Size(70, 20);
            this.msThongTin.Text = "Thông tin";
            this.msThongTin.Click += new System.EventHandler(this.msThongTin_Click);
            // 
            // msPrivateChat
            // 
            this.msPrivateChat.Name = "msPrivateChat";
            this.msPrivateChat.Size = new System.Drawing.Size(81, 20);
            this.msPrivateChat.Text = "Private chat";
            // 
            // btnLogout
            // 
            this.btnLogout.BorderRadius = 8;
            this.btnLogout.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(586, 401);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(108, 34);
            this.btnLogout.TabIndex = 14;
            this.btnLogout.Text = "Logout";
            // 
            // rdDisplayName
            // 
            this.rdDisplayName.AutoSize = true;
            this.rdDisplayName.Location = new System.Drawing.Point(212, 23);
            this.rdDisplayName.Name = "rdDisplayName";
            this.rdDisplayName.Size = new System.Drawing.Size(87, 17);
            this.rdDisplayName.TabIndex = 15;
            this.rdDisplayName.TabStop = true;
            this.rdDisplayName.Text = "DisplayName";
            this.rdDisplayName.UseVisualStyleBackColor = true;
            // 
            // rdUserName
            // 
            this.rdUserName.AutoSize = true;
            this.rdUserName.Location = new System.Drawing.Point(305, 23);
            this.rdUserName.Name = "rdUserName";
            this.rdUserName.Size = new System.Drawing.Size(75, 17);
            this.rdUserName.TabIndex = 17;
            this.rdUserName.TabStop = true;
            this.rdUserName.Text = "UserName";
            this.rdUserName.UseVisualStyleBackColor = true;
            // 
            // cbbRole
            // 
            this.cbbRole.FormattingEnabled = true;
            this.cbbRole.Location = new System.Drawing.Point(386, 22);
            this.cbbRole.Name = "cbbRole";
            this.cbbRole.Size = new System.Drawing.Size(97, 21);
            this.cbbRole.TabIndex = 18;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(218, 413);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(69, 17);
            this.chkSelectAll.TabIndex = 19;
            this.chkSelectAll.Text = "Select all";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxOnline);
            this.groupBox1.Location = new System.Drawing.Point(526, 114);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 276);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Online";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdUserName);
            this.groupBox2.Controls.Add(this.txtServerSearch);
            this.groupBox2.Controls.Add(this.rdDisplayName);
            this.groupBox2.Controls.Add(this.cbbRole);
            this.groupBox2.Location = new System.Drawing.Point(12, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(502, 52);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnServerSend);
            this.Controls.Add(this.txtServerMessage);
            this.Controls.Add(this.lvListUser);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ServerForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnStart;
        private System.Windows.Forms.ListBox listBoxOnline;
        private Guna.UI2.WinForms.Guna2Button btnStop;
        private Guna.UI2.WinForms.Guna2TextBox txtServerSearch;
        private System.Windows.Forms.ListView lvListUser;
        private Guna.UI2.WinForms.Guna2TextBox txtServerMessage;
        private Guna.UI2.WinForms.Guna2Button btnServerSend;
        private Guna.UI2.WinForms.Guna2TextBox txtIP;
        private Guna.UI2.WinForms.Guna2TextBox txtPort;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msThongTin;
        private System.Windows.Forms.ToolStripMenuItem msPrivateChat;
        private Guna.UI2.WinForms.Guna2Button btnLogout;
        private System.Windows.Forms.RadioButton rdDisplayName;
        private System.Windows.Forms.RadioButton rdUserName;
        private System.Windows.Forms.ComboBox cbbRole;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

