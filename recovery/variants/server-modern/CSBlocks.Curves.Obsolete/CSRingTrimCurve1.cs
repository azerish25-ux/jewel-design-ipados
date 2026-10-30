using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("RingTrimCurve_V1", "{2A6B01E2-2B48-471c-B31A-709E265DF9F8}")]
public sealed class CSRingTrimCurve1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iStartAngle { get; }

	public CSInputChannel iEndAngle { get; }

	public CSOutputChannel oCurves { get; }

	public CSRingTrimCurve1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iStartAngle = new CSInputChannel(GetPropertyName(() => iStartAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndAngle = new CSInputChannel(GetPropertyName(() => iEndAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		if (iCurve == null)
		{
			return UpdateMissing("Curve Input Missing");
		}
		CSData data = iCurve.Data;
		if (data == null)
		{
			return UpdateError("Curve Input Invalid");
		}
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
		double value = 10.0;
		double value2 = 45.0;
		if (iStartAngle != null)
		{
			iStartAngle.GetDouble(ref value);
		}
		if (iEndAngle != null)
		{
			iEndAngle.GetDouble(ref value2);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnNurbsCurve val = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val == null)
			{
				continue;
			}
			OnBoundingBox val2 = new OnBoundingBox();
			((OnGeometry)val).GetBoundingBox(ref val2);
			OnPlaneSurface val3 = new OnPlaneSurface((IOnPlane)new OnPlane(OnUtil.On_origin, OnUtil.On_xaxis));
			((OnGeometry)val3).Translate((IOn3dVector)(object)(new On3dPoint(OnUtil.On_origin) - (IOn3dPoint)(object)((OnSurface)val3).PointAt(0.5, 0.0)));
			double num = Math.Max(val2.m_max.x, 0.0 - val2.m_min.x) + Math.Max(val2.m_max.z, 0.0 - val2.m_min.z) + 1.0;
			double num2 = Math.Max(val2.m_max.y, 0.0 - val2.m_min.y) + 1.0;
			OnXform val4 = new OnXform();
			val4.Scale(1.0, num2 * 2.1, num * 2.1);
			val3.Transform((IOnXform)(object)val4);
			OnPlaneSurface val5 = new OnPlaneSurface((IOnPlaneSurface)(object)val3);
			((OnGeometry)val3).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value), OnUtil.On_yaxis, OnUtil.On_origin);
			((OnGeometry)val5).Rotate(CSBlockUtil.CSUtilMath.ToRadians(value2), OnUtil.On_yaxis, OnUtil.On_origin);
			ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
			((OnCurve)val).IntersectSurface((IOnSurface)(object)val3, ref val6);
			if (val6.Count() > 0)
			{
				if (((OnCurve)val).IsClosed())
				{
					((OnCurve)val).ChangeClosedCurveSeam(val6[0][0]);
					val6.Empty();
					((OnCurve)val).IntersectSurface((IOnSurface)(object)val5, ref val6);
					if (val6.Count() > 0)
					{
						((OnCurve)val).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), val6[0][0]));
					}
				}
				else
				{
					ArrayOnX_EVENT val7 = new ArrayOnX_EVENT();
					((OnCurve)val).IntersectSurface((IOnSurface)(object)val5, ref val7);
					if (val7.Count() > 0)
					{
						((OnCurve)val).Trim((IOnInterval)new OnInterval(val6[0][0], val7[0][0]));
					}
					else
					{
						((OnCurve)val).Trim((IOnInterval)new OnInterval(val6[0][0], ((OnCurve)val).Domain().Max()));
					}
				}
			}
			else
			{
				val6.Empty();
				((OnCurve)val).IntersectSurface((IOnSurface)(object)val5, ref val6);
				if (val6.Count() > 0)
				{
					((OnCurve)val).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), val6[0][0]));
				}
			}
			if (flattenArray.GetArrayCount() > 1)
			{
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val.Convert()));
			}
			else
			{
				oCurves.SetData(new CSOnGeometryData((GeometryBase)(object)val.Convert()));
			}
		}
		if (flattenArray.GetArrayCount() > 1)
		{
			oCurves.SetData(cSDataArray);
		}
		return base.Update();
	}
}
