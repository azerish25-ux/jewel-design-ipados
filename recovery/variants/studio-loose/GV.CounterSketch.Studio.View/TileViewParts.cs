using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using Microsoft.Expression.Interactivity.Media;

namespace GV.CounterSketch.Studio.View;

public class TileViewParts : UserControl, IComponentConnector
{
	internal Grid GridMainParts;

	internal ControlStoryboardAction ScrollSectionAnimation;

	internal StackPanel StackPanelParts;

	internal Grid TileUIGrid;

	internal Border ToolboxBorder;

	internal Button AddCustomTile;

	internal Border FixedTools;

	internal Viewbox TileLayoutControlViewbox;

	internal Popup Popup;

	internal StackPanel EditTemplatesPanel;

	internal StackPanel EditActionsPanel;

	internal Grid SearchPartsViewGrid;

	private bool _contentLoaded;

	public TileViewParts()
	{
		InitializeComponent();
		if (SecurityHelper.IsStreaming)
		{
			AddCustomTile.Visibility = Visibility.Collapsed;
		}
	}

	private void Storyboard_Completed(object sender, EventArgs e)
	{
		if (ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.IsShowingPartSearchResults)
		{
			ViewModelLocator.Current.TileSearchMainVM.PopulateSearchResults();
		}
		ViewModelLocator.Current.TileViewPartsVM.SetSectionTransitionTimeToNormal();
	}

	private void TileLayoutControl_TouchUp(object sender, TouchEventArgs e)
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/tileviewparts.xaml", UriKind.Relative);
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
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			GridMainParts = (Grid)target;
			break;
		case 2:
			ScrollSectionAnimation = (ControlStoryboardAction)target;
			break;
		case 3:
			((Storyboard)target).Completed += Storyboard_Completed;
			break;
		case 4:
			StackPanelParts = (StackPanel)target;
			break;
		case 5:
			TileUIGrid = (Grid)target;
			break;
		case 6:
			ToolboxBorder = (Border)target;
			break;
		case 7:
			AddCustomTile = (Button)target;
			break;
		case 8:
			FixedTools = (Border)target;
			break;
		case 9:
			TileLayoutControlViewbox = (Viewbox)target;
			break;
		case 10:
			((UIElement)(TileLayoutControl)target).TouchUp += TileLayoutControl_TouchUp;
			break;
		case 11:
			Popup = (Popup)target;
			break;
		case 12:
			EditTemplatesPanel = (StackPanel)target;
			break;
		case 13:
			EditActionsPanel = (StackPanel)target;
			break;
		case 14:
			SearchPartsViewGrid = (Grid)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
