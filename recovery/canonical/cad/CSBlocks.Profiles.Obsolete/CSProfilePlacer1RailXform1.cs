using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacer1RailXform_V1", "{0D140D70-56C1-4ca5-8BC2-2233B555B06C}")]
public sealed class CSProfilePlacer1RailXform1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iParameter { get; }

	public CSInputChannel iMethod { get; }

	public CSOutputChannel oXform { get; }

	public CSOutputChannel oParam { get; }

	public CSProfilePlacer1RailXform1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iParameter = new CSInputChannel(GetPropertyName(() => iParameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMethod = new CSInputChannel(GetPropertyName(() => iMethod), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(GetPropertyName(() => oXform), this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(GetPropertyName(() => oParam), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve onNurbsCurve = iRail.GetFlattenArray().GetArrayItem(0).GetNurbsCurve()
			.Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		double value = 0.0;
		iParameter.GetDouble(ref value);
		int value2 = 0;
		iMethod.GetInteger(ref value2);
		double t = 0.0;
		if (!onNurbsCurve.GetNormalizedArcLengthPoint(value, ref t))
		{
			return UpdateError("Get Arc Length Failed");
		}
		oParam.SetData(new CSNumberData(t));
		On3dPoint on3dPoint = onNurbsCurve.PointAt(t);
		On3dVector destination = onNurbsCurve.TangentAt(t);
		OnXform xform = new OnXform();
		switch (value2)
		{
		case 0:
		{
			On3dVector destination2 = on3dPoint * -1.0;
			destination2.Unitize();
			xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, destination, destination2);
			break;
		}
		case 1:
		{
			On3dVector destination2 = on3dPoint * -1.0;
			destination2.y = 0.0;
			destination2.Unitize();
			xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, destination, destination2);
			break;
		}
		case 2:
		{
			On3dVector destination2 = new On3dVector(0.0, 0.0, -1.0);
			xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, destination, destination2);
			break;
		}
		case 3:
		{
			On3dVector destination2 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane onPlane2 = new OnPlane();
			if (onNurbsCurve.IsPlanar(onPlane2))
			{
				destination2 = new On3dVector(onPlane2.Normal());
			}
			xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, destination, destination2);
			break;
		}
		case 4:
		{
			On3dVector destination2 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane onPlane = new OnPlane();
			if (onNurbsCurve.IsPlanar(onPlane))
			{
				destination2 = new On3dVector(onPlane.Normal());
			}
			xform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(1.0, 0.0, 0.0), new On3dVector(0.0, 0.0, -1.0), on3dPoint, destination, destination2);
			break;
		}
		}
		oXform.SetData(new CSXformData(xform.Convert()));
		return base.Update();
	}
}
