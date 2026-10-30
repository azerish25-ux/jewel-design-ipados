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
[CSBlock("Cluster_V3", "{ADC1DC83-046A-4B68-9BCE-E8A21145E511}")]
public sealed class CSCluster3 : CSBlock
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

	public CSCluster3(string identifier)
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
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val = CSBlockUtil.CSUtilMath.Rotation(value5, Vector3d.ZAxis, Point3d.Origin);
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
			foreach (GemSize size in cSGemData2.Sizes)
			{
				double num5 = size.X / cSGemData2.GemXSize;
				double num6 = size.Y / cSGemData2.GemYSize;
				double num7 = (num5 + num6) / 2.0;
				Transform val2 = Transform.Scale(Plane.WorldXY, num5, num6, num7);
				CSDataHelper.GetNurbsCurve(cSGemData2, out var value17);
				((GeometryBase)value17).Transform(val * val2);
				BoundingBox boundingBox = ((GeometryBase)value17).GetBoundingBox(true);
				if (!((BoundingBox)(ref boundingBox)).IsValid)
				{
					return UpdateError("BoundingBox Failed");
				}
				Point3d val3 = ((BoundingBox)(ref boundingBox)).Max;
				double num8 = Math.Abs(((Point3d)(ref val3)).X);
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				double num9 = num8 + Math.Abs(((Point3d)(ref val3)).X);
				val3 = ((BoundingBox)(ref boundingBox)).Max;
				double num10 = Math.Abs(((Point3d)(ref val3)).Y);
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				double num11 = num10 + Math.Abs(((Point3d)(ref val3)).Y);
				double num12 = Math.Abs(num2 - num11);
				if (double.IsNaN(num4) || num12 < num4)
				{
					gemSize = size;
					gemDefinitionSizeX = num9;
					num3 = num11;
					num4 = num12;
				}
			}
			cSGemData2.ScaleToSize(gemSize.X, gemSize.Y);
		}
		double num13 = cSGemData2.GemXSize / 2.0 - cSGemData2.GemXMax;
		double num14 = cSGemData2.GemYSize / 2.0 - cSGemData2.GemYMax;
		Transform val4 = Transform.Translation(num13, num14, 0.0);
		Transform val5 = Transform.Translation(0.0, 0.0, 0.0 - Math.Abs(cSGemData2.GemZMax));
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(value4, Vector3d.XAxis, Point3d.Origin);
		Transform xTransformLocal = Transform.Translation(0.0, 0.0, value6) * val6 * val5 * val4 * val;
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
		((Vector3d)(ref v0)).Unitize();
		((Vector3d)(ref v1)).Unitize();
		return ((Vector3d)(ref v0)).X * ((Vector3d)(ref v1)).X + ((Vector3d)(ref v0)).Y * ((Vector3d)(ref v1)).Y;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtEnd = curveLhs.PointAtEnd;
		Point3d pointAtStart = curveRhs.PointAtStart;
		if (((Point3d)(ref pointAtEnd)).DistanceTo(pointAtStart) > 0.001)
		{
			return false;
		}
		LineCurve val = new LineCurve(pointAtEnd, pointAtStart);
		Interval domain = ((Curve)val).Domain;
		Point3d destination = ((Curve)val).PointAt(((Interval)(ref domain)).Mid);
		domain = curveLhs.Domain;
		Vector3d val2 = curveLhs.TangentAt(((Interval)(ref domain)).Max);
		domain = curveRhs.Domain;
		Vector3d val3 = curveRhs.TangentAt(((Interval)(ref domain)).Min);
		Vector3d destination2 = val2 + val3;
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, destination, destination2, centerGem.Up);
		CSData data = gemDefinition.Duplicate();
		CSDataHelper.Transform(data, val4 * xTransformLocal);
		outputGems.Append(data);
		return true;
	}

	private static bool PlaceGemCorners(IList<Curve> curves, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, CSDataArray outputGems)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
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
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, val * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsEmerald(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 9)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve obj = val3.DuplicateCurve();
		Interval domain = obj.Domain;
		double min = ((Interval)(ref domain)).Min;
		domain = val3.Domain;
		Curve val4 = obj.Trim(new Interval(min, ((Interval)(ref domain)).Mid));
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
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0), gemDefinition.Normal, pointAtStart, destination, centerGem.Normal);
		CSData data = gemDefinition.Duplicate();
		CSDataHelper.Transform(data, val4 * xOrientLocal);
		outputGems.Append(data);
	}

	private static bool PlaceGemsHeart(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 6)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[3]
		{
			array[0],
			array[1],
			array[2]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[3]
		{
			array[3],
			array[4],
			array[5]
		}, out Curve mergedCrv2, 0.01))
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
		if (!PlaceGemCorners((IList<Curve>)(object)new Curve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquise(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 2)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = array[3];
		Curve[] array3 = (Curve[])(object)new Curve[2] { val4, val };
		Curve[] array4 = (Curve[])(object)new Curve[2] { val2, val3 };
		Curve[] array5 = Curve.JoinCurves((IEnumerable<Curve>)array3);
		if (array5 == null)
		{
			return false;
		}
		Curve val5 = array5[0];
		array5 = Curve.JoinCurves((IEnumerable<Curve>)array4);
		if (array5 == null)
		{
			return false;
		}
		Curve val6 = array5[0];
		if (!PlaceGemsCurveSide(val5, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(val6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemCorners((IList<Curve>)(object)new Curve[2] { val5, val6 }, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsPear(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			array[0],
			array[1]
		});
		if (array3 == null)
		{
			return false;
		}
		Curve val = array3[0];
		array3 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			array[2],
			array[3]
		});
		if (array3 == null)
		{
			return false;
		}
		Curve val2 = array3[0];
		if (!PlaceGemsCurveSide(val, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(val2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemCorners((IList<Curve>)(object)new Curve[2] { val, val2 }, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsRound(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double fixPosition, bool isEven, ref CSDataArray outputGems)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		if (!GemOffsetCurveV2.GemOffset(centerGemShape, centerGem, offsetRailX, out Curve curve, doTransformUp: true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		((GeometryBase)curve).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
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
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, val * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsSquare(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, ref CSDataArray outputGems)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 5)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = array[3];
		Curve val5 = array[4];
		Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val, val5 });
		if (array3 == null)
		{
			return false;
		}
		Curve val6 = array3[0];
		if (!PlaceGemsCurveSide(val6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(val2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(val3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(val4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !PlaceGemCorners((IList<Curve>)(object)new Curve[4] { val6, val2, val3, val4 }, centerGem, gemDefinition, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsSquareRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, ref CSDataArray outputGems)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurveV2.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(new Vector3d(centerGem.Normal) * offsetRailZ);
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
		if (style == 0 && !PlaceGemCorners((IList<Curve>)(object)new Curve[4]
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
