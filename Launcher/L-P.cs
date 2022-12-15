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
            RC4 encoder = new RC4(key);
            byte[] val = Encoding.ASCII.GetBytes(value);
            byte[] result = encoder.Encode(val, val.Length);
            return ParseFromStr(Encoding.Default.GetString(result));
        }

        public string Decode(string value, string key)
        {
            RC4 decoder = new RC4(key);
            byte[] val = Encoding.ASCII.GetBytes(value);
            byte[] result = decoder.Decode(val, val.Length);
            return ParseToStr(Encoding.Default.GetString(result));
        }

        public string ParseFromStr(string value)
        {
            var res = "";
            var cc = value.ToCharArray();

            for (int i = 0; i < cc.Length; i++)
            {
                if ((i + 1) % 5 == 0)
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
    }

    public class RC4
    {
        byte[] S = new byte[256];
        int x = 0;
        int y = 0;

        private void init(string value)
        {
            var key = ASCIIEncoding.ASCII.GetBytes(value);
            int keyLength = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + key[i % keyLength]) % 256;
                S.Swap(i, j);
            }
        }

        public RC4(string key)
        {
            init(key);
        }

        private byte keyItem()
        {
            x = (x + 1) % 256;
            y = (y + S[x]) % 256;

            S.Swap(x, y);

            return S[(S[x] + S[y]) % 256];
        }

        public byte[] Encode(byte[] dataB, int size)
        {
            byte[] data = dataB.Take(size).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int m = 0; m < data.Length; m++)
            {
                cipher[m] = (byte)(data[m] ^ keyItem());
            }

            return cipher;
        }

        public byte[] Decode(byte[] dataB, int size)
        {
            return Encode(dataB, size);
        }
    }

    static class SwapExt
    {
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
}
