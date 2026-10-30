using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Tools;

[CSBlock("ObjectsOnCrv", "{DE567370-79F0-4B77-A4C3-0534FA1CA97D}")]
public sealed class CSObjectsOnCrv : CSBlock
{
	private enum EStatus
	{
		FindStart,
		FindNext,
		FoundGem,
		Finished
	}

	private CSData _objectData;

	private OnXform objectXformInvese;

	private OnCurve _crvPrimary;

	private OnBrep _srf;

	private double _vDiameterStart;

	private double _vDiameterEnd;

	private double _vParamStart;

	private double _vParamEnd;

	private double _vSpacing;

	private double _vZOffsetStart;

	private double _vZOffsetEnd;

	private double _vRoll;

	private double _vTilt;

	private int _vPointAt;

	private int _vSpacingMethod;

	private bool _vDirection;

	private bool _vMirror;

	private bool _vTaperDiameter;

	private bool _vTaperSpacing;

	private bool _vTaperZOffset;

	private CSDataArray _outputGemInsts;

	private readonly List<On3dVector> _crvTangents = new List<On3dVector>();

	private readonly List<On3dPoint> _crvPoints = new List<On3dPoint>();

	private readonly List<double> _crvParams = new List<double>();

	private List<double> _radii;

	private List<double> _radiiAdjusted;

	private List<double> _zOffsets;

	private List<int> _gemIndices;

	public CSInputChannel iObject { get; }

	public CSInputChannel iObjectCenterPosition { get; }

	public CSInputChannel iCurve { get; }

	public CSInputChannel iSurface { get; }

	public CSInputChannel iDiameterEnd { get; }

	public CSInputChannel iPositionStart { get; }

	public CSInputChannel iPositionEnd { get; }

	public CSInputChannel iSpacing { get; }

	public CSInputChannel iZOffsetStart { get; }

	public CSInputChannel iZOffsetEnd { get; }

	public CSInputChannel iRoll { get; }

	public CSInputChannel iTilt { get; }

	public CSInputChannel iSpacingMethod { get; }

	public CSInputChannel iDirection { get; }

	public CSInputChannel iMirror { get; }

	public CSInputChannel iTaperDiameter { get; }

	public CSInputChannel iTaperSpacing { get; }

	public CSInputChannel iTaperZOffset { get; }

	public CSInputChannel iPointAt { get; }

	public CSOutputChannel oArray { get; }

	public CSOutputChannel oStartParam { get; }

	public CSOutputChannel oStopParam { get; }

