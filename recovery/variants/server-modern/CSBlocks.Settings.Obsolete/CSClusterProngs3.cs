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
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ClusterProngs_V3", "{4E10CCCB-F729-4420-8115-358DA72109B3}")]
public sealed class CSClusterProngs3 : CSBlock
{
	private readonly CSProngCache _prongCache = new CSProngCache();

	public CSInputChannel InCenterGem { get; }

	public CSInputChannel InClusterGems { get; }

	public CSInputChannel InDoMeshProngs { get; }

	public CSInputChannel InFillet { get; }

	public CSInputChannel InInsideEdgeNudge { get; }

	public CSInputChannel InInsideHeight { get; }

	public CSInputChannel InInsideSize { get; }

	public CSInputChannel InInsideVerticalLocation { get; }

	public CSInputChannel InNudge { get; }

	public CSInputChannel InOutsideEdgeNudge { get; }

	public CSInputChannel InOutsideHeight { get; }

	public CSInputChannel InOutsideSize { get; }

	public CSInputChannel InOutsideVerticalLocation { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InSizeMax { get; }

	public CSInputChannel InSizeMin { get; }

	public CSInputChannel InStyle { get; }

	public CSInputChannel InTaper { get; }

	public CSOutputChannel OutProfiles { get; }

	public CSOutputChannel OutProngs { get; }

	public CSClusterProngs3(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InClusterGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMin = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMax = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideEdgeNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInsideVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideEdgeNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOutsideVerticalLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InDoMeshProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		return UpdateSync();
	}

	private bool UpdateConcurrent()
	{
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		InitializeCancellationTokenSource();
		if (!(InCenterGem.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Center Gem");
		}
		CSDataArray channelGemDatas = InClusterGems.Data as CSDataArray;
		if (channelGemDatas == null || channelGemDatas.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
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
		double vTaperPer = 0.0;
		InTaper.GetDoublePercentage(ref vTaperPer);
		double vSizeMin = 0.0;
		InSizeMin.GetDouble(ref vSizeMin);
		double vSizeMax = 100.0;
		InSizeMax.GetDouble(ref vSizeMax);
		double vInsideSize = 0.5;
		InInsideSize.GetDouble(ref vInsideSize);
		double value = 0.0;
		InInsideEdgeNudge.GetDouble(ref value);
		double vInsideHeight = 0.0;
		InInsideHeight.GetDouble(ref vInsideHeight);
		double vInsideVerticalLocation = 0.0;
		InInsideVerticalLocation.GetDouble(ref vInsideVerticalLocation);
		double vOutsideSize = 0.5;
		InOutsideSize.GetDouble(ref vOutsideSize);
		double value2 = 0.0;
		InOutsideEdgeNudge.GetDouble(ref value2);
		double vOutsideHeight = 0.0;
		InOutsideHeight.GetDouble(ref vOutsideHeight);
		double vOutsideCerticalLocation = 0.0;
		InOutsideVerticalLocation.GetDouble(ref vOutsideCerticalLocation);
		ConcurrentBag<CSData> prongs = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> profiles = new ConcurrentBag<CSData>();
		_prongCache.Clear();
		int gemCount = channelGemDatas.GetArrayCount();
		double edgeNudgeLeft = value;
		double edgeNudgeRight = value2;
		double heightLeft = vInsideHeight;
		double heightRight = vOutsideHeight;
		double vOffsetLeft = vInsideVerticalLocation;
		double vOffsetRight = vOutsideCerticalLocation;
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(channelGemDatas, 0);
		Point3d val2 = channelGemData.Left;
		double num = ((Point3d)(ref val2)).DistanceTo(cSGemData.Center);
		val2 = channelGemData.Right;
		double num2 = ((Point3d)(ref val2)).DistanceTo(cSGemData.Center);
		bool leftOutside = false;
		if (num > num2)
		{
			leftOutside = true;
			edgeNudgeLeft = value2;
			edgeNudgeRight = value;
			heightLeft = vOutsideHeight;
			heightRight = vInsideHeight;
			vOffsetLeft = vOutsideCerticalLocation;
			vOffsetRight = vInsideVerticalLocation;
		}
		bool rightOutside = !leftOutside;
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		OnCurve obj = CSUtilGemRails.Rail(channelGemDatas);
		OnCurve rail = ((obj != null) ? obj.DuplicateCurve() : null);
		OnCurve obj2 = CSUtilGemRails.Right(channelGemDatas, rail);
		OnCurve right = ((obj2 != null) ? obj2.DuplicateCurve() : null);
		OnCurve obj3 = CSUtilGemRails.Left(channelGemDatas, rail);
		OnCurve left = ((obj3 != null) ? obj3.DuplicateCurve() : null);
		Parallel.For(0, gemCount, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Expected O, but got Unknown
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Expected O, but got Unknown
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Expected O, but got Unknown
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Expected O, but got Unknown
			//IL_0277: Expected O, but got Unknown
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Expected O, but got Unknown
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Expected O, but got Unknown
			//IL_0437: Expected O, but got Unknown
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Expected O, but got Unknown
			//IL_0481: Expected O, but got Unknown
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Expected O, but got Unknown
			//IL_039e: Expected O, but got Unknown
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Expected O, but got Unknown
			//IL_03e8: Expected O, but got Unknown
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0530: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Expected O, but got Unknown
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Expected O, but got Unknown
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_056e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0578: Expected O, but got Unknown
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Expected O, but got Unknown
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Expected O, but got Unknown
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Expected O, but got Unknown
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Expected O, but got Unknown
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Expected O, but got Unknown
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Expected O, but got Unknown
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Expected O, but got Unknown
			//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Expected O, but got Unknown
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Expected O, but got Unknown
			//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0702: Expected O, but got Unknown
			//IL_0739: Unknown result type (might be due to invalid IL or missing references)
			//IL_0758: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e7: Expected O, but got Unknown
			//IL_0a1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0870: Unknown result type (might be due to invalid IL or missing references)
			//IL_0877: Expected O, but got Unknown
			//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b57: Expected O, but got Unknown
			//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
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
					On3dPoint val3 = (new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dPoint)(object)cSChannelGemData.Center.Convert()) / 2.0;
					OnCircle circleEdgeRight;
					if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)left, edgeNudgeLeft, out var circleEdgeLeft))
					{
						UpdateError("CalculateCircleEdgeLeft failed");
						loopState.Stop();
					}
					else if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)right, edgeNudgeRight, out circleEdgeRight))
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
						On3dVector val7 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
						val7.Unitize();
						double num4 = 0.0;
						val5.ClosestPointTo((IOn3dPoint)(object)val3, ref num4);
						val5.Rotate(num4 + OnUtil.On_PI / 2.0, val5.Normal());
						val5.Translate((IOn3dVector)(object)(val7 * (vOffsetLeft - heightLeft)));
						double num5 = 0.0;
						val6.ClosestPointTo((IOn3dPoint)(object)val3, ref num5);
						val6.Rotate(num5 + OnUtil.On_PI / 2.0, val6.Normal());
						val6.Translate((IOn3dVector)(object)(val7 * (vOffsetRight - heightRight)));
						On3dPoint val8;
						On3dPoint val9;
						if (leftOutside)
						{
							val8 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (vOutsideSize / 2.0 + cSChannelGemData2.Size - vNudge));
							val9 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * (vInsideSize / 2.0 + cSChannelGemData2.Size - vNudge));
						}
						else
						{
							val8 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * (vOutsideSize / 2.0 + cSChannelGemData2.Size - vNudge));
							val9 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (vInsideSize / 2.0 + cSChannelGemData2.Size - vNudge));
						}
						OnCircle val10;
						OnCircle val11;
						if (OnUtil.ON_CrossProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (IOn3dVector)(object)cSChannelGemData2.Normal.Convert() > 0.0)
						{
							val10 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val8, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), vOutsideSize / 2.0);
							val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert())), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), vInsideSize / 2.0);
						}
						else
						{
							val10 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val8, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), vOutsideSize / 2.0);
							val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert())), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), vInsideSize / 2.0);
						}
						val10.Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * (vOutsideCerticalLocation - vOutsideHeight)));
						val11.Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * (vInsideVerticalLocation - vInsideHeight)));
						if ((vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10) && val5.radius * 2.0 > vSizeMin && val5.radius * 2.0 < vSizeMax)
						{
							if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, val5.radius * 2.0, heightLeft, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong, out var prongBaseRail))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform val12 = new OnXform();
							val12.ChangeBasis((IOnPlane)(object)val5.plane, OnUtil.On_xy_plane);
							prong.Transform((IOnXform)(object)val12);
							prongBaseRail.Transform((IOnXform)(object)val12);
							prongs.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val12.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val12.Convert(), applyTransform: false));
						}
						if ((vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10) && val6.radius * 2.0 > vSizeMin && val6.radius * 2.0 < vSizeMax)
						{
							if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, val6.radius * 2.0, heightRight, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong2, out var prongBaseRail2))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform val13 = new OnXform();
							val13.ChangeBasis((IOnPlane)(object)val6.plane, OnUtil.On_xy_plane);
							prong2.Transform((IOnXform)(object)val13);
							prongBaseRail2.Transform((IOnXform)(object)val13);
							prongs.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val13.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val13.Convert(), applyTransform: false));
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle2) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle3) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle8) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, val10.radius * 2.0, vOutsideHeight, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong3, out var prongBaseRail3))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform val14 = new OnXform();
							val14.ChangeBasis((IOnPlane)(object)val10.plane, OnUtil.On_xy_plane);
							prong3.Transform((IOnXform)(object)val14);
							prongBaseRail3.Transform((IOnXform)(object)val14);
							prongs.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val14.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail3).Convert(), val14.Convert(), applyTransform: false));
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle2) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle3) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle8) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, val11.radius * 2.0, vInsideHeight, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong4, out var prongBaseRail4))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform val15 = new OnXform();
							val15.ChangeBasis((IOnPlane)(object)val11.plane, OnUtil.On_xy_plane);
							prong4.Transform((IOnXform)(object)val15);
							prongBaseRail4.Transform((IOnXform)(object)val15);
							prongs.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val15.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val15.Convert(), applyTransform: false));
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
		return base.Update();
	}

	private bool UpdateSync()
	{
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Expected O, but got Unknown
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Expected O, but got Unknown
		//IL_04ca: Expected O, but got Unknown
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Expected O, but got Unknown
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Expected O, but got Unknown
		//IL_0669: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06ae: Expected O, but got Unknown
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_05da: Expected O, but got Unknown
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Expected O, but got Unknown
		//IL_061f: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Expected O, but got Unknown
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Expected O, but got Unknown
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Expected O, but got Unknown
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Expected O, but got Unknown
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Expected O, but got Unknown
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Expected O, but got Unknown
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Expected O, but got Unknown
		//IL_0bce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		if (!(InCenterGem.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Center Gem");
		}
		if (!(InClusterGems.Data is CSDataArray cSDataArray) || cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
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
		InTaper.GetDoublePercentage(ref value4);
		double value5 = 0.0;
		InSizeMin.GetDouble(ref value5);
		double value6 = 100.0;
		InSizeMax.GetDouble(ref value6);
		double value7 = 0.5;
		InInsideSize.GetDouble(ref value7);
		double value8 = 0.0;
		InInsideEdgeNudge.GetDouble(ref value8);
		double value9 = 0.0;
		InInsideHeight.GetDouble(ref value9);
		double value10 = 0.0;
		InInsideVerticalLocation.GetDouble(ref value10);
		double value11 = 0.5;
		InOutsideSize.GetDouble(ref value11);
		double value12 = 0.0;
		InOutsideEdgeNudge.GetDouble(ref value12);
		double value13 = 0.0;
		InOutsideHeight.GetDouble(ref value13);
		double value14 = 0.0;
		InOutsideVerticalLocation.GetDouble(ref value14);
		ConcurrentBag<CSData> concurrentBag = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> concurrentBag2 = new ConcurrentBag<CSData>();
		_prongCache.Clear();
		int arrayCount = cSDataArray.GetArrayCount();
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
		OnCurve obj = CSUtilGemRails.Rail(cSDataArray);
		OnCurve rail = ((obj != null) ? obj.DuplicateCurve() : null);
		OnCurve obj2 = CSUtilGemRails.Right(cSDataArray, rail);
		OnCurve rail2 = ((obj2 != null) ? obj2.DuplicateCurve() : null);
		OnCurve obj3 = CSUtilGemRails.Left(cSDataArray, rail);
		OnCurve rail3 = ((obj3 != null) ? obj3.DuplicateCurve() : null);
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
			if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)rail3, edgeNudge, out var circleEdgeLeft))
			{
				return UpdateError("CalculateCircleEdgeLeft failed");
			}
			if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, (IOnCurve)(object)rail2, edgeNudge2, out var circleEdgeRight))
			{
				return UpdateError("CalculateCircleEdgeRight failed");
			}
			OnCircle val5 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData.Center.Convert(), (IOn3dVector)(object)cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value2);
			OnCircle c = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value2);
			OnCircle val6 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val5, (IOnCircle)(object)c, (IOnCircle)(object)circleEdgeLeft);
			OnCircle val7 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)val5, (IOnCircle)(object)c, (IOnCircle)(object)circleEdgeRight);
			On3dVector val8 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData.Normal.Convert();
			val8.Unitize();
			double num8 = 0.0;
			val6.ClosestPointTo((IOn3dPoint)(object)val4, ref num8);
			val6.Rotate(num8 + OnUtil.On_PI / 2.0, val6.Normal());
			val6.Translate((IOn3dVector)(object)(val8 * (num3 - num)));
			double num9 = 0.0;
			val7.ClosestPointTo((IOn3dPoint)(object)val4, ref num9);
			val7.Rotate(num9 + OnUtil.On_PI / 2.0, val7.Normal());
			val7.Translate((IOn3dVector)(object)(val8 * (num4 - num2)));
			On3dPoint val9;
			On3dPoint val10;
			if (flag)
			{
				val9 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value2));
				val10 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * (value7 / 2.0 + cSChannelGemData2.Size - value2));
			}
			else
			{
				val9 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value2));
				val10 = new On3dPoint((IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) + (IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (value7 / 2.0 + cSChannelGemData2.Size - value2));
			}
			OnCircle val11;
			OnCircle val12;
			if (OnUtil.ON_CrossProduct((IOn3dVector)(object)cSChannelGemData2.ForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()) * (IOn3dVector)(object)cSChannelGemData2.Normal.Convert() > 0.0)
			{
				val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value11 / 2.0);
				val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val10, (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert())), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value7 / 2.0);
			}
			else
			{
				val11 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val9, (IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert(), (IOn3dVector)(object)cSChannelGemData2.UnitRightVector.Convert()), value11 / 2.0);
				val12 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val10, (IOn3dVector)(object)(-new On3dVector((IOn3dVector)(object)cSChannelGemData2.UnitForwardVector.Convert())), (IOn3dVector)(object)cSChannelGemData2.UnitLeftVector.Convert()), value7 / 2.0);
			}
			val11.Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * (value14 - value13)));
			val12.Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)cSChannelGemData2.Normal.Convert()) * (value10 - value9)));
			if ((whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag2 && whichItem == ValueListItem.ClusterProngStyle6) || (flag && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10) && val6.radius * 2.0 > value5 && val6.radius * 2.0 < value6)
			{
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, val6.radius * 2.0, num, value3, value4, value, out var prong, out var prongBaseRail))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val13 = new OnXform();
				val13.ChangeBasis((IOnPlane)(object)val6.plane, OnUtil.On_xy_plane);
				prong.Transform((IOnXform)(object)val13);
				prongBaseRail.Transform((IOnXform)(object)val13);
				concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val13.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val13.Convert(), applyTransform: false));
			}
			if ((whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag2 && whichItem == ValueListItem.ClusterProngStyle4) || (flag && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10) && val7.radius * 2.0 > value5 && val7.radius * 2.0 < value6)
			{
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, val7.radius * 2.0, num2, value3, value4, value, out var prong2, out var prongBaseRail2))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val14 = new OnXform();
				val14.ChangeBasis((IOnPlane)(object)val7.plane, OnUtil.On_xy_plane);
				prong2.Transform((IOnXform)(object)val14);
				prongBaseRail2.Transform((IOnXform)(object)val14);
				concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val14.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val14.Convert(), applyTransform: false));
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || (flag && whichItem == ValueListItem.ClusterProngStyle2) || (flag2 && whichItem == ValueListItem.ClusterProngStyle3) || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || (flag && whichItem == ValueListItem.ClusterProngStyle8) || (flag2 && whichItem == ValueListItem.ClusterProngStyle9))
			{
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, val11.radius * 2.0, value13, value3, value4, value, out var prong3, out var prongBaseRail3))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val15 = new OnXform();
				val15.ChangeBasis((IOnPlane)(object)val11.plane, OnUtil.On_xy_plane);
				prong3.Transform((IOnXform)(object)val15);
				prongBaseRail3.Transform((IOnXform)(object)val15);
				concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong3).Convert(), val15.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail3).Convert(), val15.Convert(), applyTransform: false));
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || (flag2 && whichItem == ValueListItem.ClusterProngStyle2) || (flag && whichItem == ValueListItem.ClusterProngStyle3) || (flag2 && whichItem == ValueListItem.ClusterProngStyle4) || (flag && whichItem == ValueListItem.ClusterProngStyle5) || (flag2 && whichItem == ValueListItem.ClusterProngStyle6) || (flag && whichItem == ValueListItem.ClusterProngStyle7) || (flag2 && whichItem == ValueListItem.ClusterProngStyle8) || (flag && whichItem == ValueListItem.ClusterProngStyle9))
			{
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, val12.radius * 2.0, value9, value3, value4, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val16 = new OnXform();
				val16.ChangeBasis((IOnPlane)(object)val12.plane, OnUtil.On_xy_plane);
				prong4.Transform((IOnXform)(object)val16);
				prongBaseRail4.Transform((IOnXform)(object)val16);
				concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong4).Convert(), val16.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail4).Convert(), val16.Convert(), applyTransform: false));
			}
		}
		if (base.Error)
		{
			return true;
		}
		OutProngs.SetData(new CSDataArray(concurrentBag));
		OutProfiles.SetData(new CSDataArray(concurrentBag2));
		return base.Update();
	}
}
