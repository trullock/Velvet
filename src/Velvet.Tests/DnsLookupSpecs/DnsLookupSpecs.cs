using System;
using System.Net;
using System.Net.Sockets;
using ARSoft.Tools.Net.Dns;
using NUnit.Framework;

namespace Velvet.Tests.DnsLookupSpecs
{
	internal sealed class DnsLookupSpecs : Spec
	{
		DnsMessageBase response;

		protected override void Given()
		{
			this.hostsFile = @"
127.0.0.1 A *.dev
	192.168.0.1		A	     *.foo
	192.168.0.1		A	     *.dev
 noise that wont parse
";
		}

		protected override void When()
		{
			this.response = this.dnsLookup.ProcessQuery(
				new DnsMessage
					{
						Questions =
							new System.Collections.Generic.List<DnsQuestion> { new DnsQuestion("test.dev", RecordType.A, RecordClass.Any) }
					},
				IPAddress.Parse("192.168.0.1"), ProtocolType.IPv4);
		}

		[Then]
		public void AllAnswersShouldBeReturned()
		{
			var dnsMessage = response as DnsMessage;
			var aRecord = dnsMessage.AnswerRecords[0] as ARecord;
			Assert.AreEqual("127.0.0.1", aRecord.Address.ToString());

			var aRecord1 = dnsMessage.AnswerRecords[1] as ARecord;
			Assert.AreEqual("192.168.0.1", aRecord1.Address.ToString());
		}
	}
}