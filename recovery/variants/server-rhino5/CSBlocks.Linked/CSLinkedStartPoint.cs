using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedStartPoint", "{B4A3CB2A-03E9-4FFC-9373-323B22304F39}")]
public sealed class CSLinkedStartPoint : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InPoint;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedStartPoint(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail");
		}
		Point3d point3d = InPoint.Point3d;
		if (point3d == Point3d.Unset)
		{
			return UpdateMissing("Point");
		}
		if (!nurbsCurve.ClosestPoint(point3d, out var t))
		{
			return UpdateError("GetClosestPoint failed");
		}
		Point3d point3d2 = nurbsCurve.PointAt(t);
		Vector3d vector3d = CSBlockUtil.CSUtilMath.AverageNormalAt(nurbsCurve, Vector3d.YAxis, t, CSApplicationHelper.Application.Tolerance);
		vector3d.Unitize();
		if (Vector3d.Multiply(new Vector3d(point3d2), vector3d) < 0.0)
		{
			vector3d.Reverse();
		}
		OutRightRailPoint.SetData(new CSPointData(point3d2));
		OutRightRailVector.SetData(new CSVectorData(vector3d));
		return base.Update();
	}
}
