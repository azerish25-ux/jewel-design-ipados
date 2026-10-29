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
public class CSFeedbackQuestionAnswer
{
	private int? answerIdField;

	private string freeFormAnswerField;

	private int questionIdField;

	[XmlElement(IsNullable = true)]
	public int? AnswerId
	{
		get
		{
			return answerIdField;
		}
		set
		{
			answerIdField = value;
		}
	}

	public string FreeFormAnswer
	{
		get
		{
			return freeFormAnswerField;
		}
		set
		{
			freeFormAnswerField = value;
		}
	}

	public int QuestionId
	{
		get
		{
			return questionIdField;
		}
		set
		{
			questionIdField = value;
		}
	}
}
