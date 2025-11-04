using System;
using System.Windows.Forms;

namespace Lab03_Bai05
{
    public partial class Lab03_Bai05 : Form
    {
        private ServerForm serverInstance = null;

        public Lab03_Bai05()
        {
            InitializeComponent();
        }

        private void btnTaoServer_Click(object sender, EventArgs e)
        {
            if (serverInstance == null)
            {
                serverInstance = new ServerForm();
                serverInstance.FormClosed += (s, args) =>
                {
                    serverInstance = null;
                    if (!this.IsDisposed)
                    {
                        btnTaoServer.Enabled = true;
                        btnTaoServer.Text = "Khởi Động Server";
                    }
                };
                serverInstance.Show();
                btnTaoServer.Enabled = false;
                btnTaoServer.Text = "Server Đang Chạy";
            }
        }

        // SỬA ĐỔI QUAN TRỌNG: Lấy IP từ TextBox và truyền vào Client
        private void btnTaoClient_Click(object sender, EventArgs e)
        {
            string serverIp = txtServerIp.Text.Trim();
            if (string.IsNullOrEmpty(serverIp))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ IP của Server!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (serverInstance != null && !serverInstance.IsDisposed)
            {
                ClientForm newClient = new ClientForm(serverIp);
                newClient.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng khởi động Server trước khi tạo Client!", "Server chưa chạy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
