using BackendCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Launcher
{
    public class goConn
    {
        private string ip;
        private int port;

        private NetworkStream goStream;

        public goConn(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public TcpClient GetBackIp()
        {
            var goServ = new TcpClient(ip, port);
            goStream = goServ.GetStream();
            goStream.writeString("getbackip");
            var bip = goStream.readString();
            var bport = goStream.readInt();
            return new TcpClient(bip, bport);
        }

        public string[,] GetServers()
        {
            var goServ = new TcpClient(ip, port);
            goStream = goServ.GetStream();
            goStream.writeString("getserverlist");
            int serverCount = goStream.readInt();
            int stringCount = goStream.readInt();
            string[,] servers = new string[serverCount, stringCount];
            for (int i = 0; i < serverCount; i++)
            {
                for (int j = 0; j < stringCount; j++)
                {
                    servers[i, j] = goStream.readString();
                }
            }
            return servers;
        }

        public bool VerifyUser(string name, string password)
        {
            var goServ = new TcpClient(ip, port);
            goStream = goServ.GetStream();
            goStream.writeString("verifyuser");
            goStream.writeString(name);
            goStream.writeString(password);
            return goStream.readInt() == 1;
        }
    }
}
