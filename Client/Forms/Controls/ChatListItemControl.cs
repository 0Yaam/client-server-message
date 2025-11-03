using System;
using System.ComponentModel;
using System.Drawing;
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
        public Color HoverColor { get; set; } = Color.FromArgb(240, 240, 240);

        [Browsable(true)]
        [Category("Appearance")]
        public Color SelectedColor { get; set; } = Color.FromArgb(220, 235, 255);

        public bool Selected { get; private set; }

        // Event duy nhất
        public event EventHandler ItemClicked;

        public ChatListItemControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            if (this.Height < 56) this.Height = 60;

            // forward click từ child lên cha 1 cách an toàn
            EventHandler forward = (s, e) => ItemClicked?.Invoke(this, EventArgs.Empty);
            this.Click += forward;
            foreach (Control c in this.Controls) c.Click += forward;

            this.MouseEnter += (s, e) => { if (!Selected) this.BackColor = HoverColor; };
            this.MouseLeave += (s, e) => { if (!Selected) this.BackColor = Color.White; };
            foreach (Control c in this.Controls)
            {
                c.MouseEnter += (s, e) => { if (!Selected) this.BackColor = HoverColor; };
                c.MouseLeave += (s, e) => { if (!Selected) this.BackColor = Color.White; };
            }
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
            this.BackColor = selected ? SelectedColor : Color.White;
        }

        public void Bind(string username, string displayName, string lastMessage, DateTime time)
        {
            this.Username = username;
            this.DisplayName = displayName ?? username;
            this.LastMessage = lastMessage ?? "";
            this.Time = time;

            if (lblName != null) lblName.Text = this.DisplayName;
            if (lblLastMessage != null) lblLastMessage.Text = this.LastMessage;
            if (lblTime != null) lblTime.Text = time.ToString("HH:mm");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            MakeAvatarCircle();
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
                int w = pbAvatar.Width, h = pbAvatar.Height;
                if (w <= 0 || h <= 0) return;

                using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    gp.AddEllipse(0, 0, w - 1, h - 1);
                    pbAvatar.Region = new Region(gp);
                }
            }
            catch { /* ignore */ }
        }




    }
}
