using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public partial class WindowDebugDesignInfo : Window, IComponentConnector
{
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
}
