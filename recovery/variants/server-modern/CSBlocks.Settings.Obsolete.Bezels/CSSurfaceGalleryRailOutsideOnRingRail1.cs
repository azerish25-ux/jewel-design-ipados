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

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("SurfaceGalleryRailOutsideOnRingRail_V1", "{B865D441-D557-4718-846A-CA764526AEDD}")]
public sealed class CSSurfaceGalleryRailOutsideOnRingRail1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iThickness { get; }

	public CSInputChannel iTube { get; }

	public CSOutputChannel oBrep { get; }

	public CSSurfaceGalleryRailOutsideOnRingRail1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iThickness = new CSInputChannel(GetPropertyName(() => iThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTube = new CSInputChannel(GetPropertyName(() => iTube), this, ChannelDataType.Surface, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve nurbsCurve = iOutsideProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing outside profile curve");
		}
		double value = 0.0;
		iPlacement.GetDouble(ref value);
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		double value3 = 0.0;
		iThickness.GetDouble(ref value3);
		OnNurbsSurface nurbsSurface = iTube.GetNurbsSurface();
		CSData outputSurfaceData = null;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(value4, out var xUp, out var xDown))
			{
				xUp = Transform.Identity;
				((Transform)(ref xUp)).TryGetInverse(ref xDown);
			}
			OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
			if (val != null)
			{
				OnNurbsCurve val2 = val.Duplicate();
				val2.Transform((IOnXform)(object)xDown.Convert());
				OnNurbsCurve val3 = nurbsCurve.Duplicate();
				if (val3 == null)
				{
					return UpdateError("Duplicate failed");
				}
				if (!GalleryRailOutside(xUp.Convert(), val2, val3, value, value2, value3, nurbsSurface, ref outputSurfaceData))
				{
					return UpdateError("GalleryRailOutside failed");
				}
				cSDataArray.Append(outputSurfaceData);
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

	private static bool GalleryRailOutside(OnXform xUp, OnNurbsCurve curveNC, OnNurbsCurve profileNC, double placement, double angle, double thickness, OnNurbsSurface tube, ref CSData outputSurfaceData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		new On3dPoint();
		OnBoundingBox val = new OnBoundingBox();
		OnXform val2 = new OnXform();
		OnXform val3 = new OnXform();
		if (!((OnGeometry)profileNC).GetTightBoundingBox(ref val))
		{
			return false;
		}
		On3dVector val4 = new On3dVector((IOn3dPoint)new On3dPoint(val.Max().x, val.Max().y, 0.0));
		val4.Reverse();
		val2.Translation((IOn3dVector)(object)val4);
		val3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		val2 = val3 * (IOnXform)(object)val2;
		val3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		val2 = val3 * (IOnXform)(object)val2;
		profileNC.Transform((IOnXform)(object)val2);
		OnLineCurve val5 = OnLineCurve.op_Implicit((IOnLine)new OnLine(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, -50.0)));
		val5.Transform((IOnXform)(object)xUp);
		OnCurve[] array = null;
		On3dPointArray val6 = null;
		if (!RhUtil.RhinoCurveBrepIntersect((IOnCurve)(object)val5, (IOnBrep)(object)((OnGeometry)tube).BrepForm(), CSApplicationHelper.Application.Tolerance, ref array, ref val6))
		{
			return false;
		}
		if (array.Length != 0)
		{
			OnCurve[] array2 = array;
			foreach (OnCurve val7 in array2)
			{
				((ArrayOn3dPoint)val6).Append((IOn3dPoint)(object)val7.PointAtStart());
				((ArrayOn3dPoint)val6).Append((IOn3dPoint)(object)val7.PointAtEnd());
			}
		}
		double num = 5.0;
		for (int j = 0; j < ((ArrayOn3dPoint)val6).Count(); j++)
		{
			double num2 = ((OnCurve)val5).PointAtStart().DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val6)[j]);
			if (j == 0 || num2 < num)
			{
				num = num2;
			}
		}
		num += placement;
		double x = val.Max().x;
		double num3 = thickness / x;
		double y = val.Max().y;
		double num4 = num / y;
		val2.Scale(1.0, num3, num4);
		profileNC.Transform((IOnXform)(object)val2);
		val4 = new On3dVector((IOn3dPoint)(object)((OnCurve)curveNC).PointAtStart())
		{
			x = 0.0,
			z = 0.0
		};
		val2.Translation((IOn3dVector)(object)val4);
		profileNC.Transform((IOnXform)(object)val2);
		double num5 = Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angle));
		for (int k = 0; k < profileNC.CVCount(); k++)
		{
			On3dPoint val8 = new On3dPoint();
			profileNC.GetCV(k, ref val8);
			val8.y -= val8.z * num5;
			profileNC.SetCV(k, (IOn3dPoint)(object)val8);
		}
		val4 = new On3dVector((IOn3dPoint)(object)((OnCurve)curveNC).PointAtStart())
		{
			x = 0.0,
			y = 0.0
		};
		val2.Translation((IOn3dVector)(object)val4);
		profileNC.Transform((IOnXform)(object)val2);
		OnNurbsSurface val9 = new OnNurbsSurface();
		RhUtil.RhinoRailRevolve((IOnCurve)(object)((OnCurve)profileNC).DuplicateCurve(), (IOnCurve)(object)((OnCurve)curveNC).DuplicateCurve(), (IOnLine)new OnLine(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, 1.0)), false, val9);
		OnBrep val10 = ((OnGeometry)val9).BrepForm();
		((OnGeometry)val10).Transform((IOnXform)(object)xUp);
		outputSurfaceData = new CSOnGeometryData((GeometryBase)(object)val10.Convert(), xUp.Convert(), applyTransform: false);
		return true;
	}
}
