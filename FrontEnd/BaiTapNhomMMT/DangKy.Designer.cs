namespace BaiTapNhomMMT
{
    partial class DangKy
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2Panel pnlMain;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirm;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2Button btnRegister;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2PictureBox logoBox;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.pnlMain = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.btnRegister = new Guna.UI2.WinForms.Guna2Button();
            this.txtConfirm = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.logoBox = new Guna.UI2.WinForms.Guna2PictureBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BorderRadius = 20;
            this.pnlMain.Controls.Add(this.guna2HtmlLabel1);
            this.pnlMain.Controls.Add(this.btnBack);
            this.pnlMain.Controls.Add(this.btnRegister);
            this.pnlMain.Controls.Add(this.txtConfirm);
            this.pnlMain.Controls.Add(this.txtPassword);
            this.pnlMain.Controls.Add(this.txtUsername);
            this.pnlMain.Controls.Add(this.txtEmail);
            this.pnlMain.Controls.Add(this.logoBox);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.FillColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(420, 550);
            this.pnlMain.TabIndex = 0;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(158, 131);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(103, 31);
            this.guna2HtmlLabel1.TabIndex = 8;
            this.guna2HtmlLabel1.Text = "Đăng ký";
            // 
            // btnBack
            // 
            this.btnBack.BorderRadius = 10;
            this.btnBack.FillColor = System.Drawing.Color.LightGray;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBack.ForeColor = System.Drawing.Color.Black;
            this.btnBack.Location = new System.Drawing.Point(70, 470);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(280, 40);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Quay lại";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.BorderRadius = 10;
            this.btnRegister.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(135)))), ((int)(((byte)(255)))));
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(70, 420);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(280, 45);
            this.btnRegister.TabIndex = 1;
            this.btnRegister.Text = "Đăng ký";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // txtConfirm
            // 
            this.txtConfirm.BorderRadius = 10;
            this.txtConfirm.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtConfirm.DefaultText = "";
            this.txtConfirm.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtConfirm.Location = new System.Drawing.Point(70, 355);
            this.txtConfirm.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.PasswordChar = '•';
            this.txtConfirm.PlaceholderText = "Xác nhận mật khẩu";
            this.txtConfirm.SelectedText = "";
            this.txtConfirm.Size = new System.Drawing.Size(280, 45);
            this.txtConfirm.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderRadius = 10;
            this.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPassword.DefaultText = "";
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPassword.Location = new System.Drawing.Point(70, 300);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.PlaceholderText = "Mật khẩu";
            this.txtPassword.SelectedText = "";
            this.txtPassword.Size = new System.Drawing.Size(280, 45);
            this.txtPassword.TabIndex = 3;
            // 
            // txtUsername
            // 
            this.txtUsername.BorderRadius = 10;
            this.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsername.DefaultText = "";
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtUsername.Location = new System.Drawing.Point(70, 245);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderText = "Tên đăng nhập";
            this.txtUsername.SelectedText = "";
            this.txtUsername.Size = new System.Drawing.Size(280, 45);
            this.txtUsername.TabIndex = 4;
            // 
            // txtEmail
            // 
            this.txtEmail.BorderRadius = 10;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail.Location = new System.Drawing.Point(70, 190);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderText = "Email";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(280, 45);
            this.txtEmail.TabIndex = 5;
            // 
            // logoBox
            // 
            this.logoBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.logoBox.BorderRadius = 45;
            this.logoBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(135)))), ((int)(((byte)(255)))));
            this.logoBox.ImageRotate = 0F;
            this.logoBox.Location = new System.Drawing.Point(165, 35);
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new System.Drawing.Size(90, 90);
            this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoBox.TabIndex = 7;
            this.logoBox.TabStop = false;
            // 
            // DangKy
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(420, 550);
            this.Controls.Add(this.pnlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DangKy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng ký";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
    }
}
