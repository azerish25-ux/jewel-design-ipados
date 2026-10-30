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

[CSBlock("SMICutterInsideRoundWithFlat", "{43D7A88C-6967-4c72-B033-F0CDB9A85481}")]
public sealed class CSSMICutterInsideRoundWithFlat : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iMiddleWidth { get; }

	public CSInputChannel iEdgeHeight { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMICutterInsideRoundWithFlat(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iMiddleWidth = new CSInputChannel(GetPropertyName(() => iMiddleWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		iEdgeHeight.GetDouble(ref value3);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.CutterInsideRoundWithFlat(onCurve, value.Convert(), value2, value3, ref outputProfile))
		{
			return UpdateError("Cutter failed");
		}
		oProfile.SetData(new CSOnGeometryData(outputProfile.Convert()));
		return base.Update();
	}
}
