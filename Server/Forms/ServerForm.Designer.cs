namespace Server
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new Guna.UI2.WinForms.Guna2Button();
            this.listBoxOnline = new System.Windows.Forms.ListBox();
            this.btnStop = new Guna.UI2.WinForms.Guna2Button();
            this.txtSendToAll = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSendToAll = new Guna.UI2.WinForms.Guna2Button();
            this.lvDanhSach = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStart.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStart.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStart.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(376, 11);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(180, 45);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // listBoxOnline
            // 
            this.listBoxOnline.FormattingEnabled = true;
            this.listBoxOnline.Location = new System.Drawing.Point(490, 62);
            this.listBoxOnline.Name = "listBoxOnline";
            this.listBoxOnline.Size = new System.Drawing.Size(262, 355);
            this.listBoxOnline.TabIndex = 1;
            // 
            // btnStop
            // 
            this.btnStop.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStop.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStop.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStop.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(572, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(180, 45);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtSendToAll
            // 
            this.txtSendToAll.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSendToAll.DefaultText = "";
            this.txtSendToAll.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSendToAll.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSendToAll.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSendToAll.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSendToAll.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSendToAll.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSendToAll.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSendToAll.Location = new System.Drawing.Point(63, 358);
            this.txtSendToAll.Name = "txtSendToAll";
            this.txtSendToAll.PlaceholderText = "";
            this.txtSendToAll.SelectedText = "";
            this.txtSendToAll.Size = new System.Drawing.Size(200, 36);
            this.txtSendToAll.TabIndex = 3;
            // 
            // btnSendToAll
            // 
            this.btnSendToAll.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSendToAll.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSendToAll.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSendToAll.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSendToAll.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSendToAll.ForeColor = System.Drawing.Color.White;
            this.btnSendToAll.Location = new System.Drawing.Point(290, 349);
            this.btnSendToAll.Name = "btnSendToAll";
            this.btnSendToAll.Size = new System.Drawing.Size(180, 45);
            this.btnSendToAll.TabIndex = 4;
            this.btnSendToAll.Text = "SendToAll";
            // 
            // lvDanhSach
            // 
            this.lvDanhSach.HideSelection = false;
            this.lvDanhSach.Location = new System.Drawing.Point(29, 23);
            this.lvDanhSach.Name = "lvDanhSach";
            this.lvDanhSach.Size = new System.Drawing.Size(301, 309);
            this.lvDanhSach.TabIndex = 5;
            this.lvDanhSach.UseCompatibleStateImageBehavior = false;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lvDanhSach);
            this.Controls.Add(this.btnSendToAll);
            this.Controls.Add(this.txtSendToAll);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.listBoxOnline);
            this.Controls.Add(this.btnStart);
            this.Name = "ServerForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnStart;
        private System.Windows.Forms.ListBox listBoxOnline;
        private Guna.UI2.WinForms.Guna2Button btnStop;
        private Guna.UI2.WinForms.Guna2TextBox txtSendToAll;
        private Guna.UI2.WinForms.Guna2Button btnSendToAll;
        private System.Windows.Forms.ListView lvDanhSach;
    }
}

