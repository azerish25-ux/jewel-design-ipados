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
		CSDataHelper.GetBrep(iSurface.Data, out var value);
		NurbsSurface nurbsSurface = ((value == null) ? iSurface.Data.GetNurbsSurface() : value.Faces[0].ToNurbsSurface());
		Interval interval = nurbsSurface.Domain(0);
		Interval interval2 = nurbsSurface.Domain(1);
		Point2d point2d = new Point2d(interval.Min, interval2.Mid);
		Point2d to = new Point2d(interval.Max, interval2.Mid);
		NurbsCurve curve2d = new LineCurve(point2d, to).ToNurbsCurve();
		Curve curve = nurbsSurface.Pushup(curve2d, 0.05);
		if (curve == null)
		{
			return UpdateError("Error pushing curve");
		}
		oCurve.SetData(new CSOnGeometryData(curve.ToNurbsCurve()));
		return base.Update();
	}
}
