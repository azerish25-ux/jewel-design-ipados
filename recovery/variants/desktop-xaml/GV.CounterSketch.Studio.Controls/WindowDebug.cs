using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public partial class WindowDebug : Window, INotifyPropertyChanged, IComponentConnector
{
	private static BackgroundWorker _backgroundWorker;

	private ObservableCollection<string> _logEntries;

	public ObservableCollection<string> LogEntries
	{
		get
		{
			return _logEntries;
		}
		set
		{
			_logEntries = value;
			RaisePropertyChanged("LogEntries");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public WindowDebug()
	{
		InitializeComponent();
		base.Loaded += WindowDebug_Loaded;
	}

	private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		e.Result = GlobalErrorHandler.GetLogEntries();
	}

	private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Result is List<string> list)
		{
			LogEntries = new ObservableCollection<string>();
			foreach (string item in list)
			{
				if (chkErrorsOnly.IsChecked == true)
				{
					if (item.ToLowerInvariant().Contains("error"))
					{
						LogEntries.Add(item);
					}
				}
				else
				{
					LogEntries.Add(item);
				}
			}
		}
		_backgroundWorker.DoWork -= _backgroundWorker_DoWork;
		_backgroundWorker.RunWorkerCompleted -= _backgroundWorker_RunWorkerCompleted;
		VisualStateManager.GoToElementState(mainDock, "Default", useTransitions: true);
		DispatcherHelper.WaitForPriority();
	}

	private void BtnCloseClick(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void btnCopyText_Click(object sender, RoutedEventArgs e)
	{
		CopySelectedItemsToClipboard();
	}

	private void btnRefresh_Click(object sender, RoutedEventArgs e)
	{
		RefreshEventList();
	}

	private void BtnReportClick(object sender, RoutedEventArgs e)
	{
		SendDebugReport(DebuggingHelper.GetUserComments());
	}

	private void chkErrorsOnly_Click(object sender, RoutedEventArgs e)
	{
		RefreshEventList();
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

	private void RaisePropertyChanged(string info)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(info));
		}
	}

	private void RefreshEventList()
	{
		VisualStateManager.GoToElementState(mainDock, "Loading", useTransitions: true);
		DispatcherHelper.WaitForPriority();
		_backgroundWorker = new BackgroundWorker();
		_backgroundWorker.DoWork += _backgroundWorker_DoWork;
		_backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
		_backgroundWorker.RunWorkerAsync();
	}

	private void SendDebugReport(string userComments)
	{
		DebuggingHelper.SendDebugReport(userComments);
	}

	private void WindowDebug_Loaded(object sender, RoutedEventArgs e)
	{
		base.Foreground = new SolidColorBrush(Colors.Black);
		listBox.Foreground = new SolidColorBrush(Colors.Black);
		chkErrorsOnly.Foreground = new SolidColorBrush(Colors.Black);
		RefreshEventList();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
