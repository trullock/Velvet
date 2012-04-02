using System;
using System.Collections.Generic;
using Velvet.Mappings;

namespace Velvet
{
	internal sealed class MappingEventArgs : EventArgs
	{
		/// <summary>
		/// The mappings 
		/// </summary>
		public readonly IEnumerable<Mapping> Mappings;

		public MappingEventArgs(IEnumerable<Mapping> mappings)
		{
			this.Mappings = mappings;
		}
	}
}