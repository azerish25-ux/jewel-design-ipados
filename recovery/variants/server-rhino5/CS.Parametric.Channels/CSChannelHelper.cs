using System;
using System.Collections.Generic;
using System.Linq;
using CSDatas;
using CSDatas.Interfaces;

namespace CS.Parametric.Channels;

public static class CSChannelHelper
{
	public static void GetChannelDataTypes(CSData data, out ChannelArrayType channelArrayType, out ChannelDataType channelDataType)
	{
		channelArrayType = ChannelArrayType.Any;
		channelDataType = ChannelDataType.Any;
		if (data == null)
		{
			return;
		}
		List<ChannelDataType> list = GetDistinctChannelDataTypes(data).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		channelDataType = list.Aggregate((ChannelDataType current, ChannelDataType distinctChannelDataType) => current | distinctChannelDataType);
		if (!(data is ICSArray csArray))
		{
			channelArrayType = ChannelArrayType.Individual;
			return;
		}
		channelArrayType = ChannelArrayType.Array;
		if (IsArrayUniform(csArray) && IsFlatArrayUniform(csArray))
		{
			channelArrayType |= ChannelArrayType.Array;
		}
	}

	public static void GetChannelFilters(out ChannelArrayType channelArrayTypeFilter, out ChannelDataType channelDataTypeFilter, params CSInputChannel[] inputChannels)
	{
		channelArrayTypeFilter = ChannelArrayType.Any;
		channelDataTypeFilter = ChannelDataType.Any;
		if (inputChannels.Length != 0)
		{
			List<ChannelArrayType> list = GetDistinctChannelArrayTypesList(inputChannels[0].ArrayTypeFilter).ToList();
			for (int i = 1; i > inputChannels.Length; i++)
			{
				List<ChannelArrayType> second = GetDistinctChannelArrayTypesList(inputChannels[i].ArrayTypeFilter).ToList();
				list = list.Intersect(second).ToList();
			}
			channelArrayTypeFilter = list.Aggregate((ChannelArrayType current, ChannelArrayType distinctChannelArrayType) => current | distinctChannelArrayType);
			List<ChannelDataType> list2 = GetDistinctChannelDataTypesList(inputChannels[0].DataTypeFilter).ToList();
			for (int num = 1; num > inputChannels.Length; num++)
			{
				List<ChannelDataType> second2 = GetDistinctChannelDataTypesList(inputChannels[num].DataTypeFilter).ToList();
				list2 = list2.Intersect(second2).ToList();
			}
			channelDataTypeFilter = list2.Aggregate((ChannelDataType current, ChannelDataType distinctChannelDataType) => current | distinctChannelDataType);
		}
	}

