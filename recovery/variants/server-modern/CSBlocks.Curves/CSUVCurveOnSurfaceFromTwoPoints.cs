using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("UVCurveOnSurfaceFromTwoPoints", "{D9836B66-B1FC-4FB9-A3B9-553185A65870}")]
public sealed class CSUVCurveOnSurfaceFromTwoPoints : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iPointStart;

	public readonly CSInputChannel iPointEnd;

	public readonly CSOutputChannel oCurve;

	public readonly CSOutputChannel oCurveFull;

	public CSUVCurveOnSurfaceFromTwoPoints(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPointStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPointEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oCurveFull = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		NurbsSurface nurbsSurface = flattenArray.GetArrayItem(0).GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSDataArray flattenArray2 = iPointStart.GetFlattenArray();
		if (flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Point Start");
		}
		Point3d value = default(Point3d);
		if (!CSDataHelper.GetPoint(flattenArray2.GetArrayItem(0), out value))
		{
			return UpdateMissing("Missing Point Start");
		}
		CSDataArray flattenArray3 = iPointEnd.GetFlattenArray();
		if (flattenArray3.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Point End");
		}
		Point3d value2 = default(Point3d);
		if (!CSDataHelper.GetPoint(flattenArray3.GetArrayItem(0), out value2))
		{
			return UpdateMissing("Missing Point End");
		}
		((GeometryBase)nurbsSurface).GetBoundingBox(true);
		if (flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Surface");
		}
		double num = 0.0;
		double num2 = 0.0;
		if (!((Surface)nurbsSurface).ClosestPoint(value, ref num, ref num2))
		{
			return UpdateError("Closest Point 1 Failed");
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(num, num2, 0.0);
		if (!((Surface)nurbsSurface).ClosestPoint(value2, ref num, ref num2))
		{
			return UpdateError("Closest Point 2 Failed");
		}
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num, num2, 0.0);
		Line val3 = default(Line);
		((Line)(ref val3))._002Ector(val, val2);
		if (((Line)(ref val3)).Length < double.Epsilon)
		{
			return UpdateError("Curve Legnth 0");
		}
		Curve val4 = (Curve)new LineCurve(val, val2);
		Curve val5 = ((Surface)nurbsSurface).Pushup(val4, 0.01);
		if (val5 == null)
		{
			return UpdateError("Unable to Pushup");
		}
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val5));
		Interval val6 = ((Surface)nurbsSurface).Domain(0);
		double length = ((Interval)(ref val6)).Length;
		val6 = ((Surface)nurbsSurface).Domain(1);
		double num3 = (length + ((Interval)(ref val6)).Length) / ((Line)(ref val3)).Length;
		Curve val7 = CSUtilGeometry.Trim2dLineCurveToSurface((Curve)new LineCurve(((Line)(ref val3)).PointAt(0.0 - num3), ((Line)(ref val3)).PointAt(1.0 + num3)), (Surface)(object)nurbsSurface);
		if (val7 == null)
		{
			return UpdateError("Unable to trim");
		}
		Curve val8 = ((Surface)nurbsSurface).Pushup(val7, 0.01);
		if (val8 == null)
		{
			return UpdateError("Unable to push up trimmed curve");
		}
		oCurveFull.SetData(new CSOnGeometryData((GeometryBase)(object)val8));
		return base.Update();
	}
}
