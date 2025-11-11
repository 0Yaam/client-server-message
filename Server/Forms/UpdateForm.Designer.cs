namespace Server.Forms
{
    partial class UpdateForm
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
            this.lblUserName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblRole = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtBrowse = new System.Windows.Forms.Button();
            this.lblDisplayName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pbAvatar = new Guna.UI2.WinForms.Guna2PictureBox();
            this.txtDisplayName = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtUserName = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtOldPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cbbRole = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.Salt = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnExit = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUserName
            // 
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Location = new System.Drawing.Point(12, 108);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(53, 15);
            this.lblUserName.TabIndex = 11;
            this.lblUserName.Text = "UserName";
            // 
            // lblRole
            // 
            this.lblRole.BackColor = System.Drawing.Color.Transparent;
            this.lblRole.Location = new System.Drawing.Point(12, 39);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(25, 15);
            this.lblRole.TabIndex = 10;
            this.lblRole.Text = "Role";
            // 
            // txtBrowse
            // 
            this.txtBrowse.Location = new System.Drawing.Point(278, 227);
            this.txtBrowse.Name = "txtBrowse";
            this.txtBrowse.Size = new System.Drawing.Size(26, 23);
            this.txtBrowse.TabIndex = 8;
            this.txtBrowse.Text = "...";
            this.txtBrowse.UseVisualStyleBackColor = true;
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplayName.Location = new System.Drawing.Point(12, 82);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(65, 15);
            this.lblDisplayName.TabIndex = 7;
            this.lblDisplayName.Text = "DisplayName";
            // 
            // pbAvatar
            // 
            this.pbAvatar.BorderRadius = 20;
            this.pbAvatar.ImageRotate = 0F;
            this.pbAvatar.Location = new System.Drawing.Point(278, 21);
            this.pbAvatar.Name = "pbAvatar";
            this.pbAvatar.Size = new System.Drawing.Size(200, 200);
            this.pbAvatar.TabIndex = 6;
            this.pbAvatar.TabStop = false;
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.BorderRadius = 5;
            this.txtDisplayName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDisplayName.DefaultText = "";
            this.txtDisplayName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDisplayName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDisplayName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDisplayName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDisplayName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDisplayName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDisplayName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDisplayName.Location = new System.Drawing.Point(88, 70);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.PlaceholderText = "";
            this.txtDisplayName.SelectedText = "";
            this.txtDisplayName.Size = new System.Drawing.Size(168, 27);
            this.txtDisplayName.TabIndex = 13;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderRadius = 5;
            this.txtUserName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUserName.DefaultText = "";
            this.txtUserName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtUserName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtUserName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtUserName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtUserName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtUserName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtUserName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtUserName.Location = new System.Drawing.Point(88, 108);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.PlaceholderText = "";
            this.txtUserName.SelectedText = "";
            this.txtUserName.Size = new System.Drawing.Size(168, 27);
            this.txtUserName.TabIndex = 14;
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.BorderRadius = 5;
            this.txtOldPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOldPassword.DefaultText = "";
            this.txtOldPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtOldPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtOldPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtOldPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtOldPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtOldPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOldPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtOldPassword.Location = new System.Drawing.Point(88, 151);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PlaceholderText = "";
            this.txtOldPassword.SelectedText = "";
            this.txtOldPassword.Size = new System.Drawing.Size(168, 27);
            this.txtOldPassword.TabIndex = 16;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(12, 151);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(49, 15);
            this.guna2HtmlLabel1.TabIndex = 15;
            this.guna2HtmlLabel1.Text = "Password";
            // 
            // cbbRole
            // 
            this.cbbRole.FormattingEnabled = true;
            this.cbbRole.Location = new System.Drawing.Point(88, 33);
            this.cbbRole.Name = "cbbRole";
            this.cbbRole.Size = new System.Drawing.Size(121, 21);
            this.cbbRole.TabIndex = 17;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BorderRadius = 5;
            this.btnUpdate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUpdate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(115, 237);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(94, 30);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "Update";
            // 
            // guna2TextBox1
            // 
            this.guna2TextBox1.BorderRadius = 5;
            this.guna2TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox1.DefaultText = "";
            this.guna2TextBox1.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Location = new System.Drawing.Point(88, 194);
            this.guna2TextBox1.Name = "guna2TextBox1";
            this.guna2TextBox1.PlaceholderText = "";
            this.guna2TextBox1.ReadOnly = true;
            this.guna2TextBox1.SelectedText = "";
            this.guna2TextBox1.Size = new System.Drawing.Size(168, 27);
            this.guna2TextBox1.TabIndex = 22;
            // 
            // Salt
            // 
            this.Salt.BackColor = System.Drawing.Color.Transparent;
            this.Salt.Location = new System.Drawing.Point(12, 194);
            this.Salt.Name = "Salt";
            this.Salt.Size = new System.Drawing.Size(21, 15);
            this.Salt.TabIndex = 21;
            this.Salt.Text = "Salt";
            // 
            // btnExit
            // 
            this.btnExit.BorderRadius = 5;
            this.btnExit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(384, 237);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(94, 30);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "Exit";
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 279);
            this.Controls.Add(this.guna2TextBox1);
            this.Controls.Add(this.Salt);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.cbbRole);
            this.Controls.Add(this.txtOldPassword);
            this.Controls.Add(this.guna2HtmlLabel1);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtBrowse);
            this.Controls.Add(this.lblDisplayName);
            this.Controls.Add(this.pbAvatar);
            this.Name = "UpdateForm";
            this.Text = "UpdateForm";
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel lblUserName;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRole;
        private System.Windows.Forms.Button txtBrowse;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDisplayName;
        private Guna.UI2.WinForms.Guna2PictureBox pbAvatar;
        private Guna.UI2.WinForms.Guna2TextBox txtDisplayName;
        private Guna.UI2.WinForms.Guna2TextBox txtUserName;
        private Guna.UI2.WinForms.Guna2TextBox txtOldPassword;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private System.Windows.Forms.ComboBox cbbRole;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel Salt;
        private Guna.UI2.WinForms.Guna2Button btnExit;
    }
}