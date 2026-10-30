using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveCornerBlend", "{548C8C81-38C2-46b3-AB66-1ACFBFAEEDC4}")]
public sealed class CSCurveCornerBlend : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSInputChannel iBlendSize;

	public readonly CSInputChannel iBlendAmount;

	public readonly CSInputChannel iAngleMin;

	public readonly CSInputChannel iAngleMax;

	public readonly CSOutputChannel oCurves;

	public CSCurveCornerBlend(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iBlendSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngleMin = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngleMax = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		double value = 1.0;
		iAngleMin.GetDouble(ref value);
		double value2 = 180.0;
		iAngleMax.GetDouble(ref value2);
		double value3 = 0.1;
		iBlendSize.GetDouble(ref value3);
		double value4 = 1.0;
		iBlendAmount.GetDouble(ref value4);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			CSDataHelper.GetNurbsCurve(arrayItem, out var value5);
			if (value5 != null)
			{
				if (!value5.IsClosed && value5.IsClosable(CSApplicationHelper.Application.Tolerance))
				{
					Curve.MakeEndsMeet(value5, adjustStartCurveA: false, value5, adjustStartCurveB: true);
				}
				Curve outputCurve = null;
				CSUtilCurves.BlendCurveCorners(value5, CSBlockUtil.CSUtilMath.ToRadians(value), CSBlockUtil.CSUtilMath.ToRadians(value2), value3, value4, ref outputCurve);
				if (outputCurve != null)
				{
					cSDataArray.Append(new CSOnGeometryData(outputCurve));
				}
				else
				{
					cSDataArray.Append(new CSOnGeometryData(value5));
				}
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oCurves.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oCurves.SetData(cSDataArray);
		}
		return base.Update();
	}
}
