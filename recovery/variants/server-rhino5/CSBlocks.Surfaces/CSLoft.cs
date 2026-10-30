using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Surfaces;

[CSBlock("Loft", "{C436D730-1E1F-47af-A1D8-CAFEF28C629A}")]
public sealed class CSLoft : CSBlock
{
	public CSInputChannel iCurve1 { get; }

	public CSInputChannel iCurve2 { get; }

	public CSOutputChannel oSurface { get; }

	public CSLoft(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(GetPropertyName(() => iCurve1), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(GetPropertyName(() => iCurve2), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve1.GetFlattenArray();
		CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
		if (flattenArray.GetArrayCount() < 1 || flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Curve");
		}
		OnNurbsCurve val = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		OnNurbsCurve val2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val == null || val2 == null)
		{
			return UpdateMissing("Missing Curve");
		}
		OnBrep val3 = CSUtilSweep.LoftMatchCurves((IOnCurve[])(object)new IOnCurve[2]
		{
			(IOnCurve)val,
			(IOnCurve)val2
		}, (eLoftType)3, capped: false);
		if (val3 == null)
		{
			return UpdateError("Failed Loft");
		}
		oSurface.SetData(new CSOnGeometryData(val3.Convert()));
		return base.Update();
	}
}
