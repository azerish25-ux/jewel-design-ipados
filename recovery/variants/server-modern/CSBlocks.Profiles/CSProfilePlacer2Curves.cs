using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacer2Curves", "{77B462BA-C38F-49CD-B9E1-D0212C8BD1EF}")]
public sealed class CSProfilePlacer2Curves : CSBlock
{
	public readonly CSInputChannel iRailBase;

	public readonly CSInputChannel iRailOutside;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iParameter;

	public readonly CSInputChannel iMethod;

	public readonly CSInputChannel iWidth;

	public readonly CSInputChannel iHeightAdd;

	public readonly CSInputChannel iTransform;

	public readonly CSOutputChannel oProfile;

	public CSProfilePlacer2Curves(string identifier)
		: base(identifier)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		iRailBase = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRailOutside = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.OrientationMethodClosestPointOnCurve);
		iWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5.0);
		iHeightAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		iTransform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Identity);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		if (!iRailBase.GetValue(out Curve value))
		{
			return InputError();
		}
		if (!iRailOutside.GetValue(out Curve value2))
		{
			return InputError();
		}
		if (!iProfile.GetValue(out Curve value3))
		{
			return InputError();
		}
		if (!iParameter.GetValue(out double value4))
		{
			return InputError();
		}
		if (!iMethod.GetValue(out int value5))
		{
			return InputError();
		}
		if (!iWidth.GetValue(out double value6))
		{
			return InputError();
		}
		if (!iHeightAdd.GetValue(out double value7))
		{
			return InputError();
		}
		if (!iTransform.GetValue(out Transform value8))
		{
			return InputError();
		}
		if (value4 < 0.0)
		{
			value4 = 0.0;
		}
		if (value4 > 1.0)
		{
			value4 = 1.0;
		}
		double num = default(double);
		value.NormalizedLengthParameter(value4, ref num);
		Point3d val = value.PointAt(num);
		Vector3d val2 = value.TangentAt(num);
		Point3d val3;
		Vector3d val6;
		if (value5 == 140)
		{
			double num2 = default(double);
			value2.ClosestPoint(val, ref num2);
			val3 = value2.PointAt(num2);
		}
		else
		{
			Plane val4 = default(Plane);
			((Plane)(ref val4))._002Ector(val, val2);
			CurveIntersections obj = Intersection.CurvePlane(value2, val4, CSApplicationHelper.Application.Tolerance);
			double num3 = double.MaxValue;
			Point3d val5 = Point3d.Unset;
			foreach (IntersectionEvent item in obj)
			{
				if (item.IsPoint)
				{
					Point3d pointA = item.PointA;
					val6 = val - pointA;
					double length = ((Vector3d)(ref val6)).Length;
					if (length < num3)
					{
						num3 = length;
						val5 = pointA;
					}
				}
			}
			if (!((Point3d)(ref val5)).IsValid)
			{
				double num4 = default(double);
				value2.ClosestPoint(val, ref num4);
				val3 = value2.PointAt(num4);
			}
			else
			{
				val3 = val5;
			}
		}
		Vector3d val7 = val3 - val;
		double length2 = ((Vector3d)(ref val7)).Length;
		((Vector3d)(ref val7)).Unitize();
		Vector3d destination = Vector3d.CrossProduct(val7, val2);
		Transform val8 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, val, destination, val7);
		BoundingBox boundingBox = ((GeometryBase)value3).GetBoundingBox(true);
		Point3d min = ((BoundingBox)(ref boundingBox)).Min;
		double num5 = 0.0 - ((Point3d)(ref min)).Y;
		min = ((BoundingBox)(ref boundingBox)).Min;
		Transform val9 = Transform.Translation(0.0, num5, 0.0 - ((Point3d)(ref min)).Z);
		Plane worldXY = Plane.WorldXY;
		double num6 = value6;
		val6 = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num7 = num6 / ((Vector3d)(ref val6)).X;
		double num8 = length2 + value7;
		val6 = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val10 = Transform.Scale(worldXY, num7, num8 / ((Vector3d)(ref val6)).Y, 1.0);
		Transform val11 = val8 * value8 * val10 * val9;
		Curve val12 = value3.DuplicateCurve();
		((GeometryBase)val12).Transform(val11);
		oProfile.SetData(new CSOnGeometryData((GeometryBase)(object)val12));
		return base.Update();
	}
}
