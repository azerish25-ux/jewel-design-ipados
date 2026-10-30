using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

[CSBlock("SurfaceOffset", "{973027B8-3814-44FF-96C2-3213B89FB3C0}")]
public sealed class CSSurfaceOffset : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iDistance;

	public readonly CSOutputChannel oSurface;

	public CSSurfaceOffset(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface, ChannelArrayType.Array | ChannelArrayType.Individual);
		iDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		double value = double.NaN;
		if (!iDistance.GetDouble(ref value))
		{
			UpdateError("Error getting distance");
		}
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is ICSSurface iCSSurface)
			{
				Surface geometry = iCSSurface.GetSurface().Offset(value, CSApplicationHelper.Application.Tolerance);
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)geometry));
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oSurface.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oSurface.SetData(cSDataArray);
		}
		return base.Update();
	}
}
