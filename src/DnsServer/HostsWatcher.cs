using System;
using System.IO;

namespace Velvet
{
	internal sealed class HostsWatcher : IDisposable, IHostsWatcher
	{
		readonly FileSystemWatcher watcher;
		readonly IHostsParser parser;

		public event MappingsChangedHandler MappingsChanged;

		public HostsWatcher(string path)
			: this(new HostsParser(), path)
		{
		}

		public HostsWatcher(IHostsParser parser, string path)
		{
			this.parser = parser;

			var directoryName = Path.GetDirectoryName(path);
			var fileName = Path.GetFileName(path);

			this.watcher = new FileSystemWatcher(directoryName, fileName);
			watcher.NotifyFilter = NotifyFilters.LastWrite;

			this.watcher.Changed += this.WatcherChanged;

			watcher.EnableRaisingEvents = true;
		}

		void WatcherChanged(object sender, FileSystemEventArgs e)
		{
			var readFile = ReadFile(e.FullPath);
			var enumerable = parser.ParseFile(readFile);
			if(MappingsChanged != null)
				MappingsChanged(new MappingEventArgs(enumerable));
		}

		static string ReadFile(string path)
		{
			using (var reader = new StreamReader(path))
				return reader.ReadToEnd();
		}

		public void Dispose()
		{
			this.watcher.EnableRaisingEvents = false;
			this.watcher.Dispose();
		}
	}
}