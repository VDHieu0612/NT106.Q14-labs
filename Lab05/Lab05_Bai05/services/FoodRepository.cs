// File: LocalFoodRepository.cs
using HomNayAnGi.Models; // Đảm bảo đã có
using SQLite;
using System;
using System.IO; // Sử dụng Path thay vì System.Net.NetworkInformation
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HomNayAnGi.Services
{
    public class FoodRepository
    {
        private SQLiteAsyncConnection _database;

        public FoodRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            // Đảm bảo bảng được tạo khi khởi tạo, sử dụng lớp Food
            _database.CreateTableAsync<Food>().Wait();
        }

        // Khởi tạo Repository với đường dẫn mặc định
        public FoodRepository()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "homnayangi_contributions.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            // Lệnh này kiểm tra: Nếu file chưa có -> Tạo mới. Nếu bảng chưa có -> Tạo bảng.
            // Nếu đã có rồi -> Giữ nguyên dữ liệu cũ để load lên.
            _database.CreateTableAsync<Food>().Wait();
        }

        /// <summary>
        /// Thêm một món ăn đóng góp mới vào cơ sở dữ liệu cục bộ.
        /// </summary>
        /// <param name="food">Đối tượng món ăn đóng góp.</param>
        /// <returns>Số hàng đã được thêm vào.</returns>
        public async Task<int> AddFoodAsync(Food food) // Sử dụng Food
        {
            return await _database.InsertAsync(food);
        }

        /// <summary>
        /// Kiểm tra xem tên món ăn đã tồn tại trong cơ sở dữ liệu cục bộ chưa.
        /// </summary>
        /// <param name="tenMonAn">Tên món ăn cần kiểm tra.</param>
        /// <returns>True nếu tồn tại, ngược lại False.</returns>
        public async Task<bool> IsFoodNameExistsAsync(string tenMonAn)
        {
            // So sánh không phân biệt hoa thường và bỏ qua khoảng trắng thừa
            string searchName = tenMonAn.ToLower().Trim();
            var existingFood = await _database.Table<Food>()
                                    .Where(f => f.TenMonAn.ToLower() == searchName)
                                    .FirstOrDefaultAsync();
            return existingFood != null;
        }

        /// <summary>
        /// Lấy tất cả món ăn đóng góp từ cơ sở dữ liệu cục bộ.
        /// </summary>
        /// <returns>Danh sách các món ăn đóng góp.</returns>
        public async Task<List<Food>> GetAllContributedFoodsAsync() // Trả về List<Food>
        {
            return await _database.Table<Food>().ToListAsync(); // Sử dụng Food
        }

        /// <summary>
        /// Chọn ngẫu nhiên một món ăn từ các món đã đóng góp.
        /// </summary>
        /// <returns>Một đối tượng Food ngẫu nhiên, hoặc null nếu không có món ăn nào.</returns>
        public async Task<Food> GetRandomContributedFoodAsync() // Trả về Food
        {
            var allFoods = await GetAllContributedFoodsAsync();
            if (allFoods == null || !allFoods.Any())
            {
                return null;
            }

            Random rand = new Random();
            int index = rand.Next(allFoods.Count);
            return allFoods[index];
        }

        // Có thể thêm các phương thức khác như GetById, Delete, Update nếu cần
    }
}