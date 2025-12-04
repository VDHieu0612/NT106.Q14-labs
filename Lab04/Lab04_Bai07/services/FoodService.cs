using HomNayAnGi.Models;
using Lab04_Bai07;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        /// <summary>
        /// Lấy danh sách món ăn theo phân trang
        /// </summary>
        public async Task<FoodResponse> GetAllFoodAsync(int pageIndex, int pageSize)
        {
            string url = $"{_baseUrl}/api/v1/monan/all";

            var requestBody = new FoodRequest
            {
                Current = pageIndex,
                PageSize = pageSize
            };

            string jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<FoodResponse>(jsonResult, _jsonOptions);
                }
                else
                {
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

        /// <summary>
        /// Lấy món ăn của tôi
        /// </summary>
        public async Task<FoodResponse> GetMyFoodAsync(int pageIndex, int pageSize)
        {
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

        /// <summary>
        /// Thêm món ăn mới
        /// </summary>
        public async Task<bool> AddFoodAsync(AddFoodRequest newFood)
        {
            string url = $"{_baseUrl}/api/v1/monan/add";

            string jsonContent = JsonSerializer.Serialize(newFood, _jsonOptions);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
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

        /// <summary>
        /// Xóa món ăn
        /// </summary>
        public async Task<bool> DeleteFoodAsync(int id)
        {
            string url = $"{_baseUrl}/api/v1/monan/{id}";

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
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

        /// <summary>
        /// Lấy TẤT CẢ món ăn (không phân trang) - Dùng để kiểm tra trùng
        /// LƯU Ý: Chỉ dùng khi cần thiết vì tốn tài nguyên
        /// </summary>
        public async Task<List<Food>> GetAllFoodsWithoutPagingAsync()
        {
            try
            {
                List<Food> allFoods = new List<Food>();
                int pageIndex = 1;
                int pageSize = 100;
                bool hasMorePages = true;

                while (hasMorePages)
                {
                    FoodResponse response = await GetAllFoodAsync(pageIndex, pageSize);

                    if (response == null || response.Data == null || response.Data.Count == 0)
                        break;

                    allFoods.AddRange(response.Data);

                    // Nếu số món trả về < pageSize nghĩa là đã hết
                    if (response.Data.Count < pageSize)
                    {
                        hasMorePages = false;
                    }
                    else
                    {
                        pageIndex++;
                    }
                }

                return allFoods;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy toàn bộ món ăn: " + ex.Message);
                return new List<Food>();
            }
        }

        /// <summary>
        /// Kiểm tra tên món ăn có tồn tại không
        /// </summary>
        public async Task<bool> IsFoodNameExistsAsync(string tenMonAn)
        {
            try
            {
                string searchName = tenMonAn.Trim().ToLower();
                int pageIndex = 1;
                int pageSize = 100;
                bool hasMorePages = true;

                while (hasMorePages)
                {
                    FoodResponse response = await GetAllFoodAsync(pageIndex, pageSize);

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
                    if (response.Data.Count < pageSize)
                    {
                        hasMorePages = false;
                    }
                    else
                    {
                        pageIndex++;
                    }
                }

                return false; // Không trùng
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra tên món: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra tên món có trùng không (trừ ID hiện tại - dùng cho Update)
        /// </summary>
        public async Task<bool> IsFoodNameExistsExceptAsync(string tenMonAn, int excludeId)
        {
            try
            {
                string searchName = tenMonAn.Trim().ToLower();
                int pageIndex = 1;
                int pageSize = 100;
                bool hasMorePages = true;

                while (hasMorePages)
                {
                    FoodResponse response = await GetAllFoodAsync(pageIndex, pageSize);

                    if (response == null || response.Data == null)
                        break;

                    // Kiểm tra trong trang hiện tại (bỏ qua ID hiện tại)
                    foreach (var food in response.Data)
                    {
                        if (food.Id != excludeId &&
                            food.TenMonAn.Trim().ToLower() == searchName)
                        {
                            return true; // Tìm thấy trùng
                        }
                    }

                    if (response.Data.Count < pageSize)
                    {
                        hasMorePages = false;
                    }
                    else
                    {
                        pageIndex++;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra tên món: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Tìm kiếm món ăn theo tên trong TẤT CẢ các trang
        /// </summary>
        public async Task<List<Food>> SearchFoodsByNameAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return new List<Food>();

                string searchLower = searchTerm.Trim().ToLower();
                List<Food> results = new List<Food>();
                int pageIndex = 1;
                int pageSize = 100;
                bool hasMorePages = true;

                while (hasMorePages)
                {
                    FoodResponse response = await GetAllFoodAsync(pageIndex, pageSize);

                    if (response == null || response.Data == null)
                        break;

                    // Lọc món ăn chứa từ khóa tìm kiếm
                    foreach (var food in response.Data)
                    {
                        if (food.TenMonAn.ToLower().Contains(searchLower) ||
                            (food.MoTa != null && food.MoTa.ToLower().Contains(searchLower)))
                        {
                            results.Add(food);
                        }
                    }

                    if (response.Data.Count < pageSize)
                    {
                        hasMorePages = false;
                    }
                    else
                    {
                        pageIndex++;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                return new List<Food>();
            }
        }
    }
}