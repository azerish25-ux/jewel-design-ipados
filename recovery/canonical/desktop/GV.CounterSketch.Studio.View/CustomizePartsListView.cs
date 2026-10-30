using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.AttachedBehaviors;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public class CustomizePartsListView : UserControl, IComponentConnector
{
	internal DevExpressReorderDragDropManager manager1;

	private bool _contentLoaded;

	public CustomizePartsListView()
	{
		InitializeComponent();
	}

	private void Control_MouseEnter(object sender, MouseEventArgs e)
	{
		if (Mouse.LeftButton != MouseButtonState.Pressed)
		{
			UIHelper.CoreDataService.ReselectViewportControls();
		}
	}

	private void Control_MouseLeave(object sender, MouseEventArgs e)
	{
		if (Mouse.LeftButton != MouseButtonState.Pressed)
		{
			UIHelper.CoreDataService.DeselectViewportControls();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/customizepartslistview.xaml", UriKind.Relative);
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
		if (connectionId == 1)
		{
			manager1 = (DevExpressReorderDragDropManager)target;
		}
		else
		{
			_contentLoaded = true;
		}
	}
}
