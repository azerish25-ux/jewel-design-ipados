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
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
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
		double num = default(double);
		if (!((Curve)nurbsCurve).ClosestPoint(point3d, ref num))
		{
			return UpdateError("GetClosestPoint failed");
		}
		Point3d val = ((Curve)nurbsCurve).PointAt(num);
		Vector3d val2 = CSBlockUtil.CSUtilMath.AverageNormalAt((Curve)(object)nurbsCurve, Vector3d.YAxis, num, CSApplicationHelper.Application.Tolerance);
		((Vector3d)(ref val2)).Unitize();
		if (Vector3d.Multiply(new Vector3d(val), val2) < 0.0)
		{
			((Vector3d)(ref val2)).Reverse();
		}
		OutRightRailPoint.SetData(new CSPointData(val));
		OutRightRailVector.SetData(new CSVectorData(val2));
		return base.Update();
	}
}
