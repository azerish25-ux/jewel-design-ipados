using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSBlocks.Shank.Helpers;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.Matching;

[CSBlock("MatchingShank", "{4AADD042-B9B1-4DCA-B7F0-E649E2679693}")]
public sealed class CSMatchingShank : CSBlock
{
	private const bool _debugShow4Rails = false;

	private const bool _debugShow4Rails2 = false;

	private const bool _debugShowChannelWalls = false;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseCurve;

	public readonly CSInputChannel InOrientationMethod;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InChannelWallWidth;

	public readonly CSInputChannel InChannelWallDrop;

	public readonly CSInputChannel InChannelWallDropStart;

	public readonly CSInputChannel InOverrideChannelWallDropStart;

	public readonly CSInputChannel InChannelWallDropMiddle;

	public readonly CSInputChannel InOverrideChannelWallDropMiddle;

	public readonly CSInputChannel InChannelWallDropEnd;

	public readonly CSInputChannel InOverrideChannelWallDropEnd;

	public readonly CSInputChannel InChannelWallGemOverlap;

	public readonly CSInputChannel InChannelBlend;

	public readonly CSInputChannel InChannelPullDown;

	public readonly CSInputChannel InShankWidthAdd;

	public readonly CSInputChannel InShowGems;

	public readonly CSInputChannel InForceChannel;

	public readonly CSInputChannel InNoChannel;

	public readonly CSInputChannel InShankClosed;

	public readonly CSInputChannel InStartChannelProfilePosition;

	public readonly CSInputChannel InStartChannelProfileY;

	public readonly CSInputChannel InEndChannelProfilePosition;

	public readonly CSInputChannel InEndChannelProfileY;

	public readonly CSInputChannel InAutoChannelDepth;

	public readonly CSInputChannel InStartShankProfileEnabled;

	public readonly CSInputChannel InStartShankProfilePosition;

	public readonly CSInputChannel InStartShankProfileWidth;

	public readonly CSInputChannel InStartShankProfileTangent;

	public readonly CSInputChannel InStartChannelProfileTangent;

	public readonly CSInputChannel InEndShankProfileEnabled;

	public readonly CSInputChannel InEndShankProfilePosition;

	public readonly CSInputChannel InEndShankProfileWidth;

	public readonly CSInputChannel InEndShankProfileTangent;

	public readonly CSInputChannel InEndChannelProfileTangent;

	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InPositionMiddle;

	public readonly CSInputChannel InChannelEnabled;

	public readonly CSInputChannel InGemsClosedLoop;

	public readonly CSInputChannel InDirection;

	public readonly CSInputChannel InOverrideStartOffset;

	public readonly CSInputChannel InOverrideMiddleOffset;

	public readonly CSInputChannel InOverrideEndOffset;

	public readonly CSInputChannel InChannelBottomOffset;

	public readonly CSInputChannel InChannelBottomOffsetStart;

	public readonly CSInputChannel InChannelBottomOffsetMiddle;

	public readonly CSInputChannel InChannelBottomOffsetEnd;

	public readonly CSInputChannel InFront;

	public readonly CSInputChannel InSideBuild;

	public readonly CSInputChannel InMetalWidthToggle;

	public readonly CSInputChannel InEndChannelWallDrop;

	public readonly CSInputChannel InProfileWidthStart;

	public readonly CSInputChannel InProfileWidthEnd;

	public readonly CSInputChannel InUseMinCulet;

	public readonly CSOutputChannel OutSurface;

	public readonly CSOutputChannel OutProfileStart;

	public readonly CSOutputChannel OutProfileEnd;

	public readonly CSOutputChannel OutSweepRailsOuter;

	public readonly CSOutputChannel OutSweepRailsInner;

	public readonly CSOutputChannel OutUntrimmed;

	public readonly CSOutputChannel OutChannelWidthStart;

	public readonly CSOutputChannel OutChannelWidthEnd;

	public readonly CSOutputChannel OutBaseSurface;

	public readonly CSOutputChannel OutBaseLeftSurface;

	public readonly CSOutputChannel OutBaseRightSurface;

	public readonly CSOutputChannel OutChannelLeftSurface;

	public readonly CSOutputChannel OutChannelRightSurface;

	public readonly CSOutputChannel OutChannelCenterSurface;

	public readonly CSOutputChannel OutTopStartSurface;

	public readonly CSOutputChannel OutTopEndSurface;

