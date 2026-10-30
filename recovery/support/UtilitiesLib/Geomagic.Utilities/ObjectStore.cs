using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Geomagic.Utilities;

[Serializable]
public class ObjectStore
{
	private static ObjectStore _Inst;

	private Dictionary<string, ObjectStoreItem> _InternalDictionary = new Dictionary<string, ObjectStoreItem>();

	[NonSerialized]
	protected ObjectStoreItemChangedEventHandler _ItemChanged;

	public static ObjectStore Instance => GetInstance();

	[XmlIgnore]
	public ObjectStoreItem this[string Name] => _InternalDictionary[Name];

	[XmlIgnore]
	public string[] Names => _InternalDictionary.Keys.ToArray();

	[XmlIgnore]
	public ObjectStoreItem[] Values => _InternalDictionary.Values.ToArray();

	[XmlArray("Items")]
	[XmlArrayItem("Item", Type = typeof(ObjectStoreItem))]
	public ObjectStoreItem[] Items
	{
		get
		{
			return _InternalDictionary.Values.ToArray();
		}
		set
		{
			if (value == null)
			{
				_InternalDictionary.Clear();
				return;
			}
			_InternalDictionary.Clear();
			foreach (ObjectStoreItem objectStoreItem in value)
			{
				_InternalDictionary.Add(objectStoreItem.Name, objectStoreItem);
			}
		}
	}

	public event ObjectStoreItemChangedEventHandler ItemChanged
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			_ItemChanged = (ObjectStoreItemChangedEventHandler)Delegate.Combine(_ItemChanged, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			_ItemChanged = (ObjectStoreItemChangedEventHandler)Delegate.Remove(_ItemChanged, value);
		}
	}

	public static ObjectStore GetInstance()
	{
		if (_Inst == null)
		{
			_Inst = new ObjectStore();
		}
		return _Inst;
	}

	protected void NotifyItemChanged(ObjectStoreItem Item)
	{
		if (_ItemChanged != null)
		{
			_ItemChanged(this, new ObjectStoreItemChangedEventArgs(Item));
		}
	}

	public void Add(string Name, object Value)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		ObjectStoreItem item = new ObjectStoreItem(Name, Value);
		Add(item);
	}

	public void Add(ObjectStoreItem Item)
	{
		if (Item == null)
		{
			throw new ArgumentNullException("Item");
		}
		string name = Item.Name;
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Item name cannot be null.");
		}
		if (Contains(name))
		{
			throw new ArgumentException("Item with the name " + name + " already exists.");
		}
		_InternalDictionary.Add(name, Item);
		AddItemHandlers(Item);
	}

	private void AddItemHandlers(ObjectStoreItem Item)
	{
		if (Item != null)
		{
			Item.PropertyChanged += OnItemPropertyChanged;
		}
	}

	private void RemoveItemHandlers(ObjectStoreItem Item)
	{
		if (Item != null)
		{
			Item.PropertyChanged -= OnItemPropertyChanged;
		}
	}

	private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		ObjectStoreItem item = sender as ObjectStoreItem;
		NotifyItemChanged(item);
	}

	public bool Contains(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		return _InternalDictionary.ContainsKey(Name);
	}

	public ObjectStoreItem GetItem(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		return _InternalDictionary[Name];
	}

	public T GetOverride<T>(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		return (T)_InternalDictionary[Name].Override;
	}

	public T GetValue<T>(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		return (T)_InternalDictionary[Name].Value;
	}

	public bool HasOverride(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		return _InternalDictionary[Name].HasOverride;
	}

	public bool IsPersistent(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		return _InternalDictionary[Name].IsPersistent;
	}

	public void ListenForChange(string Name, PropertyChangedEventHandler Handler)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		if (Handler == null)
		{
			throw new ArgumentNullException("Handler");
		}
		ObjectStoreItem objectStoreItem = _InternalDictionary[Name];
		objectStoreItem.PropertyChanged += Handler;
	}

	public void Merge(ObjectStore Other)
	{
		if (Other == null)
		{
			throw new ArgumentNullException("Other");
		}
		string[] names = Other.Names;
		foreach (string text in names)
		{
			ObjectStoreItem objectStoreItem = Other[text];
			if (_InternalDictionary.ContainsKey(text))
			{
				ObjectStoreItem item = _InternalDictionary[text];
				RemoveItemHandlers(item);
				_InternalDictionary[text] = objectStoreItem;
			}
			else
			{
				_InternalDictionary.Add(text, objectStoreItem);
			}
			AddItemHandlers(objectStoreItem);
		}
	}

	public void Remove(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		ObjectStoreItem item = _InternalDictionary[Name];
		RemoveItemHandlers(item);
		_InternalDictionary.Remove(Name);
	}

	public void RemoveListener(string Name, PropertyChangedEventHandler Handler)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		if (Handler == null)
		{
			throw new ArgumentNullException("Handler");
		}
		ObjectStoreItem objectStoreItem = _InternalDictionary[Name];
		objectStoreItem.PropertyChanged -= Handler;
	}

	public void RestoreBase()
	{
		List<ObjectStoreItem> list = Values.ToList();
		foreach (ObjectStoreItem item in list)
		{
			item.RestoreBase();
		}
	}

	public void RestoreDefault()
	{
		List<ObjectStoreItem> list = Values.ToList();
		foreach (ObjectStoreItem item in list)
		{
			item.RestoreDefault();
		}
	}

	public void SetCurrentValueAsDefault(string Name)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		_InternalDictionary[Name].SetCurrentValueAsDefault();
	}

	public void SetOverride<T>(string Name, T Override)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		_InternalDictionary[Name].Override = Override;
	}

	public void SetPersistent(string Name, bool Persistent)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		_InternalDictionary[Name].IsPersistent = Persistent;
	}

	public void SetValue<T>(string Name, T Value)
	{
		if (string.IsNullOrEmpty(Name))
		{
			throw new ArgumentNullException("Name");
		}
		_InternalDictionary[Name].Value = Value;
	}
}
