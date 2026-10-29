using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CounterSketchQuoteStatusResponse", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CounterSketchQuoteStatusResponse : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private CounterSketchQuoteStatus[] CounterSketchQuoteStatusesField;

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
	public CounterSketchQuoteStatus[] CounterSketchQuoteStatuses
	{
		get
		{
			return CounterSketchQuoteStatusesField;
		}
		set
		{
			if (CounterSketchQuoteStatusesField != value)
			{
				CounterSketchQuoteStatusesField = value;
				RaisePropertyChanged("CounterSketchQuoteStatuses");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
