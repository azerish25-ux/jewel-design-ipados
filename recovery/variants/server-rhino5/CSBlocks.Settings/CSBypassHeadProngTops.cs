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
		double dz = InCenterProngZHeight.Double;
		Transform transform2 = default(Transform);
		NurbsCurve nurbsCurve = new Circle(Plane.WorldXY, num3).ToNurbsCurve();
		NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve3 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve4 = nurbsCurve.ToNurbsCurve();
		transform2 = Transform.Translation(num2 / 2.0 + num3 * num * num4, num2 / 2.0 + num3 * num * num5, dz);
		transform2 = transform * transform2;
		nurbsCurve.Transform(transform2);
		transform2 = Transform.Translation(num2 / 2.0 + num3 * num * num4, 0.0 - (num2 / 2.0 + num3 * num * num5), dz);
		transform2 = transform * transform2;
		nurbsCurve2.Transform(transform2);
		transform2 = Transform.Translation(0.0 - (num2 / 2.0 + num3 * num * num4), 0.0 - (num2 / 2.0 + num3 * num * num5), dz);
		transform2 = transform * transform2;
		nurbsCurve3.Transform(transform2);
		transform2 = Transform.Translation(0.0 - (num2 / 2.0 + num3 * num * num4), num2 / 2.0 + num3 * num * num5, dz);
		transform2 = transform * transform2;
		nurbsCurve4.Transform(transform2);
		OutProngCurve0.SetData(new CSOnGeometryData(nurbsCurve, transform2, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData(nurbsCurve2, transform2, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData(nurbsCurve3, transform2, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData(nurbsCurve4, transform2, applyTransform: false));
		return base.Update();
	}
}
