using System;
using Yubikey.API.Interfaces;
namespace Yubikey.Domain
{
    public class SecurityLock
    {
        private readonly IYubiKeyAuthentication authentication;
        private readonly IYubikeyEncryptor yubikeyEncryptor;
        public bool IsUnlocked { get; internal set; }
        public int ValueId { get; internal set; }
        public int ObjectId { get; internal set; }

        public SecurityLock(IYubiKeyAuthentication authentication, IYubikeyEncryptor yubikeyEncryptor, SecurityInfo info, string challenge, string response)
        {
            this.authentication = authentication;
            this.yubikeyEncryptor = yubikeyEncryptor;
            var auth = AuthenticateYubiKey(info, challenge, response);
            IsUnlocked = auth.IsValid;
            ValueId = auth.ValueId;
            ObjectId = auth.ObjectId;
        }

        private YubiKeyResponse AuthenticateYubiKey(SecurityInfo info, string challenge, string response)
        {
            var yubiKeyRequest = new YubiKeyRequest
            {
                UserId = info.UserId,
                AgentId = info.AgentId,
                AgentLocId = info.LocationId,
                TimezoneId = 0, //we don't use this
                AppId = info.AppId,
                AppObjectId = 0,
                ComputerId = info.LoginId,
                Challenge = challenge,
                Response = response,                
            };

            return AuthenticateYubiKey(yubiKeyRequest);
        }

        private YubiKeyResponse AuthenticateYubiKey(YubiKeyRequest request)
        {
            var aesResponse = this.authentication.LoadAESKey(request);
            request.ValueId = aesResponse.ValueId;
            request.uid = this.Decrypt(request.Challenge, request.Response, aesResponse.AES);

            return request.uid == null ? YubiKeyResponse.Invalid : this.authentication.LoadYubiKeyUID(request);
        }

        private byte[] Decrypt(string challenge, string response, char[] aes)
        {
            return this.yubikeyEncryptor.GetUID(response, challenge, aes);
        }
    }
}