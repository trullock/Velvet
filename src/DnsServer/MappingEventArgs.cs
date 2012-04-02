using System;
using System.Collections.Generic;

namespace Velvet
{
	internal sealed class MappingEventArgs : EventArgs
	{
		public readonly IEnumerable<Mapping> Mappings;

		public MappingEventArgs(IEnumerable<Mapping> mappings)
		{
			this.Mappings = mappings;
		}
	}
}