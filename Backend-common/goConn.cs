using BackendCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BackendCommon
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

        private NetworkStream initConnection()
        {
            var goServ = new TcpClient(ip, port);
            return goServ.GetStream();
        }

        public bool WriteAcc(string data)
        {
            goStream = initConnection();
            goStream.writeString("sacc");
            goStream.writeString(data);
            return goStream.readInt() == 1;
        }

        public TcpClient GetBackIp()
        {
            goStream = initConnection();
            goStream.writeString("getbackip");
            var bip = goStream.readString();
            var bport = goStream.readInt();
            return new TcpClient(bip, bport);
        }

        public string[,] GetServers()
        {
            goStream = initConnection();
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
            goStream = initConnection();
            goStream.writeString("verifyuser");
            goStream.writeString(name);
            goStream.writeString(password);
            var id = goStream.readInt();
            return goStream.readInt() == 1;
        }

        public string FileCR(string name)
        {
            goStream = initConnection();
            goStream.writeString("filecr");
            goStream.writeString(name);
            var key = goStream.readString();
            var id = goStream.readInt();
        }
    }
}
