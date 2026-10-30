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

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("GalleryRailSurfaceOutside_V1", "{9E3E1288-A189-459c-8D77-88690DD05BCD}")]
public sealed class CSGalleryRailSurfaceOutside1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iHeight { get; }

	public CSInputChannel iThickness { get; }

	public CSOutputChannel oBrep { get; }

	public CSGalleryRailSurfaceOutside1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		iThickness = new CSInputChannel(GetPropertyName(() => iThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0064: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve nurbsCurve = iOutsideProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			On3dPoint val = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint val2 = new On3dPoint(0.0, 0.0, 0.0);
			if (((OnCurve)new OnLineCurve((IOn3dPoint)val, (IOn3dPoint)(object)val2)).GetNurbForm(ref nurbsCurve) != 1)
			{
				return UpdateError("Create default profile failed");
			}
		}
		double value = 0.0;
		iAngle.GetDouble(ref value);
		double value2 = 0.0;
		iThickness.GetDouble(ref value2);
		CSDataArray flattenArray2 = iHeight.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		Transform transform = default(Transform);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
			if (!((Transform)(ref value3)).IsValid)
			{
				value3 = Transform.Identity;
			}
			((Transform)(ref value3)).TryGetInverse(ref transform);
			OnNurbsCurve val3 = arrayItem.GetNurbsCurve().Convert();
			if (val3 == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve obj = val3.Duplicate();
			obj.Transform((IOnXform)(object)transform.Convert());
			OnNurbsCurve profile = nurbsCurve.Duplicate();
			int index = 0;
			if (flattenArray2.GetArrayCount() > 1)
			{
				index = i;
			}
			CSData arrayItem2 = flattenArray2.GetArrayItem(index);
			double value4 = 0.0;
			arrayItem2.GetDouble(ref value4);
			OnBrep srf = null;
			if (SurfaceGalleryRailOutside(obj, profile, value, value4, value2, ref srf))
			{
				((OnGeometry)srf).Transform((IOnXform)(object)value3.Convert());
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)srf.Convert(), value3, applyTransform: false));
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

	private static bool SurfaceGalleryRailOutside(OnNurbsCurve rail, OnNurbsCurve profile, double angle, double height, double thickness, ref OnBrep srf)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_022b: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected O, but got Unknown
		if (rail == null || profile == null)
		{
			return false;
		}
		OnBoundingBox val = new OnBoundingBox();
		if (!((OnGeometry)profile).GetTightBoundingBox(ref val))
		{
			return false;
		}
		On3dVector val2 = new On3dVector((IOn3dPoint)new On3dPoint(val.Max().x, val.Max().y, 0.0));
		val2.Reverse();
		OnXform val3 = new OnXform();
		val3.Translation((IOn3dVector)(object)val2);
		OnXform val4 = new OnXform();
		val4.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		val3 = val4 * (IOnXform)(object)val3;
		val4.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		val3 = val4 * (IOnXform)(object)val3;
		profile.Transform((IOnXform)(object)val3);
		double x = val.Max().x;
		double num = thickness / x;
		double y = val.Max().y;
		double num2 = height / y;
		val3.Scale(1.0, num, num2);
		profile.Transform((IOnXform)(object)val3);
		val2 = new On3dVector((IOn3dPoint)(object)((OnCurve)rail).PointAtStart())
		{
			x = 0.0,
			z = 0.0
		};
		val3.Translation((IOn3dVector)(object)val2);
		profile.Transform((IOnXform)(object)val3);
		double num3 = Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angle));
		for (int i = 0; i < profile.CVCount(); i++)
		{
			On3dPoint val5 = new On3dPoint();
			profile.GetCV(i, ref val5);
			val5.y -= val5.z * num3;
			profile.SetCV(i, (IOn3dPoint)(object)val5);
		}
		val2 = new On3dVector((IOn3dPoint)(object)((OnCurve)rail).PointAtStart());
		val2.x = 0.0;
		val2.y = 0.0;
		val3.Translation((IOn3dVector)(object)val2);
		profile.Transform((IOnXform)(object)val3);
		OnNurbsSurface val6 = new OnNurbsSurface();
		OnBoundingBox val7 = new OnBoundingBox();
		if (!((OnGeometry)rail).GetTightBoundingBox(ref val7))
		{
			return false;
		}
		OnLineCurve val8 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, val7.Center().z), (IOn3dPoint)new On3dPoint(0.0, val7.Max().y * 1.1, val7.Center().z));
		ArrayOnX_EVENT val9 = new ArrayOnX_EVENT();
		((OnCurve)rail).IntersectCurve((IOnCurve)(object)val8, ref val9);
		if (val9.Count() < 1)
		{
			return false;
		}
		((OnCurve)rail).DuplicateCurve().ChangeClosedCurveSeam(val9[0][0]);
		val6 = CSUtilRailRevolve.RailRevolveRhino4((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)rail);
		if (val6 == null)
		{
			return false;
		}
		if (!((OnObject)val6).IsValid())
		{
			return false;
		}
		if (!((OnGeometry)val6).HasBrepForm())
		{
			return false;
		}
		srf = ((OnGeometry)val6).BrepForm();
		return true;
	}
}
