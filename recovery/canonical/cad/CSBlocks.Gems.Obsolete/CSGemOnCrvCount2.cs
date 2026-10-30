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
using Rhino;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrvCount_V2", "{D24FB0D6-B2B4-4423-9073-40A54357D1C0}")]
public sealed class CSGemOnCrvCount2 : CSBlock
{
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

	private CSGemData _sourceGemInst;

	private OnCurve _crvPrimary;

	private OnBrep _srf;

	private double _vDiameterStart;

	private double _vDiameterEnd;

	private double _vParamStart;

	private double _vParamEnd;

	private double _vZOffsetStart;

	private double _vZOffsetEnd;

	private double _vRoll;

	private double _vTilt;

	private int _vCount;

	private int _vPlacementMethod;

	private int _vPointAt;

	private bool _vDirection;

	private bool _vFlip;

	private bool _vMirror;

	private bool _vTaperDiameter;

	private bool _vTaperZOffset;

	private CSDataArray _outputGemInsts;

	private readonly List<On3dVector> _crvTangents = new List<On3dVector>();

	private readonly List<On3dPoint> _crvPoints = new List<On3dPoint>();

	private readonly List<double> _crvParams = new List<double>();

	private List<double> _radii;

	private List<double> _zOffsets;

	private List<int> _gemIndices;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InSurface;

	public readonly CSInputChannel InCount;

	public readonly CSInputChannel InDiameterEnd;

	public readonly CSInputChannel InPositionStart;

	public readonly CSInputChannel InPositionEnd;

	public readonly CSInputChannel InZOffsetStart;

	public readonly CSInputChannel InZOffsetEnd;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InDirection;

	public readonly CSInputChannel InFlip;

	public readonly CSInputChannel InMirror;

	public readonly CSInputChannel InTaperDiameter;

	public readonly CSInputChannel InTaperZOffset;

	public readonly CSInputChannel InPointAt;

	public readonly CSOutputChannel OutArray;

