using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class DesignImageSummaryView : UserControl, IComponentConnector
{
	internal Spinner spinIndicator;

	private bool _contentLoaded;

	public DesignImageSummaryView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessMessage, false);
	}

	private void ProcessMessage(string message)
	{
		if (message != null && message == "loaded")
		{
			spinIndicator.Visibility = Visibility.Collapsed;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/designimagesummaryview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		if (connectionId == 1)
		{
			spinIndicator = (Spinner)target;
		}
		else
		{
			_contentLoaded = true;
		}
	}
}
