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
[CSBlock("GalleryRailSurfaceOutside_V1", "{9E3E1288-A189-459c-8D77-88690DD05BCD}")]
public sealed class CSGalleryRailSurfaceOutside1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iHeight { get; }

	public CSInputChannel iThickness { get; }

	public CSOutputChannel oBrep { get; }

	public CSGalleryRailSurfaceOutside1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		iThickness = new CSInputChannel(GetPropertyName(() => iThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve nurbs_curve = iOutsideProfile.GetNurbsCurve();
		if (nurbs_curve == null)
		{
			On3dPoint arg = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint arg2 = new On3dPoint(0.0, 0.0, 0.0);
			if (new OnLineCurve(arg, arg2).GetNurbForm(ref nurbs_curve) != 1)
			{
				return UpdateError("Create default profile failed");
			}
		}
		double value = 0.0;
		iAngle.GetDouble(ref value);
		double value2 = 0.0;
		iThickness.GetDouble(ref value2);
		CSDataArray flattenArray2 = iHeight.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
			if (!value3.IsValid)
			{
				value3 = Transform.Identity;
			}
			value3.TryGetInverse(out var inverseTransform);
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				cSDataArray.Append(new CSData());
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
			double value4 = 0.0;
			arrayItem2.GetDouble(ref value4);
			OnBrep srf = null;
			if (SurfaceGalleryRailOutside(onNurbsCurve2, profile, value, value4, value2, ref srf))
			{
				srf.Transform(value3.Convert());
				cSDataArray.Append(new CSOnGeometryData(srf.Convert(), value3, applyTransform: false));
			}
			else
			{
				cSDataArray.Append(new CSData());
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
		return base.Update();
	}

	private static bool SurfaceGalleryRailOutside(OnNurbsCurve rail, OnNurbsCurve profile, double angle, double height, double thickness, ref OnBrep srf)
	{
		if (rail == null || profile == null)
		{
			return false;
		}
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!profile.GetTightBoundingBox(ref tight_bbox))
		{
			return false;
		}
		On3dVector on3dVector = new On3dVector(new On3dPoint(tight_bbox.Max().x, tight_bbox.Max().y, 0.0));
		on3dVector.Reverse();
		OnXform onXform = new OnXform();
		onXform.Translation(on3dVector);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		onXform = onXform2 * onXform;
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		onXform = onXform2 * onXform;
		profile.Transform(onXform);
		double x = tight_bbox.Max().x;
		double y_scale_factor = thickness / x;
		double y = tight_bbox.Max().y;
		double z_scale_factor = height / y;
		onXform.Scale(1.0, y_scale_factor, z_scale_factor);
		profile.Transform(onXform);
		on3dVector = new On3dVector(rail.PointAtStart())
		{
			x = 0.0,
			z = 0.0
		};
		onXform.Translation(on3dVector);
		profile.Transform(onXform);
		double num = Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angle));
		for (int i = 0; i < profile.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			profile.GetCV(i, ref vertex_value);
			vertex_value.y -= vertex_value.z * num;
			profile.SetCV(i, vertex_value);
		}
		on3dVector = new On3dVector(rail.PointAtStart());
		on3dVector.x = 0.0;
		on3dVector.y = 0.0;
		onXform.Translation(on3dVector);
		profile.Transform(onXform);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		OnBoundingBox tight_bbox2 = new OnBoundingBox();
		if (!rail.GetTightBoundingBox(ref tight_bbox2))
		{
			return false;
		}
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, 0.0, tight_bbox2.Center().z), new On3dPoint(0.0, tight_bbox2.Max().y * 1.1, tight_bbox2.Center().z));
		ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
		rail.IntersectCurve(curveB, ref x2);
		if (x2.Count() < 1)
		{
			return false;
		}
		rail.DuplicateCurve().ChangeClosedCurveSeam(x2[0].get_m_a(0));
		onNurbsSurface = CSUtilRailRevolve.RailRevolveRhino4(profile, rail);
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
		return true;
	}
}
