using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Threading.Tasks;

namespace Lab05_Bai06
{
    public partial class Form1 : Form
    {
        private ImapClient _imapClient;

        // Lưu danh sách message tóm tắt để khi click vào đọc thì biết lấy ID nào
        private List<IMessageSummary> _messages;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToggleControls(false); // Chưa đăng nhập thì khóa các nút chức năng
        }

        // --- XỬ LÝ ĐĂNG NHẬP ---
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string imapServer = txtImapServer.Text.Trim();
            int imapPort = (int)txtImapPort.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Email và Mật khẩu!");
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang kết nối...";

                _imapClient = new ImapClient();
                // Bỏ qua check SSL certificate nếu server local, với Gmail thì không cần dòng này nhưng an toàn
                _imapClient.ServerCertificateValidationCallback = (s, c, h, errors) => true;

                await _imapClient.ConnectAsync(imapServer, imapPort, true);
                await _imapClient.AuthenticateAsync(email, password);

                MessageBox.Show("Đăng nhập thành công!");
                ToggleControls(true);

                // Tự động tải mail sau khi login
                await LoadMailList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
                if (_imapClient.IsConnected) await _imapClient.DisconnectAsync(true);
                ToggleControls(false);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
        }

        // --- XỬ LÝ LẤY DANH SÁCH MAIL ---
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadMailList();
        }

        private async Task LoadMailList()
        {
            if (_imapClient == null || !_imapClient.IsConnected) return;

            try
            {
                var inbox = _imapClient.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                // Lấy 50 mail gần nhất
                int count = inbox.Count;
                int startIndex = Math.Max(0, count - 50);

                // Chỉ lấy thông tin Envelope (Tiêu đề, người gửi...) để nhẹ, không tải nội dung
                _messages = (await inbox.FetchAsync(startIndex, -1, MessageSummaryItems.Envelope | MessageSummaryItems.UniqueId)).Reverse().ToList();

                dataGridView1.Rows.Clear();
                int index = 0;
                foreach (var item in _messages)
                {
                    dataGridView1.Rows.Add(
                        index++, // Số thứ tự dòng
                        item.Envelope.From.ToString(),
                        item.Envelope.Subject,
                        item.Envelope.Date.HasValue ? item.Envelope.Date.Value.ToString("dd/MM/yyyy HH:mm") : ""
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải mail: " + ex.Message);
            }
        }

        // --- XỬ LÝ ĐĂNG XUẤT ---
        private async void btnLogout_Click(object sender, EventArgs e)
        {
            if (_imapClient != null)
            {
                await _imapClient.DisconnectAsync(true);
                _imapClient.Dispose();
                _imapClient = null;
            }
            dataGridView1.Rows.Clear();
            ToggleControls(false);
            MessageBox.Show("Đã đăng xuất.");
        }

        // --- XỬ LÝ ĐỌC MAIL (Double Click vào dòng) ---
        private async void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _messages == null) return;

            try
            {
                // Lấy UniqueId từ list message đã lưu
                var summary = _messages[e.RowIndex];
                var inbox = _imapClient.Inbox;

                // Tải nội dung đầy đủ (Body) của mail này
                var message = await inbox.GetMessageAsync(summary.UniqueId);

                // Mở Form đọc mail (Xem class ReadMailForm bên dưới)
                ReadMailForm readForm = new ReadMailForm(message, this);
                readForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đọc mail: " + ex.Message);
            }
        }

        // --- XỬ LÝ GỬI MAIL (Mở Form soạn thảo) ---
        private void btnCompose_Click(object sender, EventArgs e)
        {
            // Truyền thông tin đăng nhập sang Form soạn thảo để nó tự gửi
            var composeForm = new ComposeMailForm(
                txtEmail.Text,
                txtPassword.Text,
                txtSmtpServer.Text,
                (int)txtSmtpPort.Value
            );
            composeForm.Show();
        }

        private void ToggleControls(bool isLoggedIn)
        {
            grpLogin.Enabled = !isLoggedIn;
            grpSettings.Enabled = !isLoggedIn; // Không cho sửa cấu hình khi đang login

            btnCompose.Enabled = isLoggedIn;
            btnRefresh.Enabled = isLoggedIn;
            btnLogout.Enabled = isLoggedIn;
            dataGridView1.Enabled = isLoggedIn;
        }

        // ==========================================================
        // HELPER CLASSES (FORM PHỤ) - ĐỂ CHUNG FILE CHO GỌN
        // ==========================================================

        /// <summary>
        /// Form xem chi tiết nội dung mail (HTML)
        /// </summary>
        public class ReadMailForm : Form
        {
            private WebBrowser _browser;
            private Label _lblInfo;
            private Form1 _parent;
            private MimeMessage _message;

            public ReadMailForm(MimeMessage msg, Form1 parent)
            {
                _message = msg;
                _parent = parent;
                this.Text = "Đọc Email: " + msg.Subject;
                this.Size = new System.Drawing.Size(800, 600);
                this.StartPosition = FormStartPosition.CenterScreen;

                // Panel thông tin header
                Panel pnlHeader = new Panel() { Dock = DockStyle.Top, Height = 100, BackColor = System.Drawing.Color.WhiteSmoke };
                _lblInfo = new Label()
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10),
                    Text = $"From: {msg.From}\nTo: {msg.To}\nSubject: {msg.Subject}\nDate: {msg.Date}"
                };
                Button btnReply = new Button() { Text = "Reply", Left = 700, Top = 10 };
                btnReply.Click += BtnReply_Click;

                pnlHeader.Controls.Add(btnReply);
                pnlHeader.Controls.Add(_lblInfo);

                // Browser hiển thị nội dung HTML
                _browser = new WebBrowser() { Dock = DockStyle.Fill };

                // Xử lý hiển thị Body (Ưu tiên HTML, nếu ko có thì lấy Text)
                string htmlBody = !string.IsNullOrEmpty(msg.HtmlBody) ? msg.HtmlBody : $"<pre>{msg.TextBody}</pre>";
                _browser.DocumentText = htmlBody;

                this.Controls.Add(_browser);
                this.Controls.Add(pnlHeader);
            }

            private void BtnReply_Click(object sender, EventArgs e)
            {
                // Mở form soạn thảo ở chế độ Reply
                // Lấy thông tin server từ Form cha
                var compose = new ComposeMailForm(
                    _parent.Controls.Find("txtEmail", true)[0].Text,
                    _parent.Controls.Find("txtPassword", true)[0].Text,
                    _parent.Controls.Find("txtSmtpServer", true)[0].Text,
                    int.Parse(_parent.Controls.Find("txtSmtpPort", true)[0].Text)
                );
                compose.SetReply(_message);
                compose.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Form soạn thảo và gửi mail
        /// </summary>
        public class ComposeMailForm : Form
        {
            TextBox txtTo, txtSubject;
            RichTextBox txtBody;
            Label lblAttach;
            string _attachPath = "";
            string _user, _pass, _smtpHost;
            int _smtpPort;

            public ComposeMailForm(string user, string pass, string host, int port)
            {
                _user = user; _pass = pass; _smtpHost = host; _smtpPort = port;

                this.Text = "Soạn thư mới";
                this.Size = new System.Drawing.Size(600, 500);

                Label l1 = new Label() { Text = "To:", Left = 10, Top = 15, Width = 50 };
                txtTo = new TextBox() { Left = 70, Top = 12, Width = 400 };

                Label l2 = new Label() { Text = "Subject:", Left = 10, Top = 45, Width = 50 };
                txtSubject = new TextBox() { Left = 70, Top = 42, Width = 400 };

                Button btnAttach = new Button() { Text = "Đính kèm file...", Left = 10, Top = 70, Width = 100 };
                btnAttach.Click += (s, e) => {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        _attachPath = ofd.FileName;
                        lblAttach.Text = System.IO.Path.GetFileName(_attachPath);
                    }
                };
                lblAttach = new Label() { Left = 120, Top = 75, AutoSize = true, Text = "Chưa chọn file" };

                txtBody = new RichTextBox() { Left = 10, Top = 100, Width = 560, Height = 300, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };

                Button btnSend = new Button() { Text = "Gửi (Send)", Left = 470, Top = 410, Width = 100, Height = 30, Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
                btnSend.Click += BtnSend_Click;

                this.Controls.AddRange(new Control[] { l1, txtTo, l2, txtSubject, btnAttach, lblAttach, txtBody, btnSend });
            }

            public void SetReply(MimeMessage original)
            {
                txtTo.Text = original.From.ToString();
                txtSubject.Text = "Re: " + original.Subject;
                this.Text = "Trả lời mail";
            }

            private async void BtnSend_Click(object sender, EventArgs e)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_user, _user));
                message.To.Add(MailboxAddress.Parse(txtTo.Text));
                message.Subject = txtSubject.Text;

                var builder = new BodyBuilder();
                builder.HtmlBody = txtBody.Text; // Coi như nội dung soạn là HTML đơn giản

                if (!string.IsNullOrEmpty(_attachPath))
                {
                    builder.Attachments.Add(_attachPath);
                }

                message.Body = builder.ToMessageBody();

                try
                {
                    using (var client = new SmtpClient())
                    {
                        // Gmail yêu cầu StartTls hoặc SslOnConnect
                        await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.Auto);
                        await client.AuthenticateAsync(_user, _pass);
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }
                    MessageBox.Show("Gửi thành công!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi gửi mail: " + ex.Message);
                }
            }
        }
    }
}