using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using GV.CounterSketch.Studio.UserControls.PagedListBox;

namespace GV.CounterSketch.Studio.View;

public class MaterialOptionSelectorView : UserControl, IComponentConnector
{
	internal PagedListBox pagedListBox;

	private bool _contentLoaded;

	public MaterialOptionSelectorView()
	{
		InitializeComponent();
	}

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
	{
		Window window = Window.GetWindow(this);
		if (window != null && !ComponentDispatcher.IsThreadModal)
		{
			window.Close();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/materialoptionselectorview.xaml", UriKind.Relative);
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
			pagedListBox = (PagedListBox)target;
			break;
		case 2:
			((Button)target).Click += ButtonBase_OnClick;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
