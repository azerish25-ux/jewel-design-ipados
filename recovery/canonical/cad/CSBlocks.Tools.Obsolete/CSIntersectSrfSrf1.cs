using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("IntersectSrfSrf_V1", "{C4BFDB8B-E567-4544-88F9-5242B92CB400}")]
public sealed class CSIntersectSrfSrf1 : CSBlock
{
	public CSOutputChannel OutCurves { get; }

	public CSInputChannel InSurface { get; }

	public CSInputChannel InIntersector { get; }

	public CSIntersectSrfSrf1(string identifier)
		: base(identifier)
	{
		InSurface = new CSInputChannel(GetPropertyName(() => InSurface), this, ChannelDataType.Surface, ChannelArrayType.Array | ChannelArrayType.Individual);
		InIntersector = new CSInputChannel(GetPropertyName(() => InIntersector), this, ChannelDataType.Curve | ChannelDataType.Surface, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(GetPropertyName(() => OutCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface 1");
		}
		OnNurbsSurface onNurbsSurface = flattenArray.GetArrayItem(0).GetNurbsSurface().Convert();
		if (onNurbsSurface == null)
		{
			return UpdateMissing("Missing Surface 1");
		}
		CSDataArray flattenArray2 = InIntersector.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface 2");
		}
		OnNurbsSurface onNurbsSurface2 = flattenArray2.GetArrayItem(0).GetNurbsSurface().Convert();
		OnNurbsCurve onNurbsCurve = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsSurface2 == null && onNurbsCurve == null)
		{
			return UpdateMissing("Missing Surface 2");
		}
		OnNurbsSurface onNurbsSurface3 = null;
		if (onNurbsSurface2 != null)
		{
			onNurbsSurface3 = onNurbsSurface2;
		}
		else
		{
			OnPlane onPlane = new OnPlane();
			if (onNurbsCurve.IsPlanar(onPlane))
			{
				On3dPoint fixed_point = onPlane.PointAt(0.5, 0.5);
				OnXform onXform = new OnXform();
				onXform.Scale(fixed_point, 1000.0);
				OnPlaneSurface onPlaneSurface = new OnPlaneSurface(onPlane);
				onPlaneSurface.Transform(onXform);
				onNurbsSurface3 = onPlaneSurface.NurbsSurface();
			}
		}
		if (onNurbsSurface3 == null)
		{
			return UpdateError("Unable to use Intersect Surface");
		}
		if (!RhUtil.RhinoIntersectSurfaces(onNurbsSurface, onNurbsSurface2, CSApplicationHelper.Application.Tolerance, out var curves, out var _))
		{
			return UpdateError("Unable to intersect");
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnCurve[] array = curves;
		foreach (OnCurve crv in array)
		{
			cSDataArray.Append(new CSOnGeometryData(crv.Convert()));
		}
		OutCurves.SetData(cSDataArray);
		return base.Update();
	}
}
