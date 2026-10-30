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
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
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
		NurbsSurface val = null;
		Plane val2 = default(Plane);
		if (((Curve)value).TryGetPlane(ref val2))
		{
			PlaneSurface val3 = new PlaneSurface(val2, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
			Transform val4 = Transform.Scale(((Plane)(ref val2)).PointAt(0.5, 0.5), 1000.0);
			((GeometryBase)val3).Transform(val4);
			val = ((Surface)val3).ToNurbsSurface();
		}
		if (val != null)
		{
			oSurface.SetData(new CSOnGeometryData((GeometryBase)(object)val));
		}
		return base.Update();
	}
}
