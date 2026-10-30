using System.Collections.Concurrent;
using System.Collections.Generic;
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

namespace CSBlocks.Settings;

[CSBlock("ClusterProngs", "{6D1EDE61-A26C-47E3-826D-17BAEFB4F1E8}")]
public sealed class CSClusterProngs : CSBlock
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

	public CSClusterProngs(string identifier)
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
			OnCircle onCircle = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			vProfile = new OnNurbsCurve();
			onCircle.GetNurbForm(ref vProfile);
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
		double num = channelGemData.Left.DistanceTo(cSGemData.Center);
		double num2 = channelGemData.Right.DistanceTo(cSGemData.Center);
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
		OnCurve rail = CSUtilGemRails.Rail(channelGemDatas)?.DuplicateCurve();
		OnCurve right = CSUtilGemRails.Right(channelGemDatas, rail)?.DuplicateCurve();
		OnCurve left = CSUtilGemRails.Left(channelGemDatas, rail)?.DuplicateCurve();
		Parallel.For(0, gemCount, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
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
					On3dPoint point = (new On3dPoint(cSChannelGemData2.Center.Convert()) + cSChannelGemData.Center.Convert()) / 2.0;
					OnCircle circleEdgeRight;
					if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, left, edgeNudgeLeft, out var circleEdgeLeft))
					{
						UpdateError("CalculateCircleEdgeLeft failed");
						loopState.Stop();
					}
					else if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, right, edgeNudgeRight, out circleEdgeRight))
					{
						UpdateError("CalculateCircleEdgeRight failed");
						loopState.Stop();
					}
					else
					{
						OnCircle c = new OnCircle(new OnPlane(cSChannelGemData.Center.Convert(), cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - vNudge);
						OnCircle c2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - vNudge);
						OnCircle onCircle2 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, circleEdgeLeft);
						OnCircle onCircle3 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, circleEdgeRight);
						On3dVector on3dVector = new On3dVector(cSChannelGemData2.Normal.Convert()) + cSChannelGemData.Normal.Convert();
						on3dVector.Unitize();
						double t = 0.0;
						onCircle2.ClosestPointTo(point, ref t);
						onCircle2.Rotate(t + OnUtil.On_PI / 2.0, onCircle2.Normal());
						onCircle2.Translate(on3dVector * (vOffsetLeft - heightLeft));
						double t2 = 0.0;
						onCircle3.ClosestPointTo(point, ref t2);
						onCircle3.Rotate(t2 + OnUtil.On_PI / 2.0, onCircle3.Normal());
						onCircle3.Translate(on3dVector * (vOffsetRight - heightRight));
						On3dPoint origin;
						On3dPoint origin2;
						if (leftOutside)
						{
							origin = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * (vOutsideSize / 2.0 + cSChannelGemData2.Size - vNudge);
							origin2 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * (vInsideSize / 2.0 + cSChannelGemData2.Size - vNudge);
						}
						else
						{
							origin = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * (vOutsideSize / 2.0 + cSChannelGemData2.Size - vNudge);
							origin2 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * (vInsideSize / 2.0 + cSChannelGemData2.Size - vNudge);
						}
						OnCircle onCircle4;
						OnCircle onCircle5;
						if (OnUtil.ON_CrossProduct(cSChannelGemData2.ForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()) * cSChannelGemData2.Normal.Convert() > 0.0)
						{
							onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), vOutsideSize / 2.0);
							onCircle5 = new OnCircle(new OnPlane(origin2, -new On3dVector(cSChannelGemData2.UnitForwardVector.Convert()), cSChannelGemData2.UnitRightVector.Convert()), vInsideSize / 2.0);
						}
						else
						{
							onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), vOutsideSize / 2.0);
							onCircle5 = new OnCircle(new OnPlane(origin2, -new On3dVector(cSChannelGemData2.UnitForwardVector.Convert()), cSChannelGemData2.UnitLeftVector.Convert()), vInsideSize / 2.0);
						}
						onCircle4.Translate(new On3dVector(cSChannelGemData2.Normal.Convert()) * (vOutsideCerticalLocation - vOutsideHeight));
						onCircle5.Translate(new On3dVector(cSChannelGemData2.Normal.Convert()) * (vInsideVerticalLocation - vInsideHeight));
						if ((vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10) && onCircle2.radius * 2.0 > vSizeMin && onCircle2.radius * 2.0 < vSizeMax)
						{
							if (!_prongCache.Create(vProfile, onCircle2.radius * 2.0, heightLeft, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong, out var prongBaseRail))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform onXform = new OnXform();
							onXform.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
							prong.Transform(onXform);
							prongBaseRail.Transform(onXform);
							prongs.Add(new CSOnGeometryData(prong.Convert(), onXform.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform.Convert(), applyTransform: false));
						}
						if ((vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10) && onCircle3.radius * 2.0 > vSizeMin && onCircle3.radius * 2.0 < vSizeMax)
						{
							if (!_prongCache.Create(vProfile, onCircle3.radius * 2.0, heightRight, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong2, out var prongBaseRail2))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform onXform2 = new OnXform();
							onXform2.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
							prong2.Transform(onXform2);
							prongBaseRail2.Transform(onXform2);
							prongs.Add(new CSOnGeometryData(prong2.Convert(), onXform2.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform2.Convert(), applyTransform: false));
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle2) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle3) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle8) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (!_prongCache.Create(vProfile, onCircle4.radius * 2.0, vOutsideHeight, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong3, out var prongBaseRail3))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform onXform3 = new OnXform();
							onXform3.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
							prong3.Transform(onXform3);
							prongBaseRail3.Transform(onXform3);
							prongs.Add(new CSOnGeometryData(prong3.Convert(), onXform3.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(prongBaseRail3.Convert(), onXform3.Convert(), applyTransform: false));
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle2) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle3) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle8) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (!_prongCache.Create(vProfile, onCircle5.radius * 2.0, vInsideHeight, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong4, out var prongBaseRail4))
							{
								UpdateError("Create prong cache failed");
								loopState.Stop();
								return;
							}
							OnXform onXform4 = new OnXform();
							onXform4.ChangeBasis(onCircle5.plane, OnUtil.On_xy_plane);
							prong4.Transform(onXform4);
							prongBaseRail4.Transform(onXform4);
							prongs.Add(new CSOnGeometryData(prong4.Convert(), onXform4.Convert(), applyTransform: false));
							profiles.Add(new CSOnGeometryData(prongBaseRail4.Convert(), onXform4.Convert(), applyTransform: false));
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
		if (!(InCenterGem.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Center Gem");
		}
		if (!(InClusterGems.Data is CSDataArray cSDataArray) || cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
		}
		OnNurbsCurve nurbs_curve = InProfile.GetNurbsCurve();
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
		List<CSData> list = new List<CSData>();
		List<CSData> list2 = new List<CSData>();
		_prongCache.Clear();
		int arrayCount = cSDataArray.GetArrayCount();
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
		OnCurve rail = CSUtilGemRails.Rail(cSDataArray)?.DuplicateCurve();
		OnCurve rail2 = CSUtilGemRails.Right(cSDataArray, rail)?.DuplicateCurve();
		OnCurve rail3 = CSUtilGemRails.Left(cSDataArray, rail)?.DuplicateCurve();
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
			On3dPoint point = (new On3dPoint(cSChannelGemData2.Center.Convert()) + cSChannelGemData.Center.Convert()) / 2.0;
			if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, rail3, edgeNudge, out var circleEdgeLeft))
			{
				return UpdateError("CalculateCircleEdgeLeft failed");
			}
			if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, rail2, edgeNudge2, out var circleEdgeRight))
			{
				return UpdateError("CalculateCircleEdgeRight failed");
			}
			OnCircle c = new OnCircle(new OnPlane(cSChannelGemData.Center.Convert(), cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value2);
			OnCircle c2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value2);
			OnCircle onCircle2 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, circleEdgeLeft);
			OnCircle onCircle3 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, circleEdgeRight);
			On3dVector on3dVector = new On3dVector(cSChannelGemData2.Normal.Convert()) + cSChannelGemData.Normal.Convert();
			on3dVector.Unitize();
			double t = 0.0;
			onCircle2.ClosestPointTo(point, ref t);
			onCircle2.Rotate(t + OnUtil.On_PI / 2.0, onCircle2.Normal());
			onCircle2.Translate(on3dVector * (num3 - num));
			double t2 = 0.0;
			onCircle3.ClosestPointTo(point, ref t2);
			onCircle3.Rotate(t2 + OnUtil.On_PI / 2.0, onCircle3.Normal());
			onCircle3.Translate(on3dVector * (num4 - num2));
			On3dPoint origin;
			On3dPoint origin2;
			if (flag)
			{
				origin = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value2);
				origin2 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * (value7 / 2.0 + cSChannelGemData2.Size - value2);
			}
			else
			{
				origin = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitRightVector.Convert()) * (value11 / 2.0 + cSChannelGemData2.Size - value2);
				origin2 = new On3dPoint(cSChannelGemData2.Center.Convert()) + new On3dVector(cSChannelGemData2.UnitLeftVector.Convert()) * (value7 / 2.0 + cSChannelGemData2.Size - value2);
			}
			OnCircle onCircle4;
			OnCircle onCircle5;
			if (OnUtil.ON_CrossProduct(cSChannelGemData2.ForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()) * cSChannelGemData2.Normal.Convert() > 0.0)
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), value11 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -new On3dVector(cSChannelGemData2.UnitForwardVector.Convert()), cSChannelGemData2.UnitRightVector.Convert()), value7 / 2.0);
			}
			else
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), value11 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -new On3dVector(cSChannelGemData2.UnitForwardVector.Convert()), cSChannelGemData2.UnitLeftVector.Convert()), value7 / 2.0);
			}
			onCircle4.Translate(new On3dVector(cSChannelGemData2.Normal.Convert()) * (value14 - value13));
			onCircle5.Translate(new On3dVector(cSChannelGemData2.Normal.Convert()) * (value10 - value9));
			if ((whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag2 && whichItem == ValueListItem.ClusterProngStyle6) || (flag && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10) && onCircle2.radius * 2.0 > value5 && onCircle2.radius * 2.0 < value6)
			{
				if (!_prongCache.Create(nurbs_curve, onCircle2.radius * 2.0, num, value3, value4, value, out var prong, out var prongBaseRail))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform = new OnXform();
				onXform.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
				prong.Transform(onXform);
				prongBaseRail.Transform(onXform);
				list.Add(new CSOnGeometryData(prong.Convert(), onXform.Convert(), applyTransform: false));
				list2.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform.Convert(), applyTransform: false));
			}
			if ((whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag2 && whichItem == ValueListItem.ClusterProngStyle4) || (flag && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10) && onCircle3.radius * 2.0 > value5 && onCircle3.radius * 2.0 < value6)
			{
				if (!_prongCache.Create(nurbs_curve, onCircle3.radius * 2.0, num2, value3, value4, value, out var prong2, out var prongBaseRail2))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform2 = new OnXform();
				onXform2.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
				prong2.Transform(onXform2);
				prongBaseRail2.Transform(onXform2);
				list.Add(new CSOnGeometryData(prong2.Convert(), onXform2.Convert(), applyTransform: false));
				list2.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform2.Convert(), applyTransform: false));
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || (flag && whichItem == ValueListItem.ClusterProngStyle2) || (flag2 && whichItem == ValueListItem.ClusterProngStyle3) || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || (flag && whichItem == ValueListItem.ClusterProngStyle8) || (flag2 && whichItem == ValueListItem.ClusterProngStyle9))
			{
				if (!_prongCache.Create(nurbs_curve, onCircle4.radius * 2.0, value13, value3, value4, value, out var prong3, out var prongBaseRail3))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform3 = new OnXform();
				onXform3.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
				prong3.Transform(onXform3);
				prongBaseRail3.Transform(onXform3);
				list.Add(new CSOnGeometryData(prong3.Convert(), onXform3.Convert(), applyTransform: false));
				list2.Add(new CSOnGeometryData(prongBaseRail3.Convert(), onXform3.Convert(), applyTransform: false));
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || (flag2 && whichItem == ValueListItem.ClusterProngStyle2) || (flag && whichItem == ValueListItem.ClusterProngStyle3) || (flag2 && whichItem == ValueListItem.ClusterProngStyle4) || (flag && whichItem == ValueListItem.ClusterProngStyle5) || (flag2 && whichItem == ValueListItem.ClusterProngStyle6) || (flag && whichItem == ValueListItem.ClusterProngStyle7) || (flag2 && whichItem == ValueListItem.ClusterProngStyle8) || (flag && whichItem == ValueListItem.ClusterProngStyle9))
			{
				if (!_prongCache.Create(nurbs_curve, onCircle5.radius * 2.0, value9, value3, value4, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform4 = new OnXform();
				onXform4.ChangeBasis(onCircle5.plane, OnUtil.On_xy_plane);
				prong4.Transform(onXform4);
				prongBaseRail4.Transform(onXform4);
				list.Add(new CSOnGeometryData(prong4.Convert(), onXform4.Convert(), applyTransform: false));
				list2.Add(new CSOnGeometryData(prongBaseRail4.Convert(), onXform4.Convert(), applyTransform: false));
			}
		}
		if (base.Error)
		{
			return true;
		}
		OutProngs.SetData(new CSDataArray(list));
		OutProfiles.SetData(new CSDataArray(list2));
		return base.Update();
	}
}
