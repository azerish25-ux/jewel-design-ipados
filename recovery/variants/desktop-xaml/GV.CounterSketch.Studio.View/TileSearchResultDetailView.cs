using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public partial class TileSearchResultDetailView : UserControl, IComponentConnector
{
	public TileSearchResultDetailView()
	{
		InitializeComponent();
	}

	private void Grid_TouchDown(object sender, TouchEventArgs e)
	{
	}
}
