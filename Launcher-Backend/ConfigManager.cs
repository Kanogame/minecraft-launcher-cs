using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_Backend
{
    internal class ConfigManager
    {
        Logging log;
        private string configPath;
        private string filepath;

        public ConfigManager(Logging log, string configPath) 
        {
            this.log = log;
            this.configPath = configPath;
            readConfig();
        }

        private void readConfig() 
        {
            using (StreamReader sr = new StreamReader(configPath))
            {
                filepath = getConfigVaule(sr.ReadLine());
            }
        }

        public bool checkFile(string fileName)
        {
            return File.Exists(Path.Combine(filepath, fileName));
        }

        public string getFilePath(string filename)
        {
            return Path.Combine(filepath, filename);
        }

        private string getConfigVaule(string configLine)
        {
            var cc = configLine.ToCharArray();
            string res = "";

            int i = 0;
            while (cc[i] != '=')
            {
                i++;
            }
            i++;
            while (i < cc.Length)
            {
                res += cc[i];
                i++;
            }
            return res;
        }
    }
}
