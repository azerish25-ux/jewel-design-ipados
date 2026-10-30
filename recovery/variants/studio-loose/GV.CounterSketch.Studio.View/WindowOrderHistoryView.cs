using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class WindowOrderHistoryView : Window, IComponentConnector
{
	internal WindowOrderHistoryView TheWindow;

	internal Button BtnCancel;

	internal ListBox ListboxQuotehistory;

	private bool _contentLoaded;

	public WindowOrderHistoryView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessMessage, false);
	}

	private void ProcessMessage(string message)
	{
		if (message == "close")
		{
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windoworderhistoryview.xaml", UriKind.Relative);
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
			TheWindow = (WindowOrderHistoryView)target;
			break;
		case 2:
			BtnCancel = (Button)target;
			break;
		case 3:
			ListboxQuotehistory = (ListBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
