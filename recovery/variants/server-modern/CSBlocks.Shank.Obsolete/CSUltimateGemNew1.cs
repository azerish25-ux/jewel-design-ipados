using System;
using System.Collections.Generic;
using System.Linq;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSBlocks.Shank.Helpers;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.Obsolete;

[Obsolete]
[CSBlock("UltimateGemNew_V1", "{3D68B663-28CA-440C-9616-73C94178095F}")]
public sealed class CSUltimateGemNew1 : CSBlock
{
	private Curve _crv;

	private double _crvLength;

	private bool _crvIsClosed;

	private Curve _baseCrv;

	private Brep _baseSrf;

	private CSGemData _gemShape;

	private Transform _xRoll;

	private Transform _xTilt;

	private ValueListItem _vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;

	private ValueListItem _vPlacementMethod = ValueListItem.GemPlacementMethodGirdle;

	private ValueListItem _vSpacingMethod = ValueListItem.GemSpacingMethodMinimum;

	private bool _OrientationFollowCurve;

	private bool _doFlip;

	private int _direction;

	private double _gemRotatedStartXSize;

	private double _gemRotatedMiddleXSize;

	private double _gemRotatedEndXSize;

	private double _xStart;

	private double _xMiddle;

	private double _xEnd;

	private double _yStart;

	private double _yMiddle;

	private double _yEnd;

	private double _placementStart;

	private double _placementMiddle;

	private double _placementEnd;

	private double _positionStartInput;

	private double _positionStartGem;

	private double _positionEndInput;

	private double _positionEndGem;

	private double _positionMiddle;

	private bool _doMirror;

	private bool _sideBuilt;

	private bool _inFront;

	private double _startGemSpacing;

	private double _startGemSpacingTolerance;

	private double _middleGemSpacing;

	private double _middleGemSpacingTolerance;

	private double _endGemSpacing;

	private double _endGemSpacingTolerance;

	private Curve _profile;

	private double _wallWidth;

	private bool _channelEnabled;

	private double _channelWallGemOverlap;

	private double _channelStartSlopeLength;

	private double _channelEndSlopeLength;

	private bool _gemsClosedLoopSoNoChannelEnds;

	private bool _vUseMinCuletDistance;

	private double _vMinCuletDistance;

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

	public readonly CSInputChannel InSpacingTolleranceStart;

	public readonly CSInputChannel InSpacingTolleranceMiddle;

	public readonly CSInputChannel InSpacingTolleranceEnd;

	public readonly CSInputChannel InWallWidth;

	public readonly CSInputChannel InChannelWallGemOverlap;

	public readonly CSInputChannel InChannelStartSlopeSpacing;

	public readonly CSInputChannel InChannelEndSlopeSpacing;

	public readonly CSInputChannel InMoveGems;

	public readonly CSInputChannel InSideBuild;

	public readonly CSInputChannel InFront;

	public readonly CSInputChannel InEuroGemToggle;

	public readonly CSInputChannel InEuroGemAngle;

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

