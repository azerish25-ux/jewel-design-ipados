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
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.Obsolete;

[Obsolete]
[CSBlock("UltimateGem_V2", "{3DEB064C-EC80-4B47-89D1-68EF2527AF63}")]
public sealed class CSUltimateGem2 : CSBlock
{
	private Curve _crv;

	private double _crvLength;

	private bool _crvIsClosed;

	private Curve _baseCrv;

	private Brep _srf;

	private CSGemData _gemShape;

	private Transform _xRoll;

	private Transform _xTilt;

	private ValueListItem _vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;

	private ValueListItem _vPlacementMethod = ValueListItem.GemPlacementMethodGirdle;

	private ValueListItem _vSpacingMethod = ValueListItem.GemSpacingMethodMinimum;

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

	private double _gemSpacing;

	private double _gemSpacingTollerance;

	private Curve _profile;

	private double _wallWidth;

	private bool _channelEnabled;

	private double _channelWallGemOverlap;

	private double _channelSlopeLength;

	private bool _gemsClosedLoopSoNoChannelEnds;

	private bool _vUseMinCuletDistance;

	private double _vMinCuletDistance;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseCurve;

	public readonly CSInputChannel InSurface;

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

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InSpacingTollerance;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InOrientationMethod;

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

	public readonly CSInputChannel InChannelWallWidth;

	public readonly CSInputChannel InChannelEnabled;

	public readonly CSInputChannel InChannelWallGemOverlap;

	public readonly CSInputChannel InChannelStartDistance;

	public readonly CSInputChannel InMirrorMode;

	public readonly CSInputChannel InChannelClosed;

	public readonly CSInputChannel InUseMinCuletDistance;

	public readonly CSInputChannel InMinCuletDistance;

	public readonly CSOutputChannel OutGemXFormArray;

	public readonly CSOutputChannel OutGems;

	public readonly CSOutputChannel OutChannelYStart;

	public readonly CSOutputChannel OutChannelYEnd;

	public readonly CSOutputChannel OutPositionStart;

	public readonly CSOutputChannel OutPositionMiddle;

	public readonly CSOutputChannel OutPositionEnd;

	public readonly CSOutputChannel OutChannelEnabled;

	public readonly CSOutputChannel OutChannelSlopeLength;

	public readonly CSOutputChannel OutGemsClosedLoop;

	public readonly CSOutputChannel OutDirection;

