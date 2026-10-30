using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrv_V1", "{75C4E7C9-B17A-45af-9B5A-FCBD39E8EEE0}")]
public sealed class CSGemOnCrv1 : CSBlock
{
	private enum StatusType
	{
		FindFirst,
		FindNext,
		FoundGem,
		Finished
	}

	private int crvPointCount;

	private CSInputChannel masterCrvChannel;

	private OnCurve crvMaster;

	private CSInputChannel masterSurfaceChannel;

	private double crvLength;

	private double[] normalizedParams;

	private double[] crvParams;

	private On3dPoint[] crvPoints;

	private On3dVector[] crvNormals;

	private On3dVector[] crvTangents;

	public CSInputChannel InCurve;

	public CSInputChannel InGemDefinition;

	public CSInputChannel InSurface;

	public CSInputChannel InParamStart;

	public CSInputChannel InParamEnd;

	public CSInputChannel InSpacing;

	public CSInputChannel InZOffset;

	public CSInputChannel InFlip;

	public CSInputChannel InMirror;

	public CSInputChannel InTaper;

	public CSInputChannel InEndGemSize;

	public CSInputChannel InTableOnCurve;

	public CSOutputChannel oArray { get; }

	public CSGemOnCrv1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InTaper = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEndGemSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InTableOnCurve = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		oArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<int> list = new List<int>();
		List<double> list2 = new List<double>();
		CSGemData gem = InGemDefinition.Gem;
		double num = InParamStart.Double;
		double num2 = InParamEnd.Double;
		double num3 = InSpacing.Double;
		double num4 = InZOffset.Double;
		double gemXSize = gem.GemXSize;
		double num5 = InEndGemSize.Double;
		bool boolean = InFlip.Boolean;
		bool flag = BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock);
		if (InSurface != null)
		{
			flag |= BeenUpdatedAfterLastUpdate(InSurface.OutputChannelBlock);
		}
		if (crvMaster == null)
		{
			flag = true;
		}
		if (normalizedParams == null)
		{
			flag = true;
		}
		if (flag || masterCrvChannel != InCurve || masterSurfaceChannel != InSurface)
		{
			masterCrvChannel = InCurve;
			masterSurfaceChannel = InSurface;
			crvMaster = InCurve.NurbsCurve.Convert();
			if (crvMaster == null)
			{
				return UpdateMissing("Curve");
			}
			crvMaster.GetLength(ref crvLength);
			crvPointCount = (int)(crvLength * 50.0);
			normalizedParams = new double[crvPointCount + 1];
			crvParams = new double[crvPointCount + 1];
			crvPoints = new On3dPoint[crvPointCount + 1];
			crvNormals = new On3dVector[crvPointCount + 1];
			crvTangents = new On3dVector[crvPointCount + 1];
			for (int i = 0; i <= crvPointCount; i++)
			{
				normalizedParams[i] = (double)i / (double)crvPointCount;
			}
			crvPointCount++;
			crvMaster.GetNormalizedArcLengthPoints(normalizedParams, ref crvParams);
			for (int j = 0; j < crvPointCount; j++)
			{
				On3dPoint point = new On3dPoint();
				On3dVector first_derivative = new On3dVector();
				crvMaster.Ev1Der(crvParams[j], ref point, ref first_derivative);
				crvPoints[j] = point;
				crvTangents[j] = first_derivative;
			}
			for (int k = 0; k < crvPointCount; k++)
			{
				crvNormals[k] = null;
			}
		}
		OnBrep onBrep = InSurface.Brep.Convert();
		if (onBrep == null)
		{
			return UpdateMissing("Surface");
		}
		int num6 = 0;
		bool boolean2 = InTaper.Boolean;
		bool flag2 = false;
		bool boolean3 = InMirror.Boolean;
		double num7 = InTableOnCurve.Double;
		OnXform onXform = new OnXform();
		if (num7 == 0.0)
		{
			onXform.Identity();
		}
		else
		{
			onXform.Translation(0.0, 0.0, 0.0 - gem.GemZMax);
		}
		int num8 = 1;
		bool flag3 = crvMaster.IsClosed();
		OnInterval onInterval = new OnInterval();
		if (!boolean2)
		{
			num5 = gemXSize;
		}
		onInterval = crvMaster.Domain();
		double num9 = onInterval.Min() + onInterval.Length() * num;
		int num10 = 0;
		double num11 = 999.0;
		for (int l = 0; l < crvPointCount; l++)
		{
			if (Math.Abs(num9 - crvParams[l]) < num11)
			{
				num11 = Math.Abs(num9 - crvParams[l]);
				num10 = l;
			}
		}
		num9 = onInterval.Min() + onInterval.Length() * num2;
		int num12 = 0;
		num11 = 999.0;
		for (int m = 0; m < crvPointCount; m++)
		{
			if (Math.Abs(num9 - crvParams[m]) < num11)
			{
				num11 = Math.Abs(num9 - crvParams[m]);
				num12 = m;
			}
		}
		int num13 = ((num10 <= num12) ? 1 : (-1));
		num13 *= num8;
		double num14 = (gemXSize / 2.0 + num3 / 2.0) / (gemXSize / 2.0);
		StatusType statusType = StatusType.FindFirst;
		int num15 = 0;
		int num16 = 0;
		int num17 = 0;
		list.Clear();
		list2.Clear();
		num15 = num10 - num13;
		int num18 = 0;
		num18 = Math.Abs(num12 - num10);
		if (num8 == -1)
		{
			num18 = crvPointCount - num18;
		}
		while (statusType != StatusType.Finished)
		{
			num15 += num13;
			num16++;
			double num19 = gemXSize / 2.0 + (num5 / 2.0 - gemXSize / 2.0) * ((double)num16 / (double)num18);
			if (CPF(num15) == num12)
			{
				if (statusType == StatusType.FindNext)
				{
					if (num17 == 0)
					{
						num17 = 1;
					}
					double num20 = num15 - list[list.Count - 1];
					double num21;
					double num22;
					if (num6 == 1)
					{
						num21 = num20 / ((double)(list.Count - num17) - 0.5);
						num22 = 0.5;
					}
					else
					{
						num21 = num20 / (double)(list.Count - num17);
						num22 = 1.0;
					}
					for (int n = num17; n < list.Count; n++)
					{
						if (num6 < 2)
						{
							list[n] += (int)(num21 * num22);
						}
						if (num6 == 1)
						{
							list2[n] += num21 / 100.0;
						}
						num22 += 1.0;
					}
				}
				statusType = StatusType.Finished;
			}
			if (statusType == StatusType.FindFirst)
			{
				statusType = StatusType.FoundGem;
				for (int num23 = 0; num23 < list.Count; num23++)
				{
					double num24 = ((!flag2) ? (num19 + list2[num23] + num3) : ((num19 + list2[num23]) * num14));
					if ((crvPoints[CPF(num15)] - crvPoints[CPF(list[num23])]).LengthSquared() < num24 * num24)
					{
						statusType = StatusType.FindFirst;
						break;
					}
				}
				if (statusType == StatusType.FoundGem)
				{
					list.Add(num15);
					list2.Add(num19);
					num17 = list.Count - 1;
				}
			}
			if (statusType == StatusType.FindNext)
			{
				double num24 = ((!flag2) ? (num19 + list2[list.Count - 1] + num3) : ((num19 + list2[list.Count - 1]) * num14));
				if ((crvPoints[CPF(num15)] - crvPoints[CPF(list[list.Count - 1])]).Length() > num24)
				{
					list.Add(num15);
					list2.Add(num19);
					statusType = StatusType.FoundGem;
				}
			}
			if (statusType == StatusType.FindNext)
			{
				int num25 = -1;
				for (int num26 = 0; num26 < list.Count - 1; num26++)
				{
					double num24 = ((!flag2) ? (num19 + list2[num26] + num3) : ((num19 + list2[num26]) * num14));
					if ((crvPoints[CPF(num15)] - crvPoints[CPF(list[num26])]).LengthSquared() < num24 * num24)
					{
						num25 = num26;
						break;
					}
				}
				if (num25 >= 0)
				{
					if (num25 > num17)
					{
						num17 = num25 + 1;
					}
					if (num17 == 0)
					{
						num17 = 1;
					}
					double num27 = num15 - list[list.Count - 1];
					double num28;
					if (num6 == 1)
					{
						num27 /= (double)(list.Count - num17);
						num28 = 0.5;
					}
					else
					{
						num27 /= (double)(list.Count - num17 + 1);
						num28 = 1.0;
					}
					for (int num29 = num17; num29 < list.Count; num29++)
					{
						if (num6 < 2)
						{
							list[num29] += (int)(num27 * num28);
						}
						if (num6 == 1)
						{
							list2[num29] += num27 / 100.0;
						}
						num28 += 1.0;
					}
					statusType = StatusType.FindFirst;
				}
			}
			if (statusType == StatusType.FoundGem)
			{
				statusType = StatusType.FindNext;
			}
		}
		if (boolean3 && list.Count >= 2)
		{
			int num30 = list.Count - 2;
			double num19 = list2[num30];
			double num31 = (crvPoints[CPF(list[num30 + 1])] - crvPoints[CPF(list[num30])]).Length() - (list2[num30 + 1] + num19);
			num30--;
			statusType = StatusType.FindFirst;
			while (statusType != StatusType.Finished)
			{
				num15 += num13;
				num16++;
				if (!flag3 && num15 != CPF(num15))
				{
					statusType = StatusType.Finished;
				}
				if (num16 > crvPointCount)
				{
					statusType = StatusType.Finished;
				}
				if (statusType == StatusType.Finished)
				{
					continue;
				}
				statusType = StatusType.FoundGem;
				if ((crvPoints[CPF(num15)] - crvPoints[CPF(list[0])]).Length() - (list2[0] + num19) < 0.0)
				{
					statusType = StatusType.FindFirst;
				}
				for (int num32 = 1; num32 < list.Count; num32++)
				{
					if ((crvPoints[CPF(num15)] - crvPoints[CPF(list[num32])]).Length() - (list2[num32] + num19) < num31)
					{
						statusType = StatusType.FindFirst;
					}
				}
				if (statusType == StatusType.FoundGem)
				{
					list.Add(CPF(num15));
					list2.Add(num19);
					if (num30 >= 0)
					{
						num19 = list2[num30];
						num31 = (crvPoints[CPF(list[num30 + 1])] - crvPoints[CPF(list[num30])]).Length() - (list2[num30 + 1] + num19);
						num30--;
					}
					else
					{
						statusType = StatusType.Finished;
					}
				}
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		BoundingBox boundingBox = BoundingBox.Unset;
		gem.GetBoundingBox(ref boundingBox);
		double val = (boundingBox.Max.X - boundingBox.Min.X) / 2.0;
		val = Math.Max((boundingBox.Max.Y - boundingBox.Min.Y) / 2.0, val);
		for (int num33 = 0; num33 < list.Count; num33++)
		{
			On3dPoint destination = crvPoints[list[num33]];
			if (crvNormals[list[num33]] == null)
			{
				if (onBrep == null)
				{
					crvNormals[list[num33]] = new On3dPoint(0.0, 0.0, 1.0);
				}
				else
				{
					crvNormals[list[num33]] = new On3dVector();
					CSUtilGeometry.GetClosestNormalToBrep(onBrep, crvPoints[list[num33]], ref crvNormals[list[num33]]);
				}
			}
			On3dVector destination2 = new On3dVector(crvNormals[list[num33]]);
			On3dVector destination3 = new On3dVector(crvTangents[list[num33]]);
			CSGemData cSGemData = gem.Duplicate() as CSGemData;
			cSGemData.SettingType = gem.SettingType;
			if (cSGemData != null)
			{
				OnXform obj = (boolean ? CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0 - num4), new On3dVector(0.0, 0.0, -1.0), new On3dVector(-1.0, 0.0, 0.0), destination, destination2, destination3) : CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0 - num4), new On3dVector(0.0, 0.0, 1.0), new On3dVector(1.0, 0.0, 0.0), destination, destination2, destination3));
				OnXform onXform2 = new OnXform();
				onXform2.Scale(list2[num33] / val, list2[num33] / val, list2[num33] / val);
				OnXform xform = obj * onXform2 * onXform;
				cSGemData.ApplyTransform(xform.Convert());
				cSDataArray.Append(cSGemData);
			}
		}
		oArray.SetData(cSDataArray);
		return base.Update();
	}

	private int CPF(int val)
	{
		int i;
		for (i = val; i < 0; i += crvPointCount)
		{
		}
		return i % crvPointCount;
	}
}
