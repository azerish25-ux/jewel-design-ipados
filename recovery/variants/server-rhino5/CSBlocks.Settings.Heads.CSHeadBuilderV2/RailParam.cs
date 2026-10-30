using System.Collections.Generic;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

public sealed class RailParam
{
	public double RailPos;

	public bool RailUpperToggle;

	public double RailX;

	public double RailY;

	public double RailOffset;

	public double RailShearMultiplier;

	public CSGemData RailGem;

	public Curve RailProfile;

	public bool RailMove;

	public bool RailPullToRingRail;

	public List<Brep> RailObjects;

	public List<Curve> RailCurves;

	public static RailParam[] SetRailParam(CSInputChannel inRailPos, CSInputChannel inRailUpperToggle, CSInputChannel inRailX, CSInputChannel inRailY, CSInputChannel inRailOffset, CSInputChannel inRailShearMultiplier, CSInputChannel inRailGem, CSInputChannel inRailProfile, CSInputChannel inRailMove, CSInputChannel inRailPullToRingRail)
	{
		List<double> doubleList = inRailPos.DoubleList;
		List<bool> booleanList = inRailUpperToggle.BooleanList;
		List<double> doubleList2 = inRailX.DoubleList;
		List<double> doubleList3 = inRailY.DoubleList;
		List<double> doubleList4 = inRailOffset.DoubleList;
		List<double> doubleList5 = inRailShearMultiplier.DoubleList;
		List<CSGemData> gemList = inRailGem.GemList;
		List<Curve> curveList = inRailProfile.CurveList;
		List<bool> booleanList2 = inRailMove.BooleanList;
		List<bool> booleanList3 = inRailPullToRingRail.BooleanList;
		int count = doubleList.Count;
		RailParam[] array = new RailParam[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new RailParam
			{
				RailPos = doubleList[i],
				RailUpperToggle = booleanList[i],
				RailX = doubleList2[i],
				RailY = doubleList3[i],
				RailOffset = doubleList4[i],
				RailShearMultiplier = doubleList5[i],
				RailGem = gemList[i],
				RailProfile = curveList[i],
				RailMove = booleanList2[i],
				RailPullToRingRail = booleanList3[i]
			};
		}
		return array;
	}
}
