using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Lab04
{
    public partial class Lab04_Bai05 : Form
    {
        // Lưu token
        private string accessToken = "";
        private string tokenType = "";

        public Lab04_Bai05()
        {
            InitializeComponent();

            // Set URL mặc định
            URLtb.Text = "https://nt106.uitiot.vn/auth/token";

            // Set PasswordChar cho textbox password
            Passwordtb.PasswordChar = '*';

            // Thêm event handler cho button
            btnLogin.Click += btnLogin_Click;

            // Cho phép Enter để login
            Usernametb.KeyPress += TextBox_KeyPress;
            Passwordtb.KeyPress += TextBox_KeyPress;
        }

        // Xử lý Enter key
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        // Xử lý đăng nhập
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // Kiểm tra URL
            if (string.IsNullOrWhiteSpace(URLtb.Text))
            {
                MessageBox.Show("Vui lòng nhập URL!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                URLtb.Focus();
                return;
            }

            // Kiểm tra Username
            if (string.IsNullOrWhiteSpace(Usernametb.Text))
            {
                MessageBox.Show("Vui lòng nhập Username!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Usernametb.Focus();
                return;
            }

            // Kiểm tra Password
            if (string.IsNullOrWhiteSpace(Passwordtb.Text))
            {
                MessageBox.Show("Vui lòng nhập Password!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Passwordtb.Focus();
                return;
            }

            // Disable button và hiển thị trạng thái
            btnLogin.Enabled = false;
            Detailrtb.Clear();
            Detailrtb.Text = "Đang đăng nhập...\n";
            Detailrtb.AppendText("Vui lòng đợi...");

            try
            {
                await LoginAsync(URLtb.Text.Trim(), Usernametb.Text.Trim(), Passwordtb.Text);
            }
            catch (HttpRequestException ex)
            {
                Detailrtb.Clear();
                Detailrtb.Text = "Lỗi kết nối:\n";
                Detailrtb.AppendText($"{ex.Message}\n\n");
                Detailrtb.AppendText("Vui lòng kiểm tra:\n");
                Detailrtb.AppendText("- Kết nối Internet\n");
                Detailrtb.AppendText("- URL chính xác");

                MessageBox.Show("Không thể kết nối đến server!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Detailrtb.Clear();
                Detailrtb.Text = $"Lỗi: {ex.Message}\n\n";
                Detailrtb.AppendText($"Chi tiết: {ex.GetType().Name}");

                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private async Task LoginAsync(string url, string username, string password)
        {
            using (var client = new HttpClient())
            {
                // Set timeout
                client.Timeout = TimeSpan.FromSeconds(15);

                // Tạo form-data content
                var content = new MultipartFormDataContent
                {
                    { new StringContent(username), "username" },
                    { new StringContent(password), "password" }
                };

                // Gửi POST request
                var response = await client.PostAsync(url, content);

                // Đọc response
                var responseString = await response.Content.ReadAsStringAsync();

                // Parse JSON
                JObject responseObject;
                try
                {
                    responseObject = JObject.Parse(responseString);
                }
                catch (Exception)
                {
                    Detailrtb.Clear();
                    Detailrtb.Text = "Lỗi: Không thể đọc dữ liệu từ server\n\n";
                    Detailrtb.AppendText("Response từ server:\n");
                    Detailrtb.AppendText(responseString);
                    return;
                }

                // Clear RichTextBox
                Detailrtb.Clear();

                // Kiểm tra kết quả
                if (!response.IsSuccessStatusCode)
                {
                    // Đăng nhập thất bại
                    var detail = responseObject["detail"]?.ToString() ?? "Đăng nhập thất bại!";

                    Detailrtb.AppendText($"Đăng nhập thất bại!\n");
                    Detailrtb.AppendText($"Lỗi: {detail}\n\n");
                    Detailrtb.AppendText($"Status Code: {(int)response.StatusCode} ({response.StatusCode})\n");

                    MessageBox.Show(detail, "Đăng nhập thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Đăng nhập thành công
                    tokenType = responseObject["token_type"]?.ToString() ?? "";
                    accessToken = responseObject["access_token"]?.ToString() ?? "";

                    Detailrtb.AppendText($"Token Type: {tokenType}\n\n");
                    Detailrtb.AppendText($"Access Token:\n");
                    Detailrtb.AppendText($"{accessToken}\n\n");

                    Detailrtb.AppendText("Đăng nhập thành công!\n");
                    Detailrtb.AppendText($"Username: {username}\n");
                    Detailrtb.AppendText($"Status Code: {(int)response.StatusCode} ({response.StatusCode})");

                    MessageBox.Show("Đăng nhập thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}