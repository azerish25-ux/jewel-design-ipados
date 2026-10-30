using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public class WindowExportDesign : Window, IComponentConnector
{
	internal Button button_Copy;

	internal Button button;

	internal TextBlock textBlock;

	internal RadioButton radioButton;

	internal RadioButton radioButton1;

	internal TextBlock textBlock3;

	internal TextBlock textBlock1;

	internal CheckBox checkBox;

	internal CheckBox checkBox1;

	internal CheckBox checkBox2;

	internal TextBlock textBlock2;

	internal RadioButton radioButton3;

	internal RadioButton radioButton2;

	internal CheckBox checkBox3;

	internal StackPanel InternationalExportStats;

	internal StackPanel FeatureExportHistory;

	private bool _contentLoaded;

	public WindowExportDesign()
	{
		InitializeComponent();
		base.Loaded += WindowExportDesign_Loaded;
		base.Closed += WindowExportDesign_Closed;
	}

	private void WindowExportDesign_Closed(object sender, EventArgs e)
	{
		UIHelper.CoreDataService.SetCAMModeState(CAMModeOn: false);
		ViewModelLocator.Current.OrderSummaryVM.ExportWindow = null;
	}

	private void WindowExportDesign_Loaded(object sender, RoutedEventArgs e)
	{
		ViewModelLocator.Current.OrderSummaryVM.ExportWindow = this;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowexportdesign.xaml", UriKind.Relative);
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
			button_Copy = (Button)target;
			break;
		case 2:
			button = (Button)target;
			break;
		case 3:
			textBlock = (TextBlock)target;
			break;
		case 4:
			radioButton = (RadioButton)target;
			break;
		case 5:
			radioButton1 = (RadioButton)target;
			break;
		case 6:
			textBlock3 = (TextBlock)target;
			break;
		case 7:
			textBlock1 = (TextBlock)target;
			break;
		case 8:
			checkBox = (CheckBox)target;
			break;
		case 9:
			checkBox1 = (CheckBox)target;
			break;
		case 10:
			checkBox2 = (CheckBox)target;
			break;
		case 11:
			textBlock2 = (TextBlock)target;
			break;
		case 12:
			radioButton3 = (RadioButton)target;
			break;
		case 13:
			radioButton2 = (RadioButton)target;
			break;
		case 14:
			checkBox3 = (CheckBox)target;
			break;
		case 15:
			InternationalExportStats = (StackPanel)target;
			break;
		case 16:
			FeatureExportHistory = (StackPanel)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
