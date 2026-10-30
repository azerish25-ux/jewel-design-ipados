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
		nurbsCurve = RhUtil.RhinoRebuildCurve(nurbsCurve, 3, 80);
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
		On4dPoint vertex_value = new On4dPoint();
		for (int i = 0; i < nurbsCurve.m_cv_count; i++)
		{
			nurbsCurve.GetCV(i, ref vertex_value);
			double num8 = CSBlockUtil.CSUtilMath.AngleOfPointD(vertex_value.x, vertex_value.z);
			double num9 = Math.Sqrt(vertex_value.x * vertex_value.x + vertex_value.z * vertex_value.z);
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
			vertex_value.x = num9 * Math.Sin(num10);
			vertex_value.z = num9 * Math.Cos(num10);
			nurbsCurve.SetCV(i, vertex_value);
		}
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, 0.0, 0.0), new On3dPoint(12.0 * Math.Sin(num3), 0.0, 12.0 * Math.Cos(num3)));
		OnLineCurve onLineCurve2 = new OnLineCurve(new On3dPoint(0.0, 0.0, 0.0), new On3dPoint(12.0 * Math.Sin(num5), 0.0, 12.0 * Math.Cos(num5)));
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		onLineCurve.GetNurbForm(ref nurbs_curve);
		onLineCurve2.GetNurbForm(ref nurbs_curve2);
		ORingRail.SetData(new CSOutsideRingRailData(nurbsCurve.Convert()));
		OLine0.SetData(new CSOnGeometryData(nurbs_curve.Convert()));
		OLine1.SetData(new CSOnGeometryData(nurbs_curve2.Convert()));
		return base.Update();
	}
}
