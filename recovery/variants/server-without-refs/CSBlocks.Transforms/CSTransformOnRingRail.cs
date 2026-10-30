using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TransformOnRingRail", "{287B55DE-A069-431B-B2D1-658BFAC765A1}")]
public sealed class CSTransformOnRingRail : CSBlock
{
	public readonly CSInputChannel Curve;

	public readonly CSInputChannel GemZOffset;

	public readonly CSInputChannel GemYOffset;

	public readonly CSInputChannel GemTilt;

	public readonly CSInputChannel GemAngle;

	public readonly CSInputChannel GemRotation;

	public readonly CSInputChannel GemPosition;

	public readonly CSOutputChannel oTransform;

	public CSTransformOnRingRail(string identifier)
		: base(identifier)
	{
		Curve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		GemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		GemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		GemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		GemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		GemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		GemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oTransform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		CSData data = Curve.Data;
		if (data == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetNurbsCurve(data, out var value);
		if (value == null)
		{
			return UpdateMissing("Curve");
		}
		double value2 = 0.0;
		GemZOffset.GetDouble(ref value2);
		double value3 = 0.0;
		GemYOffset.GetDouble(ref value3);
		double value4 = 0.0;
		GemTilt.GetDouble(ref value4);
		value4 /= 100.0;
		double value5 = 0.0;
		GemAngle.GetDouble(ref value5);
		value5 = CSBlockUtil.CSUtilMath.ToRadians(value5);
		double value6 = 0.0;
		GemRotation.GetDouble(ref value6);
		value6 = CSBlockUtil.CSUtilMath.ToRadians(value6);
		double value7 = 0.0;
		GemPosition.GetDouble(ref value7);
		CSXformData data2 = new CSXformData();
		CSDataHelper.GetTransformNoArray(data, out var value8);
		if (!((Transform)(ref value8)).IsValid)
		{
			value8 = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(value8);
		Vector3d val = default(Vector3d);
		Interval domain;
		Vector3d val3;
		if (data is CSOutsideRingRailData || data is CSRingRailData)
		{
			((Vector3d)(ref val))._002Ector(Vector3d.ZAxis);
			((Vector3d)(ref val)).Transform(value8);
			((Vector3d)(ref val)).Unitize();
			Vector3d val2 = val;
			NurbsCurve obj = value;
			domain = ((Curve)value).Domain;
			val3 = Vector3d.CrossProduct(val2, ((Curve)obj).TangentAt(((Interval)(ref domain)).Min));
			((Vector3d)(ref val3)).Unitize();
		}
		else
		{
			((Vector3d)(ref val))._002Ector(Vector3d.YAxis);
			((Vector3d)(ref val)).Transform(value8);
			((Vector3d)(ref val)).Unitize();
			NurbsCurve obj2 = value;
			domain = ((Curve)value).Domain;
			val3 = Vector3d.CrossProduct(((Curve)obj2).TangentAt(((Interval)(ref domain)).Min), val);
			((Vector3d)(ref val3)).Unitize();
		}
		double num = 0.0;
		Curve val4 = CSUtilGeometry.Offset((Curve)(object)value, val3, origin, 0.0 - (num + value2), (CurveOffsetCornerStyle)1);
		if (val4 == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = val4.GetLength();
		if (double.IsNaN(length) || length <= 0.0)
		{
			return UpdateError("Offset Curve Length");
		}
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(value5, Vector3d.XAxis, Point3d.Origin);
		Transform val6 = Transform.Translation(0.0, value3, 0.0);
		Transform val7 = CSBlockUtil.CSUtilMath.Rotation(value6, Vector3d.ZAxis, Point3d.Origin);
		Transform val8 = val5 * val7;
		if (1 > 0)
		{
			double num2 = value7;
			if (num2 > 1.0)
			{
				num2 -= 1.0;
			}
			if (num2 < 0.0)
			{
				num2 += 1.0;
			}
			double num3 = default(double);
			if (!val4.NormalizedLengthParameter(num2, ref num3))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			Vector3d val9 = Vector3d.CrossProduct(val4.TangentAt(num3), val3);
			((Vector3d)(ref val9)).Unitize();
			Vector3d destination = val9 * value4 + val * (1.0 - value4);
			if (((Vector3d)(ref destination)).Length < 0.001)
			{
				((Vector3d)(ref destination))._002Ector(Vector3d.ZAxis);
			}
			else
			{
				((Vector3d)(ref destination)).Unitize();
			}
			data2 = new CSXformData(CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.YAxis, Vector3d.ZAxis, val4.PointAt(num3), val3, destination) * val6 * val8);
		}
		oTransform.SetData(data2);
		return base.Update();
	}
}
