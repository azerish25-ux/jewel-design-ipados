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
[CSBlock("UltimateGemNew_V5", "{132A7773-D76D-416F-A34D-BB8CA9EC6895}")]
public sealed class CSUltimateGemNew5 : CSBlock
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

	public readonly CSInputChannel InShowGems;

	public readonly CSInputChannel InSpacingStart;

	public readonly CSInputChannel InSpacingMiddle;

	public readonly CSInputChannel InSpacingEnd;

	public readonly CSInputChannel InSpacingToleranceStart;

	public readonly CSInputChannel InSpacingToleranceMiddle;

	public readonly CSInputChannel InSpacingToleranceEnd;

	public readonly CSInputChannel InWallWidth;

	public readonly CSInputChannel InChannelWallGemOverlap;

	public readonly CSInputChannel InChannelStartSlopeSpacing;

	public readonly CSInputChannel InChannelEndSlopeSpacing;

	public readonly CSInputChannel InMoveGems;

	public readonly CSInputChannel InSideBuild;

	public readonly CSInputChannel InFront;

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

	public readonly CSOutputChannel OutWallWidth;

	public readonly CSOutputChannel OutChannelWallGemOverlap;

	public readonly CSOutputChannel OutCurveLength;

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

	public CSUltimateGemNew5(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.Surface, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
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
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirrorMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemOnCrvMirrorOddAndEven);
		InChannelClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseMinCuletDistance = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMinCuletDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InSpacingStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingToleranceStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingToleranceMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingToleranceEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelStartSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelEndSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMoveGems = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.ShankEdge);
		InSideBuild = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFront = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InEuroGemToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEuroGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 135.0);
		InGemCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5);
		InGemCountToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMetalWidthToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InProfileWidthMiddleToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InProfileWidthEndToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InForceGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutDisplayGemArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGemArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGems = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutChannelYStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelYEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionMiddle = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelStartSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEndSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		bool flag = InShowGems.Boolean;
		_crv = InCurve.Curve;
		double gemCount = InGemCount.Double;
		bool boolean = InGemCountToggle.Boolean;
		_baseCrv = InBaseObject.Curve;
		CSDataHelper.GetBrep(InBaseObject, out _baseSrf);
		_gemShape = InGemShape.Gem;
		CSGemData gem = InDiameterStart.Gem;
		CSGemData gem2 = InDiameterMiddle.Gem;
		CSGemData gem3 = InDiameterEnd.Gem;
		_positionStartInput = InPositionStart.Double;
		_positionEndInput = InPositionEnd.Double;
		_positionMiddle = InPositionMiddle.Percentage;
		_placementStart = InPlacementStart.Double;
		_placementMiddle = InPlacementMiddle.Double;
		_placementEnd = InPlacementEnd.Double;
		bool boolean2 = InEuroGemToggle.Boolean;
		double num = InEuroGemAngle.Double;
		if (boolean2)
		{
			Vector3d val = new Vector3d(_crv.PointAtStart - _crv.PointAtNormalizedLength(0.5));
			double length = ((Vector3d)(ref val)).Length;
			Surface val2 = (Surface)(object)NurbsSurface.CreateFromCorners(new Point3d(0.0, -1.0, 0.0), new Point3d(0.0, -1.0, length), new Point3d(0.0, 1.0, length), new Point3d(0.0, 1.0, 0.0));
			((GeometryBase)val2).Transform(Transform.Rotation(RhinoMath.ToRadians(num), Vector3d.YAxis, Point3d.Origin));
			CurveIntersections val3 = Intersection.CurveSurface(_crv, val2, 0.01, 0.01);
			double num2 = default(double);
			_crv.ClosestPoint(val3[0].PointA, ref num2);
			double num3 = default(double);
			_crv.ClosestPoint(_crv.PointAtStart, ref num3);
			double length2 = _crv.GetLength();
			double length3 = _crv.GetLength(new Interval(num3, num2));
			if (_positionEndInput > length3 / length2)
			{
				_positionEndInput = length3 / length2;
			}
		}
		_startGemSpacing = InSpacingStart.Double;
		_startGemSpacingTolerance = InSpacingToleranceStart.Double;
		_middleGemSpacing = InSpacingMiddle.Double;
		_middleGemSpacingTolerance = InSpacingToleranceMiddle.Double;
		_endGemSpacing = InSpacingEnd.Double;
		_endGemSpacingTolerance = InSpacingToleranceEnd.Double;
		double angleDegrees = InRoll.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InTilt.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		_vOrientationMethod = InOrientationMethod.ValueListItem;
		_vPlacementMethod = InPlacementMethod.ValueListItem;
		_vSpacingMethod = InSpacingMethod.ValueListItem;
		if (boolean)
		{
			_vSpacingMethod = ValueListItem.GemSpacingMethodFixed;
		}
		_orientationFollowCurve = InOrientationFollowCurve.Boolean;
		bool boolean3 = InEnableDiameterMiddle.Boolean;
		bool boolean4 = InEnableDiameterEnd.Boolean;
		bool boolean5 = InEnablePlacementMiddle.Boolean;
		bool boolean6 = InEnablePlacementEnd.Boolean;
		ValueListItem valueListItem = InMoveGems.ValueListItem;
		_doFlip = InDoFlip.Boolean;
		_doMirror = InDoMirror.Boolean;
		ValueListItem valueListItem2 = InMirrorMode.ValueListItem;
		bool boolean7 = InDoReverseDirection.Boolean;
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
		_sideBuilt = InSideBuild.Boolean;
		bool boolean8 = InMetalWidthToggle.Boolean;
		double profileWidthStart = InProfileWidthStart.Double;
		double profileWidthMiddle = InProfileWidthMiddle.Double;
		bool boolean9 = InProfileWidthMiddleToggle.Boolean;
		double profileWidthEnd = InProfileWidthEnd.Double;
		bool boolean10 = InProfileWidthEndToggle.Boolean;
		if (InForceGems.Boolean)
		{
			flag = true;
		}
		UltGem(gem, gem2, gem3, boolean4, boolean3, boolean6, boolean5, boolean7, valueListItem, valueListItem2, angleDegrees, angleDegrees2, boolean, gemCount, boolean8, profileWidthStart, profileWidthMiddle, boolean9, profileWidthEnd, boolean10, out var outputGems, out var gemList);
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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(ultimateGemData, profileWidth);
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(ultimateGemData.GemTransform);
			MoveAGem(ultimateGemData, _positionStartGem, endError, vLen, profileWidth);
			Point3d origin2 = Point3d.Origin;
			((Point3d)(ref origin2)).Transform(ultimateGemData.GemTransform);
			Vector3d val = origin2 - origin;
			double length = ((Vector3d)(ref val)).Length;
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
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
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
			Vector3d val = gemPoint - gemPoint2;
			if (((Vector3d)(ref val)).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - ultimateGemData.Spacing * 0.8 < 0.0)
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
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		profileWidth = profileWidthStart;
		Point3d val = Point3d.Unset;
		Point3d val2 = Point3d.Unset;
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
			Vector3d val3;
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
					val = ultimateGemData2.GemPoint;
					val2 = val;
					num = ultimateGemData.SizeXRotated;
				}
				val3 = ultimateGemData.GemPoint - val;
				double length2 = ((Vector3d)(ref val3)).Length;
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
				val = ultimateGemData.GemPoint;
			}
			if (doMirror)
			{
				num6 = 999.0;
				num7 = 20;
				if (val2 == Point3d.Unset)
				{
					val2 = val;
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
							val3 = ultimateGemData2.GemPoint - val2;
							double length3 = ((Vector3d)(ref val3)).Length;
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
						val2 = ultimateGemData2.GemPoint;
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
				val = gemList[gemList.Count - 1].GemPoint;
				val3 = gemPoint - val;
				if (((Vector3d)(ref val3)).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - gemList[gemList.Count - 1].Spacing < 0.0)
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
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
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
		CSUtilGem.GetGemScaleFromTable(_gemShape, num2, num3, useLargestPossible: true, useIterationOnly: false, out Transform xBestScale);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(firstGem.GemTransform);
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
			Vector3d val = ultimateGemData.GemPoint - origin;
			double length2 = ((Vector3d)(ref val)).Length;
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		double num = gemOnCurveInfo.SizeX / _gemShape.GemXSize;
		Transform val;
		if (gemOnCurveInfo.SizeY > 0.0)
		{
			double num2 = gemOnCurveInfo.SizeY / _gemShape.GemYSize;
			val = Transform.Scale(Plane.WorldXY, num, num2, num);
		}
		else
		{
			val = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		CSGemData cSGemData = new CSGemData(_gemShape);
		cSGemData.ApplyTransform(val);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, _vPlacementMethod, ref zOffset);
		CalculatePointsOnCrv(_crv, _baseCrv, _vOrientationMethod, gemOnCurveInfo);
		Point3d val2 = Point3d.Unset;
		if (_baseCrv != null)
		{
			val2 = _baseCrv.PointAt(gemOnCurveInfo.BaseParam);
			if (_sideBuilt)
			{
				((Point3d)(ref val2)).Y = (_inFront ? (((Point3d)(ref val2)).Y - gemOnCurveInfo.SizeY / 2.0) : (((Point3d)(ref val2)).Y + gemOnCurveInfo.SizeY / 2.0));
			}
		}
		Point3d val3 = _crv.PointAt(gemOnCurveInfo.Param);
		if (_sideBuilt)
		{
			((Point3d)(ref val3)).Y = (_inFront ? (((Point3d)(ref val3)).Y - gemOnCurveInfo.SizeY / 2.0) : (((Point3d)(ref val3)).Y + gemOnCurveInfo.SizeY / 2.0));
		}
		Vector3d val4 = _crv.TangentAt(gemOnCurveInfo.Param);
		double num3 = 0.0;
		if (_channelEnabled && _profile != null)
		{
			double num4 = ProfileCut(_channelWallGemOverlap, _profile, profileWidth, gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, _crv, _baseCrv, _sideBuilt, _inFront, out var pScale, out var pRail);
			Vector3d val5 = val3 - val2;
			num3 = ((Vector3d)(ref val5)).Length * (1.0 - num4);
			((GeometryBase)gemOnCurveInfo.ProfileCurve).Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Vector3d val6 = (_doFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d val7 = (_doFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal = default(Vector3d);
		switch (_vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(_baseSrf, val3, ref brepNormal);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			((Vector3d)(ref brepNormal))._002Ector(val3);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			((Vector3d)(ref brepNormal))._002Ector(((Point3d)(ref val3)).X, 0.0, ((Point3d)(ref val3)).Z);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			break;
		case ValueListItem.OrientationMethodClosestPointOnCurve:
		case ValueListItem.OrientationMethodIntersectPointOnCurve:
			brepNormal = (Vector3d)((val2 != Point3d.Unset) ? (val3 - val2) : new Vector3d(val3));
			((Vector3d)(ref brepNormal)).Unitize();
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			break;
		}
		((Vector3d)(ref brepNormal)).Unitize();
		Transform val8 = (_orientationFollowCurve ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val6, val3, val4, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val6, val7, val3, brepNormal, val4));
		double num5 = 0.0;
		if (_vUseMinCuletDistance)
		{
			Vector3d val9 = val3 - val2;
			double length = ((Vector3d)(ref val9)).Length;
			if (cSGemData.GemZSize > length - _vMinCuletDistance - num3)
			{
				num5 = (gemOnCurveInfo.MinCuletMovement = cSGemData.GemZSize - (length - _vMinCuletDistance - num3));
			}
		}
		double num6 = zOffset + gemOnCurveInfo.ZPlacement - num3 + num5;
		Transform val10 = Transform.Translation(brepNormal * num6);
		gemOnCurveInfo.GemTransform = val10 * val8 * _xRoll * _xTilt * val;
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem vOrientationMethod, UltimateGemData gem)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		crv.NormalizedLengthParameter(num, ref gem.Param);
		Point3d val = crv.PointAt(gem.Param);
		Vector3d val2 = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (vOrientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
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

	private static bool CalculateRotateTestGem(CSGemData gemShape, double xSize, double ySize, double rollAngle, out double xSizeOut)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		Transform val = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, useLargestPossible: true, useIterationOnly: false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(gemShape);
		cSGemData.ApplyTransform(val * xBestScale);
		Curve curve = cSGemData.GetCurve();
		NurbsCurve val2 = (NurbsCurve)(object)((curve is NurbsCurve) ? curve : null);
		if (val2 == null)
		{
			xSizeOut = 0.0;
			return false;
		}
		Point3d val4;
		for (int i = 0; i < val2.Points.Count; i++)
		{
			ControlPoint val3 = val2.Points[i];
			val4 = ((ControlPoint)(ref val3)).Location;
			double x = ((Point3d)(ref val4)).X;
			val4 = ((ControlPoint)(ref val3)).Location;
			((ControlPoint)(ref val3)).Location = new Point3d(x, ((Point3d)(ref val4)).Y, 0.0);
			val2.Points[i] = val3;
		}
		BoundingBox boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double y = ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double num = y2 - ((Point3d)(ref val4)).Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections val5 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), (Curve)(object)val2, 0.001, 0.001);
			if (val5.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in val5)
			{
				Point3d pointA = item.PointA;
				if (((Point3d)(ref pointA)).X > num4)
				{
					num4 = ((Point3d)(ref pointA)).X;
				}
				if (((Point3d)(ref pointA)).X < num3)
				{
					num3 = ((Point3d)(ref pointA)).X;
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve val = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve val2 = new LineCurve(new Point3d(sizeY / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform val3 = Transform.Scale(Plane.WorldXY, profileWidth, 1.0, 1.0);
		((GeometryBase)gemOnCurveInfo.ProfileCurve).Transform(val3);
		CurveIntersections obj = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, (Curve)(object)val, 0.001, 0.001);
		CurveIntersections val4 = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, (Curve)(object)val2, 0.001, 0.001);
		double num = -1.0;
		foreach (IntersectionEvent item in obj)
		{
			Point3d pointA = item.PointA;
			if (((Point3d)(ref pointA)).Y > num)
			{
				num = ((Point3d)(ref pointA)).Y;
				gemOnCurveInfo.ParamLeft = item.ParameterA;
			}
		}
		double num2 = -1.0;
		foreach (IntersectionEvent item2 in val4)
		{
			Point3d pointA2 = item2.PointA;
			if (((Point3d)(ref pointA2)).Y > num2)
			{
				num2 = ((Point3d)(ref pointA2)).Y;
				gemOnCurveInfo.ParamRight = item2.ParameterA;
			}
		}
		return (num + num2) / 2.0;
	}

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, bool sideBuilt, bool inFront, out Transform pScale, out Transform pRail)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = crv.PointAt(gem.Param);
		if (sideBuilt)
		{
			((Point3d)(ref val)).Y = (inFront ? (((Point3d)(ref val)).Y - gem.SizeY / 2.0) : (((Point3d)(ref val)).Y + gem.SizeY / 2.0));
		}
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d val2 = baseCrv.PointAt(gem.BaseParam);
		if (sideBuilt)
		{
			((Point3d)(ref val2)).Y = (inFront ? (((Point3d)(ref val2)).Y - gem.SizeY / 2.0) : (((Point3d)(ref val2)).Y + gem.SizeY / 2.0));
		}
		Vector3d destination2 = val - val2;
		double length = ((Vector3d)(ref destination2)).Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val, destination2, destination);
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

	private void UltGem(CSGemData gemDiameterStart, CSGemData gemDiameterMiddle, CSGemData gemDiameterEnd, bool vEnableDiameterEnd, bool vEnableDiameterMiddle, bool vEnablePlacementEnd, bool vEnablePlacementMiddle, bool vDoReverseDirection, ValueListItem moveGems, ValueListItem vMirrorMode, double rollAngle, double tiltAngle, bool toggleGemCount, double gemCount, bool metalWidthToggle, double profileWidthStart, double profileWidthMiddle, bool profileWidthMiddleToggle, double profileWidthEnd, bool profileWidthEndToggle, out CSDataArray outputGems, out List<UltimateGemData> gemList)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		outputGems = new CSDataArray();
		Vector3d val;
		Point3d val2;
		if (_profile != null)
		{
			BoundingBox boundingBox = ((GeometryBase)_profile).GetBoundingBox(true);
			val = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Diagonal;
			double y = ((Vector3d)(ref val)).Y;
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double x2 = ((Point3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double num = (0.0 - (x2 + ((Point3d)(ref val2)).X)) / 2.0;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			Transform val3 = Transform.Translation(num, 0.0 - ((Point3d)(ref val2)).Y, 0.0);
			((GeometryBase)_profile).Transform(val3);
			Transform val4 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			((GeometryBase)_profile).Transform(val4);
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
			double num2 = -1.23432101234321E+308;
			double num3 = -1.23432101234321E+308;
			double num4 = -1.23432101234321E+308;
			double num5 = profileWidthStart - _wallWidth * 2.0;
			double num6 = profileWidthMiddle - _wallWidth * 2.0;
			double num7 = profileWidthEnd - _wallWidth * 2.0;
			foreach (GemSize size in gemDiameterStart.Sizes)
			{
				double num8 = size.X / gemDiameterStart.GemXSize;
				double num9 = size.Y / gemDiameterStart.GemYSize;
				double num10 = (num8 + num9) / 2.0;
				Transform val5 = Transform.Scale(Plane.WorldXY, num8, num9, num10);
				NurbsCurve nurbsCurve = gemDiameterStart.GetNurbsCurve();
				((GeometryBase)nurbsCurve).Transform(val5);
				BoundingBox boundingBox2 = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
				val2 = ((BoundingBox)(ref boundingBox2)).Max;
				double num11 = Math.Abs(((Point3d)(ref val2)).Y);
				val2 = ((BoundingBox)(ref boundingBox2)).Min;
				double num12 = num11 + Math.Abs(((Point3d)(ref val2)).Y);
				double num13 = Math.Abs(num5 - num12);
				if (!RhinoMath.IsValidDouble(num2) || num13 < num2)
				{
					gemSize = size;
					num2 = num13;
				}
				double num14 = size.X / gemDiameterMiddle.GemXSize;
				double num15 = size.Y / gemDiameterMiddle.GemYSize;
				double num16 = (num14 + num15) / 2.0;
				Transform val6 = Transform.Scale(Plane.WorldXY, num14, num15, num16);
				NurbsCurve nurbsCurve2 = gemDiameterMiddle.GetNurbsCurve();
				((GeometryBase)nurbsCurve2).Transform(val6);
				BoundingBox boundingBox3 = ((GeometryBase)nurbsCurve2).GetBoundingBox(true);
				val2 = ((BoundingBox)(ref boundingBox3)).Max;
				double num17 = Math.Abs(((Point3d)(ref val2)).Y);
				val2 = ((BoundingBox)(ref boundingBox3)).Min;
				double num18 = num17 + Math.Abs(((Point3d)(ref val2)).Y);
				double num19 = Math.Abs(num6 - num18);
				if (!RhinoMath.IsValidDouble(num3) || num19 < num3)
				{
					gemSize2 = size;
					num3 = num19;
				}
				double num20 = size.X / gemDiameterEnd.GemXSize;
				double num21 = size.Y / gemDiameterEnd.GemYSize;
				double num22 = (num20 + num21) / 2.0;
				Transform val7 = Transform.Scale(Plane.WorldXY, num20, num21, num22);
				NurbsCurve nurbsCurve3 = gemDiameterEnd.GetNurbsCurve();
				((GeometryBase)nurbsCurve3).Transform(val7);
				BoundingBox boundingBox4 = ((GeometryBase)nurbsCurve3).GetBoundingBox(true);
				val2 = ((BoundingBox)(ref boundingBox4)).Max;
				double num23 = Math.Abs(((Point3d)(ref val2)).Y);
				val2 = ((BoundingBox)(ref boundingBox4)).Min;
				double num24 = num23 + Math.Abs(((Point3d)(ref val2)).Y);
				double num25 = Math.Abs(num7 - num24);
				if (!RhinoMath.IsValidDouble(num4) || num25 < num4)
				{
					gemSize3 = size;
					num4 = num25;
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
			double num26 = _positionEndInput - _positionStartInput;
			if (num26 > 0.499 && !toggleGemCount)
			{
				_positionEndInput = _positionStartInput + 0.5;
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelStartSlopeLength = 0.0;
				_channelEndSlopeLength = 0.0;
			}
			else if (num26 < -0.499 && !toggleGemCount)
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
			CalcGemPos(positionStartGem, toggleGemCount, gemCount, out gemList, out var _, doMirror: false, mirrorModeEven: false, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
			if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
			{
				AdjustGemPosEnd(profileWidth, toggleGemCount, gemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, ref gemList);
			}
		}
		else
		{
			List<UltimateGemData> gemList2 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror2 = new List<UltimateGemData>();
			double num27 = 999.0;
			double num28 = 999.0;
			if (vMirrorMode == ValueListItem.GemOnCrvMirrorEven || vMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(positionStartGem, toggleGemCount, gemCount, out gemList2, out gemListMirror2, doMirror: true, mirrorModeEven: true, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
				if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(profileWidth, toggleGemCount, gemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, ref gemList2, ref gemListMirror2);
				}
				int count = gemList2.Count;
				if (count > 1)
				{
					val = gemList2[0].GemPoint - gemList2[1].GemPoint;
					num27 = ((Vector3d)(ref val)).Length - gemList2[0].SizeXRotated / 2.0 - gemList2[1].SizeXRotated / 2.0;
				}
				if (count == 1 && gemListMirror2.Count == 1)
				{
					val = gemList2[0].GemPoint - gemListMirror2[0].GemPoint;
					num27 = ((Vector3d)(ref val)).Length - gemList2[0].SizeXRotated / 2.0 - gemListMirror2[0].SizeXRotated / 2.0;
				}
				if (count >= 1)
				{
					num28 = Math.Abs(_positionEndGem - gemList2[count - 1].NormParam);
				}
				if (num27 < 0.0)
				{
					num27 = 999.0;
					num28 = 999.0;
				}
			}
			List<UltimateGemData> gemList3 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror3 = new List<UltimateGemData>();
			double num29 = 999.0;
			double num30 = 999.0;
			if (vMirrorMode == ValueListItem.GemOnCrvMirrorOdd || vMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(positionStartGem, toggleGemCount, gemCount, out gemList3, out gemListMirror3, doMirror: true, mirrorModeEven: false, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, out profileWidth);
				if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(profileWidth, toggleGemCount, gemCount, metalWidthToggle, profileWidthStart, profileWidthMiddle, profileWidthEnd, ref gemList3, ref gemListMirror3);
				}
				int count2 = gemList3.Count;
				if (count2 > 1)
				{
					val = gemList3[0].GemPoint - gemList3[1].GemPoint;
					num29 = ((Vector3d)(ref val)).Length - gemList3[0].SizeXRotated / 2.0 - gemList3[1].SizeXRotated / 2.0;
				}
				if (count2 >= 1)
				{
					num30 = Math.Abs(_positionEndGem - gemList3[count2 - 1].NormParam);
				}
			}
			List<UltimateGemData> gemListMirror;
			if (num30 < 0.01 && num28 < 0.01)
			{
				if (num29 <= num27)
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
			else if (num30 <= num28)
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
			double num31 = default(double);
			_crv.NormalizedLengthParameter(_positionStartInput, ref num31);
			if (_crv.DuplicateCurve().Trim(num31, gemList[gemList.Count - 1].Param).GetLength() >= _crv.GetLength() * 0.48)
			{
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelStartSlopeLength = 0.0;
				_channelEndSlopeLength = 0.0;
			}
		}
	}
}
