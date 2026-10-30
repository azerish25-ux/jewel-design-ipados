using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Pricing.Enums;
using GV.CounterSketch.Studio.RhinoServicesReference;
using GV.CounterSketch.Studio.ViewModel;
using Newtonsoft.Json;
using Stuller.Web.API.v2.Models;
using Stuller.Web.API.v2.Models.Attributes;
using Stuller.Web.API.v2.Models.Cam;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class StullerCamAPIHelper
{
	public static CamJob CreateCamJob(string buyerFirstName, string buyerLastName, string emailAddress, string phoneNumber, bool sendSMSNotifications, string shipToAccountNumber, string companyName, MetalMaterial? metalMaterialPrimary, MetalMaterial? metalMaterialSecondary, List<GemGroup> gemGroupings, string currencyCode, decimal ringSize, string engraveMessage, string engraveFont, string engraveFontSize, string engraveFill, int castings, bool isMoreThanARingIncludeInOrder, bool isMoreThanARingIncludeDigitalOnlyInOrder, string finishAndSetting, string tradeMark, int stoneStullerCount, int stoneCustomerCount, int totalStoneCount, int stoneNoneCount, string stoneComments, string customerStoneComments, string comments, string customerReference, string scsNumber, string seriesNumber, string quoteNumber, bool isMatchingWeddingBand, bool willSendMatchingEngagement, bool isPrototypeOrder, List<ClientMarkup> markups, IEnumerable<MetalMaterial> estimateMetals = null, bool isQuickEstimateOnly = false, bool includeDuplicateMatchingBand = false, bool includeWaxMemo = false)
	{
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		List<CamJobMetal> list;
		if (isQuickEstimateOnly && estimateMetals != null)
		{
			list = new List<CamJobMetal>();
			foreach (MetalMaterial estimateMetal in estimateMetals)
			{
				List<CamJobMetal> list2 = CreateCamJobMetals(estimateMetal, estimateMetal, isPrototypeOrder: false);
				list2.ForEach(delegate(CamJobMetal _)
				{
					_.CamJobMetalCastType = null;
				});
				CamJobMetal val = list2.First();
				val.Weight = list2.Sum((CamJobMetal _) => _.Weight).Value;
				list.Add(val);
			}
		}
		else
		{
			list = CreateCamJobMetals(metalMaterialPrimary.GetValueOrDefault(), metalMaterialSecondary.GetValueOrDefault(), isPrototypeOrder, includeDuplicateMatchingBand);
		}
		List<CamJobGem> gems = null;
		if (gemGroupings != null)
		{
			gems = new List<CamJobGem>(gemGroupings.Select(delegate(GemGroup _)
			{
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				int? serialNumber = null;
				if (!_.GemsInGroup.First().IsMeleeStone)
				{
					int.TryParse(_.StullerId, out var result);
					if (result != 0)
					{
						serialNumber = result;
					}
				}
				if (_.IsNoStoneSupplied)
				{
					_.Quantity = 0;
				}
				return CreateCamJobGem(_.StullerId, _.Shape, _.Quantity, _.Width, _.Height, _.IsCustomerStoneSupplied, Convert.ToDecimal(_.Value), serialNumber, GetSetType(_.SetType), _.GemMountingType, string.Join(",", _.GemNumbers));
			}));
		}
		CamJobEngraving insideCamJobEngraving = null;
		if (engraveMessage != null && engraveMessage.Length > 0)
		{
			insideCamJobEngraving = CreateCamJobEngraving(CreateFontFace(engraveFont), CreateFontSize(engraveFontSize), engraveMessage, CreateFontColor(engraveFill));
		}
		CamJobMoreThanARingOption camJobMoreThanARingOption = CreateMoreThanARingOption(isMoreThanARingIncludeInOrder, isMoreThanARingIncludeDigitalOnlyInOrder);
		string finishComment;
		FinishType finishType = GetFinishType(finishAndSetting, out finishComment);
		SettingType settingType = GetSettingType(finishAndSetting);
		TrademarkType trademarkType = GetTrademarkType(tradeMark);
		StoneType stoneType = GetStoneType(stoneStullerCount, stoneCustomerCount, totalStoneCount, stoneNoneCount);
		int numberofVisibleFreehandParts = UIHelper.CoreDataService.GetNumberofVisibleFreehandParts();
		return CreateCamJob(buyerFirstName, buyerLastName, emailAddress, phoneNumber, sendSMSNotifications, shipToAccountNumber, companyName, CreateCurrencyOption(currencyCode), list, gems, ringSize, insideCamJobEngraving, null, castings, camJobMoreThanARingOption, finishType, trademarkType, settingType, stoneType, stoneComments, customerStoneComments, finishComment, comments, customerReference, scsNumber, seriesNumber, isMatchingWeddingBand, willSendMatchingEngagement, markups, numberofVisibleFreehandParts, includeWaxMemo);
	}

	public static CamJobEngraving CreateCamJobEngraving(FontFace fontFace, FontSize fontSize, string text, FillColor fillColor)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		return new CamJobEngraving
		{
			FontFace = fontFace,
			FontSize = fontSize,
			Text = text,
			FillColor = fillColor
		};
	}

	public static CamJobGem CreateCamJobGem(string itemNumber, string shape, int quantity, double width, double height, bool isCustomerStone, decimal? customerStoneValue, int? serialNumber, SetTypeOption setTypeOption, string gemMountingType, string stoneMapReference)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (serialNumber.HasValue && serialNumber > 0)
		{
			itemNumber = null;
		}
		return new CamJobGem
		{
			ItemNumber = itemNumber,
			Width = (decimal)width,
			Length = (decimal)height,
			Quantity = quantity,
			IsCustomerStone = isCustomerStone,
			CustomerStoneValue = customerStoneValue,
			SerialNumber = serialNumber,
			SetTypeOption = GetSetTypeOptionFromMountingType(gemMountingType),
			StoneMapReference = stoneMapReference,
			GemShapeOption = ConvertGemvisionCounterSketchStoneShapeToStullerGemShapeOption(shape)
		};
	}

	public static CurrencyOption CreateCurrencyOption(string currencyCode)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Enum.TryParse<CurrencyOption>(new RegionInfo(CultureInfo.GetCultureInfo(currencyCode).LCID).ISOCurrencySymbol, out CurrencyOption result);
		return result;
	}

	public static FillColor CreateFontColor(string fontFill)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Enum.TryParse<FillColor>(fontFill, ignoreCase: true, out FillColor result);
		return result;
	}

	public static FontFace CreateFontFace(string fontFace)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (Enum.TryParse<FontFace>(fontFace, ignoreCase: true, out FontFace result))
		{
			return result;
		}
		return (FontFace)(fontFace.ToLower().Substring(0, 4) switch
		{
			"book" => 0, 
			"colo" => 1, 
			"edwa" => 2, 
			"luci" => 3, 
			"old " => 4, 
			"scri" => 5, 
			_ => result, 
		});
	}

	public static FontSize CreateFontSize(string fontSize)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Enum.TryParse<FontSize>(fontSize, ignoreCase: true, out FontSize result);
		return result;
	}

	public static CamJobMoreThanARingOption CreateMoreThanARingOption(bool isMoreThanARingIncludeInOrder, bool isMoreThanARingIncludeDigitalOnlyInOrder)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		CamJobMoreThanARingOption result = (CamJobMoreThanARingOption)0;
		if (isMoreThanARingIncludeInOrder)
		{
			result = (CamJobMoreThanARingOption)1;
		}
		else if (isMoreThanARingIncludeDigitalOnlyInOrder)
		{
			result = (CamJobMoreThanARingOption)2;
		}
		return result;
	}

	public static MetalQualityOption GemvisionMetalMaterialToStullerMetalOption(MetalMaterial metalMaterial, bool isPrototypeOrder)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		MetalQualityOption result = (MetalQualityOption)9;
		if (isPrototypeOrder)
		{
			switch (metalMaterial)
			{
			case MetalMaterial.YellowGold:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.WhiteGold:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.RoseGold:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.Platinum:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.YellowGold10K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.YellowGold18K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.Palladium:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.Silver:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.WhiteGold14KX1:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.Continuum:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.WhiteGold18K:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.WhiteGold10K:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.WhiteGold9K:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.YellowGold9K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.RoseGold18K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.RoseGold10K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.GreenGold10K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.GreenGold14K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.GreenGold18K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.YellowGold22K:
				result = (MetalQualityOption)27;
				break;
			case MetalMaterial.PalladiumWhite14K:
				result = (MetalQualityOption)28;
				break;
			case MetalMaterial.PalladiumWhite18K:
				result = (MetalQualityOption)28;
				break;
			}
		}
		else
		{
			switch (metalMaterial)
			{
			case MetalMaterial.YellowGold:
				result = (MetalQualityOption)8;
				break;
			case MetalMaterial.WhiteGold:
				result = (MetalQualityOption)9;
				break;
			case MetalMaterial.RoseGold:
				result = (MetalQualityOption)10;
				break;
			case MetalMaterial.Platinum:
				result = (MetalQualityOption)21;
				break;
			case MetalMaterial.YellowGold10K:
				result = (MetalQualityOption)3;
				break;
			case MetalMaterial.YellowGold18K:
				result = (MetalQualityOption)14;
				break;
			case MetalMaterial.Palladium:
				result = (MetalQualityOption)24;
				break;
			case MetalMaterial.Silver:
				result = (MetalQualityOption)25;
				break;
			case MetalMaterial.WhiteGold14KX1:
				result = (MetalQualityOption)12;
				break;
			case MetalMaterial.Continuum:
				result = (MetalQualityOption)26;
				break;
			case MetalMaterial.WhiteGold18K:
				result = (MetalQualityOption)15;
				break;
			case MetalMaterial.WhiteGold10K:
				result = (MetalQualityOption)4;
				break;
			case MetalMaterial.WhiteGold9K:
				result = (MetalQualityOption)4;
				break;
			case MetalMaterial.YellowGold9K:
				result = (MetalQualityOption)3;
				break;
			case MetalMaterial.RoseGold18K:
				result = (MetalQualityOption)16;
				break;
			case MetalMaterial.RoseGold10K:
				result = (MetalQualityOption)5;
				break;
			case MetalMaterial.GreenGold10K:
				result = (MetalQualityOption)6;
				break;
			case MetalMaterial.GreenGold14K:
				result = (MetalQualityOption)13;
				break;
			case MetalMaterial.GreenGold18K:
				result = (MetalQualityOption)19;
				break;
			case MetalMaterial.YellowGold22K:
				result = (MetalQualityOption)20;
				break;
			case MetalMaterial.PalladiumWhite14K:
				result = (MetalQualityOption)11;
				break;
			case MetalMaterial.PalladiumWhite18K:
				result = (MetalQualityOption)18;
				break;
			}
		}
		return result;
	}

	public static CamJobResponse GetEstimate(CamJob camJob)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		GlobalErrorHandler.WriteMethodInfo("GetEstimate");
		CamJobRequest val = new CamJobRequest();
		val.CamJob = camJob;
		val.CamJobAction = (CamJobAction)0;
		switch (UIHelper.KeyProfile.KeyProfileId)
		{
		case 1:
			val.CamJobSource = (CamJobSource)1;
			break;
		case 2:
			val.CamJobSource = (CamJobSource)7;
			break;
		case 3:
			val.CamJobSource = (CamJobSource)14;
			break;
		case 4:
			val.CamJobSource = (CamJobSource)16;
			break;
		case 5:
			val.CamJobSource = (CamJobSource)15;
			break;
		}
		StullerAPIClient val2 = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)LogRequest);
		CamJobResponse val3;
		try
		{
			val3 = val2.PostCamJobRequest(val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		LogResponse(val3);
		return val3;
	}

	public static FinishType GetFinishType(string finishType, out string finishComment)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		FinishType result = (FinishType)1;
		finishComment = string.Empty;
		switch (finishType)
		{
		case "SemiFinish":
			result = (FinishType)0;
			break;
		case "Finished":
		case "FinishedAndSet":
			result = (FinishType)1;
			finishComment = "Finished & Set Casting:  As selected in GEMS section.";
			break;
		}
		return result;
	}

	public static SettingType GetSettingType(string settingType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		SettingType result = (SettingType)2;
		switch (settingType)
		{
		case "SemiFinish":
			result = (SettingType)2;
			break;
		case "Finished":
			result = (SettingType)1;
			break;
		case "FinishedAndSet":
			result = (SettingType)0;
			break;
		}
		return result;
	}

	public static StoneType GetStoneType(int stoneStullerCount, int stoneCustomerCount, int totalStoneCount, int stoneNoneCount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		StoneType result = (StoneType)3;
		if (totalStoneCount > 0)
		{
			if (totalStoneCount == stoneStullerCount)
			{
				result = (StoneType)0;
			}
			else if (totalStoneCount == stoneCustomerCount)
			{
				result = (StoneType)1;
			}
			else if (totalStoneCount == stoneNoneCount)
			{
				result = (StoneType)3;
			}
			else if (stoneStullerCount > 0 && stoneCustomerCount > 0)
			{
				result = (StoneType)2;
			}
			else if (totalStoneCount == stoneStullerCount + stoneNoneCount)
			{
				result = (StoneType)0;
			}
			else if (totalStoneCount == stoneCustomerCount + stoneNoneCount)
			{
				result = (StoneType)1;
			}
		}
		return result;
	}

	public static TrademarkType GetTrademarkType(string trademarkType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		TrademarkType result = (TrademarkType)3;
		switch (trademarkType)
		{
		case "QualityStuller":
			result = (TrademarkType)0;
			break;
		case "QualityLogo":
			result = (TrademarkType)1;
			break;
		case "QualityLogoStuller":
			result = (TrademarkType)2;
			break;
		}
		return result;
	}

	public static CamJobResponse Send24HourOrder(CamJob camJob)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		GlobalErrorHandler.WriteMethodInfo("Send24HourOrder");
		CamJobRequest val = new CamJobRequest();
		val.CamJob = camJob;
		val.CamJobAction = (CamJobAction)1;
		switch (UIHelper.KeyProfile.KeyProfileId)
		{
		case 1:
			val.CamJobSource = (CamJobSource)1;
			break;
		case 2:
			val.CamJobSource = (CamJobSource)7;
			break;
		case 3:
			val.CamJobSource = (CamJobSource)14;
			break;
		}
		StullerAPIClient val2 = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
		try
		{
			return val2.PostCamJobRequest(val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static CamJobResponse SendOrder(CamJob camJob)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		CamJobRequest val = new CamJobRequest();
		val.CamJob = camJob;
		val.CamJobAction = (CamJobAction)3;
		switch (UIHelper.KeyProfile.KeyProfileId)
		{
		case 1:
			val.CamJobSource = (CamJobSource)1;
			break;
		case 2:
			val.CamJobSource = (CamJobSource)7;
			break;
		case 3:
			val.CamJobSource = (CamJobSource)14;
			break;
		}
		StullerAPIClient val2 = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
		try
		{
			return val2.PostCamJobRequest(val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static GemShapeOption ConvertGemvisionCounterSketchStoneShapeToStullerGemShapeOption(string gemShape)
	{
		return (GemShapeOption)(gemShape.Trim().ToLowerInvariant() switch
		{
			"cushion" => 3, 
			"cushionsquare" => 2, 
			"emeraldsquare" => 5, 
			"emerald" => 4, 
			"halfmoon" => 6, 
			"heart" => 7, 
			"marquise" => 8, 
			"oval" => 9, 
			"pear" => 10, 
			"radiant" => 12, 
			"round" => 13, 
			"princess" => 11, 
			"baguettestraight" => 0, 
			"triangle" => 14, 
			"trillion" => 15, 
			"trillionround" => 16, 
			"baguettetapered" => 1, 
			_ => 13, 
		});
	}

	private static CamJob CreateCamJob(string buyerFirstName, string buyerLastName, string emailAddress, string phoneNumber, bool sendSMSNotifications, string shipToAccountNumber, string companyName, CurrencyOption currencyOption, List<CamJobMetal> metals, List<CamJobGem> gems, decimal ringSize, CamJobEngraving insideCamJobEngraving, CamJobEngraving outsideCamJobEngraving, int castings, CamJobMoreThanARingOption camJobMoreThanARingOption, FinishType finishType, TrademarkType trademarkType, SettingType settingType, StoneType stoneType, string stoneComments, string customerStoneComments, string finishComments, string comments, string customerReference, string scsNumber, string seriesNumber, bool isMatchingWeddingBand, bool willSendMatchingEngagement, List<ClientMarkup> clientMarkups, int numberofVisibleFreehandParts, bool includeWaxMemo)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		CamJobMoreThanARing val = new CamJobMoreThanARing();
		val.CamJobMoreThanARingOption = camJobMoreThanARingOption;
		return new CamJob
		{
			BuyerFirstName = buyerFirstName,
			BuyerLastName = buyerLastName,
			EmailAddress = emailAddress,
			PhoneNumber = phoneNumber,
			ShipToAccountNumber = shipToAccountNumber,
			CompanyName = companyName,
			CurrencyOption = currencyOption,
			CamJobMetals = metals,
			CamJobGems = gems,
			CamJobRingSize = new CamJobRingSize
			{
				RingSize = ringSize
			},
			InsideCamJobEngraving = insideCamJobEngraving,
			OutsideCamJobEngraving = outsideCamJobEngraving,
			Castings = castings,
			CamJobMoreThanARing = val,
			FinishType = finishType,
			TrademarkType = trademarkType,
			AssemblyType = (AssemblyType)0,
			SettingType = settingType,
			StoneType = stoneType,
			StoneComments = stoneComments,
			CustomerStoneComments = customerStoneComments,
			FinishComments = finishComments,
			Comments = comments,
			CustomerReference = customerReference,
			ScsNumber = scsNumber,
			Series = seriesNumber,
			IsMatchingWeddingBand = isMatchingWeddingBand,
			WillSendMatchingEngagement = willSendMatchingEngagement,
			ClientMarkups = clientMarkups,
			Quantity = 1,
			IndependentPartCount = numberofVisibleFreehandParts,
			WaxMemo = includeWaxMemo,
			SendSmsStatusUpdate = sendSMSNotifications
		};
	}

	private static bool CreateCamJobMetal(ComponentsEnum whichComponent, MaterialClassification whichMaterialClassification, MetalMaterial whichMetalMaterial, bool isPrototypeOrder, out CamJobMetal camJobMetal)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		camJobMetal = null;
		Tuple<ComponentsEnum, MaterialClassification> key = new Tuple<ComponentsEnum, MaterialClassification>(whichComponent, whichMaterialClassification);
		if (!ViewModelLocator.Current.OrderSummaryVM.CalculatedVolumes.ContainsKey(key))
		{
			return false;
		}
		double num = ViewModelLocator.Current.OrderSummaryVM.CalculatedVolumes[key];
		if (num <= 0.0)
		{
			return false;
		}
		camJobMetal = new CamJobMetal
		{
			CamJobMetalCastType = (CamJobMetalCastType)1,
			MetalQualityOption = GemvisionMetalMaterialToStullerMetalOption(whichMetalMaterial, isPrototypeOrder),
			Weight = CSMetalHelper.GetMetalPennyWeight(num, whichMetalMaterial)
		};
		return true;
	}

	private static List<CamJobMetal> CreateCamJobMetals(MetalMaterial metalMaterialPrimary, MetalMaterial metalMaterialSecondary, bool isPrototypeOrder, bool includeDuplicateMatchingBand = false)
	{
		List<CamJobMetal> list = new List<CamJobMetal>();
		if (ViewModelLocator.Current.OrderSummaryVM.CalculatedVolumes.Keys.Any((Tuple<ComponentsEnum, MaterialClassification> key) => key.Item1 == ComponentsEnum.Pendant))
		{
			if (CreateCamJobMetal(ComponentsEnum.Pendant, MaterialClassification.MetalPrimary, metalMaterialPrimary, isPrototypeOrder, out var camJobMetal))
			{
				camJobMetal.CamJobMetalCastType = (CamJobMetalCastType)0;
				camJobMetal.Description = ComponentsEnum.Pendant.ToString();
				list.Add(camJobMetal);
			}
			if (CreateCamJobMetal(ComponentsEnum.Pendant, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal2))
			{
				camJobMetal2.CamJobMetalCastType = (CamJobMetalCastType)1;
				camJobMetal2.Description = ComponentsEnum.Pendant.ToString();
				list.Add(camJobMetal2);
			}
		}
		else
		{
			if (CreateCamJobMetal(ComponentsEnum.Ring, MaterialClassification.MetalPrimary, metalMaterialPrimary, isPrototypeOrder, out var camJobMetal3))
			{
				camJobMetal3.CamJobMetalCastType = (CamJobMetalCastType)0;
				camJobMetal3.Description = ComponentsEnum.Ring.ToString();
				list.Add(camJobMetal3);
			}
			if (CreateCamJobMetal(ComponentsEnum.Ring, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal4))
			{
				camJobMetal4.CamJobMetalCastType = (CamJobMetalCastType)1;
				camJobMetal4.Description = ComponentsEnum.Ring.ToString();
				list.Add(camJobMetal4);
			}
			if (CreateCamJobMetal(ComponentsEnum.Band1, MaterialClassification.MetalPrimary, metalMaterialPrimary, isPrototypeOrder, out var camJobMetal5))
			{
				camJobMetal5.CamJobMetalCastType = (CamJobMetalCastType)2;
				camJobMetal5.Description = ComponentsEnum.Band1.ToString();
				list.Add(camJobMetal5);
				if (includeDuplicateMatchingBand)
				{
					list.Add(camJobMetal5);
				}
			}
			if (CreateCamJobMetal(ComponentsEnum.Band1, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal6))
			{
				camJobMetal6.CamJobMetalCastType = (CamJobMetalCastType)3;
				camJobMetal6.Description = ComponentsEnum.Band1.ToString();
				list.Add(camJobMetal6);
				if (includeDuplicateMatchingBand)
				{
					list.Add(camJobMetal6);
				}
			}
			if (CreateCamJobMetal(ComponentsEnum.Band2, MaterialClassification.MetalPrimary, metalMaterialPrimary, isPrototypeOrder, out var camJobMetal7))
			{
				camJobMetal7.CamJobMetalCastType = (CamJobMetalCastType)2;
				camJobMetal7.Description = ComponentsEnum.Band2.ToString();
				list.Add(camJobMetal7);
			}
			if (CreateCamJobMetal(ComponentsEnum.Band2, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal8))
			{
				camJobMetal8.CamJobMetalCastType = (CamJobMetalCastType)3;
				camJobMetal8.Description = ComponentsEnum.Band2.ToString();
				list.Add(camJobMetal8);
			}
		}
		return list;
	}

	private static SetTypeOption GetSetType(StudioSetType studioSetType)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return (SetTypeOption)(studioSetType switch
		{
			StudioSetType.Prong => 18, 
			StudioSetType.Bezel => 3, 
			StudioSetType.Channel => 6, 
			_ => 18, 
		});
	}

	private static SetTypeOption? GetSetTypeOptionFromMountingType(string gemMountingType)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		Type typeFromHandle = typeof(SetTypeOption);
		foreach (SetTypeOption value in Enum.GetValues(typeFromHandle))
		{
			object[] customAttributes = typeFromHandle.GetMember(((object)value/*cast due to .constrained prefix*/).ToString())[0].GetCustomAttributes(typeof(SetTypeOptionMetaDataAttribute), inherit: false);
			if (customAttributes.Any())
			{
				object obj = customAttributes.First();
				SetTypeOptionMetaDataAttribute val = (SetTypeOptionMetaDataAttribute)((obj is SetTypeOptionMetaDataAttribute) ? obj : null);
				if (val != null && val.SetType == gemMountingType)
				{
					return value;
				}
			}
		}
		return (SetTypeOption)26;
	}

	private static void LogRequest(string requestLog)
	{
		GlobalErrorHandler.WriteInfoToEventLog("CAM API Request :\n\t\t" + requestLog);
	}

	private static void LogResponse(CamJobResponse camJobResponse)
	{
		try
		{
			string text = JsonConvert.SerializeObject((object)camJobResponse);
			GlobalErrorHandler.WriteInfoToEventLog("CAM API Response :\n\t\t" + text);
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Problem Serializing CamJobResponse: {arg}");
		}
	}
}
