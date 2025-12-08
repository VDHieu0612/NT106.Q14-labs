using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab05_Bai04.Model;

namespace Lab05_Bai04
{
    public partial class Lab05_Bai04 : Form
    {
        // Danh sách phim toàn cục để lưu trữ
        private List<Movie> globalMoviesList = new List<Movie>();

        public Lab05_Bai04()
        {
            InitializeComponent();
            Bookbtn.Enabled = false; // Không cho người dùng bấm nút đặt vé khi chưa lấy ds phim
            Bookbtn.Text = "Vui lòng lấy DS";
        }

        private async void btnGet_Click(object sender, EventArgs e)
        {
            // Reset giao diện cũ
            flowLayoutPanel1.Controls.Clear();
            progressBar1.Value = 0;
            globalMoviesList.Clear();

            Bookbtn.Enabled = false;
            btnGet.Enabled = false;

            string url = "https://betacinemas.vn/phim.htm";
            HtmlWeb web = new HtmlWeb();
            // Fake UserAgent để tránh bị chặn
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36";

            try
            {
                var doc = await Task.Run(() => web.Load(url));

                // XPath tìm phim
                var listFilmNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'tab-pane') and contains(@class, 'active')]//div[@class='row']//div[contains(@class, 'col-lg-4')]");
                if (listFilmNodes == null) listFilmNodes = doc.DocumentNode.SelectNodes("//div[@class='film-item']");

                if (listFilmNodes == null)
                {
                    MessageBox.Show("Không tìm thấy phim. Kiểm tra lại web!");
                    btnGet.Enabled = true;
                    return;
                }

                progressBar1.Maximum = listFilmNodes.Count;
                // Do khi chọn phim, không có dữ liệu phòng chiếu cụ thể + giá vé thay đổi theo thời gian
                Random rand = new Random(); // Biến random để tạo giá vé và phòng chiếu

                foreach (var node in listFilmNodes)
                {
                    Movie movie = new Movie();

                    // Scraping dữ liệu thật
                    var titleNode = node.SelectSingleNode(".//h3/a");
                    movie.Title = titleNode?.InnerText.Trim();
                    if (string.IsNullOrEmpty(movie.Title)) continue;

                    var imgNode = node.SelectSingleNode(".//img");
                    movie.ImageUrl = imgNode?.GetAttributeValue("src", "");

                    string href = titleNode.GetAttributeValue("href", "");
                    // Xử lý link chi tiết
                    if (!string.IsNullOrEmpty(href))
                    {
                        if (href.Contains("product-pop-up") || href == "#")
                        {
                            var linkImg = node.SelectSingleNode(".//div[@class='film-item']/a");
                            if (linkImg != null) href = linkImg.GetAttributeValue("href", "");
                        }
                        if (!href.StartsWith("http"))
                        {
                            movie.DetailUrl = "https://betacinemas.vn" + href;
                            movie.DetailUrl = movie.DetailUrl.Replace(".vn//", ".vn/");
                        }
                        else movie.DetailUrl = href;
                    }

                    // Tạo dữ liệu giả cho giá vé và phòng
                    // Random giá từ 45k - 100k
                    movie.Price = rand.Next(9, 21) * 5000;

                    // Random danh sách phòng chiếu
                    int soPhong = rand.Next(1, 4); // 1-3 phòng
                    for (int i = 0; i < soPhong; i++)
                    {
                        int p = rand.Next(1, 6); // Phòng 1-5
                        if (!movie.Rooms.Contains(p)) movie.Rooms.Add(p);
                    }
                    movie.Rooms.Sort();

                    // Thêm vào list toàn cục
                    globalMoviesList.Add(movie);

                    // Hiển thị ra màn hình
                    AddMovieToUI(movie);

                    progressBar1.Value++;
                    await Task.Delay(20); // Delay
                }

                // Lưu JSON
                string json = JsonConvert.SerializeObject(globalMoviesList, Formatting.Indented);
                File.WriteAllText("movies.json", json);

                // Mở khoá nút Đặt vé khi đã lấy được ds phim
                if (globalMoviesList.Count > 0)
                {
                    Bookbtn.Enabled = true;
                    Bookbtn.Text = "Đặt vé"; // Đổi text lại
                    MessageBox.Show("Đã tải xong danh sách phim!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                btnGet.Enabled = true; // Mở lại nút Get
            }
        }

        // Đặt vé
        private void Bookbtn_Click(object sender, EventArgs e)
        {
            if (globalMoviesList.Count == 0)
            {
                MessageBox.Show("Chưa có danh sách phim!");
                return;
            }

            // Gọi Form Đặt Vé và truyền List phim sang
            Datvephim frmBooking = new Datvephim(globalMoviesList);
            frmBooking.Show();
        }

        private void AddMovieToUI(Movie movie)
        {
            // List phim
            Panel pnl = new Panel();
            pnl.Width = flowLayoutPanel1.ClientSize.Width - 30;
            pnl.Height = 110;
            pnl.Margin = new Padding(0, 0, 0, 5);
            pnl.BackColor = Color.White;
            pnl.BorderStyle = BorderStyle.FixedSingle;

            PictureBox pic = new PictureBox();
            pic.Location = new Point(5, 5);
            pic.Size = new Size(80, 100);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.ImageLocation = movie.ImageUrl;
            pic.Cursor = Cursors.Hand;
            pic.Click += (s, e) => { MoChiTietPhim(movie); };

            Label lblTitle = new Label();
            lblTitle.Text = movie.Title;
            lblTitle.Location = new Point(100, 10);
            lblTitle.AutoSize = true;
            lblTitle.ForeColor = Color.Chocolate;
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitle.Cursor = Cursors.Hand;
            lblTitle.Click += (s, e) => { MoChiTietPhim(movie); };

            Label lblInfo = new Label();
            // Hiển thị cả Giá vé random
            lblInfo.Text = $"Giá vé từ: {movie.Price:N0} đ | Phòng: {string.Join(", ", movie.Rooms)}";
            lblInfo.Location = new Point(100, 45);
            lblInfo.Size = new Size(pnl.Width - 110, 50);
            lblInfo.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            pnl.Controls.Add(pic);
            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblInfo);

            flowLayoutPanel1.Controls.Add(pnl);
        }

        private void MoChiTietPhim(Movie movie)
        {
            ChitietPhim frm = new ChitietPhim(movie);
            frm.Show();
        }
    }
}