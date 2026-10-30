using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio.ViewModel;

public class BackupRestoreViewModel : ViewModelBase
{
	private string _backupDirectory;

	private string _backupLog;

	private string _backupStatistics;

	private string _recoveryFileName;

	private string _portfolioSavePath;

	public RelayCommand CommandResetRecoveryFileDirectory { get; private set; }

	public RelayCommand CommandResetPortfolioPathToDefault { get; private set; }

	public RelayCommand CommandResetBackupDirectory { get; private set; }

	public RelayCommand CommandRestoreBackup { get; private set; }

	public RelayCommand CommandBrowseRecoveryLocation { get; private set; }

	public RelayCommand CommandCreateBackup { get; private set; }

	public RelayCommand CommandBrowseBackupDirectory { get; private set; }

	public RelayCommand CommandSelectPortfolioSaveLocation { get; private set; }

	public RelayCommand CommandOpenDefaultPortfolioFolderLocation { get; private set; }

	public string BackupStatistics
	{
		get
		{
			return _backupStatistics;
		}
		set
		{
			_backupStatistics = value;
			RaisePropertyChanged(() => BackupStatistics);
		}
	}

	public string BackupLog
	{
		get
		{
			return _backupLog;
		}
		set
		{
			_backupLog = value;
			RaisePropertyChanged(() => BackupLog);
		}
	}

	public string RecoveryFileName
	{
		get
		{
			return _recoveryFileName;
		}
		set
		{
			_recoveryFileName = value;
			RaisePropertyChanged(() => RecoveryFileName);
		}
	}

	public string PortfolioSavePath
	{
		get
		{
			return UIHelper.CounterSketchOptions.UserDefinedPortfolioPath;
		}
		set
		{
			_portfolioSavePath = value;
			UIHelper.CSProjectPath = value;
			UIHelper.CounterSketchOptions.UserDefinedPortfolioPath = value;
			UIHelper.CounterSketchOptions.WriteSettings();
			RaisePropertyChanged("PortfolioSavePath");
		}
	}

	public string BackupDirectory
	{
		get
		{
			return _backupDirectory;
		}
		set
		{
			_backupDirectory = value;
			RaisePropertyChanged(() => BackupDirectory);
		}
	}

	public RelayCommand CommandOpenUserPortfolioFolderLocation { get; private set; }

	public BackupRestoreViewModel()
	{
		if (!base.IsInDesignMode)
		{
			InitializeCommands();
			UpdateBackupStatistics();
			BackupDirectory = UIHelper.CounterSketchOptions.BackupDirectory;
		}
	}

	public void AddToBackupLog(string text)
	{
		BackupLog = BackupLog + DateTime.Now.ToShortTimeString() + ": " + text + "\n";
	}

	public void ClearBackupLog()
	{
		BackupLog = string.Empty;
	}

	public void UpdateBackupStatistics()
	{
		long num = BackupHelper.CalculateDriveFreeSpace(UIHelper.CounterSketchOptions.BackupDirectory);
		long num2 = BackupHelper.CalculateCSBackupSize(UIHelper.CSDataRootPath);
		BackupStatistics = $"{Strings.Backup_Customer_Portfolios} {ProjectManagementHelper.CustomerProjectCollection.Count}\n{Strings.Backup_File_Size} {num2} MB\n{Strings.Backup_Total_Free_Space} {num} MB";
	}

	private void BrowseBackupDirectory()
	{
		if (BackupHelper.BrowseBackupDirectory())
		{
			BackupDirectory = UIHelper.CounterSketchOptions.BackupDirectory;
			UpdateBackupStatistics();
		}
	}

	private void CreateBackup()
	{
		BackupHelper.CreateBackup();
	}

	private void InitializeCommands()
	{
		CommandResetRecoveryFileDirectory = new RelayCommand(ResetRecoveryFileDirectory);
		CommandResetBackupDirectory = new RelayCommand(ResetBackupDirectory);
		CommandRestoreBackup = new RelayCommand(RestoreBackup);
		CommandBrowseRecoveryLocation = new RelayCommand(ResetRecoveryLocation);
		CommandCreateBackup = new RelayCommand(CreateBackup);
		CommandBrowseBackupDirectory = new RelayCommand(BrowseBackupDirectory);
		CommandSelectPortfolioSaveLocation = new RelayCommand(SelectPortfolioLocation);
		CommandOpenDefaultPortfolioFolderLocation = new RelayCommand(OpenDefaultPortfolioFolderLocation);
		CommandOpenUserPortfolioFolderLocation = new RelayCommand(OpenUserPortfolioFolderLocation);
		CommandResetPortfolioPathToDefault = new RelayCommand(ResetPortfolioPathToDefault);
	}

	private void OpenDefaultPortfolioFolderLocation()
	{
		if (Directory.Exists(UIHelper.DefaultCSProjectPath))
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = UIHelper.DefaultCSProjectPath,
				UseShellExecute = true,
				Verb = "open"
			});
		}
	}

	private void OpenUserPortfolioFolderLocation()
	{
		if (Directory.Exists(UIHelper.CounterSketchOptions.UserDefinedPortfolioPath))
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = UIHelper.CounterSketchOptions.UserDefinedPortfolioPath,
				UseShellExecute = true,
				Verb = "open"
			});
		}
	}

	private void ResetBackupDirectory()
	{
		BackupDirectory = string.Empty;
		BackupHelper.ResetBackupDirectory();
		UpdateBackupStatistics();
	}

	private void ResetPortfolioPathToDefault()
	{
		PortfolioSavePath = UIHelper.DefaultCSProjectPath;
	}

	private void ResetRecoveryFileDirectory()
	{
		RecoveryFileName = string.Empty;
	}

	private void ResetRecoveryLocation()
	{
		Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
		{
			Filter = "Compatible Files (*.csb)|*.csb",
			Multiselect = false,
			Title = Strings.Browse
		};
		if (openFileDialog.ShowDialog() == true && openFileDialog.FileName != null)
		{
			RecoveryFileName = openFileDialog.FileName;
		}
	}

	private void RestoreBackup()
	{
		BackupHelper.RestoreFilePath = RecoveryFileName;
		BackupHelper.RestoreBackup();
	}

	private void SelectPortfolioLocation()
	{
		using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		folderBrowserDialog.SelectedPath = _portfolioSavePath;
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
		{
			PortfolioSavePath = folderBrowserDialog.SelectedPath;
		}
	}
}
