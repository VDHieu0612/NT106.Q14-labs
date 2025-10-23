using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TCPServer
{
    public partial class ServerForm : Form
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private bool isListening = false;

        public ServerForm()
        {
            InitializeComponent();

            // Xử lý lỗi cross-thread
            CheckForIllegalCrossThreadCalls = false;

            // Disable nút Stop ban đầu
            btnStop.Enabled = false;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (!isListening)
            {
                // Bắt đầu lắng nghe trong thread riêng
                listenThread = new Thread(new ThreadStart(StartListening));
                listenThread.IsBackground = true;
                listenThread.Start();

                btnListen.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        private void StartListening()
        {
            try
            {
                // Tạo TCP Listener tại địa chỉ IP và port 8080
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                tcpListener = new TcpListener(ipAddress, 8080);

                // Bắt đầu lắng nghe
                tcpListener.Start();
                isListening = true;

                AddMessage("Server started!");
                AddMessage("Waiting for connections...");

                while (isListening)
                {
                    // Chấp nhận kết nối từ client
                    TcpClient client = tcpListener.AcceptTcpClient();

                    // Lấy thông tin client
                    IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                    AddMessage($"Connection accepted from {clientEndPoint.Address}:{clientEndPoint.Port}");
                    AddMessage($"From client: This is Lab03");

                    // Tạo thread mới để xử lý client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
            }
            catch (SocketException)
            {
                // Server bị stop
                AddMessage("Server stopped.");
            }
            catch (Exception ex)
            {
                AddMessage("Error: " + ex.Message);
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = null;

            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while (isListening && (bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Nhận dữ liệu từ client (hỗ trợ UTF8)
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Kiểm tra lệnh ngắt kết nối
                    if (message.Trim().ToLower() == "quit")
                    {
                        AddMessage("Client disconnected");
                        break;
                    }

                    // Hiển thị message nhận được
                    AddMessage("Received: " + message);
                }
            }
            catch (Exception ex)
            {
                AddMessage("Client error: " + ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                client.Close();
            }
        }

        private void AddMessage(string message)
        {
            if (rtbListen.InvokeRequired)
            {
                rtbListen.Invoke(new Action(() =>
                {
                    rtbListen.AppendText(message + Environment.NewLine);
                }));
            }
            else
            {
                rtbListen.AppendText(message + Environment.NewLine);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                isListening = false;

                if (tcpListener != null)
                {
                    tcpListener.Stop();
                }

                btnListen.Enabled = true;
                btnStop.Enabled = false;

                AddMessage("Server stopped.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stop error: " + ex.Message, "Error");
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isListening = false;
            if (tcpListener != null)
            {
                tcpListener.Stop();
            }
        }
    }
}