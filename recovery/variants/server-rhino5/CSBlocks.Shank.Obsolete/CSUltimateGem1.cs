using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.Obsolete;

[Obsolete]
[CSBlock("UltimateGem_V1", "{BD1E0322-31B1-4431-8D4D-C084593812B1}")]
public sealed class CSUltimateGem1 : CSBlock
{
	private sealed class GemInfoUltimateGem
	{
		public double SizeX;

		public double SizeY;

		public double SizeZ;

		public double NormParam;

		public double Param;

		public double BaseParam;

		public double ZPlacement;

		public double Spacing;

		public double SizeXRotated;

		public Transform GemTransform;

		public Transform ProfileTransform;

		public Curve ProfileCurve;

		public double ParamLeft;

		public double ParamRight;
	}

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

	private Curve _profile;

	private double _profileWidth;

	private bool _channelEnabled;

	private double _channelWallGemOverlap;

	private double _channelSlopeLength;

	private bool _gemsClosedLoop;

	public CSInputChannel InCurve { get; }

	public CSInputChannel InBaseCurve { get; }

	public CSInputChannel InSurface { get; }

	public CSInputChannel InGemShape { get; }

	public CSInputChannel InDiameterStart { get; }

	public CSInputChannel InDiameterMiddle { get; }

	public CSInputChannel InDiameterEnd { get; }

	public CSInputChannel InPositionStart { get; }

	public CSInputChannel InPositionEnd { get; }

	public CSInputChannel InPositionMiddle { get; }

	public CSInputChannel InPlacementStart { get; }

	public CSInputChannel InPlacementMiddle { get; }

	public CSInputChannel InPlacementEnd { get; }

	public CSInputChannel InSpacing { get; }

	public CSInputChannel InRoll { get; }

	public CSInputChannel InTilt { get; }

	public CSInputChannel InOrientationMethod { get; }

	public CSInputChannel InPlacementMethod { get; }

	public CSInputChannel InSpacingMethod { get; }

	public CSInputChannel InEnableDiameterMiddle { get; }

	public CSInputChannel InEnableDiameterEnd { get; }

	public CSInputChannel InEnablePlacementMiddle { get; }

	public CSInputChannel InEnablePlacementEnd { get; }

	public CSInputChannel InDoFlip { get; }

	public CSInputChannel InDoMirror { get; }

