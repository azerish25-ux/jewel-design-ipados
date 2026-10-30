using System;
using System.Collections.Generic;
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
[CSBlock("ClusterRound_V1", "{33F2A999-F524-4b50-B433-78BD4B7F84CD}")]
public sealed class CSClusterRound1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iCount { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iFixPosition { get; }

	public CSInputChannel iNudge { get; }

	public CSInputChannel iInsideAmount { get; }

	public CSInputChannel iOutsideAmount { get; }

	public CSInputChannel iHeight { get; }

	public CSInputChannel iChannelDepth { get; }

	public CSInputChannel iDepth { get; }

	public CSOutputChannel oGem { get; }

	public CSOutputChannel oMetal { get; }

	public CSClusterRound1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCount = new CSInputChannel(GetPropertyName(() => iCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFixPosition = new CSInputChannel(GetPropertyName(() => iFixPosition), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iNudge = new CSInputChannel(GetPropertyName(() => iNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iInsideAmount = new CSInputChannel(GetPropertyName(() => iInsideAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideAmount = new CSInputChannel(GetPropertyName(() => iOutsideAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelDepth = new CSInputChannel(GetPropertyName(() => iChannelDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDepth = new CSInputChannel(GetPropertyName(() => iDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGem = new CSOutputChannel(GetPropertyName(() => oGem), this, ChannelDataType.Gem, ChannelArrayType.Array);
		oMetal = new CSOutputChannel(GetPropertyName(() => oMetal), this, ChannelDataType.Any, ChannelArrayType.Array);
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
		double value4 = 0.5;
		iGirdleSpacing.GetDouble(ref value4);
		double value5 = 0.0;
		iPlacement.GetDouble(ref value5);
		double value6 = 0.0;
		iFixPosition.GetDouble(ref value6);
		double value7 = 0.5;
		iNudge.GetDouble(ref value7);
		double value8 = 1.0;
		iInsideAmount.GetDouble(ref value8);
		double value9 = 1.0;
		iOutsideAmount.GetDouble(ref value9);
		double value10 = 0.5;
		iHeight.GetDouble(ref value10);
		double value11 = 1.0;
		iChannelDepth.GetDouble(ref value11);
		double value12 = 2.0;
		iDepth.GetDouble(ref value12);
		OnXform onXform = new OnXform();
		onXform.Rotation(value3, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(value2, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, 0.0, value5);
		OnXform xform = onXform3 * onXform2 * onXform;
		CSData cSData = cSGemData.Duplicate();
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Tight Bounding Box");
		}
		CSDataHelper.Transform(cSData, xform.Convert());
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox2))
		{
			return UpdateError("Tight Bounding Box Rotated");
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		double num = 0.0 - boundingBox.Min.Y;
		OnPolyline onPolyline = new OnPolyline();
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y - value7 + num, 0.0, value10 - value11));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y - value7 + num, 0.0, value10));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y - value7 + num + value9, 0.0, value10));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y - value7 + num + value9, 0.0, value10 - value12));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y + value7 - num - value8, 0.0, value10 - value12));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y + value7 - num - value8, 0.0, value10));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y + value7 - num, 0.0, value10));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y + value7 - num, 0.0, value10 - value11));
		onPolyline.Append(new On3dPoint(0.0 - boundingBox.Min.Y - value7 + num, 0.0, value10 - value11));
		onPolyline.Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - value2), OnUtil.On_yaxis, new On3dPoint(0.0 - boundingBox.Min.Y, 0.0, 0.0));
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			if (!GemOffsetCurveV1.GemOffset("Round", cSGemData2, value4 + Math.Abs(boundingBox2.Min.Y), out OnCurve curve, true, out string error))
			{
				return UpdateError(error);
			}
			List<OnCurve> list = new List<OnCurve>();
			On3dVector on3dVector = new On3dVector(cSGemData2.Up.Convert());
			on3dVector.Unitize();
			for (int j = 0; j < onPolyline.Count(); j++)
			{
				On3dPoint on3dPoint = new On3dPoint(onPolyline[j]);
				if (!GemOffsetCurveV1.GemOffset("Round", cSGemData2, value4 + on3dPoint.x, out OnCurve curve2, true, out error))
				{
					return UpdateError(error);
				}
				curve2.Translate(new On3dVector(on3dVector) * on3dPoint.z);
				list.Add(curve2);
			}
			for (int k = 0; k < list.Count - 1; k++)
			{
				CSUtilSweep.Sweep2NoProfile(list[k], list[k + 1], out var breps);
				if (breps != null)
				{
					for (int l = 0; l < breps.Length; l++)
					{
						cSDataArray2.Append(new CSOnGeometryData(breps[l].Convert()));
					}
				}
			}
			double[] array = new double[value];
			double[] t = new double[value];
			double num2 = 1.0 / (double)value;
			double num3 = value6 % num2;
			for (int m = 0; m < value; m++)
			{
				array[m] = num3;
				num3 += num2;
			}
			curve.GetNormalizedArcLengthPoints(array, ref t);
			CSDataArray cSDataArray3 = new CSDataArray();
			for (int n = 0; n < value; n++)
			{
				OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, cSGemData.Up.Convert(), curve.PointAt(t[n]), curve.TangentAt(t[n]), cSGemData2.Up.Convert());
				CSData data = cSGemData.Duplicate();
				CSDataHelper.Transform(data, xform2.Convert() * xform.Convert());
				cSDataArray3.Append(data);
			}
			cSDataArray.Append(cSDataArray3);
		}
		oGem.SetData(cSDataArray);
		oMetal.SetData(cSDataArray2);
		return base.Update();
	}
}
