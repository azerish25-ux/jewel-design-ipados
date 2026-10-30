using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Linked;

[CSBlock("LinkedProfileOn2RingRail", "{D0E33A43-794E-4FE4-9318-D2424ABAC382}")]
public sealed class CSLinkedProfileOn2RingRail : CSBlock
{
	public CSInputChannel InRail;

	public CSInputChannel InOutsideRail;

	public CSInputChannel InAOffset;

	public CSInputChannel InLastRightRailPoint;

	public CSInputChannel InLastRightRailVector;

	public CSOutputChannel OutXform;

	public CSOutputChannel OutParam;

	public CSOutputChannel OutParam2;

	public CSOutputChannel OutYSize;

	public CSOutputChannel OutRightRailPoint;

	public CSOutputChannel OutRightRailVector;

	public CSLinkedProfileOn2RingRail(string identifier)
		: base(identifier)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Rail");
		}
		NurbsCurve nurbsCurve2 = InOutsideRail.NurbsCurve;
		double num = 1.0;
		if (InOutsideRail.Data is CSOutsideRingRailData cSOutsideRingRailData)
		{
			num = cSOutsideRingRailData.Scale;
		}
		double num2 = InAOffset.Double;
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
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(num2 / ((Vector3d)(ref val5)).Length, Vector3d.YAxis, Point3d.Origin);
		Transform val7;
		if (flag)
		{
			val5 = new Vector3d(point3d);
			double length = ((Vector3d)(ref val5)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val, inputVC, length, ref intersectPoint);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, point3d, new Point3d(0.0, 1.0, 0.0));
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
		double num3 = default(double);
		((Curve)nurbsCurve).ClosestPoint(val3, ref num3);
		Transform x2 = default(Transform);
		CSUtilGeometry.OrientProfileToCurve((Curve)(object)nurbsCurve, num3, ref x2, num / 2.0);
		OutXform.SetData(new CSXformData(x2));
		if (nurbsCurve2 != null)
		{
			LineCurve val8 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			((GeometryBase)val8).Transform(x2);
			CurveIntersections val9 = Intersection.CurveCurve((Curve)(object)val8, (Curve)(object)nurbsCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val9.Count > 0)
			{
				Point3d pointAtStart = ((Curve)val8).PointAtStart;
				Point3d pointA = val9[0].PointA;
				double num4 = ((Point3d)(ref pointAtStart)).DistanceTo(pointA);
				if (num4 < 0.01)
				{
					num4 = 0.01;
				}
				OutYSize.SetData(new CSNumberData(num4));
				double num5 = val9[0].ParameterB;
				double num6 = num3;
				Interval domain = ((Curve)nurbsCurve).Domain;
				if (Math.Abs(num6 - ((Interval)(ref domain)).Min) < 0.0001)
				{
					double num7 = num5;
					domain = ((Curve)nurbsCurve2).Domain;
					if (Math.Abs(num7 - ((Interval)(ref domain)).Max) < 0.0001)
					{
						domain = ((Curve)nurbsCurve2).Domain;
						num5 = ((Interval)(ref domain)).Min;
					}
				}
				double num8 = num3;
				domain = ((Curve)nurbsCurve).Domain;
				if (Math.Abs(num8 - ((Interval)(ref domain)).Max) < 0.0001)
				{
					double num9 = num5;
					domain = ((Curve)nurbsCurve2).Domain;
					if (Math.Abs(num9 - ((Interval)(ref domain)).Min) < 0.0001)
					{
						domain = ((Curve)nurbsCurve2).Domain;
						num5 = ((Interval)(ref domain)).Max;
					}
				}
				OutParam2.SetData(new CSNumberData(num5));
			}
		}
		OutXform.SetData(new CSXformData(val7 * x));
		OutParam.SetData(new CSNumberData(num3));
		OutRightRailPoint.SetData(new CSPointData(val3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
