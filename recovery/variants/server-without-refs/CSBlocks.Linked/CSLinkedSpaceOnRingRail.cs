using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedSpaceOnRingRail", "{23C6F7FD-E44C-4B5A-93CC-D273D098A470}")]
public sealed class CSLinkedSpaceOnRingRail : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedSpaceOnRingRail(string identifier)
		: base(identifier)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Origin);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		double num = InAOffset.Double;
		bool flag = true;
		Point3d point3d = InLastRightRailPoint.Point3d;
		if (point3d == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value);
		if (value == Vector3d.Unset)
		{
			flag = false;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, 0.0, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, 1.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, 0.0, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0, 1.0, 0.0);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)nurbsCurve, 0.0, ref x, 0.0);
		((Point3d)(ref val)).Transform(x);
		((Point3d)(ref val2)).Transform(x);
		((Point3d)(ref val3)).Transform(x);
		((Point3d)(ref val4)).Transform(x);
		Vector3d inputVC = val2 - val;
		((Vector3d)(ref inputVC)).Unitize();
		Vector3d val5 = new Vector3d(val);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(num / ((Vector3d)(ref val5)).Length, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform val7;
		if (flag)
		{
			val5 = new Vector3d(point3d);
			double length = ((Vector3d)(ref val5)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val, inputVC, length, ref intersectPoint);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), point3d, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val7 = Transform.Identity;
		}
		val7 *= val6;
		((Point3d)(ref val3)).Transform(val7);
		((Point3d)(ref val4)).Transform(val7);
		Vector3d vectorStore = val4 - val3;
		((Vector3d)(ref vectorStore)).Unitize();
		OutRightRailPoint.SetData(new CSPointData(val3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
