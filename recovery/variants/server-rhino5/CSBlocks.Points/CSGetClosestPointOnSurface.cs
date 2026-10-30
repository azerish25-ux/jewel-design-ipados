using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Points;

[CSBlock("GetClosestPointOnSurface", "{5C8727FF-F280-496F-8E7D-DE0A76D9E348}")]
public sealed class CSGetClosestPointOnSurface : CSBlock
{
	public readonly CSInputChannel iPoint;

	public readonly CSInputChannel iSurface;

	public readonly CSOutputChannel oSurfacePoint;

	public readonly CSOutputChannel oUValue;

	public readonly CSOutputChannel oVValue;

	public CSGetClosestPointOnSurface(string identifier)
		: base(identifier)
	{
		iPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		oSurfacePoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oUValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oVValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		Point3d value = Point3d.Unset;
		if (!CSDataHelper.GetPoint(iPoint.Data, out value))
		{
			return UpdateMissing("Point");
		}
		CSDataHelper.GetBrep(iSurface.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Surface");
		}
		Point3d closestPoint = Point3d.Unset;
		if (!value2.ClosestPoint(value, out closestPoint, out var ci, out var s, out var t, 9999.0, out var _) || closestPoint == Point3d.Unset)
		{
			return UpdateError("Unable to get closest point");
		}
		if (ci.ComponentIndexType == ComponentIndexType.BrepFace)
		{
			BrepFace brepFace = value2.Faces[ci.Index];
			s = (s - brepFace.Domain(0).Min) / brepFace.Domain(0).Length;
			t = (t - brepFace.Domain(1).Min) / brepFace.Domain(1).Length;
		}
		oSurfacePoint.SetData(new CSPointData(closestPoint));
		oUValue.SetData(new CSNumberData(s));
		oVValue.SetData(new CSNumberData(t));
		return base.Update();
	}
}
