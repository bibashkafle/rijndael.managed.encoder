using System;
using System.Configuration;
using Yubikey.API.Interfaces;
using Yubikey.Domain;
namespace Yubikey.Domain
{
    public class YubiKeyEncryptor : IYubikeyEncryptor
    {
        private readonly RinjndaelEncoder encoder;
        private const string ChallengePlaceHolder = "keyboard mode";

        #region SecretStuff - Don't Look      

        public YubiKeyEncryptor()
        {
                        var initializationVector = this.StringToByteArray("0C790D11A2C619DB07C19F526CC505F5");
                        var key = this.StringToByteArray("4801CB755CB29BEABB7BFF0C1161A52D");
                        this.encoder = new RinjndaelEncoder(key, initializationVector);
        }
        #endregion

        public DecryptionToken DecryptYubiKey(string otp, string challenge, byte[] key)
        {
            byte[] xorBytes;
            DecryptionToken token;

            try
            {
                xorBytes = Challenge.Decode(challenge);
                token = OneTimePassword.Parse(otp, key);
            }
            catch (Exception)
            {
                return null;
            }

            var startingUid = token.getUid();

            for (var i = 0; i < startingUid.Length; i++)
            {
                startingUid[i] = Convert.ToByte(xorBytes[i] ^ startingUid[i]);
            }

            token.setUid(startingUid);

            return token;
        }

        public DecryptionToken DecryptYubiKey(string otp, byte[] key)
        {
            DecryptionToken token;

            try
            {
                token = OneTimePassword.Parse(otp, key);
            }
            catch (Exception)
            {
                return null;
            }

            var startingUid = token.getUid();

            for (var i = 0; i < startingUid.Length; i++)
            {
                startingUid[i] = Convert.ToByte(startingUid[i]);
            }

            token.setUid(startingUid);

            return token;
        }

        public string CreateRandomChallenge()
        {
            return Challenge.Encode(Guid.NewGuid().ToByteArray());
        }

        public string CreateKeyboardModePlaceHolder()
        {
            return ChallengePlaceHolder;
        }

        public byte[] DecodeAESKey(char[] AESKey)
        {
            try
            {
                return this.encoder.Decode(BinaryHelper.GetLowOrderBytes(AESKey));
            }
            catch
            {
                return null;
            }
        }

        public byte[] GetUID(string otp, string challenge, char[] AESKey)
        {
            DecryptionToken token;

            if (challenge == ChallengePlaceHolder)
            {
                token = this.DecryptYubiKey(otp, this.DecodeAESKey(AESKey));
            }
            else
            {
                token = this.DecryptYubiKey(otp, challenge, this.DecodeAESKey(AESKey));
            }

            return token == null ? null : this.encoder.Encode(token.getUid());
        }

        private byte[] StringToByteArray(String hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
