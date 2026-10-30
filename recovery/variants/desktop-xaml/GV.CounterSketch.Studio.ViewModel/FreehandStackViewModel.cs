using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio.ViewModel;

public class FreehandStackViewModel : ViewModelBase
{
	private static List<CSValueXFormObjectControl> _xFormObjectControls;

	private readonly List<CSCategoryControl> _categoryControlCache = new List<CSCategoryControl>();

	private ObservableCollection<DesignComponentDisplay> _designComponents;

	private ObservableCollection<DesignStackItemContainer> _freehandStackItemContainers;

	private bool _isDesignComponentsSelectorVisible;

	private bool _isFreehandCheckBoxActive;

	private List<Guid> _secondarySelectedGuids;

	private UIElement _xFormObjectControl;

	private bool _hasFreehandParts;

	private Collection<UIControl> _expandedControls;

	private bool _hasExpandedSliders;

	public bool HasExpandedSliders
	{
		get
		{
			if (UIHelper.CounterSketchOptions.UserTypeId < 8)
			{
				return false;
			}
			return _hasExpandedSliders;
		}
		set
		{
			_hasExpandedSliders = value;
			RaisePropertyChanged("HasExpandedSliders");
		}
	}

	public Collection<UIControl> ExpandedControls
	{
		get
		{
			return _expandedControls;
		}
		set
		{
			_expandedControls = value;
			RaisePropertyChanged("ExpandedControls");
			HasExpandedSliders = value.Any();
		}
	}

	public RelayCommand<UIControl> CommandResetUIControl { get; set; }

	public RelayCommand<ImportGroup> CommandDeleteFreehandControl { get; protected set; }

	public RelayCommand<ImportGroup> CommandDuplicateImportGroup { get; set; }

	public RelayCommand CommandDuplicateSelectedImportGroup { get; set; }

	public RelayCommand CommandSaveSortedImportGroups { get; set; }

	public RelayCommand CommandOpenPartsToolBoxWindow { get; private set; }

	public RelayCommand<Guid> CommandSelectImportedGroupByIdentifier { get; set; }

	public RelayCommand<ImportGroup> CommandSetImportGroupToPrimaryMetal { get; set; }

	public RelayCommand<ImportGroup> CommandSetImportGroupToSecondaryMetal { get; set; }

	public RelayCommand<ImportGroup> CommandSetImportGroupVisibilityByIdentifier { get; set; }

	public RelayCommand<DesignComponentDisplay> CommandUpdateVisibleDisplayComponents { get; protected set; }

	public ObservableCollection<DesignComponentDisplay> DesignComponents
	{
		get
		{
			return _designComponents ?? new ObservableCollection<DesignComponentDisplay>();
		}
		set
		{
			_designComponents = value;
			RaisePropertyChanged(() => DesignComponents);
		}
	}

	public ObservableCollection<DesignStackItemContainer> FreehandStackItemContainers
	{
		get
		{
			if (_freehandStackItemContainers == null)
			{
				_freehandStackItemContainers = new ObservableCollection<DesignStackItemContainer>();
			}
			return _freehandStackItemContainers;
		}
		set
		{
			_freehandStackItemContainers = value;
			RaisePropertyChanged(() => FreehandStackItemContainers);
		}
	}

	public bool IsDesignComponentsSelectorVisible
	{
		get
		{
			return _isDesignComponentsSelectorVisible;
		}
		set
		{
			_isDesignComponentsSelectorVisible = value;
			RaisePropertyChanged(() => IsDesignComponentsSelectorVisible);
		}
	}

	public UIElement XFormObjectControl
	{
		get
		{
			return _xFormObjectControl;
		}
		set
		{
			_xFormObjectControl = value;
			RaisePropertyChanged(() => XFormObjectControl);
		}
	}

	public static List<CSValueXFormObjectControl> XFormObjectControls
	{
		get
		{
			if (_xFormObjectControls == null)
			{
				_xFormObjectControls = new List<CSValueXFormObjectControl>();
			}
			return _xFormObjectControls;
		}
		set
		{
			_xFormObjectControls = value;
		}
	}

	public RelayCommand<SelectionChangedEventArgs> CommandFlipChanged { get; set; }

	public RelayCommand<SelectionChangedEventArgs> CommandMirrorChanged { get; set; }

	public bool IsFreehandCheckBoxActive
	{
		get
		{
			return _isFreehandCheckBoxActive;
		}
		set
		{
			_isFreehandCheckBoxActive = value;
			UIHelper.CoreDataService.ToggleFreehandVisibility(_isFreehandCheckBoxActive);
			RaisePropertyChanged("IsFreehandCheckBoxActive");
		}
	}

