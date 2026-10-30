using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public partial class OrderEstimateDebugWindow : Window, IComponentConnector
{
	public OrderEstimateDebugWindow()
	{
		InitializeComponent();
		base.Closing += Window_Closing;
		string iconPath = UIHelper.KeyProfile.IconPath;
		Uri uriSource = new Uri("pack://application:,,,/CounterSketch Studio;component/Images/" + iconPath);
		base.Icon = new BitmapImage(uriSource);
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
		base.Visibility = Visibility.Hidden;
	}
}
