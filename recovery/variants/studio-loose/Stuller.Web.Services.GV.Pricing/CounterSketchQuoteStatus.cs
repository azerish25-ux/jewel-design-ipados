using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CounterSketchQuoteStatus", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CounterSketchQuoteStatus : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private int RequestIDField;

	private int? OrderNumberField;

	private string DescriptionField;

	private CounterSketchQuoteComment[] CounterSketchQuoteCommentsField;

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
	public int RequestID
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
	public int? OrderNumber
	{
		get
		{
			return OrderNumberField;
		}
		set
		{
			if (!OrderNumberField.Equals(value))
			{
				OrderNumberField = value;
				RaisePropertyChanged("OrderNumber");
			}
		}
	}

	[DataMember(Order = 2)]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			if ((object)DescriptionField != value)
			{
				DescriptionField = value;
				RaisePropertyChanged("Description");
			}
		}
	}

	[DataMember(Order = 3)]
	public CounterSketchQuoteComment[] CounterSketchQuoteComments
	{
		get
		{
			return CounterSketchQuoteCommentsField;
		}
		set
		{
			if (CounterSketchQuoteCommentsField != value)
			{
				CounterSketchQuoteCommentsField = value;
				RaisePropertyChanged("CounterSketchQuoteComments");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
