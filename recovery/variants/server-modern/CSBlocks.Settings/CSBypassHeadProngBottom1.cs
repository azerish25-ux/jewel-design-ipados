using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("BypassHeadProngBottom1", "{F5A007BF-050A-4d46-BE40-F34153411937}")]
public sealed class CSBypassHeadProngBottom1 : CSBlock
{
	public readonly CSInputChannel InBottomOutSize;

	public readonly CSInputChannel InBottomOutAngle;

	public readonly CSInputChannel InCenterProngSize;

	public readonly CSInputChannel InCenterGemSize;

	public readonly CSInputChannel InSideGemSize1;

	public readonly CSInputChannel InSideGemSize2;

	public readonly CSInputChannel InRailRadius;

	public readonly CSOutputChannel OutProngCurve0;

	public readonly CSOutputChannel OutProngCurve1;

	public readonly CSOutputChannel OutProngCurve2;

	public readonly CSOutputChannel OutProngCurve3;

	public CSBypassHeadProngBottom1(string identifier)
		: base(identifier)
	{
		InBottomOutSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBottomOutAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
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
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		double num = InBottomOutSize.Double;
		double num2 = InBottomOutAngle.Double;
		double num3 = InCenterProngSize.Double;
		double num4 = InCenterGemSize.Double;
		double num5 = InSideGemSize1.Double;
		double num6 = InSideGemSize2.Double;
		double num7 = InRailRadius.Double;
		double num8 = (num5 + num6) / 2.0;
		double num9 = num3 / num4 * num8;
		Circle val = default(Circle);
		((Circle)(ref val))._002Ector(new Plane(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0)), num9);
		NurbsCurve val2 = ((Circle)(ref val)).ToNurbsCurve();
		NurbsCurve val3 = ((Curve)val2).ToNurbsCurve();
		NurbsCurve val4 = ((Curve)val2).ToNurbsCurve();
		NurbsCurve val5 = ((Curve)val2).ToNurbsCurve();
		Transform val6 = default(Transform);
		Transform val7 = default(Transform);
		val7 = Transform.Translation(0.0, num, num7 + num9);
		Transform val8 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num2), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		val6 = val8 * val7;
		((GeometryBase)val2).Transform(val6);
		val7 = Transform.Translation(0.0, 0.0 - num, num7 + num9);
		val6 = val8 * val7;
		((GeometryBase)val3).Transform(val6);
		val7 = Transform.Translation(0.0, 0.0 - num, num7 + num9);
		Transform val9 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num2), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		val6 = val9 * val7;
		((GeometryBase)val4).Transform(val6);
		val7 = Transform.Translation(0.0, num, num7 + num9);
		val6 = val9 * val7;
		((GeometryBase)val5).Transform(val6);
		OutProngCurve0.SetData(new CSOnGeometryData((GeometryBase)(object)val2, val6, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData((GeometryBase)(object)val3, val6, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData((GeometryBase)(object)val4, val6, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData((GeometryBase)(object)val5, val6, applyTransform: false));
		return base.Update();
	}
}
