using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Lab04_Bai03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                string url = txtUrl.Text.Trim();
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }

                try
                {
                    webView21.CoreWebView2.Navigate(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Thêm đoạn này để khớp với Test Case
                MessageBox.Show("Vui lòng nhập URL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 != null)
            {
                webView21.CoreWebView2.Reload();
            }
        }

        private async void btnDownFiles_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null || string.IsNullOrEmpty(webView21.CoreWebView2.Source))
            {
                MessageBox.Show("Vui lòng tải một trang web trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string html = await webView21.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
                html = System.Text.Json.JsonSerializer.Deserialize<string>(html);

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "HTML Files (*.html)|*.html";
                    sfd.FileName = "webpage.html";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(sfd.FileName, html);
                        MessageBox.Show("Đã lưu file HTML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDownResources_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null || string.IsNullOrEmpty(webView21.CoreWebView2.Source))
            {
                MessageBox.Show("Vui lòng tải một trang web trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Disable nút để tránh click nhiều lần
                btnDownResources.Enabled = false;
                btnDownResources.Text = "Đang tải...";

                string html = await webView21.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
                html = System.Text.Json.JsonSerializer.Deserialize<string>(html);

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");

                if (imgNodes == null || imgNodes.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hình ảnh nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Tạo thư mục tự động với tên có timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string folderName = $"Images_{timestamp}";
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folderName);

                // Tạo thư mục nếu chưa có
                Directory.CreateDirectory(downloadPath);

                int count = 0;
                Uri baseUri = new Uri(webView21.CoreWebView2.Source);

                // Sử dụng HttpClient thay vì WebClient
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    foreach (var img in imgNodes)
                    {
                        try
                        {
                            string imgUrl = img.GetAttributeValue("src", "");
                            if (string.IsNullOrEmpty(imgUrl)) continue;

                            // Skip data URLs
                            if (imgUrl.StartsWith("data:")) continue;

                            if (!imgUrl.StartsWith("http"))
                            {
                                imgUrl = new Uri(baseUri, imgUrl).ToString();
                            }

                            string fileName = Path.GetFileName(new Uri(imgUrl).LocalPath);
                            if (string.IsNullOrEmpty(fileName) || fileName.Length > 50)
                            {
                                fileName = $"image_{count + 1}.jpg";
                            }

                            string savePath = Path.Combine(downloadPath, fileName);

                            int duplicate = 1;
                            while (File.Exists(savePath))
                            {
                                string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                                string ext = Path.GetExtension(fileName);
                                savePath = Path.Combine(downloadPath, $"{nameWithoutExt}_{duplicate}{ext}");
                                duplicate++;
                            }

                            // Download async
                            byte[] imageBytes = await httpClient.GetByteArrayAsync(imgUrl);
                            // Sử dụng FileStream async
                            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                            {
                                await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
                            }
                            count++;

                            // Update UI
                            btnDownResources.Text = $"Đang tải ({count}/{imgNodes.Count})...";
                            Application.DoEvents();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                MessageBox.Show($"Đã tải xuống {count} hình ảnh!\nLưu tại: {downloadPath}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở thư mục sau khi download xong
                System.Diagnostics.Process.Start("explorer.exe", downloadPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDownResources.Enabled = true;
                btnDownResources.Text = "Down Resource";
            }
        }

        private void txtUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLoad_Click(sender, e);
                e.Handled = true;
            }
        }

        private void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (webView21.CoreWebView2 != null)
            {
                txtUrl.Text = webView21.CoreWebView2.Source;
            }
        }
    }
}