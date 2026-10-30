using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Controls;

public class StoneReport : UserControl, IComponentConnector
{
	public static readonly DependencyProperty IsStandaloneProperty = DependencyProperty.Register("IsStandalone", typeof(bool), typeof(StoneReport));

	internal StoneReport stoneReportControl;

	internal VisualStateGroup VisualStateGroup;

	internal VisualState Default;

	internal VisualState IndividualStones;

	internal Border DontDeleteThisBorder;

	internal Grid StoneReportHeaderRow;

	internal Border Sub_Total;

	internal TextBlock txtCenter;

	internal TextBlock txtCenterQty;

	internal TextBlock txtMelee;

	internal TextBlock txtMeleeQty;

	internal TextBlock txtLarge;

	internal TextBlock txtLargeQty;

	internal TextBlock txtTotal;

	internal TextBlock txtTotalQty;

	internal ListBox StoneGroupReportListBox;

	private bool _contentLoaded;

	public bool IsStandalone
	{
		get
		{
			return (bool)GetValue(IsStandaloneProperty);
		}
		set
		{
			SetValue(IsStandaloneProperty, value);
		}
	}

	public StoneReport()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/stonereport.xaml", UriKind.Relative);
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
			stoneReportControl = (StoneReport)target;
			break;
		case 2:
			VisualStateGroup = (VisualStateGroup)target;
			break;
		case 3:
			Default = (VisualState)target;
			break;
		case 4:
			IndividualStones = (VisualState)target;
			break;
		case 5:
			DontDeleteThisBorder = (Border)target;
			break;
		case 6:
			StoneReportHeaderRow = (Grid)target;
			break;
		case 7:
			Sub_Total = (Border)target;
			break;
		case 8:
			txtCenter = (TextBlock)target;
			break;
		case 9:
			txtCenterQty = (TextBlock)target;
			break;
		case 10:
			txtMelee = (TextBlock)target;
			break;
		case 11:
			txtMeleeQty = (TextBlock)target;
			break;
		case 12:
			txtLarge = (TextBlock)target;
			break;
		case 13:
			txtLargeQty = (TextBlock)target;
			break;
		case 14:
			txtTotal = (TextBlock)target;
			break;
		case 15:
			txtTotalQty = (TextBlock)target;
			break;
		case 16:
			StoneGroupReportListBox = (ListBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
