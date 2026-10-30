using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public class OrderEstimateDebugWindow : Window, IComponentConnector
{
	internal Border TopBar;

	internal Button buttonCancel;

	internal Border WindowFrame;

	private bool _contentLoaded;

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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/orderestimatedebugwindow.xaml", UriKind.Relative);
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
			TopBar = (Border)target;
			break;
		case 2:
			buttonCancel = (Button)target;
			break;
		case 3:
			WindowFrame = (Border)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
