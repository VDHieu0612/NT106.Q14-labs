using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public void SetData(string name, string price, string address, string contributor, string imageUrl)
        {
            lblFoodName.Text = name;
            lblPrice.Text = $"Giá: {price}";
            lblAddress.Text = $"Địa chỉ: {address}";
            lblContributor.Text = $"Đóng góp: {contributor}";

            // Xử lý ảnh an toàn
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    // Dùng LoadAsync để không đơ Form
                    picFood.LoadAsync(imageUrl);
                }
                catch
                {
                    // Nếu lỗi link ảnh thì hiện ảnh lỗi hoặc bỏ qua
                    picFood.Image = null; // Hoặc gán 1 ảnh mặc định từ Resources
                }
            }
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
