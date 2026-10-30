using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class WindowExportDesignViewModel : ViewModelBase
{
	private int _exportsAllowed;

	private int _exportsCompleted;

	private int _exportsRemaining;

	private bool _isDesignOptionDefault;

	private bool _isDesignOptionProngsExtended;

	private bool _isExportOption3DM;

	private bool _isExportOptionSTL;

	private bool _isRepairMeshEnabled;

	private bool _isStoneOptionAccentEnabled;

	private bool _isStoneOptionCenterEnabled;

	private bool _isStoneOptionSideEnabled;

	private string _savePath;

	private bool _isExportFeatureEnabled;

	private int _exportsCompletedCurrentMonth;

	public bool IsExportFeatureEnabled
	{
		get
		{
			return true;
		}
		set
		{
			_isExportFeatureEnabled = value;
			((ObservableObject)this).RaisePropertyChanged("IsExportFeatureEnabled");
		}
	}

	public bool IsRepairMeshEnabled
	{
		get
		{
			return _isRepairMeshEnabled;
		}
		set
		{
			_isRepairMeshEnabled = value;
			((ObservableObject)this).RaisePropertyChanged("IsRepairMeshEnabled");
		}
	}

	public string SavePath
	{
		get
		{
			return _savePath;
		}
		private set
		{
			_savePath = value;
			((ObservableObject)this).RaisePropertyChanged("SavePath");
		}
	}

	public RelayCommand CommandBrowse { get; set; }

	public RelayCommand CommandExportDesign { get; set; }

	public bool IsDesignOptionDefault
	{
		get
		{
			return _isDesignOptionDefault;
		}
		set
		{
			_isDesignOptionDefault = value;
			((ObservableObject)this).RaisePropertyChanged("IsDesignOptionDefault");
		}
	}

	public bool IsDesignOptionProngsExtended
	{
		get
		{
			return _isDesignOptionProngsExtended;
		}
		set
		{
			_isDesignOptionProngsExtended = value;
			((ObservableObject)this).RaisePropertyChanged("IsDesignOptionProngsExtended");
			UIHelper.ShowWaitWindow(waitVisible: true, Strings.PreparingDesign);
			UIHelper.CoreDataService.SetCAMModeState(value);
			UIHelper.ShowWaitWindow(waitVisible: false);
		}
	}

	public bool IsStoneOptionCenterEnabled
	{
		get
		{
			return _isStoneOptionCenterEnabled;
		}
		set
		{
			_isStoneOptionCenterEnabled = value;
			((ObservableObject)this).RaisePropertyChanged("IsStoneOptionCenterEnabled");
		}
	}

	public bool IsStoneOptionSideEnabled
	{
		get
		{
			return _isStoneOptionSideEnabled;
		}
		set
		{
			_isStoneOptionSideEnabled = value;
			((ObservableObject)this).RaisePropertyChanged("IsStoneOptionSideEnabled");
		}
	}

	public bool IsStoneOptionAccentEnabled
	{
		get
		{
			return _isStoneOptionAccentEnabled;
		}
		set
		{
			_isStoneOptionAccentEnabled = value;
			((ObservableObject)this).RaisePropertyChanged("IsStoneOptionAccentEnabled");
		}
	}

	public bool IsExportOptionSTL
	{
		get
		{
			return _isExportOptionSTL;
		}
		set
		{
			_isExportOptionSTL = value;
			((ObservableObject)this).RaisePropertyChanged("IsExportOptionSTL");
		}
	}

	public bool IsExportOption3DM
	{
		get
		{
			return _isExportOption3DM;
		}
		set
		{
			_isExportOption3DM = value;
			((ObservableObject)this).RaisePropertyChanged("IsExportOption3DM");
		}
	}

	public int ExportsCompleted
	{
		get
		{
			return _exportsCompleted;
		}
		set
		{
			_exportsCompleted = value;
			((ObservableObject)this).RaisePropertyChanged("ExportsCompleted");
		}
	}

	public int ExportsCompletedCurrentMonth
	{
		get
		{
			return _exportsCompletedCurrentMonth;
		}
		set
		{
			_exportsCompletedCurrentMonth = value;
			((ObservableObject)this).RaisePropertyChanged("ExportsCompletedCurrentMonth");
		}
	}

	public int ExportsAllowed
	{
		get
		{
			return int.MaxValue;
		}
		set
		{
			_exportsAllowed = value;
			((ObservableObject)this).RaisePropertyChanged("ExportsAllowed");
		}
	}

	public int ExportsRemaining
	{
		get
		{
			return int.MaxValue;
		}
		set
		{
			_exportsRemaining = value;
			((ObservableObject)this).RaisePropertyChanged("ExportsRemaining");
		}
	}

	public string ExportButtonText { get; set; }

	public WindowExportDesignViewModel()
	{
		IsDesignOptionDefault = true;
		IsExportOption3DM = true;
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitializeCommands();
			SavePath = UIHelper.CounterSketchOptions.LastDesignExportPath;
			Populate();
		}
		else
		{
			ExportsAllowed = 99;
			ExportsCompleted = 17;
			ExportsRemaining = ExportsAllowed - ExportsCompleted;
			IsExportFeatureEnabled = true;
		}
	}

	public void Populate()
	{
		ExportButtonText = Strings.Purchase;
	}

	private void Browse()
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			SavePath = folderBrowserDialog.SelectedPath;
			UIHelper.CounterSketchOptions.LastDesignExportPath = SavePath;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		else
		{
			SavePath = null;
		}
	}

	private void ExportDesign()
	{
		//Discarded unreachable code: IL_024d, IL_02eb
		if (IsExportFeatureEnabled)
		{
			UIHelper.RaiseApplicationMessageEvent(Strings.Purchase, ApplicationMessageType.AcceptPrompt, Strings.PurchaseDesignExportConfirmation, messageIsModal: true);
			if (UIHelper.LastMessageBoxAccepted != true)
			{
				return;
			}
		}
		if (!IsExportFeatureEnabled && ExportsRemaining < 1)
		{
			MessageBox.Show(Strings.No_More_Exports_Available);
			return;
		}
		if (string.IsNullOrWhiteSpace(SavePath))
		{
			MessageBox.Show(Strings.Specify_Output_Directory_Before_Export);
		}
		if (SavePath != null)
		{
			UIHelper.ShowWaitWindow(waitVisible: true, Strings.ExportingPleaseWait);
			bool flag = false;
			if (IsExportOption3DM)
			{
				string savePath = Path.Combine(SavePath, string.Format("Export_{1}_{0:MMddyyhhmmss}.3dm", DateTime.Now, UIHelper.LoadedDesign.SPINumber));
				flag = UIHelper.CoreDataService.ExportDesignAs3DM(savePath, IsStoneOptionCenterEnabled, IsStoneOptionSideEnabled, IsStoneOptionAccentEnabled);
			}
			if (IsExportOptionSTL)
			{
				string savePath2 = Path.Combine(SavePath, string.Format("Export_{1}_{0:MMddyyhhmmss}.stl", DateTime.Now, UIHelper.LoadedDesign.SPINumber));
				flag = UIHelper.CoreDataService.ExportDesignAsSTL(savePath2, IsStoneOptionCenterEnabled, IsStoneOptionSideEnabled, IsStoneOptionAccentEnabled, IsRepairMeshEnabled);
			}
			if (flag)
			{
				int designId = UIHelper.LoadedDesign.DesignId;
				bool flag2 = designId > 0;
				int numberofVisibleFreehandParts = UIHelper.CoreDataService.GetNumberofVisibleFreehandParts();
				bool flag3 = numberofVisibleFreehandParts > 0;
				string text = string.Empty;
				if (flag3)
				{
					text = string.Join(",", from _ in CustomizePartsListViewModel.PartsCollection
						where _.IsViewportVisible
						select _.Name);
				}
				string text2 = (IsExportOption3DM ? "3DM" : "STL");
				if (IsExportFeatureEnabled)
				{
					if (flag2 && flag3)
					{
						EventHelper.RecordEvent(EventHelper.EventTypes.Feature_Export_DesignWithParts, "ExportType|DesignId|PartList", $"{text2}|{designId}|{text}");
					}
					else if (flag2)
					{
						EventHelper.RecordEvent(EventHelper.EventTypes.Feature_Export_Design, "ExportType|DesignId", $"{text2}|{designId}");
					}
					else if (flag3)
					{
						EventHelper.RecordEvent(EventHelper.EventTypes.Feature_Export_Parts, "ExportType|PartList", text2 + "|" + text);
					}
				}
				if (flag2 && flag3)
				{
					EventHelper.RecordEvent(EventHelper.EventTypes.International_Export_DesignWithParts, "ExportType|DesignId|PartList", $"{text2}|{designId}|{text}");
				}
				else if (flag2)
				{
					EventHelper.RecordEvent(EventHelper.EventTypes.International_Export_Design, "ExportType|DesignId", $"{text2}|{designId}");
				}
				else if (flag3)
				{
					EventHelper.RecordEvent(EventHelper.EventTypes.International_Export_Parts, "ExportType|PartList", text2 + "|" + text);
				}
			}
			else
			{
				MessageBox.Show(Strings.Failed_To_Export_Design);
			}
		}
		Populate();
		UIHelper.ShowWaitWindow(waitVisible: false);
	}

	private void InitializeCommands()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		CommandBrowse = new RelayCommand((Action)Browse, false);
		CommandExportDesign = new RelayCommand((Action)ExportDesign, false);
	}
}
