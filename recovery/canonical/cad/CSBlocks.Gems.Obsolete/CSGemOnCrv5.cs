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
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrv_V5", "{EEB6AB1E-7457-415c-9D1F-161A9D481827}")]
public sealed class CSGemOnCrv5 : CSBlock
{
	private enum EStatus
	{
		FindStart,
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

	public readonly CSOutputChannel OutArray;

	private CSGemData _sourceGemInst;

	private OnCurve _crvPrimary;

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

	private bool _vDirection;

	private bool _vFlip;

	private bool _vMirror;

	private bool _vTaperDiameter;

	private bool _vTaperSpacing;

	private bool _vTaperZOffset;

	private int _vPointAt;

	private CSDataArray _outputGemInsts;

	private readonly List<On3dPoint> _crvPoints = new List<On3dPoint>();

	private readonly List<On3dVector> _crvTangents = new List<On3dVector>();

	private readonly List<double> _crvParams = new List<double>();

	private List<double> _radii;

	private List<double> _radiiAdjusted;

	private List<double> _zOffsets;

	private List<int> _gemIndices;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InSurface;

	public readonly CSInputChannel InDiameterEnd;

	public readonly CSInputChannel InPositionStart;

	public readonly CSInputChannel InPositionEnd;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InZOffsetStart;

	public readonly CSInputChannel InZOffsetEnd;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InSpacingMethod;

	public readonly CSInputChannel InDirection;

	public readonly CSInputChannel InFlip;

	public readonly CSInputChannel InMirror;

	public readonly CSInputChannel InTaperDiameter;

	public readonly CSInputChannel InTaperSpacing;

	public readonly CSInputChannel InTaperZOffset;

	public readonly CSInputChannel InPointAt;

