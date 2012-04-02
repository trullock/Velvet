using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ARSoft.Tools.Net.Dns;
using NUnit.Framework;

namespace Velvet.Tests
{
	[TestFixture]
	public sealed class DnsLookupSpecs
	{
		string tempPath;
		DnsLookup dnsLookup;

		[SetUp]
		public void SetUp()
		{
			this.tempPath = Path.GetTempFileName();
			using (var streamWriter = File.CreateText(this.tempPath))
				streamWriter.WriteLine();

			this.dnsLookup = new DnsLookup(this.tempPath);

			using (var streamWriter = File.CreateText(this.tempPath))
			{
				streamWriter.WriteLine(@"127.0.0.1 A \.dev$");
				streamWriter.WriteLine(@"	192.168.0.1		A	     .*\.foo");
				streamWriter.WriteLine(@"	192.168.0.1		A	     .*\.dev");
				streamWriter.WriteLine(@" noise that wont parse");

				streamWriter.Flush();
				streamWriter.Close();
			}

			// give filewatcher time to update
			Thread.Sleep(1000);
		}

		[Test]
		public void AllAnswersShouldBeReturned()
		{
			var response = dnsLookup.ProcessQuery(
				new DnsMessage
					{
						Questions =
							new System.Collections.Generic.List<DnsQuestion> {new DnsQuestion("test.dev", RecordType.A, RecordClass.Any)}
					},
				IPAddress.Parse("192.168.0.1"), ProtocolType.IPv4);

			var dnsMessage = response as DnsMessage;
			var aRecord = dnsMessage.AnswerRecords[0] as ARecord;
			Assert.AreEqual("127.0.0.1", aRecord.Address.ToString());

			var aRecord1 = dnsMessage.AnswerRecords[1] as ARecord;
			Assert.AreEqual("192.168.0.1", aRecord1.Address.ToString());
		}

		[TearDown]
		public void TearDown()
		{
			this.dnsLookup.Dispose();
			File.Delete(tempPath);
		}
	}
}