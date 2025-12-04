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
        private FoodService service;

        public AddFoodForm()
        {
            InitializeComponent();
            service = new FoodService();
        }

        private async void txtSubmit_Click(object sender, EventArgs e)
        {
            // 1. Validate dữ liệu (Không được để trống)
            if (string.IsNullOrWhiteSpace(txtTenMon.Text) ||
                string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên món và Giá!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validate Giá (Phải là số)
            if (!decimal.TryParse(txtGia.Text, out decimal giaTien))
            {
                MessageBox.Show("Giá tiền phải là số!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Validate Giá (Phải lớn hơn 0)
            if (giaTien <= 0)
            {
                MessageBox.Show("Giá tiền phải lớn hơn 0!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnAdd.Enabled = false;
            btnAdd.Text = "Đang kiểm tra...";

            try
            {
                // 4. Kiểm tra trùng lặp tên món ăn
                bool isDuplicate = await CheckDuplicateFoodAsync(txtTenMon.Text);

                if (isDuplicate)
                {
                    MessageBox.Show(
                        $"Món ăn '{txtTenMon.Text}' đã tồn tại trong hệ thống!\n\n" +
                        "Vui lòng nhập tên món khác.",
                        "Trùng lặp dữ liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    btnAdd.Enabled = true;
                    btnAdd.Text = "Thêm";
                    txtTenMon.Focus();
                    txtTenMon.SelectAll();
                    return;
                }

                btnAdd.Text = "Đang thêm...";

                // 5. Tạo gói dữ liệu
                var newFood = new AddFoodRequest
                {
                    TenMonAn = txtTenMon.Text.Trim(),
                    Gia = giaTien,
                    DiaChi = txtDiaChi.Text.Trim(),
                    HinhAnh = txtHinhAnh.Text.Trim(),
                    MoTa = txtMoTa.Text.Trim()
                };

                // 6. Gọi API để thêm món ăn
                bool result = await service.AddFoodAsync(newFood);

                if (result)
                {
                    MessageBox.Show(
                        "Thêm món ăn thành công!",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

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
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Có lỗi xảy ra:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                btnAdd.Enabled = true;
                btnAdd.Text = "Thêm";
            }
        }

        /// <summary>
        /// Kiểm tra món ăn có bị trùng tên không
        /// Lấy TẤT CẢ món ăn từ API (all pages) để kiểm tra
        /// </summary>
        private async Task<bool> CheckDuplicateFoodAsync(string tenMonAn)
        {
            try
            {
                string searchName = tenMonAn.Trim().ToLower();
                int pageIndex = 1;
                int pageSize = 100; // Lấy 100 món mỗi lần
                bool hasMorePages = true;

                while (hasMorePages)
                {
                    // Gọi API lấy danh sách món ăn theo trang
                    FoodResponse response = await service.GetAllFoodAsync(pageIndex, pageSize);

                    if (response == null || response.Data == null)
                        break;

                    // Kiểm tra trong trang hiện tại
                    foreach (var food in response.Data)
                    {
                        if (food.TenMonAn.Trim().ToLower() == searchName)
                        {
                            return true; // Tìm thấy trùng
                        }
                    }

                    // Kiểm tra còn trang nào không
                    // response.Data.Count < pageSize nghĩa là đã hết dữ liệu
                    if (response.Data.Count < pageSize)
                    {
                        hasMorePages = false;
                    }
                    else
                    {
                        pageIndex++; // Chuyển sang trang tiếp theo
                    }
                }

                return false; // Không tìm thấy trùng
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi kiểm tra trùng lặp:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }

        private void txtClear_Click(object sender, EventArgs e)
        {
            // Xác nhận trước khi xóa
            var confirmResult = MessageBox.Show(
                "Bạn có chắc muốn xóa toàn bộ thông tin đã nhập?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult == DialogResult.Yes)
            {
                txtTenMon.Clear();
                txtGia.Clear();
                txtDiaChi.Clear();
                txtHinhAnh.Clear();
                txtMoTa.Clear();
                txtTenMon.Focus();
            }
        }

        // Tự động trim khoảng trắng khi rời khỏi textbox
        private void txtTenMon_Leave(object sender, EventArgs e)
        {
            txtTenMon.Text = txtTenMon.Text.Trim();
        }

        private void txtDiaChi_Leave(object sender, EventArgs e)
        {
            txtDiaChi.Text = txtDiaChi.Text.Trim();
        }

        private void txtHinhAnh_Leave(object sender, EventArgs e)
        {
            txtHinhAnh.Text = txtHinhAnh.Text.Trim();
        }
    }
}