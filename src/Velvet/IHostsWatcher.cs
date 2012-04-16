namespace Velvet
{
	internal interface IHostsWatcher
	{
		/// <summary>
		/// Occurrs when the mappings have been modified
		/// </summary>
		event MappingsChangedHandler MappingsChanged;

		void RefreshFile();
	}
}