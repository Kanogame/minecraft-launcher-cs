using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    public class L_P
    {
        int id;
        string passwd;

        public L_P(int id, string passwd)
        {
            this.id = id;
            this.passwd = passwd;
        }

        public string Encode(string value, string key)
        {
            byte[] result = Decrypt(Encoding.Unicode.GetBytes(key), Encoding.Unicode.GetBytes(value));
            return ParseFromStr(Convert.ToBase64String(result));
        }

        public string Decode(string value, string key)
        {
            byte[] result = Encrypt(Encoding.Unicode.GetBytes(key), Convert.FromBase64String(ParseToStr(value)));
            return Encoding.Unicode.GetString(result);
        }

        public string ParseFromStr(string value)
        {
            var res = "";
            var cc = value.ToCharArray();

            for (int i = 0; i < cc.Length; i++)
            {
                if (i % 5 == 0 && i != 0)
                {
                    res += "-";
                }
                res += cc[i];
            }
            return res;
        }

        public string ParseToStr(string value)
        {
            var cc = value.ToCharArray();
            var res = "";
            for (int i = 0; i < cc.Length; i++)
            {
                if (cc[i] != '-')
                {
                    res += cc[i];
                }
            }
            return res;
        }

        public static byte[] Encrypt(byte[] pwd, byte[] data)
        {
            int a, i, j, k, tmp;
            int[] key, box;
            byte[] cipher;

            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = pwd[i % pwd.Length];
                box[i] = i;
            }
            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < data.Length; i++)
            {
                a++;
                a %= 256;
                j += box[a];
                j %= 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(data[i] ^ k);
            }
            return cipher;
        }

        public static byte[] Decrypt(byte[] pwd, byte[] data)
        {
            return Encrypt(pwd, data);
        }
    }
}
