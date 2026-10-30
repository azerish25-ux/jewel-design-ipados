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
			On3dPoint arg = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint arg2 = new On3dPoint(0.0, 0.0, 0.0);
			OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
			profile = new OnNurbsCurve();
			if (onLineCurve.GetNurbForm(ref profile) != 1)
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
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		double value = 0.0;
		iGirdleSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iPlacement.GetDouble(ref value2);
		OnNurbsCurve nurbs_curve = iOutsideProfile.GetNurbsCurve();
		if (nurbs_curve == null)
		{
			On3dPoint arg = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint arg2 = new On3dPoint(0.0, 0.0, 0.0);
			OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
			nurbs_curve = new OnNurbsCurve();
			if (onLineCurve.GetNurbForm(ref nurbs_curve) != 1)
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
			if (!MakeGalleryRail(gemData, value2, value, nurbs_curve, flag, value7, value5, value6, point, onPoint, value3, value4, out var galleryRail, out var msg))
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
		msg = string.Empty;
		galleryRail = null;
		CSDataHelper.SetSettingType(gemData, GemSettingType.BZ);
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(gemData.Xform.Convert(), out var xUp, out var xDown))
		{
			xUp.Identity();
			xDown = xUp.Inverse();
		}
		OnNurbsCurve onNurbsCurve = gemData.GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return false;
		}
		onNurbsCurve.Duplicate().Transform(xDown);
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0, placement);
		if (!GemOffsetCurveV2.GemOffset(gemData, girdleSpacing, out OnCurve curve, false, out msg, 0.0))
		{
			return false;
		}
		if (curve == null)
		{
			msg = "Under Bezel Top Outer Offset Curve Unable to Create Output";
			return false;
		}
		curve.Transform(onXform);
		OnNurbsCurve profile2 = profile.Duplicate();
		OnBrep srf = null;
		if (bRail)
		{
			On3dPoint on3dPoint = new On3dPoint(0.0, 0.0, 0.0);
			on3dPoint.Transform(xUp);
			on3dPoint.y = 0.0;
			height = on3dPoint.DistanceTo(OnUtil.On_origin) - railRadius - railOffset + placement;
		}
		else if (bPoint)
		{
			On3dPoint on3dPoint2 = new On3dPoint(0.0, 0.0, 0.0);
			on3dPoint2.Transform(xUp);
			on3dPoint2.y = 0.0;
			height = point.DistanceTo(on3dPoint2) + placement;
		}
		if (!SurfaceGalleryRailOutside(curve, profile2, angle, height, thickness, ref srf))
		{
			return false;
		}
		srf.Transform(xUp);
		galleryRail = new CSOnGeometryData(srf.Convert(), xUp.Convert(), applyTransform: false);
		return true;
	}

	private static bool SurfaceGalleryRailOutside(OnCurve rail, OnNurbsCurve profile, double angle, double height, double thickness, ref OnBrep srf)
	{
		if (rail == null || profile == null)
		{
			return false;
		}
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!profile.GetTightBoundingBox(ref tight_bbox))
		{
			return false;
		}
		On3dVector on3dVector = new On3dVector(new On3dPoint(tight_bbox.Max().x, tight_bbox.Max().y, 0.0));
		on3dVector.Reverse();
		OnXform onXform = new OnXform();
		onXform.Translation(on3dVector);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		onXform = onXform2 * onXform;
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		onXform = onXform2 * onXform;
		profile.Transform(onXform);
		double x = tight_bbox.Max().x;
		double y_scale_factor = thickness / x;
		double y = tight_bbox.Max().y;
		double z_scale_factor = height / y;
		onXform.Scale(1.0, y_scale_factor, z_scale_factor);
		profile.Transform(onXform);
		on3dVector = new On3dVector(rail.PointAtStart());
		on3dVector.x = 0.0;
		on3dVector.z = 0.0;
		onXform.Translation(on3dVector);
		profile.Transform(onXform);
		double num = Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angle));
		for (int i = 0; i < profile.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			profile.GetCV(i, ref vertex_value);
			vertex_value.y -= vertex_value.z * num;
			profile.SetCV(i, vertex_value);
		}
		on3dVector = new On3dVector(rail.PointAtStart());
		on3dVector.x = 0.0;
		on3dVector.y = 0.0;
		onXform.Translation(on3dVector);
		profile.Transform(onXform);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		OnBoundingBox tight_bbox2 = new OnBoundingBox();
		if (!rail.GetTightBoundingBox(ref tight_bbox2))
		{
			return false;
		}
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, 0.0, tight_bbox2.Center().z), new On3dPoint(0.0, tight_bbox2.Max().y * 1.1, tight_bbox2.Center().z));
		ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
		rail.IntersectCurve(curveB, ref x2);
		if (x2.Count() < 1)
		{
			return false;
		}
		OnCurve onCurve = rail.DuplicateCurve();
		onCurve.ChangeClosedCurveSeam(x2[0].get_m_a(0));
		OnBoundingBox tight_bbox3 = new OnBoundingBox();
		if (!profile.GetTightBoundingBox(ref tight_bbox3))
		{
			return false;
		}
		double t = 0.0;
		profile.GetClosestPoint(tight_bbox3.Max(), ref t);
		profile.ChangeClosedCurveSeam(t);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onCurve.GetNurbForm(ref nurbs_curve);
		onNurbsSurface = CSUtilRailRevolve.RailRevolveGen(profile, nurbs_curve, 0);
		if (onNurbsSurface == null)
		{
			return false;
		}
		if (!onNurbsSurface.IsValid())
		{
			return false;
		}
		if (!onNurbsSurface.HasBrepForm())
		{
			return false;
		}
		srf = onNurbsSurface.BrepForm();
		return true;
	}
}
