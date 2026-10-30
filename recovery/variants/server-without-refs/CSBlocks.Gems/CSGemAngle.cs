using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("GemAngle", "{67FC3E88-94BC-4303-B8AB-170788432E2A}")]
public sealed class CSGemAngle : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSOutputChannel OutFirstGem;

	public readonly CSOutputChannel OutLastGem;

	public readonly CSOutputChannel OutFirstAngle;

	public readonly CSOutputChannel OutLastAngle;

	public CSGemAngle(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutFirstGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		OutLastGem = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		OutFirstAngle = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutLastAngle = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGems.GemList;
		double num = 180.0;
		double num2 = 0.0;
		CSGemData data = null;
		CSGemData data2 = null;
		Point3d val = default(Point3d);
		Vector3d val3 = default(Vector3d);
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			Point3d center = cSGemData.Center;
			((Point3d)(ref val))._002Ector(0.0, ((Point3d)(ref center)).Y, 0.0);
			Vector3d val2 = center - val;
			((Vector3d)(ref val3))._002Ector(new Point3d(0.0, ((Point3d)(ref center)).Y, 1.0) - new Point3d(0.0, ((Point3d)(ref center)).Y, 0.0));
			double num3 = CSBlockUtil.CSUtilMath.ToDegrees(Vector3d.VectorAngle(val2, val3));
			if (((Point3d)(ref center)).X < 0.0)
			{
				num3 = 360.0 - num3;
			}
			if (num3 < num)
			{
				num = num3;
				data = cSGemData;
			}
			if (num3 > 180.0 && num2 > 180.0)
			{
				if (num3 < num2)
				{
					num2 = num3;
					data2 = cSGemData;
				}
			}
			else if (num3 > num2 && num3 < 180.0)
			{
				num2 = num3;
				data2 = cSGemData;
			}
		}
		OutFirstAngle.SetData(new CSNumberData(num));
		OutLastAngle.SetData(new CSNumberData(num2));
		OutFirstGem.SetData(data);
		OutLastGem.SetData(data2);
		return base.Update();
	}
}
