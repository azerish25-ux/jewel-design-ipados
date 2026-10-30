using System.Windows;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public partial class WindowDesignNotes : Window, IComponentConnector
{
	public WindowDesignNotes()
	{
		InitializeComponent();
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}
}
