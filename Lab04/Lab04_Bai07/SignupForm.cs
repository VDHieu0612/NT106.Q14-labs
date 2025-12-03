using HomNayAnGi.Models;
using HomNayAnGi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_Bai07
{
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
            // Mặc định chọn Tiếng Việt và Nam
            cboLanguage.SelectedIndex = 0;
            rdoMale.Checked = true;
        }

        private async void btnSignup_Click(object sender, EventArgs e)
        {
            // --- PHẦN 1: VALIDATE DỮ LIỆU (Kiểm tra kỹ càng) ---

            // 1. Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường bắt buộc (*)", "Cảnh báo");
                return;
            }

            // 2. Kiểm tra định dạng Email (Ví dụ: abc@gmail.com)
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtEmail.Text, emailPattern))
            {
                MessageBox.Show("Email không hợp lệ! Vui lòng kiểm tra lại.", "Lỗi định dạng");
                txtEmail.Focus();
                return;
            }

            // 3. Kiểm tra số điện thoại (Phải là số, độ dài 10 số)
            // Logic: Kiểm tra xem có ký tự nào không phải số không, và độ dài
            long checkPhone;
            if (!long.TryParse(txtPhone.Text, out checkPhone) || txtPhone.Text.Length != 10)
            {
                MessageBox.Show("Số điện thoại phải là 10 chữ số!", "Lỗi định dạng");
                txtPhone.Focus();
                return;
            }

            // 4. Kiểm tra độ dài mật khẩu (Ví dụ: tối thiểu 6 ký tự)
            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Cảnh báo");
                return;
            }


            btnSignup.Enabled = false;  
            btnSignup.Text = "Đang gửi...";

            // 2. Gom dữ liệu vào Model
            var requestData = new SignupRequest
            {
                username = txtUsername.Text,
                password = txtPassword.Text,
                email = txtEmail.Text,
                first_name = txtFirstname.Text,
                last_name = txtLastname.Text,
                phone = txtPhone.Text,
                language = cboLanguage.SelectedItem?.ToString() ?? "vi",

                // Quy ước: Nam = 0, Nữ = 1 (tuỳ server)
                sex = rdoMale.Checked ? 0 : 1,

                // Format ngày bắt buộc: YYYY-MM-DD
                birthday = dtpBirthday.Value.ToString("yyyy-MM-dd")
            };

            // 3. Gọi API
            AuthService service = new AuthService();
            bool isSuccess = await service.RegisterAsync(requestData);

            if (isSuccess)
            {
                MessageBox.Show("Đăng ký thành công! Hãy quay lại đăng nhập.");
                this.Close(); // Đóng form đăng ký
            }

            btnSignup.Enabled = true;
            btnSignup.Text = "Đăng ký";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void StyleControls()
        {
            // 1. Chỉnh màu nền Form
            this.BackColor = System.Drawing.Color.White;
            // this.FormBorderStyle = FormBorderStyle.FixedSingle; // Hoặc None nếu muốn tự làm nút đóng

            // 2. Lặp qua tất cả các control để làm đẹp
            foreach (Control c in this.Controls)
            {
                // Làm đẹp Button
                if (c is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = System.Drawing.Color.FromArgb(255, 87, 34); // Màu Cam Đỏ
                    btn.ForeColor = System.Drawing.Color.White;
                    btn.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                    btn.Cursor = Cursors.Hand;
                    btn.Height = 40; // Nút cao hơn cho dễ bấm
                }

                // Làm đẹp TextBox (Tạo cảm giác rộng rãi)
                if (c is TextBox txt)
                {
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Font = new System.Drawing.Font("Segoe UI", 10);
                    // Muốn padding (cách lề) thì hơi khó với textbox thường, 
                    // mẹo là đặt nó vào trong 1 panel
                }

                // Làm đẹp Label
                if (c is Label lbl)
                {
                    lbl.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    lbl.ForeColor = System.Drawing.Color.DimGray;
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void SignupForm_Load(object sender, EventArgs e)
        {

        }
    }
}
