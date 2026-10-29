using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CounterSketchQuoteComment", Namespace = "http://services.stuller.com/v3/GemVision")]
public class CounterSketchQuoteComment : IExtensibleDataObject, INotifyPropertyChanged
{
	private ExtensionDataObject extensionDataField;

	private DateTime CommentDateField;

	private string DescriptionField;

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
	public DateTime CommentDate
	{
		get
		{
			return CommentDateField;
		}
		set
		{
			if (!CommentDateField.Equals(value))
			{
				CommentDateField = value;
				RaisePropertyChanged("CommentDate");
			}
		}
	}

	[DataMember]
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

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
