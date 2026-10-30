using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using GV.CounterSketch.Studio.UserControls.ScrollableListBox;

namespace GV.CounterSketch.Studio.View;

public class PartsToolBoxView : UserControl, IComponentConnector
{
	internal Grid Grid;

	internal ListBox PartsTabList;

	internal ScrollableListBox PartsList;

	internal Border Border;

	internal Rectangle Rectangle;

	internal Border LineBar;

	internal Viewbox Viewbox;

	internal ListBox ListBox;

	internal Button BtnReload;

	internal VisualStateGroup TabStates;

	internal VisualState TabsClosed;

	internal VisualState PartTabsOpen;

	internal VisualStateGroup GemTaStates;

	private bool _contentLoaded;

	public PartsToolBoxView()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/partstoolboxview.xaml", UriKind.Relative);
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
			Grid = (Grid)target;
			break;
		case 2:
			PartsTabList = (ListBox)target;
			break;
		case 3:
			PartsList = (ScrollableListBox)target;
			break;
		case 4:
			Border = (Border)target;
			break;
		case 5:
			Rectangle = (Rectangle)target;
			break;
		case 6:
			LineBar = (Border)target;
			break;
		case 7:
			Viewbox = (Viewbox)target;
			break;
		case 8:
			ListBox = (ListBox)target;
			break;
		case 9:
			BtnReload = (Button)target;
			break;
		case 10:
			TabStates = (VisualStateGroup)target;
			break;
		case 11:
			TabsClosed = (VisualState)target;
			break;
		case 12:
			PartTabsOpen = (VisualState)target;
			break;
		case 13:
			GemTaStates = (VisualStateGroup)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
