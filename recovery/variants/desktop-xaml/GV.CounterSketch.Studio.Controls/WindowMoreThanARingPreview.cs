using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public partial class WindowMoreThanARingPreview : Window, IComponentConnector
{
	public WindowMoreThanARingPreview()
	{
		InitializeComponent();
		string iconPath = UIHelper.KeyProfile.IconPath;
		Uri uriSource = new Uri("pack://application:,,,/CounterSketch Studio;component/Images/" + iconPath);
		base.Icon = new BitmapImage(uriSource);
	}

	private void btnNext_Click(object sender, RoutedEventArgs e)
	{
		book.AnimateToNextPage(fromTop: true, 700);
		if (book.RightPageNumber == book.LastPageNumber - 2)
		{
			btnNext.Opacity = 0.0;
		}
		else
		{
			btnNext.Opacity = 1.0;
		}
		btnPrevious.Opacity = 1.0;
	}

	private void btnPrevious_Click(object sender, RoutedEventArgs e)
	{
		book.AnimateToPreviousPage(fromTop: true, 700);
		if (book.LeftPageNumber < book.FirstPageNumber + 2)
		{
			btnPrevious.Opacity = 0.0;
		}
		else
		{
			btnPrevious.Opacity = 1.0;
		}
		btnNext.Opacity = 1.0;
	}
}
