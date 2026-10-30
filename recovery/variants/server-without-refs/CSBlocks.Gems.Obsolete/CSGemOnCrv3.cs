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

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrv_V3", "{E65CD145-E6FF-49d6-AE40-95132777CE17}")]
public sealed class CSGemOnCrv3 : CSBlock
{
	private enum EStatus
	{
		FindFirst,
		FindNext,
		FoundGem,
		Finished
	}

	private int _crvPointCount;

	private CSInputChannel _crvMasterChannel;

	private OnCurve _crvMaster;

	private CSInputChannel _srfMasterChannel;

	private double _crvLength;

	private double[] _normalizedParams;

	private double[] _unnormalizedParams;

	private On3dPoint[] _crvPoints;

	private On3dVector[] _crvNormals;

	private On3dVector[] _crvTangents;

	public readonly CSOutputChannel OutArray;

	public CSInputChannel InGemDefinition;

	public CSInputChannel InCurve;

	public CSInputChannel InSurface;

	public CSInputChannel InDiameterEnd;

	public CSInputChannel InParamStart;

	public CSInputChannel InParamEnd;

	public CSInputChannel InSpacing;

	public CSInputChannel InZOffsetStart;

	public CSInputChannel InZOffsetEnd;

	public CSInputChannel InFlip;

	public CSInputChannel InMirror;

	public CSInputChannel InTaperDiameter;

	public CSInputChannel InTaperSpacing;

	public CSInputChannel InTaperOffset;

	public CSInputChannel InPlacementMethod;

	public CSInputChannel InSpacingMethod;

