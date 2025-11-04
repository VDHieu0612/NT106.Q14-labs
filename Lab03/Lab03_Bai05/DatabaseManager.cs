using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Lab03_Bai05
{
    public class DatabaseManager
    {
        private readonly string dbPath = "MonAnDB.db";

        public void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();
                    string sql = "CREATE TABLE MonAn (TenMonAn TEXT PRIMARY KEY, HinhAnh TEXT, TenNguoiDongGop TEXT)";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }

        public List<MonAnDayDu> LayDanhSachMonAn()
        {
            var list = new List<MonAnDayDu>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM MonAn";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new MonAnDayDu
                        {
                            TenMonAn = reader["TenMonAn"].ToString(),
                            HinhAnh = reader["HinhAnh"].ToString(),
                            TenNguoiDongGop = reader["TenNguoiDongGop"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public void ThemMonAn(string tenMonAn, string hinhAnh, string tenNguoiDongGop)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = "INSERT INTO MonAn (TenMonAn, HinhAnh, TenNguoiDongGop) VALUES (@ten, @hinh, @nguoi)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ten", tenMonAn);
                    command.Parameters.AddWithValue("@hinh", hinhAnh);
                    command.Parameters.AddWithValue("@nguoi", tenNguoiDongGop);
                    command.ExecuteNonQuery();
                }
            }
        }

        public MonAnDayDu LayMonAnNgauNhien()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM MonAn ORDER BY RANDOM() LIMIT 1";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MonAnDayDu
                        {
                            TenMonAn = reader["TenMonAn"].ToString(),
                            HinhAnh = reader["HinhAnh"].ToString(),
                            TenNguoiDongGop = reader["TenNguoiDongGop"].ToString()
                        };
                    }
                }
            }
            return null;
        }

    }
}
