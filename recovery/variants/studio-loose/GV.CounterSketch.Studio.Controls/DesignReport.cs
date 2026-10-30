using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Controls;

public class DesignReport : UserControl, IComponentConnector
{
	internal DesignReport DesignReportControl;

	internal ListBox LargeOrderedStones;

	internal ListBox LargeStoneReportListBox;

	internal Grid MountingGrid;

	internal Grid MountingFinish;

	internal ComboBox ComboFinish;

	internal Grid MountingComponents;

	internal Grid MountingSize;

	internal ComboBox RingSizeSelection;

	internal Grid MountingModel;

	internal Grid MountingMetalPrimary;

	internal Image ImagePrimary;

	internal ComboBox ComboBoxPrimary;

	internal Grid MountingMetalSecondary;

	internal Image ImageSecondary;

	internal ComboBox ComboBoxSecondary;

	internal CheckBox CheckBox;

	internal ListBox MeleeOrderedStones;

	internal ListBox StoneGroupReportListBox;

	internal VisualStateGroup VisualStateGroup;

	internal VisualState Default;

	internal VisualState Ordered;

	private bool _contentLoaded;

	public DesignReport()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/designreport.xaml", UriKind.Relative);
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
			DesignReportControl = (DesignReport)target;
			break;
		case 2:
			LargeOrderedStones = (ListBox)target;
			break;
		case 3:
			LargeStoneReportListBox = (ListBox)target;
			break;
		case 4:
			MountingGrid = (Grid)target;
			break;
		case 5:
			MountingFinish = (Grid)target;
			break;
		case 6:
			ComboFinish = (ComboBox)target;
			break;
		case 7:
			MountingComponents = (Grid)target;
			break;
		case 8:
			MountingSize = (Grid)target;
			break;
		case 9:
			RingSizeSelection = (ComboBox)target;
			break;
		case 10:
			MountingModel = (Grid)target;
			break;
		case 11:
			MountingMetalPrimary = (Grid)target;
			break;
		case 12:
			ImagePrimary = (Image)target;
			break;
		case 13:
			ComboBoxPrimary = (ComboBox)target;
			break;
		case 14:
			MountingMetalSecondary = (Grid)target;
			break;
		case 15:
			ImageSecondary = (Image)target;
			break;
		case 16:
			ComboBoxSecondary = (ComboBox)target;
			break;
		case 17:
			CheckBox = (CheckBox)target;
			break;
		case 18:
			MeleeOrderedStones = (ListBox)target;
			break;
		case 19:
			StoneGroupReportListBox = (ListBox)target;
			break;
		case 20:
			VisualStateGroup = (VisualStateGroup)target;
			break;
		case 21:
			Default = (VisualState)target;
			break;
		case 22:
			Ordered = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
