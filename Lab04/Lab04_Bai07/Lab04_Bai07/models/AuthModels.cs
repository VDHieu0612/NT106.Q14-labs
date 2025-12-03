using System;
using System.Collections.Generic;

namespace HomNayAnGi.Models
{
    // 1. Dùng để hứng thông tin User (dùng chung cho cả Login và Signup response)
    public class UserInfo
    {
        public string id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string avatar { get; set; }
        public int sex { get; set; }           // 0 hoặc 1
        public string birthday { get; set; }   // Dạng "YYYY-MM-DD"
        public string language { get; set; }
        public string phone { get; set; }
        public bool is_active { get; set; }
        public bool is_superuser { get; set; }
    }

    // 2. Dùng để hứng kết quả khi Login thành công
    public class LoginResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public UserInfo user { get; set; }
    }

    // 3. Dùng để gửi dữ liệu khi Đăng ký (Signup)
    public class SignupRequest
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int sex { get; set; }           // API yêu cầu int
        public string birthday { get; set; }   // API yêu cầu string "YYYY-MM-DD"
        public string language { get; set; }
        public string phone { get; set; }
    }

    // Class đại diện cho 1 lỗi nhỏ bên trong (loc, msg, type)
    public class ValidationErrorDetail
    {
        // loc là mảng hỗn hợp (chuỗi và số), nên dùng List<object>
        public List<object> loc { get; set; }
        public string msg { get; set; }
        public string type { get; set; }
    }

    // Class đại diện cho cục JSON to chứa danh sách lỗi
    public class ApiValidationError
    {
        public List<ValidationErrorDetail> detail { get; set; }
    }

    // Class phụ: Dùng cho trường hợp lỗi đơn giản (chỉ có dòng thông báo)
    public class SimpleApiError
    {
        public string detail { get; set; }
    }
}