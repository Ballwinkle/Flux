using System;
using System.Text;
using System.Security.Cryptography;

namespace Flux
{
    public static class Hashing
    {
        public static string md5encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            MD5CryptoServiceProvider md5hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = md5hasher.ComputeHash(encoder.GetBytes(phrase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha1encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA1CryptoServiceProvider sha1hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = sha1hasher.ComputeHash(encoder.GetBytes(phrase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha256encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha384encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA384Managed sha384hasher = new SHA384Managed();
            byte[] hashedDataBytes = sha384hasher.ComputeHash(encoder.GetBytes(phrase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string sha512encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA512Managed sha512hasher = new SHA512Managed();
            byte[] hashedDataBytes = sha512hasher.ComputeHash(encoder.GetBytes(phrase));
            return byteArrayToString(hashedDataBytes);
        }
        public static string byteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }
    }
}
