using System;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace Lab05
{
    public partial class Lab05_Bai01 : Form
    {
        public Lab05_Bai01()
        {
            InitializeComponent();
            // Gắn sự kiện click cho button Send
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra các trường nhập liệu
                if (string.IsNullOrWhiteSpace(Fromtb.Text))
                {
                    MessageBox.Show("Vui lòng nhập email người gửi!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Totb.Text))
                {
                    MessageBox.Show("Vui lòng nhập email người nhận!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Subtb.Text))
                {
                    MessageBox.Show("Vui lòng nhập tiêu đề email!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Thông tin Gmail SMTP
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 465;
                string username = Fromtb.Text.Trim(); // Gmail
                string appPassword = ""; // App Password

                // Hiển thị dialog để nhập App Password
                using (var passwordForm = new Form())
                {
                    passwordForm.Text = "Nhập App Password";
                    passwordForm.Width = 400;
                    passwordForm.Height = 150;
                    passwordForm.StartPosition = FormStartPosition.CenterParent;
                    passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    passwordForm.MaximizeBox = false;
                    passwordForm.MinimizeBox = false;

                    var label = new Label() { Left = 20, Top = 20, Text = "App Password:" };
                    var textBox = new TextBox()
                    {
                        Left = 20,
                        Top = 50,
                        Width = 340,
                        UseSystemPasswordChar = true
                    };
                    var btnOK = new Button()
                    {
                        Text = "OK",
                        Left = 280,
                        Top = 85,
                        DialogResult = DialogResult.OK
                    };

                    passwordForm.Controls.Add(label);
                    passwordForm.Controls.Add(textBox);
                    passwordForm.Controls.Add(btnOK);
                    passwordForm.AcceptButton = btnOK;

                    if (passwordForm.ShowDialog() == DialogResult.OK)
                    {
                        appPassword = textBox.Text;
                    }
                    else
                    {
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(appPassword))
                {
                    MessageBox.Show("Vui lòng nhập App Password!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Vô hiệu hóa button để tránh click nhiều lần
                btnSend.Enabled = false;
                btnSend.Text = "Đang gửi...";

                // Lưu thông tin để gửi
                string fromEmail = Fromtb.Text.Trim();
                string toEmail = Totb.Text.Trim();
                string subject = Subtb.Text.Trim();
                string body = Contentrtb.Text;

                // Gửi email
                await Task.Run(() => SendEmail(smtpHost, smtpPort, username, appPassword,
                    fromEmail, toEmail, subject, body));

                MessageBox.Show("Email đã được gửi thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa nội dung sau khi gửi email thành công
                Totb.Clear();
                Subtb.Clear();
                Contentrtb.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi gửi email:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục lại button
                btnSend.Enabled = true;
                btnSend.Text = "SEND";
            }
        }

        private void SendEmail(string smtpHost, int smtpPort, string username,
            string appPassword, string fromEmail, string toEmail, string subject, string body)
        {
            // Tạo email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            // Tạo body của email
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            // Gửi email
            using (var client = new SmtpClient())
            {
                // Kết nối tới Gmail SMTP server
                client.Connect(smtpHost, smtpPort, SecureSocketOptions.SslOnConnect);

                // Xác thực
                client.Authenticate(username, appPassword);

                // Gửi email
                client.Send(message);

                // Ngắt kết nối
                client.Disconnect(true);
            }
        }
    }
}