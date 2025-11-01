using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Lab03_Bai06
{
    public partial class Client : Form
    {
        TcpClient tcpClient;
        NetworkStream stream;
        Dictionary<string, RichTextBox> chatBoxes = new Dictionary<string, RichTextBox>();
        string currentChatTarget = "All";

        public Client()
        {
            InitializeComponent();
            sendBtn.Enabled = false;
            btnSendFile.Enabled = false;

            // Tạo chat box cho "All" (tin nhắn chung)
            chatBoxes["All"] = chatDisplay;
            chatBoxes["All"].Visible = true;
            chatBoxes["All"].ReadOnly = true;
            this.FormClosing += Client_FormClosing;
        }
        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    // Đóng kết nối một cách graceful
                    stream?.Close();
                    tcpClient?.Close();
                }
            }
            catch { }
        }
        private async void connectBtn_Click(object sender, EventArgs e)
        {
            // Validation: Kiểm tra tên client
            if (string.IsNullOrWhiteSpace(clientName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên của bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                clientName.Focus();
                return;
            }

            // Kết nối đến server
            tcpClient = new TcpClient();
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            try
            {
                await tcpClient.ConnectAsync(ip, 2005);
                stream = tcpClient.GetStream();

                byte[] buffer = Encoding.UTF8.GetBytes(clientName.Text);
                await stream.WriteAsync(buffer, 0, buffer.Length);

                _ = HandleRecieve();

                connectBtn.Enabled = false;
                sendBtn.Enabled = true;
                btnSendFile.Enabled = true;
                clientName.Enabled = false;

                // Thêm "All" vào đầu danh sách
                lbClients.Items.Insert(0, "All");
                lbClients.SelectedIndex = 0;

                chatDisplay.SelectionColor = Color.Green;
                chatDisplay.SelectionFont = new Font(chatDisplay.Font, FontStyle.Bold);
                chatDisplay.AppendText("=== Đã kết nối đến server ===\n");
                chatDisplay.SelectionColor = Color.Black;
                chatDisplay.SelectionFont = chatDisplay.Font;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối: {ex.Message}", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task HandleRecieve()
        {
            byte[] buffer = new byte[8192];
            try
            {
                while (true)
                {
                    int rec = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (rec <= 0) throw new SocketException();

                    string data = Encoding.UTF8.GetString(buffer, 0, rec);

                    if (data.StartsWith("[USERLIST]"))
                    {
                        string userListData = data.Substring(10);
                        string[] userNames = userListData.Split(',');

                        Invoke((MethodInvoker)delegate {
                            string selected = lbClients.SelectedItem?.ToString();

                            lbClients.Items.Clear();
                            lbClients.Items.Add("All");

                            foreach (string userName in userNames)
                            {
                                if (userName != clientName.Text && !string.IsNullOrWhiteSpace(userName))
                                {
                                    lbClients.Items.Add(userName);

                                    if (!chatBoxes.ContainsKey(userName))
                                    {
                                        CreateChatBox(userName);
                                    }
                                }
                            }

                            // XÓA chatbox của user đã offline
                            List<string> toRemove = new List<string>();
                            foreach (var key in chatBoxes.Keys)
                            {
                                if (key != "All" && !lbClients.Items.Contains(key))
                                {
                                    toRemove.Add(key);
                                }
                            }
                            foreach (var key in toRemove)
                            {
                                chatBoxes[key].Dispose();
                                chatBoxes.Remove(key);
                            }

                            if (currentChatTarget != "All" && !lbClients.Items.Contains(currentChatTarget))
                            {
                                // User đã offline, chuyển về "All"
                                lbClients.SelectedIndex = 0;
                                currentChatTarget = "All";

                                chatBoxes["All"].SelectionColor = Color.Red;
                                chatBoxes["All"].SelectionFont = new Font(chatBoxes["All"].Font, FontStyle.Italic);
                                chatBoxes["All"].AppendText($"[Thông báo] Người dùng đã ngắt kết nối.\n");
                                chatBoxes["All"].SelectionColor = Color.Black;
                                chatBoxes["All"].SelectionFont = chatBoxes["All"].Font;
                            }
                            else if (selected != null && lbClients.Items.Contains(selected))
                            {
                                lbClients.SelectedItem = selected;
                            }
                            else
                            {
                                lbClients.SelectedIndex = 0;
                            }
                        });
                    }
                    else if (data.StartsWith("[FILE]"))
                    {
                        // Nhận file
                        await ReceiveFile(data);
                    }
                    else if (data.StartsWith("[PM tu "))
                    {
                        int endIndex = data.IndexOf("]:");
                        string sender = data.Substring(7, endIndex - 7);
                        string message = data.Substring(endIndex + 2);

                        Invoke((MethodInvoker)delegate {
                            if (chatBoxes.ContainsKey(sender))
                            {
                                chatBoxes[sender].SelectionColor = Color.Blue;
                                chatBoxes[sender].SelectionFont = new Font(chatBoxes[sender].Font, FontStyle.Bold);
                                chatBoxes[sender].AppendText($"{sender}: ");
                                chatBoxes[sender].SelectionColor = Color.Black;
                                chatBoxes[sender].SelectionFont = chatBoxes[sender].Font;
                                chatBoxes[sender].AppendText($"{message}\n");
                            }
                        });
                    }
                    else if (data.StartsWith("[PM den "))
                    {
                        int endIndex = data.IndexOf("]:");
                        string target = data.Substring(8, endIndex - 8);
                        string message = data.Substring(endIndex + 2);

                        Invoke((MethodInvoker)delegate {
                            if (chatBoxes.ContainsKey(target))
                            {
                                chatBoxes[target].SelectionColor = Color.Green;
                                chatBoxes[target].SelectionFont = new Font(chatBoxes[target].Font, FontStyle.Bold);
                                chatBoxes[target].AppendText("Me: ");
                                chatBoxes[target].SelectionColor = Color.Black;
                                chatBoxes[target].SelectionFont = chatBoxes[target].Font;
                                chatBoxes[target].AppendText($"{message}\n");
                            }
                        });
                    }
                    else
                    {
                        Invoke((MethodInvoker)delegate {
                            chatBoxes["All"].AppendText(data + "\n");
                        });
                    }
                }
            }
            catch
            {
                Invoke((MethodInvoker)delegate {
                    chatDisplay.SelectionColor = Color.Red;
                    chatDisplay.SelectionFont = new Font(chatDisplay.Font, FontStyle.Bold);
                    chatDisplay.AppendText("\n=== Mất kết nối với server ===\n");
                    chatDisplay.SelectionColor = Color.Black;
                    chatDisplay.SelectionFont = chatDisplay.Font;

                    connectBtn.Enabled = true;
                    sendBtn.Enabled = false;
                    btnSendFile.Enabled = false;
                    clientName.Enabled = true;
                    lbClients.Items.Clear();

                    foreach (var key in new List<string>(chatBoxes.Keys))
                    {
                        if (key != "All")
                        {
                            chatBoxes[key].Dispose();
                            chatBoxes.Remove(key);
                        }
                    }

                    tcpClient.Close();
                });
            }
        }

        private async Task ReceiveFile(string header)
        {
            try
            {
                // Parse: [FILE]sender|target|fileName|fileSize
                var parts = header.Substring(6).Split('|');
                string sender = parts[0];
                string target = parts[1];
                string fileName = parts[2];
                long fileSize = long.Parse(parts[3]);

                // Đọc dữ liệu file
                byte[] fileData = new byte[fileSize];
                int totalRead = 0;
                while (totalRead < fileSize)
                {
                    int read = await stream.ReadAsync(fileData, totalRead, (int)(fileSize - totalRead));
                    if (read <= 0) break;
                    totalRead += read;
                }

                // Lưu file
                string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                               "ChatFiles");
                Directory.CreateDirectory(savePath);
                string fullPath = Path.Combine(savePath, $"{DateTime.Now:yyyyMMdd_HHmmss}_{fileName}");
                File.WriteAllBytes(fullPath, fileData);

                // Hiển thị thông báo
                Invoke((MethodInvoker)delegate {
                    RichTextBox targetBox = target == "All" ? chatBoxes["All"] :
                                           (chatBoxes.ContainsKey(sender) ? chatBoxes[sender] : chatBoxes["All"]);

                    targetBox.SelectionColor = Color.Purple;
                    targetBox.SelectionFont = new Font(targetBox.Font, FontStyle.Bold);
                    targetBox.AppendText($"📎 {sender} đã gửi file: ");

                    // Thêm link có thể click
                    targetBox.SelectionColor = Color.Blue;
                    targetBox.InsertLink(fileName);
                    targetBox.AppendText($" ({FormatFileSize(fileSize)})\n");
                    targetBox.SelectionColor = Color.Black;
                    targetBox.SelectionFont = targetBox.Font;

                    targetBox.AppendText($"   Lưu tại: {fullPath}\n");

                    // Nếu là ảnh, hiển thị
                    string ext = Path.GetExtension(fileName).ToLower();
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".bmp")
                    {
                        Image img = Image.FromFile(fullPath);
                        Clipboard.SetImage(img);
                        targetBox.Paste();
                        targetBox.AppendText("\n");
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhận file: {ex.Message}");
            }
        }

        private void CreateChatBox(string userName)
        {
            RichTextBox newChatBox = new RichTextBox();
            newChatBox.Location = chatDisplay.Location;
            newChatBox.Size = chatDisplay.Size;
            newChatBox.Anchor = chatDisplay.Anchor;
            newChatBox.Font = chatDisplay.Font;
            newChatBox.ReadOnly = true;
            newChatBox.Visible = false;
            newChatBox.LinkClicked += ChatBox_LinkClicked;

            this.Controls.Add(newChatBox);
            newChatBox.BringToFront();
            chatBoxes[userName] = newChatBox;

            newChatBox.SelectionColor = Color.DarkGreen;
            newChatBox.SelectionFont = new Font(newChatBox.Font, FontStyle.Bold);
            newChatBox.AppendText($"=== Chat riêng với {userName} ===\n\n");
            newChatBox.SelectionColor = Color.Black;
            newChatBox.SelectionFont = newChatBox.Font;
        }

        private void ChatBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch { }
        }

        private void lbClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbClients.SelectedItem == null) return;

            foreach (var box in chatBoxes.Values)
            {
                box.Visible = false;
            }

            currentChatTarget = lbClients.SelectedItem.ToString();
            if (chatBoxes.ContainsKey(currentChatTarget))
            {
                chatBoxes[currentChatTarget].Visible = true;
                chatBoxes[currentChatTarget].BringToFront();
            }

            if (currentChatTarget == "All")
            {
                lblChatWith.Text = "💬 Đang chat: Tất cả mọi người";
                lblChatWith.ForeColor = Color.FromArgb(41, 128, 185);
            }
            else
            {
                lblChatWith.Text = $"💬 Đang chat riêng với: {currentChatTarget}";
                lblChatWith.ForeColor = Color.FromArgb(231, 76, 60);
            }
        }

        private async void sendBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(msgSend.Text)) return;

                string message = msgSend.Text;
                string messageToSend;

                if (currentChatTarget != "All")
                {
                    messageToSend = $"/pm {currentChatTarget} {message}";
                }
                else
                {
                    messageToSend = message;

                    Invoke((MethodInvoker)delegate {
                        chatBoxes["All"].SelectionColor = Color.Green;
                        chatBoxes["All"].SelectionFont = new Font(chatBoxes["All"].Font, FontStyle.Bold);
                        chatBoxes["All"].AppendText("Me: ");
                        chatBoxes["All"].SelectionColor = Color.Black;
                        chatBoxes["All"].SelectionFont = chatBoxes["All"].Font;
                        chatBoxes["All"].AppendText(message + "\n");
                    });
                }

                byte[] buffer = Encoding.UTF8.GetBytes(messageToSend);
                await stream.WriteAsync(buffer, 0, buffer.Length);

                msgSend.Clear();
                msgSend.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSendFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "All Files (*.*)|*.*|Images (*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif|Text Files (*.txt)|*.txt";
            openFile.Title = "Chọn file để gửi";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFile.FileName;
                    string fileName = Path.GetFileName(filePath);
                    byte[] fileData = File.ReadAllBytes(filePath);
                    long fileSize = fileData.Length;

                    // Kiểm tra kích thước (giới hạn 10MB)
                    if (fileSize > 10 * 1024 * 1024)
                    {
                        MessageBox.Show("File quá lớn! Tối đa 10MB.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Gửi header: [FILE]sender|target|fileName|fileSize
                    string header = $"[FILE]{clientName.Text}|{currentChatTarget}|{fileName}|{fileSize}";
                    byte[] headerBytes = Encoding.UTF8.GetBytes(header);
                    await stream.WriteAsync(headerBytes, 0, headerBytes.Length);

                    // Gửi dữ liệu file
                    await stream.WriteAsync(fileData, 0, fileData.Length);

                    // Hiển thị thông báo
                    RichTextBox targetBox = currentChatTarget == "All" ? chatBoxes["All"] : chatBoxes[currentChatTarget];
                    targetBox.SelectionColor = Color.Green;
                    targetBox.SelectionFont = new Font(targetBox.Font, FontStyle.Bold);
                    targetBox.AppendText($"📎 Bạn đã gửi file: ");
                    targetBox.SelectionColor = Color.Blue;
                    targetBox.AppendText($"{fileName} ({FormatFileSize(fileSize)})\n");
                    targetBox.SelectionColor = Color.Black;
                    targetBox.SelectionFont = targetBox.Font;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi gửi file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void msgSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && sendBtn.Enabled)
            {
                e.Handled = true;
                sendBtn_Click(sender, e);
            }
        }
    }

    // Extension method để thêm link vào RichTextBox
    public static class RichTextBoxExtensions
    {
        public static void InsertLink(this RichTextBox rtb, string text)
        {
            rtb.InsertLink(text, text);
        }

        public static void InsertLink(this RichTextBox rtb, string text, string hyperlink)
        {
            rtb.SelectedText = text;
            rtb.Select(rtb.TextLength - text.Length, text.Length);
            rtb.SelectionColor = Color.Blue;
            rtb.Select(rtb.TextLength, 0);
        }
    }
}