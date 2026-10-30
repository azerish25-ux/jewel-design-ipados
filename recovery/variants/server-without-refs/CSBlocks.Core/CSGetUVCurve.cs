using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("GetUVCurve", "{0F661E15-1327-4a78-9174-128B8817F045}")]
public sealed class CSGetUVCurve : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSOutputChannel oCurve;

	public CSGetUVCurve(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetBrep(iSurface.Data, out var value);
		NurbsSurface val = ((value == null) ? iSurface.Data.GetNurbsSurface() : ((Surface)value.Faces[0]).ToNurbsSurface());
		Interval val2 = ((Surface)val).Domain(0);
		Interval val3 = ((Surface)val).Domain(1);
		Point2d val4 = new Point2d(((Interval)(ref val2)).Min, ((Interval)(ref val3)).Mid);
		Point2d val5 = default(Point2d);
		((Point2d)(ref val5))._002Ector(((Interval)(ref val2)).Max, ((Interval)(ref val3)).Mid);
		NurbsCurve val6 = ((Curve)new LineCurve(val4, val5)).ToNurbsCurve();
		Curve val7 = ((Surface)val).Pushup((Curve)(object)val6, 0.05);
		if (val7 == null)
		{
			return UpdateError("Error pushing curve");
		}
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val7.ToNurbsCurve()));
		return base.Update();
	}
}
