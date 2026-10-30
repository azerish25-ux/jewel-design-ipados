using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
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
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => BackupStatistics));
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
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => BackupLog));
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
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => RecoveryFileName));
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
			((ObservableObject)this).RaisePropertyChanged("PortfolioSavePath");
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
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => BackupDirectory));
		}
	}

	public RelayCommand CommandOpenUserPortfolioFolderLocation { get; private set; }

	public BackupRestoreViewModel()
	{
		if (!((ViewModelBase)this).IsInDesignMode)
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
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		CommandResetRecoveryFileDirectory = new RelayCommand((Action)ResetRecoveryFileDirectory, false);
		CommandResetBackupDirectory = new RelayCommand((Action)ResetBackupDirectory, false);
		CommandRestoreBackup = new RelayCommand((Action)RestoreBackup, false);
		CommandBrowseRecoveryLocation = new RelayCommand((Action)ResetRecoveryLocation, false);
		CommandCreateBackup = new RelayCommand((Action)CreateBackup, false);
		CommandBrowseBackupDirectory = new RelayCommand((Action)BrowseBackupDirectory, false);
		CommandSelectPortfolioSaveLocation = new RelayCommand((Action)SelectPortfolioLocation, false);
		CommandOpenDefaultPortfolioFolderLocation = new RelayCommand((Action)OpenDefaultPortfolioFolderLocation, false);
		CommandOpenUserPortfolioFolderLocation = new RelayCommand((Action)OpenUserPortfolioFolderLocation, false);
		CommandResetPortfolioPathToDefault = new RelayCommand((Action)ResetPortfolioPathToDefault, false);
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
