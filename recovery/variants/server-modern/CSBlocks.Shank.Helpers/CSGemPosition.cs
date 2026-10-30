using System;
using System.Collections.Generic;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.Helpers;

internal static class CSGemPosition
{
	public static void AdjustGemPosEnd(CSShankGemParam inChannels, double positionStartGem, double positionEndGem, Transform xTrans, ValueListItem vOrientationMethod, ref List<UltimateGemData> gemList, ref int direction)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		bool grewSmaller = false;
		AdjustGemPosEnd(inChannels, null, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, direction, positionStartGem, positionEndGem, vOrientationMethod, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, xTrans, ref grewSmaller, ref gemList);
	}

	public static void AdjustGemPosEnd(CSShankGemParam inChannels, Curve profile, double profileWidth, double profileWidthMiddle, double profileWidthEnd, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double placementStart, double placementMiddle, double placementEnd, double direction, double positionStartGem, double positionEndGem, ValueListItem vOrientationMethod, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, Transform xTrans, ref bool grewSmaller, ref List<UltimateGemData> gemList)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count <= 1)
		{
			return;
		}
		double paramPosition = positionEndGem;
		if (inChannels.InChannelClosed)
		{
			CalcLastLoopingGemPos(inChannels, gemList[0], gemList[gemList.Count - 1], positionEndGem, xTrans, 0.0 - direction, vOrientationMethod, profile, profileWidth, out paramPosition);
			if (Math.Abs(positionEndGem - paramPosition) > 0.001)
			{
				double num = (paramPosition - gemList[gemList.Count - 1].NormParam) / (double)(gemList.Count - 1);
				paramPosition -= num;
			}
		}
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = paramPosition - normParam;
		double num2 = normParam - positionStartGem;
		for (int i = 0; i < gemList.Count; i++)
		{
			AdjustEndErrorSoGemSpacingLessGemTollerance(inChannels, gemList, profile, num2, i, profileWidth, positionStartGem, vOrientationMethod, xTrans, ref endError);
			CalcGemSize(inChannels, profile, gemList[i].NormParam, i, profileWidthMiddle, profileWidthEnd, gemRotatedStartXSize, gemRotatedMiddleXSize, gemRotatedEndXSize, placementStart, placementMiddle, placementEnd, positionStartGem, positionEndGem, vOrientationMethod, xStart, xMiddle, xEnd, yStart, yMiddle, yEnd, ref grewSmaller, out profileWidth);
			MoveAGem(inChannels, gemList[i], profile, positionStartGem, endError, num2, profileWidth, vOrientationMethod, xTrans);
		}
	}

	public static void AdjustGemPosEndMirror(CSShankGemParam inChannels, double positionStartGem, double positionEndGem, Transform xTrans, int direction, ValueListItem vOrientationMethod, ref List<UltimateGemData> gemList, ref List<UltimateGemData> gemListMirror)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		bool grewSmaller = false;
		AdjustGemPosEndMirror(inChannels, null, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, direction, positionStartGem, positionEndGem, vOrientationMethod, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, xTrans, ref grewSmaller, ref gemList, ref gemListMirror);
	}

	public static void AdjustGemPosEndMirror(CSShankGemParam inChannels, Curve profile, double profileWidth, double profileWidthMiddle, double profileWidthEnd, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double placementStart, double placementMiddle, double placementEnd, double direction, double positionStartGem, double positionEndGem, ValueListItem vOrientationMethod, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, Transform xTrans, ref bool grewSmaller, ref List<UltimateGemData> gemList, ref List<UltimateGemData> gemListMirror)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count <= 1 && gemListMirror.Count <= 0)
		{
			return;
		}
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = positionEndGem - normParam;
		double num = normParam - positionStartGem;
		AdjustEndErrorSoGemSpacingLessGemTollerance(inChannels, gemList, profile, num, gemList.Count - 1, profileWidth, positionStartGem, vOrientationMethod, xTrans, ref endError);
		double endError2 = endError;
		double gemRowLength = num;
		if (gemList.Count > 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			UltimateGemData ultimateGemData2 = gemListMirror[gemListMirror.Count - 1].Duplicate();
			MoveAGem(inChannels, ultimateGemData, profile, positionStartGem, endError, num, profileWidth, vOrientationMethod, xTrans);
			MoveAGem(inChannels, ultimateGemData2, profile, positionStartGem, endError2, gemRowLength, profileWidth, vOrientationMethod, xTrans);
			Point3d gemPoint = ultimateGemData.GemPoint;
			Point3d gemPoint2 = ultimateGemData2.GemPoint;
			Vector3d val = gemPoint - gemPoint2;
			if (((Vector3d)(ref val)).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - ultimateGemData.Spacing * 0.8 < 0.0)
			{
				endError = positionStartGem + 0.5 * direction - normParam;
				num = normParam - positionStartGem;
				endError2 = endError;
				gemRowLength = num;
				gemList.RemoveAt(gemList.Count - 1);
			}
		}
		for (int i = 0; i < gemList.Count; i++)
		{
			CalcGemSize(inChannels, profile, gemList[i].NormParam, i, profileWidthMiddle, profileWidthEnd, gemRotatedStartXSize, gemRotatedMiddleXSize, gemRotatedEndXSize, placementStart, placementMiddle, placementEnd, positionStartGem, positionEndGem, vOrientationMethod, xStart, xMiddle, xEnd, yStart, yMiddle, yEnd, ref grewSmaller, out profileWidth);
			MoveAGem(inChannels, gemList[i], profile, positionStartGem, endError, num, profileWidth, vOrientationMethod, xTrans);
		}
		for (int j = 0; j < gemListMirror.Count; j++)
		{
			CalcGemSize(inChannels, profile, gemListMirror[j].NormParam, j, profileWidthMiddle, profileWidthEnd, gemRotatedStartXSize, gemRotatedMiddleXSize, gemRotatedEndXSize, placementStart, placementMiddle, placementEnd, positionStartGem, positionEndGem, vOrientationMethod, xStart, xMiddle, xEnd, yStart, yMiddle, yEnd, ref grewSmaller, out profileWidth);
			MoveAGem(inChannels, gemListMirror[j], profile, positionStartGem, endError2, gemRowLength, profileWidth, vOrientationMethod, xTrans);
		}
	}

	public static void CalcGemPos(CSShankGemParam inChannels, double paramPosition, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, bool doMirror, bool mirrorModeEven, double positionStartGem, double positionEndGem, Transform xTrans, int direction, ValueListItem vOrientationMethod, Curve profile, double profileWidthMiddle, double profileWidthEnd, double placementStart, double placementMiddle, double placementEnd, ref bool grewSmaller, out List<UltimateGemData> gemList, out List<UltimateGemData> gemListMirror, out double profileWidth)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		profileWidth = inChannels.InProfileWidthStart;
		Point3d val = Point3d.Unset;
		Point3d val2 = Point3d.Unset;
		double num = 0.0;
		double length = inChannels.InCurve.GetLength();
		double num2 = paramPosition;
		double num3 = paramPosition;
		bool flag = false;
		int num4 = 0;
		while (true)
		{
			bool num5;
			if (inChannels.InGemCountToggle)
			{
				num5 = inChannels.InGemCount > (double)num4;
			}
			else
			{
				if (!(paramPosition * (double)direction < positionEndGem * (double)direction))
				{
					break;
				}
				num5 = !flag;
			}
			if (!num5)
			{
				break;
			}
			double num6 = 999.0;
			int num7 = 20;
			UltimateGemData ultimateGemData = null;
			UltimateGemData ultimateGemData2 = null;
			Vector3d val3;
			while (num6 > 0.01 && num7 > 0)
			{
				num7--;
				ultimateGemData = CalcGemSize(inChannels, profile, paramPosition, num4, profileWidthMiddle, profileWidthEnd, gemRotatedStartXSize, gemRotatedMiddleXSize, gemRotatedEndXSize, placementStart, placementMiddle, placementEnd, positionStartGem, positionEndGem, vOrientationMethod, xStart, xMiddle, xEnd, yStart, yMiddle, yEnd, ref grewSmaller, out profileWidth);
				CalculateGemXform(inChannels, xTrans, vOrientationMethod, ultimateGemData, profile, profileWidth);
				if (gemList.Count == 0)
				{
					if (!(doMirror && mirrorModeEven))
					{
						num6 = 0.0;
						continue;
					}
					ultimateGemData2 = new UltimateGemData
					{
						NormParam = num3 - (paramPosition - num3),
						SizeX = ultimateGemData.SizeX,
						SizeY = ultimateGemData.SizeY,
						SizeZ = ultimateGemData.SizeZ,
						ZPlacement = ultimateGemData.ZPlacement,
						Spacing = ultimateGemData.Spacing,
						SizeXRotated = ultimateGemData.SizeXRotated
					};
					CalculateGemXform(inChannels, xTrans, vOrientationMethod, ultimateGemData2, profile, profileWidth);
					val = ultimateGemData2.GemPoint;
					val2 = val;
					num = ultimateGemData.SizeXRotated;
				}
				val3 = ultimateGemData.GemPoint - val;
				double length2 = ((Vector3d)(ref val3)).Length;
				length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - num / 2.0;
				num6 = Math.Abs(length2 - ultimateGemData.Spacing);
				if (!(num6 > 0.01))
				{
					continue;
				}
				double num8 = (ultimateGemData.Spacing - length2) * (double)direction;
				if (doMirror && mirrorModeEven && gemList.Count == 0)
				{
					num8 /= 2.0;
				}
				paramPosition += num8 / length * 0.8;
				if (!inChannels.InCurve.IsClosed)
				{
					if (paramPosition > 1.0)
					{
						paramPosition = 1.0;
					}
					if (paramPosition < 0.0)
					{
						paramPosition = 0.0;
					}
				}
			}
			if (ultimateGemData != null)
			{
				gemList.Add(ultimateGemData);
				num4++;
				val = ultimateGemData.GemPoint;
			}
			if (doMirror)
			{
				num6 = 999.0;
				num7 = 20;
				if (val2 == Point3d.Unset)
				{
					val2 = val;
				}
				if (gemList.Count <= 1)
				{
					num6 = 0.0;
				}
				if (inChannels.InCurve.IsClosed || (num2 < 1.01 && num2 > -0.01))
				{
					if (!inChannels.InCurve.IsClosed)
					{
						if (num2 > 1.0)
						{
							num2 = 1.0;
						}
						if (num2 < 0.0)
						{
							num2 = 0.0;
						}
					}
					while (num6 > 0.01 && num7 > 0)
					{
						num7--;
						if (ultimateGemData != null)
						{
							ultimateGemData2 = new UltimateGemData
							{
								NormParam = num2,
								SizeX = ultimateGemData.SizeX,
								SizeY = ultimateGemData.SizeY,
								SizeZ = ultimateGemData.SizeZ,
								ZPlacement = ultimateGemData.ZPlacement,
								Spacing = ultimateGemData.Spacing,
								SizeXRotated = ultimateGemData.SizeXRotated,
								vY = ultimateGemData.vY
							};
						}
						CalculateGemXform(inChannels, xTrans, vOrientationMethod, ultimateGemData2, profile, profileWidth);
						if (ultimateGemData2 != null)
						{
							val3 = ultimateGemData2.GemPoint - val2;
							double length3 = ((Vector3d)(ref val3)).Length;
							length3 = length3 - ultimateGemData2.SizeXRotated / 2.0 - num / 2.0;
							num6 = Math.Abs(length3 - ultimateGemData2.Spacing);
							if (!(num6 > 0.01))
							{
								continue;
							}
							double num9 = (ultimateGemData2.Spacing - length3) * (double)direction;
							num2 -= num9 / length * 0.8;
						}
						if (!inChannels.InCurve.IsClosed)
						{
							if (num2 > 1.0)
							{
								num2 = 1.0;
							}
							if (num2 < 0.0)
							{
								num2 = 0.0;
							}
						}
					}
					if (ultimateGemData2 != null)
					{
						gemListMirror.Add(ultimateGemData2);
						val2 = ultimateGemData2.GemPoint;
					}
				}
			}
			if (ultimateGemData != null)
			{
				num = ultimateGemData.SizeXRotated;
				double num10 = num + ultimateGemData.Spacing;
				paramPosition += num10 / length * (double)direction;
				num2 -= num10 / length * (double)direction;
			}
			if (!doMirror && inChannels.InChannelClosed && gemList.Count >= 3)
			{
				Point3d gemPoint = gemList[0].GemPoint;
				val = gemList[gemList.Count - 1].GemPoint;
				val3 = gemPoint - val;
				if (((Vector3d)(ref val3)).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - gemList[gemList.Count - 1].Spacing < 0.0)
				{
					gemList.RemoveAt(gemList.Count - 1);
					flag = true;
					num4 = (int)inChannels.InGemCount;
				}
			}
		}
	}

	public static bool CalculateRotateTestGem(CSGemData gemShape, double xSize, double ySize, double rollAngle, bool rotated90, out double xSizeOut)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		if (rotated90)
		{
			rollAngle += Math.PI / 2.0;
		}
		Transform val = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, true, false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(gemShape);
		cSGemData.ApplyTransform(val * xBestScale);
		Curve curve = cSGemData.GetCurve();
		NurbsCurve val2 = (NurbsCurve)(object)((curve is NurbsCurve) ? curve : null);
		if (val2 == null)
		{
			xSizeOut = 0.0;
			return false;
		}
		Point3d val4;
		for (int i = 0; i < val2.Points.Count; i++)
		{
			ControlPoint val3 = val2.Points[i];
			val4 = ((ControlPoint)(ref val3)).Location;
			double x = ((Point3d)(ref val4)).X;
			val4 = ((ControlPoint)(ref val3)).Location;
			((ControlPoint)(ref val3)).Location = new Point3d(x, ((Point3d)(ref val4)).Y, 0.0);
			val2.Points[i] = val3;
		}
		BoundingBox boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double y = ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double num = y2 - ((Point3d)(ref val4)).Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections val5 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), (Curve)(object)val2, 0.001, 0.001);
			if (val5.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in val5)
			{
				Point3d pointA = item.PointA;
				if (((Point3d)(ref pointA)).X > num4)
				{
					num4 = ((Point3d)(ref pointA)).X;
				}
				if (((Point3d)(ref pointA)).X < num3)
				{
					num3 = ((Point3d)(ref pointA)).X;
				}
			}
			if (num4 - num3 > num2)
			{
				num2 = num4 - num3;
			}
		}
		xSizeOut = num2;
		return true;
	}

	private static void AdjustEndErrorSoGemSpacingLessGemTollerance(CSShankGemParam inChannels, List<UltimateGemData> gemList, Curve profile, double vLen, int gemIndex, double profileWidth, double positionStartGem, ValueListItem vOrientationMethod, Transform xTrans, ref double endError)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(inChannels, xTrans, vOrientationMethod, ultimateGemData, profile, profileWidth);
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(ultimateGemData.GemTransform);
			MoveAGem(inChannels, ultimateGemData, profile, positionStartGem, endError, vLen, profileWidth, vOrientationMethod, xTrans);
			Point3d origin2 = Point3d.Origin;
			((Point3d)(ref origin2)).Transform(ultimateGemData.GemTransform);
			Vector3d val = origin2 - origin;
			double length = ((Vector3d)(ref val)).Length;
			int num = gemList.Count - 1;
			if (num == 0)
			{
				num = 1;
			}
			length /= (double)num;
			double num2 = 0.0;
			if (gemIndex == 0)
			{
				num2 = inChannels.InSpacingToleranceStart;
			}
			if (gemIndex > 0 && (double)gemIndex <= (double)(gemList.Count - 1) / 2.0)
			{
				num2 = inChannels.InSpacingToleranceStart * (1.0 - (double)gemIndex / ((double)gemList.Count - 1.0) / 2.0) + inChannels.InSpacingToleranceMiddle * ((double)gemIndex / ((double)gemList.Count - 1.0) / 2.0);
			}
			if ((double)gemIndex > (double)(gemList.Count - 1) / 2.0 && gemIndex < gemList.Count - 1)
			{
				num2 = inChannels.InSpacingToleranceMiddle * ((double)gemIndex / (double)(gemList.Count - 1) / (double)(gemList.Count - 1) / 2.0) + inChannels.InSpacingToleranceEnd * (double)(1 - gemIndex / (gemList.Count - 1) / (gemList.Count - 1) / 2);
			}
			if (gemIndex == gemList.Count - 1)
			{
				num2 = inChannels.InSpacingToleranceEnd;
			}
			if (length > num2)
			{
				endError *= num2 / length;
			}
		}
	}

	private static UltimateGemData CalcGemSize(CSShankGemParam inChannels, Curve profile, double paramPosition, double count, double profileWidthMiddle, double profileWidthEnd, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double placementStart, double placementMiddle, double placementEnd, double positionStartGem, double positionEndGem, ValueListItem vOrientationMethod, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, ref bool grewSmaller, out double profileWidth)
	{
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		UltimateGemData ultimateGemData = new UltimateGemData
		{
			NormParam = paramPosition
		};
		double value = (paramPosition - positionStartGem) / (positionEndGem - positionStartGem);
		value = Math.Abs(value);
		if (inChannels.InGemCountToggle)
		{
			value = count / (inChannels.InGemCount - 1.0);
		}
		double zPlacement;
		double num;
		double num2;
		double num3;
		double spacing;
		double num4;
		if (inChannels.InGemCountToggle ? (count < (inChannels.InGemCount - 1.0) / 2.0) : (value <= inChannels.InPositionMiddle))
		{
			value /= inChannels.InPositionMiddle;
			zPlacement = placementStart * (1.0 - value) + placementMiddle * value;
			num = (inChannels.InRotate90 ? (yStart * (1.0 - value) + yMiddle * value) : (xStart * (1.0 - value) + xMiddle * value));
			num2 = (inChannels.InRotate90 ? (xStart * (1.0 - value) + xMiddle * value) : (yStart * (1.0 - value) + yMiddle * value));
			num3 = gemRotatedStartXSize * (1.0 - value) + gemRotatedMiddleXSize * value;
			spacing = inChannels.InSpacingStart * (1.0 - value) + inChannels.InSpacingMiddle * value;
			num4 = inChannels.InProfileWidthStart * (1.0 - value) + profileWidthMiddle * value;
		}
		else
		{
			value = (value - inChannels.InPositionMiddle) / (1.0 - inChannels.InPositionMiddle);
			zPlacement = placementMiddle * (1.0 - value) + placementEnd * value;
			num = (inChannels.InRotate90 ? (yMiddle * (1.0 - value) + yEnd * value) : (xMiddle * (1.0 - value) + xEnd * value));
			num2 = (inChannels.InRotate90 ? (xMiddle * (1.0 - value) + xEnd * value) : (yMiddle * (1.0 - value) + yEnd * value));
			num3 = gemRotatedMiddleXSize * (1.0 - value) + gemRotatedEndXSize * value;
			spacing = inChannels.InSpacingMiddle * (1.0 - value) + inChannels.InSpacingEnd * value;
			num4 = profileWidthMiddle * (1.0 - value) + profileWidthEnd * value;
		}
		if (inChannels.InMetalWidthToggle)
		{
			profileWidth = num4;
		}
		else
		{
			profileWidth = (inChannels.InRotate90 ? (num + inChannels.InWallWidth * 2.0) : (num2 + inChannels.InWallWidth * 2.0));
		}
		Transform xBestScale;
		if (inChannels.InUseMinCuletDistance == ValueListItem.MinCuletGrowSmaller)
		{
			CalculatePointsOnCrv(inChannels.InCurve, inChannels.InBaseCurve, vOrientationMethod, ultimateGemData);
			Point3d val = inChannels.InCurve.PointAtNormalizedLength(paramPosition);
			Point3d val2 = inChannels.InBaseCurve.PointAt(ultimateGemData.BaseParam);
			Vector3d val3 = val - val2;
			double length = ((Vector3d)(ref val3)).Length;
			double num5 = length - inChannels.InMinCuletDistance;
			CSUtilGem.GetGemScaleFromTableWithMaxZ(inChannels.InGemShape, num, num2, num5, useLargestPossible: true, useIterationOnly: false, out xBestScale);
			if (inChannels.InChannelEnabled && profile != null)
			{
				if (!(inChannels.InGemShape.Duplicate() is CSGemData cSGemData))
				{
					return null;
				}
				cSGemData.ApplyTransform(xBestScale);
				ultimateGemData.SizeY = cSGemData.GemYSize;
				double num6 = ProfileCut(inChannels.InChannelWallGemOverlap, profile, profileWidth, inChannels.InRotate90, ultimateGemData);
				double num7 = length * (1.0 - num6);
				CSUtilGem.GetGemScaleFromTableWithMaxZ(inChannels.InGemShape, num, num2, num5 - num7, useLargestPossible: true, useIterationOnly: false, out xBestScale);
				if (!inChannels.InMetalWidthToggle)
				{
					profileWidth = (inChannels.InRotate90 ? (cSGemData.GemXSize + inChannels.InWallWidth * 2.0) : (cSGemData.GemYSize + inChannels.InWallWidth * 2.0));
				}
				if (num > cSGemData.GemXSize || num2 > cSGemData.GemYSize)
				{
					grewSmaller = true;
				}
			}
		}
		else
		{
			CSUtilGem.GetGemScaleFromTable(inChannels.InGemShape, num, num2, true, false, out xBestScale);
		}
		if (inChannels.InGemShape.Duplicate() is CSGemData cSGemData2)
		{
			cSGemData2.ApplyTransform(xBestScale);
			double num8 = (cSGemData2.GemXSize + cSGemData2.GemYSize) / (num + num2);
			ultimateGemData.SizeX = cSGemData2.GemXSize;
			ultimateGemData.SizeY = cSGemData2.GemYSize;
			ultimateGemData.SizeZ = cSGemData2.GemZSize;
			ultimateGemData.SizeXRotated = num3 * num8;
			ultimateGemData.ZPlacement = zPlacement;
			ultimateGemData.Spacing = spacing;
			ultimateGemData.vY = num2;
		}
		return ultimateGemData;
	}

	private static void CalcLastLoopingGemPos(CSShankGemParam inChannels, UltimateGemData firstGem, UltimateGemData lastGem, double neededGemPos, Transform xTrans, double direction, ValueListItem vOrientationMethod, Curve profile, double profileWidth, out double paramPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(firstGem.GemTransform);
		double length = inChannels.InCurve.GetLength();
		double num = 999.0;
		int num2 = 20;
		paramPosition = neededGemPos;
		bool flag = true;
		while (num > 0.01 && num2 > 0)
		{
			num2--;
			UltimateGemData ultimateGemData = new UltimateGemData
			{
				NormParam = paramPosition,
				SizeX = lastGem.SizeX,
				SizeY = lastGem.SizeY,
				SizeZ = lastGem.SizeZ,
				ZPlacement = lastGem.ZPlacement,
				Spacing = lastGem.Spacing,
				SizeXRotated = lastGem.SizeXRotated
			};
			CalculateGemXform(inChannels, xTrans, vOrientationMethod, ultimateGemData, profile, profileWidth);
			Vector3d val = ultimateGemData.GemPoint - origin;
			double length2 = ((Vector3d)(ref val)).Length;
			length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - firstGem.SizeXRotated / 2.0;
			if (flag)
			{
				if (length2 - ultimateGemData.Spacing > 0.0)
				{
					paramPosition = neededGemPos;
					break;
				}
				flag = false;
				paramPosition = lastGem.NormParam;
			}
			else
			{
				num = Math.Abs(length2 - ultimateGemData.Spacing);
				if (num > 0.01)
				{
					double num3 = (ultimateGemData.Spacing - length2) * direction;
					paramPosition += num3 / length * 0.8;
				}
			}
		}
	}

	private static void CalculateGemXform(CSShankGemParam inChannels, Transform xTrans, ValueListItem vOrientationMethod, UltimateGemData gemOnCurveInfo, Curve profile, double profileWidth)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		double num = gemOnCurveInfo.SizeX / inChannels.InGemShape.GemXSize;
		Transform val;
		if (gemOnCurveInfo.SizeY > 0.0)
		{
			double num2 = gemOnCurveInfo.SizeY / inChannels.InGemShape.GemYSize;
			val = Transform.Scale(Plane.WorldXY, num, num2, num);
		}
		else
		{
			val = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		CSGemData cSGemData = new CSGemData(inChannels.InGemShape);
		cSGemData.ApplyTransform(val);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, inChannels.InPlacementMethod, ref zOffset);
		CalculatePointsOnCrv(inChannels.InCurve, inChannels.InBaseCurve, vOrientationMethod, gemOnCurveInfo);
		Point3d val2 = Point3d.Unset;
		if (inChannels.InBaseCurve != null)
		{
			val2 = inChannels.InBaseCurve.PointAt(gemOnCurveInfo.BaseParam);
			if (inChannels.InSideBuild)
			{
				((Point3d)(ref val2)).Y = (inChannels.InFront ? (((Point3d)(ref val2)).Y - gemOnCurveInfo.SizeY / 2.0) : (((Point3d)(ref val2)).Y + gemOnCurveInfo.SizeY / 2.0));
			}
		}
		Point3d val3 = inChannels.InCurve.PointAt(gemOnCurveInfo.Param);
		if (inChannels.InSideBuild)
		{
			((Point3d)(ref val3)).Y = (inChannels.InFront ? (((Point3d)(ref val3)).Y - gemOnCurveInfo.SizeY / 2.0) : (((Point3d)(ref val3)).Y + gemOnCurveInfo.SizeY / 2.0));
		}
		Vector3d val4 = inChannels.InCurve.TangentAt(gemOnCurveInfo.Param);
		double num3 = 0.0;
		double num4 = 0.0;
		if (inChannels.InChannelEnabled && profile != null)
		{
			double num5 = ProfileCut(inChannels.InChannelWallGemOverlap, profile, profileWidth, inChannels.InRotate90, gemOnCurveInfo);
			Vector3d val5 = val3 - val2;
			double length = ((Vector3d)(ref val5)).Length;
			num3 = length * (1.0 - num5);
			if ((inChannels.InUseMinCuletDistance == ValueListItem.MinCuletGrowUp || inChannels.InUseMinCuletDistance == ValueListItem.MinCuletShankGrowUp) && cSGemData.GemZSize > length - inChannels.InMinCuletDistance - num3)
			{
				num4 = (gemOnCurveInfo.MinCuletMovement = cSGemData.GemZSize - (length - inChannels.InMinCuletDistance - num3));
			}
			ProfilePositionScale(gemOnCurveInfo, inChannels.InCurve, inChannels.InBaseCurve, inChannels.InSideBuild, inChannels.InFront, (inChannels.InUseMinCuletDistance == ValueListItem.MinCuletShankGrowUp) ? num4 : 0.0, out var pScale, out var pRail);
			((GeometryBase)gemOnCurveInfo.ProfileCurve).Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Vector3d val6 = (inChannels.InDoFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d val7 = (inChannels.InDoFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal = default(Vector3d);
		switch (vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(inChannels.InBaseSurface, val3, ref brepNormal);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			((Vector3d)(ref brepNormal))._002Ector(val3);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			((Vector3d)(ref brepNormal))._002Ector(((Point3d)(ref val3)).X, 0.0, ((Point3d)(ref val3)).Z);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			break;
		case ValueListItem.OrientationMethodClosestPointOnCurve:
		case ValueListItem.OrientationMethodIntersectPointOnCurve:
			brepNormal = (Vector3d)((val2 != Point3d.Unset) ? (val3 - val2) : new Vector3d(val3));
			((Vector3d)(ref brepNormal)).Unitize();
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			break;
		}
		((Vector3d)(ref brepNormal)).Unitize();
		Transform val8 = (inChannels.InOrientationFollowCurve ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val6, val3, val4, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val6, val7, val3, brepNormal, val4));
		double num6 = zOffset + gemOnCurveInfo.ZPlacement - num3 + num4;
		Transform val9 = Transform.Translation(brepNormal * num6);
		gemOnCurveInfo.GemTransform = val9 * val8 * xTrans * val;
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem vOrientationMethod, UltimateGemData gem)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		crv.NormalizedLengthParameter(num, ref gem.Param);
		Point3d val = crv.PointAt(gem.Param);
		Vector3d val2 = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (vOrientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
		{
			baseCrv.ClosestPoint(val, ref gem.BaseParam);
			return;
		}
		Plane val3 = default(Plane);
		((Plane)(ref val3))._002Ector(val, val2);
		CurveIntersections obj = Intersection.CurvePlane(baseCrv, val3, 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in obj)
		{
			Vector3d val4 = item.PointA - val;
			double length = ((Vector3d)(ref val4)).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private static void MoveAGem(CSShankGemParam inChannels, UltimateGemData gemPos, Curve profile, double positionStartGem, double endError, double gemRowLength, double profileWidth, ValueListItem vOrientationMethod, Transform xTrans)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		double num = (gemPos.NormParam - positionStartGem) * endError / gemRowLength;
		gemPos.NormParam += num;
		CalculateGemXform(inChannels, xTrans, vOrientationMethod, gemPos, profile, profileWidth);
	}

	private static double ProfileCut(double channelWallGemOverlap, Curve profile, double profileWidth, bool rotated90, UltimateGemData gemOnCurveInfo)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		double num = (rotated90 ? gemOnCurveInfo.SizeX : gemOnCurveInfo.SizeY);
		LineCurve val = new LineCurve(new Point3d((0.0 - num) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - num) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve val2 = new LineCurve(new Point3d(num / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(num / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform val3 = Transform.Scale(Plane.WorldXY, profileWidth, 1.0, 1.0);
		((GeometryBase)gemOnCurveInfo.ProfileCurve).Transform(val3);
		CurveIntersections obj = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, (Curve)(object)val, 0.001, 0.001);
		CurveIntersections val4 = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, (Curve)(object)val2, 0.001, 0.001);
		double num2 = -1.0;
		foreach (IntersectionEvent item in obj)
		{
			Point3d pointA = item.PointA;
			if (((Point3d)(ref pointA)).Y > num2)
			{
				num2 = ((Point3d)(ref pointA)).Y;
				gemOnCurveInfo.ParamLeft = item.ParameterA;
			}
		}
		double num3 = -1.0;
		foreach (IntersectionEvent item2 in val4)
		{
			Point3d pointA2 = item2.PointA;
			if (((Point3d)(ref pointA2)).Y > num3)
			{
				num3 = ((Point3d)(ref pointA2)).Y;
				gemOnCurveInfo.ParamRight = item2.ParameterA;
			}
		}
		return (num2 + num3) / 2.0;
	}

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, bool sideBuilt, bool inFront, double moveUp, out Transform pScale, out Transform pRail)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = crv.PointAt(gem.Param);
		if (sideBuilt)
		{
			((Point3d)(ref val)).Y = (inFront ? (((Point3d)(ref val)).Y - gem.SizeY / 2.0) : (((Point3d)(ref val)).Y + gem.SizeY / 2.0));
		}
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d val2 = baseCrv.PointAt(gem.BaseParam);
		if (sideBuilt)
		{
			((Point3d)(ref val2)).Y = (inFront ? (((Point3d)(ref val2)).Y - gem.SizeY / 2.0) : (((Point3d)(ref val2)).Y + gem.SizeY / 2.0));
		}
		Vector3d destination2 = val - val2;
		double num = ((Vector3d)(ref destination2)).Length + moveUp;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, num, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val2, destination2, destination);
	}
}
