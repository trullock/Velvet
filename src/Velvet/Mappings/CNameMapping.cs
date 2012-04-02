using ARSoft.Tools.Net.Dns;

namespace Velvet.Mappings
{
	/// <summary>
	/// Represents a mapping for an CName record
	/// </summary>
	internal sealed class CNameMapping : Mapping
	{
		readonly string canonicalName;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern">The name pattern to match with</param>
		/// <param name="canonicalName">the answer canonical name</param>
		public CNameMapping(string pattern, string canonicalName) : base(pattern)
		{
			this.canonicalName = canonicalName;
		}

		public override DnsRecordBase Answer(DnsQuestion question)
		{
			if (pattern.IsMatch(question.Name))
				return new CNameRecord(question.Name, 1, this.canonicalName);

			return null;
		}
	}
}