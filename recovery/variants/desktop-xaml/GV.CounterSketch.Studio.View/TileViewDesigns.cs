using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public partial class TileViewDesigns : UserControl, IComponentConnector
{
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
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
