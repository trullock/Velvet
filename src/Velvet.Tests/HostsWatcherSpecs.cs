using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ARSoft.Tools.Net.Dns;
using NUnit.Framework;

namespace Velvet.Tests
{
	[TestFixture]
	public class HostsWatcherSpecs
	{
		IEnumerable<Mapping> mappings;
		string tempPath;

		[SetUp]
		public void SetUp()
		{
			this.tempPath = Path.GetTempFileName();
			using (var streamWriter = File.CreateText(this.tempPath))
				streamWriter.WriteLine();

			var hostsWatcher = new HostsWatcher(this.tempPath);
			hostsWatcher.MappingsChanged += this.HostsWatcherMappingsChanged;

			using (var streamWriter = File.CreateText(this.tempPath))
			{
				streamWriter.WriteLine(@"127.0.0.1 A \.dev$");
				streamWriter.WriteLine();
				streamWriter.WriteLine(@"	192.168.0.1		A	     .*\.foo");
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
			Assert.AreEqual(2, array.Length);
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

		[TearDown]
		public void TearDown()
		{
			File.Delete(tempPath);
		}

		void HostsWatcherMappingsChanged(MappingEventArgs args)
		{
			this.mappings = args.Mappings;
		}
	}
}
