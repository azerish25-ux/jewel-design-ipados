using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public class TileSearchResultDetailView : UserControl, IComponentConnector
{
	internal TileSearchResultDetailView UserControl;

	internal DockPanel DesignImages;

	internal StackPanel SelectedDesignInfo;

	internal TextBox TextBox;

	internal TextBox TextBox1;

	internal TextBox TextBox2;

	private bool _contentLoaded;

	public TileSearchResultDetailView()
	{
		InitializeComponent();
	}

	private void Grid_TouchDown(object sender, TouchEventArgs e)
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/tilesearchresultdetailview.xaml", UriKind.Relative);
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
			UserControl = (TileSearchResultDetailView)target;
			break;
		case 2:
			DesignImages = (DockPanel)target;
			break;
		case 3:
			SelectedDesignInfo = (StackPanel)target;
			break;
		case 4:
			TextBox = (TextBox)target;
			break;
		case 5:
			TextBox1 = (TextBox)target;
			break;
		case 6:
			TextBox2 = (TextBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
