using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public class WindowMessage : Window, IComponentConnector
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

	internal WindowMessage TheWindow;

	internal StackPanel StackPanel;

	internal DockPanel MessageHeader;

	internal TextBlock TxtQuestion;

	internal StackPanel MessageContent;

	internal ScrollViewer MessageScroller;

	internal TextBlock TxtMessage;

	internal TextBox TxtAnswer;

	internal StackPanel ActionButtons;

	internal Button BtnOk;

	internal Button BtnNotOk;

	internal Button BtnCancel;

	private bool _contentLoaded;

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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowmessage.xaml", UriKind.Relative);
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
			TheWindow = (WindowMessage)target;
			break;
		case 2:
			StackPanel = (StackPanel)target;
			break;
		case 3:
			MessageHeader = (DockPanel)target;
			break;
		case 4:
			TxtQuestion = (TextBlock)target;
			break;
		case 5:
			MessageContent = (StackPanel)target;
			break;
		case 6:
			MessageScroller = (ScrollViewer)target;
			break;
		case 7:
			TxtMessage = (TextBlock)target;
			break;
		case 8:
			TxtAnswer = (TextBox)target;
			TxtAnswer.KeyUp += TxtAnswer_OnKeyUp;
			break;
		case 9:
			ActionButtons = (StackPanel)target;
			break;
		case 10:
			BtnOk = (Button)target;
			BtnOk.Click += BtnOKClick;
			break;
		case 11:
			BtnNotOk = (Button)target;
			BtnNotOk.Click += BtnNotOKClick;
			break;
		case 12:
			BtnCancel = (Button)target;
			BtnCancel.Click += ButtonCancelClick;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
