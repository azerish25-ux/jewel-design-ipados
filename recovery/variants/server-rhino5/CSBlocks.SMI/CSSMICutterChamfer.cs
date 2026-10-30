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

[CSBlock("SMICutterChamfer", "{F9CBB04F-8FD3-45b3-B715-3B3613D94221}")]
public sealed class CSSMICutterChamfer : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iCuttingDepth { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMICutterChamfer(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		OnCurve val = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing profile curve");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		double value3 = 0.0;
		iCuttingDepth.GetDouble(ref value3);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.CutterChamfer(val, value.Convert(), value2, value3, ref outputProfile))
		{
			return UpdateError("Cutter failed");
		}
		oProfile.SetData(new CSOnGeometryData(outputProfile.Convert()));
		return base.Update();
	}
}
