using System.Collections.Generic;
using System.Linq;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class PreferenceMaterialsViewModel : ViewModelBase
{
	private List<MaterialOptionItem> _gemMaterialOptionItems;

	private List<MaterialOptionItem> _metalNames;

	private string _currentMetalPrimaryName;

	private string _currentMetalSecondaryName;

	private string _currentCenterGemMaterialName;

	private string _currentSideGemMaterialName;

	private string _currentAccentGemMaterialName;

	private bool _materialDataIsDirty;

	private List<string> _oldMetalMaterialExclusionList;

	private List<string> _oldGemMaterialExclusionList;

	public List<MaterialOptionItem> MetalNamesList
	{
		get
		{
			if (_metalNames == null)
			{
				_metalNames = new List<MaterialOptionItem>();
			}
			return _metalNames;
		}
		set
		{
			_metalNames = value;
			RaisePropertyChanged("MetalNamesList");
		}
	}

	public List<MaterialOptionItem> GemMaterialOptionItems
	{
		get
		{
			if (_gemMaterialOptionItems == null)
			{
				_gemMaterialOptionItems = new List<MaterialOptionItem>();
			}
			return _gemMaterialOptionItems;
		}
		set
		{
			_gemMaterialOptionItems = value;
			RaisePropertyChanged("GemMaterialOptionItems");
		}
	}

	public PreferenceMaterialsViewModel()
	{
		if (base.IsInDesignMode)
		{
			MetalNamesList = new List<MaterialOptionItem>
			{
				new MaterialOptionItem
				{
					Description = "Platinum is a white metal, but unlike gold it is used in jewelry in almost its pure form (approximately 95% pure). It is also more durable than gold, and has a heavier, more substantial feel.",
					IsFavorite = false,
					PrimaryText = "Platinum .950",
					SecondaryText = string.Empty,
					IsReadOnly = true,
					MetalMaterial = MetalMaterial.Platinum
				},
				new MaterialOptionItem
				{
					Description = "Karat palladium white gold has a greyer tone than nickel whites. Palladium is used as the whitener in place of nickel. This metal will meet European standards for purity. This metal is less prone to stress corrosion and is softer as compared to nickel white gold.",
					IsFavorite = false,
					PrimaryText = "18K Palladium White Gold",
					SecondaryText = string.Empty,
					MetalMaterial = MetalMaterial.PalladiumWhite18K,
					IsReadOnly = true
				}
			};
			GemMaterialOptionItems = new List<MaterialOptionItem>
			{
				new MaterialOptionItem
				{
					Description = "Platinum is a white metal, but unlike gold it is used in jewelry in almost its pure form (approximately 95% pure). It is also more durable than gold, and has a heavier, more substantial feel.",
					IsFavorite = false,
					PrimaryText = "Platinum .950",
					SecondaryText = string.Empty,
					IsReadOnly = true,
					MetalMaterial = MetalMaterial.Platinum
				},
				new MaterialOptionItem
				{
					Description = "Karat palladium white gold has a greyer tone than nickel whites. Palladium is used as the whitener in place of nickel. This metal will meet European standards for purity. This metal is less prone to stress corrosion and is softer as compared to nickel white gold.",
					IsFavorite = false,
					PrimaryText = "18K Palladium White Gold",
					SecondaryText = string.Empty,
					MetalMaterial = MetalMaterial.PalladiumWhite18K,
					IsReadOnly = true
				}
			};
		}
		else
		{
			Populate();
		}
	}

	public void Populate()
	{
		_oldGemMaterialExclusionList = UIHelper.CounterSketchOptions.ExcludedGemMaterialsList;
		_oldMetalMaterialExclusionList = UIHelper.CounterSketchOptions.ExcludedMaterialsList;
		MetalNamesList = MetalHelper.GetMetalMaterialOptionItems(isResultsFiltered: false).ToList();
		GemMaterialOptionItems = GemHelper.GetGemMaterialOptionItems().ToList();
		List<MaterialOptionItem> list = _metalNames.Where((MaterialOptionItem _) => UIHelper.CounterSketchOptions.ExcludedMaterialsList.Contains(_.PrimaryText)).ToList();
		_oldGemMaterialExclusionList = UIHelper.CounterSketchOptions.ExcludedGemMaterialsList;
		_oldMetalMaterialExclusionList = UIHelper.CounterSketchOptions.ExcludedMaterialsList;
		list.ForEach(delegate(MaterialOptionItem _)
		{
			_.IsDisabled = true;
		});
		_gemMaterialOptionItems.Where((MaterialOptionItem _) => UIHelper.CounterSketchOptions.ExcludedGemMaterialsList.Contains(_.PrimaryText)).ToList().ForEach(delegate(MaterialOptionItem _)
		{
			_.IsDisabled = true;
		});
		MaterialOptionItem materialOptionItem = _metalNames.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText.Trim() == UIHelper.CounterSketchOptions.PrimaryDefaultMaterialName.Trim());
		MaterialOptionItem materialOptionItem2 = _metalNames.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText.Trim() == UIHelper.CounterSketchOptions.SecondaryDefaultMaterialName.Trim());
		MaterialOptionItem materialOptionItem3 = _gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText.Trim() == UIHelper.CounterSketchOptions.CenterGemDefaultMaterialName.Trim());
		MaterialOptionItem materialOptionItem4 = _gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText.Trim() == UIHelper.CounterSketchOptions.SideGemDefaultMaterialName.Trim());
		MaterialOptionItem materialOptionItem5 = _gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem _) => _.PrimaryText.Trim() == UIHelper.CounterSketchOptions.AccentGemDefaultMaterialName.Trim());
		if (materialOptionItem3 != null)
		{
			materialOptionItem3.IsCenter = true;
			materialOptionItem3.IsDisableBoxVisible = false;
			_currentCenterGemMaterialName = materialOptionItem3.PrimaryText;
		}
		if (materialOptionItem4 != null)
		{
			materialOptionItem4.IsSide = true;
			materialOptionItem4.IsDisableBoxVisible = false;
			_currentSideGemMaterialName = materialOptionItem4.PrimaryText;
		}
		if (materialOptionItem5 != null)
		{
			materialOptionItem5.IsAccent = true;
			materialOptionItem5.IsDisableBoxVisible = false;
			_currentAccentGemMaterialName = materialOptionItem5.PrimaryText;
		}
		if (materialOptionItem != null)
		{
			materialOptionItem.IsPrimary = true;
			materialOptionItem.IsDisableBoxVisible = false;
			_currentMetalPrimaryName = materialOptionItem.PrimaryText;
		}
		if (materialOptionItem2 != null)
		{
			materialOptionItem2.IsSecondary = true;
			materialOptionItem2.IsDisableBoxVisible = false;
			_currentMetalSecondaryName = materialOptionItem2.PrimaryText;
		}
	}

	public void UpdateMaterialPrefrenceSettings()
	{
		UpdateCounterSketchOptionsMaterialExclusionsList();
		UpdatePrimarySecondaryDefaultMaterials();
		UpdateGemDefaultMaterials();
		if (_materialDataIsDirty)
		{
			ViewModelLocator.Current.StudioMaterialSelectionVM.ReloadMaterialSearchFilters();
			_materialDataIsDirty = false;
		}
	}

	private void CheckForExclusionChanges()
	{
		IEnumerable<string> source = UIHelper.CounterSketchOptions.ExcludedMaterialsList.Except(_oldMetalMaterialExclusionList);
		if (UIHelper.CounterSketchOptions.ExcludedGemMaterialsList.Except(_oldGemMaterialExclusionList).Any() || source.Any())
		{
			_materialDataIsDirty = true;
			_oldMetalMaterialExclusionList = UIHelper.CounterSketchOptions.ExcludedMaterialsList;
			_oldGemMaterialExclusionList = UIHelper.CounterSketchOptions.ExcludedGemMaterialsList;
		}
	}

	private void UpdateCounterSketchOptionsMaterialExclusionsList()
	{
		List<string> excludedMaterialsList = (from n in _metalNames
			where n.IsDisabled
			select n.PrimaryText).ToList();
		UIHelper.CounterSketchOptions.ExcludedMaterialsList = excludedMaterialsList;
		List<string> excludedGemMaterialsList = (from n in _gemMaterialOptionItems
			where n.IsDisabled
			select n.PrimaryText).ToList();
		UIHelper.CounterSketchOptions.ExcludedGemMaterialsList = excludedGemMaterialsList;
		CheckForExclusionChanges();
	}

	private void UpdateGemDefaultMaterials()
	{
		MaterialOptionItem materialOptionItem = _gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem _) => _.IsCenter);
		MaterialOptionItem materialOptionItem2 = _gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem _) => _.IsSide);
		MaterialOptionItem materialOptionItem3 = _gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem _) => _.IsAccent);
		if (materialOptionItem != null && materialOptionItem.PrimaryText != _currentCenterGemMaterialName)
		{
			UIHelper.CounterSketchOptions.ExcludedGemMaterialsList.Remove(materialOptionItem.PrimaryText);
			ViewModelLocator.Current.StudioMaterialSelectionVM.GemCenterMaterial = materialOptionItem;
			UIHelper.CounterSketchOptions.CenterGemDefaultMaterialName = materialOptionItem.PrimaryText;
			_currentCenterGemMaterialName = materialOptionItem.PrimaryText;
			_materialDataIsDirty = true;
		}
		if (materialOptionItem2 != null && materialOptionItem2.PrimaryText != _currentSideGemMaterialName)
		{
			UIHelper.CounterSketchOptions.ExcludedGemMaterialsList.Remove(materialOptionItem2.PrimaryText);
			ViewModelLocator.Current.StudioMaterialSelectionVM.GemSideMaterial = materialOptionItem2;
			UIHelper.CounterSketchOptions.SideGemDefaultMaterialName = materialOptionItem2.PrimaryText;
			_currentSideGemMaterialName = materialOptionItem2.PrimaryText;
			_materialDataIsDirty = true;
		}
		if (materialOptionItem3 != null && materialOptionItem3.PrimaryText != _currentAccentGemMaterialName)
		{
			UIHelper.CounterSketchOptions.ExcludedGemMaterialsList.Remove(materialOptionItem3.PrimaryText);
			ViewModelLocator.Current.StudioMaterialSelectionVM.GemAccentMaterial = materialOptionItem3;
			UIHelper.CounterSketchOptions.AccentGemDefaultMaterialName = materialOptionItem3.PrimaryText;
			_currentAccentGemMaterialName = materialOptionItem3.PrimaryText;
			_materialDataIsDirty = true;
		}
	}

	private void UpdatePrimarySecondaryDefaultMaterials()
	{
		MaterialOptionItem materialOptionItem = _metalNames.FirstOrDefault((MaterialOptionItem _) => _.IsPrimary);
		if (materialOptionItem != null && materialOptionItem.PrimaryText != _currentMetalPrimaryName)
		{
			UIHelper.CounterSketchOptions.ExcludedMaterialsList.Remove(materialOptionItem.PrimaryText);
			ViewModelLocator.Current.StudioMaterialSelectionVM.SetMetal(isPrimary: true, materialOptionItem.SearchAttributeId);
			UIHelper.CounterSketchOptions.PrimaryDefaultMaterialName = materialOptionItem.PrimaryText;
			_currentMetalPrimaryName = materialOptionItem.PrimaryText;
			_materialDataIsDirty = true;
		}
		MaterialOptionItem materialOptionItem2 = _metalNames.FirstOrDefault((MaterialOptionItem _) => _.IsSecondary);
		if (materialOptionItem2 != null && materialOptionItem2.PrimaryText != _currentMetalSecondaryName)
		{
			UIHelper.CounterSketchOptions.ExcludedMaterialsList.Remove(materialOptionItem2.PrimaryText);
			ViewModelLocator.Current.StudioMaterialSelectionVM.SetMetal(isPrimary: false, materialOptionItem2.SearchAttributeId);
			UIHelper.CounterSketchOptions.SecondaryDefaultMaterialName = materialOptionItem2.PrimaryText;
			_currentMetalSecondaryName = materialOptionItem2.PrimaryText;
			_materialDataIsDirty = true;
		}
	}
}
