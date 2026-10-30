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
			OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			double t = onNurbsCurve.Domain().Max();
			if (value.IsValid)
			{
				if (!onNurbsCurve.GetClosestPoint(value.Convert(), ref t, value5))
				{
					return UpdateError("GetClosestPoint failed (p1)");
				}
			}
			else if (!onNurbsCurve.GetNormalizedArcLengthPoint(value2, ref t))
			{
				return UpdateError("GetNormalizedArcLengthPoint (p1)");
			}
			if (t < onNurbsCurve.Domain().Min())
			{
				t = onNurbsCurve.Domain().Min();
			}
			if (t > onNurbsCurve.Domain().Max())
			{
				t = onNurbsCurve.Domain().Max();
			}
			if (!flag)
			{
				OnCurve left_side = null;
				OnCurve right_side = null;
				if (!onNurbsCurve.Split(t, ref left_side, ref right_side))
				{
					return UpdateError("Split failed");
				}
				cSDataArray.Append((left_side != null) ? new CSOnGeometryData(left_side.Convert()) : new CSData());
				cSDataArray2.Append((right_side != null) ? new CSOnGeometryData(right_side.Convert()) : new CSData());
				cSDataArray3.Append(new CSData());
				continue;
			}
			double t2 = onNurbsCurve.Domain().Max();
			if (value3.IsValid)
			{
				if (!onNurbsCurve.GetClosestPoint(value3.Convert(), ref t2, value5))
				{
					return UpdateError("GetClosestPoint failed (p2)");
				}
			}
			else if (!onNurbsCurve.GetNormalizedArcLengthPoint(value4, ref t2))
			{
				return UpdateError("GetNormalizedArcLengthPoint (p2)");
			}
			if (t2 < onNurbsCurve.Domain().Min())
			{
				t2 = onNurbsCurve.Domain().Min();
			}
			if (t2 > onNurbsCurve.Domain().Max())
			{
				t2 = onNurbsCurve.Domain().Max();
			}
			if (t > t2)
			{
				double num = t;
				t = t2;
				t2 = num;
			}
			OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
			OnNurbsCurve onNurbsCurve3 = onNurbsCurve.Duplicate();
			OnNurbsCurve onNurbsCurve4 = onNurbsCurve.Duplicate();
			onNurbsCurve2.Trim(new OnInterval(onNurbsCurve.Domain().Min(), t));
			onNurbsCurve3.Trim(new OnInterval(t, t2));
			onNurbsCurve4.Trim(new OnInterval(t2, onNurbsCurve.Domain().Max()));
			cSDataArray.Append(new CSOnGeometryData(onNurbsCurve2.Convert()));
			cSDataArray2.Append(new CSOnGeometryData(onNurbsCurve3.Convert()));
			cSDataArray3.Append(new CSOnGeometryData(onNurbsCurve4.Convert()));
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
