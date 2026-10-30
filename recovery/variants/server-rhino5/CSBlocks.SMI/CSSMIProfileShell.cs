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

[CSBlock("SMIProfileShell", "{AEE465FF-0CAB-4d25-89DF-84FCF60891FD}")]
public sealed class CSSMIProfileShell : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iMiddleHeight { get; }

	public CSInputChannel iOutsideHeight { get; }

	public CSInputChannel iOutsideWidth { get; }

	public CSInputChannel iOutsideFillet { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMIProfileShell(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iMiddleHeight = new CSInputChannel(GetPropertyName(() => iMiddleHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideHeight = new CSInputChannel(GetPropertyName(() => iOutsideHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideWidth = new CSInputChannel(GetPropertyName(() => iOutsideWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideFillet = new CSInputChannel(GetPropertyName(() => iOutsideFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(GetPropertyName(() => oProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Missing profile");
		}
		OnCurve val = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing profile data");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		double value2 = 0.0;
		iMiddleHeight.GetDouble(ref value2);
		double value3 = 0.0;
		iOutsideHeight.GetDouble(ref value3);
		double value4 = 0.0;
		iOutsideWidth.GetDouble(ref value4);
		double value5 = 0.0;
		iOutsideFillet.GetDouble(ref value5);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.Shell(val, value.Convert(), value2, value3, value4, value5, ref outputProfile))
		{
			return UpdateError("Shell failed");
		}
		oProfile.SetData(new CSOnGeometryData(outputProfile.Convert()));
		return base.Update();
	}
}
