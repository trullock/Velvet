using System;
using System.Configuration;
using System.IO;
using Topshelf;

namespace Velvet
{
	static class Program
	{
		static void Main(string[] args)
		{
			var hostsPath = Path.GetFullPath(Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "system32/drivers/etc/hosts"));

			var configPath = ConfigurationManager.AppSettings["HostsPath"];

			if (!string.IsNullOrEmpty(configPath))
				hostsPath = Path.GetFullPath(configPath);

			HostFactory.Run(x =>
			{
				x.Service<VelvetService>(s =>
				{
					s.SetServiceName("Velvet");
					s.ConstructUsing(name => new VelvetService(hostsPath));
					s.WhenStarted(r => r.Start());
					s.WhenStopped(r => r.Stop());
				});
				x.SetDescription("Provides regex capabilities in you hosts file");
				x.SetDisplayName("Velvet");
				x.SetServiceName("Velvet");
			});
		}
	}
}
