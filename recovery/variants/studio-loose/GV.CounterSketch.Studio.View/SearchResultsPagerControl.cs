using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public class SearchResultsPagerControl : UserControl, IComponentConnector
{
	internal Button ButtonPrevious;

	internal Button ButtonFirst;

	internal Button ButtonLast;

	internal Button ButtonNext;

	private bool _contentLoaded;

	public SearchResultsPagerControl()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/searchresultspagercontrol.xaml", UriKind.Relative);
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
			ButtonPrevious = (Button)target;
			break;
		case 2:
			ButtonFirst = (Button)target;
			break;
		case 3:
			ButtonLast = (Button)target;
			break;
		case 4:
			ButtonNext = (Button)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
