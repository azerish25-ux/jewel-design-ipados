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
[CSBlock("ClusterMarquise_V1", "{64CF8156-C6C4-4c72-A9F2-FEBAB0FC7EE8}")]
public sealed class CSClusterMarquise1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iCount { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSOutputChannel oGem { get; }

	public CSClusterMarquise1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCount = new CSInputChannel(GetPropertyName(() => iCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(GetPropertyName(() => oGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		CSGemData cSGemData = iGemDefinition.Data as CSGemData;
		int value = 0;
		iCount.GetInteger(ref value);
		if (value <= 0)
		{
			return UpdateMissing("Gem Input");
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
			return UpdateError("BoundingBox Error");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData2.Xform.Convert(), out var _, out var _);
			OnCurve[] array = GemOffsetCurveV1.GemOffsetCurvesOld("Marquise", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), doTransformUp: true);
			if (array.Length != 2)
			{
				return UpdateError("GemOffsetCurve Failed");
			}
			OnCurve onCurve = array[0];
			OnCurve onCurve2 = array[1];
			int num = value / 2 + value % 2;
			double[] array2 = new double[num];
			double[] t = new double[num];
			if (num > 0)
			{
				for (int j = 0; j < num; j++)
				{
					array2[j] = (double)j / (double)num;
				}
				onCurve.GetNormalizedArcLengthPoints(array2, ref t);
			}
			int num2 = value / 2;
			double[] array3 = new double[num2];
			double[] t2 = new double[num2];
			if (num2 > 0)
			{
				for (int k = 0; k < num2; k++)
				{
					array3[k] = (double)k / (double)num2;
				}
				onCurve2.GetNormalizedArcLengthPoints(array3, ref t2);
			}
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval onInterval = onCurve.Domain();
			for (int l = 0; l < num; l++)
			{
				OnXform xform = ((t[l] != onInterval.Max()) ? ((t[l] != onInterval.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), onCurve.PointAt(t[l]), onCurve.TangentAt(t[l]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve.PointAt(t[l]), cSGemData2.North.Convert(), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve.PointAt(t[l]), -new On3dVector(cSGemData2.North.Convert()), cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform.Convert() * transform3);
				cSDataArray2.Append(data);
			}
			OnInterval onInterval2 = onCurve2.Domain();
			for (int m = 0; m < num2; m++)
			{
				OnXform xform2 = ((t2[m] != onInterval2.Max()) ? ((t2[m] != onInterval2.Min()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), onCurve2.PointAt(t2[m]), onCurve2.TangentAt(t2[m]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve2.PointAt(t2[m]), -new On3dVector(cSGemData2.North.Convert()), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), onCurve2.PointAt(t2[m]), cSGemData2.North.Convert(), cSGemData2.Up.Convert()));
				CSData data2 = cSGemData.Duplicate();
				CSDataHelper.Transform(data2, xform2.Convert() * transform3);
				cSDataArray2.Append(data2);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		return base.Update();
	}
}
