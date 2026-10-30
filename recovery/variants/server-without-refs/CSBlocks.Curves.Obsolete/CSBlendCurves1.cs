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
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
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
		Interval domain;
		Vector3d vectorStart;
		if (value3)
		{
			pointStart = ((Curve)value).PointAtStart;
			NurbsCurve obj = value;
			domain = ((Curve)value).Domain;
			vectorStart = -((Curve)obj).TangentAt(((Interval)(ref domain)).Min);
		}
		else
		{
			pointStart = ((Curve)value).PointAtEnd;
			NurbsCurve obj2 = value;
			domain = ((Curve)value).Domain;
			vectorStart = ((Curve)obj2).TangentAt(((Interval)(ref domain)).Max);
		}
		Point3d pointEnd;
		Vector3d vectorEnd;
		if (value4)
		{
			pointEnd = ((Curve)value2).PointAtStart;
			NurbsCurve obj3 = value2;
			domain = ((Curve)value2).Domain;
			vectorEnd = -((Curve)obj3).TangentAt(((Interval)(ref domain)).Min);
		}
		else
		{
			pointEnd = ((Curve)value2).PointAtEnd;
			NurbsCurve obj4 = value2;
			domain = ((Curve)value2).Domain;
			vectorEnd = ((Curve)obj4).TangentAt(((Interval)(ref domain)).Max);
		}
		NurbsCurve val = CSUtilCurves.BlendCurve(pointStart, pointEnd, vectorStart, vectorEnd, value7, value5, value6);
		if (val == null)
		{
			return UpdateError("Error with blend");
		}
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val));
		return base.Update();
	}
}
