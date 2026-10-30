using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public class BreadCrumbView : UserControl, IComponentConnector
{
	internal Button ButtonOrder;

	internal Button ButtonCustomize;

	internal Button ButtonSearch;

	internal Button ButtonHome;

	private bool _contentLoaded;

	public BreadCrumbView()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/breadcrumbview.xaml", UriKind.Relative);
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
			ButtonOrder = (Button)target;
			break;
		case 2:
			ButtonCustomize = (Button)target;
			break;
		case 3:
			ButtonSearch = (Button)target;
			break;
		case 4:
			ButtonHome = (Button)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
