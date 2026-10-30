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
		Mesh mesh = new Mesh();
		foreach (CSData data in datas)
		{
			if (data == null)
			{
				continue;
			}
			Brep brep = data.GetBrep();
			Mesh mesh2;
			if (brep != null)
			{
				if (brep.SolidOrientation != BrepSolidOrientation.Inward)
				{
					brep.Flip();
				}
				mesh2 = new CSOnGeometryData(brep).GetMesh();
			}
			else
			{
				mesh2 = data.GetMesh();
			}
			if (mesh2 != null)
			{
				mesh2.UnifyNormals();
				mesh.Append(mesh2);
			}
		}
		mesh.Compact();
		return mesh;
	}

	public static Mesh ConvertToVolumeableMesh(IEnumerable<CSData> datas)
	{
		Mesh mesh = new Mesh();
		foreach (CSData data in datas)
		{
			if (data == null)
			{
				continue;
			}
			Brep brep = data.GetBrep();
			Mesh mesh2;
			if (brep != null)
			{
				if (brep.SolidOrientation != BrepSolidOrientation.Inward)
				{
					brep.Flip();
				}
				CSOnGeometryData cSOnGeometryData = new CSOnGeometryData(brep);
				MeshingParameters mp = new MeshingParameters
				{
					Tolerance = 0.1
				};
				mesh2 = cSOnGeometryData.MeshObject(mp);
			}
			else
			{
				mesh2 = data.GetMesh();
			}
			if (mesh2 != null)
			{
				mesh2.UnifyNormals();
				mesh.Append(mesh2);
			}
		}
		mesh.Compact();
		return mesh;
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
		Brep brep = (data as ICSBrep)?.GetBrep();
		if (brep == null)
		{
			return false;
		}
		value = brep;
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
		Brep brep = (flattenArray.GetArrayItem(0) as ICSBrep)?.GetBrep();
		if (brep == null)
		{
			return false;
		}
		value = brep;
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
		BrepEdge brepEdge = (flattenArray.GetArrayItem(0) as ICSBrepEdge)?.GetBrepEdge();
		if (brepEdge == null)
		{
			return false;
		}
		value = brepEdge;
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
		Curve curve = (flattenArray.GetArrayItem(0) as ICSCurve)?.GetCurve();
		if (curve == null)
		{
			return false;
		}
		value = curve;
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
		Mesh mesh = (flattenArray.GetArrayItem(0) as ICSMesh)?.GetMesh();
		if (mesh == null)
		{
			return false;
		}
		value = mesh;
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
		NurbsCurve nurbsCurve = (flattenArray.GetArrayItem(0) as ICSCurve)?.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return false;
		}
		value = nurbsCurve;
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
		NurbsCurve nurbsCurve = (data as ICSCurve)?.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return false;
		}
		value = nurbsCurve;
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
		NurbsSurface nurbsSurface = (flattenArray.GetArrayItem(0) as ICSSurface)?.GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return false;
		}
		value = nurbsSurface;
		return true;
	}

	public static bool GetPoint(CSInputChannel inputChannel, out Point3d value)
	{
		return GetPoint(inputChannel.Data, out value);
	}

	public static bool GetPoint(CSData data, ref On3dPoint value)
	{
		if (!GetPoint(data, out var value2))
		{
			return false;
		}
		value = value2.Convert();
		return true;
	}

	public static bool GetPoint(CSData data, out Point3d value)
	{
		value = Point3d.Unset;
		if (data is ICSPoint iCSPoint)
		{
			return iCSPoint.GetPoint(out value);
		}
		return false;
	}

	public static bool GetTransform(CSData data, out Transform value)
	{
		value = Rhino.Geometry.Transform.Unset;
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
		value = Rhino.Geometry.Transform.Unset;
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
		if (!GetVector(data, out var value2))
		{
			return false;
		}
		value = value2.Convert();
		return true;
	}

	public static bool GetVector(CSData data, out Vector3d value)
	{
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
		if (!transform.IsValid)
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
