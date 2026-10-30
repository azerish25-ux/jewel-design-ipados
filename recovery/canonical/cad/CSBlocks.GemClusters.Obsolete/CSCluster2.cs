using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using CSGemInstance.GemTableData;
using Rhino;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("Cluster_V2", "{EC9A8E23-4617-4C6D-8497-5B735AF4EA95}")]
public sealed class CSCluster2 : CSBlock
{
	public readonly CSInputChannel iCenterGem;

	public readonly CSInputChannel iCenterGemShapeOverrideEnabled;

	public readonly CSInputChannel iCenterGemShapeOverride;

	public readonly CSInputChannel iCenterGemSpacing;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iSpacing;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iRotation;

	public readonly CSInputChannel iPlacement;

	public readonly CSInputChannel iFixPosition;

	public readonly CSInputChannel iStyle;

	public readonly CSInputChannel iEven;

	public readonly CSInputChannel iProfileWidth;

	public readonly CSInputChannel iChannelWallThicknessInner;

	public readonly CSInputChannel iChannelWallThicknessOuter;

	public readonly CSInputChannel iChannelSpacing;

	public readonly CSInputChannel iShowChannel;

	public readonly CSInputChannel iGemHeightOffset;

	public readonly CSInputChannel iXYScaleToGem;

	public readonly CSOutputChannel oGem;

	public readonly CSOutputChannel oGemSizeZ;

