using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;

namespace CSBlocks.Transforms;

[CSBlock("InverseXformUnscale", "{464027B8-C645-434F-8640-AADCEF857ACE}")]
public sealed class CSInverseXformUnscale : CSBlock
{
	public readonly CSInputChannel iXform;

	public readonly CSInputChannel iScale;

	public readonly CSOutputChannel oXform;

	public CSInverseXformUnscale(string identifier)
		: base(identifier)
	{
		iXform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		iScale = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
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
		bool value2 = true;
		iScale.GetBoolean(ref value2);
		if (value2)
		{
			value.TryGetInverse(out var inverseTransform);
			oXform.SetData(new CSXformData(inverseTransform));
		}
		else
		{
			CSBlockUtil.CSUtilMath.GetUnscaledXform(value, out var xUp);
			if (!xUp.IsValid)
			{
				return UpdateError("Bad Xform");
			}
			xUp.TryGetInverse(out var inverseTransform2);
			oXform.SetData(new CSXformData(inverseTransform2));
		}
		return base.Update();
	}
}
