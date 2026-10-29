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
public class ApplicationEvent : INotifyPropertyChanged
{
	private Guid applicationEventIdField;

	private ApplicationEventType eventTypeField;

	private string eventMessageField;

	private DateTime eventDateTimeField;

	public Guid ApplicationEventId
	{
		get
		{
			return applicationEventIdField;
		}
		set
		{
			applicationEventIdField = value;
			RaisePropertyChanged("ApplicationEventId");
		}
	}

	public ApplicationEventType EventType
	{
		get
		{
			return eventTypeField;
		}
		set
		{
			eventTypeField = value;
			RaisePropertyChanged("EventType");
		}
	}

	public string EventMessage
	{
		get
		{
			return eventMessageField;
		}
		set
		{
			eventMessageField = value;
			RaisePropertyChanged("EventMessage");
		}
	}

	public DateTime EventDateTime
	{
		get
		{
			return eventDateTimeField;
		}
		set
		{
			eventDateTimeField = value;
			RaisePropertyChanged("EventDateTime");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
