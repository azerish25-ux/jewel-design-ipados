using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("BlendCurves", "{E0BD8818-2E3D-4BA6-BBA9-6C2D97489682}")]
public sealed class CSBlendCurves : CSBlock
{
	public CSInputChannel InCurve1;

	public CSInputChannel InCurve2;

	public CSInputChannel InDoBlendFromStart1;

	public CSInputChannel InDoBlendFromStart2;

	public CSInputChannel InBlendStart;

	public CSInputChannel InBlendEnd;

	public CSInputChannel InBlendAmount;

	public CSOutputChannel OutCurve;

	public CSBlendCurves(string identifier)
		: base(identifier)
	{
		InCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InDoBlendFromStart1 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InDoBlendFromStart2 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InBlendStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		double value5 = 1.0;
		InBlendStart.GetDouble(ref value5);
		double value6 = 1.0;
		InBlendEnd.GetDouble(ref value6);
		double value7 = 1.0;
		InBlendAmount.GetDouble(ref value7);
		Curve curve = CSUtilCurves.BlendCurve((Curve)value, (Curve)value2, value3, value4, value7, value5, value6, true);
		if (curve == null)
		{
			return UpdateError("BlendCurve failed");
		}
		OutCurve.SetData(new CSOnGeometryData(curve));
		return base.Update();
	}
}
