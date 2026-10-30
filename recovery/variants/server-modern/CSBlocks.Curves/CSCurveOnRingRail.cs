using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Curves;

[CSBlock("CurveOnRingRail", "{A201BFC3-CA86-4ce5-B13A-DCC84C763EE8}")]
public sealed class CSCurveOnRingRail : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iCurveAngle;

	public readonly CSInputChannel iCurveWidth;

	public readonly CSInputChannel iParam;

	public readonly CSInputChannel iYOffset;

	public readonly CSInputChannel iZRotate;

	public readonly CSOutputChannel oCurve;

	public CSCurveOnRingRail(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurveAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCurveWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParam = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Ring Rail");
		}
		NurbsCurve value2 = null;
		CSDataHelper.GetNurbsCurve(iCurve.Data, out value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing Curve");
		}
		double value3 = 0.0;
		if (!iCurveAngle.GetDouble(ref value3))
		{
			return UpdateError("Missing Curve Angle");
		}
		double value4 = 0.0;
		if (!iCurveWidth.GetDouble(ref value4))
		{
			return UpdateError("Missing Curve Width");
		}
		double value5 = 0.0;
		if (!iParam.GetDouble(ref value5))
		{
			return UpdateError("Missing Shank Position");
		}
		NurbsCurve obj = value2;
		Interval domain = ((Curve)value2).Domain;
		Vector3d val = ((Curve)obj).TangentAt(((Interval)(ref domain)).Min);
		if (((Vector3d)(ref val)).X < 0.0)
		{
			((Curve)value2).Reverse();
		}
		Point3d val2 = ((Curve)value2).PointAtStart;
		double y = ((Point3d)(ref val2)).Y;
		double value6 = 0.0;
		iZRotate.GetDouble(ref value6);
		value6 = CSBlockUtil.CSUtilMath.ToRadians(value6);
		Transform identity = Transform.Identity;
		identity = CSBlockUtil.CSUtilMath.Rotation(value6, Vector3d.ZAxis, ((Curve)value2).PointAtStart);
		((GeometryBase)value2).Transform(identity);
		BoundingBox boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return UpdateError("Invalid Curve Boudning Box");
		}
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num = x - ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = y2 - ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		Transform val3 = Transform.Translation(new Vector3d(0.0 - (((Point3d)(ref val2)).X - num / 2.0), 0.0, 0.0));
		((GeometryBase)value2).Transform(val3);
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)value2).Transform(val4);
		boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return UpdateError("Invalid Tight Curve Boudning Box");
		}
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double x2 = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		num = x2 - ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double y3 = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		num2 = y3 - ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num3 = 0.0 - ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		Transform val5 = Transform.Translation(num3, 0.0 - ((Point3d)(ref val2)).Y, 0.0);
		((GeometryBase)value2).Transform(val5);
		LineCurve val6 = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, 50.0));
		((GeometryBase)val6).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.YAxis, Point3d.Origin);
		CurveIntersections val7 = Intersection.CurveCurve((Curve)(object)value, (Curve)(object)val6, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val7.Count == 0)
		{
			return UpdateError("Rail doesn't Intersect curve");
		}
		Surface val8 = Surface.CreateExtrusion((Curve)(object)((Curve)value).Trim(new Interval(0.0, val7[0].ParameterA)).ToNurbsCurve(), Vector3d.YAxis * (0.0 - value4));
		if (val8 == null)
		{
			return UpdateError("Error Extrude");
		}
		Transform val9 = Transform.Translation(new Vector3d(0.0, value4 / 2.0, 0.0));
		((GeometryBase)val8).Transform(val9);
		Interval val10 = val8.Domain(0);
		Interval val11 = val8.Domain(1);
		Transform val12 = Transform.Scale(Plane.WorldXY, (((Interval)(ref val10))[1] - ((Interval)(ref val10))[0]) / num, (((Interval)(ref val11))[1] - ((Interval)(ref val11))[0]) / num2, 1.0);
		((GeometryBase)value2).Transform(val12);
		Transform val13 = Transform.Translation(((Interval)(ref val10))[0], ((Interval)(ref val11))[0], 0.0);
		((GeometryBase)value2).Transform(val13);
		Curve val14 = val8.Pushup(((Curve)value2).DuplicateCurve(), CSApplicationHelper.Application.Tolerance);
		if (val14 == null)
		{
			return UpdateError("Error Push Up");
		}
		val14.ChangeDimension(3);
		val2 = val14.PointAtStart;
		double y4 = ((Point3d)(ref val2)).Y;
		Transform val15 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0), Vector3d.XAxis, Point3d.Origin);
		((GeometryBase)val14).Transform(val15);
		Transform val16 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(-90.0), Vector3d.YAxis, Point3d.Origin);
		((GeometryBase)val14).Transform(val16);
		BoundingBox boundingBox2 = ((GeometryBase)val14).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox2)).IsValid)
		{
			return UpdateError("Error Output Bounding Box");
		}
		val2 = ((BoundingBox)(ref boundingBox2)).Max;
		Transform val17 = Transform.Translation(0.0, 0.0 - ((Point3d)(ref val2)).Y, 0.0);
		((GeometryBase)val14).Transform(val17);
		Transform x3 = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp((Curve)(object)value, value5, ref x3, 1.0);
		double value7 = 0.0;
		iYOffset.GetDouble(ref value7);
		Transform identity2 = Transform.Identity;
		identity2 = Transform.Translation(0.0 - y4 + y + value7, 0.0, 0.0);
		Transform val18 = default(Transform);
		val18 = x3 * identity2;
		((GeometryBase)val14).Transform(val18);
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val14, val18, applyTransform: false));
		return base.Update();
	}
}
