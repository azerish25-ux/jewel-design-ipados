using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Caps;

[CSBlock("ProfileCapNorth", "{666100F2-C854-4a48-BCA8-FBF6D94EED85}")]
public sealed class CSProfileCapNorth : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iDomeThickness;

	public readonly CSOutputChannel oBrep;

	public CSProfileCapNorth(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iDomeThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve value = null;
		if (iCurve != null)
		{
			CSDataHelper.GetNurbsCurve(iCurve.Data, out value);
		}
		if (value == null)
		{
			return UpdateError("ProfileEndCap Missing Nurbscurv");
		}
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)(object)value, out var plane))
		{
			return UpdateError("ProfileEndCap missing");
		}
		NurbsCurve val = new NurbsCurve(value);
		Transform val2 = Transform.ChangeBasis(Plane.WorldXY, plane);
		((GeometryBase)val).Transform(val2);
		BoundingBox boundingBox = ((GeometryBase)val).GetBoundingBox(false);
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double num = 0.0 - ((Point3d)(ref center)).X;
		boundingBox = ((GeometryBase)val).GetBoundingBox(false);
		center = ((BoundingBox)(ref boundingBox)).Center;
		Transform val3 = Transform.Translation(new Vector3d(num, 0.0 - ((Point3d)(ref center)).Y, 0.0));
		((GeometryBase)val).Transform(val3);
		Transform val4 = val3 * val2;
		Transform val5 = default(Transform);
		((Transform)(ref val4)).TryGetInverse(ref val5);
		double value2 = 0.5;
		if (iDomeThickness != null)
		{
			iDomeThickness.GetDouble(ref value2);
		}
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(0.0, 0.0, 1.0);
		((Vector3d)(ref val6)).Transform(val5);
		if (new Vector3d(0.0, 0.0, 1.0) * val6 < 0.0)
		{
			value2 = 0.0 - value2;
		}
		NurbsSurface val7 = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDomeRC(value2), val, 0, zAdjust: true);
		((GeometryBase)val7).Transform(val5);
		oBrep.SetData(new CSOnGeometryData((GeometryBase)(object)((Surface)val7).ToBrep(), val5, applyTransform: false));
		return base.Update();
	}
}
