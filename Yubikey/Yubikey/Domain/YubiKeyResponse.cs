
namespace Yubikey.Domain
{
	public class YubiKeyResponse
	{
		public int ValueId { get; internal set; }
		public int ObjectId { get; internal set; }
		public bool IsValid { get; set; }


		static public YubiKeyResponse Invalid
		{
			get
			{
				return new YubiKeyResponse(0, 0, false);
			}
		}

		public YubiKeyResponse(int valueId, int objectId, bool isValid)
		{
			ValueId = valueId;
			ObjectId = objectId;
			IsValid = isValid;
		}
	}
}
