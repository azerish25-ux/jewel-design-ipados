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
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Expected O, but got Unknown
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_0392: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_04f0: Expected O, but got Unknown
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		CSData data = iHalo.Data;
		if (data == null)
		{
			return UpdateMissing("Halo Missing");
		}
		OnBrep val = data.GetBrep().Convert();
		if (val == null)
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
		double num = CSBlockUtil.CSUtilMath.ToRadians(90.0) * (double)value5;
		CSDataHelper.GetTransformNoArray(data, out var value6);
		if (!((Transform)(ref value6)).IsValid)
		{
			value6 = Transform.Identity;
		}
		On3dVector val2 = new On3dVector(OnUtil.On_zaxis);
		val2.Transform((IOnXform)(object)value6.Convert());
		val2.y = 0.0;
		OnXform val3 = new OnXform();
		val3.Rotation(OnUtil.On_zaxis, (IOn3dVector)(object)val2, OnUtil.On_origin);
		OnXform val4 = val3.Inverse();
		OnCurve cutterProfile = null;
		if (!MakeCutterProfile(value + value2, value3, ref cutterProfile))
		{
			return UpdateError("Make Cutter Failed");
		}
		((OnGeometry)cutterProfile).Transform((IOnXform)(object)val3);
		double num2 = value4 / 2.0;
		OnCurve val5 = cutterProfile.Duplicate();
		((OnGeometry)val5).Translate((IOn3dVector)new On3dVector(0.0, 0.0 - num2, 0.0));
		OnCurve val6 = cutterProfile.Duplicate();
		((OnGeometry)val6).Translate((IOn3dVector)new On3dVector(0.0, num2, 0.0));
		OnNurbsSurface val7 = new OnNurbsSurface();
		val7.CreateRuledSurface((IOnCurve)(object)val5, (IOnCurve)(object)val6);
		OnBrep val8 = CSUtilRhino.CapPlanarHoles(((OnGeometry)val7).BrepForm(), CSApplicationHelper.Application.Tolerance);
		if (val8 == null)
		{
			return UpdateError("Cap holes failed");
		}
		OnBrep[] array = (OnBrep[])(object)new OnBrep[1] { val };
		CSApplicationHelper.Application.InRunScriptBlock = true;
		MRhinoBrepObject val9 = RhUtil.RhinoApp().ActiveDoc().AddBrepObject((IOnBrep)(object)array[0]);
		if (val9 != null)
		{
			array[0] = OnBrep.Cast(((IOnObject)val9.Brep()).DuplicateOnObject());
			RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)val9));
		}
		CSApplicationHelper.Application.InRunScriptBlock = false;
		OnBrep[] array2 = (OnBrep[])(object)new OnBrep[1] { val8 };
		CSApplicationHelper.Application.InRunScriptBlock = true;
		MRhinoBrepObject val10 = RhUtil.RhinoApp().ActiveDoc().AddBrepObject((IOnBrep)(object)array2[0]);
		if (val10 != null)
		{
			array2[0] = OnBrep.Cast(((IOnObject)val10.Brep()).DuplicateOnObject());
			RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)val10));
		}
		CSApplicationHelper.Application.InRunScriptBlock = false;
		bool flag = false;
		OnBrep[] array3 = null;
		IOnBrep[] array4 = (IOnBrep[])(object)array;
		IOnBrep[] array5 = array4;
		array4 = (IOnBrep[])(object)array2;
		if (!RhUtil.RhinoBooleanIntersection(array5, array4, CSApplicationHelper.Application.Tolerance, ref flag, ref array3))
		{
			return UpdateError("Intersection failed");
		}
		if (!flag)
		{
			return UpdateError("Intersection failed");
		}
		if (array3.Length != 1)
		{
			return UpdateError("Intersection failed");
		}
		OnBrep val11 = array3[0];
		val11.ShrinkSurfaces();
		OnBrep obj = val11.Duplicate();
		((OnGeometry)obj).Transform((IOnXform)(object)val4);
		OnBoundingBox val12 = new OnBoundingBox();
		if (!((OnGeometry)obj).GetTightBoundingBox(ref val12))
		{
			return UpdateError("BoundingBox Failed");
		}
		OnLineCurve val13 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, val12.Min().y, val12.Center().z), (IOn3dPoint)new On3dPoint(0.0, 0.0, val12.Center().z));
		val13.Transform((IOnXform)(object)val3);
		double num3 = 0.0;
		int num4 = -1;
		OnCurve[] array6 = default(OnCurve[]);
		On3dPointArray val14 = default(On3dPointArray);
		for (int i = 0; i < val11.m_F.Count(); i++)
		{
			if (!RhUtil.RhinoCurveFaceIntersect((IOnCurve)(object)val13, (IOnBrepFace)(object)val11.m_F[i], CSApplicationHelper.Application.Tolerance, ref array6, ref val14))
			{
				continue;
			}
			foreach (On3dPoint item in (ArrayOn3dPoint)val14)
			{
				On3dPoint val15 = item;
				double num5 = ((OnCurve)val13).PointAtStart().y - val15.y;
				if (num4 == -1 || num5 > num3)
				{
					num3 = num5;
					num4 = i;
				}
			}
		}
		OnBrep val16 = val11.ExtractFace(num4);
		On3dPoint val17 = new On3dPoint(0.0, 100.0, 100.0);
		On3dPoint val18 = new On3dPoint(0.0, -100.0, 100.0);
		On3dPoint val19 = new On3dPoint(0.0, -100.0, -100.0);
		On3dPoint val20 = new On3dPoint(0.0, 100.0, -100.0);
		OnNurbsSurface val21 = RhUtil.RhinoCreateSurfaceFromCorners((IOn3dPoint)val17, (IOn3dPoint)(object)val18, ref val19, ref val20);
		((OnGeometry)val21).Rotate(num, OnUtil.On_zaxis, OnUtil.On_origin);
		val21.Transform((IOnXform)(object)val3);
		OnBrep val22 = new OnBrep();
		if (!val22.Create(val21))
		{
			return UpdateError("Surface failed");
		}
		OnBrep[] array7 = null;
		if (!RhUtil.RhinoBrepSplit((IOnBrep)(object)val16, (IOnBrep)(object)val22, CSApplicationHelper.Application.Tolerance, ref array7))
		{
			return UpdateError("Brep split Failed");
		}
		if (array7.Length != 2)
		{
			return UpdateError("Brep split Failed");
		}
		OnBrep val23 = array7[0];
		val23.ShrinkSurfaces();
		CSDataArray cSDataArray = new CSDataArray();
		foreach (OnBrepEdge item2 in val23.m_E)
		{
			OnBrepEdge val24 = item2;
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)val24).NurbsCurve().Convert()));
		}
		oCutter.SetData(new CSOnGeometryData((GeometryBase)(object)val8.Convert()));
		oIntersectCurve.SetData(cSDataArray);
		oIntersectSurface.SetData(new CSOnGeometryData((GeometryBase)(object)val23.Convert()));
		return base.Update();
	}

	private static bool MakeCutterProfile(double arcRadius, double height, ref OnCurve cutterProfile)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0052: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		On3dPoint val = new On3dPoint(arcRadius, 0.0, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, 0.0, arcRadius);
		On3dPoint val3 = new On3dPoint(0.0 - arcRadius, 0.0, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		On3dPointArray val5 = new On3dPointArray();
		((ArrayOn3dPoint)val5).Append(0.0 - arcRadius, 0.0, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - arcRadius - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - arcRadius - 1.0, 0.0, arcRadius + height);
		((ArrayOn3dPoint)val5).Append(arcRadius + 1.0, 0.0, arcRadius + height);
		((ArrayOn3dPoint)val5).Append(arcRadius + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val5).Append(arcRadius, 0.0, 0.0);
		OnPolylineCurve val6 = new OnPolylineCurve((IOn3dPointArray)val5);
		OnCurve[] obj = new OnCurve[2]
		{
			(OnCurve)val4,
			(OnCurve)val6
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		cutterProfile = array[0];
		return true;
	}
}