	public List<Guid> SecondarySelectedGuids
	{
		get
		{
			return _secondarySelectedGuids;
		}
		set
		{
			_secondarySelectedGuids = value;
			RaisePropertyChanged(() => SecondarySelectedGuids);
		}
	}

	public bool HasFreehandParts
	{
		get
		{
			return _hasFreehandParts;
		}
		set
		{
			_hasFreehandParts = value;
			RaisePropertyChanged("HasFreehandParts");
		}
	}

	public FreehandStackViewModel()
	{
		if (!base.IsInDesignMode)
		{
			InitializeCommands();
			IsFreehandCheckBoxActive = true;
		}
	}

	public void ResetUIControl(UIControl uIControl)
	{
		CSUIControl cSUIControl = FreehandStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls).SelectMany((CSCategoryControl ui) => from CSUIControl _ in ui.Children
			select (_)).ToList()
			.OfType<CSUIControl>()
			.FirstOrDefault((CSUIControl csuiControl) => csuiControl != null && csuiControl.LocalUIControl.EnglishControlName == uIControl.EnglishControlName && csuiControl.LocalUIControl.BlockInstanceGuid == uIControl.BlockInstanceGuid);
		if (cSUIControl != null && cSUIControl is CSValueRangeControl cSValueRangeControl)
		{
			cSValueRangeControl.Reset();
		}
	}

	public void ClearComponentContainersAndCategories()
	{
		FreehandStackItemContainers?.SelectMany((DesignStackItemContainer container) => container.CategoryControls).ToList().ForEach(delegate(CSCategoryControl _)
		{
			_.Visibility = Visibility.Collapsed;
		});
	}

	public void RemoveImportedControlsFromCachedControlCollection(Guid identifier)
	{
		if (ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection == null || FreehandStackItemContainers == null)
		{
			return;
		}
		(from category in FreehandStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls)
			where category.ImportGroupIdentifier == identifier
			select category).ToList().ForEach(delegate(CSCategoryControl categoryControl)
		{
			FreehandStackItemContainers.Where((DesignStackItemContainer container) => container.CategoryControls.Contains(categoryControl)).ToList().ForEach(delegate(DesignStackItemContainer container)
			{
				container.CategoryControls.Remove(categoryControl);
			});
		});
		CSValueXFormObjectControl cSValueXFormObjectControl = XFormObjectControls.FirstOrDefault((CSValueXFormObjectControl control) => control.LocalUIControl.ImportGroupIdentifier == identifier);
		if (cSValueXFormObjectControl != null)
		{
			if (XFormObjectControl is CSValueXFormObjectControl cSValueXFormObjectControl2 && cSValueXFormObjectControl2.LocalUIControl.BlockInstanceGuid == cSValueXFormObjectControl.LocalUIControl.BlockInstanceGuid)
			{
				XFormObjectControl = null;
			}
			XFormObjectControls.Remove(cSValueXFormObjectControl);
		}
	}

	public void Reset()
	{
		FreehandStackItemContainers?.Clear();
		_categoryControlCache?.ForEach(delegate(CSCategoryControl _)
		{
			_.TitleClicked -= SectionControlTitleClicked;
		});
		_categoryControlCache?.Clear();
		XFormObjectControls?.Clear();
		XFormObjectControl = null;
	}

	public void UpdateCategoryControlName(ImportGroup part)
	{
		CSCategoryControl cSCategoryControl = _categoryControlCache.FirstOrDefault((CSCategoryControl csCategoryControl) => csCategoryControl.ImportGroupIdentifier == part.Identifier && !string.IsNullOrWhiteSpace(csCategoryControl.ImportGroupName));
		if (cSCategoryControl != null)
		{
			cSCategoryControl.ImportGroupName = part.Name;
		}
	}

	public void UpdateFreehandStackControls(IEnumerable<UIControl> uiControls, Exception exception)
	{
		if (exception != null)
		{
			throw exception;
		}
		ClearComponentContainersAndCategories();
		if (uiControls != null)
		{
			uiControls = uiControls.Where((UIControl control) => control.ImportGroupIdentifier != Guid.Empty);
			HasFreehandParts = uiControls.Any();
			ExpandedControls = new Collection<UIControl>(uiControls.Where((UIControl _) => _.HasMissingObject).ToList());
			foreach (UIControl uiControl in uiControls)
			{
				uiControl.ControlName = TranslationHelper.GetTranslatedDesignOrPartString(uiControl.EnglishControlName);
				uiControl.CategoryName = TranslationHelper.GetTranslatedDesignOrPartString(uiControl.CategoryName);
				if (uiControl.CurrentUIControlType == UIControlType.CSValueXFormObject)
				{
					CSValueXFormObjectControl cSValueXFormObjectControl = XFormObjectControls.FirstOrDefault((CSValueXFormObjectControl control) => control.LocalUIControl.BlockInstanceGuid == uiControl.BlockInstanceGuid);
					if (cSValueXFormObjectControl == null)
					{
						cSValueXFormObjectControl = CSCategoryControl.CreateControlFromUIControlObject(uiControl) as CSValueXFormObjectControl;
						XFormObjectControls.Add(cSValueXFormObjectControl);
					}
					else
					{
						cSValueXFormObjectControl.LocalUIControl = uiControl;
					}
					XFormObjectControl = cSValueXFormObjectControl;
					continue;
				}
				CSCategoryControl csCategoryControl = _categoryControlCache.FirstOrDefault((CSCategoryControl category) => category.EnglishCategoryName == uiControl.EnglishCategoryName && category.ImportGroupIdentifier == uiControl.ImportGroupIdentifier);
				if (csCategoryControl == null)
				{
					csCategoryControl = new CSCategoryControl
					{
						CategoryName = uiControl.CategoryName,
						EnglishCategoryName = uiControl.EnglishCategoryName,
						ImportGroupIdentifier = uiControl.ImportGroupIdentifier,
						ImportGroupPartGroupId = uiControl.ImportGroupPartGroupId,
						Title = uiControl.CategoryName,
						DeleteVisibility = Visibility.Collapsed
					};
					_categoryControlCache.Add(csCategoryControl);
					csCategoryControl.TitleClicked += SectionControlTitleClicked;
				}
				else
				{
					csCategoryControl.ImportGroupPartGroupId = uiControl.ImportGroupPartGroupId;
				}
				if (uiControl.CurrentUIControlStatus == UIControlStatus.Changed)
				{
					csCategoryControl.UpdateCSUIControlSource(uiControl);
					csCategoryControl.Sort();
				}
				ComponentsEnum assignedComponent = uiControl.AssignedDisplayComponents.FirstOrDefault();
				if (assignedComponent != ComponentsEnum.None)
				{
					DesignStackItemContainer designStackItemContainer = FreehandStackItemContainers.FirstOrDefault((DesignStackItemContainer container) => container.AssociatedComponent == assignedComponent);
					if (designStackItemContainer == null)
					{
						designStackItemContainer = new DesignStackItemContainer
						{
							IsVisible = true,
							AssociatedComponent = assignedComponent,
							CategoryControls = new ObservableCollection<CSCategoryControl>()
						};
						designStackItemContainer.ContainerName = designStackItemContainer.ContainerName.ToUpper();
						FreehandStackItemContainers.Add(designStackItemContainer);
					}
					string importGroupName = uiControl.ImportGroupName ?? string.Empty;
					if (designStackItemContainer.CategoryControls.Where((CSCategoryControl _) => _.ImportGroupIdentifier != Guid.Empty).All((CSCategoryControl _) => _.ImportGroupIdentifier != uiControl.ImportGroupIdentifier))
					{
						csCategoryControl.ImportGroupName = importGroupName;
					}
					if (designStackItemContainer.CategoryControls.All((CSCategoryControl _) => _.ImportGroupIdentifier != csCategoryControl.ImportGroupIdentifier))
					{
						designStackItemContainer.CategoryControls.Add(csCategoryControl);
					}
					else if (designStackItemContainer.CategoryControls.Where((CSCategoryControl _) => _.ImportGroupIdentifier == csCategoryControl.ImportGroupIdentifier).All((CSCategoryControl _) => _.EnglishCategoryName != csCategoryControl.EnglishCategoryName))
					{
						designStackItemContainer.CategoryControls.Add(csCategoryControl);
					}
					csCategoryControl.Visibility = Visibility.Visible;
				}
			}
		}
		ViewModelLocator.Current.MainVM.SetIsRhinoBusy(isBusy: false);
	}

	public void UpdateSelectedFreehandPartControls()
	{
		if (FreehandStackItemContainers == null)
		{
			return;
		}
		FreehandStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls).ForEach(delegate(CSCategoryControl categoryControl)
		{
			if (ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection != null)
			{
				categoryControl.Visibility = ((!CustomizePartsListViewModel.SelectedImportGroupIdentifiers.Contains(categoryControl.ImportGroupIdentifier) && !CustomizePartsListViewModel.SelectedImportGroupIdentifiers.Contains(categoryControl.ImportGroupPartGroupId)) ? Visibility.Collapsed : Visibility.Visible);
			}
		});
		if (ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection == null)
		{
			return;
		}
		CSValueXFormObjectControl cSValueXFormObjectControl = XFormObjectControls.FirstOrDefault((CSValueXFormObjectControl control) => ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection.Any((ImportGroup importGroup) => importGroup.Identifier == control.LocalUIControl.ImportGroupIdentifier && importGroup.IsSelected));
		if (cSValueXFormObjectControl != null)
		{
			XFormObjectControl = cSValueXFormObjectControl;
		}
	}

	private void CollapseAllButOne(object expandedControl = null)
	{
		foreach (CSCategoryControl item in from categoryControl in FreehandStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls)
			where categoryControl != null && !object.Equals(categoryControl, expandedControl)
			select categoryControl)
		{
			item.Collapsed = true;
		}
	}

	private void InitializeCommands()
	{
		CommandOpenPartsToolBoxWindow = new RelayCommand(OpenPartsToolBoxWindow);
		CommandUpdateVisibleDisplayComponents = new RelayCommand<DesignComponentDisplay>(UpdateVisibleDisplayComponents);
		CommandSaveSortedImportGroups = new RelayCommand(SaveSortedImportGroups);
		CommandFlipChanged = new RelayCommand<SelectionChangedEventArgs>(UpdateGraphPartFlip);
		CommandMirrorChanged = new RelayCommand<SelectionChangedEventArgs>(UpdateGraphPartMirror);
		CommandResetUIControl = new RelayCommand<UIControl>(ResetUIControl);
	}

	private void OpenPartsToolBoxWindow()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = "Compatible CounterPart Files (*.csg)|*.csg",
			Multiselect = false,
			Title = Strings.Browse
		};
		if (openFileDialog.ShowDialog() == true && openFileDialog.FileNames[0] != null)
		{
			string text = openFileDialog.FileNames[0];
			if (!string.IsNullOrWhiteSpace(text))
			{
				UIHelper.CoreDataService.DocumentImportCSGPart(text);
			}
		}
	}

	private void SaveSortedImportGroups()
	{
	}

	private void SectionControlTitleClicked(object sender, RoutedEventArgs e)
	{
		CollapseAllButOne(sender);
	}

	private void SetHandEnabled(bool isHandEnabled)
	{
		if (!isHandEnabled)
		{
			ViewModelLocator.Current.WindowDesignTryOnVM.TurnHandOff();
		}
		ViewModelLocator.Current.WindowDesignTryOnVM.IsHandEnabled = isHandEnabled;
	}

	private static void UpdateGraphPartFlip(object parameters)
	{
		SelectionChangedEventArgs selectionChangedParameters = parameters as SelectionChangedEventArgs;
		if (selectionChangedParameters != null)
		{
			XFormObjectControls.Where((CSValueXFormObjectControl _) => CustomizePartsListViewModel.SelectedImportGroupIdentifiers.Contains(_.LocalUIControl.ImportGroupIdentifier) || CustomizePartsListViewModel.SelectedImportGroupIdentifiers.Contains(_.LocalUIControl.ImportGroupPartGroupId)).ForEach(delegate(CSValueXFormObjectControl _)
			{
				_.FlipSelector_OnSelectionChanged(null, selectionChangedParameters);
			});
		}
	}

	private static void UpdateGraphPartMirror(object parameters)
	{
		SelectionChangedEventArgs selectionChangedParameters = parameters as SelectionChangedEventArgs;
		if (selectionChangedParameters != null)
		{
			XFormObjectControls.Where((CSValueXFormObjectControl _) => CustomizePartsListViewModel.SelectedImportGroupIdentifiers.Contains(_.LocalUIControl.ImportGroupIdentifier) || CustomizePartsListViewModel.SelectedImportGroupIdentifiers.Contains(_.LocalUIControl.ImportGroupPartGroupId)).ForEach(delegate(CSValueXFormObjectControl _)
			{
				_.MirrorSelector_OnSelectionChanged(null, selectionChangedParameters);
			});
		}
	}

	private void UpdateVisibleDisplayComponents(DesignComponentDisplay designComponentDisplay)
	{
		Enum.GetNames(typeof(ComponentsEnum)).ToList().ForEach(delegate(string componentName)
		{
			if (Enum.TryParse<ComponentsEnum>(componentName, ignoreCase: true, out var result) && designComponentDisplay.Name == componentName)
			{
				SetHandEnabled(componentName != "Pendant");
				DesignComponentDisplay designComponentDisplay2 = DesignComponents.FirstOrDefault((DesignComponentDisplay component) => component.Name == componentName);
				if (designComponentDisplay2 != null)
				{
					designComponentDisplay2.IsEnabled = !designComponentDisplay2.IsEnabled;
					UIHelper.CoreDataService.SetComponentVisibility(result, designComponentDisplay2.IsEnabled);
				}
			}
		});
		ViewModelLocator.Current.DesignStackVM.UpdateDesignStackCategoryFromComponents();
	}
}
