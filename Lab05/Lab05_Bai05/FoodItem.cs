using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_Bai07
{
    public partial class FoodItem : UserControl
    {
        // 1. Tạo sự kiện Xóa
        public event EventHandler OnDeleteClicked;
        public FoodItem()
        {
            InitializeComponent();
        }
        private async void LoadImageFromUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                picFood.Image = null; // Hoặc ảnh mặc định
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // BẮT BUỘC phải bật TLS 1.2 ở Program.cs để tải qua HTTPS
                    // Nếu bạn chưa làm, hãy làm bước đó trước!
                    //client.DefaultRequestHeaders.UserAgent.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                    client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Mozilla", "5.0"));
                    // Tải dữ liệu ảnh về dưới dạng byte array
                    byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);

                    // Chuyển byte array thành Image
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        picFood.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi: Hiển thị ảnh lỗi hoặc để trống
                    picFood.Image = null; // Hoặc picFood.Image = Properties.Resources.ImageError;
                    Console.WriteLine($"Lỗi load ảnh {imageUrl}: {ex.Message}");
                }
            }
        }

        public void SetData(string name, string price, string address, string contributor, string imageUrl)
        {
            lblFoodName.Text = name;
            //lblPrice.Text = $"Giá: {price}";
            //lblAddress.Text = $"Địa chỉ: {address}";
            lblContributor.Text = $"Đóng góp: {contributor}";

            // Xử lý ảnh an toàn
            LoadImageFromUrl(imageUrl);
        }

        public void ShowDeleteButton(bool isVisible)
        {
            btnXoaMonAn.Visible = isVisible;
        }

        private void btnXoaMonAn_Click(object sender, EventArgs e)
        {
            // Kích hoạt sự kiện để MainForm xử lý
            OnDeleteClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
