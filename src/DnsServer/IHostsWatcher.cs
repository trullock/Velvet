namespace Velvet
{
	internal interface IHostsWatcher
	{
		event MappingsChangedHandler MappingsChanged;
	}
}