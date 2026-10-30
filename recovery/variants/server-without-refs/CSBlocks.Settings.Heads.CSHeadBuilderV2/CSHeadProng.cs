using System.Collections.Generic;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadProng
{
	public static bool Prong(int prongIndex, CSGemData gem, ProngParam[] prongParams, Curve headTop, Curve headBase, Curve bezelProngLine, ValueListItem baseStyle, double basePointSize, bool enableDoItYourself, bool planarDome, Point3d focalPoint, double cornerBlend, out string error)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
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
		double paramTop = default(double);
		headTop.ClosestPoint(prongRail.PointAtStart, ref paramTop);
		double paramBase = default(double);
		headBase.ClosestPoint(prongRail.PointAtEnd, ref paramBase);
		double prongBaseOffset = (0.0 - prongParam.ProngBaseY) * prongParam.ProngBaseNudge;
		double prongTopOffset = (0.0 - prongParam.ProngTopY) * prongParam.ProngTopNudge;
		if (baseStyle == ValueListItem.HeadBuilderProngHead)
		{
			prongTopOffset = 0.0;
			prongBaseOffset = 0.0;
			Point3d pointAtStart = prongRail.PointAtStart;
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(((Point3d)(ref pointAtStart)).X, ((Point3d)(ref pointAtStart)).Y, 0.0);
			((Point3d)(ref pointAtStart)).Transform(Transform.Translation(val));
			headTop.ClosestPoint(pointAtStart, ref paramTop);
			Point3d pointAtEnd = prongRail.PointAtEnd;
			((Vector3d)(ref val))._002Ector(((Point3d)(ref pointAtEnd)).X, ((Point3d)(ref pointAtEnd)).Y, 0.0);
			((Point3d)(ref pointAtEnd)).Transform(Transform.Translation(val));
			headBase.ClosestPoint(pointAtEnd, ref paramBase);
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
				((GeometryBase)outProng).Transform(xUp);
				prongParam.OutProngs.Add(outProng);
			}
		}
		else
		{
			if (!CSHeadProngProfileSweep.ProngProfileSweep(prongIndex, prongParams, headTop, paramTop, prongTopOffset, headBase, paramBase, prongBaseOffset, baseStyle, basePointSize, xUp, enableDoItYourself, planarDome, focalPoint, out prongParam.TopDome, out prongParam.MiddleProng, out prongParam.LowerDome, out var outCurves, out var outProngs, out error))
			{
				return false;
			}
			prongParam.OutCurves.AddRange(outCurves);
			prongParam.OutProngs.AddRange(outProngs);
		}
		return true;
	}
}
