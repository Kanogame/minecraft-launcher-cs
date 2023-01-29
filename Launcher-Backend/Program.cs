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

namespace Launcher_Backend
{
    internal class Program
    {
        Logging log;
        ConfigManager cfgManager;
        private int Port = 23032;

        static void Main(string[] args)
        {
            var p = new Program();
            p.main();
        }

        void main()
        {
            log = new Logging();
            log.OpenFileWrite();
            cfgManager = new ConfigManager(log, "./config.txt");
            var acceptClientThreads = new Thread(AcceptClients);
            acceptClientThreads.IsBackground = true;
            acceptClientThreads.Start();
            Console.ReadKey();
        }

        private void AcceptClients()
        {
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                log.FileWrite($"sending files to {client.Client.RemoteEndPoint} from port {Port}");
                Clienthandle(client);
            }
        }

        private void Clienthandle(TcpClient client)
        {
            var ns = client.GetStream();
            var clientName = ns.readString();
            log.FileWrite("connecting to user with name:" + clientName);
            string serverRequest = ns.readString() + ".zip";
            if (cfgManager.checkFile(serverRequest))
            {
                ns.writeBool(true);
                Console.WriteLine(clientName + ": " + serverRequest);
                Console.WriteLine(client.Client.RemoteEndPoint.ToString());
                log.FileWrite("sending WS4 package");
                log.FileWrite("reading from " + cfgManager.getFilePath(serverRequest));
                SendPackage(ns, cfgManager.getFilePath(serverRequest));
            }
            else
            {
                ns.writeBool(false);
            }
        }

        private void SendPackage(NetworkStream ns, string path)
        {
            ns.WriteByte((byte)TransferCommands.Send);
            ns.writeLong(new FileInfo(path).Length);
            ns.writeString(Path.GetFileName(path));
            bool confirmation = ns.readBool();
            if (confirmation)
            {
                using (Stream source = File.OpenRead(path))
                {
                    byte[] buffer = new byte[2048];
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ns.Write(buffer, 0, bytesRead);
                    }
                }
            }
            log.FileWrite("package sucsessfully sended");
        }
    }
}