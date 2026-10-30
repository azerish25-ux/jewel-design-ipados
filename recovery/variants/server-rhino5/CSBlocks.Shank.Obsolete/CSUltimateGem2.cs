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
			double num = _positionEndInput - _positionStartInput;
			if (num > 0.499)
			{
				_positionEndInput = _positionStartInput + 0.5;
				_gemsClosedLoopSoNoChannelEnds = true;
				_channelSlopeLength = 0.0;
			}
			else if (num < -0.499)
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
			double num2 = 999.0;
			double num3 = 999.0;
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
					num2 = (gemList2[0].GemPoint - gemList2[1].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemList2[1].SizeXRotated / 2.0;
				}
				if (count == 1 && gemListMirror2.Count == 1)
				{
					num2 = (gemList2[0].GemPoint - gemListMirror2[0].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemListMirror2[0].SizeXRotated / 2.0;
				}
				if (count >= 1)
				{
					num3 = Math.Abs(_positionEndGem - gemList2[count - 1].NormParam);
				}
				if (num2 < 0.0)
				{
					num2 = 999.0;
					num3 = 999.0;
				}
			}
			List<UltimateGemData> gemList3 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror3 = new List<UltimateGemData>();
			double num4 = 999.0;
			double num5 = 999.0;
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
					num4 = (gemList3[0].GemPoint - gemList3[1].GemPoint).Length - gemList3[0].SizeXRotated / 2.0 - gemList3[1].SizeXRotated / 2.0;
				}
				if (count2 >= 1)
				{
					num5 = Math.Abs(_positionEndGem - gemList3[count2 - 1].NormParam);
				}
			}
			List<UltimateGemData> gemListMirror;
			if (num5 < 0.01 && num3 < 0.01)
			{
				if (num4 <= num2)
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
			else if (num5 <= num3)
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
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(ultimateGemData);
			Point3d origin = Point3d.Origin;
			origin.Transform(ultimateGemData.GemTransform);
			moveAGem(ultimateGemData, _positionStartGem, endError, vLen);
			Point3d origin2 = Point3d.Origin;
			origin2.Transform(ultimateGemData.GemTransform);
			double length = (origin2 - origin).Length;
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
			if ((gemPoint - gemPoint2).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - _gemSpacing * 0.8 < 0.0)
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
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		Point3d point3d = Point3d.Unset;
		Point3d point3d2 = Point3d.Unset;
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
					point3d = ultimateGemData2.GemPoint;
					point3d2 = point3d;
					num = ultimateGemData.SizeXRotated;
				}
				double length2 = (ultimateGemData.GemPoint - point3d).Length;
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
				point3d = ultimateGemData.GemPoint;
			}
			if (DoMirror)
			{
				num4 = 999.0;
				num5 = 20;
				if (point3d2 == Point3d.Unset)
				{
					point3d2 = point3d;
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
						double length3 = (ultimateGemData2.GemPoint - point3d2).Length;
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
						point3d2 = ultimateGemData2.GemPoint;
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
				point3d = gemList[gemList.Count - 1].GemPoint;
				if ((gemPoint - point3d).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - _gemSpacing < 0.0)
				{
					gemList.RemoveAt(gemList.Count - 1);
					flag = true;
				}
			}
		}
	}

	private UltimateGemData CalcGemSize(double paramPosition)
	{
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
			_crv.NormalizedLengthParameter(paramPosition, out var t);
			Point3d point3d = _crv.PointAt(t);
			_baseCrv.ClosestPoint(point3d, out var t2);
			Point3d point3d2 = _baseCrv.PointAt(t2);
			double length = (point3d - point3d2).Length;
			double num5 = length - _vMinCuletDistance;
			CSUtilGem.GetGemScaleFromTableWithMaxZ(_gemShape, num2, num3, num5, useLargestPossible: true, useIterationOnly: false, out xBestScale);
			if (_channelEnabled && _profile != null)
			{
				CSGemData cSGemData = _gemShape.Duplicate() as CSGemData;
				cSGemData.ApplyTransform(xBestScale);
				ultimateGemData.SizeY = cSGemData.GemYSize;
				double num6 = ProfileCut(_channelWallGemOverlap, _wallWidth, _profile, ultimateGemData);
				double num7 = length * (1.0 - num6);
				CSUtilGem.GetGemScaleFromTableWithMaxZ(_gemShape, num2, num3, num5 - num7, useLargestPossible: true, useIterationOnly: false, out xBestScale);
			}
		}
		else
		{
			CSUtilGem.GetGemScaleFromTable(_gemShape, num2, num3, true, false, out xBestScale);
		}
		if (_gemShape.Duplicate() is CSGemData cSGemData2)
		{
			cSGemData2.ApplyTransform(xBestScale);
			double num8 = (cSGemData2.GemXSize + cSGemData2.GemYSize) / (num2 + num3);
			ultimateGemData.SizeX = cSGemData2.GemXSize;
			ultimateGemData.SizeY = cSGemData2.GemYSize;
			ultimateGemData.SizeZ = cSGemData2.GemZSize;
			ultimateGemData.SizeXRotated = num4 * num8;
			ultimateGemData.ZPlacement = zPlacement;
			ultimateGemData.Spacing = _gemSpacing;
		}
		return ultimateGemData;
	}

	private void CalcLastLoopingGemPos(UltimateGemData FirstGem, UltimateGemData LastGem, double neededGemPos, out double paramPosition)
	{
		Point3d origin = Point3d.Origin;
		origin.Transform(FirstGem.GemTransform);
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
			double length2 = (ultimateGemData.GemPoint - origin).Length;
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
		}
		Point3d point3d2 = _crv.PointAt(gemOnCurveInfo.Param);
		Vector3d vector3d = _crv.TangentAt(gemOnCurveInfo.Param);
		double num2 = 0.0;
		if (_channelEnabled && _profile != null)
		{
			double num3 = ProfileCut(_channelWallGemOverlap, _wallWidth, _profile, gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, _crv, _baseCrv, out var pScale, out var pRail);
			num2 = (point3d2 - point3d).Length * (1.0 - num3);
			gemOnCurveInfo.ProfileCurve.Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Transform transform2 = Transform.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.ZPlacement - num2);
		Vector3d vector3d2 = (_doFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d vector3d3 = (_doFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Transform transform3;
		switch (_vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
		{
			Vector3d zAxis = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(_srf, point3d2, ref zAxis);
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d3, point3d2, zAxis, vector3d);
			break;
		}
		case ValueListItem.GemOrientationMethodOrigin:
		{
			Vector3d zAxis = new Vector3d(point3d2);
			zAxis.Unitize();
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d3, vector3d2, point3d2, vector3d, zAxis);
			break;
		}
		case ValueListItem.GemOrientationMethodAxisY:
		{
			Vector3d zAxis = new Vector3d(point3d2.X, 0.0, point3d2.Z);
			zAxis.Unitize();
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d3, vector3d2, point3d2, vector3d, zAxis);
			break;
		}
		case ValueListItem.GemOrientationMethodDown:
		{
			Vector3d zAxis = Vector3d.ZAxis;
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d3, point3d2, zAxis, vector3d);
			break;
		}
		case ValueListItem.GemOrientationMethodCurve:
		{
			Vector3d zAxis = ((point3d != Point3d.Unset) ? (point3d2 - point3d) : new Vector3d(point3d2));
			zAxis.Unitize();
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d3, point3d2, zAxis, vector3d);
			break;
		}
		default:
		{
			Vector3d zAxis = Vector3d.ZAxis;
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d3, point3d2, zAxis, vector3d);
			break;
		}
		}
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
		if (vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
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
		NurbsCurve nurbsCurve = cSGemData.GetNurbsCurve();
		if (nurbsCurve == null)
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

	private void moveAGem(UltimateGemData gemPos, double positionStartGem, double endError, double gemRowLength)
	{
		double num = (gemPos.NormParam - positionStartGem) * endError / gemRowLength;
		gemPos.NormParam += num;
		CalculateGemXform(gemPos);
	}

	private static double ProfileCut(double channelWallGemOverlap, double wallWidth, Curve profile, UltimateGemData gemOnCurveInfo)
	{
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve curveB = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve curveB2 = new LineCurve(new Point3d(sizeY / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform xform = Transform.Scale(Plane.WorldXY, wallWidth + gemOnCurveInfo.SizeY, 1.0, 1.0);
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

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = crv.PointAt(gem.Param);
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d point3d2 = baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = point3d - point3d2;
		double length = destination2.Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d, destination2, destination);
	}
}
