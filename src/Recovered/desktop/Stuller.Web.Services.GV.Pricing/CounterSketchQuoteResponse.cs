using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CounterSketchQuoteResponse", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CounterSketchQuoteResponse : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private int? RequestIDField;

	private bool CreatedField;

	private Error[] ErrorsField;

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
	public int? RequestID
	{
		get
		{
			return RequestIDField;
		}
		set
		{
			if (!RequestIDField.Equals(value))
			{
				RequestIDField = value;
				RaisePropertyChanged("RequestID");
			}
		}
	}

	[DataMember(Order = 1)]
	public bool Created
	{
		get
		{
			return CreatedField;
		}
		set
		{
			if (!CreatedField.Equals(value))
			{
				CreatedField = value;
				RaisePropertyChanged("Created");
			}
		}
	}

	[DataMember(Order = 2)]
	public Error[] Errors
	{
		get
		{
			return ErrorsField;
		}
		set
		{
			if (ErrorsField != value)
			{
				ErrorsField = value;
				RaisePropertyChanged("Errors");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
