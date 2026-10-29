using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Customer", Namespace = "http://schemas.datacontract.org/2004/07/CounterSketchServices.BLL")]
public class Customer : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string EmailField;

	[OptionalField]
	private int IdField;

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
	public string Email
	{
		get
		{
			return EmailField;
		}
		set
		{
			if ((object)EmailField != value)
			{
				EmailField = value;
				RaisePropertyChanged("Email");
			}
		}
	}

	[DataMember]
	public int Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if (!IdField.Equals(value))
			{
				IdField = value;
				RaisePropertyChanged("Id");
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
