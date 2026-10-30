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
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerGC_V4", "{AB6E7458-242F-43AF-BE66-B6A3871DF69E}")]
public sealed class CSProngPlacerGC4 : CSBlock
{
	private readonly CSProngCache _prongCache = new CSProngCache();

	public CSInputChannel InDoMeshedProngs { get; }

	public CSInputChannel InEdgeNudge { get; }

	public CSInputChannel InFillet { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InNudge { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InSizeMax { get; }

	public CSInputChannel InSizeMin { get; }

	public CSInputChannel InSortedGems { get; }

	public CSInputChannel InTaper { get; }

	public CSInputChannel InVerticalLocation { get; }

	public CSOutputChannel OutProfiles { get; }

	public CSOutputChannel OutProngs { get; }

	public CSProngPlacerGC4(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(GetPropertyName(() => InSortedGems), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(GetPropertyName(() => InProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InFillet = new CSInputChannel(GetPropertyName(() => InFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InTaper = new CSInputChannel(GetPropertyName(() => InTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudge = new CSInputChannel(GetPropertyName(() => InNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMin = new CSInputChannel(GetPropertyName(() => InSizeMin), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSizeMax = new CSInputChannel(GetPropertyName(() => InSizeMax), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InHeight = new CSInputChannel(GetPropertyName(() => InHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEdgeNudge = new CSInputChannel(GetPropertyName(() => InEdgeNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVerticalLocation = new CSInputChannel(GetPropertyName(() => InVerticalLocation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InDoMeshedProngs = new CSInputChannel(GetPropertyName(() => InDoMeshedProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutProngs = new CSOutputChannel(GetPropertyName(() => OutProngs), this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProfiles = new CSOutputChannel(GetPropertyName(() => OutProfiles), this, ChannelDataType.Profile, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		if (!CSApplicationHelper.Application.IsBlockParallelProcessingEnabled)
		{
			return UpdateSync();
		}
		return UpdateConcurrent();
	}

	private bool UpdateConcurrent()
	{
		InitializeCancellationTokenSource();
		CSDataArray channelGemDatas = InSortedGems.Data as CSDataArray;
		if (channelGemDatas == null)
		{
			return UpdateMissing("Gems");
		}
		if (channelGemDatas.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
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
		bool vDoMeshedProngs = true;
		InDoMeshedProngs.GetBoolean(ref vDoMeshedProngs);
		double vFilletPer = 0.0;
		InFillet.GetDoublePercentage(ref vFilletPer);
		double vTaperPer = 0.0;
		InTaper.GetDoublePercentage(ref vTaperPer);
		double vNudge = 0.0;
		InNudge.GetDouble(ref vNudge);
		double vSizeMin = 0.0;
		InSizeMin.GetDouble(ref vSizeMin);
		double vSizeMax = 100.0;
		InSizeMax.GetDouble(ref vSizeMax);
		double vHeight = 0.0;
		InHeight.GetDouble(ref vHeight);
		double vEdgeNudge = 0.0;
		InEdgeNudge.GetDouble(ref vEdgeNudge);
		double vVerticalLocation = 0.0;
		InVerticalLocation.GetDouble(ref vVerticalLocation);
		ConcurrentBag<CSData> prongs = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> profiles = new ConcurrentBag<CSData>();
		OnCurve rail = CSUtilGemRails.Rail(channelGemDatas);
		OnCurve railLeft = CSUtilGemRails.Left(channelGemDatas, rail).DuplicateCurve();
		if (!railLeft.IsClosed())
		{
			double length = -1.23432101234321E+308;
			if (!railLeft.GetLength(ref length))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref railLeft, IRhinoExtend.Type.Smooth, 0, length * 0.5);
			RhUtil.RhinoExtendCurve(ref railLeft, IRhinoExtend.Type.Smooth, 1, length * 0.5);
		}
		OnCurve railRight = CSUtilGemRails.Right(channelGemDatas, rail).DuplicateCurve();
		if (!railRight.IsClosed())
		{
			double length2 = -1.23432101234321E+308;
			if (!railRight.GetLength(ref length2))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref railRight, IRhinoExtend.Type.Smooth, 0, length2 * 0.5);
			RhUtil.RhinoExtendCurve(ref railRight, IRhinoExtend.Type.Smooth, 1, length2 * 0.5);
		}
		_prongCache.Clear();
		int gemCount = channelGemDatas.GetArrayCount();
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		bool isClosed = CSUtilGemRails.IsClosed(channelGemDatas.Array);
		Parallel.For(0, gemCount + 1, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			if (!base.Error && !loopState.ShouldExitCurrentIteration && !parallelOptions.CancellationToken.IsCancellationRequested && !loopState.IsStopped && !loopState.IsExceptional)
			{
				CSChannelGemData cSChannelGemData = null;
				if (isClosed)
				{
					cSChannelGemData = CSUtilGemRails.GetChannelGemData(channelGemDatas, (i - 2 + gemCount) % gemCount);
				}
				else if (i - 2 >= 0)
				{
					cSChannelGemData = CSUtilGemRails.GetChannelGemData(channelGemDatas, i - 2);
				}
				CSChannelGemData cSChannelGemData2 = null;
				if (isClosed)
				{
					cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(channelGemDatas, (i - 1 + gemCount) % gemCount);
				}
				else if (i - 1 >= 0)
				{
					cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i - 1);
				}
				CSChannelGemData cSChannelGemData3 = null;
				if (isClosed)
				{
					cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i % gemCount);
				}
				else if (i < channelGemDatas.GetArrayCount())
				{
					cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i);
				}
				CSChannelGemData cSChannelGemData4 = null;
				if (isClosed)
				{
					cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(channelGemDatas, (i + 1) % gemCount);
				}
				else if (i + 1 < channelGemDatas.GetArrayCount())
				{
					cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(channelGemDatas, i + 1);
				}
				if (cSChannelGemData2 == null && cSChannelGemData4 != null && cSChannelGemData3 != null)
				{
					cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
					cSChannelGemData2.ApplyTransform(cSChannelGemData3.GetMirrorForwardBack());
					cSChannelGemData2.UnitForwardVector = -cSChannelGemData2.UnitForwardVector;
				}
				if (cSChannelGemData3 == null && cSChannelGemData2 != null && cSChannelGemData != null)
				{
					cSChannelGemData3 = new CSChannelGemData(cSChannelGemData);
					cSChannelGemData3.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
					cSChannelGemData3.UnitForwardVector = -cSChannelGemData3.UnitForwardVector;
				}
				if (cSChannelGemData3 != null && cSChannelGemData2 != null)
				{
					CSDataHelper.SetSettingType(cSChannelGemData3, GemSettingType.PR);
					On3dPoint point = (new On3dPoint(cSChannelGemData3.Center.Convert()) + cSChannelGemData2.Center.Convert()) / 2.0;
					OnCircle circleEdgeRight;
					if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData2, cSChannelGemData3, railLeft, vEdgeNudge, out var circleEdgeLeft))
					{
						UpdateError("CalculateCircleEdgeLeft failed");
						loopState.Stop();
					}
					else if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData2, cSChannelGemData3, railRight, vEdgeNudge, out circleEdgeRight))
					{
						UpdateError("CalculateCircleEdgeRight failed");
						loopState.Stop();
					}
					else
					{
						OnCircle onCircle2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - vNudge);
						OnCircle onCircle3 = new OnCircle(new OnPlane(cSChannelGemData3.Center.Convert(), cSChannelGemData3.Normal.Convert()), cSChannelGemData3.SizeAverage - vNudge);
						OnCircle onCircle4 = CSUtilGeometry.CalculateApolloniusCircle(onCircle2, onCircle3, circleEdgeLeft);
						OnCircle onCircle5 = CSUtilGeometry.CalculateApolloniusCircle(onCircle2, onCircle3, circleEdgeRight);
						On3dPoint arg = onCircle2.ClosestPointTo(onCircle3.Center());
						On3dPoint arg2 = onCircle3.ClosestPointTo(onCircle2.Center());
						OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
						OnLineCurve onLineCurve2 = new OnLineCurve(onCircle2.Center(), onCircle3.Center());
						On3dPoint on3dPoint = onLineCurve.PointAt(onLineCurve.Domain().Mid());
						double t = -1.23432101234321E+308;
						if (!onLineCurve2.GetClosestPoint(on3dPoint, ref t))
						{
							UpdateError("GetClosestPoint failed");
							loopState.Stop();
						}
						else
						{
							On3dPoint arg3 = onLineCurve2.PointAt(t);
							double num = on3dPoint.DistanceTo(arg3);
							double num2 = (cSChannelGemData2.GemZMin + cSChannelGemData3.GemZMin) / 2.0;
							double height = (cSChannelGemData2.GemZSize + cSChannelGemData3.GemZSize) / 2.0 + num + vHeight - vVerticalLocation;
							On3dVector on3dVector = new On3dVector(cSChannelGemData3.Normal.Convert()) + cSChannelGemData2.Normal.Convert();
							on3dVector.Unitize();
							double t2 = 0.0;
							onCircle4.ClosestPointTo(point, ref t2);
							onCircle4.Rotate(t2 + OnUtil.On_PI / 2.0, onCircle4.Normal());
							onCircle4.Translate(on3dVector * (vVerticalLocation - num2));
							double t3 = 0.0;
							onCircle5.ClosestPointTo(point, ref t3);
							onCircle5.Rotate(t3 + OnUtil.On_PI / 2.0, onCircle5.Normal());
							onCircle5.Translate(on3dVector * (vVerticalLocation - num2));
							if (onCircle4.radius * 2.0 > vSizeMin && onCircle4.radius * 2.0 < vSizeMax)
							{
								if (!_prongCache.Create(vProfile, onCircle4.radius * 2.0, height, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong, out var prongBaseRail))
								{
									UpdateError("Create prong cache failed");
									loopState.Stop();
									return;
								}
								OnXform onXform = new OnXform();
								onXform.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
								prong.Transform(onXform);
								prongBaseRail.Transform(onXform);
								prongs.Add(new CSOnGeometryData(prong.Convert(), onXform.Convert(), applyTransform: false));
								profiles.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform.Convert(), applyTransform: false));
							}
							if (onCircle5.radius * 2.0 > vSizeMin && onCircle5.radius * 2.0 < vSizeMax)
							{
								if (!_prongCache.Create(vProfile, onCircle5.radius * 2.0, height, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong2, out var prongBaseRail2))
								{
									UpdateError("Create prong cache failed");
									loopState.Stop();
									return;
								}
								OnXform onXform2 = new OnXform();
								onXform2.ChangeBasis(onCircle5.plane, OnUtil.On_xy_plane);
								prong2.Transform(onXform2);
								prongBaseRail2.Transform(onXform2);
								prongs.Add(new CSOnGeometryData(prong2.Convert(), onXform2.Convert(), applyTransform: false));
								profiles.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform2.Convert(), applyTransform: false));
							}
							CS.CSUtil.CheckAndTerminateOrphanedParallelTasks();
						}
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
		if (!(InSortedGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
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
		bool value = true;
		InDoMeshedProngs.GetBoolean(ref value);
		double value2 = 0.0;
		InFillet.GetDoublePercentage(ref value2);
		double value3 = 0.0;
		InTaper.GetDoublePercentage(ref value3);
		double value4 = 0.0;
		InNudge.GetDouble(ref value4);
		double value5 = 0.0;
		InSizeMin.GetDouble(ref value5);
		double value6 = 100.0;
		InSizeMax.GetDouble(ref value6);
		double value7 = 0.0;
		InHeight.GetDouble(ref value7);
		double value8 = 0.0;
		InEdgeNudge.GetDouble(ref value8);
		double value9 = 0.0;
		InVerticalLocation.GetDouble(ref value9);
		ConcurrentBag<CSData> concurrentBag = new ConcurrentBag<CSData>();
		ConcurrentBag<CSData> concurrentBag2 = new ConcurrentBag<CSData>();
		OnCurve rail = CSUtilGemRails.Rail(cSDataArray);
		OnCurve crv = CSUtilGemRails.Left(cSDataArray, rail).DuplicateCurve();
		if (!crv.IsClosed())
		{
			double length = -1.23432101234321E+308;
			if (!crv.GetLength(ref length))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Smooth, 0, length * 0.5);
			RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Smooth, 1, length * 0.5);
		}
		OnCurve crv2 = CSUtilGemRails.Right(cSDataArray, rail).DuplicateCurve();
		if (!crv2.IsClosed())
		{
			double length2 = -1.23432101234321E+308;
			if (!crv2.GetLength(ref length2))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Smooth, 0, length2 * 0.5);
			RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Smooth, 1, length2 * 0.5);
		}
		_prongCache.Clear();
		int arrayCount = cSDataArray.GetArrayCount();
		bool flag = CSUtilGemRails.IsClosed(cSDataArray.Array);
		for (int i = 0; i < arrayCount + 1; i++)
		{
			CSChannelGemData cSChannelGemData = null;
			if (flag)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 2 + arrayCount) % arrayCount);
			}
			else if (i - 2 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 2);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (flag)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (flag)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < cSDataArray.GetArrayCount())
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData4 = null;
			if (flag)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i + 1) % arrayCount);
			}
			else if (i + 1 < cSDataArray.GetArrayCount())
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, i + 1);
			}
			if (cSChannelGemData2 == null && cSChannelGemData4 != null && cSChannelGemData3 != null)
			{
				cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
				cSChannelGemData2.ApplyTransform(cSChannelGemData3.GetMirrorForwardBack());
				cSChannelGemData2.UnitForwardVector = -cSChannelGemData2.UnitForwardVector;
			}
			if (cSChannelGemData3 == null && cSChannelGemData2 != null && cSChannelGemData != null)
			{
				cSChannelGemData3 = new CSChannelGemData(cSChannelGemData);
				cSChannelGemData3.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
				cSChannelGemData3.UnitForwardVector = -cSChannelGemData3.UnitForwardVector;
			}
			if (cSChannelGemData3 == null || cSChannelGemData2 == null)
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSChannelGemData3, GemSettingType.PR);
			On3dPoint point = (new On3dPoint(cSChannelGemData3.Center.Convert()) + cSChannelGemData2.Center.Convert()) / 2.0;
			if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData2, cSChannelGemData3, crv, value8, out var circleEdgeLeft))
			{
				return UpdateError("CalculateCircleEdgeLeft failed");
			}
			if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData2, cSChannelGemData3, crv2, value8, out var circleEdgeRight))
			{
				return UpdateError("CalculateCircleEdgeRight failed");
			}
			OnCircle onCircle2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value4);
			OnCircle onCircle3 = new OnCircle(new OnPlane(cSChannelGemData3.Center.Convert(), cSChannelGemData3.Normal.Convert()), cSChannelGemData3.SizeAverage - value4);
			OnCircle onCircle4 = CSUtilGeometry.CalculateApolloniusCircle(onCircle2, onCircle3, circleEdgeLeft);
			OnCircle onCircle5 = CSUtilGeometry.CalculateApolloniusCircle(onCircle2, onCircle3, circleEdgeRight);
			On3dPoint arg = onCircle2.ClosestPointTo(onCircle3.Center());
			On3dPoint arg2 = onCircle3.ClosestPointTo(onCircle2.Center());
			OnLineCurve onLineCurve = new OnLineCurve(arg, arg2);
			OnLineCurve onLineCurve2 = new OnLineCurve(onCircle2.Center(), onCircle3.Center());
			On3dPoint on3dPoint = onLineCurve.PointAt(onLineCurve.Domain().Mid());
			double t = -1.23432101234321E+308;
			if (!onLineCurve2.GetClosestPoint(on3dPoint, ref t))
			{
				return UpdateError("GetClosestPoint failed");
			}
			On3dPoint arg3 = onLineCurve2.PointAt(t);
			double num = on3dPoint.DistanceTo(arg3);
			double num2 = (cSChannelGemData2.GemZMin + cSChannelGemData3.GemZMin) / 2.0;
			double height = (cSChannelGemData2.GemZSize + cSChannelGemData3.GemZSize) / 2.0 + num + value7 - value9;
			On3dVector on3dVector = new On3dVector(cSChannelGemData3.Normal.Convert()) + cSChannelGemData2.Normal.Convert();
			on3dVector.Unitize();
			double t2 = 0.0;
			onCircle4.ClosestPointTo(point, ref t2);
			onCircle4.Rotate(t2 + OnUtil.On_PI / 2.0, onCircle4.Normal());
			onCircle4.Translate(on3dVector * (value9 - num2));
			double t3 = 0.0;
			onCircle5.ClosestPointTo(point, ref t3);
			onCircle5.Rotate(t3 + OnUtil.On_PI / 2.0, onCircle5.Normal());
			onCircle5.Translate(on3dVector * (value9 - num2));
			if (onCircle4.radius * 2.0 > value5 && onCircle4.radius * 2.0 < value6)
			{
				if (!_prongCache.Create(nurbs_curve, onCircle4.radius * 2.0, height, value2, value3, value, out var prong, out var prongBaseRail))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform = new OnXform();
				onXform.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
				prong.Transform(onXform);
				prongBaseRail.Transform(onXform);
				concurrentBag.Add(new CSOnGeometryData(prong.Convert(), onXform.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(prongBaseRail.Convert(), onXform.Convert(), applyTransform: false));
			}
			if (onCircle5.radius * 2.0 > value5 && onCircle5.radius * 2.0 < value6)
			{
				if (!_prongCache.Create(nurbs_curve, onCircle5.radius * 2.0, height, value2, value3, value, out var prong2, out var prongBaseRail2))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform onXform2 = new OnXform();
				onXform2.ChangeBasis(onCircle5.plane, OnUtil.On_xy_plane);
				prong2.Transform(onXform2);
				prongBaseRail2.Transform(onXform2);
				concurrentBag.Add(new CSOnGeometryData(prong2.Convert(), onXform2.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(prongBaseRail2.Convert(), onXform2.Convert(), applyTransform: false));
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
