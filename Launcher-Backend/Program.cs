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

        static void Main(string[] args)
        {
            var p = new Program();
            p.main();
        }

        void main()
        {
            var acceptClientThreads = new Thread(AcceptClients);
            acceptClientThreads.IsBackground = true;
            acceptClientThreads.Start();
            Console.ReadKey();
        }

        private void AcceptClients()
        {
            var listener = new TcpListener(IPAddress.Any, 23032);
            listener.Start();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                Clienthandle(client);
                Console.WriteLine($"sending testing to {client.Client.RemoteEndPoint} with port 23032");
            }
        }

        private void Clienthandle(TcpClient client)
        {
            var ns = client.GetStream();
            var clientGuidBytes = ns.read(16);
            Console.WriteLine("connecting to user with guid:" + clientGuidBytes.ToString());
            string serverRequest = ns.readString();
            Console.WriteLine(clientGuidBytes.ToString() + ": " + serverRequest);
            Console.WriteLine(client.Client.RemoteEndPoint.ToString());
            if (serverRequest == "WS4-fabric")
            {
                Console.WriteLine("sending WS4 package");
                SendPackage(ns, "C:\\Users\\OneSmiLe\\Desktop\\Temp\\Rectangle 2.3.png");
            }
            else
            {
                //TODO
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
                    int i = 0;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ns.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}