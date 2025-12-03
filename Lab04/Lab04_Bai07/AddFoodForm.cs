using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomNayAnGi.Models;
using HomNayAnGi.Services;

namespace Lab04_Bai07
{
    public partial class AddFoodForm : Form
    {
        public AddFoodForm()
        {
            InitializeComponent();
        }

        private async void txtSubmit_Click(object sender, EventArgs e)
        {
            // 1. Validate dữ liệu (Không được để trống)
            if (string.IsNullOrWhiteSpace(txtTenMon.Text) ||
                string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên món và Giá!", "Cảnh báo");
                return;
            }

            // 2. Validate Giá (Phải là số)
            if (!decimal.TryParse(txtGia.Text, out decimal giaTien))
            {
                MessageBox.Show("Giá tiền phải là số!", "Lỗi");
                return;
            }

            btnAdd.Enabled = false;
            btnAdd.Text = "Đang thêm...";

            // 3. Tạo gói dữ liệu
            var newFood = new AddFoodRequest
            {
                TenMonAn = txtTenMon.Text,
                Gia = giaTien,
                DiaChi = txtDiaChi.Text,
                HinhAnh = txtHinhAnh.Text,
                MoTa = txtMoTa.Text
            };

            // 4. Gọi API
            FoodService service = new FoodService();
            bool result = await service.AddFoodAsync(newFood);

            if (result)
            {
                MessageBox.Show("Thêm món ăn thành công!");

                // Đặt kết quả là OK để Form chính biết mà load lại danh sách
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                btnAdd.Enabled = true;
                btnAdd.Text = "Thêm";
            }
        }

        private void txtClear_Click(object sender, EventArgs e)
        {
            txtTenMon.Clear();
            txtGia.Clear();
            txtDiaChi.Clear();
            txtHinhAnh.Clear();
            txtMoTa.Clear();
            txtTenMon.Focus();
        }
    }
}
