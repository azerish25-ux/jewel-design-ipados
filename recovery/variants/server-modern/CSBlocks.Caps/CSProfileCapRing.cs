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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve value = null;
		if (iCurve != null)
		{
			CSDataHelper.GetNurbsCurve(iCurve.Data, out value);
		}
		if (value == null)
		{
			return UpdateError("ProfileEndCap Missing Nurbscurve");
		}
		if (!CSBlockUtil.CSUtilMath.AveragePlaneOfCurve((Curve)(object)value, out var plane))
		{
			return UpdateError("ProfileEndCap missing");
		}
		NurbsCurve val = (NurbsCurve)((GeometryBase)value).Duplicate();
		Transform val2 = Transform.ChangeBasis(Plane.WorldXY, plane);
		((GeometryBase)val).Transform(val2);
		Transform val3 = default(Transform);
		BoundingBox boundingBox = ((GeometryBase)val).GetBoundingBox(false);
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double num = 0.0 - ((Point3d)(ref center)).X;
		boundingBox = ((GeometryBase)val).GetBoundingBox(false);
		center = ((BoundingBox)(ref boundingBox)).Center;
		val3 = Transform.Translation(new Vector3d(num, 0.0 - ((Point3d)(ref center)).Y, 0.0));
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
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(0.0, 0.0, 0.0);
		((Point3d)(ref val7)).Transform(val5);
		if (new Vector3d(val7 - Point3d.Origin) * val6 < 0.0)
		{
			value2 = 0.0 - value2;
		}
		NurbsSurface val8 = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDomeRC(value2), val, 0, zAdjust: true);
		((GeometryBase)val8).Transform(val5);
		oBrep.SetData(new CSOnGeometryData((GeometryBase)(object)((Surface)val8).ToBrep(), val5, applyTransform: false));
		return base.Update();
	}
}
