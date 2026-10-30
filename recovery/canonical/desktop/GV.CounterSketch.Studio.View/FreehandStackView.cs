using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public class FreehandStackView : UserControl, IComponentConnector
{
	internal TextBlock TxtTitle;

	internal ListBox FreehandStackListBox;

	private bool _contentLoaded;

	public FreehandStackView()
	{
		InitializeComponent();
	}

	private void freehandStackListBox_MouseEnter(object sender, MouseEventArgs e)
	{
		if (Mouse.LeftButton != MouseButtonState.Pressed)
		{
			UIHelper.CoreDataService.DeselectViewportControls();
		}
	}

	private void freehandStackListBox_MouseLeave(object sender, MouseEventArgs e)
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/freehandstackview.xaml", UriKind.Relative);
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
			TxtTitle = (TextBlock)target;
			break;
		case 2:
			FreehandStackListBox = (ListBox)target;
			FreehandStackListBox.MouseEnter += freehandStackListBox_MouseEnter;
			FreehandStackListBox.MouseLeave += freehandStackListBox_MouseLeave;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
