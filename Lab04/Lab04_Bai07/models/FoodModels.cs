using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomNayAnGi.Models
{
    // 1. Đối tượng Món ăn (Map đúng với JSON trong "data")
    public class Food
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ten_mon_an")]
        public string TenMonAn { get; set; }

        [JsonPropertyName("gia")]
        public decimal Gia { get; set; } // Giá tiền nên để decimal hoặc int

        [JsonPropertyName("mo_ta")]
        public string MoTa { get; set; }

        [JsonPropertyName("hinh_anh")]
        public string HinhAnh { get; set; } // URL ảnh

        [JsonPropertyName("dia_chi")]
        public string DiaChi { get; set; }

        [JsonPropertyName("nguoi_dong_gop")]
        public string NguoiDongGop { get; set; }
    }

    // 2. Đối tượng Phân trang (Pagination)
    public class Pagination
    {
        [JsonPropertyName("current")]
        public int Current { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    // 3. Đối tượng Phản hồi tổng (Response Wrapper)
    public class FoodResponse
    {
        [JsonPropertyName("data")]
        public List<Food> Data { get; set; }

        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; }
    }

    // 4. Đối tượng Gửi đi để yêu cầu dữ liệu (Request Body)
    public class FoodRequest
    {
        [JsonPropertyName("current")]
        public int Current { get; set; } = 1;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 5;
    }


    public class AddFoodRequest
    {
        [JsonPropertyName("ten_mon_an")]
        public string TenMonAn { get; set; }

        [JsonPropertyName("gia")]
        public decimal Gia { get; set; } // API yêu cầu số

        [JsonPropertyName("mo_ta")]
        public string MoTa { get; set; }

        [JsonPropertyName("hinh_anh")]
        public string HinhAnh { get; set; }

        [JsonPropertyName("dia_chi")]
        public string DiaChi { get; set; }
    }
}