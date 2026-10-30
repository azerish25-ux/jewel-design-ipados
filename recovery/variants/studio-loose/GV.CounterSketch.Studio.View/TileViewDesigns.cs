using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
		object originalSource = e.OriginalSource;
		Tile val = (Tile)((originalSource is Tile) ? originalSource : null);
		if (val != null)
		{
			ICommand command = val.Command;
			object commandParameter = val.CommandParameter;
			if (command.CanExecute(null))
			{
				command.Execute(commandParameter);
			}
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
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
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
			((UIElement)(TileLayoutControl)target).TouchUp += TileLayoutControl_TouchUp;
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
