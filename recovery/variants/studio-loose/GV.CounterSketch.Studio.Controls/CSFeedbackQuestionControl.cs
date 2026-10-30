using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketchServices.WebServices;

namespace GV.CounterSketch.Studio.Controls;

public class CSFeedbackQuestionControl : UserControl, IComponentConnector
{
	public static readonly DependencyProperty IsFreeFormQuestionProperty = DependencyProperty.Register("IsFreeFormQuestion", typeof(bool), typeof(CSFeedbackQuestionControl), new UIPropertyMetadata(false));

	public static readonly DependencyProperty IsMultipleChoiceQuestionProperty = DependencyProperty.Register("IsMultipleChoiceQuestion", typeof(bool), typeof(CSFeedbackQuestionControl), new UIPropertyMetadata(false));

	public static readonly DependencyProperty IsYesNoQuestionProperty = DependencyProperty.Register("IsYesNoQuestion", typeof(bool), typeof(CSFeedbackQuestionControl), new UIPropertyMetadata(false));

	internal CSFeedbackQuestionControl CSFeedbackQuestionUserControl;

	internal DockPanel LayoutRoot;

	internal TextBlock txtQuestion;

	internal Border borderMultipleChoice;

	internal ComboBox cmboAnswer;

	internal Border borderYesNo;

	internal RadioButton radioYes;

	internal RadioButton radioNo;

	internal Border borderFreeForm;

	internal TextBox txtFreeform;

	private bool _contentLoaded;

	public bool IsFreeFormQuestion
	{
		get
		{
			return (bool)GetValue(IsFreeFormQuestionProperty);
		}
		set
		{
			SetValue(IsFreeFormQuestionProperty, value);
		}
	}

	public bool IsMultipleChoiceQuestion
	{
		get
		{
			return (bool)GetValue(IsMultipleChoiceQuestionProperty);
		}
		set
		{
			SetValue(IsMultipleChoiceQuestionProperty, value);
		}
	}

	public bool IsYesNoQuestion
	{
		get
		{
			return (bool)GetValue(IsYesNoQuestionProperty);
		}
		set
		{
			SetValue(IsYesNoQuestionProperty, value);
		}
	}

	public int? SelectedAnswerId { get; set; }

	public CSFeedbackQuestion FeedbackQuestion { get; set; }

	public CSFeedbackQuestionControl(CSFeedbackQuestion feedbackQuestion)
		: this()
	{
		FeedbackQuestion = feedbackQuestion;
		SetControl();
	}

	public CSFeedbackQuestionControl()
	{
		InitializeComponent();
	}

	public string RetrieveAnswer()
	{
		string result = string.Empty;
		switch (FeedbackQuestion.QuestionType)
		{
		case FeedbackQuestionType.MultipleChoice:
			if (cmboAnswer.SelectedItem is ComboBoxItem comboBoxItem)
			{
				SelectedAnswerId = (int)comboBoxItem.Tag;
				result = (string)comboBoxItem.Content;
			}
			break;
		case FeedbackQuestionType.YesNo:
			if (radioYes.IsChecked == true)
			{
				result = "Yes";
				SelectedAnswerId = (int)radioYes.Tag;
			}
			else if (radioNo.IsChecked == true)
			{
				result = "No";
				SelectedAnswerId = (int)radioNo.Tag;
			}
			break;
		case FeedbackQuestionType.Freeform:
			if (!string.IsNullOrEmpty(txtFreeform.Text))
			{
				result = txtFreeform.Text;
				SelectedAnswerId = null;
			}
			break;
		}
		return result;
	}

	public void SetControl()
	{
		txtQuestion.Text = FeedbackQuestion.Question;
		switch (FeedbackQuestion.QuestionType)
		{
		case FeedbackQuestionType.MultipleChoice:
		{
			IsMultipleChoiceQuestion = true;
			CSFeedbackQuestionAnswer[] answerChoices = FeedbackQuestion.AnswerChoices;
			foreach (CSFeedbackQuestionAnswer cSFeedbackQuestionAnswer in answerChoices)
			{
				ComboBoxItem comboBoxItem = new ComboBoxItem();
				comboBoxItem.Tag = cSFeedbackQuestionAnswer.AnswerId;
				comboBoxItem.Content = cSFeedbackQuestionAnswer.FreeFormAnswer;
				comboBoxItem.Foreground = new SolidColorBrush(Colors.White);
				cmboAnswer.Items.Add(comboBoxItem);
			}
			break;
		}
		case FeedbackQuestionType.YesNo:
			IsYesNoQuestion = true;
			radioYes.GroupName = Guid.NewGuid().ToString();
			radioNo.GroupName = radioYes.GroupName;
			if (FeedbackQuestion.AnswerChoices[0].FreeFormAnswer.ToLower() == "yes")
			{
				radioYes.Tag = FeedbackQuestion.AnswerChoices[0].AnswerId;
				radioNo.Tag = FeedbackQuestion.AnswerChoices[1].AnswerId;
			}
			else
			{
				radioNo.Tag = FeedbackQuestion.AnswerChoices[0].AnswerId;
				radioYes.Tag = FeedbackQuestion.AnswerChoices[1].AnswerId;
			}
			break;
		case FeedbackQuestionType.Freeform:
			IsFreeFormQuestion = true;
			break;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csfeedbackquestioncontrol.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			CSFeedbackQuestionUserControl = (CSFeedbackQuestionControl)target;
			break;
		case 2:
			LayoutRoot = (DockPanel)target;
			break;
		case 3:
			txtQuestion = (TextBlock)target;
			break;
		case 4:
			borderMultipleChoice = (Border)target;
			break;
		case 5:
			cmboAnswer = (ComboBox)target;
			break;
		case 6:
			borderYesNo = (Border)target;
			break;
		case 7:
			radioYes = (RadioButton)target;
			break;
		case 8:
			radioNo = (RadioButton)target;
			break;
		case 9:
			borderFreeForm = (Border)target;
			break;
		case 10:
			txtFreeform = (TextBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
