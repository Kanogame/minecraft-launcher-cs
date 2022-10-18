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
        NetworkStream CurrentClient;

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

        void AcceptClients()
        {
            var listener = new TcpListener(IPAddress.Any, 23032);
            listener.Start();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                Clienthandle(client);
                Console.WriteLine($"sending testing to {client.Client.AddressFamily} with port 23032");
            }
        }

        void Clienthandle(TcpClient client)
        {
            CurrentClient = client.GetStream();
            CurrentClient.writeString("testing");
        }
    }
}
