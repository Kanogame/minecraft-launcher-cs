using BackendCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_WPF
{
    public class AccManager
    {
        string savePath;
        goConn goConn;

        public AccManager(string savePath, goConn goConn) 
        {
            this.savePath = savePath;
            this.goConn = goConn;
        }

        public bool sendAcc()
        {
            string data;
            using (var reader = new StreamReader(savePath))
            {
                data = reader.ReadLine();
            }
            return goConn.WriteAcc(data);
        }
    }
}
