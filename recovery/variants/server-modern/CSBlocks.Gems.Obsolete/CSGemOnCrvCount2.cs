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
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		_sourceGemInst = InGemDefinition.Gem;
		if (_sourceGemInst == null)
		{
			return UpdateMissing("Gem");
		}
		_crvPrimary = (OnCurve)(object)InCurve.NurbsCurve.Convert();
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
		if (!GetClosestIndexToPosition((IOnCurve)(object)_crvPrimary, _crvParams, _vParamStart, ref index))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int index2 = -1;
		if (!GetClosestIndexToPosition((IOnCurve)(object)_crvPrimary, _crvParams, _vParamEnd, ref index2))
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
		foreach (GemOnCurveInfo item in _gemIndices.Select((int t, int index3) => new GemOnCurveInfo((IOn3dPoint)(object)_crvPoints[t], (IOn3dVector)(object)_crvTangents[t], _radii[index3], _zOffsets[index3])).ToList())
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
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		double num = gemOnCurveInfo.Radius / _sourceGemInst.GemXMax;
		OnXform val = new OnXform();
		val.Scale(num, num, num);
		OnXform val2 = new OnXform();
		val2.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform val3 = new OnXform();
		val3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), OnUtil.On_xaxis, OnUtil.On_origin);
		CSGemData cSGemData = new CSGemData(_sourceGemInst);
		cSGemData.ApplyTransform(val.Convert());
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, (CSUtilGem.EGemPlacementMethod)_vPlacementMethod, ref zOffset);
		OnXform val4 = new OnXform();
		val4.Translation(0.0, 0.0, zOffset + gemOnCurveInfo.OffsetZ);
		IOn3dPoint pt = gemOnCurveInfo.Pt;
		IOn3dVector tan = gemOnCurveInfo.Tan;
		On3dVector val5 = (_vFlip ? new On3dVector(0.0, 0.0, -1.0) : new On3dVector(0.0, 0.0, 1.0));
		On3dVector val6 = (_vFlip ? new On3dVector(-1.0, 0.0, 0.0) : new On3dVector(1.0, 0.0, 0.0));
		OnXform val7;
		switch (_vPointAt)
		{
		case 0:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
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
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		default:
		{
			On3dVector destination = new On3dVector(OnUtil.On_zaxis);
			val7 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)(object)val5, (IOn3dVector)(object)val6, pt, (IOn3dVector)(object)destination, tan);
			break;
		}
		}
		return val7 * (IOnXform)(object)val4 * (IOnXform)(object)val2 * (IOnXform)(object)val3 * (IOnXform)(object)val;
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
		OnInterval val = crv.Domain();
		double param = val.Min() + val.Length() * pos;
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
