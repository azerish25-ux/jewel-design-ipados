using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ClusterProngsToCurve_V1", "{89A495C3-8D42-4F2B-B6F6-C1C68C07F6D3}")]
public sealed class CSClusterProngsToCurve1 : CSBlock
{
	private readonly CSProngCache _prongCache = new CSProngCache();

	private readonly CSProngDomeCache _prongDomeCache = new CSProngDomeCache();

	public CSInputChannel InBlendAdjustment { get; }

	public CSInputChannel InBlendAmount { get; }

	public CSInputChannel InBlendAmountBase { get; }

	public CSInputChannel InBlendAmountTop { get; }

	public CSInputChannel InBlendTilt { get; }

	public CSInputChannel InCenterGem { get; }

	public CSInputChannel InClusterGems { get; }

	public CSInputChannel InCurve { get; }

	public CSInputChannel InDoMeshProngs { get; }

	public CSInputChannel InEnableBlendInside { get; }

	public CSInputChannel InEnableBlendOutside { get; }

	public CSInputChannel InFillet { get; }

	public CSInputChannel InInsideEdgeNudge { get; }

	public CSInputChannel InInsideHeight { get; }

	public CSInputChannel InInsideSizeBase { get; }

	public CSInputChannel InInsideSizeTop { get; }

	public CSInputChannel InInsideVerticalLocation { get; }

	public CSInputChannel InNudge { get; }

	public CSInputChannel InOutsideEdgeNudge { get; }

	public CSInputChannel InOutsideHeight { get; }

	public CSInputChannel InOutsideSizeBase { get; }

	public CSInputChannel InOutsideSizeTop { get; }

