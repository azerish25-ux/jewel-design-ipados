using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.SMI;

[CSBlock("SMIRectangle", "{7E014843-02E6-4b49-AB34-02A799527A7E}")]
public sealed class CSSMIRectangle : CSBlock
{
	public readonly CSInputChannel iHeight;

	public readonly CSInputChannel iWidth;

	public readonly CSOutputChannel oProfile;

	public CSSMIRectangle(string identifier)
		: base(identifier)
	{
		iHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 0.0;
		iHeight.GetDouble(ref value);
		double value2 = 0.0;
		iWidth.GetDouble(ref value2);
		OnCurve outputProfile = null;
		CSUtilSMI.Rectangle(value, value2, ref outputProfile);
		oProfile.SetData(new CSOnGeometryData((GeometryBase)(object)outputProfile.Convert()));
		return base.Update();
	}
}
