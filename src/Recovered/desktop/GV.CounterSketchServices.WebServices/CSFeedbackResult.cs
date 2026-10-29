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
public class CSFeedbackResult : INotifyPropertyChanged
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
			RaisePropertyChanged("SurveyId");
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
			RaisePropertyChanged("KeyId");
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
			RaisePropertyChanged("StullerDesignId");
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
			RaisePropertyChanged("FeedbackQuestionAnswers");
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
			RaisePropertyChanged("Comments");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
