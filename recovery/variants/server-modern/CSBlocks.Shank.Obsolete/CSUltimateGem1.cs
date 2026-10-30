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
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
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
			BoundingBox boundingBox = ((GeometryBase)_profile).GetBoundingBox(true);
			Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double y = ((Vector3d)(ref diagonal)).Y;
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double x2 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = (0.0 - (x2 + ((Point3d)(ref val)).X)) / 2.0;
			val = ((BoundingBox)(ref boundingBox)).Min;
			Transform val2 = Transform.Translation(num, 0.0 - ((Point3d)(ref val)).Y, 0.0);
			((GeometryBase)_profile).Transform(val2);
			Transform val3 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			((GeometryBase)_profile).Transform(val3);
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
			double num2 = _positionEndInput - _positionStartInput;
			if (num2 > 0.499)
			{
				_positionEndInput = _positionStartInput + 0.5 - _gemSpacing / _crvLength / 2.0;
				_gemsClosedLoop = true;
				_channelSlopeLength = 0.0;
			}
			else if (num2 < -0.499)
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
			int num3 = (flag ? (count + 1) : count);
			for (int num4 = gemList.Count - 1; num4 >= num3; num4--)
			{
				list.Add(gemList[num4]);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		Point3d unset = Point3d.Unset;
		double length = _crv.GetLength();
		bool flag = gemList.Count > 0;
		int count = gemList.Count;
		int i;
		Point3d val = default(Point3d);
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
				if (unset == Point3d.Unset)
				{
					num = 0.0;
					continue;
				}
				((Point3d)(ref val))._002Ector(0.0, 0.0, 0.0);
				((Point3d)(ref val)).Transform(gemInfoUltimateGem.GemTransform);
				Vector3d val2 = val - unset;
				double length2 = ((Vector3d)(ref val2)).Length;
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
				((Point3d)(ref unset))._002Ector(0.0, 0.0, 0.0);
				((Point3d)(ref unset)).Transform(gemInfoUltimateGem.GemTransform);
			}
			double num6 = _gemRotatedEndXSize + _gemSpacing;
			paramPosition = ((!flag) ? (paramPosition + num6 / length * (double)_direction) : (paramPosition - num6 / length * (double)_direction));
		}
		endpos = ((gemList.Count > 0) ? gemList[i - 1].NormParam : paramPosition);
	}

	private void CalculateGemXform(GemInfoUltimateGem gemOnCurveInfo)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
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
		CalculatePointsOnCrv(gemOnCurveInfo);
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
			double num4 = ProfileCut(gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, out var pScale, out var pRail);
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

	private void CalculatePointsOnCrv(GemInfoUltimateGem gem)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		_crv.NormalizedLengthParameter(num, ref gem.Param);
		Point3d val = _crv.PointAt(gem.Param);
		Vector3d val2 = _crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (_baseCrv == null)
		{
			return;
		}
		if (_vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
		{
			_baseCrv.ClosestPoint(val, ref gem.BaseParam);
			return;
		}
		Plane val3 = default(Plane);
		((Plane)(ref val3))._002Ector(val, val2);
		CurveIntersections obj = Intersection.CurvePlane(_baseCrv, val3, 0.001);
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

	private bool CalculateRotateTestGem(double xSize, double ySize, Transform xform, out double xSizeOut)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d val2;
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint val = nurbsCurve.Points[i];
			val2 = ((ControlPoint)(ref val)).Location;
			double x = ((Point3d)(ref val2)).X;
			val2 = ((ControlPoint)(ref val)).Location;
			((ControlPoint)(ref val)).Location = new Point3d(x, ((Point3d)(ref val2)).Y, 0.0);
			nurbsCurve.Points[i] = val;
		}
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double y = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num = y2 - ((Point3d)(ref val2)).Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections val3 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), (Curve)(object)nurbsCurve, 0.001, 0.001);
			if (val3.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in val3)
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

	private double ProfileCut(GemInfoUltimateGem gemOnCurveInfo)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve val = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + _channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + _channelWallGemOverlap, -2.0, 0.0));
		LineCurve val2 = new LineCurve(new Point3d(sizeY / 2.0 - _channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - _channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = _profile.DuplicateCurve();
		Transform val3 = Transform.Scale(Plane.WorldXY, _profileWidth + gemOnCurveInfo.SizeY, 1.0, 1.0);
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

	private void ProfilePositionScale(GemInfoUltimateGem gem, out Transform pScale, out Transform pRail)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = _crv.PointAt(gem.Param);
		Vector3d destination = _crv.TangentAt(gem.Param);
		Point3d val2 = _baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = val - val2;
		double length = ((Vector3d)(ref destination2)).Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val, destination2, destination);
	}
}
