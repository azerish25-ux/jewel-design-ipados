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

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerGC_V1", "{2704C4D6-7BDF-4CCF-94B1-471F8279B84F}")]
public sealed class CSProngPlacerGC1 : CSBlock
{
	private double _prongCachedDiameter = -1.0;

	private CSData _prongCachedData;

	public CSInputChannel iSortedGem { get; }

	public CSInputChannel iGemNudge { get; }

	public CSInputChannel iEdgeNudge { get; }

	public CSInputChannel iMinProngSize { get; }

	public CSInputChannel iMaxProngSize { get; }

	public CSInputChannel iProngHeight { get; }

	public CSInputChannel iProngVOffset { get; }

	public CSInputChannel iTaper { get; }

	public CSInputChannel iFillet { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iMeshedProngs { get; }

	public CSOutputChannel oSurface { get; }

	public CSOutputChannel oAvgProngDiameter { get; }

	public CSProngPlacerGC1(string identifier)
		: base(identifier)
	{
		iSortedGem = new CSInputChannel(GetPropertyName(() => iSortedGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemNudge = new CSInputChannel(GetPropertyName(() => iGemNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudge = new CSInputChannel(GetPropertyName(() => iEdgeNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMinProngSize = new CSInputChannel(GetPropertyName(() => iMinProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMaxProngSize = new CSInputChannel(GetPropertyName(() => iMaxProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngHeight = new CSInputChannel(GetPropertyName(() => iProngHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVOffset = new CSInputChannel(GetPropertyName(() => iProngVOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		iProngHeight.GetDouble(ref value3);
		double value4 = 0.0;
		iFillet.GetDoublePercentage(ref value4);
		double value5 = 0.0;
		iTaper.GetDoublePercentage(ref value5);
		double value6 = 0.0;
		iProngVOffset.GetDouble(ref value6);
		double value7 = 0.0;
		iMinProngSize.GetDouble(ref value7);
		double value8 = 100.0;
		iMaxProngSize.GetDouble(ref value8);
		bool value9 = false;
		iMeshedProngs.GetBoolean(ref value9);
		CSDataArray cSDataArray2 = new CSDataArray();
		int arrayCount = cSDataArray.GetArrayCount();
		ClearProngCache();
		for (int i = 0; i <= arrayCount + 1; i++)
		{
			CSChannelGemData cSChannelGemData = null;
			bool num3 = CSUtilGemRails.IsClosed(cSDataArray.Array);
			if (num3)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (num3)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < cSDataArray.GetArrayCount())
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (num3)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i + 1) % arrayCount);
			}
			else if (i + 1 < cSDataArray.GetArrayCount())
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i + 1);
			}
			if (cSChannelGemData == null && cSChannelGemData3 != null && cSChannelGemData2 != null)
			{
				cSChannelGemData = new CSChannelGemData(cSChannelGemData3);
				cSChannelGemData.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
				cSChannelGemData.UnitForwardVector = -cSChannelGemData.UnitForwardVector;
			}
			CSChannelGemData cSChannelGemData4 = null;
			if (num3)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 2 + arrayCount) % arrayCount);
			}
			else if (i - 2 >= 0)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 2);
			}
			if (cSChannelGemData2 == null && cSChannelGemData != null && cSChannelGemData4 != null)
			{
				cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
				cSChannelGemData2.ApplyTransform(cSChannelGemData.GetMirrorForwardBack());
				cSChannelGemData2.UnitForwardVector = -cSChannelGemData2.UnitForwardVector;
			}
			if (cSChannelGemData2 == null || cSChannelGemData == null)
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSChannelGemData2, GemSettingType.PR);
			On3dPoint point = (new On3dPoint(cSChannelGemData2.Center.Convert()) + cSChannelGemData.Center.Convert()) / 2.0;
			OnCircle edgeCircleRight = GetEdgeCircleRight(value2, cSChannelGemData, cSChannelGemData2);
			OnCircle edgeCircleLeft = GetEdgeCircleLeft(value2, cSChannelGemData, cSChannelGemData2);
			OnCircle c = new OnCircle(new OnPlane(cSChannelGemData.Center.Convert(), cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value);
			OnCircle c2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value);
			OnCircle onCircle2 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, edgeCircleLeft);
			OnCircle onCircle3 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, edgeCircleRight);
			On3dVector on3dVector = new On3dVector(cSChannelGemData2.Normal.Convert()) + cSChannelGemData.Normal.Convert();
			on3dVector.Unitize();
			double t = 0.0;
			onCircle2.ClosestPointTo(point, ref t);
			onCircle2.Rotate(t + OnUtil.On_PI / 2.0, onCircle2.Normal());
			onCircle3.ClosestPointTo(point, ref t);
			onCircle3.Rotate(t + OnUtil.On_PI / 2.0, onCircle3.Normal());
			onCircle2.Translate(on3dVector * value6);
			onCircle3.Translate(on3dVector * value6);
			foreach (OnCircle item in new List<OnCircle> { onCircle2, onCircle3 })
			{
				CSData data = CreateProngCached(nurbs_curve, value3, value4, value5, item.radius * 2.0, value9);
				OnXform onXform = new OnXform();
				onXform.ChangeBasis(item.plane, OnUtil.On_xy_plane);
				if (item.radius * 2.0 > value7 && item.radius * 2.0 < value8)
				{
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

	private void ClearProngCache()
	{
		_prongCachedDiameter = -1.0;
		_prongCachedData = null;
	}

	private CSData CreateProngCached(IOnNurbsCurve vProfile, double height, double filletPer, double prongTaperPer, double diameter, bool meshedProngs)
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
		if (meshedProngs)
		{
			if (CSUtilGeometry.CreateMeshProng(vProfile, diameter, height, filletPer, prongTaperPer, out var meshSurface))
			{
				_prongCachedDiameter = diameter;
				_prongCachedData = new CSOnGeometryData(meshSurface.Convert());
				return _prongCachedData.Duplicate();
			}
		}
		else if (CSUtilGeometry.CreateProng(vProfile, diameter, height, filletPer, prongTaperPer, out nurbsSurface))
		{
			_prongCachedDiameter = diameter;
			_prongCachedData = new CSOnGeometryData(nurbsSurface.Convert());
			return _prongCachedData.Duplicate();
		}
		return new CSData();
	}

	private static OnCircle GetEdgeCircleLeft(double edgeNudge, CSChannelGemData gemPrevious, CSChannelGemData gemCurrent)
	{
		On3dPoint q = new OnLine(gemPrevious.Left.Convert(), gemCurrent.Left.Convert()).PointAt(0.5);
		On3dPoint p = new On3dPoint(gemPrevious.Left.Convert()) + new On3dVector(gemPrevious.LeftVector.Convert()) * 0.1;
		On3dPoint r = new On3dPoint(gemCurrent.Left.Convert()) + new On3dVector(gemCurrent.LeftVector.Convert()) * 0.1;
		OnCircle onCircle = new OnCircle(p, q, r);
		onCircle.radius -= edgeNudge;
		return onCircle;
	}

	private static OnCircle GetEdgeCircleRight(double edgeNudge, CSChannelGemData gemPrevious, CSChannelGemData gemCurrent)
	{
		On3dPoint q = new OnLine(gemPrevious.Right.Convert(), gemCurrent.Right.Convert()).PointAt(0.5);
		On3dPoint p = new On3dPoint(gemPrevious.Right.Convert()) + new On3dVector(gemPrevious.RightVector.Convert()) * 0.1;
		On3dPoint r = new On3dPoint(gemCurrent.Right.Convert()) + new On3dVector(gemCurrent.RightVector.Convert()) * 0.1;
		OnCircle onCircle = new OnCircle(p, q, r);
		onCircle.radius -= edgeNudge;
		return onCircle;
	}
}
