using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Velvet.Mappings;

namespace Velvet
{
	internal sealed class HostsParser : IHostsParser
	{
		public IEnumerable<Mapping> ParseFile(string file)
		{
			var lines = file.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

			var lineRegex = new Regex(@"^\s*([^\s]+)\s+([^\s]+)\s*$", RegexOptions.Singleline);

			var mappings = new List<Mapping>();

			foreach (var line in lines)
			{
				var trimmed = line.Trim();

				var match = lineRegex.Match(trimmed);

				if (!match.Success)
					continue;

				var mapping = ParseLine(match);
				if (mapping != null)
					mappings.Add(mapping);
			}

			return mappings;
		}

		static Mapping ParseLine(Match match)
		{
			IPAddress ip;
			if (IPAddress.TryParse(match.Groups[1].Value, out ip))
				return new AMapping(match.Groups[2].Value, ip);

			return new CNameMapping(match.Groups[2].Value, match.Groups[1].Value);
		}
	}
}