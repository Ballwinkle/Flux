using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Flux
{
    public class Common
    {
        public static byte[] StringToByteArray(string str)
        {
            UTF8Encoding UTF8 = new UTF8Encoding();
            Console.WriteLine("Str=>Array: " + str);
            return UTF8.GetBytes(str);
        }

        public static string ByteArrayToString(byte[] str)
        {
            UTF8Encoding UTF8 = new UTF8Encoding();
            Console.WriteLine(UTF8.GetString(str));
            return UTF8.GetString(str);
        }

        public static readonly byte Version = 0x01;
    }
}
