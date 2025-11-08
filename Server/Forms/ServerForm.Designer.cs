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
            this.txtServerSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lvListUser = new System.Windows.Forms.ListView();
            this.txtServerMessage = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnServerSend = new Guna.UI2.WinForms.Guna2Button();
            this.guna2CustomRadioButton1 = new Guna.UI2.WinForms.Guna2CustomRadioButton();
            this.guna2CustomRadioButton2 = new Guna.UI2.WinForms.Guna2CustomRadioButton();
            this.guna2CustomRadioButton3 = new Guna.UI2.WinForms.Guna2CustomRadioButton();
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
            this.btnStart.Location = new System.Drawing.Point(135, 28);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(133, 34);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // listBoxOnline
            // 
            this.listBoxOnline.FormattingEnabled = true;
            this.listBoxOnline.Location = new System.Drawing.Point(502, 83);
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
            this.btnStop.Location = new System.Drawing.Point(322, 28);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(152, 34);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtServerSearch
            // 
            this.txtServerSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtServerSearch.DefaultText = "";
            this.txtServerSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtServerSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtServerSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtServerSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerSearch.Location = new System.Drawing.Point(68, 83);
            this.txtServerSearch.Name = "txtServerSearch";
            this.txtServerSearch.PlaceholderText = "";
            this.txtServerSearch.SelectedText = "";
            this.txtServerSearch.Size = new System.Drawing.Size(200, 36);
            this.txtServerSearch.TabIndex = 3;
            // 
            // lvListUser
            // 
            this.lvListUser.HideSelection = false;
            this.lvListUser.Location = new System.Drawing.Point(68, 125);
            this.lvListUser.Name = "lvListUser";
            this.lvListUser.Size = new System.Drawing.Size(406, 265);
            this.lvListUser.TabIndex = 5;
            this.lvListUser.UseCompatibleStateImageBehavior = false;
            // 
            // txtServerMessage
            // 
            this.txtServerMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtServerMessage.DefaultText = "";
            this.txtServerMessage.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtServerMessage.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtServerMessage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerMessage.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtServerMessage.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtServerMessage.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtServerMessage.Location = new System.Drawing.Point(68, 402);
            this.txtServerMessage.Name = "txtServerMessage";
            this.txtServerMessage.PlaceholderText = "";
            this.txtServerMessage.SelectedText = "";
            this.txtServerMessage.Size = new System.Drawing.Size(200, 36);
            this.txtServerMessage.TabIndex = 6;
            // 
            // btnServerSend
            // 
            this.btnServerSend.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnServerSend.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnServerSend.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnServerSend.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnServerSend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnServerSend.ForeColor = System.Drawing.Color.White;
            this.btnServerSend.Location = new System.Drawing.Point(341, 404);
            this.btnServerSend.Name = "btnServerSend";
            this.btnServerSend.Size = new System.Drawing.Size(133, 34);
            this.btnServerSend.TabIndex = 7;
            this.btnServerSend.Text = "Start";
            // 
            // guna2CustomRadioButton1
            // 
            this.guna2CustomRadioButton1.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CustomRadioButton1.CheckedState.BorderThickness = 0;
            this.guna2CustomRadioButton1.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CustomRadioButton1.CheckedState.InnerColor = System.Drawing.Color.White;
            this.guna2CustomRadioButton1.Location = new System.Drawing.Point(274, 93);
            this.guna2CustomRadioButton1.Name = "guna2CustomRadioButton1";
            this.guna2CustomRadioButton1.Size = new System.Drawing.Size(20, 20);
            this.guna2CustomRadioButton1.TabIndex = 8;
            this.guna2CustomRadioButton1.Text = "guna2CustomRadioButton1";
            this.guna2CustomRadioButton1.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CustomRadioButton1.UncheckedState.BorderThickness = 2;
            this.guna2CustomRadioButton1.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.guna2CustomRadioButton1.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            // 
            // guna2CustomRadioButton2
            // 
            this.guna2CustomRadioButton2.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CustomRadioButton2.CheckedState.BorderThickness = 0;
            this.guna2CustomRadioButton2.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CustomRadioButton2.CheckedState.InnerColor = System.Drawing.Color.White;
            this.guna2CustomRadioButton2.Location = new System.Drawing.Point(418, 93);
            this.guna2CustomRadioButton2.Name = "guna2CustomRadioButton2";
            this.guna2CustomRadioButton2.Size = new System.Drawing.Size(20, 20);
            this.guna2CustomRadioButton2.TabIndex = 9;
            this.guna2CustomRadioButton2.Text = "guna2CustomRadioButton2";
            this.guna2CustomRadioButton2.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CustomRadioButton2.UncheckedState.BorderThickness = 2;
            this.guna2CustomRadioButton2.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.guna2CustomRadioButton2.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            // 
            // guna2CustomRadioButton3
            // 
            this.guna2CustomRadioButton3.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CustomRadioButton3.CheckedState.BorderThickness = 0;
            this.guna2CustomRadioButton3.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CustomRadioButton3.CheckedState.InnerColor = System.Drawing.Color.White;
            this.guna2CustomRadioButton3.Location = new System.Drawing.Point(341, 93);
            this.guna2CustomRadioButton3.Name = "guna2CustomRadioButton3";
            this.guna2CustomRadioButton3.Size = new System.Drawing.Size(20, 20);
            this.guna2CustomRadioButton3.TabIndex = 10;
            this.guna2CustomRadioButton3.Text = "guna2CustomRadioButton3";
            this.guna2CustomRadioButton3.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CustomRadioButton3.UncheckedState.BorderThickness = 2;
            this.guna2CustomRadioButton3.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.guna2CustomRadioButton3.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guna2CustomRadioButton3);
            this.Controls.Add(this.guna2CustomRadioButton2);
            this.Controls.Add(this.guna2CustomRadioButton1);
            this.Controls.Add(this.btnServerSend);
            this.Controls.Add(this.txtServerMessage);
            this.Controls.Add(this.lvListUser);
            this.Controls.Add(this.txtServerSearch);
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
        private Guna.UI2.WinForms.Guna2TextBox txtServerSearch;
        private System.Windows.Forms.ListView lvListUser;
        private Guna.UI2.WinForms.Guna2TextBox txtServerMessage;
        private Guna.UI2.WinForms.Guna2Button btnServerSend;
        private Guna.UI2.WinForms.Guna2CustomRadioButton guna2CustomRadioButton1;
        private Guna.UI2.WinForms.Guna2CustomRadioButton guna2CustomRadioButton2;
        private Guna.UI2.WinForms.Guna2CustomRadioButton guna2CustomRadioButton3;
    }
}

