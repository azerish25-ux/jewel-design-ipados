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
[CSBlock("ClusterSquare_V1", "{DA51AEBC-2C02-4879-8C38-21A57BE955A0}")]
public sealed class CSClusterSquare1 : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iGemDefinition;

	public readonly CSInputChannel iCount;

	public readonly CSInputChannel iAngle;

	public readonly CSInputChannel iRotation;

	public readonly CSInputChannel iGirdleSpacing;

	public readonly CSInputChannel iPlacement;

	public readonly CSInputChannel iStyle;

	public readonly CSOutputChannel oGem;

	public CSClusterSquare1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
			return UpdateMissing("Count");
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
		double value6 = 0.0;
		iStyle.GetDouble(ref value6);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value3, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Translation(0.0, 0.0, value5) * transform2 * transform;
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, transform3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Boundingbox Error");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			Curve[] array = GemOffsetCurveV1.GemOffsetCurves("Baguette Straight", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), doTransformUp: true);
			if (array.Length != 5)
			{
				return UpdateError("Curve Count error");
			}
			Curve curve = array[0];
			Curve curve2 = array[1];
			Curve curve3 = array[2];
			Curve curve4 = array[3];
			Curve curve5 = array[4];
			Curve[] array2 = Curve.JoinCurves(new Curve[2] { curve, curve5 });
			if (array2 == null)
			{
				return UpdateError("Merge Curve Error");
			}
			Curve curve6 = array2[0];
			int num = value / 4;
			int num2 = value / 4;
			int num3 = value / 4;
			int num4 = value / 4;
			int num5 = value % 4;
			if (cSGemData2.GemXSize >= cSGemData2.GemYSize)
			{
				if (num5 > 0)
				{
					num++;
				}
				if (num5 > 1)
				{
					num3++;
				}
				if (num5 > 2)
				{
					num4++;
				}
			}
			else
			{
				if (num5 > 0)
				{
					num4++;
				}
				if (num5 > 1)
				{
					num2++;
				}
				if (num5 > 2)
				{
					num++;
				}
			}
			double[] array3 = new double[num];
			double[] array4 = new double[num];
			if (num > 0)
			{
				for (int j = 0; j < num; j++)
				{
					array3[j] = GemPosition(j, num, value6);
				}
				array4 = curve6.NormalizedLengthParameters(array3, CSApplicationHelper.Application.Tolerance);
			}
			double[] array5 = new double[num2];
			double[] array6 = new double[num2];
			if (num2 > 0)
			{
				for (int k = 0; k < num2; k++)
				{
					array5[k] = GemPosition(k, num2, value6);
				}
				array6 = curve2.NormalizedLengthParameters(array5, CSApplicationHelper.Application.Tolerance);
			}
			double[] array7 = new double[num3];
			double[] array8 = new double[num3];
			if (num3 > 0)
			{
				for (int l = 0; l < num3; l++)
				{
					array7[l] = GemPosition(l, num3, value6);
				}
				array8 = curve3.NormalizedLengthParameters(array7, CSApplicationHelper.Application.Tolerance);
			}
			double[] array9 = new double[num4];
			double[] array10 = new double[num4];
			if (num4 > 0)
			{
				for (int m = 0; m < num4; m++)
				{
					array9[m] = GemPosition(m, num4, value6);
				}
				array10 = curve4.NormalizedLengthParameters(array9, CSApplicationHelper.Application.Tolerance);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			Interval domain = curve6.Domain;
			for (int n = 0; n < num; n++)
			{
				Transform transform4 = default(Transform);
				transform4 = ((array4[n] != domain.Max) ? ((array4[n] != domain.Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, curve6.PointAt(array4[n]), curve6.TangentAt(array4[n]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, curve6.PointAt(array4[n]), curve6.TangentAt(array4[n]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, curve6.PointAt(array4[n]), curve6.TangentAt(array4[n]), cSGemData2.Up));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, transform4 * transform3);
				cSDataArray2.Append(data);
			}
			Interval domain2 = curve2.Domain;
			for (int num6 = 0; num6 < num2; num6++)
			{
				Transform transform5 = default(Transform);
				transform5 = ((array6[num6] != domain2.Max) ? ((array6[num6] != domain2.Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, curve2.PointAt(array6[num6]), curve2.TangentAt(array6[num6]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, curve2.PointAt(array6[num6]), curve2.TangentAt(array6[num6]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, curve2.PointAt(array6[num6]), curve2.TangentAt(array6[num6]), cSGemData2.Up));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, transform5 * transform3);
				cSDataArray2.Append(data2);
			}
			Interval domain3 = curve3.Domain;
			for (int num7 = 0; num7 < num3; num7++)
			{
				Transform transform6 = default(Transform);
				transform6 = ((array8[num7] != domain3.Max) ? ((array8[num7] != domain3.Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, curve3.PointAt(array8[num7]), curve3.TangentAt(array8[num7]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, curve3.PointAt(array8[num7]), curve3.TangentAt(array8[num7]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, curve3.PointAt(array8[num7]), curve3.TangentAt(array8[num7]), cSGemData2.Up));
				CSData data3 = cSGemData.Duplicate();
				CSDataHelper.Transform(data3, transform6 * transform3);
				cSDataArray2.Append(data3);
			}
			Interval domain4 = curve4.Domain;
			for (int num8 = 0; num8 < num4; num8++)
			{
				Transform transform7 = default(Transform);
				transform7 = ((array10[num8] != domain4.Max) ? ((array10[num8] != domain4.Min) ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, cSGemData.Up, curve4.PointAt(array10[num8]), curve4.TangentAt(array10[num8]), cSGemData2.Up) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(1.0, -1.0, 0.0), cSGemData.Up, curve4.PointAt(array10[num8]), curve4.TangentAt(array10[num8]), cSGemData2.Up)) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, new Vector3d(-1.0, 1.0, 0.0), cSGemData.Up, curve4.PointAt(array10[num8]), curve4.TangentAt(array10[num8]), cSGemData2.Up));
				CSData data4 = cSGemData.Duplicate();
				CSDataHelper.Transform(data4, transform7 * transform3);
				cSDataArray2.Append(data4);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		return base.Update();
	}

	private static double GemPosition(int gem, int totalGems, double style)
	{
		if (style == 0.0)
		{
			return (double)gem / (double)totalGems;
		}
		double num = 1.0 / (2.0 * Math.Sqrt(2.0) + (double)(2 * (totalGems - 1)));
		return num * Math.Sqrt(2.0) + num * 2.0 * (double)gem;
	}
}
