using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("BypassHeadProngTopsSides", "{8E0FE695-9F94-495b-85E9-32AD9A5C962F}")]
public sealed class CSBypassHeadProngTopsSides : CSBlock
{
	public readonly CSInputChannel InSideGemXUp;

	public readonly CSInputChannel InSideGemSize;

	public readonly CSInputChannel InCenterGemSize;

	public readonly CSInputChannel InCenterProngSize;

	public readonly CSInputChannel InMiddleProngXNudge;

	public readonly CSInputChannel InMiddleProngYNudge;

	public readonly CSInputChannel InOuterProngXNudge;

	public readonly CSInputChannel InOuterProngYNudge;

	public readonly CSInputChannel InCenterProngZHeight;

	public readonly CSInputChannel InSideGemSide;

	public readonly CSOutputChannel OutProngCurve0;

	public readonly CSOutputChannel OutProngCurve1;

	public readonly CSOutputChannel OutProngCurve2;

	public readonly CSOutputChannel OutProngCurve3;

	public CSBypassHeadProngTopsSides(string identifier)
		: base(identifier)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		InSideGemXUp = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Unset);
		InSideGemSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterGemSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterProngSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleProngXNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMiddleProngYNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOuterProngXNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOuterProngYNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterProngZHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSideGemSide = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutProngCurve0 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve1 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve2 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve3 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = InSideGemXUp.Transform;
		if (transform == Transform.Unset)
		{
			return UpdateMissing("Missing side gem Xform");
		}
		double num;
		double num2;
		if (InSideGemSide.Double < 0.0)
		{
			num = 1.0;
			num2 = -1.0;
		}
		else
		{
			num = -1.0;
			num2 = 1.0;
		}
		double num3 = 0.707;
		double num4 = InCenterGemSize.Double;
		double num5 = InSideGemSize.Double;
		double num6 = InCenterProngSize.Double;
		double num7 = InCenterProngZHeight.Double;
		double num8 = InMiddleProngXNudge.Double;
		double num9 = InMiddleProngYNudge.Double;
		double num10 = InOuterProngXNudge.Double;
		double num11 = InOuterProngYNudge.Double;
		double num12 = num7 / num4 * num5;
		double num13 = num6 / num4 * num5;
		Circle val = default(Circle);
		((Circle)(ref val))._002Ector(Plane.WorldXY, num13);
		NurbsCurve val2 = ((Circle)(ref val)).ToNurbsCurve();
		NurbsCurve val3 = ((Curve)val2).ToNurbsCurve();
		NurbsCurve val4 = ((Curve)val2).ToNurbsCurve();
		NurbsCurve val5 = ((Curve)val2).ToNurbsCurve();
		Transform val6 = Transform.Translation(num * (num5 / 2.0 + num13 * num3 * num8), num5 / 2.0 + num13 * num3 * num9, num12);
		val6 = transform * val6;
		((GeometryBase)val2).Transform(val6);
		val6 = Transform.Translation(num * (num5 / 2.0 + num13 * num3 * num8), 0.0 - (num5 / 2.0 + num13 * num3 * num9), num12);
		val6 = transform * val6;
		((GeometryBase)val3).Transform(val6);
		val6 = Transform.Translation(num2 * (num5 / 2.0 + num13 * num3 * num10), 0.0 - (num5 / 2.0 + num13 * num3 * num11), num12);
		val6 = transform * val6;
		((GeometryBase)val4).Transform(val6);
		val6 = Transform.Translation(num2 * (num5 / 2.0 + num13 * num3 * num10), num5 / 2.0 + num13 * num3 * num11, num12);
		val6 = transform * val6;
		((GeometryBase)val5).Transform(val6);
		OutProngCurve0.SetData(new CSOnGeometryData((GeometryBase)(object)val2, val6, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData((GeometryBase)(object)val3, val6, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData((GeometryBase)(object)val4, val6, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData((GeometryBase)(object)val5, val6, applyTransform: false));
		return base.Update();
	}
}
