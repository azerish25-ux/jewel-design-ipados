using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace GV.CounterSketch.Studio.View;

public class WindowDebugDesignInfo : Window, IComponentConnector
{
	internal DockPanel mainDock;

	internal DockPanel ActionButtons;

	internal Button btnClose;

	internal Button btnCopyText;

	internal Path Bar;

	internal ListBox listBox;

	internal VisualStateGroup VisualStateGroup;

	internal VisualState Default;

	internal VisualState Loading;

	private bool _contentLoaded;

	public WindowDebugDesignInfo()
	{
		InitializeComponent();
	}

	private void BtnCloseClick(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void btnCopyText_Click(object sender, RoutedEventArgs e)
	{
		CopySelectedItemsToClipboard();
	}

	private void CopyCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
	{
		if ((e.OriginalSource as ListBox).SelectedItems.Count > 0)
		{
			e.CanExecute = true;
		}
		else
		{
			e.CanExecute = false;
		}
	}

	private void CopyCmdExecuted(object sender, ExecutedRoutedEventArgs e)
	{
		CopySelectedItemsToClipboard();
	}

	private void CopySelectedItemsToClipboard()
	{
		string text = string.Empty;
		foreach (object selectedItem in listBox.SelectedItems)
		{
			if (selectedItem != null)
			{
				text += selectedItem;
				text += Environment.NewLine;
			}
		}
		Clipboard.SetText(text);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowdebugdesigninfo.xaml", UriKind.Relative);
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
			mainDock = (DockPanel)target;
			break;
		case 2:
			ActionButtons = (DockPanel)target;
			break;
		case 3:
			btnClose = (Button)target;
			btnClose.Click += BtnCloseClick;
			break;
		case 4:
			btnCopyText = (Button)target;
			btnCopyText.Click += btnCopyText_Click;
			break;
		case 5:
			Bar = (Path)target;
			break;
		case 6:
			listBox = (ListBox)target;
			break;
		case 7:
			((CommandBinding)target).CanExecute += CopyCmdCanExecute;
			((CommandBinding)target).Executed += CopyCmdExecuted;
			break;
		case 8:
			VisualStateGroup = (VisualStateGroup)target;
			break;
		case 9:
			Default = (VisualState)target;
			break;
		case 10:
			Loading = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
