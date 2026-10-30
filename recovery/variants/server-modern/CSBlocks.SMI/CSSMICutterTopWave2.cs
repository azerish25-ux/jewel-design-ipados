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

[CSBlock("SMICutterTopWave2", "{A4162039-F9FF-435b-A9BB-5B9E106D212B}")]
public sealed class CSSMICutterTopWave2 : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iMiddleWidth { get; }

	public CSInputChannel iInsideWidth { get; }

	public CSInputChannel iOutsideWidth { get; }

	public CSInputChannel iEdgeHeight { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMICutterTopWave2(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iMiddleWidth = new CSInputChannel(GetPropertyName(() => iMiddleWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iInsideWidth = new CSInputChannel(GetPropertyName(() => iInsideWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideWidth = new CSInputChannel(GetPropertyName(() => iOutsideWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeHeight = new CSInputChannel(GetPropertyName(() => iEdgeHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(GetPropertyName(() => oProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Missing profile data");
		}
		OnCurve val = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing profile curve");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		double value2 = 0.0;
		iMiddleWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iInsideWidth.GetDouble(ref value3);
		double value4 = 0.0;
		iOutsideWidth.GetDouble(ref value4);
		double value5 = 0.0;
		iEdgeHeight.GetDouble(ref value5);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.CutterTopWave2(val, value.Convert(), value2, value3, value4, value5, ref outputProfile))
		{
			return UpdateError("Cutter failed");
		}
		oProfile.SetData(new CSOnGeometryData((GeometryBase)(object)outputProfile.Convert()));
		return base.Update();
	}
}
