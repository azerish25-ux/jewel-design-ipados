using System;
using System.IO;
using System.Threading.Tasks;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.KeyCodeServiceReference;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.ViewModel;
using GV.CounterSketchServices.WebServices;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class UpdateHelper
{
	public static bool CheckOnlineForUpdates()
	{
		GlobalErrorHandler.WriteMethodInfo("CheckOnlineForUpdates");
		if (UIHelper.IsInternetAccessible())
		{
			GlobalErrorHandler.WriteInfoToEventLog("Checking Online for Application Updates");
			UpdateSetCsVersion();
			UpdateCheckCsVersion();
			UpdateUserTypeId();
			if (UIHelper.CounterSketchOptions.UserTypeId < 7 && UIHelper.Version > UIHelper.LatestVersion)
			{
				UIHelper.RaiseApplicationMessageEvent("UpdateAll()", ApplicationMessageType.MultiLineAlert, string.Format("Your license does not permit running version {1} of CounterSketch.\n\nCounterSketch version {0} is your last eligible version.\n\nContact countersketch@gemvision.com to renew your license.", UIHelper.LatestVersionString, UIHelper.VersionString), messageIsModal: true);
				return false;
			}
			Task.Run(delegate
			{
				StullerAccountUtility.UpdateCustomerFromAccountNumber(UIHelper.CounterSketchOptions.ShipToAccountNumber);
			});
		}
		return true;
	}

	public static void ManualUpdate()
	{
		UIHelper.ShowWaitWindow(waitVisible: true);
		CheckOnlineForUpdates();
		TriggerCSUpdateOrDownload();
		CSLibraryHelper.LibraryUpdaterNew(runModelImportCheck: false);
		CSLibraryHelper.LibraryDatabaseUpdateNew();
		UIHelper.ShowWaitWindow(waitVisible: false);
	}

	public static void MinimumUpdate()
	{
		UpdateCheckAppDbSchema();
		CSLibraryHelper.SetUpCSGLibraryStaticFileLocation();
		CSLibraryHelper.LibraryDatabaseUpdateNew();
		CSLibraryHelper.IsInitialized = true;
		UIHelper.GenerateNewDesignTileImages();
	}

	public static bool UpdateAll()
	{
		return false;
	}

	public static void UpdateCheckAppDbSchema()
	{
		bool flag = false;
		string thisAppVersion = "0";
		using (CSApplicationDAL cSApplicationDAL = new CSApplicationDAL())
		{
			if (!cSApplicationDAL.DatabaseUpToDate())
			{
				thisAppVersion = cSApplicationDAL.GetDatabaseVersion();
				flag = true;
			}
		}
		GC.Collect();
		if (flag)
		{
			CSDatabaseUpdateDAL.ApplicationDatabaseSchemaUpdate(thisAppVersion);
		}
	}

	public static void UpdateCheckCsVersion()
	{
		GlobalErrorHandler.WriteMethodInfo("UpdateCheckCsVersion");
		using KeyCodeServiceClient keyCodeServiceClient = new KeyCodeServiceClient();
		string cSVersionNeeded = keyCodeServiceClient.GetCSVersionNeeded(UIHelper.CounterSketchOptions.GetKeyId());
		if (cSVersionNeeded != null)
		{
			UIHelper.LatestVersion = new Version(cSVersionNeeded);
			UIHelper.CounterSketchOptions.ApplicationLatestVersion = cSVersionNeeded;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		string rhinoBuildDateNeeded = keyCodeServiceClient.GetRhinoBuildDateNeeded(cSVersionNeeded);
		if (rhinoBuildDateNeeded != null)
		{
			UIHelper.LatestRhinoBuildDate = rhinoBuildDateNeeded;
			UIHelper.CounterSketchOptions.ApplicationLatestRhinoBuildDate = rhinoBuildDateNeeded;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
	}

	public static void TriggerCSUpdateOrDownload()
	{
		if (!(UIHelper.Version < UIHelper.LatestVersion))
		{
			return;
		}
		GlobalErrorHandler.WriteInfoToEventLog($"Application is out of Date, Version: {UIHelper.VersionString}, Latest: {UIHelper.LatestVersionString}");
		bool flag = UIHelper.CoreDataService.IsRhino64Bit();
		bool includeRhinoUpdate = false;
		string[] array = UIHelper.RhinoBuildDate.Split('-');
		DateTime dateTime = new DateTime(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
		string[] array2 = UIHelper.LatestRhinoBuildDate.Split('-');
		if (new DateTime(int.Parse(array2[0]), int.Parse(array2[1]), int.Parse(array2[2])) > dateTime || !flag)
		{
			includeRhinoUpdate = true;
			GlobalErrorHandler.WriteInfoToEventLog($"Rhino is out of Date for Latest CS Version Rhino will be added to the Update Job, Version: {UIHelper.RhinoBuildDate}, Latest: {UIHelper.LatestRhinoBuildDate}");
		}
		string text = string.Format("{0}{1}.exe", "CounterSketchInstaller_", UIHelper.LatestVersionString);
		if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.ApplicationMsiUpdatePath))
		{
			string[] array3 = UIHelper.CounterSketchOptions.ApplicationMsiUpdatePath.Split('\\');
			if (array3.Length >= 1 && array3[array3.Length - 1] == text && File.Exists(UIHelper.CounterSketchOptions.ApplicationMsiUpdatePath) && File.Exists(UIHelper.CounterSketchOptions.ApplicationRhinoUpdatePath))
			{
				GlobalErrorHandler.WriteInfoToEventLog($"Application is out of Date, but latest file is downloaded. Latest: {UIHelper.LatestVersionString}");
				ViewModelLocator.Current.UpdateInfoVM.SetApplicationUpdateComplete();
				return;
			}
		}
		BitsHelper.CreateApplicationUpdateJob(UIHelper.LatestVersionString, includeRhinoUpdate, UIHelper.LatestRhinoBuildDate);
		GlobalErrorHandler.WriteInfoToEventLog("Application is out of Date, Downloading Update Begun.");
	}

	public static void UpdateSetCsVersion()
	{
		GlobalErrorHandler.WriteMethodInfo("UpdateSetCsVersion");
		string rhinoExeVersion = string.Empty;
		string rhinoExeServiceRelease = string.Empty;
		string text = string.Empty;
		int keyId = UIHelper.CounterSketchOptions.GetKeyId();
		string versionString = UIHelper.VersionString;
		string text2 = UIHelper.CounterSketchOptions.Email;
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "";
		}
		string[] rhinoVersion = new string[0];
		UIHelper.CoreDataService.RhinoGetVersion(delegate(string version)
		{
			if (version != null)
			{
				rhinoVersion = version.Split(':');
			}
		});
		if (rhinoVersion != null)
		{
			rhinoExeVersion = rhinoVersion[0];
			rhinoExeServiceRelease = rhinoVersion[1];
			text = rhinoVersion[2];
		}
		UIHelper.CounterSketchOptions.RhinoSerialCode = UIHelper.CoreDataService.GetRhinoSerialCode();
		UIHelper.CounterSketchOptions.WriteSettings();
		if (text != null)
		{
			UIHelper.RhinoBuildDate = text;
			UIHelper.CounterSketchOptions.ApplicationCurrentRhinoBuildDate = text;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		string libraryVersion;
		using (CSLibraryDAL cSLibraryDAL = new CSLibraryDAL())
		{
			libraryVersion = cSLibraryDAL.GetLibraryVersion();
		}
		using KeyCodeServiceClient keyCodeServiceClient = new KeyCodeServiceClient();
		keyCodeServiceClient.SetCSVersionInfo(keyId, versionString, rhinoExeVersion, rhinoExeServiceRelease, text, libraryVersion, text2);
	}

	public static void UpdateUserTypeId()
	{
		int userTypeId = 1;
		if (UIHelper.IsInternetAccessible())
		{
			using (CounterSketchService counterSketchService = new CounterSketchService())
			{
				userTypeId = counterSketchService.GetUserTypeId(UIHelper.CounterSketchOptions.GetKeyId());
			}
			UIHelper.CounterSketchOptions.UserTypeId = userTypeId;
			UIHelper.CounterSketchOptions.WriteSettings();
			ViewModelLocator.Current.MainVM.IsTester = UIHelper.CounterSketchOptions.UserTypeId > 7;
		}
	}

	public static bool ValidateSerialOrKey()
	{
		//Discarded unreachable code: IL_0004, IL_0121
		return false;
	}
}