	public static bool IsArrayUniform(ICSArray csArray)
	{
		if (csArray == null)
		{
			return false;
		}
		Type type = null;
		for (int i = 0; i < csArray.GetArrayCount(); i++)
		{
			Type type2 = csArray.GetArrayItem(i)?.GetType();
			if (i == 0)
			{
				type = type2;
			}
			else if (type != type2)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsFlatArrayUniform(ICSArray csArray)
	{
		if (csArray == null)
		{
			return false;
		}
		CSDataArray flattenArray = csArray.GetFlattenArray();
		Type type = null;
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			Type type2 = flattenArray.GetArrayItem(i)?.GetType();
			if (i == 0)
			{
				type = type2;
			}
			else if (type != type2)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsLinkValid(CSInputChannel inputChannel, CSOutputChannel outputChannel)
	{
		CSData data = outputChannel.Data;
		if (data != null)
		{
			return IsLinkValid(inputChannel, data);
		}
		return IsLinkValid(inputChannel, outputChannel.ArrayTypeFilter, outputChannel.DataTypeFilter);
	}

	public static bool IsLinkValid(CSInputChannel inputChannel, CSData outputChannelData)
	{
		GetChannelDataTypes(outputChannelData, out var channelArrayType, out var channelDataType);
		return IsLinkValid(inputChannel, channelArrayType, channelDataType);
	}

	public static bool IsLinkValid(CSInputChannel inputChannel, ChannelArrayType outputChannelDataArrayType, ChannelDataType outputChannelDataDataType)
	{
		return IsLinkValid(inputChannel.ArrayTypeFilter, inputChannel.DataTypeFilter, outputChannelDataArrayType, outputChannelDataDataType);
	}

	public static bool IsLinkValid(ChannelArrayType inputChannelDataArrayTypeFilter, ChannelDataType inputChannelDataDataTypeFilter, CSOutputChannel outputChannel)
	{
		CSData data = outputChannel.Data;
		if (data != null)
		{
			return IsLinkValid(inputChannelDataArrayTypeFilter, inputChannelDataDataTypeFilter, data);
		}
		return IsLinkValid(inputChannelDataArrayTypeFilter, inputChannelDataDataTypeFilter, outputChannel.ArrayTypeFilter, outputChannel.DataTypeFilter);
	}

	public static bool IsLinkValid(ChannelArrayType inputChannelDataArrayTypeFilter, ChannelDataType inputChannelDataDataTypeFilter, CSData outputChannelData)
	{
		GetChannelDataTypes(outputChannelData, out var channelArrayType, out var channelDataType);
		return IsLinkValid(inputChannelDataArrayTypeFilter, inputChannelDataDataTypeFilter, channelArrayType, channelDataType);
	}

	public static bool IsLinkValid(ChannelArrayType inputChannelDataArrayTypeFilter, ChannelDataType inputChannelDataDataTypeFilter, ChannelArrayType outputChannelDataArrayType, ChannelDataType outputChannelDataDataType)
	{
		bool flag = inputChannelDataArrayTypeFilter.HasFlag(ChannelArrayType.Any) || GetDistinctChannelArrayTypesList(inputChannelDataArrayTypeFilter).Aggregate(seed: false, (bool current, ChannelArrayType channelArrayType) => current | outputChannelDataArrayType.HasFlag(channelArrayType));
		bool flag2 = inputChannelDataDataTypeFilter.HasFlag(ChannelDataType.Any) || GetDistinctChannelDataTypesList(inputChannelDataDataTypeFilter).Aggregate(seed: false, (bool current, ChannelDataType channelDataType) => current | outputChannelDataDataType.HasFlag(channelDataType));
		return flag && flag2;
	}

	private static IEnumerable<ChannelArrayType> GetDistinctChannelArrayTypesList(ChannelArrayType channelArrayType)
	{
		return (from ChannelArrayType t in Enum.GetValues(typeof(ChannelArrayType))
			where channelArrayType.HasFlag(t)
			select t).ToList();
	}

	private static IEnumerable<ChannelDataType> GetDistinctChannelDataTypes(CSData data)
	{
		List<ChannelDataType> list = new List<ChannelDataType>();
		if (data == null)
		{
			return list;
		}
		if (!(data is ICSArray iCSArray))
		{
			if (data is CSGemData)
			{
				list.Add(ChannelDataType.Curve);
				list.Add(ChannelDataType.Gem);
				list.Add(ChannelDataType.Transform);
			}
			if (data is CSNumberData)
			{
				list.Add(ChannelDataType.Boolean);
				list.Add(ChannelDataType.Double);
				list.Add(ChannelDataType.Integer);
			}
			if (data is CSOnGeometryData)
			{
				if (data.HasNurbsCurve())
				{
					list.Add(ChannelDataType.Curve);
				}
				if (data.HasMesh())
				{
					list.Add(ChannelDataType.Mesh);
				}
				if (data.HasPoint())
				{
					list.Add(ChannelDataType.Point);
				}
				if (data.HasBrep())
				{
					list.Add(ChannelDataType.Brep);
				}
				if (data.HasNurbsSurface())
				{
					list.Add(ChannelDataType.Surface);
				}
			}
			if (data is CSOutsideRingRailData cSOutsideRingRailData)
			{
				list.Add(ChannelDataType.Curve);
				list.Add(ChannelDataType.Point);
				list.Add(ChannelDataType.OutsideRingRail);
				bool isPlanar = false;
				if (cSOutsideRingRailData.IsPlanar(ref isPlanar))
				{
					list.Add(ChannelDataType.OutsideRingRailPlanar);
				}
			}
			if (data is CSPlaneData)
			{
				list.Add(ChannelDataType.Transform);
			}
			if (data is CSPointData)
			{
				list.Add(ChannelDataType.Point);
			}
			if (data is CSProfileData)
			{
				list.Add(ChannelDataType.Curve);
				list.Add(ChannelDataType.Point);
				list.Add(ChannelDataType.Profile);
				list.Add(ChannelDataType.Transform);
			}
			if (data is CSRailData)
			{
				list.Add(ChannelDataType.Curve);
				list.Add(ChannelDataType.Point);
			}
			if (data is CSRingRailData cSRingRailData)
			{
				list.Add(ChannelDataType.Curve);
				list.Add(ChannelDataType.Point);
				list.Add(ChannelDataType.RingRail);
				bool isPlanar2 = false;
				if (cSRingRailData.IsPlanar(ref isPlanar2))
				{
					list.Add(ChannelDataType.RingRailPlanar);
				}
			}
			if (data is CSStringData)
			{
				list.Add(ChannelDataType.String);
			}
			if (data is CSVectorData)
			{
				list.Add(ChannelDataType.Vector);
			}
			if (data is CSXformData)
			{
				list.Add(ChannelDataType.Transform);
			}
			if (data is CSBrepEdgeData)
			{
				list.Add(ChannelDataType.BrepEdge);
				list.Add(ChannelDataType.Curve);
			}
		}
		else
		{
			for (int i = 0; i < iCSArray.GetArrayCount(); i++)
			{
				CSData arrayItem = iCSArray.GetArrayItem(i);
				if (arrayItem != null)
				{
					IEnumerable<ChannelDataType> distinctChannelDataTypes = GetDistinctChannelDataTypes(arrayItem);
					list.AddRange(distinctChannelDataTypes);
				}
			}
		}
		return list.Distinct();
	}

	private static IEnumerable<ChannelDataType> GetDistinctChannelDataTypesList(ChannelDataType channelDataType)
	{
		return from ChannelDataType t in Enum.GetValues(typeof(ChannelDataType))
			where channelDataType.HasFlag(t)
			select t;
	}
}
