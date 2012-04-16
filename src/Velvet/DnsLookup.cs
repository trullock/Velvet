using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using ARSoft.Tools.Net.Dns;
using Velvet.Mappings;

namespace Velvet
{
	internal sealed class DnsLookup : IDnsLookup, IDisposable
	{
		readonly IHostsWatcher hostsWatcher;
		volatile IEnumerable<Mapping> mappings;

		public DnsLookup(string path)
			 : this(new HostsWatcher(path))
		{
		}

		internal DnsLookup(IHostsWatcher watcher)
		{
			this.hostsWatcher = watcher;
			hostsWatcher.MappingsChanged += this.HostsWatcherMappingsChanged;
			hostsWatcher.RefreshFile();
		}

		void HostsWatcherMappingsChanged(MappingEventArgs args)
		{
			this.mappings = args.Mappings;
		}

		public DnsMessageBase ProcessQuery(DnsMessageBase message, IPAddress clientAddress, ProtocolType protocol)
		{
			message.IsQuery = false;

			var query = message as DnsMessage;

			if ((query != null) && (query.Questions.Count == 1))
			{
				var question = query.Questions[0];

				var answers = this.mappings.Select(m => m.Answer(question)).Where(a => a != null);
				if (answers.Any())
				{
					query.ReturnCode = ReturnCode.NoError;
					foreach (var answer in answers)
						query.AnswerRecords.Add(answer);
					return query;
				}
			}

			message.ReturnCode = ReturnCode.ServerFailure;
			return message;
		}

		public void Dispose()
		{
			if (hostsWatcher is IDisposable)
				(hostsWatcher as IDisposable).Dispose();
		}
	}
}