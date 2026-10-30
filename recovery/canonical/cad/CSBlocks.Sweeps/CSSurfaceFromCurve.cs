using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Sweeps;

[CSBlock("SurfaceFromCurve", "{DF3C453E-8FE5-470a-B3B8-D1E151382B77}")]
public sealed class CSSurfaceFromCurve : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSOutputChannel oBrep { get; }

	public CSSurfaceFromCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve nurbsCurve = iCurve.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Curve");
		}
		OnPlane onPlane = new OnPlane();
		if (!nurbsCurve.IsPlanar(onPlane, 0.01))
		{
			return UpdateError("Missing Is Not Planar");
		}
		OnPlaneSurface onPlaneSurface = new OnPlaneSurface(onPlane);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbsCurve.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Scale(onPlane.PointAt(0.5, 0.5), tight_bbox.Diagonal().Length() * 2.0 + 1.0);
		onPlaneSurface.Transform(onXform);
		OnCurve[] edgeCurves = new OnCurve[1] { nurbsCurve };
		RhUtil.RhinoCutUpSurface(onPlaneSurface, bFlip: false, edgeCurves, 0.01, 0.01, out var Breps);
		RhUtil.RhinoCutUpSurface(onPlaneSurface, bFlip: true, edgeCurves, 0.01, 0.01, out var Breps2);
		bool flag = false;
		if (Breps == null)
		{
			Breps = new OnBrep[0];
		}
		if (Breps2 == null)
		{
			Breps2 = new OnBrep[0];
		}
		if (Breps.Length == 0 && Breps2.Length == 0)
		{
			return UpdateError("RhinoCutUpSurface failed");
		}
		if (Breps.Length != 0 && Breps2.Length != 0)
		{
			OnBoundingBox tight_bbox2 = new OnBoundingBox();
			Breps[0].GetTightBoundingBox(ref tight_bbox2);
			OnBoundingBox tight_bbox3 = new OnBoundingBox();
			Breps2[0].GetTightBoundingBox(ref tight_bbox3);
			if (tight_bbox2.Volume() > tight_bbox3.Volume())
			{
				flag = true;
			}
		}
		else if (Breps.Length != 0)
		{
			flag = true;
		}
		if (flag)
		{
			Breps[0].ShrinkSurfaces();
			oBrep.SetData(new CSOnGeometryData(Breps[0].Convert()));
		}
		else
		{
			Breps2[0].ShrinkSurfaces();
			oBrep.SetData(new CSOnGeometryData(Breps2[0].Convert()));
		}
		return base.Update();
	}
}
