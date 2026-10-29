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
public class CSFeedbackResult
{
	private int? surveyIdField;

	private int keyIdField;

	private string stullerDesignIdField;

	private CSFeedbackQuestionAnswer[] feedbackQuestionAnswersField;

	private string commentsField;

	[XmlElement(IsNullable = true)]
	public int? SurveyId
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

	public int KeyId
	{
		get
		{
			return keyIdField;
		}
		set
		{
			keyIdField = value;
		}
	}

	public string StullerDesignId
	{
		get
		{
			return stullerDesignIdField;
		}
		set
		{
			stullerDesignIdField = value;
		}
	}

	[XmlArrayItem(IsNullable = false)]
	public CSFeedbackQuestionAnswer[] FeedbackQuestionAnswers
	{
		get
		{
			return feedbackQuestionAnswersField;
		}
		set
		{
			feedbackQuestionAnswersField = value;
		}
	}

	public string Comments
	{
		get
		{
			return commentsField;
		}
		set
		{
			commentsField = value;
		}
	}
}
