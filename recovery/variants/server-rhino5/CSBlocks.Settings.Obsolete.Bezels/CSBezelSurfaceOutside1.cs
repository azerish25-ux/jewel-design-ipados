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
[CSBlock("BezelSurfaceOutside_V1", "{79A34A3E-D655-478a-8740-9DDCD3BCC9BA}")]
public sealed class CSBezelSurfaceOutside1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iHeight { get; }

	public CSOutputChannel oBrep { get; }

	public CSOutputChannel oCurve { get; }

	public CSBezelSurfaceOutside1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Array);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0064: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve val = iOutsideProfile.GetNurbsCurve();
		if (val == null)
		{
			On3dPoint val2 = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint val3 = new On3dPoint(0.0, 0.0, 0.0);
			OnLineCurve val4 = new OnLineCurve((IOn3dPoint)val2, (IOn3dPoint)(object)val3);
			val = new OnNurbsCurve();
			if (((OnCurve)val4).GetNurbForm(ref val) != 1)
			{
				return UpdateError("OutsideProfile invalid");
			}
		}
		double value = 0.0;
		iAngle.GetDouble(ref value);
		CSDataArray flattenArray2 = iHeight.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
			if (!value2.IsValid)
			{
				value2 = Transform.Identity;
			}
			value2.TryGetInverse(out var inverseTransform);
			OnNurbsCurve val5 = arrayItem.GetNurbsCurve().Convert();
			if (val5 == null)
			{
				cSDataArray.Append(new CSData());
				cSDataArray2.Append(new CSData());
				continue;
			}
			OnNurbsCurve obj = val5.Duplicate();
			obj.Transform((IOnXform)(object)inverseTransform.Convert());
			OnNurbsCurve profile = val.Duplicate();
			int index = 0;
			if (flattenArray2.GetArrayCount() > 1)
			{
				index = i;
			}
			CSData arrayItem2 = flattenArray2.GetArrayItem(index);
			double value3 = 0.0;
			arrayItem2.GetDouble(ref value3);
			OnBrep srf = null;
			OnNurbsCurve crv = null;
			if (SurfaceBezelOutside(obj, profile, value, value3, ref srf, ref crv))
			{
				cSDataArray.Append(new CSOnGeometryData(srf.Convert(), value2));
				cSDataArray2.Append(new CSOnGeometryData(crv.Convert(), value2));
			}
			else
			{
				cSDataArray.Append(new CSData());
				cSDataArray2.Append(new CSData());
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
		if (cSDataArray2.GetArrayCount() == 1)
		{
			oCurve.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oCurve.SetData(cSDataArray2);
		}
		return base.Update();
	}

	private static bool SurfaceBezelOutside(OnNurbsCurve rail, OnNurbsCurve profile, double angle, double height, ref OnBrep srf, ref OnNurbsCurve crv)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		if (rail == null || profile == null)
		{
			return false;
		}
		On3dPoint val = ((OnCurve)profile).PointAtStart();
		On3dVector val2 = new On3dVector((IOn3dPoint)(object)val);
		val2.Reverse();
		OnXform val3 = new OnXform();
		val3.Translation((IOn3dVector)(object)val2);
		OnXform val4 = new OnXform();
		val4.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform val5 = new OnXform();
		val5.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		profile.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4 * (IOnXform)(object)val3));
		On3dPoint val6 = ((OnCurve)profile).PointAtEnd();
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(val6.y, 0.0 - val6.z);
		OnXform val7 = new OnXform();
		val7.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, OnUtil.On_xaxis, OnUtil.On_origin);
		profile.Transform((IOnXform)(object)val7);
		double num2 = 0.0 - ((OnCurve)profile).PointAtEnd().z;
		double num3 = height / num2;
		OnXform val8 = new OnXform();
		val8.Scale(num3, num3, num3);
		profile.Transform((IOnXform)(object)val8);
		On3dPoint val9 = ((OnCurve)rail).PointAtStart();
		val = ((OnCurve)profile).PointAtStart();
		val3.Translation((IOn3dVector)(object)(val9 - (IOn3dPoint)(object)val));
		profile.Transform((IOnXform)(object)val3);
		OnNurbsSurface val10 = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)rail, 0);
		if (val10 == null)
		{
			return false;
		}
		if (!((OnObject)val10).IsValid())
		{
			return false;
		}
		if (!((OnGeometry)val10).HasBrepForm())
		{
			return false;
		}
		srf = ((OnGeometry)val10).BrepForm();
		On3dPoint val11 = ((OnCurve)profile).PointAtStart();
		val11.z = 0.0;
		On3dPoint obj = ((OnCurve)profile).PointAtEnd();
		obj.z = 0.0;
		double num4 = val11.DistanceTo(OnUtil.On_origin);
		num3 = obj.DistanceTo(OnUtil.On_origin) / num4;
		val8.Scale(num3, num3, 1.0);
		rail.Transform((IOnXform)(object)val8);
		val3.Translation(0.0, 0.0, 0.0 - height);
		rail.Transform((IOnXform)(object)val3);
		crv = rail;
		return true;
	}
}
