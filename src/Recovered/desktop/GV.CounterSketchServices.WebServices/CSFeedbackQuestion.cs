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
public class CSFeedbackQuestion : INotifyPropertyChanged
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
			RaisePropertyChanged("AnswerChoices");
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
			RaisePropertyChanged("Id");
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
			RaisePropertyChanged("SurveyId");
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
			RaisePropertyChanged("Question");
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
			RaisePropertyChanged("QuestionType");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
