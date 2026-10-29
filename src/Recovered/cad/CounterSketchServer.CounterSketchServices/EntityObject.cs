using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CounterSketchServer.CounterSketchServices;

[Serializable]
[XmlInclude(typeof(IterationAttribute))]
[GeneratedCode("System.Xml", "4.6.1586.0")]
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
		}
	}
}
