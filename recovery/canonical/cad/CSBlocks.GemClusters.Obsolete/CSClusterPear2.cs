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
[CSBlock("ClusterPear_V2", "{702A46B8-7848-4d50-8145-7380BD694242}")]
public sealed class CSClusterPear2 : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iSpacing;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iRotation;

	public readonly CSInputChannel iGirdleSpacing;

	public readonly CSInputChannel iPlacement;

	public readonly CSInputChannel iEven;

	public readonly CSOutputChannel oGem;

	public readonly CSOutputChannel oShape;

	public CSClusterPear2(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEven = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		oShape = new CSOutputChannel(this, ChannelDataType.String, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gems");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		double value = 0.0;
		iSpacing.GetDouble(ref value);
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
		bool value6 = false;
		iEven.GetBoolean(ref value6);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, value5);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Boundingbox error");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is CSGemData cSGemData2)
			{
				if (!GemOffsetCurveV1.GemOffset("Pear", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), out Curve curve, true, out string error))
				{
					return UpdateError(error);
				}
				double length = curve.GetLength();
				if (length < 0.0)
				{
					return UpdateError("Curve Length");
				}
				int num2 = (int)(length / (num + value));
				if (value6 && num2 % 2 != 0)
				{
					num2--;
				}
				double[] array = new double[num2];
				double[] array2 = new double[num2];
				for (int j = 0; j < num2; j++)
				{
					array[j] = (double)j / (double)num2;
				}
				array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
				CSDataArray cSDataArray2 = new CSDataArray();
				Interval domain = curve.Domain;
				for (int k = 0; k < num2; k++)
				{
					Transform transform4 = ((array2[k] != domain.Max && array2[k] != domain.Mid) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, curve.PointAt(array2[k]), curve.TangentAt(array2[k]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, cSGemData.North, cSGemData.Up, curve.PointAt(array2[k]), -new Vector3d(cSGemData2.North), cSGemData2.Up));
					CSData data = cSGemData.Duplicate();
					CSDataHelper.Transform(data, transform4 * transform3 * transform2 * transform);
					cSDataArray2.Append(data);
				}
				cSDataArray.Append(cSDataArray2);
			}
		}
		oGem.SetData(cSDataArray);
		oShape.SetData(new CSStringData("Pear"));
		return base.Update();
	}
}
