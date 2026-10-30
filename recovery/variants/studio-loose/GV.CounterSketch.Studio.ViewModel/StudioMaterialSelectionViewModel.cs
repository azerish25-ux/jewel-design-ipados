using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class StudioMaterialSelectionViewModel : ViewModelBase
{
	private ObservableCollection<MaterialOptionGroup> _materialGroupOptions;

	private MaterialOptionItem _metalPrimaryMaterial;

	private MaterialOptionItem _metalSecondaryMaterial;

	public RelayCommand<MaterialOptionGroup> CommandCloseAllOtherMaterialGroupOptions { get; set; }

	public MaterialOptionItem GemAccentMaterial { get; set; }

	public MaterialOptionItem GemCenterMaterial { get; set; }

	public MaterialOptionItem GemSideMaterial { get; set; }

	public ObservableCollection<MaterialOptionGroup> MaterialGroupOptions
	{
		get
		{
			return _materialGroupOptions;
		}
		set
		{
			if (_materialGroupOptions != value)
			{
				_materialGroupOptions = value;
				((ObservableObject)this).RaisePropertyChanged<ObservableCollection<MaterialOptionGroup>>((Expression<Func<ObservableCollection<MaterialOptionGroup>>>)(() => MaterialGroupOptions));
			}
		}
	}

	public MaterialOptionItem MetalPrimaryMaterial
	{
		get
		{
			return _metalPrimaryMaterial;
		}
		set
		{
			_metalPrimaryMaterial = value;
			((ObservableObject)this).RaisePropertyChanged("MetalPrimaryMaterial");
		}
	}

	public MaterialOptionItem MetalSecondaryMaterial
	{
		get
		{
			return _metalSecondaryMaterial;
		}
		set
		{
			_metalSecondaryMaterial = value;
			((ObservableObject)this).RaisePropertyChanged("MetalSecondaryMaterial");
		}
	}

	public StudioMaterialSelectionViewModel()
	{
		MaterialGroupOptions = GenerateMaterialSearchFilters();
		DefaultMaterialGroupOptions();
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitializeCommands();
		}
	}

	public void CloseAllMaterialPickers()
	{
		MaterialGroupOptions.ForEach(delegate(MaterialOptionGroup materialGroup)
		{
			if (materialGroup.MaterialOptionSelectorVM.IsPopupOpen)
			{
				materialGroup.MaterialOptionSelectorVM.IsPopupOpen = false;
			}
		});
	}

	public void DefaultMaterialGroupOptions()
	{
		MaterialGroupOptions.ForEach(delegate(MaterialOptionGroup searchFilter)
		{
			if (searchFilter.MaterialOptionSelectorVM.CurrentAssignmentMode != MaterialAssignmentMode.MetalPrimary && searchFilter.MaterialOptionSelectorVM.CurrentAssignmentMode != MaterialAssignmentMode.MetalSecondary)
			{
				searchFilter.MaterialOptionSelectorVM.SelectedGemColorType = GemColorType.White;
			}
		});
		MaterialGroupOptions.ForEach(delegate(MaterialOptionGroup _)
		{
			switch (_.Title)
			{
			case "Primary Metal":
			{
				MaterialOptionItem materialOptionItem5 = _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem z) => UIHelper.CounterSketchOptions.PrimaryDefaultMaterialName == z.PrimaryText) ?? _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault();
				_.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(materialOptionItem5);
				if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.PrimaryDefaultMaterialName) && materialOptionItem5 != null)
				{
					UIHelper.CounterSketchOptions.PrimaryDefaultMaterialName = materialOptionItem5.PrimaryText;
				}
				break;
			}
			case "Secondary Metal":
			{
				MaterialOptionItem materialOptionItem6 = _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem z) => UIHelper.CounterSketchOptions.SecondaryDefaultMaterialName == z.PrimaryText) ?? _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault();
				_.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(materialOptionItem6);
				if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.SecondaryDefaultMaterialName) && materialOptionItem6 != null)
				{
					UIHelper.CounterSketchOptions.SecondaryDefaultMaterialName = materialOptionItem6.PrimaryText;
				}
				break;
			}
			case "Center Stones":
			{
				MaterialOptionItem materialOptionItem4 = _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem z) => UIHelper.CounterSketchOptions.CenterGemDefaultMaterialName == z.PrimaryText) ?? _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault();
				_.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(materialOptionItem4);
				if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.CenterGemDefaultMaterialName) && materialOptionItem4 != null)
				{
					UIHelper.CounterSketchOptions.CenterGemDefaultMaterialName = materialOptionItem4.PrimaryText;
				}
				break;
			}
			case "Side Stones":
			{
				MaterialOptionItem materialOptionItem7 = _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem z) => UIHelper.CounterSketchOptions.SideGemDefaultMaterialName == z.PrimaryText) ?? _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault();
				_.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(materialOptionItem7);
				if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.SideGemDefaultMaterialName) && materialOptionItem7 != null)
				{
					UIHelper.CounterSketchOptions.SideGemDefaultMaterialName = materialOptionItem7.PrimaryText;
				}
				break;
			}
			case "Accent Stones":
			{
				MaterialOptionItem materialOptionItem3 = _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem z) => UIHelper.CounterSketchOptions.AccentGemDefaultMaterialName == z.PrimaryText) ?? _.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault();
				_.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(materialOptionItem3);
				if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.AccentGemDefaultMaterialName) && materialOptionItem3 != null)
				{
					UIHelper.CounterSketchOptions.AccentGemDefaultMaterialName = materialOptionItem3.PrimaryText;
				}
				break;
			}
			default:
				_.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(_.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault());
				break;
			}
		});
		if (UIHelper.KeyProfile.KeyProfileId == 2)
		{
			MaterialOptionGroup materialOptionGroup = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup _) => _.Title == Strings.PrimaryMetal);
			if (materialOptionGroup != null)
			{
				MaterialOptionItem materialOptionItem = materialOptionGroup.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem _) => _.MetalMaterial == MetalMaterial.WhiteGold);
				if (materialOptionItem != null)
				{
					materialOptionGroup.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(materialOptionItem);
				}
			}
			MaterialOptionGroup materialOptionGroup2 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup _) => _.Title == Strings.SecondaryMetal);
			if (materialOptionGroup2 != null)
			{
				MaterialOptionItem materialOptionItem2 = materialOptionGroup2.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem _) => _.MetalMaterial == MetalMaterial.WhiteGold);
				if (materialOptionItem2 != null)
				{
					materialOptionGroup2.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(materialOptionItem2);
				}
			}
		}
		UpdateSelectedMaterialViewModelProperties();
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			if (MetalPrimaryMaterial != null && MetalSecondaryMaterial != null)
			{
				UIHelper.CoreDataService.MaterialsSetMetal(new MetalMaterial[2]
				{
					(MetalMaterial)MetalPrimaryMaterial.SearchAttributeId,
					(MetalMaterial)MetalSecondaryMaterial.SearchAttributeId
				});
			}
			if (GemCenterMaterial != null && GemSideMaterial != null && GemAccentMaterial != null)
			{
				UIHelper.CoreDataService.MaterialsSetGem(new GemMaterial[3]
				{
					(GemMaterial)GemCenterMaterial.SearchAttributeId,
					(GemMaterial)GemSideMaterial.SearchAttributeId,
					(GemMaterial)GemAccentMaterial.SearchAttributeId
				});
			}
		}
	}

	public void ReloadMaterialsAfterAccountChanged()
	{
		string gemMaterialCenter = GemCenterMaterial.PrimaryText;
		string gemMaterialSide = GemSideMaterial.PrimaryText;
		string gemMaterialAccent = GemAccentMaterial.PrimaryText;
		string metalMaterialPrimary = MetalPrimaryMaterial.PrimaryText;
		string metalMaterialSecondary = MetalSecondaryMaterial.PrimaryText;
		MaterialGroupOptions = GenerateMaterialSearchFilters();
		MaterialGroupOptions.Where((MaterialOptionGroup _) => _.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.GemCenter).ForEach(delegate(MaterialOptionGroup searchFilter)
		{
			MaterialOptionItem materialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText == gemMaterialCenter);
			if (materialOptionItem != null)
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = materialOptionItem;
			}
			else
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.First();
			}
		});
		MaterialGroupOptions.Where((MaterialOptionGroup _) => _.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.GemSide).ForEach(delegate(MaterialOptionGroup searchFilter)
		{
			MaterialOptionItem materialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText == gemMaterialSide);
			if (materialOptionItem != null)
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = materialOptionItem;
			}
			else
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.First();
			}
		});
		MaterialGroupOptions.Where((MaterialOptionGroup _) => _.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.GemAccent).ForEach(delegate(MaterialOptionGroup searchFilter)
		{
			MaterialOptionItem materialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText == gemMaterialAccent);
			if (materialOptionItem != null)
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = materialOptionItem;
			}
			else
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.First();
			}
		});
		MaterialGroupOptions.Where((MaterialOptionGroup _) => _.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.MetalPrimary).ForEach(delegate(MaterialOptionGroup searchFilter)
		{
			MaterialOptionItem materialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText == metalMaterialPrimary);
			if (materialOptionItem != null)
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = materialOptionItem;
			}
			else
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.First();
			}
		});
		MaterialGroupOptions.Where((MaterialOptionGroup _) => _.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.MetalSecondary).ForEach(delegate(MaterialOptionGroup searchFilter)
		{
			MaterialOptionItem materialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText == metalMaterialSecondary);
			if (materialOptionItem != null)
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = materialOptionItem;
			}
			else
			{
				searchFilter.MaterialOptionSelectorVM.SelectedMaterialOptionItem = searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.First();
			}
		});
		UpdateSelectedMaterialViewModelProperties();
		if (MetalPrimaryMaterial != null && MetalSecondaryMaterial != null)
		{
			UIHelper.CoreDataService.MaterialsSetMetal(new MetalMaterial[2]
			{
				(MetalMaterial)MetalPrimaryMaterial.SearchAttributeId,
				(MetalMaterial)MetalSecondaryMaterial.SearchAttributeId
			});
		}
		if (GemCenterMaterial != null && GemSideMaterial != null && GemAccentMaterial != null)
		{
			UIHelper.CoreDataService.MaterialsSetGem(new GemMaterial[3]
			{
				(GemMaterial)GemCenterMaterial.SearchAttributeId,
				(GemMaterial)GemSideMaterial.SearchAttributeId,
				(GemMaterial)GemAccentMaterial.SearchAttributeId
			});
		}
	}

	public void ReloadMaterialSearchFilters()
	{
		MaterialGroupOptions = GenerateMaterialSearchFilters();
		DefaultMaterialGroupOptions();
	}

	public void ResetCenterGemToDiamondMetarial()
	{
		MaterialGroupOptions.Where((MaterialOptionGroup searchFilter) => searchFilter.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.GemCenter).ForEach(delegate(MaterialOptionGroup searchFilter)
		{
			searchFilter.MaterialOptionSelectorVM.SelectedGemColorType = GemColorType.White;
			searchFilter.MaterialOptionSelectorVM.InitializeSelectedMaterialOptionItem(searchFilter.MaterialOptionSelectorVM.AvailableMaterialOptions.FirstOrDefault());
		});
		UpdateSelectedMaterialViewModelProperties();
		if (!ViewModelBase.IsInDesignModeStatic && GemCenterMaterial != null && GemSideMaterial != null && GemAccentMaterial != null)
		{
			UIHelper.CoreDataService.MaterialsSetGem(new GemMaterial[3]
			{
				(GemMaterial)GemCenterMaterial.SearchAttributeId,
				(GemMaterial)GemSideMaterial.SearchAttributeId,
				(GemMaterial)GemAccentMaterial.SearchAttributeId
			});
		}
	}

	public void ResetCenterGemToSpecifiedMetarial()
	{
		if (!ViewModelBase.IsInDesignModeStatic && GemCenterMaterial != null && GemSideMaterial != null && GemAccentMaterial != null)
		{
			UIHelper.CoreDataService.MaterialsSetGem(new GemMaterial[3]
			{
				(GemMaterial)GemCenterMaterial.SearchAttributeId,
				(GemMaterial)GemSideMaterial.SearchAttributeId,
				(GemMaterial)GemAccentMaterial.SearchAttributeId
			});
		}
	}

	public void SetMaterialsFromDesign()
	{
		UIHelper.CoreDataService.MaterialsGetGem(delegate(GemMaterial[] gemMaterials, Exception exception)
		{
			if (exception != null)
			{
				throw exception;
			}
			MaterialOptionGroup materialOptionGroup = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialOptionGroup4) => materialOptionGroup4.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.GemCenter);
			if (materialOptionGroup != null)
			{
				materialOptionGroup.MaterialOptionSelectorVM.SelectedMaterialOptionItem = GemHelper.GetGemMaterialOptionItems().FirstOrDefault((MaterialOptionItem gemmaterialOption) => gemmaterialOption.SearchAttributeId == (int)gemMaterials[0]);
			}
			MaterialOptionGroup materialOptionGroup2 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialOptionGroup4) => materialOptionGroup4.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.GemSide);
			if (materialOptionGroup2 != null)
			{
				materialOptionGroup2.MaterialOptionSelectorVM.SelectedMaterialOptionItem = GemHelper.GetGemMaterialOptionItems().FirstOrDefault((MaterialOptionItem gemmaterialOption) => gemmaterialOption.SearchAttributeId == (int)gemMaterials[1]);
			}
			MaterialOptionGroup materialOptionGroup3 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialOptionGroup4) => materialOptionGroup4.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.GemAccent);
			if (materialOptionGroup3 != null)
			{
				materialOptionGroup3.MaterialOptionSelectorVM.SelectedMaterialOptionItem = GemHelper.GetGemMaterialOptionItems().FirstOrDefault((MaterialOptionItem gemmaterialOption) => gemmaterialOption.SearchAttributeId == (int)gemMaterials[2]);
			}
		});
		UIHelper.CoreDataService.MaterialsGetMetal(delegate(MetalMaterial[] metalMaterials, Exception exception)
		{
			if (exception != null)
			{
				throw exception;
			}
			MaterialOptionGroup materialOptionGroup = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialOptionGroup3) => materialOptionGroup3.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.MetalPrimary);
			if (materialOptionGroup != null)
			{
				materialOptionGroup.MaterialOptionSelectorVM.SelectedMaterialOptionItem = MetalHelper.GetMetalMaterialOptionItems().FirstOrDefault((MaterialOptionItem gemmaterialOption) => gemmaterialOption.SearchAttributeId == (int)metalMaterials[0]);
			}
			MaterialOptionGroup materialOptionGroup2 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialOptionGroup3) => materialOptionGroup3.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.MetalSecondary);
			if (materialOptionGroup2 != null)
			{
				materialOptionGroup2.MaterialOptionSelectorVM.SelectedMaterialOptionItem = MetalHelper.GetMetalMaterialOptionItems().FirstOrDefault((MaterialOptionItem gemmaterialOption) => gemmaterialOption.SearchAttributeId == (int)metalMaterials[1]);
			}
		});
		UpdateSelectedMaterialViewModelProperties();
	}

	public void SetMetal(bool isPrimary, int searchAttributeId)
	{
		if (isPrimary)
		{
			MaterialOptionGroup materialOptionGroup = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialOptionGroup3) => materialOptionGroup3.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.MetalPrimary);
			if (materialOptionGroup != null)
			{
				materialOptionGroup.MaterialOptionSelectorVM.SelectedMaterialOptionItem = MetalHelper.GetMetalMaterialOptionItems().FirstOrDefault((MaterialOptionItem gemmaterialOption) => gemmaterialOption.SearchAttributeId == searchAttributeId);
			}
		}
		else
		{
			MaterialOptionGroup materialOptionGroup2 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialOptionGroup3) => materialOptionGroup3.MaterialOptionSelectorVM.CurrentAssignmentMode == MaterialAssignmentMode.MetalSecondary);
			if (materialOptionGroup2 != null)
			{
				materialOptionGroup2.MaterialOptionSelectorVM.SelectedMaterialOptionItem = MetalHelper.GetMetalMaterialOptionItems().FirstOrDefault((MaterialOptionItem gemmaterialOption) => gemmaterialOption.SearchAttributeId == searchAttributeId);
			}
		}
		UpdateSelectedMaterialViewModelProperties();
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			if (MetalPrimaryMaterial != null && MetalSecondaryMaterial != null)
			{
				UIHelper.CoreDataService.MaterialsSetMetal(new MetalMaterial[2]
				{
					(MetalMaterial)MetalPrimaryMaterial.SearchAttributeId,
					(MetalMaterial)MetalSecondaryMaterial.SearchAttributeId
				});
			}
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateDesignMaterials();
		}
	}

	public void UpdateSelectedMaterialViewModelProperties()
	{
		MaterialOptionGroup materialOptionGroup = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.Title == Strings.PrimaryMetal);
		if (materialOptionGroup != null && materialOptionGroup.MaterialOptionSelectorVM.SelectedMaterialOptionItem != null)
		{
			MetalPrimaryMaterial = materialOptionGroup.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
		}
		MaterialOptionGroup materialOptionGroup2 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.Title == Strings.SecondaryMetal);
		if (materialOptionGroup2 != null && materialOptionGroup2.MaterialOptionSelectorVM.SelectedMaterialOptionItem != null)
		{
			MetalSecondaryMaterial = materialOptionGroup2.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
		}
		MaterialOptionGroup materialOptionGroup3 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.Title == Strings.CenterStones);
		if (materialOptionGroup3 != null && materialOptionGroup3.MaterialOptionSelectorVM.SelectedMaterialOptionItem != null)
		{
			GemCenterMaterial = materialOptionGroup3.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
		}
		MaterialOptionGroup materialOptionGroup4 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.Title == Strings.SideStones);
		if (materialOptionGroup4 != null && materialOptionGroup4.MaterialOptionSelectorVM.SelectedMaterialOptionItem != null)
		{
			GemSideMaterial = materialOptionGroup4.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
		}
		MaterialOptionGroup materialOptionGroup5 = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup filter) => filter.Title == Strings.AccentStones);
		if (materialOptionGroup5 != null && materialOptionGroup5.MaterialOptionSelectorVM.SelectedMaterialOptionItem != null)
		{
			GemAccentMaterial = materialOptionGroup5.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
		}
	}

	private void CloseAllOtherMaterialGroupOptions(MaterialOptionGroup materialOptionGroup)
	{
		MaterialGroupOptions.ForEach(delegate(MaterialOptionGroup materialGroup)
		{
			if (materialGroup.MaterialOptionSelectorVM.IsPopupOpen)
			{
				materialGroup.MaterialOptionSelectorVM.IsPopupOpen = false;
			}
		});
		if (materialOptionGroup?.MaterialOptionSelectorVM != null)
		{
			materialOptionGroup.MaterialOptionSelectorVM.IsPopupOpen = true;
		}
	}

	private ObservableCollection<MaterialOptionGroup> GenerateMaterialSearchFilters()
	{
		ObservableCollection<MaterialOptionGroup> obj = new ObservableCollection<MaterialOptionGroup>
		{
			new MaterialOptionGroup
			{
				Title = Strings.CenterStones,
				MaterialOptionSelectorVM = new MaterialOptionSelectorViewModel(Strings.CenterStones, MaterialAssignmentMode.GemCenter, (GemCenterMaterial != null) ? GemCenterMaterial.SearchAttributeId : (-1)),
				IsMaterialSelectorAvailable = true,
				IsSelectorBannerVisible = false,
				SelectorBannerTitle = Strings.Center.ToUpperInvariant()
			},
			new MaterialOptionGroup
			{
				Title = Strings.SideStones,
				MaterialOptionSelectorVM = new MaterialOptionSelectorViewModel(Strings.SideStones, MaterialAssignmentMode.GemSide, (GemSideMaterial != null) ? GemSideMaterial.SearchAttributeId : (-1)),
				IsMaterialSelectorAvailable = (UIHelper.KeyProfile.KeyProfileId != 2),
				IsSelectorBannerVisible = false,
				SelectorBannerTitle = Strings.Side.ToUpperInvariant()
			},
			new MaterialOptionGroup
			{
				Title = Strings.AccentStones,
				MaterialOptionSelectorVM = new MaterialOptionSelectorViewModel(Strings.AccentStones, MaterialAssignmentMode.GemAccent, (GemAccentMaterial != null) ? GemAccentMaterial.SearchAttributeId : (-1)),
				IsMaterialSelectorAvailable = true,
				IsSelectorBannerVisible = false,
				SelectorBannerTitle = Strings.Accent.ToUpperInvariant()
			},
			new MaterialOptionGroup
			{
				Title = Strings.PrimaryMetal,
				MaterialOptionSelectorVM = new MaterialOptionSelectorViewModel(Strings.PrimaryMetal, MaterialAssignmentMode.MetalPrimary, (MetalPrimaryMaterial != null) ? MetalPrimaryMaterial.SearchAttributeId : (-1)),
				IsMaterialSelectorAvailable = true,
				IsSelectorBannerVisible = false,
				SelectorBannerTitle = Strings.Primary.ToUpperInvariant()
			},
			new MaterialOptionGroup
			{
				Title = Strings.SecondaryMetal,
				MaterialOptionSelectorVM = new MaterialOptionSelectorViewModel(Strings.SecondaryMetal, MaterialAssignmentMode.MetalSecondary, (MetalSecondaryMaterial != null) ? MetalSecondaryMaterial.SearchAttributeId : (-1)),
				IsMaterialSelectorAvailable = true,
				IsSelectorBannerVisible = false,
				SelectorBannerTitle = Strings.Secondary.ToUpperInvariant()
			}
		};
		obj.ForEach(delegate(MaterialOptionGroup materialOptionGroup)
		{
			materialOptionGroup.MaterialOptionSelectorVM.MaterialOptionSelectionChanged += MaterialOptionSelectorVMOnMaterialOptionSelectionChanged;
		});
		return obj;
	}

	private void InitializeCommands()
	{
		CommandCloseAllOtherMaterialGroupOptions = new RelayCommand<MaterialOptionGroup>((Action<MaterialOptionGroup>)CloseAllOtherMaterialGroupOptions, false);
	}

	private void MaterialOptionSelectorVMOnMaterialOptionSelectionChanged(object sender, EventArgs eventArgs)
	{
		MaterialOptionGroup materialOptionGroup = MaterialGroupOptions.FirstOrDefault((MaterialOptionGroup materialGroupOption) => materialGroupOption.MaterialOptionSelectorVM == sender);
		MaterialGroupOptions.ForEach(delegate(MaterialOptionGroup _)
		{
			Application.Current.Dispatcher.Invoke(() => _.MaterialOptionSelectorVM.IsPopupOpen = false);
		});
		if (materialOptionGroup != null)
		{
			UpdateSelectedMaterialGroupOptions(materialOptionGroup);
		}
	}

	private void UpdateSelectedMaterialGroupOptions(MaterialOptionGroup materialOptionSelectorViewModel)
	{
		switch (materialOptionSelectorViewModel.MaterialOptionSelectorVM.CurrentAssignmentMode)
		{
		case MaterialAssignmentMode.MetalPrimary:
			MetalPrimaryMaterial = materialOptionSelectorViewModel.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
			break;
		case MaterialAssignmentMode.MetalSecondary:
			MetalSecondaryMaterial = materialOptionSelectorViewModel.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
			break;
		case MaterialAssignmentMode.GemCenter:
			GemCenterMaterial = materialOptionSelectorViewModel.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
			break;
		case MaterialAssignmentMode.GemSide:
			GemSideMaterial = materialOptionSelectorViewModel.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
			break;
		case MaterialAssignmentMode.GemAccent:
			GemAccentMaterial = materialOptionSelectorViewModel.MaterialOptionSelectorVM.SelectedMaterialOptionItem;
			break;
		}
		if (ViewModelBase.IsInDesignModeStatic)
		{
			return;
		}
		if (ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns)
		{
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateDesignMaterials();
		}
		else
		{
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.UpdateDesignMaterials();
		}
		DispatcherHelper.WaitForPriority();
		if (materialOptionSelectorViewModel.MaterialOptionSelectorVM.IsMetalMaterialSelection)
		{
			if (MetalPrimaryMaterial != null && MetalSecondaryMaterial != null)
			{
				UIHelper.CoreDataService.MaterialsSetMetal(new MetalMaterial[2]
				{
					(MetalMaterial)MetalPrimaryMaterial.SearchAttributeId,
					(MetalMaterial)MetalSecondaryMaterial.SearchAttributeId
				});
			}
		}
		else if (GemCenterMaterial != null && GemSideMaterial != null && GemAccentMaterial != null)
		{
			UIHelper.CoreDataService.MaterialsSetGem(new GemMaterial[3]
			{
				(GemMaterial)GemCenterMaterial.SearchAttributeId,
				(GemMaterial)GemSideMaterial.SearchAttributeId,
				(GemMaterial)GemAccentMaterial.SearchAttributeId
			});
			ViewModelLocator.Current.GemReportVM.Populate(updateMaterialsOnly: true);
		}
	}
}
