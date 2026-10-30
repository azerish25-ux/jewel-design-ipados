using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public partial class WindowNewLibrary : Window, IComponentConnector
{
	public WindowNewLibrary()
	{
		InitializeComponent();
	}

	private void WindowNewLibrary_OnClosing(object sender, CancelEventArgs e)
	{
		ViewModelLocator.Current.UpdateInfoVM.StopNewImageTimer();
	}
}
