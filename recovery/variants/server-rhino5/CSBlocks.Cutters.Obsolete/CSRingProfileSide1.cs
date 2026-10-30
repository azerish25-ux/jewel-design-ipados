using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Cutters.Obsolete;

[Obsolete]
[CSBlock("RingProfileSide_V1", "{B874FCA6-4412-4186-BF6F-F1D0DEEADF59}")]
public sealed class CSRingProfileSide1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iLocation { get; }

	public CSInputChannel iOffset { get; }

	public CSInputChannel iCenterOffset { get; }

	public CSInputChannel iLength { get; }

	public CSInputChannel iCount { get; }

	public CSInputChannel iCountSub { get; }

	public CSInputChannel iRotation { get; }

	public CSOutputChannel oObject { get; }

	public CSRingProfileSide1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iLocation = new CSInputChannel(GetPropertyName(() => iLocation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffset = new CSInputChannel(GetPropertyName(() => iOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCenterOffset = new CSInputChannel(GetPropertyName(() => iCenterOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLength = new CSInputChannel(GetPropertyName(() => iLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCount = new CSInputChannel(GetPropertyName(() => iCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCountSub = new CSInputChannel(GetPropertyName(() => iCountSub), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0226: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Expected O, but got Unknown
		//IL_02b9: Expected O, but got Unknown
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Expected O, but got Unknown
		OnNurbsCurve nurbsCurve = iRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		OnNurbsCurve nurbsCurve2 = iProfile.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		double value = 0.0;
		iOffset.GetDouble(ref value);
		double value2 = 0.0;
		iCenterOffset.GetDouble(ref value2);
		double value3 = 0.0;
		iLength.GetDouble(ref value3);
		int value4 = 1;
		iCount.GetInteger(ref value4);
		int value5 = 1;
		iCountSub.GetInteger(ref value5);
		double value6 = 0.0;
		iRotation.GetDouble(ref value6);
		double value7 = 0.0;
		iLocation.GetDouble(ref value7);
		nurbsCurve2.MakeDeformable();
		OnCurve val = (OnCurve)(object)nurbsCurve2.Duplicate();
		OnCurve val2 = (OnCurve)(object)nurbsCurve2.Duplicate();
		if (val == null)
		{
			return UpdateError("outputCurve Failed");
		}
		OnXform val3 = new OnXform();
		val3.Translation(0.0, value, (0.0 - value3) / 2.0);
		((OnGeometry)val).Transform((IOnXform)(object)val3);
		val3.Translation(0.0, value, value3 / 2.0);
		((OnGeometry)val2).Transform((IOnXform)(object)val3);
		OnCurve[] array;
		if (value2 == 0.0)
		{
			array = (OnCurve[])(object)new OnCurve[2] { val, val2 };
		}
		else
		{
			val3.Translation(0.0, value + value2, 0.0);
			OnCurve val4 = (OnCurve)(object)nurbsCurve2.Duplicate();
			((OnGeometry)val4).Transform((IOnXform)(object)val3);
			array = (OnCurve[])(object)new OnCurve[3] { val, val4, val2 };
		}
		IOnCurve[] profiles = (IOnCurve[])(object)array;
		OnBrep obj = CSUtilSweep.Loft(profiles, (eLoftType)0, capped: true);
		OnXform val5 = new OnXform();
		val5.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6 + 90.0), (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0));
		((OnGeometry)obj).Transform((IOnXform)(object)val5);
		CSDataArray cSDataArray = new CSDataArray();
		CSData cSData = new CSOnGeometryData(obj.Convert(), val5.Convert(), applyTransform: false);
		for (int i = 0; i < value4; i++)
		{
			for (int j = 0; j < value5; j++)
			{
				OnXform val6 = new OnXform();
				val6.Rotation(CSBlockUtil.CSUtilMath.ToRadians(j * 180 / value5), (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0));
				OnXform val7 = new OnXform();
				val7.Translation(0.0, (double)j * 0.09, 0.0);
				double param = ((double)i + value7) / (double)value4;
				OnXform x = new OnXform();
				CSUtilGeometry.OrientProfileToCurveNalp((IOnCurve)(object)nurbsCurve, param, ref x, 1.0);
				CSData data = cSData.Duplicate();
				CSDataHelper.Transform(data, x.Convert() * val6.Convert() * val7.Convert());
				cSDataArray.Append(data);
			}
		}
		oObject.SetData(cSDataArray);
		return base.Update();
	}
}
