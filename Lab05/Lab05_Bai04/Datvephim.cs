using Lab05_Bai04.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using DotNetEnv;
using System.Threading.Tasks;

namespace Lab05_Bai04
{
    public partial class Datvephim : Form
    {
        // Dữ liệu phim
        private Dictionary<string, (double Price, List<int> Rooms)> movies = new Dictionary<string, (double Price, List<int> Rooms)>();
        private Dictionary<string, string> moviePosters = new Dictionary<string, string>();

        // Dữ liệu vé đã bán
        private Dictionary<string, List<string>> soldTickets = new Dictionary<string, List<string>>();

        // Ghế đang chọn
        private List<Button> selectedSeats = new List<Button>();

        // Đường dẫn file
        private const string TicketFile = "tickets.txt";
        private const string OutputFile = "output5.txt";

        // Constructor mặc định
        public Datvephim()
        {
            InitializeComponent();
        }

        // Constructor 2: Nhận danh sách phim từ Web Scraper
        public Datvephim(List<Movie> moviesFromWeb) : this()
        {
            DotNetEnv.Env.Load();
            // Chuyển đổi từ List<Movie> sang Dictionary để tái sử dụng logic cũ
            foreach (var item in moviesFromWeb)
            {
                // Key là Tên Phim
                if (!movies.ContainsKey(item.Title))
                {
                    movies.Add(item.Title, (item.Price, item.Rooms));
                    moviePosters.Add(item.Title, item.ImageUrl);
                }
            }
        }

        private void Datvephim_Load(object sender, EventArgs e)
        {
            // Đổ tên phim vào ComboBox
            if (movies.Count > 0)
            {
                cbChonPhim.Items.AddRange(movies.Keys.ToArray());
            }
            else
            {
                MessageBox.Show("Không có dữ liệu phim nào!");
            }

            // Gán sự kiện cho các nút ghế
            foreach (Control c in gbChonGhe.Controls)
                if (c is Button b) b.Click += SeatButton_Click;

            // Load vé đã bán
            LoadTicketsFromFile(TicketFile);

            // Ẩn ProgressBar ban đầu
            if (progressBar1 != null)
                progressBar1.Visible = false;
        }

        // Chọn phim và phòng chiếu
        private void cbChonPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPhongChieu.Items.Clear();
            txtThongTin.Clear();
            ResetSeats();
            cbPhongChieu.SelectedIndex = -1; // Reset selection

            if (cbChonPhim.SelectedItem == null) return;

            string movieName = cbChonPhim.Text;
            if (movies.ContainsKey(movieName))
            {
                foreach (int room in movies[movieName].Rooms)
                    cbPhongChieu.Items.Add(room);

                // Nếu chỉ có 1 phòng thì chọn luôn cho tiện
                if (cbPhongChieu.Items.Count > 0)
                    cbPhongChieu.SelectedIndex = 0;
            }
        }

