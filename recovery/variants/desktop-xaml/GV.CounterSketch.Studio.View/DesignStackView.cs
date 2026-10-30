using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public partial class DesignStackView : UserControl, IComponentConnector
{
	public DesignStackView()
	{
		InitializeComponent();
	}

	private void pnlControls_MouseEnter(object sender, MouseEventArgs e)
	{
		if (Mouse.LeftButton != MouseButtonState.Pressed)
		{
			UIHelper.CoreDataService.DeselectViewportControls();
		}
	}

	private void pnlControls_MouseLeave(object sender, MouseEventArgs e)
	{
		if (Mouse.LeftButton != MouseButtonState.Pressed)
		{
			UIHelper.CoreDataService.ReselectViewportControls();
		}
	}
}
