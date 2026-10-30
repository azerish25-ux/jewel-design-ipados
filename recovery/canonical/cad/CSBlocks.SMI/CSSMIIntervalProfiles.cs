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
		double value = 0.0;
		iAngle.GetDouble(ref value);
		value = CSBlockUtil.CSUtilMath.ToRadians(value);
		CSDataArray cSDataArray = new CSDataArray();
		OnArcCurve crv;
		if (value != 2.0 * OnUtil.On_PI)
		{
			OnXform onXform = new OnXform();
			onXform.Rotation(value / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			OnCurve onCurve2 = onCurve.DuplicateCurve();
			onCurve2.Transform(onXform);
			cSDataArray.Append(new CSOnGeometryData(onCurve2.Convert(), onXform.Convert(), applyTransform: false));
			OnXform onXform2 = new OnXform();
			onXform2.Rotation(0.0 - value / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			OnCurve onCurve3 = onCurve.DuplicateCurve();
			onCurve3.Transform(onXform2);
			cSDataArray.Append(new CSOnGeometryData(onCurve3.Convert(), onXform2.Convert(), applyTransform: false));
			oProfiles.SetData(cSDataArray);
			crv = new OnArc(onCurve2.PointAtStart(), onCurve.PointAtStart(), onCurve3.PointAtStart());
		}
		else
		{
			oProfiles.SetData(new CSOnGeometryData(onCurve.Convert()));
			crv = new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, onCurve.PointAtStart().DistanceTo(OnUtil.On_origin));
		}
		oRail.SetData(new CSOnGeometryData(crv.Convert()));
		return base.Update();
	}
}
