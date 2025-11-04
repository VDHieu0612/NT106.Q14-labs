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
            this.Text = "Bảng Điều Khiển";
        }

        private void btnTaoServer_Click(object sender, EventArgs e)
        {
            if (serverInstance == null)
            {
                serverInstance = new ServerForm();
                serverInstance.FormClosed += (s, args) => {
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

        private void btnTaoClient_Click(object sender, EventArgs e)
        {
            if (serverInstance != null && !serverInstance.IsDisposed)
            {
                ClientForm newClient = new ClientForm();
                newClient.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng khởi động Server trước khi tạo Client!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}