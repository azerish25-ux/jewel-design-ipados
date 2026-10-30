using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Localization;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio.ViewModel;

public class UserPartsViewModel : ViewModelBase
{
	private static readonly string _pathToLegacyUserParts = Path.Combine(UIHelper.CSDataRootPath, "Library\\Parts");

	private static readonly string _pathToUserParts = Path.Combine(UIHelper.CSDataRootPath, "UserParts");

	private static BackgroundWorker _backgroundWorker;

	private ObservableCollection<PartItem> _parts;

	public RelayCommand<PartItem> CommandAddSelectedPart { get; set; }

	public RelayCommand<PartItem> CommandDeleteUserPart { get; set; }

	public RelayCommand CommandImportPart { get; set; }

	public ObservableCollection<PartItem> Parts
	{
		get
		{
			return _parts;
		}
		set
		{
			_parts = value;
			RaisePropertyChanged("Parts");
		}
	}

	public UserPartsViewModel()
	{
		if (base.IsInDesignMode)
		{
			Parts = new ObservableCollection<PartItem>();
		}
		else
		{
			InitializeCommands();
		}
	}

	public void AddSelectedPart(PartItem partItem)
	{
		UIHelper.ShowWaitWindow(waitVisible: true);
		GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
		ViewModelLocator.Current.MainVM.IsViewportControlsVisible = true;
		UIHelper.CoreDataService.SetComponentVisibility(ComponentsEnum.Ring, visible: true);
		ViewModelLocator.Current.DesignStackVM.UpdateDesignStackDisplayComponents(isResetingComponents: true);
		EventHelper.RecordEvent(EventHelper.EventTypes.Part_Opened, "UserPart", partItem.Name);
		if (partItem.IsCounterPart)
		{
			UIHelper.CoreDataService.DocumentImportCSGPart(partItem.Item3DMPath);
		}
		else
		{
			UIHelper.CoreDataService.InvokePartPatchWire(partItem.Item3DMPath, partItem.Name, ViewModelLocator.Current.DesignStackVM.UpdateDesignStackUIControls);
		}
		ViewModelLocator.Current.DesignStackVM.ExpandLastControlGroup();
		ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = false;
		ViewModelLocator.Current.BreadCrumbVM.RefreshOrderVisibility();
		UIHelper.ShowWaitWindow(waitVisible: false);
	}

