using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public class WindowAboutView : Window, IComponentConnector
{
	internal WindowAboutView Window;

	internal Image Background_Image;

	internal Button btnCancel;

	internal Viewbox CSButtonCloseAbout;

	internal ContentControl GVLOGO;

	internal StackPanel Progress_Bar;

	private bool _contentLoaded;

	public WindowAboutView()
	{
		InitializeComponent();
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow(e.Uri.ToString());
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowaboutview.xaml", UriKind.Relative);
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
			Window = (WindowAboutView)target;
			break;
		case 2:
			Background_Image = (Image)target;
			break;
		case 3:
			btnCancel = (Button)target;
			break;
		case 4:
			CSButtonCloseAbout = (Viewbox)target;
			break;
		case 5:
			GVLOGO = (ContentControl)target;
			break;
		case 6:
			Progress_Bar = (StackPanel)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
