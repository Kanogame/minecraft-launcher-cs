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
            byte[] guid = thisClientId.ToByteArray();
            int port = StartListener();
            string server = "localhost";

            client = new TcpClient(server, port);
            clientStream = client.GetStream();
            clientStream.write(guid);
            clientStream.writeString("WS4-fabric");

            var t = new Thread(processTransfer);
            t.IsBackground = true;
            t.Start();
        }

        private int StartListener()
        {
            TcpListener listener;
            int port;
            while (true)
            {
                port = 23032;
                try
                {
                    listener = new TcpListener(IPAddress.Any, port);
                    listener.Start();

                    break;
                }
                catch (Exception)
                {
                    //ignore
                }
            }
            var t = new Thread(transferFiles);
            t.IsBackground = true;
            t.Start(listener);
            return port;
        }

        private void transferFiles(object o)
        {
            var listener = (TcpListener)o;
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var t = new Thread(processTransfer);
                t.IsBackground = true;
                t.Start(client);
            }
        }

        private void processTransfer(object o)
        {
            var client = (TcpClient)o;
            var ns = client.GetStream();
            var command = (TransferCommands)ns.ReadByte();
            if (command == TransferCommands.Ping)
            {
                ns.write(thisClientId.ToByteArray());
            }
            else if (command == TransferCommands.Send)
            {
                int fileCount = ns.readInt();
                long[] lengthArray = new long[fileCount];
                string[] NameArray = new string[fileCount];
                string fileDescriptions = "";
                for (int i = 0; i < fileCount; i++)
                {
                    lengthArray[i] = ns.readLong();
                    NameArray[i] = ns.readString();
                    fileDescriptions = $"{NameArray[i]}, {lengthArray[i]} байт\n";
                }
                string question = $"вы желаете принять {fileCount} файлов от {client.Client.RemoteEndPoint}: \n" + fileDescriptions;
                bool confirmation = MessageBox.Show(question, "принять", MessageBoxButtons.YesNoCancel) == DialogResult.Yes;
                if (confirmation)
                {
                    string folderPath = "";
                    this.Invoke(new Action(() => {
                        using (var folderDialog = new FolderBrowserDialog())
                        {
                            folderDialog.Description = "в какую папку вы желаете сожранить файлы?";
                            confirmation = folderDialog.ShowDialog() == DialogResult.OK;
                            folderPath = folderDialog.SelectedPath;
                        }
                    }));
                    for (int i = 0; i < 0; i++)
                    {
                        string path = Path.Combine(folderPath, NameArray[i]);
                        if (File.Exists(path))
                        {
                            confirmation = false;
                            MessageBox.Show($"файл уже существует: {path}");
                            break;
                        }
                    }
                    ns.writeBool(confirmation);
                    if (confirmation)
                    {
                        for (int i = 0; i < fileCount; i++)
                        {
                            long left = lengthArray[i];
                            string path = Path.Combine(folderPath, NameArray[i]);
                            using (Stream f = File.OpenWrite(path))
                            {
                                while (left > 0)
                                {
                                    int cnt = (int)Math.Min(left, 2048);
                                    byte[] bytes = ns.read(cnt);
                                    f.Write(bytes, 0, bytes.Length);
                                    left -= cnt;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
