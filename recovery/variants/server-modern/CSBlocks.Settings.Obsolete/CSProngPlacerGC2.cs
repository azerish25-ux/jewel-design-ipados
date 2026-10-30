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
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerGC_V2", "{26943638-8187-42D1-AF8B-6BEA70EB5C33}")]
public sealed class CSProngPlacerGC2 : CSBlock
{
	private double _prongCachedDiameter = -1.0;

	private CSData _prongCachedData;

	public CSInputChannel iSortedGem { get; }

	public CSInputChannel iGemNudge { get; }

	public CSInputChannel iEdgeNudge { get; }

	public CSInputChannel iMinProngSize { get; }

	public CSInputChannel iMaxProngSize { get; }

	public CSInputChannel iHeightOffset { get; }

	public CSInputChannel iPlacementOffset { get; }

	public CSInputChannel iTaper { get; }

	public CSInputChannel iFillet { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iMeshedProngs { get; }

	public CSOutputChannel oSurface { get; }

	public CSOutputChannel oAvgProngDiameter { get; }

	public CSProngPlacerGC2(string identifier)
		: base(identifier)
	{
		iSortedGem = new CSInputChannel(GetPropertyName(() => iSortedGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemNudge = new CSInputChannel(GetPropertyName(() => iGemNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudge = new CSInputChannel(GetPropertyName(() => iEdgeNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMinProngSize = new CSInputChannel(GetPropertyName(() => iMinProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMaxProngSize = new CSInputChannel(GetPropertyName(() => iMaxProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
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
		OnNurbsCurve val = iProfile.GetNurbsCurve();
		if (val == null)
		{
			OnCircle val2 = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			val = new OnNurbsCurve();
			val2.GetNurbForm(ref val);
			if (val == null)
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
		iMinProngSize.GetDouble(ref value7);
		double value8 = 100.0;
		iMaxProngSize.GetDouble(ref value8);
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
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (flag)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < cSDataArray.GetArrayCount())
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (flag)
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
			if (flag)
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
			On3dPoint val3 = (new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dPoint)(object)cSChannelGemData.Center.Convert()) / 2.0;
			OnCircle edgeCircleRight = GetEdgeCircleRight(value2, cSChannelGemData, cSChannelGemData2);
			OnCircle edgeCircleLeft = GetEdgeCircleLeft(value2, cSChannelGemData, cSChannelGemData2);
			OnCircle val4 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value);
			OnCircle val5 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value);
			On3dPoint obj = val4.ClosestPointTo(val5.Center());
			On3dPoint val6 = val5.ClosestPointTo(val4.Center());
			OnLineCurve val7 = new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val6);
			OnLineCurve val8 = new OnLineCurve(val4.Center(), val5.Center());
			On3dPoint val9 = ((OnCurve)val7).PointAt(((OnCurve)val7).Domain().Mid());
			double num3 = -1.23432101234321E+308;
			if (!((OnCurve)val8).GetClosestPoint((IOn3dPoint)(object)val9, ref num3))
			{
				return UpdateError("GetClosestPoint failed");
			}
			On3dPoint val10 = ((OnCurve)val8).PointAt(num3);
			double num4 = val9.DistanceTo((IOn3dPoint)(object)val10);
			double num5 = (cSChannelGemData.GemZMin + cSChannelGemData2.GemZMin) / 2.0;
			double num6 = (cSChannelGemData.GemZSize + cSChannelGemData2.GemZSize) / 2.0;
			On3dVector val11 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
			val11.Unitize();
			double num7 = 0.0;
			OnCircle val12 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val4, (IOnCircle)(object)val5, (IOnCircle)(object)edgeCircleLeft);
			val12.ClosestPointTo((IOn3dPoint)(object)val3, ref num7);
			val12.Rotate(num7 + OnUtil.On_PI / 2.0, val12.Normal());
			val12.Translate((IOn3dVector)(object)(val11 * (value6 - num5)));
			double num8 = 0.0;
			OnCircle val13 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val4, (IOnCircle)(object)val5, (IOnCircle)(object)edgeCircleRight);
			val13.ClosestPointTo((IOn3dPoint)(object)val3, ref num8);
			val13.Rotate(num8 + OnUtil.On_PI / 2.0, val13.Normal());
			val13.Translate((IOn3dVector)(object)(val11 * (value6 - num5)));
			double height = num6 + num4 + value3 - value6;
			foreach (OnCircle item in new List<OnCircle> { val12, val13 })
			{
				if (item.radius * 2.0 > value7 && item.radius * 2.0 < value8)
				{
					OnXform val14 = new OnXform();
					val14.ChangeBasis((IOnPlane)(object)item.plane, OnUtil.On_xy_plane);
					CSData data = CreateProngCached((IOnNurbsCurve)(object)val, height, value4, value5, item.radius * 2.0, value9);
					CSDataHelper.Transform(data, val14.Convert());
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
				_prongCachedData = new CSOnGeometryData((GeometryBase)(object)meshSurface.Convert());
				return _prongCachedData.Duplicate();
			}
		}
		else if (CSUtilGeometry.CreateProng(vProfile, diameter, height, filletPer, taperPer, out nurbsSurface))
		{
			_prongCachedDiameter = diameter;
			_prongCachedData = new CSOnGeometryData((GeometryBase)(object)nurbsSurface.Convert());
			return _prongCachedData.Duplicate();
		}
		return new CSData();
	}

	private static OnCircle GetEdgeCircleLeft(double edgeNudge, CSChannelGemData gemPrevious, CSChannelGemData gemCurrent)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0090: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		On3dPoint val = new OnLine((IOn3dPoint)(object)gemPrevious.Left.Convert(), (IOn3dPoint)(object)gemCurrent.Left.Convert()).PointAt(0.5);
		On3dPoint obj = new On3dPoint((IOn3dPoint)(object)gemPrevious.Left.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)gemPrevious.LeftVector.Convert()) * 0.1);
		On3dPoint val2 = new On3dPoint((IOn3dPoint)(object)gemCurrent.Left.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)gemCurrent.LeftVector.Convert()) * 0.1);
		OnCircle val3 = new OnCircle((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val, (IOn3dPoint)(object)val2);
		val3.radius -= edgeNudge;
		return val3;
	}

	private static OnCircle GetEdgeCircleRight(double edgeNudge, CSChannelGemData gemPrevious, CSChannelGemData gemCurrent)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0090: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		On3dPoint val = new OnLine((IOn3dPoint)(object)gemPrevious.Right.Convert(), (IOn3dPoint)(object)gemCurrent.Right.Convert()).PointAt(0.5);
		On3dPoint obj = new On3dPoint((IOn3dPoint)(object)gemPrevious.Right.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)gemPrevious.RightVector.Convert()) * 0.1);
		On3dPoint val2 = new On3dPoint((IOn3dPoint)(object)gemCurrent.Right.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)gemCurrent.RightVector.Convert()) * 0.1);
		OnCircle val3 = new OnCircle((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val, (IOn3dPoint)(object)val2);
		val3.radius -= edgeNudge;
		return val3;
	}
}
