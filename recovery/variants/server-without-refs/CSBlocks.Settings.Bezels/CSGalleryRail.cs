using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Bezels;

[CSBlock("GalleryRail", "{37B6BEB7-183B-46E0-93C2-6BCBC70E5015}")]
public sealed class CSGalleryRail : CSBlock
{
	public CSOutputChannel OutBrep { get; }

	public CSInputChannel InOutsideProfile { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InThickness { get; }

	public CSInputChannel InGem { get; }

	public CSInputChannel InGirdleSpacing { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InRailRadius { get; }

	public CSInputChannel InRailOffset { get; }

	public CSInputChannel InPoint { get; }

	public CSGalleryRail(string identifier)
		: base(identifier)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		InOutsideProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Origin);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
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
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00c5: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		InitializeCancellationTokenSource();
		List<CSGemData> gemDatas = InGem.GemList;
		if (gemDatas.Count <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		double girdleSpacing = InGirdleSpacing.Double;
		double placement = InPlacement.Double;
		OnNurbsCurve profile = InOutsideProfile.NurbsCurve.Convert();
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
		double angle = InAngle.Double;
		double thickness = InThickness.Double;
		double railRadius = InRailRadius.Double;
		double railOffset = InRailOffset.Double;
		bool bRail = Math.Abs(railRadius) > 0.001;
		double height = InHeight.Double;
		On3dPoint point = InPoint.Point3d.Convert();
		bool bPoint = point != (IOn3dPoint)(object)Point3d.Origin.Convert();
		CSDataArray cSDataArray = new CSDataArray();
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		int count = gemDatas.Count;
		CSData[] arrGalleryRails = new CSData[count];
		string error;
		Parallel.For(0, count, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			if (!loopState.ShouldExitCurrentIteration && !parallelOptions.CancellationToken.IsCancellationRequested && !loopState.IsStopped && !loopState.IsExceptional)
			{
				CSGemData cSGemData = gemDatas[i];
				CSOnGeometryData galleryRail;
				if (cSGemData == null)
				{
					UpdateError("Invalid gem");
					loopState.Stop();
				}
				else if (!MakeGalleryRail(cSGemData, placement, girdleSpacing, profile, bRail, height, railRadius, railOffset, bPoint, point, angle, thickness, out galleryRail, out error))
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
		for (int num = 0; num < count; num++)
		{
			if (arrGalleryRails[num] != null)
			{
				cSDataArray.Append(arrGalleryRails[num]);
			}
		}
		OutBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private bool UpdateSync()
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0096: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		double girdleSpacing = InGirdleSpacing.Double;
		double placement = InPlacement.Double;
		OnNurbsCurve val = InOutsideProfile.NurbsCurve.Convert();
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
		double angle = InAngle.Double;
		double thickness = InThickness.Double;
		double num = InRailRadius.Double;
		double railOffset = InRailOffset.Double;
		bool bRail = Math.Abs(num) > 0.001;
		double height = InHeight.Double;
		On3dPoint val5 = InPoint.Point3d.Convert();
		bool bPoint = val5 != (IOn3dPoint)(object)Point3d.Origin.Convert();
		CSDataArray cSDataArray = new CSDataArray();
		int count = gemList.Count;
		CSData[] array = new CSData[count];
		for (int i = 0; i < count; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateError("Invalid gem");
			}
			if (!MakeGalleryRail(cSGemData, placement, girdleSpacing, val, bRail, height, num, railOffset, bPoint, val5, angle, thickness, out var galleryRail, out var msg))
			{
				return UpdateError(msg);
			}
			array[i] = galleryRail;
		}
		if (base.Error)
		{
			return true;
		}
		for (int j = 0; j < count; j++)
		{
			if (array[j] != null)
			{
				cSDataArray.Append(array[j]);
			}
		}
		OutBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private static bool MakeGalleryRail(CSGemData gemData, double placement, double girdleSpacing, OnNurbsCurve profile, bool bRail, double height, double railRadius, double railOffset, bool bPoint, On3dPoint point, double angle, double thickness, out CSOnGeometryData galleryRail, out string msg)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		msg = string.Empty;
		galleryRail = null;
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
		if (!GemOffsetCurve.GemOffset(gemData, girdleSpacing, out OnCurve curve, doTransformUp: false, out msg, 0.0))
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
		if (!SurfaceGalleryRailOutside((IOnCurve)(object)curve, profile2, angle, height, thickness, ref srf))
		{
			return false;
		}
		((OnGeometry)srf).Transform((IOnXform)(object)xUp);
		galleryRail = new CSOnGeometryData((GeometryBase)(object)srf.Convert(), xUp.Convert(), applyTransform: false);
		return true;
	}

	private static bool SurfaceGalleryRailOutside(IOnCurve rail, OnNurbsCurve profile, double angle, double height, double thickness, ref OnBrep srf)
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
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0224: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
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
		val2 = new On3dVector((IOn3dPoint)(object)rail.PointAtStart())
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
		val2 = new On3dVector((IOn3dPoint)(object)rail.PointAtStart())
		{
			x = 0.0,
			y = 0.0
		};
		val3.Translation((IOn3dVector)(object)val2);
		profile.Transform((IOnXform)(object)val3);
		OnBoundingBox val6 = new OnBoundingBox();
		if (!((IOnGeometry)rail).GetTightBoundingBox(ref val6))
		{
			return false;
		}
		OnLineCurve val7 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, val6.Center().z), (IOn3dPoint)new On3dPoint(0.0, val6.Max().y * 1.1, val6.Center().z));
		ArrayOnX_EVENT val8 = new ArrayOnX_EVENT();
		rail.IntersectCurve((IOnCurve)(object)val7, ref val8);
		if (val8.Count() < 1)
		{
			return false;
		}
		OnCurve val9 = rail.DuplicateCurve();
		val9.ChangeClosedCurveSeam(val8[0][0]);
		OnBoundingBox val10 = new OnBoundingBox();
		if (!((OnGeometry)profile).GetTightBoundingBox(ref val10))
		{
			return false;
		}
		double num4 = 0.0;
		((OnCurve)profile).GetClosestPoint((IOn3dPoint)(object)val10.Max(), ref num4);
		((OnCurve)profile).ChangeClosedCurveSeam(num4);
		OnNurbsCurve rail2 = new OnNurbsCurve();
		val9.GetNurbForm(ref rail2);
		OnNurbsSurface val11 = CSUtilRailRevolve.RailRevolveGen((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)rail2, 0);
		if (val11 == null)
		{
			return false;
		}
		if (!((OnObject)val11).IsValid())
		{
			return false;
		}
		if (!((OnGeometry)val11).HasBrepForm())
		{
			return false;
		}
		srf = ((OnGeometry)val11).BrepForm();
		return true;
	}
}
