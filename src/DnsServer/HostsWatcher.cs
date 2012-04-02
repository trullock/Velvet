using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DnsServer
{
	internal sealed class HostsWatcher : IDisposable
	{
		FileSystemWatcher watcher;

		public delegate void MappingsChangedHandler(MappingEventArgs args);

		public event MappingsChangedHandler MappingsChanged;

		public HostsWatcher(string path)
		{
			//var path = Path.GetFullPath(Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "system32/drivers/etc/hosts"));
			var directoryName = Path.GetDirectoryName(path);
			var fileName = Path.GetFileName(path);
			this.watcher = new FileSystemWatcher(directoryName, fileName);
			watcher.NotifyFilter = NotifyFilters.LastWrite;
			this.watcher.Changed += this.watcher_Changed;
			watcher.EnableRaisingEvents = true;
		}

		void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			var readFile = ReadFile(e.FullPath);
			var enumerable = ParseFile(readFile);
			if(MappingsChanged != null)
				MappingsChanged(new MappingEventArgs(enumerable));
		}

		static string ReadFile(string path)
		{
			using (var reader = new StreamReader(path))
				return reader.ReadToEnd();
		}

		static IEnumerable<Mapping> ParseFile(string file)
		{
			var lines = file.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

			var lineRegex = new Regex(@"([^\s]+)\s(.*?)", RegexOptions.Singleline);

			foreach(var line in lines)
			{
				var trimmed = line.Trim();

				var match = lineRegex.Match(trimmed);
				
				if(!match.Success)
					continue;

				yield return new Mapping(match.Groups[1].Value, match.Groups[2].Value);
			}
		}

		public void Dispose()
		{
			this.watcher.EnableRaisingEvents = false;
			this.watcher.Dispose();
		}
	}
}