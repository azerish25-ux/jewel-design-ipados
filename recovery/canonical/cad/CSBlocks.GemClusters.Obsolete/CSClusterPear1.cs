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
[CSBlock("ClusterPear_V1", "{7706FEFC-7324-4f6a-86F6-19E15FC7AC4D}")]
public sealed class CSClusterPear1 : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iCount;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iRotation;

	public readonly CSInputChannel iGirdleSpacing;

	public readonly CSInputChannel iPlacement;

	public readonly CSOutputChannel oGem;

	public CSClusterPear1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		CSGemData cSGemData = iGemDefinition.Data as CSGemData;
		int value = 0;
		iCount.GetInteger(ref value);
		if (value <= 0)
		{
			return UpdateMissing("Gem Count");
		}
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		double value3 = 0.0;
		iRotation.GetDouble(ref value3);
		value3 = CSBlockUtil.CSUtilMath.ToRadians(value3);
		double value4 = 0.0;
		iGirdleSpacing.GetDouble(ref value4);
		double value5 = 0.0;
		iPlacement.GetDouble(ref value5);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, value5) * transform2 * transform;
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gme Tight Bounding Box");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			if (!GemOffsetCurveV1.GemOffset("Pear", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), out Curve curve, true, out string error))
			{
				return UpdateError(error);
			}
			double[] array = new double[value];
			double[] array2 = new double[value];
			for (int j = 0; j < value; j++)
			{
				array[j] = (double)j / (double)value;
			}
			array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			CSDataArray cSDataArray2 = new CSDataArray();
			Interval domain = curve.Domain;
			for (int k = 0; k < value; k++)
			{
				Transform transform4 = ((array2[k] != domain.Max && array2[k] != domain.Mid) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, curve.PointAt(array2[k]), curve.TangentAt(array2[k]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, cSGemData.North, cSGemData.Up, curve.PointAt(array2[k]), -new Vector3d(cSGemData2.North), cSGemData2.Up));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, transform4 * transform3);
				cSDataArray2.Append(data);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		return base.Update();
	}
}
