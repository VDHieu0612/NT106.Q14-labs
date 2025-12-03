using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HomNayAnGi.Models; // Nhớ using namespace chứa Models
using System.Collections.Generic;
using System.Windows.Forms;

namespace HomNayAnGi.Services
{
    public class AuthService
    {
        // Thay đường dẫn này bằng đường dẫn gốc API của bạn
        private readonly string _baseUrl = " https://nt106.uitiot.vn";
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        // --- CHỨC NĂNG 1: ĐĂNG NHẬP (Form-UrlEncoded) ---
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            string url = $"{_baseUrl}/auth/token";

            // Quan trọng: API Login yêu cầu x-www-form-urlencoded
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                // new KeyValuePair<string, string>("grant_type", "password") // Thường mặc định là có, nếu lỗi thì bỏ comment dòng này
            };

            var content = new FormUrlEncodedContent(keyValues);

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LoginResponse>(jsonResult);
                }
                else
                {
                    // Đọc lỗi từ server trả về nếu có
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Đăng nhập thất bại: {response.StatusCode}\n{error}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                return null;
            }
        }

        // --- CHỨC NĂNG 2: ĐĂNG KÝ (JSON) ---
        public async Task<bool> RegisterAsync(SignupRequest requestData)
        {
            string url = $"{_baseUrl}/api/v1/user/signup";

            // Quan trọng: API Signup yêu cầu JSON
            string jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Đăng ký thành công
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Đăng ký thất bại: {response.StatusCode}\n{error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                return false;
            }
        }
    }
}