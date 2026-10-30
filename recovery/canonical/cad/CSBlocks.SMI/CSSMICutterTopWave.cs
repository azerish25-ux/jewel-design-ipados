using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.SMI;

[CSBlock("SMICutterTopWave", "{ADB729DB-868D-4b34-A39B-2F858B124DBD}")]
public sealed class CSSMICutterTopWave : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iMiddleWidth { get; }

	public CSInputChannel iOutsideWidth { get; }

	public CSInputChannel iEdgeHeight { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMICutterTopWave(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iMiddleWidth = new CSInputChannel(GetPropertyName(() => iMiddleWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideWidth = new CSInputChannel(GetPropertyName(() => iOutsideWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeHeight = new CSInputChannel(GetPropertyName(() => iEdgeHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(GetPropertyName(() => oProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Missing profile data");
		}
		OnCurve onCurve = data.GetNurbsCurve().Convert();
		if (onCurve == null)
		{
			return UpdateMissing("Missing profile curve");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		double value2 = 0.0;
		iMiddleWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iOutsideWidth.GetDouble(ref value3);
		double value4 = 0.0;
		iEdgeHeight.GetDouble(ref value4);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.CutterTopWave(onCurve, value.Convert(), value2, value3, value4, ref outputProfile))
		{
			return UpdateError("Cutter failed");
		}
		oProfile.SetData(new CSOnGeometryData(outputProfile.Convert()));
		return base.Update();
	}
}
