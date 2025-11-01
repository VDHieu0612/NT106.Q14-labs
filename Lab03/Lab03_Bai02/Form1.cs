using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Lab03_Bai02
{
    public partial class Form1 : Form
    {
        Socket server;
        Socket acc;
        public Form1()
        {
            InitializeComponent();
        }
        public Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            server = socket();
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 8080);
            server.Bind(ipe);
            server.Listen(0);
            new Thread(() =>
            {
                acc = server.Accept();
                recieveMessage();
            }

            ).Start();

        }
        private void recieveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[255];
                    int rec = acc.Receive(buffer, 0, buffer.Length, 0);
                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }
                    Array.Resize(ref buffer, rec);
                    string message = Encoding.UTF8.GetString(buffer);
                    Invoke((MethodInvoker)delegate
                    {
                        listMessage.Items.Add(message);
                    });
                }
                catch
                {
                    Invoke((MethodInvoker)delegate
                    {
                        listMessage.Items.Add("A client is disconnected!");

                    });
                    break;
                }
                
            }
            
        }

    }
}
