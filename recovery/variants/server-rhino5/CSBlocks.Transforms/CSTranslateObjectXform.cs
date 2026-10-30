using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("TranslateObjectXform", "{F9AF1449-1903-4d26-92DF-6FEBC8A61427}")]
public sealed class CSTranslateObjectXform : CSBlock
{
	public readonly CSInputChannel iX;

	public readonly CSInputChannel iY;

	public readonly CSInputChannel iZ;

	public readonly CSInputChannel iMirror;

	public readonly CSOutputChannel oXform;

	public CSTranslateObjectXform(string identifier)
		: base(identifier)
	{
		iX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMirror = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 0.0;
		if (iX != null)
		{
			iX.GetDouble(ref value);
		}
		double value2 = 0.0;
		if (iY != null)
		{
			iY.GetDouble(ref value2);
		}
		double value3 = 0.0;
		if (iZ != null)
		{
			iZ.GetDouble(ref value3);
		}
		double value4 = 0.0;
		if (iMirror != null)
		{
			iMirror.GetDouble(ref value4);
		}
		if (value4 > 0.5)
		{
			value = 0.0 - value;
			value3 = 0.0 - value3;
		}
		Transform transform = Transform.Translation(value, value2, value3);
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