	public CSInputChannel InDoReverseDirection { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InProfileWidth { get; }

	public CSInputChannel InChannelEnabled { get; }

	public CSInputChannel InChannelWallGemOverlap { get; }

	public CSInputChannel InChannelStartDistance { get; }

	public CSInputChannel InMirrorMode { get; }

	public CSInputChannel InChannelClosed { get; }

	public CSOutputChannel OutGemXFormArray { get; }

	public CSOutputChannel OutShankBuilder { get; }

	public CSUltimateGem1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(GetPropertyName(() => InCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(GetPropertyName(() => InBaseCurve), this, ChannelDataType.Curve, ChannelArrayType.Any);
		InSurface = new CSInputChannel(GetPropertyName(() => InSurface), this, ChannelDataType.Brep, ChannelArrayType.Any);
		InGemShape = new CSInputChannel(GetPropertyName(() => InGemShape), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterStart = new CSInputChannel(GetPropertyName(() => InDiameterStart), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(GetPropertyName(() => InDiameterMiddle), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(GetPropertyName(() => InDiameterEnd), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InPositionStart = new CSInputChannel(GetPropertyName(() => InPositionStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPositionEnd = new CSInputChannel(GetPropertyName(() => InPositionEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPositionMiddle = new CSInputChannel(GetPropertyName(() => InPositionMiddle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacementStart = new CSInputChannel(GetPropertyName(() => InPlacementStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacementMiddle = new CSInputChannel(GetPropertyName(() => InPlacementMiddle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacementEnd = new CSInputChannel(GetPropertyName(() => InPlacementEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacing = new CSInputChannel(GetPropertyName(() => InSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRoll = new CSInputChannel(GetPropertyName(() => InRoll), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InTilt = new CSInputChannel(GetPropertyName(() => InTilt), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOrientationMethod = new CSInputChannel(GetPropertyName(() => InOrientationMethod), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InPlacementMethod = new CSInputChannel(GetPropertyName(() => InPlacementMethod), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InSpacingMethod = new CSInputChannel(GetPropertyName(() => InSpacingMethod), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InEnableDiameterMiddle = new CSInputChannel(GetPropertyName(() => InEnableDiameterMiddle), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableDiameterEnd = new CSInputChannel(GetPropertyName(() => InEnableDiameterEnd), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnablePlacementMiddle = new CSInputChannel(GetPropertyName(() => InEnablePlacementMiddle), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnablePlacementEnd = new CSInputChannel(GetPropertyName(() => InEnablePlacementEnd), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoFlip = new CSInputChannel(GetPropertyName(() => InDoFlip), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoMirror = new CSInputChannel(GetPropertyName(() => InDoMirror), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoReverseDirection = new CSInputChannel(GetPropertyName(() => InDoReverseDirection), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(GetPropertyName(() => InProfile), this, ChannelDataType.Curve, ChannelArrayType.Any);
		InProfileWidth = new CSInputChannel(GetPropertyName(() => InProfileWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEnabled = new CSInputChannel(GetPropertyName(() => InChannelEnabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InChannelWallGemOverlap = new CSInputChannel(GetPropertyName(() => InChannelWallGemOverlap), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelStartDistance = new CSInputChannel(GetPropertyName(() => InChannelStartDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMirrorMode = new CSInputChannel(GetPropertyName(() => InMirrorMode), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InChannelClosed = new CSInputChannel(GetPropertyName(() => InChannelClosed), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutGemXFormArray = new CSOutputChannel(GetPropertyName(() => OutGemXFormArray), this, ChannelDataType.Any, ChannelArrayType.Any);
		OutShankBuilder = new CSOutputChannel(GetPropertyName(() => OutShankBuilder), this, ChannelDataType.Any, ChannelArrayType.Any);
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
		CSDataHelper.GetCurve(InProfile, out _profile);
		_channelEnabled = false;
		InChannelEnabled.GetBoolean(ref _channelEnabled);
		InProfileWidth.GetDouble(ref _profileWidth);
		InChannelWallGemOverlap.GetDouble(ref _channelWallGemOverlap);
		InChannelStartDistance.GetDouble(ref _channelSlopeLength);
		_gemsClosedLoop = false;
		InChannelClosed.GetBoolean(ref _gemsClosedLoop);
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
		if (!CalculateRotateTestGem(_xStart, _yStart, _xRoll, out _gemRotatedStartXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(_xMiddle, _yMiddle, _xRoll, out _gemRotatedMiddleXSize))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(_xEnd, _yEnd, _xRoll, out _gemRotatedEndXSize))
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
		if (_doMirror)
		{
			double num = _positionEndInput - _positionStartInput;
			if (num > 0.499)
			{
				_positionEndInput = _positionStartInput + 0.5 - _gemSpacing / _crvLength / 2.0;
				_gemsClosedLoop = true;
				_channelSlopeLength = 0.0;
			}
			else if (num < -0.499)
			{
				_positionEndInput = _positionStartInput - 0.5 + _gemSpacing / _crvLength / 2.0;
				_gemsClosedLoop = true;
				_channelSlopeLength = 0.0;
			}
		}
		_positionStartGem = _positionStartInput;
		if (!_doMirror && _channelEnabled)
		{
			_positionStartGem += (_xStart / 2.0 + _channelSlopeLength) / _crvLength * (double)_direction;
		}
		_positionEndGem = _positionEndInput;
		if (_channelEnabled || _gemsClosedLoop)
		{
			_positionEndGem -= (_xEnd / 2.0 + _channelSlopeLength) / _crvLength * (double)_direction;
		}
		double positionStartGem = _positionStartGem;
		double endpos = double.NaN;
		List<GemInfoUltimateGem> gemList = new List<GemInfoUltimateGem>();
		if (!_doMirror || whichItem == ValueListItem.GemOnCrvMirrorOdd || whichItem == ValueListItem.GemOnCrvMirrorOddAndEven)
		{
			CalcGemPos(positionStartGem, ref gemList, out endpos);
		}
		double vRealEndPos = endpos;
		if (_doMirror)
		{
			positionStartGem = _positionStartGem + (_gemRotatedStartXSize + _gemSpacing) / 2.0 / _crvLength;
			double endpos2 = double.NaN;
			List<GemInfoUltimateGem> gemList2 = new List<GemInfoUltimateGem>();
			if (whichItem == ValueListItem.GemOnCrvMirrorEven || whichItem == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(positionStartGem, ref gemList2, out endpos2);
			}
			bool flag;
			if (double.IsNaN(endpos) || (!double.IsNaN(endpos2) && Math.Abs(endpos2 - _positionEndGem) < Math.Abs(endpos - _positionEndGem)))
			{
				gemList = gemList2;
				flag = false;
				vRealEndPos = endpos2;
				positionStartGem = _positionStartGem - (_gemRotatedStartXSize + _gemSpacing) / 2.0 / _crvLength;
			}
			else
			{
				flag = true;
				vRealEndPos = endpos;
				positionStartGem = _positionStartGem;
			}
			int count = gemList.Count;
			CalcGemPos(positionStartGem, ref gemList, out var _);
			List<GemInfoUltimateGem> list = new List<GemInfoUltimateGem>();
			int num2 = (flag ? (count + 1) : count);
			for (int num3 = gemList.Count - 1; num3 >= num2; num3--)
			{
				list.Add(gemList[num3]);
			}
			for (int i = 0; i < count; i++)
			{
				list.Add(gemList[i]);
			}
			gemList = list;
		}
		if (_vSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
		{
			AdjustGemPosEnd(vRealEndPos, ref gemList);
		}
		CSDataArray cSDataArray = new CSDataArray();
		foreach (GemInfoUltimateGem item in gemList)
		{
			cSDataArray.Append(new CSXformData(item.GemTransform));
		}
		OutGemXFormArray.SetData(cSDataArray);
		CSDataArray cSDataArray2 = new CSDataArray();
		foreach (GemInfoUltimateGem item2 in gemList)
		{
			CSDataArray cSDataArray3 = new CSDataArray();
			cSDataArray3.Append(new CSNumberData(item2.SizeX));
			cSDataArray3.Append(new CSNumberData(item2.SizeY));
			cSDataArray3.Append(new CSNumberData(item2.SizeZ));
			cSDataArray3.Append(new CSNumberData(item2.NormParam));
			cSDataArray3.Append(new CSNumberData(item2.Param));
			cSDataArray3.Append(new CSNumberData(item2.BaseParam));
			cSDataArray3.Append(new CSXformData(item2.ProfileTransform));
			cSDataArray3.Append(CSOnGeometryData.FromCurve(item2.ProfileCurve));
			cSDataArray3.Append(new CSNumberData(item2.ParamLeft));
			cSDataArray3.Append(new CSNumberData(item2.ParamRight));
			cSDataArray2.Append(cSDataArray3);
		}
		CSDataArray cSDataArray4 = new CSDataArray();
		cSDataArray4.Append(cSDataArray2);
		cSDataArray4.Append(new CSNumberData(_yStart));
		cSDataArray4.Append(new CSNumberData(_yEnd));
		cSDataArray4.Append(new CSNumberData(_positionStartInput));
		cSDataArray4.Append(new CSNumberData(_positionMiddle));
		cSDataArray4.Append(new CSNumberData(_positionEndInput));
		cSDataArray4.Append(new CSNumberData(_channelEnabled ? 1 : 0));
		cSDataArray4.Append(new CSNumberData(_channelSlopeLength));
		cSDataArray4.Append(new CSNumberData(_gemsClosedLoop ? 1 : 0));
		cSDataArray4.Append(new CSNumberData(_direction));
		OutShankBuilder.SetData(cSDataArray4);
		return base.Update();
	}

	private void AdjustGemPosEnd(double vRealEndPos, ref List<GemInfoUltimateGem> gemList)
	{
		if (gemList.Count <= 1)
		{
			return;
		}
		double num = _positionEndGem - vRealEndPos;
		double num2 = vRealEndPos - _positionStartGem;
		for (int i = 0; i < gemList.Count; i++)
		{
			GemInfoUltimateGem gemInfoUltimateGem = gemList[i];
			double num3 = (gemInfoUltimateGem.NormParam - _positionStartGem) * num / num2;
			gemInfoUltimateGem.NormParam += num3;
			if (!_crvIsClosed)
			{
				double num4 = (_channelEnabled ? ((gemInfoUltimateGem.SizeX / 2.0 + _channelSlopeLength) / _crvLength) : 0.0);
				if (gemInfoUltimateGem.NormParam < num4 || gemInfoUltimateGem.NormParam > 1.0 - num4)
				{
					gemList.RemoveAt(i);
					i--;
				}
			}
			CalculateGemXform(gemInfoUltimateGem);
		}
	}

	private void CalcGemPos(double paramPosition, ref List<GemInfoUltimateGem> gemList, out double endpos)
	{
		Point3d point3d = Point3d.Unset;
		double length = _crv.GetLength();
		bool flag = gemList.Count > 0;
		int count = gemList.Count;
		int i;
		for (i = 0; (!flag && paramPosition * (double)_direction < _positionEndGem * (double)_direction) || (flag && i < count); i++)
		{
			double num = 999.0;
			int num2 = 50;
			GemInfoUltimateGem gemInfoUltimateGem = null;
			while (num > 0.01 && num2 > 0)
			{
				num2--;
				if (flag)
				{
					GemInfoUltimateGem gemInfoUltimateGem2 = gemList[i];
					if (!_crvIsClosed)
					{
						double num3 = (_channelEnabled ? ((gemInfoUltimateGem2.SizeX / 2.0 + _channelSlopeLength) / _crvLength) : 0.0);
						if (paramPosition < num3 || paramPosition > 1.0 - num3)
						{
							endpos = ((gemList.Count > 0) ? gemList[gemList.Count - 1].NormParam : paramPosition);
							return;
						}
					}
					gemInfoUltimateGem = new GemInfoUltimateGem
					{
						NormParam = paramPosition,
						SizeX = gemInfoUltimateGem2.SizeX,
						SizeY = gemInfoUltimateGem2.SizeY,
						SizeZ = gemInfoUltimateGem2.SizeZ,
						ZPlacement = gemInfoUltimateGem2.ZPlacement,
						Spacing = gemInfoUltimateGem2.Spacing,
						SizeXRotated = gemInfoUltimateGem2.SizeXRotated
					};
				}
				else
				{
					gemInfoUltimateGem = new GemInfoUltimateGem
					{
						NormParam = paramPosition
					};
					double num4 = (paramPosition - _positionStartGem) / (_positionEndGem - _positionStartGem);
					double zPlacement;
					double requestedSizeX;
					double requestedSizeY;
					double sizeXRotated;
					if (num4 <= _positionMiddle)
					{
						num4 /= _positionMiddle;
						zPlacement = _placementStart * (1.0 - num4) + _placementMiddle * num4;
						requestedSizeX = _xStart * (1.0 - num4) + _xMiddle * num4;
						requestedSizeY = _yStart * (1.0 - num4) + _yMiddle * num4;
						sizeXRotated = _gemRotatedStartXSize * (1.0 - num4) + _gemRotatedMiddleXSize * num4;
					}
					else
					{
						num4 = (num4 - _positionMiddle) / (1.0 - _positionMiddle);
						zPlacement = _placementMiddle * (1.0 - num4) + _placementEnd * num4;
						requestedSizeX = _xMiddle * (1.0 - num4) + _xEnd * num4;
						requestedSizeY = _yMiddle * (1.0 - num4) + _yEnd * num4;
						sizeXRotated = _gemRotatedMiddleXSize * (1.0 - num4) + _gemRotatedEndXSize * num4;
					}
					CSUtilGem.GetGemScaleFromTable(_gemShape, requestedSizeX, requestedSizeY, true, false, out Transform xBestScale);
					if (_gemShape.Duplicate() is CSGemData cSGemData)
					{
						cSGemData.ApplyTransform(xBestScale);
						gemInfoUltimateGem.SizeX = cSGemData.GemXSize;
						gemInfoUltimateGem.SizeY = cSGemData.GemYSize;
						gemInfoUltimateGem.SizeZ = cSGemData.GemZSize;
						gemInfoUltimateGem.SizeXRotated = sizeXRotated;
						gemInfoUltimateGem.ZPlacement = zPlacement;
						gemInfoUltimateGem.Spacing = _gemSpacing;
					}
				}
				CalculateGemXform(gemInfoUltimateGem);
				if (point3d == Point3d.Unset)
				{
					num = 0.0;
					continue;
				}
				Point3d point3d2 = new Point3d(0.0, 0.0, 0.0);
				point3d2.Transform(gemInfoUltimateGem.GemTransform);
				double length2 = (point3d2 - point3d).Length;
				length2 = length2 - gemInfoUltimateGem.SizeXRotated / 2.0 - gemList[i - 1].SizeXRotated / 2.0;
				num = Math.Abs(length2 - gemInfoUltimateGem.Spacing);
				if (num > 0.01)
				{
					double num5 = (_gemSpacing - length2) * (double)_direction;
					if (flag)
					{
						num5 = 0.0 - num5;
					}
					paramPosition += num5 / length * 0.8;
				}
			}
			if (gemInfoUltimateGem != null)
			{
				gemList.Add(gemInfoUltimateGem);
				point3d = new Point3d(0.0, 0.0, 0.0);
				point3d.Transform(gemInfoUltimateGem.GemTransform);
			}
			double num6 = _gemRotatedEndXSize + _gemSpacing;
			paramPosition = ((!flag) ? (paramPosition + num6 / length * (double)_direction) : (paramPosition - num6 / length * (double)_direction));
		}
		endpos = ((gemList.Count > 0) ? gemList[i - 1].NormParam : paramPosition);
	}

	private void CalculateGemXform(GemInfoUltimateGem gemOnCurveInfo)
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
		CalculatePointsOnCrv(gemOnCurveInfo);
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
			double num3 = ProfileCut(gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, out var pScale, out var pRail);
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

	private void CalculatePointsOnCrv(GemInfoUltimateGem gem)
	{
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		_crv.NormalizedLengthParameter(num, out gem.Param);
		Point3d point3d = _crv.PointAt(gem.Param);
		Vector3d normal = _crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (_baseCrv == null)
		{
			return;
		}
		if (_vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
		{
			_baseCrv.ClosestPoint(point3d, out gem.BaseParam);
			return;
		}
		CurveIntersections obj = Intersection.CurvePlane(plane: new Plane(point3d, normal), curve: _baseCrv, tolerance: 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in obj)
		{
			double length = (item.PointA - point3d).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private bool CalculateRotateTestGem(double xSize, double ySize, Transform xform, out double xSizeOut)
	{
		if (!CSUtilGem.GetGemScaleFromTable(_gemShape, xSize, ySize, true, false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(_gemShape);
		cSGemData.ApplyTransform(xform * xBestScale);
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

	private double ProfileCut(GemInfoUltimateGem gemOnCurveInfo)
	{
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve curveB = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + _channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + _channelWallGemOverlap, -2.0, 0.0));
		LineCurve curveB2 = new LineCurve(new Point3d(sizeY / 2.0 - _channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - _channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = _profile.DuplicateCurve();
		Transform xform = Transform.Scale(Plane.WorldXY, _profileWidth + gemOnCurveInfo.SizeY, 1.0, 1.0);
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

	private void ProfilePositionScale(GemInfoUltimateGem gem, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = _crv.PointAt(gem.Param);
		Vector3d destination = _crv.TangentAt(gem.Param);
		Point3d point3d2 = _baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = point3d - point3d2;
		double length = destination2.Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d, destination2, destination);
	}
}
