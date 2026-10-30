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
		if (!value8.IsValid)
		{
			value8 = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		origin.Transform(value8);
		Vector3d vector3d;
		Vector3d vector3d2;
		if (data is CSOutsideRingRailData || data is CSRingRailData)
		{
			vector3d = new Vector3d(Vector3d.ZAxis);
			vector3d.Transform(value8);
			vector3d.Unitize();
			vector3d2 = Vector3d.CrossProduct(vector3d, value.TangentAt(value.Domain.Min));
			vector3d2.Unitize();
		}
		else
		{
			vector3d = new Vector3d(Vector3d.YAxis);
			vector3d.Transform(value8);
			vector3d.Unitize();
			vector3d2 = Vector3d.CrossProduct(value.TangentAt(value.Domain.Min), vector3d);
			vector3d2.Unitize();
		}
		double num = 0.0;
		Curve curve = CSUtilGeometry.Offset(value, vector3d2, origin, 0.0 - (num + value2));
		if (curve == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = curve.GetLength();
		if (double.IsNaN(length) || length <= 0.0)
		{
			return UpdateError("Offset Curve Length");
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value5, Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = Transform.Translation(0.0, value3, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(value6, Vector3d.ZAxis, Point3d.Origin);
		Transform transform4 = transform * transform3;
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
			if (!curve.NormalizedLengthParameter(num2, out var t))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			Vector3d vector3d3 = Vector3d.CrossProduct(curve.TangentAt(t), vector3d2);
			vector3d3.Unitize();
			Vector3d destination = vector3d3 * value4 + vector3d * (1.0 - value4);
			if (destination.Length < 0.001)
			{
				destination = new Vector3d(Vector3d.ZAxis);
			}
			else
			{
				destination.Unitize();
			}
			data2 = new CSXformData(CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.YAxis, Vector3d.ZAxis, curve.PointAt(t), vector3d2, destination) * transform2 * transform4);
		}
		oTransform.SetData(data2);
		return base.Update();
	}
}
