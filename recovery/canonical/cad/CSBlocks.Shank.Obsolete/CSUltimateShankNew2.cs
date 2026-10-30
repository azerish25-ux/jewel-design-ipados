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

namespace CSBlocks.Shank.Obsolete;

[Obsolete]
[CSBlock("UltimateShankNew_V2", "{0F55511A-2893-465D-81C4-E0C613463F22}")]
public sealed class CSUltimateShankNew2 : CSBlock
{
	private const bool DebugShow4Rails = false;

	private const bool DebugShow4Rails2 = false;

	private const bool DebugShowChannelWalls = false;

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

	public CSUltimateShankNew2(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InGems = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InChannelWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InPositionMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemsClosedLoop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDirection = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InSideBuild = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFront = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOverrideChannelWallDropStart = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideChannelWallDropMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideChannelWallDropEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InChannelWallDrop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallDropStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelWallDropMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelWallDropEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InChannelBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelPullDown = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShankWidthAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InForceChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
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
		if (value3 != null)
		{
			BoundingBox boundingBox = value3.GetBoundingBox(accurate: true);
			double x = boundingBox.Diagonal.X;
			double y = boundingBox.Diagonal.Y;
			Transform xform = Transform.Translation((0.0 - (boundingBox.Max.X + boundingBox.Min.X)) / 2.0, 0.0 - boundingBox.Min.Y, 0.0);
			value3.Transform(xform);
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			value3.Transform(xform2);
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
		InChannelWallDrop.GetDouble(ref value9);
		double value10 = 0.0;
		InChannelWallGemOverlap.GetDouble(ref value10);
		double value11 = 0.0;
		InChannelBlend.GetDouble(ref value11);
		double value12 = 0.1;
		InChannelPullDown.GetDouble(ref value12);
		double value13 = 0.0;
		InShankWidthAdd.GetDouble(ref value13);
		value13 -= value10 * 2.0;
		bool value14 = true;
		InShowGems.GetBoolean(ref value14);
		InForceChannel.GetValue(out bool value15);
		bool value16 = false;
		InNoChannel.GetBoolean(ref value16);
		bool value17 = false;
		InShankClosed.GetBoolean(ref value17);
		bool value18 = false;
		InAutoChannelDepth.GetBoolean(ref value18);
		bool value19 = false;
		InStartShankProfileEnabled.GetBoolean(ref value19);
		double value20 = 0.0;
		InStartShankProfilePosition.GetDouble(ref value20);
		double value21 = 2.0;
		InStartShankProfileWidth.GetDouble(ref value21);
		bool value22 = false;
		InStartShankProfileTangent.GetBoolean(ref value22);
		bool value23 = false;
		InStartChannelProfileTangent.GetBoolean(ref value23);
		bool value24 = false;
		InEndShankProfileEnabled.GetBoolean(ref value24);
		double value25 = 0.0;
		InEndShankProfilePosition.GetDouble(ref value25);
		double value26 = 2.0;
		InEndShankProfileWidth.GetDouble(ref value26);
		bool value27 = false;
		InEndShankProfileTangent.GetBoolean(ref value27);
		bool value28 = false;
		InEndChannelProfileTangent.GetBoolean(ref value28);
		List<UltimateGemData> list = new List<UltimateGemData>();
		if (!(InGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Ultimate GemData Array");
		}
		double num = double.MinValue;
		for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
		{
			if (!(cSDataArray.GetArrayItem(i) is CSDataArray gemInfo))
			{
				return UpdateMissing("Ultimate GemData GemArray");
			}
			UltimateGemData ultimateGemData = UltimateGemData.ReadFromArray(gemInfo);
			list.Add(ultimateGemData);
			if (ultimateGemData.SizeY > num)
			{
				num = ultimateGemData.SizeY;
			}
		}
		InFront.GetValue(out bool value29);
		InSideBuild.GetValue(out bool value30);
		if (Math.Abs(value13) > 0.01)
		{
			foreach (UltimateGemData item in list)
			{
				double x2 = item.ProfileCurve.GetBoundingBox(accurate: true).Diagonal.X;
				Transform xform3 = Transform.Scale(Plane.WorldXY, (x2 + value13) / x2, 1.0, 1.0);
				item.ProfileCurve.Transform(xform3);
			}
		}
		double value31 = 0.0;
		InPositionMiddle.GetDouble(ref value31);
		bool value32 = false;
		InChannelEnabled.GetBoolean(ref value32);
		bool value33 = false;
		InGemsClosedLoop.GetBoolean(ref value33);
		double value34 = 0.0;
		InDirection.GetDouble(ref value34);
		InOverrideStartOffset.GetValue(out bool value35);
		InOverrideMiddleOffset.GetValue(out bool value36);
		InOverrideEndOffset.GetValue(out bool value37);
		InChannelBottomOffset.GetValue(out double value38);
		InChannelBottomOffset.GetValue(out double value39);
		InChannelBottomOffset.GetValue(out double value40);
		if (value35)
		{
			InChannelBottomOffsetStart.GetValue(out value38);
		}
		if (value36)
		{
			InChannelBottomOffsetMiddle.GetValue(out value39);
		}
		if (value37)
		{
			InChannelBottomOffsetEnd.GetValue(out value40);
		}
		InOverrideChannelWallDropStart.GetValue(out bool value41);
		InOverrideChannelWallDropMiddle.GetValue(out bool value42);
		InOverrideChannelWallDropEnd.GetValue(out bool value43);
		InChannelWallDrop.GetValue(out double value44);
		InChannelWallDrop.GetValue(out double value45);
		InChannelWallDrop.GetValue(out double value46);
		if (value41)
		{
			InChannelWallDropStart.GetValue(out value44);
		}
		if (value42)
		{
			InChannelWallDropMiddle.GetValue(out value45);
		}
		if (value43)
		{
			InChannelWallDropEnd.GetValue(out value46);
		}
		bool noChannel = !value14;
		if (value15)
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
		if (value16)
		{
			BuildChannel(list, value3, value, value2, whichItem, whichItem2, value34, value13, value4, value5, value6, value7, value8, value44, value45, value46, value38, value39, value40, value11, value12, value18, noChannel: true, value32, value33, value17, value30, value29, value19, value20, value21, value22, value23, value24, value25, value26, value27, value28, out channel, out profileStartArray, out profileEndArray, out sweepRailsOuter, out sweepRailsInner, out channelWidthStart, out channelWidthEnd, out sweepBaseSurface, out sweepBaseLeftSurface, out sweepBaseRightSurface, out sweepChannelLeftSurface, out sweepChannelRightSurface, out sweepChannelCenterSurface, out sweepTopStartSurface, out sweepTopEndSurface);
		}
		else
		{
			channel = new CSData();
		}
		BuildChannel(list, value3, value, value2, whichItem, whichItem2, value34, value13, value4, value5, value6, value7, value8, value44, value45, value46, value38, value39, value40, value11, value12, value18, noChannel, value32, value33, value17, value30, value29, value19, value20, value21, value22, value23, value24, value25, value26, value27, value28, out var channel2, out profileStartArray, out profileEndArray, out sweepRailsOuter, out sweepRailsInner, out channelWidthStart, out channelWidthEnd, out sweepBaseSurface, out sweepBaseLeftSurface, out sweepBaseRightSurface, out sweepChannelLeftSurface, out sweepChannelRightSurface, out sweepChannelCenterSurface, out sweepTopStartSurface, out sweepTopEndSurface);
		OutSurface.SetData(channel2);
		OutProfileStart.SetData(profileStartArray);
		OutProfileEnd.SetData(profileEndArray);
		OutSweepRailsOuter.SetData(sweepRailsOuter);
		OutSweepRailsInner.SetData(sweepRailsInner);
		OutUntrimmed.SetData(channel);
		OutChannelWidthStart.SetData(new CSNumberData(channelWidthStart));
		OutChannelWidthEnd.SetData(new CSNumberData(channelWidthEnd));
		OutBaseSurface.SetData(new CSOnGeometryData(sweepBaseSurface));
		OutBaseLeftSurface.SetData(new CSOnGeometryData(sweepBaseLeftSurface));
		OutBaseRightSurface.SetData(new CSOnGeometryData(sweepBaseRightSurface));
		OutChannelLeftSurface.SetData(new CSOnGeometryData(sweepChannelLeftSurface));
		OutChannelRightSurface.SetData(new CSOnGeometryData(sweepChannelRightSurface));
		OutChannelCenterSurface.SetData(new CSOnGeometryData(sweepChannelCenterSurface));
		OutTopStartSurface.SetData(new CSOnGeometryData(sweepTopStartSurface));
		OutTopEndSurface.SetData(new CSOnGeometryData(sweepTopEndSurface));
		return base.Update();
	}

	private static void BuildChannel(List<UltimateGemData> gemList, Curve profile, Curve crv, Curve baseCrv, ValueListItem orientationMethod, ValueListItem placementMethod, double direction, double shankWidthAdd, double firstChannelProfilePosition, double firstChannelProfileY, double lastChannelProfilePosition, double lastChannelProfileY, double channelWallWidth, double channelWallDropStart, double channelWallDropMiddle, double channelWallDropEnd, double channelBottomOffsetStart, double channelBottomOffsetMiddle, double channelBottomOffsetEnd, double channelBlend, double channelEndPulldownDistance, bool autoChannelDepth, bool noChannel, bool channelEnabled, bool gemsClosedLoop, bool closedShank, bool sideBuilt, bool inFront, bool startShankProfileEnabled, double startShankProfilePosition, double startShankProfileWidth, bool startShankProfileTangent, bool startChannelProfileTangent, bool endShankProfileEnabled, double endShankProfilePosition, double endShankProfileWidth, bool endShankProfileTangent, bool endChannelProfileTangent, out CSData channel, out CSDataArray profileStartArray, out CSDataArray profileEndArray, out CSDataArray sweepRailsOuter, out CSData sweepRailsInner, out double channelWidthStart, out double channelWidthEnd, out Brep sweepBaseSurface, out Brep sweepBaseLeftSurface, out Brep sweepBaseRightSurface, out Brep sweepChannelLeftSurface, out Brep sweepChannelRightSurface, out Brep sweepChannelCenterSurface, out Brep sweepTopStartSurface, out Brep sweepTopEndSurface)
	{
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
					SizeY = startShankProfileWidth - channelWallWidth
				};
				list8.Add(startShankProfileWidth / 2.0);
				MakeEndProfileParts(g, crv, baseCrv, profile, orientationMethod, channelWallWidth, shankWidthAdd, channelBlend, sideBuilt, inFront, list, list2, list3, list4, list9, list10, out profileOut, out var _);
				cSProfileData = CSProfileData.GetCSProfile(profileOut, "StartProfile");
				profileStartArray = new CSDataArray(cSProfileData);
				profileStartArray.Append(cSProfileData);
				profileStartArray.Append(new CSNumberData(list10[list10.Count - 1]));
				profileStartArray.Append(new CSNumberData(list9[list9.Count - 1]));
			}
			UltimateGemData ultimateGemData = ((gemList.Count <= 0) ? new UltimateGemData
			{
				ParamLeft = profile.Domain.Min + profile.Domain.Length * 0.4,
				ParamRight = profile.Domain.Min + profile.Domain.Length * 0.6
			} : new UltimateGemData
			{
				ParamLeft = gemList[0].ParamLeft,
				ParamRight = gemList[0].ParamRight
			});
			ultimateGemData.NormParam = firstChannelProfilePosition;
			ultimateGemData.SizeY = firstChannelProfileY - channelWallWidth;
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
			if (i <= (gemList.Count - 1) / 2)
			{
				bottomOffset = channelBottomOffsetStart * ((double)(gemList.Count - 1) / 2.0 - (double)i) / ((double)(gemList.Count - 1) / 2.0) + channelBottomOffsetMiddle * ((double)i / ((double)(gemList.Count - 1) / 2.0));
			}
			if (i > (gemList.Count - 1) / 2)
			{
				bottomOffset = channelBottomOffsetMiddle * ((double)(gemList.Count - 1) - (double)i) / (double)(gemList.Count - 1) + channelBottomOffsetEnd * ((double)i / (double)(gemList.Count - 1));
			}
			double channelWallDrop = 0.0;
			if (i <= (gemList.Count - 1) / 2)
			{
				channelWallDrop = channelWallDropStart * ((double)(gemList.Count - 1) / 2.0 - (double)i) / ((double)(gemList.Count - 1) / 2.0) + channelWallDropMiddle * ((double)i / ((double)(gemList.Count - 1) / 2.0));
			}
			if (i > (gemList.Count - 1) / 2)
			{
				channelWallDrop = channelWallDropMiddle * ((double)(gemList.Count - 1) - (double)i) / (double)(gemList.Count - 1) + channelWallDropEnd * ((double)i / (double)(gemList.Count - 1));
			}
			MakeGemProfileParts(gemList[i], placementMethod, autoChannelDepth, channelWallDrop, channelBlend, bottomOffset, list, list2, list3, list4, list9, list10);
		}
		if (!gemsClosedLoop)
		{
			UltimateGemData ultimateGemData2 = ((gemList.Count <= 0) ? new UltimateGemData
			{
				ParamLeft = profile.Domain.Min + profile.Domain.Length * 0.4,
				ParamRight = profile.Domain.Min + profile.Domain.Length * 0.6
			} : new UltimateGemData
			{
				ParamLeft = gemList[gemList.Count - 1].ParamLeft,
				ParamRight = gemList[gemList.Count - 1].ParamRight
			});
			ultimateGemData2.NormParam = lastChannelProfilePosition;
			ultimateGemData2.SizeY = lastChannelProfileY - channelWallWidth;
			list8.Add(lastChannelProfileY / 2.0);
			MakeEndProfileParts(ultimateGemData2, crv, baseCrv, profile, orientationMethod, channelWallWidth, shankWidthAdd, channelBlend, sideBuilt, inFront, list, list2, list3, list4, list9, list10, out var profileOut3, out channelWidthEnd);
			if (endShankProfileEnabled)
			{
				UltimateGemData g2 = new UltimateGemData
				{
					ParamLeft = gemList[gemList.Count - 1].ParamLeft,
					ParamRight = gemList[gemList.Count - 1].ParamRight,
					NormParam = endShankProfilePosition,
					SizeY = endShankProfileWidth - channelWallWidth
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
			if (CSUtilSweepRC.Sweep2(crv, baseCrv, list, list9, list10, out sweepBaseSurface, true, true) && sweepBaseSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list2, list9, list10, out sweepChannelLeftSurface, true, true) && sweepChannelLeftSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list4, list9, list10, out sweepChannelRightSurface, true, true) && sweepChannelRightSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list3, list9, list10, out sweepChannelCenterSurface, true, true) && sweepChannelCenterSurface != null)
			{
				Brep[] array2 = Brep.JoinBreps(new List<Brep> { sweepBaseSurface, sweepChannelLeftSurface, sweepChannelCenterSurface, sweepChannelRightSurface }, CSApplicationHelper.Application.Tolerance);
				if (array2.Length == 1)
				{
					channel = CSOnGeometryData.FromBrep(array2[0]);
				}
			}
			return;
		}
		if (list.Count <= 2)
		{
			if (CSUtilSweepRC.Sweep2(crv, baseCrv, list, list9, list10, out sweepBaseSurface, false, true) && sweepBaseSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list2, list9, list10, out sweepChannelLeftSurface, false, true) && sweepChannelLeftSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list4, list9, list10, out sweepChannelRightSurface, false, true) && sweepChannelRightSurface != null && CSUtilSweepRC.Sweep2(crv, baseCrv, list3, list9, list10, out sweepChannelCenterSurface, false, true) && sweepChannelCenterSurface != null)
			{
				Brep[] array3 = Brep.JoinBreps(new List<Brep> { sweepBaseSurface, sweepChannelLeftSurface, sweepChannelCenterSurface, sweepChannelRightSurface }, CSApplicationHelper.Application.Tolerance);
				if (array3.Length == 1)
				{
					channel = CSOnGeometryData.FromBrep(array3[0]);
				}
			}
			return;
		}
		Create4Rails(crv, baseCrv, channelWallWidth, list8, list9, list10, startShankProfileEnabled, startShankProfileTangent, startChannelProfileTangent, endShankProfileEnabled, endShankProfileTangent, endChannelProfileTangent, out var baseCrv2, out var baseCrv3, out var gemCrv, out var gemCrv2, out var baseCrv0Param, out var baseCrv1Param, out var gemCrv0Param, out var gemCrv1Param);
		sweepRailsOuter.Append(new CSOnGeometryData(baseCrv3));
		sweepRailsOuter.Append(new CSOnGeometryData(gemCrv2));
		sweepRailsOuter.Append(new CSOnGeometryData(gemCrv));
		sweepRailsOuter.Append(new CSOnGeometryData(baseCrv2));
		double t;
		if (cSProfileData != null)
		{
			cSProfileData.SetCurveParameterCount(4);
			Curve curve = cSProfileData.GetCurve();
			curve.ClosestPoint(baseCrv2.PointAtStart, out t);
			cSProfileData.SetCurveParameter(0, t);
			curve.ClosestPoint(gemCrv.PointAtStart, out t);
			cSProfileData.SetCurveParameter(1, t);
			curve.ClosestPoint(gemCrv2.PointAtStart, out t);
			cSProfileData.SetCurveParameter(2, t);
			curve.ClosestPoint(baseCrv3.PointAtStart, out t);
			cSProfileData.SetCurveParameter(3, t);
		}
		if (cSProfileData2 != null)
		{
			cSProfileData2.SetCurveParameterCount(4);
			Curve curve2 = cSProfileData2.GetCurve();
			curve2.ClosestPoint(baseCrv2.PointAtEnd, out t);
			cSProfileData2.SetCurveParameter(0, t);
			curve2.ClosestPoint(gemCrv.PointAtEnd, out t);
			cSProfileData2.SetCurveParameter(1, t);
			curve2.ClosestPoint(gemCrv2.PointAtEnd, out t);
			cSProfileData2.SetCurveParameter(2, t);
			curve2.ClosestPoint(baseCrv3.PointAtEnd, out t);
			cSProfileData2.SetCurveParameter(3, t);
		}
		for (int j = 0; j < list.Count; j++)
		{
			Curve curve3 = list[j];
			Point3d testPoint = baseCrv2.PointAt(baseCrv0Param[j]);
			curve3.ClosestPoint(testPoint, out var t2);
			Point3d testPoint2 = baseCrv3.PointAt(baseCrv1Param[j]);
			curve3.ClosestPoint(testPoint2, out var t3);
			list5.Add(curve3.Trim(curve3.Domain.Min, t2));
			list6.Add(curve3.Trim(t2, t3));
			list7.Add(curve3.Trim(t3, curve3.Domain.Max));
		}
		if (!CSUtilSweepRC.Sweep2(baseCrv3, gemCrv2, list7, baseCrv1Param, gemCrv1Param, out sweepBaseLeftSurface, false, true) || sweepBaseLeftSurface == null || !CSUtilSweepRC.Sweep2(baseCrv2, gemCrv, list5, baseCrv0Param, gemCrv1Param, out sweepBaseRightSurface, false, true) || sweepBaseRightSurface == null)
		{
			return;
		}
		gemCrv = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseRightSurface, list5[0].PointAtStart, list5[list5.Count - 1].PointAtStart, list5[1].PointAtStart);
		baseCrv2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseRightSurface, list5[0].PointAtEnd, list5[list5.Count - 1].PointAtEnd, list5[1].PointAtEnd);
		baseCrv3 = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseLeftSurface, list7[0].PointAtStart, list7[list7.Count - 1].PointAtStart, list7[1].PointAtStart);
		gemCrv2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(sweepBaseLeftSurface, list7[0].PointAtEnd, list7[list7.Count - 1].PointAtEnd, list7[1].PointAtEnd);
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData(baseCrv2));
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData(gemCrv));
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData(gemCrv2));
		((CSDataArray)sweepRailsInner).Append(new CSOnGeometryData(baseCrv3));
		for (int k = 0; k < list.Count; k++)
		{
			gemCrv.ClosestPoint(list5[k].PointAtStart, out var t4);
			gemCrv0Param[k] = t4;
			gemCrv2.ClosestPoint(list7[k].PointAtEnd, out t4);
			gemCrv1Param[k] = t4;
			baseCrv2.ClosestPoint(list5[k].PointAtEnd, out t4);
			baseCrv0Param[k] = t4;
			baseCrv3.ClosestPoint(list7[k].PointAtStart, out t4);
			baseCrv1Param[k] = t4;
		}
		if (!CSUtilSweepRC.Sweep2(baseCrv2, baseCrv3, list6, baseCrv0Param, baseCrv1Param, out sweepBaseSurface, false, true) || sweepBaseSurface == null)
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
		if (!CSUtilSweepRC.Sweep2(gemCrv, gemCrv2, list12, list15, list16, out sweepChannelRightSurface, false, true) || sweepChannelRightSurface == null || !CSUtilSweepRC.Sweep2(gemCrv, gemCrv2, list13, list15, list16, out sweepChannelLeftSurface, false, true) || sweepChannelLeftSurface == null)
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
		curveFromBrepEdges.ClosestPoint(list14[0].PointAtStart, out var t5, CSApplicationHelper.Application.Tolerance);
		list9.Add(t5);
		curveFromBrepEdges2.ClosestPoint(list14[0].PointAtEnd, out var t6, CSApplicationHelper.Application.Tolerance);
		list10.Add(t6);
		if (channelEndPulldownDistance > 0.01 && !noChannel)
		{
			double normalizedParameter = GetNormalizedParameter(curveFromBrepEdges, length, t5);
			curveFromBrepEdges.NormalizedLengthParameter(normalizedParameter + channelEndPulldownDistance / length, out var t7);
			Point3d point3d = curveFromBrepEdges.PointAt(t7);
			double normalizedParameter2 = GetNormalizedParameter(curveFromBrepEdges2, length2, t6);
			curveFromBrepEdges2.NormalizedLengthParameter(normalizedParameter2 + channelEndPulldownDistance / length2, out var t8);
			Point3d to = curveFromBrepEdges2.PointAt(t8);
			Curve item = new LineCurve(point3d, to);
			list17.Add(item);
			list9.Add(t7);
			list10.Add(t8);
		}
		list17.Add(list14[1]);
		curveFromBrepEdges.ClosestPoint(list14[1].PointAtStart, out var t9, CSApplicationHelper.Application.Tolerance);
		list9.Add(t9);
		curveFromBrepEdges2.ClosestPoint(list14[1].PointAtEnd, out t9, CSApplicationHelper.Application.Tolerance);
		list10.Add(t9);
		int index2 = list14.Count - 2;
		if (list14.Count > 3)
		{
			list17.Add(list14[index2]);
			curveFromBrepEdges.ClosestPoint(list14[index2].PointAtStart, out t9, CSApplicationHelper.Application.Tolerance);
			list9.Add(t9);
			curveFromBrepEdges2.ClosestPoint(list14[index2].PointAtEnd, out t9, CSApplicationHelper.Application.Tolerance);
			list10.Add(t9);
		}
		curveFromBrepEdges.ClosestPoint(list14[list14.Count - 1].PointAtStart, out t5, CSApplicationHelper.Application.Tolerance);
		curveFromBrepEdges2.ClosestPoint(list14[list14.Count - 1].PointAtEnd, out t6, CSApplicationHelper.Application.Tolerance);
		if (channelEndPulldownDistance > 0.01 && !noChannel)
		{
			double normalizedParameter3 = GetNormalizedParameter(curveFromBrepEdges, length, t5);
			curveFromBrepEdges.NormalizedLengthParameter(normalizedParameter3 - channelEndPulldownDistance / length, out var t10);
			double normalizedParameter4 = GetNormalizedParameter(curveFromBrepEdges2, length2, t6);
			curveFromBrepEdges2.NormalizedLengthParameter(normalizedParameter4 - channelEndPulldownDistance / length2, out var t11);
			Point3d point3d2 = curveFromBrepEdges.PointAt(t10);
			Point3d to2 = curveFromBrepEdges2.PointAt(t11);
			Curve item2 = new LineCurve(point3d2, to2);
			list17.Add(item2);
			list9.Add(t10);
			list10.Add(t11);
		}
		list17.Add(list14[list14.Count - 1]);
		list9.Add(t5);
		list10.Add(t6);
		if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges, curveFromBrepEdges2, list17, list9, list10, out sweepChannelCenterSurface, false, true) || sweepChannelCenterSurface == null)
		{
			return;
		}
		if (startShankProfileEnabled)
		{
			Curve[] array4 = Curve.JoinCurves(new List<Curve>
			{
				list4[0],
				list3[0],
				list2[0]
			});
			Curve[] array5 = Curve.JoinCurves(new List<Curve>
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
			}, out sweepTopStartSurface, false, true) || sweepTopStartSurface == null)
			{
				return;
			}
		}
		if (endShankProfileEnabled)
		{
			int num = list4.Count - 1;
			Curve[] array6 = Curve.JoinCurves(new List<Curve>
			{
				list4[num - 1],
				list3[num - 1],
				list2[num - 1]
			});
			Curve[] array7 = Curve.JoinCurves(new List<Curve>
			{
				list4[num],
				list3[num],
				list2[num]
			});
			if (!CSUtilSweepRC.Sweep2(gemCrv, gemCrv2, new List<Curve>
			{
				array6[0],
				array7[0]
			}, new List<double>
			{
				gemCrv0Param[num - 1],
				gemCrv0Param[num]
			}, new List<double>
			{
				gemCrv1Param[num - 1],
				gemCrv1Param[num]
			}, out sweepTopEndSurface, false, true) || sweepTopEndSurface == null)
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
		Brep[] array8 = Brep.JoinBreps(list18, CSApplicationHelper.Application.Tolerance);
		if (array8.Length == 1)
		{
			channel = CSOnGeometryData.FromBrep(array8[0]);
		}
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem orientationMethod, UltimateGemData gem)
	{
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
		crv.NormalizedLengthParameter(num, out gem.Param);
		Point3d point3d = crv.PointAt(gem.Param);
		Vector3d normal = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (orientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
		{
			baseCrv.ClosestPoint(point3d, out gem.BaseParam);
			return;
		}
		Plane plane = new Plane(point3d, normal);
		CurveIntersections curveIntersections = Intersection.CurvePlane(baseCrv, plane, 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			double length = (item.PointA - point3d).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private static void Create4Rails(Curve crv, Curve baseCrv, double channelWallWidth, List<double> shankWidthatGem, List<double> gemCrvParamIn, List<double> baseCrvParamIn, bool startShankProfileEnabled, bool startShankProfileTangent, bool startChannelProfileTangent, bool endShankProfileEnabled, bool endShankProfileTangent, bool endChannelProfileTangent, out Curve baseCrv0, out Curve baseCrv1, out Curve gemCrv0, out Curve gemCrv1, out List<double> baseCrv0Param, out List<double> baseCrv1Param, out List<double> gemCrv0Param, out List<double> gemCrv1Param)
	{
		List<double> list = new List<double>(gemCrvParamIn);
		List<double> list2 = new List<double>(baseCrvParamIn);
		Curve curve = crv.DuplicateCurve();
		Curve curve2 = baseCrv.DuplicateCurve();
		if (curve.IsClosed && curve2.IsClosed)
		{
			curve.ChangeClosedCurveSeam(list[0]);
			curve2.ChangeClosedCurveSeam(list2[0]);
			list[0] = curve.Domain.Min;
			list2[0] = curve2.Domain.Min;
			for (int i = 1; i < shankWidthatGem.Count; i++)
			{
				Point3d testPoint = crv.PointAt(list[i]);
				curve.ClosestPoint(testPoint, out var t);
				list[i] = t;
				testPoint = baseCrv.PointAt(list2[i]);
				curve2.ClosestPoint(testPoint, out t);
				list2[i] = t;
			}
		}
		double num = 0.0;
		foreach (double item3 in shankWidthatGem)
		{
			if (item3 > num)
			{
				num = item3;
			}
		}
		num = (num + channelWallWidth) / 2.0;
		num *= 1.5;
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<Point3d> list5 = new List<Point3d>();
		List<Point3d> list6 = new List<Point3d>();
		for (int j = 0; j < shankWidthatGem.Count; j++)
		{
			ProfilePositionScale(curve, curve2, list[j], list2[j], out var pScale, out var pRail);
			LineCurve lineCurve = new LineCurve(new Point3d(0.0 - num, 0.0, 0.0), new Point3d(num, 0.0, 0.0));
			lineCurve.Transform(pRail);
			list3.Add(lineCurve);
			LineCurve lineCurve2 = new LineCurve(new Point3d(0.0 - num, 1.0, 0.0), new Point3d(num, 1.0, 0.0));
			lineCurve2.Transform(pScale);
			lineCurve2.Transform(pRail);
			list4.Add(lineCurve2);
			Point3d item = new Point3d(0.0, 0.0, 0.0);
			item.Transform(pRail);
			list5.Add(item);
			Point3d item2 = new Point3d(0.0, 1.0, 0.0);
			item2.Transform(pScale);
			item2.Transform(pRail);
			list6.Add(item2);
		}
		SweepTwoRail obj = new SweepTwoRail
		{
			SweepTolerance = CSApplicationHelper.Application.Tolerance,
			AngleToleranceRadians = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians,
			MaintainHeight = true,
			ClosedSweep = false
		};
		Brep[] array = obj.PerformSweep(curve, curve2, list3, list, list2);
		Brep[] array2 = obj.PerformSweep(curve, curve2, list4, list, list2);
		NurbsSurface nurbsSurface = null;
		if (array[0].Faces.Count > 0)
		{
			nurbsSurface = array[0].Faces[0].ToNurbsSurface();
		}
		NurbsSurface nurbsSurface2 = null;
		if (array2[0].Faces.Count > 0)
		{
			nurbsSurface2 = array2[0].Faces[0].ToNurbsSurface();
		}
		if (nurbsSurface == null || nurbsSurface2 == null)
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
		for (int k = 0; k < shankWidthatGem.Count; k++)
		{
			double num2 = shankWidthatGem[k];
			nurbsSurface.ClosestPoint(list5[k], out var u, out var v);
			list7.Add(new Point3d(u, v + num2, 0.0));
			list8.Add(new Point3d(u, v - num2, 0.0));
			nurbsSurface2.ClosestPoint(list6[k], out u, out v);
			list9.Add(new Point3d(u, v + num2, 0.0));
			list10.Add(new Point3d(u, v - num2, 0.0));
		}
		List<Point3d> list11 = new List<Point3d>(list7);
		List<Point3d> list12 = new List<Point3d>(list8);
		List<Point3d> list13 = new List<Point3d>(list9);
		List<Point3d> list14 = new List<Point3d>(list10);
		Vector3d startTangent = Vector3d.Unset;
		Vector3d endTangent = Vector3d.Unset;
		if ((startShankProfileTangent && startShankProfileEnabled) || (startChannelProfileTangent && !startShankProfileEnabled))
		{
			startTangent = Vector3d.XAxis;
		}
		if (startChannelProfileTangent && startShankProfileEnabled)
		{
			list11.Insert(1, new Point3d(list11[1].X - 0.2, list11[1].Y, 0.0));
			list12.Insert(1, new Point3d(list12[1].X - 0.2, list12[1].Y, 0.0));
			list13.Insert(1, new Point3d(list13[1].X - 0.2, list13[1].Y, 0.0));
			list14.Insert(1, new Point3d(list14[1].X - 0.2, list14[1].Y, 0.0));
			list11.Insert(1, new Point3d(list11[1].X - 0.2, list11[1].Y, 0.0));
			list12.Insert(1, new Point3d(list12[1].X - 0.2, list12[1].Y, 0.0));
			list13.Insert(1, new Point3d(list13[1].X - 0.2, list13[1].Y, 0.0));
			list14.Insert(1, new Point3d(list14[1].X - 0.2, list14[1].Y, 0.0));
		}
		if ((endShankProfileTangent && endShankProfileEnabled) || (endChannelProfileTangent && !endShankProfileEnabled))
		{
			endTangent = Vector3d.XAxis;
		}
		if (endChannelProfileTangent && endShankProfileEnabled)
		{
			int num3 = list11.Count - 2;
			list11.Insert(num3 + 1, new Point3d(list11[num3].X + 0.2, list11[num3].Y, 0.0));
			list12.Insert(num3 + 1, new Point3d(list12[num3].X + 0.2, list12[num3].Y, 0.0));
			list13.Insert(num3 + 1, new Point3d(list13[num3].X + 0.2, list13[num3].Y, 0.0));
			list14.Insert(num3 + 1, new Point3d(list14[num3].X + 0.2, list14[num3].Y, 0.0));
			num3 = list11.Count - 2;
			list11.Insert(num3 + 1, new Point3d(list11[num3].X + 0.2, list11[num3].Y, 0.0));
			list12.Insert(num3 + 1, new Point3d(list12[num3].X + 0.2, list12[num3].Y, 0.0));
			list13.Insert(num3 + 1, new Point3d(list13[num3].X + 0.2, list13[num3].Y, 0.0));
			list14.Insert(num3 + 1, new Point3d(list14[num3].X + 0.2, list14[num3].Y, 0.0));
		}
		Curve curve2d = Curve.CreateInterpolatedCurve(list11, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		Curve curve2d2 = Curve.CreateInterpolatedCurve(list12, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		Curve curve2d3 = Curve.CreateInterpolatedCurve(list13, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		Curve curve2d4 = Curve.CreateInterpolatedCurve(list14, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		baseCrv0 = nurbsSurface.Pushup(curve2d, 0.01);
		baseCrv1 = nurbsSurface.Pushup(curve2d2, 0.01);
		gemCrv0 = nurbsSurface2.Pushup(curve2d3, 0.01);
		gemCrv1 = nurbsSurface2.Pushup(curve2d4, 0.01);
		baseCrv0Param = new List<double>();
		baseCrv1Param = new List<double>();
		gemCrv0Param = new List<double>();
		gemCrv1Param = new List<double>();
		baseCrv0Param.Add(baseCrv0.Domain.Min);
		baseCrv1Param.Add(baseCrv1.Domain.Min);
		gemCrv0Param.Add(gemCrv0.Domain.Min);
		gemCrv1Param.Add(gemCrv1.Domain.Min);
		for (int l = 1; l < shankWidthatGem.Count; l++)
		{
			Point3d testPoint2 = nurbsSurface.PointAt(list7[l].X, list7[l].Y);
			baseCrv0.ClosestPoint(testPoint2, out var t2, 0.01);
			baseCrv0Param.Add(t2);
			testPoint2 = nurbsSurface.PointAt(list8[l].X, list8[l].Y);
			baseCrv1.ClosestPoint(testPoint2, out t2, 0.01);
			baseCrv1Param.Add(t2);
			testPoint2 = nurbsSurface2.PointAt(list9[l].X, list9[l].Y);
			gemCrv0.ClosestPoint(testPoint2, out t2, 0.01);
			gemCrv0Param.Add(t2);
			testPoint2 = nurbsSurface2.PointAt(list10[l].X, list10[l].Y);
			gemCrv1.ClosestPoint(testPoint2, out t2, 0.01);
			gemCrv1Param.Add(t2);
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
					array[num6] = true;
					num6 += 4;
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
		if (Math.Abs(crv.Domain.Min - param) < 0.001)
		{
			return 0.0;
		}
		return crv.Trim(crv.Domain.Min, param).GetLength() / cLength;
	}

	private static void MakeEndProfileParts(UltimateGemData g, Curve crv, Curve baseCrv, Curve profileIn, ValueListItem orientationMethod, double channelWallWidth, double shankWidthAdd, double channelBlend, bool sideBuilt, bool inFront, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1, out Curve profileOut, out double shankWidth)
	{
		CalculatePointsOnCrv(crv, baseCrv, orientationMethod, g);
		ProfilePositionScale(crv, baseCrv, g.Param, g.BaseParam, out var pScale, out var pRail);
		profileOut = profileIn.DuplicateCurve();
		shankWidth = channelWallWidth * 2.0 + g.SizeY + shankWidthAdd;
		Transform transform = Transform.Scale(Plane.WorldXY, shankWidth, 1.0, 1.0);
		profileOut.Transform(pRail * pScale * transform);
		if (sideBuilt)
		{
			profileOut.Transform(Transform.Translation(0.0, inFront ? ((0.0 - g.SizeY) / 2.0) : (g.SizeY / 2.0), 0.0));
		}
		double length = profileOut.GetLength();
		double normalizedParameter = GetNormalizedParameter(profileOut, length, g.ParamLeft);
		double normalizedParameter2 = GetNormalizedParameter(profileOut, length, g.ParamRight);
		double num = channelBlend / 2.0 / length;
		profileOut.NormalizedLengthParameter(normalizedParameter - num, out var t);
		profileOut.NormalizedLengthParameter(normalizedParameter + num, out var t2);
		profileOut.NormalizedLengthParameter(normalizedParameter2 - num, out var t3);
		profileOut.NormalizedLengthParameter(normalizedParameter2 + num, out var t4);
		Curve item = profileOut.Trim(profileOut.Domain.Min, t);
		Curve item2 = profileOut.Trim(t, t2);
		Curve item3 = profileOut.Trim(t2, t3);
		Curve item4 = profileOut.Trim(t3, t4);
		Curve item5 = profileOut.Trim(t4, profileOut.Domain.Max);
		Curve[] array = Curve.JoinCurves(new List<Curve> { item5, item }, CSApplicationHelper.Application.Tolerance);
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

	private static void MakeGemProfileParts(UltimateGemData g, ValueListItem placementMethod, bool autoChannelDepth, double channelWallDrop, double channelBlend, double bottomOffset, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1)
	{
		Point3d point3d = g.ProfileCurve.PointAt(g.ParamLeft);
		Point3d point3d2 = g.ProfileCurve.PointAt(g.ParamRight);
		double num2;
		double num3;
		if (autoChannelDepth)
		{
			double num = g.SizeZ - g.ZPlacement;
			if (placementMethod == ValueListItem.GemPlacementMethodCulet)
			{
				num -= Math.Abs(g.GemZMin) + Math.Abs(g.GemZMax);
			}
			if (placementMethod == ValueListItem.GemPlacementMethodGirdle)
			{
				num -= Math.Abs(g.GemZMax);
			}
			if (num < 0.0)
			{
				num = 0.0;
			}
			num2 = (point3d.Y + point3d2.Y) / 2.0 - num;
			num3 = num - g.SizeY / 2.0;
		}
		else
		{
			num2 = (point3d.Y + point3d2.Y) / 2.0 - g.SizeZ;
			num3 = channelWallDrop;
		}
		bool flag = num2 + bottomOffset < 0.0;
		bool num4 = num2 + bottomOffset > point3d.Y - channelWallDrop;
		Point3d point3d3 = new Point3d(0.0, flag ? 0.0 : (num2 + bottomOffset), 0.0);
		if (num4 && !autoChannelDepth)
		{
			point3d3.Y = point3d.Y - channelWallDrop;
		}
		Curve crv = g.ProfileCurve.Trim(g.ProfileCurve.Domain.Min, g.ParamLeft);
		Curve crv4;
		Curve crv7;
		if (num3 > channelBlend)
		{
			Point3d point3d4 = new Point3d(point3d);
			point3d4.Y -= num3;
			Point3d point3d5 = new Point3d(point3d2);
			point3d5.Y -= num3;
			Curve crv2 = new LineCurve(point3d, point3d4);
			Curve crv3 = new LineCurve(point3d4, point3d3);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, channelBlend);
			Curve item = CSUtilCurves.BlendCurve(crv2, crv3);
			crv4 = Curve.JoinCurves(new List<Curve> { crv2, item, crv3 }, CSApplicationHelper.Application.Tolerance)[0];
			Curve crv5 = new LineCurve(point3d3, point3d5);
			Curve crv6 = new LineCurve(point3d5, point3d2);
			CSUtilGeometry.TrimEndOfCrv(ref crv5, channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv6, channelBlend);
			Curve item2 = CSUtilCurves.BlendCurve(crv5, crv6);
			crv7 = Curve.JoinCurves(new List<Curve> { crv5, item2, crv6 }, CSApplicationHelper.Application.Tolerance)[0];
		}
		else
		{
			crv4 = new LineCurve(point3d, point3d3);
			crv7 = new LineCurve(point3d3, point3d2);
		}
		Curve crv8 = g.ProfileCurve.Trim(g.ParamRight, g.ProfileCurve.Domain.Max);
		CSUtilGeometry.TrimEndOfCrv(ref crv, channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv4, channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv7, channelBlend);
		CSUtilGeometry.TrimStartOfCrv(ref crv8, channelBlend);
		Curve item3 = CSUtilCurves.BlendCurve(crv, crv4);
		Curve curve = CSUtilCurves.BlendCurve(crv4, crv7);
		Curve item4 = CSUtilCurves.BlendCurve(crv7, crv8);
		Curve[] array = Curve.JoinCurves(new List<Curve> { crv, crv8 }, CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
		{
			return;
		}
		Curve curve2 = array[0];
		array = Curve.JoinCurves(new List<Curve> { crv4, item3 }, CSApplicationHelper.Application.Tolerance);
		if (array != null && array.Length == 1)
		{
			Curve curve3 = array[0];
			array = Curve.JoinCurves(new List<Curve> { item4, crv7 }, CSApplicationHelper.Application.Tolerance);
			if (array != null && array.Length == 1)
			{
				Curve curve4 = array[0];
				curve2.Transform(g.ProfileTransform);
				curve3.Transform(g.ProfileTransform);
				curve4.Transform(g.ProfileTransform);
				curve.Transform(g.ProfileTransform);
				lstBase.Add(curve2);
				lstLeft.Add(curve3);
				lstCenter.Add(curve);
				lstRight.Add(curve4);
				param.Add(g.Param);
				param1.Add(g.BaseParam);
			}
		}
	}

	private static void ProfilePositionScale(Curve gemCrv, Curve baseCrv, double gemParam, double gemBaseParam, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = gemCrv.PointAt(gemParam);
		Vector3d destination = gemCrv.TangentAt(gemParam);
		Point3d point3d2 = baseCrv.PointAt(gemBaseParam);
		Vector3d destination2 = point3d - point3d2;
		double length = destination2.Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d, destination2, destination);
		point3d2.Transform(pRail);
	}
}
