using System;

namespace Yubikey.Domain
{
	public class SecurityInfo
	{
		public int AgentType { get; set; }
		public int UserId { get; set; }
		public int AgentId { get; set; }
		public int LocationId { get; set; }
		public int TimeZoneId { get; set; }
		public DateTime Date { get; set; }
		public int AppId { get; set; }
		public int LoginId { get; set; }
		public int CountryId { get; set; }
		public int StateId { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
	}

}
