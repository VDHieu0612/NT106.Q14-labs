// File: FoodModels.cs
using System.Collections.Generic;
using SQLite;

namespace HomNayAnGi.Models
{
    // 1. Đối tượng Món ăn (Cho SQLite và hiển thị)
    public class Food
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string TenMonAn { get; set; }

        public decimal Gia { get; set; } // Giá tiền nên để decimal hoặc int

        public string MoTa { get; set; }

        public string HinhAnh { get; set; } // URL ảnh

        public string DiaChi { get; set; }

        [NotNull]
        public string NguoiDongGop { get; set; } // Tên người đóng góp từ email
    }

    // 2. Đối tượng Phân trang (Nếu vẫn dùng cho API cũ)
    // Nếu không dùng nữa thì có thể xóa hoặc di chuyển sang file riêng nếu cần
    public class Pagination
    {
        public int Current { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }

    // 3. Đối tượng Phản hồi tổng (Response Wrapper - Nếu vẫn dùng cho API cũ)
    // Nếu không dùng nữa thì có thể xóa hoặc di chuyển sang file riêng nếu cần
    public class FoodResponse
    {
        public List<Food> Data { get; set; }
        public Pagination Pagination { get; set; }
    }

    // 4. Đối tượng Gửi đi để yêu cầu dữ liệu (Request Body - Nếu vẫn dùng cho API cũ)
    // Nếu không dùng nữa thì có thể xóa hoặc di chuyển sang file riêng nếu cần
    public class FoodRequest
    {
        public int Current { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    // Đối tượng Gửi đi để thêm món ăn mới (Nếu vẫn dùng cho API cũ)
    // Nếu không dùng nữa thì có thể xóa hoặc di chuyển sang file riêng nếu cần
    public class AddFoodRequest
    {
        public string TenMonAn { get; set; }
        public decimal Gia { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string DiaChi { get; set; }
        public string NguoiDongGop { get; set; } // Giữ lại nếu muốn truyền, nhưng API có thể bỏ qua
    }
}