using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Backend_common
{
    public class FIleOperations
    {
        public void UnZip(string filePath, string UnpackPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(filePath, UnpackPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("при распаковке что-то пошло не так, " + ex.Message);
            }
        }

        public void CreateTextFile(string filePath, IEnumerable<string> text)
        {
            var data = text.ToArray();
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var item in data)
                {
                    sw.WriteLine(item);
                }
            }
        }
        public string Sha256(string path)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] readText = File.ReadAllBytes(path);
                return String.Concat(hash.ComputeHash(readText).Select(item => item.ToString("x2")));
            }
        }
        public string GetHash(string value)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(value));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
