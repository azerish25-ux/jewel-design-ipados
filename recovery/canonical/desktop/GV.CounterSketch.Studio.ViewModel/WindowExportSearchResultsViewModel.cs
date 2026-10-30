using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
			RaisePropertyChanged(() => Count);
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
			RaisePropertyChanged(() => CurrentImageSource);
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
			RaisePropertyChanged(() => Designs);
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
			RaisePropertyChanged(() => ExportSearchResultsPath);
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
			RaisePropertyChanged(() => IsExporting);
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
			RaisePropertyChanged(() => IsComplete);
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
			RaisePropertyChanged(() => ProgressIndex);
		}
	}

	public WindowExportSearchResultsViewModel()
	{
		if (!base.IsInDesignMode)
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
		RaisePropertyChanged("IsViewEnabled");
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
		RaisePropertyChanged("IsViewEnabled");
	}

	private void InitializeCommands()
	{
		CommandExport = new RelayCommand(Export);
		CommandPickFolder = new RelayCommand(PickFolder);
		CommandView = new RelayCommand(View);
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
