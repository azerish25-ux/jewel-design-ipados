using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveSplit", "{BDA20234-F83E-4303-B6B1-4719489696B5}")]
public sealed class CSCurveSplit : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iPointParameter { get; }

	public CSInputChannel iMaxDistance { get; }

	public CSInputChannel iPointParameter2 { get; }

	public CSOutputChannel oCurve1 { get; }

	public CSOutputChannel oCurve2 { get; }

	public CSOutputChannel oCurve3 { get; }

	public CSCurveSplit(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPointParameter = new CSInputChannel(GetPropertyName(() => iPointParameter), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iMaxDistance = new CSInputChannel(GetPropertyName(() => iMaxDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPointParameter2 = new CSInputChannel(GetPropertyName(() => iPointParameter2), this, ChannelDataType.Any, ChannelArrayType.Individual);
		oCurve1 = new CSOutputChannel(GetPropertyName(() => oCurve1), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve2 = new CSOutputChannel(GetPropertyName(() => oCurve2), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve3 = new CSOutputChannel(GetPropertyName(() => oCurve3), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Missing Curve");
		}
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curve");
		}
		CSData data = iPointParameter.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Point Parameter");
		}
		Point3d value = Point3d.Unset;
		double value2 = -1.23432101234321E+308;
		if (!(data is ICSPoint iCSPoint))
		{
			if (!data.GetDouble(ref value2))
			{
				return UpdateMissing("Missing Point Parameter (Double)");
			}
		}
		else if (!iCSPoint.GetPoint(out value))
		{
			return UpdateMissing("Missing Point Parameter (Point)");
		}
		Point3d value3 = Point3d.Unset;
		double value4 = -1.23432101234321E+308;
		bool flag = false;
		CSData data2 = iPointParameter2.Data;
		if (data2 != null)
		{
			if (!(data2 is ICSPoint iCSPoint2))
			{
				if (data2.GetDouble(ref value4))
				{
					flag = true;
				}
			}
			else if (iCSPoint2.GetPoint(out value3))
			{
				flag = true;
			}
		}
		double value5 = 0.01;
		iMaxDistance.GetDouble(ref value5);
		if (value5 < 0.001)
		{
			value5 = 0.001;
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnNurbsCurve val = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			double num = ((OnCurve)val).Domain().Max();
			if (((Point3d)(ref value)).IsValid)
			{
				if (!((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)value.Convert(), ref num, value5))
				{
					return UpdateError("GetClosestPoint failed (p1)");
				}
			}
			else if (!((OnCurve)val).GetNormalizedArcLengthPoint(value2, ref num))
			{
				return UpdateError("GetNormalizedArcLengthPoint (p1)");
			}
			if (num < ((OnCurve)val).Domain().Min())
			{
				num = ((OnCurve)val).Domain().Min();
			}
			if (num > ((OnCurve)val).Domain().Max())
			{
				num = ((OnCurve)val).Domain().Max();
			}
			if (!flag)
			{
				OnCurve val2 = null;
				OnCurve val3 = null;
				if (!((OnCurve)val).Split(num, ref val2, ref val3))
				{
					return UpdateError("Split failed");
				}
				cSDataArray.Append((val2 != null) ? new CSOnGeometryData((GeometryBase)(object)val2.Convert()) : new CSData());
				cSDataArray2.Append((val3 != null) ? new CSOnGeometryData((GeometryBase)(object)val3.Convert()) : new CSData());
				cSDataArray3.Append(new CSData());
				continue;
			}
			double num2 = ((OnCurve)val).Domain().Max();
			if (((Point3d)(ref value3)).IsValid)
			{
				if (!((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)value3.Convert(), ref num2, value5))
				{
					return UpdateError("GetClosestPoint failed (p2)");
				}
			}
			else if (!((OnCurve)val).GetNormalizedArcLengthPoint(value4, ref num2))
			{
				return UpdateError("GetNormalizedArcLengthPoint (p2)");
			}
			if (num2 < ((OnCurve)val).Domain().Min())
			{
				num2 = ((OnCurve)val).Domain().Min();
			}
			if (num2 > ((OnCurve)val).Domain().Max())
			{
				num2 = ((OnCurve)val).Domain().Max();
			}
			if (num > num2)
			{
				double num3 = num;
				num = num2;
				num2 = num3;
			}
			OnNurbsCurve val4 = val.Duplicate();
			OnNurbsCurve val5 = val.Duplicate();
			OnNurbsCurve val6 = val.Duplicate();
			((OnCurve)val4).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), num));
			((OnCurve)val5).Trim((IOnInterval)new OnInterval(num, num2));
			((OnCurve)val6).Trim((IOnInterval)new OnInterval(num2, ((OnCurve)val).Domain().Max()));
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val4.Convert()));
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val5.Convert()));
			cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)val6.Convert()));
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oCurve1.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oCurve1.SetData(cSDataArray);
		}
		if (cSDataArray2.GetArrayCount() == 1)
		{
			oCurve2.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oCurve2.SetData(cSDataArray2);
		}
		if (cSDataArray3.GetArrayCount() == 1)
		{
			oCurve3.SetData(cSDataArray3.GetArrayItem(0));
		}
		else
		{
			oCurve3.SetData(cSDataArray3);
		}
		return base.Update();
	}
}
