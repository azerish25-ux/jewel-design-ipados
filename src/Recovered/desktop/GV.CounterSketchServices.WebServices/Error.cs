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
public class Error : INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private string exceptionField;

	private string messageField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
			RaisePropertyChanged("ExtensionData");
		}
	}

	public string Exception
	{
		get
		{
			return exceptionField;
		}
		set
		{
			exceptionField = value;
			RaisePropertyChanged("Exception");
		}
	}

	public string Message
	{
		get
		{
			return messageField;
		}
		set
		{
			messageField = value;
			RaisePropertyChanged("Message");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
