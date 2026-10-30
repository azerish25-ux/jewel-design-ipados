using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfileBlend4Rail", "{DF22A32B-3F5C-4092-ACE6-B72683CB3317}")]
public sealed class CSProfileBlend4Rail : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iBlendAmount;

	public readonly CSInputChannel iBlendAmount1;

	public readonly CSInputChannel iBlendAmount2;

	public CSOutputChannel oSurface;

	public CSProfileBlend4Rail(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		CSData arrayItem = iCurve1.GetFlattenArray().GetArrayItem(0);
		OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing curve 1");
		}
		CSData arrayItem2 = iCurve2.GetFlattenArray().GetArrayItem(0);
		OnNurbsCurve val2 = arrayItem2.GetNurbsCurve().Convert();
		if (val2 == null)
		{
			return UpdateMissing("Missing curve 2");
		}
		double value = 1.0;
		double value2 = 1.0;
		double value3 = 1.0;
		iBlendAmount.GetDouble(ref value3);
		iBlendAmount1.GetDouble(ref value);
		iBlendAmount2.GetDouble(ref value2);
		val = val.Duplicate();
		val2 = val2.Duplicate();
		OnPlane val3 = new OnPlane();
		OnPlane val4 = new OnPlane();
		((OnCurve)val).IsPlanar(val3);
		((OnCurve)val2).IsPlanar(val4);
		if (!val3.IsValid() || !val4.IsValid())
		{
			return UpdateError("Nonplanar curves");
		}
		if (!((OnCurve)val).IsClosed() || !((OnCurve)val2).IsClosed())
		{
			return UpdateError("Open curves");
		}
		On3dPoint val5 = ((OnCurve)val).PointAtStart();
		On3dPoint val6 = ((OnCurve)val2).PointAtStart();
		On3dVector val7 = val5 - (IOn3dPoint)(object)val6;
		val7.Unitize();
		On3dVector val8 = new On3dVector(val3.Normal());
		On3dVector val9 = new On3dVector(val4.Normal());
		if (val8 * (IOn3dVector)(object)val7 > 0.0)
		{
			val8.Reverse();
		}
		if (val9 * (IOn3dVector)(object)val7 > 0.0)
		{
			val9.Reverse();
		}
		if (Math.Abs(val8 * (IOn3dVector)(object)val7) < 0.01 && Math.Abs(val9 * (IOn3dVector)(object)val7) < 0.01)
		{
			val9 = new On3dVector((IOn3dVector)(object)val8);
		}
		if (value2 < 0.0)
		{
			val9.Reverse();
			value2 = 0.0 - value2;
		}
		if (value < 0.0)
		{
			val8.Reverse();
			value = 0.0 - value;
		}
		double num = val5.DistanceTo((IOn3dPoint)(object)val6);
		On3dPointArray val10 = new On3dPointArray();
		((ArrayOn3dPoint)val10).Empty();
		((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)val5);
		((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)(val5 + (IOn3dVector)(object)(val8 * num * 0.2 * value3 * value)));
		((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)(val5 + (IOn3dVector)(object)(val8 * num * 0.4 * value3 * value)));
		((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)(val6 - (IOn3dVector)(object)(val9 * num * 0.4 * value3 * value2)));
		((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)(val6 - (IOn3dVector)(object)(val9 * num * 0.2 * value3 * value2)));
		((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)val6);
		OnBezierCurve val11 = new OnBezierCurve((IOn3dPointArray)val10);
		OnNurbsCurve val12 = new OnNurbsCurve();
		val11.GetNurbForm(ref val12);
		val12.RemoveShortSegments(0.01);
		val12.RepairBadKnots();
		List<IOnCurve> list = new List<IOnCurve>
		{
			(IOnCurve)(object)arrayItem.GetCurveSegment(0, 4, fromStart: false).Convert(),
			(IOnCurve)(object)arrayItem.GetCurveSegment(1, 4, fromStart: false).Convert(),
			(IOnCurve)(object)arrayItem.GetCurveSegment(2, 4, fromStart: false).Convert(),
			(IOnCurve)(object)arrayItem.GetCurveSegment(3, 4, fromStart: false).Convert(),
			(IOnCurve)(object)arrayItem2.GetCurveSegment(0, 4, fromStart: false).Convert(),
			(IOnCurve)(object)arrayItem2.GetCurveSegment(1, 4, fromStart: false).Convert(),
			(IOnCurve)(object)arrayItem2.GetCurveSegment(2, 4, fromStart: false).Convert(),
			(IOnCurve)(object)arrayItem2.GetCurveSegment(3, 4, fromStart: false).Convert()
		};
		List<double> list2 = new List<double>
		{
			val11.Domain().Min(),
			val11.Domain().Max()
		};
		OnBrep brep = CSUtilRhino.JoinAllBreps(CSUtilSweep.Sweep1C4Simple((OnCurve)(object)val12, list.ToArray(), list2.ToArray(), closed: false).ToArray(), CSApplicationHelper.Application.Tolerance);
		oSurface.SetData(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
		return base.Update();
	}
}
