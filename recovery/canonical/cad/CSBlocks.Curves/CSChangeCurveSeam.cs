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
					value = value3.PointAtStart;
				}
				else
				{
					BoundingBox boundingBox = BoundingBox.Unset;
					if (iObjectOrNumberToMoveTo.Data.GetTightBoundingBox(ref boundingBox))
					{
						value = boundingBox.Center;
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
			if (!value4.IsClosed)
			{
				if (flag)
				{
					if (value2 > 0.5)
					{
						value4.Reverse();
					}
				}
				else if (value != Point3d.Unset)
				{
					double t = value4.Domain.Min;
					value4.ClosestPoint(value, out t);
					Point3d point3d = value4.PointAt(t);
					if (point3d.DistanceTo(value4.PointAtStart) > point3d.DistanceTo(value4.PointAtEnd))
					{
						value4.Reverse();
					}
				}
			}
			else if (flag)
			{
				double t2 = 0.0;
				value4.NormalizedLengthParameter(value2, out t2);
				value4.ChangeClosedCurveSeam(t2);
			}
			else if (value != Point3d.Unset)
			{
				double t3 = value4.Domain.Min;
				value4.ClosestPoint(value, out t3);
				value4.ChangeClosedCurveSeam(t3);
			}
			bool value5 = false;
			iFlipCurve.GetBoolean(ref value5);
			if (value5)
			{
				value4.Reverse();
			}
			cSDataArray.Append(new CSOnGeometryData(value4));
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
