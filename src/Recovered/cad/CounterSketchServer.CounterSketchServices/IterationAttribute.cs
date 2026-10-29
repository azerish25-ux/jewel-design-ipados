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
public class IterationAttribute : EntityObject
{
	private int iterationAttributeIdField;

	private int? iterationAttributeIndexField;

	private string attributeNameField;

	private EntityReferenceOfIterationAttributeType iterationAttributeTypeReferenceField;

	public int IterationAttributeId
	{
		get
		{
			return iterationAttributeIdField;
		}
		set
		{
			iterationAttributeIdField = value;
		}
	}

	[XmlElement(IsNullable = true)]
	public int? IterationAttributeIndex
	{
		get
		{
			return iterationAttributeIndexField;
		}
		set
		{
			iterationAttributeIndexField = value;
		}
	}

	public string AttributeName
	{
		get
		{
			return attributeNameField;
		}
		set
		{
			attributeNameField = value;
		}
	}

	public EntityReferenceOfIterationAttributeType IterationAttributeTypeReference
	{
		get
		{
			return iterationAttributeTypeReferenceField;
		}
		set
		{
			iterationAttributeTypeReferenceField = value;
		}
	}
}
