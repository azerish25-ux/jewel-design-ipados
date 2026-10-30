using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GV.CounterSketch.Studio.ViewModel;

public class UpdateInfoViewModel : ViewModelBase
{
	private string _applicationUpdateName;

	private int _applicationUpdatePercentage;

	private bool _isApplicationUpdateInProgress;

	private bool _isApplicationUpdateReady;

	private bool _isDownloadCompleted;

	private bool _isLibraryUpdateInProgress;

	private bool _isLibraryUpdateReady;

	private string _libraryUpdateName;

	private int _libraryUpdatePercentage;

	private BitmapSource _imageTopLeft;

	private BitmapSource _imageTopRight;

	private BitmapSource _imageBottomLeft;

	private BitmapSource _imageBottomRight;

	private bool _hasNewParts;

	private bool _hasNewDesigns;

	private static DispatcherTimer _imageSwitchTimer;

	private static readonly Random _random = new Random();

	private static int _currentImage = 1;

	public bool IsUpdateInProgress
	{
		get
		{
			if (!IsLibraryUpdateInProgress)
			{
				return IsApplicationUpdateInProgress;
			}
			return true;
		}
	}

	public string ApplicationUpdateName
	{
		get
		{
			return _applicationUpdateName;
		}
		set
		{
			_applicationUpdateName = value;
			RaisePropertyChanged("ApplicationUpdateName");
		}
	}

	public int ApplicationUpdatePercentage
	{
		get
		{
			return _applicationUpdatePercentage;
		}
		set
		{
			_applicationUpdatePercentage = value;
			RaisePropertyChanged("ApplicationUpdatePercentage");
		}
	}

	public RelayCommand CommandApplyUpdates { get; private set; }

	public RelayCommand CommandDelayUpdates { get; private set; }

	public bool IsApplicationUpdateInProgress
	{
		get
		{
			return _isApplicationUpdateInProgress;
		}
		set
		{
			if (value != _isApplicationUpdateInProgress)
			{
				_isApplicationUpdateInProgress = value;
				RaisePropertyChanged("IsApplicationUpdateInProgress");
				RaisePropertyChanged(() => IsUpdateInProgress);
				RaisePropertyChanged(() => IsApplicationUpdateReady);
			}
		}
	}

	public bool IsApplicationUpdateReady
	{
		get
		{
			return _isApplicationUpdateReady;
		}
		set
		{
			_isApplicationUpdateReady = value;
			RaisePropertyChanged("IsApplicationUpdateReady");
		}
	}

	public bool IsDownloadCompleted
	{
		get
		{
			return _isDownloadCompleted;
		}
		set
		{
			_isDownloadCompleted = value;
			RaisePropertyChanged("IsDownloadCompleted");
		}
	}

	public bool IsLibraryUpdateInProgress
	{
		get
		{
			return _isLibraryUpdateInProgress;
		}
		set
		{
			_isLibraryUpdateInProgress = value;
			RaisePropertyChanged("IsLibraryUpdateInProgress");
			RaisePropertyChanged(() => IsUpdateInProgress);
		}
	}

	public bool IsLibraryUpdateReady
	{
		get
		{
			return _isLibraryUpdateReady;
		}
		set
		{
			_isLibraryUpdateReady = value;
			RaisePropertyChanged("IsLibraryUpdateReady");
		}
	}

	public string LibraryUpdateName
	{
		get
		{
			return _libraryUpdateName;
		}
		set
		{
			_libraryUpdateName = value;
			RaisePropertyChanged("LibraryUpdateName");
		}
	}

	public int LibraryUpdatePercentage
	{
		get
		{
			return _libraryUpdatePercentage;
		}
		set
		{
			_libraryUpdatePercentage = value;
			RaisePropertyChanged("LibraryUpdatePercentage");
		}
	}

	public RelayCommand CommandShowNewDesigns { get; set; } = new RelayCommand(ShowNewDesigns);

	public RelayCommand CommandShowNewParts { get; set; } = new RelayCommand(ShowNewParts);

	public bool HasNewDesigns
	{
		get
		{
			return _hasNewDesigns;
		}
		set
		{
			_hasNewDesigns = value;
			RaisePropertyChanged("HasNewDesigns");
		}
	}

	public bool HasNewParts
	{
		get
		{
			return _hasNewParts;
		}
		set
		{
			_hasNewParts = value;
			RaisePropertyChanged("HasNewParts");
		}
	}

