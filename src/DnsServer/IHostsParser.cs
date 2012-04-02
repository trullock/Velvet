using System.Collections.Generic;
using Velvet.Mappings;

namespace Velvet
{
	internal interface IHostsParser
	{
		/// <summary>
		/// Parse a hosts file for DNS mappings
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		IEnumerable<Mapping> ParseFile(string file);
	}
}