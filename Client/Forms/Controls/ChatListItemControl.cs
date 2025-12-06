using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Client.Forms.Controls
{
    public partial class ChatListItemControl : UserControl
    {
        private const int PreferredItemWidth = 380;

        [Browsable(true)]
        [Category("Data")]
        public string Username { get; set; }

        private string _displayName;
        [Browsable(true)]
        [Category("Data")]
        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value ?? ""; if (lblName != null) lblName.Text = _displayName; }
        }

        private string _lastMessage;
        [Browsable(true)]
        [Category("Data")]
        public string LastMessage
        {
            get => _lastMessage;
            set { _lastMessage = value ?? ""; if (lblLastMessage != null) lblLastMessage.Text = _lastMessage; }
        }

        private DateTime _time = DateTime.Now;
        [Browsable(true)]
        [Category("Data")]
        public DateTime Time
        {
            get => _time;
            set { _time = value; if (lblTime != null) lblTime.Text = FormatTime(_time); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Color HoverColor { get; set; } = Color.FromArgb(245, 247, 250);

        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedColor { get; set; } = Color.FromArgb(220, 235, 255);

        [Browsable(true)]
        [Category("Appearance")]
        public Color BaseColor { get; set; } = Color.FromArgb(250, 251, 253);

        [Browsable(true)]
        [Category("Appearance")]
        public int CornerRadius { get; set; } = 8;

        public bool Selected { get; private set; }
        private bool _isHover = false;

        public event EventHandler ItemClicked;

        public ChatListItemControl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            this.Height = 72;
            this.BackColor = Color.White; 
            if (lblName != null) lblName.Font = new Font(lblName.Font.FontFamily, 10F, FontStyle.Bold);
            if (lblTime != null)
            {
                lblTime.Font = new Font(lblTime.Font.FontFamily, 8.5F, FontStyle.Regular);
                lblTime.ForeColor = Color.DimGray;
            }
            if (lblLastMessage != null)
            {
                lblLastMessage.Font = new Font(lblLastMessage.Font.FontFamily, 9F, FontStyle.Regular);
                lblLastMessage.ForeColor = Color.Gray;
                lblLastMessage.AutoEllipsis = true;
            }

            this.AutoSize = false;

            this.Width = PreferredItemWidth;
            this.MinimumSize = new Size(Math.Min(PreferredItemWidth, 150), 72);
            this.MaximumSize = new Size(PreferredItemWidth, 72);

            AttachHandlersRecursive(this);

            TryFixAvatarSize(48);

            BringChildrenToFront();
        }

        private void BringChildrenToFront()
        {
            try
            {
                if (pbAvatar != null) pbAvatar.BringToFront();
                if (tblMain != null) tblMain.BringToFront();
            }
            catch { }
        }

        private void TryFixAvatarSize(int size)
        {
            try
            {
                if (pbAvatar == null) return;
                pbAvatar.Width = pbAvatar.Height = size;
                pbAvatar.SizeMode = PictureBoxSizeMode.Zoom;
                MakeAvatarCircle();
            }
            catch { }
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            // Width controlled by ChatForm
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            // No-op: width handled by ChatForm
        }

        private void AdjustWidthToParent()
        {
            // Unused: kept for compatibility
            try
            {
                if (Parent == null) return;

                int parentInnerWidth = Parent.ClientSize.Width - Parent.Padding.Left - Parent.Padding.Right;
                if (parentInnerWidth <= 0)
                {
                    parentInnerWidth = PreferredItemWidth;
                }

                int available = Math.Max(150, parentInnerWidth - this.Margin.Left - this.Margin.Right);
                int target = Math.Min(PreferredItemWidth, available);

                this.MaximumSize = new Size(target, 72);
                this.Width = target;
                this.Height = 72;
            }
            catch { }
        }

        private void AttachHandlersRecursive(Control root)
        {
            void ForwardClick(object s, EventArgs e) => ItemClicked?.Invoke(this, EventArgs.Empty);

            void OnEnter(object s, EventArgs e) { _isHover = true; if (!Selected) Invalidate(); }
            void OnLeave(object s, EventArgs e) { _isHover = false; if (!Selected) Invalidate(); }

            this.Click -= ForwardClick;
            this.Click += ForwardClick;
            this.MouseEnter -= OnEnter;
            this.MouseEnter += OnEnter;
            this.MouseLeave -= OnLeave;
            this.MouseLeave += OnLeave;

            foreach (Control c in root.Controls)
            {
                c.Click -= ForwardClick;
                c.Click += ForwardClick;

                c.MouseEnter -= OnEnter;
                c.MouseEnter += OnEnter;

                c.MouseLeave -= OnLeave;
                c.MouseLeave += OnLeave;

                if (c.HasChildren) AttachHandlersRecursive(c);
            }
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
            Invalidate();
        }

        public void Bind(string username, string displayName, string lastMessage, DateTime time)
        {
            Username = username;
            DisplayName = displayName;
            LastMessage = lastMessage;
            Time = time;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            TryFixAvatarSize(Math.Max(40, Math.Min(56, this.Height - 20)));

            try
            {
                if (pbAvatar == null || lblLastMessage == null) return;

                int paddingHorizontal = this.Padding.Left + this.Padding.Right;
                int contentWidth = Math.Max(80, this.ClientSize.Width - pbAvatar.Width - paddingHorizontal - 24);
                lblLastMessage.MaximumSize = new Size(contentWidth, 0);
            }
            catch { }

            // ensure children visible order
            BringChildrenToFront();
        }

        private static string FormatTime(DateTime t)
        {
            var today = DateTime.Today;
            if (t.Date == today) return t.ToString("HH:mm");
            if (t.Date == today.AddDays(-1)) return "Hôm qua";
            if ((today - t.Date).TotalDays < 7) return t.ToString("ddd");
            return t.ToString("dd/MM");
        }

        private void MakeAvatarCircle()
        {
            try
            {
                if (pbAvatar == null) return;
                int size = Math.Min(pbAvatar.Width, pbAvatar.Height);
                if (size <= 0) return;
                pbAvatar.Width = pbAvatar.Height = size;
                using (var gp = new GraphicsPath())
                {
                    gp.AddEllipse(0, 0, size - 1, size - 1);
                    pbAvatar.Region = new Region(gp);
                }
            }
            catch { }
        }

        public void SetAvatar(Image img)
        {
            try
            {
                if (pbAvatar == null) return;

                var old = pbAvatar.Image;
                pbAvatar.Image = img;
                if (old != null && !object.ReferenceEquals(old, img))
                {
                    try { old.Dispose(); } catch { }
                }
            }
            catch { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = this.ClientRectangle;
            rect.Inflate(-2, -2);

            Color fill = (Selected || _isHover) ? HoverColor : Color.White;

            using (var brush = new SolidBrush(fill))
            using (var path = GetRoundRect(rect, CornerRadius))
            {
                g.FillPath(brush, path);
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
    }
}
