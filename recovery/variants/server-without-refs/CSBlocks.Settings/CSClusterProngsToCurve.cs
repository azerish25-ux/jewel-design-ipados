using System.Collections.Concurrent;
using System.Threading.Tasks;
using CS;
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

namespace CSBlocks.Settings;

[CSBlock("ClusterProngsToCurve", "{96C893E9-1D11-429A-ABFF-F3B63B5BA947}")]
public sealed class CSClusterProngsToCurve : CSBlock
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

	public CSClusterProngsToCurve(string identifier)
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
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Expected O, but got Unknown
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Expected O, but got Unknown
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Expected O, but got Unknown
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Expected O, but got Unknown
			//IL_0274: Expected O, but got Unknown
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Expected O, but got Unknown
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Expected O, but got Unknown
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Expected O, but got Unknown
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Expected O, but got Unknown
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Expected O, but got Unknown
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Expected O, but got Unknown
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Expected O, but got Unknown
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Expected O, but got Unknown
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c7: Expected O, but got Unknown
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fd: Expected O, but got Unknown
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ff: Expected O, but got Unknown
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Expected O, but got Unknown
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_073e: Expected O, but got Unknown
			//IL_073f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0746: Expected O, but got Unknown
			//IL_0e24: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e79: Expected O, but got Unknown
			//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7b: Expected O, but got Unknown
			//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e9a: Expected O, but got Unknown
			//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eba: Expected O, but got Unknown
			//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ec2: Expected O, but got Unknown
			//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a98: Expected O, but got Unknown
			//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ace: Expected O, but got Unknown
			//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad0: Expected O, but got Unknown
			//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aef: Expected O, but got Unknown
			//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0f: Expected O, but got Unknown
			//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b17: Expected O, but got Unknown
			//IL_0934: Unknown result type (might be due to invalid IL or missing references)
			//IL_093b: Expected O, but got Unknown
			//IL_0972: Unknown result type (might be due to invalid IL or missing references)
			//IL_0991: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bb: Expected O, but got Unknown
			//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1111: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0829: Unknown result type (might be due to invalid IL or missing references)
			//IL_0834: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d05: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d0c: Expected O, but got Unknown
			//IL_0d43: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f33: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bfa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_11e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1202: Unknown result type (might be due to invalid IL or missing references)
			//IL_1212: Unknown result type (might be due to invalid IL or missing references)
			//IL_121e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1228: Expected O, but got Unknown
			//IL_1223: Unknown result type (might be due to invalid IL or missing references)
			//IL_122a: Expected O, but got Unknown
			//IL_123f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1249: Expected O, but got Unknown
			//IL_125f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1269: Expected O, but got Unknown
			//IL_126a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1271: Expected O, but got Unknown
			//IL_1463: Unknown result type (might be due to invalid IL or missing references)
			//IL_146a: Expected O, but got Unknown
			//IL_14a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_14c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1354: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05f2: Expected O, but got Unknown
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Expected O, but got Unknown
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Expected O, but got Unknown
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Expected O, but got Unknown
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Expected O, but got Unknown
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Expected O, but got Unknown
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Expected O, but got Unknown
		//IL_0f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f88: Expected O, but got Unknown
		//IL_0f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Expected O, but got Unknown
		//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa9: Expected O, but got Unknown
		//IL_0fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc9: Expected O, but got Unknown
		//IL_0fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd1: Expected O, but got Unknown
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c99: Expected O, but got Unknown
		//IL_0ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Expected O, but got Unknown
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Expected O, but got Unknown
		//IL_0cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce6: Expected O, but got Unknown
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Expected O, but got Unknown
		//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Expected O, but got Unknown
		//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Expected O, but got Unknown
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1131: Expected O, but got Unknown
		//IL_1164: Unknown result type (might be due to invalid IL or missing references)
		//IL_117f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7f: Expected O, but got Unknown
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1219: Unknown result type (might be due to invalid IL or missing references)
		//IL_1224: Unknown result type (might be due to invalid IL or missing references)
		//IL_1230: Unknown result type (might be due to invalid IL or missing references)
		//IL_123a: Expected O, but got Unknown
		//IL_1235: Unknown result type (might be due to invalid IL or missing references)
		//IL_123c: Expected O, but got Unknown
		//IL_1251: Unknown result type (might be due to invalid IL or missing references)
		//IL_125b: Expected O, but got Unknown
		//IL_1271: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Expected O, but got Unknown
		//IL_127c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1283: Expected O, but got Unknown
		//IL_13dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e3: Expected O, but got Unknown
		//IL_1416: Unknown result type (might be due to invalid IL or missing references)
		//IL_1431: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1342: Unknown result type (might be due to invalid IL or missing references)
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
