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
        public Color HoverColor { get; set; } = Color.FromArgb(245, 245, 245);

        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedColor { get; set; } = Color.FromArgb(220, 235, 255);

        public bool Selected { get; private set; }


        public event EventHandler ItemClicked;

        public ChatListItemControl()
        {
            InitializeComponent();
            DoubleBuffered = true;


            if (Height < 64) Height = 72;
            BackColor = Color.White;


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


            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;


            AttachHandlersRecursive(this);


            MakeAvatarCircle();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);


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


                var flp = Parent as FlowLayoutPanel;
                int parentInnerWidth = Parent.ClientSize.Width - Parent.Padding.Left - Parent.Padding.Right;


                int target = Math.Max(80, parentInnerWidth - this.Margin.Left - this.Margin.Right);


                this.MaximumSize = new Size(target, 0);


                this.Width = target;
            }
            catch
            {

            }
        }

        private void AttachHandlersRecursive(Control root)
        {

            void ForwardClick(object s, EventArgs e) => ItemClicked?.Invoke(this, EventArgs.Empty);


            void OnEnter(object s, EventArgs e) { if (!Selected) BackColor = HoverColor; }
            void OnLeave(object s, EventArgs e) { if (!Selected) BackColor = Color.White; }


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


            try
            {
                if (pbAvatar == null || lblLastMessage == null) return;


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
            catch {  }
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
    }
}
