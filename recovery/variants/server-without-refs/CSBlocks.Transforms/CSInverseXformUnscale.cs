using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

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
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetTransformNoArray(iXform.Data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			return UpdateMissing("Xform");
		}
		bool value2 = true;
		iScale.GetBoolean(ref value2);
		if (value2)
		{
			Transform transform = default(Transform);
			((Transform)(ref value)).TryGetInverse(ref transform);
			oXform.SetData(new CSXformData(transform));
		}
		else
		{
			CSBlockUtil.CSUtilMath.GetUnscaledXform(value, out var xUp);
			if (!((Transform)(ref xUp)).IsValid)
			{
				return UpdateError("Bad Xform");
			}
			Transform transform2 = default(Transform);
			((Transform)(ref xUp)).TryGetInverse(ref transform2);
			oXform.SetData(new CSXformData(transform2));
		}
		return base.Update();
	}
}
