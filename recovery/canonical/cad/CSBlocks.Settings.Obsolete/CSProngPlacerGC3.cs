using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerGC_V3", "{5081B43E-6AE3-4107-9D38-8D37B9837204}")]
public sealed class CSProngPlacerGC3 : CSBlock
{
	private CSData _prongCachedData;

	private double _prongCachedDiameter = -1.0;

	public CSInputChannel iSortedGem { get; }

	public CSInputChannel iGemNudge { get; }

	public CSInputChannel iEdgeNudge { get; }

	public CSInputChannel iProngSizeMin { get; }

	public CSInputChannel iProngSizeMax { get; }

	public CSInputChannel iHeightOffset { get; }

	public CSInputChannel iPlacementOffset { get; }

	public CSInputChannel iTaper { get; }

	public CSInputChannel iFillet { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iMeshedProngs { get; }

	public CSOutputChannel oSurface { get; }

	public CSOutputChannel oAvgProngDiameter { get; }

	public CSProngPlacerGC3(string identifier)
		: base(identifier)
	{
		iSortedGem = new CSInputChannel(GetPropertyName(() => iSortedGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemNudge = new CSInputChannel(GetPropertyName(() => iGemNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudge = new CSInputChannel(GetPropertyName(() => iEdgeNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngSizeMin = new CSInputChannel(GetPropertyName(() => iProngSizeMin), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngSizeMax = new CSInputChannel(GetPropertyName(() => iProngSizeMax), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeightOffset = new CSInputChannel(GetPropertyName(() => iHeightOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacementOffset = new CSInputChannel(GetPropertyName(() => iPlacementOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTaper = new CSInputChannel(GetPropertyName(() => iTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFillet = new CSInputChannel(GetPropertyName(() => iFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iMeshedProngs = new CSInputChannel(GetPropertyName(() => iMeshedProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Array);
		oAvgProngDiameter = new CSOutputChannel(GetPropertyName(() => oAvgProngDiameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		double num = 0.0;
		double num2 = 0.0;
		if (!(iSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve nurbs_curve = iProfile.GetNurbsCurve();
		if (nurbs_curve == null)
		{
			OnCircle onCircle = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			nurbs_curve = new OnNurbsCurve();
			onCircle.GetNurbForm(ref nurbs_curve);
			if (nurbs_curve == null)
			{
				return UpdateMissing("Profile");
			}
		}
		double value = 0.0;
		iGemNudge.GetDouble(ref value);
		double value2 = 0.0;
		iEdgeNudge.GetDouble(ref value2);
		double value3 = 0.0;
		iHeightOffset.GetDouble(ref value3);
		double value4 = 0.0;
		iFillet.GetDoublePercentage(ref value4);
		double value5 = 0.0;
		iTaper.GetDoublePercentage(ref value5);
		double value6 = 0.0;
		iPlacementOffset.GetDouble(ref value6);
		double value7 = 0.0;
		iProngSizeMin.GetDouble(ref value7);
		double value8 = 100.0;
		iProngSizeMax.GetDouble(ref value8);
		bool value9 = false;
		iMeshedProngs.GetBoolean(ref value9);
		CSDataArray cSDataArray2 = new CSDataArray();
		int arrayCount = cSDataArray.GetArrayCount();
		ClearProngCache();
		bool flag = CSUtilGemRails.IsClosed(cSDataArray.Array);
		for (int i = 0; i <= arrayCount + 1; i++)
		{
			CSChannelGemData cSChannelGemData = null;
			if (flag)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 2 + arrayCount) % arrayCount);
			}
			else if (i - 2 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 2);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (flag)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (flag)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < cSDataArray.GetArrayCount())
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData4 = null;
			if (flag)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i + 1) % arrayCount);
			}
			else if (i + 1 < cSDataArray.GetArrayCount())
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, i + 1);
			}
			if (cSChannelGemData2 == null && cSChannelGemData4 != null && cSChannelGemData3 != null)
			{
				cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
				cSChannelGemData2.ApplyTransform(cSChannelGemData3.GetMirrorForwardBack());
				cSChannelGemData2.UnitForwardVector = -cSChannelGemData2.UnitForwardVector;
			}
			if (cSChannelGemData3 == null && cSChannelGemData2 != null && cSChannelGemData != null)
			{
				cSChannelGemData3 = new CSChannelGemData(cSChannelGemData);
				cSChannelGemData3.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
				cSChannelGemData3.UnitForwardVector = -cSChannelGemData3.UnitForwardVector;
			}
			if (cSChannelGemData3 == null || cSChannelGemData2 == null)
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSChannelGemData3, GemSettingType.PR);
			OnCurve rail = CSUtilGemRails.Rail(cSDataArray);
			OnCurve crv = CSUtilGemRails.Left(cSDataArray, rail).DuplicateCurve();
			double length = -1.23432101234321E+308;
			if (!crv.GetLength(ref length))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Smooth, 0, length * 0.5);
			RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Smooth, 1, length * 0.5);
			OnCurve crv2 = CSUtilGemRails.Right(cSDataArray, rail).DuplicateCurve();
			double length2 = -1.23432101234321E+308;
			if (!crv2.GetLength(ref length2))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Smooth, 0, length2 * 0.5);
			RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Smooth, 1, length2 * 0.5);
			On3dPoint point = (new On3dPoint(cSChannelGemData3.Center.Convert()) + cSChannelGemData2.Center.Convert()) / 2.0;
			if (!CalculateCircleEdgeLeft(cSChannelGemData2, cSChannelGemData3, crv, value2, out var circleEdgeLeft))
			{
				return UpdateError("GetCircleEdgeLeft failed");
			}
			if (!CalculateCircleEdgeRight(cSChannelGemData2, cSChannelGemData3, crv2, value2, out var circleEdgeRight))
			{
				return UpdateError("GetCircleEdgeRight failed");
			}
			OnCircle onCircle2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value);
			OnCircle onCircle3 = new OnCircle(new OnPlane(cSChannelGemData3.Center.Convert(), cSChannelGemData3.Normal.Convert()), cSChannelGemData3.SizeAverage - value);
			On3dPoint arg = onCircle2.ClosestPointTo(onCircle3.Center());
			On3dPoint arg2 = onCircle3.ClosestPointTo(onCircle2.Center());
			OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
			OnLineCurve onLineCurve2 = new OnLineCurve(onCircle2.Center(), onCircle3.Center());
			On3dPoint on3dPoint = onLineCurve.PointAt(onLineCurve.Domain().Mid());
			double t = -1.23432101234321E+308;
			if (!onLineCurve2.GetClosestPoint(on3dPoint, ref t))
			{
				return UpdateError("GetClosestPoint failed");
			}
			On3dPoint arg3 = onLineCurve2.PointAt(t);
			double num3 = on3dPoint.DistanceTo(arg3);
			double num4 = (cSChannelGemData2.GemZMin + cSChannelGemData3.GemZMin) / 2.0;
			double num5 = (cSChannelGemData2.GemZSize + cSChannelGemData3.GemZSize) / 2.0;
			On3dVector on3dVector = new On3dVector(cSChannelGemData3.Normal.Convert()) + cSChannelGemData2.Normal.Convert();
			on3dVector.Unitize();
			double t2 = 0.0;
			OnCircle onCircle4 = CSUtilGeometry.CalculateApolloniusCircle(onCircle2, onCircle3, circleEdgeLeft);
			onCircle4.ClosestPointTo(point, ref t2);
			onCircle4.Rotate(t2 + OnUtil.On_PI / 2.0, onCircle4.Normal());
			onCircle4.Translate(on3dVector * (value6 - num4));
			double t3 = 0.0;
			OnCircle onCircle5 = CSUtilGeometry.CalculateApolloniusCircle(onCircle2, onCircle3, circleEdgeRight);
			onCircle5.ClosestPointTo(point, ref t3);
			onCircle5.Rotate(t3 + OnUtil.On_PI / 2.0, onCircle5.Normal());
			onCircle5.Translate(on3dVector * (value6 - num4));
			double height = num5 + num3 + value3 - value6;
			foreach (OnCircle item in new List<OnCircle> { onCircle4, onCircle5 })
			{
				if (item.radius * 2.0 > value7 && item.radius * 2.0 < value8)
				{
					OnXform onXform = new OnXform();
					onXform.ChangeBasis(item.plane, OnUtil.On_xy_plane);
					CSData data = CreateProngCached(nurbs_curve, height, value4, value5, item.radius * 2.0, value9);
					CSDataHelper.Transform(data, onXform.Convert());
					cSDataArray2.Append(data);
					num += item.radius * 2.0;
					num2 += 1.0;
				}
			}
		}
		oSurface.SetData(cSDataArray2);
		if (num2 > 0.0)
		{
			oAvgProngDiameter.SetData(new CSNumberData(num / num2));
		}
		return base.Update();
	}

	private static bool CalculateCircleEdgeLeft(CSChannelGemData gemPrevious, CSChannelGemData gemCurrent, IOnCurve rail, double edgeNudge, out OnCircle circleEdgeLeft)
	{
		circleEdgeLeft = new OnCircle();
		OnCurve onCurve = rail.DuplicateCurve();
		double t = -1.23432101234321E+308;
		if (!onCurve.GetClosestPoint(gemPrevious.Left.Convert(), ref t))
		{
			return false;
		}
		double t2 = -1.23432101234321E+308;
		if (!onCurve.GetClosestPoint(gemCurrent.Left.Convert(), ref t2))
		{
			return false;
		}
		if (!onCurve.Trim(new OnInterval(t, t2)))
		{
			return false;
		}
		On3dPoint on3dPoint = onCurve.PointAt(onCurve.Domain().Mid());
		On3dVector on3dVector = new On3dVector(gemPrevious.LeftVector.Convert()) + new On3dVector(gemCurrent.LeftVector.Convert());
		on3dVector.Unitize();
		On3dPoint on3dPoint2 = on3dPoint + on3dVector * 10.0;
		On3dVector on3dVector2 = new On3dVector(gemPrevious.Normal.Convert()) + new On3dVector(gemCurrent.Normal.Convert());
		on3dVector2.Unitize();
		OnPlane plane = new OnPlane(on3dPoint2, on3dVector2);
		double radius = on3dPoint.DistanceTo(on3dPoint2);
		circleEdgeLeft = new OnCircle(plane, on3dPoint2, radius);
		circleEdgeLeft.radius -= edgeNudge;
		return true;
	}

	private static bool CalculateCircleEdgeRight(CSChannelGemData gemPrevious, CSChannelGemData gemCurrent, IOnCurve rail, double edgeNudge, out OnCircle circleEdgeRight)
	{
		circleEdgeRight = new OnCircle();
		OnCurve onCurve = rail.DuplicateCurve();
		double t = -1.23432101234321E+308;
		if (!onCurve.GetClosestPoint(gemPrevious.Right.Convert(), ref t))
		{
			return false;
		}
		double t2 = -1.23432101234321E+308;
		if (!onCurve.GetClosestPoint(gemCurrent.Right.Convert(), ref t2))
		{
			return false;
		}
		if (!onCurve.Trim(new OnInterval(t, t2)))
		{
			return false;
		}
		On3dPoint on3dPoint = onCurve.PointAt(onCurve.Domain().Mid());
		On3dVector on3dVector = new On3dVector(gemPrevious.RightVector.Convert()) + new On3dVector(gemCurrent.RightVector.Convert());
		on3dVector.Unitize();
		On3dPoint on3dPoint2 = on3dPoint + on3dVector * 10.0;
		On3dVector on3dVector2 = new On3dVector(gemPrevious.Normal.Convert()) + new On3dVector(gemCurrent.Normal.Convert());
		on3dVector2.Unitize();
		OnPlane plane = new OnPlane(on3dPoint2, on3dVector2);
		double radius = on3dPoint.DistanceTo(on3dPoint2);
		circleEdgeRight = new OnCircle(plane, on3dPoint2, radius);
		circleEdgeRight.radius -= edgeNudge;
		return true;
	}

	private void ClearProngCache()
	{
		_prongCachedDiameter = -1.0;
		_prongCachedData = null;
	}

	private CSData CreateProngCached(IOnNurbsCurve vProfile, double height, double filletPer, double taperPer, double diameter, bool doMeshedProngs)
	{
		if (_prongCachedData != null && Math.Abs(diameter - _prongCachedDiameter) < 0.01)
		{
			return _prongCachedData.Duplicate();
		}
		if (CSApplicationHelper.Application.IsCamMode)
		{
			height += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		OnNurbsSurface nurbsSurface;
		if (doMeshedProngs)
		{
			if (CSUtilGeometry.CreateMeshProng(vProfile, diameter, height, filletPer, taperPer, out var meshSurface))
			{
				_prongCachedDiameter = diameter;
				_prongCachedData = new CSOnGeometryData(meshSurface.Convert());
				return _prongCachedData.Duplicate();
			}
		}
		else if (CSUtilGeometry.CreateProng(vProfile, diameter, height, filletPer, taperPer, out nurbsSurface))
		{
			_prongCachedDiameter = diameter;
			_prongCachedData = new CSOnGeometryData(nurbsSurface.Convert());
			return _prongCachedData.Duplicate();
		}
		return new CSData();
	}
}
