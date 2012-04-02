using System.Net;
using System.Net.Sockets;
using ARSoft.Tools.Net.Dns;

namespace Velvet
{
	internal interface IDnsLookup
	{
		/// <summary>
		/// Processes a DNS lookup
		/// </summary>
		/// <param name="message"></param>
		/// <param name="clientAddress"></param>
		/// <param name="protocol"></param>
		/// <returns></returns>
		DnsMessageBase ProcessQuery(DnsMessageBase message, IPAddress clientAddress, ProtocolType protocol);
	}
}