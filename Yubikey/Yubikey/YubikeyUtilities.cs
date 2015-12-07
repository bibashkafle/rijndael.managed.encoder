using Yubikey.Domain;
namespace Yubikey
{
    public class YubikeyUtilities
    {
        public string GetChallenge()
        {
            return (new YubiKeyEncryptor().CreateRandomChallenge());
        }

        public SecurityLock Authenticate(string user, string agent, string challenge, string response)
        {
            var secInfo = new SecurityInfo { AgentId = int.Parse(agent), UserId = 12345 };
            var _yubiKeyEncryptorObj = new YubiKeyEncryptor();
            var _yubiKeyAuthenticationObj = new YubiKeyAuthentication();
            var _securityLock = (new SecurityLock(_yubiKeyAuthenticationObj, _yubiKeyEncryptorObj, secInfo, challenge, response));
            return _securityLock;
        }

        public bool AllowYubikey(string user, string agent, string branch)
        {
            return false;
        }

        public object GetEvents(string user, string agent, string branch)
        {
            return false;
        }
    }
}
