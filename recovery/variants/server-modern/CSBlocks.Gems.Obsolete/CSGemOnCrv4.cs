using System;
using System.Collections.Generic;
using System.Linq;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrv_V4", "{280F8918-68B4-497d-81D6-BEF4D4BFC764}")]
public sealed class CSGemOnCrv4 : CSBlock
{
	private enum EStatus
	{
		FindFirst,
		FindNext,
		FoundGem,
		Finished
	}

	private struct GemOnCurveInfo
	{
		public IOn3dPoint Pt { get; }

		public IOn3dVector Tan { get; }

		public double Radius { get; }

		public double OffsetZ { get; }

		public GemOnCurveInfo(IOn3dPoint pt, IOn3dVector tan, double radius, double offsetZ)
		{
			Pt = pt;
			Tan = tan;
			Radius = radius;
			OffsetZ = offsetZ;
		}
	}

	private CSGemData _sourceGemInstance;

	private OnCurve _crvMaster;

	private OnBrep _srf;

	private int _vPlacementMethod;

	private int _vSpacingMethod;

	private double _vDiameterStart;

	private double _vDiameterEnd;

	private double _vParamStart;

	private double _vParamEnd;

	private double _vSpacing;

	private double _vZOffsetStart;

	private double _vZOffsetEnd;

	private double _vRoll;

	private double _vTilt;

	private bool _vFlip;

	private bool _vMirror;

	private bool _vTaperDiameter;

	private bool _vTaperSpacing;

	private bool _vTaperZOffset;

	private int _vPointAt;

	private CSDataArray _outputGems;

	private List<int> _gemIndices;

	private List<double> _gemSizes;

	private List<double> _gemSizesAdjusted;

	private List<double> _gemZOffsets;

	private On3dPoint[] _crvPoints;

	private On3dVector[] _crvTangents;

	private double[] _unnormalizedParams;

	private double[] _normalizedParams;

	private double _crvLength;

	private double _paramLastStart;

	private double _paramLastEnd;

	private double _spacingActual;

	private int _crvPointCount;

	private int _dirOver;

	public CSOutputChannel OutArray { get; }

	public CSInputChannel InGemDefinition { get; }

	public CSInputChannel InCurve { get; }

	public CSInputChannel InSurface { get; }

	public CSInputChannel InDiameterEnd { get; }

	public CSInputChannel InParamStart { get; }

	public CSInputChannel InParamEnd { get; }

	public CSInputChannel InSpacing { get; }

	public CSInputChannel InZOffsetStart { get; }

	public CSInputChannel InZOffsetEnd { get; }

	public CSInputChannel InRoll { get; }

	public CSInputChannel InTilt { get; }

	public CSInputChannel InPlacementMethod { get; }

	public CSInputChannel InSpacingMethod { get; }

	public CSInputChannel InFlip { get; }

	public CSInputChannel InMirror { get; }

	public CSInputChannel InTaperDiameter { get; }

	public CSInputChannel InTaperSpacing { get; }

	public CSInputChannel InTaperZOffset { get; }

	public CSInputChannel InPointAt { get; }

