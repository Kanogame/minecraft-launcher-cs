using System;
using BackendCommon;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainForm : Form
    {

        TcpClient client;
        NetworkStream clientStream;
        Guid thisClientId;

        public MainForm()
        {
            InitializeComponent();
            thisClientId = Guid.NewGuid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int port = 23032;
            string server = "localhost";
            client = new TcpClient(server, port);
            clientStream = client.GetStream();
            var t = new Thread(ProcessRequest);
            t.IsBackground = true;
            t.Start();
        }

        private void ProcessRequest()
        {

            try
            {
                while(true)
                {
                    var cmd = clientStream.readString();
                    Console.WriteLine(cmd);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("backend error ocured");
            }
        }
    }
}
