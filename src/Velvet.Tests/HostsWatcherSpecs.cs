using ARSoft.Tools.Net.Dns;
using NUnit.Framework;

namespace Velvet.Tests
{
	public sealed class HostsWatcherSpecs : Spec
	{
		protected override void Given()
		{
			hostsFile = @"
127.0.0.1 A *.dev
	192.168.0.1		A	     *.foo
	blog.muonlab.com  		C	     *.muon
 noise that wont parse
";
		}


		[Then]
		public void ShouldUpdateTwoMappings()
		{
			Assert.AreEqual(3, this.mappings.Length);
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
			var answer = this.mappings[1].Answer(new DnsQuestion("baz.bar.foo", RecordType.A, RecordClass.Any)) as ARecord;
			Assert.AreEqual("192.168.0.1", answer.Address.ToString());
		}

		[Test]
		public void ShouldParseCMappingWithWhitespace()
		{
			// TODO: what is the correct record type and class?
			var answer = this.mappings[2].Answer(new DnsQuestion("blog.muon", RecordType.A, RecordClass.Any)) as CNameRecord;
			Assert.AreEqual("blog.muonlab.com", answer.CanonicalName);
		}

	}
}
