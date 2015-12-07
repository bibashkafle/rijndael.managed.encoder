using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yubikey.API.Interfaces
{
    public interface ISymmetricEncoder
    {
        byte[] EncryptionKey
        {
            get;
            set;
        }

        byte[] InitializationVector
        {
            get;
            set;
        }

        byte[] Encode(byte[] dataToEncrypt);
        byte[] Decode(byte[] encryptedData);
        void GenerateKey();
    }
}
