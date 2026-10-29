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
public class CSFeedbackQuestion
{
	private CSFeedbackQuestionAnswer[] answerChoicesField;

	private int idField;

	private int surveyIdField;

	private string questionField;

	private FeedbackQuestionType questionTypeField;

	[XmlArrayItem(IsNullable = false)]
	public CSFeedbackQuestionAnswer[] AnswerChoices
	{
		get
		{
			return answerChoicesField;
		}
		set
		{
			answerChoicesField = value;
		}
	}

	public int Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	public int SurveyId
	{
		get
		{
			return surveyIdField;
		}
		set
		{
			surveyIdField = value;
		}
	}

	public string Question
	{
		get
		{
			return questionField;
		}
		set
		{
			questionField = value;
		}
	}

	public FeedbackQuestionType QuestionType
	{
		get
		{
			return questionTypeField;
		}
		set
		{
			questionTypeField = value;
		}
	}
}
