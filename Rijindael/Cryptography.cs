using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Rijindael
{
    public class Cryptography
    {
        public string Encrypt(string plainText, string password,string ivKey)
        {
            byte[] Key = GetBytes(password);
            byte[] IV = GetBytes(ivKey);
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plain Text Is Empty");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Password Is Empty");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV Key Is Empty");
            byte[] encrypted;
            // Create an Rijndael object 
            // with the specified key and IV. 
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            //return encrypted;
            return GetString(encrypted);

        }

        public string Decrypt(string encryptText, string password, string ivkey)
        {
            byte[] cipherText = GetBytes(encryptText);
            byte[] Key = GetBytes(password);
            byte[] IV = GetBytes(ivkey);
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("Encrypt Text Is Empty");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Password Is Empty");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Iv Key Is Empty");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Rijndael object 
            // with the specified key and IV. 
             
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.KeySize = 256;
                rijAlg.BlockSize = 128;
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
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
    }
}
