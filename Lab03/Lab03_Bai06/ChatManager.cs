using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_Bai06
{
    public partial class ChatManager : Form
    {
        int numClient = 0;
        public ChatManager()
        {
            InitializeComponent();
        }

        private void startServer_Click(object sender, EventArgs e)
        {
            Server server = new Server();
            server.Show();
        }

        private void startClient1_Click(object sender, EventArgs e)
        {
            Client newClient = new Client();

            // Đặt tiêu đề cho cửa sổ để phân biệt
            newClient.Text = "Client " + (++numClient);

            newClient.Show();
        }

       
    }
}
