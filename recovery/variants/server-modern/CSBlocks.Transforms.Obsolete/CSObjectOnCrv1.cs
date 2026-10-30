using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms.Obsolete;

[Obsolete]
[CSBlock("ObjectOnCrv_V1", "{8A07B959-06E0-4338-9362-B49E5B73457B}")]
public sealed class CSObjectOnCrv1 : CSBlock
{
	private enum StatusType
	{
		FindFirst,
		FindNext,
		FoundGem,
		Finished
	}

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InMilgrainSize;

	public readonly CSInputChannel InParamStart;

	public readonly CSInputChannel InParamEnd;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InMirror;

	public readonly CSInputChannel InTaper;

	public readonly CSInputChannel InEndSize;

	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InNormal;

	public readonly CSInputChannel InOffsetZ;

	public readonly CSInputChannel InFlip;

	public readonly CSInputChannel InPipe;

	public readonly CSOutputChannel OutObjects;

	private Point3d[] _crvPoints;

	private double[] _normalizedParams;

	private double[] _crvParams;

	private int _crvPointCount;

	private CSInputChannel _masterCrvChannel;

	private NurbsCurve _masterCrv;

	private double _crvLength;

	public CSObjectOnCrv1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InMilgrainSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InTaper = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEndSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InNormal = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		InOffsetZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InPipe = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5c: Unknown result type (might be due to invalid IL or missing references)
		List<int> list = new List<int>();
		List<double> list2 = new List<double>();
		if (BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock) || _masterCrv == null || _normalizedParams == null || _masterCrvChannel != InCurve)
		{
			_masterCrvChannel = InCurve;
			CSDataArray flattenArray = InCurve.GetFlattenArray();
			if (flattenArray == null)
			{
				return UpdateMissing("Missing Objects");
			}
			if (flattenArray.GetArrayCount() == 0)
			{
				return UpdateMissing("Missing Objects");
			}
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out _masterCrv);
			if (_masterCrv == null)
			{
				return UpdateMissing("Missing Master Curve");
			}
			_crvLength = ((Curve)_masterCrv).GetLength();
			_crvPointCount = (int)(_crvLength * 50.0);
			_normalizedParams = new double[_crvPointCount + 1];
			_crvParams = new double[_crvPointCount + 1];
			_crvPoints = (Point3d[])(object)new Point3d[_crvPointCount + 1];
			for (int i = 0; i <= _crvPointCount; i++)
			{
				_normalizedParams[i] = (double)i / (double)_crvPointCount;
			}
			_crvPointCount++;
			_crvParams = ((Curve)_masterCrv).NormalizedLengthParameters(_normalizedParams, CSApplicationHelper.Application.Tolerance);
			for (int j = 0; j < _crvPointCount; j++)
			{
				_crvPoints[j] = ((Curve)_masterCrv).PointAt(_crvParams[j]);
			}
		}
		double value = 0.0;
		InParamStart.GetDouble(ref value);
		double value2 = 1.0;
		InParamEnd.GetDouble(ref value2);
		double value3 = 0.0;
		InSpacing.GetDouble(ref value3);
		double value4 = 1.0;
		InMilgrainSize.GetDouble(ref value4);
		double value5 = 1.0;
		InEndSize.GetDouble(ref value5);
		bool value6 = false;
		InTaper.GetBoolean(ref value6);
		bool flag = false;
		bool value7 = false;
		InMirror.GetBoolean(ref value7);
		bool value8 = false;
		InFlip.GetBoolean(ref value8);
		int num = 1;
		bool isClosed = ((Curve)_masterCrv).IsClosed;
		Interval val = default(Interval);
		if (!value6)
		{
			value5 = value4;
		}
		val = ((Curve)_masterCrv).Domain;
		double num2 = ((Interval)(ref val)).Min + ((Interval)(ref val)).Length * value;
		int num3 = 0;
		double num4 = 999.0;
		for (int k = 0; k < _crvPointCount; k++)
		{
			if (Math.Abs(num2 - _crvParams[k]) < num4)
			{
				num4 = Math.Abs(num2 - _crvParams[k]);
				num3 = k;
			}
		}
		num2 = ((Interval)(ref val)).Min + ((Interval)(ref val)).Length * value2;
		int num5 = 0;
		num4 = 999.0;
		for (int l = 0; l < _crvPointCount; l++)
		{
			if (Math.Abs(num2 - _crvParams[l]) < num4)
			{
				num4 = Math.Abs(num2 - _crvParams[l]);
				num5 = l;
			}
		}
		int num6 = ((num3 <= num5) ? 1 : (-1));
		num6 *= num;
		double num7 = (value4 / 2.0 + value3 / 2.0) / (value4 / 2.0);
		StatusType statusType = StatusType.FindFirst;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		list.Clear();
		list2.Clear();
		num9 = num3 - num6;
		int num12 = 0;
		num12 = Math.Abs(num5 - num3);
		if (num == -1)
		{
			num12 = _crvPointCount - num12;
		}
		Vector3d val2;
		while (statusType != StatusType.Finished)
		{
			num9 += num6;
			num10++;
			double num13 = value4 / 2.0 + (value5 / 2.0 - value4 / 2.0) * ((double)num10 / (double)num12);
			if (CPF(num9) == num5)
			{
				if (statusType == StatusType.FindNext)
				{
					if (num11 == 0)
					{
						num11 = 1;
					}
					double num14 = num9 - list[list.Count - 1];
					double num15;
					double num16;
					if (num8 == 1)
					{
						num15 = num14 / ((double)(list.Count - num11) - 0.5);
						num16 = 0.5;
					}
					else
					{
						num15 = num14 / (double)(list.Count - num11);
						num16 = 1.0;
					}
					for (int m = num11; m < list.Count; m++)
					{
						if (num8 < 2)
						{
							list[m] += (int)(num15 * num16);
						}
						if (num8 == 1)
						{
							list2[m] += num15 / 100.0;
						}
						num16 += 1.0;
					}
				}
				statusType = StatusType.Finished;
			}
			if (statusType == StatusType.FindFirst)
			{
				statusType = StatusType.FoundGem;
				for (int n = 0; n < list.Count; n++)
				{
					double num17 = ((!flag) ? (num13 + list2[n] + value3) : ((num13 + list2[n]) * num7));
					val2 = _crvPoints[CPF(num9)] - _crvPoints[CPF(list[n])];
					if (((Vector3d)(ref val2)).SquareLength < num17 * num17)
					{
						statusType = StatusType.FindFirst;
						break;
					}
				}
				if (statusType == StatusType.FoundGem)
				{
					list.Add(num9);
					list2.Add(num13);
					num11 = list.Count - 1;
				}
			}
			if (statusType == StatusType.FindNext)
			{
				double num17 = ((!flag) ? (num13 + list2[list.Count - 1] + value3) : ((num13 + list2[list.Count - 1]) * num7));
				val2 = _crvPoints[CPF(num9)] - _crvPoints[CPF(list[list.Count - 1])];
				if (((Vector3d)(ref val2)).Length > num17)
				{
					list.Add(num9);
					list2.Add(num13);
					statusType = StatusType.FoundGem;
				}
			}
			if (statusType == StatusType.FindNext)
			{
				int num18 = -1;
				for (int num19 = 0; num19 < list.Count - 1; num19++)
				{
					double num17 = ((!flag) ? (num13 + list2[num19] + value3) : ((num13 + list2[num19]) * num7));
					val2 = _crvPoints[CPF(num9)] - _crvPoints[CPF(list[num19])];
					if (((Vector3d)(ref val2)).SquareLength < num17 * num17)
					{
						num18 = num19;
						break;
					}
				}
				if (num18 >= 0)
				{
					if (num18 > num11)
					{
						num11 = num18 + 1;
					}
					if (num11 == 0)
					{
						num11 = 1;
					}
					double num20 = num9 - list[list.Count - 1];
					double num21;
					if (num8 == 1)
					{
						num20 /= (double)(list.Count - num11);
						num21 = 0.5;
					}
					else
					{
						num20 /= (double)(list.Count - num11 + 1);
						num21 = 1.0;
					}
					for (int num22 = num11; num22 < list.Count; num22++)
					{
						if (num8 < 2)
						{
							list[num22] += (int)(num20 * num21);
						}
						if (num8 == 1)
						{
							list2[num22] += num20 / 100.0;
						}
						num21 += 1.0;
					}
					statusType = StatusType.FindFirst;
				}
			}
			if (statusType == StatusType.FoundGem)
			{
				statusType = StatusType.FindNext;
			}
		}
		if (value7 && list.Count >= 2)
		{
			int num23 = list.Count - 2;
			double num13 = list2[num23];
			val2 = _crvPoints[CPF(list[num23 + 1])] - _crvPoints[CPF(list[num23])];
			double num24 = ((Vector3d)(ref val2)).Length - (list2[num23 + 1] + num13);
			num23--;
			statusType = StatusType.FindFirst;
			while (statusType != StatusType.Finished)
			{
				num9 += num6;
				num10++;
				if (!isClosed && num9 != CPF(num9))
				{
					statusType = StatusType.Finished;
				}
				if (num10 > _crvPointCount)
				{
					statusType = StatusType.Finished;
				}
				if (statusType == StatusType.Finished)
				{
					continue;
				}
				statusType = StatusType.FoundGem;
				val2 = _crvPoints[CPF(num9)] - _crvPoints[CPF(list[0])];
				if (((Vector3d)(ref val2)).Length - (list2[0] + num13) < 0.0)
				{
					statusType = StatusType.FindFirst;
				}
				for (int num25 = 1; num25 < list.Count; num25++)
				{
					val2 = _crvPoints[CPF(num9)] - _crvPoints[CPF(list[num25])];
					if (((Vector3d)(ref val2)).Length - (list2[num25] + num13) < num24)
					{
						statusType = StatusType.FindFirst;
					}
				}
				if (statusType == StatusType.FoundGem)
				{
					list.Add(CPF(num9));
					list2.Add(num13);
					if (num23 >= 0)
					{
						num13 = list2[num23];
						val2 = _crvPoints[CPF(list[num23 + 1])] - _crvPoints[CPF(list[num23])];
						num24 = ((Vector3d)(ref val2)).Length - (list2[num23 + 1] + num13);
						num23--;
					}
					else
					{
						statusType = StatusType.Finished;
					}
				}
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		double num26 = 0.1;
		for (int num27 = 0; num27 < list2.Count; num27++)
		{
			num26 = Math.Max(num26, list2[num27]);
		}
		CSData cSData = null;
		bool flag2 = false;
		if (InObject == null || InObject.OutputChannelBlock == null)
		{
			Sphere val3 = default(Sphere);
			((Sphere)(ref val3))._002Ector(Point3d.Origin, num26);
			cSData = ((!CSApplicationHelper.Application.ProductionMeshes) ? new CSOnGeometryData((GeometryBase)(object)Mesh.CreateFromSphere(val3, Math.Max(10, (int)(num26 * 40.0)), Math.Max((int)(num26 * 40.0), 10))) : new CSOnGeometryData((GeometryBase)(object)Mesh.CreateFromSphere(val3, Math.Max(40, (int)(num26 * 40.0 * 4.0)), Math.Max((int)(num26 * 40.0 * 4.0), 40))));
			flag2 = true;
		}
		else
		{
			cSData = InObject.Data;
		}
		CSDataHelper.GetBrep_NoArraySupport(InNormal.Data, out var value9);
		CSDataHelper.GetNurbsCurve_NoArraySupport(InNormal.Data, out var value10);
		if (!CSDataHelper.GetPoint(InNormal.Data, out var value11))
		{
			value11 = Point3d.Unset;
		}
		double value12 = 0.0;
		InOffsetZ.GetDouble(ref value12);
		bool value13 = false;
		InPipe.GetBoolean(ref value13);
		if (flag2)
		{
			if (CSApplicationHelper.Application.PipeMode || value13)
			{
				List<Point3d> list3 = new List<Point3d>();
				for (int num28 = 0; num28 < list.Count; num28++)
				{
					list3.Add(_crvPoints[list[num28]]);
				}
				Mesh geometry = CSUtilSweep.MeshPipe((Curve)(object)_masterCrv, list3.ToArray(), list2.ToArray(), 0.01);
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)geometry));
			}
			else
			{
				for (int num29 = 0; num29 < list.Count; num29++)
				{
					Point3d val4 = _crvPoints[list[num29]];
					Transform val5 = Transform.Scale(Plane.WorldXY, list2[num29] / num26, list2[num29] / num26, list2[num29] / num26);
					Transform val6 = Transform.Translation(val4 - Point3d.Origin);
					CSData data = cSData.Duplicate();
					CSDataHelper.Transform(data, val6 * val5);
					cSDataArray.Append(data);
				}
			}
		}
		else
		{
			Vector3d brepNormal = default(Vector3d);
			((Vector3d)(ref brepNormal))._002Ector(0.0, 0.0, 1.0);
			Vector3d val8 = default(Vector3d);
			double num32 = default(double);
			for (int num30 = 0; num30 < list.Count; num30++)
			{
				int num31 = list[num30];
				Point3d val7 = _crvPoints[num31];
				((Vector3d)(ref val8))._002Ector(1.0, 0.0, 0.0);
				Point3d val9 = Point3d.Unset;
				if (value9 != null)
				{
					CSUtilGeometry.GetClosestNormalToBrep(value9, val7, ref brepNormal);
				}
				else if (value10 != null)
				{
					((Curve)value10).ClosestPoint(val7, ref num32);
					val9 = ((Curve)value10).PointAt(num32);
				}
				else if (value11 != Point3d.Unset)
				{
					val9 = value11;
				}
				if (val9 != Point3d.Unset)
				{
					brepNormal = val7 - val9;
					((Vector3d)(ref brepNormal)).Unitize();
				}
				if (num31 + 1 < _crvPoints.Length)
				{
					val8 = _crvPoints[num31 + 1] - val7;
				}
				else if (num31 - 1 >= 0)
				{
					val8 = val7 - _crvPoints[num31 - 1];
				}
				((Vector3d)(ref val8)).Unitize();
				Transform val10 = Transform.Scale(Plane.WorldXY, list2[num30], list2[num30], list2[num30]);
				Transform val11 = (value8 ? CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0 - value12), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, -1.0), val7, val8, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0 - value12), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), val7, val8, brepNormal));
				Vector3d val12 = Vector3d.CrossProduct(brepNormal, val8);
				((Vector3d)(ref val12)).Unitize();
				if (Math.Abs(val12 * val8) < 0.99)
				{
					brepNormal = Vector3d.CrossProduct(val8, val12);
					((Vector3d)(ref brepNormal)).Unitize();
				}
				CSData data2 = cSData.Duplicate();
				CSDataHelper.Transform(data2, val11 * val10);
				cSDataArray.Append(data2);
			}
		}
		OutObjects.SetData(cSDataArray);
		return base.Update();
	}

	private int CPF(int val)
	{
		int i;
		for (i = val; i < 0; i += _crvPointCount)
		{
		}
		return i % _crvPointCount;
	}
}
