using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ARSoft.Tools.Net.Dns;
using NUnit.Framework;
using Velvet.Mappings;

namespace Velvet.Tests
{
	[TestFixture]
	public sealed class HostsWatcherSpecs
	{
		IEnumerable<Mapping> mappings;
		string tempPath;
		HostsWatcher hostsWatcher;

		[SetUp]
		public void SetUp()
		{
			this.tempPath = Path.GetTempFileName();
			using (var streamWriter = File.CreateText(this.tempPath))
				streamWriter.WriteLine();

			this.hostsWatcher = new HostsWatcher(this.tempPath);
			this.hostsWatcher.MappingsChanged += this.HostsWatcherMappingsChanged;

			using (var streamWriter = File.CreateText(this.tempPath))
			{
				streamWriter.WriteLine(@"127.0.0.1 A *.dev");
				streamWriter.WriteLine();
				streamWriter.WriteLine(@"	192.168.0.1		A	     *.foo");
				streamWriter.WriteLine(@"	blog.muonlab.com  		C	     *.muon");
				streamWriter.WriteLine(@" noise that wont parse");

				streamWriter.Flush();
				streamWriter.Close();
			}

			// give filewatcher time to update
			Thread.Sleep(1000);
		}

		[Test]
		public void ShouldUpdateTwoMappings()
		{
			var array = this.mappings.ToArray();
			Assert.AreEqual(3, array.Length);
		}

		[Test]
		public void ShouldParseAMapping()
		{
			var array = this.mappings.ToArray();

			// TODO: what is the correct record type and class?
			var answer = array[0].Answer(new DnsQuestion("local.dev", RecordType.A, RecordClass.Any)) as ARecord;
			Assert.AreEqual("127.0.0.1", answer.Address.ToString());
		}

		[Test]
		public void ShouldParseAMappingWithWhitespace()
		{
			var array = this.mappings.ToArray();

			// TODO: what is the correct record type and class?
			var answer = array[1].Answer(new DnsQuestion("baz.bar.foo", RecordType.A, RecordClass.Any)) as ARecord;
			Assert.AreEqual("192.168.0.1", answer.Address.ToString());
		}

		[Test]
		public void ShouldParseCMappingWithWhitespace()
		{
			var array = this.mappings.ToArray();

			// TODO: what is the correct record type and class?
			var answer = array[2].Answer(new DnsQuestion("blog.muon", RecordType.A, RecordClass.Any)) as CNameRecord;
			Assert.AreEqual("blog.muonlab.com", answer.CanonicalName);
		}

		[TearDown]
		public void TearDown()
		{
			this.hostsWatcher.Dispose();
			File.Delete(tempPath);
		}

		void HostsWatcherMappingsChanged(MappingEventArgs args)
		{
			this.mappings = args.Mappings;
		}
	}
}
