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
using System.IO.Compression;

namespace Launcher
{
    public partial class MainForm : Form
    {
        string defaultTempPath = "C:\\Users\\OneSmiLe\\Desktop\\Temp\\Resenved";
        string defaultGamePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kanoCraft", "instances");
        string instanceName = "WS4-fabric";

        TcpClient client;
        NetworkStream clientStream;
        Guid thisClientId;
        goConn GoConn;

        public MainForm()
        {
            InitializeComponent();
            thisClientId = Guid.NewGuid();
            GoConn = new goConn("127.0.0.1", 8081);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] guid = thisClientId.ToByteArray();
            string server = "127.0.0.1";
            int port = 23032;

            client = new TcpClient(server, port);
            clientStream = client.GetStream();
            clientStream.write(guid);
            clientStream.writeString(instanceName);

            var t = new Thread(processTransfer);
            t.IsBackground = true;
            t.Start(clientStream);
        }

        private void processTransfer(object o)
        {
            var ns = (NetworkStream)o;
            var command = (TransferCommands)ns.ReadByte();
            if (command == TransferCommands.Ping)
            {
                Console.WriteLine("ping");
                MessageBox.Show("Ping");
            }
            else if (command == TransferCommands.Send)
            {
                ns.writeBool(true);
                long length = ns.readLong();
                string name = ns.readString();
                string path = Path.Combine("C:\\Users\\OneSmiLe\\Desktop\\Temp\\Resenved", name);
                using (Stream f = File.OpenWrite(path))
                {
                    while (length > 0)
                    {
                        int cnt = (int)Math.Min(length, 2048);
                        byte[] bytes = ns.read(cnt);
                        f.Write(bytes, 0, bytes.Length);
                        length -= cnt;
                    }
                }
                MessageBox.Show("DONE!");
                string tempPath = Path.Combine(defaultTempPath, name);
                UnZip(tempPath);
                TempClearing(tempPath);
            }
        }

        private void UnZip(string tempPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(tempPath, Path.Combine(defaultGamePath, instanceName));
            }
            catch (Exception)
            {
                MessageBox.Show("при распаковке что-то пошло не так");
            }
        }

        private void TempClearing(string tempPath)
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GoConn.Listen();
        }
    }
}
