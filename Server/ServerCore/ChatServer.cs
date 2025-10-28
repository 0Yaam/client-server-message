using Server.ServerCore;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server.ServerCore
{
    public class ChatServer
    {
        private readonly TcpListener _listener;

        public ChatServer(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            _listener.Start();
        }

        public async Task AcceptLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var tcp = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(async () =>
                {
                    var session = new ClientSession(tcp);
                    await session.RunAsync(ct);
                }, ct);
            }
        }

        public void Stop()
        {
            try { _listener.Stop(); } catch { }
        }
    }
}
