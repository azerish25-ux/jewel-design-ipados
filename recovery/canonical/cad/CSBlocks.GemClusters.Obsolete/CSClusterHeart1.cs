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
[CSBlock("ClusterHeart_V1", "{05A5F4E1-291D-47c8-AA0C-2346EBCC4BB6}")]
public sealed class CSClusterHeart1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iCount { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSOutputChannel oGem { get; }

	public CSClusterHeart1(string identifier)
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
		OnXform onXform = new OnXform();
		onXform.Rotation(value3, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(value2, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, 0.0, value5);
		OnXform xform = onXform3 * onXform2 * onXform;
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, xform.Convert());
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("BoundingBox Failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData2.Xform.Convert(), out var _, out var _);
			if (!GemOffsetCurveV1.GemOffset("Heart", cSGemData2, value4 + Math.Abs(boundingBox.Min.Y), out OnCurve curve, true, out string error))
			{
				return UpdateError(error);
			}
			double[] array = new double[value];
			double[] t = new double[value];
			for (int j = 0; j < value; j++)
			{
				array[j] = (double)j / (double)value;
			}
			curve.GetNormalizedArcLengthPoints(array, ref t);
			CSDataArray cSDataArray2 = new CSDataArray();
			OnInterval onInterval = curve.Domain();
			for (int k = 0; k < value; k++)
			{
				OnXform xform2 = ((t[k] != onInterval.Min() && t[k] != onInterval.Max()) ? ((t[k] != onInterval.Mid()) ? CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), curve.PointAt(t[k]), curve.TangentAt(t[k]), cSGemData2.Up.Convert()) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), curve.PointAt(t[k]), -new On3dVector(cSGemData2.North.Convert()), cSGemData2.Up.Convert())) : CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Up.Convert(), curve.PointAt(t[k]), cSGemData2.North.Convert(), cSGemData2.Up.Convert()));
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform2.Convert() * xform.Convert());
				cSDataArray2.Append(data);
			}
			cSDataArray.Append(cSDataArray2);
		}
		oGem.SetData(cSDataArray);
		return base.Update();
	}
}
