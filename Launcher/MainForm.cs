﻿using System;
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
                Console.WriteLine("ping");
            }
            else if (command == TransferCommands.Send)
            {
                ns.writeBool(true);
                long length = ns.readLong();
                string name = ns.readString();
                string path = Path.Combine("C://Users//OneSmiLe//Pictures", name);
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
            }
            MessageBox.Show("DONE!");
        }
    }
}