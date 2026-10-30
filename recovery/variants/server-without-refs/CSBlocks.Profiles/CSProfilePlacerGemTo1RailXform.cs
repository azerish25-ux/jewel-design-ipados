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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
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
		NurbsCurve val = value.ToNurbsCurve();
		Transform val2 = value2;
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(Point3d.Origin);
		((Point3d)(ref val3)).Transform(val2);
		double num = default(double);
		if (!((Curve)val).ClosestPoint(val3, ref num))
		{
			return UpdateError("GetClosestPoint to rail failed");
		}
		Interval domain = ((Curve)val).Domain;
		double length = ((Curve)val).GetLength(0.01, new Interval(((Interval)(ref domain)).Min, num));
		if (double.IsNaN(length) || length < 0.0)
		{
			return UpdateError("GetLength to gemPoint failed");
		}
		length += value3;
		double length2 = ((Curve)val).GetLength();
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
		double num2 = default(double);
		if (!((Curve)val).NormalizedLengthParameter(length / length2, ref num2))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		Point3d val4 = ((Curve)val).PointAt(num2);
		Vector3d destination = ((Curve)val).TangentAt(num2);
		Transform transform = default(Transform);
		Vector3d destination2 = default(Vector3d);
		switch (value4)
		{
		case 0:
			destination2 = (Vector3d)val4 * -1.0;
			((Vector3d)(ref destination2)).Unitize();
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val4, destination, destination2);
			break;
		case 1:
			destination2 = (Vector3d)val4 * -1.0;
			((Vector3d)(ref destination2)).Y = 0.0;
			((Vector3d)(ref destination2)).Unitize();
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val4, destination, destination2);
			break;
		case 2:
			((Vector3d)(ref destination2))._002Ector(0.0, 0.0, -1.0);
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val4, destination, destination2);
			break;
		case 3:
		{
			((Vector3d)(ref destination2))._002Ector(0.0, 0.0, -1.0);
			Plane val6 = default(Plane);
			if (((Curve)val).TryGetPlane(ref val6))
			{
				((Vector3d)(ref destination2))._002Ector(((Plane)(ref val6)).Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val4, destination, destination2);
			break;
		}
		case 4:
		{
			((Vector3d)(ref destination2))._002Ector(0.0, 0.0, -1.0);
			Plane val5 = default(Plane);
			if (((Curve)val).TryGetPlane(ref val5))
			{
				((Vector3d)(ref destination2))._002Ector(((Plane)(ref val5)).Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, -1.0), val4, destination, destination2);
			break;
		}
		}
		oXform.SetData(new CSXformData(transform));
		oParam.SetData(new CSNumberData(num2));
		return base.Update();
	}
}
