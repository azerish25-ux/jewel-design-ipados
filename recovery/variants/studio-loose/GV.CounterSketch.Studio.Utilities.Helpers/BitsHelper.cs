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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate") && (int)item.State == 2)
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
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)_manager.Jobs).Values.ToList())
		{
			if (item.DisplayName == jobName)
			{
				item.Cancel();
			}
		}
	}

	public static void CancelLibraryJobs()
	{
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)_manager.Jobs).Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate"))
			{
				item.Cancel();
			}
		}
	}

	public static void Cleanup()
	{
		Manager.EnumJobs((JobOwner)0);
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
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)_manager.Jobs).Values.ToList())
		{
			if (item.DisplayName == jobName && (int)item.State == 6)
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate") && (int)item.State == 6)
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
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		if (!JobExists("DownloadCounterSketchStudioUpdate") || !IsExistingJobValid("DownloadCounterSketchStudioUpdate"))
		{
			string text = "Rhino_" + versionRhinoToDownload + ".zip";
			string text2 = "CounterSketchInstaller_" + versionToDownload + ".exe";
			BitsJob val = Manager.CreateJob("DownloadCounterSketchStudioUpdate", (JobType)0);
			val.Priority = (JobPriority)1;
			val.NotificationFlags = (NotificationFlags)11;
			val.Description = versionToDownload;
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
			val.AddFile(_remoteAddressApplicationPrefix + text2, text3);
			if (includeRhinoUpdate)
			{
				string text4 = Path.Combine(UIHelper.CSUpdatesPath, text);
				val.AddFile(_remoteAddressRhinoPrefix + text, text4);
				UIHelper.CounterSketchOptions.ApplicationRhinoUpdatePath = text4;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			else
			{
				UIHelper.CounterSketchOptions.ApplicationRhinoUpdatePath = null;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			val.ProxySettings.ProxyUsage = (ProxyUsage)3;
			BitsCredentials val2 = new BitsCredentials
			{
				UserName = "GEMVISION\\csupdates",
				Password = "REDACTED_EMBEDDED_CREDENTIAL",
				AuthenticationScheme = (AuthenticationScheme)3,
				AuthenticationTarget = (AuthenticationTarget)1
			};
			val.AddCredentials(val2);
			val.Resume();
		}
	}

	public static void CreateLibraryUpdateJob(string[] libraryFilesToDownload, string description)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		string[] array = libraryFilesToDownload.ToList().Distinct().ToArray();
		int num = 0;
		LibraryFilesDownloading = new Dictionary<string, int>();
		LibraryBitsJobs = new List<BitsJob>();
		while (LibraryFilesDownloading.Count < array.Length)
		{
			num++;
			BitsJob val = Manager.CreateJob("DownloadCounterSketchStudioLibraryUpdate_" + num, (JobType)0);
			val.Description = description;
			val.Priority = (JobPriority)1;
			val.NotificationFlags = (NotificationFlags)11;
			BitsCredentials val2 = new BitsCredentials
			{
				UserName = "GEMVISION\\csupdates",
				Password = "REDACTED_EMBEDDED_CREDENTIAL",
				AuthenticationScheme = (AuthenticationScheme)3,
				AuthenticationTarget = (AuthenticationTarget)1
			};
			val.AddCredentials(val2);
			val.ProxySettings.ProxyUsage = (ProxyUsage)3;
			int num2 = 1;
			for (int i = GetStartingFileIndex(num); i < array.Count(); i++)
			{
				if (num2 > 200)
				{
					break;
				}
				string text = Path.Combine(UIHelper.CSUpdatesPath, array[i]);
				val.AddFile(_remoteAddressLibraryPrefix + array[i], text);
				LibraryFilesDownloading.Add(text, num);
				num2++;
			}
			LibraryBitsJobs.Add(val);
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
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		try
		{
			Manager.EnumJobs((JobOwner)0);
			foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
			{
				if (item.DisplayName == jobName && (int)item.State != 6)
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
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
		{
			if (item.DisplayName == jobName)
			{
				return item.State;
			}
		}
		return (JobState)4;
	}

	public static int GetNumberOfLibraryFilesDownloading()
	{
		Manager.EnumJobs((JobOwner)0);
		int num = 0;
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
		{
			if (item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate"))
			{
				item.EnumFiles();
				if (item.Files != null)
				{
					num += ((List<BitsFile>)(object)item.Files).Count;
				}
			}
		}
		return num;
	}

	public static void Initialize()
	{
		UpdateServerLocationForDownloads(UIHelper.CounterSketchOptions.UseStullerServerForUpdates);
		Manager.EnumJobs((JobOwner)0);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
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
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
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
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		LibraryFilesDownloading = new Dictionary<string, int>();
		Manager.EnumJobs((JobOwner)0);
		_numberOfLibraryFilesDownloadedSoFar = 0;
		TotalNumberOfLibraryFilesToDownload = 0;
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
		{
			if (!item.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate") || (int)item.State == 6 || (int)item.State == 4 || (int)item.State == 5)
			{
				continue;
			}
			item.EnumFiles();
			TotalNumberOfLibraryFilesToDownload += (int)item.Progress.FilesTotal;
			_numberOfLibraryFilesDownloadedSoFar += (int)item.Progress.FilesTransferred;
			foreach (BitsFile item2 in (List<BitsFile>)(object)item.Files)
			{
				if (!LibraryFilesDownloading.ContainsKey(item2.LocalName))
				{
					LibraryFilesDownloading.Add(item2.LocalName, 99);
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected I4, but got Unknown
		JobState val = CurrentStateOfUpdate(jobName);
		switch ((int)val)
		{
		case 7:
			return true;
		case 2:
			return true;
		case 1:
			return true;
		case 6:
			CompleteTransferredJobs(jobName);
			return true;
		case 0:
			StartUpdate(jobName);
			return true;
		case 3:
			CancelJob(jobName);
			break;
		case 4:
			CancelJob(jobName);
			break;
		case 5:
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
		Manager.EnumJobs((JobOwner)0);
		return ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList().Any((BitsJob job) => job.DisplayName == jobName);
	}

	private static void Manager_OnJobAdded(object sender, NotificationEventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Job Added: " + e.Job.DisplayName);
	}

	private static void Manager_OnJobError(object sender, ErrorNotificationEventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Job Errored: " + ((NotificationEventArgs)e).Job.DisplayName);
	}

	private static void Manager_OnJobModified(object sender, NotificationEventArgs e)
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected I4, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected I4, but got Unknown
		string displayName = e.Job.DisplayName;
		if (displayName != null && displayName == "DownloadCounterSketchStudioUpdate")
		{
			JobState state = e.Job.State;
			switch ((int)state)
			{
			case 2:
				ViewModelLocator.Current.UpdateInfoVM.SetApplicationUpdateInProgress(GetJobDescription("DownloadCounterSketchStudioUpdate") ?? "", isUpdating: true);
				ViewModelLocator.Current.UpdateInfoVM.SetApplicationUpdatePercentage((int)CurrentProgressPercentage("DownloadCounterSketchStudioUpdate"));
				break;
			case 3:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case 4:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case 5:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case 6:
				CompleteTransferredJobs("DownloadCounterSketchStudioUpdate");
				ViewModelLocator.Current.UpdateInfoVM.SetApplicationUpdateComplete();
				break;
			case 8:
				ViewModelLocator.Current.UpdateInfoVM.IsApplicationUpdateInProgress = false;
				break;
			case 0:
			case 1:
			case 7:
				break;
			}
		}
		else
		{
			if (!e.Job.DisplayName.Contains("DownloadCounterSketchStudioLibraryUpdate"))
			{
				return;
			}
			JobState state = e.Job.State;
			switch ((int)state)
			{
			case 2:
				if (LibraryUpdateStatus != null)
				{
					UpdateCurrentLibraryDownloadInfo();
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdateInProgress(isUpdating: true);
					ViewModelLocator.Current.UpdateInfoVM.LibraryUpdateName = string.Format("{2}, Designs {0} of {1}", _numberOfLibraryFilesDownloadedSoFar, TotalNumberOfLibraryFilesToDownload, LibraryUpdateStatus.Item4);
					double num = (double)_numberOfLibraryFilesDownloadedSoFar / (double)TotalNumberOfLibraryFilesToDownload * 100.0;
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdatePercentage((int)num);
				}
				break;
			case 4:
				ViewModelLocator.Current.UpdateInfoVM.IsLibraryUpdateInProgress = false;
				break;
			case 5:
				ViewModelLocator.Current.UpdateInfoVM.IsLibraryUpdateInProgress = false;
				break;
			case 6:
				UpdateCurrentLibraryDownloadInfo();
				if (TotalNumberOfLibraryFilesToDownload - _numberOfLibraryFilesDownloadedSoFar < 1)
				{
					CompleteTransferredLibraryJobs();
					ViewModelLocator.Current.UpdateInfoVM.LibraryUpdateName = LibraryUpdateStatus.Item4.ToString();
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdateInProgress(isUpdating: false);
					ViewModelLocator.Current.UpdateInfoVM.SetLibraryUpdateComplete();
				}
				break;
			case 8:
				ViewModelLocator.Current.UpdateInfoVM.IsLibraryUpdateInProgress = false;
				break;
			case 0:
			case 1:
			case 3:
			case 7:
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
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		Manager.EnumJobs((JobOwner)0);
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
		{
			if (item.DisplayName == jobName && (int)item.State != 2)
			{
				item.Resume();
			}
		}
	}

	private static void UpdateCurrentLibraryDownloadInfo()
	{
		LibraryFilesDownloading = new Dictionary<string, int>();
		Manager.EnumJobs((JobOwner)0);
		int num = 0;
		int num2 = 0;
		foreach (BitsJob item in ((Dictionary<Guid, BitsJob>)(object)Manager.Jobs).Values.ToList())
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
