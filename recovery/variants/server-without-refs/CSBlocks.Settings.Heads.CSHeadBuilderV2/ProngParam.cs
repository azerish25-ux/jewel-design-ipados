using System.Collections.Generic;
using CS.Parametric.Channels;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

public sealed class ProngParam
{
	public Curve ProngRail;

	public List<Curve> OutCurves;

	public List<Brep> OutProngs;

	public List<Brep> TopDome;

	public List<Brep> MiddleProng;

	public List<Brep> LowerDome;

	public int ProngType { get; private set; }

	public int ProngPoint { get; private set; }

	public int ProngArrangement { get; private set; }

	public double ProngOverlap { get; private set; }

	public double ProngPointAngle { get; private set; }

	public double ProngTopX { get; private set; }

	public double ProngTopY { get; private set; }

	public double ProngTopNudge { get; private set; }

	public double ProngTopBlend { get; private set; }

	public double ProngBaseX { get; private set; }

	public double ProngBaseY { get; private set; }

	public double ProngBaseNudge { get; private set; }

	public double ProngBaseBlend { get; private set; }

	public double ProngDomeHeight { get; private set; }

	public double ProngDomeSettingDistance { get; private set; }

	public double ProngDomeSettingAngle { get; private set; }

	public double ProngBaseDomeHeight { get; private set; }

	public Curve ProngTopProfile { get; private set; }

	public Curve ProngBaseProfile { get; private set; }

	public double DomeBlendAmountInside { get; private set; }

	public double DomeBlendAmountOutside { get; private set; }

	public double DomeBlendStartInside { get; private set; }

	public double DomeBlendStartOutside { get; private set; }

	public double DomeBlendEndInside { get; private set; }

	public double DomeBlendEndOutside { get; private set; }

	public bool StraightProng { get; private set; }

	public static ProngParam[] SetProngParams(CSInputChannel inProngType, CSInputChannel inProngPoint, CSInputChannel inProngArrangement, CSInputChannel inProngOverlap, CSInputChannel inProngPointAngle, CSInputChannel inProngTopX, CSInputChannel inProngTopY, CSInputChannel inProngTopNudge, CSInputChannel inProngTopBlend, CSInputChannel inProngBaseX, CSInputChannel inProngBaseY, CSInputChannel inProngBaseNudge, CSInputChannel inProngBaseBlend, CSInputChannel inProngDomeHeight, CSInputChannel inProngDomeSettingDistance, CSInputChannel inProngDomeSettingAngle, CSInputChannel inProngBaseDomeHeight, CSInputChannel inProngTopProfile, CSInputChannel inProngBaseProfile, CSInputChannel inDomeBlendAmountInside, CSInputChannel inDomeBlendAmountOutside, CSInputChannel inDomeBlendStartInside, CSInputChannel inDomeBlendStartOutside, CSInputChannel inDomeBlendEndInside, CSInputChannel inDomeBlendEndOutside, CSInputChannel inStraightProng)
	{
		List<int> integerList = inProngType.IntegerList;
		List<int> integerList2 = inProngPoint.IntegerList;
		List<int> integerList3 = inProngArrangement.IntegerList;
		List<double> doubleList = inProngOverlap.DoubleList;
		List<double> doubleList2 = inProngPointAngle.DoubleList;
		List<double> doubleList3 = inProngTopX.DoubleList;
		List<double> doubleList4 = inProngTopY.DoubleList;
		List<double> doubleList5 = inProngTopNudge.DoubleList;
		List<double> doubleList6 = inProngTopBlend.DoubleList;
		List<double> doubleList7 = inProngBaseX.DoubleList;
		List<double> doubleList8 = inProngBaseY.DoubleList;
		List<double> doubleList9 = inProngBaseNudge.DoubleList;
		List<double> doubleList10 = inProngBaseBlend.DoubleList;
		List<double> doubleList11 = inProngDomeHeight.DoubleList;
		List<double> doubleList12 = inProngDomeSettingDistance.DoubleList;
		List<double> doubleList13 = inProngDomeSettingAngle.DoubleList;
		List<double> doubleList14 = inProngBaseDomeHeight.DoubleList;
		List<Curve> curveList = inProngTopProfile.CurveList;
		List<Curve> curveList2 = inProngBaseProfile.CurveList;
		List<double> doubleList15 = inDomeBlendAmountInside.DoubleList;
		List<double> doubleList16 = inDomeBlendAmountOutside.DoubleList;
		List<double> doubleList17 = inDomeBlendStartInside.DoubleList;
		List<double> doubleList18 = inDomeBlendStartOutside.DoubleList;
		List<double> doubleList19 = inDomeBlendEndInside.DoubleList;
		List<double> doubleList20 = inDomeBlendEndOutside.DoubleList;
		List<bool> booleanList = inStraightProng.BooleanList;
		int count = integerList.Count;
		ProngParam[] array = new ProngParam[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new ProngParam
			{
				ProngType = integerList[i],
				ProngPoint = integerList2[i],
				ProngArrangement = integerList3[i],
				ProngOverlap = doubleList[i],
				ProngPointAngle = doubleList2[i],
				ProngTopX = doubleList3[i],
				ProngTopY = doubleList4[i],
				ProngTopNudge = doubleList5[i],
				ProngTopBlend = doubleList6[i],
				ProngBaseX = doubleList7[i],
				ProngBaseY = doubleList8[i],
				ProngBaseNudge = doubleList9[i],
				ProngBaseBlend = doubleList10[i],
				ProngDomeHeight = doubleList11[i],
				ProngDomeSettingDistance = doubleList12[i],
				ProngDomeSettingAngle = doubleList13[i],
				ProngBaseDomeHeight = doubleList14[i],
				ProngTopProfile = curveList[i],
				ProngBaseProfile = curveList2[i],
				DomeBlendAmountInside = doubleList15[i],
				DomeBlendAmountOutside = doubleList16[i],
				DomeBlendStartInside = doubleList17[i],
				DomeBlendStartOutside = doubleList18[i],
				DomeBlendEndInside = doubleList19[i],
				DomeBlendEndOutside = doubleList20[i],
				StraightProng = booleanList[i]
			};
		}
		return array;
	}
}
