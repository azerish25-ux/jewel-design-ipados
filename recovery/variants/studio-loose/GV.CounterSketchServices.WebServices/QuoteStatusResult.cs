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
public class QuoteStatusResult : INotifyPropertyChanged
{
	private int requestIdField;

	private string statusField;

	private int orderNumberField;

	public int RequestId
	{
		get
		{
			return requestIdField;
		}
		set
		{
			requestIdField = value;
			RaisePropertyChanged("RequestId");
		}
	}

	public string Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
			RaisePropertyChanged("Status");
		}
	}

	public int OrderNumber
	{
		get
		{
			return orderNumberField;
		}
		set
		{
			orderNumberField = value;
			RaisePropertyChanged("OrderNumber");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
