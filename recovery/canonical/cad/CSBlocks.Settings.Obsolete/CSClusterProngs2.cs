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
[CSBlock("ClusterProngs_V2", "{EA7D1ABD-A7DF-4E6F-8209-4F431EEE63F6}")]
public sealed class CSClusterProngs2 : CSBlock
{
	private sealed class ProngCacheItem
	{
		private readonly CSOnGeometryData _prong;

		private readonly CSOnGeometryData _prongBaseRail;

		private readonly IOnCurve _profile;

		private readonly double _diameter;

		private readonly double _height;

		private readonly double _filletPer;

		private readonly double _taperPer;

		public CSData Prong => _prong.Duplicate();

		public CSData ProngBaseRail => _prongBaseRail.Duplicate();

		public ProngCacheItem(OnGeometry prong, OnGeometry prongBaseRail, IOnCurve profile, double diameter, double height, double filletPer, double taperPer)
		{
			_prong = new CSOnGeometryData(prong.Convert());
			_prongBaseRail = new CSOnGeometryData(prongBaseRail.Convert());
			_profile = profile;
			_diameter = diameter;
			_height = height;
			_filletPer = filletPer;
			_taperPer = taperPer;
		}

		public bool Compare(IOnCurve profile, double diameter, double height, double filletPer, double taperPer, double tolerance = 0.001)
		{
			if (_profile.Domain().Min() - profile.Domain().Min() > tolerance)
			{
				return false;
			}
			if (_profile.Domain().Mid() - profile.Domain().Mid() > tolerance)
			{
				return false;
			}
			if (_profile.Domain().Max() - profile.Domain().Max() > tolerance)
			{
				return false;
			}
			if (Math.Abs(_diameter - diameter) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_height - height) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_filletPer - filletPer) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_taperPer - taperPer) > tolerance)
			{
				return false;
			}
			return true;
		}
	}

	private readonly List<ProngCacheItem> _prongCache = new List<ProngCacheItem>();

	public CSInputChannel iCenterGem { get; }

	public CSInputChannel iClusterGems { get; }

	public CSInputChannel iProngProfile { get; }

	public CSInputChannel iCenterShape { get; }

	public CSInputChannel iGemNudge { get; }

	public CSInputChannel iEdgeNudgeOutisde { get; }

	public CSInputChannel iEdgeNudgeInside { get; }

	public CSInputChannel iMinProngSize { get; }

	public CSInputChannel iMaxProngSize { get; }

	public CSInputChannel iProngHeightInside { get; }

	public CSInputChannel iProngHeightOutside { get; }

	public CSInputChannel iProngVOffsetInside { get; }

	public CSInputChannel iProngVOffsetOutside { get; }

	public CSInputChannel iTaper { get; }

	public CSInputChannel iFillet { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iFarInsideProngsSize { get; }

	public CSInputChannel iFarOutsideProngsSize { get; }

	public CSInputChannel iMeshProngs { get; }

	public CSOutputChannel oProngs { get; }

	public CSOutputChannel oProfiles { get; }

	public CSClusterProngs2(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(GetPropertyName(() => iCenterGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iClusterGems = new CSInputChannel(GetPropertyName(() => iClusterGems), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iProngProfile = new CSInputChannel(GetPropertyName(() => iProngProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCenterShape = new CSInputChannel(GetPropertyName(() => iCenterShape), this, ChannelDataType.Any, ChannelArrayType.Any);
		iGemNudge = new CSInputChannel(GetPropertyName(() => iGemNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudgeOutisde = new CSInputChannel(GetPropertyName(() => iEdgeNudgeOutisde), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudgeInside = new CSInputChannel(GetPropertyName(() => iEdgeNudgeInside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMinProngSize = new CSInputChannel(GetPropertyName(() => iMinProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMaxProngSize = new CSInputChannel(GetPropertyName(() => iMaxProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngHeightInside = new CSInputChannel(GetPropertyName(() => iProngHeightInside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngHeightOutside = new CSInputChannel(GetPropertyName(() => iProngHeightOutside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVOffsetInside = new CSInputChannel(GetPropertyName(() => iProngVOffsetInside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVOffsetOutside = new CSInputChannel(GetPropertyName(() => iProngVOffsetOutside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTaper = new CSInputChannel(GetPropertyName(() => iTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFillet = new CSInputChannel(GetPropertyName(() => iFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFarInsideProngsSize = new CSInputChannel(GetPropertyName(() => iFarInsideProngsSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFarOutsideProngsSize = new CSInputChannel(GetPropertyName(() => iFarOutsideProngsSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMeshProngs = new CSInputChannel(GetPropertyName(() => iMeshProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oProngs = new CSOutputChannel(GetPropertyName(() => oProngs), this, ChannelDataType.Surface, ChannelArrayType.Array);
		oProfiles = new CSOutputChannel(GetPropertyName(() => oProfiles), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		if (!(iCenterGem.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Center Gem");
		}
		if (!(iClusterGems.Data is CSDataArray cSDataArray) || cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
		}
		OnNurbsCurve nurbs_curve = iProngProfile.GetNurbsCurve();
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
		int value = 0;
		iStyle.GetInteger(ref value);
		bool value2 = false;
		iMeshProngs.GetBoolean(ref value2);
		double value3 = 0.0;
		iGemNudge.GetDouble(ref value3);
		double value4 = 0.0;
		iFillet.GetDoublePercentage(ref value4);
		double value5 = 0.0;
		iTaper.GetDoublePercentage(ref value5);
		double value6 = 0.0;
		iMinProngSize.GetDouble(ref value6);
		double value7 = 100.0;
		iMaxProngSize.GetDouble(ref value7);
		double value8 = 0.0;
		iEdgeNudgeInside.GetDouble(ref value8);
		double value9 = 0.0;
		iProngHeightInside.GetDouble(ref value9);
		double value10 = 0.0;
		iProngVOffsetInside.GetDouble(ref value10);
		double value11 = 0.5;
		iFarInsideProngsSize.GetDouble(ref value11);
		double value12 = 0.0;
		iEdgeNudgeOutisde.GetDouble(ref value12);
		double value13 = 0.0;
		iProngHeightOutside.GetDouble(ref value13);
		double value14 = 0.0;
		iProngVOffsetOutside.GetDouble(ref value14);
		double value15 = 0.5;
		iFarOutsideProngsSize.GetDouble(ref value15);
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		int arrayCount = cSDataArray.GetArrayCount();
		_prongCache.Clear();
		double edgeNudge = value8;
		double edgeNudge2 = value12;
		double num = value9;
		double num2 = value13;
		double num3 = value10;
		double num4 = value14;
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		double num5 = channelGemData.Left.DistanceTo(cSGemData.Center);
		double num6 = channelGemData.Right.DistanceTo(cSGemData.Center);
		bool flag = false;
		if (num5 > num6)
		{
			flag = true;
			edgeNudge = value12;
			edgeNudge2 = value8;
			num = value13;
			num2 = value9;
			num3 = value14;
			num4 = value10;
		}
		bool flag2 = !flag;
		for (int i = 0; i < arrayCount; i++)
		{
			CSChannelGemData cSChannelGemData = null;
			bool num7 = CSUtilGemRails.IsClosed(cSDataArray.Array);
			if (num7)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (num7)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < cSDataArray.GetArrayCount())
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (num7)
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
			}
			CSChannelGemData cSChannelGemData4 = null;
			if (num7)
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
			}
			if (cSChannelGemData2 == null || cSChannelGemData == null)
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSChannelGemData2, GemSettingType.PR);
			On3dPoint point = (new On3dPoint(cSChannelGemData2.Center.Convert()) + cSChannelGemData.Center.Convert()) / 2.0;
			((On3dVector)(new On3dPoint(cSChannelGemData2.Normal.Convert()) + cSChannelGemData.Normal.Convert())).Unitize();
			OnCircle edgeCircleRight = GetEdgeCircleRight(edgeNudge2, cSChannelGemData, cSChannelGemData2);
			OnCircle edgeCircleLeft = GetEdgeCircleLeft(edgeNudge, cSChannelGemData, cSChannelGemData2);
			OnCircle c = new OnCircle(new OnPlane(cSChannelGemData.Center.Convert(), cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value3);
			OnCircle c2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value3);
			OnCircle onCircle2 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, edgeCircleLeft);
			OnCircle onCircle3 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, edgeCircleRight);
			On3dVector on3dVector = new On3dVector(cSChannelGemData2.Normal.Convert()) + cSChannelGemData.Normal.Convert();
			on3dVector.Unitize();
			double t = 0.0;
			onCircle2.ClosestPointTo(point, ref t);
			onCircle2.Rotate(t + OnUtil.On_PI / 2.0, onCircle2.Normal());
			onCircle3.ClosestPointTo(point, ref t);
			onCircle3.Rotate(t + OnUtil.On_PI / 2.0, onCircle3.Normal());
			onCircle2.Translate(on3dVector * (num3 - num));
			onCircle3.Translate(on3dVector * (num4 - num2));
			On3dPoint origin;
			On3dPoint origin2;
			if (flag)
			{
				origin = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * (value15 / 2.0 + cSChannelGemData2.Size - value3);
				origin2 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value3);
			}
			else
			{
				origin = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * (value15 / 2.0 + cSChannelGemData2.Size - value3);
				origin2 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value3);
			}
			OnCircle onCircle4;
			OnCircle onCircle5;
			if (OnUtil.ON_CrossProduct(cSChannelGemData2.ForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()) * cSChannelGemData2.Normal.Convert() > 0.0)
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), value15 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -new On3dVector(cSChannelGemData2.UnitForwardVector.Convert()), cSChannelGemData2.UnitRightVector.Convert()), value11 / 2.0);
			}
			else
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), value15 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -new On3dVector(cSChannelGemData2.UnitForwardVector.Convert()), cSChannelGemData2.UnitLeftVector.Convert()), value11 / 2.0);
			}
			onCircle4.Translate(new On3dVector(cSChannelGemData2.Normal.Convert()) * (value14 - value13));
			onCircle5.Translate(new On3dVector(cSChannelGemData2.Normal.Convert()) * (value10 - value9));
			if ((value == 0 || value == 1 || (flag2 && value == 2) || (flag && value == 3) || value == 6 || value == 7) && onCircle2.radius * 2.0 > value6 && onCircle2.radius * 2.0 < value7)
			{
				if (!CreateProngCache(nurbs_curve, onCircle2.radius * 2.0, num, value4, value5, value2, out var prong, out var prongBaseRail))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform = new OnXform();
				onXform.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong, onXform.Convert());
				CSDataHelper.Transform(prongBaseRail, onXform.Convert());
				cSDataArray3.Append(prongBaseRail);
				cSDataArray2.Append(prong);
			}
			if ((value == 0 || value == 1 || (flag && value == 2) || (flag2 && value == 3) || value == 6 || value == 7) && onCircle3.radius * 2.0 > value6 && onCircle3.radius * 2.0 < value7)
			{
				if (!CreateProngCache(nurbs_curve, onCircle3.radius * 2.0, num2, value4, value5, value2, out var prong2, out var prongBaseRail2))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform2 = new OnXform();
				onXform2.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong2, onXform2.Convert());
				CSDataHelper.Transform(prongBaseRail2, onXform2.Convert());
				cSDataArray3.Append(prongBaseRail2);
				cSDataArray2.Append(prong2);
			}
			if (value == 0 || value == 1 || value == 3 || value == 4 || value == 5)
			{
				if (!CreateProngCache(nurbs_curve, onCircle4.radius * 2.0, value13, value4, value5, value2, out var prong3, out var prongBaseRail3))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform3 = new OnXform();
				onXform3.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong3, onXform3.Convert());
				CSDataHelper.Transform(prongBaseRail3, onXform3.Convert());
				cSDataArray3.Append(prongBaseRail3);
				cSDataArray2.Append(prong3);
			}
			if (value == 0 || value == 7)
			{
				if (!CreateProngCache(nurbs_curve, onCircle5.radius * 2.0, value9, value4, value5, value2, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform4 = new OnXform();
				onXform4.ChangeBasis(onCircle5.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong4, onXform4.Convert());
				CSDataHelper.Transform(prongBaseRail4, onXform4.Convert());
				cSDataArray3.Append(prongBaseRail4);
				cSDataArray2.Append(prong4);
			}
		}
		oProngs.SetData(cSDataArray2);
		oProfiles.SetData(cSDataArray3);
		return base.Update();
	}

	private bool CreateProngCache(IOnNurbsCurve profile, double diameter, double height, double filletPer, double taperPer, bool doMesh, out CSData prong, out CSData prongBaseRail)
	{
		if (CSApplicationHelper.Application.IsCamMode)
		{
			height += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		foreach (ProngCacheItem item in _prongCache)
		{
			if (item.Compare(profile, diameter, height, filletPer, taperPer))
			{
				prong = item.Prong;
				prongBaseRail = item.ProngBaseRail;
				return true;
			}
		}
		prong = null;
		prongBaseRail = null;
		ProngCacheItem prongCacheItem;
		if (doMesh)
		{
			if (!CSUtilGeometry.CreateMeshProng(profile, diameter, height, filletPer, taperPer, out var meshSurface, out var outRailLine))
			{
				return false;
			}
			prongCacheItem = new ProngCacheItem(meshSurface, new OnPolylineCurve(outRailLine), profile, diameter, height, filletPer, taperPer);
		}
		else
		{
			if (!CSUtilGeometry.CreateProng(profile, diameter, height, filletPer, taperPer, out var nurbsSurface, out var outputRail))
			{
				return false;
			}
			prongCacheItem = new ProngCacheItem(nurbsSurface, outputRail, profile, diameter, height, filletPer, taperPer);
		}
		_prongCache.Add(prongCacheItem);
		prong = prongCacheItem.Prong;
		prongBaseRail = prongCacheItem.ProngBaseRail;
		return true;
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
