using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Caps;

[CSBlock("ProfileCap", "{C88750AE-3440-48d5-9501-C3BD55E8B2B8}")]
public sealed class CSProfileCap : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iDomeThickness;

	public readonly CSOutputChannel oBrep;

	public CSProfileCap(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iDomeThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		NurbsCurve value = null;
		if (iCurve != null)
		{
			CSDataHelper.GetNurbsCurve(iCurve.Data, out value);
		}
		if (value == null)
		{
			return UpdateError("ProfileEndCap Missing Nurbscurve");
		}
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)value, out Plane plane))
		{
			return UpdateError("ProfileEndCap missing");
		}
		NurbsCurve nurbsCurve = new NurbsCurve(value);
		Transform transform = Transform.ChangeBasis(Plane.WorldXY, plane);
		nurbsCurve.Transform(transform);
		Transform transform2 = default(Transform);
		transform2 = Transform.Translation(new Vector3d(0.0 - nurbsCurve.GetBoundingBox(accurate: false).Center.X, 0.0 - nurbsCurve.GetBoundingBox(accurate: false).Center.Y, 0.0));
		nurbsCurve.Transform(transform2);
		(transform2 * transform).TryGetInverse(out var inverseTransform);
		double value2 = 0.5;
		if (iDomeThickness != null)
		{
			iDomeThickness.GetDouble(ref value2);
		}
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDomeRC(value2), nurbsCurve, 0, zAdjust: true);
		nurbsSurface.Transform(inverseTransform);
		oBrep.SetData(new CSOnGeometryData(nurbsSurface.ToBrep(), inverseTransform, applyTransform: false));
		return base.Update();
	}
}
