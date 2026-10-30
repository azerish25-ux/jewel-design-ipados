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
		double num = channelGemData.Left.DistanceTo(centerGemData.Center);
		double num2 = channelGemData.Right.DistanceTo(centerGemData.Center);
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
					On3dPoint point = (cSChannelGemData2.Center.Convert() + cSChannelGemData.Center.Convert()) / 2.0;
					OnCircle circleEdgeRight;
					if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, crvLeft.Convert(), edgeNudgeLeft, out var circleEdgeLeft))
					{
						UpdateError("CalculateCircleEdgeLeft failed");
						loopState.Stop();
					}
					else if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, crvRight.Convert(), edgeNudgeRight, out circleEdgeRight))
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
						On3dVector on3dVector = cSChannelGemData2.Normal.Convert() + cSChannelGemData.Normal.Convert();
						on3dVector.Unitize();
						double t = 0.0;
						onCircle2.ClosestPointTo(point, ref t);
						onCircle2.Rotate(t + OnUtil.On_PI / 2.0, onCircle2.Normal());
						onCircle2.Translate(on3dVector * (vOffsetLeft - heightLeft));
						double t2 = 0.0;
						onCircle3.ClosestPointTo(point, ref t2);
						onCircle3.Rotate(t2 + OnUtil.On_PI / 2.0, onCircle3.Normal());
						onCircle3.Translate(on3dVector * (vOffsetRight - heightRight));
						(cSChannelGemData2.Normal.Convert() + cSChannelGemData.Normal.Convert()).Unitize();
						On3dPoint origin;
						On3dPoint origin2;
						if (leftOutside)
						{
							origin = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitLeftVector.Convert() * (vOutsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge);
							origin2 = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitRightVector.Convert() * (vInsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge);
						}
						else
						{
							origin = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitRightVector.Convert() * (vOutsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge);
							origin2 = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitLeftVector.Convert() * (vInsideSizeTop / 2.0 + cSChannelGemData2.Size - vNudge);
						}
						OnCircle onCircle4;
						OnCircle onCircle5;
						if (OnUtil.ON_CrossProduct(cSChannelGemData2.ForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()) * cSChannelGemData2.Normal.Convert() > 0.0)
						{
							onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), vOutsideSizeTop / 2.0);
							onCircle5 = new OnCircle(new OnPlane(origin2, -cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), vInsideSizeTop / 2.0);
						}
						else
						{
							onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), vOutsideSizeTop / 2.0);
							onCircle5 = new OnCircle(new OnPlane(origin2, -cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), vInsideSizeTop / 2.0);
						}
						onCircle4.Translate(cSChannelGemData2.Normal.Convert() * (vOutsideVerticalLocation - vOutsideHeight));
						onCircle5.Translate(cSChannelGemData2.Normal.Convert() * (vInsideVerticalLocation - vInsideHeight));
						if (vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10)
						{
							double num4 = onCircle2.radius * 2.0;
							double num5 = prongSizeLeftBase * (num4 / prongSizeLeftTop);
							if (num4 > vSizeMin && num4 < vSizeMax)
							{
								if ((!leftOutside && vDoBlendInside) || (leftOutside && vDoBlendOutside))
								{
									On3dVector on3dVector2 = new On3dPoint(onCircle2.Center()) - centerGemData.Center.Convert();
									on3dVector2.Unitize();
									OnPlaneSurface onPlaneSurface = new OnPlaneSurface(new OnPlane(centerGemData.Center.Convert(), centerGemData.Normal.Convert(), on3dVector2));
									onPlaneSurface.SetExtents(0, new OnInterval(-50.0, 50.0));
									onPlaneSurface.SetExtents(1, new OnInterval(0.0, 50.0));
									ArrayOnX_EVENT x = new ArrayOnX_EVENT();
									blendToCurve.IntersectSurface(onPlaneSurface, ref x);
									if (x.Count() <= 0)
									{
										UpdateError("Intersection failed");
										loopState.Stop();
										return;
									}
									double num6 = -1.23432101234321E+308;
									double num7 = -1.23432101234321E+308;
									for (int j = 0; j < x.Count(); j++)
									{
										double num8 = x[j].get_m_a(0);
										double num9 = ((IOn3dPoint)blendToCurve.PointAt(num8)).DistanceTo((IOn3dPoint)centerGemData.Center.Convert());
										if (!RhinoMath.IsValidDouble(num6) || !RhinoMath.IsValidDouble(num7) || !(num9 >= num7))
										{
											num6 = num8;
											num7 = num9;
										}
									}
									On3dPoint on3dPoint = blendToCurve.PointAt(num6);
									On3dVector on3dVector3 = CSBlockUtil.CSUtilMath.AverageTangentAt(blendToCurve, on3dPoint);
									on3dVector3.Unitize();
									if (OnUtil.ON_DotProduct(cSChannelGemData.ForwardVector.Convert() + cSChannelGemData2.ForwardVector.Convert(), on3dVector3) < 0.0)
									{
										on3dVector3.Reverse();
									}
									if (!BuildBlendedProng(centerGemData, vProfile, num4, num5, heightLeft, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, on3dPoint, on3dVector3, onCircle2, prongs, profiles, rails, out var msg))
									{
										UpdateError("BuildBlendProng:  " + msg);
										loopState.Stop();
										return;
									}
								}
								else
								{
									double taperPer = num5 / num4 - 1.0;
									if (!_prongCache.Create(vProfile, num4, heightRight, vFilletPer, taperPer, vDoMeshedProngs, out var prong, out var prongBaseRail))
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
							}
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || vStyle == ValueListItem.ClusterProngStyle2 || vStyle == ValueListItem.ClusterProngStyle3 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || vStyle == ValueListItem.ClusterProngStyle10)
						{
							double num10 = onCircle3.radius * 2.0;
							double num11 = prongSizeRightBase * (num10 / prongSizeRightTop);
							if (num10 > vSizeMin && num10 < vSizeMax)
							{
								if ((!leftOutside && vDoBlendOutside) || (leftOutside && vDoBlendInside))
								{
									On3dVector on3dVector4 = new On3dPoint(onCircle3.Center()) - centerGemData.Center.Convert();
									on3dVector4.Unitize();
									OnPlaneSurface onPlaneSurface2 = new OnPlaneSurface(new OnPlane(centerGemData.Center.Convert(), centerGemData.Normal.Convert(), on3dVector4));
									onPlaneSurface2.SetExtents(0, new OnInterval(-50.0, 50.0));
									onPlaneSurface2.SetExtents(1, new OnInterval(0.0, 50.0));
									ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
									blendToCurve.IntersectSurface(onPlaneSurface2, ref x2);
									if (x2.Count() <= 0)
									{
										UpdateError("Intersection failed");
										loopState.Stop();
										return;
									}
									double num12 = -1.23432101234321E+308;
									double num13 = -1.23432101234321E+308;
									for (int k = 0; k < x2.Count(); k++)
									{
										double num14 = x2[k].get_m_a(0);
										double num15 = ((IOn3dPoint)blendToCurve.PointAt(num14)).DistanceTo((IOn3dPoint)centerGemData.Center.Convert());
										if (!RhinoMath.IsValidDouble(num12) || !RhinoMath.IsValidDouble(num13) || !(num15 >= num13))
										{
											num12 = num14;
											num13 = num15;
										}
									}
									On3dPoint on3dPoint2 = blendToCurve.PointAt(num12);
									On3dVector on3dVector5 = CSBlockUtil.CSUtilMath.AverageTangentAt(blendToCurve, on3dPoint2);
									on3dVector5.Unitize();
									if (OnUtil.ON_DotProduct(cSChannelGemData.ForwardVector.Convert() + cSChannelGemData2.ForwardVector.Convert(), on3dVector5) < 0.0)
									{
										on3dVector5.Reverse();
									}
									if (!BuildBlendedProng(centerGemData, vProfile, num10, num11, heightRight, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, on3dPoint2, on3dVector5, onCircle3, prongs, profiles, rails, out var msg2))
									{
										UpdateError("BuildBlendProng:  " + msg2);
										loopState.Stop();
										return;
									}
								}
								else
								{
									double taperPer2 = num11 / num10 - 1.0;
									if (!_prongCache.Create(vProfile, num10, heightRight, vFilletPer, taperPer2, vDoMeshedProngs, out var prong2, out var prongBaseRail2))
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
							}
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (leftOutside && vStyle == ValueListItem.ClusterProngStyle2) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle3) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle4) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle5) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle6) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle7) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle8) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (vDoBlendOutside)
							{
								On3dVector on3dVector6 = cSChannelGemData2.Center.Convert() - centerGemData.Center.Convert();
								on3dVector6.Unitize();
								OnPlaneSurface onPlaneSurface3 = new OnPlaneSurface(new OnPlane(centerGemData.Center.Convert(), centerGemData.Normal.Convert(), on3dVector6));
								onPlaneSurface3.SetExtents(0, new OnInterval(-50.0, 50.0));
								onPlaneSurface3.SetExtents(1, new OnInterval(0.0, 50.0));
								ArrayOnX_EVENT x3 = new ArrayOnX_EVENT();
								blendToCurve.IntersectSurface(onPlaneSurface3, ref x3);
								if (x3.Count() <= 0)
								{
									UpdateError("Intersection failed");
									loopState.Stop();
									return;
								}
								double num16 = -1.23432101234321E+308;
								double num17 = -1.23432101234321E+308;
								for (int l = 0; l < x3.Count(); l++)
								{
									double num18 = x3[l].get_m_a(0);
									double num19 = ((IOn3dPoint)blendToCurve.PointAt(num18)).DistanceTo((IOn3dPoint)centerGemData.Center.Convert());
									if (!RhinoMath.IsValidDouble(num16) || !RhinoMath.IsValidDouble(num17) || !(num19 >= num17))
									{
										num16 = num18;
										num17 = num19;
									}
								}
								On3dPoint on3dPoint3 = blendToCurve.PointAt(num16);
								On3dVector on3dVector7 = CSBlockUtil.CSUtilMath.AverageTangentAt(blendToCurve, on3dPoint3);
								on3dVector7.Unitize();
								if (OnUtil.ON_DotProduct(cSChannelGemData2.ForwardVector.Convert(), on3dVector7) < 0.0)
								{
									on3dVector7.Reverse();
								}
								if (!BuildBlendedProng(centerGemData, vProfile, vOutsideSizeTop, vOutsideSizeBase, vOutsideHeight, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, on3dPoint3, on3dVector7, onCircle4, prongs, profiles, rails, out var msg3))
								{
									UpdateError("BuildBlendProng:  " + msg3);
									loopState.Stop();
									return;
								}
							}
							else
							{
								double taperPer3 = vOutsideSizeBase / vOutsideSizeTop - 1.0;
								if (!_prongCache.Create(vProfile, vOutsideSizeTop, vOutsideHeight, vFilletPer, taperPer3, vDoMeshedProngs, out var prong3, out var prongBaseRail3))
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
						}
						if (vStyle == ValueListItem.ClusterProngStyle1 || (rightOutside && vStyle == ValueListItem.ClusterProngStyle2) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle3) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle4) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle5) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle6) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle7) || (rightOutside && vStyle == ValueListItem.ClusterProngStyle8) || (leftOutside && vStyle == ValueListItem.ClusterProngStyle9))
						{
							if (vDoBlendInside)
							{
								On3dVector on3dVector8 = cSChannelGemData2.Center.Convert() - centerGemData.Center.Convert();
								on3dVector8.Unitize();
								OnPlaneSurface onPlaneSurface4 = new OnPlaneSurface(new OnPlane(centerGemData.Center.Convert(), centerGemData.Normal.Convert(), on3dVector8));
								onPlaneSurface4.SetExtents(0, new OnInterval(-50.0, 50.0));
								onPlaneSurface4.SetExtents(1, new OnInterval(0.0, 50.0));
								ArrayOnX_EVENT x4 = new ArrayOnX_EVENT();
								blendToCurve.IntersectSurface(onPlaneSurface4, ref x4);
								if (x4.Count() <= 0)
								{
									UpdateError("Intersection failed");
									loopState.Stop();
									return;
								}
								double num20 = -1.23432101234321E+308;
								double num21 = -1.23432101234321E+308;
								for (int m = 0; m < x4.Count(); m++)
								{
									double num22 = x4[m].get_m_a(0);
									double num23 = ((IOn3dPoint)blendToCurve.PointAt(num22)).DistanceTo((IOn3dPoint)centerGemData.Center.Convert());
									if (!RhinoMath.IsValidDouble(num20) || !RhinoMath.IsValidDouble(num21) || !(num23 >= num21))
									{
										num20 = num22;
										num21 = num23;
									}
								}
								On3dPoint on3dPoint4 = blendToCurve.PointAt(num20);
								On3dVector on3dVector9 = CSBlockUtil.CSUtilMath.AverageTangentAt(blendToCurve, on3dPoint4);
								on3dVector9.Unitize();
								if (OnUtil.ON_DotProduct(cSChannelGemData2.ForwardVector.Convert(), on3dVector9) < 0.0)
								{
									on3dVector9.Reverse();
								}
								if (!BuildBlendedProng(centerGemData, vProfile, vInsideSizeTop, vInsideSizeBase, vInsideHeight, vFilletPer, vBlendTilt, vBlendAmount, vBlendAmountTop, vBlendAmountBase, on3dPoint4, on3dVector9, onCircle5, prongs, profiles, rails, out var msg4))
								{
									UpdateError("BuildBlendProng:  " + msg4);
									loopState.Stop();
									return;
								}
							}
							else
							{
								double taperPer4 = vInsideSizeBase / vInsideSizeTop - 1.0;
								if (!_prongCache.Create(vProfile, vInsideSizeTop, vInsideHeight, vFilletPer, taperPer4, vDoMeshedProngs, out var prong4, out var prongBaseRail4))
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
		double num9 = channelGemData.Left.DistanceTo(cSGemData.Center);
		double num10 = channelGemData.Right.DistanceTo(cSGemData.Center);
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
			On3dPoint point = (cSChannelGemData2.Center.Convert() + cSChannelGemData.Center.Convert()) / 2.0;
			if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData, cSChannelGemData2, outRail.Convert(), edgeNudge, out var circleEdgeLeft))
			{
				return UpdateError("CalculateCircleEdgeLeft failed");
			}
			if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData, cSChannelGemData2, outRail2.Convert(), edgeNudge2, out var circleEdgeRight))
			{
				return UpdateError("CalculateCircleEdgeRight failed");
			}
			OnCircle c = new OnCircle(new OnPlane(cSChannelGemData.Center.Convert(), cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value2);
			OnCircle c2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value2);
			OnCircle onCircle2 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, circleEdgeLeft);
			OnCircle onCircle3 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, circleEdgeRight);
			On3dVector on3dVector = cSChannelGemData2.Normal.Convert() + cSChannelGemData.Normal.Convert();
			on3dVector.Unitize();
			double t = 0.0;
			onCircle2.ClosestPointTo(point, ref t);
			onCircle2.Rotate(t + OnUtil.On_PI / 2.0, onCircle2.Normal());
			onCircle2.Translate(on3dVector * (num7 - num5));
			double t2 = 0.0;
			onCircle3.ClosestPointTo(point, ref t2);
			onCircle3.Rotate(t2 + OnUtil.On_PI / 2.0, onCircle3.Normal());
			onCircle3.Translate(on3dVector * (num8 - num6));
			(cSChannelGemData2.Normal.Convert() + cSChannelGemData.Normal.Convert()).Unitize();
			On3dPoint origin;
			On3dPoint origin2;
			if (flag)
			{
				origin = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitLeftVector.Convert() * (value11 / 2.0 + cSChannelGemData2.Size - value2);
				origin2 = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitRightVector.Convert() * (value6 / 2.0 + cSChannelGemData2.Size - value2);
			}
			else
			{
				origin = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitRightVector.Convert() * (value11 / 2.0 + cSChannelGemData2.Size - value2);
				origin2 = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitLeftVector.Convert() * (value6 / 2.0 + cSChannelGemData2.Size - value2);
			}
			OnCircle onCircle4;
			OnCircle onCircle5;
			if (OnUtil.ON_CrossProduct(cSChannelGemData2.ForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()) * cSChannelGemData2.Normal.Convert() > 0.0)
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), value11 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), value6 / 2.0);
			}
			else
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), value11 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), value6 / 2.0);
			}
			onCircle4.Translate(cSChannelGemData2.Normal.Convert() * (value15 - value13));
			onCircle5.Translate(cSChannelGemData2.Normal.Convert() * (value10 - value8));
			if (whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag2 && whichItem == ValueListItem.ClusterProngStyle6) || (flag && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10)
			{
				double num12 = onCircle2.radius * 2.0;
				double num13 = num3 * (num12 / num);
				if (num12 > value4 && num12 < value5)
				{
					if ((!flag && value16) || (flag && value17))
					{
						On3dVector on3dVector2 = new On3dPoint(onCircle2.Center()) - cSGemData.Center.Convert();
						on3dVector2.Unitize();
						OnPlaneSurface onPlaneSurface = new OnPlaneSurface(new OnPlane(cSGemData.Center.Convert(), cSGemData.Normal.Convert(), on3dVector2));
						onPlaneSurface.SetExtents(0, new OnInterval(-50.0, 50.0));
						onPlaneSurface.SetExtents(1, new OnInterval(0.0, 50.0));
						ArrayOnX_EVENT x = new ArrayOnX_EVENT();
						nurbsCurve.IntersectSurface(onPlaneSurface, ref x);
						if (x.Count() <= 0)
						{
							return UpdateError("Intersection failed");
						}
						double num14 = -1.23432101234321E+308;
						double num15 = -1.23432101234321E+308;
						for (int j = 0; j < x.Count(); j++)
						{
							double num16 = x[j].get_m_a(0);
							double num17 = ((IOn3dPoint)nurbsCurve.PointAt(num16)).DistanceTo((IOn3dPoint)cSGemData.Center.Convert());
							if (!RhinoMath.IsValidDouble(num14) || !RhinoMath.IsValidDouble(num15) || !(num17 >= num15))
							{
								num14 = num16;
								num15 = num17;
							}
						}
						On3dPoint on3dPoint = nurbsCurve.PointAt(num14);
						On3dVector on3dVector3 = CSBlockUtil.CSUtilMath.AverageTangentAt(nurbsCurve, on3dPoint);
						on3dVector3.Unitize();
						if (OnUtil.ON_DotProduct(cSChannelGemData.ForwardVector.Convert() + cSChannelGemData2.ForwardVector.Convert(), on3dVector3) < 0.0)
						{
							on3dVector3.Reverse();
						}
						if (!BuildBlendedProng(cSGemData, nurbs_curve, num12, num13, num5, value3, value21, value18, value19, value20, on3dPoint, on3dVector3, onCircle2, concurrentBag, concurrentBag2, concurrentBag3, out var msg))
						{
							return UpdateError("BuildBlendProng:  " + msg);
						}
					}
					else
					{
						double taperPer = num13 / num12 - 1.0;
						if (!_prongCache.Create(nurbs_curve, num12, num6, value3, taperPer, value, out var prong, out var prongBaseRail))
						{
							return UpdateError("Create prong cache failed");
						}
						OnXform onXform = new OnXform();
						onXform.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
						prong.Transform(onXform);
						prongBaseRail.Transform(onXform);
						concurrentBag.Add(new CSOnGeometryData(prong.Convert(), onXform.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform.Convert(), applyTransform: false));
					}
				}
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || whichItem == ValueListItem.ClusterProngStyle2 || whichItem == ValueListItem.ClusterProngStyle3 || (flag2 && whichItem == ValueListItem.ClusterProngStyle4) || (flag && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || whichItem == ValueListItem.ClusterProngStyle10)
			{
				double num18 = onCircle3.radius * 2.0;
				double num19 = num4 * (num18 / num2);
				if (num18 > value4 && num18 < value5)
				{
					if ((!flag && value17) || (flag && value16))
					{
						On3dVector on3dVector4 = new On3dPoint(onCircle3.Center()) - cSGemData.Center.Convert();
						on3dVector4.Unitize();
						OnPlaneSurface onPlaneSurface2 = new OnPlaneSurface(new OnPlane(cSGemData.Center.Convert(), cSGemData.Normal.Convert(), on3dVector4));
						onPlaneSurface2.SetExtents(0, new OnInterval(-50.0, 50.0));
						onPlaneSurface2.SetExtents(1, new OnInterval(0.0, 50.0));
						ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
						nurbsCurve.IntersectSurface(onPlaneSurface2, ref x2);
						if (x2.Count() <= 0)
						{
							return UpdateError("Intersection failed");
						}
						double num20 = -1.23432101234321E+308;
						double num21 = -1.23432101234321E+308;
						for (int k = 0; k < x2.Count(); k++)
						{
							double num22 = x2[k].get_m_a(0);
							double num23 = ((IOn3dPoint)nurbsCurve.PointAt(num22)).DistanceTo((IOn3dPoint)cSGemData.Center.Convert());
							if (!RhinoMath.IsValidDouble(num20) || !RhinoMath.IsValidDouble(num21) || !(num23 >= num21))
							{
								num20 = num22;
								num21 = num23;
							}
						}
						On3dPoint on3dPoint2 = nurbsCurve.PointAt(num20);
						On3dVector on3dVector5 = CSBlockUtil.CSUtilMath.AverageTangentAt(nurbsCurve, on3dPoint2);
						on3dVector5.Unitize();
						if (OnUtil.ON_DotProduct(cSChannelGemData.ForwardVector.Convert() + cSChannelGemData2.ForwardVector.Convert(), on3dVector5) < 0.0)
						{
							on3dVector5.Reverse();
						}
						if (!BuildBlendedProng(cSGemData, nurbs_curve, num18, num19, num6, value3, value21, value18, value19, value20, on3dPoint2, on3dVector5, onCircle3, concurrentBag, concurrentBag2, concurrentBag3, out var msg2))
						{
							return UpdateError("BuildBlendProng:  " + msg2);
						}
					}
					else
					{
						double taperPer2 = num19 / num18 - 1.0;
						if (!_prongCache.Create(nurbs_curve, num18, num6, value3, taperPer2, value, out var prong2, out var prongBaseRail2))
						{
							return UpdateError("Create prong cache failed");
						}
						OnXform onXform2 = new OnXform();
						onXform2.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
						prong2.Transform(onXform2);
						prongBaseRail2.Transform(onXform2);
						concurrentBag.Add(new CSOnGeometryData(prong2.Convert(), onXform2.Convert(), applyTransform: false));
						concurrentBag2.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform2.Convert(), applyTransform: false));
					}
				}
			}
			if (whichItem == ValueListItem.ClusterProngStyle1 || (flag && whichItem == ValueListItem.ClusterProngStyle2) || (flag2 && whichItem == ValueListItem.ClusterProngStyle3) || (flag && whichItem == ValueListItem.ClusterProngStyle4) || (flag2 && whichItem == ValueListItem.ClusterProngStyle5) || (flag && whichItem == ValueListItem.ClusterProngStyle6) || (flag2 && whichItem == ValueListItem.ClusterProngStyle7) || (flag && whichItem == ValueListItem.ClusterProngStyle8) || (flag2 && whichItem == ValueListItem.ClusterProngStyle9))
			{
				if (value17)
				{
					On3dVector on3dVector6 = cSChannelGemData2.Center.Convert() - cSGemData.Center.Convert();
					on3dVector6.Unitize();
					OnPlaneSurface onPlaneSurface3 = new OnPlaneSurface(new OnPlane(cSGemData.Center.Convert(), cSGemData.Normal.Convert(), on3dVector6));
					onPlaneSurface3.SetExtents(0, new OnInterval(-50.0, 50.0));
					onPlaneSurface3.SetExtents(1, new OnInterval(0.0, 50.0));
					ArrayOnX_EVENT x3 = new ArrayOnX_EVENT();
					nurbsCurve.IntersectSurface(onPlaneSurface3, ref x3);
					if (x3.Count() <= 0)
					{
						return UpdateError("Intersection failed");
					}
					double num24 = -1.23432101234321E+308;
					double num25 = -1.23432101234321E+308;
					for (int l = 0; l < x3.Count(); l++)
					{
						double num26 = x3[l].get_m_a(0);
						double num27 = ((IOn3dPoint)nurbsCurve.PointAt(num26)).DistanceTo((IOn3dPoint)cSGemData.Center.Convert());
						if (!RhinoMath.IsValidDouble(num24) || !RhinoMath.IsValidDouble(num25) || !(num27 >= num25))
						{
							num24 = num26;
							num25 = num27;
						}
					}
					On3dPoint on3dPoint3 = nurbsCurve.PointAt(num24);
					On3dVector on3dVector7 = CSBlockUtil.CSUtilMath.AverageTangentAt(nurbsCurve, on3dPoint3);
					on3dVector7.Unitize();
					if (OnUtil.ON_DotProduct(cSChannelGemData2.ForwardVector.Convert(), on3dVector7) < 0.0)
					{
						on3dVector7.Reverse();
					}
					if (!BuildBlendedProng(cSGemData, nurbs_curve, value11, value12, value13, value3, value21, value18, value19, value20, on3dPoint3, on3dVector7, onCircle4, concurrentBag, concurrentBag2, concurrentBag3, out var msg3))
					{
						return UpdateError("BuildBlendProng:  " + msg3);
					}
				}
				else
				{
					double taperPer3 = value12 / value11 - 1.0;
					if (!_prongCache.Create(nurbs_curve, value11, value13, value3, taperPer3, value, out var prong3, out var prongBaseRail3))
					{
						return UpdateError("Create prong cache failed");
					}
					OnXform onXform3 = new OnXform();
					onXform3.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
					prong3.Transform(onXform3);
					prongBaseRail3.Transform(onXform3);
					concurrentBag.Add(new CSOnGeometryData(prong3.Convert(), onXform3.Convert(), applyTransform: false));
					concurrentBag2.Add(new CSOnGeometryData(prongBaseRail3.Convert(), onXform3.Convert(), applyTransform: false));
				}
			}
			if (whichItem != ValueListItem.ClusterProngStyle1 && (!flag2 || whichItem != ValueListItem.ClusterProngStyle2) && (!flag || whichItem != ValueListItem.ClusterProngStyle3) && (!flag2 || whichItem != ValueListItem.ClusterProngStyle4) && (!flag || whichItem != ValueListItem.ClusterProngStyle5) && (!flag2 || whichItem != ValueListItem.ClusterProngStyle6) && (!flag || whichItem != ValueListItem.ClusterProngStyle7) && (!flag2 || whichItem != ValueListItem.ClusterProngStyle8) && (!flag || whichItem != ValueListItem.ClusterProngStyle9))
			{
				continue;
			}
			if (value16)
			{
				On3dVector on3dVector8 = cSChannelGemData2.Center.Convert() - cSGemData.Center.Convert();
				on3dVector8.Unitize();
				OnPlaneSurface onPlaneSurface4 = new OnPlaneSurface(new OnPlane(cSGemData.Center.Convert(), cSGemData.Normal.Convert(), on3dVector8));
				onPlaneSurface4.SetExtents(0, new OnInterval(-50.0, 50.0));
				onPlaneSurface4.SetExtents(1, new OnInterval(0.0, 50.0));
				ArrayOnX_EVENT x4 = new ArrayOnX_EVENT();
				nurbsCurve.IntersectSurface(onPlaneSurface4, ref x4);
				if (x4.Count() <= 0)
				{
					return UpdateError("Intersection failed");
				}
				double num28 = -1.23432101234321E+308;
				double num29 = -1.23432101234321E+308;
				for (int m = 0; m < x4.Count(); m++)
				{
					double num30 = x4[m].get_m_a(0);
					double num31 = ((IOn3dPoint)nurbsCurve.PointAt(num30)).DistanceTo((IOn3dPoint)cSGemData.Center.Convert());
					if (!RhinoMath.IsValidDouble(num28) || !RhinoMath.IsValidDouble(num29) || !(num31 >= num29))
					{
						num28 = num30;
						num29 = num31;
					}
				}
				On3dPoint on3dPoint4 = nurbsCurve.PointAt(num28);
				On3dVector on3dVector9 = CSBlockUtil.CSUtilMath.AverageTangentAt(nurbsCurve, on3dPoint4);
				on3dVector9.Unitize();
				if (OnUtil.ON_DotProduct(cSChannelGemData2.ForwardVector.Convert(), on3dVector9) < 0.0)
				{
					on3dVector9.Reverse();
				}
				if (!BuildBlendedProng(cSGemData, nurbs_curve, value6, value7, value8, value3, value21, value18, value19, value20, on3dPoint4, on3dVector9, onCircle5, concurrentBag, concurrentBag2, concurrentBag3, out var msg4))
				{
					return UpdateError("BuildBlendProng:  " + msg4);
				}
			}
			else
			{
				double taperPer4 = value7 / value6 - 1.0;
				if (!_prongCache.Create(nurbs_curve, value6, value8, value3, taperPer4, value, out var prong4, out var prongBaseRail4))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform4 = new OnXform();
				onXform4.ChangeBasis(onCircle5.plane, OnUtil.On_xy_plane);
				prong4.Transform(onXform4);
				prongBaseRail4.Transform(onXform4);
				concurrentBag.Add(new CSOnGeometryData(prong4.Convert(), onXform4.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(prongBaseRail4.Convert(), onXform4.Convert(), applyTransform: false));
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
		msg = string.Empty;
		double radius = diameterTop / 2.0;
		double radius2 = diameterBase / 2.0;
		double num = CalculateProngTipHeightToDome(diameterTop, height, filletPer);
		double num2 = (CSApplicationHelper.Application.IsCamMode ? CSApplicationHelper.Application.CamProngExtensionHeight : 0.0);
		OnCircle onCircle = new OnCircle(new OnPlane(new On3dPoint(circle.Center()) + new On3dVector(circle.Normal()) * (num + num2), circle.plane.xaxis, circle.plane.yaxis), radius);
		OnCircle onCircle2 = new OnCircle(new OnPlane(new On3dPoint(circle.Center()) + new On3dVector(circle.Normal()) * num, circle.plane.xaxis, circle.plane.yaxis), radius);
		OnCircle onCircle3 = new OnCircle(new OnPlane(blendToPoint, circle.plane.xaxis, circle.plane.yaxis), radius2);
		On3dVector on3dVector = OnUtil.ON_CrossProduct(new On3dPoint(blendToPoint) - centerGem.Center.Convert(), centerGem.Normal.Convert());
		on3dVector.Unitize();
		onCircle3.Rotate(CSBlockUtil.CSUtilMath.ToRadians(blendTilt), on3dVector);
		if (!BuildSweepRail(onCircle, onCircle2, onCircle3, blendAmount, blendAmountTop, blendAmountBase, blendTilt, blendToPointTangent, new On3dVector(circle.plane.yaxis) * 1.0, out var sweepRail, out var msg2))
		{
			msg = "BuildSweepRail failed:  " + msg2;
			return false;
		}
		rails.Add(new CSOnGeometryData(sweepRail.Convert()));
		if (!BuildSweepRail(onCircle, onCircle2, onCircle3, blendAmount, blendAmountTop, blendAmountBase, blendTilt, blendToPointTangent, new On3dVector(circle.plane.yaxis) * -1.0, out var sweepRail2, out var msg3))
		{
			msg = "BuildSweepRail failed:  " + msg3;
			return false;
		}
		rails.Add(new CSOnGeometryData(sweepRail2.Convert()));
		OnXform onXform = new OnXform();
		onXform.ChangeBasis(onCircle.plane, OnUtil.On_xy_plane);
		OnXform onXform2 = new OnXform();
		onXform2.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
		OnXform onXform3 = new OnXform();
		onXform3.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
		if (!_prongDomeCache.Create(prongProfile, diameterTop, filletPer, 0.0, out var prongDome))
		{
			msg = "Create prong dome cache failed";
			return false;
		}
		prongDome.Transform(CSApplicationHelper.Application.IsCamMode ? onXform : onXform2);
		OnNurbsCurve onNurbsCurve = BuildSweepProfile(prongProfile, diameterTop);
		onNurbsCurve.Transform(onXform2);
		profiles.Add(new CSOnGeometryData(onNurbsCurve.Convert(), onXform2.Convert(), applyTransform: false));
		OnNurbsCurve onNurbsCurve2 = BuildSweepProfile(prongProfile, diameterBase);
		onNurbsCurve2.Transform(onXform3);
		profiles.Add(new CSOnGeometryData(onNurbsCurve2.Convert(), onXform3.Convert(), applyTransform: false));
		IOnCurve[] profiles2;
		if (!CSApplicationHelper.Application.IsCamMode)
		{
			profiles2 = new IOnCurve[2] { onNurbsCurve, onNurbsCurve2 };
		}
		else
		{
			OnNurbsCurve onNurbsCurve3 = BuildSweepProfile(prongProfile, diameterTop);
			onNurbsCurve3.Transform(onXform);
			profiles.Add(new CSOnGeometryData(onNurbsCurve3.Convert(), onXform.Convert(), applyTransform: false));
			profiles2 = new IOnCurve[3] { onNurbsCurve3, onNurbsCurve, onNurbsCurve2 };
		}
		if (!CSUtilSweep.Sweep2NoParam(sweepRail, sweepRail2, profiles2, out var breps, maintainHeight: false, firstAtStart: false, closed: false, flipProfiles: false))
		{
			msg = "Sweep failed";
			return false;
		}
		if (breps.Length == 0)
		{
			msg = "Sweep did not produce any results";
			return false;
		}
		OnBrep onBrep = breps[0];
		OnBrep onBrep2 = CSUtilRhino.JoinAllBreps(new OnBrep[2]
		{
			prongDome.BrepForm(),
			onBrep
		}, CSApplicationHelper.Application.Tolerance);
		if (onBrep2 == null)
		{
			msg = "RhinoJoinBreps failed";
			return false;
		}
		OnBrep brep = CSUtilRhino.CapPlanarHoles(onBrep2, CSApplicationHelper.Application.Tolerance);
		if (brep == null)
		{
			msg = "RhinoBrepCapPlanarHoles failed";
			return false;
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref brep, doFullClean: true);
		prongs.Add(new CSOnGeometryData(brep.Convert()));
		return true;
	}

	private static OnNurbsCurve BuildSweepProfile(IOnNurbsCurve profile, double diameter)
	{
		OnBoundingBox tight_bbox = new OnBoundingBox();
		profile.GetTightBoundingBox(ref tight_bbox);
		double num = tight_bbox.Max().x - tight_bbox.Min().x;
		double num2 = tight_bbox.Max().y - tight_bbox.Min().y;
		OnXform onXform = new OnXform();
		onXform.Scale(diameter / num, diameter / num2, 1.0);
		OnXform onXform2 = new OnXform();
		onXform2.Translation(new On3dPoint(OnUtil.On_origin) - tight_bbox.Center());
		OnNurbsCurve onNurbsCurve = OnNurbsCurve.New(profile);
		onNurbsCurve.Transform(onXform * onXform2);
		return onNurbsCurve;
	}

	private static bool BuildSweepRail(IOnCircle circleTopExtra, IOnCircle circleTop, IOnCircle circleBase, double blendAmount, double blendAmountTop, double blendAmountBase, double blendTilt, IOn3dVector blendToPointTangent, IOn3dVector dir, out OnCurve sweepRail, out string msg)
	{
		sweepRail = null;
		msg = string.Empty;
		On3dPoint pointStart = new On3dPoint(circleTop.Center()) + new On3dVector(dir) * circleTop.radius;
		On3dPoint on3dPoint = new On3dPoint(circleBase.Center()) + new On3dVector(dir) * circleBase.radius;
		on3dPoint.Rotate(CSBlockUtil.CSUtilMath.ToRadians(blendTilt), blendToPointTangent, circleBase.Center());
		On3dVector on3dVector = new On3dVector(circleTop.Normal());
		on3dVector.Reverse();
		on3dVector.Unitize();
		On3dVector on3dVector2 = new On3dVector(circleBase.Normal());
		on3dVector2.Unitize();
		OnNurbsCurve onNurbsCurve = CSUtilCurves.BlendCurve(pointStart, on3dPoint, on3dVector, on3dVector2, blendAmount, blendAmountTop, blendAmountBase);
		if (!CSApplicationHelper.Application.IsCamMode)
		{
			sweepRail = onNurbsCurve;
			return true;
		}
		On3dPoint arg = new On3dPoint(circleTopExtra.Center()) + new On3dVector(dir) * circleTopExtra.radius;
		On3dPoint arg2 = new On3dPoint(circleTop.Center()) + new On3dVector(dir) * circleTop.radius;
		OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
		IOnCurve[] input_curves = new IOnCurve[2] { onLineCurve, onNurbsCurve };
		OnCurve[] output = null;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			msg = "RhinoMergeCurves failed";
			return false;
		}
		if (output.Length == 0)
		{
			msg = "RhinoMergeCurves failed to produce results";
			return false;
		}
		sweepRail = output[0];
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
