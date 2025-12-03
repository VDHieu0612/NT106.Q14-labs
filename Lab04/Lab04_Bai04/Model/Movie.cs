using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab04_Bai04.Model
{
    public class Movie
    {
        public string Title { get; set; }       // Tên phim
        public string ImageUrl { get; set; }    // Link hình ảnh poster
        public string DetailUrl { get; set; }   // Link chi tiết để đặt vé

        // Constructor mặc định
        public Movie() { }
    }
}