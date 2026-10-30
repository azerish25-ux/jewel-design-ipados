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
			RaisePropertyChanged("IterationAttributeId");
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
			RaisePropertyChanged("IterationAttributeIndex");
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
			RaisePropertyChanged("AttributeName");
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
			RaisePropertyChanged("IterationAttributeTypeReference");
		}
	}
}
