using System.Collections.Generic;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadProng
{
	public static bool Prong(int prongIndex, CSGemData gem, ProngParam[] prongParams, Curve headTop, Curve headBase, Curve bezelProngLine, ValueListItem baseStyle, double basePointSize, bool enableDoItYourself, bool planarDome, Point3d focalPoint, double cornerBlend, out string error)
	{
		ProngParam prongParam = prongParams[prongIndex];
		prongParam.OutCurves = new List<Curve>();
		prongParam.OutProngs = new List<Brep>();
		prongParam.TopDome = null;
		prongParam.MiddleProng = null;
		prongParam.LowerDome = null;
		if (!gem.GetGemXformRC(out var xDown, out var xUp))
		{
			error = "GemXform null";
			return false;
		}
		Curve prongRail = prongParam.ProngRail;
		headTop.ClosestPoint(prongRail.PointAtStart, out var t);
		headBase.ClosestPoint(prongRail.PointAtEnd, out var t2);
		double prongBaseOffset = (0.0 - prongParam.ProngBaseY) * prongParam.ProngBaseNudge;
		double prongTopOffset = (0.0 - prongParam.ProngTopY) * prongParam.ProngTopNudge;
		if (baseStyle == ValueListItem.HeadBuilderProngHead)
		{
			prongTopOffset = 0.0;
			prongBaseOffset = 0.0;
			Point3d pointAtStart = prongRail.PointAtStart;
			Vector3d motion = new Vector3d(pointAtStart.X, pointAtStart.Y, 0.0);
			pointAtStart.Transform(Transform.Translation(motion));
			headTop.ClosestPoint(pointAtStart, out t);
			Point3d pointAtEnd = prongRail.PointAtEnd;
			motion = new Vector3d(pointAtEnd.X, pointAtEnd.Y, 0.0);
			pointAtEnd.Transform(Transform.Translation(motion));
			headBase.ClosestPoint(pointAtEnd, out t2);
		}
		if (prongParam.ProngType == 1)
		{
			Brep outProng = null;
			CSGemData cSGemData = (CSGemData)gem.Duplicate();
			cSGemData.ApplyTransform(xDown);
			if (CSHeadBezel.BezelProfile(bezelProngLine, prongParam, prongTopOffset, prongBaseOffset, baseStyle, planarDome, 0.0, 0.0, 0.0, 0.0, out var bezelProngOutline, out error) && !CSHeadVProng.VProng(prongIndex, prongParam, headTop, headBase, bezelProngOutline, cSGemData, baseStyle, cornerBlend, out outProng, out error))
			{
				return false;
			}
			if (outProng != null)
			{
				outProng.Transform(xUp);
				prongParam.OutProngs.Add(outProng);
			}
		}
		else
		{
			if (!CSHeadProngProfileSweep.ProngProfileSweep(prongIndex, prongParams, headTop, t, prongTopOffset, headBase, t2, prongBaseOffset, baseStyle, basePointSize, xUp, enableDoItYourself, planarDome, focalPoint, out prongParam.TopDome, out prongParam.MiddleProng, out prongParam.LowerDome, out var outCurves, out var outProngs, out error))
			{
				return false;
			}
			prongParam.OutCurves.AddRange(outCurves);
			prongParam.OutProngs.AddRange(outProngs);
		}
		return true;
	}
}
