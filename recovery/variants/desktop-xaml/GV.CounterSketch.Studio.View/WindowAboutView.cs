using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public partial class WindowAboutView : Window, IComponentConnector
{
	public WindowAboutView()
	{
		InitializeComponent();
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow(e.Uri.ToString());
	}
}
