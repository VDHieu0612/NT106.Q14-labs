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

namespace Lab04_Bai01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Hàm lấy dữ liệu HTML từ URL
        private string getHTML(string szURL)
        {
            try
            {
                // Create a request for the URL
                WebRequest request = WebRequest.Create(szURL);

                // Get the response
                WebResponse response = request.GetResponse();

                // Get the stream containing content returned by the server
                Stream dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access
                StreamReader reader = new StreamReader(dataStream);

                // Read the content
                string responseFromServer = reader.ReadToEnd();

                // Close the response
                response.Close();

                return responseFromServer;
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }

        // Xử lý sự kiện click nút GET
        private void btnGet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtURL.Text))
            {
                MessageBox.Show("Vui lòng nhập URL!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rtbContent.Text = "Đang tải...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string htmlContent = getHTML(txtURL.Text);
                rtbContent.Text = htmlContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy nội dung: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