	public CSInputChannel InOutsideVerticalLocation { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InSizeMax { get; }

	public CSInputChannel InSizeMin { get; }

	public CSInputChannel InStyle { get; }

	public CSOutputChannel OutProfiles { get; }

	public CSOutputChannel OutProngs { get; }

	public CSOutputChannel OutRails { get; }

	public CSClusterProngsToCurve1(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InClusterGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMin = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMax = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideSizeTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideSizeBase = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideEdgeNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideSizeTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideSizeBase = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideEdgeNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InDoMeshProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableBlendInside = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableBlendOutside = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAmountTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAmountBase = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAdjustment = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		OutRails = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		return UpdateSync();
	}

	private bool UpdateConcurrent()
	{
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		InitializeCancellationTokenSource();
		CSGemData centerGemData = InCenterGem.Data as CSGemData;
		if (centerGemData == null)
		{
			return UpdateMissing("Center Gem");
		}
		CSDataArray channelGemDatas = InClusterGems.Data as CSDataArray;
		if (channelGemDatas == null || channelGemDatas.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
		}
		OnNurbsCurve blendToCurve = InCurve.GetNurbsCurve();
		if (blendToCurve == null)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsCurve vProfile = InProfile.GetNurbsCurve();
		if (vProfile == null)
		{
			OnCircle val = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			vProfile = new OnNurbsCurve();
			val.GetNurbForm(ref vProfile);
			if (vProfile == null)
			{
				return UpdateMissing("Profile");
			}
		}
		ValueListItem vStyle = ValueListItem.ClusterProngStyle1;
		InStyle.GetValueListItem(ref vStyle);
		bool vDoMeshedProngs = true;
		InDoMeshProngs.GetBoolean(ref vDoMeshedProngs);
		double vNudge = 0.0;
		InNudge.GetDouble(ref vNudge);
		double vFilletPer = 0.0;
		InFillet.GetDoublePercentage(ref vFilletPer);
		double vSizeMin = 0.0;
		InSizeMin.GetDouble(ref vSizeMin);
		double vSizeMax = 100.0;
		InSizeMax.GetDouble(ref vSizeMax);
		double vInsideSizeTop = 0.5;
		InInsideSizeTop.GetDouble(ref vInsideSizeTop);
		double vInsideSizeBase = 0.5;
		InInsideSizeBase.GetDouble(ref vInsideSizeBase);
		double vInsideHeight = 0.0;
		InInsideHeight.GetDouble(ref vInsideHeight);
		double value = 0.0;
		InInsideEdgeNudge.GetDouble(ref value);
		double vInsideVerticalLocation = 0.0;
		InInsideVerticalLocation.GetDouble(ref vInsideVerticalLocation);
		double vOutsideSizeTop = 0.5;
		InOutsideSizeTop.GetDouble(ref vOutsideSizeTop);
		double vOutsideSizeBase = 0.5;
		InOutsideSizeBase.GetDouble(ref vOutsideSizeBase);
		double vOutsideHeight = 0.0;
		InOutsideHeight.GetDouble(ref vOutsideHeight);
		double value2 = 0.0;
		InOutsideEdgeNudge.GetDouble(ref value2);
		double vOutsideVerticalLocation = 0.0;
		InOutsideVerticalLocation.GetDouble(ref vOutsideVerticalLocation);
		bool vDoBlendInside = false;
		InEnableBlendInside.GetBoolean(ref vDoBlendInside);
		bool vDoBlendOutside = false;
		InEnableBlendOutside.GetBoolean(ref vDoBlendOutside);
		double vBlendAmount = 0.5;
		InBlendAmount.GetDouble(ref vBlendAmount);
		double vBlendAmountTop = 1.0;
		InBlendAmountTop.GetDouble(ref vBlendAmountTop);
		double vBlendAmountBase = 1.0;
		InBlendAmountBase.GetDouble(ref vBlendAmountBase);
		double vBlendTilt = 0.0;
		InBlendTilt.GetDouble(ref vBlendTilt);
		double value3 = 0.2;
		InBlendAdjustment.GetDouble(ref value3);
		ConcurrentBag<CSData> prongs = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> profiles = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> rails = new ConcurrentBag<CSData>();
		_prongCache.Clear();
		_prongDomeCache.Clear();
		int gemCount = channelGemDatas.GetArrayCount();
		double edgeNudgeLeft = value;
		double edgeNudgeRight = value2;
		double prongSizeLeftTop = vInsideSizeTop;
		double prongSizeRightTop = vOutsideSizeTop;
		double prongSizeLeftBase = vInsideSizeBase;
		double prongSizeRightBase = vOutsideSizeBase;
		double heightLeft = vInsideHeight;
		double heightRight = vOutsideHeight;
		double vOffsetLeft = vInsideVerticalLocation;
		double vOffsetRight = vOutsideVerticalLocation;
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(channelGemDatas, 0);
		Point3d val2 = channelGemData.Left;
		double num = ((Point3d)(ref val2)).DistanceTo(centerGemData.Center);
		val2 = channelGemData.Right;
		double num2 = ((Point3d)(ref val2)).DistanceTo(centerGemData.Center);
		bool leftOutside = false;
		if (num > num2)
		{
			leftOutside = true;
			edgeNudgeLeft = value2;
			edgeNudgeRight = value;
			prongSizeLeftTop = vOutsideSizeTop;
			prongSizeRightTop = vInsideSizeTop;
			prongSizeLeftBase = vOutsideSizeBase;
			prongSizeRightBase = vInsideSizeBase;
			heightLeft = vOutsideHeight;
			heightRight = vInsideHeight;
			vOffsetLeft = vOutsideVerticalLocation;
			vOffsetRight = vInsideVerticalLocation;
		}
		bool rightOutside = !leftOutside;
		OnCurve inRail = CSUtilGemRails.Rail(channelGemDatas);
		if (!CSUtilGemRails.CalculateLeft(channelGemDatas.Array, inRail, 0.0, 0.0, out Curve crvLeft))
		{
			return UpdateError("CalculateLeft failed");
		}
		if (!CSUtilGemRails.CalculateRight(channelGemDatas.Array, inRail, 0.0, 0.0, out Curve crvRight))
		{
			return UpdateError("CalculateRight failed");
		}
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		Parallel.For(0, gemCount, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Expected O, but got Unknown
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Expected O, but got Unknown
			//IL_027c: Expected O, but got Unknown
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_043d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0553: Expected O, but got Unknown
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Expected O, but got Unknown
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Unknown result type (might be due to invalid IL or missing references)
			//IL_0572: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Expected O, but got Unknown
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058e: Expected O, but got Unknown
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Expected O, but got Unknown
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Expected O, but got Unknown
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Expected O, but got Unknown
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Expected O, but got Unknown
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cf: Expected O, but got Unknown
			//IL_06df: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0705: Expected O, but got Unknown
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_0707: Expected O, but got Unknown
			//IL_071c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Expected O, but got Unknown
			//IL_073c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0746: Expected O, but got Unknown
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Expected O, but got Unknown
			//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e81: Expected O, but got Unknown
			//IL_0e7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e83: Expected O, but got Unknown
			//IL_0e98: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea2: Expected O, but got Unknown
			//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ec2: Expected O, but got Unknown
			//IL_0ec3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eca: Expected O, but got Unknown
			//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa0: Expected O, but got Unknown
			//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad6: Expected O, but got Unknown
			//IL_0ad1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad8: Expected O, but got Unknown
			//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af7: Expected O, but got Unknown
			//IL_0b0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b17: Expected O, but got Unknown
			//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b1f: Expected O, but got Unknown
			//IL_093c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0943: Expected O, but got Unknown
			//IL_097a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0999: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_10c3: Expected O, but got Unknown
			//IL_10fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_1119: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_083c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d14: Expected O, but got Unknown
			//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_11db: Unknown result type (might be due to invalid IL or missing references)
			//IL_11eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_120a: Unknown result type (might be due to invalid IL or missing references)
			//IL_121a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1226: Unknown result type (might be due to invalid IL or missing references)
			//IL_1230: Expected O, but got Unknown
			//IL_122b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1232: Expected O, but got Unknown
			//IL_1247: Unknown result type (might be due to invalid IL or missing references)
			//IL_1251: Expected O, but got Unknown
			//IL_1267: Unknown result type (might be due to invalid IL or missing references)
			//IL_1271: Expected O, but got Unknown
			//IL_1272: Unknown result type (might be due to invalid IL or missing references)
			//IL_1279: Expected O, but got Unknown
			//IL_146b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1472: Expected O, but got Unknown
			//IL_14a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_14c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_135c: Unknown result type (might be due to invalid IL or missing references)
			if (!base.Error && !loopState.ShouldExitCurrentIteration && !parallelOptions.CancellationToken.IsCancellationRequested && !loopState.IsStopped && !loopState.IsExceptional)
			{
				CSChannelGemData cSChannelGemData = null;
				bool num3 = CSUtilGemRails.IsClosed(channelGemDatas.Array);
				if (num3)
				{
					cSChannelGemData = CSUtilGemRails.GetChannelGemData(channelGemDatas, (i - 1 + gemCount) % gemCount);
				}
				else if (i - 1 >= 0)
				{
					cSChannelGemData = CSUtilGemRails.GetChannelGemData(channelGemDatas, i - 1);
				}
				CSChannelGemData cSChannelGemData2 = null;
				if (num3)
				{
					cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i % gemCount);
				}
				else if (i < channelGemDatas.GetArrayCount())
				{
					cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i);
				}
				CSChannelGemData cSChannelGemData3 = null;
				if (num3)
				{
					cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(channelGemDatas, (i + 1) % gemCount);
				}
				else if (i + 1 < channelGemDatas.GetArrayCount())
				{
					cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i + 1);
				}
				if (cSChannelGemData == null && cSChannelGemData3 != null && cSChannelGemData2 != null)
				{
					cSChannelGemData = new CSChannelGemData(cSChannelGemData3);
					cSChannelGemData.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
				}
				CSChannelGemData cSChannelGemData4 = null;
				if (num3)
				{
					cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(channelGemDatas, (i - 2 + gemCount) % gemCount);
				}
				else if (i - 2 >= 0)
				{
					cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i - 2);
				}
				if (cSChannelGemData2 == null && cSChannelGemData != null && cSChannelGemData4 != null)
				{
					cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
					cSChannelGemData2.ApplyTransform(cSChannelGemData.GetMirrorForwardBack());
				}
				if (cSChannelGemData2 != null && cSChannelGemData != null)
				{
					CSDataHelper.SetSettingType(cSChannelGemData2, GemSettingType.PR);
					On3dPoint val3 = (cSChannelGemData2.Center.Convert() + (IOn3dPoint)(object)cSChannelGemData.Center.Convert()) / 2.0;
					OnCircle circleEdgeRight;
					if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)crvLeft.Convert(), edgeNudgeLeft, out var circleEdgeLeft))
					{
						UpdateError("CalculateCircleEdgeLeft failed");
						loopState.Stop();
					}
					else if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)crvRight.Convert(), edgeNudgeRight, out circleEdgeRight))
					{
						UpdateError("CalculateCircleEdgeRight failed");
						loopState.Stop();
					}
					else
					{
						OnCircle val4 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - vNudge);
						OnCircle c = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - vNudge);
						OnCircle val5 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val4, (IOnCircle)(object)c, (IOnCircle)(object)circleEdgeLeft);
						OnCircle val6 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val4, (IOnCircle)(object)c, (IOnCircle)(object)circleEdgeRight);
						On3dVector val7 = cSChannelGemData2.Normal.Convert() + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
						val7.Unitize();
						double num4 = 0.0;
						val5.ClosestPointTo((IOn3dPoint)(object)val3, ref num4);
						val5.Rotate(num4 + OnUtil.On_PI / 2.0, val5.Normal());
						val5.Translate((IOn3dVector)(object)(val7 * (vOffsetLeft - heightLeft)));
						double num5 = 0.0;
						val6.ClosestPointTo((IOn3dPoint)(object)val3, ref num5);
						val6.Rotate(num5 + OnUtil.On_PI / 2.0, val6.Normal());
						val6.Translate((IOn3dVector)(object)(val7 * (vOffsetRight - heightRight)));
						(cSChannelGemData2.Normal.Convert() + (IOn3dVector)(object)cSChannelGemData.Normal.Convert()).Unitize();
						On3dPoint val8;
						On3dPoint val9;
						if (leftOutside)
						{
							val8 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitLeftVector.Convert() * (vOutsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge));
							val9 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitRightVector.Convert() * (vInsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge));
						}
						else
						{
							val8 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitRightVector.Convert() * (vOutsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge));
							val9 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitLeftVector.Convert() * (vInsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge));
						}
						OnCircle val10;
						OnCircle val11;
						if (OnUtil.ON_CrossProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (IOn3dVector)(object)cSChannelGemData2.Normal.Convert() > 0.0)
						{
							val10 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val8, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), vOutsideSizeTop / 2.0);
							val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)(-cSChannelGemData2.UnitForwardVector.Convert()), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), vInsideSizeTop / 2.0);
						}
						else
						{
							val10 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val8, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), vOutsideSizeTop / 2.0);
							val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)(-cSChannelGemData2.UnitForwardVector.Convert()), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), vInsideSizeTop / 2.0);
						}
						val10.Translate((IOn3dVector)(object)(cSChannelGemData2.Normal.Convert() * (vOutsideVerticalLocation - vOutsideHeight)));
						val11.Translate((IOn3dVector)(object)(cSChannelGemData2.Normal.Convert() * (vInsideVerticalLocation - vInsideHeight)));
						if (vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10)
						{
							double num6 = val5.radius * 2.0;
							double num7 = prongSizeLeftBase * (num6 / prongSizeLeftTop);
							if (num6 > vSizeMin && num6 < vSizeMax)
							{
								if ((!leftOutside && vDoBlendInside) || (leftOutside && vDoBlendOutside))
								{
									On3dVector val12 = new On3dPoint(val5.Center()) - (IOn3dPoint)(object)centerGemData.Center.Convert();
									val12.Unitize();
									OnPlaneSurface val13 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)centerGemData.Center.Convert(), (IOn3dVector)(object)centerGemData.Normal.Convert(), (IOn3dVector)(object)val12));
									val13.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
									val13.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
									ArrayOnX_EVENT val14 = new ArrayOnX_EVENT();
									((OnCurve)blendToCurve).IntersectSurface((IOnSurface)(object)val13, ref val14);
									if (val14.Count() <= 0)
									{
										UpdateError("Intersection failed");
										loopState.Stop();
										return;
									}
									double num8 = -1.23432101234321E+308;
									double num9 = -1.23432101234321E+308;
									for (int j = 0; j < val14.Count(); j++)
									{
										double num10 = val14[j][0];
										double num11 = ((IOn3dPoint)((OnCurve)blendToCurve).PointAt(num10)).DistanceTo((IOn3dPoint)(object)centerGemData.Center.Convert());
										if (!RhinoMath.IsValidDouble(num8) || !RhinoMath.IsValidDouble(num9) || !(num11 >= num9))
										{
											num8 = num10;
											num9 = num11;
										}
									}
									On3dPoint val15 = ((OnCurve)blendToCurve).PointAt(num8);
									On3dVector val16 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)blendToCurve, (IOn3dPoint)(object)val15);
									val16.Unitize();
									if (OnUtil.ON_DotProduct((IOn3dVector)(object)(cSChannelGemData.ForwardVector.Convert() + (IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert()), (IOn3dVector)(object)val16) < 0.0)
									{
										val16.Reverse();
									}
									if (!BuildBlendedProng(centerGemData, (IOnNurbsCurve)(object)vProfile, num6, num7, heightLeft, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, (IOn3dPoint)(object)val15, (IOn3dVector)(object)val16, (IOnCircle)(object)val5, prongs, profiles, rails, out var msg))
									{
										UpdateError("BuildBlendProng:  " + msg);
										loopState.Stop();
										return;
									}
								}
								else
								{
									double taperPer = num7 / num6 - 1.0;
									if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, num6, heightRight, vFilletPer, taperPer, vDoMeshedProngs, out var prong, out var prongBaseRail))
									{
										UpdateError("Create prong cache failed");
										loopState.Stop();
										return;
									}
									OnXform val17 = new OnXform();
									val17.ChangeBasis((IOnPlane)(object)val5.plane, OnUtil.On_xy_plane);
									prong.Transform((IOnXform)(object)val17);
									prongBaseRail.Transform((IOnXform)(object)val17);
									prongs.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val17.Convert(), applyTransform: false));
									profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val17.Convert(), applyTransform: false));
								}
							}
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10)
						{
							double num12 = val6.radius * 2.0;
							double num13 = prongSizeRightBase * (num12 / prongSizeRightTop);
							if (num12 > vSizeMin && num12 < vSizeMax)
							{
								if ((!leftOutside && vDoBlendOutside) || (leftOutside && vDoBlendInside))
								{
									On3dVector val18 = new On3dPoint(val6.Center()) - (IOn3dPoint)(object)centerGemData.Center.Convert();
									val18.Unitize();
									OnPlaneSurface val19 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)centerGemData.Center.Convert(), (IOn3dVector)(object)centerGemData.Normal.Convert(), (IOn3dVector)(object)val18));
									val19.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
									val19.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
									ArrayOnX_EVENT val20 = new ArrayOnX_EVENT();
									((OnCurve)blendToCurve).IntersectSurface((IOnSurface)(object)val19, ref val20);
									if (val20.Count() <= 0)
									{
										UpdateError("Intersection failed");
										loopState.Stop();
										return;
									}
									double num14 = -1.23432101234321E+308;
									double num15 = -1.23432101234321E+308;
									for (int k = 0; k < val20.Count(); k++)
									{
										double num16 = val20[k][0];
										double num17 = ((IOn3dPoint)((OnCurve)blendToCurve).PointAt(num16)).DistanceTo((IOn3dPoint)(object)centerGemData.Center.Convert());
										if (!RhinoMath.IsValidDouble(num14) || !RhinoMath.IsValidDouble(num15) || !(num17 >= num15))
										{
											num14 = num16;
											num15 = num17;
										}
									}
									On3dPoint val21 = ((OnCurve)blendToCurve).PointAt(num14);
									On3dVector val22 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)blendToCurve, (IOn3dPoint)(object)val21);
									val22.Unitize();
									if (OnUtil.ON_DotProduct((IOn3dVector)(object)(cSChannelGemData.ForwardVector.Convert() + (IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert()), (IOn3dVector)(object)val22) < 0.0)
									{
										val22.Reverse();
									}
									if (!BuildBlendedProng(centerGemData, (IOnNurbsCurve)(object)vProfile, num12, num13, heightRight, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, (IOn3dPoint)(object)val21, (IOn3dVector)(object)val22, (IOnCircle)(object)val6, prongs, profiles, rails, out var msg2))
									{
										UpdateError("BuildBlendProng:  " + msg2);
										loopState.Stop();
										return;
									}
								}
								else
								{
									double taperPer2 = num13 / num12 - 1.0;
									if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, num12, heightRight, vFilletPer, taperPer2, vDoMeshedProngs, out var prong2, out var prongBaseRail2))
									{
										UpdateError("Create prong cache failed");
										loopState.Stop();
										return;
									}
									OnXform val23 = new OnXform();
									val23.ChangeBasis((IOnPlane)(object)val6.plane, OnUtil.On_xy_plane);
									prong2.Transform((IOnXform)(object)val23);
									prongBaseRail2.Transform((IOnXform)(object)val23);
									prongs.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val23.Convert(), applyTransform: false));
									profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val23.Convert(), applyTransform: false));
								}
							}
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle2) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle3) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle8) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (vDoBlendOutside)
							{
								On3dVector val24 = cSChannelGemData2.Center.Convert() - (IOn3dPoint)(object)centerGemData.Center.Convert();
								val24.Unitize();
								OnPlaneSurface val25 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)centerGemData.Center.Convert(), (IOn3dVector)(object)centerGemData.Normal.Convert(), (IOn3dVector)(object)val24));
								val25.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
								val25.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
								ArrayOnX_EVENT val26 = new ArrayOnX_EVENT();
								((OnCurve)blendToCurve).IntersectSurface((IOnSurface)(object)val25, ref val26);
								if (val26.Count() <= 0)
								{
									UpdateError("Intersection failed");
									loopState.Stop();
									return;
								}
								double num18 = -1.23432101234321E+308;
								double num19 = -1.23432101234321E+308;
								for (int l = 0; l < val26.Count(); l++)
								{
									double num20 = val26[l][0];
									double num21 = ((IOn3dPoint)((OnCurve)blendToCurve).PointAt(num20)).DistanceTo((IOn3dPoint)(object)centerGemData.Center.Convert());
									if (!RhinoMath.IsValidDouble(num18) || !RhinoMath.IsValidDouble(num19) || !(num21 >= num19))
									{
										num18 = num20;
										num19 = num21;
									}
								}
								On3dPoint val27 = ((OnCurve)blendToCurve).PointAt(num18);
								On3dVector val28 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)blendToCurve, (IOn3dPoint)(object)val27);
								val28.Unitize();
								if (OnUtil.ON_DotProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)val28) < 0.0)
								{
									val28.Reverse();
								}
								if (!BuildBlendedProng(centerGemData, (IOnNurbsCurve)(object)vProfile, vOutsideSizeTop, vOutsideSizeBase, vOutsideHeight, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, (IOn3dPoint)(object)val27, (IOn3dVector)(object)val28, (IOnCircle)(object)val10, prongs, profiles, rails, out var msg3))
								{
									UpdateError("BuildBlendProng:  " + msg3);
									loopState.Stop();
									return;
								}
							}
							else
							{
								double taperPer3 = vOutsideSizeBase / vOutsideSizeTop - 1.0;
								if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, vOutsideSizeTop, vOutsideHeight, vFilletPer, taperPer3, vDoMeshedProngs, out var prong3, out var prongBaseRail3))
								{
									UpdateError("Create prong cache failed");
									loopState.Stop();
									return;
								}
								OnXform val29 = new OnXform();
								val29.ChangeBasis((IOnPlane)(object)val10.plane, OnUtil.On_xy_plane);
								prong3.Transform((IOnXform)(object)val29);
								prongBaseRail3.Transform((IOnXform)(object)val29);
								prongs.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val29.Convert(), applyTransform: false));
								profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail3).Convert(), val29.Convert(), applyTransform: false));
							}
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle2) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle3) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle8) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (vDoBlendInside)
							{
								On3dVector val30 = cSChannelGemData2.Center.Convert() - (IOn3dPoint)(object)centerGemData.Center.Convert();
								val30.Unitize();
								OnPlaneSurface val31 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)centerGemData.Center.Convert(), (IOn3dVector)(object)centerGemData.Normal.Convert(), (IOn3dVector)(object)val30));
								val31.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
								val31.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
								ArrayOnX_EVENT val32 = new ArrayOnX_EVENT();
								((OnCurve)blendToCurve).IntersectSurface((IOnSurface)(object)val31, ref val32);
								if (val32.Count() <= 0)
								{
									UpdateError("Intersection failed");
									loopState.Stop();
									return;
								}
								double num22 = -1.23432101234321E+308;
								double num23 = -1.23432101234321E+308;
								for (int m = 0; m < val32.Count(); m++)
								{
									double num24 = val32[m][0];
									double num25 = ((IOn3dPoint)((OnCurve)blendToCurve).PointAt(num24)).DistanceTo((IOn3dPoint)(object)centerGemData.Center.Convert());
									if (!RhinoMath.IsValidDouble(num22) || !RhinoMath.IsValidDouble(num23) || !(num25 >= num23))
									{
										num22 = num24;
										num23 = num25;
									}
								}
								On3dPoint val33 = ((OnCurve)blendToCurve).PointAt(num22);
								On3dVector val34 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)blendToCurve, (IOn3dPoint)(object)val33);
								val34.Unitize();
								if (OnUtil.ON_DotProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)val34) < 0.0)
								{
									val34.Reverse();
								}
								if (!BuildBlendedProng(centerGemData, (IOnNurbsCurve)(object)vProfile, vInsideSizeTop, vInsideSizeBase, vInsideHeight, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, (IOn3dPoint)(object)val33, (IOn3dVector)(object)val34, (IOnCircle)(object)val11, prongs, profiles, rails, out var msg4))
								{
									UpdateError("BuildBlendProng:  " + msg4);
									loopState.Stop();
									return;
								}
							}
							else
							{
								double taperPer4 = vInsideSizeBase / vInsideSizeTop - 1.0;
								if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, vInsideSizeTop, vInsideHeight, vFilletPer, taperPer4, vDoMeshedProngs, out var prong4, out var prongBaseRail4))
								{
									UpdateError("Create prong cache failed");
									loopState.Stop();
									return;
								}
								OnXform val35 = new OnXform();
								val35.ChangeBasis((IOnPlane)(object)val11.plane, OnUtil.On_xy_plane);
								prong4.Transform((IOnXform)(object)val35);
								prongBaseRail4.Transform((IOnXform)(object)val35);
								prongs.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val35.Convert(), applyTransform: false));
								profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val35.Convert(), applyTransform: false));
							}
						}
						CS.CSUtil.CheckAndTerminateOrphanedParallelTasks();
					}
				}
			}
		});
		if (base.Error)
		{
			return true;
		}
		OutProngs.SetData(new CSDataArray(prongs));
		OutProfiles.SetData(new CSDataArray(profiles));
		OutRails.SetData(new CSDataArray(rails));
		return base.Update();
	}

	private bool UpdateSync()
	{
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Expected O, but got Unknown
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_05fb: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Expected O, but got Unknown
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Expected O, but got Unknown
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08de: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Expected O, but got Unknown
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Expected O, but got Unknown
		//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Expected O, but got Unknown
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Expected O, but got Unknown
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a09: Expected O, but got Unknown
		//IL_0a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Expected O, but got Unknown
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Expected O, but got Unknown
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f91: Expected O, but got Unknown
		//IL_0f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f93: Expected O, but got Unknown
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Expected O, but got Unknown
		//IL_0fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd2: Expected O, but got Unknown
		//IL_0fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fda: Expected O, but got Unknown
		//IL_0c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca2: Expected O, but got Unknown
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cce: Expected O, but got Unknown
		//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd0: Expected O, but got Unknown
		//IL_0ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cef: Expected O, but got Unknown
		//IL_0d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0f: Expected O, but got Unknown
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Expected O, but got Unknown
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1133: Unknown result type (might be due to invalid IL or missing references)
		//IL_113a: Expected O, but got Unknown
		//IL_116d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e88: Expected O, but got Unknown
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1030: Unknown result type (might be due to invalid IL or missing references)
		//IL_1099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1208: Unknown result type (might be due to invalid IL or missing references)
		//IL_1222: Unknown result type (might be due to invalid IL or missing references)
		//IL_122d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1239: Unknown result type (might be due to invalid IL or missing references)
		//IL_1243: Expected O, but got Unknown
		//IL_123e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Expected O, but got Unknown
		//IL_125a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1264: Expected O, but got Unknown
		//IL_127a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1284: Expected O, but got Unknown
		//IL_1285: Unknown result type (might be due to invalid IL or missing references)
		//IL_128c: Expected O, but got Unknown
		//IL_13e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ec: Expected O, but got Unknown
		//IL_141f: Unknown result type (might be due to invalid IL or missing references)
		//IL_143a: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_134b: Unknown result type (might be due to invalid IL or missing references)
		if (!(InCenterGem.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Center Gem");
		}
		if (!(InClusterGems.Data is CSDataArray cSDataArray) || cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
		}
		OnNurbsCurve nurbsCurve = InCurve.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsCurve val = InProfile.GetNurbsCurve();
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
		ValueListItem whichItem = ValueListItem.ClusterProngStyle1;
		InStyle.GetValueListItem(ref whichItem);
		bool value = true;
		InDoMeshProngs.GetBoolean(ref value);
		double value2 = 0.0;
		InNudge.GetDouble(ref value2);
		double value3 = 0.0;
		InFillet.GetDoublePercentage(ref value3);
		double value4 = 0.0;
		InSizeMin.GetDouble(ref value4);
		double value5 = 100.0;
		InSizeMax.GetDouble(ref value5);
		double value6 = 0.5;
		InInsideSizeTop.GetDouble(ref value6);
		double value7 = 0.5;
		InInsideSizeBase.GetDouble(ref value7);
		double value8 = 0.0;
		InInsideHeight.GetDouble(ref value8);
		double value9 = 0.0;
		InInsideEdgeNudge.GetDouble(ref value9);
		double value10 = 0.0;
		InInsideVerticalLocation.GetDouble(ref value10);
		double value11 = 0.5;
		InOutsideSizeTop.GetDouble(ref value11);
		double value12 = 0.5;
		InOutsideSizeBase.GetDouble(ref value12);
		double value13 = 0.0;
		InOutsideHeight.GetDouble(ref value13);
		double value14 = 0.0;
		InOutsideEdgeNudge.GetDouble(ref value14);
		double value15 = 0.0;
		InOutsideVerticalLocation.GetDouble(ref value15);
		bool value16 = false;
		InEnableBlendInside.GetBoolean(ref value16);
		bool value17 = false;
		InEnableBlendOutside.GetBoolean(ref value17);
		double value18 = 0.5;
		InBlendAmount.GetDouble(ref value18);
		double value19 = 1.0;
		InBlendAmountTop.GetDouble(ref value19);
		double value20 = 1.0;
		InBlendAmountBase.GetDouble(ref value20);
		double value21 = 0.0;
		InBlendTilt.GetDouble(ref value21);
		double value22 = 0.2;
		InBlendAdjustment.GetDouble(ref value22);
		ConcurrentBag<CSData> concurrentBag = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> concurrentBag2 = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> concurrentBag3 = new ConcurrentBag<CSData>();
		_prongCache.Clear();
		_prongDomeCache.Clear();
		int arrayCount = cSDataArray.GetArrayCount();
		double edgeNudge = value9;
		double edgeNudge2 = value14;
		double num = value6;
		double num2 = value11;
		double num3 = value7;
		double num4 = value12;
		double num5 = value8;
		double num6 = value13;
		double num7 = value10;
		double num8 = value15;
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		Point3d val3 = channelGemData.Left;
		double num9 = ((Point3d)(ref val3)).DistanceTo(cSGemData.Center);
		val3 = channelGemData.Right;
		double num10 = ((Point3d)(ref val3)).DistanceTo(cSGemData.Center);
		bool flag = false;
		if (num9 > num10)
		{
			flag = true;
			edgeNudge = value14;
			edgeNudge2 = value9;
			num = value11;
			num2 = value6;
			num3 = value12;
			num4 = value7;
			num5 = value13;
			num6 = value8;
			num7 = value15;
			num8 = value10;
		}
		bool flag2 = !flag;
		OnCurve inRail = CSUtilGemRails.Rail(cSDataArray);
		if (!CSUtilGemRails.CalculateLeft(cSDataArray.Array, inRail, 0.0, 0.0, out Curve outRail))
		{
			return UpdateError("CalculateLeft failed");
		}
		if (!CSUtilGemRails.CalculateRight(cSDataArray.Array, inRail, 0.0, 0.0, out Curve outRail2))
		{
			return UpdateError("CalculateRight failed");
		}
		for (int i = 0; i < arrayCount; i++)
		{
			CSChannelGemData cSChannelGemData = null;
			bool num11 = CSUtilGemRails.IsClosed(cSDataArray.Array);
			if (num11)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (num11)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < cSDataArray.GetArrayCount())
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (num11)
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
			if (num11)
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
			On3dPoint val4 = (cSChannelGemData2.Center.Convert() + (IOn3dPoint)(object)cSChannelGemData.Center.Convert()) / 2.0;
			if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)outRail.Convert(), edgeNudge, out var circleEdgeLeft))
			{
				return UpdateError("CalculateCircleEdgeLeft failed");
			}
			if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)outRail2.Convert(), edgeNudge2, out var circleEdgeRight))
			{
				return UpdateError("CalculateCircleEdgeRight failed");
			}
			OnCircle val5 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value2);
			OnCircle c = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value2);
			OnCircle val6 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val5, (IOnCircle)(object)c, (IOnCircle)(object)circleEdgeLeft);
			OnCircle val7 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val5, (IOnCircle)(object)c, (IOnCircle)(object)circleEdgeRight);
			On3dVector val8 = cSChannelGemData2.Normal.Convert() + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
			val8.Unitize();
			double num12 = 0.0;
			val6.ClosestPointTo((IOn3dPoint)(object)val4, ref num12);
			val6.Rotate(num12 + OnUtil.On_PI / 2.0, val6.Normal());
			val6.Translate((IOn3dVector)(object)(val8 * (num7 - num5)));
			double num13 = 0.0;
			val7.ClosestPointTo((IOn3dPoint)(object)val4, ref num13);
			val7.Rotate(num13 + OnUtil.On_PI / 2.0, val7.Normal());
			val7.Translate((IOn3dVector)(object)(val8 * (num8 - num6)));
			(cSChannelGemData2.Normal.Convert() + (IOn3dVector)(object)cSChannelGemData.Normal.Convert()).Unitize();
			On3dPoint val9;
			On3dPoint val10;
			if (flag)
			{
				val9 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitLeftVector.Convert() * (value11 / 2.0 + cSChannelGemData2.Size - value2));
				val10 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitRightVector.Convert() * (value6 / 2.0 + cSChannelGemData2.Size - value2));
			}
			else
			{
				val9 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitRightVector.Convert() * (value11 / 2.0 + cSChannelGemData2.Size - value2));
				val10 = cSChannelGemData2.Center.Convert() + (IOn3dVector)(object)(cSChannelGemData2.UnitLeftVector.Convert() * (value6 / 2.0 + cSChannelGemData2.Size - value2));
			}
			OnCircle val11;
			OnCircle val12;
			if (OnUtil.ON_CrossProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (IOn3dVector)(object)cSChannelGemData2.Normal.Convert() > 0.0)
			{
				val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value11 / 2.0);
				val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val10, (IOn3dVector)(object)(-cSChannelGemData2.UnitForwardVector.Convert()), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value6 / 2.0);
			}
			else
			{
				val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value11 / 2.0);
				val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val10, (IOn3dVector)(object)(-cSChannelGemData2.UnitForwardVector.Convert()), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value6 / 2.0);
			}
			val11.Translate((IOn3dVector)(object)(cSChannelGemData2.Normal.Convert() * (value15 - value13)));
			val12.Translate((IOn3dVector)(object)(cSChannelGemData2.Normal.Convert() * (value10 - value8)));
			if (whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag2 && whichItem == ValueListItem.ClusterProngStyle6) || (flag && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10)
			{
				double num14 = val6.radius * 2.0;
				double num15 = num3 * (num14 / num);
				if (num14 > value4 && num14 < value5)
				{
					if ((!flag && value16) || (flag && value17))
					{
						On3dVector val13 = new On3dPoint(val6.Center()) - (IOn3dPoint)(object)cSGemData.Center.Convert();
						val13.Unitize();
						OnPlaneSurface val14 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)cSGemData.Center.Convert(), (IOn3dVector)(object)cSGemData.Normal.Convert(), (IOn3dVector)(object)val13));
						val14.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
						val14.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
						ArrayOnX_EVENT val15 = new ArrayOnX_EVENT();
						((OnCurve)nurbsCurve).IntersectSurface((IOnSurface)(object)val14, ref val15);
						if (val15.Count() <= 0)
						{
							return UpdateError("Intersection failed");
						}
						double num16 = -1.23432101234321E+308;
						double num17 = -1.23432101234321E+308;
						for (int j = 0; j < val15.Count(); j++)
						{
							double num18 = val15[j][0];
							double num19 = ((IOn3dPoint)((OnCurve)nurbsCurve).PointAt(num18)).DistanceTo((IOn3dPoint)(object)cSGemData.Center.Convert());
							if (!RhinoMath.IsValidDouble(num16) || !RhinoMath.IsValidDouble(num17) || !(num19 >= num17))
							{
								num16 = num18;
								num17 = num19;
							}
						}
						On3dPoint val16 = ((OnCurve)nurbsCurve).PointAt(num16);
						On3dVector val17 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)nurbsCurve, (IOn3dPoint)(object)val16);
						val17.Unitize();
						if (OnUtil.ON_DotProduct((IOn3dVector)(object)(cSChannelGemData.ForwardVector.Convert() + (IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert()), (IOn3dVector)(object)val17) < 0.0)
						{
							val17.Reverse();
						}
						if (!BuildBlendedProng(cSGemData, (IOnNurbsCurve)(object)val, num14, num15, num5, value3, value21, value18, value19, value20, (IOn3dPoint)(object)val16, (IOn3dVector)(object)val17, (IOnCircle)(object)val6, concurrentBag, concurrentBag2, concurrentBag3, out var msg))
						{
							return UpdateError("BuildBlendProng:  " + msg);
						}
					}
					else
					{
						double taperPer = num15 / num14 - 1.0;
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, num14, num6, value3, taperPer, value, out var prong, out var prongBaseRail))
						{
							return UpdateError("Create prong cache failed");
						}
						OnXform val18 = new OnXform();
						val18.ChangeBasis((IOnPlane)(object)val6.plane, OnUtil.On_xy_plane);
						prong.Transform((IOnXform)(object)val18);
						prongBaseRail.Transform((IOnXform)(object)val18);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val18.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val18.Convert(), applyTransform: false));
					}
				}
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag2 && whichItem == ValueListItem.ClusterProngStyle4) || (flag && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10)
			{
				double num20 = val7.radius * 2.0;
				double num21 = num4 * (num20 / num2);
				if (num20 > value4 && num20 < value5)
				{
					if ((!flag && value17) || (flag && value16))
					{
						On3dVector val19 = new On3dPoint(val7.Center()) - (IOn3dPoint)(object)cSGemData.Center.Convert();
						val19.Unitize();
						OnPlaneSurface val20 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)cSGemData.Center.Convert(), (IOn3dVector)(object)cSGemData.Normal.Convert(), (IOn3dVector)(object)val19));
						val20.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
						val20.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
						ArrayOnX_EVENT val21 = new ArrayOnX_EVENT();
						((OnCurve)nurbsCurve).IntersectSurface((IOnSurface)(object)val20, ref val21);
						if (val21.Count() <= 0)
						{
							return UpdateError("Intersection failed");
						}
						double num22 = -1.23432101234321E+308;
						double num23 = -1.23432101234321E+308;
						for (int k = 0; k < val21.Count(); k++)
						{
							double num24 = val21[k][0];
							double num25 = ((IOn3dPoint)((OnCurve)nurbsCurve).PointAt(num24)).DistanceTo((IOn3dPoint)(object)cSGemData.Center.Convert());
							if (!RhinoMath.IsValidDouble(num22) || !RhinoMath.IsValidDouble(num23) || !(num25 >= num23))
							{
								num22 = num24;
								num23 = num25;
							}
						}
						On3dPoint val22 = ((OnCurve)nurbsCurve).PointAt(num22);
						On3dVector val23 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)nurbsCurve, (IOn3dPoint)(object)val22);
						val23.Unitize();
						if (OnUtil.ON_DotProduct((IOn3dVector)(object)(cSChannelGemData.ForwardVector.Convert() + (IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert()), (IOn3dVector)(object)val23) < 0.0)
						{
							val23.Reverse();
						}
						if (!BuildBlendedProng(cSGemData, (IOnNurbsCurve)(object)val, num20, num21, num6, value3, value21, value18, value19, value20, (IOn3dPoint)(object)val22, (IOn3dVector)(object)val23, (IOnCircle)(object)val7, concurrentBag, concurrentBag2, concurrentBag3, out var msg2))
						{
							return UpdateError("BuildBlendProng:  " + msg2);
						}
					}
					else
					{
						double taperPer2 = num21 / num20 - 1.0;
						if (!_prongCache.Create((IOnNurbsCurve)(object)val, num20, num6, value3, taperPer2, value, out var prong2, out var prongBaseRail2))
						{
							return UpdateError("Create prong cache failed");
						}
						OnXform val24 = new OnXform();
						val24.ChangeBasis((IOnPlane)(object)val7.plane, OnUtil.On_xy_plane);
						prong2.Transform((IOnXform)(object)val24);
						prongBaseRail2.Transform((IOnXform)(object)val24);
						concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val24.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val24.Convert(), applyTransform: false));
					}
				}
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || (flag && whichItem == ValueListItem.ClusterProngStyle2) || (flag2 && whichItem == ValueListItem.ClusterProngStyle3) || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || (flag && whichItem == ValueListItem.ClusterProngStyle8) || (flag2 && whichItem == ValueListItem.ClusterProngStyle9))
			{
				if (value17)
				{
					On3dVector val25 = cSChannelGemData2.Center.Convert() - (IOn3dPoint)(object)cSGemData.Center.Convert();
					val25.Unitize();
					OnPlaneSurface val26 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)cSGemData.Center.Convert(), (IOn3dVector)(object)cSGemData.Normal.Convert(), (IOn3dVector)(object)val25));
					val26.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
					val26.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
					ArrayOnX_EVENT val27 = new ArrayOnX_EVENT();
					((OnCurve)nurbsCurve).IntersectSurface((IOnSurface)(object)val26, ref val27);
					if (val27.Count() <= 0)
					{
						return UpdateError("Intersection failed");
					}
					double num26 = -1.23432101234321E+308;
					double num27 = -1.23432101234321E+308;
					for (int l = 0; l < val27.Count(); l++)
					{
						double num28 = val27[l][0];
						double num29 = ((IOn3dPoint)((OnCurve)nurbsCurve).PointAt(num28)).DistanceTo((IOn3dPoint)(object)cSGemData.Center.Convert());
						if (!RhinoMath.IsValidDouble(num26) || !RhinoMath.IsValidDouble(num27) || !(num29 >= num27))
						{
							num26 = num28;
							num27 = num29;
						}
					}
					On3dPoint val28 = ((OnCurve)nurbsCurve).PointAt(num26);
					On3dVector val29 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)nurbsCurve, (IOn3dPoint)(object)val28);
					val29.Unitize();
					if (OnUtil.ON_DotProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)val29) < 0.0)
					{
						val29.Reverse();
					}
					if (!BuildBlendedProng(cSGemData, (IOnNurbsCurve)(object)val, value11, value12, value13, value3, value21, value18, value19, value20, (IOn3dPoint)(object)val28, (IOn3dVector)(object)val29, (IOnCircle)(object)val11, concurrentBag, concurrentBag2, concurrentBag3, out var msg3))
					{
						return UpdateError("BuildBlendProng:  " + msg3);
					}
				}
				else
				{
					double taperPer3 = value12 / value11 - 1.0;
					if (!_prongCache.Create((IOnNurbsCurve)(object)val, value11, value13, value3, taperPer3, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					OnXform val30 = new OnXform();
					val30.ChangeBasis((IOnPlane)(object)val11.plane, OnUtil.On_xy_plane);
					prong3.Transform((IOnXform)(object)val30);
					prongBaseRail3.Transform((IOnXform)(object)val30);
					concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val30.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail3).Convert(), val30.Convert(), applyTransform: false));
				}
			}
			if (whichItem != ValueListItem.ClusterProngStyle1 && (!flag2 || whichItem != ValueListItem.ClusterProngStyle2) && (!flag || whichItem != ValueListItem.ClusterProngStyle3) && (!flag2 || whichItem != ValueListItem.ClusterProngStyle4) && (!flag || whichItem != ValueListItem.ClusterProngStyle5) && (!flag2 || whichItem != ValueListItem.ClusterProngStyle6) && (!flag || whichItem != ValueListItem.ClusterProngStyle7) && (!flag2 || whichItem != ValueListItem.ClusterProngStyle8) && (!flag || whichItem != ValueListItem.ClusterProngStyle9))
			{
				continue;
			}
			if (value16)
			{
				On3dVector val31 = cSChannelGemData2.Center.Convert() - (IOn3dPoint)(object)cSGemData.Center.Convert();
				val31.Unitize();
				OnPlaneSurface val32 = new OnPlaneSurface((IOnPlane)new OnPlane((IOn3dPoint)(object)cSGemData.Center.Convert(), (IOn3dVector)(object)cSGemData.Normal.Convert(), (IOn3dVector)(object)val31));
				val32.SetExtents(0, (IOnInterval)new OnInterval(-50.0, 50.0));
				val32.SetExtents(1, (IOnInterval)new OnInterval(0.0, 50.0));
				ArrayOnX_EVENT val33 = new ArrayOnX_EVENT();
				((OnCurve)nurbsCurve).IntersectSurface((IOnSurface)(object)val32, ref val33);
				if (val33.Count() <= 0)
				{
					return UpdateError("Intersection failed");
				}
				double num30 = -1.23432101234321E+308;
				double num31 = -1.23432101234321E+308;
				for (int m = 0; m < val33.Count(); m++)
				{
					double num32 = val33[m][0];
					double num33 = ((IOn3dPoint)((OnCurve)nurbsCurve).PointAt(num32)).DistanceTo((IOn3dPoint)(object)cSGemData.Center.Convert());
					if (!RhinoMath.IsValidDouble(num30) || !RhinoMath.IsValidDouble(num31) || !(num33 >= num31))
					{
						num30 = num32;
						num31 = num33;
					}
				}
				On3dPoint val34 = ((OnCurve)nurbsCurve).PointAt(num30);
				On3dVector val35 = CSBlockUtil.CSUtilMath.AverageTangentAt((IOnCurve)(object)nurbsCurve, (IOn3dPoint)(object)val34);
				val35.Unitize();
				if (OnUtil.ON_DotProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)val35) < 0.0)
				{
					val35.Reverse();
				}
				if (!BuildBlendedProng(cSGemData, (IOnNurbsCurve)(object)val, value6, value7, value8, value3, value21, value18, value19, value20, (IOn3dPoint)(object)val34, (IOn3dVector)(object)val35, (IOnCircle)(object)val12, concurrentBag, concurrentBag2, concurrentBag3, out var msg4))
				{
					return UpdateError("BuildBlendProng:  " + msg4);
				}
			}
			else
			{
				double taperPer4 = value7 / value6 - 1.0;
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, value6, value8, value3, taperPer4, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val36 = new OnXform();
				val36.ChangeBasis((IOnPlane)(object)val12.plane, OnUtil.On_xy_plane);
				prong4.Transform((IOnXform)(object)val36);
				prongBaseRail4.Transform((IOnXform)(object)val36);
				concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val36.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val36.Convert(), applyTransform: false));
			}
		}
		if (base.Error)
		{
			return true;
		}
		OutProngs.SetData(new CSDataArray(concurrentBag));
		OutProfiles.SetData(new CSDataArray(concurrentBag2));
		OutRails.SetData(new CSDataArray(concurrentBag3));
		return base.Update();
	}

	private bool BuildBlendedProng(CSGemData centerGem, IOnNurbsCurve prongProfile, double diameterTop, double diameterBase, double height, double filletPer, double blendTilt, double blendAmount, double blendAmountTop, double blendAmountBase, IOn3dPoint blendToPoint, IOn3dVector blendToPointTangent, IOnCircle circle, ConcurrentBag<CSData> prongs, ConcurrentBag<CSData> profiles, ConcurrentBag<CSData> rails, out string msg)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0073: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00bb: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		msg = string.Empty;
		double num = diameterTop / 2.0;
		double num2 = diameterBase / 2.0;
		double num3 = CalculateProngTipHeightToDome(diameterTop, height, filletPer);
		double num4 = (CSApplicationHelper.Application.IsCamMode ? CSApplicationHelper.Application.CamProngExtensionHeight : 0.0);
		OnCircle val = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)(new On3dPoint(circle.Center()) + (IOn3dVector)(object)(new On3dVector(circle.Normal()) * (num3 + num4))), circle.plane.xaxis, circle.plane.yaxis), num);
		OnCircle val2 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)(new On3dPoint(circle.Center()) + (IOn3dVector)(object)(new On3dVector(circle.Normal()) * num3)), circle.plane.xaxis, circle.plane.yaxis), num);
		OnCircle val3 = new OnCircle((IOnPlane)new OnPlane(blendToPoint, circle.plane.xaxis, circle.plane.yaxis), num2);
		On3dVector val4 = OnUtil.ON_CrossProduct((IOn3dVector)(object)(new On3dPoint(blendToPoint) - (IOn3dPoint)(object)centerGem.Center.Convert()), (IOn3dVector)(object)centerGem.Normal.Convert());
		val4.Unitize();
		val3.Rotate(CSBlockUtil.CSUtilMath.ToRadians(blendTilt), (IOn3dVector)(object)val4);
		if (!BuildSweepRail((IOnCircle)(object)val, (IOnCircle)(object)val2, (IOnCircle)(object)val3, blendAmount, blendAmountTop, blendAmountBase, blendTilt, blendToPointTangent, (IOn3dVector)(object)(new On3dVector(circle.plane.yaxis) * 1.0), out var sweepRail, out var msg2))
		{
			msg = "BuildSweepRail failed:  " + msg2;
			return false;
		}
		rails.Add(new CSOnGeometryData((GeometryBase)(object)sweepRail.Convert()));
		if (!BuildSweepRail((IOnCircle)(object)val, (IOnCircle)(object)val2, (IOnCircle)(object)val3, blendAmount, blendAmountTop, blendAmountBase, blendTilt, blendToPointTangent, (IOn3dVector)(object)(new On3dVector(circle.plane.yaxis) * -1.0), out var sweepRail2, out var msg3))
		{
			msg = "BuildSweepRail failed:  " + msg3;
			return false;
		}
		rails.Add(new CSOnGeometryData((GeometryBase)(object)sweepRail2.Convert()));
		OnXform val5 = new OnXform();
		val5.ChangeBasis((IOnPlane)(object)val.plane, OnUtil.On_xy_plane);
		OnXform val6 = new OnXform();
		val6.ChangeBasis((IOnPlane)(object)val2.plane, OnUtil.On_xy_plane);
		OnXform val7 = new OnXform();
		val7.ChangeBasis((IOnPlane)(object)val3.plane, OnUtil.On_xy_plane);
		if (!_prongDomeCache.Create(prongProfile, diameterTop, filletPer, 0.0, out var prongDome))
		{
			msg = "Create prong dome cache failed";
			return false;
		}
		prongDome.Transform((IOnXform)(object)(CSApplicationHelper.Application.IsCamMode ? val5 : val6));
		OnNurbsCurve val8 = BuildSweepProfile(prongProfile, diameterTop);
		val8.Transform((IOnXform)(object)val6);
		profiles.Add(new CSOnGeometryData((GeometryBase)(object)val8.Convert(), val6.Convert(), applyTransform: false));
		OnNurbsCurve val9 = BuildSweepProfile(prongProfile, diameterBase);
		val9.Transform((IOnXform)(object)val7);
		profiles.Add(new CSOnGeometryData((GeometryBase)(object)val9.Convert(), val7.Convert(), applyTransform: false));
		IOnCurve[] profiles2;
		if (!CSApplicationHelper.Application.IsCamMode)
		{
			profiles2 = (IOnCurve[])(object)new IOnCurve[2]
			{
				(IOnCurve)val8,
				(IOnCurve)val9
			};
		}
		else
		{
			OnNurbsCurve val10 = BuildSweepProfile(prongProfile, diameterTop);
			val10.Transform((IOnXform)(object)val5);
			profiles.Add(new CSOnGeometryData((GeometryBase)(object)val10.Convert(), val5.Convert(), applyTransform: false));
			profiles2 = (IOnCurve[])(object)new IOnCurve[3]
			{
				(IOnCurve)val10,
				(IOnCurve)val8,
				(IOnCurve)val9
			};
		}
		if (!CSUtilSweep.Sweep2NoParam((IOnCurve)(object)sweepRail, (IOnCurve)(object)sweepRail2, profiles2, out var breps, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			msg = "Sweep failed";
			return false;
		}
		if (breps.Length == 0)
		{
			msg = "Sweep did not produce any results";
			return false;
		}
		OnBrep val11 = breps[0];
		OnBrep val12 = CSUtilRhino.JoinAllBreps((OnBrep[])(object)new OnBrep[2]
		{
			((OnGeometry)prongDome).BrepForm(),
			val11
		}, CSApplicationHelper.Application.Tolerance);
		if (val12 == null)
		{
			msg = "RhinoJoinBreps failed";
			return false;
		}
		OnBrep brep = CSUtilRhino.CapPlanarHoles(val12, CSApplicationHelper.Application.Tolerance);
		if (brep == null)
		{
			msg = "RhinoBrepCapPlanarHoles failed";
			return false;
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref brep, doFullClean: true);
		prongs.Add(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
		return true;
	}

	private static OnNurbsCurve BuildSweepProfile(IOnNurbsCurve profile, double diameter)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		OnBoundingBox val = new OnBoundingBox();
		((IOnGeometry)profile).GetTightBoundingBox(ref val);
		double num = val.Max().x - val.Min().x;
		double num2 = val.Max().y - val.Min().y;
		OnXform val2 = new OnXform();
		val2.Scale(diameter / num, diameter / num2, 1.0);
		OnXform val3 = new OnXform();
		val3.Translation((IOn3dVector)(object)(new On3dPoint(OnUtil.On_origin) - (IOn3dPoint)(object)val.Center()));
		OnNurbsCurve obj = OnNurbsCurve.New(profile);
		obj.Transform((IOnXform)(object)(val2 * (IOnXform)(object)val3));
		return obj;
	}

	private static bool BuildSweepRail(IOnCircle circleTopExtra, IOnCircle circleTop, IOnCircle circleBase, double blendAmount, double blendAmountTop, double blendAmountBase, double blendTilt, IOn3dVector blendToPointTangent, IOn3dVector dir, out OnCurve sweepRail, out string msg)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_002e: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0050: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00d5: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f7: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		sweepRail = null;
		msg = string.Empty;
		On3dPoint pointStart = new On3dPoint(circleTop.Center()) + (IOn3dVector)(object)(new On3dVector(dir) * circleTop.radius);
		On3dPoint val = new On3dPoint(circleBase.Center()) + (IOn3dVector)(object)(new On3dVector(dir) * circleBase.radius);
		val.Rotate(CSBlockUtil.CSUtilMath.ToRadians(blendTilt), blendToPointTangent, circleBase.Center());
		On3dVector val2 = new On3dVector(circleTop.Normal());
		val2.Reverse();
		val2.Unitize();
		On3dVector val3 = new On3dVector(circleBase.Normal());
		val3.Unitize();
		OnNurbsCurve val4 = CSUtilCurves.BlendCurve((IOn3dPoint)(object)pointStart, (IOn3dPoint)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)val3, blendAmount, blendAmountTop, blendAmountBase);
		if (!CSApplicationHelper.Application.IsCamMode)
		{
			sweepRail = (OnCurve)(object)val4;
			return true;
		}
		On3dPoint obj = new On3dPoint(circleTopExtra.Center()) + (IOn3dVector)(object)(new On3dVector(dir) * circleTopExtra.radius);
		On3dPoint val5 = new On3dPoint(circleTop.Center()) + (IOn3dVector)(object)(new On3dVector(dir) * circleTop.radius);
		OnLineCurve val6 = new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val5);
		IOnCurve[] obj2 = new IOnCurve[2]
		{
			(IOnCurve)val6,
			(IOnCurve)val4
		};
		OnCurve[] array = null;
		if (!RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj2, ref array))
		{
			msg = "RhinoMergeCurves failed";
			return false;
		}
		if (array.Length == 0)
		{
			msg = "RhinoMergeCurves failed to produce results";
			return false;
		}
		sweepRail = array[0];
		return true;
	}

	private static double CalculateProngTipHeightToDome(double diameter, double height, double filletPer)
	{
		if (filletPer > 0.99)
		{
			filletPer = 0.99;
		}
		if (filletPer < 0.01)
		{
			filletPer = 0.01;
		}
		double num = diameter / 2.0 * filletPer;
		if (num > height)
		{
			height = num + 0.01;
		}
		return height - num;
	}
}
