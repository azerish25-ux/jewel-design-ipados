using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace GV.CounterSketchServices.WebServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.18020")]
[XmlType(Namespace = "http://CounterSketch.biz/")]
public enum FeedbackQuestionType
{
	MultipleChoice,
	YesNo,
	Freeform
}
