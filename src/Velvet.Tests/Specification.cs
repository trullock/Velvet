using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Velvet.Mappings;

namespace Velvet.Tests
{
	[TestFixture]
	public abstract class Spec
	{
		string hostPath;

		protected string hostsFile;
		HostsWatcher hostsWatcher;
		protected Mapping[] mappings;

		[SetUp]
		public void SetUp()
		{
			this.hostPath = Path.GetTempFileName();
			using (var streamWriter = File.CreateText(this.hostPath))
				streamWriter.WriteLine();

			this.hostsWatcher = new HostsWatcher(this.hostPath);
			this.hostsWatcher.MappingsChanged += this.HostsWatcherMappingsChanged;


			this.Given();
			this.When();

			// give filewatcher time to update
			Thread.Sleep(1000);
		}

		protected virtual void Given()
		{
		}

		protected virtual void When()
		{
			using (var streamWriter = File.CreateText(this.hostPath))
			{
				streamWriter.Write(this.hostsFile);

				streamWriter.Flush();
				streamWriter.Close();
			}
		}


		[TearDown]
		public void TearDown()
		{
			this.hostsWatcher.Dispose();
			File.Delete(this.hostPath);
		}

		void HostsWatcherMappingsChanged(MappingEventArgs args)
		{
			this.mappings = args.Mappings.ToArray();
		}
	}
}