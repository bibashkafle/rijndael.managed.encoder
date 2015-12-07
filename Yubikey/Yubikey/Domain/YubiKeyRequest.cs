
namespace Yubikey.Domain
{
	public class YubiKeyRequest
	{
		private string challenge = "";
		private string response = "";

		public int AgentId { get; set; }
		public int AgentLocId { get; set; }
		public int UserId { get; set; }
		public int TimezoneId { get; set; }
		public int AppId { get; set; }
		public int AppObjectId { get; set; }
		public int ComputerId { get; set; }
		public byte[] uid { get; set; }
		public int ValueId { get; set; }

		public string Challenge
		{
			get
			{
				return this.challenge;
			}
			set
			{
				this.challenge = value;
			}
		}

		public string Response
		{
			get
			{
				return this.response;
			}
			set
			{
				this.response = value;
			}
		}
	}
}
