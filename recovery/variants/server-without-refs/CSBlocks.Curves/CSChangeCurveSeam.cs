using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("ChangeCurveSeam", "{CA578749-045F-4c5f-A39F-34FEB9753FA1}")]
public sealed class CSChangeCurveSeam : CSBlock
{
	public readonly CSInputChannel iObjectOrNumberToMoveTo;

	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iFlipCurve;

	public readonly CSOutputChannel oCurve;

	public CSChangeCurveSeam(string identifier)
		: base(identifier)
	{
		iObjectOrNumberToMoveTo = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iFlipCurve = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iObjectOrNumberToMoveTo.GetFlattenArray();
		Point3d value = Point3d.Unset;
		bool flag = false;
		double value2 = 0.0;
		if (flattenArray.GetArrayCount() >= 1)
		{
			CSData arrayItem = flattenArray.GetArrayItem(0);
			if (arrayItem is ICSPoint iCSPoint)
			{
				iCSPoint.GetPoint(out value);
			}
			if (value == Point3d.Unset)
			{
				CSDataHelper.GetNurbsCurve(arrayItem, out var value3);
				if (value3 != null)
				{
					value = ((Curve)value3).PointAtStart;
				}
				else
				{
					BoundingBox boundingBox = BoundingBox.Unset;
					if (iObjectOrNumberToMoveTo.Data.GetTightBoundingBox(ref boundingBox))
					{
						value = ((BoundingBox)(ref boundingBox)).Center;
					}
					else if (iObjectOrNumberToMoveTo.GetDouble(ref value2))
					{
						flag = true;
					}
				}
			}
		}
		CSDataArray flattenArray2 = iCurve.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray2.GetArrayCount(); i++)
		{
			CSData arrayItem2 = flattenArray2.GetArrayItem(i);
			if (arrayItem2 == null)
			{
				continue;
			}
			CSDataHelper.GetNurbsCurve(arrayItem2, out var value4);
			if (value4 == null)
			{
				continue;
			}
			Interval domain;
			if (!((Curve)value4).IsClosed)
			{
				if (flag)
				{
					if (value2 > 0.5)
					{
						((Curve)value4).Reverse();
					}
				}
				else if (value != Point3d.Unset)
				{
					domain = ((Curve)value4).Domain;
					double min = ((Interval)(ref domain)).Min;
					((Curve)value4).ClosestPoint(value, ref min);
					Point3d val = ((Curve)value4).PointAt(min);
					if (((Point3d)(ref val)).DistanceTo(((Curve)value4).PointAtStart) > ((Point3d)(ref val)).DistanceTo(((Curve)value4).PointAtEnd))
					{
						((Curve)value4).Reverse();
					}
				}
			}
			else if (flag)
			{
				double num = 0.0;
				((Curve)value4).NormalizedLengthParameter(value2, ref num);
				((Curve)value4).ChangeClosedCurveSeam(num);
			}
			else if (value != Point3d.Unset)
			{
				domain = ((Curve)value4).Domain;
				double min2 = ((Interval)(ref domain)).Min;
				((Curve)value4).ClosestPoint(value, ref min2);
				((Curve)value4).ChangeClosedCurveSeam(min2);
			}
			bool value5 = false;
			iFlipCurve.GetBoolean(ref value5);
			if (value5)
			{
				((Curve)value4).Reverse();
			}
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)value4));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
