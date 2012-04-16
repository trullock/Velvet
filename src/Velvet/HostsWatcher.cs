using System;
using System.IO;

namespace Velvet
{
	internal sealed class HostsWatcher : IDisposable, IHostsWatcher
	{
		readonly FileSystemWatcher watcher;
		readonly IHostsParser parser;
		readonly string path;

		public event MappingsChangedHandler MappingsChanged;

		public HostsWatcher(string path)
			: this(new HostsParser(), path)
		{
		}

		public HostsWatcher(IHostsParser parser, string path)
		{
			this.parser = parser;
			this.path = path;

			var directoryName = Path.GetDirectoryName(path);
			var fileName = Path.GetFileName(path);

			this.watcher = new FileSystemWatcher(directoryName, fileName);
			watcher.NotifyFilter = NotifyFilters.LastWrite;

			this.watcher.Changed += this.WatcherChanged;
			
			watcher.EnableRaisingEvents = true;
		}

		void WatcherChanged(object sender, FileSystemEventArgs e)
		{
			LoadMappings(e.FullPath);
		}

		void LoadMappings(string path)
		{
			var readFile = ReadFile(path);
			var enumerable = this.parser.ParseFile(readFile);
			if(this.MappingsChanged != null)
				this.MappingsChanged(new MappingEventArgs(enumerable));
		}

		public void RefreshFile()
		{
			this.LoadMappings(this.path);
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