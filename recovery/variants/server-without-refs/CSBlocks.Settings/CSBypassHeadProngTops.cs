using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("BypassHeadProngTops", "{64B09614-E346-4a79-8CDD-6E43FDC359A9}")]
public sealed class CSBypassHeadProngTops : CSBlock
{
	public readonly CSInputChannel InCenterGemXUp;

	public readonly CSInputChannel InCenterGemSize;

	public readonly CSInputChannel InCenterProngSize;

	public readonly CSInputChannel InCenterProngXNudge;

	public readonly CSInputChannel InCenterProngYNudge;

	public readonly CSInputChannel InCenterProngZHeight;

	public readonly CSOutputChannel OutProngCurve0;

	public readonly CSOutputChannel OutProngCurve1;

	public readonly CSOutputChannel OutProngCurve2;

	public readonly CSOutputChannel OutProngCurve3;

	public CSBypassHeadProngTops(string identifier)
		: base(identifier)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		InCenterGemXUp = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Unset);
		InCenterGemSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterProngSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterProngXNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterProngYNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterProngZHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutProngCurve0 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve1 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve2 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutProngCurve3 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.707;
		Transform transform = InCenterGemXUp.Transform;
		if (transform == Transform.Unset)
		{
			return UpdateMissing("Missing center gem Xform");
		}
		double num2 = InCenterGemSize.Double;
		double num3 = InCenterProngSize.Double;
		double num4 = InCenterProngXNudge.Double;
		double num5 = InCenterProngYNudge.Double;
		double num6 = InCenterProngZHeight.Double;
		Transform val = default(Transform);
		Circle val2 = default(Circle);
		((Circle)(ref val2))._002Ector(Plane.WorldXY, num3);
		NurbsCurve val3 = ((Circle)(ref val2)).ToNurbsCurve();
		NurbsCurve val4 = ((Curve)val3).ToNurbsCurve();
		NurbsCurve val5 = ((Curve)val3).ToNurbsCurve();
		NurbsCurve val6 = ((Curve)val3).ToNurbsCurve();
		val = Transform.Translation(num2 / 2.0 + num3 * num * num4, num2 / 2.0 + num3 * num * num5, num6);
		val = transform * val;
		((GeometryBase)val3).Transform(val);
		val = Transform.Translation(num2 / 2.0 + num3 * num * num4, 0.0 - (num2 / 2.0 + num3 * num * num5), num6);
		val = transform * val;
		((GeometryBase)val4).Transform(val);
		val = Transform.Translation(0.0 - (num2 / 2.0 + num3 * num * num4), 0.0 - (num2 / 2.0 + num3 * num * num5), num6);
		val = transform * val;
		((GeometryBase)val5).Transform(val);
		val = Transform.Translation(0.0 - (num2 / 2.0 + num3 * num * num4), num2 / 2.0 + num3 * num * num5, num6);
		val = transform * val;
		((GeometryBase)val6).Transform(val);
		OutProngCurve0.SetData(new CSOnGeometryData((GeometryBase)(object)val3, val, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData((GeometryBase)(object)val4, val, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData((GeometryBase)(object)val5, val, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData((GeometryBase)(object)val6, val, applyTransform: false));
		return base.Update();
	}
}
