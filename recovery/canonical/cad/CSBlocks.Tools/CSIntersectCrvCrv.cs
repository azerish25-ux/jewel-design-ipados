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
		OnNurbsCurve onNurbsCurve = iCurve1.GetNurbsCurve();
		if (onNurbsCurve == null)
		{
			CSDataArray flattenArray = iCurve1.GetFlattenArray();
			if (flattenArray.GetArrayCount() > 0)
			{
				onNurbsCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
			}
		}
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Missing Curve1");
		}
		OnNurbsCurve onNurbsCurve2 = iCurve2.GetNurbsCurve();
		if (onNurbsCurve2 == null)
		{
			CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
			if (flattenArray2.GetArrayCount() > 0)
			{
				onNurbsCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
			}
		}
		if (onNurbsCurve2 == null)
		{
			return UpdateMissing("Missing Curve2");
		}
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onNurbsCurve.IntersectCurve(onNurbsCurve2, ref x, 0.01);
		if (x.Count() > 0)
		{
			List<int> list = new List<int>();
			while (true)
			{
				int num = -1;
				double num2 = double.MaxValue;
				for (int i = 0; i < x.Count(); i++)
				{
					if (!list.Contains(i) && !x[i].IsOverlapEvent())
					{
						double num3 = x[i].get_m_a(0);
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
				int index = list[0];
				OnNurbsCurve onNurbsCurve3 = onNurbsCurve.Duplicate();
				OnNurbsCurve onNurbsCurve4 = onNurbsCurve2.Duplicate();
				onNurbsCurve3.Trim(new OnInterval(onNurbsCurve.Domain().Min(), x[index].get_m_a(0)));
				onNurbsCurve4.Trim(new OnInterval(onNurbsCurve2.Domain().Min(), x[index].get_m_a(1)));
				double length = 0.0;
				onNurbsCurve3.GetLength(ref length);
				double length2 = 0.0;
				onNurbsCurve4.GetLength(ref length2);
				double length3 = 0.0;
				onNurbsCurve.GetLength(ref length3);
				double length4 = 0.0;
				onNurbsCurve2.GetLength(ref length4);
				oPoints.SetData(new CSPointData(x[index].get_m_pointA(0).Convert()));
				if (length3 > 0.0)
				{
					oParameters1.SetData(new CSNumberData(length / length3));
				}
				if (length4 > 0.0)
				{
					oParameters2.SetData(new CSNumberData(length2 / length4));
				}
			}
			else
			{
				CSDataArray cSDataArray = new CSDataArray();
				CSDataArray cSDataArray2 = new CSDataArray();
				CSDataArray cSDataArray3 = new CSDataArray();
				foreach (int item in list)
				{
					OnNurbsCurve onNurbsCurve5 = onNurbsCurve.Duplicate();
					OnNurbsCurve onNurbsCurve6 = onNurbsCurve2.Duplicate();
					onNurbsCurve5.Trim(new OnInterval(onNurbsCurve.Domain().Min(), x[item].get_m_a(0)));
					onNurbsCurve6.Trim(new OnInterval(onNurbsCurve2.Domain().Min(), x[item].get_m_a(1)));
					double length5 = 0.0;
					onNurbsCurve5.GetLength(ref length5);
					double length6 = 0.0;
					onNurbsCurve6.GetLength(ref length6);
					double length7 = 0.0;
					onNurbsCurve.GetLength(ref length7);
					double length8 = 0.0;
					onNurbsCurve2.GetLength(ref length8);
					cSDataArray.Append(new CSPointData(x[item].get_m_pointA(0).Convert()));
					if (length7 < double.Epsilon)
					{
						length7 = 1.0;
					}
					cSDataArray2.Append(new CSNumberData(length5 / length7));
					if (length8 < double.Epsilon)
					{
						length8 = 1.0;
					}
					cSDataArray3.Append(new CSNumberData(length6 / length8));
				}
				oPoints.SetData(cSDataArray);
				oParameters1.SetData(cSDataArray2);
				oParameters2.SetData(cSDataArray3);
			}
		}
		return base.Update();
	}
}