	public CSUltimateGem2(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InSurface = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Any);
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
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacingTollerance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
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
		InChannelWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelStartDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMirrorMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InChannelClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InUseMinCuletDistance = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InMinCuletDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemXFormArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGems = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutChannelYStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelYEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionMiddle = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEnabled = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutChannelSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemsClosedLoop = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutDirection = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		if (!CSDataHelper.GetCurve(InCurve, out _crv))
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetCurve(InBaseCurve, out _baseCrv);
		CSDataHelper.GetBrep(InSurface, out _srf);
		_gemShape = InGemShape.Data as CSGemData;
		if (_gemShape == null)
		{
			return UpdateMissing("Gem Shape");
		}
		if (!(InDiameterStart.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Start Diameter");
		}
		if (!(InDiameterMiddle.Data is CSGemData cSGemData2))
		{
			return UpdateMissing("Middle Diameter");
		}
		if (!(InDiameterEnd.Data is CSGemData cSGemData3))
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
		_gemSpacing = 0.0;
		InSpacing.GetDouble(ref _gemSpacing);
		_gemSpacingTollerance = 0.0;
		InSpacingTollerance.GetDouble(ref _gemSpacingTollerance);
		double value = 0.0;
		InRoll.GetDouble(ref value);
		value = CSBlockUtil.CSUtilMath.ToRadians(value);
		double value2 = 0.0;
		InTilt.GetDouble(ref value2);
		value2 = CSBlockUtil.CSUtilMath.ToRadians(value2);
		_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		InOrientationMethod.GetValueListItem(ref _vOrientationMethod);
		_vPlacementMethod = ValueListItem.GemPlacementMethodGirdle;
		InPlacementMethod.GetValueListItem(ref _vPlacementMethod);
		_vSpacingMethod = ValueListItem.GemSpacingMethodMinimum;
		InSpacingMethod.GetValueListItem(ref _vSpacingMethod);
		bool value3 = false;
		InEnableDiameterMiddle.GetBoolean(ref value3);
		bool value4 = false;
		InEnableDiameterEnd.GetBoolean(ref value4);
		bool value5 = false;
		InEnablePlacementMiddle.GetBoolean(ref value5);
		bool value6 = false;
		InEnablePlacementEnd.GetBoolean(ref value6);
		_doFlip = false;
		InDoFlip.GetBoolean(ref _doFlip);
		_doMirror = false;
		InDoMirror.GetBoolean(ref _doMirror);
		ValueListItem whichItem = ValueListItem.GemOnCrvMirrorOddAndEven;
		InMirrorMode.GetValueListItem(ref whichItem);
		bool value7 = false;
		InDoReverseDirection.GetBoolean(ref value7);
		_vUseMinCuletDistance = false;
		InUseMinCuletDistance.GetBoolean(ref _vUseMinCuletDistance);
		_vMinCuletDistance = 0.0;
		InMinCuletDistance.GetDouble(ref _vMinCuletDistance);
		CSDataHelper.GetCurve(InProfile, out _profile);
		_channelEnabled = false;
		InChannelEnabled.GetBoolean(ref _channelEnabled);
		InChannelWallWidth.GetDouble(ref _wallWidth);
		InChannelWallGemOverlap.GetDouble(ref _channelWallGemOverlap);
		InChannelStartDistance.GetDouble(ref _channelSlopeLength);
		_gemsClosedLoopSoNoChannelEnds = false;
		InChannelClosed.GetBoolean(ref _gemsClosedLoopSoNoChannelEnds);
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
		if (_srf == null && _vOrientationMethod == ValueListItem.GemOrientationMethodSurface)
		{
			_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		}
		if (_baseCrv == null && _vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
		{
			_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		}
		_xStart = cSGemData.GemXSize;
		_xMiddle = cSGemData2.GemXSize;
		_xEnd = cSGemData3.GemXSize;
		_yStart = cSGemData.GemYSize;
		_yMiddle = cSGemData2.GemYSize;
		_yEnd = cSGemData3.GemYSize;
		if (!value4)
		{
			_xEnd = _xStart;
			_yEnd = _yStart;
		}
		if (!value3)
		{
			_xMiddle = _xStart + (_xEnd - _xStart) * _positionMiddle;
			_yMiddle = _yStart + (_yEnd - _yStart) * _positionMiddle;
		}
		if (!value6)
		{
			_placementEnd = _placementStart;
		}
		if (!value5)
		{
			_placementMiddle = _placementStart + (_placementEnd - _placementStart) * _positionMiddle;
		}
		_xRoll = CSBlockUtil.CSUtilMath.Rotation(value, Vector3d.ZAxis, Point3d.Origin);
		_xTilt = CSBlockUtil.CSUtilMath.Rotation(value2, Vector3d.XAxis, Point3d.Origin);
		if (!CalculateRotateTestGem(_gemShape, _xStart, _yStart, value, out _gemRotatedStartXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(_gemShape, _xMiddle, _yMiddle, value, out _gemRotatedMiddleXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(_gemShape, _xEnd, _yEnd, value, out _gemRotatedEndXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		_direction = ((!(_positionStartInput > _positionEndInput)) ? 1 : (-1));
		_crvIsClosed = _crv.IsClosed;
		if (_crvIsClosed && value7)
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
				_channelSlopeLength = 0.0;
			}
			else if (num2 < -0.499)
			{
				_positionEndInput = _positionStartInput - 0.5;
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelSlopeLength = 0.0;
			}
		}
		_positionStartGem = _positionStartInput;
		if (!_doMirror && _channelEnabled)
		{
			_positionStartGem += (_xStart / 2.0 + _channelSlopeLength) / _crvLength * (double)_direction;
		}
		_positionEndGem = _positionEndInput;
		if (_channelEnabled)
		{
			_positionEndGem -= (_xEnd / 2.0 + _channelSlopeLength) / _crvLength * (double)_direction;
		}
		double positionStartGem = _positionStartGem;
		List<UltimateGemData> gemList;
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
			if (whichItem == ValueListItem.GemOnCrvMirrorEven || whichItem == ValueListItem.GemOnCrvMirrorOddAndEven)
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
			if (whichItem == ValueListItem.GemOnCrvMirrorOdd || whichItem == ValueListItem.GemOnCrvMirrorOddAndEven)
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
		CSDataArray cSDataArray = new CSDataArray();
		foreach (UltimateGemData item in gemList)
		{
			cSDataArray.Append(new CSXformData(item.GemTransform));
		}
		OutGemXFormArray.SetData(cSDataArray);
		CSDataArray cSDataArray2 = new CSDataArray();
		foreach (UltimateGemData item2 in gemList)
		{
			cSDataArray2.Append(item2.AddToArray());
		}
		OutGems.SetData(cSDataArray2);
		OutChannelYStart.SetData(new CSNumberData(_yStart));
		OutChannelYEnd.SetData(new CSNumberData(_yEnd));
		OutPositionStart.SetData(new CSNumberData(_positionStartInput));
		OutPositionMiddle.SetData(new CSNumberData(_positionMiddle));
		OutPositionEnd.SetData(new CSNumberData(_positionEndInput));
		OutChannelEnabled.SetData(new CSNumberData(_channelEnabled ? 1 : 0));
		OutChannelSlopeLength.SetData(new CSNumberData(_channelSlopeLength));
		OutGemsClosedLoop.SetData(new CSNumberData(_gemsClosedLoopSoNoChannelEnds ? 1 : 0));
		OutDirection.SetData(new CSNumberData(_direction));
		return base.Update();
	}

	private void AdjustEndErrorSoGemSpacingLessGemTollerance(List<UltimateGemData> gemList, double vLen, ref double endError)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
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
			if (length > _gemSpacingTollerance)
			{
				endError *= _gemSpacingTollerance / length;
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
		AdjustEndErrorSoGemSpacingLessGemTollerance(gemList, num2, ref endError);
		foreach (UltimateGemData gem in gemList)
		{
			moveAGem(gem, _positionStartGem, endError, num2);
		}
	}

	private void AdjustGemPosEndMirror(ref List<UltimateGemData> gemList, ref List<UltimateGemData> gemListMirror)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count <= 1 && gemListMirror.Count <= 0)
		{
			return;
		}
		double positionEndGem = _positionEndGem;
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = positionEndGem - normParam;
		double num = normParam - _positionStartGem;
		AdjustEndErrorSoGemSpacingLessGemTollerance(gemList, num, ref endError);
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
			if (((Vector3d)(ref val)).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - _gemSpacing * 0.8 < 0.0)
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
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
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
				double num6 = (_gemSpacing - length2) * (double)_direction;
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
						double num7 = (_gemSpacing - length3) * (double)_direction;
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
			double num8 = num + _gemSpacing;
			paramPosition += num8 / length * (double)_direction;
			num2 -= num8 / length * (double)_direction;
			if (!DoMirror && _crvIsClosed && gemList.Count >= 3)
			{
				Point3d gemPoint = gemList[0].GemPoint;
				val = gemList[gemList.Count - 1].GemPoint;
				val3 = gemPoint - val;
				if (((Vector3d)(ref val3)).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - _gemSpacing < 0.0)
				{
					gemList.RemoveAt(gemList.Count - 1);
					flag = true;
				}
			}
		}
	}

