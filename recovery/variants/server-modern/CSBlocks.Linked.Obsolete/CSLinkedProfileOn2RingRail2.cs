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

namespace CSBlocks.Linked.Obsolete;

[Obsolete]
[CSBlock("LinkedProfileOn2RingRail_V2", "{848770E1-1A36-40ed-893B-394B61A2092F}")]
public sealed class CSLinkedProfileOn2RingRail2 : CSBlock
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

	public CSLinkedProfileOn2RingRail2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(InRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Rail");
		}
		CSDataHelper.GetNurbsCurve(InOutsideRail.Data, out var value2);
		double num = 1.0;
		if (InOutsideRail.Data is CSOutsideRingRailData cSOutsideRingRailData)
		{
			num = cSOutsideRingRailData.Scale;
		}
		double value3 = 0.0;
		InAOffset.GetDouble(ref value3);
		bool flag = true;
		CSDataHelper.GetPoint(InLastRightRailPoint, out var value4);
		if (value4 == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value5);
		if (value5 == Vector3d.Unset)
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
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)value, 0.0, ref x, 0.0);
		((Point3d)(ref val)).Transform(x);
		((Point3d)(ref val2)).Transform(x);
		((Point3d)(ref val3)).Transform(x);
		((Point3d)(ref val4)).Transform(x);
		Vector3d inputVC = val2 - val;
		((Vector3d)(ref inputVC)).Unitize();
		double num2 = value3;
		Vector3d val5 = new Vector3d(val);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(num2 / ((Vector3d)(ref val5)).Length, Vector3d.YAxis, Point3d.Origin);
		Transform val7;
		if (flag)
		{
			val5 = new Vector3d(value4);
			double length = ((Vector3d)(ref val5)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val, inputVC, length, ref intersectPoint);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value4, new Point3d(0.0, 1.0, 0.0));
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
		((Curve)value).ClosestPoint(val3, ref num3);
		Transform x2 = default(Transform);
		CSUtilGeometry.OrientProfileToCurve((Curve)(object)value, num3, ref x2, num / 2.0);
		OutXform.SetData(new CSXformData(x2));
		if (value2 != null)
		{
			LineCurve val8 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			((GeometryBase)val8).Transform(x2);
			CurveIntersections val9 = Intersection.CurveCurve((Curve)(object)val8, (Curve)(object)value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
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
				Interval domain = ((Curve)value).Domain;
				if (Math.Abs(num6 - ((Interval)(ref domain)).Min) < 0.0001)
				{
					double num7 = num5;
					domain = ((Curve)value2).Domain;
					if (Math.Abs(num7 - ((Interval)(ref domain)).Max) < 0.0001)
					{
						domain = ((Curve)value2).Domain;
						num5 = ((Interval)(ref domain)).Min;
					}
				}
				double num8 = num3;
				domain = ((Curve)value).Domain;
				if (Math.Abs(num8 - ((Interval)(ref domain)).Max) < 0.0001)
				{
					double num9 = num5;
					domain = ((Curve)value2).Domain;
					if (Math.Abs(num9 - ((Interval)(ref domain)).Min) < 0.0001)
					{
						domain = ((Curve)value2).Domain;
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
