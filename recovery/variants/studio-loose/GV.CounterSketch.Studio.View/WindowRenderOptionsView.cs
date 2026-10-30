using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class WindowRenderOptionsView : Window, IComponentConnector
{
	internal WindowRenderOptionsView TheWindow;

	internal CheckBox CheckBox;

	internal CheckBox CheckBox_Copy;

	internal CheckBox CheckBox_Copy1;

	internal Button BtnRenderSettings;

	private bool _contentLoaded;

	public WindowRenderOptionsView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessGenericMessage, false);
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
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowrenderoptionsview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			TheWindow = (WindowRenderOptionsView)target;
			break;
		case 2:
			CheckBox = (CheckBox)target;
			break;
		case 3:
			CheckBox_Copy = (CheckBox)target;
			break;
		case 4:
			CheckBox_Copy1 = (CheckBox)target;
			break;
		case 5:
			BtnRenderSettings = (Button)target;
			break;
		case 6:
			((Button)target).Click += ButtonBase_OnClick;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
