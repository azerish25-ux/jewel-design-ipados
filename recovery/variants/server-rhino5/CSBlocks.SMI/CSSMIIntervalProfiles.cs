using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.SMI;

[CSBlock("SMIIntervalProfiles", "{C7929B86-B470-498b-B79E-E66E91F8C1F0}")]
public sealed class CSSMIIntervalProfiles : CSBlock
{
	public CSInputChannel iProfile { get; }

	public CSInputChannel iAngle { get; }

	public CSOutputChannel oRail { get; }

	public CSOutputChannel oProfiles { get; }

	public CSSMIIntervalProfiles(string identifier)
		: base(identifier)
	{
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRail = new CSOutputChannel(GetPropertyName(() => oRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oProfiles = new CSOutputChannel(GetPropertyName(() => oProfiles), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
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
		double value = 0.0;
		iAngle.GetDouble(ref value);
		value = CSBlockUtil.CSUtilMath.ToRadians(value);
		CSDataArray cSDataArray = new CSDataArray();
		OnArcCurve crv;
		if (value != 2.0 * OnUtil.On_PI)
		{
			OnXform val2 = new OnXform();
			val2.Rotation(value / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			OnCurve val3 = val.DuplicateCurve();
			((OnGeometry)val3).Transform((IOnXform)(object)val2);
			cSDataArray.Append(new CSOnGeometryData(val3.Convert(), val2.Convert(), applyTransform: false));
			OnXform val4 = new OnXform();
			val4.Rotation(0.0 - value / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			OnCurve val5 = val.DuplicateCurve();
			((OnGeometry)val5).Transform((IOnXform)(object)val4);
			cSDataArray.Append(new CSOnGeometryData(val5.Convert(), val4.Convert(), applyTransform: false));
			oProfiles.SetData(cSDataArray);
			crv = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val3.PointAtStart(), (IOn3dPoint)(object)val.PointAtStart(), (IOn3dPoint)(object)val5.PointAtStart()));
		}
		else
		{
			oProfiles.SetData(new CSOnGeometryData(val.Convert()));
			crv = OnArcCurve.op_Implicit((IOnCircle)new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, val.PointAtStart().DistanceTo(OnUtil.On_origin)));
		}
		oRail.SetData(new CSOnGeometryData(((OnCurve)(object)crv).Convert()));
		return base.Update();
	}
}
