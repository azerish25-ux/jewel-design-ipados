using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public class FilterResultsView : UserControl, IComponentConnector
{
	internal Viewbox CsButtonViewPerspectiveDefaultCopy;

	internal Viewbox CsButtonViewTopDownDefaultCopy;

	internal Viewbox CsButtonViewThroughFingerDefaultCopy;

	internal Viewbox CsButtonViewSideDefault;

	private bool _contentLoaded;

	public FilterResultsView()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/filterresultsview.xaml", UriKind.Relative);
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
			CsButtonViewPerspectiveDefaultCopy = (Viewbox)target;
			break;
		case 2:
			CsButtonViewTopDownDefaultCopy = (Viewbox)target;
			break;
		case 3:
			CsButtonViewThroughFingerDefaultCopy = (Viewbox)target;
			break;
		case 4:
			CsButtonViewSideDefault = (Viewbox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
