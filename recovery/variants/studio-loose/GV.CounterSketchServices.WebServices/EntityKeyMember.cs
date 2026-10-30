using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace GV.CounterSketchServices.WebServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CounterSketch.biz/")]
public class EntityKeyMember : INotifyPropertyChanged
{
	private string keyField;

	private object valueField;

	public string Key
	{
		get
		{
			return keyField;
		}
		set
		{
			keyField = value;
			RaisePropertyChanged("Key");
		}
	}

	public object Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
			RaisePropertyChanged("Value");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
