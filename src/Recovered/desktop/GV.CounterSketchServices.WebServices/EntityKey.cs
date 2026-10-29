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
public class EntityKey : INotifyPropertyChanged
{
	private string entitySetNameField;

	private string entityContainerNameField;

	private EntityKeyMember[] entityKeyValuesField;

	public string EntitySetName
	{
		get
		{
			return entitySetNameField;
		}
		set
		{
			entitySetNameField = value;
			RaisePropertyChanged("EntitySetName");
		}
	}

	public string EntityContainerName
	{
		get
		{
			return entityContainerNameField;
		}
		set
		{
			entityContainerNameField = value;
			RaisePropertyChanged("EntityContainerName");
		}
	}

	public EntityKeyMember[] EntityKeyValues
	{
		get
		{
			return entityKeyValuesField;
		}
		set
		{
			entityKeyValuesField = value;
			RaisePropertyChanged("EntityKeyValues");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
