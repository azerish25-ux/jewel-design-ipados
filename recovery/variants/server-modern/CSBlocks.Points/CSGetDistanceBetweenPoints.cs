using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Points;

[CSBlock("GetDistanceBetweenPoints", "{5E299BBA-E882-4E47-8315-02998454AF6C}")]
public sealed class CSGetDistanceBetweenPoints : CSBlock
{
	public readonly CSOutputChannel oDistance;

	public readonly CSInputChannel iPoint1;

	public readonly CSInputChannel iPoint2;

	public CSGetDistanceBetweenPoints(string identifier)
		: base(identifier)
	{
		iPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iPoint2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oDistance = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetPoint(iPoint1, out var value);
		CSDataHelper.GetPoint(iPoint2, out var value2);
		Vector3d val = value - value2;
		double length = ((Vector3d)(ref val)).Length;
		if (double.IsNaN(length))
		{
			return UpdateError("Error getting distance");
		}
		oDistance.SetData(new CSNumberData(length));
		return base.Update();
	}
}
