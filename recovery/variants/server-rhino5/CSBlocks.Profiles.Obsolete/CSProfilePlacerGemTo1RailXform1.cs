using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacerGemTo1RailXform_V1", "{A03F108F-09AD-45b4-9CB8-0EA2AFA60E30}")]
public sealed class CSProfilePlacerGemTo1RailXform1 : CSBlock
{
	public CSInputChannel iRail;

	public CSInputChannel iGem;

	public CSInputChannel iParameter;

	public CSInputChannel iMethod;

	public CSOutputChannel oXform;

	public CSOutputChannel oParam;

	public CSProfilePlacerGemTo1RailXform1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRail.GetFlattenArray().GetArrayItem(0), out var value);
		if (value == null)
		{
			return UpdateMissing("Profile Placer simple: Missing Rail");
		}
		CSData arrayItem = iGem.GetFlattenArray().GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateMissing("Profile Placer: Missing Gem");
		}
		double value2 = 0.0;
		if (!iParameter.GetDouble(ref value2))
		{
			return UpdateMissing("ProfilePlacerSimple: Unable to find parameter");
		}
		int value3 = 0;
		iMethod.GetInteger(ref value3);
		CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
		if (!value4.IsValid)
		{
			value4 = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		origin.Transform(value4);
		if (!value.ClosestPoint(origin, out var t))
		{
			return UpdateError("Get Closest Point Failed");
		}
		double length = value.GetLength(0.01, new Interval(value.Domain.Min, t));
		if (double.IsNaN(length) || length < 0.0)
		{
			return UpdateError("Get Length Failed");
		}
		length += value2;
		double length2 = value.GetLength();
		if (double.IsNaN(length2) || length2 < 0.0)
		{
			return UpdateError("Get Length Failed");
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
			return UpdateError("Get Arc Length Failed");
		}
		oParam.SetData(new CSNumberData(t2));
		Point3d point3d = value.PointAt(t2);
		Vector3d destination = value.TangentAt(t2);
		Transform transform = default(Transform);
		switch (value3)
		{
		case 0:
		{
			Vector3d destination2 = new Vector3d(point3d * -1.0);
			destination2.Unitize();
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), point3d, destination, destination2);
			break;
		}
		case 1:
		{
			Vector3d vector3d = new Vector3d(point3d * -1.0);
			vector3d.Y = 0.0;
			Vector3d destination2 = vector3d;
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
			if (value.TryGetPlane(out var plane2))
			{
				destination2 = new Vector3d(plane2.Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), point3d, destination, destination2);
			break;
		}
		case 4:
		{
			Vector3d destination2 = new Vector3d(0.0, 0.0, -1.0);
			if (value.TryGetPlane(out var plane))
			{
				destination2 = new Vector3d(plane.Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, -1.0), point3d, destination, destination2);
			break;
		}
		}
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
