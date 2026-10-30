using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using DesignLibrary;
using DesignLibrary.LibraryFormat;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketchServices.WebServices;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class CSLibraryHelper
{
	public static bool IsInitialized { get; set; }

	public static void LibraryDatabaseUpdateNew()
	{
		GlobalErrorHandler.WriteMethodInfo("LibraryDatabaseUpdateNew");
		if (File.Exists(UIHelper.CSLibraryDatabasePath))
		{
			try
			{
				File.Delete(UIHelper.CSLibraryDatabasePath);
			}
			catch
			{
			}
		}
		LibraryPacker.CreateSqLiteDBFromZBinFile(UIHelper.ZBinFileName, UIHelper.CSLibraryDatabasePath);
		GlobalErrorHandler.WriteInfoToEventLog("Updated Library DB Schema");
		using CSLibraryDAL cSLibraryDAL = new CSLibraryDAL();
		CSGLLibrary.CheckLibraryFiles(cSLibraryDAL.Connection);
		cSLibraryDAL.PopulateDesignSearch();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("CREATE INDEX IF NOT EXISTS IDX_DesignGroupDesigns_DesignGroupId on [DesignGroupDesigns]([DesignGroupId]);CREATE INDEX IF NOT EXISTS IDX_DesignAttribute_AttributeId on [Attribute]([AttributeId]);CREATE INDEX IF NOT EXISTS IDX_DesignKeywords on [Designs]([CreationDate], [CSGLFound], [Description], [Designer], [DesignId], [DesignTypeId], [DesignVersionId], [FirstReleaseLibraryId], [LastUpdateDate], [Name], [SeriesNumber], [StullerNo], [Version]);", cSLibraryDAL.Connection))
		{
			sQLiteCommand.ExecuteNonQuery();
		}
		ThreadPool.QueueUserWorkItem(delegate
		{
			SearchHelper.UpdateDesignKeywordsDictionary();
		});
		UIHelper.GenerateNewDesignTileImages();
	}

	public static bool LibraryUpdaterNew(bool runModelImportCheck)
	{
		if (runModelImportCheck && CSGLLibrary.CSGLsCount() < 50)
		{
			UIHelper.RaiseApplicationMessageEvent("LibraryModelCountCheck", ApplicationMessageType.SingleLineInfo, "Your library is incomplete.  \nPlease wait while library files download.", messageIsModal: true);
		}
		if (!UIHelper.IsInternetAccessible())
		{
			return false;
		}
		int myLibraryVersion;
		using (CounterSketchService counterSketchService = new CounterSketchService())
		{
			myLibraryVersion = counterSketchService.GetMyLibraryVersion(UIHelper.CounterSketchOptions.GetKeyId());
		}
		List<Designs> lstDesigns;
		int libraryVersion = GetLibraryVersion(UIHelper.ZBinFileName, out lstDesigns);
		List<string> neededfilenames;
		if (libraryVersion != myLibraryVersion)
		{
			Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Downloading_Updated_Design_Database);
			if (GetLibraryVersion(UIHelper.ZBinFileNameNew, out var lstDesigns2) != myLibraryVersion)
			{
				File.Delete(UIHelper.ZBinFileNameNew);
				GetLibraryVersion(UIHelper.ZBinFileNameNew, out lstDesigns2);
			}
			CSGLLibrary.CheckMissingFiles(lstDesigns2, out neededfilenames, deleteUnFound: false);
			CSGLLibrary.PullInMissingFromDir(neededfilenames, UIHelper.CSUpdatesPath);
			if (neededfilenames.Count != 0)
			{
				BitsHelper.LibraryUpdateStatus = new Tuple<int, int, int, int>(lstDesigns2.Count - neededfilenames.Count, lstDesigns2.Count, libraryVersion, myLibraryVersion);
				GetRequiredLibraryFiles(neededfilenames, myLibraryVersion.ToString());
				return false;
			}
			if (File.Exists(UIHelper.ZBinFileName))
			{
				File.Delete(UIHelper.ZBinFileName);
			}
			if (File.Exists(UIHelper.ZBinFileNameNew))
			{
				File.Move(UIHelper.ZBinFileNameNew, UIHelper.ZBinFileName);
			}
			lstDesigns = lstDesigns2;
		}
		CSGLLibrary.CheckMissingFiles(lstDesigns, out neededfilenames, lstDesigns.Count > 0);
		CSGLLibrary.PullInMissingFromDir(neededfilenames, UIHelper.CSUpdatesPath);
		if (neededfilenames.Count != 0)
		{
			BitsHelper.LibraryUpdateStatus = new Tuple<int, int, int, int>(lstDesigns.Count - neededfilenames.Count, lstDesigns.Count, libraryVersion, myLibraryVersion);
			Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Downloading_Missing_Design_Files);
			GetRequiredLibraryFiles(neededfilenames, libraryVersion + " (Library was missing designs.)");
			return false;
		}
		return true;
	}

	public static void SetUpCSGLibraryStaticFileLocation()
	{
		CSGLLibrary.SetLocalDir(UIHelper.CSModelLibraryCSGL);
	}

	private static int GetLibraryVersion(string zBinfilename, out List<Designs> lstDesigns)
	{
		if (!File.Exists(zBinfilename))
		{
			using CounterSketchService counterSketchService = new CounterSketchService();
			byte[] myLibraryZBin = counterSketchService.GetMyLibraryZBin(UIHelper.CounterSketchOptions.GetKeyId());
			if (myLibraryZBin == null || myLibraryZBin.Length == 0)
			{
				lstDesigns = new List<Designs>();
				return 0;
			}
			File.WriteAllBytes(zBinfilename, myLibraryZBin);
		}
		LibraryPacker.GetDesignsFromZBinFile(zBinfilename, out lstDesigns, out var libraryId);
		return libraryId;
	}

	private static void GetRequiredLibraryFiles(List<string> missingCsglFiles, string downloadMessage)
	{
		CSGLLibrary.CSGLsCount();
		int numberOfLibraryFilesDownloading = BitsHelper.GetNumberOfLibraryFilesDownloading();
		int count = missingCsglFiles.Count;
		if (count > 0 && numberOfLibraryFilesDownloading > 0 && count != numberOfLibraryFilesDownloading)
		{
			BitsHelper.CancelLibraryJobs();
		}
		if (!BitsHelper.AnyLibraryUpdateDownloading())
		{
			BitsHelper.CreateLibraryUpdateJob(missingCsglFiles.ToArray(), downloadMessage);
		}
	}
}
