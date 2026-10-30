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
		bool grewSmaller = false;
		AdjustGemPosEnd(inChannels, null, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, direction, positionStartGem, positionEndGem, vOrientationMethod, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, xTrans, ref grewSmaller, ref gemList);
	}

	public static void AdjustGemPosEnd(CSShankGemParam inChannels, Curve profile, double profileWidth, double profileWidthMiddle, double profileWidthEnd, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double placementStart, double placementMiddle, double placementEnd, double direction, double positionStartGem, double positionEndGem, ValueListItem vOrientationMethod, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, Transform xTrans, ref bool grewSmaller, ref List<UltimateGemData> gemList)
	{
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
		bool grewSmaller = false;
		AdjustGemPosEndMirror(inChannels, null, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, direction, positionStartGem, positionEndGem, vOrientationMethod, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, xTrans, ref grewSmaller, ref gemList, ref gemListMirror);
	}

	public static void AdjustGemPosEndMirror(CSShankGemParam inChannels, Curve profile, double profileWidth, double profileWidthMiddle, double profileWidthEnd, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double placementStart, double placementMiddle, double placementEnd, double direction, double positionStartGem, double positionEndGem, ValueListItem vOrientationMethod, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, Transform xTrans, ref bool grewSmaller, ref List<UltimateGemData> gemList, ref List<UltimateGemData> gemListMirror)
	{
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
			if ((gemPoint - gemPoint2).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - ultimateGemData.Spacing * 0.8 < 0.0)
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
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		profileWidth = inChannels.InProfileWidthStart;
		Point3d point3d = Point3d.Unset;
		Point3d point3d2 = Point3d.Unset;
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
					point3d = ultimateGemData2.GemPoint;
					point3d2 = point3d;
					num = ultimateGemData.SizeXRotated;
				}
				double length2 = (ultimateGemData.GemPoint - point3d).Length;
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
				point3d = ultimateGemData.GemPoint;
			}
			if (doMirror)
			{
				num6 = 999.0;
				num7 = 20;
				if (point3d2 == Point3d.Unset)
				{
					point3d2 = point3d;
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
							double length3 = (ultimateGemData2.GemPoint - point3d2).Length;
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
						point3d2 = ultimateGemData2.GemPoint;
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
				point3d = gemList[gemList.Count - 1].GemPoint;
				if ((gemPoint - point3d).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - gemList[gemList.Count - 1].Spacing < 0.0)
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
		if (rotated90)
		{
			rollAngle += Math.PI / 2.0;
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, true, false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(gemShape);
		cSGemData.ApplyTransform(transform * xBestScale);
		if (!(cSGemData.GetCurve() is NurbsCurve nurbsCurve))
		{
			xSizeOut = 0.0;
			return false;
		}
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint value = nurbsCurve.Points[i];
			value.Location = new Point3d(value.Location.X, value.Location.Y, 0.0);
			nurbsCurve.Points[i] = value;
		}
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		double y = boundingBox.Min.Y;
		double num = boundingBox.Max.Y - boundingBox.Min.Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), nurbsCurve, 0.001, 0.001);
			if (curveIntersections.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in curveIntersections)
			{
				Point3d pointA = item.PointA;
				if (pointA.X > num4)
				{
					num4 = pointA.X;
				}
				if (pointA.X < num3)
				{
					num3 = pointA.X;
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
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(inChannels, xTrans, vOrientationMethod, ultimateGemData, profile, profileWidth);
			Point3d origin = Point3d.Origin;
			origin.Transform(ultimateGemData.GemTransform);
			MoveAGem(inChannels, ultimateGemData, profile, positionStartGem, endError, vLen, profileWidth, vOrientationMethod, xTrans);
			Point3d origin2 = Point3d.Origin;
			origin2.Transform(ultimateGemData.GemTransform);
			double length = (origin2 - origin).Length;
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
			Point3d point3d = inChannels.InCurve.PointAtNormalizedLength(paramPosition);
			Point3d point3d2 = inChannels.InBaseCurve.PointAt(ultimateGemData.BaseParam);
			double length = (point3d - point3d2).Length;
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
		Point3d origin = Point3d.Origin;
		origin.Transform(firstGem.GemTransform);
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
			double length2 = (ultimateGemData.GemPoint - origin).Length;
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
		double num = gemOnCurveInfo.SizeX / inChannels.InGemShape.GemXSize;
		Transform transform;
		if (gemOnCurveInfo.SizeY > 0.0)
		{
			double yScaleFactor = gemOnCurveInfo.SizeY / inChannels.InGemShape.GemYSize;
			transform = Transform.Scale(Plane.WorldXY, num, yScaleFactor, num);
		}
		else
		{
			transform = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		CSGemData cSGemData = new CSGemData(inChannels.InGemShape);
		cSGemData.ApplyTransform(transform);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, inChannels.InPlacementMethod, ref zOffset);
		CalculatePointsOnCrv(inChannels.InCurve, inChannels.InBaseCurve, vOrientationMethod, gemOnCurveInfo);
		Point3d point3d = Point3d.Unset;
		if (inChannels.InBaseCurve != null)
		{
			point3d = inChannels.InBaseCurve.PointAt(gemOnCurveInfo.BaseParam);
			if (inChannels.InSideBuild)
			{
				point3d.Y = (inChannels.InFront ? (point3d.Y - gemOnCurveInfo.SizeY / 2.0) : (point3d.Y + gemOnCurveInfo.SizeY / 2.0));
			}
		}
		Point3d point3d2 = inChannels.InCurve.PointAt(gemOnCurveInfo.Param);
		if (inChannels.InSideBuild)
		{
			point3d2.Y = (inChannels.InFront ? (point3d2.Y - gemOnCurveInfo.SizeY / 2.0) : (point3d2.Y + gemOnCurveInfo.SizeY / 2.0));
		}
		Vector3d vector3d = inChannels.InCurve.TangentAt(gemOnCurveInfo.Param);
		double num2 = 0.0;
		double num3 = 0.0;
		if (inChannels.InChannelEnabled && profile != null)
		{
			double num4 = ProfileCut(inChannels.InChannelWallGemOverlap, profile, profileWidth, inChannels.InRotate90, gemOnCurveInfo);
			double length = (point3d2 - point3d).Length;
			num2 = length * (1.0 - num4);
			if ((inChannels.InUseMinCuletDistance == ValueListItem.MinCuletGrowUp || inChannels.InUseMinCuletDistance == ValueListItem.MinCuletShankGrowUp) && cSGemData.GemZSize > length - inChannels.InMinCuletDistance - num2)
			{
				num3 = (gemOnCurveInfo.MinCuletMovement = cSGemData.GemZSize - (length - inChannels.InMinCuletDistance - num2));
			}
			ProfilePositionScale(gemOnCurveInfo, inChannels.InCurve, inChannels.InBaseCurve, inChannels.InSideBuild, inChannels.InFront, (inChannels.InUseMinCuletDistance == ValueListItem.MinCuletShankGrowUp) ? num3 : 0.0, out var pScale, out var pRail);
			gemOnCurveInfo.ProfileCurve.Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Vector3d vector3d2 = (inChannels.InDoFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d vector3d3 = (inChannels.InDoFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal;
		switch (vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(inChannels.InBaseSurface, point3d2, ref brepNormal);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			brepNormal = new Vector3d(point3d2);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			brepNormal = new Vector3d(point3d2.X, 0.0, point3d2.Z);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			break;
		case ValueListItem.OrientationMethodClosestPointOnCurve:
		case ValueListItem.OrientationMethodIntersectPointOnCurve:
			brepNormal = ((point3d != Point3d.Unset) ? (point3d2 - point3d) : new Vector3d(point3d2));
			brepNormal.Unitize();
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			break;
		}
		brepNormal.Unitize();
		Transform transform2 = (inChannels.InOrientationFollowCurve ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d3, vector3d2, point3d2, vector3d, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d3, point3d2, brepNormal, vector3d));
		double num5 = zOffset + gemOnCurveInfo.ZPlacement - num2 + num3;
		Transform transform3 = Transform.Translation(brepNormal * num5);
		gemOnCurveInfo.GemTransform = transform3 * transform2 * xTrans * transform;
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem vOrientationMethod, UltimateGemData gem)
	{
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		crv.NormalizedLengthParameter(num, out gem.Param);
		Point3d point3d = crv.PointAt(gem.Param);
		Vector3d normal = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (vOrientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
		{
			baseCrv.ClosestPoint(point3d, out gem.BaseParam);
			return;
		}
		Plane plane = new Plane(point3d, normal);
		CurveIntersections curveIntersections = Intersection.CurvePlane(baseCrv, plane, 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			double length = (item.PointA - point3d).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private static void MoveAGem(CSShankGemParam inChannels, UltimateGemData gemPos, Curve profile, double positionStartGem, double endError, double gemRowLength, double profileWidth, ValueListItem vOrientationMethod, Transform xTrans)
	{
		double num = (gemPos.NormParam - positionStartGem) * endError / gemRowLength;
		gemPos.NormParam += num;
		CalculateGemXform(inChannels, xTrans, vOrientationMethod, gemPos, profile, profileWidth);
	}

	private static double ProfileCut(double channelWallGemOverlap, Curve profile, double profileWidth, bool rotated90, UltimateGemData gemOnCurveInfo)
	{
		double num = (rotated90 ? gemOnCurveInfo.SizeX : gemOnCurveInfo.SizeY);
		LineCurve curveB = new LineCurve(new Point3d((0.0 - num) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - num) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve curveB2 = new LineCurve(new Point3d(num / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(num / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform xform = Transform.Scale(Plane.WorldXY, profileWidth, 1.0, 1.0);
		gemOnCurveInfo.ProfileCurve.Transform(xform);
		CurveIntersections curveIntersections = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, curveB, 0.001, 0.001);
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, curveB2, 0.001, 0.001);
		double num2 = -1.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			Point3d pointA = item.PointA;
			if (pointA.Y > num2)
			{
				num2 = pointA.Y;
				gemOnCurveInfo.ParamLeft = item.ParameterA;
			}
		}
		double num3 = -1.0;
		foreach (IntersectionEvent item2 in curveIntersections2)
		{
			Point3d pointA2 = item2.PointA;
			if (pointA2.Y > num3)
			{
				num3 = pointA2.Y;
				gemOnCurveInfo.ParamRight = item2.ParameterA;
			}
		}
		return (num2 + num3) / 2.0;
	}

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, bool sideBuilt, bool inFront, double moveUp, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = crv.PointAt(gem.Param);
		if (sideBuilt)
		{
			point3d.Y = (inFront ? (point3d.Y - gem.SizeY / 2.0) : (point3d.Y + gem.SizeY / 2.0));
		}
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d point3d2 = baseCrv.PointAt(gem.BaseParam);
		if (sideBuilt)
		{
			point3d2.Y = (inFront ? (point3d2.Y - gem.SizeY / 2.0) : (point3d2.Y + gem.SizeY / 2.0));
		}
		Vector3d destination2 = point3d - point3d2;
		double yScaleFactor = destination2.Length + moveUp;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, yScaleFactor, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d2, destination2, destination);
	}
}
