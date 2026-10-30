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
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		OnNurbsSurface onNurbsSurface = flattenArray.GetArrayItem(0).GetNurbsSurface().Convert();
		if (onNurbsSurface == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSDataArray flattenArray2 = iCurve.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curve");
		}
		OnBoundingBox tight_bbox = new OnBoundingBox();
		onNurbsSurface.GetTightBoundingBox(ref tight_bbox);
		int value = 0;
		iMode.GetInteger(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		List<OnSurface> list = new List<OnSurface>();
		for (int i = 0; i < flattenArray2.GetArrayCount(); i++)
		{
			OnNurbsCurve onNurbsCurve = flattenArray2.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				continue;
			}
			OnBoundingBox tight_bbox2 = new OnBoundingBox();
			onNurbsCurve.GetTightBoundingBox(ref tight_bbox2);
			OnBoundingBox onBoundingBox = new OnBoundingBox();
			onBoundingBox.Union(tight_bbox2);
			onBoundingBox.Union(tight_bbox);
			switch (value)
			{
			case 0:
			{
				OnCurve onCurve = onNurbsSurface.Pullback(onNurbsCurve, 0.01);
				if (onCurve == null)
				{
					return UpdateError("Pullback failed");
				}
				if (onCurve.TangentAt(onCurve.Domain().Min()).x < 0.0)
				{
					onCurve.Reverse();
				}
				OnCurve onCurve2 = onNurbsSurface.Pushup(onCurve, 0.0);
				if (onCurve2 == null)
				{
					return UpdateError("Pushup failed");
				}
				cSDataArray.Append(new CSOnGeometryData(onCurve2.Convert()));
				break;
			}
			case 1:
			{
				OnPlane onPlane = new OnPlane();
				if (onNurbsCurve.IsPlanar(onPlane, CSApplicationHelper.Application.Tolerance))
				{
					OnSurface item5 = CSUtilGeometry.ExtrudeSurface(onNurbsCurve, onPlane.Normal(), onBoundingBox.Diagonal().Length(), bothSides: true);
					list.Add(item5);
				}
				break;
			}
			case 2:
			{
				OnSurface item4 = RhUtil.RhinoExtrudeCurveToPoint(onNurbsCurve, OnUtil.On_origin);
				list.Add(item4);
				break;
			}
			case 3:
			{
				OnSurface item3 = CSUtilGeometry.ExtrudeSurface(onNurbsCurve, OnUtil.On_zaxis, onBoundingBox.Diagonal().Length(), bothSides: true);
				list.Add(item3);
				break;
			}
			case 4:
			{
				OnSurface item2 = CSUtilGeometry.ExtrudeSurface(onNurbsCurve, OnUtil.On_xaxis, onBoundingBox.Diagonal().Length(), bothSides: true);
				list.Add(item2);
				break;
			}
			case 5:
			{
				OnSurface item = CSUtilGeometry.ExtrudeSurface(onNurbsCurve, OnUtil.On_yaxis, onBoundingBox.Diagonal().Length(), bothSides: true);
				list.Add(item);
				break;
			}
			}
		}
		if (value != 0)
		{
			foreach (OnSurface item6 in list)
			{
				if (item6 == null)
				{
					continue;
				}
				RhUtil.RhinoIntersectSurfaces(onNurbsSurface, item6, 0.01, out var curves, out var _);
				OnCurve[] output = new OnCurve[0];
				IOnCurve[] input_curves = curves;
				RhUtil.RhinoMergeCurves(input_curves, ref output);
				if (output == null)
				{
					return UpdateError("Merge curves failed");
				}
				OnCurve[] array = output;
				foreach (OnCurve obj in array)
				{
					OnNurbsCurve nurbs_curve = new OnNurbsCurve();
					obj.GetNurbForm(ref nurbs_curve);
					if (nurbs_curve != null)
					{
						double s = 0.0;
						double t = 0.0;
						On3dPoint on3dPoint = nurbs_curve.PointAtStart();
						onNurbsSurface.GetClosestPoint(on3dPoint, ref s, ref t);
						On3dVector on3dVector = nurbs_curve.TangentAt(nurbs_curve.Domain().Min());
						double s2 = 0.0;
						double t2 = 0.0;
						onNurbsSurface.GetClosestPoint(on3dPoint + on3dVector / 1000.0, ref s2, ref t2);
						double t3 = 0.0;
						double t4 = 0.0;
						onNurbsSurface.GetDomain(0, ref t3, ref t4);
						double num = Math.Abs(t3 - t4);
						bool flag = Math.Abs(s - s2) > num * 0.9;
						if (s > s2 != flag)
						{
							nurbs_curve.Reverse();
						}
						cSDataArray.Append(new CSOnGeometryData(nurbs_curve.Convert()));
					}
				}
			}
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
