using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("ScaleObjectXform", "{A72F685A-B103-4580-90DE-6F264DF98602}")]
public sealed class CSScaleObjectXform : CSBlock
{
	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oXform;

	public CSScaleObjectXform(string identifier)
		: base(identifier)
	{
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 1.0;
		iX.GetDouble(ref value);
		double value2 = 1.0;
		iY.GetDouble(ref value2);
		double value3 = 1.0;
		iZ.GetDouble(ref value3);
		Transform transform = Transform.Scale(Plane.WorldXY, value, value2, value3);
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
