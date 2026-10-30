using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public class DesignStackView : UserControl, IComponentConnector
{
	internal DesignStackView UserControl;

	internal Path Path_0;

	internal Path Path;

	internal Path BorderExpanded;

	internal Border BorderBottom;

	internal ListBox PnlControls;

	private bool _contentLoaded;

	public DesignStackView()
	{
		InitializeComponent();
	}

	private void pnlControls_MouseEnter(object sender, MouseEventArgs e)
	{
		if (Mouse.LeftButton != MouseButtonState.Pressed)
		{
			UIHelper.CoreDataService.DeselectViewportControls();
		}
	}

	private void pnlControls_MouseLeave(object sender, MouseEventArgs e)
	{
		if (Mouse.LeftButton != MouseButtonState.Pressed)
		{
			UIHelper.CoreDataService.ReselectViewportControls();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/designstackview.xaml", UriKind.Relative);
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
			UserControl = (DesignStackView)target;
			break;
		case 2:
			Path_0 = (Path)target;
			break;
		case 3:
			Path = (Path)target;
			break;
		case 4:
			BorderExpanded = (Path)target;
			break;
		case 5:
			BorderBottom = (Border)target;
			break;
		case 6:
			PnlControls = (ListBox)target;
			PnlControls.MouseEnter += pnlControls_MouseEnter;
			PnlControls.MouseLeave += pnlControls_MouseLeave;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
