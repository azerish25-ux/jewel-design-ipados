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

[CSBlock("SMICutterChannelFromCenter", "{AAECBF6B-B784-4917-9916-D94493236FF0}")]
public sealed class CSSMICutterChannelFromCenter : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iWidth { get; }

	public CSInputChannel iDistance { get; }

	public CSInputChannel iCuttingDepth { get; }

	public CSOutputChannel oProfile { get; }

	public CSSMICutterChannelFromCenter(string identifier)
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
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
		iWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iDistance.GetDouble(ref value3);
		double value4 = 0.0;
		iCuttingDepth.GetDouble(ref value4);
		OnCurve outputProfile = null;
		if (!CSUtilSMI.CutterChannelFromCenter(val, value.Convert(), value2, value4, value3, ref outputProfile))
		{
			return UpdateError("Cutter failed");
		}
		oProfile.SetData(new CSOnGeometryData((GeometryBase)(object)outputProfile.Convert()));
		return base.Update();
	}
}
