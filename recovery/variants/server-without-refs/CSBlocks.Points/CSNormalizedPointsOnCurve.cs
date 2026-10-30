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
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
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
		double num2 = default(double);
		if (!value.NormalizedLengthParameter(value2, ref num2))
		{
			return UpdateError("Error getting Normalized length");
		}
		double num3 = default(double);
		if (!value.NormalizedLengthParameter(value3, ref num3))
		{
			return UpdateError("Error getting Normalized length");
		}
		Curve obj = value;
		Interval domain = value.Domain;
		double length = obj.GetLength(new Interval(((Interval)(ref domain)).Min, num2));
		Curve obj2 = value;
		domain = value.Domain;
		double num4 = obj2.GetLength(new Interval(((Interval)(ref domain)).Min, num3)) - length;
		bool value4 = false;
		iRounding.GetBoolean(ref value4);
		double value5 = double.NaN;
		iSpacing.GetDouble(ref value5);
		double num5 = num4 / value5;
		int num6 = (int)(value4 ? Math.Ceiling(num5) : Math.Floor(num5));
		int num7 = num6;
		if (flag)
		{
			num7--;
		}
		ValueListItem whichItem = ValueListItem.SortAxisXNeg;
		iOtherAxis.GetValueListItem(ref whichItem);
		double num8 = 0.0;
		double num9 = 0.0;
		double num10 = 0.0;
		switch (whichItem)
		{
		case ValueListItem.SortAxisXPos:
			num8 = 1.0;
			break;
		case ValueListItem.SortAxisXNeg:
			num8 = -1.0;
			break;
		case ValueListItem.SortAxisYPos:
			num9 = 1.0;
			break;
		case ValueListItem.SortAxisYNeg:
			num9 = -1.0;
			break;
		case ValueListItem.SortAxisZPos:
			num10 = 1.0;
			break;
		case ValueListItem.SortAxisZNeg:
			num10 = -1.0;
			break;
		default:
			return UpdateError("Invalid Sort Direction");
		}
		Vector3d destination = default(Vector3d);
		((Vector3d)(ref destination))._002Ector(num8, num9, num10);
		CSDataArray cSDataArray = new CSDataArray();
		double num12 = default(double);
		for (int i = 0; i <= num7; i++)
		{
			double num11 = value2 + (value3 - value2) * (double)i / (double)num6;
			Point3d destination2 = value.PointAtNormalizedLength(num11);
			if (value.NormalizedLengthParameter(num11, ref num12))
			{
				Vector3d destination3 = value.TangentAt(num12);
				Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, destination2, destination3, destination);
				cSDataArray.Append(new CSXformData(transform));
			}
		}
		oTransformArray.SetData(cSDataArray);
		return base.Update();
	}
}
