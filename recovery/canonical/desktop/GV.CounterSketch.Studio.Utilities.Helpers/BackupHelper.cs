using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GV.CounterSketch.Common;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class BackupHelper
{
	private const string AppDataBackupFileName = "CSCommonAppDataContent.zip";

	private const string CsBackupPassword = "CounterSketch";

	private const string TempBackupDirectoryName = "CSBackup";

	private static readonly List<string> FileList = new List<string>();

	public static string BackupLogMessage;

	public static bool IsBackingUp;

	private static bool _executeRestoreAfterBackup;

	private static string _recoveryFileLocation;

	private static string _saveFileName;

	private static string _tempBackupRecoveryDirectory;

	public static string RestoreFilePath { get; set; }

	public static bool BrowseBackupDirectory()
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
		{
			Description = Strings.Select_Backup_Save_Folder
		};
		if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
		{
			return false;
		}
		if (folderBrowserDialog.SelectedPath.StartsWith("\\\\"))
		{
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineAlert, Strings.Backup_UNC_Path_Not_Allowed, messageIsModal: false);
			return false;
		}
		UIHelper.CounterSketchOptions.BackupDirectory = folderBrowserDialog.SelectedPath;
		UIHelper.CounterSketchOptions.WriteSettings();
		return true;
	}

	public static long CalculateCSBackupSize(string sourceDir)
	{
		FileList.Clear();
		FileInfo[] files = new DirectoryInfo(sourceDir).GetFiles("*.*", SearchOption.AllDirectories);
		DirSearch(sourceDir);
		return files.Where((FileInfo f) => FileList.Contains(f.FullName)).Sum((FileInfo f) => f.Length) / 1000000;
	}

	public static long CalculateDriveFreeSpace(string path)
	{
		if (path != null)
		{
			long result = 0L;
			if ((path.Length > 0 && Directory.Exists(path)) || File.Exists(path))
			{
				result = new DriveInfo(Directory.GetDirectoryRoot(path)).AvailableFreeSpace / 1000000;
			}
			return result;
		}
		return 0L;
	}

	public static void CleanBackupFiles(string backupDirectory)
	{
		if (Directory.Exists(backupDirectory))
		{
			Directory.Delete(backupDirectory, recursive: true);
		}
	}

	public static void CreateBackup()
	{
		if (IsBackingUp)
		{
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineAlert, Strings.ThereIsAlreadyABackupInProgress + ".", messageIsModal: false);
			return;
		}
		bool flag = true;
		if (UIHelper.CounterSketchOptions.BackupDirectory != null)
		{
			if (UIHelper.CounterSketchOptions.BackupDirectory.Length == 0 || !Directory.Exists(UIHelper.CounterSketchOptions.BackupDirectory))
			{
				flag = BrowseBackupDirectory();
			}
		}
		else
		{
			flag = BrowseBackupDirectory();
		}
		if (flag)
		{
			ViewModelLocator.Current.BackupRestoreVM.ClearBackupLog();
			ViewModelLocator.Current.BackupRestoreVM.BackupDirectory = UIHelper.CounterSketchOptions.BackupDirectory;
			ViewModelLocator.Current.BackupRestoreVM.UpdateBackupStatistics();
			long num = CalculateDriveFreeSpace(UIHelper.CounterSketchOptions.BackupDirectory);
			if (CalculateCSBackupSize(UIHelper.CSDataRootPath) * 2 > num)
			{
				string notEnoughFreeSpaceOnSelectedDriveForBackup = Strings.NotEnoughFreeSpaceOnSelectedDriveForBackup;
				GlobalErrorHandler.WriteInfoToEventLog(notEnoughFreeSpaceOnSelectedDriveForBackup, 2);
				UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineAlert, notEnoughFreeSpaceOnSelectedDriveForBackup, messageIsModal: false);
				return;
			}
			ViewModelLocator.Current.BackupRestoreVM.AddToBackupLog("Backup Started...");
			ViewModelLocator.Current.MainVM.BackupProgressBarVisibility = true;
			ViewModelLocator.Current.MainVM.BackupProgressBarValue = 10;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += CreateBackupArchive_DoWork;
			backgroundWorker.RunWorkerCompleted += CreateBackupArchive_RunWorkerCompleted;
			backgroundWorker.RunWorkerAsync();
		}
	}

	public static long DirSize(DirectoryInfo d)
	{
		long num = d.GetFiles().Sum((FileInfo fi) => fi.Length);
		DirectoryInfo[] directories = d.GetDirectories();
		return num + directories.Where((DirectoryInfo di) => di.Name != "Updates" && di.Name != "update" && di.Name != "Library").Sum((DirectoryInfo di) => DirSize(di));
	}

	public static void ResetBackupDirectory()
	{
		UIHelper.CounterSketchOptions.BackupDirectory = "";
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	public static void RestoreBackup()
	{
		if (IsBackingUp)
		{
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineAlert, "You cannot start a file restore while there is an active backup in progress.", messageIsModal: false);
			return;
		}
		if (string.IsNullOrEmpty(RestoreFilePath) || !RestoreFilePath.EndsWith(".csb") || !File.Exists(RestoreFilePath))
		{
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.SingleLineInfo, "Please choose a valid CounterSketch backup file (.CSB).", messageIsModal: true);
			return;
		}
		FileInfo fileInfo = new FileInfo(RestoreFilePath);
		if (fileInfo.Directory != null)
		{
			_tempBackupRecoveryDirectory = Path.Combine(fileInfo.Directory.FullName, "CSBackup");
			long num = CalculateDriveFreeSpace(RestoreFilePath);
			if (new FileInfo(RestoreFilePath).Length / 1000000 * 2 > num)
			{
				GlobalErrorHandler.WriteInfoToEventLog("Not enough free space on selected drive for restore process. Please free up space and try again.", 2);
				UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineAlert, "Not enough free space on selected drive for restore process. Please free up space and try again.", messageIsModal: false);
				return;
			}
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.AcceptPrompt, "It is recommended you make a backup as a precaution before starting the Restore process. Would you like to create a Backup now?", messageIsModal: true);
			if (UIHelper.LastMessageBoxAccepted == true)
			{
				CreateBackup();
				_executeRestoreAfterBackup = true;
			}
			else
			{
				ExecuteRestore(RestoreFilePath);
			}
		}
		else
		{
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.SingleLineInfo, "Please choose a valid CounterSketch backup file (.CSB).", messageIsModal: true);
		}
	}

	private static void CreateBackupArchive_DoWork(object sender, DoWorkEventArgs e)
	{
		UIHelper.AllowApplicationClose = false;
		IsBackingUp = true;
		_tempBackupRecoveryDirectory = Path.Combine(UIHelper.CounterSketchOptions.BackupDirectory, "CSBackup");
		Directory.CreateDirectory(_tempBackupRecoveryDirectory);
		ViewModelLocator.Current.BackupRestoreVM.AddToBackupLog("Backing up Application Data...");
		ViewModelLocator.Current.MainVM.BackupProgressBarValue = 60;
		CreateZip(UIHelper.CSDataRootPath, Path.Combine(_tempBackupRecoveryDirectory, "CSCommonAppDataContent.zip"), 0, passwordProtect: false);
		_saveFileName = Path.Combine(UIHelper.CounterSketchOptions.BackupDirectory, string.Format("CS_{1}_Backup_{0:MMddyyhhmmss}.csb", DateTime.Now, UIHelper.VersionString));
		CreateZip(_tempBackupRecoveryDirectory, _saveFileName, 0, passwordProtect: true);
	}

	private static void CreateBackupArchive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (sender is BackgroundWorker backgroundWorker)
		{
			backgroundWorker.DoWork -= CreateBackupArchive_DoWork;
		}
		if (e.Error != null)
		{
			CleanBackupFiles(_tempBackupRecoveryDirectory);
			GlobalErrorHandler.HandleException("BackupHelper", e.Error);
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineInfo, "An error occured while trying to access project files during the backup process. Please try the backup again.", messageIsModal: false);
			ViewModelLocator.Current.BackupRestoreVM.AddToBackupLog("Backup Aborted Due to Error.");
			IsBackingUp = false;
			return;
		}
		ViewModelLocator.Current.BackupRestoreVM.AddToBackupLog("Saving Backup File...");
		ViewModelLocator.Current.MainVM.BackupProgressBarValue = 70;
		ViewModelLocator.Current.BackupRestoreVM.AddToBackupLog("Cleaning up...");
		ViewModelLocator.Current.MainVM.BackupProgressBarValue = 85;
		CleanBackupFiles(_tempBackupRecoveryDirectory);
		ViewModelLocator.Current.BackupRestoreVM.AddToBackupLog("Backup Completed.");
		ViewModelLocator.Current.MainVM.BackupProgressBarValue = 100;
		ViewModelLocator.Current.MainVM.BackupProgressBarVisibility = false;
		if (!_executeRestoreAfterBackup)
		{
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineInfo, $"Backup created successfully to:\n{_saveFileName}", messageIsModal: false);
		}
		ViewModelLocator.Current.BackupRestoreVM.UpdateBackupStatistics();
		IsBackingUp = false;
		UIHelper.AllowApplicationClose = true;
		if (_executeRestoreAfterBackup)
		{
			_executeRestoreAfterBackup = false;
			ExecuteRestore(RestoreFilePath);
		}
	}

	private static void CreateZip(string sourceDirectory, string targetFile, int compressionLevel, bool passwordProtect)
	{
		ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(targetFile));
		if (compressionLevel < 0)
		{
			compressionLevel = 0;
		}
		if (compressionLevel > 9)
		{
			compressionLevel = 9;
		}
		zipOutputStream.SetLevel(compressionLevel);
		if (passwordProtect)
		{
			zipOutputStream.Password = "CounterSketch";
		}
		FileList.Clear();
		DirSearch(sourceDirectory);
		foreach (string file in FileList)
		{
			FileInfo fileInfo = new FileInfo(file);
			ZipEntry entry = new ZipEntry(file.Replace(sourceDirectory + "\\", ""))
			{
				DateTime = fileInfo.LastWriteTime
			};
			zipOutputStream.PutNextEntry(entry);
			byte[] buffer = new byte[4096];
			using (FileStream source = File.OpenRead(file))
			{
				StreamUtils.Copy(source, zipOutputStream, buffer);
			}
			zipOutputStream.CloseEntry();
		}
		zipOutputStream.IsStreamOwner = true;
		zipOutputStream.Close();
	}

	private static void DirSearch(string subDir)
	{
		string[] files = Directory.GetFiles(subDir);
		foreach (string text in files)
		{
			if (IsFileAllowed(text))
			{
				FileList.Add(text);
			}
		}
		files = Directory.GetDirectories(subDir);
		foreach (string text2 in files)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(text2);
			if (directoryInfo.Name.ToLower() != "updates" && directoryInfo.Name.ToLower() != "update")
			{
				DirSearch(text2);
			}
		}
	}

	private static void ExecuteRestore(string recoveryFile)
	{
		_recoveryFileLocation = recoveryFile;
		UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.AcceptPrompt, "WARNING: Restoring can potentially DELETE newer Portfolios and Designs that have not been backed up. Do you want to restore changes?", messageIsModal: true);
		if (UIHelper.LastMessageBoxAccepted == true)
		{
			UIHelper.ShowWaitWindow(waitVisible: true);
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += ExecuteRestore_DoWork;
			backgroundWorker.RunWorkerCompleted += ExecuteRestore_RunWorkerCompleted;
			backgroundWorker.RunWorkerAsync();
		}
	}

	private static void ExecuteRestore_DoWork(object sender, DoWorkEventArgs e)
	{
		Directory.CreateDirectory(_tempBackupRecoveryDirectory);
		FastZip fastZip = new FastZip();
		fastZip.Password = "CounterSketch";
		fastZip.ExtractZip(_recoveryFileLocation, _tempBackupRecoveryDirectory, ".*\\.zip$");
		FastZip fastZip2 = new FastZip();
		string zipFileName = Path.Combine(_tempBackupRecoveryDirectory, "CSCommonAppDataContent.zip");
		fastZip2.ExtractZip(zipFileName, UIHelper.CSDataRootPath, FastZip.Overwrite.Always, null, null, null, restoreDateTime: true);
		CleanBackupFiles(_tempBackupRecoveryDirectory);
	}

	private static void ExecuteRestore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (sender is BackgroundWorker backgroundWorker)
		{
			backgroundWorker.DoWork -= ExecuteRestore_DoWork;
		}
		UIHelper.ShowWaitWindow(waitVisible: false);
		if (e.Error != null)
		{
			GlobalErrorHandler.HandleException("BackupHelper", e.Error);
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineInfo, e.Error.Message, messageIsModal: false);
			ViewModelLocator.Current.BackupRestoreVM.AddToBackupLog("Backup Aborted Due to Error.");
			IsBackingUp = false;
		}
		else
		{
			UIHelper.CounterSketchOptions = CounterSketchOptions.ReadSettings(UIHelper.CSDataRootPath);
			Messenger.Default.Send<string, WindowModalOptions>("ClosePreferencesWindow");
			ViewModelLocator.Current.MainVM.ResetApplication();
			UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineInfo, "Restore completed successfully.", messageIsModal: false);
		}
	}

	private static bool IsFileAllowed(string filePath)
	{
		DirectoryInfo parent = Directory.GetParent(filePath);
		switch (Path.GetExtension(filePath))
		{
		case ".tmp":
		case ".csl":
		case ".exe":
		case ".s3db":
		case ".db":
		case ".backup":
		case ".csgl":
		case ".log":
		case ".zbin":
			return false;
		default:
			_ = parent.Name.ToLower() == "userparts";
			return true;
		}
	}
}
