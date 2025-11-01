using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_Bai06
{
    public partial class Server : Form
    {
        TcpListener listener;
        Dictionary<TcpClient, string> clients = new Dictionary<TcpClient, string>();

        public Server()
        {
            InitializeComponent();
        }

        private async void serverListen_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, 2005);
            listener.Start();
            dataBox.AppendText("Server started...\r\n");
            serverListen.Enabled = false;

            try
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[255];
                    int rec = await stream.ReadAsync(buffer, 0, buffer.Length);

                    string clientName = Encoding.UTF8.GetString(buffer, 0, rec);

                    clients.Add(client, clientName);
                    dataBox.AppendText($"Client {clientName} ({client.Client.RemoteEndPoint}) connected.\r\n");

                    await BroadcastMessage($"[Server]: {clientName} da tham gia.", client, false);
                    await BroadcastUserList();

                    _ = ClientListen(client);
                }
            }
            catch (Exception ex)
            {
                dataBox.AppendText($"Server error: {ex.Message}\r\n");
            }
        }

        private async Task ClientListen(TcpClient client)
        {
            string clientName = clients[client];
            NetworkStream stream = client.GetStream();

            try
            {
                while (true)
                {
                    byte[] buffer = new byte[8192];
                    int rec = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (rec <= 0) break;

                    string dataRec = Encoding.UTF8.GetString(buffer, 0, rec);

                    // Xử lý file transfer
                    if (dataRec.StartsWith("[FILE]"))
                    {
                        await HandleFileTransfer(client, dataRec, stream);
                    }
                    // Tin nhắn riêng
                    else if (dataRec.StartsWith("/pm "))
                    {
                        var parts = dataRec.Split(new[] { ' ' }, 3);
                        if (parts.Length == 3)
                        {
                            string targetName = parts[1];
                            string message = parts[2];
                            await SendPrivateMessage(client, clientName, targetName, message);
                        }
                    }
                    // Tin nhắn chung
                    else
                    {
                        string msgBroadcast = clientName + ": " + dataRec;
                        Invoke((MethodInvoker)delegate {
                            dataBox.AppendText(msgBroadcast + "\r\n");
                        });
                        await BroadcastMessage(msgBroadcast, client, false);
                    }
                }
            }
            catch
            {
                Invoke((MethodInvoker)delegate {
                    dataBox.AppendText($"Client {clientName} disconnected!\r\n");
                });
            }
            finally
            {
                clients.Remove(client);
                client.Close();
                await BroadcastUserList();
                await BroadcastMessage($"[Server]: {clientName} da roi phong.", client, false);
            }
        }

        private async Task HandleFileTransfer(TcpClient senderClient, string header, NetworkStream stream)
        {
            try
            {
                // Parse: [FILE]sender|target|fileName|fileSize
                var parts = header.Substring(6).Split('|');
                string sender = parts[0];
                string target = parts[1];
                string fileName = parts[2];
                long fileSize = long.Parse(parts[3]);

                Invoke((MethodInvoker)delegate {
                    dataBox.AppendText($"[File Transfer] {sender} -> {target}: {fileName} ({fileSize} bytes)\r\n");
                });

                // Đọc dữ liệu file
                byte[] fileData = new byte[fileSize];
                int totalRead = 0;
                while (totalRead < fileSize)
                {
                    int read = await stream.ReadAsync(fileData, totalRead, (int)(fileSize - totalRead));
                    if (read <= 0) break;
                    totalRead += read;
                }

                // Gửi file đến đích
                if (target == "All")
                {
                    // Gửi cho tất cả trừ người gửi
                    foreach (var pair in clients.ToList())
                    {
                        if (pair.Key != senderClient)
                        {
                            try
                            {
                                // Gửi header
                                byte[] headerBytes = Encoding.UTF8.GetBytes(header);
                                await pair.Key.GetStream().WriteAsync(headerBytes, 0, headerBytes.Length);

                                // Gửi file data
                                await pair.Key.GetStream().WriteAsync(fileData, 0, fileData.Length);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    // Gửi file riêng
                    var targetPair = clients.FirstOrDefault(c => c.Value == target);
                    if (targetPair.Key != null)
                    {
                        try
                        {
                            // Gửi cho người nhận
                            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
                            await targetPair.Key.GetStream().WriteAsync(headerBytes, 0, headerBytes.Length);
                            await targetPair.Key.GetStream().WriteAsync(fileData, 0, fileData.Length);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate {
                    dataBox.AppendText($"[File Transfer Error]: {ex.Message}\r\n");
                });
            }
        }

        private async Task SendPrivateMessage(TcpClient senderClient, string senderName, string targetName, string message)
        {
            var targetPair = clients.FirstOrDefault(c => c.Value == targetName);
            if (targetPair.Key != null)
            {
                TcpClient targetClient = targetPair.Key;
                string msgForTarget = $"[PM tu {senderName}]: {message}";
                byte[] dataTarget = Encoding.UTF8.GetBytes(msgForTarget);

                await targetClient.GetStream().WriteAsync(dataTarget, 0, dataTarget.Length);

                string msgForSender = $"[PM den {targetName}]: {message}";
                byte[] dataSender = Encoding.UTF8.GetBytes(msgForSender);
                await senderClient.GetStream().WriteAsync(dataSender, 0, dataSender.Length);
            }
            else
            {
                string msgError = $"[Server]: Khong tim thay user '{targetName}'.";
                byte[] dataError = Encoding.UTF8.GetBytes(msgError);
                await senderClient.GetStream().WriteAsync(dataError, 0, dataError.Length);
            }
        }

        private async Task BroadcastUserList()
        {
            string userNames = string.Join(",", clients.Values);
            string message = "[USERLIST]" + userNames;
            await BroadcastMessage(message, null, true);
        }

        private async Task BroadcastMessage(string message, TcpClient excludeClient, bool includeSelf)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            foreach (var pair in clients.ToList())
            {
                if ((pair.Key != excludeClient && !includeSelf) || (includeSelf))
                {
                    try
                    {
                        await pair.Key.GetStream().WriteAsync(buffer, 0, buffer.Length);
                    }
                    catch { }
                }
            }
        }
    }
}