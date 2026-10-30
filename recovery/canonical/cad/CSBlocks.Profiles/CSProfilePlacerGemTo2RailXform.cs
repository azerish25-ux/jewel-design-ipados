using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacerGemTo2RailXform", "{D8FCA1AB-512C-4955-9C46-9E9A895DCDBE}")]
public sealed class CSProfilePlacerGemTo2RailXform : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iOutsideRail;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iParameter;

	public readonly CSOutputChannel oXform;

	public readonly CSOutputChannel oParam;

	public readonly CSOutputChannel oParam2;

	public readonly CSOutputChannel oYSize;

	public CSProfilePlacerGemTo2RailXform(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail");
		}
		CSDataHelper.GetNurbsCurve(iOutsideRail.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Outside Rail");
		}
		CSData data = iGem.Data;
		if (data == null)
		{
			return UpdateMissing("Gem");
		}
		double value3 = 0.0;
		iParameter.GetDouble(ref value3);
		CSDataHelper.GetTransformNoArray(data, out var value4);
		if (!value4.IsValid)
		{
			return UpdateError("Unable to get Xform from gem");
		}
		Point3d testPoint = new Point3d(Point3d.Origin);
		testPoint.Transform(value4);
		if (!value.ClosestPoint(testPoint, out var t))
		{
			return UpdateError("GetClosestPoint to rail failed");
		}
		double length = value.GetLength(0.01, new Interval(value.Domain.Min, t));
		if (double.IsNaN(length) || length < 0.0)
		{
			return UpdateError("GetLength to gemPoint failed");
		}
		length += value3;
		double length2 = value.GetLength();
		if (double.IsNaN(length2) || length2 < 0.0)
		{
			return UpdateError("GetLength of rail failed");
		}
		while (length >= length2)
		{
			length -= length2;
		}
		for (; length < 0.0; length += length2)
		{
		}
		if (!value.NormalizedLengthParameter(length / length2, out var t2))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		Point3d point3d = value.PointAt(t2);
		Vector3d vector3d = CSBlockUtil.CSUtilMath.AverageTangentAt(value, t2, CSApplicationHelper.Application.Tolerance);
		vector3d.Unitize();
		Vector3d vector3d2 = CSBlockUtil.CSUtilMath.AverageNormalAt(Vector3d.YAxis, vector3d);
		vector3d2.Unitize();
		Vector3d vector = new Vector3d(point3d);
		vector.Y = 0.0;
		if (Vector3d.Multiply(vector, vector3d2) < 0.0)
		{
			vector3d2.Reverse();
		}
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, point3d, vector3d, vector3d2);
		LineCurve curveB = new LineCurve(point3d, point3d + vector3d2 * 100.0);
		CurveIntersections curveIntersections = Intersection.CurveCurve(value2, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count <= 0)
		{
			return UpdateError("Unable to intersect outside rail");
		}
		double num = double.NaN;
		double num2 = double.NaN;
		for (int i = 0; i < curveIntersections.Count; i++)
		{
			Point3d pointA = curveIntersections[i].PointA;
			double parameterA = curveIntersections[i].ParameterA;
			double num3 = point3d.DistanceTo(pointA);
			if (!double.IsNaN(num) || !double.IsNaN(num2) || !(num3 >= num2))
			{
				num = parameterA;
				num2 = num3;
			}
		}
		oXform.SetData(new CSXformData(transform));
		oParam.SetData(new CSNumberData(t2));
		oParam2.SetData(new CSNumberData(num));
		oYSize.SetData(new CSNumberData(num2));
		return base.Update();
	}
}
