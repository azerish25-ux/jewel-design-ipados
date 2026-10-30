using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveNormalDir", "{8952884D-213C-4C47-BAC1-63476266B895}")]
public sealed class CSCurveNormalDir : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iCurveMatch;

	public readonly CSInputChannel iPosition;

	public readonly CSInputChannel iDirection;

	public readonly CSOutputChannel oCurve;

	public CSCurveNormalDir(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurveMatch = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curves");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		double value2 = 0.0;
		if (iPosition != null)
		{
			iPosition.GetDouble(ref value2);
		}
		bool value3 = false;
		if (iDirection != null)
		{
			iDirection.GetBoolean(ref value3);
		}
		CSDataArray flattenArray2 = iCurveMatch.GetFlattenArray();
		Vector3d vector3d = Vector3d.Unset;
		if (flattenArray2.GetArrayCount() > 0)
		{
			CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value4);
			if (CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)value4, out Plane plane))
			{
				vector3d = plane.Normal;
			}
		}
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)value, out Plane plane2))
		{
			oCurve.SetData(null);
			return UpdateError("Input Curve not planar");
		}
		if (vector3d != Vector3d.Unset)
		{
			Vector3d normal = plane2.Normal;
			if (Vector3d.Multiply(vector3d, normal) < 0.0)
			{
				value.Reverse();
				plane2.Flip();
			}
		}
		double t = 0.0;
		if (!value.NormalizedLengthParameter(value2, out t))
		{
			oCurve.SetData(null);
			return UpdateError("Invalid point on crv");
		}
		Point3d point3d = value.PointAt(t);
		Vector3d vector3d2 = new Vector3d(plane2.Normal);
		if (value3)
		{
			vector3d2 *= -1.0;
		}
		LineCurve geometry = new LineCurve(point3d, point3d + vector3d2);
		oCurve.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
