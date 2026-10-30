using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

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
		CSDataHelper.GetPoint(iPoint1, out var value);
		CSDataHelper.GetPoint(iPoint2, out var value2);
		double length = (value - value2).Length;
		if (double.IsNaN(length))
		{
			return UpdateError("Error getting distance");
		}
		oDistance.SetData(new CSNumberData(length));
		return base.Update();
	}
}
