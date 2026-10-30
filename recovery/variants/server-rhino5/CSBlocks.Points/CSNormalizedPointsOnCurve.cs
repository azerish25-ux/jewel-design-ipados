using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Points;

[CSBlock("NormalizedPointsOnCurve", "{145B8CEF-5D1B-414A-861D-8D4F17B3804C}")]
public sealed class CSNormalizedPointsOnCurve : CSBlock
{
	public readonly CSOutputChannel oTransformArray;

	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iSpacing;

	public readonly CSInputChannel iStart;

	public readonly CSInputChannel iEnd;

	public readonly CSInputChannel iRounding;

	public readonly CSInputChannel iOtherAxis;

	public CSNormalizedPointsOnCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRounding = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iOtherAxis = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		oTransformArray = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!CSDataHelper.GetCurve(iCurve, out var value))
		{
			return UpdateError("Error getting Curve");
		}
		double value2 = double.NaN;
		iStart.GetDouble(ref value2);
		if (double.IsNaN(value2))
		{
			return UpdateError("Error getting start of domain");
		}
		double value3 = double.NaN;
		iEnd.GetDouble(ref value3);
		if (double.IsNaN(value3))
		{
			return UpdateError("Error getting end of domain");
		}
		if (value2 > value3)
		{
			double num = value2;
			value2 = value3;
			value3 = num;
		}
		bool flag = Math.Abs(value2) < 0.001 && Math.Abs(value3) > 0.999 && value.IsClosed;
		if (!value.NormalizedLengthParameter(value2, out var t))
		{
			return UpdateError("Error getting Normalized length");
		}
		if (!value.NormalizedLengthParameter(value3, out var t2))
		{
			return UpdateError("Error getting Normalized length");
		}
		double length = value.GetLength(new Interval(value.Domain.Min, t));
		double num2 = value.GetLength(new Interval(value.Domain.Min, t2)) - length;
		bool value4 = false;
		iRounding.GetBoolean(ref value4);
		double value5 = double.NaN;
		iSpacing.GetDouble(ref value5);
		double num3 = num2 / value5;
		int num4 = (int)(value4 ? Math.Ceiling(num3) : Math.Floor(num3));
		int num5 = num4;
		if (flag)
		{
			num5--;
		}
		ValueListItem whichItem = ValueListItem.SortAxisXNeg;
		iOtherAxis.GetValueListItem(ref whichItem);
		double x = 0.0;
		double y = 0.0;
		double z = 0.0;
		switch (whichItem)
		{
		case ValueListItem.SortAxisXPos:
			x = 1.0;
			break;
		case ValueListItem.SortAxisXNeg:
			x = -1.0;
			break;
		case ValueListItem.SortAxisYPos:
			y = 1.0;
			break;
		case ValueListItem.SortAxisYNeg:
			y = -1.0;
			break;
		case ValueListItem.SortAxisZPos:
			z = 1.0;
			break;
		case ValueListItem.SortAxisZNeg:
			z = -1.0;
			break;
		default:
			return UpdateError("Invalid Sort Direction");
		}
		Vector3d destination = new Vector3d(x, y, z);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i <= num5; i++)
		{
			double num6 = value2 + (value3 - value2) * (double)i / (double)num4;
			Point3d destination2 = value.PointAtNormalizedLength(num6);
			if (value.NormalizedLengthParameter(num6, out var t3))
			{
				Vector3d destination3 = value.TangentAt(t3);
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, destination2, destination3, destination);
				cSDataArray.Append(new CSXformData(transform));
			}
		}
		oTransformArray.SetData(cSDataArray);
		return base.Update();
	}
}
