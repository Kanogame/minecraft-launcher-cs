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
            goStream.writeString("Hello world");
            //ReadGo(goStream);
        }

        private void ReadGo(NetworkStream goStream)
        {
            MessageBox.Show(goStream.readString());
        }
    }
}
