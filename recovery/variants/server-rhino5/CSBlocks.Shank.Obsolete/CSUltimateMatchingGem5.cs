using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSBlocks.Shank.Helpers;
using CSDatas;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.Obsolete;

[Obsolete]
[CSBlock("UltimateMatchingGem_V5", "{A697D6F2-2DA8-4DB0-9D7F-2FCCE129CFF4}")]
public sealed class CSUltimateMatchingGem5 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseObject;

	public readonly CSInputChannel InGemShape;

	public readonly CSInputChannel InDiameterStart;

	public readonly CSInputChannel InDiameterMiddle;

	public readonly CSInputChannel InDiameterEnd;

	public readonly CSInputChannel InPositionStart;

	public readonly CSInputChannel InPositionEnd;

	public readonly CSInputChannel InPositionMiddle;

	public readonly CSInputChannel InPlacementStart;

	public readonly CSInputChannel InPlacementMiddle;

	public readonly CSInputChannel InPlacementEnd;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InOrientationMethod;

	public readonly CSInputChannel InOrientationFollowCurve;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InSpacingMethod;

	public readonly CSInputChannel InEnableDiameterMiddle;

	public readonly CSInputChannel InEnableDiameterEnd;

	public readonly CSInputChannel InEnablePlacementMiddle;

	public readonly CSInputChannel InEnablePlacementEnd;

	public readonly CSInputChannel InDoFlip;

	public readonly CSInputChannel InDoMirror;

	public readonly CSInputChannel InDoReverseDirection;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InProfile2;

	public readonly CSInputChannel InChannelEnabled;

	public readonly CSInputChannel InMirrorMode;

	public readonly CSInputChannel InChannelClosed;

	public readonly CSInputChannel InUseMinCuletDistance;

	public readonly CSInputChannel InMinCuletDistance;

	public readonly CSInputChannel InMoveGems;

	public readonly CSInputChannel InOverrideStartPosition;

	public readonly CSInputChannel InOverrideEndPosition;

	public readonly CSInputChannel InOverrideStart;

	public readonly CSInputChannel InOverrideEnd;

	public readonly CSInputChannel InFront;

	public readonly CSInputChannel InShankWidthAdd;

	public readonly CSInputChannel InSideBuild;

	public readonly CSInputChannel InOverrideDiameterStart;

	public readonly CSInputChannel InOverrideDStartToggle;

	public readonly CSInputChannel InOverrideDiameterEnd;

	public readonly CSInputChannel InOverrideDEndToggle;

	public readonly CSInputChannel InShowGems;

	public readonly CSInputChannel InSpacingStart;

	public readonly CSInputChannel InSpacingMiddle;

	public readonly CSInputChannel InSpacingEnd;

	public readonly CSInputChannel InSpacingTolleranceStart;

	public readonly CSInputChannel InSpacingTolleranceMiddle;

	public readonly CSInputChannel InSpacingTolleranceEnd;

	public readonly CSInputChannel InWallWidth;

	public readonly CSInputChannel InChannelWallGemOverlap;

	public readonly CSInputChannel InChannelStartSlopeSpacing;

	public readonly CSInputChannel InChannelEndSlopeSpacing;

	public readonly CSInputChannel InEuroGemToggle;

	public readonly CSInputChannel InEuroGemAngle;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemCountToggle;

	public readonly CSInputChannel InMetalWidthToggle;

	public readonly CSInputChannel InProfileWidthStart;

	public readonly CSInputChannel InProfileWidthMiddle;

	public readonly CSInputChannel InProfileWidthEnd;

	public readonly CSInputChannel InProfileWidthMiddleToggle;

	public readonly CSInputChannel InProfileWidthEndToggle;

	public readonly CSInputChannel InForceGems;

	public readonly CSOutputChannel OutDisplayGemArray;

	public readonly CSOutputChannel OutGemArray;

	public readonly CSOutputChannel OutGems;

	public readonly CSOutputChannel OutChannelYStart;

	public readonly CSOutputChannel OutChannelYEnd;

	public readonly CSOutputChannel OutPositionStart;

	public readonly CSOutputChannel OutPositionMiddle;

	public readonly CSOutputChannel OutPositionEnd;

	public readonly CSOutputChannel OutChannelEnabled;

	public readonly CSOutputChannel OutChannelStartSlopeLength;

	public readonly CSOutputChannel OutChannelEndSlopeLength;

	public readonly CSOutputChannel OutGemsClosedLoop;

	public readonly CSOutputChannel OutDirection;

	public readonly CSOutputChannel OutGemCount;

	public readonly CSOutputChannel OutGem0SizeX;

	public readonly CSOutputChannel OutGem0SizeY;

	public readonly CSOutputChannel OutGem1SizeY;

	public readonly CSOutputChannel OutGem0Position;

	public readonly CSOutputChannel OutGem1Position;

	public readonly CSOutputChannel OutGemLast0SizeX;

	public readonly CSOutputChannel OutGemLast0SizeY;

	public readonly CSOutputChannel OutGemLast1SizeY;

	public readonly CSOutputChannel OutGemLast0Position;

	public readonly CSOutputChannel OutGemLast1Position;

	public readonly CSOutputChannel OutCurveLength;

	public readonly CSOutputChannel OutWallWidth;

	public readonly CSOutputChannel OutChannelWallGemOverlap;

	public readonly CSOutputChannel OutProfile;

	public readonly CSOutputChannel OutStartGemXSize;

	public readonly CSOutputChannel OutStartGemYSize;

	public readonly CSOutputChannel OutStartGemZSize;

	public readonly CSOutputChannel OutStartGemZMin;

	public readonly CSOutputChannel OutStartGemZMax;

	public readonly CSOutputChannel OutMiddleGemXSize;

	public readonly CSOutputChannel OutMiddleGemYSize;

	public readonly CSOutputChannel OutMiddleGemZSize;

	public readonly CSOutputChannel OutMiddleGemZMin;

	public readonly CSOutputChannel OutMiddleGemZMax;

	public readonly CSOutputChannel OutEndGemXSize;

	public readonly CSOutputChannel OutEndGemYSize;

	public readonly CSOutputChannel OutEndGemZSize;

	public readonly CSOutputChannel OutEndGemZMin;

	public readonly CSOutputChannel OutEndGemZMax;

	private Curve _baseCrv;

	private Brep _baseSrf;

	private bool _channelEnabled;

	private double _channelEndSlopeLength;

	private double _channelStartSlopeLength;

	private double _channelWallGemOverlap;

	private Curve _crv;

	private bool _crvIsClosed;

	private double _crvLength;

	private int _direction;

	private bool _doFlip;

	private bool _doMirror;

	private double _endGemSpacing;

	private double _endGemSpacingTolerance;

	private double _gemRotatedEndXSize;

	private double _gemRotatedMiddleXSize;

	private double _gemRotatedStartXSize;

	private bool _gemsClosedLoopSoNoChannelEnds;

	private CSGemData _gemShape;

	private bool _inFront;

	private double _middleGemSpacing;

	private double _middleGemSpacingTolerance;

	private bool _orientationFollowCurve;

	private double _placementEnd;

	private double _placementMiddle;

	private double _placementStart;

	private double _positionEndGem;

	private double _positionEndInput;

	private double _positionMiddle;

	private double _positionStartGem;

	private double _positionStartInput;

	private Curve _profile;

	private bool _sideBuilt;

	private double _startGemSpacing;

	private double _startGemSpacingTolerance;

	private double _vMinCuletDistance;

	private ValueListItem _vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;

	private ValueListItem _vPlacementMethod = ValueListItem.GemPlacementMethodGirdle;

	private ValueListItem _vSpacingMethod = ValueListItem.GemSpacingMethodMinimum;

	private bool _vUseMinCuletDistance;

	private double _wallWidth;

	private double _xEnd;

	private double _xMiddle;

	private Transform _xRoll;

	private double _xStart;

	private Transform _xTilt;

	private double _yEnd;

	private double _yMiddle;

	private double _yStart;

	public CSUltimateMatchingGem5(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.Surface, ChannelArrayType.Any);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InOverrideDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InOverrideDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InPositionStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPositionEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InPositionMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 50);
		InPlacementStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemOrientationMethodAxisY);
		InOrientationFollowCurve = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemPlacementMethodGirdle);
		InSpacingMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemSpacingMethodMinimum);
		InEnableDiameterMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnableDiameterEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoReverseDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirrorMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemOnCrvMirrorOddAndEven);
		InChannelClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseMinCuletDistance = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMinCuletDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InMoveGems = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.ShankEdge);
		InOverrideStartPosition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideEndPosition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.3);
		InOverrideEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.6);
		InFront = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InShankWidthAdd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSideBuild = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOverrideDStartToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideDEndToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InSpacingStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingTolleranceStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingTolleranceMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingTolleranceEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelStartSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelEndSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEuroGemToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEuroGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 135.0);
		InMetalWidthToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InProfileWidthMiddleToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InProfileWidthEndToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InForceGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5);
		InGemCountToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutDisplayGemArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGemArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGems = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutChannelYStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelYEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionMiddle = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEnabled = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutChannelStartSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEndSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemsClosedLoop = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutDirection = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutGemCount = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutGem0SizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem0SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem1SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem0Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem1Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0SizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast1SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast1Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCurveLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutWallWidth = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelWallGemOverlap = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEnabled = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutGemsClosedLoop = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutStartGemXSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemZSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemZMax = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemXSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemZSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemZMax = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemXSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemZSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemZMax = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		bool flag = InShowGems.Boolean;
		_crv = InCurve.Curve;
		_baseCrv = InBaseObject.Curve;
		_baseSrf = InBaseObject.Brep;
		bool boolean = InOverrideDStartToggle.Boolean;
		bool boolean2 = InOverrideDEndToggle.Boolean;
		double inputGemCount = InGemCount.Double;
		bool boolean3 = InGemCountToggle.Boolean;
		_gemShape = InGemShape.Gem;
		CSGemData gem = InDiameterStart.Gem;
		if (boolean)
		{
			gem = InOverrideDiameterStart.Gem;
		}
		CSGemData gem2 = InDiameterMiddle.Gem;
		CSGemData gem3 = InDiameterEnd.Gem;
		if (boolean2)
		{
			gem3 = InOverrideDiameterEnd.Gem;
		}
		if (!InOverrideStartPosition.Boolean)
		{
			_positionStartInput = InPositionStart.Double;
		}
		else
		{
			_positionStartInput = InOverrideStart.Double;
		}
		if (!InOverrideEndPosition.Boolean)
		{
			_positionEndInput = InPositionEnd.Double;
		}
		else
		{
			_positionEndInput = InOverrideEnd.Double;
		}
		_positionMiddle = InPositionMiddle.Percentage;
		_placementStart = InPlacementStart.Double;
		_placementMiddle = InPlacementMiddle.Double;
		_placementEnd = InPlacementEnd.Double;
		bool boolean4 = InEuroGemToggle.Boolean;
		double degrees = InEuroGemAngle.Double;
		if (boolean4)
		{
			double length = new Vector3d(_crv.PointAtStart - _crv.PointAtNormalizedLength(0.5)).Length;
			Surface surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, -1.0, 0.0), new Point3d(0.0, -1.0, length), new Point3d(0.0, 1.0, length), new Point3d(0.0, 1.0, 0.0));
			surface.Transform(Transform.Rotation(RhinoMath.ToRadians(degrees), Vector3d.YAxis, Point3d.Origin));
			CurveIntersections curveIntersections = Intersection.CurveSurface(_crv, surface, 0.01, 0.01);
			_crv.ClosestPoint(curveIntersections[0].PointA, out var t);
			_crv.ClosestPoint(_crv.PointAtStart, out var t2);
			double length2 = _crv.GetLength();
			double length3 = _crv.GetLength(new Interval(t2, t));
			if (_positionEndInput > length3 / length2)
			{
				_positionEndInput = length3 / length2;
			}
		}
		_startGemSpacing = InSpacingStart.Double;
		_startGemSpacingTolerance = InSpacingTolleranceStart.Double;
		_middleGemSpacing = InSpacingMiddle.Double;
		_middleGemSpacingTolerance = InSpacingTolleranceMiddle.Double;
		_endGemSpacing = InSpacingEnd.Double;
		_endGemSpacingTolerance = InSpacingTolleranceEnd.Double;
		double angleDegrees = InRoll.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InTilt.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		_vOrientationMethod = InOrientationMethod.ValueListItem;
		_vPlacementMethod = InPlacementMethod.ValueListItem;
		_vSpacingMethod = InSpacingMethod.ValueListItem;
		if (boolean3)
		{
			_vSpacingMethod = ValueListItem.GemSpacingMethodFixed;
		}
		_orientationFollowCurve = InOrientationFollowCurve.Boolean;
		bool boolean5 = InEnableDiameterMiddle.Boolean;
		bool boolean6 = InEnableDiameterEnd.Boolean;
		bool boolean7 = InEnablePlacementMiddle.Boolean;
		bool boolean8 = InEnablePlacementEnd.Boolean;
		ValueListItem valueListItem = InMoveGems.ValueListItem;
		_doFlip = InDoFlip.Boolean;
		_doMirror = InDoMirror.Boolean;
		ValueListItem valueListItem2 = InMirrorMode.ValueListItem;
		bool boolean9 = InDoReverseDirection.Boolean;
		_vUseMinCuletDistance = InUseMinCuletDistance.Boolean;
		_vMinCuletDistance = InMinCuletDistance.Double;
		if (flag)
		{
			_profile = InProfile2.Curve;
			OutProfile.SetData(InProfile2.Data);
		}
		else
		{
			_profile = InProfile.Curve;
			OutProfile.SetData(InProfile.Data);
		}
		_channelEnabled = InChannelEnabled.Boolean;
		_wallWidth = InWallWidth.Double;
		_channelWallGemOverlap = InChannelWallGemOverlap.Double;
		_channelStartSlopeLength = InChannelStartSlopeSpacing.Double;
		_channelEndSlopeLength = InChannelEndSlopeSpacing.Double;
		_gemsClosedLoopSoNoChannelEnds = InChannelClosed.Boolean;
		_inFront = InFront.Boolean;
		double num = InShankWidthAdd.Double;
		_sideBuilt = InSideBuild.Boolean;
		if (_sideBuilt && _channelEnabled)
		{
			_crv.Transform(Transform.Translation(0.0, _inFront ? (0.0 - (num / 2.0 + _wallWidth)) : (num / 2.0 + _wallWidth), 0.0));
			_baseCrv.Transform(Transform.Translation(0.0, _inFront ? (0.0 - (num / 2.0 + _wallWidth)) : (num / 2.0 + _wallWidth), 0.0));
		}
		bool boolean10 = InMetalWidthToggle.Boolean;
		double profileWidthStart = InProfileWidthStart.Double;
		double profileWidthMiddle = InProfileWidthMiddle.Double;
		bool boolean11 = InProfileWidthMiddleToggle.Boolean;
		double profileWidthEnd = InProfileWidthEnd.Double;
		bool boolean12 = InProfileWidthEndToggle.Boolean;
		if (InForceGems.Boolean)
		{
			flag = true;
		}
		UltGem(gem, gem2, gem3, boolean6, boolean5, boolean8, boolean7, boolean9, valueListItem, valueListItem2, angleDegrees, angleDegrees2, boolean3, inputGemCount, boolean10, profileWidthStart, boolean11, profileWidthMiddle, boolean12, profileWidthEnd, out var outputGems, out var gemList);
		double length4 = _crv.GetLength();
		int count = gemList.Count;
		ShankMath(gemList, count, out var gem0SizeX, out var gem0SizeY, out var gem1SizeY, out var gem0Position, out var gem1Position, out var gemLast0SizeX, out var gemLast0SizeY, out var gemLast1SizeY, out var gemLast0Position, out var gemLast1Position);
		if (flag)
		{
			OutDisplayGemArray.SetData(outputGems);
		}
		OutGemArray.SetData(outputGems);
		CSDataArray cSDataArray = new CSDataArray();
		foreach (UltimateGemData item in gemList)
		{
			cSDataArray.Append(item.AddToArray());
		}
		OutGems.SetData(cSDataArray);
		OutChannelYStart.SetData(new CSNumberData(_yStart));
		OutChannelYEnd.SetData(new CSNumberData(_yEnd));
		OutPositionStart.SetData(new CSNumberData(_positionStartInput));
		OutPositionMiddle.SetData(new CSNumberData(_positionMiddle));
		OutPositionEnd.SetData(new CSNumberData(_positionEndInput));
		OutChannelEnabled.SetData(new CSNumberData(_channelEnabled ? 1 : 0));
		OutChannelStartSlopeLength.SetData(new CSNumberData(_channelStartSlopeLength));
		OutChannelEndSlopeLength.SetData(new CSNumberData(_channelEndSlopeLength));
		OutGemsClosedLoop.SetData(new CSNumberData(_gemsClosedLoopSoNoChannelEnds ? 1 : 0));
		OutDirection.SetData(new CSNumberData(_direction));
		OutGemCount.SetData(new CSNumberData(count));
		OutGem0SizeX.SetData(new CSNumberData(gem0SizeX));
		OutGem0SizeY.SetData(new CSNumberData(gem0SizeY));
		OutGem1SizeY.SetData(new CSNumberData(gem1SizeY));
		OutGem0Position.SetData(new CSNumberData(gem0Position));
		OutGem1Position.SetData(new CSNumberData(gem1Position));
		OutGemLast0SizeX.SetData(new CSNumberData(gemLast0SizeX));
		OutGemLast0SizeY.SetData(new CSNumberData(gemLast0SizeY));
		OutGemLast1SizeY.SetData(new CSNumberData(gemLast1SizeY));
		OutGemLast0Position.SetData(new CSNumberData(gemLast0Position));
		OutGemLast1Position.SetData(new CSNumberData(gemLast1Position));
		OutCurveLength.SetData(new CSNumberData(length4));
		OutChannelWallGemOverlap.SetData(InChannelWallGemOverlap.Data);
		OutWallWidth.SetData(InWallWidth.Data);
		if (count > 0)
		{
			OutStartGemXSize.SetData(new CSNumberData(gemList[0].SizeX));
			OutStartGemYSize.SetData(new CSNumberData(gemList[0].SizeY));
			OutStartGemZSize.SetData(new CSNumberData(gemList[0].SizeZ));
			OutStartGemZMax.SetData(new CSNumberData(gemList[0].GemZMax));
			OutStartGemZMin.SetData(new CSNumberData(gemList[0].GemZMin));
			OutMiddleGemXSize.SetData(new CSNumberData(gemList[count / 2].SizeX));
			OutMiddleGemYSize.SetData(new CSNumberData(gemList[count / 2].SizeY));
			OutMiddleGemZSize.SetData(new CSNumberData(gemList[count / 2].SizeZ));
			OutMiddleGemZMax.SetData(new CSNumberData(gemList[count / 2].GemZMax));
			OutMiddleGemZMin.SetData(new CSNumberData(gemList[count / 2].GemZMin));
			OutEndGemXSize.SetData(new CSNumberData(gemList[count - 1].SizeX));
			OutEndGemYSize.SetData(new CSNumberData(gemList[count - 1].SizeY));
			OutEndGemZSize.SetData(new CSNumberData(gemList[count - 1].SizeZ));
			OutEndGemZMax.SetData(new CSNumberData(gemList[count - 1].GemZMax));
			OutEndGemZMin.SetData(new CSNumberData(gemList[count - 1].GemZMin));
		}
		return base.Update();
	}

	private void AdjustEndErrorSoGemSpacingLessGemTollerance(List<UltimateGemData> gemList, double vLen, int gemIndex, double profileWidth, ref double endError)
	{
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(ultimateGemData, profileWidth);
			Point3d origin = Point3d.Origin;
			origin.Transform(ultimateGemData.GemTransform);
			MoveAGem(ultimateGemData, _positionStartGem, endError, vLen, profileWidth);
			Point3d origin2 = Point3d.Origin;
			origin2.Transform(ultimateGemData.GemTransform);
			double length = (origin2 - origin).Length;
			int num = gemList.Count - 1;
			if (num == 0)
			{
				num = 1;
			}
			length /= (double)num;
			double num2 = 0.0;
			if (gemIndex == 0)
			{
				num2 = _startGemSpacingTolerance;
			}
			if (gemIndex > 0 && (double)gemIndex <= (double)(gemList.Count - 1) / 2.0)
			{
				num2 = _startGemSpacingTolerance * (1.0 - (double)gemIndex / ((double)gemList.Count - 1.0) / 2.0) + _middleGemSpacingTolerance * ((double)gemIndex / ((double)gemList.Count - 1.0) / 2.0);
			}
			if ((double)gemIndex > (double)(gemList.Count - 1) / 2.0 && gemIndex < gemList.Count - 1)
			{
				num2 = _middleGemSpacingTolerance * ((double)gemIndex / (double)(gemList.Count - 1) / (double)(gemList.Count - 1) / 2.0) + _endGemSpacingTolerance * (double)(1 - gemIndex / (gemList.Count - 1) / (gemList.Count - 1) / 2);
			}
			if (gemIndex == gemList.Count - 1)
			{
				num2 = _endGemSpacingTolerance;
			}
			if (length > num2)
			{
				endError *= num2 / length;
			}
		}
	}

	private void AdjustGemPosEnd(double profileWidth, bool gemCountToggle, double maxCount, bool metalWidthToggle, double profileWidthStart, double profileWidthMiddle, double profileWidthEnd, ref List<UltimateGemData> gemList)
	{
		if (gemList.Count <= 1)
		{
			return;
		}
		double paramPosition = _positionEndGem;
		if (_crvIsClosed)
		{
			_direction = -_direction;
			CalcLastLoopingGemPos(gemList[0], gemList[gemList.Count - 1], _positionEndGem, profileWidth, out paramPosition);
			if (Math.Abs(_positionEndGem - paramPosition) > 0.001)
			{
				double num = (paramPosition - gemList[gemList.Count - 1].NormParam) / (double)(gemList.Count - 1);
				paramPosition -= num;
			}
			_direction = -_direction;
		}
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = paramPosition - normParam;
		double num2 = normParam - _positionStartGem;
		for (int i = 0; i < gemList.Count; i++)
		{
			AdjustEndErrorSoGemSpacingLessGemTollerance(gemList, num2, i, profileWidth, ref endError);
			CalcGemSize(gemList[i].NormParam, gemCountToggle, i, maxCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
			MoveAGem(gemList[i], _positionStartGem, endError, num2, profileWidth);
		}
	}

	private void AdjustGemPosEndMirror(double profileWidth, bool gemCountToggle, double gemCount, bool metalWidthToggle, double profileWidthStart, double profileWidthMiddle, double profileWidthEnd, ref List<UltimateGemData> gemList, ref List<UltimateGemData> gemListMirror)
	{
		if (gemList.Count <= 1 && gemListMirror.Count <= 0)
		{
			return;
		}
		double positionEndGem = _positionEndGem;
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = positionEndGem - normParam;
		double num = normParam - _positionStartGem;
		AdjustEndErrorSoGemSpacingLessGemTollerance(gemList, num, gemList.Count - 1, profileWidth, ref endError);
		double endError2 = endError;
		double gemRowLength = num;
		if (gemList.Count > 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			UltimateGemData ultimateGemData2 = gemListMirror[gemListMirror.Count - 1].Duplicate();
			MoveAGem(ultimateGemData, _positionStartGem, endError, num, profileWidth);
			MoveAGem(ultimateGemData2, _positionStartGem, endError2, gemRowLength, profileWidth);
			Point3d gemPoint = ultimateGemData.GemPoint;
			Point3d gemPoint2 = ultimateGemData2.GemPoint;
			if ((gemPoint - gemPoint2).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - ultimateGemData.Spacing * 0.8 < 0.0)
			{
				endError = _positionStartGem + 0.5 * (double)_direction - normParam;
				num = normParam - _positionStartGem;
				endError2 = endError;
				gemRowLength = num;
				gemList.RemoveAt(gemList.Count - 1);
			}
		}
		for (int i = 0; i < gemList.Count; i++)
		{
			CalcGemSize(gemList[i].NormParam, gemCountToggle, i, gemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
			MoveAGem(gemList[i], _positionStartGem, endError, num, profileWidth);
		}
		for (int j = 0; j < gemListMirror.Count; j++)
		{
			CalcGemSize(gemListMirror[j].NormParam, gemCountToggle, j, gemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
			MoveAGem(gemListMirror[j], _positionStartGem, endError2, gemRowLength, profileWidth);
		}
	}

	private void CalcGemPos(double paramPosition, bool toggleGemCount, double gemCount, out List<UltimateGemData> gemList, out List<UltimateGemData> gemListMirror, bool doMirror, bool mirrorModeEven, bool metalWidthToggle, double profileWidthStart, double profileWidthMiddle, double profileWidthEnd, out double profileWidth)
	{
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		profileWidth = profileWidthStart;
		Point3d point3d = Point3d.Unset;
		Point3d point3d2 = Point3d.Unset;
		double num = 0.0;
		double length = _crv.GetLength();
		double num2 = paramPosition;
		double num3 = paramPosition;
		bool flag = false;
		int num4 = 0;
		while (true)
		{
			bool num5;
			if (toggleGemCount)
			{
				num5 = gemCount > (double)num4;
			}
			else
			{
				if (!(paramPosition * (double)_direction < _positionEndGem * (double)_direction))
				{
					break;
				}
				num5 = !flag;
			}
			if (!num5)
			{
				break;
			}
			double num6 = 999.0;
			int num7 = 20;
			UltimateGemData ultimateGemData = null;
			UltimateGemData ultimateGemData2 = null;
			while (num6 > 0.01 && num7 > 0)
			{
				num7--;
				ultimateGemData = CalcGemSize(paramPosition, toggleGemCount, num4, gemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
				CalculateGemXform(ultimateGemData, profileWidth);
				if (gemList.Count == 0)
				{
					if (!(doMirror && mirrorModeEven))
					{
						num6 = 0.0;
						continue;
					}
					ultimateGemData2 = new UltimateGemData
					{
						NormParam = num3 - (paramPosition - num3),
						SizeX = ultimateGemData.SizeX,
						SizeY = ultimateGemData.SizeY,
						SizeZ = ultimateGemData.SizeZ,
						ZPlacement = ultimateGemData.ZPlacement,
						Spacing = ultimateGemData.Spacing,
						SizeXRotated = ultimateGemData.SizeXRotated
					};
					CalculateGemXform(ultimateGemData2, profileWidth);
					point3d = ultimateGemData2.GemPoint;
					point3d2 = point3d;
					num = ultimateGemData.SizeXRotated;
				}
				double length2 = (ultimateGemData.GemPoint - point3d).Length;
				length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - num / 2.0;
				num6 = Math.Abs(length2 - ultimateGemData.Spacing);
				if (!(num6 > 0.01))
				{
					continue;
				}
				double num8 = (ultimateGemData.Spacing - length2) * (double)_direction;
				if (doMirror && mirrorModeEven && gemList.Count == 0)
				{
					num8 /= 2.0;
				}
				paramPosition += num8 / length * 0.8;
				if (!_crv.IsClosed)
				{
					if (paramPosition > 1.0)
					{
						paramPosition = 1.0;
					}
					if (paramPosition < 0.0)
					{
						paramPosition = 0.0;
					}
				}
			}
			if (ultimateGemData != null)
			{
				gemList.Add(ultimateGemData);
				num4++;
				point3d = ultimateGemData.GemPoint;
			}
			if (doMirror)
			{
				num6 = 999.0;
				num7 = 20;
				if (point3d2 == Point3d.Unset)
				{
					point3d2 = point3d;
				}
				if (gemList.Count <= 1)
				{
					num6 = 0.0;
				}
				if (_crv.IsClosed || (num2 < 1.01 && num2 > -0.01))
				{
					if (!_crv.IsClosed)
					{
						if (num2 > 1.0)
						{
							num2 = 1.0;
						}
						if (num2 < 0.0)
						{
							num2 = 0.0;
						}
					}
					while (num6 > 0.01 && num7 > 0)
					{
						num7--;
						if (ultimateGemData != null)
						{
							ultimateGemData2 = new UltimateGemData
							{
								NormParam = num2,
								SizeX = ultimateGemData.SizeX,
								SizeY = ultimateGemData.SizeY,
								SizeZ = ultimateGemData.SizeZ,
								ZPlacement = ultimateGemData.ZPlacement,
								Spacing = ultimateGemData.Spacing,
								SizeXRotated = ultimateGemData.SizeXRotated
							};
						}
						CalculateGemXform(ultimateGemData2, profileWidth);
						if (ultimateGemData2 != null)
						{
							double length3 = (ultimateGemData2.GemPoint - point3d2).Length;
							length3 = length3 - ultimateGemData2.SizeXRotated / 2.0 - num / 2.0;
							num6 = Math.Abs(length3 - ultimateGemData2.Spacing);
							if (!(num6 > 0.01))
							{
								continue;
							}
							double num9 = (ultimateGemData2.Spacing - length3) * (double)_direction;
							num2 -= num9 / length * 0.8;
						}
						if (!_crv.IsClosed)
						{
							if (num2 > 1.0)
							{
								num2 = 1.0;
							}
							if (num2 < 0.0)
							{
								num2 = 0.0;
							}
						}
					}
					if (ultimateGemData2 != null)
					{
						gemListMirror.Add(ultimateGemData2);
						point3d2 = ultimateGemData2.GemPoint;
					}
				}
			}
			if (ultimateGemData != null)
			{
				num = ultimateGemData.SizeXRotated;
				double num10 = num + ultimateGemData.Spacing;
				paramPosition += num10 / length * (double)_direction;
				num2 -= num10 / length * (double)_direction;
			}
			if (!doMirror && _crvIsClosed && gemList.Count >= 3)
			{
				Point3d gemPoint = gemList[0].GemPoint;
				point3d = gemList[gemList.Count - 1].GemPoint;
				if ((gemPoint - point3d).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - gemList[gemList.Count - 1].Spacing < 0.0)
				{
					gemList.RemoveAt(gemList.Count - 1);
					flag = true;
					num4 = (int)gemCount;
				}
			}
		}
	}

	private UltimateGemData CalcGemSize(double paramPosition, bool gemCountToggle, double count, double maxCount, bool metalWidthToggle, double profileWidthStart, double profileWidthMiddle, double profileWidthEnd, out double profileWidth)
	{
		UltimateGemData ultimateGemData = new UltimateGemData
		{
			NormParam = paramPosition
		};
		double num = (paramPosition - _positionStartGem) / (_positionEndGem - _positionStartGem);
		if (gemCountToggle)
		{
			num = count / (maxCount - 1.0);
		}
		double zPlacement;
		double num2;
		double num3;
		double num4;
		double spacing;
		double num5;
		if (gemCountToggle ? (count < (maxCount - 1.0) / 2.0) : (num <= _positionMiddle))
		{
			num /= _positionMiddle;
			zPlacement = _placementStart * (1.0 - num) + _placementMiddle * num;
			num2 = _xStart * (1.0 - num) + _xMiddle * num;
			num3 = _yStart * (1.0 - num) + _yMiddle * num;
			num4 = _gemRotatedStartXSize * (1.0 - num) + _gemRotatedMiddleXSize * num;
			spacing = _startGemSpacing * (1.0 - num) + _middleGemSpacing * num;
			num5 = profileWidthStart * (1.0 - num) + profileWidthMiddle * num;
		}
		else
		{
			num = (num - _positionMiddle) / (1.0 - _positionMiddle);
			zPlacement = _placementMiddle * (1.0 - num) + _placementEnd * num;
			num2 = _xMiddle * (1.0 - num) + _xEnd * num;
			num3 = _yMiddle * (1.0 - num) + _yEnd * num;
			num4 = _gemRotatedMiddleXSize * (1.0 - num) + _gemRotatedEndXSize * num;
			spacing = _middleGemSpacing * (1.0 - num) + _endGemSpacing * num;
			num5 = profileWidthMiddle * (1.0 - num) + profileWidthEnd * num;
		}
		if (metalWidthToggle)
		{
			profileWidth = num5;
		}
		else
		{
			profileWidth = num3 + _wallWidth * 2.0;
		}
		CSUtilGem.GetGemScaleFromTable(_gemShape, num2, num3, true, false, out Transform xBestScale);
		if (_gemShape.Duplicate() is CSGemData cSGemData)
		{
			cSGemData.ApplyTransform(xBestScale);
			double num6 = (cSGemData.GemXSize + cSGemData.GemYSize) / (num2 + num3);
			ultimateGemData.SizeX = cSGemData.GemXSize;
			ultimateGemData.SizeY = cSGemData.GemYSize;
			ultimateGemData.SizeZ = cSGemData.GemZSize;
			ultimateGemData.SizeXRotated = num4 * num6;
			ultimateGemData.ZPlacement = zPlacement;
			ultimateGemData.Spacing = spacing;
		}
		return ultimateGemData;
	}

	private void CalcLastLoopingGemPos(UltimateGemData firstGem, UltimateGemData lastGem, double neededGemPos, double profileWidth, out double paramPosition)
	{
		Point3d origin = Point3d.Origin;
		origin.Transform(firstGem.GemTransform);
		double length = _crv.GetLength();
		double num = 999.0;
		int num2 = 20;
		paramPosition = neededGemPos;
		bool flag = true;
		while (num > 0.01 && num2 > 0)
		{
			num2--;
			UltimateGemData ultimateGemData = new UltimateGemData
			{
				NormParam = paramPosition,
				SizeX = lastGem.SizeX,
				SizeY = lastGem.SizeY,
				SizeZ = lastGem.SizeZ,
				ZPlacement = lastGem.ZPlacement,
				Spacing = lastGem.Spacing,
				SizeXRotated = lastGem.SizeXRotated
			};
			CalculateGemXform(ultimateGemData, profileWidth);
			double length2 = (ultimateGemData.GemPoint - origin).Length;
			length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - firstGem.SizeXRotated / 2.0;
			if (flag)
			{
				if (length2 - ultimateGemData.Spacing > 0.0)
				{
					paramPosition = neededGemPos;
					break;
				}
				flag = false;
				paramPosition = lastGem.NormParam;
			}
			else
			{
				num = Math.Abs(length2 - ultimateGemData.Spacing);
				if (num > 0.01)
				{
					double num3 = (ultimateGemData.Spacing - length2) * (double)_direction;
					paramPosition += num3 / length * 0.8;
				}
			}
		}
	}

	private void CalculateGemXform(UltimateGemData gemOnCurveInfo, double profileWidth)
	{
		double num = gemOnCurveInfo.SizeX / _gemShape.GemXSize;
		Transform transform;
		if (gemOnCurveInfo.SizeY > 0.0)
		{
			double yScaleFactor = gemOnCurveInfo.SizeY / _gemShape.GemYSize;
			transform = Transform.Scale(Plane.WorldXY, num, yScaleFactor, num);
		}
		else
		{
			transform = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		CSGemData cSGemData = new CSGemData(_gemShape);
		cSGemData.ApplyTransform(transform);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, _vPlacementMethod, ref zOffset);
		CalculatePointsOnCrv(_crv, _baseCrv, _vOrientationMethod, gemOnCurveInfo);
		Point3d point3d = Point3d.Unset;
		if (_baseCrv != null)
		{
			point3d = _baseCrv.PointAt(gemOnCurveInfo.BaseParam);
			if (_sideBuilt)
			{
				point3d.Y = (_inFront ? (point3d.Y - gemOnCurveInfo.SizeY / 2.0) : (point3d.Y + gemOnCurveInfo.SizeY / 2.0));
			}
		}
		Point3d point3d2 = _crv.PointAt(gemOnCurveInfo.Param);
		if (_sideBuilt)
		{
			point3d2.Y = (_inFront ? (point3d2.Y - gemOnCurveInfo.SizeY / 2.0) : (point3d2.Y + gemOnCurveInfo.SizeY / 2.0));
		}
		Vector3d vector3d = _crv.TangentAt(gemOnCurveInfo.Param);
		double num2 = 0.0;
		if (_channelEnabled && _profile != null)
		{
			double num3 = ProfileCut(_channelWallGemOverlap, _profile, profileWidth, gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, _crv, _baseCrv, _sideBuilt, _inFront, out var pScale, out var pRail);
			num2 = (point3d2 - point3d).Length * (1.0 - num3);
			gemOnCurveInfo.ProfileCurve.Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Vector3d vector3d2 = (_doFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d vector3d3 = (_doFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal;
		switch (_vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(_baseSrf, point3d2, ref brepNormal);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			brepNormal = new Vector3d(point3d2);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			brepNormal = new Vector3d(point3d2.X, 0.0, point3d2.Z);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			break;
		case ValueListItem.OrientationMethodClosestPointOnCurve:
		case ValueListItem.OrientationMethodIntersectPointOnCurve:
			brepNormal = ((point3d != Point3d.Unset) ? (point3d2 - point3d) : new Vector3d(point3d2));
			brepNormal.Unitize();
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			break;
		}
		brepNormal.Unitize();
		Transform transform2 = (_orientationFollowCurve ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d3, vector3d2, point3d2, vector3d, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d3, point3d2, brepNormal, vector3d));
		double num4 = 0.0;
		if (_vUseMinCuletDistance)
		{
			double length = (point3d2 - point3d).Length;
			if (cSGemData.GemZSize > length - _vMinCuletDistance - num2)
			{
				num4 = (gemOnCurveInfo.MinCuletMovement = cSGemData.GemZSize - (length - _vMinCuletDistance - num2));
			}
		}
		double num5 = zOffset + gemOnCurveInfo.ZPlacement - num2 + num4;
		Transform transform3 = Transform.Translation(brepNormal * num5);
		gemOnCurveInfo.GemTransform = transform3 * transform2 * _xRoll * _xTilt * transform;
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem vOrientationMethod, UltimateGemData gem)
	{
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		crv.NormalizedLengthParameter(num, out gem.Param);
		Point3d point3d = crv.PointAt(gem.Param);
		Vector3d normal = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (vOrientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
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

	private static bool CalculateRotateTestGem(CSGemData gemShape, double xSize, double ySize, double rollAngle, out double xSizeOut)
	{
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, true, false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(gemShape);
		cSGemData.ApplyTransform(transform * xBestScale);
		if (!(cSGemData.GetCurve() is NurbsCurve nurbsCurve))
		{
			xSizeOut = 0.0;
			return false;
		}
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint value = nurbsCurve.Points[i];
			value.Location = new Point3d(value.Location.X, value.Location.Y, 0.0);
			nurbsCurve.Points[i] = value;
		}
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		double y = boundingBox.Min.Y;
		double num = boundingBox.Max.Y - boundingBox.Min.Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), nurbsCurve, 0.001, 0.001);
			if (curveIntersections.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in curveIntersections)
			{
				Point3d pointA = item.PointA;
				if (pointA.X > num4)
				{
					num4 = pointA.X;
				}
				if (pointA.X < num3)
				{
					num3 = pointA.X;
				}
			}
			if (num4 - num3 > num2)
			{
				num2 = num4 - num3;
			}
		}
		xSizeOut = num2;
		return true;
	}

	private void MoveAGem(UltimateGemData gemPos, double positionStartGem, double endError, double gemRowLength, double profileWidth)
	{
		double num = (gemPos.NormParam - positionStartGem) * endError / gemRowLength;
		gemPos.NormParam += num;
		CalculateGemXform(gemPos, profileWidth);
	}

	private static double ProfileCut(double channelWallGemOverlap, Curve profile, double profileWidth, UltimateGemData gemOnCurveInfo)
	{
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve curveB = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve curveB2 = new LineCurve(new Point3d(sizeY / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform xform = Transform.Scale(Plane.WorldXY, profileWidth, 1.0, 1.0);
		gemOnCurveInfo.ProfileCurve.Transform(xform);
		CurveIntersections curveIntersections = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, curveB, 0.001, 0.001);
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, curveB2, 0.001, 0.001);
		double num = -1.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			Point3d pointA = item.PointA;
			if (pointA.Y > num)
			{
				num = pointA.Y;
				gemOnCurveInfo.ParamLeft = item.ParameterA;
			}
		}
		double num2 = -1.0;
		foreach (IntersectionEvent item2 in curveIntersections2)
		{
			Point3d pointA2 = item2.PointA;
			if (pointA2.Y > num2)
			{
				num2 = pointA2.Y;
				gemOnCurveInfo.ParamRight = item2.ParameterA;
			}
		}
		return (num + num2) / 2.0;
	}

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, bool sideBuilt, bool inFront, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = crv.PointAt(gem.Param);
		if (sideBuilt)
		{
			point3d.Y = (inFront ? (point3d.Y - gem.SizeY / 2.0) : (point3d.Y + gem.SizeY / 2.0));
		}
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d point3d2 = baseCrv.PointAt(gem.BaseParam);
		if (sideBuilt)
		{
			point3d2.Y = (inFront ? (point3d2.Y - gem.SizeY / 2.0) : (point3d2.Y + gem.SizeY / 2.0));
		}
		Vector3d destination2 = point3d - point3d2;
		double length = destination2.Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d, destination2, destination);
	}

	private void ShankMath(List<UltimateGemData> gemList, int gemCount, out double gem0SizeX, out double gem0SizeY, out double gem1SizeY, out double gem0Position, out double gem1Position, out double gemLast0SizeX, out double gemLast0SizeY, out double gemLast1SizeY, out double gemLast0Position, out double gemLast1Position)
	{
		gem0SizeX = 0.0;
		gem0SizeY = 0.0;
		gem1SizeY = 0.0;
		gem0Position = 0.0;
		gem1Position = 0.0;
		gemLast0SizeX = 0.0;
		gemLast0SizeY = 0.0;
		gemLast1SizeY = 0.0;
		gemLast0Position = 0.0;
		gemLast1Position = 0.0;
		if (gemCount > 0)
		{
			UltimateGemData ultimateGemData = gemList[0];
			gem0SizeX = ultimateGemData.SizeX;
			gem0SizeY = ultimateGemData.SizeY;
			gem0Position = ultimateGemData.NormParam;
			ultimateGemData = gemList[gemCount - 1];
			gemLast0SizeX = ultimateGemData.SizeX;
			gemLast0SizeY = ultimateGemData.SizeY;
			gemLast0Position = ultimateGemData.NormParam;
		}
		if (gemCount > 1)
		{
			UltimateGemData ultimateGemData2 = gemList[1];
			gem1SizeY = ultimateGemData2.SizeY;
			gem1Position = ultimateGemData2.NormParam;
			ultimateGemData2 = gemList[gemCount - 2];
			gemLast1SizeY = ultimateGemData2.SizeY;
			gemLast1Position = ultimateGemData2.NormParam;
		}
	}

	private void UltGem(CSGemData gemDiameterStart, CSGemData gemDiameterMiddle, CSGemData gemDiameterEnd, bool vEnableDiameterEnd, bool vEnableDiameterMiddle, bool vEnablePlacementEnd, bool vEnablePlacementMiddle, bool vDoReverseDirection, ValueListItem moveGems, ValueListItem vMirrorMode, double rollAngle, double tiltAngle, bool toggleGemCount, double inputGemCount, bool metalWidthToggle, double profileWidthStart, bool profileWidthMiddleToggle, double profileWidthMiddle, bool profileWidthEndToggle, double profileWidthEnd, out CSDataArray outputGems, out List<UltimateGemData> gemList)
	{
		outputGems = new CSDataArray();
		if (_profile != null)
		{
			BoundingBox boundingBox = _profile.GetBoundingBox(accurate: true);
			double x = boundingBox.Diagonal.X;
			double y = boundingBox.Diagonal.Y;
			Transform xform = Transform.Translation((0.0 - (boundingBox.Max.X + boundingBox.Min.X)) / 2.0, 0.0 - boundingBox.Min.Y, 0.0);
			_profile.Transform(xform);
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			_profile.Transform(xform2);
		}
		if (_baseSrf == null && _vOrientationMethod == ValueListItem.GemOrientationMethodSurface)
		{
			_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		}
		if (_baseCrv == null && (_vOrientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve || _vOrientationMethod == ValueListItem.OrientationMethodIntersectPointOnCurve))
		{
			_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		}
		if (metalWidthToggle)
		{
			GemSize gemSize = null;
			GemSize gemSize2 = null;
			GemSize gemSize3 = null;
			double num = -1.23432101234321E+308;
			double num2 = -1.23432101234321E+308;
			double num3 = -1.23432101234321E+308;
			double num4 = profileWidthStart - _wallWidth * 2.0;
			double num5 = profileWidthMiddle - _wallWidth * 2.0;
			double num6 = profileWidthEnd - _wallWidth * 2.0;
			foreach (GemSize size in gemDiameterStart.Sizes)
			{
				double num7 = size.X / gemDiameterStart.GemXSize;
				double num8 = size.Y / gemDiameterStart.GemYSize;
				double zScaleFactor = (num7 + num8) / 2.0;
				Transform xform3 = Transform.Scale(Plane.WorldXY, num7, num8, zScaleFactor);
				NurbsCurve nurbsCurve = gemDiameterStart.GetNurbsCurve();
				nurbsCurve.Transform(xform3);
				BoundingBox boundingBox2 = nurbsCurve.GetBoundingBox(accurate: true);
				double num9 = Math.Abs(boundingBox2.Max.Y) + Math.Abs(boundingBox2.Min.Y);
				double num10 = Math.Abs(num4 - num9);
				if (!RhinoMath.IsValidDouble(num) || num10 < num)
				{
					gemSize = size;
					num = num10;
				}
				double num11 = size.X / gemDiameterMiddle.GemXSize;
				double num12 = size.Y / gemDiameterMiddle.GemYSize;
				double zScaleFactor2 = (num11 + num12) / 2.0;
				Transform xform4 = Transform.Scale(Plane.WorldXY, num11, num12, zScaleFactor2);
				NurbsCurve nurbsCurve2 = gemDiameterMiddle.GetNurbsCurve();
				nurbsCurve2.Transform(xform4);
				BoundingBox boundingBox3 = default(BoundingBox);
				boundingBox3 = nurbsCurve2.GetBoundingBox(accurate: true);
				double num13 = Math.Abs(boundingBox3.Max.Y) + Math.Abs(boundingBox3.Min.Y);
				double num14 = Math.Abs(num5 - num13);
				if (!RhinoMath.IsValidDouble(num2) || num14 < num2)
				{
					gemSize2 = size;
					num2 = num14;
				}
				double num15 = size.X / gemDiameterEnd.GemXSize;
				double num16 = size.Y / gemDiameterEnd.GemYSize;
				double zScaleFactor3 = (num15 + num16) / 2.0;
				Transform xform5 = Transform.Scale(Plane.WorldXY, num15, num16, zScaleFactor3);
				NurbsCurve nurbsCurve3 = gemDiameterEnd.GetNurbsCurve();
				nurbsCurve3.Transform(xform5);
				BoundingBox boundingBox4 = nurbsCurve3.GetBoundingBox(accurate: true);
				double num17 = Math.Abs(boundingBox4.Max.Y) + Math.Abs(boundingBox4.Min.Y);
				double num18 = Math.Abs(num6 - num17);
				if (!RhinoMath.IsValidDouble(num3) || num18 < num3)
				{
					gemSize3 = size;
					num3 = num18;
				}
			}
			if (gemSize != null)
			{
				gemDiameterStart.ScaleToSize(gemSize.X, gemSize.Y);
			}
			if (gemSize2 != null)
			{
				gemDiameterMiddle.ScaleToSize(gemSize2.X, gemSize2.Y);
			}
			if (gemSize3 != null)
			{
				gemDiameterEnd.ScaleToSize(gemSize3.X, gemSize3.Y);
			}
		}
		_xStart = gemDiameterStart.GemXSize;
		_xMiddle = gemDiameterMiddle.GemXSize;
		_xEnd = gemDiameterEnd.GemXSize;
		_yStart = gemDiameterStart.GemYSize;
		_yMiddle = gemDiameterMiddle.GemYSize;
		_yEnd = gemDiameterEnd.GemYSize;
		if (metalWidthToggle)
		{
			if (!profileWidthEndToggle)
			{
				_xEnd = _xStart;
				_yEnd = _yStart;
				profileWidthEnd = profileWidthStart;
			}
		}
		else if (!vEnableDiameterEnd)
		{
			_xEnd = _xStart;
			_yEnd = _yStart;
		}
		if (metalWidthToggle)
		{
			if (!profileWidthMiddleToggle)
			{
				_xMiddle = _xStart + (_xEnd - _xStart) * _positionMiddle;
				_yMiddle = _yStart + (_yEnd - _yStart) * _positionMiddle;
				profileWidthMiddle = (profileWidthStart + profileWidthEnd) / 2.0;
			}
		}
		else if (!vEnableDiameterMiddle)
		{
			_xMiddle = _xStart + (_xEnd - _xStart) * _positionMiddle;
			_yMiddle = _yStart + (_yEnd - _yStart) * _positionMiddle;
		}
		if (!vEnablePlacementEnd)
		{
			_placementEnd = _placementStart;
		}
		if (!vEnablePlacementMiddle)
		{
			_placementMiddle = _placementStart + (_placementEnd - _placementStart) * _positionMiddle;
		}
		_xRoll = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		_xTilt = CSBlockUtil.CSUtilMath.Rotation(tiltAngle, Vector3d.XAxis, Point3d.Origin);
		if (!CalculateRotateTestGem(_gemShape, _xStart, _yStart, rollAngle, out _gemRotatedStartXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(_gemShape, _xMiddle, _yMiddle, rollAngle, out _gemRotatedMiddleXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(_gemShape, _xEnd, _yEnd, rollAngle, out _gemRotatedEndXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		_direction = ((!(_positionStartInput > _positionEndInput)) ? 1 : (-1));
		if (toggleGemCount)
		{
			_direction = 1;
		}
		_crvIsClosed = _crv.IsClosed;
		if (_crvIsClosed && vDoReverseDirection)
		{
			_positionStartInput += _direction;
			_direction = -_direction;
		}
		_crvLength = _crv.GetLength();
		if (_doMirror && _crv.IsClosed)
		{
			double num19 = _positionEndInput - _positionStartInput;
			if (num19 > 0.499 && !toggleGemCount)
			{
				_positionEndInput = _positionStartInput + 0.5;
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelStartSlopeLength = 0.0;
				_channelEndSlopeLength = 0.0;
			}
			else if (num19 < -0.499 && !toggleGemCount)
			{
				_positionEndInput = _positionStartInput - 0.5;
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelStartSlopeLength = 0.0;
				_channelEndSlopeLength = 0.0;
			}
		}
		_positionStartGem = _positionStartInput;
		if (!_doMirror && _channelEnabled && moveGems == ValueListItem.ShankEdge)
		{
			_positionStartGem += (_xStart / 2.0 + _channelStartSlopeLength) / _crvLength * (double)_direction;
		}
		if (moveGems == ValueListItem.GemEdge)
		{
			_positionStartGem = _positionStartInput + _xStart / 2.0 / _crvLength * (double)_direction;
		}
		_positionEndGem = _positionEndInput;
		if (_channelEnabled && moveGems == ValueListItem.ShankEdge)
		{
			_positionEndGem -= (_xEnd / 2.0 + _channelEndSlopeLength) / _crvLength * (double)_direction;
		}
		if (moveGems == ValueListItem.GemEdge)
		{
			_positionEndGem = _positionEndInput - _xEnd / 2.0 / _crvLength * (double)_direction;
		}
		double positionStartGem = _positionStartGem;
		double profileWidth;
		if (!_doMirror)
		{
			CalcGemPos(positionStartGem, toggleGemCount, inputGemCount, out gemList, out var _, doMirror: false, mirrorModeEven: false, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
			if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
			{
				AdjustGemPosEnd(profileWidth, toggleGemCount, inputGemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, ref gemList);
			}
		}
		else
		{
			List<UltimateGemData> gemList2 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror2 = new List<UltimateGemData>();
			double num20 = 999.0;
			double num21 = 999.0;
			if (vMirrorMode == ValueListItem.GemOnCrvMirrorEven || vMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(positionStartGem, toggleGemCount, inputGemCount, out gemList2, out gemListMirror2, doMirror: true, mirrorModeEven: true, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
				if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(profileWidth, toggleGemCount, inputGemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, ref gemList2, ref gemListMirror2);
				}
				int count = gemList2.Count;
				if (count > 1)
				{
					num20 = (gemList2[0].GemPoint - gemList2[1].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemList2[1].SizeXRotated / 2.0;
				}
				if (count == 1 && gemListMirror2.Count == 1)
				{
					num20 = (gemList2[0].GemPoint - gemListMirror2[0].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemListMirror2[0].SizeXRotated / 2.0;
				}
				if (count >= 1)
				{
					num21 = Math.Abs(_positionEndGem - gemList2[count - 1].NormParam);
				}
				if (num20 < 0.0)
				{
					num20 = 999.0;
					num21 = 999.0;
				}
			}
			List<UltimateGemData> gemList3 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror3 = new List<UltimateGemData>();
			double num22 = 999.0;
			double num23 = 999.0;
			if (vMirrorMode == ValueListItem.GemOnCrvMirrorOdd || vMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(positionStartGem, toggleGemCount, inputGemCount, out gemList3, out gemListMirror3, doMirror: true, mirrorModeEven: false, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
				if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(profileWidth, toggleGemCount, inputGemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, ref gemList3, ref gemListMirror3);
				}
				int count2 = gemList3.Count;
				if (count2 > 1)
				{
					num22 = (gemList3[0].GemPoint - gemList3[1].GemPoint).Length - gemList3[0].SizeXRotated / 2.0 - gemList3[1].SizeXRotated / 2.0;
				}
				if (count2 >= 1)
				{
					num23 = Math.Abs(_positionEndGem - gemList3[count2 - 1].NormParam);
				}
			}
			List<UltimateGemData> gemListMirror;
			if (num23 < 0.01 && num21 < 0.01)
			{
				if (num22 <= num20)
				{
					gemList = gemList3;
					gemListMirror = gemListMirror3;
				}
				else
				{
					gemList = gemList2;
					gemListMirror = gemListMirror2;
				}
			}
			else if (num23 <= num21)
			{
				gemList = gemList3;
				gemListMirror = gemListMirror3;
			}
			else
			{
				gemList = gemList2;
				gemListMirror = gemListMirror2;
			}
			for (int i = 0; i < gemListMirror.Count; i++)
			{
				gemList.Insert(0, gemListMirror[i]);
			}
		}
		List<Transform> list = new List<Transform>();
		foreach (UltimateGemData gem in gemList)
		{
			list.Add(gem.GemTransform);
		}
		if (list != null)
		{
			for (int j = 0; j < list.Count; j++)
			{
				CSGemData cSGemData = InGemShape.Data as CSGemData;
				CSDataHelper.Transform(cSGemData, list[j]);
				outputGems.Append(cSGemData);
				gemList[j].GemZMax = cSGemData.GemZMax;
				gemList[j].GemZMin = cSGemData.GemZMin;
			}
		}
		if (toggleGemCount && _doMirror)
		{
			_crv.NormalizedLengthParameter(_positionStartInput, out var t);
			if (_crv.DuplicateCurve().Trim(t, gemList[gemList.Count - 1].Param).GetLength() >= _crv.GetLength() * 0.48)
			{
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelStartSlopeLength = 0.0;
				_channelEndSlopeLength = 0.0;
			}
		}
	}
}
