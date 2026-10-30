using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Rails;

[CSBlock("RingRailOpen", "{682CFB86-0EBF-4a50-AEB2-710CE3644040}")]
public sealed class CSRingRailOpen : CSBlock
{
	public readonly CSInputChannel IRingRadius;

	public readonly CSInputChannel ICurve;

	public readonly CSInputChannel ISpan;

	public readonly CSInputChannel IBlend;

	public readonly CSInputChannel IWidthLock;

	public readonly CSInputChannel IWidth;

	public readonly CSInputChannel IMirror;

	public readonly CSOutputChannel ORingRail;

	public CSRingRailOpen(string identifier)
		: base(identifier)
	{
		IRingRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		ICurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		ISpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		IBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		IWidthLock = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		IWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		IMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		ORingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		double value = 0.0;
		if (!IRingRadius.GetDouble(ref value))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Ring Rail\n");
		}
		CSDataHelper.GetNurbsCurve(ICurve.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Ring Rail Custom:  Missing Curve\n");
		}
		double value3 = 0.0;
		if (!ISpan.GetDouble(ref value3))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Span\n");
		}
		double value4 = 0.0;
		if (!IBlend.GetDouble(ref value4))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Blend\n");
		}
		bool value5 = false;
		IMirror.GetBoolean(ref value5);
		bool value6 = false;
		IWidthLock.GetBoolean(ref value6);
		double value7 = 0.0;
		IWidth.GetDouble(ref value7);
		Transform val = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		((GeometryBase)value2).Transform(val);
		Point3d val2 = ((Curve)value2).PointAtStart;
		double x = ((Point3d)(ref val2)).X;
		val2 = ((Curve)value2).PointAtEnd;
		if (x > ((Point3d)(ref val2)).X)
		{
			((Curve)value2).Reverse();
		}
		BoundingBox boundingBox = ((GeometryBase)value2).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double x2 = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num = x2 - ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		_ = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		_ = ((Point3d)(ref val2)).Y;
		value3 = value3 / 2.0 / 10.0;
		value4 /= 10.0;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = 0.0 - ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		val = Transform.Translation(num2, 0.0, 0.0 - ((Point3d)(ref val2)).Z);
		((GeometryBase)value2).Transform(val);
		val = Transform.Scale(Plane.WorldXY, value4 / num, 1.0, 0.0);
		((GeometryBase)value2).Transform(val);
		val = Transform.Translation(value3, 0.0, 0.0);
		((GeometryBase)value2).Transform(val);
		NurbsCurve val3 = new NurbsCurve(value2);
		val = ((!value5) ? Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0) : Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0));
		((GeometryBase)val3).Transform(val);
		val = Transform.Translation(36.0, 0.0, 0.0);
		((GeometryBase)val3).Transform(val);
		Point3d val4 = default(Point3d);
		Vector3d val5 = default(Vector3d);
		Vector3d val6 = default(Vector3d);
		Point3d pointAtEnd = ((Curve)value2).PointAtEnd;
		val4 = ((Curve)val3).PointAtEnd;
		NurbsCurve obj = value2;
		Interval domain = ((Curve)value2).Domain;
		val5 = ((Curve)obj).TangentAt(((Interval)(ref domain)).Max);
		domain = ((Curve)val3).Domain;
		val6 = ((Curve)val3).TangentAt(((Interval)(ref domain)).Max);
		NurbsCurve val7 = CSUtilCurves.BlendCurve(pointAtEnd, val4, val5, val6);
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3]
		{
			(Curve)value2,
			(Curve)val7,
			(Curve)val3
		});
		if (array.Length != 1)
		{
			return UpdateError("RhinoMergeCurves failed");
		}
		value2 = array[0].ToNurbsCurve();
		Transform val8 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)value2).Transform(val8);
		BoundingBox boundingBox2 = ((GeometryBase)value2).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox2)).IsValid)
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		val2 = ((BoundingBox)(ref boundingBox2)).Max;
		double x3 = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox2)).Min;
		double num3 = x3 - ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox2)).Min;
		Transform val9 = Transform.Translation(0.0 - ((Point3d)(ref val2)).X, 0.0, 0.0);
		((GeometryBase)value2).Transform(val9);
		Circle val10 = default(Circle);
		((Circle)(ref val10))._002Ector(Plane.WorldZX, Point3d.Origin, value);
		Surface val11 = Surface.CreateExtrusion((Curve)(object)((Circle)(ref val10)).ToNurbsCurve(), Vector3d.YAxis * (0.0 - num3));
		if (val11 == null)
		{
			return UpdateError("RhinoExtrudeCurveStraight failed");
		}
		val2 = ((BoundingBox)(ref boundingBox2)).Min;
		val = Transform.Translation(0.0, 0.0 - ((Point3d)(ref val2)).X, 0.0);
		((GeometryBase)val11).Transform(val);
		val = ((!value6) ? Transform.Scale(Plane.WorldXY, 1.0, value7 / num3, 1.0) : Transform.Scale(Plane.WorldXY, 1.0, 1.0, 1.0));
		((GeometryBase)val11).Transform(val);
		Interval val12 = val11.Domain(0);
		Interval val13 = val11.Domain(1);
		Transform val14 = Transform.Scale(Plane.WorldXY, (((Interval)(ref val12))[1] - ((Interval)(ref val12))[0]) / num3, (((Interval)(ref val13))[1] - ((Interval)(ref val13))[0]) / 36.0, 1.0);
		((GeometryBase)value2).Transform(val14);
		Curve val15 = val11.Pushup((Curve)(object)value2, CSApplicationHelper.Application.Tolerance);
		if (val15 == null)
		{
			return UpdateError("Pushup failed");
		}
		val15.ChangeDimension(3);
		ORingRail.SetData(new CSRingRailData(val15, value));
		return base.Update();
	}
}
