using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class WindowDesignTryOnView : Window, IComponentConnector
{
	public WindowDesignTryOnView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessGenericMessage);
	}

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
	{
		Hide();
	}

	private void ProcessGenericMessage(string message)
	{
		if (message != null && message == "CloseWindow")
		{
			Hide();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
