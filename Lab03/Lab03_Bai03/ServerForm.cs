using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Lab03_Bai03
{
    public partial class ServerForm : Form
    {
        private TcpListener server;
        private Thread listenThread;
        private bool isRunning = false;

        public ServerForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                // Khởi động thread lắng nghe
                listenThread = new Thread(StartListening);
                listenThread.IsBackground = true;
                listenThread.Start();

                btnListen.Enabled = false;
                btnStop.Enabled = true;
                isRunning = true;
            }
        }

        private void StartListening()
        {
            try
            {
                // Tạo TcpListener lắng nghe trên IP 127.0.0.1 và port 8080
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(ipAddress, 8080);
                server.Start();

                UpdateRichTextBox("Server started!\r\n");
                UpdateRichTextBox($"Listening on {ipAddress}:8080\r\n");

                while (isRunning)
                {
                    // Chờ kết nối từ client
                    TcpClient client = server.AcceptTcpClient();

                    // Xử lý client trong thread riêng
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (SocketException)
            {
                // Server đã dừng
                UpdateRichTextBox("Server stopped.\r\n");
            }
            catch (Exception ex)
            {
                UpdateRichTextBox($"Error: {ex.Message}\r\n");
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                // Lấy thông tin client
                IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                UpdateRichTextBox($"Connection accepted from {clientEndPoint.Address}:{clientEndPoint.Port}\r\n");

                // Đọc dữ liệu từ client
                while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                    UpdateRichTextBox($"From client: {receivedData}\r\n");

                    // Kiểm tra lệnh quit
                    if (receivedData.ToLower() == "quit")
                    {
                        UpdateRichTextBox($"Client {clientEndPoint.Address}:{clientEndPoint.Port} disconnected.\r\n");
                        break;
                    }
                }

                ns.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                UpdateRichTextBox($"Client error: {ex.Message}\r\n");
            }
        }

        private void UpdateRichTextBox(string message)
        {
            if (rtbListen.InvokeRequired)
            {
                rtbListen.Invoke(new Action(() =>
                {
                    rtbListen.AppendText(message);
                    rtbListen.ScrollToCaret();
                }));
            }
            else
            {
                rtbListen.AppendText(message);
                rtbListen.ScrollToCaret();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                isRunning = false;
                server?.Stop();
                listenThread?.Abort();

                UpdateRichTextBox("Server stopped by user.\r\n");

                btnListen.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping server: {ex.Message}", "Error");
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                isRunning = false;
                server?.Stop();
                listenThread?.Abort();
            }
            catch { }
        }
    }
}