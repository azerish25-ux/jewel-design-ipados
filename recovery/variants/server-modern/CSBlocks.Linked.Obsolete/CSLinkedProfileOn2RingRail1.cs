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
[CSBlock("LinkedProfileOn2RingRail_V1", "{310FF09C-A24C-447f-A169-D36556CDDBEF}")]
public sealed class CSLinkedProfileOn2RingRail1 : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iOutsideRail;

	public readonly CSInputChannel iOutsideRailScale;

	public readonly CSInputChannel iAOffset;

	public readonly CSInputChannel iLastRightRailPoint;

	public readonly CSInputChannel iLastRightRailVector;

	public readonly CSOutputChannel oXform;

	public readonly CSOutputChannel oParam;

	public readonly CSOutputChannel oParam2;

	public readonly CSOutputChannel oYSize;

	public readonly CSOutputChannel oRightRailPoint;

	public readonly CSOutputChannel oRightRailVector;

	public CSLinkedProfileOn2RingRail1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRailScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		oRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		CSDataHelper.GetNurbsCurve(iOutsideRail.Data, out var value2);
		double value3 = 1.0;
		iOutsideRailScale.GetDouble(ref value3);
		double value4 = 0.0;
		iAOffset.GetDouble(ref value4);
		bool flag = true;
		Point3d value5 = Point3d.Unset;
		CSDataHelper.GetPoint(iLastRightRailPoint.Data, out value5);
		if (value5 == Point3d.Unset)
		{
			flag = false;
		}
		Vector3d value6 = Vector3d.Unset;
		CSDataHelper.GetVector(iLastRightRailVector.Data, out value6);
		if (value6 == Vector3d.Unset)
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
		Vector3d inputVC = default(Vector3d);
		inputVC = val2 - val;
		((Vector3d)(ref inputVC)).Unitize();
		double num = value4;
		Vector3d val5 = new Vector3d(val);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(num / ((Vector3d)(ref val5)).Length, Vector3d.YAxis, Point3d.Origin);
		Transform val7;
		if (flag)
		{
			val5 = new Vector3d(value5);
			double length = ((Vector3d)(ref val5)).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(val, inputVC, length, ref intersectPoint);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value5, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			val7 = Transform.Identity;
		}
		val7 *= val6;
		((Point3d)(ref val3)).Transform(val7);
		((Point3d)(ref val4)).Transform(val7);
		Vector3d vectorStore = default(Vector3d);
		vectorStore = val4 - val3;
		((Vector3d)(ref vectorStore)).Unitize();
		double num2 = 0.0;
		((Curve)value).ClosestPoint(val3, ref num2);
		Transform x2 = default(Transform);
		CSUtilGeometry.OrientProfileToCurve((Curve)(object)value, num2, ref x2, value3 / 2.0);
		oXform.SetData(new CSXformData(x2));
		if (value2 != null)
		{
			LineCurve val8 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			((GeometryBase)val8).Transform(x2);
			CurveIntersections val9 = Intersection.CurveCurve((Curve)(object)val8, (Curve)(object)value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val9.Count > 0)
			{
				Point3d val10 = default(Point3d);
				Point3d pointAtStart = ((Curve)val8).PointAtStart;
				val10 = val9[0].PointA;
				double num3 = ((Point3d)(ref pointAtStart)).DistanceTo(val10);
				if (num3 < 0.01)
				{
					num3 = 0.01;
				}
				oYSize.SetData(new CSNumberData(num3));
				double num4 = 0.0;
				num4 = val9[0].ParameterB;
				double num5 = num2;
				Interval domain = ((Curve)value).Domain;
				if (Math.Abs(num5 - ((Interval)(ref domain)).Min) < 0.0001)
				{
					double num6 = num4;
					domain = ((Curve)value2).Domain;
					if (Math.Abs(num6 - ((Interval)(ref domain)).Max) < 0.0001)
					{
						domain = ((Curve)value2).Domain;
						num4 = ((Interval)(ref domain)).Min;
					}
				}
				double num7 = num2;
				domain = ((Curve)value).Domain;
				if (Math.Abs(num7 - ((Interval)(ref domain)).Max) < 0.0001)
				{
					double num8 = num4;
					domain = ((Curve)value2).Domain;
					if (Math.Abs(num8 - ((Interval)(ref domain)).Min) < 0.0001)
					{
						domain = ((Curve)value2).Domain;
						num4 = ((Interval)(ref domain)).Max;
					}
				}
				oParam2.SetData(new CSNumberData(num4));
			}
		}
		oXform.SetData(new CSXformData(val7 * x));
		oParam.SetData(new CSNumberData(num2));
		oRightRailPoint.SetData(new CSPointData(val3));
		oRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
