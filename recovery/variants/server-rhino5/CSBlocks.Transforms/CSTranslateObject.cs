using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TranslateObject", "{E342D760-7FD5-4369-9FA2-CBC4629FC248}")]
public sealed class CSTranslateObject : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oObject;

	public CSTranslateObject(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object");
		}
		double value = 0.0;
		iX.GetDouble(ref value);
		double value2 = 0.0;
		iY.GetDouble(ref value2);
		double value3 = 0.0;
		iZ.GetDouble(ref value3);
		Transform transform = Transform.Translation(value, value2, value3);
		CSData cSData = data.Duplicate();
		if (cSData != null)
		{
			CSDataHelper.Transform(cSData, transform);
			oObject.SetData(cSData);
		}
		return base.Update();
	}
}
