using Newtonsoft.Json;
using Server.ServerCore;
using Shared.OL;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.ServerCore
{
    public class ClientSession
    {
        private readonly TcpClient _tcp;
        private readonly NetworkStream _ns;

        public string Username { get; private set; } = "";
        public UserRole Role { get; private set; } = UserRole.User;

        public ClientSession(TcpClient tcp)
        {
            _tcp = tcp;
            _ns = _tcp.GetStream();
        }

        public async Task RunAsync(CancellationToken ct)
        {
            try
            {
                string firstLine = await ReadLineAsync(ct);
                if (firstLine == null)
                {
                    await CloseAsync();
                    return;
                }

                dynamic obj = JsonConvert.DeserializeObject(firstLine);
                if ((string)obj.type != "AUTH")
                {
                    await SendAsync(new { type = "AUTH_FAIL", reason = "first packet must be AUTH" }, ct);
                    await CloseAsync();
                    return;
                }

                string user = (string)obj.username;
                string pass = (string)obj.password;

                if (AuthManager.Validate(user, pass, out var acc))
                {
                    Username = acc.Username;
                    Role = acc.Role;

                    OnlineRegistry.Add(this);
                    await SendAsync(new { type = "AUTH_OK", username = Username, role = Role.ToString() }, ct);

                    await CommandLoopAsync(ct);
                }
                else
                {
                    await SendAsync(new { type = "AUTH_FAIL", reason = "invalid" }, ct);
                    await CloseAsync();
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                OnlineRegistry.Remove(Username);
                await CloseAsync();
            }
        }

        private async Task CommandLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                string line = await ReadLineAsync(ct);
                if (line == null) break;

                dynamic cmd = JsonConvert.DeserializeObject(line);
                string type = (string)cmd.type;

                switch (type)
                {
                    case "PING":
                        await SendAsync(new { type = "PONG", time = DateTime.UtcNow }, ct);
                        break;
                    case "MSG_TO":
                        string to = (string)cmd.to;
                        string msg = (string)cmd.message;
                        var target = OnlineRegistry.Get(to);

                        if (target != null)
                        {
                            // gửi cho người nhận
                            await target.SendAsync(new
                            {
                                type = "MSG_RECV",
                                from = this.Username,
                                message = msg,
                                time = DateTime.UtcNow
                            }, ct);

                            // echo forr sender
                            await SendAsync(new
                            {
                                type = "MSG_SENT",
                                to = to,
                                message = msg,
                                time = DateTime.UtcNow
                            }, ct);
                        }
                        else
                        {
                            await SendAsync(new { type = "ERROR", text = "User offline" }, ct);
                        }
                        break;
                    case "LIST":
                        {
                            Console.WriteLine($"LIST from {Username}");
                            var all = OnlineRegistry.ListUsernames();
                            var others = all.Where(u => !u.Equals(this.Username, StringComparison.OrdinalIgnoreCase)).ToArray();
                            await SendAsync(new { type = "LIST_OK", users = others }, ct);
                            break;
                        }

                    default:
                        await SendAsync(new { type = "ERROR", text = "unknown command" }, ct);
                        break;
                }
            }
        }

        private async Task SendAsync(object obj, CancellationToken ct)
        {
            var json = JsonConvert.SerializeObject(obj) + "\n";
            var buf = Encoding.UTF8.GetBytes(json);
            await _ns.WriteAsync(buf, 0, buf.Length, ct);
        }

        private async Task<string> ReadLineAsync(CancellationToken ct)
        {
            var ms = new System.IO.MemoryStream();
            var buffer = new byte[1];

            while (!ct.IsCancellationRequested)
            {
                int read = await _ns.ReadAsync(buffer, 0, 1, ct);
                if (read == 0) return null; 
                if (buffer[0] == (byte)'\n') break;
                ms.WriteByte(buffer[0]);
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }


        private async Task CloseAsync()
        {
            try { _ns.Close(); } catch { }
            try { _tcp.Close(); } catch { }
            await Task.CompletedTask;
        }
    }
}
