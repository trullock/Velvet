using System.Text.RegularExpressions;
using ARSoft.Tools.Net.Dns;

namespace Velvet.Mappings
{
	public abstract class Mapping
	{
		protected readonly Regex pattern;

		protected Mapping(string pattern)
		{
			this.pattern = new Regex(pattern, RegexOptions.IgnoreCase);
		}

		public abstract DnsRecordBase Answer(DnsQuestion question);
	}
}