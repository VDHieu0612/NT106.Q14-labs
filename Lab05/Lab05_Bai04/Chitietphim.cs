using Lab05_Bai04.Model;
using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace Lab05_Bai04
{
    public partial class ChitietPhim : Form
    {
        private Movie _movie;

        public ChitietPhim(Movie movie)
        {
            InitializeComponent();
            _movie = movie;

            this.Text = "Đang xem: " + _movie.Title;
            this.Load += ChitietPhim_Load;
        }

        private async void ChitietPhim_Load(object sender, EventArgs e)
        {
            try
            {
                // Khởi tạo WebView2
                await webView21.EnsureCoreWebView2Async();

                if (!string.IsNullOrEmpty(_movie.DetailUrl))
                {
                    webView21.CoreWebView2.Navigate(_movie.DetailUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải trang: " + ex.Message);
            }
        }
    }
}