        private void cbPhongChieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetSeats();
            LoadSoldSeatsDisplay(); // Cập nhật trạng thái ghế đỏ (đã bán)
            txtThongTin.Clear();
        }

        // Chọn ghế
        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button seat = sender as Button;
            if (seat == null || !seat.Enabled) return; // Kiểm tra null

            if (seat.BackColor == SystemColors.Control)
            {
                seat.BackColor = Color.SkyBlue;
                selectedSeats.Add(seat);
            }
            else if (seat.BackColor == Color.SkyBlue)
            {
                seat.BackColor = SystemColors.Control;
                selectedSeats.Remove(seat);
            }
        }

        // Thanh toán
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                cbChonPhim.SelectedItem == null ||
                cbPhongChieu.SelectedItem == null ||
                selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng nhập họ tên và chọn ghế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập Email để nhận vé!");
                txtEmail.Focus();
                return;
            }

            string movie = cbChonPhim.Text;
            int room = Convert.ToInt32(cbPhongChieu.Text);

            // Lấy giá vé cơ bản từ Dictionary
            double basePrice = movies[movie].Price;
            double total = 0;

            var sb = new StringBuilder();
            sb.AppendLine($"Họ tên: {txtHoTen.Text}");
            sb.AppendLine($"Phim: {movie}");
            sb.AppendLine($"Phòng chiếu: {room}");
            sb.AppendLine("Ghế:");

            var key = $"{movie}-{room}";
            if (!soldTickets.ContainsKey(key)) soldTickets[key] = new List<string>();

            // Danh sách tạm để ghi file
            var seatsToWrite = new List<(Button seat, double price)>();
            // List dùng để lấy chuỗi tên ghế gửi email
            List<string> seatNames = new List<string>();

            foreach (var seat in selectedSeats)
            {
                double price = CalcPrice(seat.Text, basePrice);
                sb.AppendLine($"- {seat.Text}: {price:N0} đ");
                total += price;

                // Thêm vào danh sách vé đã bán (Memory)
                if (!soldTickets[key].Contains(seat.Text))
                {
                    soldTickets[key].Add(seat.Text);
                    seatsToWrite.Add((seat, price));
                    seatNames.Add(seat.Text);
                }
            }

            // Ghi ra file ticket (Persistent)
            try
            {
                using (StreamWriter sw = File.AppendText(TicketFile))
                {
                    foreach (var item in seatsToWrite)
                    {
                        // Định dạng ghi: Phim|Phòng|Ghế|Giá
                        sw.WriteLine($"{movie}|{room}|{item.seat.Text}|{item.price}");
                    }
                }

                // Cập nhật giao diện ghế
                foreach (var seat in selectedSeats)
                {
                    seat.BackColor = Color.Red;
                    seat.Enabled = false;
                }

                txtThongTin.Text = sb.ToString() + $"\r\n--------------------\r\nTỔNG TIỀN: {total:N0} VNĐ";
                selectedSeats.Clear();

                string urlPoster = "";
                // Lấy link poster từ Dictionary đã tạo ở Bước 1
                if (moviePosters.ContainsKey(movie)) urlPoster = moviePosters[movie];

                // Tạo chuỗi ghế "A1, A2, B5"
                string chuoiGhe = string.Join(", ", seatNames);

                // Gọi hàm gửi mail
                GuiMailXacNhan(txtEmail.Text.Trim(), txtHoTen.Text, movie, chuoiGhe, total, urlPoster);

                MessageBox.Show($"Đặt vé thành công!\nEmail xác nhận đang được gửi đến {txtEmail.Text}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cập nhật thống kê nếu cần (Ghi ra OutputFile)
                WriteStatisticsToFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi ghi file vé: {ex.Message}");
            }
        }

        // Huỷ
        private void btnHuy_Click(object sender, EventArgs e)
        {
            foreach (var s in selectedSeats)
                s.BackColor = SystemColors.Control;

            selectedSeats.Clear();
            txtHoTen.Clear();
            txtThongTin.Clear();

            // Giữ nguyên phim đang chọn cho tiện, chỉ reset ghế
            ResetSeats();
            LoadSoldSeatsDisplay(); // Load lại ghế đỏ
        }

        // Tính giá
        private double CalcPrice(string seat, double basePrice)
        {
            if (seat.Length < 2) return basePrice;
            char row = seat[0];
            int col = int.Parse(seat.Substring(1));

            // Vé vớt (A1, A5, C1, C5): 1/4 giá
            if ((row == 'A' || row == 'C') && (col == 1 || col == 5)) return basePrice * 0.25;

            // Vé VIP (Hàng B, giữa): Nhân đôi giá
            if (row == 'B' && col >= 2 && col <= 4) return basePrice * 2;

            // Vé thường
            return basePrice;
        }

        // Reset giao diện ghế
        private void ResetSeats()
        {
            foreach (Control c in gbChonGhe.Controls)
            {
                if (c is Button b)
                {
                    // Nếu là ghế đang đỏ (đã bán load từ đầu) thì không reset
                    b.Enabled = true;
                    b.BackColor = SystemColors.Control;
                }
            }
            selectedSeats.Clear();
        }

        // Reset toàn bộ dữ liệu
        private void btnResetAll_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn xóa sạch dữ liệu vé bán?",
                                          "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No) return;

            try
            {
                if (File.Exists(TicketFile)) File.WriteAllText(TicketFile, string.Empty);
                if (File.Exists(OutputFile)) File.WriteAllText(OutputFile, string.Empty);

                soldTickets.Clear();
                selectedSeats.Clear();
                txtHoTen.Clear();
                txtThongTin.Clear();
                cbChonPhim.SelectedIndex = -1;
                cbPhongChieu.Items.Clear();

                ResetSeats();
                MessageBox.Show("Đã xóa sạch dữ liệu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa file: " + ex.Message);
            }
        }

        // Xử lý file I/O
        private void LoadSoldSeatsDisplay()
        {
            if (cbChonPhim.SelectedItem == null || cbPhongChieu.SelectedItem == null)
                return;

            string movie = cbChonPhim.Text.Trim();
            string room = cbPhongChieu.Text.Trim();
            string key = $"{movie}-{room}";

            // Lấy danh sách ghế đã bán cho phim+phòng này từ bộ nhớ (đã load từ file lúc đầu + vừa bán thêm)
            List<string> seatsOfRoom = new List<string>();

            if (soldTickets.ContainsKey(key))
            {
                seatsOfRoom.AddRange(soldTickets[key]);
            }

            // Cập nhật lên giao diện
            foreach (Control c in gbChonGhe.Controls)
            {
                if (c is Button b)
                {
                    string seatName = b.Text.Trim();
                    if (seatsOfRoom.Contains(seatName))
                    {
                        b.BackColor = Color.Red;
                        b.Enabled = false; // Khóa ghế đã bán
                    }
                    else if (!selectedSeats.Contains(b))
                    {
                        b.BackColor = SystemColors.Control;
                        b.Enabled = true;
                    }
                }
            }
        }

        private void LoadTicketsFromFile(string file)
        {
            if (!File.Exists(file)) return;

            soldTickets.Clear();

            try
            {
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split('|');
                    if (parts.Length < 3) continue;

                    string m = parts[0].Trim();
                    string r = parts[1].Trim();
                    string s = parts[2].Trim();

                    string key = $"{m}-{r}";
                    if (!soldTickets.ContainsKey(key)) soldTickets[key] = new List<string>();

                    if (!soldTickets[key].Contains(s)) soldTickets[key].Add(s);
                }
            }
            catch { }
        }

        private void WriteStatisticsToFile()
        {
            if (progressBar1 != null) { progressBar1.Value = 0; progressBar1.Visible = true; }

            // Thống kê nhanh
            var statList = new List<(string Movie, int Sold, int Total, double Percent, double Revenue)>();

            int count = 0;
            foreach (var mv in movies)
            {
                int totalSeats = 15 * mv.Value.Rooms.Count;

                // Đếm vé bán trong soldTickets (bộ nhớ)
                int sold = 0;
                double revenue = 0;

                foreach (var key in soldTickets.Keys)
                {
                    if (key.StartsWith(mv.Key + "-"))
                    {
                        sold += soldTickets[key].Count;
                    }
                }

                // Tính doanh thu chính xác từ File (để lấy giá thực tế đã bán)
                if (File.Exists(TicketFile))
                {
                    var lines = File.ReadAllLines(TicketFile);
                    revenue = lines.Where(l => l.StartsWith(mv.Key + "|")).Sum(l =>
                    {
                        var p = l.Split('|');
                        return p.Length >= 4 ? double.Parse(p[3]) : 0;
                    });
                }

                double percent = totalSeats > 0 ? (double)sold / totalSeats * 100 : 0;
                statList.Add((mv.Key, sold, totalSeats, percent, revenue));

                count++;
                if (progressBar1 != null) progressBar1.Value = (int)((double)count / movies.Count * 100);
            }

            // Ghi file
            try
            {
                using (StreamWriter sw = new StreamWriter(OutputFile))
                {
                    sw.WriteLine("BÁO CÁO DOANH THU");
                    sw.WriteLine("-----------------------------------------------------------------------------");
                    sw.WriteLine(String.Format("{0,-30} | {1,10} | {2,20}", "Phim", "Vé Bán", "Doanh Thu"));
                    sw.WriteLine("-----------------------------------------------------------------------------");

                    var ranked = statList.OrderByDescending(x => x.Revenue);
                    foreach (var item in ranked)
                    {
                        sw.WriteLine(String.Format("{0,-30} | {1,10} | {2,20:N0}", item.Movie, item.Sold, item.Revenue));
                    }
                    sw.WriteLine("-----------------------------------------------------------------------------");
                    sw.WriteLine($"Tổng: {ranked.Sum(x => x.Revenue):N0} VNĐ");
                }

                if (progressBar1 != null) { progressBar1.Visible = false; }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi ghi báo cáo: " + ex.Message); }
        }

        private void btnStat_Click(object sender, EventArgs e)
        {
            WriteStatisticsToFile();
            if (File.Exists(OutputFile))
                System.Diagnostics.Process.Start("notepad.exe", OutputFile);
        }
        private void GuiMailXacNhan(string toEmail, string tenKhach, string tenPhim, string soGhe, double tongTien, string posterUrl)
        {
            // Cấu hình thông tin người gửi
            string fromEmail = Environment.GetEnvironmentVariable("MAIL_FROM");
            string appPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

            // Kiểm tra xem có lấy được không
            if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(appPassword))
            {
                MessageBox.Show("Chưa cấu hình file .env hoặc file chưa được copy vào thư mục bin!", "Lỗi Cấu Hình");
                return;
            }
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail, "Beta Cinemas - Ticket System");
                message.To.Add(toEmail);
                message.Subject = $"Xác nhận đặt vé thành công - {tenPhim}";
                message.IsBodyHtml = true;

                // Xử lý ảnh Poster để nhúng vào Email
                string contentID = "PosterImage";

                // Tải ảnh về
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(posterUrl);
                MemoryStream ms = new MemoryStream(imageBytes);

                // Tạo LinkedResource (ảnh nhúng)
                LinkedResource posterResource = new LinkedResource(ms, "image/jpeg");
                posterResource.ContentId = contentID;

                // Tạo nội dung HTML
                // Thiết kế poster làm background
                string htmlBody = $@"
        <html>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 8px rgba(0,0,0,0.1);'>
                
                <!-- PHẦN POSTER NHÚNG VÀO HEADER (HOẶC BACKGROUND) -->
                <!-- Sử dụng cid:{contentID} để gọi ảnh đã nhúng -->
                <div style='width: 100%; text-align: center; background-color: #333;'>
                     <img src='cid:{contentID}' style='width: 100%; max-height: 400px; object-fit: cover;' />
                </div>

                <div style='padding: 20px;'>
                    <h2 style='color: #d32f2f; text-align: center;'>ĐẶT VÉ THÀNH CÔNG!</h2>
                    
                    <p>Xin chào <strong>{tenKhach}</strong>,</p>
                    <p>Cảm ơn bạn đã lựa chọn Beta Cinemas. Dưới đây là thông tin vé của bạn:</p>
                    
                    <table style='width: 100%; border-collapse: collapse; margin-top: 10px;'>
                        <tr style='border-bottom: 1px solid #ddd;'>
                            <td style='padding: 10px;'><strong>Phim:</strong></td>
                            <td style='padding: 10px; color: #d32f2f;'>{tenPhim}</td>
                        </tr>
                        <tr style='border-bottom: 1px solid #ddd;'>
                            <td style='padding: 10px;'><strong>Vị trí ghế:</strong></td>
                            <td style='padding: 10px; font-weight: bold;'>{soGhe}</td>
                        </tr>
                        <tr style='border-bottom: 1px solid #ddd;'>
                            <td style='padding: 10px;'><strong>Tổng tiền:</strong></td>
                            <td style='padding: 10px;'>{tongTien:N0} đ</td>
                        </tr>
                    </table>

                    <!-- SLOGAN / CÂU CHÚC -->
                    <div style='margin-top: 30px; text-align: center; font-style: italic; color: #555;'>
                        <p>""Rạp phim Beta Cinemas - Trải nghiệm điện ảnh đỉnh cao!""</p>
                        <p>Chúc bạn có những giây phút xem phim vui vẻ.</p>
                    </div>
                </div>
                
                <div style='background-color: #333; color: #fff; text-align: center; padding: 10px; font-size: 12px;'>
                    <p>© 2024 Beta Cinemas. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>
        ";

                // Tạo AlternateView cho nội dung HTML và đính kèm resource
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                htmlView.LinkedResources.Add(posterResource);
                message.AlternateViews.Add(htmlView);

                // Cấu hình SMTP để gửi
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); // Gmail port 587
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(fromEmail, appPassword);

                // Gửi Async
                client.SendCompleted += (s, e) => {
                    // Giải phóng tài nguyên sau khi gửi xong
                    message.Dispose();
                    ms.Dispose();
                };
                client.SendAsync(message, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi mail: {ex.Message}\nTuy nhiên vé vẫn đã được đặt thành công!");
            }
        }
    }
}

