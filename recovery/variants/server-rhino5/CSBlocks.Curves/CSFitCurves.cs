using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("FitCurves", "{1B6EB907-26FA-4894-AA32-1452DB6039E0}")]
public sealed class CSFitCurves : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSInputChannel iDegree;

	public readonly CSInputChannel iTolerance;

	public readonly CSOutputChannel oCurves;

	public CSFitCurves(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iDegree = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTolerance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curves");
		}
		int value = 3;
		iDegree.GetInteger(ref value);
		double value2 = 0.05;
		iTolerance.GetDouble(ref value2);
		iDegree.GetInteger(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetNurbsCurve(arrayItem, out var value3);
			if (value3 != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
				if (!value4.IsValid)
				{
					value4 = Transform.Identity;
				}
				value4.TryGetInverse(out var inverseTransform);
				value3.Transform(inverseTransform);
				NurbsCurve nurbsCurve = value3.Fit(value, value2, 0.1).ToNurbsCurve();
				nurbsCurve.Transform(value4);
				cSDataArray.Append(new CSOnGeometryData(nurbsCurve, value4, applyTransform: false));
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
