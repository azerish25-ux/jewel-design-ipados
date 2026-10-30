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
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
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
		Plane val = default(Plane);
		if (!((Curve)value).TryGetPlane(ref val))
		{
			return UpdateError("ExtrudePlanarCurve: Curve Not Planar");
		}
		NurbsCurve val2 = ((Curve)value).DuplicateCurve().ToNurbsCurve();
		((GeometryBase)value).Translate(new Vector3d(((Plane)(ref val)).Normal) * value2);
		((GeometryBase)val2).Translate(new Vector3d(((Plane)(ref val)).Normal) * (0.0 - value2));
		Brep val3 = ((Surface)NurbsSurface.CreateRuledSurface((Curve)(object)value, (Curve)(object)val2)).ToBrep();
		if (val3 == null)
		{
			return UpdateError("BrepForm failed");
		}
		Brep val4 = CSUtilRhino.CapPlanarHoles(val3, CSApplicationHelper.Application.Tolerance);
		if (val4 != null)
		{
			OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)val4));
		}
		return base.Update();
	}
}
