using Yubikey.Domain;
namespace Yubikey.API.Interfaces
{
	public interface IYubiKeyAuthentication
	{
		YubiKeyResponse LoadYubiKeyUID(YubiKeyRequest request);
		AESResponse LoadAESKey(YubiKeyRequest request);
	}
}
