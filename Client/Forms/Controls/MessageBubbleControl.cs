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
                lblMessage.Text = _messageText;
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

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            ResizeParentHook();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
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
            // max width 70
            if (Parent != null && Parent.ClientSize.Width > 0)
            {
                int maxWidth = (int)(Parent.ClientSize.Width * 0.70);
                MaximumSize = new Size(maxWidth, 0);
            }

            if (IsOutgoing)
            {
                BackColor = Color.FromArgb(179, 229, 252); // xanh nhạt
                lblMessage.TextAlign = ContentAlignment.MiddleRight;
                Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }
            else
            {
                BackColor = Color.FromArgb(224, 224, 224); // xám nhạt
                lblMessage.TextAlign = ContentAlignment.MiddleLeft;
                Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
        }
    }
}
