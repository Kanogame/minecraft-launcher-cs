using BackendCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BackendCommon
{
    public class goConn
    {
        private string ip;
        private int port;

        private string token;
        private string tokenPWD;

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
            if (goStream.readInt() == 1)
            {
                token = goStream.readString(); //yes
                tokenPWD = goStream.readString(); //yes
                MessageBox.Show(token, tokenPWD);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool VerifyToken(string token, string tokenPWD, NetworkStream goStream)
        {
            goStream.writeString(token);
            goStream.writeString(GetHash(tokenPWD));
            return goStream.readInt() == 1;
        }

        public string[] FileCR(string name, string password)
        {
            goStream = initConnection();
            goStream.writeString("filecr");
            if (VerifyToken(token, tokenPWD, goStream))
            {
                goStream.writeString(name);
                string key = goStream.readString();
                int id = goStream.readInt();
                MessageBox.Show(key);
                MessageBox.Show(id.ToString());
                return new string[2]
                {
                    id.ToString(), key
                };
            }
            MessageBox.Show("сессия устарела");
            return null;
        }

        public bool WriteData(string data)
        {
            goStream = initConnection();
            goStream.writeString("decrypt");
            goStream.writeString(data);
            return goStream.readInt() == 1;
        }

        private string GetHash(string value)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(value));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public void ImageHandler(string path)
        {
            goStream = initConnection();
            goStream.writeString("images");
            var imageCount = goStream.readInt();
            goStream.writeInt(1);
            var filename = goStream.readString();
            goStream.readFile(Path.Combine(path, filename));
        }
    }
}
