using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio.View;

public class UpdateInfoView : UserControl, IComponentConnector
{
	internal Popup popup;

	internal Grid grid;

	internal StackPanel UpdateStatuses;

	internal Grid Action_Buttons;

	internal RadBusyIndicator radBusyIndicator;

	internal Grid LEDIndicator;

	internal Border alignmentBorder;

	internal VisualStateGroup UpdateStates;

	internal VisualState Normal;

	internal VisualState Updating;

	internal VisualState Completed;

	private bool _contentLoaded;

	public UpdateInfoView()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/updateinfoview.xaml", UriKind.Relative);
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
			popup = (Popup)target;
			break;
		case 2:
			grid = (Grid)target;
			break;
		case 3:
			UpdateStatuses = (StackPanel)target;
			break;
		case 4:
			Action_Buttons = (Grid)target;
			break;
		case 5:
			radBusyIndicator = (RadBusyIndicator)target;
			break;
		case 6:
			LEDIndicator = (Grid)target;
			break;
		case 7:
			alignmentBorder = (Border)target;
			break;
		case 8:
			UpdateStates = (VisualStateGroup)target;
			break;
		case 9:
			Normal = (VisualState)target;
			break;
		case 10:
			Updating = (VisualState)target;
			break;
		case 11:
			Completed = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
