using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_Bai01
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 1. Tạo một luồng mới cho form Client
            Thread clientThread = new Thread(() => {
                // Application.Run khởi động message loop cho form trên luồng này
                Application.Run(new UdpClient());
            });

            // 2. Thiết lập ApartmentState thành Single Threaded Apartment (STA),
            //    đây là yêu cầu bắt buộc cho các ứng dụng Windows Forms.
            clientThread.SetApartmentState(ApartmentState.STA);

            // 3. Bắt đầu luồng của Client
            clientThread.Start();

            // 4. Chạy form Server trên luồng chính (Main thread)
            // Luồng chính sẽ bị block ở đây cho đến khi form Server được đóng.
            Application.Run(new UdpServer());
        }
    }
}
