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
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Cutters.Obsolete;

[Obsolete]
[CSBlock("HaloCutter_V1", "{15F5D72D-52B5-4bbf-A824-D4E4ECF37A20}")]
public sealed class CSHaloCutter1 : CSBlock
{
	public CSInputChannel iHalo { get; }

	public CSInputChannel iRingRailRadius { get; }

	public CSInputChannel iRingRailOffset { get; }

	public CSInputChannel iHeight { get; }

	public CSInputChannel iLength { get; }

	public CSInputChannel iFlowSurface { get; }

	public CSOutputChannel oCutter { get; }

	public CSOutputChannel oIntersectCurve { get; }

	public CSOutputChannel oIntersectSurface { get; }

	public CSHaloCutter1(string identifier)
		: base(identifier)
	{
		iHalo = new CSInputChannel(GetPropertyName(() => iHalo), this, ChannelDataType.Any, ChannelArrayType.Any);
		iRingRailRadius = new CSInputChannel(GetPropertyName(() => iRingRailRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRingRailOffset = new CSInputChannel(GetPropertyName(() => iRingRailOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLength = new CSInputChannel(GetPropertyName(() => iLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFlowSurface = new CSInputChannel(GetPropertyName(() => iFlowSurface), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCutter = new CSOutputChannel(GetPropertyName(() => oCutter), this, ChannelDataType.Surface, ChannelArrayType.Individual);
		oIntersectCurve = new CSOutputChannel(GetPropertyName(() => oIntersectCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oIntersectSurface = new CSOutputChannel(GetPropertyName(() => oIntersectSurface), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = iHalo.Data;
		if (data == null)
		{
			return UpdateMissing("Halo Missing");
		}
		OnBrep onBrep = data.GetBrep().Convert();
		if (onBrep == null)
		{
			return UpdateError("Halo Brep Missing");
		}
		double value = 0.0;
		iRingRailRadius.GetDouble(ref value);
		double value2 = 0.0;
		iRingRailOffset.GetDouble(ref value2);
		double value3 = 0.0;
		iHeight.GetDouble(ref value3);
		double value4 = 50.0;
		iLength.GetDouble(ref value4);
		int value5 = 0;
		iFlowSurface.GetInteger(ref value5);
		double rotation_angle = CSBlockUtil.CSUtilMath.ToRadians(90.0) * (double)value5;
		CSDataHelper.GetTransformNoArray(data, out var value6);
		if (!value6.IsValid)
		{
			value6 = Transform.Identity;
		}
		On3dVector on3dVector = new On3dVector(OnUtil.On_zaxis);
		on3dVector.Transform(value6.Convert());
		on3dVector.y = 0.0;
		OnXform onXform = new OnXform();
		onXform.Rotation(OnUtil.On_zaxis, on3dVector, OnUtil.On_origin);
		OnXform xform = onXform.Inverse();
		OnCurve cutterProfile = null;
		if (!MakeCutterProfile(value + value2, value3, ref cutterProfile))
		{
			return UpdateError("Make Cutter Failed");
		}
		cutterProfile.Transform(onXform);
		double num = value4 / 2.0;
		OnCurve onCurve = cutterProfile.Duplicate();
		onCurve.Translate(new On3dVector(0.0, 0.0 - num, 0.0));
		OnCurve onCurve2 = cutterProfile.Duplicate();
		onCurve2.Translate(new On3dVector(0.0, num, 0.0));
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.CreateRuledSurface(onCurve, onCurve2);
		OnBrep onBrep2 = CSUtilRhino.CapPlanarHoles(onNurbsSurface.BrepForm(), CSApplicationHelper.Application.Tolerance);
		if (onBrep2 == null)
		{
			return UpdateError("Cap holes failed");
		}
		OnBrep[] array = new OnBrep[1] { onBrep };
		CSApplicationHelper.Application.InRunScriptBlock = true;
		MRhinoBrepObject mRhinoBrepObject = RhUtil.RhinoApp().ActiveDoc().AddBrepObject(array[0]);
		if (mRhinoBrepObject != null)
		{
			array[0] = OnBrep.Cast(mRhinoBrepObject.Brep().DuplicateOnObject());
			RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(mRhinoBrepObject));
		}
		CSApplicationHelper.Application.InRunScriptBlock = false;
		OnBrep[] array2 = new OnBrep[1] { onBrep2 };
		CSApplicationHelper.Application.InRunScriptBlock = true;
		MRhinoBrepObject mRhinoBrepObject2 = RhUtil.RhinoApp().ActiveDoc().AddBrepObject(array2[0]);
		if (mRhinoBrepObject2 != null)
		{
			array2[0] = OnBrep.Cast(mRhinoBrepObject2.Brep().DuplicateOnObject());
			RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(mRhinoBrepObject2));
		}
		CSApplicationHelper.Application.InRunScriptBlock = false;
		bool bResult = false;
		OnBrep[] OutBreps = null;
		IOnBrep[] array3 = array;
		IOnBrep[] inBreps = array3;
		array3 = array2;
		if (!RhUtil.RhinoBooleanIntersection(inBreps, array3, CSApplicationHelper.Application.Tolerance, ref bResult, out OutBreps))
		{
			return UpdateError("Intersection failed");
		}
		if (!bResult)
		{
			return UpdateError("Intersection failed");
		}
		if (OutBreps.Length != 1)
		{
			return UpdateError("Intersection failed");
		}
		OnBrep onBrep3 = OutBreps[0];
		onBrep3.ShrinkSurfaces();
		OnBrep onBrep4 = onBrep3.Duplicate();
		onBrep4.Transform(xform);
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!onBrep4.GetTightBoundingBox(ref tight_bbox))
		{
			return UpdateError("BoundingBox Failed");
		}
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, tight_bbox.Min().y, tight_bbox.Center().z), new On3dPoint(0.0, 0.0, tight_bbox.Center().z));
		onLineCurve.Transform(onXform);
		double num2 = 0.0;
		int num3 = -1;
		for (int i = 0; i < onBrep3.m_F.Count(); i++)
		{
			if (!RhUtil.RhinoCurveFaceIntersect(onLineCurve, onBrep3.m_F[i], CSApplicationHelper.Application.Tolerance, out var _, out var points))
			{
				continue;
			}
			foreach (On3dPoint item in points)
			{
				double num4 = onLineCurve.PointAtStart().y - item.y;
				if (num3 == -1 || num4 > num2)
				{
					num2 = num4;
					num3 = i;
				}
			}
		}
		OnBrep brep = onBrep3.ExtractFace(num3);
		On3dPoint a = new On3dPoint(0.0, 100.0, 100.0);
		On3dPoint b = new On3dPoint(0.0, -100.0, 100.0);
		On3dPoint c = new On3dPoint(0.0, -100.0, -100.0);
		On3dPoint d = new On3dPoint(0.0, 100.0, -100.0);
		OnNurbsSurface onNurbsSurface2 = RhUtil.RhinoCreateSurfaceFromCorners(a, b, ref c, ref d);
		onNurbsSurface2.Rotate(rotation_angle, OnUtil.On_zaxis, OnUtil.On_origin);
		onNurbsSurface2.Transform(onXform);
		OnBrep onBrep5 = new OnBrep();
		if (!onBrep5.Create(onNurbsSurface2))
		{
			return UpdateError("Surface failed");
		}
		OnBrep[] pieces = null;
		if (!RhUtil.RhinoBrepSplit(brep, onBrep5, CSApplicationHelper.Application.Tolerance, out pieces))
		{
			return UpdateError("Brep split Failed");
		}
		if (pieces.Length != 2)
		{
			return UpdateError("Brep split Failed");
		}
		OnBrep onBrep6 = pieces[0];
		onBrep6.ShrinkSurfaces();
		CSDataArray cSDataArray = new CSDataArray();
		foreach (OnBrepEdge item2 in onBrep6.m_E)
		{
			cSDataArray.Append(new CSOnGeometryData(item2.NurbsCurve().Convert()));
		}
		oCutter.SetData(new CSOnGeometryData(onBrep2.Convert()));
		oIntersectCurve.SetData(cSDataArray);
		oIntersectSurface.SetData(new CSOnGeometryData(onBrep6.Convert()));
		return base.Update();
	}

	private static bool MakeCutterProfile(double arcRadius, double height, ref OnCurve cutterProfile)
	{
		On3dPoint p = new On3dPoint(arcRadius, 0.0, 0.0);
		On3dPoint q = new On3dPoint(0.0, 0.0, arcRadius);
		On3dPoint r = new On3dPoint(0.0 - arcRadius, 0.0, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0 - arcRadius, 0.0, 0.0);
		on3dPointArray.Append(0.0 - arcRadius - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - arcRadius - 1.0, 0.0, arcRadius + height);
		on3dPointArray.Append(arcRadius + 1.0, 0.0, arcRadius + height);
		on3dPointArray.Append(arcRadius + 1.0, 0.0, 0.0);
		on3dPointArray.Append(arcRadius, 0.0, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[2] { onArcCurve, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		cutterProfile = output[0];
		return true;
	}
}
