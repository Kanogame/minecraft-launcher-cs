using Backend_common;
using BackendCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        private FIleOperations fileOperation;

        public goConn(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            fileOperation = new FIleOperations();
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
            goStream.Close();
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
            goStream.Close();
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
                goStream.Close();
                return true;
            }
            else
            {
                goStream.Close();
                return false;
            }
        }

        private bool VerifyToken(string token, string tokenPWD, NetworkStream goStream)
        {
            goStream.writeString(token);
            goStream.writeString(fileOperation.GetHash(tokenPWD));
            goStream.Close();
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
                return new string[2]
                {
                    id.ToString(), key
                };
            }
            MessageBox.Show("сессия устарела");
            goStream.Close();
            return null;
        }

        public string GetMCname()
        {
            goStream = initConnection();
            goStream.writeString("getmcname");
            if (VerifyToken(token, tokenPWD, goStream))
            {
                return goStream.readString();
            }
            MessageBox.Show("сессия устарела");
            goStream.Close();
            return null;
        }

        public bool WriteData(string data)
        {
            goStream = initConnection();
            goStream.writeString("decrypt");
            goStream.writeString(data);
            if (goStream.readInt() == 1)
            {
                token = goStream.readString();
                tokenPWD = goStream.readString();
                goStream.Close();
                return true;
            }
            goStream.Close();
            return false;
        }

        public string ImageHandler(string path)
        {
            var tempPath = Path.Combine(path, "temp");
            goStream = initConnection();
            goStream.writeString("images");
            goStream.readFile(tempPath);
            var UnpackPath = Path.Combine(tempPath, "images");
            Directory.CreateDirectory(UnpackPath);
            var hash = fileOperation.Sha256(Path.Combine(tempPath, "images.zip"));
            fileOperation.UnZip(Path.Combine(tempPath, "images.zip"), UnpackPath);
            fileOperation.CreateTextFile(Path.Combine(path, "data", "imagesHash.txt"), new string[1] { hash });
            goStream.Close();
            return UnpackPath;
        }

        public bool VeryfyImageHash(string hash)
        {
            goStream = initConnection();
            goStream.writeString("imageshash");
            goStream.writeString(hash);
            if (goStream.readInt() == 1)
            {
                goStream.Close();
                return true;
            }
            goStream.Close();
            return false;

        }
    }
}
