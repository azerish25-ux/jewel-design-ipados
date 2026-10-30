using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("LineInX", "{2DEEDFE8-D3AD-4987-9D56-ABC691E268CC}")]
public sealed class CSLineInX : CSBlock
{
	public CSInputChannel iXLength;

	public CSInputChannel iYLocation;

	public CSOutputChannel oCurve;

	public CSLineInX(string identifier)
		: base(identifier)
	{
		iXLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 10.0;
		double value2 = 0.0;
		iXLength.GetDouble(ref value);
		iYLocation.GetDouble(ref value2);
		LineCurve geometry = new LineCurve(new Point3d(0.0, value2, 0.0), new Point3d(value, value2, 0.0));
		oCurve.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
