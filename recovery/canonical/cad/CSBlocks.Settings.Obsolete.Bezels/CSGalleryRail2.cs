using System;
using System.Collections.Generic;
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
[CSBlock("GalleryRail_V2", "{AA4A60B1-581C-4B3C-ACCD-978C0C47A8E5}")]
public sealed class CSGalleryRail2 : CSBlock
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

	public CSGalleryRail2(string identifier)
		: base(identifier)
	{
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
			On3dPoint arg = new On3dPoint(0.0, 10.0, 0.0);
			On3dPoint arg2 = new On3dPoint(0.0, 0.0, 0.0);
			OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
			profile = new OnNurbsCurve();
			if (onLineCurve.GetNurbForm(ref profile) != 1)
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
		bool bPoint = point != Point3d.Origin.Convert();
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
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		double girdleSpacing = InGirdleSpacing.Double;
		double placement = InPlacement.Double;
		OnNurbsCurve nurbs_curve = InOutsideProfile.NurbsCurve.Convert();
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
		double angle = InAngle.Double;
		double thickness = InThickness.Double;
		double num = InRailRadius.Double;
		double railOffset = InRailOffset.Double;
		bool bRail = Math.Abs(num) > 0.001;
		double height = InHeight.Double;
		On3dPoint on3dPoint = InPoint.Point3d.Convert();
		bool bPoint = on3dPoint != Point3d.Origin.Convert();
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
			if (!MakeGalleryRail(cSGemData, placement, girdleSpacing, nurbs_curve, bRail, height, num, railOffset, bPoint, on3dPoint, angle, thickness, out var galleryRail, out var msg))
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
		if (!GemOffsetCurve.GemOffset(gemData, girdleSpacing, out OnCurve curve, false, out msg, 0.0))
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

	private static bool SurfaceGalleryRailOutside(IOnCurve rail, OnNurbsCurve profile, double angle, double height, double thickness, ref OnBrep srf)
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
		on3dVector = new On3dVector(rail.PointAtStart())
		{
			x = 0.0,
			z = 0.0
		};
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
		on3dVector = new On3dVector(rail.PointAtStart())
		{
			x = 0.0,
			y = 0.0
		};
		onXform.Translation(on3dVector);
		profile.Transform(onXform);
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
		OnNurbsSurface onNurbsSurface = CSUtilRailRevolve.RailRevolveGen(profile, nurbs_curve, 0);
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
