using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("BlendCurvesAdvanced", "{AB731785-76E1-481D-A402-DBE94F91CB92}")]
public sealed class CSBlendCurvesAdvanced : CSBlock
{
	public readonly CSInputChannel InCurve1;

	public readonly CSInputChannel InCurve2;

	public readonly CSInputChannel InDoBlendFromStart1;

	public readonly CSInputChannel InDoBlendFromStart2;

	public readonly CSInputChannel InG1;

	public readonly CSInputChannel InG2;

	public readonly CSInputChannel InBlendStartA;

	public readonly CSInputChannel InBlendStartB;

	public readonly CSInputChannel InBlendStartC;

	public readonly CSInputChannel InBlendEndA;

	public readonly CSInputChannel InBlendEndB;

	public readonly CSInputChannel InBlendEndC;

	public readonly CSOutputChannel OutCurve;

	public CSBlendCurvesAdvanced(string identifier)
		: base(identifier)
	{
		InCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InDoBlendFromStart1 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoBlendFromStart2 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InG1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InG2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendStartA = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendStartB = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendStartC = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendEndA = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendEndB = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendEndC = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InCurve1.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curve Data 1");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Curve 1");
		}
		CSDataArray flattenArray2 = InCurve2.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curve Data 2");
		}
		CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Missing Curve 2");
		}
		bool value3 = true;
		InDoBlendFromStart1.GetBoolean(ref value3);
		bool value4 = true;
		InDoBlendFromStart2.GetBoolean(ref value4);
		int value5 = 1;
		InG1.GetInteger(ref value5);
		int value6 = 1;
		InG2.GetInteger(ref value6);
		double value7 = 1.0;
		InBlendStartA.GetDouble(ref value7);
		double value8 = 1.0;
		InBlendStartB.GetDouble(ref value8);
		double value9 = 1.0;
		InBlendStartC.GetDouble(ref value9);
		double value10 = 1.0;
		InBlendEndA.GetDouble(ref value10);
		double value11 = 1.0;
		InBlendEndB.GetDouble(ref value11);
		double value12 = 1.0;
		InBlendEndC.GetDouble(ref value12);
		Curve curve = CSUtilCurves.BlendCurveAdvanced(value, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, doClean: true);
		if (curve == null)
		{
			return UpdateError("BlendCurveAdvanced failed");
		}
		OutCurve.SetData(new CSOnGeometryData(curve));
		return base.Update();
	}
}
