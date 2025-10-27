namespace BaiTapNhomMMT
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2Panel pnlMain;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2Button btnLogin;
        private Guna.UI2.WinForms.Guna2Button btnExit;
        private Guna.UI2.WinForms.Guna2PictureBox logoBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.pnlMain = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new Guna.UI2.WinForms.Guna2Button();
            this.btnLogin = new Guna.UI2.WinForms.Guna2Button();
            this.txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.logoBox = new Guna.UI2.WinForms.Guna2PictureBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlMain.BorderRadius = 20;
            this.pnlMain.Controls.Add(this.guna2HtmlLabel1);
            this.pnlMain.Controls.Add(this.linkLabel1);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.btnExit);
            this.pnlMain.Controls.Add(this.btnLogin);
            this.pnlMain.Controls.Add(this.txtPassword);
            this.pnlMain.Controls.Add(this.txtUsername);
            this.pnlMain.Controls.Add(this.logoBox);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.FillColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(400, 500);
            this.pnlMain.TabIndex = 0;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(128, 136);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(135, 31);
            this.guna2HtmlLabel1.TabIndex = 8;
            this.guna2HtmlLabel1.Text = "Đăng nhập";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(218, 455);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(56, 16);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Đăng ký";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 455);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Chưa có tài khoản?";
            // 
            // btnExit
            // 
            this.btnExit.BorderRadius = 10;
            this.btnExit.FillColor = System.Drawing.Color.LightGray;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(60, 400);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(280, 40);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "Thoát";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BorderRadius = 10;
            this.btnLogin.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(135)))), ((int)(((byte)(255)))));
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(60, 340);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(280, 45);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderRadius = 10;
            this.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPassword.DefaultText = "";
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPassword.Location = new System.Drawing.Point(60, 270);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.PlaceholderText = "Mật khẩu";
            this.txtPassword.SelectedText = "";
            this.txtPassword.Size = new System.Drawing.Size(280, 45);
            this.txtPassword.TabIndex = 2;
            // 
            // txtUsername
            // 
            this.txtUsername.BorderRadius = 10;
            this.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsername.DefaultText = "";
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtUsername.Location = new System.Drawing.Point(60, 210);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderText = "Tên đăng nhập";
            this.txtUsername.SelectedText = "";
            this.txtUsername.Size = new System.Drawing.Size(280, 45);
            this.txtUsername.TabIndex = 3;
            // 
            // logoBox
            // 
            this.logoBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.logoBox.BorderRadius = 45;
            this.logoBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(135)))), ((int)(((byte)(255)))));
            this.logoBox.ImageRotate = 0F;
            this.logoBox.Location = new System.Drawing.Point(155, 40);
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new System.Drawing.Size(90, 90);
            this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoBox.TabIndex = 5;
            this.logoBox.TabStop = false;
            // 
            // FormLogin
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
    }
}
