using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Points;

[CSBlock("GetClosestPointOnCurve", "{DB405D21-F339-4B7D-9ABC-E360C7175FAB}")]
public sealed class CSGetClosestPointOnCurve : CSBlock
{
	public readonly CSInputChannel iPoint;

	public readonly CSInputChannel iCurve;

	public readonly CSOutputChannel oCurveParam;

	public readonly CSOutputChannel oCurvePoint;

	public CSGetClosestPointOnCurve(string identifier)
		: base(identifier)
	{
		iPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oCurveParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurvePoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		Point3d value = Point3d.Unset;
		if (!CSDataHelper.GetPoint(iPoint.Data, out value))
		{
			return UpdateMissing("Point");
		}
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Curve");
		}
		double num = double.NaN;
		if (!((Curve)value2).ClosestPoint(value, ref num))
		{
			return UpdateError("Unable to get closest point");
		}
		Point3d pointStore = ((Curve)value2).PointAt(num);
		oCurveParam.SetData(new CSNumberData(num));
		oCurvePoint.SetData(new CSPointData(pointStore));
		return base.Update();
	}
}
