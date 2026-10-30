using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public class TileViewDesigns : UserControl, IComponentConnector
{
	internal Grid TileUIGrid;

	internal Border ToolboxBorder;

	internal Button AddCustomTile;

	internal Popup Popup;

	internal StackPanel EditTemplatesPanel;

	internal StackPanel EditActionsPanel;

	private bool _contentLoaded;

	public TileViewDesigns()
	{
		InitializeComponent();
		if (SecurityHelper.IsStreaming)
		{
			AddCustomTile.Visibility = Visibility.Collapsed;
		}
	}

	private void TileLayoutControl_TouchUp(object sender, RoutedEventArgs e)
	{
		if (e.OriginalSource is Tile { Command: var command, CommandParameter: var commandParameter } && command.CanExecute(null))
		{
			command.Execute(commandParameter);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/tileviewdesigns.xaml", UriKind.Relative);
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
			TileUIGrid = (Grid)target;
			break;
		case 2:
			ToolboxBorder = (Border)target;
			break;
		case 3:
			AddCustomTile = (Button)target;
			break;
		case 4:
			((TileLayoutControl)target).TouchUp += TileLayoutControl_TouchUp;
			break;
		case 5:
			Popup = (Popup)target;
			break;
		case 6:
			EditTemplatesPanel = (StackPanel)target;
			break;
		case 7:
			EditActionsPanel = (StackPanel)target;
			break;
		case 8:
			((Button)target).Click += TileLayoutControl_TouchUp;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
