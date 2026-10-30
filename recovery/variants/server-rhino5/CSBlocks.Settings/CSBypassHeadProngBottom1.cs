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
		double num = InBottomOutSize.Double;
		double num2 = InBottomOutAngle.Double;
		double num3 = InCenterProngSize.Double;
		double num4 = InCenterGemSize.Double;
		double num5 = InSideGemSize1.Double;
		double num6 = InSideGemSize2.Double;
		double num7 = InRailRadius.Double;
		double num8 = (num5 + num6) / 2.0;
		double num9 = num3 / num4 * num8;
		NurbsCurve nurbsCurve = new Circle(new Plane(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0)), num9).ToNurbsCurve();
		NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve3 = nurbsCurve.ToNurbsCurve();
		NurbsCurve nurbsCurve4 = nurbsCurve.ToNurbsCurve();
		Transform transform = default(Transform);
		Transform transform2 = default(Transform);
		transform2 = Transform.Translation(0.0, num, num7 + num9);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num2), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		transform = transform3 * transform2;
		nurbsCurve.Transform(transform);
		transform2 = Transform.Translation(0.0, 0.0 - num, num7 + num9);
		transform = transform3 * transform2;
		nurbsCurve2.Transform(transform);
		transform2 = Transform.Translation(0.0, 0.0 - num, num7 + num9);
		Transform transform4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num2), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		transform = transform4 * transform2;
		nurbsCurve3.Transform(transform);
		transform2 = Transform.Translation(0.0, num, num7 + num9);
		transform = transform4 * transform2;
		nurbsCurve4.Transform(transform);
		OutProngCurve0.SetData(new CSOnGeometryData(nurbsCurve, transform, applyTransform: false));
		OutProngCurve1.SetData(new CSOnGeometryData(nurbsCurve2, transform, applyTransform: false));
		OutProngCurve2.SetData(new CSOnGeometryData(nurbsCurve3, transform, applyTransform: false));
		OutProngCurve3.SetData(new CSOnGeometryData(nurbsCurve4, transform, applyTransform: false));
		return base.Update();
	}
}
