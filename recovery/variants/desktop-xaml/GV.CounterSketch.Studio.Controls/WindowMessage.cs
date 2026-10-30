using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public partial class WindowMessage : Window, IComponentConnector
{
	public enum WindowType
	{
		MessageOnly,
		Confirmation,
		QuestionAnswer
	}

	public static readonly DependencyProperty AnswerProperty = DependencyProperty.Register("Answer", typeof(string), typeof(WindowMessage), new UIPropertyMetadata(""));

	public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(WindowMessage));

	public static readonly DependencyProperty QuestionProperty = DependencyProperty.Register("Question", typeof(string), typeof(WindowMessage), new UIPropertyMetadata(""));

	public string Answer
	{
		get
		{
			return (string)GetValue(AnswerProperty);
		}
		set
		{
			SetValue(AnswerProperty, value);
		}
	}

	public string Message
	{
		get
		{
			return (string)GetValue(MessageProperty);
		}
		set
		{
			SetValue(MessageProperty, value);
			if (!value.StartsWith("An Application Error has occurred."))
			{
				MessageScroller.ScrollToEnd();
			}
		}
	}

	public string Question
	{
		get
		{
			return (string)GetValue(QuestionProperty);
		}
		set
		{
			SetValue(QuestionProperty, value);
		}
	}

	public bool? IsAccepted { get; set; }

	public bool OKButtonEnabled
	{
		get
		{
			return BtnOk.IsEnabled;
		}
		set
		{
			BtnOk.IsEnabled = value;
			BtnOk.Visibility = ((!BtnOk.IsEnabled) ? Visibility.Collapsed : Visibility.Visible);
			DispatcherHelper.WaitForPriority();
		}
	}

	public bool? IsCancelled { get; set; }

	public WindowMessage(WindowType windowType)
		: this()
	{
		SetWindowType(windowType);
	}

	public WindowMessage()
	{
		InitializeComponent();
		SetWindowType(WindowType.MessageOnly);
		TxtAnswer.Focus();
	}

	public void SetOKText(string text)
	{
		BtnOk.Content = text;
	}

	private void BtnNotOKClick(object sender, RoutedEventArgs e)
	{
		IsAccepted = false;
		IsCancelled = false;
		Close();
	}

	private void BtnOKClick(object sender, RoutedEventArgs e)
	{
		IsAccepted = true;
		IsCancelled = false;
		Answer = TxtAnswer.Text;
		Close();
	}

	private void ButtonCancelClick(object sender, RoutedEventArgs e)
	{
		IsAccepted = false;
		IsCancelled = true;
		Close();
	}

	private void SetWindowType(WindowType windowType)
	{
		switch (windowType)
		{
		case WindowType.MessageOnly:
			Question = "Information:";
			BtnOk.Content = "OK";
			BtnNotOk.Visibility = Visibility.Collapsed;
			BtnCancel.Visibility = Visibility.Collapsed;
			TxtQuestion.Visibility = Visibility.Collapsed;
			TxtAnswer.Visibility = Visibility.Collapsed;
			TxtMessage.Visibility = Visibility.Visible;
			break;
		case WindowType.Confirmation:
			Question = "Confirm?";
			BtnOk.Content = "YES";
			BtnNotOk.Visibility = Visibility.Visible;
			BtnCancel.Visibility = Visibility.Visible;
			TxtQuestion.Visibility = Visibility.Collapsed;
			TxtAnswer.Visibility = Visibility.Collapsed;
			TxtMessage.Visibility = Visibility.Visible;
			break;
		case WindowType.QuestionAnswer:
			BtnOk.Content = "SUBMIT";
			BtnNotOk.Visibility = Visibility.Collapsed;
			BtnCancel.Visibility = Visibility.Visible;
			TxtQuestion.Visibility = Visibility.Visible;
			TxtAnswer.Visibility = Visibility.Visible;
			TxtMessage.Visibility = Visibility.Collapsed;
			break;
		default:
			throw new ArgumentOutOfRangeException("windowType");
		}
	}

	private void TxtAnswer_OnKeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			e.Handled = true;
			IsAccepted = true;
			IsCancelled = false;
			Answer = TxtAnswer.Text;
			Close();
		}
	}
}
