using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Lab03_Bai04
{
    public partial class ServerForm : Form
    {
        private TcpListener server;
        private Thread listenThread;
        private bool isRunning = false;
        private Dictionary<string, List<Room>> movieRooms;
        private List<TcpClient> connectedClients;
        private readonly object lockObj = new object();

        public ServerForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            connectedClients = new List<TcpClient>();
            movieRooms = new Dictionary<string, List<Room>>();
        }

        #region Server Lifecycle & Data I/O

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            LoadMoviesFromFile();
            LoadTicketsFromFile();
            if (movieRooms.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu phim! Vui lòng kiểm tra file Movies.txt.", "Lỗi");
                return;
            }
            listenThread = new Thread(StartListening);
            listenThread.IsBackground = true;
            listenThread.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            isRunning = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!isRunning) return;
            try
            {
                isRunning = false;
                server?.Stop();
                lock (lockObj)
                {
                    connectedClients.ForEach(client => client?.Close());
                    connectedClients.Clear();
                }
                SaveTicketsToFile();
                UpdateLog("Server stopped by user.\r\n");
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi khi dừng server: {ex.Message}", "Lỗi"); }
        }

        private void LoadMoviesFromFile()
        {
            try
            {
                string filePath = "D:\\LTM\\Thực hành\\Group06-NT106.Q14-labs\\Lab03\\Lab03_Bai04\\DB\\Movies.txt";
                if (!File.Exists(filePath))
                {
                    UpdateLog($"ERROR: File {filePath} không tồn tại!\r\n");
                    MessageBox.Show($"Không tìm thấy file {filePath}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                movieRooms.Clear();
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('|');
                    if (parts.Length < 3) continue;
                    string movieName = parts[0].Trim();
                    double basePrice = double.Parse(parts[1].Trim());
                    string[] rooms = parts[2].Split(',');
                    movieRooms[movieName] = new List<Room>();
                    foreach (string roomName in rooms)
                    {
                        var room = new Room { RoomName = roomName.Trim(), MovieName = movieName, BasePrice = basePrice, Tickets = new List<Ticket>() };
                        string[] rows = { "A", "B", "C" };
                        for (int i = 0; i < rows.Length; i++)
                        {
                            for (int j = 1; j <= 5; j++)
                            {
                                room.Tickets.Add(new Ticket { SeatNumber = $"{rows[i]}{j}", IsBooked = false, Price = basePrice });
                            }
                        }
                        movieRooms[movieName].Add(room);
                    }
                }
                UpdateLog($"✓ Loaded {movieRooms.Count} movies from file\r\n");
            }
            catch (Exception ex)
            {
                UpdateLog($"ERROR loading movies: {ex.Message}\r\n");
            }
        }

        private void LoadTicketsFromFile()
        {
            try
            {
                string filePath = "D:\\LTM\\Thực hành\\Group06-NT106.Q14-labs\\Lab03\\Lab03_Bai04\\DB\\Tickets.txt";
                if (!File.Exists(filePath))
                {
                    UpdateLog($"INFO: File tickets không tồn tại (sẽ tạo mới khi có đặt vé)\r\n");
                    return;
                }
                string[] lines = File.ReadAllLines(filePath);
                int bookedCount = 0;
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('|');
                    if (parts.Length < 4) continue;
                    string movieName = parts[0].Trim();
                    string roomName = parts[1].Trim();
                    string seatNumber = parts[2].Trim();
                    string customerName = parts[3].Trim();
                    double? savedPrice = parts.Length >= 5 ? double.Parse(parts[4].Trim()) : (double?)null;
                    var room = GetRoom(movieName, roomName);
                    if (room != null)
                    {
                        var ticket = room.Tickets.FirstOrDefault(t => t.SeatNumber == seatNumber);
                        if (ticket != null && !ticket.IsBooked)
                        {
                            ticket.IsBooked = true;
                            ticket.CustomerName = customerName;
                            ticket.BookedTime = DateTime.Now;
                            if (savedPrice.HasValue) { ticket.Price = savedPrice.Value; }
                            bookedCount++;
                        }
                    }
                }
                UpdateLog($"✓ Loaded {bookedCount} booked tickets from file\r\n");
            }
            catch (Exception ex)
            {
                UpdateLog($"ERROR loading tickets: {ex.Message}\r\n");
            }
        }

        private void SaveTicketsToFile()
        {
            try
            {
                string filePath = "D:\\LTM\\Thực hành\\Group06-NT106.Q14-labs\\Lab03\\Lab03_Bai04\\DB\\Tickets.txt";
                List<string> lines = new List<string>();
                lock (lockObj)
                {
                    foreach (var movie in movieRooms)
                    {
                        foreach (var room in movie.Value)
                        {
                            foreach (var ticket in room.Tickets.Where(t => t.IsBooked))
                            {
                                lines.Add($"{movie.Key}|{room.RoomName}|{ticket.SeatNumber}|{ticket.CustomerName}|{ticket.Price}");
                            }
                        }
                    }
                }
                File.WriteAllLines(filePath, lines);
                UpdateLog($"✓ Saved {lines.Count} tickets to file\r\n");
            }
            catch (Exception ex) { UpdateLog($"ERROR saving tickets: {ex.Message}\r\n"); }
        }

        private void btnStat_Click(object sender, EventArgs e)
        {
            try
            {
                lock (lockObj)
                {
                    if (movieRooms.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu!", "Thông báo");
                        return;
                    }
                    string stats = "========== THỐNG KÊ ĐẶT VÉ ==========\n\n";
                    double totalRevenue = 0;
                    foreach (var movie in movieRooms)
                    {
                        stats += $"🎬 {movie.Key}\n";
                        double movieRevenue = 0;
                        int movieBooked = 0, movieTotal = 0;
                        foreach (var room in movie.Value)
                        {
                            int roomBooked = room.Tickets.Count(t => t.IsBooked);
                            int roomTotal = room.Tickets.Count;
                            double roomRevenue = room.Tickets.Where(t => t.IsBooked).Sum(t => t.Price);
                            movieBooked += roomBooked;
                            movieTotal += roomTotal;
                            movieRevenue += roomRevenue;
                            stats += $"   └─ {room.RoomName}: {roomBooked}/{roomTotal} ghế - Doanh thu: {roomRevenue:N0}đ\n";
                        }
                        totalRevenue += movieRevenue;
                        stats += $"   ➤ Tổng phim: {movieBooked}/{movieTotal} ghế - Doanh thu: {movieRevenue:N0}đ\n\n";
                    }
                    stats += $"========================================\n💰 TỔNG DOANH THU: {totalRevenue:N0}đ";
                    MessageBox.Show(stats, "Thống kê đặt vé");
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi xem thống kê: {ex.Message}", "Lỗi"); }
        }
        #endregion

        #region Server Logic
        private void StartListening()
        {
            try
            {
                string localIP = GetLocalIPAddress();
                IPAddress ipAddress = IPAddress.Parse(localIP);
                server = new TcpListener(ipAddress, 8080);
                server.Start();
                UpdateLog($"SERVER STARTED\r\nIP: {localIP}:8080\r\nWaiting for clients...\r\n");
                UpdateServerIP($"Server IP: {localIP}:8080");
                while (isRunning)
                {
                    TcpClient client = server.AcceptTcpClient();
                    lock (lockObj) { connectedClients.Add(client); }
                    UpdateLog($"✓ Client connected: {client.Client.RemoteEndPoint}\r\n");
                    UpdateClientCount();
                    new Thread(() => HandleClient(client)).Start();
                }
            }
            catch (SocketException) { /* Server stopped */ }
            catch (Exception ex) { UpdateLog($"ERROR starting listener: {ex.Message}\r\n"); }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream ns = null;
            var clientEndPoint = client.Client.RemoteEndPoint;
            try
            {
                ns = client.GetStream();
                var formatter = new BinaryFormatter();
                while (client.Connected && isRunning)
                {
                    TicketMessage request = (TicketMessage)formatter.Deserialize(ns);
                    TicketMessage response = ProcessRequest(request);
                    formatter.Serialize(ns, response);
                    if (request.Command == "BOOK")
                    {
                        SaveTicketsToFile();
                    }
                }
            }
            catch (IOException) { /* Client disconnected gracefully */ }
            catch (Exception) { /* Other errors */ }
            finally
            {
                lock (lockObj) { connectedClients.Remove(client); }
                ns?.Close(); client?.Close();
                UpdateClientCount();
                UpdateLog($"Client disconnected: {clientEndPoint}\r\n");
            }
        }

        private TicketMessage ProcessRequest(TicketMessage request)
        {
            var response = new TicketMessage { Command = request.Command, Success = false };
            lock (lockObj)
            {
                switch (request.Command)
                {
                    case "GET_INITIAL_DATA":
                        response.Movies = new List<string>(movieRooms.Keys);
                        response.MovieRoomsData = movieRooms.ToDictionary(k => k.Key, v => v.Value.Select(r => r.RoomName).ToList());
                        response.Success = true;
                        break;
                    case "GET_TICKETS":
                        var room = GetRoom(request.MovieName, request.RoomName);
                        if (room != null)
                        {
                            response.Tickets = new List<Ticket>(room.Tickets);
                            response.Success = true;
                        }
                        break;
                    case "BOOK":
                        var bookRoom = GetRoom(request.MovieName, request.RoomName);
                        var ticket = bookRoom?.Tickets.FirstOrDefault(t => t.SeatNumber == request.SeatNumber);
                        if (ticket != null && !ticket.IsBooked)
                        {
                            ticket.IsBooked = true;
                            ticket.CustomerName = request.CustomerName;
                            ticket.BookedTime = DateTime.Now;
                            response.Success = true;
                        }
                        break;
                }
            }
            return response;
        }
        #endregion

        #region Helpers & UI
        private Room GetRoom(string movieName, string roomName)
        {
            return movieRooms.ContainsKey(movieName) ? movieRooms[movieName].FirstOrDefault(r => r.RoomName == roomName) : null;
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) { return ip.ToString(); }
            }
            return "127.0.0.1";
        }

        private void UpdateLog(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() =>
                {
                    rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}");
                    rtbLog.ScrollToCaret();
                }));
            }
            else
            {
                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}");
                rtbLog.ScrollToCaret();
            }
        }

        private void UpdateServerIP(string text)
        {
            if (lblServerIP.InvokeRequired)
            {
                lblServerIP.Invoke(new Action(() => lblServerIP.Text = text));
            }
            else
            {
                lblServerIP.Text = text;
            }
        }

        private void UpdateClientCount()
        {
            if (lblClientCount.InvokeRequired)
            {
                lblClientCount.Invoke(new Action(() => lblClientCount.Text = $"Connected Clients: {connectedClients.Count}"));
            }
            else
            {
                lblClientCount.Text = $"Connected Clients: {connectedClients.Count}";
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e) => btnStop_Click(null, null);
        #endregion
    }
}