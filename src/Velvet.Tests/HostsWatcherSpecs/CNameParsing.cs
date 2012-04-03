using ARSoft.Tools.Net.Dns;
using NUnit.Framework;

namespace Velvet.Tests.HostsWatcherSpecs
{
	public sealed class CNameParsing : Spec
	{
		protected override void Given()
		{
			hostsFile = @"
	blog.muonlab.com  		C	     *.muon
";
		}

		[Then]
		public void ShouldUpdateOneMappings()
		{
			Assert.AreEqual(1, this.mappings.Length);
		}

		[Test]
		public void ShouldParseCMappingWithWhitespace()
		{
			// TODO: what is the correct record type and class?
			var answer = this.mappings[0].Answer(new DnsQuestion("blog.muon", RecordType.A, RecordClass.Any)) as CNameRecord;
			Assert.AreEqual("blog.muonlab.com", answer.CanonicalName);
		}
	}
}
