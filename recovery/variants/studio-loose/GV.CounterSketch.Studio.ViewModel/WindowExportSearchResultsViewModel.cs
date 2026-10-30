using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class WindowExportSearchResultsViewModel : ViewModelBase
{
	private int _count;

	private ImageSource _currentImageSource;

	private IEnumerable<DesignSearch> _designs;

	private string _exportSearchResultsPath;

	private bool _isComplete;

	private bool _isExporting;

	private int _progressIndex;

	public bool IsViewEnabled
	{
		get
		{
			if (Directory.Exists(ExportSearchResultsPath) && Directory.GetFiles(ExportSearchResultsPath).Any())
			{
				return true;
			}
			return false;
		}
	}

	public RelayCommand CommandView { get; set; }

	public RelayCommand CommandExport { get; set; }

	public RelayCommand CommandPickFolder { get; set; }

	public int Count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
			((ObservableObject)this).RaisePropertyChanged<int>((Expression<Func<int>>)(() => Count));
		}
	}

	public ImageSource CurrentImageSource
	{
		get
		{
			return _currentImageSource;
		}
		set
		{
			_currentImageSource = value;
			((ObservableObject)this).RaisePropertyChanged<ImageSource>((Expression<Func<ImageSource>>)(() => CurrentImageSource));
		}
	}

	public IEnumerable<DesignSearch> Designs
	{
		get
		{
			return _designs;
		}
		set
		{
			_designs = value;
			((ObservableObject)this).RaisePropertyChanged<IEnumerable<DesignSearch>>((Expression<Func<IEnumerable<DesignSearch>>>)(() => Designs));
		}
	}

	public string ExportSearchResultsPath
	{
		get
		{
			return _exportSearchResultsPath;
		}
		set
		{
			_exportSearchResultsPath = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => ExportSearchResultsPath));
			UIHelper.CounterSketchOptions.ExportSearchResultsPath = value;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
	}

	public bool IsExporting
	{
		get
		{
			return _isExporting;
		}
		set
		{
			_isExporting = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsExporting));
		}
	}

	public bool IsComplete
	{
		get
		{
			return _isComplete;
		}
		set
		{
			_isComplete = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsComplete));
		}
	}

	public int ProgressIndex
	{
		get
		{
			return _progressIndex;
		}
		set
		{
			_progressIndex = value;
			((ObservableObject)this).RaisePropertyChanged<int>((Expression<Func<int>>)(() => ProgressIndex));
		}
	}

	public WindowExportSearchResultsViewModel()
	{
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitializeCommands();
		}
	}

	public void Populate(IEnumerable<DesignSearch> designs)
	{
		_exportSearchResultsPath = UIHelper.CounterSketchOptions.ExportSearchResultsPath;
		Designs = designs;
		if (Designs != null)
		{
			Count = Designs.Count();
			CurrentImageSource = Designs.ToList().First().DesignImageSource;
		}
		ProgressIndex = 0;
		((ObservableObject)this).RaisePropertyChanged("IsViewEnabled");
	}

	private void Export()
	{
		if (string.IsNullOrWhiteSpace(ExportSearchResultsPath))
		{
			System.Windows.MessageBox.Show(Strings.Please_specify_the_folder_to_Export);
			return;
		}
		ProgressIndex = 0;
		IsExporting = true;
		IsComplete = false;
		Designs.ForEach(delegate(DesignSearch design)
		{
			ProgressIndex++;
			CurrentImageSource = design.DesignImageSource;
			design.ExportImage(ExportSearchResultsPath);
			DispatcherHelper.WaitForPriority();
		});
		IsExporting = false;
		IsComplete = true;
		((ObservableObject)this).RaisePropertyChanged("IsViewEnabled");
	}

	private void InitializeCommands()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		CommandExport = new RelayCommand((Action)Export, false);
		CommandPickFolder = new RelayCommand((Action)PickFolder, false);
		CommandView = new RelayCommand((Action)View, false);
	}

	private void PickFolder()
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		folderBrowserDialog.ShowDialog();
		ExportSearchResultsPath = folderBrowserDialog.SelectedPath;
	}

	private void View()
	{
		if (string.IsNullOrWhiteSpace(ExportSearchResultsPath))
		{
			System.Windows.MessageBox.Show(Strings.Please_specify_the_folder_to_Export);
		}
		else if (!Directory.Exists(ExportSearchResultsPath))
		{
			System.Windows.MessageBox.Show(Strings.Nothing_to_View_Export_First);
		}
		else
		{
			Process.Start(ExportSearchResultsPath);
		}
	}
}
