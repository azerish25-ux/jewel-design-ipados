using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Transforms;

[CSBlock("InverseXform", "{4080CB45-5F4A-4d0f-85C2-A54F86E1BE67}")]
public sealed class CSInverseXform : CSBlock
{
	public readonly CSInputChannel iXform;

	public readonly CSOutputChannel oXform;

	public CSInverseXform(string identifier)
		: base(identifier)
	{
		iXform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetTransformNoArray(iXform.Data, out var value);
		if (!value.IsValid)
		{
			return UpdateMissing("Xform");
		}
		value.TryGetInverse(out var inverseTransform);
		oXform.SetData(new CSXformData(inverseTransform));
		return base.Update();
	}
}
