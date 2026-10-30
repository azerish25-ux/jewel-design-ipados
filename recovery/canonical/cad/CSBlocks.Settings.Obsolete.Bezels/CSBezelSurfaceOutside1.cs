using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("BezelSurfaceOutside_V1", "{79A34A3E-D655-478a-8740-9DDCD3BCC9BA}")]
public sealed class CSBezelSurfaceOutside1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iHeight { get; }

	public CSOutputChannel oBrep { get; }

	public CSOutputChannel oCurve { get; }

	public CSBezelSurfaceOutside1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Array);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve nurbs_curve = iOutsideProfile.GetNurbsCurve();
		if (nurbs_curve == null)
		{
			On3dPoint arg = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint arg2 = new On3dPoint(0.0, 0.0, 0.0);
			OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
			nurbs_curve = new OnNurbsCurve();
			if (onLineCurve.GetNurbForm(ref nurbs_curve) != 1)
			{
				return UpdateError("OutsideProfile invalid");
			}
		}
		double value = 0.0;
		iAngle.GetDouble(ref value);
		CSDataArray flattenArray2 = iHeight.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
			if (!value2.IsValid)
			{
				value2 = Transform.Identity;
			}
			value2.TryGetInverse(out var inverseTransform);
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				cSDataArray.Append(new CSData());
				cSDataArray2.Append(new CSData());
				continue;
			}
			OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
			onNurbsCurve2.Transform(inverseTransform.Convert());
			OnNurbsCurve profile = nurbs_curve.Duplicate();
			int index = 0;
			if (flattenArray2.GetArrayCount() > 1)
			{
				index = i;
			}
			CSData arrayItem2 = flattenArray2.GetArrayItem(index);
			double value3 = 0.0;
			arrayItem2.GetDouble(ref value3);
			OnBrep srf = null;
			OnNurbsCurve crv = null;
			if (SurfaceBezelOutside(onNurbsCurve2, profile, value, value3, ref srf, ref crv))
			{
				cSDataArray.Append(new CSOnGeometryData(srf.Convert(), value2));
				cSDataArray2.Append(new CSOnGeometryData(crv.Convert(), value2));
			}
			else
			{
				cSDataArray.Append(new CSData());
				cSDataArray2.Append(new CSData());
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oBrep.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oBrep.SetData(cSDataArray);
		}
		if (cSDataArray2.GetArrayCount() == 1)
		{
			oCurve.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oCurve.SetData(cSDataArray2);
		}
		return base.Update();
	}

	private static bool SurfaceBezelOutside(OnNurbsCurve rail, OnNurbsCurve profile, double angle, double height, ref OnBrep srf, ref OnNurbsCurve crv)
	{
		if (rail == null || profile == null)
		{
			return false;
		}
		On3dPoint arg = profile.PointAtStart();
		On3dVector on3dVector = new On3dVector(arg);
		on3dVector.Reverse();
		OnXform onXform = new OnXform();
		onXform.Translation(on3dVector);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		profile.Transform(onXform3 * onXform2 * onXform);
		On3dPoint on3dPoint = profile.PointAtEnd();
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(on3dPoint.y, 0.0 - on3dPoint.z);
		OnXform onXform4 = new OnXform();
		onXform4.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, OnUtil.On_xaxis, OnUtil.On_origin);
		profile.Transform(onXform4);
		double num2 = 0.0 - profile.PointAtEnd().z;
		double num3 = height / num2;
		OnXform onXform5 = new OnXform();
		onXform5.Scale(num3, num3, num3);
		profile.Transform(onXform5);
		On3dPoint on3dPoint2 = rail.PointAtStart();
		arg = profile.PointAtStart();
		onXform.Translation(on3dPoint2 - arg);
		profile.Transform(onXform);
		OnNurbsSurface onNurbsSurface = CSUtilRailRevolve.RailRevolve(profile, rail, 0);
		if (onNurbsSurface == null)
		{
			return false;
		}
		if (!onNurbsSurface.IsValid())
		{
			return false;
		}
		if (!onNurbsSurface.HasBrepForm())
		{
			return false;
		}
		srf = onNurbsSurface.BrepForm();
		On3dPoint on3dPoint3 = profile.PointAtStart();
		on3dPoint3.z = 0.0;
		On3dPoint on3dPoint4 = profile.PointAtEnd();
		on3dPoint4.z = 0.0;
		double num4 = on3dPoint3.DistanceTo(OnUtil.On_origin);
		num3 = on3dPoint4.DistanceTo(OnUtil.On_origin) / num4;
		onXform5.Scale(num3, num3, 1.0);
		rail.Transform(onXform5);
		onXform.Translation(0.0, 0.0, 0.0 - height);
		rail.Transform(onXform);
		crv = rail;
		return true;
	}
}