	public void ImportPart(string pathToPart, bool skipPartReload)
	{
		GlobalErrorHandler.WriteInfoToEventLog("User requested Part Import of: " + pathToPart, 2);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathToPart);
		string extension = Path.GetExtension(pathToPart);
		string text = ((extension == ".3dm") ? "SP" : "CP");
		Guid guid = Guid.NewGuid();
		if (fileNameWithoutExtension != null)
		{
			fileNameWithoutExtension = fileNameWithoutExtension.Replace("_", "");
			string path = $"{fileNameWithoutExtension}_{text}_{guid}{extension}";
			string path2 = $"{fileNameWithoutExtension}_{text}_{guid}.png";
			string text2 = Path.Combine(_pathToUserParts, path);
			string text3 = Path.Combine(_pathToUserParts, path2);
			if (File.Exists(text2))
			{
				File.SetAttributes(text2, FileAttributes.Normal);
				File.Delete(text2);
			}
			if (File.Exists(text3))
			{
				File.SetAttributes(text3, FileAttributes.Normal);
				File.Delete(text3);
			}
			File.Copy(pathToPart, text2);
			GlobalErrorHandler.WriteInfoToEventLog("Extracting Image from Part file to: " + text3, 2);
			BitmapSource thumbnailImageSource = null;
			UIHelper.CoreDataService.GetThumbnailFromFile(text2, delegate(BitmapSourceByteArrayInfo bitmapSourceByteArrayInfo)
			{
				thumbnailImageSource = bitmapSourceByteArrayInfo.ItemImageSource;
			});
			ImageHelper.ExportBitmapImageAsPNG(thumbnailImageSource, text3);
			EventHelper.RecordEvent(EventHelper.EventTypes.Part_Imported);
			GlobalErrorHandler.WriteInfoToEventLog("User requested Part Imported to: " + text2, 2);
		}
		if (!skipPartReload)
		{
			GlobalErrorHandler.WriteInfoToEventLog("Reprocessing Part Files after Single File Import.", 2);
			ReloadPartsOnDisk();
		}
	}

	public void ImportParts(string[] pathsToParts)
	{
		foreach (string pathToPart in pathsToParts)
		{
			ImportPart(pathToPart, skipPartReload: true);
		}
		GlobalErrorHandler.WriteInfoToEventLog("Reprocessing Part Files after Multiple Part Import.", 2);
		ReloadPartsOnDisk();
	}

	public void Populate()
	{
		if (_backgroundWorker == null)
		{
			_backgroundWorker = new BackgroundWorker();
			_backgroundWorker.DoWork += PopulateParts_DoWork;
			_backgroundWorker.RunWorkerCompleted += PopulateParts_Completed;
			_backgroundWorker.RunWorkerAsync();
		}
	}

	private void DeleteUserPart(PartItem part)
	{
		if (MessageBox.Show(Strings.PartDeletionMessage, Strings.Delete, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			if (File.Exists(part.Item3DMPath))
			{
				File.SetAttributes(part.Item3DMPath, FileAttributes.Normal);
				File.Delete(part.Item3DMPath);
			}
			if (File.Exists(part.ItemImagePath))
			{
				File.SetAttributes(part.ItemImagePath, FileAttributes.Normal);
				File.Delete(part.ItemImagePath);
			}
			ReloadPartsOnDisk();
		}
	}

	private void ImportPart()
	{
		Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
		{
			Multiselect = true,
			Filter = "CounterSketch Document (*.csg)|*.csg|Rhino Document (*.3dm)|*.3dm|All files (*.*)|*.*"
		};
		if (openFileDialog.ShowDialog() == true)
		{
			string[] fileNames = openFileDialog.FileNames;
			ImportParts(fileNames);
		}
	}

	private void InitializeCommands()
	{
		CommandDeleteUserPart = new RelayCommand<PartItem>(DeleteUserPart);
		CommandAddSelectedPart = new RelayCommand<PartItem>(AddSelectedPart);
		CommandImportPart = new RelayCommand(ImportPart);
	}

	private void PopulateParts_Completed(object sender, RunWorkerCompletedEventArgs e)
	{
		Parts = (ObservableCollection<PartItem>)e.Result;
	}

	private void PopulateParts_DoWork(object sender, DoWorkEventArgs e)
	{
		if (!Directory.Exists(_pathToUserParts))
		{
			GlobalErrorHandler.WriteInfoToEventLog("Creating missing User Parts folder: " + _pathToUserParts, 2);
			Directory.CreateDirectory(_pathToUserParts);
		}
		if (Directory.Exists(_pathToLegacyUserParts))
		{
			GlobalErrorHandler.WriteInfoToEventLog("Legacy Parts Exist, Migrating to new Location.");
			string[] files = Directory.GetFiles(_pathToLegacyUserParts, "User_Imported_SP*.3dm");
			foreach (string text in files)
			{
				string fileName = Path.GetFileName(text);
				if (string.IsNullOrEmpty(fileName))
				{
					continue;
				}
				string[] array = fileName.Split('_');
				string directoryName = Path.GetDirectoryName(text);
				if (directoryName != null)
				{
					string text2 = Path.Combine(directoryName, array[array.Length - 1]);
					if (File.Exists(text2))
					{
						File.SetAttributes(text2, FileAttributes.Normal);
						File.Delete(text2);
					}
					GlobalErrorHandler.WriteInfoToEventLog("Migrating Part: " + text2);
					File.Move(text, text2);
					ImportPart(text2, skipPartReload: true);
					File.Delete(text2);
				}
			}
			GlobalErrorHandler.WriteInfoToEventLog("Deleting Remaining Legacy Part Files");
			FileFolderHelper.DelTree(_pathToLegacyUserParts);
		}
		List<string> list = Directory.GetFiles(_pathToUserParts, "*.3dm", SearchOption.AllDirectories).ToList();
		List<string> list2 = Directory.GetFiles(_pathToUserParts, "*.csg", SearchOption.AllDirectories).ToList();
		if (list.Count <= 0 && list2.Count <= 0)
		{
			return;
		}
		list.AddRange(list2);
		List<PartItem> partList = new List<PartItem>();
		list.ForEach(delegate(string p)
		{
			DateTime creationTime = File.GetCreationTime(p);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(p);
			if (fileNameWithoutExtension != null)
			{
				string[] array2 = fileNameWithoutExtension.Split('_');
				if (array2.Length >= 3 && array2.Length == 3)
				{
					PartItem item = new PartItem
					{
						Name = array2[0],
						IsCounterPart = (array2[1].ToUpper() == "CP"),
						Item3DMPath = p,
						ItemImagePath = Path.Combine(_pathToUserParts, fileNameWithoutExtension + ".png"),
						CreateDate = creationTime
					};
					partList.Add(item);
				}
			}
		});
		IOrderedEnumerable<PartItem> collection = from x in partList
			orderby x.Name, x.CreateDate descending, x.IsCounterPart
			select x;
		e.Result = new ObservableCollection<PartItem>(collection);
	}

	private void ReloadPartsOnDisk()
	{
		_backgroundWorker?.RunWorkerAsync();
	}
}
