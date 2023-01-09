using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BackendCommon
{
    public static class rio
    {
        public static void writeGuid(this NetworkStream stream, Guid guid)
        {
            stream.write(guid.ToByteArray());
        }

        public static Guid readGuid(this NetworkStream stream)
        {
            return new Guid(stream.read(16));
        }

        public static void writeIPAddress(this NetworkStream stream, IPAddress iPAddress)
        {
            var bytes = iPAddress.GetAddressBytes();
            stream.writeInt(bytes.Length);
            stream.write(bytes);
        }

        public static IPAddress readIpAddress(this NetworkStream stream)
        {
            var addrLen = stream.readInt();
            var addrBytes = stream.read(addrLen);
            return new IPAddress(addrBytes);
        }

        public static void writeBool(this NetworkStream stream, bool val)
        {
            stream.WriteByte(val ? (byte)1 : (byte)0);
        }

        public static bool readBool(this NetworkStream stream)
        {
            return stream.ReadByte() == 1;
        }

        public static void write(this NetworkStream stream, byte[] bytes)
        {
             stream.Write(bytes, 0, bytes.Length);
        }

        public static void writeInt(this NetworkStream stream, int val)
        {
            byte[] byteArray = BitConverter.GetBytes(val);
             stream.Write(byteArray, 0, byteArray.Length);
        }

        public static int readInt(this NetworkStream stream)
        {
             byte[] byteArray = stream.read(4);
             return BitConverter.ToInt32(byteArray, 0);
        }

        public static void writeLong(this NetworkStream stream, long val)
        {
            byte[] byteArray = BitConverter.GetBytes(val);
             stream.Write(byteArray, 0, byteArray.Length);
        }

        public static long readLong(this NetworkStream stream)
        {
             byte[] byteArray = stream.read(8);
             return BitConverter.ToInt64(byteArray, 0);
        }

        public static void writeString(this NetworkStream stream, string val)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(val);
            stream.writeInt(bytes.Length);
            stream.write(bytes);
        }

        public static string readString(this NetworkStream stream)
        {
            int len = stream.readInt();
            var bytes = stream.read(len);
            return UTF8Encoding.UTF8.GetString(bytes);
        }

        public static void readFile(this NetworkStream stream, string path)
        {
            var len = stream.readLong();
            string name = stream.readString();
            using (Stream f = File.OpenWrite(path))
            {
                f.Write(bytes, 0, bytes.Length);
            }
        }

        public static byte[] read(this NetworkStream stream, int bytesCount, Action<int> progress = null)
        {
            int left = bytesCount;
            int ind = 0;
            byte[] buffer = new byte[bytesCount];
            while (left > 0)
            {
                int cnt = stream.Read(buffer, ind, left);
                left -= cnt;
                ind += cnt;
                if (progress != null)
                {
                    progress(left);
                }
            }
            return buffer;
        }
    }
}
