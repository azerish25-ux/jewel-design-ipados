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
[CSBlock("GemOnCrv_V2", "{CD97BD37-14C5-4c99-B4C2-39EB27EA9A94}")]
public sealed class CSGemOnCrv2 : CSBlock
{
	private enum EStatus
	{
		FindFirst,
		FindNext,
		FoundGem,
		Finished
	}

	public readonly CSOutputChannel OutArray;

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

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InSurface;

	public readonly CSInputChannel InParamStart;

	public readonly CSInputChannel InParamEnd;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InZOffset;

	public readonly CSInputChannel InFlip;

	public readonly CSInputChannel InMirror;

	public readonly CSInputChannel InTaper;

	public readonly CSInputChannel InEndGemSize;

	public readonly CSInputChannel InPlacementMethod;

	public CSGemOnCrv2(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaper = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEndGemSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<int> list = new List<int>();
		List<double> list2 = new List<double>();
		CSGemData gem = InGemDefinition.Gem;
		if (gem == null)
		{
			return UpdateMissing("Gem");
		}
		bool flag = BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock);
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
			_crvMaster = InCurve.NurbsCurve.Convert();
			if (_crvMaster == null)
			{
				return UpdateMissing("Curve");
			}
			_crvMaster.GetLength(ref _crvLength);
			_crvPointCount = (int)(_crvLength * 50.0);
			_normalizedParams = new double[_crvPointCount + 1];
			_unnormalizedParams = new double[_crvPointCount + 1];
			_crvPoints = new On3dPoint[_crvPointCount + 1];
			_crvNormals = new On3dVector[_crvPointCount + 1];
			_crvTangents = new On3dVector[_crvPointCount + 1];
			for (int i = 0; i <= _crvPointCount; i++)
			{
				_normalizedParams[i] = (double)i / (double)_crvPointCount;
			}
			_crvPointCount++;
			_crvMaster.GetNormalizedArcLengthPoints(_normalizedParams, ref _unnormalizedParams);
			for (int j = 0; j < _crvPointCount; j++)
			{
				On3dPoint point = new On3dPoint();
				On3dVector first_derivative = new On3dVector();
				_crvMaster.Ev1Der(_unnormalizedParams[j], ref point, ref first_derivative);
				_crvPoints[j] = point;
				_crvTangents[j] = first_derivative;
			}
			for (int k = 0; k < _crvPointCount; k++)
			{
				_crvNormals[k] = null;
			}
		}
		OnBrep onBrep = InSurface.Brep.Convert();
		if (onBrep == null)
		{
			return UpdateMissing("Surface");
		}
		int integer = InPlacementMethod.Integer;
		double num = InParamStart.Double;
		double num2 = InParamEnd.Double;
		double num3 = InSpacing.Double;
		double num4 = InZOffset.Double;
		double gemXSize = gem.GemXSize;
		double num5 = InEndGemSize.Double;
		bool boolean = InFlip.Boolean;
		int num6 = 0;
		bool boolean2 = InTaper.Boolean;
		bool flag2 = false;
		bool boolean3 = InMirror.Boolean;
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0, zOffset);
		int num7 = 1;
		bool flag3 = _crvMaster.IsClosed();
		OnInterval onInterval = new OnInterval();
		if (!boolean2)
		{
			num5 = gemXSize;
		}
		onInterval = _crvMaster.Domain();
		double num8 = onInterval.Min() + onInterval.Length() * num;
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
		num8 = onInterval.Min() + onInterval.Length() * num2;
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
			double num18 = gemXSize / 2.0 + (num5 / 2.0 - gemXSize / 2.0) * ((double)num15 / (double)num17);
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
					if (num6 == 1)
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
						if (num6 < 2)
						{
							list[n] += (int)(num20 * num21);
						}
						if (num6 == 1)
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
					double num23 = ((!flag2) ? (num18 + list2[num22] + num3) : ((num18 + list2[num22]) * num13));
					if ((_crvPoints[GetFixedCurvePoint(num14)] - _crvPoints[GetFixedCurvePoint(list[num22])]).LengthSquared() < num23 * num23)
					{
						eStatus = EStatus.FindFirst;
						break;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					list.Add(num14);
					list2.Add(num18);
					num16 = list.Count - 1;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				double num23 = ((!flag2) ? (num18 + list2[list.Count - 1] + num3) : ((num18 + list2[list.Count - 1]) * num13));
				if ((_crvPoints[GetFixedCurvePoint(num14)] - _crvPoints[GetFixedCurvePoint(list[list.Count - 1])]).Length() > num23)
				{
					list.Add(num14);
					list2.Add(num18);
					eStatus = EStatus.FoundGem;
				}
			}
			if (eStatus == EStatus.FindNext)
			{
				int num24 = -1;
				for (int num25 = 0; num25 < list.Count - 1; num25++)
				{
					double num23 = ((!flag2) ? (num18 + list2[num25] + num3) : ((num18 + list2[num25]) * num13));
					if ((_crvPoints[GetFixedCurvePoint(num14)] - _crvPoints[GetFixedCurvePoint(list[num25])]).LengthSquared() < num23 * num23)
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
					if (num6 == 1)
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
						if (num6 < 2)
						{
							list[num28] += (int)(num26 * num27);
						}
						if (num6 == 1)
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
		if (boolean3 && list.Count >= 2)
		{
			int num29 = list.Count - 2;
			double num18 = list2[num29];
			double num30 = (_crvPoints[GetFixedCurvePoint(list[num29 + 1])] - _crvPoints[GetFixedCurvePoint(list[num29])]).Length() - (list2[num29 + 1] + num18);
			num29--;
			eStatus = EStatus.FindFirst;
			while (eStatus != EStatus.Finished)
			{
				num14 += num12;
				num15++;
				if (!flag3 && num14 != GetFixedCurvePoint(num14))
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
				if ((_crvPoints[GetFixedCurvePoint(num14)] - _crvPoints[GetFixedCurvePoint(list[0])]).Length() - (list2[0] + num18) < 0.0)
				{
					eStatus = EStatus.FindFirst;
				}
				for (int num31 = 1; num31 < list.Count; num31++)
				{
					if ((_crvPoints[GetFixedCurvePoint(num14)] - _crvPoints[GetFixedCurvePoint(list[num31])]).Length() - (list2[num31] + num18) < num30)
					{
						eStatus = EStatus.FindFirst;
					}
				}
				if (eStatus == EStatus.FoundGem)
				{
					list.Add(GetFixedCurvePoint(num14));
					list2.Add(num18);
					if (num29 >= 0)
					{
						num18 = list2[num29];
						num30 = (_crvPoints[GetFixedCurvePoint(list[num29 + 1])] - _crvPoints[GetFixedCurvePoint(list[num29])]).Length() - (list2[num29 + 1] + num18);
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
			if (_crvNormals[list[num33]] == null)
			{
				if (onBrep == null)
				{
					_crvNormals[list[num33]] = new On3dPoint(0.0, 0.0, 1.0);
				}
				else
				{
					_crvNormals[list[num33]] = new On3dVector();
					CSUtilGeometry.GetClosestNormalToBrep(onBrep, _crvPoints[list[num33]], ref _crvNormals[list[num33]]);
				}
			}
			On3dVector destination2 = new On3dVector(_crvNormals[list[num33]]);
			On3dVector destination3 = new On3dVector(_crvTangents[list[num33]]);
			CSData cSData = gem.Duplicate();
			if (cSData != null)
			{
				OnXform xform = (boolean ? CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0 - num4), new On3dVector(0.0, 0.0, -1.0), new On3dVector(-1.0, 0.0, 0.0), destination, destination2, destination3) : CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0 - num4), new On3dVector(0.0, 0.0, 1.0), new On3dVector(1.0, 0.0, 0.0), destination, destination2, destination3));
				OnXform onXform2 = new OnXform();
				onXform2.Scale(list2[num33] / num32, list2[num33] / num32, list2[num33] / num32);
				CSDataHelper.Transform(cSData, xform.Convert() * onXform2.Convert() * onXform.Convert());
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
