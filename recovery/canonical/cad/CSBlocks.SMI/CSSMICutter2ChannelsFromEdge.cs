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

[CSBlock("SMICutter2ChannelsFromEdge", "{92450045-06B7-4edc-99A8-DCEC2AEC27EA}")]
public sealed class CSSMICutter2ChannelsFromEdge : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iWidth { get; }

	public CSInputChannel iDistance { get; }

	public CSInputChannel iCuttingDepth { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMICutter2ChannelsFromEdge(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iWidth = new CSInputChannel(GetPropertyName(() => iWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDistance = new CSInputChannel(GetPropertyName(() => iDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCuttingDepth = new CSInputChannel(GetPropertyName(() => iCuttingDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		iWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iDistance.GetDouble(ref value3);
		double value4 = 0.0;
		iCuttingDepth.GetDouble(ref value4);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.Cutter2ChannelsFromEdge(onCurve, value.Convert(), value2, value3, value4, ref outputProfile))
		{
			return UpdateError("Cutter failed");
		}
		oProfile.SetData(new CSOnGeometryData(outputProfile.Convert()));
		return base.Update();
	}
}
