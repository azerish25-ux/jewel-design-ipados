using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CounterSketchServer.CounterSketchServices;

[Serializable]
[GeneratedCode("System.Xml", "4.6.1586.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CounterSketch.biz/")]
public class ApplicationEvent
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
		}
	}
}
