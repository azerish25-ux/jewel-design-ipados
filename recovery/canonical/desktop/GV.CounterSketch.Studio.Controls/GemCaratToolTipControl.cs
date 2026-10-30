using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Controls;

public class GemCaratToolTipControl : UserControl, IComponentConnector
{
	internal GemCaratToolTipControl UserControl;

	internal Grid LayoutRoot;

	private bool _contentLoaded;

	public GemCaratToolTipControl()
	{
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/gemcarattooltipcontrol.xaml", UriKind.Relative);
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
			UserControl = (GemCaratToolTipControl)target;
			break;
		case 2:
			LayoutRoot = (Grid)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
