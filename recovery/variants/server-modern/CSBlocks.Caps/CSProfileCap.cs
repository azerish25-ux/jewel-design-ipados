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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
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
		NurbsCurve val = new NurbsCurve(value);
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
		NurbsSurface val6 = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDomeRC(value2), val, 0, zAdjust: true);
		((GeometryBase)val6).Transform(val5);
		oBrep.SetData(new CSOnGeometryData((GeometryBase)(object)((Surface)val6).ToBrep(), val5, applyTransform: false));
		return base.Update();
	}
}
