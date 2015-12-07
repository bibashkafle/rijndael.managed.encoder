
namespace Yubikey.API.Interfaces
{
    public interface IYubikeyEncryptor
    {
        byte[] GetUID(string otp, string challenge, char[] AESKey);
    }
}
