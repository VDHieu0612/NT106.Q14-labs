using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Lab03_Bai05
{
    public partial class ClientForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private const int PORT = 8888;
        private const string IP_ADDRESS = "127.0.0.1";

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            ConnectToServer();
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                RequestAllDishes();
            }
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient();
                client.Connect(IP_ADDRESS, PORT);
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối đến server: {ex.Message}\nCửa sổ này sẽ đóng.", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private string SendRequestAndGetResponse(string request)
        {
            if (client == null || !client.Connected) return "ERROR|Mất kết nối";
            try
            {
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] buffer = new byte[8192 * 2];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
            catch (Exception)
            {
                MessageBox.Show("Mất kết nối với server! Cửa sổ này sẽ đóng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return "ERROR|Mất kết nối";
            }
        }

        private void RequestAllDishes()
        {
            string response = SendRequestAndGetResponse("GET_ALL_DISHES");
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

        private void btnThemMon_Click(object sender, EventArgs e)
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
            string response = SendRequestAndGetResponse(request);

            string[] responseParts = response.Split('|');
            MessageBox.Show(responseParts[1]);

            if (responseParts[0] == "ADD_SUCCESS")
            {
                RequestAllDishes();
            }
        }

        private void btnChonMon_Click(object sender, EventArgs e)
        {
            string response = SendRequestAndGetResponse("GET_RANDOM_DISH");
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
                else
                {
                    picAnhMonAn.Image = null;
                }
            }
            else
            {
                MessageBox.Show(responseParts[1]);
            }
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
    }
}