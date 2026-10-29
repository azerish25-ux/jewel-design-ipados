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
public class EntityKey
{
	private string entitySetNameField;

	private string entityContainerNameField;

	private EntityKeyMember[] entityKeyValuesField;

	public string EntitySetName
	{
		get
		{
			return entitySetNameField;
		}
		set
		{
			entitySetNameField = value;
		}
	}

	public string EntityContainerName
	{
		get
		{
			return entityContainerNameField;
		}
		set
		{
			entityContainerNameField = value;
		}
	}

	public EntityKeyMember[] EntityKeyValues
	{
		get
		{
			return entityKeyValuesField;
		}
		set
		{
			entityKeyValuesField = value;
		}
	}
}
