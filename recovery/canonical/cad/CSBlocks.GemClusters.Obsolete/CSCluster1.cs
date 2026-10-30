using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("Cluster_V1", "{0968FA93-5A07-4E1F-AFE6-A6DF1F86CED7}")]
public sealed class CSCluster1 : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSInputChannel InGemDefinition { get; }

	public CSInputChannel InSpacing { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InRotation { get; }

	public CSInputChannel InSpacingFromCenter { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InFixPosition { get; }

	public CSInputChannel InStyle { get; }

	public CSInputChannel InEven { get; }

	public CSInputChannel InShape { get; }

	public CSInputChannel InChannelWidthOffset { get; }

	public CSInputChannel InChannelWidthInside { get; }

	public CSInputChannel InProfileWidth { get; }

	public CSOutputChannel OutGem { get; }

	public CSCluster1(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingFromCenter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFixPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStyle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEven = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InShape = new CSInputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		InChannelWidthOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWidthInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem");
		}
		CSGemData cSGemData = InShape.Data as CSGemData;
		string text = string.Empty;
		if (cSGemData != null)
		{
			text = cSGemData.GemShape;
		}
		if (!(InGemDefinition.Data is CSGemData gemDefinition))
		{
			return UpdateMissing("Gem Definition");
		}
		double value = 0.0;
		InSpacing.GetDouble(ref value);
		double value2 = 0.0;
		InAngle.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		double value3 = 0.0;
		InRotation.GetDouble(ref value3);
		value3 = CSBlockUtil.CSUtilMath.ToRadians(value3);
		double value4 = 0.5;
		InSpacingFromCenter.GetDouble(ref value4);
		double value5 = 0.0;
		InPlacement.GetDouble(ref value5);
		double value6 = 0.0;
		InFixPosition.GetDouble(ref value6);
		double value7 = 0.0;
		InChannelWidthOffset.GetDouble(ref value7);
		double value8 = 0.0;
		InChannelWidthInside.GetDouble(ref value8);
		int value9 = 0;
		InStyle.GetInteger(ref value9);
		bool value10 = false;
		InEven.GetBoolean(ref value10);
		value4 += value7 / 2.0 + value8;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = cSGemData2.GemShape;
			}
			CSDataArray outputGems = new CSDataArray();
			switch (text)
			{
			case "Cushion":
			case "Oval":
			case "Round":
				if (!PlaceGemsRound(cSGemData2, gemDefinition, text, value, value2, value3, value4, value5, value6, value10, ref outputGems))
				{
					continue;
				}
				break;
			case "Baguette Straight":
			case "Princess":
				if (!PlaceGemsSquare(cSGemData2, gemDefinition, text, value, value2, value3, value4, value5, value9, ref outputGems))
				{
					continue;
				}
				break;
			case "Marquise":
				if (!PlaceGems_Marquise(cSGemData2, gemDefinition, text, value, value2, value3, value4, value5, ref outputGems))
				{
					continue;
				}
				break;
			case "Pear":
				if (!PlaceGemsPear(cSGemData2, gemDefinition, text, value, value2, value3, value4, value5, value10, ref outputGems))
				{
					continue;
				}
				break;
			default:
				continue;
			}
			cSDataArray.Append(outputGems);
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
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

	private static bool PlaceGems_Marquise(CSGemData gem, CSGemData gemDefinition, string shape, double spacing, double angle, double rotation, double girdleSpacing, double placement, ref CSDataArray outputGems)
	{
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rotation, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angle, Vector3d.XAxis, Point3d.Origin);
		Transform xTransformLocal = Transform.Translation(0.0, 0.0, placement) * transform2 * transform;
		CSData cSData = gemDefinition.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return false;
		}
		double testGemSizeX = boundingBox.Max.X - boundingBox.Min.X;
		Curve[] array = GemOffsetCurveV1.GemOffsetCurves(shape, gem, girdleSpacing + Math.Abs(boundingBox.Min.Y), doTransformUp: true);
		if (array.Length != 2)
		{
			return false;
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		if (!PlaceGemsMarquiseSide(gem, gemDefinition, curve, testGemSizeX, spacing, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsMarquiseSide(gem, gemDefinition, curve2, testGemSizeX, spacing, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquiseSide(CSGemData gem, CSGemData gemDefinition, Curve curve, double testGemSizeX, double spacing, Transform xTransformLocal, CSDataArray outputGems)
	{
		double length = curve.GetLength();
		if (length < 0.0)
		{
			return false;
		}
		int num = (int)(length / testGemSizeX);
		if (length < (double)num * testGemSizeX + (double)Math.Max(num - 1, 0) * spacing)
		{
			num--;
		}
		Interval domain = curve.Domain;
		double[] array = new double[num];
		double[] array2 = new double[num];
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				array[i] = (double)i / (double)num;
			}
			array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		}
		for (int j = 0; j < num; j++)
		{
			Transform transform = ((!(array2[j] >= domain.Max)) ? ((!(array2[j] <= domain.Min)) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), gem.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gemDefinition.North, gemDefinition.Up, curve.PointAt(array2[j]), new Vector3d(gem.North), gem.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gemDefinition.North, gemDefinition.Up, curve.PointAt(array2[j]), gem.North, gem.Up));
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, transform * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsPear(CSGemData gem, CSGemData gemDefinition, string shape, double spacing, double angle, double rotation, double girdleSpacing, double placement, bool isEven, ref CSDataArray outputGems)
	{
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rotation, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angle, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, placement);
		CSData cSData = gemDefinition.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return false;
		}
		if (!GemOffsetCurveV1.GemOffset(shape, gem, girdleSpacing + Math.Abs(boundingBox.Min.Y), out Curve curve, true, out string _))
		{
			return false;
		}
		double length = curve.GetLength();
		if (length < 0.0)
		{
			return false;
		}
		int num = (int)(length / (boundingBox.Max.X - boundingBox.Min.X + spacing));
		if (isEven && num % 2 != 0)
		{
			num--;
		}
		double[] array = new double[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (double)i / (double)num;
		}
		double[] array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		Interval domain = curve.Domain;
		for (int j = 0; j < num; j++)
		{
			Transform transform4 = ((!(array2[j] >= domain.Max) && !(array2[j] <= domain.Min)) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), gem.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gemDefinition.North, gemDefinition.Up, curve.PointAt(array2[j]), -new Vector3d(gem.North), gem.Up));
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, transform4 * transform3 * transform2 * transform);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsRound(CSGemData gem, CSGemData gemDefinition, string shape, double spacing, double angle, double rotation, double girdleSpacing, double placement, double fixPosition, bool isEven, ref CSDataArray outputGems)
	{
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rotation, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angle, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, placement);
		CSData cSData = gemDefinition.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return false;
		}
		if (!GemOffsetCurveV1.GemOffset(shape, gem, girdleSpacing + Math.Abs(boundingBox.Min.Y), out Curve curve, true, out string _))
		{
			return false;
		}
		double length = curve.GetLength();
		if (length < 0.0)
		{
			return false;
		}
		int num = (int)(length / (boundingBox.Max.X - boundingBox.Min.X + spacing));
		if (isEven && num % 2 != 0)
		{
			num--;
		}
		double[] array = new double[num];
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
		double[] array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = 0; j < num; j++)
		{
			Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), gem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, transform4 * transform3 * transform2 * transform);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsSquare(CSGemData gem, CSGemData gemDefinition, string shape, double spacing, double angle, double rotation, double girdleSpacing, double placement, int style, ref CSDataArray outputGems)
	{
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rotation, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angle, Vector3d.XAxis, Point3d.Origin);
		Transform xTransformLocal = Transform.Translation(0.0, 0.0, placement) * transform2 * transform;
		CSData cSData = gemDefinition.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return false;
		}
		double testGemSizeX = boundingBox.Max.X - boundingBox.Min.X;
		Curve[] array = GemOffsetCurveV1.GemOffsetCurves(shape, gem, girdleSpacing + Math.Abs(boundingBox.Min.Y), doTransformUp: true);
		if (array.Length != 5)
		{
			return false;
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		Curve curve5 = array[4];
		Curve curve6 = Curve.JoinCurves(new Curve[2] { curve, curve5 })[0];
		if (!PlaceGemsSquareSide(gem, gemDefinition, curve6, testGemSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsSquareSide(gem, gemDefinition, curve2, testGemSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsSquareSide(gem, gemDefinition, curve3, testGemSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (!PlaceGemsSquareSide(gem, gemDefinition, curve4, testGemSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsSquareSide(CSGemData gem, CSGemData gemDefinition, Curve curve, double testGemSizeX, double spacing, int style, Transform xTransformLocal, CSDataArray outputGems)
	{
		double length = curve.GetLength();
		if (length < 0.0)
		{
			return false;
		}
		int num = (int)(length / testGemSizeX);
		if (length < (double)num * testGemSizeX + (double)Math.Max(num - 1, 0) * spacing)
		{
			num--;
		}
		Interval domain = curve.Domain;
		double[] array = new double[num];
		double[] array2 = new double[num];
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				array[i] = GetUnnormalizedParam(i, num, style);
			}
			array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		}
		for (int j = 0; j < num; j++)
		{
			Transform transform = ((!(array2[j] >= domain.Max)) ? ((!(array2[j] <= domain.Min)) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), gem.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), gem.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), gem.Up));
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, transform * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}
}
