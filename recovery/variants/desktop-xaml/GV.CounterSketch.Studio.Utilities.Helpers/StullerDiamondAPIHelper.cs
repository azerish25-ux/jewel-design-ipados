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
		ShapeFilter result = ShapeFilter.Other;
		switch (studioGemShapeString.Trim().Replace(" ", "").ToLowerInvariant())
		{
		case "cushion":
		case "cushionsquare":
			result = ShapeFilter.Cushion;
			break;
		case "emeraldsquare":
			result = ShapeFilter.Asscher;
			break;
		case "emerald":
			result = ShapeFilter.Emerald;
			break;
		case "heart":
			result = ShapeFilter.Heart;
			break;
		case "marquise":
			result = ShapeFilter.Marquise;
			break;
		case "oval":
			result = ShapeFilter.Oval;
			break;
		case "pear":
			result = ShapeFilter.Pear;
			break;
		case "radiant":
			result = ShapeFilter.Radiant;
			break;
		case "round":
			result = ShapeFilter.Round;
			break;
		case "princess":
			result = ShapeFilter.Square;
			break;
		case "baguettestraight":
			result = ShapeFilter.StraightBaguette;
			break;
		case "triangle":
		case "trillion":
			result = ShapeFilter.Trillion;
			break;
		case "baguettetapered":
			result = ShapeFilter.TaperedBaguette;
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
			ShapeFilter.AntiqueCushion,
			ShapeFilter.Cushion,
			ShapeFilter.AntiqueSquare,
			ShapeFilter.Asscher,
			ShapeFilter.Emerald,
			ShapeFilter.Emerald,
			ShapeFilter.HalfMoon,
			ShapeFilter.Heart,
			ShapeFilter.Marquise,
			ShapeFilter.Oval,
			ShapeFilter.Pear,
			ShapeFilter.Radiant,
			ShapeFilter.Round,
			ShapeFilter.Square,
			ShapeFilter.StraightBaguette,
			ShapeFilter.Triangle,
			ShapeFilter.Trillion,
			ShapeFilter.TaperedBaguette
		};
	}

	public static Tuple<int, List<Diamond>> GetSerializedDiamondProducts(List<ShapeFilter> shapeFilter, decimal caratSizeMin, decimal caratSizeMax, List<DiamondCutLookup> cutFilter, List<DiamondColorLookup> colorFilter, List<DiamondClarityLookup> clarityFilter, double priceLowerLimit, double priceUpperLimit, bool isGIACert, bool isAGSCert, bool isIGICert, bool isEGLCert, bool isCanadianOrigin, int desiredPageSize = 1, int desiredPageNumber = 1, List<int> SerialNumbers = null)
	{
		GlobalErrorHandler.WriteMethodInfo("GetSerializedDiamondProducts");
		if (!UIHelper.CounterSketchOptions.StullerHasAuthenticated)
		{
			SecurityHelper.AuthenticationMode = SecurityHelper.LoginMode.NormalMode;
			SecurityHelper.PromptForAuthentication();
		}
		List<DiamondCertificationFilter> list = new List<DiamondCertificationFilter>();
		if (isGIACert)
		{
			list.Add(DiamondCertificationFilter.GIA);
		}
		if (isAGSCert)
		{
			list.Add(DiamondCertificationFilter.AGS);
		}
		if (isIGICert)
		{
			list.Add(DiamondCertificationFilter.IGI);
		}
		if (isCanadianOrigin)
		{
			list.Add(DiamondCertificationFilter.CanadaMark);
		}
		if (isEGLCert)
		{
			list.Add(DiamondCertificationFilter.EGL);
		}
		using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		DiamondRequest request = ((!SerialNumbers.Any()) ? new DiamondRequest
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
		DiamondResponse diamondResponse = stullerAPIClient.FetchDiamondResponse(request);
		return new Tuple<int, List<Diamond>>(diamondResponse.TotalNumberOfDiamonds, diamondResponse.Diamonds);
	}
}
