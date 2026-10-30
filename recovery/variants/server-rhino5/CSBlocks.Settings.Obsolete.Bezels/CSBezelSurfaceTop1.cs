using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("BezelSurfaceTop_V1", "{C8E15729-C852-4505-AA7D-FBEEC2FF69A0}")]
public sealed class CSBezelSurfaceTop1 : CSBlock
{
	public CSInputChannel iRail1 { get; }

	public CSInputChannel iRail2 { get; }

	public CSInputChannel iFillet { get; }

	public CSOutputChannel oBrep { get; }

	public CSBezelSurfaceTop1(string identifier)
		: base(identifier)
	{
		iRail1 = new CSInputChannel(GetPropertyName(() => iRail1), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iRail2 = new CSInputChannel(GetPropertyName(() => iRail2), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iFillet = new CSInputChannel(GetPropertyName(() => iFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iRail1.GetFlattenArray();
		CSDataArray flattenArray2 = iRail2.GetFlattenArray();
		if (flattenArray.GetArrayCount() != flattenArray2.GetArrayCount())
		{
			return UpdateError("Rail count mismatch");
		}
		double value = 0.0;
		iFillet.GetDouble(ref value);
		value /= 100.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSData arrayItem2 = flattenArray2.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
			if (!value2.IsValid)
			{
				value2 = Transform.Identity;
			}
			value2.TryGetInverse(out var inverseTransform);
			OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
			if (val == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve val2 = val.Duplicate();
			val2.Transform((IOnXform)(object)inverseTransform.Convert());
			OnNurbsCurve val3 = arrayItem2.GetNurbsCurve().Convert();
			if (val3 == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve val4 = val3.Duplicate();
			val4.Transform((IOnXform)(object)inverseTransform.Convert());
			OnBrep srf = null;
			if (SurfaceBezelTop(val2, val4, value, ref srf))
			{
				cSDataArray.Append(new CSOnGeometryData(srf.Convert(), value2));
			}
			else
			{
				cSDataArray.Append(new CSData());
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oBrep.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oBrep.SetData(cSDataArray);
		}
		return base.Update();
	}

	private static bool SurfaceBezelTop(OnNurbsCurve rail1, OnNurbsCurve rail2, double fillet, ref OnBrep srf)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		On3dPoint val = ((OnCurve)rail1).PointAtStart();
		On3dPoint val2 = ((OnCurve)rail2).PointAtStart();
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2);
		OnNurbsCurve val4 = new OnNurbsCurve();
		if (fillet == 0.0)
		{
			((OnCurve)val3).GetNurbForm(ref val4);
		}
		else
		{
			On3dPoint val5 = ((OnCurve)val3).PointAt(((OnCurve)val3).Domain().Mid());
			double num = 0.0;
			if (!((OnCurve)val3).GetLength(ref num))
			{
				return false;
			}
			double num2 = num / 2.0;
			On3dPoint val6 = val5;
			val6.z += num2 * fillet;
			On3dPoint val7 = val2;
			((OnCurve)new OnArcCurve((IOnArc)new OnArc((IOn3dPoint)(object)val, (IOn3dPoint)(object)val6, (IOn3dPoint)(object)val7))).GetNurbForm(ref val4);
		}
		OnCurve[] array = (OnCurve[])(object)new OnCurve[1] { (OnCurve)val4 };
		IOnCurve[] profiles = (IOnCurve[])(object)array;
		if (!CSUtilSweep.Sweep2((IOnCurve)(object)rail1, (IOnCurve)(object)rail2, profiles, new double[1] { ((OnCurve)rail1).Domain().Min() }, new double[1] { ((OnCurve)rail2).Domain().Min() }, out var breps, closed: true, maintainHeight: true))
		{
			CS.CSUtil.Print("Failed Sweep2\n");
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		if (breps.Length < 1)
		{
			return false;
		}
		if (!((OnObject)breps[0]).IsValid())
		{
			return false;
		}
		srf = breps[0];
		return true;
	}
}
