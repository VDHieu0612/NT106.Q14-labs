using HomNayAnGi.Models;
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
    public partial class RandomResultForm : Form
    {
        public RandomResultForm()
        {
            InitializeComponent();
        }

        // Hàm này để Form chính truyền món ăn trúng thưởng vào
        public void SetFood(Food item)
        {
            // Đặt tiêu đề Form theo tên món cho sinh động
            this.Text = $"✨ Ăn món {item.TenMonAn} đi!!! ✨";

            // Gọi hàm SetData của cái UserControl bên trong
            // Lưu ý: Đảm bảo hàm SetData bên FoodItem đang để public
            foodItemDisplay.SetData(
                item.TenMonAn,
                item.Gia.ToString("N0") + " đ",
                item.DiaChi,
                item.NguoiDongGop,
                item.HinhAnh
            );
        }
    }
}
