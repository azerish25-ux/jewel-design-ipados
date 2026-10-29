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
public class QuoteStatusResult
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
		}
	}
}
