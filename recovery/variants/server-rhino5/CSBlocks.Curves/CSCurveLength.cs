using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Curves;

[CSBlock("CurveLength", "{92715323-C862-4993-A00C-41180FEE15AB}")]
public sealed class CSCurveLength : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSOutputChannel oLengths;

	public CSCurveLength(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oLengths = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curves");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value);
			if (value != null)
			{
				double length = value.GetLength();
				cSDataArray.Append(new CSNumberData(length));
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oLengths.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oLengths.SetData(cSDataArray);
		}
		return base.Update();
	}
}
