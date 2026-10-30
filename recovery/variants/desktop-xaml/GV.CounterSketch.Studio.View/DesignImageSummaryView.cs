using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class DesignImageSummaryView : UserControl, IComponentConnector
{
	public DesignImageSummaryView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessMessage);
	}

	private void ProcessMessage(string message)
	{
		if (message != null && message == "loaded")
		{
			spinIndicator.Visibility = Visibility.Collapsed;
		}
	}
}