	public CSGemOnCrv3(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperDiameter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperSpacing = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaperOffset = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InSpacingMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_0b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Expected O, but got Unknown
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b00: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c41: Expected O, but got Unknown
		//IL_0c41: Expected O, but got Unknown
		//IL_0c41: Expected O, but got Unknown
		//IL_0b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Expected O, but got Unknown
		//IL_0bd4: Expected O, but got Unknown
		//IL_0bd4: Expected O, but got Unknown
		//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4a: Expected O, but got Unknown
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8e: Unknown result type (might be due to invalid IL or missing references)
		List<int> list = new List<int>();
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		CSGemData gem = InGemDefinition.Gem;
		if (gem == null)
		{
			return UpdateMissing("Gem");
		}
		bool flag = false;
		if (InCurve != null)
		{
			flag = BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock);
		}
		if (InSurface != null)
		{
			flag |= BeenUpdatedAfterLastUpdate(InSurface.OutputChannelBlock);
		}
		if (_crvMaster == null)
		{
			flag = true;
		}
		if (_normalizedParams == null)
		{
			flag = true;
		}
		if (flag || _crvMasterChannel != InCurve || _srfMasterChannel != InSurface)
		{
			_crvMasterChannel = InCurve;
			_srfMasterChannel = InSurface;
			_crvMaster = (OnCurve)(object)InCurve.NurbsCurve.Convert();
			if (_crvMaster == null)
			{
				return UpdateMissing("Curve");
			}
			_crvMaster.GetLength(ref _crvLength);
			_crvPointCount = (int)(_crvLength * 50.0);
			_normalizedParams = new double[_crvPointCount + 1];
			_unnormalizedParams = new double[_crvPointCount + 1];
			_crvPoints = (On3dPoint[])(object)new On3dPoint[_crvPointCount + 1];
			_crvNormals = (On3dVector[])(object)new On3dVector[_crvPointCount + 1];
			_crvTangents = (On3dVector[])(object)new On3dVector[_crvPointCount + 1];
			for (int i = 0; i <= _crvPointCount; i++)
			{
				_normalizedParams[i] = (double)i / (double)_crvPointCount;
			}
			_crvPointCount++;
			_crvMaster.GetNormalizedArcLengthPoints(_normalizedParams, ref _unnormalizedParams);
			for (int j = 0; j < _crvPointCount; j++)
			{
				On3dPoint val = new On3dPoint();
				On3dVector val2 = new On3dVector();
				_crvMaster.Ev1Der(_unnormalizedParams[j], ref val, ref val2);
				_crvPoints[j] = val;
				_crvTangents[j] = val2;
			}
			for (int k = 0; k < _crvPointCount; k++)
			{
				_crvNormals[k] = null;
			}
		}
		OnBrep val3 = InSurface.Brep.Convert();
		if (val3 == null)
		{
			UpdateMissing("Surface");
		}
		int integer = InPlacementMethod.Integer;
		int integer2 = InSpacingMethod.Integer;
		double num = InParamStart.Double;
		double num2 = InParamEnd.Double;
		double num3 = InSpacing.Double;
		double num4 = InZOffsetStart.Double;
		double num5 = InZOffsetEnd.Double;
		double gemXSize = gem.GemXSize;
		double num6 = InDiameterEnd.Double;
		bool boolean = InFlip.Boolean;
		bool boolean2 = InMirror.Boolean;
		bool boolean3 = InTaperDiameter.Boolean;
		bool boolean4 = InTaperSpacing.Boolean;
		bool boolean5 = InTaperOffset.Boolean;
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		OnXform val4 = new OnXform();
		val4.Translation(0.0, 0.0, zOffset);
		int num7 = 1;
		bool flag2 = _crvMaster.IsClosed();
		if (!boolean3)
		{
			num6 = gemXSize;
		}
		if (!boolean5)
		{
			num5 = num4;
		}
		OnInterval val5 = _crvMaster.Domain();
		double num8 = val5.Min() + val5.Length() * num;
		int num9 = 0;
		double num10 = 999.0;
		for (int l = 0; l < _crvPointCount; l++)
		{
			if (Math.Abs(num8 - _unnormalizedParams[l]) < num10)
			{
				num10 = Math.Abs(num8 - _unnormalizedParams[l]);
				num9 = l;
			}
		}
		num8 = val5.Min() + val5.Length() * num2;
		int num11 = 0;
		num10 = 999.0;
		for (int m = 0; m < _crvPointCount; m++)
		{
			if (Math.Abs(num8 - _unnormalizedParams[m]) < num10)
			{
				num10 = Math.Abs(num8 - _unnormalizedParams[m]);
				num11 = m;
			}
		}
		int num12 = ((num9 <= num11) ? 1 : (-1));
		num12 *= num7;
		double num13 = (gemXSize / 2.0 + num3 / 2.0) / (gemXSize / 2.0);
		EStatus eStatus = EStatus.FindFirst;
		int num14 = 0;
		int num15 = 0;
		int num16 = 0;
		list.Clear();
		list2.Clear();
		list3.Clear();
		num14 = num9 - num12;
		int num17 = 0;
		num17 = Math.Abs(num11 - num9);
		if (num7 == -1)
		{
			num17 = _crvPointCount - num17;
		}
		while (eStatus != EStatus.Finished)
		{
			num14 += num12;
			num15++;
			double num18 = gemXSize / 2.0 + (num6 / 2.0 - gemXSize / 2.0) * ((double)num15 / (double)num17);
			double item = num4 + (num5 - num4) * ((double)num15 / (double)num17);
			if (GetFixedCurvePoint(num14) == num11)
			{
				if (eStatus == EStatus.FindNext)
				{
					if (num16 == 0)
					{
						num16 = 1;
					}
					double num19 = num14 - list[list.Count - 1];
					double num20;
					double num21;
					if (integer2 == 1)
					{
						num20 = num19 / ((double)(list.Count - num16) - 0.5);
						num21 = 0.5;
					}
					else
					{
						num20 = num19 / (double)(list.Count - num16);
						num21 = 1.0;
					}
					for (int n = num16; n < list.Count; n++)
					{
						if (integer2 < 2)
						{
							list[n] += (int)(num20 * num21);
						}
						if (integer2 == 1)
						{
							list2[n] += num20 / 100.0;
						}
						num21 += 1.0;
					}
				}
				eStatus = EStatus.Finished;
			}
			if (eStatus == EStatus.FindFirst)
			{
				eStatus = EStatus.FoundGem;
				for (int num22 = 0; num22 < list.Count; num22++)
				{
					double num23 = ((!boolean4) ? (num18 + list2[num22] + num3) : ((num18 + list2[num22]) * num13));
					if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(list[num22])]).LengthSquared() < num23 * num23)
					{
						eStatus = EStatus.FindFirst;
						break;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					list.Add(num14);
					list2.Add(num18);
					list3.Add(item);
					num16 = list.Count - 1;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				double num23 = ((!boolean4) ? (num18 + list2[list.Count - 1] + num3) : ((num18 + list2[list.Count - 1]) * num13));
				if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(list[list.Count - 1])]).Length() > num23)
				{
					list.Add(num14);
					list2.Add(num18);
					list3.Add(item);
					eStatus = EStatus.FoundGem;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				int num24 = -1;
				for (int num25 = 0; num25 < list.Count - 1; num25++)
				{
					double num23 = ((!boolean4) ? (num18 + list2[num25] + num3) : ((num18 + list2[num25]) * num13));
					if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(list[num25])]).LengthSquared() < num23 * num23)
					{
						num24 = num25;
						break;
					}
				}
				if (num24 >= 0)
				{
					if (num24 > num16)
					{
						num16 = num24 + 1;
					}
					if (num16 == 0)
					{
						num16 = 1;
					}
					double num26 = num14 - list[list.Count - 1];
					double num27;
					if (integer2 == 1)
					{
						num26 /= (double)(list.Count - num16);
						num27 = 0.5;
					}
					else
					{
						num26 /= (double)(list.Count - num16 + 1);
						num27 = 1.0;
					}
					for (int num28 = num16; num28 < list.Count; num28++)
					{
						if (integer2 < 2)
						{
							list[num28] += (int)(num26 * num27);
						}
						if (integer2 == 1)
						{
							list2[num28] += num26 / 100.0;
						}
						num27 += 1.0;
					}
					eStatus = EStatus.FindFirst;
				}
			}
			if (eStatus == EStatus.FoundGem)
			{
				eStatus = EStatus.FindNext;
			}
		}
		if (boolean2 && list.Count >= 2)
		{
			int num29 = list.Count - 2;
			double num18 = list2[num29];
			double item = list3[num29];
			double num30 = (_crvPoints[GetFixedCurvePoint(list[num29 + 1])] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(list[num29])]).Length() - (list2[num29 + 1] + num18);
			num29--;
			eStatus = EStatus.FindFirst;
			while (eStatus != EStatus.Finished)
			{
				num14 += num12;
				num15++;
				if (!flag2 && num14 != GetFixedCurvePoint(num14))
				{
					eStatus = EStatus.Finished;
				}
				if (num15 > _crvPointCount)
				{
					eStatus = EStatus.Finished;
				}
				if (eStatus == EStatus.Finished)
				{
					continue;
				}
				eStatus = EStatus.FoundGem;
				if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(list[0])]).Length() - (list2[0] + num18) < 0.0)
				{
					eStatus = EStatus.FindFirst;
				}
				for (int num31 = 1; num31 < list.Count; num31++)
				{
					if ((_crvPoints[GetFixedCurvePoint(num14)] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(list[num31])]).Length() - (list2[num31] + num18) < num30)
					{
						eStatus = EStatus.FindFirst;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					list.Add(GetFixedCurvePoint(num14));
					list2.Add(num18);
					list3.Add(item);
					if (num29 >= 0)
					{
						num18 = list2[num29];
						item = list3[num29];
						num30 = (_crvPoints[GetFixedCurvePoint(list[num29 + 1])] - (IOn3dPoint)(object)_crvPoints[GetFixedCurvePoint(list[num29])]).Length() - (list2[num29 + 1] + num18);
						num29--;
					}
					else
					{
						eStatus = EStatus.Finished;
					}
				}
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		double num32 = gem.GemXSize / 2.0;
		for (int num33 = 0; num33 < list.Count; num33++)
		{
			On3dPoint destination = _crvPoints[list[num33]];
			if (_crvNormals[list[num33]] == (IOn3dVector)null)
			{
				if (val3 == null)
				{
					_crvNormals[list[num33]] = On3dVector.op_Implicit((IOn3dPoint)new On3dPoint(0.0, 0.0, 1.0));
				}
				else
				{
					_crvNormals[list[num33]] = new On3dVector();
					CSUtilGeometry.GetClosestNormalToBrep((IOnBrep)(object)val3, (IOn3dPoint)(object)_crvPoints[list[num33]], ref _crvNormals[list[num33]]);
				}
			}
			On3dVector destination2 = new On3dVector((IOn3dVector)(object)_crvNormals[list[num33]]);
			On3dVector destination3 = new On3dVector((IOn3dVector)(object)_crvTangents[list[num33]]);
			CSData cSData = gem.Duplicate();
			if (cSData != null)
			{
				OnXform xform = (boolean ? CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0 - list3[num33]), (IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dVector)new On3dVector(-1.0, 0.0, 0.0), (IOn3dPoint)(object)destination, (IOn3dVector)(object)destination2, (IOn3dVector)(object)destination3) : CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0 - list3[num33]), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(1.0, 0.0, 0.0), (IOn3dPoint)(object)destination, (IOn3dVector)(object)destination2, (IOn3dVector)(object)destination3));
				OnXform val6 = new OnXform();
				val6.Scale(list2[num33] / num32, list2[num33] / num32, list2[num33] / num32);
				CSDataHelper.Transform(cSData, xform.Convert() * val6.Convert() * val4.Convert());
				cSDataArray.Append(cSData);
			}
		}
		OutArray.SetData(cSDataArray);
		return base.Update();
	}

	private int GetFixedCurvePoint(int val)
	{
		int i;
		for (i = val; i < 0; i += _crvPointCount)
		{
		}
		return i % _crvPointCount;
	}
}
