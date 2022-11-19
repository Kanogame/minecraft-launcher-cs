using BackendCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    internal class goConn
    {
        private string ip;
        private int port;

        private NetworkStream goStream;

        public goConn(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void Listen()
        {
            var goServ = new TcpClient(ip, port);
            goStream = goServ.GetStream();
            goStream.writeString("getlatestdata");
            //ReadGo(goStream);
        }

        public TcpClient GetBackIp()
        {
            var goServ = new TcpClient(ip, port);
            goStream = goServ.GetStream();
            goStream.writeString("getbackip");
            var bip = goStream.readString();
            MessageBox.Show(bip);
            var bport = goStream.readInt();
            MessageBox.Show(bport.ToString());
            return new TcpClient(bip, bport);
        }

        private void ReadGo(NetworkStream goStream)
        {
            MessageBox.Show(goStream.readString());
        }
    }
}
