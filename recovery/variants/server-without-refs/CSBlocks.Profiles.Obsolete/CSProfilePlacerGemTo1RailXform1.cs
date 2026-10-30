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
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
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
		if (!((Transform)(ref value4)).IsValid)
		{
			value4 = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(value4);
		double num = default(double);
		if (!((Curve)value).ClosestPoint(origin, ref num))
		{
			return UpdateError("Get Closest Point Failed");
		}
		NurbsCurve obj = value;
		Interval domain = ((Curve)value).Domain;
		double length = ((Curve)obj).GetLength(0.01, new Interval(((Interval)(ref domain)).Min, num));
		if (double.IsNaN(length) || length < 0.0)
		{
			return UpdateError("Get Length Failed");
		}
		length += value2;
		double length2 = ((Curve)value).GetLength();
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
		double num2 = default(double);
		if (!((Curve)value).NormalizedLengthParameter(length / length2, ref num2))
		{
			return UpdateError("Get Arc Length Failed");
		}
		oParam.SetData(new CSNumberData(num2));
		Point3d val = ((Curve)value).PointAt(num2);
		Vector3d destination = ((Curve)value).TangentAt(num2);
		Transform transform = default(Transform);
		Vector3d destination2 = default(Vector3d);
		switch (value3)
		{
		case 0:
			((Vector3d)(ref destination2))._002Ector(val * -1.0);
			((Vector3d)(ref destination2)).Unitize();
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val, destination, destination2);
			break;
		case 1:
		{
			Vector3d val3 = default(Vector3d);
			((Vector3d)(ref val3))._002Ector(val * -1.0);
			((Vector3d)(ref val3)).Y = 0.0;
			destination2 = val3;
			((Vector3d)(ref destination2)).Unitize();
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val, destination, destination2);
			break;
		}
		case 2:
			((Vector3d)(ref destination2))._002Ector(0.0, 0.0, -1.0);
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val, destination, destination2);
			break;
		case 3:
		{
			((Vector3d)(ref destination2))._002Ector(0.0, 0.0, -1.0);
			Plane val4 = default(Plane);
			if (((Curve)value).TryGetPlane(ref val4))
			{
				((Vector3d)(ref destination2))._002Ector(((Plane)(ref val4)).Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(0.0, -1.0, 0.0), val, destination, destination2);
			break;
		}
		case 4:
		{
			((Vector3d)(ref destination2))._002Ector(0.0, 0.0, -1.0);
			Plane val2 = default(Plane);
			if (((Curve)value).TryGetPlane(ref val2))
			{
				((Vector3d)(ref destination2))._002Ector(((Plane)(ref val2)).Normal);
			}
			transform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, -1.0), val, destination, destination2);
			break;
		}
		}
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
