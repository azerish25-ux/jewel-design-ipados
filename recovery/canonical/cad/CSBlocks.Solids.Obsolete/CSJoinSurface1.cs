using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Solids.Obsolete;

[Obsolete]
[CSBlock("JoinSurface_V1", "{31141403-9292-4819-8B70-B767F836D440}")]
public sealed class CSJoinSurface1 : CSBlock
{
	public readonly CSInputChannel iSurfaces;

	public readonly CSOutputChannel oBrep;

	public CSJoinSurface1(string identifier)
		: base(identifier)
	{
		iSurfaces = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = iSurfaces.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Surfaces");
		}
		if (!(data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Surfaces Array");
		}
		CSData arrayItem = iCSArray.GetArrayItem(0);
		if (!(arrayItem is ICSArray iCSArray2))
		{
			CSDataHelper.GetTransformNoArray(arrayItem, out var value);
			if (value.IsValid)
			{
				List<Brep> list = new List<Brep>();
				for (int i = 0; i < iCSArray.GetArrayCount(); i++)
				{
					CSDataHelper.GetBrep(iCSArray.GetArrayItem(i), out var value2);
					if (value2 != null && value2.IsValid && !value2.IsSolid)
					{
						list.Add(value2);
					}
				}
				Brep geometry = CSUtilRhino.JoinAllBreps_BackwardCompatibility(list.ToArray(), CSApplicationHelper.Application.Tolerance);
				oBrep.SetData(new CSOnGeometryData(geometry, value, applyTransform: false));
			}
			else
			{
				List<Brep> list2 = new List<Brep>();
				for (int j = 0; j < iCSArray.GetArrayCount(); j++)
				{
					CSDataHelper.GetBrep(iCSArray.GetArrayItem(j), out var value3);
					if (value3 != null && value3.IsValid && !value3.IsSolid)
					{
						list2.Add(value3);
					}
				}
				Brep geometry2 = CSUtilRhino.JoinAllBreps_BackwardCompatibility(list2.ToArray(), CSApplicationHelper.Application.Tolerance);
				oBrep.SetData(new CSOnGeometryData(geometry2));
			}
		}
		else
		{
			CSDataArray cSDataArray = new CSDataArray();
			for (int k = 0; k < iCSArray2.GetArrayCount(); k++)
			{
				if (CSApplicationHelper.Application.Document.InterruptUpdate())
				{
					continue;
				}
				CSDataHelper.GetTransformNoArray(iCSArray2.GetArrayItem(k), out var value4);
				if (value4.IsValid)
				{
					List<Brep> list3 = new List<Brep>();
					for (int l = 0; l < iCSArray.GetArrayCount(); l++)
					{
						CSData arrayItem2 = CSDataHelper.GetArrayItem(data, l);
						if (CSDataHelper.GetArrayCount(arrayItem2) == iCSArray2.GetArrayCount())
						{
							CSDataHelper.GetBrep(CSDataHelper.GetArrayItem(arrayItem2, k), out var value5);
							if (value5 != null && value5.IsValid && !value5.IsSolid)
							{
								list3.Add(value5);
							}
						}
					}
					Brep geometry3 = CSUtilRhino.JoinAllBreps_BackwardCompatibility(list3.ToArray(), CSApplicationHelper.Application.Tolerance);
					cSDataArray.Append(new CSOnGeometryData(geometry3, value4, applyTransform: false));
					continue;
				}
				List<Brep> list4 = new List<Brep>();
				for (int m = 0; m < iCSArray.GetArrayCount(); m++)
				{
					CSData arrayItem3 = CSDataHelper.GetArrayItem(data, m);
					if (CSDataHelper.GetArrayCount(arrayItem3) == iCSArray2.GetArrayCount())
					{
						CSDataHelper.GetBrep(CSDataHelper.GetArrayItem(arrayItem3, k), out var value6);
						if (value6 != null && value6.IsValid && !value6.IsSolid)
						{
							list4.Add(value6);
						}
					}
				}
				Brep geometry4 = CSUtilRhino.JoinAllBreps_BackwardCompatibility(list4.ToArray(), CSApplicationHelper.Application.Tolerance);
				cSDataArray.Append(new CSOnGeometryData(geometry4));
			}
			oBrep.SetData(cSDataArray);
		}
		if (!CSApplicationHelper.Application.Document.InterruptUpdate())
		{
			return base.Update();
		}
		return UpdateCancel();
	}
}