	public CSGemOnCrv4(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InSpacingMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperDiameter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperSpacing = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperZOffset = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InPointAt = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		_sourceGemInstance = InGemDefinition.Gem;
		_crvMaster = (OnCurve)(object)InCurve.NurbsCurve.Convert();
		CSDataArray flattenArray = InSurface.GetFlattenArray();
		_srf = null;
		if (flattenArray.GetArrayCount() > 0)
		{
			_srf = InSurface.Brep.Convert();
			if (_srf == null)
			{
				return UpdateMissing("Brep");
			}
			MRhinoBrepObject val = RhUtil.RhinoApp().ActiveDoc().AddBrepObject((IOnBrep)(object)_srf);
			if (val != null)
			{
				_srf = OnBrep.Cast(((IOnObject)val.Brep()).DuplicateOnObject());
				RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)val));
			}
		}
		_vPlacementMethod = InPlacementMethod.Integer;
		_vSpacingMethod = InSpacingMethod.Integer;
		_vDiameterStart = _sourceGemInstance.GemXSize;
		_vDiameterEnd = InDiameterEnd.Double;
		_vParamStart = InParamStart.Double;
		_vParamEnd = InParamEnd.Double;
		_vSpacing = InSpacing.Double;
		_vZOffsetStart = InZOffsetStart.Double;
		_vZOffsetEnd = InZOffsetEnd.Double;
		_vRoll = InRoll.Double;
		_vTilt = InTilt.Double;
		_vFlip = InFlip.Boolean;
		_vMirror = InMirror.Boolean;
		_vTaperDiameter = InTaperDiameter.Boolean;
		_vTaperSpacing = InTaperSpacing.Boolean;
		_vTaperZOffset = InTaperZOffset.Boolean;
		_vPointAt = InPointAt.Integer;
		if (_srf == null && _vPointAt == 0)
		{
			_vPointAt = 2;
		}
		_outputGems = new CSDataArray();
		_dirOver = 1;
		_paramLastStart = _vParamStart;
		_paramLastEnd = _vParamEnd;
		bool flag = false;
		if (_srf != null)
		{
			flag = BeenUpdatedAfterLastUpdate(InSurface.OutputChannelBlock);
		}
		if ((BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock) || flag || BeenUpdatedAfterLastUpdate(InGemDefinition.OutputChannelBlock) || _crvPointCount == 0) && !PrepCrvMaster())
		{
			return UpdateError("Crv Prep");
		}
		UpdateBuilder();
		OutArray.SetData(_outputGems);
		return base.Update();
	}

	private OnXform CalculateGemXform(GemOnCurveInfo gemOnCurveInfo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		double num = gemOnCurveInfo.Radius / _sourceGemInstance.GemXMax;
		OnXform val = new OnXform();
		val.Scale(num, num, num);
		OnXform val2 = new OnXform();
		val2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform val3 = new OnXform();
		val3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), OnUtil.On_xaxis, OnUtil.On_origin);
		CSGemData cSGemData = new CSGemData(_sourceGemInstance);
		cSGemData.ApplyTransform(val.Convert());
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, (CSUtilGem.EGemPlacementMethod)_vPlacementMethod, ref zOffset);
		OnXform val4 = new OnXform();
		val4.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.OffsetZ);
		IOn3dPoint pt = gemOnCurveInfo.Pt;
		IOn3dVector tan = gemOnCurveInfo.Tan;
		On3dVector val5 = (_vFlip ? new On3dVector(0.0, 0.0, -1.0) : new On3dVector(0.0, 0.0, 1.0));
		On3dVector val6 = (_vFlip ? new On3dVector(-1.0, 0.0, 0.0) : new On3dVector(1.0, 0.0, 0.0));
		OnXform val7 = new OnXform();
		val7.Identity();
		switch (_vPointAt)
		{
		case 0:
		{
			On3dVector destination = new On3dVector();
			CSUtilGeometry.GetClosestNormalToBrep((IOnBrep)(object)_srf, pt, ref destination);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		case 1:
		{
			On3dVector destination = new On3dVector(pt);
			destination.Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val6, (IOn3dVector)(object)val5, pt, tan, (IOn3dVector)(object)destination);
			break;
		}
		case 2:
		{
			On3dVector destination = new On3dVector(pt.x, 0.0, pt.z);
			destination.Unitize();
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val6, (IOn3dVector)(object)val5, pt, tan, (IOn3dVector)(object)destination);
			break;
		}
		case 3:
		{
			On3dVector destination = new On3dVector(0.0, 0.0, 1.0);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		default:
		{
			On3dVector destination = new On3dVector(0.0, 0.0, 1.0);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		}
		return val7 * (IOnXform)(object)val4 * (IOnXform)(object)val2 * (IOnXform)(object)val3 * (IOnXform)(object)val;
	}

	private int GetFixedCurvePoint(int val)
	{
		int i = val;
		if (_crvPointCount == 0)
		{
			return 0;
		}
		for (; i < 0; i += _crvPointCount)
		{
		}
		return i % _crvPointCount;
	}

	private bool PrepCrvMaster()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		if (_crvMaster == null)
		{
			return false;
		}
		_crvMaster.GetLength(ref _crvLength);
		_crvPointCount = (int)(_crvLength * 50.0);
		_unnormalizedParams = new double[_crvPointCount + 1];
		_normalizedParams = new double[_crvPointCount + 1];
		_crvPoints = (On3dPoint[])(object)new On3dPoint[_crvPointCount + 1];
		_crvTangents = (On3dVector[])(object)new On3dVector[_crvPointCount + 1];
		for (int i = 0; i <= _crvPointCount; i++)
		{
			_unnormalizedParams[i] = (double)i / (double)_crvPointCount;
		}
		_crvPointCount++;
		_crvMaster.GetNormalizedArcLengthPoints(_unnormalizedParams, ref _normalizedParams);
		for (int j = 0; j < _crvPointCount; j++)
		{
			On3dPoint val = new On3dPoint();
			On3dVector val2 = new On3dVector();
			_crvMaster.Ev1Der(_normalizedParams[j], ref val, ref val2);
			_crvPoints[j] = val;
			_crvTangents[j] = val2;
		}
		return true;
	}

	private void UpdateBuilder()
	{
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		if (_crvMaster.IsClosed())
		{
			if (_vParamStart < 0.2 && _paramLastStart > 0.8)
			{
				_dirOver = -_dirOver;
			}
			if (_vParamStart > 0.8 && _paramLastStart < 0.2)
			{
				_dirOver = -_dirOver;
			}
			if (_vParamEnd < 0.2 && _paramLastEnd > 0.8)
			{
				_dirOver = -_dirOver;
			}
			if (_vParamEnd > 0.8 && _paramLastEnd < 0.2)
			{
				_dirOver = -_dirOver;
			}
		}
		_paramLastStart = _vParamStart;
		_paramLastEnd = _vParamEnd;
		if (!_vTaperDiameter)
		{
			_vDiameterEnd = _vDiameterStart;
		}
		if (!_vTaperZOffset)
		{
			_vZOffsetEnd = _vZOffsetStart;
		}
		OnInterval val = _crvMaster.Domain();
		double num = val.Min() + val.Length() * _vParamStart;
		int num2 = 0;
		double num3 = 999.0;
		for (int i = 0; i < _crvPointCount; i++)
		{
			if (Math.Abs(num - _normalizedParams[i]) < num3)
			{
				num3 = Math.Abs(num - _normalizedParams[i]);
				num2 = i;
			}
		}
		num = val.Min() + val.Length() * _vParamEnd;
		int num4 = 0;
		double num5 = 999.0;
		for (int j = 0; j < _crvPointCount; j++)
		{
			if (Math.Abs(num - _normalizedParams[j]) < num5)
			{
				num5 = Math.Abs(num - _normalizedParams[j]);
				num4 = j;
			}
		}
		int num6 = ((num2 <= num4) ? 1 : (-1));
		num6 *= _dirOver;
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), Vector3d.ZAxis, Point3d.Origin);
		Transform val3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), Vector3d.YAxis, Point3d.Origin);
		double num7 = _vDiameterStart / 2.0 / _sourceGemInstance.GemXMax;
		Transform val4 = Transform.Scale(Plane.WorldXY, num7, num7, num7);
		CSGemData cSGemData = new CSGemData(_sourceGemInstance);
		cSGemData.ApplyTransform(val2 * val3 * val4);
		BoundingBox boundingBox = BoundingBox.Unset;
		cSGemData.GetTightBoundingBox(ref boundingBox);
		Point3d val5 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val5)).X;
		val5 = ((BoundingBox)(ref boundingBox)).Min;
		double num8 = x - ((Point3d)(ref val5)).X;
		double num10;
		if (_vTaperDiameter)
		{
			double num9 = _vDiameterEnd / 2.0 / _sourceGemInstance.GemXMax;
			Transform val6 = Transform.Scale(Plane.WorldXY, num9, num9, num9);
			CSGemData cSGemData2 = new CSGemData(_sourceGemInstance);
			cSGemData2.ApplyTransform(val2 * val3 * val6);
			BoundingBox boundingBox2 = BoundingBox.Unset;
			cSGemData2.GetTightBoundingBox(ref boundingBox2);
			val5 = ((BoundingBox)(ref boundingBox2)).Max;
			double x2 = ((Point3d)(ref val5)).X;
			val5 = ((BoundingBox)(ref boundingBox2)).Min;
			num10 = x2 - ((Point3d)(ref val5)).X;
		}
		else
		{
			num10 = num8;
		}
		double num11 = (num8 / 2.0 + _vSpacing / 2.0) / (num8 / 2.0);
		EStatus eStatus = EStatus.FindFirst;
		int num12 = 0;
		int num13 = 0;
		int num14 = num2 - num6;
		int num15 = Math.Abs(num4 - num2);
		if (_dirOver == -1)
		{
			num15 = _crvPointCount - num15;
		}
		_gemIndices = new List<int>();
		_gemSizes = new List<double>();
		_gemSizesAdjusted = new List<double>();
		_gemZOffsets = new List<double>();
		while (eStatus != EStatus.Finished)
		{
			num14 += num6;
			num12++;
			double item = _vDiameterStart / 2.0 + (_vDiameterEnd / 2.0 - _vDiameterStart / 2.0) * ((double)num12 / (double)num15);
			double num16 = num8 / 2.0 + (num10 / 2.0 - num8 / 2.0) * ((double)num12 / (double)num15);
			double item2 = _vZOffsetStart + (_vZOffsetEnd - _vZOffsetStart) * ((double)num12 / (double)num15);
			if (GetFixedCurvePoint(num14) == num4)
			{
				if (eStatus == EStatus.FindNext)
				{
					if (num13 == 0)
					{
						num13 = 1;
					}
					double num17 = num14 - _gemIndices[_gemIndices.Count - 1];
					double num18;
					double num19;
					if (_vSpacingMethod == 1)
					{
						num18 = num17 / ((double)(_gemIndices.Count - num13) - 0.5);
						num19 = 0.5;
					}
					else
					{
						num18 = num17 / (double)(_gemIndices.Count - num13);
						num19 = 1.0;
					}
					for (int k = num13; k < _gemIndices.Count; k++)
					{
						if (_vSpacingMethod < 2)
						{
							_gemIndices[k] += (int)(num18 * num19);
						}
						if (_vSpacingMethod == 1)
						{
							_gemSizes[k] += num18 / 100.0;
							_gemSizesAdjusted[k] += num18 / 100.0;
						}
						num19 += 1.0;
					}
				}
				eStatus = EStatus.Finished;
			}
			if (eStatus == EStatus.FindFirst)
			{
				eStatus = EStatus.FoundGem;
				for (int l = 0; l < _gemIndices.Count; l++)
				{
					double num20 = ((!_vTaperSpacing) ? (num16 + _gemSizesAdjusted[l] + _vSpacing) : ((num16 + _gemSizesAdjusted[l]) * num11));
					(_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[l])]).LengthSquared();
					if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[l])]).LengthSquared() < num20 * num20)
					{
						eStatus = EStatus.FindFirst;
						break;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_gemIndices.Add(num14);
					_gemSizes.Add(item);
					_gemSizesAdjusted.Add(num16);
					_gemZOffsets.Add(item2);
					num13 = _gemIndices.Count - 1;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				double num20 = ((!_vTaperSpacing) ? (num16 + _gemSizesAdjusted[_gemIndices.Count - 1] + _vSpacing) : ((num16 + _gemSizesAdjusted[_gemIndices.Count - 1]) * num11));
				(_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[_gemIndices.Count - 1])]).Length();
				if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[_gemIndices.Count - 1])]).Length() > num20)
				{
					_gemIndices.Add(num14);
					_gemSizes.Add(item);
					_gemSizesAdjusted.Add(num16);
					_gemZOffsets.Add(item2);
					eStatus = EStatus.FoundGem;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				int num21 = -1;
				for (int m = 0; m < _gemIndices.Count - 1; m++)
				{
					double num20 = ((!_vTaperSpacing) ? (num16 + _gemSizesAdjusted[m] + _vSpacing) : ((num16 + _gemSizesAdjusted[m]) * num11));
					(_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[m])]).LengthSquared();
					if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[m])]).LengthSquared() < num20 * num20)
					{
						num21 = m;
						break;
					}
				}
				if (num21 >= 0)
				{
					if (num21 > num13)
					{
						num13 = num21 + 1;
					}
					if (num13 == 0)
					{
						num13 = 1;
					}
					double num22 = num14 - _gemIndices[_gemIndices.Count - 1];
					double num23;
					if (_vSpacingMethod == 1)
					{
						num22 /= (double)(_gemIndices.Count - num13);
						num23 = 0.5;
					}
					else
					{
						num22 /= (double)(_gemIndices.Count - num13 + 1);
						num23 = 1.0;
					}
					for (int n = num13; n < _gemIndices.Count; n++)
					{
						if (_vSpacingMethod < 2)
						{
							_gemIndices[n] += (int)(num22 * num23);
						}
						if (_vSpacingMethod == 1)
						{
							_gemSizes[n] += num22 / 100.0;
							_gemSizesAdjusted[n] += num22 / 100.0;
						}
						num23 += 1.0;
					}
					eStatus = EStatus.FindFirst;
				}
			}
			if (eStatus == EStatus.FoundGem)
			{
				eStatus = EStatus.FindNext;
			}
		}
		if (_vMirror && _gemIndices.Count >= 2)
		{
			int num24 = _gemIndices.Count - 2;
			double item = _gemSizes[num24];
			double num16 = _gemSizesAdjusted[num24];
			double item2 = _gemZOffsets[num24];
			double num25 = (_crvPoints[GetFixedCurvePoint(_gemIndices[num24 + 1])] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[num24])]).Length() - (_gemSizesAdjusted[num24 + 1] + num16);
			num24--;
			eStatus = EStatus.FindFirst;
			while (eStatus != EStatus.Finished)
			{
				num14 += num6;
				num12++;
				if (!_crvMaster.IsClosed() && num14 != GetFixedCurvePoint(num14))
				{
					eStatus = EStatus.Finished;
				}
				if (num12 > _crvPointCount)
				{
					eStatus = EStatus.Finished;
				}
				if (eStatus == EStatus.Finished)
				{
					continue;
				}
				eStatus = EStatus.FoundGem;
				if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[0])]).Length() - (_gemSizesAdjusted[0] + num16) < 0.0)
				{
					eStatus = EStatus.FindFirst;
				}
				for (int num26 = 1; num26 < _gemIndices.Count; num26++)
				{
					if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[num26])]).Length() - (_gemSizesAdjusted[num26] + num16) < num25)
					{
						eStatus = EStatus.FindFirst;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_gemIndices.Add(GetFixedCurvePoint(num14));
					_gemSizes.Add(item);
					_gemSizesAdjusted.Add(num16);
					_gemZOffsets.Add(item2);
					if (num24 >= 0)
					{
						item = _gemSizes[num24];
						num16 = _gemSizesAdjusted[num24];
						item2 = _gemZOffsets[num24];
						num25 = (_crvPoints[GetFixedCurvePoint(_gemIndices[num24 + 1])] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(_gemIndices[num24])]).Length() - (_gemSizesAdjusted[num24 + 1] + num16);
						num24--;
					}
					else
					{
						eStatus = EStatus.Finished;
					}
				}
			}
		}
		UpdateBuilderAux();
	}

	private void UpdateBuilderAux()
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		List<GemOnCurveInfo> list = _gemIndices.Select((int t, int i) => new GemOnCurveInfo((IOn3dPoint)(object)_crvPoints[t], (IOn3dVector)(object)_crvTangents[t], _gemSizes[i], _gemZOffsets[i])).ToList();
		_spacingActual = _vSpacing;
		for (int num = 1; num < list.Count - 1; num++)
		{
			_spacingActual = (_spacingActual + list[num].Pt.DistanceTo(list[num - 1].Pt) - (list[num].Radius + list[num - 1].Radius)) / 2.0;
		}
		_outputGems = new CSDataArray();
		foreach (GemOnCurveInfo item in list)
		{
			OnXform xform = CalculateGemXform(item);
			CSData data = _sourceGemInstance.Duplicate();
			CSDataHelper.Transform(data, xform.Convert());
			_outputGems.Append(data);
		}
	}
}
