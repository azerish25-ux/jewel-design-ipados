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
[CSBlock("ClusterCushion_V2", "{EEADD5D3-73C0-423b-B80C-64DE2B71E7C6}")]
public sealed class CSClusterCushion2 : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iSpacing;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iRotation;

	public readonly CSInputChannel iGirdleSpacing;

	public readonly CSInputChannel iPlacement;

	public readonly CSInputChannel iFixPosition;

	public readonly CSInputChannel iEven;

	public readonly CSOutputChannel oGem;

	public readonly CSOutputChannel oShape;

	public CSClusterCushion2(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFixPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		double value4 = 0.5;
		iGirdleSpacing.GetDouble(ref value4);
		double value5 = 0.0;
		iPlacement.GetDouble(ref value5);
		double value6 = 0.0;
		iFixPosition.GetDouble(ref value6);
		bool value7 = false;
		iEven.GetBoolean(ref value7);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, value5);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Tight Bounding Box");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				continue;
			}
			if (!GemOffsetCurveV1.GemOffset("Cushion", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), out Curve curve, true, out string error))
			{
				return UpdateError(error);
			}
			double length = curve.GetLength();
			if (double.IsNaN(length) || length < 0.0)
			{
				return UpdateError("Curve Length");
			}
			int num2 = (int)(length / (num + value));
			if (value7 && num2 % 2 != 0)
			{
				num2--;
			}
			double[] array = new double[num2];
			double[] array2 = new double[num2];
			double num3 = 1.0 / (double)num2;
			double num4 = value6 % num3;
			for (int j = 0; j < num2; j++)
			{
				array[j] = num4;
				num4 += num3;
				if (num4 > 1.0)
				{
					num4 -= 1.0;
				}
			}
			Array.Sort(array);
			array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
			CSDataArray cSDataArray2 = new CSDataArray();
			for (int k = 0; k < num2; k++)
			{
				Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, curve.PointAt(array2[k]), curve.TangentAt(array2[k]), cSGemData2.Up);
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, transform4 * transform3 * transform2 * transform);
				cSDataArray2.Append(data);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		oShape.SetData(new CSStringData("Cushion"));
		return base.Update();
	}
}
