using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Rails;

[CSBlock("RingRailBend", "{4AEF740F-6410-4aac-86FC-F93C31F60004}")]
public sealed class CSRingRailBend : CSBlock
{
	public CSInputChannel IRingRail { get; }

	public CSInputChannel ISourceAngle { get; }

	public CSInputChannel ISourceAngleSize { get; }

	public CSInputChannel IDestinationAngle { get; }

	public CSInputChannel IDestinationAngleSize { get; }

	public CSOutputChannel ORingRail { get; }

	public CSOutputChannel OLine0 { get; }

	public CSOutputChannel OLine1 { get; }

	public CSRingRailBend(string identifier)
		: base(identifier)
	{
		IRingRail = new CSInputChannel(GetPropertyName(() => IRingRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		ISourceAngle = new CSInputChannel(GetPropertyName(() => ISourceAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		ISourceAngleSize = new CSInputChannel(GetPropertyName(() => ISourceAngleSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IDestinationAngle = new CSInputChannel(GetPropertyName(() => IDestinationAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IDestinationAngleSize = new CSInputChannel(GetPropertyName(() => IDestinationAngleSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		ORingRail = new CSOutputChannel(GetPropertyName(() => ORingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		OLine0 = new CSOutputChannel(GetPropertyName(() => OLine0), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OLine1 = new CSOutputChannel(GetPropertyName(() => OLine1), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Expected O, but got Unknown
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		OnNurbsCurve nurbsCurve = IRingRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing ring rail");
		}
		double value = 0.0;
		ISourceAngle.GetDouble(ref value);
		double value2 = 0.0;
		ISourceAngleSize.GetDouble(ref value2);
		double value3 = 0.0;
		IDestinationAngle.GetDouble(ref value3);
		double value4 = 0.0;
		IDestinationAngleSize.GetDouble(ref value4);
		nurbsCurve = RhUtil.RhinoRebuildCurve((IOnCurve)(object)nurbsCurve, 3, 80);
		value = CSBlockUtil.CSUtilMath.ToRadians(value);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		value3 = value + CSBlockUtil.CSUtilMath.ToRadians(value3);
		value4 = value2 + CSBlockUtil.CSUtilMath.ToRadians(value4);
		double num = CSBlockUtil.CSUtilMath.ToRadians(180.0);
		double num2 = value;
		double num3 = value3;
		double num4 = value + value2;
		double num5 = value3 + value4;
		double num6 = num - num4;
		double num7 = num - num5;
		On4dPoint val = new On4dPoint();
		for (int i = 0; i < nurbsCurve.m_cv_count; i++)
		{
			nurbsCurve.GetCV(i, ref val);
			double num8 = CSBlockUtil.CSUtilMath.AngleOfPointD(val.x, val.z);
			double num9 = Math.Sqrt(val.x * val.x + val.z * val.z);
			if (num8 > num)
			{
				num8 -= CSBlockUtil.CSUtilMath.ToRadians(360.0);
			}
			double num10 = Math.Abs(num8);
			num10 = ((num10 < num2) ? (num10 * num3 / num2) : ((!(num10 < num4)) ? (num - (num - num10) * num7 / num6) : ((num10 - value) * value4 / value2 + value3)));
			if (num8 < 0.0)
			{
				num10 = 0.0 - num10;
			}
			val.x = num9 * Math.Sin(num10);
			val.z = num9 * Math.Cos(num10);
			nurbsCurve.SetCV(i, (IOn4dPoint)(object)val);
		}
		OnLineCurve val2 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint(12.0 * Math.Sin(num3), 0.0, 12.0 * Math.Cos(num3)));
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dPoint)new On3dPoint(12.0 * Math.Sin(num5), 0.0, 12.0 * Math.Cos(num5)));
		OnNurbsCurve nCrv = new OnNurbsCurve();
		OnNurbsCurve nCrv2 = new OnNurbsCurve();
		((OnCurve)val2).GetNurbForm(ref nCrv);
		((OnCurve)val3).GetNurbForm(ref nCrv2);
		ORingRail.SetData(new CSOutsideRingRailData(nurbsCurve.Convert()));
		OLine0.SetData(new CSOnGeometryData(nCrv.Convert()));
		OLine1.SetData(new CSOnGeometryData(nCrv2.Convert()));
		return base.Update();
	}
}
