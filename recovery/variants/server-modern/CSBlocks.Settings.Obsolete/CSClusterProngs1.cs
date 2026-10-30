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
[CSBlock("ClusterProngs_V1", "{526287C1-6B13-474A-9907-88785326C9F3}")]
public sealed class CSClusterProngs1 : CSBlock
{
	private sealed class CacheProng
	{
		public readonly OnCurve Curve;

		private readonly CSData _data;

		private readonly double _height;

		private readonly double _filletPer;

		private readonly double _prongTaperPer;

		private readonly double _diameter;

		public CacheProng(CSData data, double height, double filletPer, double prongTaperPer, double diameter, OnCurve crv)
		{
			Curve = crv;
			_data = data;
			_height = height;
			_filletPer = filletPer;
			_prongTaperPer = prongTaperPer;
			_diameter = diameter;
		}

		public bool Compare(double cheight, double cfilletPer, double cProngTaperPer, double cDiameter, double tolerance)
		{
			if (Math.Abs(_height - cheight) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_filletPer - cfilletPer) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_prongTaperPer - cProngTaperPer) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_diameter - cDiameter) > tolerance)
			{
				return false;
			}
			return true;
		}

		public CSData Duplicate()
		{
			return _data.Duplicate();
		}
	}

	private readonly List<CacheProng> _prongCache = new List<CacheProng>();

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

	public CSInputChannel iBlendToCurve { get; }

	public CSInputChannel iBlendInside { get; }

	public CSInputChannel iBlendOutside { get; }

	public CSInputChannel iBlendAmount { get; }

	public CSInputChannel iBlendAmountTop { get; }

	public CSInputChannel iBlendAmountBottom { get; }

	public CSInputChannel iBlendTilt { get; }

	public CSInputChannel iBlendAdjustment { get; }

	public CSOutputChannel oProngs { get; }

	public CSOutputChannel oProfiles { get; }

	public CSOutputChannel oBlends { get; }

	public CSClusterProngs1(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(GetPropertyName(() => iCenterGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		iBlendToCurve = new CSInputChannel(GetPropertyName(() => iBlendToCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iBlendInside = new CSInputChannel(GetPropertyName(() => iBlendInside), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBlendOutside = new CSInputChannel(GetPropertyName(() => iBlendOutside), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(GetPropertyName(() => iBlendAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmountBottom = new CSInputChannel(GetPropertyName(() => iBlendAmountBottom), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmountTop = new CSInputChannel(GetPropertyName(() => iBlendAmountTop), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendTilt = new CSInputChannel(GetPropertyName(() => iBlendTilt), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAdjustment = new CSInputChannel(GetPropertyName(() => iBlendAdjustment), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProngs = new CSOutputChannel(GetPropertyName(() => oProngs), this, ChannelDataType.Surface, ChannelArrayType.Array);
		oProfiles = new CSOutputChannel(GetPropertyName(() => oProfiles), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oBlends = new CSOutputChannel(GetPropertyName(() => oBlends), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Expected O, but got Unknown
		//IL_057d: Expected O, but got Unknown
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Expected O, but got Unknown
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Expected O, but got Unknown
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Expected O, but got Unknown
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Expected O, but got Unknown
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Expected O, but got Unknown
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Expected O, but got Unknown
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Expected O, but got Unknown
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Expected O, but got Unknown
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_0998: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Expected O, but got Unknown
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Expected O, but got Unknown
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Expected O, but got Unknown
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4a: Expected O, but got Unknown
		//IL_0d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Expected O, but got Unknown
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Expected O, but got Unknown
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Expected O, but got Unknown
		//IL_0c8f: Expected O, but got Unknown
		//IL_0cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbf: Expected O, but got Unknown
		//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Expected O, but got Unknown
		//IL_0ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e05: Expected O, but got Unknown
		//IL_0e11: Expected O, but got Unknown
		//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e41: Expected O, but got Unknown
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Expected O, but got Unknown
		CSDataArray flattenArray = iCenterGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData cSGemData))
		{
			return UpdateMissing("Missing gem");
		}
		CSDataArray cSDataArray = iClusterGems.Data as CSDataArray;
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
		}
		OnNurbsCurve val = iProngProfile.GetNurbsCurve();
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
		iEdgeNudgeInside.GetDouble(ref value2);
		double value3 = 0.0;
		iEdgeNudgeOutisde.GetDouble(ref value3);
		double value4 = 0.0;
		iProngHeightInside.GetDouble(ref value4);
		double value5 = 0.0;
		iProngHeightOutside.GetDouble(ref value5);
		double value6 = 0.0;
		iFillet.GetDoublePercentage(ref value6);
		double value7 = 0.0;
		iTaper.GetDoublePercentage(ref value7);
		double value8 = 0.0;
		iProngVOffsetInside.GetDouble(ref value8);
		double value9 = 0.0;
		iProngVOffsetOutside.GetDouble(ref value9);
		double value10 = 0.0;
		iMinProngSize.GetDouble(ref value10);
		double value11 = 100.0;
		iMaxProngSize.GetDouble(ref value11);
		bool value12 = false;
		iMeshProngs.GetBoolean(ref value12);
		double value13 = 0.5;
		iFarOutsideProngsSize.GetDouble(ref value13);
		double value14 = 0.5;
		iFarInsideProngsSize.GetDouble(ref value14);
		bool value15 = false;
		iBlendInside.GetBoolean(ref value15);
		bool value16 = false;
		iBlendOutside.GetBoolean(ref value16);
		int value17 = 0;
		iStyle.GetInteger(ref value17);
		double value18 = 0.5;
		iBlendAmount.GetDouble(ref value18);
		double value19 = 1.0;
		iBlendAmountTop.GetDouble(ref value19);
		double value20 = 1.0;
		iBlendAmountBottom.GetDouble(ref value20);
		double value21 = 0.0;
		iBlendTilt.GetDouble(ref value21);
		double value22 = 0.2;
		iBlendAdjustment.GetDouble(ref value22);
		CSDataArray flattenArray2 = iBlendToCurve.GetFlattenArray();
		OnNurbsCurve val3 = null;
		if (flattenArray2.GetArrayCount() > 0)
		{
			val3 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		int arrayCount = cSDataArray.GetArrayCount();
		_prongCache.Clear();
		double edgeNudge = value2;
		double edgeNudge2 = value3;
		double num = value4;
		double num2 = value5;
		double num3 = value8;
		double num4 = value9;
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		Point3d val4 = channelGemData.Left;
		double num5 = ((Point3d)(ref val4)).DistanceTo(cSGemData.Center);
		val4 = channelGemData.Right;
		double num6 = ((Point3d)(ref val4)).DistanceTo(cSGemData.Center);
		bool flag = false;
		if (num5 > num6)
		{
			flag = true;
			edgeNudge = value3;
			edgeNudge2 = value2;
			num = value5;
			num2 = value4;
			num3 = value9;
			num4 = value8;
		}
		bool flag2 = !flag;
		for (int i = 0; i <= arrayCount + 1; i++)
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
			else if (i < flattenArray.GetArrayCount())
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (num7)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i + 1) % arrayCount);
			}
			else if (i + 1 < flattenArray.GetArrayCount())
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
			On3dPoint val5 = (cSChannelGemData2.Center.Convert() + (IOn3dPoint)(object)cSChannelGemData.Center.Convert()) / 2.0;
			On3dVector val6 = cSChannelGemData2.Normal.Convert() + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
			val6.Unitize();
			OnCircle edgeCircleRight = GetEdgeCircleRight(edgeNudge2, cSChannelGemData, cSChannelGemData2);
			OnCircle edgeCircleLeft = GetEdgeCircleLeft(edgeNudge, cSChannelGemData, cSChannelGemData2);
			OnCircle val7 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value);
			OnCircle c = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value);
			OnCircle val8 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val7, (IOnCircle)(object)c, (IOnCircle)(object)edgeCircleLeft);
			OnCircle val9 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val7, (IOnCircle)(object)c, (IOnCircle)(object)edgeCircleRight);
			On3dVector val10 = cSChannelGemData2.Normal.Convert() + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
			val10.Unitize();
			double num8 = 0.0;
			val8.ClosestPointTo((IOn3dPoint)(object)val5, ref num8);
			val8.Rotate(num8 + OnUtil.On_PI / 2.0, val8.Normal());
			val9.ClosestPointTo((IOn3dPoint)(object)val5, ref num8);
			val9.Rotate(num8 + OnUtil.On_PI / 2.0, val9.Normal());
			val8.Translate((IOn3dVector)(object)(val10 * (num3 - num)));
			val9.Translate((IOn3dVector)(object)(val10 * (num4 - num2)));
			On3dPoint val11 = null;
			On3dVector val12 = null;
			if (val3 != null)
			{
				double num9 = 0.0;
				((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)val5, ref num9);
				double num10 = ((OnCurve)val3).PointAt(num9).DistanceTo((IOn3dPoint)(object)val5);
				((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)(val5 - (IOn3dVector)(object)(val6 * num10 * value22)), ref num9);
				val11 = ((OnCurve)val3).PointAt(num9);
				val12 = ((OnCurve)val3).TangentAt(num9);
			}
			On3dPoint val13;
			On3dPoint val14;
			if (flag)
			{
				val13 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitLeftVector.Convert() * (value13 / 2.0 + cSChannelGemData2.Size - value));
				val14 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitRightVector.Convert() * (value14 / 2.0 + cSChannelGemData2.Size - value));
			}
			else
			{
				val13 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitRightVector.Convert() * (value13 / 2.0 + cSChannelGemData2.Size - value));
				val14 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitLeftVector.Convert() * (value14 / 2.0 + cSChannelGemData2.Size - value));
			}
			OnCircle val15;
			OnCircle val16;
			if (OnUtil.ON_CrossProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (IOn3dVector)(object)cSChannelGemData2.Normal.Convert() > 0.0)
			{
				val15 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val13, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value13 / 2.0);
				val16 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val14, (IOn3dVector)(object)(-cSChannelGemData2.UnitForwardVector.Convert()), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value14 / 2.0);
			}
			else
			{
				val15 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val13, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value13 / 2.0);
				val16 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val14, (IOn3dVector)(object)(-cSChannelGemData2.UnitForwardVector.Convert()), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value14 / 2.0);
			}
			val15.Translate((IOn3dVector)(object)(cSChannelGemData2.Normal.Convert() * (value9 - value5)));
			val16.Translate((IOn3dVector)(object)(cSChannelGemData2.Normal.Convert() * (value8 - value4)));
			if (value17 == 0 || value17 == 1 || (flag2 && value17 == 2) || (flag && value17 == 3) || value17 == 6 || value17 == 7)
			{
				OnCurve curve;
				CSData data = CreateProngCached((IOnNurbsCurve)(object)val, num, value6, value7, val8.radius * 2.0, value12, out curve);
				OnXform val17 = new OnXform();
				val17.ChangeBasis((IOnPlane)(object)val8.plane, OnUtil.On_xy_plane);
				((OnGeometry)curve).Transform((IOnXform)(object)val17);
				if (val8.radius * 2.0 > value10 && val8.radius * 2.0 < value11)
				{
					cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)curve.Convert(), val17.Convert(), applyTransform: false));
					CSDataHelper.Transform(data, val17.Convert());
					cSDataArray2.Append(data);
					if (val11 != (IOn3dPoint)null && ((!flag && value15) || (flag && value16)))
					{
						On3dVector val18 = new On3dVector(val8.Normal());
						val18.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), (IOn3dVector)(object)val12);
						OnBrep brep = CSUtilSweep.Sweep1((IOnCurve)(object)CSUtilCurves.BlendCurve(val8.Center(), (IOn3dPoint)(object)val11, (IOn3dVector)(object)(-new On3dVector(val8.Normal())), (IOn3dVector)(object)val18, value18, value19, value20), (IOnCurve)(object)curve);
						cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
					}
				}
			}
			if (value17 == 0 || value17 == 1 || (flag && value17 == 2) || (flag2 && value17 == 3) || value17 == 6 || value17 == 7)
			{
				OnCurve curve2;
				CSData data2 = CreateProngCached((IOnNurbsCurve)(object)val, num2, value6, value7, val9.radius * 2.0, value12, out curve2);
				OnXform val19 = new OnXform();
				val19.ChangeBasis((IOnPlane)(object)val9.plane, OnUtil.On_xy_plane);
				((OnGeometry)curve2).Transform((IOnXform)(object)val19);
				if (val9.radius * 2.0 > value10 && val9.radius * 2.0 < value11)
				{
					cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)curve2.Convert(), val19.Convert(), applyTransform: false));
					CSDataHelper.Transform(data2, val19.Convert());
					cSDataArray2.Append(data2);
					if (val11 != (IOn3dPoint)null && ((!flag && value16) || (flag && value15)))
					{
						On3dVector val20 = new On3dVector(val9.Normal());
						val20.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), (IOn3dVector)(object)val12);
						OnBrep brep2 = CSUtilSweep.Sweep1((IOnCurve)(object)CSUtilCurves.BlendCurve(val9.Center(), (IOn3dPoint)(object)val11, (IOn3dVector)(object)(-new On3dVector(val9.Normal())), (IOn3dVector)(object)val20, value18, value19, value20), (IOnCurve)(object)curve2);
						cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)brep2.Convert()));
					}
				}
			}
			if (value17 == 0 || value17 == 1 || value17 == 3 || value17 == 4 || value17 == 5)
			{
				OnCurve curve3;
				CSData data3 = CreateProngCached((IOnNurbsCurve)(object)val, value5, value6, value7, val15.radius * 2.0, value12, out curve3);
				OnXform val21 = new OnXform();
				val21.ChangeBasis((IOnPlane)(object)val15.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(data3, val21.Convert());
				cSDataArray2.Append(data3);
				if (curve3 != null)
				{
					((OnGeometry)curve3).Transform((IOnXform)(object)val21);
					cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)curve3.Convert(), val21.Convert(), applyTransform: false));
					if (val11 != (IOn3dPoint)null)
					{
						On3dPoint pointEnd = null;
						On3dVector val22 = null;
						if (val3 != null)
						{
							double num11 = 0.0;
							((OnCurve)val3).GetClosestPoint(val15.Center(), ref num11);
							double num12 = ((OnCurve)val3).PointAt(num11).DistanceTo(val15.Center());
							((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)(new On3dPoint(val15.Center()) - (IOn3dVector)(object)(new On3dVector(val15.Normal()) * num12 * value22)), ref num11);
							pointEnd = ((OnCurve)val3).PointAt(num11);
							val22 = ((OnCurve)val3).TangentAt(num11);
						}
						if (value16)
						{
							On3dVector val23 = new On3dVector(val15.Normal());
							val23.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), (IOn3dVector)(object)val22);
							OnBrep brep3 = CSUtilSweep.Sweep1((IOnCurve)(object)CSUtilCurves.BlendCurve(val15.Center(), (IOn3dPoint)(object)pointEnd, (IOn3dVector)(object)(-new On3dVector(val15.Normal())), (IOn3dVector)(object)val23, value18, value19, value20), (IOnCurve)(object)curve3);
							cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)brep3.Convert()));
						}
					}
				}
			}
			if (value17 != 0 && value17 != 7)
			{
				continue;
			}
			OnCurve curve4;
			CSData data4 = CreateProngCached((IOnNurbsCurve)(object)val, value4, value6, value7, val16.radius * 2.0, value12, out curve4);
			OnXform val24 = new OnXform();
			val24.ChangeBasis((IOnPlane)(object)val16.plane, OnUtil.On_xy_plane);
			CSDataHelper.Transform(data4, val24.Convert());
			cSDataArray2.Append(data4);
			if (curve4 == null)
			{
				continue;
			}
			((OnGeometry)curve4).Transform((IOnXform)(object)val24);
			cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)curve4.Convert()));
			if (val11 != (IOn3dPoint)null)
			{
				On3dPoint pointEnd2 = null;
				On3dVector val25 = null;
				if (val3 != null)
				{
					double num13 = 0.0;
					((OnCurve)val3).GetClosestPoint(val16.Center(), ref num13);
					double num14 = ((OnCurve)val3).PointAt(num13).DistanceTo(val16.Center());
					((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)(new On3dPoint(val16.Center()) - (IOn3dVector)(object)(new On3dVector(val16.Normal()) * num14 * value22)), ref num13);
					pointEnd2 = ((OnCurve)val3).PointAt(num13);
					val25 = ((OnCurve)val3).TangentAt(num13);
				}
				if (value16)
				{
					On3dVector val26 = new On3dVector(val16.Normal());
					val26.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), (IOn3dVector)(object)val25);
					OnBrep brep4 = CSUtilSweep.Sweep1((IOnCurve)(object)CSUtilCurves.BlendCurve(val16.Center(), (IOn3dPoint)(object)pointEnd2, (IOn3dVector)(object)(-new On3dVector(val16.Normal())), (IOn3dVector)(object)val26, value18, value19, value20), (IOnCurve)(object)curve4);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)brep4.Convert()));
				}
			}
		}
		oProngs.SetData(cSDataArray2);
		oProfiles.SetData(cSDataArray3);
		oBlends.SetData(cSDataArray4);
		return base.Update();
	}

	private CSData CreateProngCached(IOnNurbsCurve vProfile, double height, double filletPer, double prongTaperPer, double diameter, bool meshedProngs, out OnCurve curve)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		curve = null;
		foreach (CacheProng item in _prongCache)
		{
			if (item.Compare(height, filletPer, prongTaperPer, diameter, 0.01))
			{
				if (item.Curve != null)
				{
					curve = item.Curve.Duplicate();
				}
				return item.Duplicate();
			}
		}
		if (CSApplicationHelper.Application.IsCamMode)
		{
			height += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		OnNurbsSurface nurbsSurface;
		OnNurbsCurve outputRail;
		if (meshedProngs)
		{
			if (CSUtilGeometry.CreateMeshProng(vProfile, diameter, height, filletPer, prongTaperPer, out var meshSurface, out var outRailLine))
			{
				CSOnGeometryData data = new CSOnGeometryData((GeometryBase)(object)meshSurface.Convert());
				OnPolylineCurve crv = new OnPolylineCurve((IOn3dPointArray)(object)outRailLine);
				CacheProng cacheProng = new CacheProng(data, height, filletPer, prongTaperPer, diameter, (OnCurve)(object)crv);
				_prongCache.Add(cacheProng);
				if (cacheProng.Curve != null)
				{
					curve = cacheProng.Curve.Duplicate();
				}
				return cacheProng.Duplicate();
			}
		}
		else if (CSUtilGeometry.CreateProng(vProfile, diameter, height, filletPer, prongTaperPer, out nurbsSurface, out outputRail))
		{
			CacheProng cacheProng2 = new CacheProng(new CSOnGeometryData((GeometryBase)(object)nurbsSurface.Convert()), height, filletPer, prongTaperPer, diameter, (OnCurve)(object)outputRail);
			_prongCache.Add(cacheProng2);
			if (cacheProng2.Curve != null)
			{
				curve = cacheProng2.Curve.Duplicate();
			}
			return cacheProng2.Duplicate();
		}
		return new CSData();
	}

	private static OnCircle GetEdgeCircleLeft(double edgeNudge, CSChannelGemData gemPrevious, CSChannelGemData gemCurrent)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		On3dPoint val = new OnLine((IOn3dPoint)(object)gemPrevious.Left.Convert(), (IOn3dPoint)(object)gemCurrent.Left.Convert()).PointAt(0.5);
		On3dPoint obj = gemPrevious.Left.Convert() + (IOn3dVector)(object)(gemPrevious.LeftVector.Convert() * 0.1);
		On3dPoint val2 = gemCurrent.Left.Convert() + (IOn3dVector)(object)(gemCurrent.LeftVector.Convert() * 0.1);
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
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		On3dPoint val = new OnLine((IOn3dPoint)(object)gemPrevious.Right.Convert(), (IOn3dPoint)(object)gemCurrent.Right.Convert()).PointAt(0.5);
		On3dPoint obj = gemPrevious.Right.Convert() + (IOn3dVector)(object)(gemPrevious.RightVector.Convert() * 0.1);
		On3dPoint val2 = gemCurrent.Right.Convert() + (IOn3dVector)(object)(gemCurrent.RightVector.Convert() * 0.1);
		OnCircle val3 = new OnCircle((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val, (IOn3dPoint)(object)val2);
		val3.radius -= edgeNudge;
		return val3;
	}
}
