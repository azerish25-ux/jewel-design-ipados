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
			_crvLength = _masterCrv.GetLength();
			_crvPointCount = (int)(_crvLength * 50.0);
			_normalizedParams = new double[_crvPointCount + 1];
			_crvParams = new double[_crvPointCount + 1];
			_crvPoints = new Point3d[_crvPointCount + 1];
			for (int i = 0; i <= _crvPointCount; i++)
			{
				_normalizedParams[i] = (double)i / (double)_crvPointCount;
			}
			_crvPointCount++;
			_crvParams = _masterCrv.NormalizedLengthParameters(_normalizedParams, CSApplicationHelper.Application.Tolerance);
			for (int j = 0; j < _crvPointCount; j++)
			{
				_crvPoints[j] = _masterCrv.PointAt(_crvParams[j]);
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
		bool isClosed = _masterCrv.IsClosed;
		Interval interval = default(Interval);
		if (!value6)
		{
			value5 = value4;
		}
		interval = _masterCrv.Domain;
		double num2 = interval.Min + interval.Length * value;
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
		num2 = interval.Min + interval.Length * value2;
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
					if ((_crvPoints[CPF(num9)] - _crvPoints[CPF(list[n])]).SquareLength < num17 * num17)
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
				if ((_crvPoints[CPF(num9)] - _crvPoints[CPF(list[list.Count - 1])]).Length > num17)
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
					if ((_crvPoints[CPF(num9)] - _crvPoints[CPF(list[num19])]).SquareLength < num17 * num17)
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
			double num24 = (_crvPoints[CPF(list[num23 + 1])] - _crvPoints[CPF(list[num23])]).Length - (list2[num23 + 1] + num13);
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
				if ((_crvPoints[CPF(num9)] - _crvPoints[CPF(list[0])]).Length - (list2[0] + num13) < 0.0)
				{
					statusType = StatusType.FindFirst;
				}
				for (int num25 = 1; num25 < list.Count; num25++)
				{
					if ((_crvPoints[CPF(num9)] - _crvPoints[CPF(list[num25])]).Length - (list2[num25] + num13) < num24)
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
						num24 = (_crvPoints[CPF(list[num23 + 1])] - _crvPoints[CPF(list[num23])]).Length - (list2[num23 + 1] + num13);
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
			Sphere sphere = new Sphere(Point3d.Origin, num26);
			cSData = ((!CSApplicationHelper.Application.ProductionMeshes) ? new CSOnGeometryData(Mesh.CreateFromSphere(sphere, Math.Max(10, (int)(num26 * 40.0)), Math.Max((int)(num26 * 40.0), 10))) : new CSOnGeometryData(Mesh.CreateFromSphere(sphere, Math.Max(40, (int)(num26 * 40.0 * 4.0)), Math.Max((int)(num26 * 40.0 * 4.0), 40))));
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
				Mesh geometry = CSUtilSweep.MeshPipe(_masterCrv, list3.ToArray(), list2.ToArray(), 0.01);
				cSDataArray.Append(new CSOnGeometryData(geometry));
			}
			else
			{
				for (int num29 = 0; num29 < list.Count; num29++)
				{
					Point3d point3d = _crvPoints[list[num29]];
					Transform transform = Transform.Scale(Plane.WorldXY, list2[num29] / num26, list2[num29] / num26, list2[num29] / num26);
					Transform transform2 = Transform.Translation(point3d - Point3d.Origin);
					CSData data = cSData.Duplicate();
					CSDataHelper.Transform(data, transform2 * transform);
					cSDataArray.Append(data);
				}
			}
		}
		else
		{
			Vector3d brepNormal = new Vector3d(0.0, 0.0, 1.0);
			for (int num30 = 0; num30 < list.Count; num30++)
			{
				int num31 = list[num30];
				Point3d point3d2 = _crvPoints[num31];
				Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
				Point3d point3d3 = Point3d.Unset;
				if (value9 != null)
				{
					CSUtilGeometry.GetClosestNormalToBrep(value9, point3d2, ref brepNormal);
				}
				else if (value10 != null)
				{
					value10.ClosestPoint(point3d2, out var t);
					point3d3 = value10.PointAt(t);
				}
				else if (value11 != Point3d.Unset)
				{
					point3d3 = value11;
				}
				if (point3d3 != Point3d.Unset)
				{
					brepNormal = point3d2 - point3d3;
					brepNormal.Unitize();
				}
				if (num31 + 1 < _crvPoints.Length)
				{
					vector3d = _crvPoints[num31 + 1] - point3d2;
				}
				else if (num31 - 1 >= 0)
				{
					vector3d = point3d2 - _crvPoints[num31 - 1];
				}
				vector3d.Unitize();
				Transform transform3 = Transform.Scale(Plane.WorldXY, list2[num30], list2[num30], list2[num30]);
				Transform transform4 = (value8 ? CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0 - value12), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, -1.0), point3d2, vector3d, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0 - value12), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), point3d2, vector3d, brepNormal));
				Vector3d vector3d2 = Vector3d.CrossProduct(brepNormal, vector3d);
				vector3d2.Unitize();
				if (Math.Abs(vector3d2 * vector3d) < 0.99)
				{
					brepNormal = Vector3d.CrossProduct(vector3d, vector3d2);
					brepNormal.Unitize();
				}
				CSData data2 = cSData.Duplicate();
				CSDataHelper.Transform(data2, transform4 * transform3);
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
