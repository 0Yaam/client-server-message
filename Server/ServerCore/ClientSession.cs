using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                string messageType = (string)obj.type;

                if (messageType == "AUTH")
                {
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
                else if (messageType == "REGISTER")
                {
                    string username = (string)obj.username;
                    string displayName = (string)obj.displayName;
                    string password = (string)obj.password;

                    if (AuthManager.Register(username, displayName, password, out string errorMessage))
                    {
                        await SendAsync(new { type = "REGISTER_OK", message = "Đăng ký thành công" }, ct);
                    }
                    else
                    {
                        await SendAsync(new { type = "REGISTER_FAIL", reason = errorMessage }, ct);
                    }
                    
                    // Đóng kết nối sau khi xử lý đăng ký
                    await CloseAsync();
                }
                else
                {
                    await SendAsync(new { type = "ERROR", reason = "first packet must be AUTH or REGISTER" }, ct);
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

                    case "GROUP_CREATE":
                        {
                            string groupName = (string)cmd.name;
                            var members = ((JArray)cmd.members).ToObject<string[]>();

                            // create server-side group id
                            string groupId = Guid.NewGuid().ToString();

                            // persist group info
                            GroupRegistry.Add(groupId, groupName, members);

                            // notify all members (broadcast GROUP_CREATED)
                            var groupCreatedMsg = new
                            {
                                type = "GROUP_CREATED",
                                groupId = groupId,
                                name = groupName,
                                members = members
                            };

                            foreach (var memberUsername in members)
                            {
                                var target = OnlineRegistry.Get(memberUsername);
                                if (target != null)
                                {
                                    try
                                    {
                                        await target.SendAsync(groupCreatedMsg, ct);
                                    }
                                    catch { /* ignore send errors */ }
                                }
                            }

                            // Optionally confirm to creator with groupId
                            await SendAsync(new { type = "GROUP_CREATE_OK", groupId = groupId, name = groupName, members = members }, ct);
                            break;
                        }

                    case "MSG_TO":
                        {
                            string to = (string)cmd.to;
                            string msg = (string)cmd.message;
                            string from = this.Username;

                            // If `to` is a group id, relay to members
                            if (GroupRegistry.TryGet(to, out var group))
                            {
                                foreach (var member in group.Members)
                                {
                                    if (member == from) continue; // don't send back to sender
                                    var target = OnlineRegistry.Get(member);
                                    if (target != null)
                                    {
                                        try
                                        {
                                            await target.SendAsync(new
                                            {
                                                type = "MSG_RECV",
                                                from = from,
                                                groupId = to,
                                                message = msg,
                                                time = DateTime.UtcNow
                                            }, ct);
                                        }
                                        catch { /* ignore per-target errors */ }
                                    }
                                }

                                // Echo to sender as MSG_SENT
                                await SendAsync(new { type = "MSG_SENT", to = to, message = msg, time = DateTime.UtcNow }, ct);
                            }
                            else
                            {
                                var target = OnlineRegistry.Get(to);

                                if (target != null)
                                {
                                    // send to recipient
                                    await target.SendAsync(new
                                    {
                                        type = "MSG_RECV",
                                        from = this.Username,
                                        message = msg,
                                        time = DateTime.UtcNow
                                    }, ct);

                                    // echo for sender
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
                            }

                            break;
                        }

                    case "MSG_TO_IMAGE":
                        {
                            string to = (string)cmd.to;
                            string b64 = (string)cmd.image;
                            string ext = (string)cmd.ext;
                            string from = this.Username;

                            // If `to` is a group id, relay to members
                            if (GroupRegistry.TryGet(to, out var group))
                            {
                                foreach (var member in group.Members)
                                {
                                    if (member == from) continue; // don't send back to sender
                                    var target = OnlineRegistry.Get(member);
                                    if (target != null)
                                    {
                                        try
                                        {
                                            await target.SendAsync(new
                                            {
                                                type = "MSG_RECV_IMAGE",
                                                from = from,
                                                groupId = to,
                                                image = b64,
                                                ext = ext,
                                                time = DateTime.UtcNow
                                            }, ct);
                                        }
                                        catch { /* ignore per-target errors */ }
                                    }
                                }

                                // Echo to sender as MSG_SENT (optional)
                                await SendAsync(new { type = "MSG_SENT", to = to, message = "[Image]", time = DateTime.UtcNow }, ct);
                            }
                            else
                            {
                                var target = OnlineRegistry.Get(to);

                                if (target != null)
                                {
                                    // send to recipient
                                    await target.SendAsync(new
                                    {
                                        type = "MSG_RECV_IMAGE",
                                        from = this.Username,
                                        image = b64,
                                        ext = ext,
                                        time = DateTime.UtcNow
                                    }, ct);

                                    // echo for sender
                                    await SendAsync(new
                                    {
                                        type = "MSG_SENT",
                                        to = to,
                                        message = "[Image]",
                                        time = DateTime.UtcNow
                                    }, ct);
                                }
                                else
                                {
                                    await SendAsync(new { type = "ERROR", text = "User offline" }, ct);
                                }
                            }

                            break;
                        }

                    case "LIST":
                        {
                            Console.WriteLine($"LIST from {Username}");
                            var all = OnlineRegistry.ListUsernames();
                            var others = all.Where(u => !u.Equals(this.Username, StringComparison.OrdinalIgnoreCase)).ToArray();
                            await SendAsync(new { type = "LIST_OK", users = others }, ct);
                            break;
                        }

                    case "PASS_CHANGE":
                        {
                            // require authenticated session
                            string oldPass = (string)cmd.oldPassword;
                            string newPass = (string)cmd.newPassword;

                            if (AuthManager.ChangePassword(this.Username, oldPass, newPass, out string err))
                            {
                                await SendAsync(new { type = "PASS_CHANGE_OK", message = "Password changed" }, ct);
                            }
                            else
                            {
                                await SendAsync(new { type = "PASS_CHANGE_FAIL", reason = err }, ct);
                            }

                            break;
                        }

                    case "AVATAR_UPLOAD":
                        {
                            string b64 = (string)cmd.image;
                            string ext = (string)cmd.ext;
                            try
                            {
                                var data = Convert.FromBase64String(b64);
                                if (AuthManager.UpdateAvatar(this.Username, data, ext, out string savedPath, out string err))
                                {
                                    // Broadcast avatar update to all online users (send image as base64)
                                    var msg = new
                                    {
                                        type = "AVATAR_UPDATED",
                                        username = this.Username,
                                        image = b64,
                                        ext = ext
                                    };

                                    var allUsers = OnlineRegistry.ListUsernames();
                                    foreach (var u in allUsers)
                                    {
                                        var target = OnlineRegistry.Get(u);
                                        if (target != null)
                                        {
                                            try { await target.SendAsync(msg, ct); } catch { }
                                        }
                                    }

                                    await SendAsync(new { type = "AVATAR_UPLOAD_OK", path = savedPath }, ct);
                                }
                                else
                                {
                                    await SendAsync(new { type = "AVATAR_UPLOAD_FAIL", reason = err }, ct);
                                }
                            }
                            catch (Exception ex)
                            {
                                await SendAsync(new { type = "AVATAR_UPLOAD_FAIL", reason = ex.Message }, ct);
                            }

                            break;
                        }
                    default:
                        await SendAsync(new { type = "ERROR", text = "unknown command" }, ct);
                        break;
                }
            }
        }

        // Public wrapper to send messages to this client session
        public Task SendObjectAsync(object obj)
        {
            try
            {
                return SendAsync(obj, CancellationToken.None);
            }
            catch
            {
                return Task.CompletedTask;
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
