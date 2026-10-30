using System;
using System.Collections.Generic;
using System.Linq;
using GV.CounterSketch.Common;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class PreferenceOrderScreenOptionsViewModel : ViewModelBase
{
	private List<OrderSummaryPrefItemModel> _orderSummaryPrefItemModels;

	public RelayCommand<string> CommandToggleSection { get; set; }

	public List<OrderSummaryPrefItemModel> OrderSummaryPrefItemModels
	{
		get
		{
			if (_orderSummaryPrefItemModels == null)
			{
				_orderSummaryPrefItemModels = new List<OrderSummaryPrefItemModel>();
			}
			return _orderSummaryPrefItemModels;
		}
		set
		{
			_orderSummaryPrefItemModels = value;
		}
	}

	public bool IsCacheInvalid { get; set; }

	public List<string> HiddenSections { get; set; }

	public PreferenceOrderScreenOptionsViewModel()
	{
		if (((ViewModelBase)this).IsInDesignMode)
		{
			Populate();
			return;
		}
		Populate();
		InitializeCommands();
		SetupFromCounterSketchOptions();
	}

	public void DisableSection(string sectionName)
	{
		if (!HiddenSections.Contains(sectionName))
		{
			HiddenSections.Add(sectionName);
		}
		OrderSummaryPrefItemModels.Where((OrderSummaryPrefItemModel i) => i.Section == sectionName).ToList().ForEach(delegate(OrderSummaryPrefItemModel _)
		{
			_.IsDisabled = false;
		});
		OrderSummaryPrefItemModels.Where((OrderSummaryPrefItemModel i) => i.Section == sectionName && i.IsCheckBoxVisible).ToList().ForEach(delegate(OrderSummaryPrefItemModel _)
		{
			_.IsDisabled = HiddenSections.Contains(_.Section);
		});
	}

	public void EnableSection(string sectionName)
	{
		if (HiddenSections.Contains(sectionName))
		{
			HiddenSections.Remove(sectionName);
		}
		OrderSummaryPrefItemModels.Where((OrderSummaryPrefItemModel s) => s.Section == sectionName).ToList().ForEach(delegate(OrderSummaryPrefItemModel _)
		{
			_.IsDisabled = HiddenSections.Contains(_.Section);
		});
	}

	public bool IsSectionDisabled(string sectionName)
	{
		return HiddenSections.Contains(sectionName);
	}

	public void Populate()
	{
		HiddenSections = new List<string>();
		List<OrderSummaryPrefItemModel> orderSummaryPrefItemModels = new List<OrderSummaryPrefItemModel>
		{
			new OrderSummaryPrefItemModel(Strings.Purchase_Stone_s, Strings.LargeStonesSource, isRadioButtonVisible: false, isCheckBoxVisible: false),
			new OrderSummaryPrefItemModel(Strings.Use_My_Stone_s, Strings.LargeStonesSource, isRadioButtonVisible: false),
			new OrderSummaryPrefItemModel(Strings.No_Stone_s_Supplied, Strings.LargeStonesSource, isRadioButtonVisible: false),
			new OrderSummaryPrefItemModel(Strings.Purchase_Stone_s, Strings.MeleeSource, isRadioButtonVisible: false, isCheckBoxVisible: false),
			new OrderSummaryPrefItemModel(Strings.Use_My_Stone_s, Strings.MeleeSource, isRadioButtonVisible: false),
			new OrderSummaryPrefItemModel(Strings.No_Stone_s_Supplied, Strings.MeleeSource, isRadioButtonVisible: false),
			new OrderSummaryPrefItemModel(Strings.FinishedSetAsSpecified, Strings.SettingFinish, isRadioButtonVisible: false, isCheckBoxVisible: false),
			new OrderSummaryPrefItemModel(Strings.FinishedCastingNoGems, Strings.SettingFinish, isRadioButtonVisible: false, isCheckBoxVisible: false),
			new OrderSummaryPrefItemModel(Strings.SemiFinishedCastingNoGems, Strings.SettingFinish, isRadioButtonVisible: false),
			new OrderSummaryPrefItemModel(Strings.Prototype, Strings.SettingFinish, isRadioButtonVisible: false),
			new OrderSummaryPrefItemModel(Strings.Noqualitymarkortrademark, Strings.HALLMARK, isRadioButtonVisible: true, isCheckBoxVisible: false),
			new OrderSummaryPrefItemModel(Strings.LaserengravequalitymarkandStullertrademark, Strings.HALLMARK),
			new OrderSummaryPrefItemModel(Strings.Laserengravequalitymarkandmylogo, Strings.HALLMARK),
			new OrderSummaryPrefItemModel(Strings.LaserengravequalitymarkStullertrademarkandmylogo, Strings.HALLMARK),
			new OrderSummaryPrefItemModel(Strings.AddCustomEngraving, Strings.ENGRAVING, isRadioButtonVisible: false),
			new OrderSummaryPrefItemModel(Strings.No, Strings.MORETHANARING, isRadioButtonVisible: true, isCheckBoxVisible: false),
			new OrderSummaryPrefItemModel(Strings.BookAndDigitalImages, Strings.MORETHANARING),
			new OrderSummaryPrefItemModel(Strings.DigitalImagesOnly, Strings.MORETHANARING),
			new OrderSummaryPrefItemModel(Strings.YesSendAWaxMemo, Strings.WaxMemo),
			new OrderSummaryPrefItemModel(Strings.NoWaxMemo, Strings.WaxMemo)
		};
		OrderSummaryPrefItemModels = orderSummaryPrefItemModels;
	}

	public void SetupFromCounterSketchOptions()
	{
		OrderSummaryPrefItemModels?.Where((OrderSummaryPrefItemModel _) => UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.Any((OrderSummaryPrefItemModel i) => i.Title == _.Title)).ToList().ForEach(delegate(OrderSummaryPrefItemModel _)
		{
			_.IsDisabled = true;
		});
		if (UIHelper.CounterSketchOptions.DefaultOrderSummaryItems.Count == 0)
		{
			UIHelper.CounterSketchOptions.DefaultOrderSummaryItems?.Add(OrderSummaryPrefItemModels.FirstOrDefault((OrderSummaryPrefItemModel _) => _.Title == "No"));
			UIHelper.CounterSketchOptions.DefaultOrderSummaryItems?.Add(OrderSummaryPrefItemModels.FirstOrDefault((OrderSummaryPrefItemModel _) => _.Title == "No quality mark or trademark."));
			UIHelper.CounterSketchOptions.DefaultOrderSummaryItems?.Add(OrderSummaryPrefItemModels.FirstOrDefault((OrderSummaryPrefItemModel _) => _.Title == "Purchase Stone(s)" && _.Section == "Melee Source"));
			UIHelper.CounterSketchOptions.DefaultOrderSummaryItems?.Add(OrderSummaryPrefItemModels.FirstOrDefault((OrderSummaryPrefItemModel _) => _.Title == "Purchase Stone(s)" && _.Section == "Large Stones Source"));
		}
		OrderSummaryPrefItemModels.Where((OrderSummaryPrefItemModel _) => UIHelper.CounterSketchOptions.DefaultOrderSummaryItems.Any((OrderSummaryPrefItemModel i) => i?.Title == _.Title)).ToList().ForEach(delegate(OrderSummaryPrefItemModel _)
		{
			_.IsDefault = true;
		});
	}

	public void UpdatePreferenceOrderSummaryCounterSketchOptions()
	{
		List<OrderSummaryPrefItemModel> disabledOrderSummaryItems = OrderSummaryPrefItemModels.Where((OrderSummaryPrefItemModel _) => _.IsDisabled).ToList();
		List<OrderSummaryPrefItemModel> defaultOrderSummaryItems = OrderSummaryPrefItemModels.Where((OrderSummaryPrefItemModel _) => _.IsDefault).ToList();
		IsCacheInvalid = true;
		UIHelper.CounterSketchOptions.DisabledOrderSummaryItems = disabledOrderSummaryItems;
		UIHelper.CounterSketchOptions.DefaultOrderSummaryItems = defaultOrderSummaryItems;
	}

	private string FormatDiamondQualityForUI(string diamondQuality)
	{
		string[] array = diamondQuality.Split('|');
		return array[1].PadLeft(4) + " : " + array[0].PadLeft(4);
	}

	private void InitializeCommands()
	{
		CommandToggleSection = new RelayCommand<string>((Action<string>)ToggleSection, false);
	}

	private void ToggleSection(string sectionName)
	{
		if (IsSectionDisabled(sectionName))
		{
			EnableSection(sectionName);
		}
		else
		{
			DisableSection(sectionName);
		}
		UpdatePreferenceOrderSummaryCounterSketchOptions();
	}
}
