using BackendCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_Backend
{
    public class computer
    {
        Guid guid;
        string clientId;
        TcpClient toServer;
        TcpClient toClient;
        bool TransferPortWorks;
        int portForFileTransfer;

        public computer(Guid guid, String clientId)
        {
            this.guid = guid;
            this.clientId = clientId;
        }

        public string getClientId()
        {
            return clientId;
        }

        public int GetPortForFileTransfer()
        {
            return portForFileTransfer;
        }

        public bool getHasAccess()
        {
            return TransferPortWorks;
        }

        public Guid getGuid()
        {
            return guid;
        }

        public IPAddress getIPAddress()
        {
            return (toClient.Client.RemoteEndPoint as IPEndPoint)?.Address;
        }


        public void SetChannelToServer(TcpClient chan)
        {
            this.toServer = chan;
        }

        public void SetChannelToClient(TcpClient chan)
        {
            this.toClient = chan;
        }

        public void SetPortForFileTranfer(int port)
        {
            this.portForFileTransfer = port;
        }

        public bool BothChannelIsExist()
        {
            return toServer != null && toClient != null;
        }

        public void CheckTransferPort()
        {
            var cli = new TcpClient();
            var ep = (IPEndPoint)toServer.Client.RemoteEndPoint;
            try
            {
                cli.Connect(ep.Address, portForFileTransfer);
                var ns = cli.GetStream();
                ns.WriteByte((byte)TransferCommands.Ping);
                Guid retrieverGuid = new Guid(ns.read(16));
                if (retrieverGuid == guid)
                {
                    TransferPortWorks = true;
                }
                else
                {
                    TransferPortWorks = false;
                }
                cli.Close();
            }
            catch (Exception)
            {
                TransferPortWorks = false;
            }
        }

        public NetworkStream getStreamToClient()
        {
            if (toClient == null)
            {
                return null;
            }
            return toClient.GetStream();
        }
    }
}
