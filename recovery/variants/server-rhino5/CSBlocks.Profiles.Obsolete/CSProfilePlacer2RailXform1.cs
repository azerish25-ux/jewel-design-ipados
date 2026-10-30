using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacer2RailXform_V1", "{FC789225-3F3B-41f5-9EB0-6C25D4FC54B8}")]
public sealed class CSProfilePlacer2RailXform1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideRailScale { get; }

	public CSInputChannel iParameter { get; }

	public CSOutputChannel oXform { get; }

	public CSOutputChannel oParam { get; }

	public CSOutputChannel oParam2 { get; }

	public CSOutputChannel oYSize { get; }

	public CSProfilePlacer2RailXform1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideRailScale = new CSInputChannel(GetPropertyName(() => iOutsideRailScale), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(GetPropertyName(() => iParameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(GetPropertyName(() => oXform), this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(GetPropertyName(() => oParam), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(GetPropertyName(() => oParam2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oYSize = new CSOutputChannel(GetPropertyName(() => oYSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_0116: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		OnNurbsCurve nurbsCurve = iRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		OnNurbsCurve nurbsCurve2 = iOutsideRail.GetNurbsCurve();
		double value = 0.0;
		if (!iParameter.GetDouble(ref value))
		{
			return UpdateMissing("ProfilePlacerSimple: Unable to find parameter");
		}
		double value2 = 1.0;
		iOutsideRailScale.GetDouble(ref value2);
		double number = 0.0;
		if (!((OnCurve)nurbsCurve).GetNormalizedArcLengthPoint(value, ref number))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		oParam.SetData(new CSNumberData(number));
		OnXform x = new OnXform();
		CSUtilGeometry.OrientProfileToCurveNalp((IOnCurve)(object)nurbsCurve, value, ref x, value2 / 2.0);
		oXform.SetData(new CSXformData(x.Convert()));
		if (nurbsCurve2 != null)
		{
			OnLineCurve val = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 30.0, 0.0));
			val.Transform((IOnXform)(object)x);
			ArrayOnX_EVENT val2 = new ArrayOnX_EVENT();
			((OnCurve)val).IntersectCurve((IOnCurve)(object)nurbsCurve2, ref val2);
			if (val2.Count() > 0)
			{
				On3dPoint val3 = new On3dPoint();
				On3dPoint obj = ((OnCurve)val).PointAtStart();
				val3 = val2[0][0];
				double num = obj.DistanceTo((IOn3dPoint)(object)val3);
				if (num < 0.01)
				{
					num = 0.01;
				}
				oYSize.SetData(new CSNumberData(num));
				double num2 = 0.0;
				num2 = val2[0][0];
				if (Math.Abs(value - ((OnCurve)nurbsCurve).Domain().Min()) < 0.0001 && Math.Abs(num2 - ((OnCurve)nurbsCurve2).Domain().Max()) < 0.0001)
				{
					num2 = ((OnCurve)nurbsCurve2).Domain().Min();
				}
				if (Math.Abs(value - ((OnCurve)nurbsCurve).Domain().Max()) < 0.0001 && Math.Abs(num2 - ((OnCurve)nurbsCurve2).Domain().Min()) < 0.0001)
				{
					num2 = ((OnCurve)nurbsCurve2).Domain().Max();
				}
				oParam2.SetData(new CSNumberData(num2));
			}
		}
		return base.Update();
	}
}
