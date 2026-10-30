using System;

namespace Geomagic.Utilities;

public class ObjectStoreItemChangedEventArgs : EventArgs
{
	public string Name { get; set; }

	public ObjectStoreItem Item { get; set; }

	public ObjectStoreItemChangedEventArgs()
	{
	}

	public ObjectStoreItemChangedEventArgs(ObjectStoreItem Item)
	{
		Name = Item.Name;
		this.Item = Item;
	}
}
