using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace GV.CounterSketchServices.WebServices;

[Serializable]
[XmlInclude(typeof(IterationAttribute))]
[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://CounterSketch.biz/")]
public abstract class EntityObject : StructuralObject
{
	private EntityKey entityKeyField;

	public EntityKey EntityKey
	{
		get
		{
			return entityKeyField;
		}
		set
		{
			entityKeyField = value;
			RaisePropertyChanged("EntityKey");
		}
	}
}
