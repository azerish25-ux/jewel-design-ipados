using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.CSClusterGemShapes;

public static class CSClusterGemEmerald
{
	public static bool PlaceGems(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double optionalEmeraldCornerXPercent, double optionalEmeraldCornerYPercent, ref CSDataArray outputGems)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		if (inCluster.InProngCornerUse)
		{
			return PlaceGemsProngs(inCluster, centerGemShape, centerGem, gemDefinition, gemDefinitionSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, 0.0, ref outputGems);
		}
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true, 0.0, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent);
		if (array.Length != 9)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = val3.DuplicateCurve();
		Curve obj = val4;
		Interval domain = val4.Domain;
		double min = ((Interval)(ref domain)).Min;
		domain = val3.Domain;
		val4 = obj.Trim(new Interval(min, ((Interval)(ref domain)).Mid));
		double cosAng = CosAng(val.PointAtStart - val.PointAtEnd, val2.PointAtEnd - val2.PointAtStart);
		double cosAng2 = CosAng(val2.PointAtStart - val2.PointAtEnd, val4.PointAtEnd - val4.PointAtStart);
		double length = val.GetLength();
		double length2 = val2.GetLength();
		double length3 = val4.GetLength();
		double num = 0.0;
		bool flag = true;
		int num2 = -1;
		double num3;
		double num4;
		int num5;
		double side;
		int num6;
		double side2;
		int num7;
		double num8;
		do
		{
			num3 = gemDefinitionSizeX + spacing + num;
			num4 = 0.0;
			num5 = (int)((length - num4) / num3);
			double a = length - (num4 + num3 * (double)num5);
			side = GetSide(cosAng, a, num3);
			num6 = (int)((length2 - side) / num3);
			double a2 = length2 - (side + num3 * (double)num6);
			side2 = GetSide(cosAng2, a2, num3);
			num7 = (int)((length3 - side2) / num3);
			num8 = length3 - (side2 + num3 * (double)num7);
			if (flag)
			{
				num2 = ((num8 > num3 / 2.0) ? 1 : 0);
				if (num2 == 1)
				{
					num8 -= num3 / 2.0;
				}
				int num9 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num9 * 0.9;
				flag = false;
				continue;
			}
			double num10 = num8 / num3;
			if (num10 < 0.25)
			{
				int num11 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num11 * 0.9;
			}
			else if (num10 < 0.75)
			{
				num8 -= num3 / 2.0;
				int num12 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num12 * 0.9;
			}
			else
			{
				num8 -= num3;
				num7++;
				int num13 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num13 * 0.9;
			}
		}
		while (num8 > 0.01);
		double num14 = length3 + num3 * (1.0 - (double)num2 / 2.0);
		int num15 = num7 - 1 + num2;
		for (int j = 0; j <= num5; j++)
		{
			PlaceGemsEmeraldSide(array, 0, gemDefinitionSizeX, num4 + (double)j * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int k = 0; k <= num6; k++)
		{
			PlaceGemsEmeraldSide(array, 1, gemDefinitionSizeX, side + (double)k * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int l = 0; l <= num7; l++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, side2 + (double)l * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int m = 0; m <= num15; m++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, num14 + (double)m * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num16 = num6; num16 >= 0; num16--)
		{
			PlaceGemsEmeraldSide(array, 3, gemDefinitionSizeX, length2 - (side + (double)num16 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num17 = num5; num17 >= 0; num17--)
		{
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, length - (num4 + (double)num17 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int n = 1; n <= num5; n++)
		{
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, length + (num4 + (double)n * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num18 = 0; num18 <= num6; num18++)
		{
			PlaceGemsEmeraldSide(array, 5, gemDefinitionSizeX, side + (double)num18 * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num19 = num15; num19 >= 0; num19--)
		{
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, length3 * 2.0 - (num14 + (double)num19 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num20 = num7; num20 >= 0; num20--)
		{
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, length3 * 2.0 - (side2 + (double)num20 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num21 = num6; num21 >= 0; num21--)
		{
			PlaceGemsEmeraldSide(array, 7, gemDefinitionSizeX, length2 - (side + (double)num21 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num22 = num5; num22 >= 1; num22--)
		{
			PlaceGemsEmeraldSide(array, 8, gemDefinitionSizeX, length - (num4 + (double)num22 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		return true;
	}

	private static double CosAng(Vector3d v0, Vector3d v1)
	{
		((Vector3d)(ref v0)).Unitize();
		((Vector3d)(ref v1)).Unitize();
		return ((Vector3d)(ref v0)).X * ((Vector3d)(ref v1)).X + ((Vector3d)(ref v0)).Y * ((Vector3d)(ref v1)).Y;
	}

	private static void GemCurve(Curve crv, CSGemData gemDef, double gemSize, double gemSpacing, double startSpacing, double endSpacing, Vector3d GemUp, Transform xTransLocal, ref CSDataArray outputGems)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		double length = crv.GetLength();
		double num = length - (startSpacing / 2.0 + endSpacing / 2.0);
		int num2 = (int)(num / (gemSize + gemSpacing));
		double num3 = num / (double)num2;
		double num4 = endSpacing / 2.0 + num3 / 2.0;
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
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDef.Up, crv.PointAt(array2[j]), crv.TangentAt(array2[j]), GemUp);
			CSGemData cSGemData = (CSGemData)gemDef.Duplicate();
			if (j == 0 || j == num2 - 1)
			{
				cSGemData.SetUserString("Corner", "2");
			}
			else
			{
				cSGemData.SetUserString("Corner", "0");
			}
			CSDataHelper.Transform(cSGemData, val * xTransLocal);
			outputGems.Append(cSGemData);
		}
	}

	private static double GetSide(double cosAng, double a, double c)
	{
		double num = -2.0 * a * cosAng;
		double num2 = 0.0 - c * c + a * a;
		return (0.0 - num + Math.Sqrt(num * num - 4.0 * num2)) / 2.0;
	}

	private static void PlaceGemsEmeraldSide(IList<Curve> curves, int crvIndex, double sizeX, double distance, CSGemData centerGem, CSGemData gemDefinition, Transform xOrientLocal, ref CSDataArray outputGems)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtStart = curves[crvIndex].PointAtStart;
		Vector3d val = curves[crvIndex].PointAtEnd - pointAtStart;
		((Vector3d)(ref val)).Unitize();
		Vector3d destination = default(Vector3d);
		((Vector3d)(ref destination))._002Ector(val);
		double num = sizeX / 2.0;
		if (distance < num)
		{
			int num2 = crvIndex - 1;
			if (num2 == -1)
			{
				num2 = curves.Count - 1;
			}
			Vector3d val2 = curves[num2].PointAtEnd - curves[num2].PointAtStart;
			((Vector3d)(ref val2)).Unitize();
			double num3 = distance;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			double num4 = num3 / num;
			double num5 = 1.0 - num4;
			destination = val * num4 + val2 * num5;
		}
		else
		{
			double length = curves[crvIndex].GetLength();
			if (length - distance < num)
			{
				int index = (crvIndex + 1) % curves.Count;
				Vector3d val3 = curves[index].PointAtEnd - curves[index].PointAtStart;
				((Vector3d)(ref val3)).Unitize();
				double num6 = length - distance;
				if (num6 < 0.0)
				{
					num6 = 0.0;
				}
				double num7 = num6 / num;
				double num8 = 1.0 - num7;
				destination = val * num7 + val3 * num8;
			}
		}
		pointAtStart += val * distance;
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Normal, pointAtStart, destination, centerGem.Normal);
		CSData data = gemDefinition.Duplicate();
		CSDataHelper.Transform(data, val4 * xOrientLocal);
		outputGems.Append(data);
	}

	private static bool PlaceGemsProngs(CSClusterParam inCluster, string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double cornerBlend, ref CSDataArray outputGems)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves("Princess", centerGem, offsetRailX, doTransformUp: true, cornerBlend);
		if (array.Length != 5)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		Curve[] array3 = (Curve[])(object)new Curve[4]
		{
			default(Curve),
			array[1],
			array[2],
			array[3]
		};
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			array[0],
			array[4]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		array3[0] = mergedCrv;
		array = array3;
		for (int j = 0; j < array.Length; j++)
		{
			GemCurve(array[j], gemDefinition, gemDefinitionSizeX, spacing, inCluster.InProngCornerSizing[j], inCluster.InProngCornerSizing[(j + 1) % array.Length], centerGem.Up, xTransformLocal, ref outputGems);
		}
		return true;
	}
}
