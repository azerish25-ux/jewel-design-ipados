using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("BypassHeadProngBottom0", "{64886FE0-CF63-44c0-A7C8-08AF07D2AF2B}")]
public sealed class CSBypassHeadProngBottom0 : CSBlock
{
	public readonly CSInputChannel InBottomCenterSize;

	public readonly CSInputChannel InBottomCenterOffset;

	public readonly CSInputChannel InBottomCenterAngle;

	public readonly CSInputChannel InCenterProngSize;

	public readonly CSInputChannel InCenterGemSize;

	public readonly CSInputChannel InSideGemSize1;

	public readonly CSInputChannel InSideGemSize2;

	public readonly CSInputChannel InRailRadius;

	public readonly CSOutputChannel OutProngCurve0;

	public readonly CSOutputChannel OutProngCurve1;

	public readonly CSOutputChannel OutProngCurve2;

	public readonly CSOutputChannel OutProngCurve3;

	public CSBypassHeadProngBottom0(string identifier)
		: base(identifier)
	{
		InBottomCenterSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBottomCenterOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBottomCenterAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterProngSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterGemSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSideGemSize1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSideGemSize2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutProngCurve0 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve1 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve2 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve3 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		double num = InBottomCenterSize.Double;
		double num2 = InBottomCenterOffset.Double;
		double num3 = InBottomCenterAngle.Double;
		double num4 = InCenterProngSize.Double;
		double num5 = InCenterGemSize.Double;
		double num6 = InSideGemSize1.Double;
		double num7 = InSideGemSize2.Double;
		double num8 = InRailRadius.Double;
		double num9 = (num6 + num7) / 2.0;
		double num10 = (num4 / num5 * num9 + num4) / 2.0;
		Circle val = default(Circle);
		((Circle)(ref val))._002Ector(new Plane(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0)), num10);
		NurbsCurve val2 = ((Circle)(ref val)).ToNurbsCurve();
		NurbsCurve val3 = ((Curve)val2).ToNurbsCurve();
		NurbsCurve val4 = ((Curve)val2).ToNurbsCurve();
		NurbsCurve val5 = ((Curve)val2).ToNurbsCurve();
		Transform val6 = default(Transform);
		Transform val7 = default(Transform);
		val7 = Transform.Translation(0.0, num - num2, num8 + num10);
		Transform val8 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		val6 = val8 * val7;
		((GeometryBase)val2).Transform(val6);
		val7 = Transform.Translation(0.0, 0.0 - num - num2, num8 + num10);
		val6 = val8 * val7;
		((GeometryBase)val3).Transform(val6);
		val7 = Transform.Translation(0.0, 0.0 - num + num2, num8 + num10);
		Transform val9 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		val6 = val9 * val7;
		((GeometryBase)val4).Transform(val6);
		val7 = Transform.Translation(0.0, num + num2, num8 + num10);
		val6 = val9 * val7;
		((GeometryBase)val5).Transform(val6);
		OutProngCurve0.SetData(new CSOnGeometryData((GeometryBase)(object)val2, val6, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData((GeometryBase)(object)val3, val6, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData((GeometryBase)(object)val4, val6, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData((GeometryBase)(object)val5, val6, applyTransform: false));
		return base.Update();
	}
}
