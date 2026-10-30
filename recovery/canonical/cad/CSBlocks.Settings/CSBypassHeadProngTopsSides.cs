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
		double dz = num7 / num4 * num5;
		double num12 = num6 / num4 * num5;
		NurbsCurve nurbsCurve = new Circle(Plane.WorldXY, num12).ToNurbsCurve();
		NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve3 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve4 = nurbsCurve.ToNurbsCurve();
		Transform transform2 = Transform.Translation(num * (num5 / 2.0 + num12 * num3 * num8), num5 / 2.0 + num12 * num3 * num9, dz);
		transform2 = transform * transform2;
		nurbsCurve.Transform(transform2);
		transform2 = Transform.Translation(num * (num5 / 2.0 + num12 * num3 * num8), 0.0 - (num5 / 2.0 + num12 * num3 * num9), dz);
		transform2 = transform * transform2;
		nurbsCurve2.Transform(transform2);
		transform2 = Transform.Translation(num2 * (num5 / 2.0 + num12 * num3 * num10), 0.0 - (num5 / 2.0 + num12 * num3 * num11), dz);
		transform2 = transform * transform2;
		nurbsCurve3.Transform(transform2);
		transform2 = Transform.Translation(num2 * (num5 / 2.0 + num12 * num3 * num10), num5 / 2.0 + num12 * num3 * num11, dz);
		transform2 = transform * transform2;
		nurbsCurve4.Transform(transform2);
		OutProngCurve0.SetData(new CSOnGeometryData(nurbsCurve, transform2, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData(nurbsCurve2, transform2, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData(nurbsCurve3, transform2, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData(nurbsCurve4, transform2, applyTransform: false));
		return base.Update();
	}
}
