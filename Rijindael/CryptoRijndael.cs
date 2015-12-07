using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Rijindael
{
    class CryptoRijndael
    {
        private RijndaelManaged _rijndaelManaged;

        private const int _maximumEncryptBuffer = 256000;

        public string Encrypt(string plainText, string password, string ivKey)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(password,ivKey);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt)){
                swEncrypt.Write(plainText);
            }
           return GetString(msEncrypt.ToArray());
        }

        public string Decrypt(string cipherText, string password, string ivkey)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            string text;

            var aesAlg = NewRijndaelManaged(password, ivkey);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = GetBytes(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;

            //byte[] decryptedData = null;
            //byte[] decryptedBuffer = new byte[_maximumEncryptBuffer];

            //ICryptoTransform decryptor = _rijndaelManaged.CreateDecryptor();
            //using (MemoryStream memoryStream = new MemoryStream(GetBytes(cipherText)))
            //{
            //    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            //    {
            //        int bytesRead = cryptoStream.Read(decryptedBuffer, 0, 1024);

            //        // prepare the return byte buffer.  			
            //        decryptedData = new byte[bytesRead];
            //        for (int i = 0; i < bytesRead; i++)
            //        {
            //            decryptedData[i] = decryptedBuffer[i];
            //        }
            //        cryptoStream.Close();
            //    }
            //    memoryStream.Close();
            //}
            //return GetString(decryptedData);
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private static RijndaelManaged NewRijndaelManaged(string pssword,  string ivkey)
        {
            if (pssword == null) throw new ArgumentNullException("Password is empty");
            var psswordBytes = Encoding.ASCII.GetBytes(pssword);
            var key = new Rfc2898DeriveBytes(ivkey, psswordBytes);

            var aesAlg = new RijndaelManaged(); //{ KeySize = 256, BlockSize = 128 };
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
            aesAlg.Padding = PaddingMode.Zeros;

            aesAlg.Mode = CipherMode.CBC;

            return aesAlg;
        }
    }
}