using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrv_V6", "{60CDD51C-592E-4b26-8929-BBA9B895B8DB}")]
public sealed class CSGemOnCrv6 : CSBlock
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

	private CSGemData _sourceGemInst;

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

	private int _vPlacementMethod;

	private int _vPointAt;

	private int _vSpacingMethod;

	private bool _vDirection;

	private bool _vFlip;

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

	public readonly CSOutputChannel OutArray;

	public CSGemOnCrv6(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
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
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8b: Unknown result type (might be due to invalid IL or missing references)
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
		_vSpacing = InSpacing.Double;
		_vZOffsetStart = InZOffsetStart.Double;
		_vZOffsetEnd = InZOffsetEnd.Double;
		_vRoll = InRoll.Double;
		_vTilt = InTilt.Double;
		_vPlacementMethod = InPlacementMethod.Integer;
		_vPointAt = InPointAt.Integer;
		_vSpacingMethod = InSpacingMethod.Integer;
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
		Transform val = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vTilt), Vector3d.YAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(_vRoll), Vector3d.ZAxis, Point3d.Origin);
		double num = _vDiameterStart / 2.0 / _sourceGemInst.GemXMax;
		Transform val3 = Transform.Scale(Plane.WorldXY, num, num, num);
		CSGemData cSGemData = new CSGemData(_sourceGemInst);
		cSGemData.ApplyTransform(val2 * val * val3);
		BoundingBox boundingBox = BoundingBox.Unset;
		cSGemData.GetTightBoundingBox(ref boundingBox);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		double num3;
		if (_vTaperDiameter)
		{
			double num2 = _vDiameterEnd / 2.0 / _sourceGemInst.GemXMax;
			Transform val4 = Transform.Scale(Plane.WorldXY, num2, num2, num2);
			CSGemData cSGemData2 = new CSGemData(_sourceGemInst);
			cSGemData2.ApplyTransform(val2 * val * val4);
			BoundingBox boundingBox2 = BoundingBox.Unset;
			cSGemData2.GetTightBoundingBox(ref boundingBox2);
			Point3d val5 = ((BoundingBox)(ref boundingBox2)).Max;
			double x2 = ((Point3d)(ref val5)).X;
			val5 = ((BoundingBox)(ref boundingBox2)).Min;
			num3 = x2 - ((Point3d)(ref val5)).X;
		}
		else
		{
			num3 = x;
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
		int num4 = ((index <= index2) ? 1 : (-1));
		int num5 = ((!_vDirection) ? 1 : (-1));
		num4 *= num5;
		double num6 = (x / 2.0 + _vSpacing / 2.0) / (x / 2.0);
		int num7 = 0;
		int num8 = index - num4;
		int num9 = 0;
		int num10 = Math.Abs(index2 - index);
		if (num5 == -1)
		{
			num10 = _crvPoints.Count - num10;
		}
		_radii = new List<double>();
		_radiiAdjusted = new List<double>();
		_zOffsets = new List<double>();
		_gemIndices = new List<int>();
		EStatus eStatus = EStatus.FindStart;
		while (eStatus != EStatus.Finished)
		{
			num8 += num4;
			num9++;
			if (GetFixedIndex(num8, _crvParams.Count) == index2)
			{
				if (eStatus == EStatus.FindNext)
				{
					if (num7 == 0)
					{
						num7 = 1;
					}
					double num11 = num8 - _gemIndices[_gemIndices.Count - 1];
					double num12;
					double num13;
					if (_vSpacingMethod == 1)
					{
						num12 = num11 / ((double)(_gemIndices.Count - num7) - 0.5);
						num13 = 0.5;
					}
					else
					{
						num12 = num11 / (double)(_gemIndices.Count - num7);
						num13 = 1.0;
					}
					for (int i = num7; i < _gemIndices.Count; i++)
					{
						if (_vSpacingMethod < 2)
						{
							_gemIndices[i] += (int)(num12 * num13);
						}
						if (_vSpacingMethod == 1)
						{
							double num14 = num12 / 100.0;
							_radii[i] = Math.Round((_radii[i] + num14) * 2.0, 1, MidpointRounding.ToEven) / 2.0;
							_radiiAdjusted[i] = (_radiiAdjusted[i] + num14) * 2.0;
						}
						num13 += 1.0;
					}
				}
				eStatus = EStatus.Finished;
			}
			double value = _vDiameterStart + (_vDiameterEnd - _vDiameterStart) * ((double)num9 / (double)num10);
			double num15 = x + (num3 - x) * ((double)num9 / (double)num10);
			double item = Math.Round(value, 1, MidpointRounding.ToEven) / 2.0;
			double num16 = num15 / 2.0;
			double item2 = _vZOffsetStart + (_vZOffsetEnd - _vZOffsetStart) * ((double)num9 / (double)num10);
			if (eStatus == EStatus.FindStart)
			{
				eStatus = EStatus.FoundGem;
				for (int j = 0; j < _gemIndices.Count; j++)
				{
					double num17 = ((!_vTaperSpacing) ? (num16 + _radiiAdjusted[j] + _vSpacing) : ((num16 + _radiiAdjusted[j]) * num6));
					if (!((_crvPoints[GetFixedIndex(num8, _crvParams.Count)] - (IOn3dPoint)(object)_crvPoints[GetFixedIndex(_gemIndices[j], _crvParams.Count)]).LengthSquared() >= num17 * num17))
					{
						eStatus = EStatus.FindStart;
						break;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_radii.Add(item);
					_radiiAdjusted.Add(num16);
					_zOffsets.Add(item2);
					_gemIndices.Add(num8);
					num7 = _gemIndices.Count - 1;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				double num17 = ((!_vTaperSpacing) ? (num16 + _radiiAdjusted[_gemIndices.Count - 1] + _vSpacing) : ((num16 + _radiiAdjusted[_gemIndices.Count - 1]) * num6));
				if ((_crvPoints[GetFixedIndex(num8, _crvParams.Count)] - (IOn3dPoint)(object)_crvPoints[GetFixedIndex(_gemIndices[_gemIndices.Count - 1], _crvParams.Count)]).Length() > num17)
				{
					_radii.Add(item);
					_radiiAdjusted.Add(num16);
					_zOffsets.Add(item2);
					_gemIndices.Add(num8);
					eStatus = EStatus.FoundGem;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				int num18 = -1;
				for (int k = 0; k < _gemIndices.Count - 1; k++)
				{
					double num17 = ((!_vTaperSpacing) ? (num16 + _radiiAdjusted[k] + _vSpacing) : ((num16 + _radiiAdjusted[k]) * num6));
					if (!((_crvPoints[GetFixedIndex(num8, _crvParams.Count)] - (IOn3dPoint)(object)_crvPoints[GetFixedIndex(_gemIndices[k], _crvParams.Count)]).LengthSquared() >= num17 * num17))
					{
						num18 = k;
						break;
					}
				}
				if (num18 >= 0)
				{
					if (num18 > num7)
					{
						num7 = num18 + 1;
					}
					if (num7 == 0)
					{
						num7 = 1;
					}
					double num19 = num8 - _gemIndices[_gemIndices.Count - 1];
					double num20;
					double num21;
					if (_vSpacingMethod == 1)
					{
						num20 = num19 / (double)(_gemIndices.Count - num7);
						num21 = 0.5;
					}
					else
					{
						num20 = num19 / ((double)(_gemIndices.Count - num7) + 1.0);
						num21 = 1.0;
					}
					for (int l = num7; l < _gemIndices.Count; l++)
					{
						if (_vSpacingMethod < 2)
						{
							_gemIndices[l] += (int)(num20 * num21);
						}
						if (_vSpacingMethod == 1)
						{
							double num22 = num20 / 100.0;
							_radii[l] = Math.Round((_radii[l] + num22) * 2.0, 1, MidpointRounding.ToEven) / 2.0;
							_radiiAdjusted[l] = (_radiiAdjusted[l] + num22) * 2.0;
						}
						num21 += 1.0;
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
			int num23 = _gemIndices.Count - 2;
			double item3 = _radii[num23];
			double num24 = _radiiAdjusted[num23];
			double item4 = _zOffsets[num23];
			double num25 = (_crvPoints[GetFixedIndex(_gemIndices[num23 + 1], _crvParams.Count)] - (IOn3dPoint)(object)_crvPoints[GetFixedIndex(_gemIndices[num23], _crvParams.Count)]).Length() - (_radiiAdjusted[num23 + 1] + num24);
			num23--;
			eStatus = EStatus.FindStart;
			while (eStatus != EStatus.Finished)
			{
				num8 += num4;
				num9++;
				if (!_crvPrimary.IsClosed() && num8 != GetFixedIndex(num8, _crvParams.Count))
				{
					eStatus = EStatus.Finished;
				}
				if (num9 > _crvPoints.Count)
				{
					eStatus = EStatus.Finished;
				}
				if (eStatus == EStatus.Finished)
				{
					continue;
				}
				eStatus = EStatus.FoundGem;
				if ((_crvPoints[GetFixedIndex(num8, _crvParams.Count)] - (IOn3dPoint)(object)_crvPoints[GetFixedIndex(_gemIndices[0], _crvParams.Count)]).Length() - (_radiiAdjusted[0] + num24) < 0.0)
				{
					eStatus = EStatus.FindStart;
				}
				for (int m = 1; m < _gemIndices.Count; m++)
				{
					if ((_crvPoints[GetFixedIndex(num8, _crvParams.Count)] - (IOn3dPoint)(object)_crvPoints[GetFixedIndex(_gemIndices[m], _crvParams.Count)]).Length() - (_radiiAdjusted[m] + num24) < num25)
					{
						eStatus = EStatus.FindStart;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					_radii.Add(item3);
					_radiiAdjusted.Add(num24);
					_zOffsets.Add(item4);
					_gemIndices.Add(GetFixedIndex(num8, _crvParams.Count));
					if (num23 >= 0)
					{
						item3 = _radii[num23];
						num24 = _radiiAdjusted[num23];
						item4 = _zOffsets[num23];
						num25 = (_crvPoints[GetFixedIndex(_gemIndices[num23 + 1], _crvParams.Count)] - (IOn3dPoint)(object)_crvPoints[GetFixedIndex(_gemIndices[num23], _crvParams.Count)]).Length() - (_radiiAdjusted[num23 + 1] + num24);
						num23--;
					}
					else
					{
						eStatus = EStatus.Finished;
					}
				}
			}
		}
		List<GemOnCurveInfo> list = new List<GemOnCurveInfo>();
		for (int n = 0; n < _gemIndices.Count; n++)
		{
			int fixedIndex = GetFixedIndex(_gemIndices[n], _crvParams.Count);
			On3dPoint pt = _crvPoints[fixedIndex];
			On3dVector tan = _crvTangents[fixedIndex];
			double radius = _radii[n];
			double offsetZ = _zOffsets[n];
			GemOnCurveInfo item5 = new GemOnCurveInfo((IOn3dPoint)(object)pt, (IOn3dVector)(object)tan, radius, offsetZ);
			list.Add(item5);
		}
		foreach (GemOnCurveInfo item6 in list)
		{
			OnXform xform = CalculateGemXform(item6);
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

	private static bool GetClosestIndexToPosition(IOnCurve crv, IList<double> crvParams, double pos, ref int index)
	{
		if (crv == null)
		{
			return false;
		}
		double param = crv.Domain().ParameterAt(pos);
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
