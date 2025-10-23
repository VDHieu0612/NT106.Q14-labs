using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_Bai01
{
    public partial class UdpServer : Form
    {
        private System.Net.Sockets.UdpClient udpServer;
        private Thread listenThread;

        public UdpServer()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                int port = int.Parse(txtPort.Text);
                udpServer = new System.Net.Sockets.UdpClient(port);
                rtbReceivedMessages.AppendText($"Server started, listening on port {port}...\n");

                // Vô hiệu hóa các control để không khởi động server nhiều lần
                btnListen.Enabled = false;
                txtPort.Enabled = false;

                // Bắt đầu một luồng (thread) riêng để lắng nghe dữ liệu,
                // tránh làm giao diện bị treo (blocking)
                listenThread = new Thread(new ThreadStart(ListenForClients));
                listenThread.IsBackground = true; // Luồng sẽ tự động kết thúc khi ứng dụng chính đóng
                listenThread.Start();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid port number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListenForClients()
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (true)
                {
                    // Chờ nhận dữ liệu từ client (đây là một lời gọi blocking)
                    byte[] receivedBytes = udpServer.Receive(ref remoteIpEndPoint);
                    string receivedData = Encoding.UTF8.GetString(receivedBytes); // Dùng UTF8 để hỗ trợ tiếng Việt
                    string messageToShow = $"{remoteIpEndPoint}: {receivedData}\n";

                    // Cập nhật giao diện từ một luồng khác một cách an toàn
                    this.Invoke((MethodInvoker)delegate
                    {
                        rtbReceivedMessages.AppendText(messageToShow);
                    });
                }
            }
            catch (SocketException)
            {
                // Ngoại lệ này sẽ xảy ra khi UdpClient bị đóng bởi luồng chính,
                // báo hiệu rằng luồng lắng nghe nên kết thúc.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Listening error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Đảm bảo đóng UdpClient và luồng khi form đóng lại
        private void UdpServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (udpServer != null)
            {
                udpServer.Close();
            }
            // Không cần phải .Abort() luồng vì việc đóng UdpClient sẽ gây ra SocketException
            // và làm cho vòng lặp trong luồng kết thúc một cách tự nhiên.
        }
    }
}