	public CSGemOnCrv5(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array, ChannelFlag.NotRequired);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InPositionStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPositionEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InSpacingMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
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
		_sourceGemInst = InGemDefinition.Gem;
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
		_vPlacementMethod = InPlacementMethod.Integer;
		_vSpacingMethod = InSpacingMethod.Integer;
		_vDiameterStart = _sourceGemInst.GemXSize;
		_vDiameterEnd = InDiameterEnd.Double;
		_vParamStart = InPositionStart.Double;
		_vParamEnd = InPositionEnd.Double;
		_vSpacing = InSpacing.Double;
		_vZOffsetStart = InZOffsetStart.Double;
		_vZOffsetEnd = InZOffsetEnd.Double;
		_vRoll = InRoll.Double;
		_vTilt = InTilt.Double;
		_vPointAt = InPointAt.Integer;
		_vDirection = InDirection.Boolean;
		_vFlip = InFlip.Boolean;
		_vMirror = InMirror.Boolean;
		_vTaperDiameter = InTaperDiameter.Boolean;
		_vTaperSpacing = InTaperSpacing.Boolean;
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
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), Vector3d.YAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), Vector3d.ZAxis, Point3d.Origin);
		double num = _vDiameterStart / 2.0 / _sourceGemInst.GemXMax;
		Transform transform3 = Transform.Scale(Plane.WorldXY, num, num, num);
		CSGemData cSGemData = new CSGemData(_sourceGemInst);
		cSGemData.ApplyTransform(transform2 * transform * transform3);
		BoundingBox boundingBox = BoundingBox.Unset;
		cSGemData.GetTightBoundingBox(ref boundingBox);
		double num2 = boundingBox.Max.X - boundingBox.Min.X;
		double num4;
		if (_vTaperDiameter)
		{
			double num3 = _vDiameterEnd / 2.0 / _sourceGemInst.GemXMax;
			Transform transform4 = Transform.Scale(Plane.WorldXY, num3, num3, num3);
			CSGemData cSGemData2 = new CSGemData(_sourceGemInst);
			cSGemData2.ApplyTransform(transform2 * transform * transform4);
			BoundingBox boundingBox2 = BoundingBox.Unset;
			cSGemData2.GetTightBoundingBox(ref boundingBox2);
			num4 = boundingBox2.Max.X - boundingBox2.Min.X;
		}
		else
		{
			num4 = num2;
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
		int num5 = ((index <= index2) ? 1 : (-1));
		int num6 = ((!_vDirection) ? 1 : (-1));
		num5 *= num6;
		double num7 = (num2 / 2.0 + _vSpacing / 2.0) / (num2 / 2.0);
		int num8 = 0;
		int num9 = index - num5;
		int num10 = 0;
		int num11 = Math.Abs(index2 - index);
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
			if (GetFixedIndex(num9, _crvParams.Count) == index2)
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
							_radii[i] += Math.Round(num13 / 100.0, 1, MidpointRounding.ToEven);
							_radiiAdjusted[i] += num13 / 100.0;
						}
						num14 += 1.0;
					}
				}
				eStatus = EStatus.Finished;
			}
			double value = _vDiameterStart / 2.0 + (_vDiameterEnd / 2.0 - _vDiameterStart / 2.0) * ((double)num10 / (double)num11);
			value = Math.Round(value, 1, MidpointRounding.ToEven);
			double num15 = num2 / 2.0 + (num4 / 2.0 - num2 / 2.0) * ((double)num10 / (double)num11);
			double item = _vZOffsetStart + (_vZOffsetEnd - _vZOffsetStart) * ((double)num10 / (double)num11);
			if (eStatus == EStatus.FindStart)
			{
				eStatus = EStatus.FoundGem;
				for (int j = 0; j < _gemIndices.Count; j++)
				{
					double num16 = ((!_vTaperSpacing) ? (num15 + _radiiAdjusted[j] + _vSpacing) : ((num15 + _radiiAdjusted[j]) * num7));
					if (!((_crvPoints[GetFixedIndex(num9, _crvParams.Count)] - _crvPoints[GetFixedIndex(_gemIndices[j], _crvParams.Count)]).LengthSquared() >= num16 * num16))
					{
						eStatus = EStatus.FindStart;
						break;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_radii.Add(value);
					_radiiAdjusted.Add(num15);
					_zOffsets.Add(item);
					_gemIndices.Add(num9);
					num8 = _gemIndices.Count - 1;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				double num16 = ((!_vTaperSpacing) ? (num15 + _radiiAdjusted[_gemIndices.Count - 1] + _vSpacing) : ((num15 + _radiiAdjusted[_gemIndices.Count - 1]) * num7));
				if ((_crvPoints[GetFixedIndex(num9, _crvParams.Count)] - _crvPoints[GetFixedIndex(_gemIndices[_gemIndices.Count - 1], _crvParams.Count)]).Length() > num16)
				{
					_radii.Add(value);
					_radiiAdjusted.Add(num15);
					_zOffsets.Add(item);
					_gemIndices.Add(num9);
					eStatus = EStatus.FoundGem;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				int num17 = -1;
				for (int k = 0; k < _gemIndices.Count - 1; k++)
				{
					double num16 = ((!_vTaperSpacing) ? (num15 + _radiiAdjusted[k] + _vSpacing) : ((num15 + _radiiAdjusted[k]) * num7));
					if (!((_crvPoints[GetFixedIndex(num9, _crvParams.Count)] - _crvPoints[GetFixedIndex(_gemIndices[k], _crvParams.Count)]).LengthSquared() >= num16 * num16))
					{
						num17 = k;
						break;
					}
				}
				if (num17 >= 0)
				{
					if (num17 > num8)
					{
						num8 = num17 + 1;
					}
					if (num8 == 0)
					{
						num8 = 1;
					}
					double num18 = num9 - _gemIndices[_gemIndices.Count - 1];
					double num19;
					double num20;
					if (_vSpacingMethod == 1)
					{
						num19 = num18 / (double)(_gemIndices.Count - num8);
						num20 = 0.5;
					}
					else
					{
						num19 = num18 / ((double)(_gemIndices.Count - num8) + 1.0);
						num20 = 1.0;
					}
					for (int l = num8; l < _gemIndices.Count; l++)
					{
						if (_vSpacingMethod < 2)
						{
							_gemIndices[l] += (int)(num19 * num20);
						}
						if (_vSpacingMethod == 1)
						{
							_radii[l] += Math.Round(num19 / 100.0, 1, MidpointRounding.ToEven);
							_radiiAdjusted[l] += num19 / 100.0;
						}
						num20 += 1.0;
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
			int num21 = _gemIndices.Count - 2;
			double item2 = _radii[num21];
			double num22 = _radiiAdjusted[num21];
			double item3 = _zOffsets[num21];
			double num23 = (_crvPoints[GetFixedIndex(_gemIndices[num21 + 1], _crvParams.Count)] - _crvPoints[GetFixedIndex(_gemIndices[num21], _crvParams.Count)]).Length() - (_radiiAdjusted[num21 + 1] + num22);
			num21--;
			eStatus = EStatus.FindStart;
			while (eStatus != EStatus.Finished)
			{
				num9 += num5;
				num10++;
				if (!_crvPrimary.IsClosed() && num9 != GetFixedIndex(num9, _crvParams.Count))
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
				if ((_crvPoints[GetFixedIndex(num9, _crvParams.Count)] - _crvPoints[GetFixedIndex(_gemIndices[0], _crvParams.Count)]).Length() - (_radiiAdjusted[0] + num22) < 0.0)
				{
					eStatus = EStatus.FindStart;
				}
				for (int m = 1; m < _gemIndices.Count; m++)
				{
					if ((_crvPoints[GetFixedIndex(num9, _crvParams.Count)] - _crvPoints[GetFixedIndex(_gemIndices[m], _crvParams.Count)]).Length() - (_radiiAdjusted[m] + num22) < num23)
					{
						eStatus = EStatus.FindStart;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_radii.Add(item2);
					_radiiAdjusted.Add(num22);
					_zOffsets.Add(item3);
					_gemIndices.Add(GetFixedIndex(num9, _crvParams.Count));
					if (num21 >= 0)
					{
						item2 = _radii[num21];
						num22 = _radiiAdjusted[num21];
						item3 = _zOffsets[num21];
						num23 = (_crvPoints[GetFixedIndex(_gemIndices[num21 + 1], _crvParams.Count)] - _crvPoints[GetFixedIndex(_gemIndices[num21], _crvParams.Count)]).Length() - (_radiiAdjusted[num21 + 1] + num22);
						num21--;
					}
					else
					{
						eStatus = EStatus.Finished;
					}
				}
			}
		}
		foreach (GemOnCurveInfo item4 in _gemIndices.Select((int t, int index3) => new GemOnCurveInfo(_crvPoints[t], _crvTangents[t], _radii[index3], _zOffsets[index3])).ToList())
		{
			OnXform xform = CalculateGemXform(item4);
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
