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
		NurbsCurve nurbsCurve = new Circle(new Plane(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0)), num10).ToNurbsCurve();
		NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve3 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve4 = nurbsCurve.ToNurbsCurve();
		Transform transform = default(Transform);
		Transform transform2 = default(Transform);
		transform2 = Transform.Translation(0.0, num - num2, num8 + num10);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		transform = transform3 * transform2;
		nurbsCurve.Transform(transform);
		transform2 = Transform.Translation(0.0, 0.0 - num - num2, num8 + num10);
		transform = transform3 * transform2;
		nurbsCurve2.Transform(transform);
		transform2 = Transform.Translation(0.0, 0.0 - num + num2, num8 + num10);
		Transform transform4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num3), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		transform = transform4 * transform2;
		nurbsCurve3.Transform(transform);
		transform2 = Transform.Translation(0.0, num + num2, num8 + num10);
		transform = transform4 * transform2;
		nurbsCurve4.Transform(transform);
		OutProngCurve0.SetData(new CSOnGeometryData(nurbsCurve, transform, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData(nurbsCurve2, transform, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData(nurbsCurve3, transform, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData(nurbsCurve4, transform, applyTransform: false));
		return base.Update();
	}
}
