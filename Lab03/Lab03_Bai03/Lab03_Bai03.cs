using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03_Bai03
{
    public partial class Lab03_Bai03 : Form
    {
        public Lab03_Bai03()
        {
            InitializeComponent();
        }

        private void btnOpenSV_Click(object sender, EventArgs e)
        {
            ServerForm serverForm = new ServerForm();
            serverForm.Show();
        }

        private void btnOpenCli_Click(object sender, EventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            clientForm.Show();
        }
    }
}
