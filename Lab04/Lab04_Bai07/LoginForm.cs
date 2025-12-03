using HomNayAnGi.Services;
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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập liệu
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!", "Thông báo");
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Đang xử lý...";

            try
            {
                // 2. Gọi API
                AuthService service = new AuthService();
                var response = await service.LoginAsync(txtUsername.Text, txtPassword.Text);

                if (response != null)
                {
                    // 3. Lưu token vào biến toàn cục
                    Program.Token = response.access_token;
                    Program.CurrentUser = response.user;

                    //MessageBox.Show($"Đăng nhập thành công!\nXin chào: {response.user.first_name}", "Success");

                    // 4. Mở Form chính (MainForm)
                    MainForm main = new MainForm();
                    this.Hide(); // Ẩn form login
                    main.ShowDialog(); // Hiện form main
                    this.Close(); // Khi form main đóng thì đóng luôn app
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignupForm signup = new SignupForm();
            signup.ShowDialog(); 
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
