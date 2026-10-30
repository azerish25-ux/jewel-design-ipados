using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public partial class CustomizePartsListView : UserControl, IComponentConnector
{
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
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
