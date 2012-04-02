using System.Collections.Generic;

namespace Velvet
{
	internal interface IHostsParser
	{
		IEnumerable<Mapping> ParseFile(string file);
	}
}