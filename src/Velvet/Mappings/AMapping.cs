using System.Net;
using ARSoft.Tools.Net.Dns;

namespace Velvet.Mappings
{
	/// <summary>
	/// Represents a mapping for an A record
	/// </summary>
	internal sealed class AMapping : Mapping
	{
		readonly IPAddress ipAddress;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern">The name pattern to match with</param>
		/// <param name="ipAddress">The answer IP</param>
		public AMapping(string pattern, IPAddress ipAddress) : base(pattern)
		{
			this.ipAddress = ipAddress;
		}

		public override DnsRecordBase Answer(DnsQuestion question)
		{
			if (pattern.IsMatch(question.Name))
				return new ARecord(question.Name, 1, this.ipAddress);

			return null;
		}
	}
}