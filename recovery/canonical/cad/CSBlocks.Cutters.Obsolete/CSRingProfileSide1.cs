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
		OnCurve onCurve = nurbsCurve2.Duplicate();
		OnCurve onCurve2 = nurbsCurve2.Duplicate();
		if (onCurve == null)
		{
			return UpdateError("outputCurve Failed");
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, value, (0.0 - value3) / 2.0);
		onCurve.Transform(onXform);
		onXform.Translation(0.0, value, value3 / 2.0);
		onCurve2.Transform(onXform);
		OnCurve[] array;
		if (value2 == 0.0)
		{
			array = new OnCurve[2] { onCurve, onCurve2 };
		}
		else
		{
			onXform.Translation(0.0, value + value2, 0.0);
			OnCurve onCurve3 = nurbsCurve2.Duplicate();
			onCurve3.Transform(onXform);
			array = new OnCurve[3] { onCurve, onCurve3, onCurve2 };
		}
		IOnCurve[] profiles = array;
		OnBrep onBrep = CSUtilSweep.Loft(profiles, IArgsRhinoLoft.eLoftType.ltNormal, capped: true);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6 + 90.0), new On3dVector(0.0, 1.0, 0.0), new On3dPoint(0.0, 0.0, 0.0));
		onBrep.Transform(onXform2);
		CSDataArray cSDataArray = new CSDataArray();
		CSData cSData = new CSOnGeometryData(onBrep.Convert(), onXform2.Convert(), applyTransform: false);
		for (int i = 0; i < value4; i++)
		{
			for (int j = 0; j < value5; j++)
			{
				OnXform onXform3 = new OnXform();
				onXform3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(j * 180 / value5), new On3dVector(0.0, 1.0, 0.0), new On3dPoint(0.0, 0.0, 0.0));
				OnXform onXform4 = new OnXform();
				onXform4.Translation(0.0, (double)j * 0.09, 0.0);
				double param = ((double)i + value7) / (double)value4;
				OnXform x = new OnXform();
				CSUtilGeometry.OrientProfileToCurveNalp(nurbsCurve, param, ref x, 1.0);
				CSData data = cSData.Duplicate();
				CSDataHelper.Transform(data, x.Convert() * onXform3.Convert() * onXform4.Convert());
				cSDataArray.Append(data);
			}
		}
		oObject.SetData(cSDataArray);
		return base.Update();
	}
}
