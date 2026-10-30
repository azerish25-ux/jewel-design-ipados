using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetTransformNoArray(iXform.Data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			return UpdateMissing("Xform");
		}
		Transform transform = default(Transform);
		((Transform)(ref value)).TryGetInverse(ref transform);
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
