using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterMarquise_V2", "{199A731C-57A3-4c4b-B48E-5F0CE9EBBEE1}")]
public sealed class CSClusterMarquise2 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iSpacing { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iEven { get; }

	public CSOutputChannel oGem { get; }

	public CSOutputChannel oShape { get; }

	public CSClusterMarquise2(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(GetPropertyName(() => iSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEven = new CSInputChannel(GetPropertyName(() => iEven), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(GetPropertyName(() => oGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		oShape = new CSOutputChannel(GetPropertyName(() => oShape), this, ChannelDataType.String, ChannelArrayType.Individual);
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
			return UpdateError("BoundingBox Error");
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData2))
			{
				return UpdateMissing("Gem");
			}
			OnCurve[] array = GemOffsetCurveV1.GemOffsetCurvesOld("Marquise", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), doTransformUp: true);
			if (array.Length != 2)
			{
				return UpdateError("GemOffsetCurve Failed");
			}
			OnCurve onCurve = array[0];
			OnCurve onCurve2 = array[1];
			double length = 0.0;
			if (!onCurve.GetLength(ref length))
			{
				return UpdateError("curveLHS length failed");
			}
			double length2 = 0.0;
			if (!onCurve2.GetLength(ref length2))
			{
				return UpdateError("curveRHS length failed");
			}
			int num2 = (int)((length + length2) / (num + value));
			if (value6 && num2 % 2 != 0)
			{
				num2--;
			}
			int num3 = num2 / 2 + num2 % 2;
			double[] array2 = new double[num3];
			double[] t = new double[num3];
			if (num3 > 0)
			{
				for (int j = 0; j < num3; j++)
				{
					array2[j] = (double)j / (double)num3;
				}
				onCurve.GetNormalizedArcLengthPoints(array2, ref t);
			}
			int num4 = num2 / 2;
			double[] array3 = new double[num4];
			double[] t2 = new double[num4];
			if (num4 > 0)
			{
				for (int k = 0; k < num4; k++)
				{
					array3[k] = (double)k / (double)num4;
				}
				onCurve2.GetNormalizedArcLengthPoints(array3, ref t2);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval onInterval = onCurve.Domain();
			for (int l = 0; l < num3; l++)
			{
				OnXform xform = ((t[l] != onInterval.Max()) ? ((t[l] != onInterval.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), onCurve.PointAt(t[l]), onCurve.TangentAt(t[l]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve.PointAt(t[l]), cSGemData2.North.Convert(), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve.PointAt(t[l]), -new On3dVector(cSGemData2.North.Convert()), cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data);
			}
			OnInterval onInterval2 = onCurve2.Domain();
			for (int m = 0; m < num4; m++)
			{
				OnXform xform2 = ((t2[m] != onInterval2.Max()) ? ((t2[m] != onInterval2.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), onCurve2.PointAt(t2[m]), onCurve2.TangentAt(t2[m]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve2.PointAt(t2[m]), -new On3dVector(cSGemData2.North.Convert()), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve2.PointAt(t2[m]), cSGemData2.North.Convert(), cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform2.Convert() * transform3 * transform2 * transform);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		oShape.SetData(new CSStringData("Marquise"));
		return base.Update();
	}
}
