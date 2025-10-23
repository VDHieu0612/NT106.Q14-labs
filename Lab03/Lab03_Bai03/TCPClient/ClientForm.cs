using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class ClientForm : Form
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private bool isConnected = false;

        public ClientForm()
        {
            InitializeComponent();

            // Đổi tên hiển thị cho RichTextBox
            richTextBox1.Text = "";

            // Disable các button ban đầu
            btnSend.Enabled = false;
            btnDisconnect.Enabled = false;

            // Gắn sự kiện
            btnConnect.Click += btnConnect_Click;
            btnSend.Click += btnSend_Click;
            btnDisconnect.Click += btnDisconnect_Click;
            this.FormClosing += ClientForm_FormClosing;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng TcpClient
                tcpClient = new TcpClient();

                // Kết nối đến Server (IP mặc định: 127.0.0.1, Port: 8080)
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 8080;
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

                tcpClient.Connect(ipEndPoint);

                // Lấy NetworkStream để gửi/nhận dữ liệu
                stream = tcpClient.GetStream();

                isConnected = true;

                // Cập nhật trạng thái buttons
                btnConnect.Enabled = false;
                btnSend.Enabled = true;
                btnDisconnect.Enabled = true;

                MessageBox.Show("Connected to server successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Please connect to server first!", "Warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("Please enter a message!", "Warning");
                return;
            }

            try
            {
                // Lấy message từ RichTextBox
                string message = richTextBox1.Text;

                // Chuyển sang byte array với UTF8 (hỗ trợ tiếng Việt)
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Gửi dữ liệu đến Server
                stream.Write(data, 0, data.Length);

                MessageBox.Show("Message sent successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa RichTextBox sau khi gửi
                richTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnected = false;
                btnConnect.Enabled = true;
                btnSend.Enabled = false;
                btnDisconnect.Enabled = false;
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectFromServer();
        }

        private void DisconnectFromServer()
        {
            try
            {
                if (isConnected && stream != null)
                {
                    // Gửi tín hiệu ngắt kết nối
                    byte[] data = Encoding.UTF8.GetBytes("quit\n");
                    stream.Write(data, 0, data.Length);

                    // Đóng stream và socket
                    stream.Close();
                    tcpClient.Close();

                    isConnected = false;

                    // Cập nhật trạng thái buttons
                    btnConnect.Enabled = true;
                    btnSend.Enabled = false;
                    btnDisconnect.Enabled = false;

                    MessageBox.Show("Disconnected from server!", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Disconnect error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectFromServer();
        }
    }
}