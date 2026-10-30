using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

[CSBlock("SurfaceFromPlanarCurve", "{DDBBF493-1C09-4154-A9FB-8940C4F3646B}")]
public sealed class CSSurfaceFromPlanarCurve : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSOutputChannel oSurface;

	public CSSurfaceFromPlanarCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			CSDataArray flattenArray = iCurve.GetFlattenArray();
			if (flattenArray.GetArrayCount() > 0)
			{
				CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out value);
			}
		}
		if (value == null)
		{
			return UpdateMissing("Missing Curve");
		}
		NurbsSurface nurbsSurface = null;
		if (value.TryGetPlane(out var plane))
		{
			PlaneSurface planeSurface = new PlaneSurface(plane, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
			Transform xform = Transform.Scale(plane.PointAt(0.5, 0.5), 1000.0);
			planeSurface.Transform(xform);
			nurbsSurface = planeSurface.ToNurbsSurface();
		}
		if (nurbsSurface != null)
		{
			oSurface.SetData(new CSOnGeometryData(nurbsSurface));
		}
		return base.Update();
	}
}