	public BitmapSource ImageTopLeft
	{
		get
		{
			return _imageTopLeft;
		}
		set
		{
			_imageTopLeft = value;
			RaisePropertyChanged("ImageTopLeft");
		}
	}

	public BitmapSource ImageTopRight
	{
		get
		{
			return _imageTopRight;
		}
		set
		{
			_imageTopRight = value;
			RaisePropertyChanged("ImageTopRight");
		}
	}

	public BitmapSource ImageBottomLeft
	{
		get
		{
			return _imageBottomLeft;
		}
		set
		{
			_imageBottomLeft = value;
			RaisePropertyChanged("ImageBottomLeft");
		}
	}

	public BitmapSource ImageBottomRight
	{
		get
		{
			return _imageBottomRight;
		}
		set
		{
			_imageBottomRight = value;
			RaisePropertyChanged("ImageBottomRight");
		}
	}

	public UpdateInfoViewModel()
	{
		if (base.IsInDesignMode)
		{
			SetApplicationUpdateInProgress("4.9.999.9999", isUpdating: true);
			SetLibraryUpdateInProgress(isUpdating: true);
			ApplicationUpdatePercentage = 75;
			LibraryUpdatePercentage = 35;
			LibraryUpdateName = "183";
			IsLibraryUpdateReady = true;
			IsApplicationUpdateReady = true;
		}
		else
		{
			InitializeCommands();
		}
	}

