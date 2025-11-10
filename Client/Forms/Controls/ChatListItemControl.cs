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
        // Data
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

        // Appearance
        [Browsable(true)]
        [Category("Appearance")]
        public Color HoverColor { get; set; } = Color.FromArgb(245, 245, 245);

        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedColor { get; set; } = Color.FromArgb(220, 235, 255);

        public bool Selected { get; private set; }

        // Event
        public event EventHandler ItemClicked;

        public ChatListItemControl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            // sensible minimum height
            if (Height < 64) Height = 72;
            BackColor = Color.White;

            // Typography & colors
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

            // keep control height auto but constrain width to parent container
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Forward clicks and hover from all child controls to this control
            AttachHandlersRecursive(this);

            // keep avatar circular on resize
            MakeAvatarCircle();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            // re-hook parent's Resize so we can match width of the user list panel
            if (Parent != null)
            {
                Parent.Resize -= Parent_Resize;
                Parent.Resize += Parent_Resize;
            }

            AdjustWidthToParent();
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            AdjustWidthToParent();
        }

        private void AdjustWidthToParent()
        {
            try
            {
                if (Parent == null) return;

                // If parent is FlowLayoutPanel with top-down flow, make this control fill available width
                var flp = Parent as FlowLayoutPanel;
                int parentInnerWidth = Parent.ClientSize.Width - Parent.Padding.Left - Parent.Padding.Right;

                // subtract this control's margin so it fits neatly
                int target = Math.Max(80, parentInnerWidth - this.Margin.Left - this.Margin.Right);

                // set maximum width so AutoSize will wrap text/height correctly while width is limited
                this.MaximumSize = new Size(target, 0);

                // also set explicit width to reduce initial misalignment in FlowLayoutPanel
                this.Width = target;
            }
            catch
            {
                // ignore layout exceptions
            }
        }

        private void AttachHandlersRecursive(Control root)
        {
            // Forward click from child controls to ItemClicked event
            void ForwardClick(object s, EventArgs e) => ItemClicked?.Invoke(this, EventArgs.Empty);

            // Hover behavior
            void OnEnter(object s, EventArgs e) { if (!Selected) BackColor = HoverColor; }
            void OnLeave(object s, EventArgs e) { if (!Selected) BackColor = Color.White; }

            // Attach handlers for the root control itself
            this.Click -= ForwardClick;
            this.Click += ForwardClick;
            this.MouseEnter -= OnEnter;
            this.MouseEnter += OnEnter;
            this.MouseLeave -= OnLeave;
            this.MouseLeave += OnLeave;

            // Attach recursively for children
            foreach (Control c in root.Controls)
            {
                c.Click -= ForwardClick;
                c.Click += ForwardClick;

                c.MouseEnter -= OnEnter;
                c.MouseEnter += OnEnter;

                c.MouseLeave -= OnLeave;
                c.MouseLeave += OnLeave;

                // recurse
                if (c.HasChildren) AttachHandlersRecursive(c);
            }
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
            BackColor = selected ? SelectedColor : Color.White;
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
            MakeAvatarCircle();

            // Recalculate wrapping width of last message so it doesn't overflow
            try
            {
                if (pbAvatar == null || lblLastMessage == null) return;

                // leave some padding for avatar and cell margins
                int paddingHorizontal = this.Padding.Left + this.Padding.Right;
                int contentWidth = Math.Max(80, this.ClientSize.Width - pbAvatar.Width - paddingHorizontal - 24);
                lblLastMessage.MaximumSize = new Size(contentWidth, 0);
            }
            catch { }
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
            catch { /* ignore */ }
        }

        public void SetAvatar(Image img)
        {
            try
            {
                if (pbAvatar == null) return;
                // Dispose previous image safely
                var old = pbAvatar.Image;
                pbAvatar.Image = img;
                if (old != null && !object.ReferenceEquals(old, img))
                {
                    try { old.Dispose(); } catch { }
                }
            }
            catch { }
        }
    }
}