	public CSMatchingShank(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InChannelWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOverrideChannelWallDropStart = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideChannelWallDropMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideChannelWallDropEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGems = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InPositionMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InGemsClosedLoop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDirection = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InChannelWallDrop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallDropStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelWallDropMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelWallDropEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelPullDown = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShankWidthAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InForceChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InNoChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InShankClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InStartChannelProfilePosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartChannelProfileY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndChannelProfilePosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndChannelProfileY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAutoChannelDepth = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InStartShankProfileEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InStartShankProfilePosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartShankProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartShankProfileTangent = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InStartChannelProfileTangent = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEndShankProfileEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEndShankProfilePosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndShankProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndShankProfileTangent = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEndChannelProfileTangent = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InOverrideStartOffset = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideMiddleOffset = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideEndOffset = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InChannelBottomOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelBottomOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelBottomOffsetMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelBottomOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InFront = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InSideBuild = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InMetalWidthToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEndChannelWallDrop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileWidthStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3.0);
		InProfileWidthEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3.0);
		InUseMinCulet = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.NoMinCulet);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutProfileStart = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Individual);
		OutProfileEnd = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Individual);
		OutSweepRailsOuter = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutSweepRailsInner = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutUntrimmed = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutChannelWidthStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelWidthEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutBaseSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutBaseLeftSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutBaseRightSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutChannelLeftSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutChannelRightSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutChannelCenterSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutTopStartSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		OutTopEndSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		if (!CSDataHelper.GetCurve(InCurve, out var value))
		{
			return UpdateMissing("Curve");
		}
		if (!CSDataHelper.GetCurve(InBaseCurve, out var value2))
		{
			return base.Update();
		}
		ValueListItem whichItem = ValueListItem.OrientationMethodClosestPointOnCurve;
		InOrientationMethod.GetValueListItem(ref whichItem);
		CSDataHelper.GetCurve(InProfile, out var value3);
		Vector3d diagonal;
		if (value3 != null)
		{
			BoundingBox boundingBox = ((GeometryBase)value3).GetBoundingBox(true);
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double y = ((Vector3d)(ref diagonal)).Y;
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double x2 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = (0.0 - (x2 + ((Point3d)(ref val)).X)) / 2.0;
			val = ((BoundingBox)(ref boundingBox)).Min;
			Transform val2 = Transform.Translation(num, 0.0 - ((Point3d)(ref val)).Y, 0.0);
			((GeometryBase)value3).Transform(val2);
			Transform val3 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			((GeometryBase)value3).Transform(val3);
		}
		ValueListItem whichItem2 = ValueListItem.GemPlacementMethodGirdle;
		InPlacementMethod.GetValueListItem(ref whichItem2);
		double value4 = 0.0;
		InStartChannelProfilePosition.GetDouble(ref value4);
		double value5 = 0.0;
		InStartChannelProfileY.GetDouble(ref value5);
		double value6 = 0.0;
		InEndChannelProfilePosition.GetDouble(ref value6);
		double value7 = 0.0;
		InEndChannelProfileY.GetDouble(ref value7);
		double value8 = 0.0;
		InChannelWallWidth.GetDouble(ref value8);
		double value9 = 0.0;
		InChannelWallGemOverlap.GetDouble(ref value9);
		double value10 = 0.0;
		InChannelBlend.GetDouble(ref value10);
		double value11 = 0.1;
		InChannelPullDown.GetDouble(ref value11);
		double value12 = 0.0;
		InShankWidthAdd.GetDouble(ref value12);
		bool value13 = true;
		InShowGems.GetBoolean(ref value13);
		bool value14 = false;
		InForceChannel.GetBoolean(ref value14);
		bool value15 = false;
		InNoChannel.GetBoolean(ref value15);
		bool value16 = false;
		InShankClosed.GetBoolean(ref value16);
		bool value17 = false;
		InAutoChannelDepth.GetBoolean(ref value17);
		bool value18 = false;
		InStartShankProfileEnabled.GetBoolean(ref value18);
		double value19 = 0.0;
		InStartShankProfilePosition.GetDouble(ref value19);
		double value20 = 2.0;
		InStartShankProfileWidth.GetDouble(ref value20);
		bool value21 = false;
		InStartShankProfileTangent.GetBoolean(ref value21);
		bool value22 = false;
		InStartChannelProfileTangent.GetBoolean(ref value22);
		bool value23 = false;
		InEndShankProfileEnabled.GetBoolean(ref value23);
		double value24 = 0.0;
		InEndShankProfilePosition.GetDouble(ref value24);
		double value25 = 2.0;
		InEndShankProfileWidth.GetDouble(ref value25);
		bool value26 = false;
		InEndShankProfileTangent.GetBoolean(ref value26);
		bool value27 = false;
		InEndChannelProfileTangent.GetBoolean(ref value27);
		InOverrideChannelWallDropStart.GetValue(out bool value28);
		InOverrideChannelWallDropMiddle.GetValue(out bool value29);
		InOverrideChannelWallDropEnd.GetValue(out bool value30);
		InChannelWallDrop.GetValue(out double value31);
		InChannelWallDrop.GetValue(out double value32);
		InChannelWallDrop.GetValue(out double value33);
		if (value28)
		{
			InChannelWallDropStart.GetValue(out value31);
		}
		if (value29)
		{
			InChannelWallDropMiddle.GetValue(out value32);
		}
		if (value30)
		{
			InChannelWallDropEnd.GetValue(out value33);
		}
		InEndChannelWallDrop.GetValue(out double value34);
		ValueListItem valueListItem = InUseMinCulet.ValueListItem;
		List<UltimateGemData> list = new List<UltimateGemData>();
		if (!(InGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Ultimate GemData Array");
		}
		double num2 = double.MinValue;
		for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
		{
			if (!(cSDataArray.GetArrayItem(i) is CSDataArray gemInfo))
			{
				return UpdateMissing("Ultimate GemData GemArray");
			}
			UltimateGemData ultimateGemData = UltimateGemData.ReadFromArray(gemInfo);
			list.Add(ultimateGemData);
			if (ultimateGemData.SizeY > num2)
			{
				num2 = ultimateGemData.SizeY;
			}
		}
		InFront.GetValue(out bool value35);
		InSideBuild.GetValue(out bool value36);
		if (value36)
		{
			((GeometryBase)value).Transform(Transform.Translation(0.0, value35 ? (0.0 - (value12 / 2.0 + value8)) : (value12 / 2.0 + value8), 0.0));
			((GeometryBase)value2).Transform(Transform.Translation(0.0, value35 ? (0.0 - (value12 / 2.0 + value8)) : (value12 / 2.0 + value8), 0.0));
		}
		InMetalWidthToggle.GetValue(out bool value37);
		if (!value37)
		{
			value12 -= value9 * 2.0;
		}
		InProfileWidthStart.GetValue(out double value38);
		InProfileWidthEnd.GetValue(out double value39);
		if (Math.Abs(value12) > 0.01 && !value37)
		{
			foreach (UltimateGemData item in list)
			{
				BoundingBox boundingBox2 = ((GeometryBase)item.ProfileCurve).GetBoundingBox(true);
				diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
				double x3 = ((Vector3d)(ref diagonal)).X;
				Transform val4 = Transform.Scale(Plane.WorldXY, (x3 + value12) / x3, 1.0, 1.0);
				((GeometryBase)item.ProfileCurve).Transform(val4);
			}
		}
		double value40 = 0.0;
		InPositionMiddle.GetDouble(ref value40);
		bool value41 = false;
		InChannelEnabled.GetBoolean(ref value41);
		bool value42 = false;
		InGemsClosedLoop.GetBoolean(ref value42);
		double value43 = 0.0;
		InDirection.GetDouble(ref value43);
		InOverrideStartOffset.GetValue(out bool value44);
		InOverrideMiddleOffset.GetValue(out bool value45);
		InOverrideEndOffset.GetValue(out bool value46);
		InChannelBottomOffset.GetValue(out double value47);
		InChannelBottomOffset.GetValue(out double value48);
		InChannelBottomOffset.GetValue(out double value49);
		if (value44)
		{
			InChannelBottomOffsetStart.GetValue(out value47);
		}
		if (value45)
		{
			InChannelBottomOffsetMiddle.GetValue(out value48);
		}
		if (value46)
		{
			InChannelBottomOffsetEnd.GetValue(out value49);
		}
		bool noChannel = !value13;
		if (value14)
		{
			noChannel = false;
		}
		CSData channel;
		CSDataArray profileStartArray;
		CSDataArray profileEndArray;
		CSDataArray sweepRailsOuter;
		CSData sweepRailsInner;
		double channelWidthStart;
		double channelWidthEnd;
		Brep sweepBaseSurface;
		Brep sweepBaseLeftSurface;
		Brep sweepBaseRightSurface;
		Brep sweepChannelLeftSurface;
		Brep sweepChannelRightSurface;
		Brep sweepChannelCenterSurface;
		Brep sweepTopStartSurface;
		Brep sweepTopEndSurface;
		if (value15)
		{
			BuildChannel(list, value3, value, value2, whichItem, whichItem2, value43, value12, value4, value5, value6, value7, value8, value31, value32, value33, value47, value48, value49, value10, value11, value17, noChannel: true, value41, value42, value16, value36, value35, value18, value19, value20, value21, value22, value23, value24, value25, value26, value27, value34, value37, value38, value39, valueListItem, out channel, out profileStartArray, out profileEndArray, out sweepRailsOuter, out sweepRailsInner, out channelWidthStart, out channelWidthEnd, out sweepBaseSurface, out sweepBaseLeftSurface, out sweepBaseRightSurface, out sweepChannelLeftSurface, out sweepChannelRightSurface, out sweepChannelCenterSurface, out sweepTopStartSurface, out sweepTopEndSurface);
		}
		else
		{
			channel = new CSData();
		}
		BuildChannel(list, value3, value, value2, whichItem, whichItem2, value43, value12, value4, value5, value6, value7, value8, value31, value32, value33, value47, value48, value49, value10, value11, value17, noChannel, value41, value42, value16, value36, value35, value18, value19, value20, value21, value22, value23, value24, value25, value26, value27, value34, value37, value38, value39, valueListItem, out var channel2, out profileStartArray, out profileEndArray, out sweepRailsOuter, out sweepRailsInner, out channelWidthStart, out channelWidthEnd, out sweepBaseSurface, out sweepBaseLeftSurface, out sweepBaseRightSurface, out sweepChannelLeftSurface, out sweepChannelRightSurface, out sweepChannelCenterSurface, out sweepTopStartSurface, out sweepTopEndSurface);
		OutSurface.SetData(channel2);
		OutProfileStart.SetData(profileStartArray);
		OutProfileEnd.SetData(profileEndArray);
		OutSweepRailsOuter.SetData(sweepRailsOuter);
		OutSweepRailsInner.SetData(sweepRailsInner);
		OutUntrimmed.SetData(channel);
		OutChannelWidthStart.SetData(new CSNumberData(channelWidthStart));
		OutChannelWidthEnd.SetData(new CSNumberData(channelWidthEnd));
		OutBaseSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepBaseSurface));
		OutBaseLeftSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepBaseLeftSurface));
		OutBaseRightSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepBaseRightSurface));
		OutChannelLeftSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepChannelLeftSurface));
		OutChannelRightSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepChannelRightSurface));
		OutChannelCenterSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepChannelCenterSurface));
		OutTopStartSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepTopStartSurface));
		OutTopEndSurface.SetData(new CSOnGeometryData((GeometryBase)(object)sweepTopEndSurface));
		return base.Update();
	}

	private static void BuildChannel(List<UltimateGemData> gemList, Curve profile, Curve crv, Curve baseCrv, ValueListItem orientationMethod, ValueListItem placementMethod, double direction, double shankWidthAdd, double firstChannelProfilePosition, double firstChannelProfileY, double lastChannelProfilePosition, double lastChannelProfileY, double channelWallWidth, double channelWallDropStart, double channelWallDropMiddle, double channelWallDropEnd, double channelBottomOffsetStart, double channelBottomOffsetMiddle, double channelBottomOffsetEnd, double channelBlend, double channelEndPulldownDistance, bool autoChannelDepth, bool noChannel, bool channelEnabled, bool gemsClosedLoop, bool closedShank, bool sideBuilt, bool inFront, bool startShankProfileEnabled, double startShankProfilePosition, double startShankProfileWidth, bool startShankProfileTangent, bool startChannelProfileTangent, bool endShankProfileEnabled, double endShankProfilePosition, double endShankProfileWidth, bool endShankProfileTangent, bool endChannelProfileTangent, double endChannelWallDrop, bool metalWidthToggle, double profileWidthStart, double profileWidthEnd, ValueListItem useMinCulet, out CSData channel, out CSDataArray profileStartArray, out CSDataArray profileEndArray, out CSDataArray sweepRailsOuter, out CSData sweepRailsInner, out double channelWidthStart, out double channelWidthEnd, out Brep sweepBaseSurface, out Brep sweepBaseLeftSurface, out Brep sweepBaseRightSurface, out Brep sweepChannelLeftSurface, out Brep sweepChannelRightSurface, out Brep sweepChannelCenterSurface, out Brep sweepTopStartSurface, out Brep sweepTopEndSurface)
	{
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1035: Unknown result type (might be due to invalid IL or missing references)
		//IL_105f: Unknown result type (might be due to invalid IL or missing references)
		//IL_110c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1135: Unknown result type (might be due to invalid IL or missing references)
		//IL_10af: Unknown result type (might be due to invalid IL or missing references)
		//IL_10da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1001: Expected O, but got Unknown
		//IL_119f: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_11af: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b8: Expected O, but got Unknown
		channel = new CSData();
		profileStartArray = null;
		profileEndArray = null;
		sweepRailsOuter = new CSDataArray();
		sweepRailsInner = new CSDataArray();
		channelWidthStart = 0.0;
		channelWidthEnd = 0.0;
		CSProfileData cSProfileData = null;
		CSProfileData cSProfileData2 = null;
		sweepBaseSurface = null;
		sweepBaseLeftSurface = null;
		sweepBaseRightSurface = null;
		sweepChannelLeftSurface = null;
		sweepChannelRightSurface = null;
		sweepChannelCenterSurface = null;
		sweepTopStartSurface = null;
		sweepTopEndSurface = null;
		if (!channelEnabled || profile == null)
		{
			return;
		}
		List<Curve> list = new List<Curve>();
		List<Curve> list2 = new List<Curve>();
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<Curve> list5 = new List<Curve>();
		List<Curve> list6 = new List<Curve>();
		List<Curve> list7 = new List<Curve>();
		List<double> list8 = new List<double>();
		List<double> list9 = new List<double>();
		List<double> list10 = new List<double>();
		Interval domain;
		if (!gemsClosedLoop)
		{
			Curve profileOut;
			if (startShankProfileEnabled)
			{
				UltimateGemData g = new UltimateGemData
				{
					ParamLeft = gemList[0].ParamLeft,
					ParamRight = gemList[0].ParamRight,
					NormParam = startShankProfilePosition,
					SizeY = startShankProfileWidth - channelWallWidth,
					MinCuletMovement = ((useMinCulet == ValueListItem.MinCuletShankGrowUp) ? gemList[0].MinCuletMovement : 0.0)
				};
				list8.Add(startShankProfileWidth / 2.0);
				MakeEndProfileParts(g, crv, baseCrv, profile, orientationMethod, channelWallWidth, shankWidthAdd, channelBlend, sideBuilt, inFront, list, list2, list3, list4, list9, list10, out profileOut, out var _);
				cSProfileData = CSProfileData.GetCSProfile(profileOut, "StartProfile");
				profileStartArray = new CSDataArray(cSProfileData);
				profileStartArray.Append(cSProfileData);
				profileStartArray.Append(new CSNumberData(list10[list10.Count - 1]));
				profileStartArray.Append(new CSNumberData(list9[list9.Count - 1]));
			}
			UltimateGemData ultimateGemData;
			if (gemList.Count > 0)
			{
				ultimateGemData = new UltimateGemData
				{
					ParamLeft = gemList[0].ParamLeft,
					ParamRight = gemList[0].ParamRight
				};
			}
			else
			{
				UltimateGemData ultimateGemData2 = new UltimateGemData();
				domain = profile.Domain;
				double min = ((Interval)(ref domain)).Min;
				domain = profile.Domain;
				ultimateGemData2.ParamLeft = min + ((Interval)(ref domain)).Length * 0.4;
				domain = profile.Domain;
				double min2 = ((Interval)(ref domain)).Min;
				domain = profile.Domain;
				ultimateGemData2.ParamRight = min2 + ((Interval)(ref domain)).Length * 0.6;
				ultimateGemData = ultimateGemData2;
			}
			ultimateGemData.NormParam = firstChannelProfilePosition;
			ultimateGemData.SizeY = (metalWidthToggle ? (profileWidthStart - shankWidthAdd - channelWallWidth * 2.0) : (firstChannelProfileY - channelWallWidth));
			ultimateGemData.MinCuletMovement = ((useMinCulet == ValueListItem.MinCuletShankGrowUp) ? gemList[0].MinCuletMovement : 0.0);
			list8.Add(firstChannelProfileY / 2.0);
			MakeEndProfileParts(ultimateGemData, crv, baseCrv, profile, orientationMethod, channelWallWidth, shankWidthAdd, channelBlend, sideBuilt, inFront, list, list2, list3, list4, list9, list10, out profileOut, out channelWidthStart);
			if (!startShankProfileEnabled)
			{
				cSProfileData = CSProfileData.GetCSProfile(profileOut, "StartProfile");
				profileStartArray = new CSDataArray();
				profileStartArray.Append(cSProfileData);
				profileStartArray.Append(new CSNumberData(list10[list10.Count - 1]));
				profileStartArray.Append(new CSNumberData(list9[list9.Count - 1]));
			}
		}
		List<double> list11 = new List<double>();
		foreach (UltimateGemData gem in gemList)
		{
			list11.Add(gem.SizeY);
		}
		bool[] array = FindMinMax(list11.ToArray());
		for (int i = 0; i < gemList.Count; i++)
		{
			if (!array[i])
			{
				continue;
			}
			list8.Add((channelWallWidth + gemList[i].SizeY) / 2.0);
			if (noChannel)
			{
				MakeEndProfileParts(gemList[i], crv, baseCrv, profile, orientationMethod, channelWallWidth, shankWidthAdd, channelBlend, sideBuilt, inFront, list, list2, list3, list4, list9, list10, out var _, out var _);
				continue;
			}
			double bottomOffset = 0.0;
			double channelWallDrop = 0.0;
			bool isEndGem = false;
			double num = endChannelWallDrop;
			if (i <= (gemList.Count - 1) / 2)
			{
				bottomOffset = channelBottomOffsetStart * ((double)(gemList.Count - 1) / 2.0 - (double)i) / ((double)(gemList.Count - 1) / 2.0) + channelBottomOffsetMiddle * ((double)i / ((double)(gemList.Count - 1) / 2.0));
				channelWallDrop = channelWallDropStart * ((double)(gemList.Count - 1) / 2.0 - (double)i) / ((double)(gemList.Count - 1) / 2.0) + channelWallDropMiddle * ((double)i / ((double)(gemList.Count - 1) / 2.0));
			}
			if (i > (gemList.Count - 1) / 2)
			{
				bottomOffset = channelBottomOffsetMiddle * ((double)(gemList.Count - 1) - (double)i) / (double)(gemList.Count - 1) + channelBottomOffsetEnd * ((double)i / (double)(gemList.Count - 1));
				channelWallDrop = channelWallDropMiddle * ((double)(gemList.Count - 1) - (double)i) / (double)(gemList.Count - 1) + channelWallDropEnd * ((double)i / (double)(gemList.Count - 1));
				num *= -1.0;
			}
			if (i == 0 || i == gemList.Count - 1)
			{
				isEndGem = true;
			}
			MakeGemProfileParts(baseCrv, gemList[i], placementMethod, autoChannelDepth, channelWallDrop, channelBlend, bottomOffset, num, isEndGem, list, list2, list3, list4, list9, list10);
		}
		if (!gemsClosedLoop)
		{
			UltimateGemData ultimateGemData3;
			if (gemList.Count > 0)
			{
				ultimateGemData3 = new UltimateGemData
				{
					ParamLeft = gemList[gemList.Count - 1].ParamLeft,
					ParamRight = gemList[gemList.Count - 1].ParamRight
				};
			}
			else
			{
				UltimateGemData ultimateGemData4 = new UltimateGemData();
				domain = profile.Domain;
				double min3 = ((Interval)(ref domain)).Min;
				domain = profile.Domain;
				ultimateGemData4.ParamLeft = min3 + ((Interval)(ref domain)).Length * 0.4;
				domain = profile.Domain;
				double min4 = ((Interval)(ref domain)).Min;
				domain = profile.Domain;
				ultimateGemData4.ParamRight = min4 + ((Interval)(ref domain)).Length * 0.6;
				ultimateGemData3 = ultimateGemData4;
			}
			ultimateGemData3.NormParam = lastChannelProfilePosition;
			ultimateGemData3.SizeY = (metalWidthToggle ? (profileWidthEnd - channelWallWidth * 2.0 - shankWidthAdd) : (lastChannelProfileY - channelWallWidth));
			ultimateGemData3.MinCuletMovement = ((useMinCulet == ValueListItem.MinCuletShankGrowUp) ? gemList[gemList.Count - 1].MinCuletMovement : 0.0);
			list8.Add(lastChannelProfileY / 2.0);
			MakeEndProfileParts(ultimateGemData3, crv, baseCrv, profile, orientationMethod, channelWallWidth, shankWidthAdd, channelBlend, sideBuilt, inFront, list, list2, list3, list4, list9, list10, out var profileOut3, out channelWidthEnd);
			if (endShankProfileEnabled)
			{
				UltimateGemData g2 = new UltimateGemData
				{
					ParamLeft = gemList[gemList.Count - 1].ParamLeft,
					ParamRight = gemList[gemList.Count - 1].ParamRight,
					NormParam = endShankProfilePosition,
					SizeY = endShankProfileWidth - channelWallWidth,
					MinCuletMovement = ((useMinCulet == ValueListItem.MinCuletShankGrowUp) ? gemList[gemList.Count - 1].MinCuletMovement : 0.0)
				};
				list8.Add(endShankProfileWidth / 2.0);
				MakeEndProfileParts(g2, crv, baseCrv, profile, orientationMethod, channelWallWidth, shankWidthAdd, channelBlend, sideBuilt, inFront, list, list2, list3, list4, list9, list10, out profileOut3, out var _);
			}
			cSProfileData2 = CSProfileData.GetCSProfile(profileOut3, "EndProfile");
			profileEndArray = new CSDataArray();
			profileEndArray.Append(cSProfileData2);
			profileEndArray.Append(new CSNumberData(list10[list10.Count - 1]));
			profileEndArray.Append(new CSNumberData(list9[list9.Count - 1]));
		}
		if (direction < 0.0)
		{
			list.Reverse();
			list2.Reverse();
			list3.Reverse();
			list4.Reverse();
			list9.Reverse();
			list10.Reverse();
		}
		if (gemsClosedLoop || closedShank)
		{
			if (CSUtilSweepRC.Sweep2(crv, baseCrv, list, list9, list10, out sweepBaseSurface, closed: true, maintainHeight: true) && sweepBaseSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list2, list9, list10, out sweepChannelLeftSurface, closed: true, maintainHeight: true) && sweepChannelLeftSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list4, list9, list10, out sweepChannelRightSurface, closed: true, maintainHeight: true) && sweepChannelRightSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list3, list9, list10, out sweepChannelCenterSurface, closed: true, maintainHeight: true) && sweepChannelCenterSurface != null)
			{
				Brep[] array2 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { sweepBaseSurface, sweepChannelLeftSurface, sweepChannelCenterSurface, sweepChannelRightSurface }, CSApplicationHelper.Application.Tolerance);
				if (array2.Length == 1)
				{
					channel = CSOnGeometryData.FromBrep(array2[0]);
				}
			}
			return;
		}
		if (list.Count <= 2)
		{
			if (CSUtilSweepRC.Sweep2(crv, baseCrv, list, list9, list10, out sweepBaseSurface, closed: false, maintainHeight: true) && sweepBaseSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list2, list9, list10, out sweepChannelLeftSurface, closed: false, maintainHeight: true) && sweepChannelLeftSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list4, list9, list10, out sweepChannelRightSurface, closed: false, maintainHeight: true) && sweepChannelRightSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list3, list9, list10, out sweepChannelCenterSurface, closed: false, maintainHeight: true) && sweepChannelCenterSurface != null)
			{
				Brep[] array3 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { sweepBaseSurface, sweepChannelLeftSurface, sweepChannelCenterSurface, sweepChannelRightSurface }, CSApplicationHelper.Application.Tolerance);
				if (array3.Length == 1)
				{
					channel = CSOnGeometryData.FromBrep(array3[0]);
				}
			}
			return;
		}
		Create4Rails(crv, baseCrv, channelWallWidth, list8, list9, list10, startShankProfileEnabled, startShankProfileTangent, startChannelProfileTangent, endShankProfileEnabled, endShankProfileTangent, endChannelProfileTangent, out var baseCrv2, out var baseCrv3, out var gemCrv, out var gemCrv2, out var baseCrv0Param, out var baseCrv1Param, out var gemCrv0Param, out var gemCrv1Param);
		sweepRailsOuter.Append(new CSOnGeometryData((GeometryBase)(object)baseCrv3));
		sweepRailsOuter.Append(new CSOnGeometryData((GeometryBase)(object)gemCrv2));
		sweepRailsOuter.Append(new CSOnGeometryData((GeometryBase)(object)gemCrv));
		sweepRailsOuter.Append(new CSOnGeometryData((GeometryBase)(object)baseCrv2));
		double v = default(double);
		if (cSProfileData != null)
		{
			cSProfileData.SetCurveParameterCount(4);
			Curve curve = cSProfileData.GetCurve();
			curve.ClosestPoint(baseCrv2.PointAtStart, ref v);
			cSProfileData.SetCurveParameter(0, v);
			curve.ClosestPoint(gemCrv.PointAtStart, ref v);
			cSProfileData.SetCurveParameter(1, v);
			curve.ClosestPoint(gemCrv2.PointAtStart, ref v);
			cSProfileData.SetCurveParameter(2, v);
			curve.ClosestPoint(baseCrv3.PointAtStart, ref v);
			cSProfileData.SetCurveParameter(3, v);
		}
		if (cSProfileData2 != null)
		{
			cSProfileData2.SetCurveParameterCount(4);
			Curve curve2 = cSProfileData2.GetCurve();
			curve2.ClosestPoint(baseCrv2.PointAtEnd, ref v);
			cSProfileData2.SetCurveParameter(0, v);
			curve2.ClosestPoint(gemCrv.PointAtEnd, ref v);
			cSProfileData2.SetCurveParameter(1, v);
			curve2.ClosestPoint(gemCrv2.PointAtEnd, ref v);
			cSProfileData2.SetCurveParameter(2, v);
			curve2.ClosestPoint(baseCrv3.PointAtEnd, ref v);
			cSProfileData2.SetCurveParameter(3, v);
		}
		double num2 = default(double);
		double num3 = default(double);
		for (int j = 0; j < list.Count; j++)
		{
			Curve val = list[j];
			Point3d val2 = baseCrv2.PointAt(baseCrv0Param[j]);
			val.ClosestPoint(val2, ref num2);
			Point3d val3 = baseCrv3.PointAt(baseCrv1Param[j]);
			val.ClosestPoint(val3, ref num3);
			domain = val.Domain;
			list5.Add(val.Trim(((Interval)(ref domain)).Min, num2));
			list6.Add(val.Trim(num2, num3));
			double num4 = num3;
			domain = val.Domain;
			list7.Add(val.Trim(num4, ((Interval)(ref domain)).Max));
		}
		if (!CSUtilSweepRC.Sweep2(baseCrv3, gemCrv2, list7, baseCrv1Param, gemCrv1Param, out sweepBaseLeftSurface, closed: false, maintainHeight: true) || sweepBaseLeftSurface == null || !CSUtilSweepRC.Sweep2(baseCrv2, gemCrv, list5, baseCrv0Param, gemCrv1Param, out sweepBaseRightSurface, closed: false, maintainHeight: true) || sweepBaseRightSurface == null)
		{
			return;
		}
		gemCrv = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseRightSurface, list5[0].PointAtStart, list5[list5.Count - 1].PointAtStart, list5[1].PointAtStart);
		baseCrv2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseRightSurface, list5[0].PointAtEnd, list5[list5.Count - 1].PointAtEnd, list5[1].PointAtEnd);
		baseCrv3 = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseLeftSurface, list7[0].PointAtStart, list7[list7.Count - 1].PointAtStart, list7[1].PointAtStart);
		gemCrv2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseLeftSurface, list7[0].PointAtEnd, list7[list7.Count - 1].PointAtEnd, list7[1].PointAtEnd);
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData((GeometryBase)(object)baseCrv2));
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData((GeometryBase)(object)gemCrv));
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData((GeometryBase)(object)gemCrv2));
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData((GeometryBase)(object)baseCrv3));
		double value = default(double);
		for (int k = 0; k < list.Count; k++)
		{
			gemCrv.ClosestPoint(list5[k].PointAtStart, ref value);
			gemCrv0Param[k] = value;
			gemCrv2.ClosestPoint(list7[k].PointAtEnd, ref value);
			gemCrv1Param[k] = value;
			baseCrv2.ClosestPoint(list5[k].PointAtEnd, ref value);
			baseCrv0Param[k] = value;
			baseCrv3.ClosestPoint(list7[k].PointAtStart, ref value);
			baseCrv1Param[k] = value;
		}
		if (!CSUtilSweepRC.Sweep2(baseCrv2, baseCrv3, list6, baseCrv0Param, baseCrv1Param, out sweepBaseSurface, closed: false, maintainHeight: true) || sweepBaseSurface == null)
		{
			return;
		}
		List<Curve> list12 = new List<Curve>(list4);
		List<Curve> list13 = new List<Curve>(list2);
		List<Curve> list14 = new List<Curve>(list3);
		List<double> list15 = new List<double>(gemCrv0Param);
		List<double> list16 = new List<double>(gemCrv1Param);
		if (startShankProfileEnabled)
		{
			list12.RemoveAt(0);
			list13.RemoveAt(0);
			list14.RemoveAt(0);
			list15.RemoveAt(0);
			list16.RemoveAt(0);
		}
		if (endShankProfileEnabled)
		{
			int index = list12.Count - 1;
			list12.RemoveAt(index);
			list13.RemoveAt(index);
			list14.RemoveAt(index);
			list15.RemoveAt(index);
			list16.RemoveAt(index);
		}
		if (!CSUtilSweepRC.Sweep2(gemCrv, gemCrv2, list12, list15, list16, out sweepChannelRightSurface, closed: false, maintainHeight: true) || sweepChannelRightSurface == null || !CSUtilSweepRC.Sweep2(gemCrv, gemCrv2, list13, list15, list16, out sweepChannelLeftSurface, closed: false, maintainHeight: true) || sweepChannelLeftSurface == null)
		{
			return;
		}
		Curve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepChannelLeftSurface, list13[0].PointAtEnd, list13[list13.Count - 1].PointAtEnd, list13[1].PointAtEnd);
		if (curveFromBrepEdges == null)
		{
			return;
		}
		Curve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepChannelRightSurface, list12[0].PointAtStart, list12[list12.Count - 1].PointAtStart, list12[1].PointAtStart);
		if (curveFromBrepEdges2 == null)
		{
			return;
		}
		double length = curveFromBrepEdges.GetLength();
		double length2 = curveFromBrepEdges2.GetLength();
		List<Curve> list17 = new List<Curve>();
		list9.Clear();
		list10.Clear();
		list17.Add(list14[0]);
		double num5 = default(double);
		curveFromBrepEdges.ClosestPoint(list14[0].PointAtStart, ref num5, CSApplicationHelper.Application.Tolerance);
		list9.Add(num5);
		double num6 = default(double);
		curveFromBrepEdges2.ClosestPoint(list14[0].PointAtEnd, ref num6, CSApplicationHelper.Application.Tolerance);
		list10.Add(num6);
		if (channelEndPulldownDistance > 0.01 && !noChannel)
		{
			double normalizedParameter = GetNormalizedParameter(curveFromBrepEdges, length, num5);
			double num7 = default(double);
			curveFromBrepEdges.NormalizedLengthParameter(normalizedParameter + channelEndPulldownDistance / length, ref num7);
			Point3d val4 = curveFromBrepEdges.PointAt(num7);
			double normalizedParameter2 = GetNormalizedParameter(curveFromBrepEdges2, length2, num6);
			double num8 = default(double);
			curveFromBrepEdges2.NormalizedLengthParameter(normalizedParameter2 + channelEndPulldownDistance / length2, ref num8);
			Point3d val5 = curveFromBrepEdges2.PointAt(num8);
			Curve item = (Curve)new LineCurve(val4, val5);
			list17.Add(item);
			list9.Add(num7);
			list10.Add(num8);
		}
		list17.Add(list14[1]);
		double item2 = default(double);
		curveFromBrepEdges.ClosestPoint(list14[1].PointAtStart, ref item2, CSApplicationHelper.Application.Tolerance);
		list9.Add(item2);
		curveFromBrepEdges2.ClosestPoint(list14[1].PointAtEnd, ref item2, CSApplicationHelper.Application.Tolerance);
		list10.Add(item2);
		int index2 = list14.Count - 2;
		if (list14.Count > 3)
		{
			list17.Add(list14[index2]);
			curveFromBrepEdges.ClosestPoint(list14[index2].PointAtStart, ref item2, CSApplicationHelper.Application.Tolerance);
			list9.Add(item2);
			curveFromBrepEdges2.ClosestPoint(list14[index2].PointAtEnd, ref item2, CSApplicationHelper.Application.Tolerance);
			list10.Add(item2);
		}
		curveFromBrepEdges.ClosestPoint(list14[list14.Count - 1].PointAtStart, ref num5, CSApplicationHelper.Application.Tolerance);
		curveFromBrepEdges2.ClosestPoint(list14[list14.Count - 1].PointAtEnd, ref num6, CSApplicationHelper.Application.Tolerance);
		if (channelEndPulldownDistance > 0.01 && !noChannel)
		{
			double normalizedParameter3 = GetNormalizedParameter(curveFromBrepEdges, length, num5);
			double num9 = default(double);
			curveFromBrepEdges.NormalizedLengthParameter(normalizedParameter3 - channelEndPulldownDistance / length, ref num9);
			double normalizedParameter4 = GetNormalizedParameter(curveFromBrepEdges2, length2, num6);
			double num10 = default(double);
			curveFromBrepEdges2.NormalizedLengthParameter(normalizedParameter4 - channelEndPulldownDistance / length2, ref num10);
			Point3d val6 = curveFromBrepEdges.PointAt(num9);
			Point3d val7 = curveFromBrepEdges2.PointAt(num10);
			Curve item3 = (Curve)new LineCurve(val6, val7);
			list17.Add(item3);
			list9.Add(num9);
			list10.Add(num10);
		}
		list17.Add(list14[list14.Count - 1]);
		list9.Add(num5);
		list10.Add(num6);
		if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges, curveFromBrepEdges2, list17, list9, list10, out sweepChannelCenterSurface, closed: false, maintainHeight: true) || sweepChannelCenterSurface == null)
		{
			return;
		}
		if (startShankProfileEnabled)
		{
			Curve[] array4 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
			{
				list4[0],
				list3[0],
				list2[0]
			});
			Curve[] array5 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
			{
				list4[1],
				list3[1],
				list2[1]
			});
			if (!CSUtilSweepRC.Sweep2(gemCrv, gemCrv2, new List<Curve>
			{
				array4[0],
				array5[0]
			}, new List<double>
			{
				gemCrv0Param[0],
				gemCrv0Param[1]
			}, new List<double>
			{
				gemCrv1Param[0],
				gemCrv1Param[1]
			}, out sweepTopStartSurface, closed: false, maintainHeight: true) || sweepTopStartSurface == null)
			{
				return;
			}
		}
		if (endShankProfileEnabled)
		{
			int num11 = list4.Count - 1;
			Curve[] array6 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
			{
				list4[num11 - 1],
				list3[num11 - 1],
				list2[num11 - 1]
			});
			Curve[] array7 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve>
			{
				list4[num11],
				list3[num11],
				list2[num11]
			});
			if (!CSUtilSweepRC.Sweep2(gemCrv, gemCrv2, new List<Curve>
			{
				array6[0],
				array7[0]
			}, new List<double>
			{
				gemCrv0Param[num11 - 1],
				gemCrv0Param[num11]
			}, new List<double>
			{
				gemCrv1Param[num11 - 1],
				gemCrv1Param[num11]
			}, out sweepTopEndSurface, closed: false, maintainHeight: true) || sweepTopEndSurface == null)
			{
				return;
			}
		}
		List<Brep> list18 = new List<Brep> { sweepBaseRightSurface, sweepBaseSurface, sweepBaseLeftSurface, sweepChannelLeftSurface, sweepChannelCenterSurface, sweepChannelRightSurface };
		if (sweepTopStartSurface != null)
		{
			list18.Add(sweepTopStartSurface);
		}
		if (sweepTopEndSurface != null)
		{
			list18.Add(sweepTopEndSurface);
		}
		Brep[] array8 = Brep.JoinBreps((IEnumerable<Brep>)list18, CSApplicationHelper.Application.Tolerance);
		if (array8.Length == 1)
		{
			channel = CSOnGeometryData.FromBrep(array8[0]);
		}
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem orientationMethod, UltimateGemData gem)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.NormParam;
		if (crv.IsClosed)
		{
			while (num > 1.0)
			{
				num -= 1.0;
			}
			for (; num < 0.0; num += 1.0)
			{
			}
		}
		else
		{
			if (num < 0.0)
			{
				num = 0.0;
			}
			if (num > 1.0)
			{
				num = 1.0;
			}
		}
		crv.NormalizedLengthParameter(num, ref gem.Param);
		Point3d val = crv.PointAt(gem.Param);
		Vector3d val2 = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (orientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
		{
			baseCrv.ClosestPoint(val, ref gem.BaseParam);
			return;
		}
		Plane val3 = default(Plane);
		((Plane)(ref val3))._002Ector(val, val2);
		CurveIntersections obj = Intersection.CurvePlane(baseCrv, val3, 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in obj)
		{
			Vector3d val4 = item.PointA - val;
			double length = ((Vector3d)(ref val4)).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private static void Create4Rails(Curve crv, Curve baseCrv, double channelWallWidth, List<double> shankWidthatGem, List<double> gemCrvParamIn, List<double> baseCrvParamIn, bool startShankProfileEnabled, bool startShankProfileTangent, bool startChannelProfileTangent, bool endShankProfileEnabled, bool endShankProfileTangent, bool endChannelProfileTangent, out Curve baseCrv0, out Curve baseCrv1, out Curve gemCrv0, out Curve gemCrv1, out List<double> baseCrv0Param, out List<double> baseCrv1Param, out List<double> gemCrv0Param, out List<double> gemCrv1Param)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09db: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		List<double> list = new List<double>(gemCrvParamIn);
		List<double> list2 = new List<double>(baseCrvParamIn);
		Curve val = crv.DuplicateCurve();
		Curve val2 = baseCrv.DuplicateCurve();
		Interval domain;
		if (val.IsClosed && val2.IsClosed)
		{
			val.ChangeClosedCurveSeam(list[0]);
			val2.ChangeClosedCurveSeam(list2[0]);
			domain = val.Domain;
			list[0] = ((Interval)(ref domain)).Min;
			domain = val2.Domain;
			list2[0] = ((Interval)(ref domain)).Min;
			double value = default(double);
			for (int i = 1; i < shankWidthatGem.Count; i++)
			{
				Point3d val3 = crv.PointAt(list[i]);
				val.ClosestPoint(val3, ref value);
				list[i] = value;
				val3 = baseCrv.PointAt(list2[i]);
				val2.ClosestPoint(val3, ref value);
				list2[i] = value;
			}
		}
		double num = 0.0;
		foreach (double item4 in shankWidthatGem)
		{
			if (item4 > num)
			{
				num = item4;
			}
		}
		num = (num + channelWallWidth) / 2.0;
		num *= 1.5;
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<Point3d> list5 = new List<Point3d>();
		List<Point3d> list6 = new List<Point3d>();
		Point3d item = default(Point3d);
		Point3d item2 = default(Point3d);
		for (int j = 0; j < shankWidthatGem.Count; j++)
		{
			ProfilePositionScale(val, val2, list[j], list2[j], 0.0, out var pScale, out var pRail);
			LineCurve val4 = new LineCurve(new Point3d(0.0 - num, 0.0, 0.0), new Point3d(num, 0.0, 0.0));
			((GeometryBase)val4).Transform(pRail);
			list3.Add((Curve)(object)val4);
			LineCurve val5 = new LineCurve(new Point3d(0.0 - num, 1.0, 0.0), new Point3d(num, 1.0, 0.0));
			((GeometryBase)val5).Transform(pScale);
			((GeometryBase)val5).Transform(pRail);
			list4.Add((Curve)(object)val5);
			((Point3d)(ref item))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref item)).Transform(pRail);
			list5.Add(item);
			((Point3d)(ref item2))._002Ector(0.0, 1.0, 0.0);
			((Point3d)(ref item2)).Transform(pScale);
			((Point3d)(ref item2)).Transform(pRail);
			list6.Add(item2);
		}
		SweepTwoRail val6 = new SweepTwoRail
		{
			SweepTolerance = CSApplicationHelper.Application.Tolerance,
			AngleToleranceRadians = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians,
			MaintainHeight = true,
			ClosedSweep = false
		};
		Brep[] array = val6.PerformSweep(val, val2, (IEnumerable<Curve>)list3, (IEnumerable<double>)list, (IEnumerable<double>)list2);
		Brep[] array2 = val6.PerformSweep(val, val2, (IEnumerable<Curve>)list4, (IEnumerable<double>)list, (IEnumerable<double>)list2);
		NurbsSurface val7 = null;
		if (array[0].Faces.Count > 0)
		{
			val7 = ((Surface)array[0].Faces[0]).ToNurbsSurface();
		}
		NurbsSurface val8 = null;
		if (array2[0].Faces.Count > 0)
		{
			val8 = ((Surface)array2[0].Faces[0]).ToNurbsSurface();
		}
		if (val7 == null || val8 == null)
		{
			baseCrv0 = null;
			baseCrv1 = null;
			gemCrv0 = null;
			gemCrv1 = null;
			baseCrv0Param = null;
			baseCrv1Param = null;
			gemCrv0Param = null;
			gemCrv1Param = null;
			return;
		}
		List<Point3d> list7 = new List<Point3d>();
		List<Point3d> list8 = new List<Point3d>();
		List<Point3d> list9 = new List<Point3d>();
		List<Point3d> list10 = new List<Point3d>();
		double num3 = default(double);
		double num4 = default(double);
		for (int k = 0; k < shankWidthatGem.Count; k++)
		{
			double num2 = shankWidthatGem[k];
			((Surface)val7).ClosestPoint(list5[k], ref num3, ref num4);
			list7.Add(new Point3d(num3, num4 + num2, 0.0));
			list8.Add(new Point3d(num3, num4 - num2, 0.0));
			((Surface)val8).ClosestPoint(list6[k], ref num3, ref num4);
			list9.Add(new Point3d(num3, num4 + num2, 0.0));
			list10.Add(new Point3d(num3, num4 - num2, 0.0));
		}
		List<Point3d> list11 = new List<Point3d>(list7);
		List<Point3d> list12 = new List<Point3d>(list8);
		List<Point3d> list13 = new List<Point3d>(list9);
		List<Point3d> list14 = new List<Point3d>(list10);
		Vector3d val9 = Vector3d.Unset;
		Vector3d val10 = Vector3d.Unset;
		if ((startShankProfileTangent && startShankProfileEnabled) || (startChannelProfileTangent && !startShankProfileEnabled))
		{
			val9 = Vector3d.XAxis;
		}
		Point3d val11;
		if (startChannelProfileTangent && startShankProfileEnabled)
		{
			val11 = list11[1];
			double num5 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list11[1];
			list11.Insert(1, new Point3d(num5, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list12[1];
			double num6 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list12[1];
			list12.Insert(1, new Point3d(num6, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list13[1];
			double num7 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list13[1];
			list13.Insert(1, new Point3d(num7, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list14[1];
			double num8 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list14[1];
			list14.Insert(1, new Point3d(num8, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list11[1];
			double num9 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list11[1];
			list11.Insert(1, new Point3d(num9, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list12[1];
			double num10 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list12[1];
			list12.Insert(1, new Point3d(num10, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list13[1];
			double num11 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list13[1];
			list13.Insert(1, new Point3d(num11, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list14[1];
			double num12 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list14[1];
			list14.Insert(1, new Point3d(num12, ((Point3d)(ref val11)).Y, 0.0));
		}
		if ((endShankProfileTangent && endShankProfileEnabled) || (endChannelProfileTangent && !endShankProfileEnabled))
		{
			val10 = Vector3d.XAxis;
		}
		if (endChannelProfileTangent && endShankProfileEnabled)
		{
			int num13 = list11.Count - 2;
			int index = num13 + 1;
			val11 = list11[num13];
			double num14 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list11[num13];
			list11.Insert(index, new Point3d(num14, ((Point3d)(ref val11)).Y, 0.0));
			int index2 = num13 + 1;
			val11 = list12[num13];
			double num15 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list12[num13];
			list12.Insert(index2, new Point3d(num15, ((Point3d)(ref val11)).Y, 0.0));
			int index3 = num13 + 1;
			val11 = list13[num13];
			double num16 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list13[num13];
			list13.Insert(index3, new Point3d(num16, ((Point3d)(ref val11)).Y, 0.0));
			int index4 = num13 + 1;
			val11 = list14[num13];
			double num17 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list14[num13];
			list14.Insert(index4, new Point3d(num17, ((Point3d)(ref val11)).Y, 0.0));
			num13 = list11.Count - 2;
			int index5 = num13 + 1;
			val11 = list11[num13];
			double num18 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list11[num13];
			list11.Insert(index5, new Point3d(num18, ((Point3d)(ref val11)).Y, 0.0));
			int index6 = num13 + 1;
			val11 = list12[num13];
			double num19 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list12[num13];
			list12.Insert(index6, new Point3d(num19, ((Point3d)(ref val11)).Y, 0.0));
			int index7 = num13 + 1;
			val11 = list13[num13];
			double num20 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list13[num13];
			list13.Insert(index7, new Point3d(num20, ((Point3d)(ref val11)).Y, 0.0));
			int index8 = num13 + 1;
			val11 = list14[num13];
			double num21 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list14[num13];
			list14.Insert(index8, new Point3d(num21, ((Point3d)(ref val11)).Y, 0.0));
		}
		Curve val12 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list11, 3, (CurveKnotStyle)1, val9, val10);
		Curve val13 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list12, 3, (CurveKnotStyle)1, val9, val10);
		Curve val14 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list13, 3, (CurveKnotStyle)1, val9, val10);
		Curve val15 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list14, 3, (CurveKnotStyle)1, val9, val10);
		baseCrv0 = ((Surface)val7).Pushup(val12, 0.01);
		baseCrv1 = ((Surface)val7).Pushup(val13, 0.01);
		gemCrv0 = ((Surface)val8).Pushup(val14, 0.01);
		gemCrv1 = ((Surface)val8).Pushup(val15, 0.01);
		baseCrv0Param = new List<double>();
		baseCrv1Param = new List<double>();
		gemCrv0Param = new List<double>();
		gemCrv1Param = new List<double>();
		List<double> obj = baseCrv0Param;
		domain = baseCrv0.Domain;
		obj.Add(((Interval)(ref domain)).Min);
		List<double> obj2 = baseCrv1Param;
		domain = baseCrv1.Domain;
		obj2.Add(((Interval)(ref domain)).Min);
		List<double> obj3 = gemCrv0Param;
		domain = gemCrv0.Domain;
		obj3.Add(((Interval)(ref domain)).Min);
		List<double> obj4 = gemCrv1Param;
		domain = gemCrv1.Domain;
		obj4.Add(((Interval)(ref domain)).Min);
		double item3 = default(double);
		for (int l = 1; l < shankWidthatGem.Count; l++)
		{
			NurbsSurface obj5 = val7;
			val11 = list7[l];
			double x = ((Point3d)(ref val11)).X;
			val11 = list7[l];
			Point3d val16 = ((Surface)obj5).PointAt(x, ((Point3d)(ref val11)).Y);
			baseCrv0.ClosestPoint(val16, ref item3, 0.01);
			baseCrv0Param.Add(item3);
			NurbsSurface obj6 = val7;
			val11 = list8[l];
			double x2 = ((Point3d)(ref val11)).X;
			val11 = list8[l];
			val16 = ((Surface)obj6).PointAt(x2, ((Point3d)(ref val11)).Y);
			baseCrv1.ClosestPoint(val16, ref item3, 0.01);
			baseCrv1Param.Add(item3);
			NurbsSurface obj7 = val8;
			val11 = list9[l];
			double x3 = ((Point3d)(ref val11)).X;
			val11 = list9[l];
			val16 = ((Surface)obj7).PointAt(x3, ((Point3d)(ref val11)).Y);
			gemCrv0.ClosestPoint(val16, ref item3, 0.01);
			gemCrv0Param.Add(item3);
			NurbsSurface obj8 = val8;
			val11 = list10[l];
			double x4 = ((Point3d)(ref val11)).X;
			val11 = list10[l];
			val16 = ((Surface)obj8).PointAt(x4, ((Point3d)(ref val11)).Y);
			gemCrv1.ClosestPoint(val16, ref item3, 0.01);
			gemCrv1Param.Add(item3);
		}
	}

	private static bool Deq(double v0, double v1)
	{
		return Math.Abs(v0 - v1) < 0.001;
	}

	private static bool[] FindMinMax(IReadOnlyList<double> vals)
	{
		bool flag = true;
		int num = (vals.Count + 1) / 2;
		for (int i = 0; i < num; i++)
		{
			if (!Deq(vals[i], vals[vals.Count - 1 - i]))
			{
				flag = false;
				break;
			}
		}
		int num2 = (flag ? num : vals.Count);
		bool[] array = new bool[vals.Count];
		array[0] = true;
		array[num2 - 1] = true;
		for (int j = 0; j < num2 - 2; j++)
		{
			if (vals[j] > vals[j + 1] && vals[j + 1] < vals[j + 2])
			{
				array[j + 1] = true;
			}
			if (vals[j] < vals[j + 1] && vals[j + 1] > vals[j + 2])
			{
				array[j + 1] = true;
			}
		}
		for (int k = 0; k < num2 - 3; k++)
		{
			if (vals[k] > vals[k + 1] && Deq(vals[k + 1], vals[k + 2]) && vals[k + 2] < vals[k + 3])
			{
				array[k + 1] = true;
				array[k + 2] = true;
			}
			if (vals[k] < vals[k + 1] && Deq(vals[k + 1], vals[k + 2]) && vals[k + 2] > vals[k + 3])
			{
				array[k + 1] = true;
				array[k + 2] = true;
			}
		}
		for (int l = 0; l < num2 - 4; l++)
		{
			if (vals[l] > vals[l + 1] && Deq(vals[l + 1], vals[l + 2]) && Deq(vals[l + 2], vals[l + 3]) && vals[l + 3] < vals[l + 4])
			{
				array[l + 2] = true;
			}
			if (vals[l] < vals[l + 1] && Deq(vals[l + 1], vals[l + 2]) && Deq(vals[l + 2], vals[l + 3]) && vals[l + 3] > vals[l + 4])
			{
				array[l + 2] = true;
			}
		}
		for (int m = 0; m < num2 - 3; m++)
		{
			if (Deq(vals[m], vals[m + 1]) && Deq(vals[m], vals[m + 2]) && Deq(vals[m], vals[m + 3]))
			{
				array[m + 1] = true;
				array[m + 2] = true;
			}
		}
		int num3 = 0;
		for (int n = 0; n < num2; n++)
		{
			if (!array[n])
			{
				continue;
			}
			int num4 = n - num3;
			if (n - num3 > 4)
			{
				int num5 = num4 / 4;
				int num6 = num3 + (num4 - num5) / 2 - 1;
				for (int num7 = 0; num7 < num5; num7++)
				{
					if (num6 < num2 - 1)
					{
						array[num6] = true;
						num6 += 4;
					}
				}
			}
			num3 = n;
		}
		if (flag)
		{
			for (int num8 = 0; num8 < num; num8++)
			{
				array[array.Length - 1 - num8] = array[num8];
			}
		}
		return array;
	}

	private static double GetNormalizedParameter(Curve crv, double cLength, double param)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Interval domain = crv.Domain;
		if (Math.Abs(((Interval)(ref domain)).Min - param) < 0.001)
		{
			return 0.0;
		}
		domain = crv.Domain;
		return crv.Trim(((Interval)(ref domain)).Min, param).GetLength() / cLength;
	}

	private static void MakeEndProfileParts(UltimateGemData g, Curve crv, Curve baseCrv, Curve profileIn, ValueListItem orientationMethod, double channelWallWidth, double shankWidthAdd, double channelBlend, bool sideBuilt, bool inFront, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1, out Curve profileOut, out double shankWidth)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		CalculatePointsOnCrv(crv, baseCrv, orientationMethod, g);
		ProfilePositionScale(crv, baseCrv, g.Param, g.BaseParam, g.MinCuletMovement, out var pScale, out var pRail);
		profileOut = profileIn.DuplicateCurve();
		shankWidth = channelWallWidth * 2.0 + g.SizeY + shankWidthAdd;
		Transform val = Transform.Scale(Plane.WorldXY, shankWidth, 1.0, 1.0);
		((GeometryBase)profileOut).Transform(pRail * pScale * val);
		if (sideBuilt)
		{
			((GeometryBase)profileOut).Transform(Transform.Translation(0.0, inFront ? ((0.0 - g.SizeY) / 2.0) : (g.SizeY / 2.0), 0.0));
		}
		double length = profileOut.GetLength();
		double normalizedParameter = GetNormalizedParameter(profileOut, length, g.ParamLeft);
		double normalizedParameter2 = GetNormalizedParameter(profileOut, length, g.ParamRight);
		double num = channelBlend / 2.0 / length;
		double num2 = default(double);
		profileOut.NormalizedLengthParameter(normalizedParameter - num, ref num2);
		double num3 = default(double);
		profileOut.NormalizedLengthParameter(normalizedParameter + num, ref num3);
		double num4 = default(double);
		profileOut.NormalizedLengthParameter(normalizedParameter2 - num, ref num4);
		double num5 = default(double);
		profileOut.NormalizedLengthParameter(normalizedParameter2 + num, ref num5);
		Curve obj = profileOut;
		Interval domain = profileOut.Domain;
		Curve item = obj.Trim(((Interval)(ref domain)).Min, num2);
		Curve item2 = profileOut.Trim(num2, num3);
		Curve item3 = profileOut.Trim(num3, num4);
		Curve item4 = profileOut.Trim(num4, num5);
		Curve obj2 = profileOut;
		double num6 = num5;
		domain = profileOut.Domain;
		Curve item5 = obj2.Trim(num6, ((Interval)(ref domain)).Max);
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { item5, item }, CSApplicationHelper.Application.Tolerance);
		if (array != null && array.Length == 1)
		{
			Curve item6 = array[0];
			lstBase.Add(item6);
			lstLeft.Add(item2);
			lstCenter.Add(item3);
			lstRight.Add(item4);
			param.Add(g.Param);
			param1.Add(g.BaseParam);
		}
	}

	private static void MakeGemProfileParts(Curve baseCrv, UltimateGemData g, ValueListItem placementMethod, bool autoChannelDepth, double channelWallDrop, double channelBlend, double bottomOffset, double endChannelWallDrop, bool isEndGem, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = g.ProfileCurve.PointAt(g.ParamLeft);
		Point3d val2 = g.ProfileCurve.PointAt(g.ParamRight);
		double num2;
		double num3;
		if (autoChannelDepth)
		{
			double num = g.SizeZ - g.ZPlacement;
			if (num < 0.0)
			{
				num = 0.0;
			}
			if (placementMethod == ValueListItem.GemPlacementMethodCulet)
			{
				num -= Math.Abs(g.GemZMin) + Math.Abs(g.GemZMax);
			}
			if (placementMethod == ValueListItem.GemPlacementMethodGirdle)
			{
				num -= Math.Abs(g.GemZMax);
			}
			num2 = (((Point3d)(ref val)).Y + ((Point3d)(ref val2)).Y) / 2.0 - num;
			num3 = num - g.SizeY / 2.0 + channelWallDrop;
		}
		else
		{
			num2 = (((Point3d)(ref val)).Y + ((Point3d)(ref val2)).Y) / 2.0 - g.SizeZ + g.MinCuletMovement;
			num3 = channelWallDrop;
		}
		bool flag = num2 + bottomOffset < 0.0;
		bool num4 = num2 + bottomOffset > ((Point3d)(ref val)).Y - channelWallDrop;
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, flag ? 0.0 : (num2 + bottomOffset), 0.0);
		if (num4 && !autoChannelDepth)
		{
			((Point3d)(ref val3)).Y = ((Point3d)(ref val)).Y - channelWallDrop;
		}
		Curve profileCurve = g.ProfileCurve;
		Interval domain = g.ProfileCurve.Domain;
		Curve crv = profileCurve.Trim(((Interval)(ref domain)).Min, g.ParamLeft);
		Curve crv4;
		Curve crv7;
		if (num3 > channelBlend)
		{
			Point3d val4 = default(Point3d);
			((Point3d)(ref val4))._002Ector(val);
			((Point3d)(ref val4)).Y = ((Point3d)(ref val4)).Y - num3;
			Point3d val5 = default(Point3d);
			((Point3d)(ref val5))._002Ector(val2);
			((Point3d)(ref val5)).Y = ((Point3d)(ref val5)).Y - num3;
			Curve crv2 = (Curve)new LineCurve(val, val4);
			Curve crv3 = (Curve)new LineCurve(val4, val3);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, channelBlend);
			Curve item = (Curve)(object)CSUtilCurves.BlendCurve(crv2, crv3);
			crv4 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv2, item, crv3 }, CSApplicationHelper.Application.Tolerance)[0];
			Curve crv5 = (Curve)new LineCurve(val3, val5);
			Curve crv6 = (Curve)new LineCurve(val5, val2);
			CSUtilGeometry.TrimEndOfCrv(ref crv5, channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv6, channelBlend);
			Curve item2 = (Curve)(object)CSUtilCurves.BlendCurve(crv5, crv6);
			crv7 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv5, item2, crv6 }, CSApplicationHelper.Application.Tolerance)[0];
		}
		else
		{
			crv4 = (Curve)new LineCurve(val, val3);
			crv7 = (Curve)new LineCurve(val3, val2);
		}
		Curve profileCurve2 = g.ProfileCurve;
		double paramRight = g.ParamRight;
		domain = g.ProfileCurve.Domain;
		Curve crv8 = profileCurve2.Trim(paramRight, ((Interval)(ref domain)).Max);
		CSUtilGeometry.TrimEndOfCrv(ref crv, channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv4, channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv7, channelBlend);
		CSUtilGeometry.TrimStartOfCrv(ref crv8, channelBlend);
		Curve item3 = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv4);
		Curve val6 = (Curve)(object)CSUtilCurves.BlendCurve(crv4, crv7);
		Curve item4 = (Curve)(object)CSUtilCurves.BlendCurve(crv7, crv8);
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv, crv8 }, CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
		{
			return;
		}
		Curve val7 = array[0];
		array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv4, item3 }, CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
		{
			return;
		}
		Curve val8 = array[0];
		array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { item4, crv7 }, CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
		{
			return;
		}
		Curve val9 = array[0];
		Transform val10 = Transform.Identity;
		if (isEndGem && endChannelWallDrop != 0.0)
		{
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(g.ProfileTransform);
			Vector3d val11 = baseCrv.TangentAt(g.BaseParam);
			((Vector3d)(ref val11)).Unitize();
			val11 *= endChannelWallDrop;
			double num5 = default(double);
			baseCrv.ClosestPoint(origin + val11, ref num5);
			Vector3d val12 = baseCrv.TangentAt(num5);
			if (endChannelWallDrop < 0.0)
			{
				val12 *= -1.0;
			}
			val10 = CSBlockUtil.CSUtilMath.Orient3Point(origin, val11, Vector3d.YAxis, origin + val11, val12, Vector3d.YAxis);
			((Point3d)(ref origin)).Transform(val10);
		}
		((GeometryBase)val7).Transform(g.ProfileTransform);
		((GeometryBase)val8).Transform(g.ProfileTransform);
		((GeometryBase)val9).Transform(g.ProfileTransform);
		((GeometryBase)val6).Transform(g.ProfileTransform);
		((GeometryBase)val7).Transform(val10);
		((GeometryBase)val8).Transform(val10);
		((GeometryBase)val9).Transform(val10);
		((GeometryBase)val6).Transform(val10);
		lstBase.Add(val7);
		lstLeft.Add(val8);
		lstCenter.Add(val6);
		lstRight.Add(val9);
		param.Add(g.Param);
		param1.Add(g.BaseParam);
	}

	private static void ProfilePositionScale(Curve gemCrv, Curve baseCrv, double gemParam, double gemBaseParam, double minCuletGrowUp, out Transform pScale, out Transform pRail)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = gemCrv.PointAt(gemParam);
		Vector3d destination = gemCrv.TangentAt(gemParam);
		Point3d val2 = baseCrv.PointAt(gemBaseParam);
		Vector3d destination2 = val - val2;
		double num = ((Vector3d)(ref destination2)).Length + minCuletGrowUp;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, num, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val2, destination2, destination);
		((Point3d)(ref val2)).Transform(pRail);
	}
}
