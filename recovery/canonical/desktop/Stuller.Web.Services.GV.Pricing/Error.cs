using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Error", Namespace = "http://services.stuller.com/v3/GemVision")]
public class Error : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private string ExceptionField;

	private string MessageField;

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
	public string Exception
	{
		get
		{
			return ExceptionField;
		}
		set
		{
			if ((object)ExceptionField != value)
			{
				ExceptionField = value;
				RaisePropertyChanged("Exception");
			}
		}
	}

	[DataMember]
	public string Message
	{
		get
		{
			return MessageField;
		}
		set
		{
			if ((object)MessageField != value)
			{
				MessageField = value;
				RaisePropertyChanged("Message");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
