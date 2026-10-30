using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Rails;

[CSBlock("RingRailCustom", "{B14A1813-4240-4a64-981D-8DB2CFE8D82C}")]
public sealed class CSRingRailCustom : CSBlock
{
	public CSInputChannel iRingRadius { get; }

	public CSInputChannel iCurve { get; }

	public CSInputChannel iSpan { get; }

	public CSInputChannel iBlend { get; }

	public CSInputChannel iWidthLock { get; }

	public CSInputChannel iWidth { get; }

	public CSOutputChannel oRingRail { get; }

	public CSRingRailCustom(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(GetPropertyName(() => iRingRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iSpan = new CSInputChannel(GetPropertyName(() => iSpan), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlend = new CSInputChannel(GetPropertyName(() => iBlend), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidthLock = new CSInputChannel(GetPropertyName(() => iWidthLock), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidth = new CSInputChannel(GetPropertyName(() => iWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRingRail = new CSOutputChannel(GetPropertyName(() => oRingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Expected O, but got Unknown
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Expected O, but got Unknown
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Expected O, but got Unknown
		//IL_074d: Expected O, but got Unknown
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Expected O, but got Unknown
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Expected O, but got Unknown
		double value = 0.0;
		if (!iRingRadius.GetDouble(ref value))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Ring Rail\n");
		}
		OnNurbsCurve nurbsCurve = iCurve.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Ring Rail Custom:  Missing Curve\n");
		}
		double value2 = 0.0;
		if (!iSpan.GetDouble(ref value2))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Span\n");
		}
		double value3 = 0.0;
		if (!iBlend.GetDouble(ref value3))
		{
			return UpdateMissing("Ring Rail Custom:  Missing Blend\n");
		}
		double value4 = 0.0;
		iWidthLock.GetDouble(ref value4);
		double value5 = 0.0;
		iWidth.GetDouble(ref value5);
		if (value2 > 340.0)
		{
			value2 = 340.0;
		}
		if (value3 < 10.0)
		{
			value3 = 10.0;
		}
		if (value2 + value3 > 350.0)
		{
			value3 = 350.0 - value2;
		}
		if (value2 == 0.0 || value5 == 0.0)
		{
			OnCircle val = new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, value);
			OnNurbsCurve nCrv = new OnNurbsCurve();
			val.GetNurbForm(ref nCrv);
			oRingRail.SetData(new CSRingRailData(nCrv.Convert(), value));
		}
		else
		{
			if (((OnCurve)nurbsCurve).TangentAt(((OnCurve)nurbsCurve).Domain().Min()).x < 0.0)
			{
				((OnCurve)nurbsCurve).Reverse();
			}
			OnBoundingBox val2 = new OnBoundingBox();
			if (!((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val2))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			double num = value2 / 360.0;
			double num2 = val2.Max().x - val2.Min().x;
			double num3 = num2 / num;
			double num4 = 1.0 - (value2 / 360.0 + value3 / 360.0);
			double num5 = num3 * num4;
			OnXform val3 = new OnXform();
			val3.Translation((IOn3dVector)new On3dVector(0.0 - (val2.Max().x - num2 / 2.0), 0.0, 0.0));
			nurbsCurve.Transform((IOnXform)(object)val3);
			On3dPoint val4 = new On3dPoint(0.0 - num3 / 2.0, 0.0, 0.0);
			On3dPoint val5 = new On3dPoint(0.0 - num3 / 2.0 + num5 / 2.0, 0.0, 0.0);
			OnLineCurve val6 = new OnLineCurve((IOn3dPoint)(object)val4, (IOn3dPoint)(object)val5);
			On3dPoint val7 = new On3dPoint(num3 / 2.0 - num5 / 2.0, 0.0, 0.0);
			On3dPoint val8 = new On3dPoint(num3 / 2.0, 0.0, 0.0);
			OnLineCurve val9 = new OnLineCurve((IOn3dPoint)(object)val7, (IOn3dPoint)(object)val8);
			On3dPoint pointStart = ((OnCurve)val6).PointAt(((OnCurve)val6).Domain().Max());
			On3dVector val10 = ((OnCurve)val6).TangentAt(((OnCurve)val6).Domain().Max());
			val10.Unitize();
			On3dPoint pointEnd = ((OnCurve)nurbsCurve).PointAt(((OnCurve)nurbsCurve).Domain().Min());
			On3dVector val11 = ((OnCurve)nurbsCurve).TangentAt(((OnCurve)nurbsCurve).Domain().Min());
			val11.Unitize();
			val11.Reverse();
			OnNurbsCurve val12 = CSUtilCurves.BlendCurve((IOn3dPoint)(object)pointStart, (IOn3dPoint)(object)pointEnd, (IOn3dVector)(object)val10, (IOn3dVector)(object)val11);
			On3dPoint pointStart2 = ((OnCurve)nurbsCurve).PointAt(((OnCurve)nurbsCurve).Domain().Max());
			On3dVector val13 = ((OnCurve)nurbsCurve).TangentAt(((OnCurve)nurbsCurve).Domain().Max());
			val13.Unitize();
			On3dPoint pointEnd2 = ((OnCurve)val9).PointAt(((OnCurve)val9).Domain().Min());
			On3dVector val14 = ((OnCurve)val9).TangentAt(((OnCurve)val9).Domain().Min());
			val14.Unitize();
			val14.Reverse();
			OnNurbsCurve val15 = CSUtilCurves.BlendCurve((IOn3dPoint)(object)pointStart2, (IOn3dPoint)(object)pointEnd2, (IOn3dVector)(object)val13, (IOn3dVector)(object)val14);
			OnCurve[] obj = new OnCurve[5]
			{
				(OnCurve)val6,
				(OnCurve)val12,
				(OnCurve)nurbsCurve,
				(OnCurve)val15,
				(OnCurve)val9
			};
			OnCurve[] array = null;
			IOnCurve[] array2 = (IOnCurve[])(object)obj;
			RhUtil.RhinoMergeCurves(array2, ref array);
			if (array.Length != 1)
			{
				return UpdateError("RhinoMergeCurves failed");
			}
			OnXform val16 = new OnXform();
			val16.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
			((OnGeometry)array[0]).Transform((IOnXform)(object)val16);
			if (!((OnGeometry)array[0]).GetTightBoundingBox(ref val2))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			num2 = val2.Max().x - val2.Min().x;
			double num6 = val2.Max().y - val2.Min().y;
			if (value4 > 0.5)
			{
				value5 = num2;
			}
			OnXform val17 = new OnXform();
			val17.Translation(0.0 - val2.Min().x, 0.0 - val2.Min().y, 0.0);
			((OnGeometry)array[0]).Transform((IOnXform)(object)val17);
			OnCircle val18 = new OnCircle(OnUtil.On_zx_plane, OnUtil.On_origin, value);
			OnNurbsCurve val19 = new OnNurbsCurve();
			val18.GetNurbForm(ref val19);
			OnSurface val20 = RhUtil.RhinoExtrudeCurveStraight((IOnCurve)(object)val19, OnUtil.On_yaxis, 0.0 - value5);
			if (val20 == null)
			{
				return UpdateError("RhinoExtrudeCurveStraight failed");
			}
			OnXform val21 = new OnXform();
			val21.Translation((IOn3dVector)((value4 > 0.5) ? new On3dVector(0.0, Math.Abs(val2.Min().x), 0.0) : new On3dVector(0.0, value5 * (Math.Abs(val2.Min().x) / num2), 0.0)));
			((OnGeometry)val20).Transform((IOnXform)(object)val21);
			OnInterval val22 = val20.Domain(0);
			OnInterval val23 = val20.Domain(1);
			OnXform val24 = new OnXform();
			val24.Scale((val22[1] - val22[0]) / num2, (val23[1] - val23[0]) / num6, 1.0);
			((OnGeometry)array[0]).Transform((IOnXform)(object)val24);
			OnXform val25 = new OnXform();
			val25.Translation(val22[0], val23[0], 0.0);
			((OnGeometry)array[0]).Transform((IOnXform)(object)val25);
			OnCurve val26 = val20.Pushup((IOnCurve)(object)array[0], CSApplicationHelper.Application.Tolerance);
			if (val26 == null)
			{
				return UpdateError("Pushup failed");
			}
			OnXform val27 = new OnXform();
			val27.Rotation(OnUtil.On_PI, OnUtil.On_yaxis, OnUtil.On_origin);
			((OnGeometry)val26).Transform((IOnXform)(object)val27);
			On3dPoint val28 = val26.PointAtStart();
			double num7 = 0.0;
			bool flag = false;
			if (val28.x == 0.0 && val28.z < 0.0)
			{
				num7 = val26.Domain().Min();
				flag = true;
			}
			if (!flag)
			{
				OnLineCurve val29 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, -50.0, 0.0 - value), (IOn3dPoint)new On3dPoint(0.0, 50.0, 0.0 - value));
				ArrayOnX_EVENT val30 = new ArrayOnX_EVENT();
				val26.IntersectCurve((IOnCurve)(object)val29, ref val30);
				if (val30.Count() > 0)
				{
					double num8 = 0.0;
					int num9 = 0;
					for (int i = 0; i < val30.Count(); i++)
					{
						double num10 = Math.Abs(val26.PointAt(val30[i][0]).y);
						if (i == 0 || num10 < num8)
						{
							num8 = num10;
							num9 = i;
						}
					}
					flag = true;
					num7 = val30[num9][0];
				}
			}
			if (flag)
			{
				On3dVector obj2 = val26.TangentAt(num7);
				double num11 = 0.0;
				double num12 = 0.0;
				double num13 = 0.0;
				val26.GetLength(ref num11);
				if (val26.Domain().Min() == num7)
				{
					num12 = 0.0;
				}
				else
				{
					OnCurve obj3 = val26.Duplicate();
					obj3.Trim((IOnInterval)new OnInterval(obj3.Domain().Min(), num7));
					obj3.GetLength(ref num12);
				}
				if (num12 > num11 / 2.0)
				{
					double num14 = (num12 - num11 / 2.0) / num11;
					val26.GetNormalizedArcLengthPoint(num14, ref num13);
					val26.ChangeClosedCurveSeam(num13);
					RhUtil.RhinoMakeCurveClosed(val26, 0.01);
				}
				else
				{
					double num15 = 1.0 - (num11 / 2.0 - num12) / num11;
					val26.GetNormalizedArcLengthPoint(num15, ref num13);
					val26.ChangeClosedCurveSeam(num13);
					RhUtil.RhinoMakeCurveClosed(val26, 0.01);
				}
				if (obj2.x > 0.0)
				{
					val26.Reverse();
				}
			}
			oRingRail.SetData(new CSRingRailData(val26.Convert(), value));
		}
		return base.Update();
	}
}
