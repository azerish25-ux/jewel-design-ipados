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
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_019f: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		IOnNurbsCurve nurbsCurve = (IOnNurbsCurve)(object)iCurve.GetNurbsCurve();
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
		On3dPoint val;
		On3dPoint val2;
		if (OnUtil.ON_DotProduct((IOn3dVector)(object)(((IOnCurve)nurbsCurve).PointAtEnd() - (IOn3dPoint)(object)((IOnCurve)nurbsCurve).PointAtStart()), OnUtil.On_xaxis) < 0.0)
		{
			val = ((IOnCurve)nurbsCurve).PointAtEnd();
			val2 = ((IOnCurve)nurbsCurve).PointAtStart();
		}
		else
		{
			val = ((IOnCurve)nurbsCurve).PointAtStart();
			val2 = ((IOnCurve)nurbsCurve).PointAtEnd();
		}
		On2dPoint val3 = new On2dPoint((IOn3dPoint)(object)val);
		On2dPoint val4 = new On2dPoint(value2, value5);
		OnXform val5 = new OnXform();
		val5.Translation((IOn3dVector)new On3dVector((IOn2dVector)(object)(val4 - (IOn2dPoint)(object)val3)));
		double num = (value - value2) / (val2.x - val.x);
		double num2 = (value4 - value5) / (val2.y - val.y);
		OnXform val6 = new OnXform();
		val6.Scale(num, num2, 1.0);
		OnCurve val7 = ((IOnCurve)nurbsCurve).DuplicateCurve();
		((OnGeometry)val7).Transform((IOnXform)(object)(val5 * (IOnXform)(object)val6));
		On2dPoint val8 = new On2dPoint(value3, 100.0);
		On2dPoint val9 = new On2dPoint(value3, -100.0);
		OnLineCurve val10 = new OnLineCurve((IOn2dPoint)val8, (IOn2dPoint)(object)val9);
		ArrayOnX_EVENT val11 = new ArrayOnX_EVENT();
		val7.IntersectCurve((IOnCurve)(object)val10, ref val11);
		if (val11.Count() <= 0)
		{
			return UpdateError("Input value out of range");
		}
		On3dPoint val12 = new On3dPoint(OnUtil.On_UNSET_POINT);
		for (int i = 0; i < val11.Count(); i++)
		{
			if (val12.Equals(OnUtil.On_UNSET_POINT))
			{
				val12 = val11[i][0];
				continue;
			}
			On3dPoint val13 = val11[i][0];
			if (val13.y >= val12.y)
			{
				val12 = val13;
			}
		}
		val12 = val11[0][0];
		CSDataArray cSDataArray = new CSDataArray();
		cSDataArray.Append(new CSOnGeometryData(val7.Convert(), val5.Convert() * val6.Convert(), applyTransform: false));
		cSDataArray.Append(new CSOnGeometryData(((OnCurve)(object)val10).Convert()));
		cSDataArray.Append(new CSPointData(val12.Convert()));
		oOutputValue.SetData(new CSNumberData(val12.y));
		oDebug.SetData(cSDataArray);
		return base.Update();
	}
}
