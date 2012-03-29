using System;
using System.Net;
using System.Net.Sockets;
using ARSoft.Tools.Net.Dns;
using Topshelf;

namespace DnsServer
{
	class Program
	{
		static void Main(string[] args)
		{
			HostFactory.Run(x =>
			{
				x.Service<ARSoft.Tools.Net.Dns.DnsServer>(s =>
				{
					s.SetServiceName("Dev DNS Server");
					s.ConstructUsing(name => new ARSoft.Tools.Net.Dns.DnsServer(IPAddress.Any, 10, 10, ProcessQuery));
					s.WhenStarted(r => r.Start());
					s.WhenStopped(r => r.Stop());
				});
				x.SetDescription("Maps any .dev address to localhost");
				x.SetDisplayName("Dev DNS Server");
				x.SetServiceName("Dev DNS Server");
			});
		}

		static DnsMessageBase ProcessQuery(DnsMessageBase message, IPAddress clientAddress, ProtocolType protocol)
		{
			message.IsQuery = false;

			var query = message as DnsMessage;

			if ((query != null) && (query.Questions.Count == 1))
			{
				var question = query.Questions[0];

				if(question.Name.EndsWith(".dev", StringComparison.InvariantCultureIgnoreCase))
				{
					query.ReturnCode = ReturnCode.NoError;
					query.AnswerRecords.Add(new ARecord(question.Name, 1, IPAddress.Parse("127.0.0.1")));
					return query;
				}
			}

			message.ReturnCode = ReturnCode.ServerFailure;
			return message;
		}
	}
}