	public CSObjectsOnCrv(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(GetPropertyName(() => iObject), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iObjectCenterPosition = new CSInputChannel(GetPropertyName(() => iObjectCenterPosition), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iSurface = new CSInputChannel(GetPropertyName(() => iSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iDiameterEnd = new CSInputChannel(GetPropertyName(() => iDiameterEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPositionStart = new CSInputChannel(GetPropertyName(() => iPositionStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPositionEnd = new CSInputChannel(GetPropertyName(() => iPositionEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(GetPropertyName(() => iSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffsetStart = new CSInputChannel(GetPropertyName(() => iZOffsetStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffsetEnd = new CSInputChannel(GetPropertyName(() => iZOffsetEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRoll = new CSInputChannel(GetPropertyName(() => iRoll), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTilt = new CSInputChannel(GetPropertyName(() => iTilt), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSpacingMethod = new CSInputChannel(GetPropertyName(() => iSpacingMethod), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDirection = new CSInputChannel(GetPropertyName(() => iDirection), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iMirror = new CSInputChannel(GetPropertyName(() => iMirror), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iTaperDiameter = new CSInputChannel(GetPropertyName(() => iTaperDiameter), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iTaperSpacing = new CSInputChannel(GetPropertyName(() => iTaperSpacing), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iTaperZOffset = new CSInputChannel(GetPropertyName(() => iTaperZOffset), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iPointAt = new CSInputChannel(GetPropertyName(() => iPointAt), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oArray = new CSOutputChannel(GetPropertyName(() => oArray), this, ChannelDataType.Any, ChannelArrayType.Array);
		oStartParam = new CSOutputChannel(GetPropertyName(() => oStartParam), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oStopParam = new CSOutputChannel(GetPropertyName(() => oStopParam), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public static bool GetClosestIndexToParameter(List<double> crvParams, double param, ref int index)
	{
		if (crvParams == null)
		{
			return false;
		}
		if (crvParams.Count <= 0)
		{
			return false;
		}
		double num = Math.Abs(param - crvParams[0]);
		index = 0;
		for (int i = 1; i < crvParams.Count; i++)
		{
			double num2 = Math.Abs(param - crvParams[i]);
			if (num2 < num)
			{
				num = num2;
				index = i;
			}
		}
		return true;
	}

	public static bool GetClosestIndexToPosition(OnCurve crv, List<double> crvParams, double pos, ref int index)
	{
		if (crv == null)
		{
			return false;
		}
		double num = 0.0;
		num = crv.Domain().ParameterAt(pos);
		return GetClosestIndexToParameter(crvParams, num, ref index);
	}

	public override bool Update()
	{
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eab: Expected O, but got Unknown
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed3: Expected O, but got Unknown
		_objectData = iObject.Data;
		if (_objectData == null)
		{
			return UpdateMissing("Object");
		}
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		CSData arrayItem = flattenArray.GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateMissing("Curve");
		}
		_crvPrimary = (OnCurve)(object)arrayItem.GetNurbsCurve().Convert();
		if (_crvPrimary == null)
		{
			return UpdateMissing("Curve");
		}
		double value = 0.0;
		iObjectCenterPosition.GetDouble(ref value);
		_vPointAt = 0;
		iPointAt.GetInteger(ref _vPointAt);
		_srf = null;
		CSDataArray flattenArray2 = iSurface.GetFlattenArray();
		if (flattenArray2.GetArrayCount() > 0)
		{
			CSData arrayItem2 = flattenArray2.GetArrayItem(0);
			if (arrayItem2 != null)
			{
				_srf = arrayItem2.GetBrep().Convert();
			}
		}
		_vDiameterStart = 1.0;
		_vDiameterEnd = 1.0;
		iDiameterEnd.GetDouble(ref _vDiameterEnd);
		_vParamStart = 0.0;
		iPositionStart.GetDouble(ref _vParamStart);
		_vParamEnd = 1.0;
		iPositionEnd.GetDouble(ref _vParamEnd);
		_vSpacing = 0.0;
		iSpacing.GetDouble(ref _vSpacing);
		_vZOffsetStart = 0.0;
		iZOffsetStart.GetDouble(ref _vZOffsetStart);
		_vZOffsetEnd = 0.0;
		iZOffsetEnd.GetDouble(ref _vZOffsetEnd);
		_vRoll = 0.0;
		iRoll.GetDouble(ref _vRoll);
		_vTilt = 0.0;
		iTilt.GetDouble(ref _vTilt);
		_vSpacingMethod = 0;
		iSpacingMethod.GetInteger(ref _vSpacingMethod);
		_vDirection = false;
		iDirection.GetBoolean(ref _vDirection);
		_vMirror = false;
		iMirror.GetBoolean(ref _vMirror);
		_vTaperDiameter = false;
		iTaperDiameter.GetBoolean(ref _vTaperDiameter);
		_vTaperSpacing = false;
		iTaperSpacing.GetBoolean(ref _vTaperSpacing);
		_vTaperZOffset = false;
		iTaperZOffset.GetBoolean(ref _vTaperZOffset);
		_outputGemInsts = new CSDataArray();
		if (_srf == null && _vPointAt == 0)
		{
			_vPointAt = 2;
		}
		if (!_vTaperZOffset)
		{
			_vZOffsetEnd = _vZOffsetStart;
		}
		bool flag = false;
		if (_srf != null)
		{
			flag = BeenUpdatedAfterLastUpdate(iSurface.OutputChannelBlock);
		}
		if ((BeenUpdatedAfterLastUpdate(iCurve.OutputChannelBlock) || flag || BeenUpdatedAfterLastUpdate(iObject.OutputChannelBlock) || _crvPoints.Count == 0) && !PrepCrvPrimary())
		{
			return UpdateError("PrepCrvPrimary failed");
		}
		int index = 0;
		if (!GetClosestIndexToPosition(_crvPrimary, _crvParams, value, ref index))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		objectXformInvese = CalculateObjectInvese((IOnCurve)(object)_crvPrimary, _crvParams[index], _vZOffsetStart);
		OnXform val = new OnXform();
		val.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), OnUtil.On_yaxis, OnUtil.On_origin);
		OnXform val2 = new OnXform();
		val2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), OnUtil.On_zaxis, OnUtil.On_origin);
		CSData cSData = _objectData.Duplicate();
		CSDataHelper.Transform(cSData, objectXformInvese.Convert());
		OnMesh val3 = cSData.GetMesh().Convert();
		OnBoundingBox val4 = new OnBoundingBox();
		((OnGeometry)val3).GetTightBoundingBox(ref val4);
		double num = val4.Diagonal().x / 2.0;
		_vDiameterStart = num * 2.0;
		if (!_vTaperDiameter)
		{
			_vDiameterEnd = _vDiameterStart;
		}
		double num2 = _vDiameterStart / 2.0 / num;
		OnXform val5 = new OnXform();
		val5.Scale(num2, num2, num2);
		OnMesh obj = val3.Duplicate();
		((OnGeometry)obj).Transform((IOnXform)(object)(val2 * (IOnXform)(object)val * (IOnXform)(object)val5));
		OnBoundingBox val6 = new OnBoundingBox();
		((OnGeometry)obj).GetTightBoundingBox(ref val6);
		double x = val6.Diagonal().x;
		double num4;
		if (_vTaperDiameter)
		{
			double num3 = _vDiameterEnd / 2.0 / num;
			OnXform val7 = new OnXform();
			val7.Scale(num3, num3, num3);
			OnMesh obj2 = val3.Duplicate();
			((OnGeometry)obj2).Transform((IOnXform)(object)(val2 * (IOnXform)(object)val * (IOnXform)(object)val7));
			OnBoundingBox val8 = new OnBoundingBox();
			((OnGeometry)obj2).GetTightBoundingBox(ref val8);
			num4 = val8.Diagonal().x;
		}
		else
		{
			num4 = x;
		}
		int index2 = -1;
		if (!GetClosestIndexToPosition(_crvPrimary, _crvParams, _vParamStart, ref index2))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int index3 = -1;
		if (!GetClosestIndexToPosition(_crvPrimary, _crvParams, _vParamEnd, ref index3))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int num5 = ((index2 <= index3) ? 1 : (-1));
		int num6 = ((!_vDirection) ? 1 : (-1));
		num5 *= num6;
		double num7 = (x / 2.0 + _vSpacing / 2.0) / (x / 2.0);
		int num8 = 0;
		int num9 = index2 - num5;
		int num10 = 0;
		int num11 = Math.Abs(index3 - index2);
		if (num6 == -1)
		{
			num11 = _crvPoints.Count - num11;
		}
		_radii = new List<double>();
		_radiiAdjusted = new List<double>();
		_zOffsets = new List<double>();
		_gemIndices = new List<int>();
		EStatus eStatus = EStatus.FindStart;
		while (eStatus != EStatus.Finished)
		{
			num9 += num5;
			num10++;
			if (GetFixedIndex(num9) == index3)
			{
				if (eStatus == EStatus.FindNext)
				{
					if (num8 == 0)
					{
						num8 = 1;
					}
					double num12 = num9 - _gemIndices[_gemIndices.Count - 1];
					double num13;
					double num14;
					if (_vSpacingMethod == 1)
					{
						num13 = num12 / ((double)(_gemIndices.Count - num8) - 0.5);
						num14 = 0.5;
					}
					else
					{
						num13 = num12 / (double)(_gemIndices.Count - num8);
						num14 = 1.0;
					}
					for (int i = num8; i < _gemIndices.Count; i++)
					{
						if (_vSpacingMethod < 2)
						{
							_gemIndices[i] += (int)(num13 * num14);
						}
						if (_vSpacingMethod == 1)
						{
							double num15 = num13 / 100.0;
							_radii[i] = Math.Round((_radii[i] + num15) * 2.0, 1, MidpointRounding.ToEven) / 2.0;
							_radiiAdjusted[i] = (_radiiAdjusted[i] + num15) * 2.0;
						}
						num14 += 1.0;
					}
				}
				eStatus = EStatus.Finished;
			}
			double value2 = _vDiameterStart + (_vDiameterEnd - _vDiameterStart) * ((double)num10 / (double)num11);
			double num16 = x + (num4 - x) * ((double)num10 / (double)num11);
			double item = Math.Round(value2, 1, MidpointRounding.ToEven) / 2.0;
			double num17 = num16 / 2.0;
			double item2 = _vZOffsetStart + (_vZOffsetEnd - _vZOffsetStart) * ((double)num10 / (double)num11);
			if (eStatus == EStatus.FindStart)
			{
				eStatus = EStatus.FoundGem;
				for (int j = 0; j < _gemIndices.Count; j++)
				{
					double num18 = ((!_vTaperSpacing) ? (num17 + _radiiAdjusted[j] + _vSpacing) : ((num17 + _radiiAdjusted[j]) * num7));
					if (!((GetPointFixedIndex(num9) - (IOn3dPoint)(object)GetPointGemIndice(j)).LengthSquared() >= num18 * num18))
					{
						eStatus = EStatus.FindStart;
						break;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_radii.Add(item);
					_radiiAdjusted.Add(num17);
					_zOffsets.Add(item2);
					_gemIndices.Add(num9);
					num8 = _gemIndices.Count - 1;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				double num18 = ((!_vTaperSpacing) ? (num17 + _radiiAdjusted[_gemIndices.Count - 1] + _vSpacing) : ((num17 + _radiiAdjusted[_gemIndices.Count - 1]) * num7));
				if ((GetPointFixedIndex(num9) - (IOn3dPoint)(object)GetPointGemIndice(_gemIndices.Count - 1)).Length() > num18)
				{
					_radii.Add(item);
					_radiiAdjusted.Add(num17);
					_zOffsets.Add(item2);
					_gemIndices.Add(num9);
					eStatus = EStatus.FoundGem;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				int num19 = -1;
				for (int k = 0; k < _gemIndices.Count - 1; k++)
				{
					double num18 = ((!_vTaperSpacing) ? (num17 + _radiiAdjusted[k] + _vSpacing) : ((num17 + _radiiAdjusted[k]) * num7));
					if (!((GetPointFixedIndex(num9) - (IOn3dPoint)(object)GetPointGemIndice(k)).LengthSquared() >= num18 * num18))
					{
						num19 = k;
						break;
					}
				}
				if (num19 >= 0)
				{
					if (num19 > num8)
					{
						num8 = num19 + 1;
					}
					if (num8 == 0)
					{
						num8 = 1;
					}
					double num20 = num9 - _gemIndices[_gemIndices.Count - 1];
					double num21;
					double num22;
					if (_vSpacingMethod == 1)
					{
						num21 = num20 / (double)(_gemIndices.Count - num8);
						num22 = 0.5;
					}
					else
					{
						num21 = num20 / ((double)(_gemIndices.Count - num8) + 1.0);
						num22 = 1.0;
					}
					for (int l = num8; l < _gemIndices.Count; l++)
					{
						if (_vSpacingMethod < 2)
						{
							_gemIndices[l] += (int)(num21 * num22);
						}
						if (_vSpacingMethod == 1)
						{
							double num23 = num21 / 100.0;
							_radii[l] = Math.Round((_radii[l] + num23) * 2.0, 1, MidpointRounding.ToEven) / 2.0;
							_radiiAdjusted[l] = (_radiiAdjusted[l] + num23) * 2.0;
						}
						num22 += 1.0;
					}
					eStatus = EStatus.FindStart;
				}
			}
			if (eStatus == EStatus.FoundGem)
			{
				eStatus = EStatus.FindNext;
			}
		}
		if (_vMirror && _gemIndices.Count > 1)
		{
			int num24 = _gemIndices.Count - 2;
			double item3 = _radii[num24];
			double num25 = _radiiAdjusted[num24];
			double item4 = _zOffsets[num24];
			double num26 = (GetPointGemIndice(num24 + 1) - (IOn3dPoint)(object)GetPointGemIndice(num24)).Length() - (_radiiAdjusted[num24 + 1] + num25);
			num24--;
			eStatus = EStatus.FindStart;
			while (eStatus != EStatus.Finished)
			{
				num9 += num5;
				num10++;
				if (!_crvPrimary.IsClosed() && num9 != GetFixedIndex(num9))
				{
					eStatus = EStatus.Finished;
				}
				if (num10 > _crvPoints.Count)
				{
					eStatus = EStatus.Finished;
				}
				if (eStatus == EStatus.Finished)
				{
					continue;
				}
				eStatus = EStatus.FoundGem;
				if ((GetPointFixedIndex(num9) - (IOn3dPoint)(object)GetPointGemIndice(0)).Length() - (_radiiAdjusted[0] + num25) < 0.0)
				{
					eStatus = EStatus.FindStart;
				}
				for (int m = 1; m < _gemIndices.Count; m++)
				{
					if ((GetPointFixedIndex(num9) - (IOn3dPoint)(object)GetPointGemIndice(m)).Length() - (_radiiAdjusted[m] + num25) < num26)
					{
						eStatus = EStatus.FindStart;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_radii.Add(item3);
					_radiiAdjusted.Add(num25);
					_zOffsets.Add(item4);
					_gemIndices.Add(GetFixedIndex(num9));
					if (num24 >= 0)
					{
						item3 = _radii[num24];
						num25 = _radiiAdjusted[num24];
						item4 = _zOffsets[num24];
						num26 = (GetPointGemIndice(num24 + 1) - (IOn3dPoint)(object)GetPointGemIndice(num24)).Length() - (_radiiAdjusted[num24 + 1] + num25);
						num24--;
					}
					else
					{
						eStatus = EStatus.Finished;
					}
				}
			}
		}
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		for (int n = 0; n < _gemIndices.Count; n++)
		{
			list.Add(_crvParams[GetFixedIndex(_gemIndices[n])]);
			list2.Add(_radii[n]);
			list3.Add(_zOffsets[n]);
		}
		CalculateObjects((IOnCurve)(object)_crvPrimary, list, list2, list3, splitBetweenStones: false, num);
		oArray.SetData(_outputGemInsts);
		if (list.Count > 0)
		{
			double num27 = 1.0;
			_crvPrimary.GetLength(ref num27);
			if (num27 < 0.001)
			{
				num27 = 1.0;
			}
			OnCurve val9 = _crvPrimary.Duplicate();
			OnCurve obj3 = _crvPrimary.Duplicate();
			val9.Trim((IOnInterval)new OnInterval(val9.Domain().Min(), list[0]));
			obj3.Trim((IOnInterval)new OnInterval(val9.Domain().Min(), list[list.Count - 1]));
			double num28 = 1.0;
			double num29 = 1.0;
			val9.GetLength(ref num28);
			obj3.GetLength(ref num29);
			oStartParam.SetData(new CSNumberData(num28 / num27));
			oStopParam.SetData(new CSNumberData(num29 / num27));
		}
		return base.Update();
	}

	private OnXform CalculateObjectInvese(IOnCurve crv, double param, double zOffset)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		On3dPoint val = crv.PointAt(param);
		On3dVector val2 = crv.TangentAt(param);
		IOn3dVector on_zaxis = OnUtil.On_zaxis;
		IOn3dVector on_xaxis = OnUtil.On_xaxis;
		OnXform val3;
		switch (_vPointAt)
		{
		case 0:
		{
			On3dVector brepNormal = new On3dVector(OnUtil.On_zaxis);
			CSUtilGeometry.GetClosestNormalToBrep((IOnBrep)(object)_srf, (IOn3dPoint)(object)val, ref brepNormal);
			val3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_zaxis, on_xaxis, (IOn3dPoint)(object)val, (IOn3dVector)(object)brepNormal, (IOn3dVector)(object)val2);
			break;
		}
		case 1:
		{
			On3dVector val5 = new On3dVector((IOn3dPoint)(object)val);
			val5.Unitize();
			val3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_xaxis, on_zaxis, (IOn3dPoint)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)val5);
			break;
		}
		case 2:
		{
			On3dVector val4 = new On3dVector((IOn3dPoint)(object)val);
			val4.y = 0.0;
			val4.Unitize();
			val3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_xaxis, on_zaxis, (IOn3dPoint)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)val4);
			break;
		}
		case 3:
			val3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_zaxis, on_xaxis, (IOn3dPoint)(object)val, OnUtil.On_zaxis, (IOn3dVector)(object)val2);
			break;
		default:
			val3 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_zaxis, on_xaxis, (IOn3dPoint)(object)val, OnUtil.On_zaxis, (IOn3dVector)(object)val2);
			break;
		}
		return val3.Inverse();
	}

	private void CalculateObjects(IOnCurve pCrv, IList<double> pParams, IList<double> pSizes, IList<double> pZOffsets, bool splitBetweenStones, double gemMaxX)
	{
		int num = Math.Min(pParams.Count, Math.Min(pSizes.Count, pZOffsets.Count));
		if (num <= 0)
		{
			return;
		}
		if (splitBetweenStones)
		{
			double num2 = Math.Min(pParams.Count - 1, pSizes.Count - 1);
			for (int i = 0; i < num && (double)i < num2; i++)
			{
				double num3 = pCrv.PointAt(pParams[i]).DistanceTo((IOn3dPoint)(object)pCrv.PointAt(pParams[i + 1]));
				double num4 = pSizes[i] + (num3 - (pSizes[i] + pSizes[i + 1])) / 2.0;
				double param = pParams[i] + (pParams[i + 1] - pParams[i]) * (num4 / num3);
				double size = (pSizes[i] + pSizes[i + 1]) / 2.0;
				double zOffset = (pZOffsets[i] + pZOffsets[i + 1]) / 2.0;
				OnXform xform = CalculateObjectXform(pCrv, param, size, zOffset, gemMaxX);
				CSData data = _objectData.Duplicate();
				CSDataHelper.Transform(data, objectXformInvese.Convert());
				CSDataHelper.Transform(data, xform.Convert());
				_outputGemInsts.Append(data);
			}
		}
		else
		{
			double num5 = Math.Min(pParams.Count, pSizes.Count);
			for (int j = 0; j < num && (double)j < num5; j++)
			{
				OnXform xform2 = CalculateObjectXform(pCrv, pParams[j], pSizes[j], pZOffsets[j], gemMaxX);
				CSData data2 = _objectData.Duplicate();
				CSDataHelper.Transform(data2, objectXformInvese.Convert());
				CSDataHelper.Transform(data2, xform2.Convert());
				_outputGemInsts.Append(data2);
			}
		}
	}

	private OnXform CalculateObjectXform(IOnCurve crv, double param, double size, double zOffset, double gemXMax)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		double num = size / gemXMax;
		OnXform val = new OnXform();
		val.Scale(num, num, num);
		OnXform val2 = new OnXform();
		val2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform val3 = new OnXform();
		val3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), OnUtil.On_xaxis, OnUtil.On_origin);
		CSData data = _objectData.Duplicate();
		CSDataHelper.Transform(data, objectXformInvese.Convert());
		CSDataHelper.Transform(data, val.Convert());
		OnXform val4 = new OnXform();
		val4.Translation(0.0, 0.0, zOffset);
		On3dPoint val5 = crv.PointAt(param);
		On3dVector val6 = crv.TangentAt(param);
		IOn3dVector on_zaxis = OnUtil.On_zaxis;
		IOn3dVector on_xaxis = OnUtil.On_xaxis;
		OnXform val7;
		switch (_vPointAt)
		{
		case 0:
		{
			On3dVector brepNormal = new On3dVector(OnUtil.On_zaxis);
			CSUtilGeometry.GetClosestNormalToBrep((IOnBrep)(object)_srf, (IOn3dPoint)(object)val5, ref brepNormal);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_zaxis, on_xaxis, (IOn3dPoint)(object)val5, (IOn3dVector)(object)brepNormal, (IOn3dVector)(object)val6);
			break;
		}
		case 1:
		{
			On3dVector val9 = new On3dVector((IOn3dPoint)(object)val5);
			val9.Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_xaxis, on_zaxis, (IOn3dPoint)(object)val5, (IOn3dVector)(object)val6, (IOn3dVector)(object)val9);
			break;
		}
		case 2:
		{
			On3dVector val8 = new On3dVector((IOn3dPoint)(object)val5);
			val8.y = 0.0;
			val8.Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_xaxis, on_zaxis, (IOn3dPoint)(object)val5, (IOn3dVector)(object)val6, (IOn3dVector)(object)val8);
			break;
		}
		case 3:
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_zaxis, on_xaxis, (IOn3dPoint)(object)val5, OnUtil.On_zaxis, (IOn3dVector)(object)val6);
			break;
		default:
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on_zaxis, on_xaxis, (IOn3dPoint)(object)val5, OnUtil.On_zaxis, (IOn3dVector)(object)val6);
			break;
		}
		return val7 * (IOnXform)(object)val4 * (IOnXform)(object)val2 * (IOnXform)(object)val3 * (IOnXform)(object)val;
	}

	private int GetFixedIndex(int index)
	{
		int count = _crvParams.Count;
		if (count == 0)
		{
			return 0;
		}
		for (index += count; index < 0; index += count)
		{
		}
		index %= count;
		return index;
	}

	private On3dPoint GetPointFixedIndex(int index)
	{
		return _crvPoints[GetFixedIndex(index)];
	}

	private On3dPoint GetPointGemIndice(int index)
	{
		return GetPointFixedIndex(_gemIndices[index]);
	}

	private bool PrepCrvPrimary()
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		if (_crvPrimary == null)
		{
			return false;
		}
		double num = 0.0;
		if (!_crvPrimary.GetLength(ref num))
		{
			return false;
		}
		int num2 = (int)(num * 50.0);
		double num3 = num2;
		int num4 = num2 + 1;
		_crvParams.Clear();
		_crvPoints.Clear();
		_crvTangents.Clear();
		double[] array = new double[num4];
		List<double> list = new List<double>();
		for (double num5 = 0.0; num5 < (double)num4; num5 += 1.0)
		{
			list.Add(num5 / num3);
		}
		if (!_crvPrimary.GetNormalizedArcLengthPoints(list.ToArray(), ref array))
		{
			return false;
		}
		double[] array2 = array;
		foreach (double num6 in array2)
		{
			On3dPoint item = new On3dPoint();
			On3dVector item2 = new On3dVector();
			_crvPrimary.Ev1Der(num6, ref item, ref item2);
			_crvParams.Add(num6);
			_crvPoints.Add(item);
			_crvTangents.Add(item2);
		}
		return true;
	}
}
