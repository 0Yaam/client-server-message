using Server.ServerCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Server
{
    public partial class ServerForm : Form
    {
        private ChatServer _server;
        private CancellationTokenSource _cts;
        public ServerForm()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            try
            {
                AuthManager.Init();

                _server = new ChatServer(9000);
                _server.Start();

                _cts = new CancellationTokenSource();
                _ = _server.AcceptLoopAsync(_cts.Token); // không chặn UI

                // ví dụ refresh online list định kỳ
                _ = Task.Run(async () =>
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        var list = OnlineRegistry.ListUsernames();
                        BeginInvoke(new Action(() =>
                        {
                            listBoxOnline.Items.Clear();
                            listBoxOnline.Items.AddRange(list);
                        }));
                        await Task.Delay(1000);
                    }
                });

                MessageBox.Show("Server started on port 9000");
            }
            finally
            {
                btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            _cts?.Cancel();
            _server?.Stop();
            MessageBox.Show("Server stopped");
            btnStart.Enabled = true;
        }
    }
}
