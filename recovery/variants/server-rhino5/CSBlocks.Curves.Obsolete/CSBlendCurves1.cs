using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("BlendCurves_V1", "{2BF21C47-9592-4b47-A151-D4196ECD854E}")]
public sealed class CSBlendCurves1 : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iStartCurve1;

	public readonly CSInputChannel iStartCurve2;

	public readonly CSInputChannel iAmount1;

	public readonly CSInputChannel iAmount2;

	public readonly CSInputChannel iAmountTotal;

	public readonly CSOutputChannel oCurve;

	public CSBlendCurves1(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		iStartCurve1 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iStartCurve2 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iAmount1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmountTotal = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve1.GetFlattenArray();
		CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0 || flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curves");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
		if (value == null || value2 == null)
		{
			return UpdateMissing("Missing Curves");
		}
		bool value3 = true;
		iStartCurve1.GetBoolean(ref value3);
		bool value4 = true;
		iStartCurve2.GetBoolean(ref value4);
		double value5 = 1.0;
		double value6 = 1.0;
		if (iAmount1 != null)
		{
			iAmount1.GetDouble(ref value5);
		}
		if (iAmount2 != null)
		{
			iAmount2.GetDouble(ref value6);
		}
		double value7 = 1.0;
		if (iAmountTotal != null)
		{
			iAmountTotal.GetDouble(ref value7);
		}
		Point3d pointStart;
		Vector3d vectorStart;
		if (value3)
		{
			pointStart = value.PointAtStart;
			vectorStart = -value.TangentAt(value.Domain.Min);
		}
		else
		{
			pointStart = value.PointAtEnd;
			vectorStart = value.TangentAt(value.Domain.Max);
		}
		Point3d pointEnd;
		Vector3d vectorEnd;
		if (value4)
		{
			pointEnd = value2.PointAtStart;
			vectorEnd = -value2.TangentAt(value2.Domain.Min);
		}
		else
		{
			pointEnd = value2.PointAtEnd;
			vectorEnd = value2.TangentAt(value2.Domain.Max);
		}
		NurbsCurve nurbsCurve = CSUtilCurves.BlendCurve(pointStart, pointEnd, vectorStart, vectorEnd, value7, value5, value6);
		if (nurbsCurve == null)
		{
			return UpdateError("Error with blend");
		}
		oCurve.SetData(new CSOnGeometryData(nurbsCurve));
		return base.Update();
	}
}
