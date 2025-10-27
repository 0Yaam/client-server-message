namespace BaiTapNhomMMT
{
    partial class Form1
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
            this.lvTinNhan = new System.Windows.Forms.ListView();
            this.dgvDanhBa = new System.Windows.Forms.DataGridView();
            this.txtTinNhan = new System.Windows.Forms.TextBox();
            this.btnGui = new System.Windows.Forms.Button();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhBa)).BeginInit();
            this.SuspendLayout();
            // 
            // lvTinNhan
            // 
            this.lvTinNhan.HideSelection = false;
            this.lvTinNhan.Location = new System.Drawing.Point(216, 0);
            this.lvTinNhan.Name = "lvTinNhan";
            this.lvTinNhan.Size = new System.Drawing.Size(867, 534);
            this.lvTinNhan.TabIndex = 0;
            this.lvTinNhan.UseCompatibleStateImageBehavior = false;
            
            // 
            // dgvDanhBa
            // 
            this.dgvDanhBa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhBa.Location = new System.Drawing.Point(1, 32);
            this.dgvDanhBa.Name = "dgvDanhBa";
            this.dgvDanhBa.RowHeadersWidth = 51;
            this.dgvDanhBa.RowTemplate.Height = 24;
            this.dgvDanhBa.Size = new System.Drawing.Size(215, 535);
            this.dgvDanhBa.TabIndex = 1;
            // 
            // txtTinNhan
            // 
            this.txtTinNhan.Location = new System.Drawing.Point(216, 535);
            this.txtTinNhan.Multiline = true;
            this.txtTinNhan.Name = "txtTinNhan";
            this.txtTinNhan.Size = new System.Drawing.Size(787, 32);
            this.txtTinNhan.TabIndex = 2;
            // 
            // btnGui
            // 
            this.btnGui.Location = new System.Drawing.Point(1000, 535);
            this.btnGui.Name = "btnGui";
            this.btnGui.Size = new System.Drawing.Size(83, 32);
            this.btnGui.TabIndex = 3;
            this.btnGui.Text = "Gửi";
            this.btnGui.UseVisualStyleBackColor = true;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(1, 0);
            this.txtTimKiem.Multiline = true;
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(215, 32);
            this.txtTimKiem.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 569);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.btnGui);
            this.Controls.Add(this.txtTinNhan);
            this.Controls.Add(this.dgvDanhBa);
            this.Controls.Add(this.lvTinNhan);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhBa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvTinNhan;
        private System.Windows.Forms.DataGridView dgvDanhBa;
        private System.Windows.Forms.TextBox txtTinNhan;
        private System.Windows.Forms.Button btnGui;
        private System.Windows.Forms.TextBox txtTimKiem;
    }
}

