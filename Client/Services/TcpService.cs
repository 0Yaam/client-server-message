using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client.Services
{
    public class TcpService
    {
        private TcpClient _tcp;
        private NetworkStream _ns;

        // Kết nối TCP tới server
        public async Task<bool> ConnectAsync(string host, int port)
        {
            _tcp = new TcpClient();
            await _tcp.ConnectAsync(host, port);
            _ns = _tcp.GetStream();
            return true;
        }

        // Gửi một đối tượng JSON (kết thúc bằng newline)
        public async Task SendAsync(object obj)
        {
            var json = JsonConvert.SerializeObject(obj) + "\n";
            var data = Encoding.UTF8.GetBytes(json);
            await _ns.WriteAsync(data, 0, data.Length);
        }

        // Đọc một dòng JSON từ stream (dừng khi gặp '\n')
        public async Task<string> ReadLineAsync(CancellationToken ct)
        {
            var ms = new MemoryStream();
            var buffer = new byte[1];

            while (!ct.IsCancellationRequested)
            {
                int read = await _ns.ReadAsync(buffer, 0, 1, ct);
                if (read == 0) return null; // Mất kết nối
                if (buffer[0] == (byte)'\n')
                    break;
                ms.WriteByte(buffer[0]);
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        // Đóng kết nối an toàn
        public async Task CloseAsync()
        {
            try { _ns?.Close(); } catch { }
            try { _tcp?.Close(); } catch { }
            await Task.CompletedTask;
        }
    }
}
