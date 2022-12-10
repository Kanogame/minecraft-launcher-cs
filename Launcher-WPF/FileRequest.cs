using BackendCommon;
using Launcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Compression;

namespace Launcher_WPF
{
    public class FileRequest
    {
        string defaultTempPath = "C:\\Users\\OneSmiLe\\Desktop\\Temp\\Resenved";
        string defaultGamePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kanoCraft", "instances");
        string instanceName = "";
        goConn GoConn;

        public FileRequest(goConn GoConn)
        {
            this.GoConn = GoConn;
        }

        public bool CheckFile(string instanceName)
        {
            return File.Exists(Path.Combine(defaultGamePath, instanceName, "versions", "version_manifest_v2.json"));
        }

        public void GetFile(string username, string instanceName)
        {
            TcpClient client = GoConn.GetBackIp();
            NetworkStream clientStream = client.GetStream();
            clientStream.writeString(username);
            clientStream.writeString(instanceName);
            this.instanceName = instanceName;

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
                if (instanceName != "")
                {
                    ZipFile.ExtractToDirectory(tempPath, Path.Combine(defaultGamePath, instanceName));
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
    }
}
