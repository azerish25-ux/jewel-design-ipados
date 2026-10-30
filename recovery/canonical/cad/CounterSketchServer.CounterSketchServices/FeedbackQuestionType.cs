using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace CounterSketchServer.CounterSketchServices;

[Serializable]
[GeneratedCode("System.Xml", "4.6.1586.0")]
[XmlType(Namespace = "http://CounterSketch.biz/")]
public enum FeedbackQuestionType
{
	MultipleChoice,
	YesNo,
	Freeform
}
