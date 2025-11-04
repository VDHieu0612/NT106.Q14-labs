using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace Lab03_Bai05
{
    public partial class ServerForm : Form
    {
        private DatabaseManager dbManager;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            dbManager = new DatabaseManager();
            dbManager.InitializeDatabase();
            Thread serverThread = new Thread(StartServer);
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(LogMessage), message);
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
        }

        private void StartServer()
        {
            TcpListener serverListener = new TcpListener(IPAddress.Any, 8888);
            try
            {
                serverListener.Start();
                LogMessage("Server đã khởi động tại 127.0.0.1:8888");
                while (true)
                {
                    TcpClient connectedClient = serverListener.AcceptTcpClient();
                    LogMessage("Một Client đã kết nối!");
                    Thread clientThread = new Thread(() => HandleClient(connectedClient));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Lỗi nghiêm trọng của Server: {ex.Message}");
            }
        }

        private void HandleClient(TcpClient tcpClient)
        {
            try
            {
                using (NetworkStream clientStream = tcpClient.GetStream())
                {
                    byte[] buffer = new byte[8192 * 2];
                    int bytesRead;
                    while ((bytesRead = clientStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
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
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        clientStream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
            catch { LogMessage("Một Client đã ngắt kết nối."); }
            finally { tcpClient.Close(); }
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
            return Path.Combine("images", fileName);
        }
    }
}