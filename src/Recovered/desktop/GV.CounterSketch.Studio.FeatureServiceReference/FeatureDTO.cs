using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Studio.FeatureServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FeatureDTO", Namespace = "http://schemas.datacontract.org/2004/07/CounterSketchSecurityServices.DTO")]
public class FeatureDTO : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int FeatureIdField;

	[OptionalField]
	private string NameField;

	[Browsable(false)]
	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public int FeatureId
	{
		get
		{
			return FeatureIdField;
		}
		set
		{
			if (!FeatureIdField.Equals(value))
			{
				FeatureIdField = value;
				RaisePropertyChanged("FeatureId");
			}
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			if ((object)NameField != value)
			{
				NameField = value;
				RaisePropertyChanged("Name");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
