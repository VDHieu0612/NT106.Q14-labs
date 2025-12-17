using HomNayAnGi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_Bai07
{
    internal static class Program
    {
        // Biến toàn cục để lưu Token và Thông tin user sau khi đăng nhập
        public static string Token = null;
        public static UserInfo CurrentUser = null;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            SQLitePCL.Batteries_V2.Init();

            // Chạy Form Login đầu tiên
            Application.Run(new MainForm());
        }
    }
}
