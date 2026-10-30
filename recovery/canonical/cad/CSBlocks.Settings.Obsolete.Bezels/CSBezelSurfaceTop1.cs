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
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
			onNurbsCurve2.Transform(inverseTransform.Convert());
			OnNurbsCurve onNurbsCurve3 = arrayItem2.GetNurbsCurve().Convert();
			if (onNurbsCurve3 == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve onNurbsCurve4 = onNurbsCurve3.Duplicate();
			onNurbsCurve4.Transform(inverseTransform.Convert());
			OnBrep srf = null;
			if (SurfaceBezelTop(onNurbsCurve2, onNurbsCurve4, value, ref srf))
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
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		On3dPoint on3dPoint = rail1.PointAtStart();
		On3dPoint on3dPoint2 = rail2.PointAtStart();
		OnLineCurve onLineCurve = new OnLineCurve(on3dPoint, on3dPoint2);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		if (fillet == 0.0)
		{
			onLineCurve.GetNurbForm(ref nurbs_curve);
		}
		else
		{
			On3dPoint on3dPoint3 = onLineCurve.PointAt(onLineCurve.Domain().Mid());
			double length = 0.0;
			if (!onLineCurve.GetLength(ref length))
			{
				return false;
			}
			double num = length / 2.0;
			On3dPoint on3dPoint4 = on3dPoint3;
			on3dPoint4.z += num * fillet;
			On3dPoint r = on3dPoint2;
			new OnArcCurve(new OnArc(on3dPoint, on3dPoint4, r)).GetNurbForm(ref nurbs_curve);
		}
		OnCurve[] array = new OnCurve[1] { nurbs_curve };
		IOnCurve[] profiles = array;
		if (!CSUtilSweep.Sweep2(rail1, rail2, profiles, new double[1] { rail1.Domain().Min() }, new double[1] { rail2.Domain().Min() }, out var breps, closed: true, maintainHeight: true))
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
		if (!breps[0].IsValid())
		{
			return false;
		}
		srf = breps[0];
		return true;
	}
}
