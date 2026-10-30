using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Caps;

[CSBlock("ProfileCapRing", "{1B83EA9E-E280-4c68-BF31-5306F4F71924}")]
public sealed class CSProfileCapRing : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iDomeThickness;

	public readonly CSOutputChannel oBrep;

	public CSProfileCapRing(string identifier)
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
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve(value, out var plane))
		{
			return UpdateError("ProfileEndCap missing");
		}
		NurbsCurve nurbsCurve = (NurbsCurve)value.Duplicate();
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
		Vector3d vector3d = new Vector3d(0.0, 0.0, 1.0);
		vector3d.Transform(inverseTransform);
		Point3d point3d = new Point3d(0.0, 0.0, 0.0);
		point3d.Transform(inverseTransform);
		if (new Vector3d(point3d - Point3d.Origin) * vector3d < 0.0)
		{
			value2 = 0.0 - value2;
		}
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDomeRC(value2), nurbsCurve, 0, zAdjust: true);
		nurbsSurface.Transform(inverseTransform);
		oBrep.SetData(new CSOnGeometryData(nurbsSurface.ToBrep(), inverseTransform, applyTransform: false));
		return base.Update();
	}
}