	public CSCluster2(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		iCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFixPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEven = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iGemHeightOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
		oGemSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool value = true;
		iShowChannel.GetBoolean(ref value);
		if (!value)
		{
			oGem.SetData(null);
			oGemSizeZ.SetData(null);
			return base.Update();
		}
		CSDataArray flattenArray = iCenterGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem");
		}
		CSGemData cSGemData = iCenterGemShapeOverride.Data as CSGemData;
		if (!(iGemDefinition.Data is CSGemData cSGemData2))
		{
			return UpdateMissing("Gem Definition");
		}
		double value2 = 0.5;
		iCenterGemSpacing.GetDouble(ref value2);
		double value3 = 0.0;
		iSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		iAngle.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		double value5 = 0.0;
		iRotation.GetDouble(ref value5);
		value5 = CSBlockUtil.CSUtilMath.ToRadians(value5);
		double value6 = 0.0;
		iPlacement.GetDouble(ref value6);
		double value7 = 0.0;
		iFixPosition.GetDouble(ref value7);
		double value8 = 0.0;
		iProfileWidth.GetDouble(ref value8);
		double value9 = 0.0;
		iChannelWallThicknessInner.GetDouble(ref value9);
		double value10 = 0.0;
		iChannelWallThicknessOuter.GetDouble(ref value10);
		double value11 = 0.0;
		iChannelSpacing.GetDouble(ref value11);
		int value12 = 0;
		iStyle.GetInteger(ref value12);
		bool value13 = false;
		iCenterGemShapeOverrideEnabled.GetBoolean(ref value13);
		bool value14 = false;
		iEven.GetBoolean(ref value14);
		double value15 = 0.0;
		iGemHeightOffset.GetDouble(ref value15);
		bool value16 = false;
		iXYScaleToGem.GetBoolean(ref value16);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value5, Vector3d.ZAxis, Point3d.Origin);
		double num = value8 - (value9 + value10);
		double num2 = num - value11 * 2.0;
		double gemDefinitionSizeX = double.NaN;
		double num3 = double.NaN;
		if (num == 0.0)
		{
			gemDefinitionSizeX = cSGemData2.GemXSize;
			num3 = cSGemData2.GemYSize;
		}
		else
		{
			GemSize gemSize = null;
			double num4 = double.NaN;
			foreach (GemSize size in cSGemData2.Def.Sizes)
			{
				double num5 = size.X / cSGemData2.GemXSize;
				double num6 = size.Y / cSGemData2.GemYSize;
				double zScaleFactor = (num5 + num6) / 2.0;
				Transform transform2 = Transform.Scale(Plane.WorldXY, num5, num6, zScaleFactor);
				CSDataHelper.GetNurbsCurve(cSGemData2, out var value17);
				value17.Transform(transform * transform2);
				BoundingBox boundingBox = value17.GetBoundingBox(accurate: true);
				if (!boundingBox.IsValid)
				{
					return UpdateError("BoundingBox Failed");
				}
				double num7 = Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox.Min.X);
				double num8 = Math.Abs(boundingBox.Max.Y) + Math.Abs(boundingBox.Min.Y);
				double num9 = Math.Abs(num2 - num8);
				if (double.IsNaN(num4) || num9 < num4)
				{
					gemSize = size;
					gemDefinitionSizeX = num7;
					num3 = num8;
					num4 = num9;
				}
			}
			cSGemData2.ScaleToSize(gemSize.X, gemSize.Y);
		}
		double dx = cSGemData2.GemXSize / 2.0 - cSGemData2.GemXMax;
		double dy = cSGemData2.GemYSize / 2.0 - cSGemData2.GemYMax;
		Transform transform3 = Transform.Translation(dx, dy, 0.0);
		Transform transform4 = Transform.Translation(0.0, 0.0, 0.0 - Math.Abs(cSGemData2.GemZMax));
		Transform transform5 = CSBlockUtil.CSUtilMath.Rotation(value4, Vector3d.XAxis, Point3d.Origin);
		Transform xTransformLocal = Transform.Translation(0.0, 0.0, value6) * transform5 * transform4 * transform3 * transform;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData3))
			{
				continue;
			}
			string gemShape;
			if (value13)
			{
				if (cSGemData == null)
				{
					cSGemData = new CSGemData(cSGemData3);
				}
				cSGemData.Xform = cSGemData3.Xform;
				gemShape = cSGemData.GemShape;
			}
			else
			{
				cSGemData = new CSGemData(cSGemData3);
				gemShape = cSGemData3.GemShape;
			}
			double offsetRailX;
			double offsetRailZ;
			if (num == 0.0)
			{
				offsetRailX = value2 + num3 / 2.0 - value15 * Math.Sin(value4);
				offsetRailZ = 0.0 + value15 * Math.Cos(value4);
			}
			else
			{
				offsetRailX = value2 + (value9 + num / 2.0) * Math.Cos(value4) - value15 * Math.Sin(value4);
				offsetRailZ = (value9 + num / 2.0) * Math.Sin(value4) + value15 * Math.Cos(value4);
			}
			CSGemData centerGem = (value16 ? cSGemData3 : cSGemData);
			CSDataArray outputGems = new CSDataArray();
			switch (gemShape)
			{
			case "Cushion":
			case "Cushion Rotated 90":
			case "Oval":
			case "Oval Rotated 90":
			case "Round":
				if (!PlaceGemsRound(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, value7, value14, ref outputGems))
				{
					continue;
				}
				break;
			case "Cushion Square":
			case "Princess":
			case "Baguette Straight":
				if (!PlaceGemsSquare(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, value12, ref outputGems))
				{
					continue;
				}
				break;
			case "Cushion Square Rotated 45":
			case "Princess Rotated 45":
				if (!PlaceGemsSquareRotated(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, value12, ref outputGems))
				{
					continue;
				}
				break;
			case "Emerald":
			case "Emerald Rotated 90":
			case "Emerald Square":
			case "Emerald Square Rotated 45":
			case "Octagon":
			case "Radiant":
			case "Radiant Square":
				if (!PlaceGemsEmerald(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
				{
					continue;
				}
				break;
			case "Heart":
				if (!PlaceGemsHeart(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
				{
					continue;
				}
				break;
			case "Marquise":
				if (!PlaceGemsMarquise(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
				{
					continue;
				}
				break;
			case "Pear":
				if (!PlaceGemsPear(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
				{
					continue;
				}
				break;
			case "Marquise Rotated 90":
				if (!PlaceGemsMarquiseRotated(gemShape, centerGem, cSGemData2, gemDefinitionSizeX, value3, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
				{
					continue;
				}
				break;
			default:
				RhinoApp.Write("Unknown gem space for cluster " + gemShape + "\n");
				continue;
			}
			cSDataArray.Append(outputGems);
		}
		oGem.SetData(cSDataArray);
		oGemSizeZ.SetData(new CSNumberData(cSGemData2.GemZSize));
		return base.Update();
	}

	private static double CosAng(Vector3d v0, Vector3d v1)
	{
		v0.Unitize();
		v1.Unitize();
		return v0.X * v1.X + v0.Y * v1.Y;
	}

	private static double GetSide(double cosAng, double a, double c)
	{
		double num = 1.0;
		double num2 = -2.0 * a * cosAng;
		double num3 = 0.0 - c * c + a * a;
		return (0.0 - num2 + Math.Sqrt(num2 * num2 - 4.0 * num * num3)) / (2.0 * num);
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

	private static bool PlaceGemCorner(Curve curveLhs, Curve curveRhs, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, CSDataArray outputGems)
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
		CSDataHelper.Transform(data, transform * xTransformLocal);
		outputGems.Append(data);
		return true;
	}

	private static bool PlaceGemCorners(IList<Curve> curves, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, CSDataArray outputGems)
	{
		for (int i = 0; i < curves.Count; i++)
		{
			int num = i;
			int num2 = num + 1;
			if (num2 >= curves.Count)
			{
				num2 -= curves.Count;
			}
			if (!PlaceGemCorner(curves[num], curves[num2], centerGem, gemDefinition, xTransformLocal, outputGems))
			{
				return false;
			}
		}
		return true;
	}

	private static bool PlaceGemsCurveSide(Curve curve, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, int style, Transform xTransformLocal, CSDataArray outputGems)
	{
		double length = curve.GetLength();
		if (length < 0.0)
		{
			return false;
		}
		int num = (int)(length / gemDefinitionSizeX);
		if (length < (double)num * gemDefinitionSizeX + (double)Math.Max(num - 1, 0) * spacing)
		{
			num--;
		}
		double[] array = new double[num];
		double[] array2 = new double[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = GetUnnormalizedParam(i, num, style);
		}
		array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = ((style == 0) ? 1 : 0); j < num; j++)
		{
			Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, transform * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsEmerald(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 9)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = curve3.DuplicateCurve();
		curve4 = curve4.Trim(new Interval(curve4.Domain.Min, curve3.Domain.Mid));
		double cosAng = CosAng(curve.PointAtStart - curve.PointAtEnd, curve2.PointAtEnd - curve2.PointAtStart);
		double cosAng2 = CosAng(curve2.PointAtStart - curve2.PointAtEnd, curve4.PointAtEnd - curve4.PointAtStart);
		double length = curve.GetLength();
		double length2 = curve2.GetLength();
		double length3 = curve4.GetLength();
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
		for (int j = 0; j <= num5; j++)
		{
			PlaceGemsEmeraldSide(array, 0, gemDefinitionSizeX, num4 + (double)j * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, length - (num4 + (double)j * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int k = 1; k <= num5; k++)
		{
			PlaceGemsEmeraldSide(array, 8, gemDefinitionSizeX, length - (num4 + (double)k * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, length + (num4 + (double)k * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int l = 0; l <= num6; l++)
		{
			PlaceGemsEmeraldSide(array, 1, gemDefinitionSizeX, side + (double)l * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
			PlaceGemsEmeraldSide(array, 3, gemDefinitionSizeX, length2 - (side + (double)l * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
			PlaceGemsEmeraldSide(array, 5, gemDefinitionSizeX, side + (double)l * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
			PlaceGemsEmeraldSide(array, 7, gemDefinitionSizeX, length2 - (side + (double)l * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int m = 0; m <= num7; m++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, side2 + (double)m * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, length3 * 2.0 - (side2 + (double)m * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		double num14 = length3 + num3 * (1.0 - (double)num2 / 2.0);
		int num15 = num7 - 1 + num2;
		for (int n = 0; n <= num15; n++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, num14 + (double)n * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, length3 * 2.0 - (num14 + (double)n * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		return true;
	}

	private static void PlaceGemsEmeraldSide(IList<Curve> curves, int crvIndex, double sizeX, double distance, CSGemData centerGem, CSGemData gemDefinition, Transform xOrientLocal, ref CSDataArray outputGems)
	{
		Point3d pointAtStart = curves[crvIndex].PointAtStart;
		Vector3d vector3d = curves[crvIndex].PointAtEnd - pointAtStart;
		vector3d.Unitize();
		Vector3d destination = new Vector3d(vector3d);
		double num = sizeX / 2.0;
		if (distance < num)
		{
			int num2 = crvIndex - 1;
			if (num2 == -1)
			{
				num2 = curves.Count - 1;
			}
			Vector3d vector3d2 = curves[num2].PointAtEnd - curves[num2].PointAtStart;
			vector3d2.Unitize();
			double num3 = distance;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			double num4 = num3 / num;
			double num5 = 1.0 - num4;
			destination = vector3d * num4 + vector3d2 * num5;
		}
		else
		{
			double length = curves[crvIndex].GetLength();
			if (length - distance < num)
			{
				int index = (crvIndex + 1) % curves.Count;
				Vector3d vector3d3 = curves[index].PointAtEnd - curves[index].PointAtStart;
				vector3d3.Unitize();
				double num6 = length - distance;
				if (num6 < 0.0)
				{
					num6 = 0.0;
				}
				double num7 = num6 / num;
				double num8 = 1.0 - num7;
				destination = vector3d * num7 + vector3d3 * num8;
			}
		}
		pointAtStart += vector3d * distance;
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0), gemDefinition.Normal, pointAtStart, destination, centerGem.Normal);
		CSData data = gemDefinition.Duplicate();
		CSDataHelper.Transform(data, transform * xOrientLocal);
		outputGems.Append(data);
	}

	private static bool PlaceGemsHeart(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 6)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[3]
		{
			array[0],
			array[1],
			array[2]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (mergedCrv == null)
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[3]
		{
			array[3],
			array[4],
			array[5]
		}, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		if (mergedCrv2 == null)
		{
			return false;
		}
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return PlaceGemCorners(new Curve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, outputGems);
	}

	private static bool PlaceGemsMarquise(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 2)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		if (!PlaceGemsCurveSide(curve, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(curve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemCorners(array, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquiseRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		Curve[] inputCurves = new Curve[2] { curve4, curve };
		Curve[] inputCurves2 = new Curve[2] { curve2, curve3 };
		Curve[] array3 = Curve.JoinCurves(inputCurves);
		if (array3 == null)
		{
			return false;
		}
		Curve curve5 = array3[0];
		array3 = Curve.JoinCurves(inputCurves2);
		if (array3 == null)
		{
			return false;
		}
		Curve curve6 = array3[0];
		if (!PlaceGemsCurveSide(curve5, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(curve6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemCorners(new Curve[2] { curve5, curve6 }, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsPear(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve[] array3 = Curve.JoinCurves(new Curve[2]
		{
			array[0],
			array[1]
		});
		if (array3 == null)
		{
			return false;
		}
		Curve curve = array3[0];
		array3 = Curve.JoinCurves(new Curve[2]
		{
			array[2],
			array[3]
		});
		if (array3 == null)
		{
			return false;
		}
		Curve curve2 = array3[0];
		if (!PlaceGemsCurveSide(curve, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(curve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemCorners(new Curve[2] { curve, curve2 }, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsRound(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double fixPosition, bool isEven, ref CSDataArray outputGems)
	{
		if (!GemOffsetCurveV2.GemOffset(centerGemShape, centerGem, offsetRailX, out Curve curve, true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		curve.Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		double length = curve.GetLength();
		if (length < 0.0)
		{
			return false;
		}
		int num = (int)(length / (gemDefinitionSizeX + spacing));
		if (isEven && num % 2 == 1)
		{
			num--;
		}
		double[] array = new double[num];
		double[] array2 = new double[num];
		double num2 = 1.0 / (double)num;
		double num3 = fixPosition % num2;
		for (int i = 0; i < num; i++)
		{
			array[i] = num3;
			num3 += num2;
			if (num3 > 1.0)
			{
				num3 -= 1.0;
			}
		}
		Array.Sort(array);
		array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = 0; j < num; j++)
		{
			Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, transform * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsSquare(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 5)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		Curve curve5 = array[4];
		Curve curve6 = Curve.JoinCurves(new Curve[2] { curve, curve5 })[0];
		if (!PlaceGemsCurveSide(curve6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(curve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(curve3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(curve4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !PlaceGemCorners(new Curve[4] { curve6, curve2, curve3, curve4 }, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsSquareRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		if (!PlaceGemsCurveSide(array[0], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(array[1], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(array[2], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(array[3], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !PlaceGemCorners(new Curve[4]
		{
			array[0],
			array[1],
			array[2],
			array[3]
		}, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}
}
