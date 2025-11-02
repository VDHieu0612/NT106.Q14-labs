using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Lab03_Bai03
{
    public partial class ClientForm : Form
    {
        private TcpClient tcpClient;
        private NetworkStream ns;

        public ClientForm()
        {
            InitializeComponent();
            // Vô hiệu hóa các button ban đầu
            btnSend.Enabled = false;
            btnDis.Enabled = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Tạo đối tượng TcpClient
                tcpClient = new TcpClient();

                // 2. Kết nối đến Server với địa chỉ và port xác định
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);
                tcpClient.Connect(ipEndPoint);

                // 3. Lấy luồng để đọc và ghi dữ liệu
                ns = tcpClient.GetStream();

                // 4. Gửi thông điệp đầu tiên đến Server
                Byte[] data = Encoding.ASCII.GetBytes("This is Lab03\n");
                ns.Write(data, 0, data.Length);

                MessageBox.Show("Đã kết nối đến Server thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cập nhật trạng thái các button
                btnConnect.Enabled = false;
                btnSend.Enabled = true;
                btnDis.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra kết nối
                if (tcpClient != null && tcpClient.Connected && ns != null)
                {
                    // Lấy nội dung từ rtbContent
                    string message = rtbContent.Text.Trim();

                    if (string.IsNullOrEmpty(message))
                    {
                        MessageBox.Show("Vui lòng nhập nội dung cần gửi!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Chuyển đổi thành byte array và gửi đến server
                    Byte[] data = Encoding.ASCII.GetBytes(message + "\n");
                    ns.Write(data, 0, data.Length);

                    MessageBox.Show("Đã gửi tin nhắn thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa nội dung sau khi gửi
                    rtbContent.Clear();
                    rtbContent.Focus();
                }
                else
                {
                    MessageBox.Show("Chưa kết nối đến Server!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDis_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    // 5. Gửi lệnh "quit" để thông báo ngắt kết nối
                    Byte[] data = Encoding.ASCII.GetBytes("quit\n");
                    ns.Write(data, 0, data.Length);

                    // Đóng luồng và socket
                    ns.Close();
                    tcpClient.Close();

                    MessageBox.Show("Đã ngắt kết nối khỏi Server!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật trạng thái các button
                    btnConnect.Enabled = true;
                    btnSend.Enabled = false;
                    btnDis.Enabled = false;

                    // Xóa nội dung
                    rtbContent.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi ngắt kết nối: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Đóng kết nối khi đóng form
                if (tcpClient != null && tcpClient.Connected)
                {
                    Byte[] data = Encoding.ASCII.GetBytes("quit\n");
                    ns?.Write(data, 0, data.Length);
                }
                ns?.Close();
                tcpClient?.Close();
            }
            catch { }
        }
    }
}