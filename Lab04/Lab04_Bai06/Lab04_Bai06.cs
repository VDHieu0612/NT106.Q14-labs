using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace Lab04_Bai06
{
    public partial class Lab04_Bai06 : Form
    {
        public Lab04_Bai06()
        {
            InitializeComponent();

            // Gán giá trị mặc định cho URL
            textBox2.Text = "https://nt106.uitiot.vn/api/v1/user/me";

            // Mặc định token type là Bearer
            textBox1.Text = "Bearer";

            // Gán sự kiện click cho nút GetInfo
            this.GetInfobtn.Click += new System.EventHandler(this.GetInfobtn_Click);
        }

        private async void GetInfobtn_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ giao diện
            string url = textBox2.Text.Trim();
            string tokenType = textBox1.Text.Trim();
            string accessToken = richTextBox1.Text.Trim();

            // Kiểm tra điền đầy đủ thông tin chưa
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(tokenType) || string.IsNullOrEmpty(accessToken))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ URL, Token Type và Token!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xử lý gọi API
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Cấu trúc: Authorization: Bearer <token>
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                    // Gửi request GET
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Đọc nội dung trả về
                    string responseString = await response.Content.ReadAsStringAsync();

                    // Kiểm tra nếu gọi thành công
                    MessageBox.Show("Lấy thông tin người dùng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse JSON dữ liệu trả về
                        // Sử dụng JObject để linh hoạt truy xuất dữ liệu
                        JObject userData = JObject.Parse(responseString);

                        // Hiển thị thông tin
                        // Username
                        label11.Text = userData["username"]?.ToString();

                        // Email
                        label12.Text = userData["email"]?.ToString();

                        // First name
                        label13.Text = userData["first_name"]?.ToString();

                        // Last name
                        label14.Text = userData["last_name"]?.ToString();

                        // Sex 
                        string sexValue = userData["sex"]?.ToString();
                        label15.Text = (sexValue == "1") ? "Male" : "Female";

                        // Birthday
                        string rawDob = userData["birthday"]?.ToString();
                        if (DateTime.TryParse(rawDob, out DateTime dob))
                        {
                            label16.Text = dob.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            label16.Text = rawDob;
                        }

                        // Language
                        label17.Text = userData["language"]?.ToString();

                        // Phone
                        label18.Text = userData["phone"]?.ToString();

                        // IsActive
                        label20.Text = userData["is_active"]?.ToString();
                    }
                    else
                    {
                        // Nếu thất bại
                        MessageBox.Show($"Lỗi gọi API: {response.StatusCode}\nChi tiết: {responseString}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chương trình (VD: Sai định dạng URL, rớt mạng...)
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