	public CSUltimateGemNew1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.Surface, ChannelArrayType.Any);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InPositionStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPositionEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPositionMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacementStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacementMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacementEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InOrientationFollowCurve = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InSpacingMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InEnableDiameterMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableDiameterEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnablePlacementMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnablePlacementEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoReverseDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InMirrorMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InChannelClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InUseMinCuletDistance = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InMinCuletDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InSpacingStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingTolleranceStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingTolleranceMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingTolleranceEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelStartSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMoveGems = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		InSideBuild = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFront = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InEuroGemToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEuroGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 135.0);
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
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		bool value = true;
		InShowGems.GetBoolean(ref value);
		if (!CSDataHelper.GetCurve(InCurve, out _crv))
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetCurve(InBaseObject, out _baseCrv);
		CSDataHelper.GetBrep(InBaseObject, out _baseSrf);
		_gemShape = InGemShape.Data as CSGemData;
		if (_gemShape == null)
		{
			return UpdateMissing("Gem Shape");
		}
		if (!(InDiameterStart.Data is CSGemData gemDiameterStart))
		{
			return UpdateMissing("Start Diameter");
		}
		if (!(InDiameterMiddle.Data is CSGemData gemDiameterMiddle))
		{
			return UpdateMissing("Middle Diameter");
		}
		if (!(InDiameterEnd.Data is CSGemData gemDiameterEnd))
		{
			return UpdateMissing("End Diameter");
		}
		_positionStartInput = 0.0;
		InPositionStart.GetDouble(ref _positionStartInput);
		_positionEndInput = 1.0;
		InPositionEnd.GetDouble(ref _positionEndInput);
		_positionMiddle = 0.5;
		InPositionMiddle.GetDoublePercentage(ref _positionMiddle);
		_placementStart = 0.0;
		InPlacementStart.GetDouble(ref _placementStart);
		_placementMiddle = 0.0;
		InPlacementMiddle.GetDouble(ref _placementMiddle);
		_placementEnd = 0.0;
		InPlacementEnd.GetDouble(ref _placementEnd);
		InEuroGemToggle.GetValue(out bool value2);
		InEuroGemAngle.GetValue(out double value3);
		if (value2)
		{
			Vector3d val = new Vector3d(_crv.PointAtStart - _crv.PointAtNormalizedLength(0.5));
			double length = ((Vector3d)(ref val)).Length;
			Surface val2 = (Surface)(object)NurbsSurface.CreateFromCorners(new Point3d(0.0, -1.0, 0.0), new Point3d(0.0, -1.0, length), new Point3d(0.0, 1.0, length), new Point3d(0.0, 1.0, 0.0));
			((GeometryBase)val2).Transform(Transform.Rotation(RhinoMath.ToRadians(value3), Vector3d.YAxis, Point3d.Origin));
			CurveIntersections val3 = Intersection.CurveSurface(_crv, val2, 0.01, 0.01);
			double num = default(double);
			_crv.ClosestPoint(val3[0].PointA, ref num);
			double num2 = default(double);
			_crv.ClosestPoint(_crv.PointAtStart, ref num2);
			double length2 = _crv.GetLength();
			double length3 = _crv.GetLength(new Interval(num2, num));
			_positionEndInput = length3 / length2;
		}
		_startGemSpacing = 0.0;
		InSpacingStart.Data.GetDouble(ref _startGemSpacing);
		_startGemSpacingTolerance = 0.0;
		InSpacingTolleranceStart.Data.GetDouble(ref _startGemSpacingTolerance);
		_middleGemSpacing = 0.0;
		InSpacingMiddle.Data.GetDouble(ref _middleGemSpacing);
		_middleGemSpacingTolerance = 0.0;
		InSpacingTolleranceMiddle.Data.GetDouble(ref _middleGemSpacingTolerance);
		InSpacingEnd.Data.GetDouble(ref _endGemSpacing);
		_endGemSpacingTolerance = 0.0;
		InSpacingTolleranceEnd.Data.GetDouble(ref _endGemSpacingTolerance);
		double value4 = 0.0;
		InRoll.GetDouble(ref value4);
		value4 = RhinoMath.ToRadians(value4);
		double value5 = 0.0;
		InTilt.GetDouble(ref value5);
		value5 = RhinoMath.ToRadians(value5);
		_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		InOrientationMethod.GetValueListItem(ref _vOrientationMethod);
		_vPlacementMethod = ValueListItem.GemPlacementMethodGirdle;
		InPlacementMethod.GetValueListItem(ref _vPlacementMethod);
		_vSpacingMethod = ValueListItem.GemSpacingMethodMinimum;
		InSpacingMethod.GetValueListItem(ref _vSpacingMethod);
		_OrientationFollowCurve = true;
		InOrientationFollowCurve.GetBoolean(ref _OrientationFollowCurve);
		bool value6 = false;
		InEnableDiameterMiddle.GetBoolean(ref value6);
		bool value7 = false;
		InEnableDiameterEnd.GetBoolean(ref value7);
		bool value8 = false;
		InEnablePlacementMiddle.GetBoolean(ref value8);
		bool value9 = false;
		InEnablePlacementEnd.GetBoolean(ref value9);
		ValueListItem whichItem = ValueListItem.ShankEdge;
		InMoveGems.GetValueListItem(ref whichItem);
		_doFlip = false;
		InDoFlip.GetBoolean(ref _doFlip);
		_doMirror = false;
		InDoMirror.GetBoolean(ref _doMirror);
		ValueListItem whichItem2 = ValueListItem.GemOnCrvMirrorOddAndEven;
		InMirrorMode.GetValueListItem(ref whichItem2);
		bool value10 = false;
		InDoReverseDirection.GetBoolean(ref value10);
		_vUseMinCuletDistance = false;
		InUseMinCuletDistance.GetBoolean(ref _vUseMinCuletDistance);
		_vMinCuletDistance = 0.0;
		InMinCuletDistance.GetDouble(ref _vMinCuletDistance);
		if (value)
		{
			CSDataHelper.GetCurve(InProfile2, out _profile);
			OutProfile.SetData(InProfile2.Data);
		}
		else
		{
			CSDataHelper.GetCurve(InProfile, out _profile);
			OutProfile.SetData(InProfile.Data);
		}
		_channelEnabled = false;
		InChannelEnabled.GetBoolean(ref _channelEnabled);
		InWallWidth.Data.GetDouble(ref _wallWidth);
		InChannelWallGemOverlap.Data.GetDouble(ref _channelWallGemOverlap);
		InChannelStartSlopeSpacing.Data.GetDouble(ref _channelStartSlopeLength);
		InChannelEndSlopeSpacing.Data.GetDouble(ref _channelEndSlopeLength);
		_gemsClosedLoopSoNoChannelEnds = false;
		InChannelClosed.GetBoolean(ref _gemsClosedLoopSoNoChannelEnds);
		InFront.GetValue(out _inFront);
		InSideBuild.GetValue(out _sideBuilt);
		UltGem(gemDiameterStart, gemDiameterMiddle, gemDiameterEnd, value7, value6, value9, value8, value10, whichItem, whichItem2, value4, value5, out var outputGems, out var gemList);
		double length4 = _crv.GetLength();
		int count = gemList.Count;
		double gem0SizeX = 0.0;
		double gem0SizeY = 0.0;
		double gem1SizeY = 0.0;
		double gem0Position = 0.0;
		double gem1Position = 0.0;
		double gemLast0SizeX = 0.0;
		double gemLast0SizeY = 0.0;
		double gemLast1SizeY = 0.0;
		double gemLast0Position = 0.0;
		double gemLast1Position = 0.0;
		ShankMath(gemList, count, out gem0SizeX, out gem0SizeY, out gem1SizeY, out gem0Position, out gem1Position, out gemLast0SizeX, out gemLast0SizeY, out gemLast1SizeY, out gemLast0Position, out gemLast1Position);
		if (value)
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
		return base.Update();
	}

	private void AdjustEndErrorSoGemSpacingLessGemTollerance(List<UltimateGemData> gemList, double vLen, double gemIndex, ref double endError)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(ultimateGemData);
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(ultimateGemData.GemTransform);
			moveAGem(ultimateGemData, _positionStartGem, endError, vLen);
			Point3d origin2 = Point3d.Origin;
			((Point3d)(ref origin2)).Transform(ultimateGemData.GemTransform);
			Vector3d val = origin2 - origin;
			double length = ((Vector3d)(ref val)).Length;
			double num = gemList.Count - 1;
			if (num == 0.0)
			{
				num = 1.0;
			}
			length /= num;
			double num2 = 0.0;
			if (gemIndex == 0.0)
			{
				num2 = _startGemSpacingTolerance;
			}
			if (gemIndex > 0.0 && gemIndex <= (double)(gemList.Count - 1) / 2.0)
			{
				num2 = _startGemSpacingTolerance * (1.0 - gemIndex / ((double)gemList.Count - 1.0) / 2.0) + _middleGemSpacingTolerance * (gemIndex / ((double)gemList.Count - 1.0) / 2.0);
			}
			if (gemIndex > (double)(gemList.Count - 1) / 2.0 && gemIndex < (double)(gemList.Count - 1))
			{
				num2 = _middleGemSpacingTolerance * (gemIndex / (double)(gemList.Count - 1) / (double)(gemList.Count - 1) / 2.0) + _endGemSpacingTolerance * (1.0 - gemIndex / (double)(gemList.Count - 1) / (double)(gemList.Count - 1) / 2.0);
			}
			if (gemIndex == (double)(gemList.Count - 1))
			{
				num2 = _endGemSpacingTolerance;
			}
			if (length > num2)
			{
				endError *= num2 / length;
			}
		}
	}

	private void AdjustGemPosEnd(ref List<UltimateGemData> gemList)
	{
		if (gemList.Count <= 1)
		{
			return;
		}
		double paramPosition = _positionEndGem;
		if (_crvIsClosed)
		{
			_direction = -_direction;
			CalcLastLoopingGemPos(gemList[0], gemList[gemList.Count - 1], _positionEndGem, out paramPosition);
			if (_positionEndGem != paramPosition)
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
			AdjustEndErrorSoGemSpacingLessGemTollerance(gemList, num2, i, ref endError);
			moveAGem(gemList[i], _positionStartGem, endError, num2);
		}
	}

	private void AdjustGemPosEndMirror(ref List<UltimateGemData> gemList, ref List<UltimateGemData> gemListMirror)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count <= 1 && gemListMirror.Count <= 0)
		{
			return;
		}
		double positionEndGem = _positionEndGem;
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = positionEndGem - normParam;
		double num = normParam - _positionStartGem;
		AdjustEndErrorSoGemSpacingLessGemTollerance(gemList, num, gemList.Count - 1, ref endError);
		double endError2 = endError;
		double gemRowLength = num;
		if (gemList.Count > 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			UltimateGemData ultimateGemData2 = gemListMirror[gemListMirror.Count - 1].Duplicate();
			moveAGem(ultimateGemData, _positionStartGem, endError, num);
			moveAGem(ultimateGemData2, _positionStartGem, endError2, gemRowLength);
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
		foreach (UltimateGemData gem in gemList)
		{
			moveAGem(gem, _positionStartGem, endError, num);
		}
		foreach (UltimateGemData item in gemListMirror)
		{
			moveAGem(item, _positionStartGem, endError2, gemRowLength);
		}
	}

	private void CalcGemPos(double paramPosition, out List<UltimateGemData> gemList, out List<UltimateGemData> gemListMirror, bool DoMirror, bool MirrorModeEven)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		Point3d val = Point3d.Unset;
		Point3d val2 = Point3d.Unset;
		double num = 0.0;
		double length = _crv.GetLength();
		double num2 = paramPosition;
		double num3 = paramPosition;
		bool flag = false;
		while (paramPosition * (double)_direction < _positionEndGem * (double)_direction && !flag)
		{
			double num4 = 999.0;
			int num5 = 20;
			UltimateGemData ultimateGemData = null;
			UltimateGemData ultimateGemData2 = null;
			Vector3d val3;
			while (num4 > 0.01 && num5 > 0)
			{
				num5--;
				ultimateGemData = CalcGemSize(paramPosition);
				CalculateGemXform(ultimateGemData);
				if (gemList.Count == 0)
				{
					if (!(DoMirror && MirrorModeEven))
					{
						num4 = 0.0;
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
					CalculateGemXform(ultimateGemData2);
					val = ultimateGemData2.GemPoint;
					val2 = val;
					num = ultimateGemData.SizeXRotated;
				}
				val3 = ultimateGemData.GemPoint - val;
				double length2 = ((Vector3d)(ref val3)).Length;
				length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - num / 2.0;
				num4 = Math.Abs(length2 - ultimateGemData.Spacing);
				if (!(num4 > 0.01))
				{
					continue;
				}
				double num6 = (ultimateGemData.Spacing - length2) * (double)_direction;
				if (DoMirror && MirrorModeEven && gemList.Count == 0)
				{
					num6 /= 2.0;
				}
				paramPosition += num6 / length * 0.8;
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
				val = ultimateGemData.GemPoint;
			}
			if (DoMirror)
			{
				num4 = 999.0;
				num5 = 20;
				if (val2 == Point3d.Unset)
				{
					val2 = val;
				}
				if (gemList.Count <= 1)
				{
					num4 = 0.0;
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
					while (num4 > 0.01 && num5 > 0)
					{
						num5--;
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
						CalculateGemXform(ultimateGemData2);
						val3 = ultimateGemData2.GemPoint - val2;
						double length3 = ((Vector3d)(ref val3)).Length;
						length3 = length3 - ultimateGemData2.SizeXRotated / 2.0 - num / 2.0;
						num4 = Math.Abs(length3 - ultimateGemData2.Spacing);
						if (!(num4 > 0.01))
						{
							continue;
						}
						double num7 = (ultimateGemData2.Spacing - length3) * (double)_direction;
						num2 -= num7 / length * 0.8;
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
			num = ultimateGemData.SizeXRotated;
			double num8 = num + ultimateGemData.Spacing;
			paramPosition += num8 / length * (double)_direction;
			num2 -= num8 / length * (double)_direction;
			if (!DoMirror && _crvIsClosed && gemList.Count >= 3)
			{
				Point3d gemPoint = gemList[0].GemPoint;
				val = gemList[gemList.Count - 1].GemPoint;
				val3 = gemPoint - val;
				if (((Vector3d)(ref val3)).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - gemList[gemList.Count - 1].Spacing < 0.0)
				{
					gemList.RemoveAt(gemList.Count - 1);
					flag = true;
				}
			}
		}
	}

	private UltimateGemData CalcGemSize(double paramPosition)
	{
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		UltimateGemData ultimateGemData = new UltimateGemData
		{
			NormParam = paramPosition
		};
		double num = (paramPosition - _positionStartGem) / (_positionEndGem - _positionStartGem);
		double zPlacement;
		double num2;
		double num3;
		double num4;
		double spacing;
		if (num <= _positionMiddle)
		{
			num /= _positionMiddle;
			zPlacement = _placementStart * (1.0 - num) + _placementMiddle * num;
			num2 = _xStart * (1.0 - num) + _xMiddle * num;
			num3 = _yStart * (1.0 - num) + _yMiddle * num;
			num4 = _gemRotatedStartXSize * (1.0 - num) + _gemRotatedMiddleXSize * num;
			spacing = _startGemSpacing * (1.0 - num) + _middleGemSpacing * num;
		}
		else
		{
			num = (num - _positionMiddle) / (1.0 - _positionMiddle);
			zPlacement = _placementMiddle * (1.0 - num) + _placementEnd * num;
			num2 = _xMiddle * (1.0 - num) + _xEnd * num;
			num3 = _yMiddle * (1.0 - num) + _yEnd * num;
			num4 = _gemRotatedMiddleXSize * (1.0 - num) + _gemRotatedEndXSize * num;
			spacing = _middleGemSpacing * (1.0 - num) + _endGemSpacing * num;
		}
		CSUtilGem.GetGemScaleFromTable(_gemShape, num2, num3, true, false, out Transform xBestScale);
		if (_gemShape.Duplicate() is CSGemData cSGemData)
		{
			cSGemData.ApplyTransform(xBestScale);
			double num5 = (cSGemData.GemXSize + cSGemData.GemYSize) / (num2 + num3);
			ultimateGemData.SizeX = cSGemData.GemXSize;
			ultimateGemData.SizeY = cSGemData.GemYSize;
			ultimateGemData.SizeZ = cSGemData.GemZSize;
			ultimateGemData.SizeXRotated = num4 * num5;
			ultimateGemData.ZPlacement = zPlacement;
			ultimateGemData.Spacing = spacing;
		}
		return ultimateGemData;
	}

	private void CalcLastLoopingGemPos(UltimateGemData FirstGem, UltimateGemData LastGem, double neededGemPos, out double paramPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(FirstGem.GemTransform);
		double length = _crv.GetLength();
		double num = 999.0;
		int num2 = 20;
		paramPosition = neededGemPos;
		bool flag = true;
		UltimateGemData ultimateGemData = null;
		while (num > 0.01 && num2 > 0)
		{
			num2--;
			ultimateGemData = new UltimateGemData
			{
				NormParam = paramPosition,
				SizeX = LastGem.SizeX,
				SizeY = LastGem.SizeY,
				SizeZ = LastGem.SizeZ,
				ZPlacement = LastGem.ZPlacement,
				Spacing = LastGem.Spacing,
				SizeXRotated = LastGem.SizeXRotated
			};
			CalculateGemXform(ultimateGemData);
			Vector3d val = ultimateGemData.GemPoint - origin;
			double length2 = ((Vector3d)(ref val)).Length;
			length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - FirstGem.SizeXRotated / 2.0;
			if (flag)
			{
				if (length2 - ultimateGemData.Spacing > 0.0)
				{
					paramPosition = neededGemPos;
					break;
				}
				flag = false;
				paramPosition = LastGem.NormParam;
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

	private void CalculateGemXform(UltimateGemData gemOnCurveInfo)
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
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
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
			double num4 = ProfileCut(_channelWallGemOverlap, _wallWidth, _profile, gemOnCurveInfo);
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
		Transform val8 = (_OrientationFollowCurve ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val6, val3, val4, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val6, val7, val3, brepNormal, val4));
		double num5 = 0.0;
		if (_vUseMinCuletDistance)
		{
			Vector3d val9 = val3 - val2;
			double length = ((Vector3d)(ref val9)).Length;
			if (cSGemData.GemZSize > length - _vMinCuletDistance - num3)
			{
				num5 = cSGemData.GemZSize - (length - _vMinCuletDistance - num3);
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
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, true, false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(gemShape);
		cSGemData.ApplyTransform(val * xBestScale);
		NurbsCurve nurbsCurve = cSGemData.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			xSizeOut = 0.0;
			return false;
		}
		Point3d val3;
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint val2 = nurbsCurve.Points[i];
			val3 = ((ControlPoint)(ref val2)).Location;
			double x = ((Point3d)(ref val3)).X;
			val3 = ((ControlPoint)(ref val2)).Location;
			((ControlPoint)(ref val2)).Location = new Point3d(x, ((Point3d)(ref val3)).Y, 0.0);
			nurbsCurve.Points[i] = val2;
		}
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		val3 = ((BoundingBox)(ref boundingBox)).Min;
		double y = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref boundingBox)).Min;
		double num = y2 - ((Point3d)(ref val3)).Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections val4 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), (Curve)(object)nurbsCurve, 0.001, 0.001);
			if (val4.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in val4)
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

	private void moveAGem(UltimateGemData gemPos, double positionStartGem, double endError, double gemRowLength)
	{
		double num = (gemPos.NormParam - positionStartGem) * endError / gemRowLength;
		gemPos.NormParam += num;
		CalculateGemXform(gemPos);
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

	private void UltGem(CSGemData gemDiameterStart, CSGemData gemDiameterMiddle, CSGemData gemDiameterEnd, bool vEnableDiameterEnd, bool vEnableDiameterMiddle, bool vEnablePlacementEnd, bool vEnablePlacementMiddle, bool vDoReverseDirection, ValueListItem moveGems, ValueListItem vMirrorMode, double rollAngle, double tiltAngle, out CSDataArray outputGems, out List<UltimateGemData> gemList)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		gemList = new List<UltimateGemData>();
		outputGems = new CSDataArray();
		Vector3d val;
		if (_profile != null)
		{
			BoundingBox boundingBox = ((GeometryBase)_profile).GetBoundingBox(true);
			val = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Diagonal;
			double y = ((Vector3d)(ref val)).Y;
			Point3d val2 = ((BoundingBox)(ref boundingBox)).Max;
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
		_xStart = gemDiameterStart.GemXSize;
		_xMiddle = gemDiameterMiddle.GemXSize;
		_xEnd = gemDiameterEnd.GemXSize;
		_yStart = gemDiameterStart.GemYSize;
		_yMiddle = gemDiameterMiddle.GemYSize;
		_yEnd = gemDiameterEnd.GemYSize;
		if (!vEnableDiameterEnd)
		{
			_xEnd = _xStart;
			_yEnd = _yStart;
		}
		if (!vEnableDiameterMiddle)
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
		_xRoll = Transform.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		_xTilt = Transform.Rotation(tiltAngle, Vector3d.XAxis, Point3d.Origin);
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
		_crvIsClosed = _crv.IsClosed;
		if (_crvIsClosed && vDoReverseDirection)
		{
			_positionStartInput += _direction;
			_direction = -_direction;
		}
		_crvLength = _crv.GetLength();
		if (_doMirror && _crv.IsClosed)
		{
			double num2 = _positionEndInput - _positionStartInput;
			if (num2 > 0.499)
			{
				_positionEndInput = _positionStartInput + 0.5;
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelStartSlopeLength = 0.0;
				_channelEndSlopeLength = 0.0;
			}
			else if (num2 < -0.499)
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
		if (!_doMirror)
		{
			CalcGemPos(positionStartGem, out gemList, out var _, DoMirror: false, MirrorModeEven: false);
			if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
			{
				AdjustGemPosEnd(ref gemList);
			}
		}
		else
		{
			List<UltimateGemData> gemList2 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror2 = new List<UltimateGemData>();
			double num3 = 999.0;
			double num4 = 999.0;
			if (vMirrorMode == ValueListItem.GemOnCrvMirrorEven || vMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(positionStartGem, out gemList2, out gemListMirror2, DoMirror: true, MirrorModeEven: true);
				if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(ref gemList2, ref gemListMirror2);
				}
				int count = gemList2.Count;
				if (count > 1)
				{
					val = gemList2[0].GemPoint - gemList2[1].GemPoint;
					num3 = ((Vector3d)(ref val)).Length - gemList2[0].SizeXRotated / 2.0 - gemList2[1].SizeXRotated / 2.0;
				}
				if (count == 1 && gemListMirror2.Count == 1)
				{
					val = gemList2[0].GemPoint - gemListMirror2[0].GemPoint;
					num3 = ((Vector3d)(ref val)).Length - gemList2[0].SizeXRotated / 2.0 - gemListMirror2[0].SizeXRotated / 2.0;
				}
				if (count >= 1)
				{
					num4 = Math.Abs(_positionEndGem - gemList2[count - 1].NormParam);
				}
				if (num3 < 0.0)
				{
					num3 = 999.0;
					num4 = 999.0;
				}
			}
			List<UltimateGemData> gemList3 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror3 = new List<UltimateGemData>();
			double num5 = 999.0;
			double num6 = 999.0;
			if (vMirrorMode == ValueListItem.GemOnCrvMirrorOdd || vMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(positionStartGem, out gemList3, out gemListMirror3, DoMirror: true, MirrorModeEven: false);
				if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(ref gemList3, ref gemListMirror3);
				}
				int count2 = gemList3.Count;
				if (count2 > 1)
				{
					val = gemList3[0].GemPoint - gemList3[1].GemPoint;
					num5 = ((Vector3d)(ref val)).Length - gemList3[0].SizeXRotated / 2.0 - gemList3[1].SizeXRotated / 2.0;
				}
				if (count2 >= 1)
				{
					num6 = Math.Abs(_positionEndGem - gemList3[count2 - 1].NormParam);
				}
			}
			List<UltimateGemData> gemListMirror;
			if (num6 < 0.01 && num4 < 0.01)
			{
				if (num5 <= num3)
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
			else if (num6 <= num4)
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
		List<Transform> list = gemList.Select((UltimateGemData gem) => gem.GemTransform).ToList();
		if (list == null)
		{
			return;
		}
		foreach (Transform item in list)
		{
			CSGemData data = InGemShape.Data as CSGemData;
			CSDataHelper.Transform(data, item);
			outputGems.Append(data);
		}
	}

	private static double ProfileCut(double channelWallGemOverlap, double wallWidth, Curve profile, UltimateGemData gemOnCurveInfo)
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
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve val = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve val2 = new LineCurve(new Point3d(sizeY / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform val3 = Transform.Scale(Plane.WorldXY, wallWidth * 2.0 + gemOnCurveInfo.SizeY, 1.0, 1.0);
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
}
