using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.CSClusterGemShapes;

public static class CSClusterGemUtils
{
	public static void GemCurve(Curve crv, Curve lastCrv, CSGemData gemDef, double gemSize, double gemSpacing, int startType, int endType, double startSpacing, double endSpacing, int cornerMetalType, Vector3d GemUp, Transform xTransLocal, ref CSDataArray outputGems)
	{
		double length = crv.GetLength();
		double num = length;
		int num2 = 0;
		double num3 = 1.0;
		double num4 = 0.0;
		if (startType != 0 && endType != 0)
		{
			num -= startSpacing / 2.0 + endSpacing / 2.0;
			num2 = (int)(num / (gemSize + gemSpacing));
			num3 = num / (double)num2;
			num4 = endSpacing / 2.0 + num3 / 2.0;
		}
		else if (startType == 0 && endType == 0)
		{
			num2 = (int)(num / (gemSize + gemSpacing));
			num3 = num / (double)num2;
			num4 = 0.0;
		}
		else if (startType == 0 && endType != 0)
		{
			num -= endSpacing / 2.0;
			num2 = (int)(num / ((gemSize + gemSpacing) / 2.0));
			if (num2 % 2 == 0)
			{
				num2--;
			}
			num3 = num / (double)num2;
			num2 = (num2 + 1) / 2;
			num4 = 0.0;
			num3 *= 2.0;
		}
		else if (startType != 0 && endType == 0)
		{
			num -= startSpacing / 2.0;
			num2 = (int)(num / ((gemSize + gemSpacing) / 2.0));
			if (num2 % 2 == 0)
			{
				num2--;
			}
			num3 = num / (double)num2;
			num2 = (num2 + 1) / 2 - 1;
			num4 = startSpacing / 2.0 + num3;
			num3 *= 2.0;
		}
		if (startType == 2)
		{
			Vector3d destination = crv.TangentAtStart + lastCrv.TangentAtEnd;
			destination.Unitize();
			Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDef.Up, crv.PointAtStart, destination, GemUp);
			CSGemData cSGemData = (CSGemData)gemDef.Duplicate();
			Transform transform2 = Transform.Scale(Plane.WorldXY, startSpacing / cSGemData.GemXSize, 1.0, 1.0);
			cSGemData.SetUserString("Corner", cornerMetalType.ToString());
			cSGemData.SetUserString("IsBead", "1");
			CSDataHelper.Transform(cSGemData, transform * xTransLocal * transform2);
			outputGems.Append(cSGemData);
		}
		double[] array = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			double num5 = num4 / length;
			array[i] = ((num5 > 1.0) ? 1.0 : num5);
			num4 += num3;
		}
		double[] array2 = crv.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = 0; j < num2; j++)
		{
			CSGemData cSGemData2 = (CSGemData)gemDef.Duplicate();
			Vector3d destination2;
			if (startType == 0 && j == 0)
			{
				destination2 = crv.TangentAtStart + lastCrv.TangentAtEnd;
				destination2.Unitize();
				cSGemData2.SetUserString("Corner", cornerMetalType.ToString());
			}
			else
			{
				destination2 = crv.TangentAt(array2[j]);
				cSGemData2.SetUserString("Corner", "0");
			}
			Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDef.Up, crv.PointAt(array2[j]), destination2, GemUp);
			CSDataHelper.Transform(cSGemData2, transform3 * xTransLocal);
			outputGems.Append(cSGemData2);
		}
	}

	public static bool PlaceGemCorner(Curve curveLhs, Curve curveRhs, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, double angle, CSDataArray outputGems)
	{
		Point3d pointAtEnd = curveLhs.PointAtEnd;
		Point3d pointAtStart = curveRhs.PointAtStart;
		if (pointAtEnd.DistanceTo(pointAtStart) > 0.001)
		{
			return false;
		}
		LineCurve lineCurve = new LineCurve(pointAtEnd, pointAtStart);
		Point3d destination = lineCurve.PointAt(lineCurve.Domain.Mid);
		Vector3d vector3d = curveLhs.TangentAt(curveLhs.Domain.Max);
		Vector3d vector3d2 = curveRhs.TangentAt(curveRhs.Domain.Min);
		Vector3d destination2 = vector3d + vector3d2;
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, destination, destination2, centerGem.Up);
		CSData data = gemDefinition.Duplicate();
		Vector3d up = centerGem.Up;
		up.Unitize();
		up = up * Math.Sin(angle) / 2.0;
		Transform transform2 = Transform.Translation(up);
		CSDataHelper.Transform(data, transform * xTransformLocal * transform2);
		outputGems.Append(data);
		return true;
	}

	public static bool PlaceGemCorners(IList<Curve> curves, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, CSDataArray outputGems, double angle)
	{
		for (int i = 0; i < curves.Count; i++)
		{
			int num = i;
			int num2 = num + 1;
			if (num2 >= curves.Count)
			{
				num2 -= curves.Count;
			}
			if (!PlaceGemCorner(curves[num], curves[num2], centerGem, gemDefinition, xTransformLocal, angle, outputGems))
			{
				return false;
			}
		}
		return true;
	}

	public static bool PlaceGemsCurveSide(Curve curve, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, int style, Transform xTransformLocal, CSDataArray outputGems)
	{
		double num = curve.GetLength();
		if (style == 1 && centerGem.GemShape == "Princess")
		{
			num -= 3.0 * gemDefinitionSizeX / 7.0;
		}
		int num2 = (int)(num / (gemDefinitionSizeX + spacing));
		if (num < (double)num2 * gemDefinitionSizeX + (double)Math.Max(num2 - 1, 0) * spacing)
		{
			num2--;
		}
		double[] array = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = GetUnnormalizedParam(i, num2, style);
		}
		double[] array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = ((style == 0) ? 1 : 0); j < num2; j++)
		{
			Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, transform * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static double GetUnnormalizedParam(int gemCurrentIndex, int gemCount, int style)
	{
		if (style == 0)
		{
			return (double)gemCurrentIndex / (double)gemCount;
		}
		double num = 1.0 / (2.0 * Math.Sqrt(2.0) + (double)(2 * (gemCount - 1)));
		return num * Math.Sqrt(2.0) + num * 2.0 * (double)gemCurrentIndex;
	}
}
