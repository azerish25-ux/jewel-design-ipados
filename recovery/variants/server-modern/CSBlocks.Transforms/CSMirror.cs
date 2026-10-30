using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("Mirror", "{486CF955-16F3-4A1B-9964-1A3FB4DF426D}")]
public sealed class CSMirror : CSBlock
{
	public CSInputChannel InObj { get; }

	public CSInputChannel InDuplicate { get; }

	public CSInputChannel InX { get; }

	public CSInputChannel InY { get; }

	public CSInputChannel InZ { get; }

	public CSInputChannel InXform { get; }

	public CSOutputChannel OutObj { get; }

	public CSMirror(string identifier)
		: base(identifier)
	{
		InObj = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InDuplicate = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InX = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InY = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InZ = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InXform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutObj = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InObj.GetFlattenArray();
		bool boolean = InDuplicate.Boolean;
		bool boolean2 = InX.Boolean;
		bool boolean3 = InY.Boolean;
		bool boolean4 = InZ.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(InXform.Data, out var value);
		Transform val = Transform.Identity;
		Transform val2 = Transform.Identity;
		Transform val3 = default(Transform);
		if (((Transform)(ref value)).IsValid && ((Transform)(ref value)).TryGetInverse(ref val3))
		{
			val = val3;
			val2 = value;
		}
		if (boolean)
		{
			if (boolean2)
			{
				Transform val4 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform = val2 * val4 * val;
				for (int i = 0; (double)i < num; i++)
				{
					CSData data = cSDataArray.GetArrayItem(i).Duplicate();
					CSDataHelper.Transform(data, transform);
					if (CSUtilMath.IsFlipTransform(transform))
					{
						CSDataHelper.Flip(data);
					}
					cSDataArray.Append(data);
				}
				for (int j = 0; j < flattenArray.GetArrayCount(); j++)
				{
					CSData data2 = flattenArray.GetArrayItem(j).Duplicate();
					CSDataHelper.Transform(data2, transform);
					if (CSUtilMath.IsFlipTransform(transform))
					{
						CSDataHelper.Flip(data2);
					}
					cSDataArray.Append(data2);
				}
			}
			if (boolean3)
			{
				Transform val4 = Transform.Scale(Plane.WorldXY, 1.0, -1.0, 1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform2 = val2 * val4 * val;
				for (int k = 0; (double)k < num; k++)
				{
					CSData data3 = cSDataArray.GetArrayItem(k).Duplicate();
					CSDataHelper.Transform(data3, transform2);
					if (CSUtilMath.IsFlipTransform(transform2))
					{
						CSDataHelper.Flip(data3);
					}
					cSDataArray.Append(data3);
				}
				for (int l = 0; l < flattenArray.GetArrayCount(); l++)
				{
					CSData data4 = flattenArray.GetArrayItem(l).Duplicate();
					CSDataHelper.Transform(data4, transform2);
					if (CSUtilMath.IsFlipTransform(transform2))
					{
						CSDataHelper.Flip(data4);
					}
					cSDataArray.Append(data4);
				}
			}
			if (boolean4)
			{
				Transform val4 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, -1.0);
				double num = cSDataArray.GetArrayCount();
				Transform transform3 = val2 * val4 * val;
				for (int m = 0; (double)m < num; m++)
				{
					CSData data5 = cSDataArray.GetArrayItem(m).Duplicate();
					CSDataHelper.Transform(data5, transform3);
					cSDataArray.Append(data5);
				}
				for (int n = 0; n < flattenArray.GetArrayCount(); n++)
				{
					CSData data6 = flattenArray.GetArrayItem(n).Duplicate();
					CSDataHelper.Transform(data6, transform3);
					if (CSUtilMath.IsFlipTransform(transform3))
					{
						CSDataHelper.Flip(data6);
					}
					cSDataArray.Append(data6);
				}
			}
		}
		else
		{
			int num2 = 1;
			if (boolean2)
			{
				num2 = -1;
			}
			int num3 = 1;
			if (boolean3)
			{
				num3 = -1;
			}
			int num4 = 1;
			if (boolean4)
			{
				num4 = -1;
			}
			Transform val4 = Transform.Scale(Plane.WorldXY, (double)num2, (double)num3, (double)num4);
			Transform transform4 = val2 * val4 * val;
			for (int num5 = 0; num5 < flattenArray.GetArrayCount(); num5++)
			{
				CSData data7 = flattenArray.GetArrayItem(num5).Duplicate();
				CSDataHelper.Transform(data7, transform4);
				if (CSUtilMath.IsFlipTransform(transform4))
				{
					CSDataHelper.Flip(data7);
				}
				cSDataArray.Append(data7);
			}
		}
		OutObj.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
