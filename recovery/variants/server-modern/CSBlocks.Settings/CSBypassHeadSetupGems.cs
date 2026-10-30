using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("BypassHeadSetupGems", "{473BA11F-9E9A-48fc-A119-54181758D7C9}")]
public sealed class CSBypassHeadSetupGems : CSBlock
{
	public readonly CSInputChannel InGemCenter;

	public readonly CSInputChannel InGemSide1;

	public readonly CSInputChannel InGemSide2;

	public readonly CSOutputChannel OutXUp0;

	public readonly CSOutputChannel OutXUp1;

	public readonly CSOutputChannel OutXUp2;

	public readonly CSOutputChannel OutSize0;

	public readonly CSOutputChannel OutSize1;

	public readonly CSOutputChannel OutSize2;

	public readonly CSOutputChannel OutSide1;

	public readonly CSOutputChannel OutSide2;

	public CSBypassHeadSetupGems(string identifier)
		: base(identifier)
	{
		InGemCenter = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSide1 = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSide2 = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		OutXUp0 = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutXUp1 = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutXUp2 = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutSize0 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSize1 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSize2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSide1 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSide2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		CSGemData gem = InGemCenter.Gem;
		CSGemData gem2 = InGemSide1.Gem;
		CSGemData gem3 = InGemSide2.Gem;
		Transform xUp = Transform.Unset;
		Transform xUp2 = Transform.Unset;
		Transform xUp3 = Transform.Unset;
		CSBlockUtil.CSUtilMath.GetUnscaledXform(gem.Xform, out xUp);
		CSBlockUtil.CSUtilMath.GetUnscaledXform(gem2.Xform, out xUp2);
		CSBlockUtil.CSUtilMath.GetUnscaledXform(gem3.Xform, out xUp3);
		Plane val = default(Plane);
		((Plane)(ref val))._002Ector(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0));
		((Plane)(ref val)).Transform(xUp);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
		((Point3d)(ref val2)).Transform(xUp2);
		OutXUp0.SetData(new CSXformData(xUp));
		OutSize0.SetData(new CSNumberData(gem.GemXSize));
		if (((Plane)(ref val)).DistanceTo(val2) > 0.0)
		{
			OutXUp1.SetData(new CSXformData(xUp2));
			OutXUp2.SetData(new CSXformData(xUp3));
			OutSize1.SetData(new CSNumberData(gem2.GemXSize));
			OutSize2.SetData(new CSNumberData(gem3.GemXSize));
			OutSide1.SetData(new CSNumberData(1.0));
			OutSide2.SetData(new CSNumberData(-1.0));
		}
		else
		{
			OutXUp1.SetData(new CSXformData(xUp3));
			OutXUp2.SetData(new CSXformData(xUp2));
			OutSize1.SetData(new CSNumberData(gem3.GemXSize));
			OutSize2.SetData(new CSNumberData(gem2.GemXSize));
			OutSide1.SetData(new CSNumberData(-1.0));
			OutSide2.SetData(new CSNumberData(1.0));
		}
		return base.Update();
	}
}
