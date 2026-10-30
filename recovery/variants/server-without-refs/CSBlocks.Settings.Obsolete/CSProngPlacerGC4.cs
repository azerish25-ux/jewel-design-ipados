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
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
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
			OnCircle val = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			vProfile = new OnNurbsCurve();
			val.GetNurbForm(ref vProfile);
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
			double num = -1.23432101234321E+308;
			if (!railLeft.GetLength(ref num))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref railLeft, (Type)2, 0, num * 0.5);
			RhUtil.RhinoExtendCurve(ref railLeft, (Type)2, 1, num * 0.5);
		}
		OnCurve railRight = CSUtilGemRails.Right(channelGemDatas, rail).DuplicateCurve();
		if (!railRight.IsClosed())
		{
			double num2 = -1.23432101234321E+308;
			if (!railRight.GetLength(ref num2))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref railRight, (Type)2, 0, num2 * 0.5);
			RhUtil.RhinoExtendCurve(ref railRight, (Type)2, 1, num2 * 0.5);
		}
		_prongCache.Clear();
		int gemCount = channelGemDatas.GetArrayCount();
		ParallelOptions parallelOptions = GetBlockParallelOptions();
		bool isClosed = CSUtilGemRails.IsClosed(channelGemDatas.Array);
		Parallel.For(0, gemCount + 1, parallelOptions, delegate(int i, ParallelLoopState loopState)
		{
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Expected O, but got Unknown
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Expected O, but got Unknown
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Expected O, but got Unknown
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Expected O, but got Unknown
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Expected O, but got Unknown
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Expected O, but got Unknown
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Expected O, but got Unknown
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Expected O, but got Unknown
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Expected O, but got Unknown
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
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
					On3dPoint val2 = (new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) + (IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) / 2.0;
					OnCircle circleEdgeRight;
					if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData2, cSChannelGemData3, (IOnCurve)(object)railLeft, vEdgeNudge, out var circleEdgeLeft))
					{
						UpdateError("CalculateCircleEdgeLeft failed");
						loopState.Stop();
					}
					else if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData2, cSChannelGemData3, (IOnCurve)(object)railRight, vEdgeNudge, out circleEdgeRight))
					{
						UpdateError("CalculateCircleEdgeRight failed");
						loopState.Stop();
					}
					else
					{
						OnCircle val3 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - vNudge);
						OnCircle val4 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), (IOn3dVector)(object)cSChannelGemData3.Normal.Convert()), cSChannelGemData3.SizeAverage - vNudge);
						OnCircle val5 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val3, (IOnCircle)(object)val4, (IOnCircle)(object)circleEdgeLeft);
						OnCircle val6 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val3, (IOnCircle)(object)val4, (IOnCircle)(object)circleEdgeRight);
						On3dPoint obj = val3.ClosestPointTo(val4.Center());
						On3dPoint val7 = val4.ClosestPointTo(val3.Center());
						OnLineCurve val8 = new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val7);
						OnLineCurve val9 = new OnLineCurve(val3.Center(), val4.Center());
						On3dPoint val10 = ((OnCurve)val8).PointAt(((OnCurve)val8).Domain().Mid());
						double num3 = -1.23432101234321E+308;
						if (!((OnCurve)val9).GetClosestPoint((IOn3dPoint)(object)val10, ref num3))
						{
							UpdateError("GetClosestPoint failed");
							loopState.Stop();
						}
						else
						{
							On3dPoint val11 = ((OnCurve)val9).PointAt(num3);
							double num4 = val10.DistanceTo((IOn3dPoint)(object)val11);
							double num5 = (cSChannelGemData2.GemZMin + cSChannelGemData3.GemZMin) / 2.0;
							double height = (cSChannelGemData2.GemZSize + cSChannelGemData3.GemZSize) / 2.0 + num4 + vHeight - vVerticalLocation;
							On3dVector val12 = new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData2.Normal.Convert();
							val12.Unitize();
							double num6 = 0.0;
							val5.ClosestPointTo((IOn3dPoint)(object)val2, ref num6);
							val5.Rotate(num6 + OnUtil.On_PI / 2.0, val5.Normal());
							val5.Translate((IOn3dVector)(object)(val12 * (vVerticalLocation - num5)));
							double num7 = 0.0;
							val6.ClosestPointTo((IOn3dPoint)(object)val2, ref num7);
							val6.Rotate(num7 + OnUtil.On_PI / 2.0, val6.Normal());
							val6.Translate((IOn3dVector)(object)(val12 * (vVerticalLocation - num5)));
							if (val5.radius * 2.0 > vSizeMin && val5.radius * 2.0 < vSizeMax)
							{
								if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, val5.radius * 2.0, height, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong, out var prongBaseRail))
								{
									UpdateError("Create prong cache failed");
									loopState.Stop();
									return;
								}
								OnXform val13 = new OnXform();
								val13.ChangeBasis((IOnPlane)(object)val5.plane, OnUtil.On_xy_plane);
								prong.Transform((IOnXform)(object)val13);
								prongBaseRail.Transform((IOnXform)(object)val13);
								prongs.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val13.Convert(), applyTransform: false));
								profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val13.Convert(), applyTransform: false));
							}
							if (val6.radius * 2.0 > vSizeMin && val6.radius * 2.0 < vSizeMax)
							{
								if (!_prongCache.Create((IOnNurbsCurve)(object)vProfile, val6.radius * 2.0, height, vFilletPer, vTaperPer, vDoMeshedProngs, out var prong2, out var prongBaseRail2))
								{
									UpdateError("Create prong cache failed");
									loopState.Stop();
									return;
								}
								OnXform val14 = new OnXform();
								val14.ChangeBasis((IOnPlane)(object)val6.plane, OnUtil.On_xy_plane);
								prong2.Transform((IOnXform)(object)val14);
								prongBaseRail2.Transform((IOnXform)(object)val14);
								prongs.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val14.Convert(), applyTransform: false));
								profiles.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val14.Convert(), applyTransform: false));
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
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Expected O, but got Unknown
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Expected O, but got Unknown
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		if (!(InSortedGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
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
		OnCurve val3 = CSUtilGemRails.Left(cSDataArray, rail).DuplicateCurve();
		if (!val3.IsClosed())
		{
			double num = -1.23432101234321E+308;
			if (!val3.GetLength(ref num))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 0, num * 0.5);
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 1, num * 0.5);
		}
		OnCurve val4 = CSUtilGemRails.Right(cSDataArray, rail).DuplicateCurve();
		if (!val4.IsClosed())
		{
			double num2 = -1.23432101234321E+308;
			if (!val4.GetLength(ref num2))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 0, num2 * 0.5);
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 1, num2 * 0.5);
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
			On3dPoint val5 = (new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) + (IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) / 2.0;
			if (!CSUtilProngPlacer.CalculateCircleEdgeLeft(cSChannelGemData2, cSChannelGemData3, (IOnCurve)(object)val3, value8, out var circleEdgeLeft))
			{
				return UpdateError("CalculateCircleEdgeLeft failed");
			}
			if (!CSUtilProngPlacer.CalculateCircleEdgeRight(cSChannelGemData2, cSChannelGemData3, (IOnCurve)(object)val4, value8, out var circleEdgeRight))
			{
				return UpdateError("CalculateCircleEdgeRight failed");
			}
			OnCircle val6 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value4);
			OnCircle val7 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), (IOn3dVector)(object)cSChannelGemData3.Normal.Convert()), cSChannelGemData3.SizeAverage - value4);
			OnCircle val8 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val6, (IOnCircle)(object)val7, (IOnCircle)(object)circleEdgeLeft);
			OnCircle val9 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val6, (IOnCircle)(object)val7, (IOnCircle)(object)circleEdgeRight);
			On3dPoint obj = val6.ClosestPointTo(val7.Center());
			On3dPoint val10 = val7.ClosestPointTo(val6.Center());
			OnLineCurve val11 = new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val10);
			OnLineCurve val12 = new OnLineCurve(val6.Center(), val7.Center());
			On3dPoint val13 = ((OnCurve)val11).PointAt(((OnCurve)val11).Domain().Mid());
			double num3 = -1.23432101234321E+308;
			if (!((OnCurve)val12).GetClosestPoint((IOn3dPoint)(object)val13, ref num3))
			{
				return UpdateError("GetClosestPoint failed");
			}
			On3dPoint val14 = ((OnCurve)val12).PointAt(num3);
			double num4 = val13.DistanceTo((IOn3dPoint)(object)val14);
			double num5 = (cSChannelGemData2.GemZMin + cSChannelGemData3.GemZMin) / 2.0;
			double height = (cSChannelGemData2.GemZSize + cSChannelGemData3.GemZSize) / 2.0 + num4 + value7 - value9;
			On3dVector val15 = new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData2.Normal.Convert();
			val15.Unitize();
			double num6 = 0.0;
			val8.ClosestPointTo((IOn3dPoint)(object)val5, ref num6);
			val8.Rotate(num6 + OnUtil.On_PI / 2.0, val8.Normal());
			val8.Translate((IOn3dVector)(object)(val15 * (value9 - num5)));
			double num7 = 0.0;
			val9.ClosestPointTo((IOn3dPoint)(object)val5, ref num7);
			val9.Rotate(num7 + OnUtil.On_PI / 2.0, val9.Normal());
			val9.Translate((IOn3dVector)(object)(val15 * (value9 - num5)));
			if (val8.radius * 2.0 > value5 && val8.radius * 2.0 < value6)
			{
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, val8.radius * 2.0, height, value2, value3, value, out var prong, out var prongBaseRail))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val16 = new OnXform();
				val16.ChangeBasis((IOnPlane)(object)val8.plane, OnUtil.On_xy_plane);
				prong.Transform((IOnXform)(object)val16);
				prongBaseRail.Transform((IOnXform)(object)val16);
				concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong).Convert(), val16.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail).Convert(), val16.Convert(), applyTransform: false));
			}
			if (val9.radius * 2.0 > value5 && val9.radius * 2.0 < value6)
			{
				if (!_prongCache.Create((IOnNurbsCurve)(object)val, val9.radius * 2.0, height, value2, value3, value, out var prong2, out var prongBaseRail2))
				{
					return UpdateError("Create prong cache failed");
				}
				OnXform val17 = new OnXform();
				val17.ChangeBasis((IOnPlane)(object)val9.plane, OnUtil.On_xy_plane);
				prong2.Transform((IOnXform)(object)val17);
				prongBaseRail2.Transform((IOnXform)(object)val17);
				concurrentBag.Add(new CSOnGeometryData(((OnObject)(object)prong2).Convert(), val17.Convert(), applyTransform: false));
				concurrentBag2.Add(new CSOnGeometryData(((OnObject)(object)prongBaseRail2).Convert(), val17.Convert(), applyTransform: false));
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
