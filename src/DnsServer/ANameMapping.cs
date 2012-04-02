using System.Net;
using ARSoft.Tools.Net.Dns;

namespace Velvet
{
	public sealed class ANameMapping : Mapping
	{
		readonly IPAddress ipAddress;

		public ANameMapping(string pattern, IPAddress ipAddress) : base(pattern)
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