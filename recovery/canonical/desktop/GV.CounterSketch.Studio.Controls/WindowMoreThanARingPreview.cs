using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Utilities.Helpers;
using WPFMitsuControls;

namespace GV.CounterSketch.Studio.Controls;

public class WindowMoreThanARingPreview : Window, IComponentConnector
{
	internal WindowMoreThanARingPreview theWindow;

	internal Grid grid;

	internal Button btnNext;

	internal Grid Next;

	internal Button btnPrevious;

	internal Grid Previous;

	internal Book book;

	private bool _contentLoaded;

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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowmorethanaringpreview.xaml", UriKind.Relative);
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
			theWindow = (WindowMoreThanARingPreview)target;
			break;
		case 2:
			grid = (Grid)target;
			break;
		case 3:
			btnNext = (Button)target;
			btnNext.Click += btnNext_Click;
			break;
		case 4:
			Next = (Grid)target;
			break;
		case 5:
			btnPrevious = (Button)target;
			btnPrevious.Click += btnPrevious_Click;
			break;
		case 6:
			Previous = (Grid)target;
			break;
		case 7:
			book = (Book)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
