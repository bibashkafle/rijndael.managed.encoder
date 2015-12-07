using System.IO;
using System.Security.Cryptography;
using Yubikey.API.Interfaces;
namespace Yubikey.Domain
{
    public class RinjndaelEncoder : ISymmetricEncoder
    {
        private const int _maximumEncryptBuffer = 256000;

        private RijndaelManaged _rijndaelManaged;

        public RinjndaelEncoder()
        {
            _rijndaelManaged = new RijndaelManaged();
        }

        public RinjndaelEncoder(byte[] encryptionKey, byte[] initializationVector)
        {
            _rijndaelManaged = new RijndaelManaged();
            EncryptionKey = encryptionKey;
            InitializationVector = initializationVector;
        }

        public byte[] InitializationVector
        {
            get
            {
                return _rijndaelManaged.IV;
            }
            set
            {
                _rijndaelManaged.IV = value;
            }
        }

        public byte[] EncryptionKey
        {
            get
            {
                return _rijndaelManaged.Key;
            }
            set
            {
                _rijndaelManaged.Key = value;
            }
        }

        public byte[] Encode(byte[] dataToEncrypt)
        {
            byte[] encryptedData = null;
            ICryptoTransform encryptor = _rijndaelManaged.CreateEncryptor();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    encryptedData = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return encryptedData;
        }

        public byte[] Decode(byte[] encryptedData)
        {
            byte[] decryptedData = null;
            byte[] decryptedBuffer = new byte[_maximumEncryptBuffer];

            ICryptoTransform decryptor = _rijndaelManaged.CreateDecryptor();
            using (MemoryStream memoryStream = new MemoryStream(encryptedData))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    int bytesRead = cryptoStream.Read(decryptedBuffer, 0, 1024);

                    // prepare the return byte buffer.  			
                    decryptedData = new byte[bytesRead];
                    for (int i = 0; i < bytesRead; i++)
                    {
                        decryptedData[i] = decryptedBuffer[i];
                    }
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }

            return decryptedData;
        }

        /// <summary>
        /// Create a new encrption key for the encoder
        /// </summary>
        /// <returns></returns>
        public void GenerateKey()
        {
            _rijndaelManaged.GenerateKey();
        }
    }
}
