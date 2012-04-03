using ARSoft.Tools.Net.Dns;
using NUnit.Framework;

namespace Velvet.Tests.HostsWatcherSpecs
{
	public sealed class ANameParsing : Spec
	{
		protected override void Given()
		{
			hostsFile = @"
127.0.0.1 A *.dev
	192.168.0.1		A	     *.foo
 noise that wont parse
";
		}

		[Then]
		public void ShouldUpdateThreeMappings()
		{
			Assert.AreEqual(2, this.mappings.Length);
		}

		[Then]
		public void ShouldParseAMapping()
		{
			// TODO: what is the correct record type and class?
			var answer = this.mappings[0].Answer(new DnsQuestion("local.dev", RecordType.A, RecordClass.Any)) as ARecord;
			Assert.AreEqual("127.0.0.1", answer.Address.ToString());
		}

		[Then]
		public void ShouldParseAMappingWithWhitespace()
		{
			// TODO: what is the correct record type and class?
			var answer = this.mappings[1].Answer(new DnsQuestion("bar.foo", RecordType.A, RecordClass.Any)) as ARecord;
			Assert.AreEqual("192.168.0.1", answer.Address.ToString());
		}
	}
}
