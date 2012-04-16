using System;
using System.Net;
using ARSoft.Tools.Net.Dns;

namespace Velvet
{
	internal sealed class VelvetService : IDisposable
	{
		readonly IDnsLookup dnsLookup;
		readonly DnsServer dnsServer;

		public VelvetService(string path)
			 : this(new DnsLookup(path))
		{
		}

		internal VelvetService(IDnsLookup dnsLookup)
		{
			this.dnsLookup = dnsLookup;
			this.dnsServer = new DnsServer(IPAddress.Any, 10, 10, dnsLookup.ProcessQuery);
		}

		public void Start()
		{
			dnsServer.Start();
		}

		public void Stop()
		{
			dnsServer.Stop();
		}

		public void Dispose()
		{
			if (dnsLookup is IDisposable)
				(dnsLookup as IDisposable).Dispose();
		}
	}
}