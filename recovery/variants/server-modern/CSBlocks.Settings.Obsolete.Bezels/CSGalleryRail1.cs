using System;
using System.Threading.Tasks;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("GalleryRail_V1", "{8B9CE5B1-4F5E-4208-9915-259F354069B9}")]
public sealed class CSGalleryRail1 : CSBlock
{
	public CSOutputChannel oBrep { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iHeight { get; }

	public CSInputChannel iThickness { get; }

	public CSInputChannel iGem { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iRailRadius { get; }

	public CSInputChannel iRailOffset { get; }

	public CSInputChannel iPoint { get; }

	public CSGalleryRail1(string identifier)
		: base(identifier)
	{
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeight = new CSInputChannel(GetPropertyName(() => iHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iThickness = new CSInputChannel(GetPropertyName(() => iThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailRadius = new CSInputChannel(GetPropertyName(() => iRailRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailOffset = new CSInputChannel(GetPropertyName(() => iRailOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint = new CSInputChannel(GetPropertyName(() => iPoint), this, ChannelDataType.Point, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		if (!CSApplicationHelper.Application.IsBlockParallelProcessingEnabled)
		{
			return UpdateSync();
		}
		return UpdateConcurrent();
	}

	private bool UpdateConcurrent()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00e0: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		InitializeCancellationTokenSource();
		CSDataArray gemDatas = iGem.GetFlattenArray();
		if (gemDatas.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		double girdleSpacing = 0.0;
		iGirdleSpacing.GetDouble(ref girdleSpacing);
		double placement = 0.0;
		iPlacement.GetDouble(ref placement);
		OnNurbsCurve profile = iOutsideProfile.GetNurbsCurve();
		if (profile == null)
		{
			On3dPoint val = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint val2 = new On3dPoint(0.0, 0.0, 0.0);
			OnLineCurve val3 = new OnLineCurve((IOn3dPoint)val, (IOn3dPoint)(object)val2);
			profile = new OnNurbsCurve();
			if (((OnCurve)val3).GetNurbForm(ref profile) != 1)
			{
				return UpdateError("Unable to create profile");
			}
		}
		double angle = 0.0;
		iAngle.GetDouble(ref angle);
		double thickness = 0.0;
		iThickness.GetDouble(ref thickness);
		double railRadius = 0.0;
		double railOffset = 0.0;
		bool bRail = iRailRadius.GetDouble(ref railRadius);
		if (bRail)
		{
			iRailOffset.GetDouble(ref railOffset);
		}
		double height = 0.0;
		iHeight.GetDouble(ref height);
		On3dPoint point = new On3dPoint(OnUtil.On_origin);
		bool bPoint = iPoint.GetPoint(ref point);
		CSDataArray cSDataArray = new CSDataArray();
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		int arrayCount = gemDatas.GetArrayCount();
		CSData[] arrGalleryRails = new CSData[arrayCount];
		string error;
		Parallel.For(0, arrayCount, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			if (!loopState.ShouldExitCurrentIteration && !parallelOptions.CancellationToken.IsCancellationRequested && !loopState.IsStopped && !loopState.IsExceptional)
			{
				CSOnGeometryData galleryRail;
				if (!(gemDatas.GetArrayItem(i) is CSGemData gemData))
				{
					UpdateError("Invalid gem");
					loopState.Stop();
				}
				else if (!MakeGalleryRail(gemData, placement, girdleSpacing, profile, bRail, height, railRadius, railOffset, bPoint, point, angle, thickness, out galleryRail, out error))
				{
					UpdateError(error);
					loopState.Stop();
				}
				else
				{
					arrGalleryRails[i] = galleryRail;
					CS.CSUtil.CheckAndTerminateOrphanedParallelTasks();
				}
			}
		});
		if (base.Error)
		{
			return true;
		}
		for (int num = 0; num < arrayCount; num++)
		{
			if (arrGalleryRails[num] != null)
			{
				cSDataArray.Append(arrGalleryRails[num]);
			}
		}
		oBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private bool UpdateSync()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00a9: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		double value = 0.0;
		iGirdleSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iPlacement.GetDouble(ref value2);
		OnNurbsCurve val = iOutsideProfile.GetNurbsCurve();
		if (val == null)
		{
			On3dPoint val2 = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint val3 = new On3dPoint(0.0, 0.0, 0.0);
			OnLineCurve val4 = new OnLineCurve((IOn3dPoint)val2, (IOn3dPoint)(object)val3);
			val = new OnNurbsCurve();
			if (((OnCurve)val4).GetNurbForm(ref val) != 1)
			{
				return UpdateError("Unable to create profile");
			}
		}
		double value3 = 0.0;
		iAngle.GetDouble(ref value3);
		double value4 = 0.0;
		iThickness.GetDouble(ref value4);
		double value5 = 0.0;
		double value6 = 0.0;
		bool flag = iRailRadius.GetDouble(ref value5);
		if (flag)
		{
			iRailOffset.GetDouble(ref value6);
		}
		double value7 = 0.0;
		iHeight.GetDouble(ref value7);
		On3dPoint onPoint = new On3dPoint(OnUtil.On_origin);
		bool point = iPoint.GetPoint(ref onPoint);
		CSDataArray cSDataArray = new CSDataArray();
		int arrayCount = flattenArray.GetArrayCount();
		CSData[] array = new CSData[arrayCount];
		for (int i = 0; i < arrayCount; i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData gemData))
			{
				return UpdateError("Invalid gem");
			}
			if (!MakeGalleryRail(gemData, value2, value, val, flag, value7, value5, value6, point, onPoint, value3, value4, out var galleryRail, out var msg))
			{
				return UpdateError(msg);
			}
			array[i] = galleryRail;
		}
		if (base.Error)
		{
			return true;
		}
		for (int j = 0; j < arrayCount; j++)
		{
			if (array[j] != null)
			{
				cSDataArray.Append(array[j]);
			}
		}
		oBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private static bool MakeGalleryRail(CSGemData gemData, double placement, double girdleSpacing, OnNurbsCurve profile, bool bRail, double height, double railRadius, double railOffset, bool bPoint, On3dPoint point, double angle, double thickness, out CSOnGeometryData galleryRail, out string msg)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		msg = string.Empty;
		galleryRail = null;
		CSDataHelper.SetSettingType(gemData, GemSettingType.BZ);
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)gemData.Xform.Convert(), out var xUp, out var xDown))
		{
			xUp.Identity();
			xDown = xUp.Inverse();
		}
		OnNurbsCurve val = gemData.GetNurbsCurve().Convert();
		if (val == null)
		{
			return false;
		}
		val.Duplicate().Transform((IOnXform)(object)xDown);
		OnXform val2 = new OnXform();
		val2.Translation(0.0, 0.0, placement);
		if (!GemOffsetCurveV2.GemOffset(gemData, girdleSpacing, out OnCurve curve, false, out msg, 0.0))
		{
			return false;
		}
		if (curve == null)
		{
			msg = "Under Bezel Top Outer Offset Curve Unable to Create Output";
			return false;
		}
		((OnGeometry)curve).Transform((IOnXform)(object)val2);
		OnNurbsCurve profile2 = profile.Duplicate();
		OnBrep srf = null;
		if (bRail)
		{
			On3dPoint val3 = new On3dPoint(0.0, 0.0, 0.0);
			val3.Transform((IOnXform)(object)xUp);
			val3.y = 0.0;
			height = val3.DistanceTo(OnUtil.On_origin) - railRadius - railOffset + placement;
		}
		else if (bPoint)
		{
			On3dPoint val4 = new On3dPoint(0.0, 0.0, 0.0);
			val4.Transform((IOnXform)(object)xUp);
			val4.y = 0.0;
			height = point.DistanceTo((IOn3dPoint)(object)val4) + placement;
		}
		if (!SurfaceGalleryRailOutside(curve, profile2, angle, height, thickness, ref srf))
		{
			return false;
		}
		((OnGeometry)srf).Transform((IOnXform)(object)xUp);
		galleryRail = new CSOnGeometryData((GeometryBase)(object)srf.Convert(), xUp.Convert(), applyTransform: false);
		return true;
	}

	private static bool SurfaceGalleryRailOutside(OnCurve rail, OnNurbsCurve profile, double angle, double height, double thickness, ref OnBrep srf)
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
		//IL_00fb: Expected O, but got Unknown
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
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
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
		val2 = new On3dVector((IOn3dPoint)(object)rail.PointAtStart());
		val2.x = 0.0;
		val2.z = 0.0;
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
		val2 = new On3dVector((IOn3dPoint)(object)rail.PointAtStart());
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
		rail.IntersectCurve((IOnCurve)(object)val8, ref val9);
		if (val9.Count() < 1)
		{
			return false;
		}
		OnCurve val10 = rail.DuplicateCurve();
		val10.ChangeClosedCurveSeam(val9[0][0]);
		OnBoundingBox val11 = new OnBoundingBox();
		if (!((OnGeometry)profile).GetTightBoundingBox(ref val11))
		{
			return false;
		}
		double num4 = 0.0;
		((OnCurve)profile).GetClosestPoint((IOn3dPoint)(object)val11.Max(), ref num4);
		((OnCurve)profile).ChangeClosedCurveSeam(num4);
		OnNurbsCurve rail2 = new OnNurbsCurve();
		val10.GetNurbForm(ref rail2);
		val6 = CSUtilRailRevolve.RailRevolveGen((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)rail2, 0);
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
