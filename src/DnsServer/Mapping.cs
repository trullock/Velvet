namespace DnsServer
{
	internal sealed class Mapping
	{
		public readonly string Destination;
		public readonly string Pattern;

		public Mapping(string destination, string pattern)
		{
			this.Destination = destination;
			this.Pattern = pattern;
		}
	}
}