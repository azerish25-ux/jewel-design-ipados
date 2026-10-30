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
				CamJobMetal camJobMetal = list2.First();
				camJobMetal.Weight = list2.Sum((CamJobMetal _) => _.Weight).Value;
				list.Add(camJobMetal);
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

	public static CamJobEngraving CreateCamJobEngraving(FontFace fontFace, FontSize fontSize, string text, Stuller.Web.API.v2.Models.Cam.FillColor fillColor)
	{
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
		Enum.TryParse<CurrencyOption>(new RegionInfo(CultureInfo.GetCultureInfo(currencyCode).LCID).ISOCurrencySymbol, out var result);
		return result;
	}

	public static Stuller.Web.API.v2.Models.Cam.FillColor CreateFontColor(string fontFill)
	{
		Enum.TryParse<Stuller.Web.API.v2.Models.Cam.FillColor>(fontFill, ignoreCase: true, out var result);
		return result;
	}

	public static FontFace CreateFontFace(string fontFace)
	{
		if (Enum.TryParse<FontFace>(fontFace, ignoreCase: true, out var result))
		{
			return result;
		}
		return fontFace.ToLower().Substring(0, 4) switch
		{
			"book" => FontFace.BOOKMAN, 
			"colo" => FontFace.COLONNA, 
			"edwa" => FontFace.EDWARDIANSCRIPT, 
			"luci" => FontFace.LUCIDAHANDWRITING, 
			"old " => FontFace.OLDENGLISH, 
			"scri" => FontFace.SCRIPTMTBOLD, 
			_ => result, 
		};
	}

	public static FontSize CreateFontSize(string fontSize)
	{
		Enum.TryParse<FontSize>(fontSize, ignoreCase: true, out var result);
		return result;
	}

	public static CamJobMoreThanARingOption CreateMoreThanARingOption(bool isMoreThanARingIncludeInOrder, bool isMoreThanARingIncludeDigitalOnlyInOrder)
	{
		CamJobMoreThanARingOption result = CamJobMoreThanARingOption.None;
		if (isMoreThanARingIncludeInOrder)
		{
			result = CamJobMoreThanARingOption.Book;
		}
		else if (isMoreThanARingIncludeDigitalOnlyInOrder)
		{
			result = CamJobMoreThanARingOption.Digital;
		}
		return result;
	}

	public static MetalQualityOption GemvisionMetalMaterialToStullerMetalOption(MetalMaterial metalMaterial, bool isPrototypeOrder)
	{
		MetalQualityOption result = MetalQualityOption.Gold14KWhite;
		if (isPrototypeOrder)
		{
			switch (metalMaterial)
			{
			case MetalMaterial.YellowGold:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.WhiteGold:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.RoseGold:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.Platinum:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.YellowGold10K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.YellowGold18K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.Palladium:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.Silver:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.WhiteGold14KX1:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.Continuum:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.WhiteGold18K:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.WhiteGold10K:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.WhiteGold9K:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.YellowGold9K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.RoseGold18K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.RoseGold10K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.GreenGold10K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.GreenGold14K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.GreenGold18K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.YellowGold22K:
				result = MetalQualityOption.AlloyYellow;
				break;
			case MetalMaterial.PalladiumWhite14K:
				result = MetalQualityOption.AlloyWhite;
				break;
			case MetalMaterial.PalladiumWhite18K:
				result = MetalQualityOption.AlloyWhite;
				break;
			}
		}
		else
		{
			switch (metalMaterial)
			{
			case MetalMaterial.YellowGold:
				result = MetalQualityOption.Gold14KYellow;
				break;
			case MetalMaterial.WhiteGold:
				result = MetalQualityOption.Gold14KWhite;
				break;
			case MetalMaterial.RoseGold:
				result = MetalQualityOption.Gold14KRose;
				break;
			case MetalMaterial.Platinum:
				result = MetalQualityOption.Platinum;
				break;
			case MetalMaterial.YellowGold10K:
				result = MetalQualityOption.Gold10KYellow;
				break;
			case MetalMaterial.YellowGold18K:
				result = MetalQualityOption.Gold18KYellow;
				break;
			case MetalMaterial.Palladium:
				result = MetalQualityOption.Palladium;
				break;
			case MetalMaterial.Silver:
				result = MetalQualityOption.Silver925;
				break;
			case MetalMaterial.WhiteGold14KX1:
				result = MetalQualityOption.Gold14KX1;
				break;
			case MetalMaterial.Continuum:
				result = MetalQualityOption.Continuum;
				break;
			case MetalMaterial.WhiteGold18K:
				result = MetalQualityOption.Gold18KWhite;
				break;
			case MetalMaterial.WhiteGold10K:
				result = MetalQualityOption.Gold10KWhite;
				break;
			case MetalMaterial.WhiteGold9K:
				result = MetalQualityOption.Gold10KWhite;
				break;
			case MetalMaterial.YellowGold9K:
				result = MetalQualityOption.Gold10KYellow;
				break;
			case MetalMaterial.RoseGold18K:
				result = MetalQualityOption.Gold18KRose;
				break;
			case MetalMaterial.RoseGold10K:
				result = MetalQualityOption.Gold10KRose;
				break;
			case MetalMaterial.GreenGold10K:
				result = MetalQualityOption.Gold10KGreen;
				break;
			case MetalMaterial.GreenGold14K:
				result = MetalQualityOption.Gold14KGreen;
				break;
			case MetalMaterial.GreenGold18K:
				result = MetalQualityOption.Gold18KGreen;
				break;
			case MetalMaterial.YellowGold22K:
				result = MetalQualityOption.Gold22KYellow;
				break;
			case MetalMaterial.PalladiumWhite14K:
				result = MetalQualityOption.Gold14PallWhite;
				break;
			case MetalMaterial.PalladiumWhite18K:
				result = MetalQualityOption.Gold18PallWhite;
				break;
			}
		}
		return result;
	}

	public static CamJobResponse GetEstimate(CamJob camJob)
	{
		GlobalErrorHandler.WriteMethodInfo("GetEstimate");
		CamJobRequest camJobRequest = new CamJobRequest();
		camJobRequest.CamJob = camJob;
		camJobRequest.CamJobAction = CamJobAction.Estimate;
		switch (UIHelper.KeyProfile.KeyProfileId)
		{
		case 1:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCH;
			break;
		case 2:
			camJobRequest.CamJobSource = CamJobSource.ROMANTICCREATIONS;
			break;
		case 3:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCHINTERNATIONAL;
			break;
		case 4:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCHBRIDAL;
			break;
		case 5:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCHPLUS;
			break;
		}
		CamJobResponse camJobResponse;
		using (StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", LogRequest))
		{
			camJobResponse = stullerAPIClient.PostCamJobRequest(camJobRequest);
		}
		LogResponse(camJobResponse);
		return camJobResponse;
	}

	public static FinishType GetFinishType(string finishType, out string finishComment)
	{
		FinishType result = FinishType.POLISHED;
		finishComment = string.Empty;
		switch (finishType)
		{
		case "SemiFinish":
			result = FinishType.SEMIPOLISHED;
			break;
		case "Finished":
		case "FinishedAndSet":
			result = FinishType.POLISHED;
			finishComment = "Finished & Set Casting:  As selected in GEMS section.";
			break;
		}
		return result;
	}

	public static SettingType GetSettingType(string settingType)
	{
		SettingType result = SettingType.NONE;
		switch (settingType)
		{
		case "SemiFinish":
			result = SettingType.NONE;
			break;
		case "Finished":
			result = SettingType.CUSTOMER;
			break;
		case "FinishedAndSet":
			result = SettingType.STULLER;
			break;
		}
		return result;
	}

	public static StoneType GetStoneType(int stoneStullerCount, int stoneCustomerCount, int totalStoneCount, int stoneNoneCount)
	{
		StoneType result = StoneType.NONE;
		if (totalStoneCount > 0)
		{
			if (totalStoneCount == stoneStullerCount)
			{
				result = StoneType.STULLER;
			}
			else if (totalStoneCount == stoneCustomerCount)
			{
				result = StoneType.CUSTOMER;
			}
			else if (totalStoneCount == stoneNoneCount)
			{
				result = StoneType.NONE;
			}
			else if (stoneStullerCount > 0 && stoneCustomerCount > 0)
			{
				result = StoneType.STULLERANDCUSTOMER;
			}
			else if (totalStoneCount == stoneStullerCount + stoneNoneCount)
			{
				result = StoneType.STULLER;
			}
			else if (totalStoneCount == stoneCustomerCount + stoneNoneCount)
			{
				result = StoneType.CUSTOMER;
			}
		}
		return result;
	}

	public static TrademarkType GetTrademarkType(string trademarkType)
	{
		TrademarkType result = TrademarkType.NONE;
		switch (trademarkType)
		{
		case "QualityStuller":
			result = TrademarkType.STULLER;
			break;
		case "QualityLogo":
			result = TrademarkType.CUSTOMER;
			break;
		case "QualityLogoStuller":
			result = TrademarkType.STULLERANDCUSTOMER;
			break;
		}
		return result;
	}

	public static CamJobResponse Send24HourOrder(CamJob camJob)
	{
		GlobalErrorHandler.WriteMethodInfo("Send24HourOrder");
		CamJobRequest camJobRequest = new CamJobRequest();
		camJobRequest.CamJob = camJob;
		camJobRequest.CamJobAction = CamJobAction.Quote;
		switch (UIHelper.KeyProfile.KeyProfileId)
		{
		case 1:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCH;
			break;
		case 2:
			camJobRequest.CamJobSource = CamJobSource.ROMANTICCREATIONS;
			break;
		case 3:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCHINTERNATIONAL;
			break;
		}
		using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		return stullerAPIClient.PostCamJobRequest(camJobRequest);
	}

	public static CamJobResponse SendOrder(CamJob camJob)
	{
		CamJobRequest camJobRequest = new CamJobRequest();
		camJobRequest.CamJob = camJob;
		camJobRequest.CamJobAction = CamJobAction.OrderWithPrices;
		switch (UIHelper.KeyProfile.KeyProfileId)
		{
		case 1:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCH;
			break;
		case 2:
			camJobRequest.CamJobSource = CamJobSource.ROMANTICCREATIONS;
			break;
		case 3:
			camJobRequest.CamJobSource = CamJobSource.COUNTERSKETCHINTERNATIONAL;
			break;
		}
		using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		return stullerAPIClient.PostCamJobRequest(camJobRequest);
	}

	private static GemShapeOption ConvertGemvisionCounterSketchStoneShapeToStullerGemShapeOption(string gemShape)
	{
		return gemShape.Trim().ToLowerInvariant() switch
		{
			"cushion" => GemShapeOption.Cushion, 
			"cushionsquare" => GemShapeOption.CushionSquare, 
			"emeraldsquare" => GemShapeOption.EmeraldSquare, 
			"emerald" => GemShapeOption.Emerald, 
			"halfmoon" => GemShapeOption.HalfMoon, 
			"heart" => GemShapeOption.Heart, 
			"marquise" => GemShapeOption.Marquise, 
			"oval" => GemShapeOption.Oval, 
			"pear" => GemShapeOption.Pear, 
			"radiant" => GemShapeOption.Radiant, 
			"round" => GemShapeOption.Round, 
			"princess" => GemShapeOption.Princess, 
			"baguettestraight" => GemShapeOption.BaguetteStraight, 
			"triangle" => GemShapeOption.Triangle, 
			"trillion" => GemShapeOption.Trillion, 
			"trillionround" => GemShapeOption.TrillionRound, 
			"baguettetapered" => GemShapeOption.BaguetteTapered, 
			_ => GemShapeOption.Round, 
		};
	}

	private static CamJob CreateCamJob(string buyerFirstName, string buyerLastName, string emailAddress, string phoneNumber, bool sendSMSNotifications, string shipToAccountNumber, string companyName, CurrencyOption currencyOption, List<CamJobMetal> metals, List<CamJobGem> gems, decimal ringSize, CamJobEngraving insideCamJobEngraving, CamJobEngraving outsideCamJobEngraving, int castings, CamJobMoreThanARingOption camJobMoreThanARingOption, FinishType finishType, TrademarkType trademarkType, SettingType settingType, StoneType stoneType, string stoneComments, string customerStoneComments, string finishComments, string comments, string customerReference, string scsNumber, string seriesNumber, bool isMatchingWeddingBand, bool willSendMatchingEngagement, List<ClientMarkup> clientMarkups, int numberofVisibleFreehandParts, bool includeWaxMemo)
	{
		CamJobMoreThanARing camJobMoreThanARing = new CamJobMoreThanARing();
		camJobMoreThanARing.CamJobMoreThanARingOption = camJobMoreThanARingOption;
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
			CamJobMoreThanARing = camJobMoreThanARing,
			FinishType = finishType,
			TrademarkType = trademarkType,
			AssemblyType = AssemblyType.STULLER,
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
			CamJobMetalCastType = CamJobMetalCastType.Trim,
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
				camJobMetal.CamJobMetalCastType = CamJobMetalCastType.Base;
				camJobMetal.Description = ComponentsEnum.Pendant.ToString();
				list.Add(camJobMetal);
			}
			if (CreateCamJobMetal(ComponentsEnum.Pendant, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal2))
			{
				camJobMetal2.CamJobMetalCastType = CamJobMetalCastType.Trim;
				camJobMetal2.Description = ComponentsEnum.Pendant.ToString();
				list.Add(camJobMetal2);
			}
		}
		else
		{
			if (CreateCamJobMetal(ComponentsEnum.Ring, MaterialClassification.MetalPrimary, metalMaterialPrimary, isPrototypeOrder, out var camJobMetal3))
			{
				camJobMetal3.CamJobMetalCastType = CamJobMetalCastType.Base;
				camJobMetal3.Description = ComponentsEnum.Ring.ToString();
				list.Add(camJobMetal3);
			}
			if (CreateCamJobMetal(ComponentsEnum.Ring, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal4))
			{
				camJobMetal4.CamJobMetalCastType = CamJobMetalCastType.Trim;
				camJobMetal4.Description = ComponentsEnum.Ring.ToString();
				list.Add(camJobMetal4);
			}
			if (CreateCamJobMetal(ComponentsEnum.Band1, MaterialClassification.MetalPrimary, metalMaterialPrimary, isPrototypeOrder, out var camJobMetal5))
			{
				camJobMetal5.CamJobMetalCastType = CamJobMetalCastType.BandBase;
				camJobMetal5.Description = ComponentsEnum.Band1.ToString();
				list.Add(camJobMetal5);
				if (includeDuplicateMatchingBand)
				{
					list.Add(camJobMetal5);
				}
			}
			if (CreateCamJobMetal(ComponentsEnum.Band1, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal6))
			{
				camJobMetal6.CamJobMetalCastType = CamJobMetalCastType.BandTrim;
				camJobMetal6.Description = ComponentsEnum.Band1.ToString();
				list.Add(camJobMetal6);
				if (includeDuplicateMatchingBand)
				{
					list.Add(camJobMetal6);
				}
			}
			if (CreateCamJobMetal(ComponentsEnum.Band2, MaterialClassification.MetalPrimary, metalMaterialPrimary, isPrototypeOrder, out var camJobMetal7))
			{
				camJobMetal7.CamJobMetalCastType = CamJobMetalCastType.BandBase;
				camJobMetal7.Description = ComponentsEnum.Band2.ToString();
				list.Add(camJobMetal7);
			}
			if (CreateCamJobMetal(ComponentsEnum.Band2, MaterialClassification.MetalSecondary, metalMaterialSecondary, isPrototypeOrder, out var camJobMetal8))
			{
				camJobMetal8.CamJobMetalCastType = CamJobMetalCastType.BandTrim;
				camJobMetal8.Description = ComponentsEnum.Band2.ToString();
				list.Add(camJobMetal8);
			}
		}
		return list;
	}

	private static SetTypeOption GetSetType(StudioSetType studioSetType)
	{
		return studioSetType switch
		{
			StudioSetType.Prong => SetTypeOption.Prong, 
			StudioSetType.Bezel => SetTypeOption.Bezel, 
			StudioSetType.Channel => SetTypeOption.Channel, 
			_ => SetTypeOption.Prong, 
		};
	}

	private static SetTypeOption? GetSetTypeOptionFromMountingType(string gemMountingType)
	{
		Type typeFromHandle = typeof(SetTypeOption);
		foreach (SetTypeOption value in Enum.GetValues(typeFromHandle))
		{
			object[] customAttributes = typeFromHandle.GetMember(value.ToString())[0].GetCustomAttributes(typeof(SetTypeOptionMetaDataAttribute), inherit: false);
			if (customAttributes.Any() && customAttributes.First() is SetTypeOptionMetaDataAttribute setTypeOptionMetaDataAttribute && setTypeOptionMetaDataAttribute.SetType == gemMountingType)
			{
				return value;
			}
		}
		return SetTypeOption.Unknown;
	}

	private static void LogRequest(string requestLog)
	{
		GlobalErrorHandler.WriteInfoToEventLog("CAM API Request :\n\t\t" + requestLog);
	}

	private static void LogResponse(CamJobResponse camJobResponse)
	{
		try
		{
			string text = JsonConvert.SerializeObject(camJobResponse);
			GlobalErrorHandler.WriteInfoToEventLog("CAM API Response :\n\t\t" + text);
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Problem Serializing CamJobResponse: {arg}");
		}
	}
}
