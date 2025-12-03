using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab04_Bai04.Model;

namespace Lab04_Bai04
{
    public partial class Lab04_Bai04 : Form
    {
        public Lab04_Bai04()
        {
            InitializeComponent();
        }

        private async void btnGet_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu cũ trên giao diện
            flowLayoutPanel1.Controls.Clear();
            progressBar1.Value = 0;
            string url = "https://betacinemas.vn/phim.htm";

            try
            {
                // Tải HTML về
                HtmlWeb web = new HtmlWeb();
                var doc = await Task.Run(() => web.Load(url));

                // Parse dữ liệu bằng HtmlAgilityPack
                // XPath này dựa trên cấu trúc BetaCinemas (có thể thay đổi theo thời gian thực tế)
                var listFilmNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'tab-pane') and contains(@class, 'active')]//div[@class='row']//div[contains(@class, 'col-lg-4')]");

                // Dự phòng nếu không tìm thấy trong tab active (tìm toàn bộ phim)
                if (listFilmNodes == null)
                {
                    listFilmNodes = doc.DocumentNode.SelectNodes("//div[@class='film-item']");
                }

                if (listFilmNodes == null)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu HTML phù hợp. Website có thể đã đổi cấu trúc.");
                    return;
                }

                List<Movie> movies = new List<Movie>();
                progressBar1.Maximum = listFilmNodes.Count;

                foreach (var node in listFilmNodes)
                {
                    Movie movie = new Movie();

                    // Lấy Tên phim
                    var titleNode = node.SelectSingleNode(".//h3/a");
                    movie.Title = titleNode?.InnerText.Trim();
                    if (string.IsNullOrEmpty(movie.Title)) continue;

                    // Lấy Ảnh
                    var imgNode = node.SelectSingleNode(".//img");
                    movie.ImageUrl = imgNode?.GetAttributeValue("src", "");

                    // Lấy Link chi tiết
                    string href = titleNode.GetAttributeValue("href", "");

                    // Xử lý link: Nếu link là tương đối (không có https) thì nối thêm vào
                    if (!string.IsNullOrEmpty(href))
                    {
                        // Loại bỏ trường hợp nó lấy nhầm link popup
                        if (href.Contains("product-pop-up") || href == "#")
                        {
                            // Nếu tên phim link sai, thử tìm link bao quanh ảnh
                            var linkImg = node.SelectSingleNode(".//div[@class='film-item']/a");
                            if (linkImg != null) href = linkImg.GetAttributeValue("href", "");
                        }

                        if (!href.StartsWith("http"))
                        {
                            // BetaCinemas thường dùng link tương đối
                            movie.DetailUrl = "https://betacinemas.vn" + href;

                            // Xử lý trường hợp bị thừa dấu
                            movie.DetailUrl = movie.DetailUrl.Replace(".vn//", ".vn/");
                        }
                        else
                        {
                            movie.DetailUrl = href;
                        }
                    }

                    movies.Add(movie);

                    // Vẽ phim lên giao diện
                    AddMovieToUI(movie);

                    progressBar1.Value++;
                    await Task.Delay(30); // Delay
                }

                // Lưu file JSON
                SaveMovieToJson(movies);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void AddMovieToUI(Movie movie)
        {
            // Tạo Panel chứa (Một dòng danh sách)
            Panel pnl = new Panel();
            pnl.Width = flowLayoutPanel1.ClientSize.Width - 25;
            pnl.Height = 110; // Chiều cao cố định cho mỗi dòng
            pnl.Margin = new Padding(0, 0, 0, 5); // Cách dòng dưới 5px
            pnl.BackColor = Color.White;
            pnl.BorderStyle = BorderStyle.FixedSingle;

            // Hình ảnh
            PictureBox pic = new PictureBox();
            pic.Location = new Point(5, 5); // Cách lề trái, trên 5px
            pic.Size = new Size(80, 100);   // Kích thước ảnh nhỏ gọn bên trái
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.ImageLocation = movie.ImageUrl;
            pic.Cursor = Cursors.Hand;
            // Click vào ảnh mở chi tiết phim
            pic.Click += (s, e) => { MoChiTietPhim(movie); };

            // Tiêu đề Phim
            Label lblTitle = new Label();
            lblTitle.Text = movie.Title;
            lblTitle.Location = new Point(100, 10);
            lblTitle.AutoSize = true; // Tự giãn kích thước theo độ dài tên
            lblTitle.ForeColor = Color.Chocolate;
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            lblTitle.Cursor = Cursors.Hand;
            lblTitle.Click += (s, e) => { MoChiTietPhim(movie); };

            // Link Chi tiết
            Label lblLink = new Label();
            lblLink.Text = movie.DetailUrl; // Hiển thị URL
            lblLink.Location = new Point(100, 45);
            lblLink.Size = new Size(pnl.Width - 110, 50); // Chiều rộng lấp đầy phần còn lại
            lblLink.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblLink.ForeColor = Color.Black;

            // Xử lý text dài quá thì tự xuống dòng
            lblLink.TextAlign = ContentAlignment.TopLeft;

            // Thêm các control vào Panel dòng
            pnl.Controls.Add(pic);
            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblLink);

            // Thêm Panel dòng vào FlowLayoutPanel chính
            flowLayoutPanel1.Controls.Add(pnl);
        }

        private void MoChiTietPhim(Movie movie)
        {
            // Mở Form ChitietPhim và truyền object movie sang
            ChitietPhim frm = new ChitietPhim(movie);
            frm.Show();
        }

        private void SaveMovieToJson(List<Movie> movies)
        {
            string json = JsonConvert.SerializeObject(movies, Formatting.Indented);
            File.WriteAllText("movies.json", json);
            MessageBox.Show($"Đã lưu {movies.Count} phim vào file movies.json trong thư mục bin/Debug.");
        }
    }
}