	public void ApplyApplicationUpdate()
	{
		if (BackupHelper.IsBackingUp)
		{
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineAlert, "There is a Backup currently active. You cannot update CounterSketch at this time.", messageIsModal: false);
		}
		else if (ProjectManagementHelper.ContinueAfterSaveDesignPrompt() && UIHelper.AllowApplicationClose)
		{
			UIHelper.RaiseApplicationMessageEvent("StartUpdate", ApplicationMessageType.SingleLineAlert, "Your application will be updated after you click OK.  You will need to restart CounterSketch after the update window disappears.", messageIsModal: true);
			string tempDirectory = FileFolderHelper.GetTempDirectory();
			string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CounterSketchUpdateInstaller");
			GlobalErrorHandler.WriteInfoToEventLog($"Copying Installer to: {tempDirectory}, from: {text}");
			FileFolderHelper.CopyDirectory(text, tempDirectory);
			GlobalErrorHandler.WriteInfoToEventLog("Installer Copied");
			string text2 = Path.Combine(tempDirectory, "CounterSketchUpdateInstaller.exe");
			GlobalErrorHandler.WriteInfoToEventLog($"Starting Installer From: {text2}");
			Process.Start(text2);
			if (Application.Current != null)
			{
				Application.Current.Shutdown();
			}
		}
	}

	public void ApplyUpdates()
	{
		bool isApplicationUpdateReady = IsApplicationUpdateReady;
		bool isLibraryUpdateReady = IsLibraryUpdateReady;
		ResetUpdateFlags();
		if (isLibraryUpdateReady)
		{
			ApplyLibraryUpdate();
		}
		if (isApplicationUpdateReady)
		{
			ApplyApplicationUpdate();
		}
	}

	public void SetApplicationUpdateComplete()
	{
		if (string.IsNullOrWhiteSpace(ApplicationUpdateName))
		{
			ApplicationUpdateName = UIHelper.CounterSketchOptions.ApplicationLatestVersion;
		}
		IsApplicationUpdateInProgress = false;
		IsApplicationUpdateReady = true;
		if (!(IsLibraryUpdateInProgress | IsApplicationUpdateInProgress))
		{
			IsDownloadCompleted = true;
		}
	}

	public void SetApplicationUpdateInProgress(string updateName, bool isUpdating)
	{
		IsApplicationUpdateInProgress = isUpdating;
		ApplicationUpdateName = updateName;
	}

	public void SetApplicationUpdatePercentage(int percentage)
	{
		ApplicationUpdatePercentage = percentage;
	}

	public void SetLibraryUpdateComplete()
	{
		IsLibraryUpdateInProgress = false;
		IsLibraryUpdateReady = true;
		if (!(IsLibraryUpdateInProgress | IsApplicationUpdateInProgress))
		{
			IsDownloadCompleted = true;
		}
	}

	public void SetLibraryUpdateInProgress(bool isUpdating)
	{
		IsLibraryUpdateInProgress = isUpdating;
	}

	public void SetLibraryUpdatePercentage(int percentage)
	{
		LibraryUpdatePercentage = percentage;
	}

	private void ApplyLibraryUpdate()
	{
		UIHelper.ShowWaitWindow(waitVisible: true);
		CSLibraryHelper.LibraryUpdaterNew(runModelImportCheck: false);
		CSLibraryHelper.LibraryDatabaseUpdateNew();
		GlobalErrorHandler.WriteInfoToEventLog("Design Library has been updated to version " + LibraryUpdateName + ".");
		UIHelper.ShowWaitWindow(waitVisible: false);
		ShowNewLibraryInfo();
	}

	private void DelayUpdates()
	{
		ResetUpdateFlags();
		UIHelper.CounterSketchOptions.UpdateDelayedPreviously = true;
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void InitializeCommands()
	{
		CommandApplyUpdates = new RelayCommand(ApplyUpdates);
		CommandDelayUpdates = new RelayCommand(DelayUpdates);
	}

	private void ResetUpdateFlags()
	{
		IsApplicationUpdateInProgress = false;
		IsLibraryUpdateInProgress = false;
		IsDownloadCompleted = false;
		IsLibraryUpdateReady = false;
		IsApplicationUpdateReady = false;
	}

	private void ShowNewLibraryInfo()
	{
		GenerateNewLibraryInfo();
		WindowNewLibrary windowNewLibrary = new WindowNewLibrary();
		windowNewLibrary.Owner = Application.Current.MainWindow;
		windowNewLibrary.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		windowNewLibrary.Show();
		windowNewLibrary.Topmost = true;
	}

	private static void ShowNewParts()
	{
		ViewModelLocator.Current.MainVM.SetSectionTransitionTimeToInstant();
		ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize);
		ViewModelLocator.Current.MainVM.SetSectionTransitionTimeToInstant();
		ViewModelLocator.Current.TileViewPartsVM.SetSectionTransitionTimeToInstant();
		ViewModelLocator.Current.CustomizePartsListVM.AddPart();
	}

	private static void ShowNewDesigns()
	{
		ViewModelLocator.Current.MainVM.SetSectionTransitionTimeToInstant();
		ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Tiles);
	}

	public void StopNewImageTimer()
	{
		_imageSwitchTimer?.Stop();
	}

	private void GenerateNewLibraryInfo()
	{
		List<DesignSearch> newDesigns = SearchHelper.GetNewDesigns(includeVariants: true);
		List<DesignSearch> newDesigns2 = SearchHelper.GetNewDesigns(includeVariants: true, isFreehand: true);
		List<DesignSearch> newItems = newDesigns2.Union(newDesigns).ToList();
		HasNewDesigns = newDesigns.Any();
		HasNewParts = newDesigns2.Any();
		DesignSearch designSearch = newItems[_random.Next(newItems.Count)];
		DesignSearch designSearch2 = newItems[_random.Next(newItems.Count)];
		DesignSearch designSearch3 = newItems[_random.Next(newItems.Count)];
		DesignSearch designSearch4 = newItems[_random.Next(newItems.Count)];
		designSearch.UpdateImageSource();
		designSearch2.UpdateImageSource();
		designSearch3.UpdateImageSource();
		designSearch4.UpdateImageSource();
		ImageTopLeft = designSearch.DesignImageSource;
		ImageBottomRight = designSearch2.DesignImageSource;
		ImageTopRight = designSearch3.DesignImageSource;
		ImageBottomLeft = designSearch4.DesignImageSource;
		if (_imageSwitchTimer != null)
		{
			_imageSwitchTimer.Stop();
			_imageSwitchTimer = null;
		}
		_imageSwitchTimer = new DispatcherTimer();
		_imageSwitchTimer.Interval = TimeSpan.FromSeconds(1.0);
		_imageSwitchTimer.Tick += delegate
		{
			DesignSearch designSearch5 = newItems[_random.Next(newItems.Count)];
			designSearch5.UpdateImageSource();
			switch (_currentImage)
			{
			case 1:
				ImageTopLeft = designSearch5.DesignImageSource;
				break;
			case 2:
				ImageBottomRight = designSearch5.DesignImageSource;
				break;
			case 3:
				ImageTopRight = designSearch5.DesignImageSource;
				break;
			case 4:
				ImageBottomLeft = designSearch5.DesignImageSource;
				break;
			}
			_currentImage++;
			if (_currentImage > 4)
			{
				_currentImage = 1;
			}
		};
		_imageSwitchTimer.Start();
	}
}
