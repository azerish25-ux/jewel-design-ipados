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
		double t = double.NaN;
		if (!value2.ClosestPoint(value, out t))
		{
			return UpdateError("Unable to get closest point");
		}
		Point3d pointStore = value2.PointAt(t);
		oCurveParam.SetData(new CSNumberData(t));
		oCurvePoint.SetData(new CSPointData(pointStore));
		return base.Update();
	}
}
