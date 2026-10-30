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
			_prong = new CSOnGeometryData(((OnObject)(object)prong).Convert());
			_prongBaseRail = new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert());
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
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Expected O, but got Unknown
		//IL_0496: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Expected O, but got Unknown
		//IL_062a: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		//IL_066f: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_059b: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_05e0: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Expected O, but got Unknown
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Expected O, but got Unknown
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Expected O, but got Unknown
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Expected O, but got Unknown
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Expected O, but got Unknown
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Expected O, but got Unknown
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Expected O, but got Unknown
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Expected O, but got Unknown
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Expected O, but got Unknown
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		if (!(iCenterGem.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Center Gem");
		}
		if (!(iClusterGems.Data is CSDataArray cSDataArray) || cSDataArray.GetArrayCount() <= 0)
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
		Point3d val3 = channelGemData.Left;
		double num5 = ((Point3d)(ref val3)).DistanceTo(cSGemData.Center);
		val3 = channelGemData.Right;
		double num6 = ((Point3d)(ref val3)).DistanceTo(cSGemData.Center);
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
			On3dPoint val4 = (new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dPoint)(object)cSChannelGemData.Center.Convert()) / 2.0;
			On3dVector.op_Implicit((IOn3dPoint)(object)(new On3dPoint((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData.Normal.Convert())).Unitize();
			OnCircle edgeCircleRight = GetEdgeCircleRight(edgeNudge2, cSChannelGemData, cSChannelGemData2);
			OnCircle edgeCircleLeft = GetEdgeCircleLeft(edgeNudge, cSChannelGemData, cSChannelGemData2);
			OnCircle val5 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value3);
			OnCircle c = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value3);
			OnCircle val6 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val5, (IOnCircle)(object)c, (IOnCircle)(object)edgeCircleLeft);
			OnCircle val7 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val5, (IOnCircle)(object)c, (IOnCircle)(object)edgeCircleRight);
			On3dVector val8 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
			val8.Unitize();
			double num8 = 0.0;
			val6.ClosestPointTo((IOn3dPoint)(object)val4, ref num8);
			val6.Rotate(num8 + OnUtil.On_PI / 2.0, val6.Normal());
			val7.ClosestPointTo((IOn3dPoint)(object)val4, ref num8);
			val7.Rotate(num8 + OnUtil.On_PI / 2.0, val7.Normal());
			val6.Translate((IOn3dVector)(object)(val8 * (num3 - num)));
			val7.Translate((IOn3dVector)(object)(val8 * (num4 - num2)));
			On3dPoint val9;
			On3dPoint val10;
			if (flag)
			{
				val9 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (value15 / 2.0 + cSChannelGemData2.Size - value3));
				val10 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value3));
			}
			else
			{
				val9 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * (value15 / 2.0 + cSChannelGemData2.Size - value3));
				val10 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value3));
			}
			OnCircle val11;
			OnCircle val12;
			if (OnUtil.ON_CrossProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (IOn3dVector)(object)cSChannelGemData2.Normal.Convert() > 0.0)
			{
				val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value15 / 2.0);
				val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val10, (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert())), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value11 / 2.0);
			}
			else
			{
				val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value15 / 2.0);
				val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val10, (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert())), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value11 / 2.0);
			}
			val11.Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * (value14 - value13)));
			val12.Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * (value10 - value9)));
			if ((value == 0 || value == 1 || (flag2 && value == 2) || (flag && value == 3) || value == 6 || value == 7) && val6.radius * 2.0 > value6 && val6.radius * 2.0 < value7)
			{
				if (!CreateProngCache((IOnNurbsCurve)(object)val, val6.radius * 2.0, num, value4, value5, value2, out var prong, out var prongBaseRail))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val13 = new OnXform();
				val13.ChangeBasis((IOnPlane)(object)val6.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong, val13.Convert());
				CSDataHelper.Transform(prongBaseRail, val13.Convert());
				cSDataArray3.Append(prongBaseRail);
				cSDataArray2.Append(prong);
			}
			if ((value == 0 || value == 1 || (flag && value == 2) || (flag2 && value == 3) || value == 6 || value == 7) && val7.radius * 2.0 > value6 && val7.radius * 2.0 < value7)
			{
				if (!CreateProngCache((IOnNurbsCurve)(object)val, val7.radius * 2.0, num2, value4, value5, value2, out var prong2, out var prongBaseRail2))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val14 = new OnXform();
				val14.ChangeBasis((IOnPlane)(object)val7.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong2, val14.Convert());
				CSDataHelper.Transform(prongBaseRail2, val14.Convert());
				cSDataArray3.Append(prongBaseRail2);
				cSDataArray2.Append(prong2);
			}
			if (value == 0 || value == 1 || value == 3 || value == 4 || value == 5)
			{
				if (!CreateProngCache((IOnNurbsCurve)(object)val, val11.radius * 2.0, value13, value4, value5, value2, out var prong3, out var prongBaseRail3))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val15 = new OnXform();
				val15.ChangeBasis((IOnPlane)(object)val11.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong3, val15.Convert());
				CSDataHelper.Transform(prongBaseRail3, val15.Convert());
				cSDataArray3.Append(prongBaseRail3);
				cSDataArray2.Append(prong3);
			}
			if (value == 0 || value == 7)
			{
				if (!CreateProngCache((IOnNurbsCurve)(object)val, val12.radius * 2.0, value9, value4, value5, value2, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val16 = new OnXform();
				val16.ChangeBasis((IOnPlane)(object)val12.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(prong4, val16.Convert());
				CSDataHelper.Transform(prongBaseRail4, val16.Convert());
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
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		if (CSApplicationHelper.Application.IsCamMode)
		{
			height += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		foreach (ProngCacheItem item in _prongCache)
		{
			if (item.Compare((IOnCurve)(object)profile, diameter, height, filletPer, taperPer))
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
			prongCacheItem = new ProngCacheItem((OnGeometry)(object)meshSurface, (OnGeometry)new OnPolylineCurve((IOn3dPointArray)(object)outRailLine), (IOnCurve)(object)profile, diameter, height, filletPer, taperPer);
		}
		else
		{
			if (!CSUtilGeometry.CreateProng(profile, diameter, height, filletPer, taperPer, out var nurbsSurface, out var outputRail))
			{
				return false;
			}
			prongCacheItem = new ProngCacheItem((OnGeometry)(object)nurbsSurface, (OnGeometry)(object)outputRail, (IOnCurve)(object)profile, diameter, height, filletPer, taperPer);
		}
		_prongCache.Add(prongCacheItem);
		prong = prongCacheItem.Prong;
		prongBaseRail = prongCacheItem.ProngBaseRail;
		return true;
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
