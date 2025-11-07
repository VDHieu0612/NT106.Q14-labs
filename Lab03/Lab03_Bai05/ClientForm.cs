using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_Bai05
{
    public partial class ClientForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private const int PORT = 8888;
        private string serverIpAddress;

        public ClientForm(string serverIp)
        {
            InitializeComponent();
            this.serverIpAddress = serverIp;
            this.FormClosing += ClientForm_FormClosing;
        }

        private async void ClientForm_Load(object sender, EventArgs e)
        {
            await ConnectToServerAsync();
            if (client != null && client.Connected)
            {
                await RequestAllDishesAsync();
            }
        }

        private async Task ConnectToServerAsync()
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(this.serverIpAddress, PORT);
                stream = client.GetStream();
                this.Text = $"CLIENT - Đã kết nối tới {serverIpAddress}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối đến server: {ex.Message}", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisableAllControls();
            }
        }

        private async Task<string> SendRequestAndGetResponseAsync(string request)
        {
            if (client == null || !client.Connected)
            {
                DisableAllControls();
                return "ERROR|Mất kết nối";
            }
            try
            {
                await SendMessageAsync(stream, request);
                string response = await ReadMessageAsync(stream);

                if (response == null) throw new IOException("Server đã đóng kết nối.");

                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mất kết nối với server! Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisableAllControls();
                return "ERROR|Mất kết nối";
            }
        }

        #region Message Framing Helpers
        private async Task SendMessageAsync(NetworkStream stream, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);
            await stream.WriteAsync(lengthBytes, 0, 4);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        private async Task<string> ReadMessageAsync(NetworkStream stream)
        {
            byte[] lengthBuffer = new byte[4];
            int bytesRead = await ReadExactlyAsync(stream, lengthBuffer, 4);
            if (bytesRead < 4) return null;

            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] messageBuffer = new byte[messageLength];
            bytesRead = await ReadExactlyAsync(stream, messageBuffer, messageLength);
            if (bytesRead < messageLength) return null;

            return Encoding.UTF8.GetString(messageBuffer);
        }

        private async Task<int> ReadExactlyAsync(NetworkStream stream, byte[] buffer, int bytesToRead)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < bytesToRead)
            {
                int bytesRead = await stream.ReadAsync(buffer, totalBytesRead, bytesToRead - totalBytesRead);
                if (bytesRead == 0) break;
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
        #endregion

        #region Event Handlers (Unchanged Logic, just async calls)
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e) { stream?.Close(); client?.Close(); }

        private void DisableAllControls()
        {
            foreach (Control control in this.Controls) { control.Enabled = false; }
            this.Text += " - (Đã mất kết nối)";
        }

        private async Task RequestAllDishesAsync()
        {
            string response = await SendRequestAndGetResponseAsync("GET_ALL_DISHES");
            if (response.StartsWith("ERROR")) return;
            string[] responseParts = response.Split('|');
            if (responseParts[0] == "ALL_DISHES")
            {
                lsVCacMonAn.Items.Clear();
                if (responseParts.Length > 1 && !string.IsNullOrEmpty(responseParts[1]))
                {
                    string[] dishes = responseParts[1].Split(';');
                    foreach (var dish in dishes)
                    {
                        string[] info = dish.Split(',');
                        var item = new ListViewItem(info[0]);
                        item.SubItems.Add(info[1]);
                        lsVCacMonAn.Items.Add(item);
                    }
                }
            }
        }

        private async void btnThemMon_Click(object sender, EventArgs e)
        {
            string tenMon = IntxtTenMonAn.Text.Trim();
            string nguoiDongGop = IntxtTenNguoiDongGop.Text.Trim();
            string hinhAnhPath = IntxtFileAnh.Tag as string;
            if (string.IsNullOrEmpty(tenMon) || string.IsNullOrEmpty(nguoiDongGop) || string.IsNullOrEmpty(hinhAnhPath))
            {
                MessageBox.Show("Vui lòng điền đủ thông tin và chọn ảnh.", "Thiếu thông tin");
                return;
            }
            string imageBase64 = Convert.ToBase64String(File.ReadAllBytes(hinhAnhPath));
            string request = $"ADD_DISH|{tenMon}|{nguoiDongGop}|{imageBase64}";
            string response = await SendRequestAndGetResponseAsync(request);
            if (response.StartsWith("ERROR")) return;
            string[] responseParts = response.Split('|');
            MessageBox.Show(responseParts[1]);
            if (responseParts[0] == "ADD_SUCCESS")
            {
                await RequestAllDishesAsync();
            }
        }

        private async void btnChonMon_Click(object sender, EventArgs e)
        {
            string response = await SendRequestAndGetResponseAsync("GET_RANDOM_DISH");
            if (response.StartsWith("ERROR")) return;
            string[] responseParts = response.Split('|');
            if (responseParts[0] == "RANDOM_DISH")
            {
                OutxtTenMonAn.Text = responseParts[1];
                OutxtTenNguoiDongGop.Text = responseParts[2];
                string imageBase64 = responseParts[3];
                if (!string.IsNullOrEmpty(imageBase64))
                {
                    byte[] imageBytes = Convert.FromBase64String(imageBase64);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        picAnhMonAn.Image = Image.FromStream(ms);
                    }
                }
                else { picAnhMonAn.Image = null; }
            }
            else { MessageBox.Show(responseParts[1]); }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    IntxtFileAnh.Text = Path.GetFileName(ofd.FileName);
                    IntxtFileAnh.Tag = ofd.FileName;
                }
            }
        }
        #endregion
    }
}