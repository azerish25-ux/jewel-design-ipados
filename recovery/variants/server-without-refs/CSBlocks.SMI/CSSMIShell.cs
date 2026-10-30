using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.SMI;

[CSBlock("SMIShell", "{AE072AA4-78B5-43d6-8DDF-624BD08F3553}")]
public sealed class CSSMIShell : CSBlock
{
	public CSInputChannel iMiddleHeight { get; }

	public CSInputChannel iMiddleWidth { get; }

	public CSInputChannel iOutsideHeight { get; }

	public CSInputChannel iOutsideWidth { get; }

	public CSInputChannel iOutsideFillet { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMIShell(string identifier)
		: base(identifier)
	{
		iMiddleHeight = new CSInputChannel(GetPropertyName(() => iMiddleHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMiddleWidth = new CSInputChannel(GetPropertyName(() => iMiddleWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideHeight = new CSInputChannel(GetPropertyName(() => iOutsideHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideWidth = new CSInputChannel(GetPropertyName(() => iOutsideWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideFillet = new CSInputChannel(GetPropertyName(() => iOutsideFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(GetPropertyName(() => oProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		double value = 0.0;
		iMiddleHeight.GetDouble(ref value);
		double value2 = 0.0;
		iMiddleWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iOutsideHeight.GetDouble(ref value3);
		double value4 = 0.0;
		iOutsideWidth.GetDouble(ref value4);
		double value5 = 0.0;
		iOutsideFillet.GetDouble(ref value5);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.Shell(value, value2, value3, value4, value5, ref outputProfile))
		{
			return UpdateError("Shell failed");
		}
		oProfile.SetData(new CSOnGeometryData((GeometryBase)(object)outputProfile.Convert()));
		return base.Update();
	}
}
