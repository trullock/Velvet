using System.IO;
using System.Threading;
using NUnit.Framework;

namespace Velvet.Tests.DnsLookupSpecs
{
	[TestFixture]
	internal abstract class Spec
	{
		string hostPath;
		protected DnsLookup dnsLookup;

		protected string hostsFile
		{
			set
			{
				using (var streamWriter = File.CreateText(this.hostPath))
				{
					streamWriter.Write(value);

					streamWriter.Flush();
					streamWriter.Close();
				}
			}
		}

		[SetUp]
		public void SetUp()
		{
			this.hostPath = Path.GetTempFileName();
			using (var streamWriter = File.CreateText(this.hostPath))
				streamWriter.WriteLine();

			this.dnsLookup = new DnsLookup(this.hostPath);

			this.Given();

			// give filewatcher time to update
			Thread.Sleep(1000);

			this.When();
		}

		protected virtual void Given()
		{
		}

		protected abstract void When();

		[TearDown]
		public void TearDown()
		{
			this.dnsLookup.Dispose();
			File.Delete(this.hostPath);
		}
	}
}