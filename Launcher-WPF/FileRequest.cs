using BackendCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Diagnostics;

namespace Launcher_WPF
{
    public class FileRequest
    {
        string defaultTempPath = "C:\\Users\\OneSmiLe\\Desktop\\Temp\\Resenved";
        string defPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kanoCraft");
        string defaultGamePath;
        string instanceName = "";
        string pathName = "";

        goConn GoConn;
        Crypt cr;
        System.Windows.Controls.ProgressBar pb;

        public event DownloadFinishDelegate DownloadCompleted;

        public FileRequest(goConn GoConn, System.Windows.Controls.ProgressBar pb)
        {
            this.GoConn = GoConn;
            this.pb = pb;
            cr = new Crypt();
            defaultGamePath = Path.Combine(defPath, "instances");
            TempClearing(defaultTempPath);
        }

        public bool ReadUserData()
        {
            return File.Exists(Path.Combine(defPath, "data", "temp.txt"));
        }

        public void WriteUserData(string name, string password)
        {
            var data = GoConn.FileCR(name, password);
            if (data != null) 
            {
                int id = int.Parse(data[0]);
                string key = data[1];
                using (StreamWriter sw = new StreamWriter(Path.Combine(defPath, "data", "temp.txt")))
                {
                    sw.WriteLine(id + "-" + cr.Encode(password, key));
                }
            }
        }

        public void GetImages()
        {
            var tempPath = Path.Combine(defPath, "temp");

            if (!File.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            DirectoryInfo di = new DirectoryInfo(tempPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            var imagePath = GoConn.ImageHandler(tempPath);
        }

        public bool SendUserData()
        {
            using (StreamReader sr = new StreamReader(Path.Combine(defPath, "data", "temp.txt")))
            {
                return GoConn.WriteData(sr.ReadLine());
            }
        }

        public bool CheckFile(string instanceName)
        {
            return Directory.Exists(Path.Combine(defaultGamePath, instanceName));
        }

        public string GetInstPath(string instanceName)
        {
            return Path.Combine(defaultGamePath, instanceName);
        }

        public string GetGamePath()
        {
            return Path.Combine(defaultGamePath);
        }

        public void GetFile(string username, string instanceName, string pathName)
        {
            TcpClient client = GoConn.GetBackIp();
            NetworkStream clientStream = client.GetStream();
            clientStream.writeString(username);
            clientStream.writeString(instanceName);
            this.instanceName = instanceName;
            this.pathName = pathName;
            if (clientStream.readBool())
            {
                var t = new Thread(processTransfer);
                t.IsBackground = true;
                t.Start(clientStream);
            }
        }

        private void processTransfer(object o)
        {
            var ns = (NetworkStream)o;
            var command = (TransferCommands)ns.ReadByte();
            if (command == TransferCommands.Ping)
            {
                Console.WriteLine("ping");
            }
            else if (command == TransferCommands.Send)
            {
                ns.writeBool(true);
                long length = ns.readLong();
                var startlength = length;
                long percent = length / 100;
                int currentPers = 0;
                int progress = 0;
                string name = ns.readString();
                string path = Path.Combine("C:\\Users\\OneSmiLe\\Desktop\\Temp\\Resenved", name);
                int iterator = 0;
                using (Stream f = File.OpenWrite(path))
                {
                    while (length > 0)
                    {
                        int cnt = (int)Math.Min(length, 2048);
                        byte[] bytes = ns.read(cnt);
                        f.Write(bytes, 0, bytes.Length);
                        length -= cnt;
                        iterator++;
                        if (iterator % 5000 == 0)
                        {
                            progress = (int)(((double)startlength - (double)length) / (double)percent);
                            if (progress > currentPers)
                            {
                                currentPers = progress;
                                pb.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                                    pb.Value = currentPers;
                                }));
                            }
                        }
                    }
                }
                pb.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    pb.Value = 100;
                    InvokeDownloadCompleted(instanceName);
                }));
                string tempPath = Path.Combine(defaultTempPath, name);
                UnZip(tempPath);
                TempClearing(tempPath);
                pb.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    InvokeDownloadCompleted(instanceName);
                }));
            }
        }

        private void UnZip(string tempPath)
        {
            try
            {
                if (instanceName != "")
                {
                    ZipFile.ExtractToDirectory(tempPath, Path.Combine(defaultGamePath, pathName));
                    instanceName = "";
                }
                else throw new Exception("instance name was null");
            }
            catch (Exception ex)
            {
                MessageBox.Show("при распаковке что-то пошло не так, " + ex.Message);
            }
        }

        private void TempClearing(string tempPath)
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }

        private void InvokeDownloadCompleted(string ins)
        {
            if (DownloadCompleted != null)
            {
                DownloadCompleted(ins);
            }
        }
    }
}
