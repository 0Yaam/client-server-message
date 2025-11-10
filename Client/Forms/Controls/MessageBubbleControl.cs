using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Client.Forms.Controls
{
    public partial class MessageBubbleControl : UserControl
    {
        private bool _isOutgoing;
        public bool IsOutgoing
        {
            get => _isOutgoing;
            set
            {
                _isOutgoing = value;
                UpdateLayoutBubble();
            }
        }

        private string _messageText = "";
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value ?? "";
                if (lblMessage != null) lblMessage.Text = _messageText;
                UpdateLayoutBubble();
            }
        }

        private Image _imageContent = null;
        public Image ImageContent
        {
            get => _imageContent;
            set
            {
                _imageContent = value;
                if (pbImage != null)
                {
                    pbImage.Image = _imageContent;
                    pbImage.Visible = _imageContent != null;
                }
                // hide text when image present
                if (lblMessage != null)
                    lblMessage.Visible = _imageContent == null;

                UpdateLayoutBubble();
            }
        }

        private DateTime _timestamp = DateTime.Now;
        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                _timestamp = value;
                lblTime.Text = _timestamp.ToString("HH:mm");
                UpdateLayoutBubble();
            }
        }

        public string MessageId { get; set; }
        public string SenderId { get; set; }

        public MessageBubbleControl()
        {
            InitializeComponent();

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(10);
            Margin = new Padding(5);
            BackColor = Color.LightGray;

            // Typography tweaks
            if (lblMessage != null) lblMessage.Font = new Font(lblMessage.Font.FontFamily, 10f);
            if (lblTime != null) { lblTime.Font = new Font(lblTime.Font.FontFamily, 8f); lblTime.ForeColor = Color.DimGray; }

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            ResizeParentHook();

            // ensure pbImage initially hidden if designer doesn't set
            if (pbImage != null) pbImage.Visible = false;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ResizeParentHook();
            UpdateLayoutBubble();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            ResizeParentHook();
            UpdateLayoutBubble();
        }

        private void ResizeParentHook()
        {
            if (Parent != null)
            {
                Parent.Resize -= Parent_Resize;
                Parent.Resize += Parent_Resize;
            }
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            UpdateLayoutBubble();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetRoundedCorners();
        }

        private void SetRoundedCorners()
        {
            int radius = 12;
            using (var path = GetRoundRect(ClientRectangle, radius))
            {
                Region?.Dispose();
                Region = new Region(path);
            }
        }

        private static GraphicsPath GetRoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public void UpdateLayoutBubble()
        {
            // compute max width based on parent, fallback to sensible default
            int maxWidth = 400;
            if (Parent != null)
            {
                maxWidth = Math.Max(120, (int)(Parent.ClientSize.Width * 0.70));
            }

            // set control constraints so AutoSize will wrap & grow vertically
            this.MaximumSize = new Size(maxWidth, 0);
            this.MinimumSize = new Size(100, 0);

            // Reserve space inside bubble for padding and time label.
            int reserved = Padding.Left + Padding.Right + 16;
            int labelMaxWidth = Math.Max(80, this.MaximumSize.Width - reserved);

            if (ImageContent != null && pbImage != null)
            {
                // show image and size it appropriately
                lblMessage.Visible = false;
                pbImage.Visible = true;

                // constrain image width to 60% of maxWidth
                int imgMaxW = (int)(this.MaximumSize.Width * 0.6);
                var img = ImageContent;
                int w = img.Width;
                int h = img.Height;
                if (w > imgMaxW)
                {
                    var ratio = (double)imgMaxW / w;
                    w = imgMaxW;
                    h = (int)(h * ratio);
                }
                pbImage.Size = new Size(w, h);
                pbImage.Margin = new Padding(8);
            }
            else
            {
                // set text label constraints
                if (lblMessage != null)
                {
                    lblMessage.Visible = true;
                    lblMessage.MaximumSize = new Size(labelMaxWidth, 0);
                    lblMessage.AutoSize = true;
                    lblMessage.Margin = new Padding(8, 8, 8, 4);
                }

                if (pbImage != null) pbImage.Visible = false;
            }

            // Layout style: message above, time below. Align time to inner edge.
            if (IsOutgoing)
            {
                BackColor = Color.FromArgb(179, 229, 252);
                if (lblMessage != null) lblMessage.TextAlign = ContentAlignment.MiddleRight;
                if (lblTime != null) lblTime.TextAlign = ContentAlignment.MiddleRight;

                this.Dock = DockStyle.None;
                this.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.Margin = new Padding(50, 5, 10, 5);

                // align time to right inside the table cell
                if (lblTime != null) lblTime.Anchor = AnchorStyles.Right;
                if (lblMessage != null) lblMessage.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            }
            else
            {
                BackColor = Color.FromArgb(240, 240, 240);
                if (lblMessage != null) lblMessage.TextAlign = ContentAlignment.MiddleLeft;
                if (lblTime != null) lblTime.TextAlign = ContentAlignment.MiddleLeft;

                this.Dock = DockStyle.None;
                this.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                this.Margin = new Padding(10, 5, 50, 5);

                if (lblTime != null) lblTime.Anchor = AnchorStyles.Left;
                if (lblMessage != null) lblMessage.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            }

            // subtle padding adjustments for better visual spacing
            if (lblTime != null) lblTime.Margin = new Padding(8, 4, 8, 8);

            // force layout recalculation
            tblLayout.SuspendLayout();
            tblLayout.PerformLayout();
            this.PerformLayout();
            tblLayout.ResumeLayout();

            SetRoundedCorners();
            Invalidate();
        }
    }
}
