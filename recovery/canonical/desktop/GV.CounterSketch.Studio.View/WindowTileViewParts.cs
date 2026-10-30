using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public class WindowTileViewParts : Window, IComponentConnector
{
	internal Border WaitMessageBorder;

	private bool _contentLoaded;

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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowtileviewparts.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		if (connectionId == 1)
		{
			WaitMessageBorder = (Border)target;
		}
		else
		{
			_contentLoaded = true;
		}
	}
}
