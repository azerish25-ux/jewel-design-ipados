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
public class EntityKeyMember
{
	private string keyField;

	private object valueField;

	public string Key
	{
		get
		{
			return keyField;
		}
		set
		{
			keyField = value;
		}
	}

	public object Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
