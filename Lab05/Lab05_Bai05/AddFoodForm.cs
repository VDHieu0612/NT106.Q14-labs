using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Lab04_Bai07
{
    public partial class AddFoodForm : Form
    {
        private const string PLACEHOLDER_TEXT = "Tên Món Ăn;Link Hình Ảnh";
        private const string DEFAULT_EMAIL = "23521269@gm.uit.edu.vn";
        private const string DEFAULT_PASSWORD = "oqdk olqi geak dvtu";

        public AddFoodForm()
        {
            InitializeComponent();
            SetupPlaceholder();
        }

        private void SetupPlaceholder()
        {
            txtBody.Text = PLACEHOLDER_TEXT;
            txtBody.ForeColor = Color.Gray;

            txtBody.Enter += (s, e) => {
                if (txtBody.Text == PLACEHOLDER_TEXT)
                {
                    txtBody.Text = "";
                    txtBody.ForeColor = Color.Black;
                }
            };

            txtBody.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtBody.Text))
                {
                    txtBody.Text = PLACEHOLDER_TEXT;
                    txtBody.ForeColor = Color.Gray;
                }
            };
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regex pattern đơn giản cho email
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private bool ValidateInput(out string errorMessage)
        {
            errorMessage = string.Empty;

            // 1. Validate Email
            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
            {
                errorMessage = "Email không hợp lệ!\nVí dụ: example@gmail.com";
                txtEmail.Focus();
                return false;
            }

            // 2. Validate Password (nếu có email thì phải có password)
            string password = txtPassword.Text.Trim();
            if (!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                errorMessage = "Vui lòng nhập Mật khẩu ứng dụng khi sử dụng email riêng!";
                txtPassword.Focus();
                return false;
            }

            // 3. Validate Body Content
            string bodyContent = txtBody.Text.Trim();
            if (string.IsNullOrEmpty(bodyContent) || bodyContent == PLACEHOLDER_TEXT)
            {
                errorMessage = "Vui lòng nhập nội dung món ăn!\nĐịnh dạng: Tên Món Ăn;Link Hình Ảnh";
                txtBody.Focus();
                return false;
            }

            // 4. Validate định dạng Body (phải có dấu ; phân cách)
            if (!bodyContent.Contains(";"))
            {
                errorMessage = "Nội dung không đúng định dạng!\nVui lòng nhập theo mẫu: Tên Món Ăn;Link Hình Ảnh";
                txtBody.Focus();
                return false;
            }

            // 5. Validate chi tiết từng phần của Body
            string[] parts = bodyContent.Split(new[] { ';' }, StringSplitOptions.None);
            if (parts.Length < 2)
            {
                errorMessage = "Nội dung thiếu thông tin!\nVui lòng nhập đầy đủ: Tên Món Ăn;Link Hình Ảnh";
                txtBody.Focus();
                return false;
            }

            string tenMonAn = parts[0].Trim();
            string linkHinhAnh = parts[1].Trim();

            if (string.IsNullOrEmpty(tenMonAn))
            {
                errorMessage = "Tên món ăn không được để trống!";
                txtBody.Focus();
                return false;
            }

            if (tenMonAn.Length < 2)
            {
                errorMessage = "Tên món ăn phải có ít nhất 2 ký tự!";
                txtBody.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(linkHinhAnh))
            {
                errorMessage = "Link hình ảnh không được để trống!";
                txtBody.Focus();
                return false;
            }

            if (!IsValidUrl(linkHinhAnh))
            {
                errorMessage = "Link hình ảnh không hợp lệ!\nVui lòng nhập URL đầy đủ (http:// hoặc https://)";
                txtBody.Focus();
                return false;
            }

            // 6. Validate Tên hiển thị (nếu có)
            string tenHienThi = txtTenHienThi.Text.Trim();
            if (!string.IsNullOrEmpty(tenHienThi) && tenHienThi.Length > 50)
            {
                errorMessage = "Tên hiển thị không được quá 50 ký tự!";
                txtTenHienThi.Focus();
                return false;
            }

            return true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // 1. Validate toàn bộ input
            if (!ValidateInput(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lấy thông tin sau khi validate
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string bodyContent = txtBody.Text.Trim();
            string tenHienThi = txtTenHienThi.Text.Trim();

            // 3. Sử dụng thông tin mặc định nếu không nhập
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                email = DEFAULT_EMAIL;
                password = DEFAULT_PASSWORD;
            }

            // 4. Xác nhận trước khi gửi
            string confirmMessage = $"Bạn có chắc muốn gửi đóng góp này?\n\n" +
                                  $"Từ: {(string.IsNullOrEmpty(tenHienThi) ? "Người ẩn danh" : tenHienThi)}\n" +
                                  $"Email: {email}\n" +
                                  $"Nội dung: {(bodyContent.Length > 50 ? bodyContent.Substring(0, 50) + "..." : bodyContent)}";

            DialogResult result = MessageBox.Show(confirmMessage, "Xác nhận gửi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            // 5. Gửi Email
            SendEmail(email, password, bodyContent, tenHienThi);
        }

        private void SendEmail(string email, string password, string bodyContent, string tenHienThi)
        {
            btnSend.Enabled = false;
            btnSend.Text = "Sending";

            try
            {
                // Cấu hình bảo mật
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Khởi tạo SMTP Client
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(email, password);
                smtpClient.Timeout = 30000; // 30 giây timeout

                // Tạo mail
                MailMessage mail = new MailMessage();
                mail.From = string.IsNullOrEmpty(tenHienThi)
                    ? new MailAddress(email, "")
                    : new MailAddress(email, tenHienThi);
                mail.To.Add(DEFAULT_EMAIL);
                mail.Subject = "Đóng góp món ăn";
                mail.Body = bodyContent;
                mail.IsBodyHtml = false;

                // Gửi mail
                smtpClient.Send(mail);

                // Thông báo thành công
                MessageBox.Show("Đã gửi đóng góp thành công!\nCảm ơn bạn đã đóng góp.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SmtpException smtpEx)
            {
                string errorMsg = "Lỗi gửi email:\n";

                switch (smtpEx.StatusCode)
                {
                    case SmtpStatusCode.MailboxBusy:
                    case SmtpStatusCode.MailboxUnavailable:
                        errorMsg += "Hộp thư không khả dụng. Vui lòng thử lại sau.";
                        break;
                    case SmtpStatusCode.InsufficientStorage:
                        errorMsg += "Hộp thư đầy. Vui lòng kiểm tra dung lượng.";
                        break;
                    default:
                        errorMsg += "Không thể kết nối đến máy chủ email.\n" +
                                  "Vui lòng kiểm tra:\n" +
                                  "- Kết nối Internet\n" +
                                  "- Email và mật khẩu ứng dụng\n" +
                                  "- Mật khẩu ứng dụng Gmail (không phải mật khẩu thường)";
                        break;
                }

                MessageBox.Show(errorMsg, "Lỗi gửi email",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định:\n{ex.Message}\n\n" +
                              "Vui lòng thử lại hoặc liên hệ quản trị viên.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSend.Enabled = true;
                btnSend.Text = "Gửi Đóng Góp";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa toàn bộ nội dung đã nhập?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                txtBody.Text = "";
                txtTenHienThi.Clear();
                txtEmail.Clear();
                txtPassword.Clear();
                SetupPlaceholder();
                txtEmail.Focus();
            }
        }
    }
}