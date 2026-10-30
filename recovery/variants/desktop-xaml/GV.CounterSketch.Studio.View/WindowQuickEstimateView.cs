using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public partial class WindowQuickEstimateView : Window, IComponentConnector
{
	public WindowQuickEstimateView()
	{
		InitializeComponent();
		base.Loaded += WindowQuickEstimateView_Loaded;
		if (SecurityHelper.IsStreaming)
		{
			button.Visibility = Visibility.Collapsed;
			CheckBoxIncludeScreenshotWhilePrinting.Visibility = Visibility.Collapsed;
		}
	}

	private void WindowQuickEstimateView_Loaded(object sender, RoutedEventArgs e)
	{
		if (ViewModelLocator.Current.QuickEstimateVM.IsMissingGems)
		{
			GlobalErrorHandler.WriteInfoToEventLog(VisualStateManager.GoToElementState(RootGrid, "MissingGems", useTransitions: false).ToString(), 2);
		}
		else
		{
			GlobalErrorHandler.WriteInfoToEventLog(VisualStateManager.GoToElementState(RootGrid, "Default", useTransitions: false).ToString(), 2);
		}
	}
}
