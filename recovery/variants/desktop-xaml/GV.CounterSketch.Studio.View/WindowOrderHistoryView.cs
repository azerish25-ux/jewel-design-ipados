using System.Windows;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class WindowOrderHistoryView : Window, IComponentConnector
{
	public WindowOrderHistoryView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessMessage);
	}

	private void ProcessMessage(string message)
	{
		if (message == "close")
		{
			Close();
		}
	}
}
