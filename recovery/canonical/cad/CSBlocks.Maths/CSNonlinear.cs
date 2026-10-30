using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Maths;

[CSBlock("Nonlinear", "{7D5700C0-4444-4C85-B521-11E2931963AD}")]
public sealed class CSNonlinear : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iInputMaximum;

	public readonly CSInputChannel iInputMinimum;

	public readonly CSInputChannel iInputValue;

	public readonly CSInputChannel iOutputMaximum;

	public readonly CSInputChannel iOutputMinimum;

	public readonly CSOutputChannel oOutputValue;

	public readonly CSOutputChannel oDebug;

	public CSNonlinear(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iInputMaximum = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iInputMinimum = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iInputValue = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutputMaximum = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutputMinimum = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oOutputValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oDebug = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		IOnNurbsCurve nurbsCurve = iCurve.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Curve");
		}
		double value = 10.0;
		iInputMaximum.GetDouble(ref value);
		double value2 = 0.0;
		iInputMinimum.GetDouble(ref value2);
		double value3 = 0.0;
		iInputValue.GetDouble(ref value3);
		if (value3 > value)
		{
			value3 = value;
		}
		if (value3 < value2)
		{
			value3 = value2;
		}
		double value4 = 10.0;
		iOutputMaximum.GetDouble(ref value4);
		double value5 = 0.0;
		iOutputMinimum.GetDouble(ref value5);
		On3dPoint on3dPoint;
		On3dPoint on3dPoint2;
		if (OnUtil.ON_DotProduct(nurbsCurve.PointAtEnd() - nurbsCurve.PointAtStart(), OnUtil.On_xaxis) < 0.0)
		{
			on3dPoint = nurbsCurve.PointAtEnd();
			on3dPoint2 = nurbsCurve.PointAtStart();
		}
		else
		{
			on3dPoint = nurbsCurve.PointAtStart();
			on3dPoint2 = nurbsCurve.PointAtEnd();
		}
		On2dPoint on2dPoint = new On2dPoint(on3dPoint);
		On2dPoint on2dPoint2 = new On2dPoint(value2, value5);
		OnXform onXform = new OnXform();
		onXform.Translation(new On3dVector(on2dPoint2 - on2dPoint));
		double x_scale_factor = (value - value2) / (on3dPoint2.x - on3dPoint.x);
		double y_scale_factor = (value4 - value5) / (on3dPoint2.y - on3dPoint.y);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(x_scale_factor, y_scale_factor, 1.0);
		OnCurve onCurve = nurbsCurve.DuplicateCurve();
		onCurve.Transform(onXform * onXform2);
		On2dPoint arg = new On2dPoint(value3, 100.0);
		On2dPoint arg2 = new On2dPoint(value3, -100.0);
		OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onCurve.IntersectCurve(onLineCurve, ref x);
		if (x.Count() <= 0)
		{
			return UpdateError("Input value out of range");
		}
		On3dPoint on3dPoint3 = new On3dPoint(OnUtil.On_UNSET_POINT);
		for (int i = 0; i < x.Count(); i++)
		{
			if (on3dPoint3.Equals(OnUtil.On_UNSET_POINT))
			{
				on3dPoint3 = x[i].get_m_pointA(0);
				continue;
			}
			On3dPoint on3dPoint4 = x[i].get_m_pointA(0);
			if (on3dPoint4.y >= on3dPoint3.y)
			{
				on3dPoint3 = on3dPoint4;
			}
		}
		on3dPoint3 = x[0].get_m_pointA(0);
		CSDataArray cSDataArray = new CSDataArray();
		cSDataArray.Append(new CSOnGeometryData(onCurve.Convert(), onXform.Convert() * onXform2.Convert(), applyTransform: false));
		cSDataArray.Append(new CSOnGeometryData(onLineCurve.Convert()));
		cSDataArray.Append(new CSPointData(on3dPoint3.Convert()));
		oOutputValue.SetData(new CSNumberData(on3dPoint3.y));
		oDebug.SetData(cSDataArray);
		return base.Update();
	}
}
