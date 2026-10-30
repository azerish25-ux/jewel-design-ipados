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
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("PullCurveToSurface", "{B71C44CF-1438-48df-A84C-46C75714E08B}")]
public sealed class CSPullCurveToSurface : CSBlock
{
	public CSOutputChannel oCurve { get; }

	public CSInputChannel iSurface { get; }

	public CSInputChannel iCurve { get; }

	public CSInputChannel iMode { get; }

	public CSPullCurveToSurface(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(GetPropertyName(() => iSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iMode = new CSInputChannel(GetPropertyName(() => iMode), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		OnNurbsSurface val = flattenArray.GetArrayItem(0).GetNurbsSurface().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSDataArray flattenArray2 = iCurve.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curve");
		}
		OnBoundingBox val2 = new OnBoundingBox();
		((OnGeometry)val).GetTightBoundingBox(ref val2);
		int value = 0;
		iMode.GetInteger(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		List<OnSurface> list = new List<OnSurface>();
		for (int i = 0; i < flattenArray2.GetArrayCount(); i++)
		{
			OnNurbsCurve val3 = flattenArray2.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val3 == null)
			{
				continue;
			}
			OnBoundingBox val4 = new OnBoundingBox();
			((OnGeometry)val3).GetTightBoundingBox(ref val4);
			OnBoundingBox val5 = new OnBoundingBox();
			val5.Union((IOnBoundingBox)(object)val4);
			val5.Union((IOnBoundingBox)(object)val2);
			switch (value)
			{
			case 0:
			{
				OnCurve val7 = ((OnSurface)val).Pullback((IOnCurve)(object)val3, 0.01);
				if (val7 == null)
				{
					return UpdateError("Pullback failed");
				}
				if (val7.TangentAt(val7.Domain().Min()).x < 0.0)
				{
					val7.Reverse();
				}
				OnCurve val8 = ((OnSurface)val).Pushup((IOnCurve)(object)val7, 0.0);
				if (val8 == null)
				{
					return UpdateError("Pushup failed");
				}
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val8.Convert()));
				break;
			}
			case 1:
			{
				OnPlane val6 = new OnPlane();
				if (((OnCurve)val3).IsPlanar(val6, CSApplicationHelper.Application.Tolerance))
				{
					OnSurface item5 = (OnSurface)(object)CSUtilGeometry.ExtrudeSurface(val3, val6.Normal(), val5.Diagonal().Length(), bothSides: true);
					list.Add(item5);
				}
				break;
			}
			case 2:
			{
				OnSurface item4 = RhUtil.RhinoExtrudeCurveToPoint((IOnCurve)(object)val3, OnUtil.On_origin);
				list.Add(item4);
				break;
			}
			case 3:
			{
				OnSurface item3 = (OnSurface)(object)CSUtilGeometry.ExtrudeSurface(val3, OnUtil.On_zaxis, val5.Diagonal().Length(), bothSides: true);
				list.Add(item3);
				break;
			}
			case 4:
			{
				OnSurface item2 = (OnSurface)(object)CSUtilGeometry.ExtrudeSurface(val3, OnUtil.On_xaxis, val5.Diagonal().Length(), bothSides: true);
				list.Add(item2);
				break;
			}
			case 5:
			{
				OnSurface item = (OnSurface)(object)CSUtilGeometry.ExtrudeSurface(val3, OnUtil.On_yaxis, val5.Diagonal().Length(), bothSides: true);
				list.Add(item);
				break;
			}
			}
		}
		if (value != 0)
		{
			OnCurve[] array = default(OnCurve[]);
			On3dPointArray val9 = default(On3dPointArray);
			foreach (OnSurface item6 in list)
			{
				if (item6 == null)
				{
					continue;
				}
				RhUtil.RhinoIntersectSurfaces((IOnSurface)(object)val, (IOnSurface)(object)item6, 0.01, ref array, ref val9);
				OnCurve[] array2 = (OnCurve[])(object)new OnCurve[0];
				IOnCurve[] array3 = (IOnCurve[])(object)array;
				RhUtil.RhinoMergeCurves(array3, ref array2);
				if (array2 == null)
				{
					return UpdateError("Merge curves failed");
				}
				OnCurve[] array4 = array2;
				foreach (OnCurve obj in array4)
				{
					OnNurbsCurve val10 = new OnNurbsCurve();
					obj.GetNurbForm(ref val10);
					if (val10 != null)
					{
						double num = 0.0;
						double num2 = 0.0;
						On3dPoint val11 = ((OnCurve)val10).PointAtStart();
						((OnSurface)val).GetClosestPoint((IOn3dPoint)(object)val11, ref num, ref num2);
						On3dVector val12 = ((OnCurve)val10).TangentAt(((OnCurve)val10).Domain().Min());
						double num3 = 0.0;
						double num4 = 0.0;
						((OnSurface)val).GetClosestPoint((IOn3dPoint)(object)(val11 + (IOn3dVector)(object)(val12 / 1000.0)), ref num3, ref num4);
						double num5 = 0.0;
						double num6 = 0.0;
						((OnSurface)val).GetDomain(0, ref num5, ref num6);
						double num7 = Math.Abs(num5 - num6);
						bool flag = Math.Abs(num - num3) > num7 * 0.9;
						if (num > num3 != flag)
						{
							((OnCurve)val10).Reverse();
						}
						cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val10.Convert()));
					}
				}
			}
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
