using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class CustomizePartsListViewModel : ViewModelBase
{
	private static readonly SolidColorBrush _groupColorBrush = new SolidColorBrush(Colors.LightSlateGray);

	private static DispatcherTimer _importGroupHighlightDispatcherTimer = new DispatcherTimer();

	private static DispatcherTimer _importGroupSelectionDispatcherTimer = new DispatcherTimer();

	private static ImportGroup _importGroupToHighlight;

	private BitmapImage _designImage;

	private bool _isDesignImageVisible;

	private bool _isSubMenuVisible;

	private bool _isPartsListVisible = true;

	private string _oldPartNamePlaceholderText;

	private string _lastPartNameUsed;

	private Guid _partRenamingId = Guid.Empty;

	private bool _isFreehandCheckBoxVisible;

	private bool _isFreehandOnly;

	private bool _isMultiSelect;

	public bool IsMultiplePartsSelected => SelectedImportGroups.Count > 1;

	public SolidColorBrush GroupColorBrush => _groupColorBrush;

	public static ObservableCollection<ImportGroup> PartsCollection { get; } = new ObservableCollection<ImportGroup>();

	public ObservableCollection<ImportGroup> ImportGroupsCollection => PartsCollection;

	public string SelectedImportGroupName
	{
		get
		{
			ImportGroup importGroup = SelectedImportGroups.FirstOrDefault((ImportGroup _) => _.IsSelectedImportGroup);
			string result = string.Empty;
			if (importGroup != null)
			{
				result = SelectedImportGroups.Select((ImportGroup _) => _.Name).Aggregate((string current, string next) => current + ", " + next);
			}
			return result;
		}
	}

	public static List<Guid> SelectedImportGroupIdentifiers => SelectedImportGroups.Select((ImportGroup part) => part.Identifier).ToList();

	public static List<ImportGroup> SelectedImportGroups => PartsCollection.Where((ImportGroup _) => _.IsSelectedImportGroup || _.IsSecondarySelectedImportGroup).ToList();

	public bool IsPartsListMultiSelect
	{
		get
		{
			return _isMultiSelect;
		}
		set
		{
			_isMultiSelect = value;
			UIHelper.CounterSketchOptions.IsCustomizePartsListMultiSelect = value;
			UIHelper.CounterSketchOptions.WriteSettings();
			RaisePropertyChanged("IsPartsListMultiSelect");
		}
	}

	public RelayCommand CommandAddRingRailGuide { get; set; } = new RelayCommand(AddRingRailGuide);

	public bool IsFreehandOnly
	{
		get
		{
			return _isFreehandOnly;
		}
		set
		{
			_isFreehandOnly = value;
			RaisePropertyChanged("IsFreehandOnly");
		}
	}

	public string OldPartNamePlaceholderText
	{
		get
		{
			return _oldPartNamePlaceholderText;
		}
		set
		{
			_oldPartNamePlaceholderText = value;
			RaisePropertyChanged("OldPartNamePlaceholderText");
		}
	}

	public bool IsPartsListVisible
	{
		get
		{
			return _isPartsListVisible;
		}
		set
		{
			_isPartsListVisible = value;
			RaisePropertyChanged("IsPartsListVisible");
		}
	}

	public bool IsSubMenuVisible
	{
		get
		{
			return _isSubMenuVisible;
		}
		set
		{
			_isSubMenuVisible = value;
			RaisePropertyChanged("IsSubMenuVisible");
		}
	}

	public RelayCommand<Guid> CommandAddToPartGroup { get; set; }

	public RelayCommand CommandToggleMultiSelectForPartsList { get; set; }

	public BitmapImage DesignImage
	{
		get
		{
			return _designImage;
		}
		set
		{
			_designImage = value;
			RaisePropertyChanged("DesignImage");
		}
	}

	public bool IsDesignImageVisible
	{
		get
		{
			return _isDesignImageVisible;
		}
		set
		{
			_isDesignImageVisible = value;
			RaisePropertyChanged("IsDesignImageVisible");
		}
	}

	public RelayCommand CommandShowHideSubMenu { get; set; }

	public RelayCommand<Guid> CommandResetPart { get; set; }

	public RelayCommand<Guid> CommandDeletePart { get; set; }

	public RelayCommand CommandTogglePartVisibility { get; set; }

	public RelayCommand CommandDuplicateImportGroup { get; set; }

	public RelayCommand CommandTogglePrimarySecondaryMetal { get; set; }

	public RelayCommand<Guid> CommandSelectedImportGroup { get; set; }

	public RelayCommand CommandSetDesignStackVisible { get; set; }

	public RelayCommand CommandSetFreehandStackVisible { get; set; }

	public RelayCommand CommandAddPart { get; private set; }

	public RelayCommand CommandAddPartToGroup { get; set; }

	public RelayCommand<Guid> CommandShowRenamePrompt { get; set; }

	public RelayCommand CommandExportSelectedImportGroupsToPath { get; set; } = new RelayCommand(ExportSelectedImportGroupsToPath);

	public RelayCommand CommandExportSelectedImportGroupsToUserParts { get; set; } = new RelayCommand(ExportSelectedImportGroupsToUserParts);

	public RelayCommand<Guid> CommandMakeSelectedPartGroupParent { get; set; }

	public RelayCommand<Guid> CommandSelectGroup { get; set; }

	public RelayCommand CommandClosePartToolbox { get; set; }

	public RelayCommand CommandToggleCommonCenterPoint { get; set; }

	public RelayCommand<ImportGroup> CommandHighlightImportGroup { get; set; }

	public RelayCommand<Guid> CommandRenameImportGroup { get; set; }

	public RelayCommand<Guid> CommandBreakPartGroup { get; set; }

	public bool IsFreehandCheckBoxVisible
	{
		get
		{
			return _isFreehandCheckBoxVisible;
		}
		set
		{
			_isFreehandCheckBoxVisible = value;
			RaisePropertyChanged("IsFreehandCheckBoxVisible");
		}
	}

	public CustomizePartsListViewModel()
	{
		Reset();
		if (!base.IsInDesignMode)
		{
			InitializeCommands();
			PartsCollection.CollectionChanged += PartsCollectionOnCollectionChanged;
		}
		else
		{
			PartsCollection.Add(new ImportGroup
			{
				BlockIdentifiers = new List<Guid> { Guid.NewGuid() },
				IsGroup = true,
				IsParentPart = true,
				Name = "Test",
				IsSelectedImportGroup = true
			});
			IsPartsListVisible = true;
			RaisePropertyChanged("SelectedImportGroupName");
		}
		IsFreehandOnly = UIHelper.KeyProfile.IsDesignLibraryDisabled;
		IsPartsListMultiSelect = UIHelper.CounterSketchOptions.IsCustomizePartsListMultiSelect;
	}

	public static void AddAGem()
	{
		ImportPart(Path.Combine(UIHelper.CSLibraryPath, "Parts\\6853.csg"));
	}

	public void AddPart(bool goBackToTiles)
	{
		ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize);
		ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = false;
		if (goBackToTiles)
		{
			ViewModelLocator.Current.TileViewPartsVM.SetSectionTransitionTimeToInstant();
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.NavigateBackToTiles();
		}
		ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = true;
	}

	public void AddPart()
	{
		AddPart(goBackToTiles: true);
	}

	public static void AddRingRailGuide()
	{
		ImportPart(Path.Combine(UIHelper.CSLibraryPath, "Parts\\RingRailGuide.csg"));
	}

	public static void AddTextTool()
	{
		ImportPart(Path.Combine(UIHelper.CSLibraryPath, "Parts\\FHTextTool.csg"), doShowFontSourceErrors: false);
	}

	public void ClosePartToolbox()
	{
		ViewModelLocator.Current.TileViewPartsVM.IsInEditMode = false;
		ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = false;
		ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = true;
	}

	public ImportGroup GetImportGroupFromGuid(Guid guid)
	{
		return PartsCollection.FirstOrDefault((ImportGroup _) => _.Identifier == guid);
	}

	public static string GetUserExportPartName()
	{
		string question = $"Please provide a name for your part... \n Cannot contain ({Path.GetInvalidFileNameChars()})";
		WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.QuestionAnswer);
		windowMessage.Title = "User Library Part Name";
		windowMessage.Question = question;
		windowMessage.Topmost = true;
		windowMessage.Owner = System.Windows.Application.Current.MainWindow;
		windowMessage.ShowDialog();
		windowMessage.Focus();
		return windowMessage.Answer;
	}

	public void Reset()
	{
		ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = true;
		PartsCollection.Clear();
		IsFreehandCheckBoxVisible = false;
		IsSubMenuVisible = false;
	}

	public void UpdateImportGroupImageCaptures(Guid identifier, Bitmap bitmap)
	{
		if (PartsCollection != null)
		{
			ImportGroup importGroup = PartsCollection?.FirstOrDefault((ImportGroup _) => _.Identifier == identifier);
			if (importGroup != null)
			{
				importGroup.ImportGroupBitmapImage = ImageHelper.ConvertBitmapToBitmapImage(bitmap);
				_ = importGroup.IsSelectedImportGroup;
			}
		}
	}

	public void UpdatePartsCollection(IEnumerable<ImportGroup> importGroups)
	{
		IsDesignImageVisible = true;
		if (UIHelper.LoadedDesign.DesignId != 0 || UIHelper.KeyProfile.IsDesignLibraryDisabled)
		{
			DesignImage = ImageHelper.ConvertBitmapToBitmapImage(UIHelper.CoreDataService.RequestScreenCaptureDefault(new System.Drawing.Size(300, 300), zoom: true, 0.0, isMainDesignOnly: true));
		}
		else
		{
			DesignImage = ImageHelper.ConvertBitmapToBitmapImage(UIHelper.CoreDataService.RequestScreenCaptureDefault(new System.Drawing.Size(300, 300), zoom: true, 0.0));
		}
		bool newPartAdded = false;
		if (importGroups.Any())
		{
			((importGroups as IList<ImportGroup>) ?? importGroups.ToList())?.ToList().ForEach(delegate(ImportGroup _)
			{
				ImportGroup importGroup = PartsCollection.FirstOrDefault((ImportGroup part) => _.Identifier == part.Identifier);
				if (importGroup != null)
				{
					importGroup.IsSelectedImportGroup = _.IsSelectedImportGroup;
					importGroup.IsSecondarySelectedImportGroup = _.IsSecondarySelectedImportGroup;
				}
				else if (PartsCollection.All((ImportGroup p) => _.Identifier != p.Identifier))
				{
					PartsCollection.Add(_);
					newPartAdded = true;
				}
				System.Windows.Application.Current.Dispatcher.InvokeAsync(_.RaiseSelectionStateChanged);
			});
			if (PartsCollection.Any((ImportGroup _) => _.HasSelectionChanged) || newPartAdded)
			{
				PartsCollection.Where((ImportGroup _) => _.HasSelectionChanged).ForEach(delegate(ImportGroup _)
				{
					_.HasSelectionChanged = false;
				});
				System.Windows.Application.Current.Dispatcher.InvokeAsync(delegate
				{
					UpdateSelectionIndicatorAndDesignStackVisibility();
				});
			}
		}
		else
		{
			ViewModelLocator.Current.FreehandStackVM.ClearComponentContainersAndCategories();
		}
		IsFreehandCheckBoxVisible = PartsCollection.Any();
	}

	private void AddToPartGroup()
	{
		UIHelper.CoreDataService.ImportGroupAddToPartGroup(SelectedImportGroupIdentifiers);
		SelectedImportGroups.Where((ImportGroup _) => _.IsSelected).ForEach(delegate(ImportGroup _)
		{
			PartsCollection.Remove(_);
		});
	}

	private void BreakPartGroup(Guid importGroupIdentifier)
	{
		ImportGroup importGroup = PartsCollection.FirstOrDefault((ImportGroup _) => _.Identifier == importGroupIdentifier);
		if (importGroup != null)
		{
			PartsCollection.Remove(importGroup);
			UIHelper.CoreDataService.ImportGroupBreakPartGroup(importGroupIdentifier);
		}
	}

	private static bool CheckSelectedImportGroupsAndPrompt()
	{
		if (SelectedImportGroups == null || !SelectedImportGroups.Any())
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.MustSelectFreehandPrompt, messageIsModal: true);
			return false;
		}
		return true;
	}

	private void ClearCurrentlySelectedImportGroups()
	{
		SelectedImportGroups.ForEach(delegate(ImportGroup _)
		{
			_.IsSecondarySelectedImportGroup = false;
			_.IsSelectedImportGroup = false;
		});
		RaisePropertyChanged("SelectedImportGroupName");
	}

	private void DeleteGroupOrPart(Guid id)
	{
		ImportGroup importGroup = PartsCollection.FirstOrDefault((ImportGroup _) => _.Identifier == id);
		if (importGroup != null)
		{
			if (importGroup.IsGroup)
			{
				UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.AcceptPrompt, Strings.PartDeletionMessage, messageIsModal: true);
			}
			if (UIHelper.LastMessageBoxAccepted == true || !importGroup.IsGroup)
			{
				DeselectImportGroup(importGroup);
				PartsCollection.Remove(importGroup);
				ViewModelLocator.Current.FreehandStackVM.RemoveImportedControlsFromCachedControlCollection(importGroup.Identifier);
				UIHelper.CoreDataService.ImportGroupDeleteFromGraph(new List<Guid> { id }, ViewModelLocator.Current.DesignStackVM.UpdateDesignStackUIControls);
			}
		}
		ClearCurrentlySelectedImportGroups();
		UpdateSelectionIndicatorAndDesignStackVisibility();
		IsFreehandCheckBoxVisible = PartsCollection.Any();
	}

	private void DeselectImportGroup(ImportGroup part)
	{
		if (SelectedImportGroups.Contains(part))
		{
			part.IsSelectedImportGroup = false;
			part.IsSecondarySelectedImportGroup = false;
		}
	}

	private void DuplicateImportGroup(List<Guid> partIds)
	{
		UIHelper.CoreDataService.ImportGroupDuplicate(partIds);
	}

	private void DuplicateSelected()
	{
		if (CheckSelectedImportGroupsAndPrompt())
		{
			DuplicateImportGroup(SelectedImportGroupIdentifiers);
		}
	}

	private static void ExportSelectedImportGroups(bool isAutoSavingToUserPartsLibrary, string exportPath = null)
	{
		string text = string.Empty;
		if (isAutoSavingToUserPartsLibrary)
		{
			do
			{
				text = GetUserExportPartName().Trim();
			}
			while (text.Any((char character) => Path.GetInvalidFileNameChars().Contains(character)));
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
		}
		string text2 = ((!isAutoSavingToUserPartsLibrary) ? exportPath : (text2 = Path.Combine(Path.GetTempPath(), text + ".csg")));
		UIHelper.CoreDataService.ImportGroupExport(SelectedImportGroupIdentifiers, text2);
		if (isAutoSavingToUserPartsLibrary)
		{
			ViewModelLocator.Current.UserPartsVM.ImportPart(text2, skipPartReload: false);
		}
	}

	private static void ExportSelectedImportGroupsToPath()
	{
		if (!CheckSelectedImportGroupsAndPrompt())
		{
			return;
		}
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			Filter = "CounterSketch Design File|*.csg",
			Title = "Save a CounterSketch Design File"
		};
		saveFileDialog.ShowDialog();
		if (saveFileDialog.FileName == "")
		{
			return;
		}
		try
		{
			ExportSelectedImportGroups(isAutoSavingToUserPartsLibrary: false, saveFileDialog.FileName);
		}
		catch (UnauthorizedAccessException)
		{
		}
	}

	private static void ExportSelectedImportGroupsToUserParts()
	{
		if (CheckSelectedImportGroupsAndPrompt())
		{
			ExportSelectedImportGroups(isAutoSavingToUserPartsLibrary: true);
		}
	}

	private void HighlightImportGroup(ImportGroup importGroup)
	{
		_importGroupHighlightDispatcherTimer?.Stop();
		_importGroupToHighlight = importGroup;
		_importGroupHighlightDispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 250), DispatcherPriority.Background, SetRhinoHighlight, System.Windows.Application.Current.Dispatcher);
		_importGroupHighlightDispatcherTimer.Start();
	}

	private static void ImportPart(string pathToPart, bool doShowFontSourceErrors = true)
	{
		UIHelper.ShowWaitWindow(waitVisible: true);
		GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
		ViewModelLocator.Current.MainVM.IsViewportControlsVisible = true;
		UIHelper.CoreDataService.DocumentImportCSGPart(pathToPart, null, doShowFontSourceErrors);
		ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = false;
		UIHelper.ShowWaitWindow(waitVisible: false);
	}

	private void InitializeCommands()
	{
		CommandSetDesignStackVisible = new RelayCommand(SetDesignStackVisible);
		CommandSetFreehandStackVisible = new RelayCommand(SetFreehandStackVisible);
		CommandAddPartToGroup = new RelayCommand(AddToPartGroup);
		CommandBreakPartGroup = new RelayCommand<Guid>(BreakPartGroup);
		CommandRenameImportGroup = new RelayCommand<Guid>(RenameImportGroup);
		CommandAddPart = new RelayCommand(AddPart);
		CommandDeletePart = new RelayCommand<Guid>(DeleteGroupOrPart);
		CommandTogglePartVisibility = new RelayCommand(TogglePartVisibility);
		CommandSelectedImportGroup = new RelayCommand<Guid>(SelectedImportGroup);
		CommandDuplicateImportGroup = new RelayCommand(DuplicateSelected);
		CommandTogglePrimarySecondaryMetal = new RelayCommand(TogglePrimarySecondaryMetal);
		CommandClosePartToolbox = new RelayCommand(ClosePartToolbox);
		CommandShowHideSubMenu = new RelayCommand(ShowhideSubMenu);
		CommandShowRenamePrompt = new RelayCommand<Guid>(ShowRenamePrompt);
		CommandToggleCommonCenterPoint = new RelayCommand(ToggleCommonCenterPoint);
		CommandHighlightImportGroup = new RelayCommand<ImportGroup>(HighlightImportGroup);
		CommandToggleMultiSelectForPartsList = new RelayCommand(ToggleMultiSelectForPartsList);
		CommandResetPart = new RelayCommand<Guid>(ResetPart);
	}

	private void PartsCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
	{
		ViewModelLocator.Current.MainVM.ShowViewportControlHandleToggle = PartsCollection.Any();
	}

	private void ProcessSelectionLogic(object sender, EventArgs args)
	{
		_importGroupSelectionDispatcherTimer.Stop();
		UIHelper.CoreDataService.ImportGroupSelectByIdentifier((from _ in SelectedImportGroups
			where _.IsSelectedImportGroup
			select _.Identifier).FirstOrDefault());
		if (SelectedImportGroupIdentifiers.Count > 1)
		{
			UIHelper.CoreDataService.ImportGroupSelectSecondaryByIdentifier((from _ in SelectedImportGroups
				where _.IsSecondarySelectedImportGroup
				select _.Identifier).ToList());
		}
	}

	private void RenameImportGroup(Guid id)
	{
		ImportGroup importGroup = PartsCollection.FirstOrDefault((ImportGroup _) => _.Identifier == _partRenamingId);
		if (importGroup != null)
		{
			importGroup.Name = OldPartNamePlaceholderText;
			if (importGroup.Name == string.Empty)
			{
				UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.InvalidCharacterInName, messageIsModal: true);
				OldPartNamePlaceholderText = _lastPartNameUsed;
				return;
			}
			UIHelper.CoreDataService.ImportGroupRename(_partRenamingId, OldPartNamePlaceholderText);
			OldPartNamePlaceholderText = string.Empty;
			_partRenamingId = Guid.Empty;
			IsPartsListVisible = true;
			ViewModelLocator.Current.FreehandStackVM.UpdateCategoryControlName(importGroup);
		}
	}

	private void ResetPart(Guid importGroupIdentifier)
	{
		UIHelper.CoreDataService.ResetHandleForImportGroup(importGroupIdentifier);
	}

	private void SelectedImportGroup(Guid obj)
	{
		ImportGroup importGroupFromGuid = GetImportGroupFromGuid(obj);
		ImportGroup importGroup = SelectedImportGroups.FirstOrDefault((ImportGroup _) => _.IsSelectedImportGroup);
		IEnumerable<ImportGroup> source = SelectedImportGroups.Where((ImportGroup _) => _.IsSecondarySelectedImportGroup);
		if (importGroup != null)
		{
			if (importGroup.Identifier == obj)
			{
				importGroupFromGuid.IsSelectedImportGroup = false;
				ImportGroup importGroup2 = source.FirstOrDefault();
				if (importGroup2 != null)
				{
					importGroup2.IsSecondarySelectedImportGroup = false;
					importGroup2.IsSelectedImportGroup = true;
				}
			}
			else if (_isMultiSelect || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				if (source.Any((ImportGroup _) => _.Identifier == obj))
				{
					importGroupFromGuid.IsSelectedImportGroup = false;
					importGroupFromGuid.IsSecondarySelectedImportGroup = false;
				}
				else
				{
					importGroupFromGuid.IsSelectedImportGroup = false;
					importGroupFromGuid.IsSecondarySelectedImportGroup = true;
				}
			}
			else
			{
				importGroup.IsSelectedImportGroup = false;
				source.ForEach(delegate(ImportGroup _)
				{
					_.IsSecondarySelectedImportGroup = false;
				});
				importGroupFromGuid.IsSelectedImportGroup = true;
				importGroupFromGuid.IsSecondarySelectedImportGroup = false;
			}
		}
		else
		{
			importGroupFromGuid.IsSelectedImportGroup = true;
		}
		UpdateSelectionIndicatorAndDesignStackVisibility();
		UpdateViewportHandleSelectionFromUI();
		RaisePropertyChanged(() => IsMultiplePartsSelected);
	}

	private void SetDesignStackVisible()
	{
		if (!UIHelper.KeyProfile.IsDesignLibraryDisabled && UIHelper.LoadedDesign.DesignId != 0)
		{
			ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = UIHelper.LoadedDesign.DesignId != 0;
			ClearCurrentlySelectedImportGroups();
			UpdateViewportHandleSelectionFromUI();
		}
	}

	private void SetFreehandStackVisible()
	{
		ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = false;
	}

	private void SetImportGroupToPrimaryMetal(List<Guid> importGroupId)
	{
		UIHelper.CoreDataService.ImportGroupSetToPrimaryMetal(importGroupId);
		importGroupId.ForEach(delegate(Guid _)
		{
			GetImportGroupFromGuid(_).IsPrimaryMetal = true;
		});
	}

	private void SetImportGroupToSecondaryMetal(List<Guid> importGroupId)
	{
		UIHelper.CoreDataService.ImportGroupSetToSecondaryMetal(importGroupId);
		importGroupId.ForEach(delegate(Guid _)
		{
			GetImportGroupFromGuid(_).IsPrimaryMetal = false;
		});
	}

	private void SetImportGroupVisibilityByIdentifier(ImportGroup importGroup)
	{
		importGroup.IsViewportVisible = !importGroup.IsViewportVisible;
		UIHelper.CoreDataService.ImportGroupSetVisibilityByIdentifier(new List<Guid> { importGroup.Identifier }, importGroup.IsViewportVisible);
	}

	private void SetRhinoHighlight(object sender, EventArgs e)
	{
		List<Guid> instanceGuids = ((_importGroupToHighlight != null) ? _importGroupToHighlight.BlockIdentifiers : new List<Guid>());
		if (_importGroupToHighlight != null && _importGroupToHighlight.IsViewportVisible)
		{
			UIHelper.CoreDataService.ViewportSetHighlightColor(System.Drawing.Color.Firebrick);
			UIHelper.CoreDataService.HighlightItemsByBlockInstanceGuid(instanceGuids);
		}
		else
		{
			UIHelper.CoreDataService.HighlightItemsByBlockInstanceGuid(new List<Guid>());
		}
		_importGroupHighlightDispatcherTimer.Stop();
		_importGroupToHighlight = null;
	}

	private void ShowhideSubMenu()
	{
		IsSubMenuVisible = !IsSubMenuVisible;
	}

	private void ShowRenamePrompt(Guid id)
	{
		ImportGroup importGroup = PartsCollection.FirstOrDefault((ImportGroup _) => _.Identifier == id);
		if (importGroup != null)
		{
			_lastPartNameUsed = importGroup.Name;
			OldPartNamePlaceholderText = importGroup.Name;
			IsPartsListVisible = false;
			_partRenamingId = importGroup.Identifier;
		}
		else
		{
			IsPartsListVisible = true;
			OldPartNamePlaceholderText = string.Empty;
		}
	}

	private void ToggleCommonCenterPoint()
	{
		if (ViewModelLocator.Current.MainVM.IsCommonCenterPointToggled)
		{
			ViewModelLocator.Current.MainVM.IsCommonCenterPointToggled = false;
		}
		else
		{
			ViewModelLocator.Current.MainVM.IsCommonCenterPointToggled = true;
		}
	}

	private void ToggleMultiSelectForPartsList()
	{
		IsPartsListMultiSelect = !IsPartsListMultiSelect;
	}

	private void TogglePartVisibility()
	{
		if (CheckSelectedImportGroupsAndPrompt())
		{
			SelectedImportGroups.ForEach(SetImportGroupVisibilityByIdentifier);
		}
	}

	private void TogglePrimarySecondaryMetal()
	{
		List<Guid> importGroupToSecondaryMetal = (from s in SelectedImportGroups
			where s.IsPrimaryMetal
			select s.Identifier).ToList();
		List<Guid> importGroupToPrimaryMetal = (from s in SelectedImportGroups
			where !s.IsPrimaryMetal
			select s.Identifier).ToList();
		SetImportGroupToPrimaryMetal(importGroupToPrimaryMetal);
		SetImportGroupToSecondaryMetal(importGroupToSecondaryMetal);
	}

	private void UpdateSelectionIndicatorAndDesignStackVisibility()
	{
		ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = !SelectedImportGroupIdentifiers.Any();
		if (PartsCollection.Any((ImportGroup _) => _.HasSelectionChanged))
		{
			PartsCollection.Where((ImportGroup _) => _.HasSelectionChanged).ForEach(delegate(ImportGroup _)
			{
				_.HasSelectionChanged = false;
			});
		}
		ViewModelLocator.Current.FreehandStackVM.UpdateSelectedFreehandPartControls();
		RaisePropertyChanged("SelectedImportGroupName");
	}

	private void UpdateViewportHandleSelectionFromUI()
	{
		_importGroupHighlightDispatcherTimer?.Stop();
		_importGroupSelectionDispatcherTimer?.Stop();
		_importGroupSelectionDispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 250), DispatcherPriority.Background, ProcessSelectionLogic, System.Windows.Application.Current.Dispatcher);
		_importGroupSelectionDispatcherTimer.Start();
	}
}
