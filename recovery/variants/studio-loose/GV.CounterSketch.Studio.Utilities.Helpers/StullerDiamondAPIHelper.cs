using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class StullerDiamondAPIHelper
{
	public static ShapeFilter ConvertGemvisionCounterSketchStoneShapeToStullerStoneShapeFilter(string studioGemShapeString)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		ShapeFilter result = (ShapeFilter)22;
		switch (studioGemShapeString.Trim().Replace(" ", "").ToLowerInvariant())
		{
		case "cushion":
		case "cushionsquare":
			result = (ShapeFilter)15;
			break;
		case "emeraldsquare":
			result = (ShapeFilter)16;
			break;
		case "emerald":
			result = (ShapeFilter)5;
			break;
		case "heart":
			result = (ShapeFilter)6;
			break;
		case "marquise":
			result = (ShapeFilter)4;
			break;
		case "oval":
			result = (ShapeFilter)3;
			break;
		case "pear":
			result = (ShapeFilter)7;
			break;
		case "radiant":
			result = (ShapeFilter)13;
			break;
		case "round":
			result = (ShapeFilter)0;
			break;
		case "princess":
			result = (ShapeFilter)2;
			break;
		case "baguettestraight":
			result = (ShapeFilter)10;
			break;
		case "triangle":
		case "trillion":
			result = (ShapeFilter)9;
			break;
		case "baguettetapered":
			result = (ShapeFilter)11;
			break;
		}
		return result;
	}

	public static List<string> ConvertStullerStoneShapeToGemvisionCounterSketchStoneShapeString(string stullerStoneShape)
	{
		List<string> list = new List<string>();
		switch (stullerStoneShape.Trim().ToLowerInvariant())
		{
		case "cushion":
			list.Add("cushion");
			list.Add("cushionsquare");
			break;
		case "antique cushion":
			list.Add("cushionsquare");
			break;
		case "asscher":
			list.Add("emeraldsquare");
			break;
		case "emerald cut":
			list.Add("emerald");
			break;
		case "heart shape":
			list.Add("heart");
			break;
		case "marquise":
			list.Add("marquise");
			break;
		case "oval":
			list.Add("oval");
			break;
		case "pear shape":
			list.Add("pear");
			break;
		case "radiant":
			list.Add("radiant");
			break;
		case "round":
			list.Add("round");
			break;
		case "square":
			list.Add("princess");
			break;
		case "straight baguette":
			list.Add("baguettestraight");
			list.Add("baguettetapered");
			break;
		case "triangle":
			list.Add("triangle");
			list.Add("trillion");
			list.Add("trillionround");
			break;
		case "trillion":
			list.Add("trillion");
			list.Add("triangle");
			list.Add("trillionround");
			break;
		case "baguettetapered":
			list.Add("baguettetapered");
			list.Add("baguettestraight");
			break;
		}
		return list;
	}

	public static List<DiamondClarityLookup> GetAllSerializedDiamondClarity()
	{
		return (from DiamondClarityLookup _ in Enum.GetValues(typeof(DiamondClarityLookup))
			orderby (int)_
			select _).ToList();
	}

	public static List<DiamondColorLookup> GetAllSerializedDiamondColors()
	{
		return (from DiamondColorLookup _ in Enum.GetValues(typeof(DiamondColorLookup))
			orderby (int)_
			select _).ToList();
	}

	public static List<DiamondCutLookup> GetAllSerializedDiamondCuts()
	{
		return (from DiamondCutLookup _ in Enum.GetValues(typeof(DiamondCutLookup))
			orderby (int)_
			select _).ToList();
	}

	public static List<ShapeFilter> GetAllSerializedDiamondShapes()
	{
		return new List<ShapeFilter>
		{
			(ShapeFilter)14,
			(ShapeFilter)15,
			(ShapeFilter)21,
			(ShapeFilter)16,
			(ShapeFilter)5,
			(ShapeFilter)5,
			(ShapeFilter)17,
			(ShapeFilter)6,
			(ShapeFilter)4,
			(ShapeFilter)3,
			(ShapeFilter)7,
			(ShapeFilter)13,
			(ShapeFilter)0,
			(ShapeFilter)2,
			(ShapeFilter)10,
			(ShapeFilter)8,
			(ShapeFilter)9,
			(ShapeFilter)11
		};
	}

	public static Tuple<int, List<Diamond>> GetSerializedDiamondProducts(List<ShapeFilter> shapeFilter, decimal caratSizeMin, decimal caratSizeMax, List<DiamondCutLookup> cutFilter, List<DiamondColorLookup> colorFilter, List<DiamondClarityLookup> clarityFilter, double priceLowerLimit, double priceUpperLimit, bool isGIACert, bool isAGSCert, bool isIGICert, bool isEGLCert, bool isCanadianOrigin, int desiredPageSize = 1, int desiredPageNumber = 1, List<int> SerialNumbers = null)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		GlobalErrorHandler.WriteMethodInfo("GetSerializedDiamondProducts");
		if (!UIHelper.CounterSketchOptions.StullerHasAuthenticated)
		{
			SecurityHelper.AuthenticationMode = SecurityHelper.LoginMode.NormalMode;
			SecurityHelper.PromptForAuthentication();
		}
		List<DiamondCertificationFilter> list = new List<DiamondCertificationFilter>();
		if (isGIACert)
		{
			list.Add((DiamondCertificationFilter)1);
		}
		if (isAGSCert)
		{
			list.Add((DiamondCertificationFilter)2);
		}
		if (isIGICert)
		{
			list.Add((DiamondCertificationFilter)3);
		}
		if (isCanadianOrigin)
		{
			list.Add((DiamondCertificationFilter)6);
		}
		if (isEGLCert)
		{
			list.Add((DiamondCertificationFilter)4);
		}
		StullerAPIClient val = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
		try
		{
			DiamondRequest val2 = ((!SerialNumbers.Any()) ? new DiamondRequest
			{
				Certification = list,
				Shape = shapeFilter,
				SizeRange = new List<decimal> { caratSizeMin, caratSizeMax },
				Cut = cutFilter,
				Color = colorFilter,
				Clarity = clarityFilter,
				PriceRange = new List<double> { priceLowerLimit, priceUpperLimit },
				DepthPercent = new List<decimal>(),
				FancyColors = new List<string>(),
				Fluorescence = new List<DiamondFluorescenceLookup>(),
				LengthToWidthRatio = new List<decimal>(),
				Polish = new List<DiamondPolishLookup>(),
				Symmetry = new List<DiamondSymmetryLookup>(),
				TablePercent = new List<decimal>(),
				PageSize = desiredPageSize,
				Page = desiredPageNumber,
				SerialNumbers = SerialNumbers
			} : new DiamondRequest
			{
				PageSize = desiredPageSize,
				Page = desiredPageNumber,
				SerialNumbers = SerialNumbers
			});
			DiamondResponse val3 = val.FetchDiamondResponse(val2);
			return new Tuple<int, List<Diamond>>(val3.TotalNumberOfDiamonds, val3.Diamonds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
