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
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Expected O, but got Unknown
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
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
		OnXform val = new OnXform();
		val.Rotation(value3, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform val2 = new OnXform();
		val2.Rotation(value2, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform val3 = new OnXform();
		val3.Translation(0.0, 0.0, value5);
		OnXform xform = val3 * (IOnXform)(object)val2 * (IOnXform)(object)val;
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
		Point3d min = ((BoundingBox)(ref boundingBox)).Min;
		double num = 0.0 - ((Point3d)(ref min)).Y;
		OnPolyline val4 = new OnPolyline();
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y - value7 + num, 0.0, value10 - value11));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y - value7 + num, 0.0, value10));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y - value7 + num + value9, 0.0, value10));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y - value7 + num + value9, 0.0, value10 - value12));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y + value7 - num - value8, 0.0, value10 - value12));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y + value7 - num - value8, 0.0, value10));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y + value7 - num, 0.0, value10));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y + value7 - num, 0.0, value10 - value11));
		min = ((BoundingBox)(ref boundingBox)).Min;
		((ArrayOn3dPoint)val4).Append((IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y - value7 + num, 0.0, value10 - value11));
		double num2 = CSBlockUtil.CSUtilMath.ToRadians(0.0 - value2);
		IOn3dVector on_yaxis = OnUtil.On_yaxis;
		min = ((BoundingBox)(ref boundingBox)).Min;
		((On3dPointArray)val4).Rotate(num2, on_yaxis, (IOn3dPoint)new On3dPoint(0.0 - ((Point3d)(ref min)).Y, 0.0, 0.0));
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData2 = flattenArray.GetArrayItem(i) as CSGemData;
			double num3 = value4;
			min = ((BoundingBox)(ref boundingBox2)).Min;
			if (!GemOffsetCurveV1.GemOffset("Round", cSGemData2, num3 + Math.Abs(((Point3d)(ref min)).Y), out OnCurve curve, true, out string error))
			{
				return UpdateError(error);
			}
			List<OnCurve> list = new List<OnCurve>();
			On3dVector val5 = new On3dVector((IOn3dVector)(object)cSGemData2.Up.Convert());
			val5.Unitize();
			for (int j = 0; j < ((ArrayOn3dPoint)val4).Count(); j++)
			{
				On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)val4)[j]);
				if (!GemOffsetCurveV1.GemOffset("Round", cSGemData2, value4 + val6.x, out OnCurve curve2, true, out error))
				{
					return UpdateError(error);
				}
				((OnGeometry)curve2).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val5) * val6.z));
				list.Add(curve2);
			}
			for (int k = 0; k < list.Count - 1; k++)
			{
				CSUtilSweep.Sweep2NoProfile((IOnCurve)(object)list[k], (IOnCurve)(object)list[k + 1], out var breps);
				if (breps != null)
				{
					for (int l = 0; l < breps.Length; l++)
					{
						cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)breps[l].Convert()));
					}
				}
			}
			double[] array = new double[value];
			double[] array2 = new double[value];
			double num4 = 1.0 / (double)value;
			double num5 = value6 % num4;
			for (int m = 0; m < value; m++)
			{
				array[m] = num5;
				num5 += num4;
			}
			curve.GetNormalizedArcLengthPoints(array, ref array2);
			CSDataArray cSDataArray3 = new CSDataArray();
			for (int n = 0; n < value; n++)
			{
				OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)cSGemData.Up.Convert(), (IOn3dPoint)(object)curve.PointAt(array2[n]), (IOn3dVector)(object)curve.TangentAt(array2[n]), (IOn3dVector)(object)cSGemData2.Up.Convert());
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
