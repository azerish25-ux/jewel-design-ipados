using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Surfaces.Obsolete;

[Obsolete]
[CSBlock("ExtrudePlanarCurve_V1", "{F1BDB88C-3CE9-43f0-9A71-C80CBB73DCA0}")]
public sealed class CSExtrudePlanarCurve1 : CSBlock
{
	public CSInputChannel InPlanarCurve { get; }

	public CSInputChannel InExtrudeDistance { get; }

	public CSOutputChannel OutSurface { get; }

	public CSExtrudePlanarCurve1(string identifier)
		: base(identifier)
	{
		InPlanarCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InExtrudeDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(InPlanarCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("ExtrudePlanarCurve: Missing Input Curve");
		}
		double value2 = 20.0;
		if (InExtrudeDistance.GetDouble(ref value2))
		{
			value2 /= 2.0;
		}
		if (value2 < 0.01)
		{
			value2 = 20.0;
		}
		if (!value.TryGetPlane(out var plane))
		{
			return UpdateError("ExtrudePlanarCurve: Curve Not Planar");
		}
		NurbsCurve nurbsCurve = value.DuplicateCurve().ToNurbsCurve();
		value.Translate(new Vector3d(plane.Normal) * value2);
		nurbsCurve.Translate(new Vector3d(plane.Normal) * (0.0 - value2));
		Brep brep = NurbsSurface.CreateRuledSurface(value, nurbsCurve).ToBrep();
		if (brep == null)
		{
			return UpdateError("BrepForm failed");
		}
		Brep brep2 = CSUtilRhino.CapPlanarHoles(brep, CSApplicationHelper.Application.Tolerance);
		if (brep2 != null)
		{
			OutSurface.SetData(new CSOnGeometryData(brep2));
		}
		return base.Update();
	}
}
