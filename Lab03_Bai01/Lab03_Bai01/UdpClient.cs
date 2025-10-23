using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

// Đảm bảo namespace đúng như yêu cầu
namespace Lab03_Bai01
{
    public partial class UdpClient : Form
    {
        public UdpClient()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Sử dụng khối 'using' để đảm bảo UdpClient được đóng đúng cách
            using (System.Net.Sockets.UdpClient udpClient = new System.Net.Sockets.UdpClient())
            {
                try
                {
                    string ipAddress = txtRemoteIp.Text;
                    int port = int.Parse(txtRemotePort.Text);
                    string message = rtbMessage.Text;

                    if (string.IsNullOrWhiteSpace(message))
                    {
                        MessageBox.Show("Please enter a message.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Chuyển đổi thông điệp sang dạng byte, sử dụng UTF8 để hỗ trợ tiếng Việt
                    byte[] sendBytes = Encoding.UTF8.GetBytes(message);

                    // Gửi dữ liệu đến Server
                    udpClient.Send(sendBytes, sendBytes.Length, ipAddress, port);

                    // Xóa tin nhắn trong RichTextBox sau khi gửi thành công
                    rtbMessage.Clear();
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
        }
    }
}