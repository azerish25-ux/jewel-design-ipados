using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Tools;

[CSBlock("IntersectCrvCrv", "{CBDA4D85-6416-435B-A145-552CC7700376}")]
public sealed class CSIntersectCrvCrv : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSOutputChannel oPoints;

	public readonly CSOutputChannel oParameters1;

	public readonly CSOutputChannel oParameters2;

	public CSIntersectCrvCrv(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oPoints = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Array);
		oParameters1 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParameters2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		OnNurbsCurve val = iCurve1.GetNurbsCurve();
		if (val == null)
		{
			CSDataArray flattenArray = iCurve1.GetFlattenArray();
			if (flattenArray.GetArrayCount() > 0)
			{
				val = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
			}
		}
		if (val == null)
		{
			return UpdateMissing("Missing Curve1");
		}
		OnNurbsCurve val2 = iCurve2.GetNurbsCurve();
		if (val2 == null)
		{
			CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
			if (flattenArray2.GetArrayCount() > 0)
			{
				val2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
			}
		}
		if (val2 == null)
		{
			return UpdateMissing("Missing Curve2");
		}
		ArrayOnX_EVENT val3 = new ArrayOnX_EVENT();
		((OnCurve)val).IntersectCurve((IOnCurve)(object)val2, ref val3, 0.01);
		if (val3.Count() > 0)
		{
			List<int> list = new List<int>();
			while (true)
			{
				int num = -1;
				double num2 = double.MaxValue;
				for (int i = 0; i < val3.Count(); i++)
				{
					if (!list.Contains(i) && !val3[i].IsOverlapEvent())
					{
						double num3 = val3[i][0];
						if (num3 < num2)
						{
							num = i;
							num2 = num3;
						}
					}
				}
				if (num < 0)
				{
					break;
				}
				list.Add(num);
			}
			if (list.Count == 1)
			{
				int num4 = list[0];
				OnNurbsCurve obj = val.Duplicate();
				OnNurbsCurve val4 = val2.Duplicate();
				((OnCurve)obj).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), val3[num4][0]));
				((OnCurve)val4).Trim((IOnInterval)new OnInterval(((OnCurve)val2).Domain().Min(), val3[num4][1]));
				double num5 = 0.0;
				((OnCurve)obj).GetLength(ref num5);
				double num6 = 0.0;
				((OnCurve)val4).GetLength(ref num6);
				double num7 = 0.0;
				((OnCurve)val).GetLength(ref num7);
				double num8 = 0.0;
				((OnCurve)val2).GetLength(ref num8);
				oPoints.SetData(new CSPointData(val3[num4][0].Convert()));
				if (num7 > 0.0)
				{
					oParameters1.SetData(new CSNumberData(num5 / num7));
				}
				if (num8 > 0.0)
				{
					oParameters2.SetData(new CSNumberData(num6 / num8));
				}
			}
			else
			{
				CSDataArray cSDataArray = new CSDataArray();
				CSDataArray cSDataArray2 = new CSDataArray();
				CSDataArray cSDataArray3 = new CSDataArray();
				foreach (int item in list)
				{
					OnNurbsCurve obj2 = val.Duplicate();
					OnNurbsCurve val5 = val2.Duplicate();
					((OnCurve)obj2).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), val3[item][0]));
					((OnCurve)val5).Trim((IOnInterval)new OnInterval(((OnCurve)val2).Domain().Min(), val3[item][1]));
					double num9 = 0.0;
					((OnCurve)obj2).GetLength(ref num9);
					double num10 = 0.0;
					((OnCurve)val5).GetLength(ref num10);
					double num11 = 0.0;
					((OnCurve)val).GetLength(ref num11);
					double num12 = 0.0;
					((OnCurve)val2).GetLength(ref num12);
					cSDataArray.Append(new CSPointData(val3[item][0].Convert()));
					if (num11 < double.Epsilon)
					{
						num11 = 1.0;
					}
					cSDataArray2.Append(new CSNumberData(num9 / num11));
					if (num12 < double.Epsilon)
					{
						num12 = 1.0;
					}
					cSDataArray3.Append(new CSNumberData(num10 / num12));
				}
				oPoints.SetData(cSDataArray);
				oParameters1.SetData(cSDataArray2);
				oParameters2.SetData(cSDataArray3);
			}
		}
		return base.Update();
	}
}