	private UltimateGemData CalcGemSize(double paramPosition)
	{
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		UltimateGemData ultimateGemData = new UltimateGemData
		{
			NormParam = paramPosition
		};
		double num = (paramPosition - _positionStartGem) / (_positionEndGem - _positionStartGem);
		double zPlacement;
		double num2;
		double num3;
		double num4;
		if (num <= _positionMiddle)
		{
			num /= _positionMiddle;
			zPlacement = _placementStart * (1.0 - num) + _placementMiddle * num;
			num2 = _xStart * (1.0 - num) + _xMiddle * num;
			num3 = _yStart * (1.0 - num) + _yMiddle * num;
			num4 = _gemRotatedStartXSize * (1.0 - num) + _gemRotatedMiddleXSize * num;
		}
		else
		{
			num = (num - _positionMiddle) / (1.0 - _positionMiddle);
			zPlacement = _placementMiddle * (1.0 - num) + _placementEnd * num;
			num2 = _xMiddle * (1.0 - num) + _xEnd * num;
			num3 = _yMiddle * (1.0 - num) + _yEnd * num;
			num4 = _gemRotatedMiddleXSize * (1.0 - num) + _gemRotatedEndXSize * num;
		}
		Transform xBestScale;
		if (_vUseMinCuletDistance && _baseCrv != null)
		{
			double num5 = default(double);
			_crv.NormalizedLengthParameter(paramPosition, ref num5);
			Point3d val = _crv.PointAt(num5);
			double num6 = default(double);
			_baseCrv.ClosestPoint(val, ref num6);
			Point3d val2 = _baseCrv.PointAt(num6);
			Vector3d val3 = val - val2;
			double length = ((Vector3d)(ref val3)).Length;
			double num7 = length - _vMinCuletDistance;
			CSUtilGem.GetGemScaleFromTableWithMaxZ(_gemShape, num2, num3, num7, useLargestPossible: true, useIterationOnly: false, out xBestScale);
			if (_channelEnabled && _profile != null)
			{
				CSGemData cSGemData = _gemShape.Duplicate() as CSGemData;
				cSGemData.ApplyTransform(xBestScale);
				ultimateGemData.SizeY = cSGemData.GemYSize;
				double num8 = ProfileCut(_channelWallGemOverlap, _wallWidth, _profile, ultimateGemData);
				double num9 = length * (1.0 - num8);
				CSUtilGem.GetGemScaleFromTableWithMaxZ(_gemShape, num2, num3, num7 - num9, useLargestPossible: true, useIterationOnly: false, out xBestScale);
			}
		}
		else
		{
			CSUtilGem.GetGemScaleFromTable(_gemShape, num2, num3, useLargestPossible: true, useIterationOnly: false, out xBestScale);
		}
		if (_gemShape.Duplicate() is CSGemData cSGemData2)
		{
			cSGemData2.ApplyTransform(xBestScale);
			double num10 = (cSGemData2.GemXSize + cSGemData2.GemYSize) / (num2 + num3);
			ultimateGemData.SizeX = cSGemData2.GemXSize;
			ultimateGemData.SizeY = cSGemData2.GemYSize;
			ultimateGemData.SizeZ = cSGemData2.GemZSize;
			ultimateGemData.SizeXRotated = num4 * num10;
			ultimateGemData.ZPlacement = zPlacement;
			ultimateGemData.Spacing = _gemSpacing;
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
					double num3 = (_gemSpacing - length2) * (double)_direction;
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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
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
		}
		Point3d val3 = _crv.PointAt(gemOnCurveInfo.Param);
		Vector3d val4 = _crv.TangentAt(gemOnCurveInfo.Param);
		double num3 = 0.0;
		if (_channelEnabled && _profile != null)
		{
			double num4 = ProfileCut(_channelWallGemOverlap, _wallWidth, _profile, gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, _crv, _baseCrv, out var pScale, out var pRail);
			Vector3d val5 = val3 - val2;
			num3 = ((Vector3d)(ref val5)).Length * (1.0 - num4);
			((GeometryBase)gemOnCurveInfo.ProfileCurve).Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Transform val6 = Transform.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.ZPlacement - num3);
		Vector3d val7 = (_doFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d val8 = (_doFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal = default(Vector3d);
		Transform val9;
		switch (_vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(_srf, val3, ref brepNormal);
			val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val8, val3, brepNormal, val4);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			((Vector3d)(ref brepNormal))._002Ector(val3);
			((Vector3d)(ref brepNormal)).Unitize();
			val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val8, val7, val3, val4, brepNormal);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			((Vector3d)(ref brepNormal))._002Ector(((Point3d)(ref val3)).X, 0.0, ((Point3d)(ref val3)).Z);
			((Vector3d)(ref brepNormal)).Unitize();
			val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val8, val7, val3, val4, brepNormal);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val8, val3, brepNormal, val4);
			break;
		case ValueListItem.GemOrientationMethodCurve:
			brepNormal = (Vector3d)((val2 != Point3d.Unset) ? (val3 - val2) : new Vector3d(val3));
			((Vector3d)(ref brepNormal)).Unitize();
			val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val8, val3, brepNormal, val4);
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val8, val3, brepNormal, val4);
			break;
		}
		gemOnCurveInfo.GemTransform = val9 * val6 * _xRoll * _xTilt * val;
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem vOrientationMethod, UltimateGemData gem)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
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
		if (vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
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
		Transform val = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, useLargestPossible: true, useIterationOnly: false, out Transform xBestScale))
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
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve val = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve val2 = new LineCurve(new Point3d(sizeY / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform val3 = Transform.Scale(Plane.WorldXY, wallWidth + gemOnCurveInfo.SizeY, 1.0, 1.0);
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

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, out Transform pScale, out Transform pRail)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = crv.PointAt(gem.Param);
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d val2 = baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = val - val2;
		double length = ((Vector3d)(ref destination2)).Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val, destination2, destination);
	}
}
