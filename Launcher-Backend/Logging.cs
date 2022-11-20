using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_Backend
{
    internal class Logging
    {
        private string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LatestLog.txt");

        public void OpenFileWrite()
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"Program Started, DateTime: {DateTime.Now}");
            }
        }

        public void FileWrite(string text) 
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine($"{GetTimeNow()}: {text}");
            }
        }

        private string GetTimeNow()
        {
            return $"[{DateTime.Now}]";
        }
    }
}
