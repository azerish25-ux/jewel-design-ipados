using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

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
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_0240: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Expected O, but got Unknown
		if (iBrep == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSData data = iBrep.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Surface");
		}
		OnBrep val = data.GetBrep().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing Surface");
		}
		if (val.m_S.Count() < 1)
		{
			return UpdateMissing("Bad Brep");
		}
		OnInterval val2 = val.m_S[0].Domain(0);
		OnInterval val3 = val.m_S[0].Domain(1);
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
		double num = val2.ParameterAt(value2);
		double num2 = val3.ParameterAt(value3);
		On3dPoint val4 = new On3dPoint(num, num2, 0.0);
		double num3 = val2.Length() + val3.Length();
		On3dVector val5 = new On3dVector(num3, 0.0, 0.0);
		val5.Rotate(value * OnUtil.On_PI * 2.0, OnUtil.On_zaxis);
		OnLine val6 = new OnLine((IOn3dPoint)(object)(val4 - (IOn3dVector)(object)val5), (IOn3dPoint)(object)(val4 + (IOn3dVector)(object)val5));
		OnLine val7 = new OnLine((IOn3dPoint)new On3dPoint(val2.Min(), num3, 0.0), (IOn3dPoint)new On3dPoint(val2.Min(), 0.0 - num3, 0.0));
		OnLine val8 = new OnLine((IOn3dPoint)new On3dPoint(val2.Max(), num3, 0.0), (IOn3dPoint)new On3dPoint(val2.Max(), 0.0 - num3, 0.0));
		OnLine val9 = new OnLine((IOn3dPoint)new On3dPoint(num3, val3.Min(), 0.0), (IOn3dPoint)new On3dPoint(0.0 - num3, val3.Min(), 0.0));
		OnLine val10 = new OnLine((IOn3dPoint)new On3dPoint(num3, val3.Max(), 0.0), (IOn3dPoint)new On3dPoint(0.0 - num3, val3.Max(), 0.0));
		double num4 = 0.0;
		double num5 = 0.0;
		if (OnUtil.ON_Intersect((IOnLine)(object)val6, (IOnLine)(object)val7, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			if (val6.to.x < val6.from.x)
			{
				val6.to = val6.PointAt(num4);
			}
			else
			{
				val6.from = val6.PointAt(num4);
			}
		}
		if (OnUtil.ON_Intersect((IOnLine)(object)val6, (IOnLine)(object)val8, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			if (val6.to.x < val6.from.x)
			{
				val6.from = val6.PointAt(num4);
			}
			else
			{
				val6.to = val6.PointAt(num4);
			}
		}
		if (OnUtil.ON_Intersect((IOnLine)(object)val6, (IOnLine)(object)val9, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			if (val6.to.y > val6.from.y)
			{
				val6.from = val6.PointAt(num4);
			}
			else
			{
				val6.to = val6.PointAt(num4);
			}
		}
		if (OnUtil.ON_Intersect((IOnLine)(object)val6, (IOnLine)(object)val10, ref num4, ref num5) && num4 > 0.0 && num4 < 1.0)
		{
			if (val6.to.y > val6.from.y)
			{
				val6.to = val6.PointAt(num4);
			}
			else
			{
				val6.from = val6.PointAt(num4);
			}
		}
		OnLineCurve val11 = new OnLineCurve((IOnLine)(object)val6);
		OnCurve val12 = val.m_S[0].Pushup((IOnCurve)(object)val11, 0.01);
		if (val12 == null)
		{
			return UpdateError("Failed Pushup");
		}
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val12.Convert()));
		return base.Update();
	}
}
