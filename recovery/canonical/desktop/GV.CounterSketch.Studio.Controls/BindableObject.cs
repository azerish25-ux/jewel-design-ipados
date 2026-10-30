using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GV.CounterSketch.Studio.Controls;

[Serializable]
public abstract class BindableObject : INotifyPropertyChanged
{
	private static readonly Dictionary<string, PropertyChangedEventArgs> eventArgCache;

	private const string ERROR_MSG = "{0} is not a public property of {1}";

	[field: NonSerialized]
	public event PropertyChangedEventHandler PropertyChanged;

	[Conditional("DEBUG")]
	private void VerifyProperty(string propertyName)
	{
		Type type = GetType();
		if (type.GetProperty(propertyName) == null)
		{
			_ = $"{propertyName} is not a public property of {type.FullName}";
		}
	}

	static BindableObject()
	{
		eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
	}

	public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			throw new ArgumentException("propertyName cannot be null or empty.");
		}
		lock (typeof(BindableObject))
		{
			if (!eventArgCache.ContainsKey(propertyName))
			{
				eventArgCache.Add(propertyName, new PropertyChangedEventArgs(propertyName));
			}
			return eventArgCache[propertyName];
		}
	}

	protected virtual void AfterPropertyChanged(string propertyName)
	{
	}

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
		if (propertyChangedEventHandler != null)
		{
			PropertyChangedEventArgs propertyChangedEventArgs = GetPropertyChangedEventArgs(propertyName);
			propertyChangedEventHandler(this, propertyChangedEventArgs);
		}
		AfterPropertyChanged(propertyName);
	}
}
