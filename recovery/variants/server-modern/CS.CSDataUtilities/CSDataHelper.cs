using System;
using System.Collections.Generic;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.CSDataUtilities;

public static class CSDataHelper
{
	public static Mesh ConvertToMesh(IEnumerable<CSData> datas)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		Mesh val = new Mesh();
		foreach (CSData data in datas)
		{
			if (data == null)
			{
				continue;
			}
			Brep brep = data.GetBrep();
			Mesh mesh;
			if (brep != null)
			{
				if ((int)brep.SolidOrientation != -1)
				{
					brep.Flip();
				}
				mesh = new CSOnGeometryData((GeometryBase)(object)brep).GetMesh();
			}
			else
			{
				mesh = data.GetMesh();
			}
			if (mesh != null)
			{
				mesh.UnifyNormals();
				val.Append(mesh);
			}
		}
		val.Compact();
		return val;
	}

	public static Mesh ConvertToVolumeableMesh(IEnumerable<CSData> datas)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		Mesh val = new Mesh();
		foreach (CSData data in datas)
		{
			if (data == null)
			{
				continue;
			}
			Brep brep = data.GetBrep();
			Mesh val2;
			if (brep != null)
			{
				if ((int)brep.SolidOrientation != -1)
				{
					brep.Flip();
				}
				CSOnGeometryData cSOnGeometryData = new CSOnGeometryData((GeometryBase)(object)brep);
				MeshingParameters mp = new MeshingParameters
				{
					Tolerance = 0.1
				};
				val2 = cSOnGeometryData.MeshObject(mp);
			}
			else
			{
				val2 = data.GetMesh();
			}
			if (val2 != null)
			{
				val2.UnifyNormals();
				val.Append(val2);
			}
		}
		val.Compact();
		return val;
	}

	public static void Flip(CSData data)
	{
		(data as ICSFlippable)?.Flip();
	}

	[Obsolete]
	public static bool GetBrep_NoArraySupport(CSData data, out Brep value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		if (data is ICSArray)
		{
			return false;
		}
		Brep val = (data as ICSBrep)?.GetBrep();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	public static int GetArrayCount(CSInputChannel inputChannel)
	{
		return GetArrayCount(inputChannel.Data);
	}

	public static int GetArrayCount(CSData data)
	{
		if (data == null)
		{
			return 0;
		}
		return (data as ICSArray)?.GetArrayCount() ?? 0;
	}

	public static CSData GetArrayItem(CSData data, int index)
	{
		if (data == null)
		{
			return new CSData();
		}
		if (!(data is ICSArray iCSArray))
		{
			return data;
		}
		if (index < 0 || index >= iCSArray.GetArrayCount())
		{
			return new CSData();
		}
		return iCSArray.GetArrayItem(index);
	}

	public static bool GetBrep(CSInputChannel inputChannel, out Brep value)
	{
		return GetBrep(inputChannel.Data, out value);
	}

	public static bool GetBrep(CSData data, out Brep value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		CSDataArray flattenArray = GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		Brep val = (flattenArray.GetArrayItem(0) as ICSBrep)?.GetBrep();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	public static bool GetBrepEdge(CSInputChannel inputChannel, out BrepEdge value)
	{
		return GetBrepEdge(inputChannel.Data, out value);
	}

	public static bool GetBrepEdge(CSData data, out BrepEdge value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		CSDataArray flattenArray = GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		BrepEdge val = (flattenArray.GetArrayItem(0) as ICSBrepEdge)?.GetBrepEdge();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	public static Type GetCommonType(CSData data, bool doIgnoreNestedArrays)
	{
		if (data == null)
		{
			return null;
		}
		ICSArray iCSArray = data as ICSArray;
		if (iCSArray == null)
		{
			return data.GetType();
		}
		if (doIgnoreNestedArrays)
		{
			iCSArray = iCSArray.GetFlattenArray();
			if (iCSArray.GetArrayCount() <= 0)
			{
				return null;
			}
		}
		CSData arrayItem = iCSArray.GetArrayItem(0);
		if (arrayItem == null)
		{
			return null;
		}
		Type commonType = GetCommonType(arrayItem, doIgnoreNestedArrays);
		if (commonType == null)
		{
			return null;
		}
		for (int i = 1; i < iCSArray.GetArrayCount(); i++)
		{
			CSData arrayItem2 = iCSArray.GetArrayItem(i);
			if (arrayItem2 == null)
			{
				return null;
			}
			Type commonType2 = GetCommonType(arrayItem2, doIgnoreNestedArrays);
			if (commonType2 == null)
			{
				return null;
			}
			if (commonType != commonType2)
			{
				return null;
			}
		}
		return commonType;
	}

	public static bool GetCurve(CSInputChannel inputChannel, out Curve value)
	{
		return GetCurve(inputChannel.Data, out value);
	}

	public static bool GetCurve(CSData data, out Curve value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		CSDataArray flattenArray = GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		Curve val = (flattenArray.GetArrayItem(0) as ICSCurve)?.GetCurve();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	public static bool GetDominantChannelDataType(ChannelDataType channelFilter, out ChannelDataType channelDataType)
	{
		if (TestType(channelFilter, ChannelDataType.Gem, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Profile, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.BrepEdge, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.RingRail, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.RingRailPlanar, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.OutsideRingRail, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.OutsideRingRailPlanar, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Mesh, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Brep, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Surface, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Curve, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Plane, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Vector, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Point, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Transform, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.String, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Double, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Integer, out channelDataType))
		{
			return true;
		}
		if (TestType(channelFilter, ChannelDataType.Boolean, out channelDataType))
		{
			return true;
		}
		return false;
	}

	public static bool GetDominantChannelDataType(CSData data, out ChannelDataType channelDataType)
	{
		channelDataType = ChannelDataType.Unknown;
		if (data == null)
		{
			return false;
		}
		if (!(data is ICSArray iCSArray))
		{
			if (data is CSGemData)
			{
				channelDataType = ChannelDataType.Gem;
				return true;
			}
			if (data is CSOutsideRingRailData cSOutsideRingRailData)
			{
				bool isPlanar = false;
				if (!cSOutsideRingRailData.IsPlanar(ref isPlanar))
				{
					channelDataType = ChannelDataType.OutsideRingRail;
					return true;
				}
				channelDataType = (isPlanar ? ChannelDataType.OutsideRingRailPlanar : ChannelDataType.OutsideRingRail);
				return true;
			}
			if (data is CSRingRailData cSRingRailData)
			{
				bool isPlanar2 = false;
				if (!cSRingRailData.IsPlanar(ref isPlanar2))
				{
					channelDataType = ChannelDataType.RingRail;
					return true;
				}
				channelDataType = (isPlanar2 ? ChannelDataType.RingRailPlanar : ChannelDataType.RingRail);
				return true;
			}
			if (data is CSBrepEdgeData)
			{
				channelDataType = ChannelDataType.BrepEdge;
				return true;
			}
			if (data is CSProfileData)
			{
				channelDataType = ChannelDataType.Profile;
				return true;
			}
			if (data is CSPlaneData)
			{
				channelDataType = ChannelDataType.Plane;
				return true;
			}
			if (data is CSXformData)
			{
				channelDataType = ChannelDataType.Transform;
				return true;
			}
			if (data is CSOnGeometryData)
			{
				if (data.HasNurbsCurve())
				{
					channelDataType = ChannelDataType.Curve;
					return true;
				}
				if (data.HasMesh())
				{
					channelDataType = ChannelDataType.Mesh;
					return true;
				}
				if (data.HasPoint())
				{
					channelDataType = ChannelDataType.Point;
					return true;
				}
				if (data.HasBrep())
				{
					channelDataType = ChannelDataType.Brep;
					return true;
				}
				if (data.HasNurbsSurface())
				{
					channelDataType = ChannelDataType.Surface;
					return true;
				}
				return false;
			}
			if (data is CSNumberData)
			{
				channelDataType = ChannelDataType.Double;
				return true;
			}
			if (data is CSPointData)
			{
				channelDataType = ChannelDataType.Point;
				return true;
			}
			if (data is CSVectorData)
			{
				channelDataType = ChannelDataType.Vector;
				return true;
			}
			if (data is CSStringData)
			{
				channelDataType = ChannelDataType.String;
				return true;
			}
			return false;
		}
		CSDataArray flattenArray = iCSArray.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		if (!GetDominantChannelDataType(flattenArray.GetArrayItem(0), out var channelDataType2))
		{
			return false;
		}
		channelDataType = channelDataType2;
		for (int i = 1; i < flattenArray.GetArrayCount(); i++)
		{
			if (!GetDominantChannelDataType(flattenArray.GetArrayItem(i), out var channelDataType3))
			{
				return false;
			}
			if (channelDataType2 != channelDataType3)
			{
				channelDataType = ChannelDataType.Any;
			}
		}
		return true;
	}

	public static CSDataArray GetFlattenArray(CSData data)
	{
		if (data == null)
		{
			return new CSDataArray();
		}
		if (data is ICSArray iCSArray)
		{
			return iCSArray.GetFlattenArray();
		}
		return new CSDataArray(data);
	}

	public static bool GetMesh(CSInputChannel inputChannel, out Mesh value)
	{
		return GetMesh(inputChannel.Data, out value);
	}

	public static bool GetMesh(CSData data, out Mesh value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		CSDataArray flattenArray = GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		Mesh val = (flattenArray.GetArrayItem(0) as ICSMesh)?.GetMesh();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	public static bool GetNurbsCurve(CSData data, out NurbsCurve value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		CSDataArray flattenArray = GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		NurbsCurve val = (flattenArray.GetArrayItem(0) as ICSCurve)?.GetNurbsCurve();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	[Obsolete]
	public static bool GetNurbsCurve_NoArraySupport(CSData data, out NurbsCurve value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		if (data is ICSArray)
		{
			return false;
		}
		NurbsCurve val = (data as ICSCurve)?.GetNurbsCurve();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	public static bool GetNurbsSurface(CSInputChannel inputChannel, out NurbsSurface value)
	{
		return GetNurbsSurface(inputChannel.Data, out value);
	}

	public static bool GetNurbsSurface(CSData data, out NurbsSurface value)
	{
		value = null;
		if (data == null)
		{
			return false;
		}
		CSDataArray flattenArray = GetFlattenArray(data);
		if (flattenArray.GetArrayCount() <= 0)
		{
			return false;
		}
		NurbsSurface val = (flattenArray.GetArrayItem(0) as ICSSurface)?.GetNurbsSurface();
		if (val == null)
		{
			return false;
		}
		value = val;
		return true;
	}

	public static bool GetPoint(CSInputChannel inputChannel, out Point3d value)
	{
		return GetPoint(inputChannel.Data, out value);
	}

	public static bool GetPoint(CSData data, ref On3dPoint value)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (!GetPoint(data, out var value2))
		{
			return false;
		}
		value = value2.Convert();
		return true;
	}

	public static bool GetPoint(CSData data, out Point3d value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		value = Point3d.Unset;
		if (data is ICSPoint iCSPoint)
		{
			return iCSPoint.GetPoint(out value);
		}
		return false;
	}

	public static bool GetTransform(CSData data, out Transform value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		value = Transform.Unset;
		if (data == null)
		{
			return false;
		}
		if (data is ICSTransform iCSTransform)
		{
			return iCSTransform.GetTransform(out value);
		}
		return false;
	}

	public static bool GetTransformNoArray(CSData data, out Transform value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		value = Transform.Unset;
		if (data == null)
		{
			return false;
		}
		if (data is CSDataArray)
		{
			return false;
		}
		if (data is ICSTransform iCSTransform)
		{
			return iCSTransform.GetTransform(out value);
		}
		return false;
	}

	public static bool GetVector(CSData data, ref On3dVector value)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (!GetVector(data, out var value2))
		{
			return false;
		}
		value = value2.Convert();
		return true;
	}

	public static bool GetVector(CSData data, out Vector3d value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		value = Vector3d.Unset;
		if (data is ICSVector iCSVector)
		{
			return iCSVector.GetVector(out value);
		}
		return false;
	}

	[Obsolete]
	public static bool SetSettingType(CSData data, GemSettingType whichGemSettingType)
	{
		bool flag = true;
		if (data is ICSArray iCSArray)
		{
			for (int i = 0; i < iCSArray.GetArrayCount(); i++)
			{
				if (iCSArray.GetArrayItem(i) != null)
				{
					flag &= SetSettingType(iCSArray.GetArrayItem(i), whichGemSettingType);
				}
			}
		}
		else if (data is CSGemData { SettingType: GemSettingType.XX } cSGemData)
		{
			cSGemData.SettingType = whichGemSettingType;
		}
		return flag;
	}

	public static bool Transform(CSData data, Transform transform)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!((Transform)(ref transform)).IsValid)
		{
			return false;
		}
		if (!(data is ICSTransformable iCSTransformable))
		{
			return false;
		}
		iCSTransformable.ApplyTransform(transform);
		return true;
	}

	private static bool TestType(ChannelDataType channelFilter, ChannelDataType toTest, out ChannelDataType channelDataType)
	{
		channelDataType = ChannelDataType.Any;
		if ((channelFilter & toTest) == 0)
		{
			return false;
		}
		channelDataType = toTest;
		return true;
	}
}
