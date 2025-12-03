using HomNayAnGi.Models;
using Lab04_Bai07;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms; // Để dùng MessageBox

namespace HomNayAnGi.Services
{
    public class FoodService
    {
        private readonly string _baseUrl = "https://nt106.uitiot.vn"; 
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public FoodService()
        {
            _httpClient = new HttpClient();

            // QUAN TRỌNG: Gắn Token vào Header nếu đã đăng nhập
            if (!string.IsNullOrEmpty(Program.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Program.Token);
            }

            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<FoodResponse> GetAllFoodAsync(int pageIndex, int pageSize)
        {
            string url = $"{_baseUrl}/api/v1/monan/all";

            // Tạo cục data gửi đi
            var requestBody = new FoodRequest
            {
                Current = pageIndex,
                PageSize = pageSize
            };

            string jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Gọi POST
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<FoodResponse>(jsonResult, _jsonOptions);
                }
                else
                {
                    // Nếu lỗi 401 (Unauthorized) nghĩa là Token hết hạn
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại.");
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách món ăn: " + ex.Message);
                return null;
            }
        }

        public async Task<FoodResponse> GetMyFoodAsync(int pageIndex, int pageSize)
        {
            // Thay đổi đường dẫn tới API "my-dishes"
            string url = $"{_baseUrl}/api/v1/monan/my-dishes";

            var requestBody = new FoodRequest
            {
                Current = pageIndex,
                PageSize = pageSize
            };

            string jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Vẫn dùng POST và Header Token đã có sẵn trong Constructor
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<FoodResponse>(jsonResult, _jsonOptions);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return null;
            }
        }


        public async Task<bool> AddFoodAsync(AddFoodRequest newFood)
        {
            string url = $"{_baseUrl}/api/v1/monan/add";

            // Chuyển object thành JSON
            string jsonContent = JsonSerializer.Serialize(newFood, _jsonOptions);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Gọi POST
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Thêm thành công
                }
                else
                {
                    // Đọc lỗi từ server trả về để debug nếu cần
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Thêm thất bại: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteFoodAsync(int id)
        {
            // URL thường là: .../api/v1/monan/123
            string url = $"{_baseUrl}/api/v1/monan/{id}";

            try
            {
                // Gọi phương thức DELETE
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Đọc lỗi từ server (ví dụ: Không được xóa món của người khác)
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Xóa thất bại: {error}");
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