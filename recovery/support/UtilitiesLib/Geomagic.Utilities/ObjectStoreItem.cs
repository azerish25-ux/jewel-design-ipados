using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Geomagic.Utilities;

[Serializable]
public class ObjectStoreItem : INotifyPropertyChanged
{
	private string _Name;

	[NonSerialized]
	private object _Override;

	private object _Value;

	private object _Default;

	private object _Minimum;

	private object _Maximum;

	private object _Step;

	private bool _IsPersistent = true;

	[NonSerialized]
	protected PropertyChangedEventHandler _PropertyChanged;

	public string Name
	{
		get
		{
			return _Name;
		}
		set
		{
			_Name = value;
			NotifyPropertyChanged("Name");
		}
	}

	[XmlIgnore]
	public object Override
	{
		get
		{
			return _Override;
		}
		set
		{
			_Override = value;
			NotifyPropertyChanged("Override");
			NotifyPropertyChanged("HasOverride");
			NotifyPropertyChanged("Value");
		}
	}

	public bool HasOverride => Override != null;

	public object Value
	{
		get
		{
			if (HasOverride)
			{
				return Override;
			}
			return _Value;
		}
		set
		{
			_Value = value;
			NotifyPropertyChanged("Value");
			NotifyPropertyChanged("Base");
		}
	}

	public object Base => _Value;

	public object Default
	{
		get
		{
			return _Default;
		}
		set
		{
			_Default = value;
			NotifyPropertyChanged("Default");
		}
	}

	public object Minimum
	{
		get
		{
			return _Minimum;
		}
		set
		{
			_Minimum = value;
			NotifyPropertyChanged("Minimum");
		}
	}

	public object Maximum
	{
		get
		{
			return _Maximum;
		}
		set
		{
			_Maximum = value;
			NotifyPropertyChanged("Maximum");
		}
	}

	public object Step
	{
		get
		{
			return _Step;
		}
		set
		{
			_Step = value;
			NotifyPropertyChanged("Step");
		}
	}

	public bool IsPersistent
	{
		get
		{
			return _IsPersistent;
		}
		set
		{
			_IsPersistent = value;
			NotifyPropertyChanged("IsPersistent");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			_PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			_PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value);
		}
	}

	public ObjectStoreItem()
	{
	}

	public ObjectStoreItem(string Name, object Value)
	{
		this.Name = Name;
		Default = (this.Value = Value);
	}

	public void ClearOverride()
	{
		Override = null;
	}

	public void RestoreDefault()
	{
		Value = Default;
	}

	public void RestoreBase()
	{
		ClearOverride();
	}

	public void SetCurrentValueAsDefault()
	{
		Default = Value;
	}

	protected void NotifyPropertyChanged(string PropertyName)
	{
		if (_PropertyChanged != null)
		{
			_PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
		}
	}
}
