using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GV.CounterSketch.Studio.ViewModel;
using SharpBits.Base;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class BitsHelper
{
	public const string _csUpdateNamePrefix = "CounterSketchInstaller_";

	private const string _applicationJobName = "DownloadCounterSketchStudioUpdate";

	private const string _csUpdateNameSuffix = ".exe";

	private const string _libraryJobName = "DownloadCounterSketchStudioLibraryUpdate";

	private const string _password = "REDACTED_EMBEDDED_CREDENTIAL";

	private static string _remoteAddressApplicationPrefix = "https://gemvision.s3.amazonaws.com/software/countersketch/application/";

	private static string _remoteAddressLibraryPrefix = "https://gemvision.s3.amazonaws.com/software/countersketch/library/";

	private static string _remoteAddressRhinoPrefix = "https://gemvision.s3.amazonaws.com/software/countersketch/supporting/rhino/";

	private const string _username = "GEMVISION\\csupdates";

	private static BitsManager _manager;

	private static int _numberOfLibraryFilesDownloadedSoFar;

	public static BitsManager Manager
	{
		get
		{
			if (_manager == null)
			{
				_manager = new BitsManager();
			}
			return _manager;
		}
	}

	public static List<BitsJob> LibraryBitsJobs { get; set; } = new List<BitsJob>();

	public static Dictionary<string, int> LibraryFilesDownloading { get; set; } = new Dictionary<string, int>();

	public static int TotalNumberOfLibraryFilesToDownload { get; set; }

	public static Tuple<int, int, int, int> LibraryUpdateStatus { get; set; }

	public static void UpdateServerLocationForDownloads(bool isUsingLegacyServers)
	{
		if (isUsingLegacyServers)
		{
			_remoteAddressApplicationPrefix = "https://countersketch.biz/applicationupdates/countersketch/";
			_remoteAddressLibraryPrefix = "https://countersketch.biz/applicationupdates/cslibrary/Library/";
			_remoteAddressRhinoPrefix = "https://countersketch.biz/applicationupdates/rhino/";
		}
		else
		{
			_remoteAddressApplicationPrefix = "https://gemvision.s3.amazonaws.com/software/countersketch/application/";
			_remoteAddressLibraryPrefix = "https://gemvision.s3.amazonaws.com/software/countersketch/library/";
			_remoteAddressRhinoPrefix = "https://gemvision.s3.amazonaws.com/software/countersketch/supporting/rhino/";
		}
	}

	public static bool AnyLibraryUpdateDownloading()
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate") && item.State == JobState.Transferring)
			{
				return true;
			}
		}
		return false;
	}

	public static void CancelApplicationUpdate()
	{
		if (JobExists("DownloadCounterSketchStudioUpdate"))
		{
			CancelJob("DownloadCounterSketchStudioUpdate");
		}
	}

	public static void CancelJob(string jobName)
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in _manager.Jobs.Values.ToList())
		{
			if (item.DisplayName == jobName)
			{
				item.Cancel();
			}
		}
	}

	public static void CancelLibraryJobs()
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in _manager.Jobs.Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate"))
			{
				item.Cancel();
			}
		}
	}

	public static void Cleanup()
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		Manager.OnJobModified -= Manager_OnJobModified;
		Manager.OnJobAdded -= Manager_OnJobAdded;
		Manager.OnJobTransferred -= Manager_OnJobTransferred;
		Manager.OnJobError -= Manager_OnJobError;
		Manager.Dispose();
	}

	public static void CompleteTransferredApplicationJobs()
	{
		CompleteTransferredJobs("DownloadCounterSketchStudioUpdate");
	}

	public static void CompleteTransferredJobs(string jobName)
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in _manager.Jobs.Values.ToList())
		{
			if (item.DisplayName == jobName && item.State == JobState.Transferred)
			{
				try
				{
					item.Complete();
				}
				catch (FileNotFoundException ex)
				{
					GlobalErrorHandler.WriteErrorToEventLog("Job unable to Complete.\nFile Not Found:" + ex.FileName);
					item.Cancel();
				}
			}
		}
	}

	public static void CompleteTransferredLibraryJobs()
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate") && item.State == JobState.Transferred)
			{
				try
				{
					item.Complete();
				}
				catch (FileNotFoundException ex)
				{
					GlobalErrorHandler.WriteErrorToEventLog("Job unable to Complete.\nFile Not Found:" + ex.FileName);
					item.Cancel();
				}
			}
		}
	}

	public static void CreateApplicationUpdateJob(string versionToDownload, bool includeRhinoUpdate, string versionRhinoToDownload)
	{
		if (!JobExists("DownloadCounterSketchStudioUpdate") || !IsExistingJobValid("DownloadCounterSketchStudioUpdate"))
		{
			string text = "Rhino_" + versionRhinoToDownload + ".zip";
			string text2 = "CounterSketchInstaller_" + versionToDownload + ".exe";
			BitsJob bitsJob = Manager.CreateJob("DownloadCounterSketchStudioUpdate", JobType.Download);
			bitsJob.Priority = JobPriority.High;
			bitsJob.NotificationFlags = NotificationFlags.JobTransferred | NotificationFlags.JobErrorOccured | NotificationFlags.JobModified;
			bitsJob.Description = versionToDownload;
			string text3 = Path.Combine(UIHelper.CSUpdatesPath, "CounterSketchInstaller_" + versionToDownload + ".exe");
			UIHelper.CounterSketchOptions.ApplicationMsiUpdatePath = text3;
			if (UIHelper.KeyProfile.KeyProfileId == 2)
			{
				UIHelper.CounterSketchOptions.ApplicationMsiUpdateSwitches = " /product=\"Romantic Creation\"";
			}
			else
			{
				UIHelper.CounterSketchOptions.ApplicationMsiUpdateSwitches = null;
			}
			UIHelper.CounterSketchOptions.WriteSettings();
			bitsJob.AddFile(_remoteAddressApplicationPrefix + text2, text3);
			if (includeRhinoUpdate)
			{
				string text4 = Path.Combine(UIHelper.CSUpdatesPath, text);
				bitsJob.AddFile(_remoteAddressRhinoPrefix + text, text4);
				UIHelper.CounterSketchOptions.ApplicationRhinoUpdatePath = text4;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			else
			{
				UIHelper.CounterSketchOptions.ApplicationRhinoUpdatePath = null;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			bitsJob.ProxySettings.ProxyUsage = ProxyUsage.AutoDetect;
			BitsCredentials credentials = new BitsCredentials
			{
				UserName = "GEMVISION\\csupdates",
				Password = "REDACTED_EMBEDDED_CREDENTIAL",
				AuthenticationScheme = AuthenticationScheme.Ntlm,
				AuthenticationTarget = AuthenticationTarget.Server
			};
			bitsJob.AddCredentials(credentials);
			bitsJob.Resume();
		}
	}

	public static void CreateLibraryUpdateJob(string[] libraryFilesToDownload, string description)
	{
		string[] array = libraryFilesToDownload.ToList().Distinct().ToArray();
		int num = 0;
		LibraryFilesDownloading = new Dictionary<string, int>();
		LibraryBitsJobs = new List<BitsJob>();
		while (LibraryFilesDownloading.Count < array.Length)
		{
			num++;
			BitsJob bitsJob = Manager.CreateJob("DownloadCounterSketchStudioLibraryUpdate_" + num, JobType.Download);
			bitsJob.Description = description;
			bitsJob.Priority = JobPriority.High;
			bitsJob.NotificationFlags = NotificationFlags.JobTransferred | NotificationFlags.JobErrorOccured | NotificationFlags.JobModified;
			BitsCredentials credentials = new BitsCredentials
			{
				UserName = "GEMVISION\\csupdates",
				Password = "REDACTED_EMBEDDED_CREDENTIAL",
				AuthenticationScheme = AuthenticationScheme.Ntlm,
				AuthenticationTarget = AuthenticationTarget.Server
			};
			bitsJob.AddCredentials(credentials);
			bitsJob.ProxySettings.ProxyUsage = ProxyUsage.AutoDetect;
			int num2 = 1;
			for (int i = GetStartingFileIndex(num); i < array.Count(); i++)
			{
				if (num2 > 200)
				{
					break;
				}
				string text = Path.Combine(UIHelper.CSUpdatesPath, array[i]);
				bitsJob.AddFile(_remoteAddressLibraryPrefix + array[i], text);
				LibraryFilesDownloading.Add(text, num);
				num2++;
			}
			LibraryBitsJobs.Add(bitsJob);
		}
		TotalNumberOfLibraryFilesToDownload = LibraryFilesDownloading.Count();
		_numberOfLibraryFilesDownloadedSoFar = 0;
		LibraryBitsJobs.ForEach(delegate(BitsJob _)
		{
			_.Resume();
		});
	}

	public static double CurrentProgressPercentage(string jobName)
	{
		try
		{
			Manager.EnumJobs(JobOwner.CurrentUser);
			foreach (BitsJob item in Manager.Jobs.Values.ToList())
			{
				if (item.DisplayName == jobName && item.State != JobState.Transferred)
				{
					ulong bytesTotal = item.Progress.BytesTotal;
					return (double)item.Progress.BytesTransferred / (double)bytesTotal * 100.0;
				}
			}
			return 100.0;
		}
		catch
		{
			return 100.0;
		}
	}

	public static JobState CurrentStateOfUpdate(string jobName)
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (item.DisplayName == jobName)
			{
				return item.State;
			}
		}
		return JobState.Error;
	}

	public static int GetNumberOfLibraryFilesDownloading()
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		int num = 0;
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate"))
			{
				item.EnumFiles();
				if (item.Files != null)
				{
					num += item.Files.Count;
				}
			}
		}
		return num;
	}

	public static void Initialize()
	{
		UpdateServerLocationForDownloads(UIHelper.CounterSketchOptions.UseStullerServerForUpdates);
		Manager.EnumJobs(JobOwner.CurrentUser);
		Manager.OnJobModified += Manager_OnJobModified;
		Manager.OnJobAdded += Manager_OnJobAdded;
		Manager.OnJobTransferred += Manager_OnJobTransferred;
		Manager.OnJobError += Manager_OnJobError;
		GetLibraryFilesDownloading();
	}

	public static void ResumeJob(string jobName)
	{
		StartUpdate(jobName);
	}

	public static bool TestBitsFunctionality()
	{
		try
		{
			new BitsManager();
			return true;
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("BITS Service not available.\n" + ex.Message);
			return false;
		}
	}

	private static string GetJobDescription(string jobName)
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (item.DisplayName == jobName)
			{
				return item.Description;
			}
		}
		return string.Empty;
	}

	private static void GetLibraryFilesDownloading()
	{
		LibraryFilesDownloading = new Dictionary<string, int>();
		Manager.EnumJobs(JobOwner.CurrentUser);
		_numberOfLibraryFilesDownloadedSoFar = 0;
		TotalNumberOfLibraryFilesToDownload = 0;
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (!item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate") || item.State == JobState.Transferred || item.State == JobState.Error || item.State == JobState.TransientError)
			{
				continue;
			}
			item.EnumFiles();
			TotalNumberOfLibraryFilesToDownload += (int)item.Progress.FilesTotal;
			_numberOfLibraryFilesDownloadedSoFar += (int)item.Progress.FilesTransferred;
			foreach (BitsFile file in item.Files)
			{
				if (!LibraryFilesDownloading.ContainsKey(file.LocalName))
				{
					LibraryFilesDownloading.Add(file.LocalName, 99);
				}
			}
		}
	}

	private static int GetStartingFileIndex(int currentBatchId)
	{
		if (currentBatchId == 1)
		{
			return 0;
		}
		return 200 * (currentBatchId - 1);
	}

	private static bool IsExistingJobValid(string jobName)
	{
		switch (CurrentStateOfUpdate(jobName))
		{
		case JobState.Acknowledged:
			return true;
		case JobState.Transferring:
			return true;
		case JobState.Connecting:
			return true;
		case JobState.Transferred:
			CompleteTransferredJobs(jobName);
			return true;
		case JobState.Queued:
			StartUpdate(jobName);
			return true;
		case JobState.Suspended:
			CancelJob(jobName);
			break;
		case JobState.Error:
			CancelJob(jobName);
			break;
		case JobState.TransientError:
			CancelJob(jobName);
			break;
		default:
			CancelJob(jobName);
			break;
		}
		return false;
	}

	private static bool JobExists(string jobName)
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		return Manager.Jobs.Values.ToList().Any((BitsJob job) => job.DisplayName == jobName);
	}

	private static void Manager_OnJobAdded(object sender, NotificationEventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Job Added: " + e.Job.DisplayName);
	}

	private static void Manager_OnJobError(object sender, ErrorNotificationEventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Job Errored: " + e.Job.DisplayName);
	}

	private static void Manager_OnJobModified(object sender, NotificationEventArgs e)
	{
		string displayName = e.Job.DisplayName;
		if (displayName != null && displayName == "DownloadCounterSketchStudioUpdate")
		{
			switch (e.Job.State)
			{
			case JobState.Transferring:
				ViewModelLocator.Current.UpdateInfoVM.SetApplicationUpdateInProgress(GetJobDescription("DownloadCounterSketchStudioUpdate") ?? "", isUpdating: true);
				ViewModelLocator.Current.UpdateInfoVM.SetApplicationUpdatePercentage((int)CurrentProgressPercentage("DownloadCounterSketchStudioUpdate"));
				break;
			case JobState.Suspended:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case JobState.Error:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case JobState.TransientError:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case JobState.Transferred:
				CompleteTransferredJobs("DownloadCounterSketchStudioUpdate");
				ViewModelLocator.Current.UpdateInfoVM.SetApplicationUpdateComplete();
				break;
			case JobState.Canceled:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case JobState.Queued:
			case JobState.Connecting:
			case JobState.Acknowledged:
				break;
			}
		}
		else
		{
			if (!e.Job.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate"))
			{
				return;
			}
			switch (e.Job.State)
			{
			case JobState.Transferring:
				if (LibraryUpdateStatus != null)
				{
					UpdateCurrentLibraryDownloadInfo();
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdateInProgress(isUpdating: true);
					ViewModelLocator.Current.UpdateInfoVM.LibraryUpdateName = string.Format("{2}, Designs {0} of {1}", _numberOfLibraryFilesDownloadedSoFar, TotalNumberOfLibraryFilesToDownload, LibraryUpdateStatus.Item4);
					double num = (double)_numberOfLibraryFilesDownloadedSoFar / (double)TotalNumberOfLibraryFilesToDownload * 100.0;
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdatePercentage((int)num);
				}
				break;
			case JobState.Error:
				ViewModelLocator.Current.UpdateInfoVM.IsLibraryUpdateInProgress = false;
				break;
			case JobState.TransientError:
				ViewModelLocator.Current.UpdateInfoVM.IsLibraryUpdateInProgress = false;
				break;
			case JobState.Transferred:
				UpdateCurrentLibraryDownloadInfo();
				if (TotalNumberOfLibraryFilesToDownload - _numberOfLibraryFilesDownloadedSoFar < 1)
				{
					CompleteTransferredLibraryJobs();
					ViewModelLocator.Current.UpdateInfoVM.LibraryUpdateName = LibraryUpdateStatus.Item4.ToString();
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdateInProgress(isUpdating: false);
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdateComplete();
				}
				break;
			case JobState.Canceled:
				ViewModelLocator.Current.UpdateInfoVM.IsLibraryUpdateInProgress = false;
				break;
			case JobState.Queued:
			case JobState.Connecting:
			case JobState.Suspended:
			case JobState.Acknowledged:
				break;
			}
		}
	}

	private static void Manager_OnJobTransferred(object sender, NotificationEventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Job Transferred: " + e.Job.DisplayName);
	}

	private static void StartUpdate(string jobName)
	{
		Manager.EnumJobs(JobOwner.CurrentUser);
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (item.DisplayName == jobName && item.State != JobState.Transferring)
			{
				item.Resume();
			}
		}
	}

	private static void UpdateCurrentLibraryDownloadInfo()
	{
		LibraryFilesDownloading = new Dictionary<string, int>();
		Manager.EnumJobs(JobOwner.CurrentUser);
		int num = 0;
		int num2 = 0;
		foreach (BitsJob item in Manager.Jobs.Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate"))
			{
				item.EnumFiles();
				num2 += (int)item.Progress.FilesTotal;
				num += (int)item.Progress.FilesTransferred;
			}
		}
		_numberOfLibraryFilesDownloadedSoFar = num;
		TotalNumberOfLibraryFilesToDownload = num2;
	}
}
