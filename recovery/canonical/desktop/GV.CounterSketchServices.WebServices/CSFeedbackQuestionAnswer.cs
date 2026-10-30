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
public class CSFeedbackQuestionAnswer : INotifyPropertyChanged
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
			RaisePropertyChanged("AnswerId");
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
			RaisePropertyChanged("FreeFormAnswer");
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
			RaisePropertyChanged("QuestionId");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