	public CSGemOnCrvCount2(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array, ChannelFlag.NotRequired);
		InCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 1);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InPositionStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPositionEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InZOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperDiameter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperZOffset = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InPointAt = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		_sourceGemInst = InGemDefinition.Gem;
		if (_sourceGemInst == null)
		{
			return UpdateMissing("Gem");
		}
		_crvPrimary = InCurve.NurbsCurve.Convert();
		if (_crvPrimary == null)
		{
			return UpdateMissing("Curve");
		}
		_srf = null;
		if (InSurface.GetFlattenArray().GetArrayCount() > 0)
		{
			_srf = InSurface.Brep.Convert();
		}
		_vDiameterStart = _sourceGemInst.GemXSize;
		_vDiameterEnd = InDiameterEnd.Double;
		_vParamStart = InPositionStart.Double;
		_vParamEnd = InPositionEnd.Double;
		_vZOffsetStart = InZOffsetStart.Double;
		_vZOffsetEnd = InZOffsetEnd.Double;
		_vRoll = InRoll.Double;
		_vTilt = InTilt.Double;
		_vCount = InCount.Integer;
		_vPlacementMethod = InPlacementMethod.Integer;
		_vPointAt = InPointAt.Integer;
		_vDirection = InDirection.Boolean;
		_vFlip = InFlip.Boolean;
		_vMirror = InMirror.Boolean;
		_vTaperDiameter = InTaperDiameter.Boolean;
		_vTaperZOffset = InTaperZOffset.Boolean;
		_outputGemInsts = new CSDataArray();
		if (_srf == null && _vPointAt == 0)
		{
			_vPointAt = 2;
		}
		if (!_vTaperDiameter)
		{
			_vDiameterEnd = _vDiameterStart;
		}
		if (!_vTaperZOffset)
		{
			_vZOffsetEnd = _vZOffsetStart;
		}
		bool flag = false;
		if (_srf != null)
		{
			flag = BeenUpdatedAfterLastUpdate(InSurface.OutputChannelBlock);
		}
		if ((BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock) || flag || BeenUpdatedAfterLastUpdate(InGemDefinition.OutputChannelBlock) || _crvPoints.Count == 0) && !PrepCrvPrimary())
		{
			return UpdateError("PrepCrvPrimary failed");
		}
		int index = -1;
		if (!GetClosestIndexToPosition(_crvPrimary, _crvParams, _vParamStart, ref index))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int index2 = -1;
		if (!GetClosestIndexToPosition(_crvPrimary, _crvParams, _vParamEnd, ref index2))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int num;
		int num2;
		if (_vCount == 1)
		{
			num = 0;
			num2 = (index + index2) / 2;
		}
		else
		{
			int num3 = ((index <= index2) ? 1 : (-1));
			int num4 = ((!_vDirection) ? 1 : (-1));
			num3 *= num4;
			int num5 = Math.Abs(index2 - index);
			if (_vDirection)
			{
				num5 = _crvParams.Count - num5;
			}
			num = Math.Abs(num5 / (_vCount - 1)) * num3;
			num2 = index;
		}
		_gemIndices = new List<int>();
		for (int i = 0; i < _vCount; i++)
		{
			_gemIndices.Add(GetFixedIndex(num2 + i * num, _crvParams.Count));
		}
		double num6;
		double num7;
		if (_vCount == 1)
		{
			num6 = 0.0;
			num7 = (_vDiameterStart + _vDiameterEnd) / 2.0;
		}
		else
		{
			num6 = (_vDiameterEnd - _vDiameterStart) / (double)(_vCount - 1);
			num7 = _vDiameterStart;
		}
		_radii = new List<double>();
		for (int j = 0; j < _vCount; j++)
		{
			_radii.Add(Math.Round(num7 + (double)j * num6, 1, MidpointRounding.ToEven) / 2.0);
		}
		double num8;
		double num9;
		if (_vCount == 1)
		{
			num8 = 0.0;
			num9 = (_vZOffsetStart + _vZOffsetEnd) / 2.0;
		}
		else
		{
			num8 = (_vZOffsetEnd - _vZOffsetStart) / (double)(_vCount - 1);
			num9 = _vZOffsetStart;
		}
		_zOffsets = new List<double>();
		for (int k = 0; k < _vCount; k++)
		{
			_zOffsets.Add(num9 + (double)k * num8);
		}
		foreach (GemOnCurveInfo item in _gemIndices.Select((int t, int index3) => new GemOnCurveInfo(_crvPoints[t], _crvTangents[t], _radii[index3], _zOffsets[index3])).ToList())
		{
			OnXform xform = CalculateGemXform(item);
			CSData data = _sourceGemInst.Duplicate();
			CSDataHelper.Transform(data, xform.Convert());
			_outputGemInsts.Append(data);
		}
		OutArray.SetData(_outputGemInsts);
		return base.Update();
	}

	private OnXform CalculateGemXform(GemOnCurveInfo gemOnCurveInfo)
	{
		double num = gemOnCurveInfo.Radius / _sourceGemInst.GemXMax;
		OnXform onXform = new OnXform();
		onXform.Scale(num, num, num);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), OnUtil.On_xaxis, OnUtil.On_origin);
		CSGemData cSGemData = new CSGemData(_sourceGemInst);
		cSGemData.ApplyTransform(onXform.Convert());
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, (CSUtilGem.EGemPlacementMethod)_vPlacementMethod, ref zOffset);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.OffsetZ);
		IOn3dPoint pt = gemOnCurveInfo.Pt;
		IOn3dVector tan = gemOnCurveInfo.Tan;
		On3dVector on3dVector = (_vFlip ? new On3dVector(0.0, 0.0, -1.0) : new On3dVector(0.0, 0.0, 1.0));
		On3dVector on3dVector2 = (_vFlip ? new On3dVector(-1.0, 0.0, 0.0) : new On3dVector(1.0, 0.0, 0.0));
		OnXform onXform5;
		switch (_vPointAt)
		{
		case 0:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			CSUtilGeometry.GetClosestNormalToBrep(_srf, pt, ref destination);
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector, on3dVector2, pt, destination, tan);
			break;
		}
		case 1:
		{
			On3dVector destination = new On3dVector(pt);
			destination.Unitize();
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector2, on3dVector, pt, tan, destination);
			break;
		}
		case 2:
		{
			On3dVector destination = new On3dVector(pt.x, 0.0, pt.z);
			destination.Unitize();
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector2, on3dVector, pt, tan, destination);
			break;
		}
		case 3:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector, on3dVector2, pt, destination, tan);
			break;
		}
		default:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			onXform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, on3dVector, on3dVector2, pt, destination, tan);
			break;
		}
		}
		return onXform5 * onXform4 * onXform2 * onXform3 * onXform;
	}

	private static bool GetClosestIndexToParameter(IList<double> crvParams, double param, ref int index)
	{
		if (crvParams == null)
		{
			return false;
		}
		if (crvParams.Count <= 0)
		{
			return false;
		}
		double num = -1.23432101234321E+308;
		for (int i = 0; i < crvParams.Count; i++)
		{
			double num2 = Math.Abs(param - crvParams[i]);
			if (!RhinoMath.IsValidDouble(num) || !(num2 >= num))
			{
				num = num2;
				index = i;
			}
		}
		return true;
	}

	private static bool GetClosestIndexToPosition(IOnCurve crv, IList<double> crvParams, double pos, ref int index)
	{
		if (crv == null)
		{
			return false;
		}
		OnInterval onInterval = crv.Domain();
		double param = onInterval.Min() + onInterval.Length() * pos;
		return GetClosestIndexToParameter(crvParams, param, ref index);
	}

	private static int GetFixedIndex(int index, int count)
	{
		int i = index;
		if (count == 0)
		{
			return 0;
		}
		for (; i < 0; i += count)
		{
		}
		return i % count;
	}

	private bool PrepCrvPrimary()
	{
		if (_crvPrimary == null)
		{
			return false;
		}
		double length = 0.0;
		if (!_crvPrimary.GetLength(ref length))
		{
			return false;
		}
		int num = (int)(length * 50.0);
		double num2 = num;
		int num3 = num + 1;
		_crvParams.Clear();
		_crvPoints.Clear();
		_crvTangents.Clear();
		double[] t = new double[num3];
		List<double> list = new List<double>();
		for (double num4 = 0.0; num4 < (double)num3; num4 += 1.0)
		{
			list.Add(num4 / num2);
		}
		if (!_crvPrimary.GetNormalizedArcLengthPoints(list.ToArray(), ref t))
		{
			return false;
		}
		double[] array = t;
		foreach (double num5 in array)
		{
			On3dPoint point = new On3dPoint();
			On3dVector first_derivative = new On3dVector();
			_crvPrimary.Ev1Der(num5, ref point, ref first_derivative);
			_crvParams.Add(num5);
			_crvPoints.Add(point);
			_crvTangents.Add(first_derivative);
		}
		return true;
	}
}
