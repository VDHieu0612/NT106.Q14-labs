using HomNayAnGi.Models; // Nhớ using namespace chứa Models
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
                    // Đọc lỗi và Dịch
                    string jsonError = await response.Content.ReadAsStringAsync();
                    string humanMessage = ParseErrorResponse(jsonError);

                    // Xử lý riêng lỗi 401 (Sai pass) cho thân thiện
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        humanMessage = "Sai tài khoản hoặc mật khẩu!";

                    MessageBox.Show(humanMessage, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối đến máy chủ!\n\nChi tiết: {ex.Message}",
                                "Lỗi kết nối",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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
                    // Đọc lỗi và Dịch
                    string jsonError = await response.Content.ReadAsStringAsync();
                    string humanMessage = ParseErrorResponse(jsonError);

                    MessageBox.Show(humanMessage, "Đăng ký thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Lỗi kết nối: " + ex.Message);

                MessageBox.Show($"Lỗi hệ thống khi đăng ký!\n\nChi tiết: {ex.Message}",
                                "Lỗi ngoại lệ",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        private string ParseErrorResponse(string jsonResult)
        {
            try
            {
                // TRƯỜNG HỢP 1: Lỗi Validation (422) - Như hình bạn gửi
                // Cấu trúc: { "detail": [ { "loc": ["body", "email"], "msg": "..." } ] }
                if (jsonResult.Contains("[") && jsonResult.Contains("\"loc\""))
                {
                    var errorObj = JsonConvert.DeserializeObject<ApiValidationError>(jsonResult);

                    if (errorObj != null && errorObj.detail != null)
                    {
                        string message = "Thông tin nhập không hợp lệ:\n";
                        foreach (var err in errorObj.detail)
                        {
                            // Lấy tên trường bị lỗi (phần tử cuối cùng trong mảng loc)
                            // Ví dụ: ["body", "email"] -> Lấy "email"
                            string fieldName = err.loc != null && err.loc.Count > 0
                                ? err.loc.Last().ToString()
                                : "Trường dữ liệu";

                            message += $"- {fieldName}: {err.msg}\n";
                        }
                        return message;
                    }
                }

                // TRƯỜNG HỢP 2: Lỗi đơn giản (400, 401, 403)
                // Cấu trúc: { "detail": "Sai mật khẩu" }
                var simpleError = JsonConvert.DeserializeObject<SimpleApiError>(jsonResult);
                if (simpleError != null && !string.IsNullOrEmpty(simpleError.detail))
                {
                    return simpleError.detail; // Trả về nguyên văn câu lỗi của server
                }

                // Nếu không dịch được thì trả về gốc
                return jsonResult;
            }
            catch
            {
                return jsonResult; // Trả về JSON gốc nếu code dịch bị lỗi
            }
        }

    }



}