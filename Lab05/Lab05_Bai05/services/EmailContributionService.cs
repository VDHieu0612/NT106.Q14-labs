using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomNayAnGi.Models;
using HomNayAnGi.Services;

namespace HomNayAnGi.Services
{
    public class EmailContributionService
    {
        private readonly string _imapServer;
        private readonly int _imapPort;
        private readonly string _emailUsername;
        private readonly string _emailPassword;
        private readonly string _contributionSubject = "Đóng góp món ăn";

        private FoodRepository _foodRepository;

        public EmailContributionService(string imapServer, int imapPort, string emailUsername, string emailPassword)
        {
            _imapServer = imapServer;
            _imapPort = imapPort;
            _emailUsername = emailUsername;
            _emailPassword = emailPassword;
            _foodRepository = new FoodRepository();
        }

        public async Task<int> ProcessNewContributionsAsync()
        {
            int newFoodsAddedCount = 0;

            using (var client = new ImapClient())
            {
                try
                {
                    // 1. Kết nối và Đăng nhập
                    await client.ConnectAsync(_imapServer, _imapPort, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(_emailUsername, _emailPassword);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadWrite);

                    // 2. TÌM KIẾM EMAIL (Đã thêm lại điều kiện NotSeen)
                    // Logic: Tiêu đề chứa "Đóng góp món ăn" VÀ Chưa đọc
                    var uids = await inbox.SearchAsync(MailKit.Search.SearchQuery.SubjectContains(_contributionSubject)
                                                    .And(MailKit.Search.SearchQuery.NotSeen));

                    if (uids.Count == 0)
                    {
                        return 0;
                    }

                    foreach (var uid in uids)
                    {
                        try
                        {
                            var message = await inbox.GetMessageAsync(uid);

                            // Kiểm tra lại tiêu đề (phòng hờ)
                            if (message?.TextBody == null || !message.Subject.Contains(_contributionSubject))
                            {
                                // Đánh dấu đã đọc để bỏ qua
                                await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                                continue;
                            }

                            // 3. Xử lý tên người đóng góp
                            string nguoiDongGop = "Người ẩn danh";
                            if (message.From != null && message.From.Mailboxes.Any())
                            {
                                var sender = message.From.Mailboxes.First();
                                // Nếu có tên thì lấy, không thì giữ nguyên là "Người ẩn danh"
                                if (!string.IsNullOrEmpty(sender.Name))
                                {
                                    nguoiDongGop = sender.Name;
                                }
                            }

                            // 4. Phân tích nội dung
                            var lines = message.TextBody.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var line in lines)
                            {
                                var parts = line.Split(new char[] { ';' }, 2, StringSplitOptions.RemoveEmptyEntries);

                                if (parts.Length == 2)
                                {
                                    string tenMonAn = parts[0].Trim();
                                    string hinhAnh = parts[1].Trim();

                                    // Kiểm tra trùng lặp trong Database
                                    if (!string.IsNullOrEmpty(tenMonAn) && !await _foodRepository.IsFoodNameExistsAsync(tenMonAn))
                                    {
                                        var newFood = new Food
                                        {
                                            TenMonAn = tenMonAn,
                                            HinhAnh = hinhAnh,
                                            NguoiDongGop = nguoiDongGop,
                                            Gia = 0,
                                            MoTa = "Đóng góp qua email",
                                            DiaChi = "Không rõ"
                                        };

                                        await _foodRepository.AddFoodAsync(newFood);
                                        newFoodsAddedCount++;
                                    }
                                }
                            }

                            // 5. QUAN TRỌNG: Đánh dấu email là ĐÃ ĐỌC (Seen)
                            // Để lần sau bấm cập nhật nó sẽ không tìm thấy email này nữa
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                        }
                        catch (Exception innerEx)
                        {
                            // Nếu lỗi xử lý email này, cũng đánh dấu đã đọc để tránh kẹt
                            Console.WriteLine(innerEx.Message);
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ném lỗi ra ngoài để MainForm hiển thị MessageBox
                    throw ex;
                }
                finally
                {
                    if (client.IsConnected) await client.DisconnectAsync(true);
                }
            }
            return newFoodsAddedCount;
        }
    }
}