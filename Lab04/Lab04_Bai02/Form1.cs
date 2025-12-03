using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_Bai02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Đặt đường dẫn mặc định cho file
            txtFilePath.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "downloaded.html");
        }

        // Xử lý chọn file để lưu
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "HTML Files (*.html)|*.html|All Files (*.*)|*.*";
            sfd.DefaultExt = "html";
            sfd.FileName = "downloaded.html";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = sfd.FileName;
            }
        }

        // Xử lý download file
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtURL.Text))
            {
                MessageBox.Show("Vui lòng nhập URL!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                MessageBox.Show("Vui lòng chọn đường dẫn lưu file!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            rtbContent.Text = "Đang download...";

            try
            {
                // Tạo WebClient
                WebClient myClient = new WebClient();

                myClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");
                // Sử dụng OpenRead để đọc nội dung
                Stream response = myClient.OpenRead(txtURL.Text);

                // Download file
                myClient.DownloadFile(txtURL.Text, txtFilePath.Text);

                // Đóng stream
                response.Close();

                // Đọc và hiển thị nội dung file vừa download
                string content = File.ReadAllText(txtFilePath.Text);
                rtbContent.Text = content;

                MessageBox.Show($"Download thành công!\nFile đã lưu tại: {txtFilePath.Text}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi download: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbContent.Text = "";
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // Xử lý xem nội dung file đã download
        private void btnView_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("File không tồn tại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string content = File.ReadAllText(txtFilePath.Text);
                rtbContent.Text = content;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
