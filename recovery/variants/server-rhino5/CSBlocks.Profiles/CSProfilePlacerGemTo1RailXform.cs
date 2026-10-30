using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacerGemTo1RailXform", "{9F0756CF-DB84-46AB-879B-D516C3F83D46}")]
public sealed class CSProfilePlacerGemTo1RailXform : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iParameter;

	public readonly CSInputChannel iMethod;

	public readonly CSOutputChannel oXform;

	public readonly CSOutputChannel oParam;

	public CSProfilePlacerGemTo1RailXform(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		iMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!iRail.GetValue(out Curve value))
		{
			return InputError();
		}
		if (!iGem.GetValue(out Transform value2))
		{
			return InputError();
		}
		if (!iParameter.GetValue(out double value3))
		{
			return InputError();
		}
		if (!iMethod.GetValue(out int value4))
		{
			return InputError();
		}
		NurbsCurve nurbsCurve = value.ToNurbsCurve();
		Transform xform = value2;
		Point3d testPoint = new Point3d(Point3d.Origin);
		testPoint.Transform(xform);
		if (!nurbsCurve.ClosestPoint(testPoint, out var t))
		{
			return UpdateError("GetClosestPoint to rail failed");
		}
		double length = nurbsCurve.GetLength(0.01, new Interval(nurbsCurve.Domain.Min, t));
		if (double.IsNaN(length) || length < 0.0)
		{
			return UpdateError("GetLength to gemPoint failed");
		}
		length += value3;
		double length2 = nurbsCurve.GetLength();
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
		if (!nurbsCurve.NormalizedLengthParameter(length / length2, out var t2))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		Point3d point3d = nurbsCurve.PointAt(t2);
		Vector3d destination = nurbsCurve.TangentAt(t2);
		Transform transform = default(Transform);
		switch (value4)
		{
		case 0:
		{
			Vector3d destination2 = (Vector3d)point3d * -1.0;
			destination2.Unitize();
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), point3d, destination, destination2);
			break;
		}
		case 1:
		{
			Vector3d destination2 = (Vector3d)point3d * -1.0;
			destination2.Y = 0.0;
			destination2.Unitize();
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), point3d, destination, destination2);
			break;
		}
		case 2:
			transform = CSBlockUtil.CSUtilMath.Orient3Point(destination2: new Vector3d(0.0, 0.0, -1.0), source0: new Point3d(0.0, 0.0, 0.0), source1: new Vector3d(0.0, 0.0, 1.0), source2: new Vector3d(0.0, -1.0, 0.0), destination0: point3d, destination1: destination);
			break;
		case 3:
		{
			Vector3d destination2 = new Vector3d(0.0, 0.0, -1.0);
			if (nurbsCurve.TryGetPlane(out var plane2))
			{
				destination2 = new Vector3d(plane2.Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), point3d, destination, destination2);
			break;
		}
		case 4:
		{
			Vector3d destination2 = new Vector3d(0.0, 0.0, -1.0);
			if (nurbsCurve.TryGetPlane(out var plane))
			{
				destination2 = new Vector3d(plane.Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, -1.0), point3d, destination, destination2);
			break;
		}
		}
		oXform.SetData(new CSXformData(transform));
		oParam.SetData(new CSNumberData(t2));
		return base.Update();
	}
}
