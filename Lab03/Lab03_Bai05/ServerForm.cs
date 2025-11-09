using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_Bai05
{
    public partial class ServerForm : Form
    {
        private DatabaseManager dbManager;
        private TcpListener serverListener;
        private readonly List<TcpClient> connectedClients = new List<TcpClient>();

        public ServerForm()
        {
            InitializeComponent();
            this.FormClosing += ServerForm_FormClosing;
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            dbManager = new DatabaseManager();
            dbManager.InitializeDatabase();
            _ = StartServerAsync();
        }

        private void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(LogMessage), message);
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private async Task StartServerAsync()
        {
            serverListener = new TcpListener(IPAddress.Any, 8888);
            try
            {
                serverListener.Start();
                LogMessage("Server đã khởi động tại 127.0.0.1:8888 và đang lắng nghe...");
                while (true)
                {
                    TcpClient connectedClient = await serverListener.AcceptTcpClientAsync();
                    // THÊM CLIENT VÀO DANH SÁCH
                    lock (connectedClients)
                    {
                        connectedClients.Add(connectedClient);
                    }
                    LogMessage($"Một Client đã kết nối từ {connectedClient.Client.RemoteEndPoint}");
                    _ = HandleClientAsync(connectedClient);
                }
            }
            catch (ObjectDisposedException) { LogMessage("Server đã dừng."); }
            catch (Exception ex) { LogMessage($"Lỗi nghiêm trọng của Server: {ex.Message}"); }
        }

        private async Task HandleClientAsync(TcpClient tcpClient)
        {
            string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            try
            {
                using (NetworkStream clientStream = tcpClient.GetStream())
                {
                    while (true)
                    {
                        string request = await ReadMessageAsync(clientStream);
                        if (request == null) break; // Client đã ngắt kết nối

                        string[] requestParts = request.Split('|');
                        string command = requestParts[0];
                        string response = "";
                        switch (command)
                        {
                            case "GET_ALL_DISHES":
                                var allDishes = dbManager.LayDanhSachMonAn();
                                response = "ALL_DISHES|" + string.Join(";", allDishes.Select(m => $"{m.TenMonAn},{m.TenNguoiDongGop}"));
                                break;
                            case "GET_RANDOM_DISH":
                                var randomDish = dbManager.LayMonAnNgauNhien();
                                if (randomDish != null)
                                {
                                    string imageBase64 = GetImageAsBase64(randomDish.HinhAnh);
                                    response = $"RANDOM_DISH|{randomDish.TenMonAn}|{randomDish.TenNguoiDongGop}|{imageBase64}";
                                }
                                else { response = "ERROR|Không có món ăn."; }
                                break;
                            case "ADD_DISH":
                                string tenMon = requestParts[1];
                                string nguoiDongGop = requestParts[2];
                                string hinhAnhBase64 = requestParts[3];
                                string imagePath = SaveImageFromBase64(hinhAnhBase64);
                                try
                                {
                                    dbManager.ThemMonAn(tenMon, imagePath, nguoiDongGop);
                                    response = "ADD_SUCCESS|Thêm món ăn thành công.";
                                }
                                catch (Exception ex) { response = $"ERROR|{ex.Message}"; }
                                break;
                        }
                        await SendMessageAsync(clientStream, response);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Lỗi khi xử lý client {clientEndPoint}: {ex.Message}");
            }
            finally
            {
                LogMessage($"Client {clientEndPoint} đã ngắt kết nối.");
                lock (connectedClients)
                {
                    connectedClients.Remove(tcpClient);
                }
                tcpClient.Close();
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

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e) {
            serverListener?.Stop();
            lock (connectedClients)
            {
                foreach (var client in connectedClients)
                {
                    client.Close(); // Lệnh này sẽ gửi tín hiệu đóng kết nối đến client
                }
                connectedClients.Clear();
            }
            LogMessage("Đã đóng tất cả các kết nối và dừng server.");
        }

        private string GetImageAsBase64(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath) || !File.Exists(relativePath)) return "";
            return Convert.ToBase64String(File.ReadAllBytes(relativePath));
        }

        private string SaveImageFromBase64(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            string imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
            Directory.CreateDirectory(imageDirectory);
            string fileName = $"{Guid.NewGuid()}.jpg";
            string fullPath = Path.Combine(imageDirectory, fileName);
            File.WriteAllBytes(fullPath, imageBytes);
            return fullPath; // Lưu đường dẫn tuyệt đối để GetImageAsBase64 có thể đọc được
        }
    }
}