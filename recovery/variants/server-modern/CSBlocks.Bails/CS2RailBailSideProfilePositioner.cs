using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Bails;

[CSBlock("2RailBailSideProfilePositioner", "{AF8FC9F8-5C64-414a-83E5-6F818049491B}")]
public sealed class CS2RailBailSideProfilePositioner : CSBlock
{
	public readonly CSInputChannel InBailRail;

	public readonly CSInputChannel InSideProfile;

	public readonly CSInputChannel InDistanceBottom;

	public readonly CSInputChannel InDistanceTop;

	public readonly CSOutputChannel OutProfile;

	public CS2RailBailSideProfilePositioner(string identifier)
		: base(identifier)
	{
		InBailRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSideProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InDistanceBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InDistanceTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		OutProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InBailRail.Data;
		NurbsCurve nurbsCurve = InSideProfile.NurbsCurve;
		if (data == null || nurbsCurve == null)
		{
			return UpdateMissing("SideProfile Missing");
		}
		double num = InDistanceBottom.Double;
		double num2 = InDistanceTop.Double;
		BoundingBox boundingBox = BoundingBox.Unset;
		data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity);
		Point3d pointAtStart = ((Curve)nurbsCurve).PointAtStart;
		Point3d pointAtEnd = ((Curve)nurbsCurve).PointAtEnd;
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		if (num < 0.0)
		{
			val = ((BoundingBox)(ref boundingBox)).Min;
			x = ((Point3d)(ref val)).X;
		}
		double num3 = x + num;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num3, y, ((Point3d)(ref val)).Z);
		double num4 = x + num2;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(num4, y2, ((Point3d)(ref val)).Z);
		double num5 = ((Point3d)(ref val2)).DistanceTo(val3);
		double num6 = ((Point3d)(ref pointAtStart)).DistanceTo(pointAtEnd);
		Transform val4 = Transform.Scale(pointAtStart, num5 / num6);
		Transform val5 = CSBlockUtil.CSUtilMath.Orient3Point(pointAtStart, pointAtEnd - pointAtStart, Vector3d.ZAxis, val2, val3 - val2, Vector3d.ZAxis);
		((GeometryBase)nurbsCurve).Transform(val5 * val4);
		OutProfile.SetData(new CSOnGeometryData((GeometryBase)(object)nurbsCurve, val5 * val4, applyTransform: false));
		return base.Update();
	}
}
