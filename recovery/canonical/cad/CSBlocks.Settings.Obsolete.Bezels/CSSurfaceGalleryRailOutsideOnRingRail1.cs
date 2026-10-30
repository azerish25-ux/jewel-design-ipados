using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("SurfaceGalleryRailOutsideOnRingRail_V1", "{B865D441-D557-4718-846A-CA764526AEDD}")]
public sealed class CSSurfaceGalleryRailOutsideOnRingRail1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iThickness { get; }

	public CSInputChannel iTube { get; }

	public CSOutputChannel oBrep { get; }

	public CSSurfaceGalleryRailOutsideOnRingRail1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iThickness = new CSInputChannel(GetPropertyName(() => iThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTube = new CSInputChannel(GetPropertyName(() => iTube), this, ChannelDataType.Surface, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve nurbsCurve = iOutsideProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing outside profile curve");
		}
		double value = 0.0;
		iPlacement.GetDouble(ref value);
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		double value3 = 0.0;
		iThickness.GetDouble(ref value3);
		OnNurbsSurface nurbsSurface = iTube.GetNurbsSurface();
		CSData outputSurfaceData = null;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out var xUp, out var xDown))
			{
				xUp = Transform.Identity;
				xUp.TryGetInverse(out xDown);
			}
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve != null)
			{
				OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
				onNurbsCurve2.Transform(xDown.Convert());
				OnNurbsCurve onNurbsCurve3 = nurbsCurve.Duplicate();
				if (onNurbsCurve3 == null)
				{
					return UpdateError("Duplicate failed");
				}
				if (!GalleryRailOutside(xUp.Convert(), onNurbsCurve2, onNurbsCurve3, value, value2, value3, nurbsSurface, ref outputSurfaceData))
				{
					return UpdateError("GalleryRailOutside failed");
				}
				cSDataArray.Append(outputSurfaceData);
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

	private static bool GalleryRailOutside(OnXform xUp, OnNurbsCurve curveNC, OnNurbsCurve profileNC, double placement, double angle, double thickness, OnNurbsSurface tube, ref CSData outputSurfaceData)
	{
		new On3dPoint();
		OnBoundingBox tight_bbox = new OnBoundingBox();
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		if (!profileNC.GetTightBoundingBox(ref tight_bbox))
		{
			return false;
		}
		On3dVector on3dVector = new On3dVector(new On3dPoint(tight_bbox.Max().x, tight_bbox.Max().y, 0.0));
		on3dVector.Reverse();
		onXform.Translation(on3dVector);
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		onXform = onXform2 * onXform;
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		onXform = onXform2 * onXform;
		profileNC.Transform(onXform);
		OnLineCurve onLineCurve = new OnLine(OnUtil.On_origin, new On3dPoint(0.0, 0.0, -50.0));
		onLineCurve.Transform(xUp);
		OnCurve[] curves = null;
		On3dPointArray points = null;
		if (!RhUtil.RhinoCurveBrepIntersect(onLineCurve, tube.BrepForm(), CSApplicationHelper.Application.Tolerance, out curves, out points))
		{
			return false;
		}
		if (curves.Length != 0)
		{
			OnCurve[] array = curves;
			foreach (OnCurve onCurve in array)
			{
				points.Append(onCurve.PointAtStart());
				points.Append(onCurve.PointAtEnd());
			}
		}
		double num = 5.0;
		for (int j = 0; j < points.Count(); j++)
		{
			double num2 = onLineCurve.PointAtStart().DistanceTo(points[j]);
			if (j == 0 || num2 < num)
			{
				num = num2;
			}
		}
		num += placement;
		double x = tight_bbox.Max().x;
		double y_scale_factor = thickness / x;
		double y = tight_bbox.Max().y;
		double z_scale_factor = num / y;
		onXform.Scale(1.0, y_scale_factor, z_scale_factor);
		profileNC.Transform(onXform);
		on3dVector = new On3dVector(curveNC.PointAtStart())
		{
			x = 0.0,
			z = 0.0
		};
		onXform.Translation(on3dVector);
		profileNC.Transform(onXform);
		double num3 = Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angle));
		for (int k = 0; k < profileNC.CVCount(); k++)
		{
			On3dPoint vertex_value = new On3dPoint();
			profileNC.GetCV(k, ref vertex_value);
			vertex_value.y -= vertex_value.z * num3;
			profileNC.SetCV(k, vertex_value);
		}
		on3dVector = new On3dVector(curveNC.PointAtStart())
		{
			x = 0.0,
			y = 0.0
		};
		onXform.Translation(on3dVector);
		profileNC.Transform(onXform);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		RhUtil.RhinoRailRevolve(profileNC.DuplicateCurve(), curveNC.DuplicateCurve(), new OnLine(OnUtil.On_origin, new On3dPoint(0.0, 0.0, 1.0)), bScaleHeight: false, onNurbsSurface);
		OnBrep onBrep = onNurbsSurface.BrepForm();
		onBrep.Transform(xUp);
		outputSurfaceData = new CSOnGeometryData(onBrep.Convert(), xUp.Convert(), applyTransform: false);
		return true;
	}
}
