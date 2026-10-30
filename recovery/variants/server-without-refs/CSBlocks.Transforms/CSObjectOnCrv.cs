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

namespace CSBlocks.Transforms;

[CSBlock("ObjectOnCrv", "{0CB8F37D-6630-4285-BE02-5236DDD61CE2}")]
public sealed class CSObjectOnCrv : CSBlock
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

	public CSObjectOnCrv(string identifier)
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
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
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
		list.Clear();
		list2.Clear();
		int num11 = num3 - num6;
		int num12 = 0;
		num12 = Math.Abs(num5 - num3);
		if (num == -1)
		{
			num12 = _crvPointCount - num12;
		}
		while (statusType != StatusType.Finished)
		{
			num11 += num6;
			num9++;
			double num13 = value4 / 2.0 + (value5 / 2.0 - value4 / 2.0) * ((double)num9 / (double)num12);
			if (CurvePointFixed(num11) == num5)
			{
				if (statusType == StatusType.FindNext)
				{
					if (num10 == 0)
					{
						num10 = 1;
					}
					double num14 = num11 - list[list.Count - 1];
					double num15;
					double num16;
					if (num8 == 1)
					{
						num15 = num14 / ((double)(list.Count - num10) - 0.5);
						num16 = 0.5;
					}
					else
					{
						num15 = num14 / (double)(list.Count - num10);
						num16 = 1.0;
					}
					for (int m = num10; m < list.Count; m++)
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
			Vector3d val2;
			if (statusType == StatusType.FindFirst)
			{
				statusType = StatusType.FoundGem;
				for (int n = 0; n < list.Count; n++)
				{
					double num17 = ((!flag) ? (num13 + list2[n] + value3) : ((num13 + list2[n]) * num7));
					val2 = _crvPoints[CurvePointFixed(num11)] - _crvPoints[CurvePointFixed(list[n])];
					if (((Vector3d)(ref val2)).SquareLength < num17 * num17)
					{
						statusType = StatusType.FindFirst;
						break;
					}
				}
				if (statusType == StatusType.FoundGem)
				{
					list.Add(num11);
					list2.Add(num13);
					num10 = list.Count - 1;
				}
			}
			if (statusType == StatusType.FindNext)
			{
				double num17 = ((!flag) ? (num13 + list2[list.Count - 1] + value3) : ((num13 + list2[list.Count - 1]) * num7));
				val2 = _crvPoints[CurvePointFixed(num11)] - _crvPoints[CurvePointFixed(list[list.Count - 1])];
				if (((Vector3d)(ref val2)).Length > num17)
				{
					list.Add(num11);
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
					val2 = _crvPoints[CurvePointFixed(num11)] - _crvPoints[CurvePointFixed(list[num19])];
					if (((Vector3d)(ref val2)).SquareLength < num17 * num17)
					{
						num18 = num19;
						break;
					}
				}
				if (num18 >= 0)
				{
					if (num18 > num10)
					{
						num10 = num18 + 1;
					}
					if (num10 == 0)
					{
						num10 = 1;
					}
					double num20 = num11 - list[list.Count - 1];
					double num21;
					if (num8 == 1)
					{
						num20 /= (double)(list.Count - num10);
						num21 = 0.5;
					}
					else
					{
						num20 /= (double)(list.Count - num10 + 1);
						num21 = 1.0;
					}
					for (int num22 = num10; num22 < list.Count; num22++)
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
			int num23 = list[list.Count - 1];
			for (int num24 = list.Count - 2; num24 >= 0; num24--)
			{
				int num25 = list[num24] - list[num24 + 1];
				num23 -= num25;
				int num26 = num23;
				if (num26 < 0)
				{
					num26 += _crvPointCount;
				}
				if (num26 > _crvPointCount)
				{
					num26 -= _crvPointCount;
				}
				list.Add(num26);
				list2.Add(list2[num24]);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		double num27 = 0.1;
		for (int num28 = 0; num28 < list2.Count; num28++)
		{
			num27 = Math.Max(num27, list2[num28]);
		}
		bool flag2 = false;
		CSData cSData;
		if (InObject == null || InObject.OutputChannelBlock == null)
		{
			Sphere val3 = default(Sphere);
			((Sphere)(ref val3))._002Ector(Point3d.Origin, num27);
			cSData = (CSApplicationHelper.Application.ProductionMeshes ? new CSOnGeometryData((GeometryBase)(object)Mesh.CreateFromSphere(val3, Math.Max(40, (int)(num27 * 40.0 * 4.0)), Math.Max((int)(num27 * 40.0 * 4.0), 40))) : new CSOnGeometryData((GeometryBase)(object)Mesh.CreateFromSphere(val3, Math.Max(10, (int)(num27 * 40.0)), Math.Max((int)(num27 * 40.0), 10))));
			flag2 = true;
		}
		else
		{
			cSData = InObject.Data;
		}
		CSDataHelper.GetBrep(InNormal.Data, out var value9);
		CSDataHelper.GetNurbsCurve(InNormal.Data, out var value10);
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
				for (int num29 = 0; num29 < list.Count; num29++)
				{
					list3.Add(_crvPoints[list[num29]]);
				}
				Mesh geometry = CSUtilSweep.MeshPipe((Curve)(object)_masterCrv, list3.ToArray(), list2.ToArray(), 0.01);
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)geometry));
			}
			else
			{
				for (int num30 = 0; num30 < list.Count; num30++)
				{
					Point3d val4 = _crvPoints[list[num30]];
					Transform val5 = Transform.Scale(Plane.WorldXY, list2[num30] / num27, list2[num30] / num27, list2[num30] / num27);
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
			double num33 = default(double);
			for (int num31 = 0; num31 < list.Count; num31++)
			{
				int num32 = list[num31];
				Point3d val7 = _crvPoints[num32];
				((Vector3d)(ref val8))._002Ector(1.0, 0.0, 0.0);
				Point3d val9 = Point3d.Unset;
				if (value9 != null)
				{
					CSUtilGeometry.GetClosestNormalToBrep(value9, val7, ref brepNormal);
				}
				else if (value10 != null)
				{
					((Curve)value10).ClosestPoint(val7, ref num33);
					val9 = ((Curve)value10).PointAt(num33);
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
				if (num32 + 1 < _crvPoints.Length)
				{
					val8 = _crvPoints[num32 + 1] - val7;
				}
				else if (num32 - 1 >= 0)
				{
					val8 = val7 - _crvPoints[num32 - 1];
				}
				((Vector3d)(ref val8)).Unitize();
				Transform val10 = Transform.Scale(Plane.WorldXY, list2[num31], list2[num31], list2[num31]);
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

	private int CurvePointFixed(int val)
	{
		int i;
		for (i = val; i < 0; i += _crvPointCount)
		{
		}
		return i % _crvPointCount;
	}
}
