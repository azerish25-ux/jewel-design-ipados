using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public partial class WindowTileViewParts : Window, IComponentConnector
{
	public WindowTileViewParts()
	{
		InitializeComponent();
		base.Closing += WindowTileViewParts_Closing;
		base.Activated += WindowTileViewParts_Activated;
	}

	private void WindowTileViewParts_Activated(object sender, EventArgs e)
	{
		ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = false;
	}

	private void WindowTileViewParts_Closing(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
		Hide();
		ViewModelLocator.Current.CustomizeVM.HasPartsToolboxBeenShown = false;
	}
}
