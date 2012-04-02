using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DnsServer;
using NUnit.Framework;

namespace Velvet.Tests
{
	[TestFixture]
	public class HostsWatcherSpecs
	{
		IEnumerable<Mapping> mappings;

		[Test]
		public void ShouldUpdateMappingsOnFileChanged()
		{
			var tempPath = Path.GetTempFileName();
			using (var streamWriter = File.CreateText(tempPath))
				streamWriter.WriteLine();

			var hostsWatcher = new HostsWatcher(tempPath);
			hostsWatcher.MappingsChanged += this.HostsWatcherMappingsChanged;

			using (var streamWriter = File.CreateText(tempPath))
			{
				streamWriter.WriteLine("abcd 1234");
				streamWriter.WriteLine();
				streamWriter.WriteLine("	pqr		4567  ");
				streamWriter.Flush();
				streamWriter.Close();
			}

			Thread.Sleep(1000);

			var array = this.mappings.ToArray();
			Assert.AreEqual(2, array.Length);
			
			File.Delete(tempPath);
		}

		void HostsWatcherMappingsChanged(MappingEventArgs args)
		{
			this.mappings = args.Mappings;
		}
	}
}
