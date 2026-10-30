using System;
using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public partial class WindowExportDesign : Window, IComponentConnector
{
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
}
