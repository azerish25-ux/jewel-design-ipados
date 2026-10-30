using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Core.Obsolete;

[Obsolete]
[CSBlock("BrepUVCurve_V1", "{04469B29-BB72-4169-BB05-D7A75FB8554C}")]
public sealed class CSBrepUVCurve1 : CSBlock
{
	public CSInputChannel iBrep { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iStartX { get; }

	public CSInputChannel iStartY { get; }

	public CSOutputChannel oCurve { get; }

	public CSBrepUVCurve1(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(GetPropertyName(() => iBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStartX = new CSInputChannel(GetPropertyName(() => iStartX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStartY = new CSInputChannel(GetPropertyName(() => iStartY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		if (iBrep == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSData data = iBrep.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Surface");
		}
		OnBrep onBrep = data.GetBrep().Convert();
		if (onBrep == null)
		{
			return UpdateMissing("Missing Surface");
		}
		if (onBrep.m_S.Count() < 1)
		{
			return UpdateMissing("Bad Brep");
		}
		OnInterval onInterval = onBrep.m_S[0].Domain(0);
		OnInterval onInterval2 = onBrep.m_S[0].Domain(1);
		double value = 0.0;
		if (iAngle != null)
		{
			iAngle.GetDouble(ref value);
		}
		double value2 = 0.5;
		double value3 = 0.5;
		if (iStartX != null)
		{
			iStartX.GetDouble(ref value2);
		}
		if (iStartY != null)
		{
			iStartY.GetDouble(ref value3);
		}
		double x = onInterval.ParameterAt(value2);
		double y = onInterval2.ParameterAt(value3);
		On3dPoint on3dPoint = new On3dPoint(x, y, 0.0);
		double num = onInterval.Length() + onInterval2.Length();
		On3dVector on3dVector = new On3dVector(num, 0.0, 0.0);
		on3dVector.Rotate(value * OnUtil.On_PI * 2.0, OnUtil.On_zaxis);
		OnLine onLine = new OnLine(on3dPoint - on3dVector, on3dPoint + on3dVector);
		OnLine lineB = new OnLine(new On3dPoint(onInterval.Min(), num, 0.0), new On3dPoint(onInterval.Min(), 0.0 - num, 0.0));
		OnLine lineB2 = new OnLine(new On3dPoint(onInterval.Max(), num, 0.0), new On3dPoint(onInterval.Max(), 0.0 - num, 0.0));
		OnLine lineB3 = new OnLine(new On3dPoint(num, onInterval2.Min(), 0.0), new On3dPoint(0.0 - num, onInterval2.Min(), 0.0));
		OnLine lineB4 = new OnLine(new On3dPoint(num, onInterval2.Max(), 0.0), new On3dPoint(0.0 - num, onInterval2.Max(), 0.0));
		double a = 0.0;
		double b = 0.0;
		if (OnUtil.ON_Intersect(onLine, lineB, ref a, ref b) && a > 0.0 && a < 1.0)
		{
			if (onLine.to.x < onLine.from.x)
			{
				onLine.to = onLine.PointAt(a);
			}
			else
			{
				onLine.from = onLine.PointAt(a);
			}
		}
		if (OnUtil.ON_Intersect(onLine, lineB2, ref a, ref b) && a > 0.0 && a < 1.0)
		{
			if (onLine.to.x < onLine.from.x)
			{
				onLine.from = onLine.PointAt(a);
			}
			else
			{
				onLine.to = onLine.PointAt(a);
			}
		}
		if (OnUtil.ON_Intersect(onLine, lineB3, ref a, ref b) && a > 0.0 && a < 1.0)
		{
			if (onLine.to.y > onLine.from.y)
			{
				onLine.from = onLine.PointAt(a);
			}
			else
			{
				onLine.to = onLine.PointAt(a);
			}
		}
		if (OnUtil.ON_Intersect(onLine, lineB4, ref a, ref b) && a > 0.0 && a < 1.0)
		{
			if (onLine.to.y > onLine.from.y)
			{
				onLine.to = onLine.PointAt(a);
			}
			else
			{
				onLine.from = onLine.PointAt(a);
			}
		}
		OnLineCurve curve_2d = new OnLineCurve(onLine);
		OnCurve onCurve = onBrep.m_S[0].Pushup(curve_2d, 0.01);
		if (onCurve == null)
		{
			return UpdateError("Failed Pushup");
		}
		oCurve.SetData(new CSOnGeometryData(onCurve.Convert()));
		return base.Update();
	}
}
