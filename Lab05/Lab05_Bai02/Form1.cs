using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab05_Bai02
{
    public partial class Form1 : Form
    {
        private string selectedProtocol = "IMAP"; // Mặc định là IMAP

        public Form1()
        {
            InitializeComponent();
        }

        private void rbIMAP_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIMAP.Checked)
            {
                selectedProtocol = "IMAP";
                lblProtocol.Text = "Đang sử dụng: IMAP Protocol";
                lblProtocol.ForeColor = System.Drawing.Color.Blue;
            }
        }

        private void rbPOP3_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPOP3.Checked)
            {
                selectedProtocol = "POP3";
                lblProtocol.Text = "Đang sử dụng: POP3 Protocol";
                lblProtocol.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ email và password!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedProtocol == "IMAP")
            {
                ReadMailWithIMAP(email, password);
            }
            else
            {
                ReadMailWithPOP3(email, password);
            }
        }

        private void ReadMailWithIMAP(string email, string password)
        {
            try
            {
                listViewEmails.Items.Clear();

                using (var client = new ImapClient())
                {
                    // Xác định server IMAP
                    string server = GetIMAPServer(email);
                    int port = 993;

                    // Kết nối và xác thực
                    client.Connect(server, port, true);
                    client.Authenticate(email, password);

                    // Mở inbox
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    int total = inbox.Count;
                    int recent = inbox.Recent;
                    int limit = Math.Min(total, 20);

                    // Cập nhật thống kê
                    lblTotal.Text = total.ToString();
                    lblRecent.Text = recent.ToString();

                    // Đọc email từ mới nhất
                    for (int i = total - 1; i >= total - limit && i >= 0; i--)
                    {
                        var message = inbox.GetMessage(i);
                        AddEmailToListView(message);
                    }

                    client.Disconnect(true);

                    MessageBox.Show($"[IMAP] Đọc thành công {limit} email!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("IMAP", ex.Message);
            }
        }

        private void ReadMailWithPOP3(string email, string password)
        {
            try
            {
                listViewEmails.Items.Clear();

                using (var client = new Pop3Client())
                {
                    // Xác định server POP3
                    string server = GetPOP3Server(email);
                    int port = 995;

                    // Kết nối và xác thực
                    client.Connect(server, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    client.Authenticate(email, password);

                    int total = client.Count;
                    int limit = Math.Min(total, 20);

                    // Cập nhật thống kê
                    lblTotal.Text = total.ToString();
                    lblRecent.Text = "N/A"; // POP3 không hỗ trợ Recent

                    // Đọc email từ mới nhất
                    for (int i = total - 1; i >= total - limit && i >= 0; i--)
                    {
                        var message = client.GetMessage(i);
                        AddEmailToListView(message);
                    }

                    client.Disconnect(true);

                    MessageBox.Show($"[POP3] Đọc thành công {limit} email!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("POP3", ex.Message);
            }
        }

        private void AddEmailToListView(MimeKit.MimeMessage message)
        {
            string subject = message.Subject ?? "(No Subject)";
            string from = message.From.ToString();
            string date = message.Date.ToString("dd/MM/yyyy HH:mm:ss");

            ListViewItem item = new ListViewItem(subject);
            item.SubItems.Add(from);
            item.SubItems.Add(date);
            listViewEmails.Items.Add(item);
        }

        private string GetIMAPServer(string email)
        {
            if (email.Contains("@gmail.com"))
                return "imap.gmail.com";
            else if (email.Contains("@outlook") || email.Contains("@hotmail"))
                return "outlook.office365.com";
            else if (email.Contains("@yahoo"))
                return "imap.mail.yahoo.com";
            else
                return "imap.gmail.com"; // Mặc định
        }

        private string GetPOP3Server(string email)
        {
            if (email.Contains("@gmail.com"))
                return "pop.gmail.com";
            else if (email.Contains("@outlook") || email.Contains("@hotmail"))
                return "outlook.office365.com";
            else if (email.Contains("@yahoo"))
                return "pop.mail.yahoo.com";
            else
                return "pop.gmail.com"; // Mặc định
        }

        private void ShowErrorMessage(string protocol, string errorMessage)
        {
            MessageBox.Show($"Lỗi kết nối [{protocol}]: {errorMessage}\n\n" +
                "Lưu ý với Gmail:\n" +
                "1. Bật xác thực 2 bước\n" +
                "2. Tạo App Password tại: https://myaccount.google.com/apppasswords\n" +
                "3. Sử dụng App Password thay vì mật khẩu thường\n\n" +
                "Lưu ý với POP3:\n" +
                "- Gmail: Cần bật POP3 trong Settings > Forwarding and POP/IMAP\n" +
                "- Outlook: POP3 có thể bị giới hạn hoặc vô hiệu hóa",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MailReaderForm_Load(object sender, EventArgs e)
        {
            lblTotal.Text = "0";
            lblRecent.Text = "0";
            rbIMAP.Checked = true; // Mặc định chọn IMAP
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listViewEmails.Items.Clear();
            lblTotal.Text = "0";
            lblRecent.Text = "0";
            txtEmail.Clear();
            txtPassword.Clear();
            txtEmail.Focus();
        }
    }
}