using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino;
using Rhino.DocObjects;
using Rhino.FileIO;
using Rhino.Geometry;

namespace CSBlockDataUtils;

public static class CSUtilGem
{
	public enum EGemPlacementMethod
	{
		Girdle,
		Culet,
		Table,
		GirdleScaled,
		CuletScaled,
		TableScaled
	}

	public static int GetGemBlockIndex(string type, string shape)
	{
		int num = shape.IndexOf(" Rotated", StringComparison.Ordinal);
		if (num != -1)
		{
			shape = shape.Substring(0, num);
		}
		string instanceDefinitionName = $"{type}_{shape}";
		InstanceDefinition instanceDefinition = RhinoDoc.ActiveDoc.InstanceDefinitions.Find(instanceDefinitionName, ignoreDeletedInstanceDefinitions: true);
		if (instanceDefinition == null)
		{
			string path = Path.Combine(CSUtil.CSLibraryPath, $"ICGems\\{type}\\{shape}.3dm");
			FileReadOptions options = new FileReadOptions
			{
				ImportMode = true,
				ImportReferenceMode = false,
				ScaleGeometry = false
			};
			RhinoDoc.ReadFile(path, options);
			instanceDefinition = RhinoDoc.ActiveDoc.InstanceDefinitions.Find(instanceDefinitionName, ignoreDeletedInstanceDefinitions: true);
		}
		return instanceDefinition?.Index ?? (-1);
	}

	public static bool GetGemScale(CSGemData gemData, double requestedSizeX, double requestedSizeY, out OnXform xBestScale)
	{
		xBestScale = null;
		if (gemData == null)
		{
			return false;
		}
		if (Math.Abs(requestedSizeX - -1.23432101234321E+308) < CSUtil.Tolerance && Math.Abs(requestedSizeY - -1.23432101234321E+308) < CSUtil.Tolerance)
		{
			return false;
		}
		double num = 0.0;
		double num2 = 0.0;
		if (requestedSizeX > 0.001 && gemData.GemXSize > 0.001)
		{
			num = requestedSizeX / gemData.GemXSize;
		}
		if (requestedSizeY > 0.001 && gemData.GemYSize > 0.001)
		{
			num2 = requestedSizeY / gemData.GemYSize;
		}
		if (num == 0.0 && num2 == 0.0)
		{
			num = (num2 = 1.0);
		}
		if (num == 0.0)
		{
			num = num2;
		}
		if (num2 == 0.0)
		{
			num2 = num;
		}
		double z_scale_factor = (num + num2) / 2.0;
		OnPlane plane = new OnPlane(gemData.Center.Convert(), gemData.East.Convert(), gemData.North.Convert());
		xBestScale = new OnXform();
		xBestScale.Scale(plane, num, num2, z_scale_factor);
		return true;
	}

	public static bool GetGemScale(CSGemData gemData, double requestedSizeX, double requestedSizeY, out Transform xBestScale)
	{
		xBestScale = Transform.Unset;
		if (gemData == null)
		{
			return false;
		}
		if (double.IsNaN(requestedSizeX) && double.IsNaN(requestedSizeY))
		{
			return false;
		}
		double num = 0.0;
		double num2 = 0.0;
		if (requestedSizeX > 0.001 && gemData.GemXSize > 0.001)
		{
			num = requestedSizeX / gemData.GemXSize;
		}
		if (requestedSizeY > 0.001 && gemData.GemYSize > 0.001)
		{
			num2 = requestedSizeY / gemData.GemYSize;
		}
		if (num == 0.0 && num2 == 0.0)
		{
			num = (num2 = 1.0);
		}
		if (num == 0.0)
		{
			num = num2;
		}
		if (num2 == 0.0)
		{
			num2 = num;
		}
		double zScaleFactor = (num + num2) / 2.0;
		Plane plane = new Plane(gemData.Center, gemData.East, gemData.North);
		xBestScale = Transform.Scale(plane, num, num2, zScaleFactor);
		return true;
	}

	public static bool GetGemScaleFromTable(CSGemData gemData, double requestedSizeX, double requestedSizeY, bool useLargestPossible, bool useIterationOnly, out Transform xBestScale)
	{
		OnXform xBestScale2;
		bool gemScaleFromTable = GetGemScaleFromTable(gemData, requestedSizeX, requestedSizeY, useLargestPossible, useIterationOnly, out xBestScale2);
		xBestScale = CSConvert.XformCopyToTransform(xBestScale2);
		return gemScaleFromTable;
	}

	public static bool GetGemScaleFromTable(CSGemData gemData, double requestedSizeX, double requestedSizeY, bool useLargestPossible, bool useIterationOnly, out OnXform xBestScale)
	{
		xBestScale = null;
		if (gemData == null)
		{
			return false;
		}
		if (Math.Abs(requestedSizeX - -1.23432101234321E+308) < CSUtil.Tolerance && Math.Abs(requestedSizeY - -1.23432101234321E+308) < CSUtil.Tolerance)
		{
			return false;
		}
		if (!GetGemSizeFromTable(gemData, requestedSizeX, requestedSizeY, useLargestPossible, useIterationOnly, out var bestSize))
		{
			return false;
		}
		double num = 0.0;
		double num2 = 0.0;
		if (bestSize.X > 0.001 && gemData.GemXSize > 0.001)
		{
			num = bestSize.X / gemData.GemXSize;
		}
		if (bestSize.Y > 0.001 && gemData.GemYSize > 0.001)
		{
			num2 = bestSize.Y / gemData.GemYSize;
		}
		if (num == 0.0 && num2 == 0.0)
		{
			num = (num2 = 1.0);
		}
		if (num == 0.0)
		{
			num = num2;
		}
		if (num2 == 0.0)
		{
			num2 = num;
		}
		double z_scale_factor = (num + num2) / 2.0;
		OnPlane plane = new OnPlane(gemData.Center.Convert(), gemData.East.Convert(), gemData.North.Convert());
		xBestScale = new OnXform();
		xBestScale.Scale(plane, num, num2, z_scale_factor);
		return true;
	}

	public static bool GetGemScaleFromTableWithMaxZ(CSGemData gemData, double requestedSizeX, double requestedSizeY, double maxZ, bool useLargestPossible, bool useIterationOnly, out Transform xBestScale)
	{
		xBestScale = Transform.Unset;
		if (gemData == null)
		{
			return false;
		}
		if (Math.Abs(requestedSizeX - -1.23432101234321E+308) < CSUtil.Tolerance && Math.Abs(requestedSizeY - -1.23432101234321E+308) < CSUtil.Tolerance)
		{
			return false;
		}
		if (!GetGemSizeFromTableMaxZ(gemData, requestedSizeX, requestedSizeY, maxZ, useLargestPossible, useIterationOnly, out var bestSize))
		{
			return false;
		}
		double num = 0.0;
		double num2 = 0.0;
		if (bestSize.X > 0.001 && gemData.GemXSize > 0.001)
		{
			num = bestSize.X / gemData.GemXSize;
		}
		if (bestSize.Y > 0.001 && gemData.GemYSize > 0.001)
		{
			num2 = bestSize.Y / gemData.GemYSize;
		}
		if (num == 0.0 && num2 == 0.0)
		{
			num = (num2 = 1.0);
		}
		if (num == 0.0)
		{
			num = num2;
		}
		if (num2 == 0.0)
		{
			num2 = num;
		}
		double zScaleFactor = (num + num2) / 2.0;
		Point3d center = gemData.Center;
		Vector3d east = gemData.East;
		Vector3d north = gemData.North;
		Plane plane = new Plane(center, east, north);
		xBestScale = Transform.Scale(plane, num, num2, zScaleFactor);
		return true;
	}

	public static bool GetGemScaleFromTableX(CSGemData gemData, double requestedSizeX, bool useLargestPossible, bool useIterationOnly, out OnXform xBestScale)
	{
		return GetGemScaleFromTable(gemData, requestedSizeX, -1.23432101234321E+308, useLargestPossible, useIterationOnly, out xBestScale);
	}

	public static bool GetGemScaleFromTableX(CSGemData gemData, double requestedSizeX, bool useLargestPossible, bool useIterationOnly, out Transform xBestScale)
	{
		return GetGemScaleFromTable(gemData, requestedSizeX, -1.23432101234321E+308, useLargestPossible, useIterationOnly, out xBestScale);
	}

	public static bool GetGemScaleFromTableY(CSGemData gemData, double requestedSizeY, bool useLargestPossible, bool useIterationOnly, out OnXform xBestScale)
	{
		return GetGemScaleFromTable(gemData, -1.23432101234321E+308, requestedSizeY, useLargestPossible, useIterationOnly, out xBestScale);
	}

	public static bool GetGemScaleFromX(CSGemData gemData, double requestedSizeX, out OnXform xBestScale)
	{
		return GetGemScale(gemData, requestedSizeX, -1.23432101234321E+308, out xBestScale);
	}

	public static bool GetGemScaleFromY(CSGemData gemData, double requestedSizeY, out OnXform xBestScale)
	{
		return GetGemScale(gemData, -1.23432101234321E+308, requestedSizeY, out xBestScale);
	}

	public static bool GetGemSizeFromTable(CSGemData gemData, double requestedSizeX, double requestedSizeY, bool useLargestPossible, bool useIterationOnly, out GemSize bestSize)
	{
		bestSize = null;
		if (gemData?.Def == null)
		{
			return false;
		}
		if (Math.Abs(requestedSizeX - -1.23432101234321E+308) < CSUtil.Tolerance && Math.Abs(requestedSizeY - -1.23432101234321E+308) < CSUtil.Tolerance)
		{
			return false;
		}
		double num = double.MaxValue;
		IEnumerable<GemSize> enumerable;
		if (!useIterationOnly)
		{
			IEnumerable<GemSize> sizes = gemData.Sizes;
			enumerable = sizes;
		}
		else
		{
			enumerable = gemData.Sizes.Where((GemSize gs) => gs.Iteration);
		}
		foreach (GemSize item in enumerable)
		{
			double num2 = 0.0;
			if (Math.Abs(requestedSizeX - -1.23432101234321E+308) > CSUtil.Tolerance)
			{
				num2 += Math.Abs(item.X - requestedSizeX);
			}
			if (Math.Abs(requestedSizeY - -1.23432101234321E+308) > CSUtil.Tolerance)
			{
				num2 += Math.Abs(item.Y - requestedSizeY);
			}
			if (num2 > num)
			{
				continue;
			}
			if (num2 < num)
			{
				num = num2;
				bestSize = item;
				continue;
			}
			GemSize gemSize;
			GemSize gemSize2;
			if (item.CompareTo(bestSize) < 0)
			{
				gemSize = bestSize;
				gemSize2 = item;
			}
			else
			{
				gemSize = item;
				gemSize2 = bestSize;
			}
			bestSize = (useLargestPossible ? gemSize : gemSize2);
		}
		return bestSize != null;
	}

	public static bool GetGemSizeFromTableMaxZ(CSGemData gemData, double requestedSizeX, double requestedSizeY, double maxSizeZ, bool useLargestPossible, bool useIterationOnly, out GemSize bestSize)
	{
		bestSize = null;
		if (gemData?.Def == null)
		{
			return false;
		}
		if (Math.Abs(requestedSizeX - -1.23432101234321E+308) < CSUtil.Tolerance && Math.Abs(requestedSizeY - -1.23432101234321E+308) < CSUtil.Tolerance)
		{
			return false;
		}
		double num = double.MaxValue;
		IEnumerable<GemSize> enumerable;
		if (!useIterationOnly)
		{
			IEnumerable<GemSize> sizes = gemData.Sizes;
			enumerable = sizes;
		}
		else
		{
			enumerable = gemData.Sizes.Where((GemSize gs) => gs.Iteration);
		}
		foreach (GemSize item in enumerable)
		{
			if (item.Z > maxSizeZ)
			{
				continue;
			}
			double num2 = 0.0;
			if (Math.Abs(requestedSizeX - -1.23432101234321E+308) > CSUtil.Tolerance)
			{
				num2 += Math.Abs(item.X - requestedSizeX);
			}
			if (Math.Abs(requestedSizeY - -1.23432101234321E+308) > CSUtil.Tolerance)
			{
				num2 += Math.Abs(item.Y - requestedSizeY);
			}
			if (num2 > num)
			{
				continue;
			}
			if (num2 < num)
			{
				num = num2;
				bestSize = item;
				continue;
			}
			GemSize gemSize;
			GemSize gemSize2;
			if (item.CompareTo(bestSize) < 0)
			{
				gemSize = bestSize;
				gemSize2 = item;
			}
			else
			{
				gemSize = item;
				gemSize2 = bestSize;
			}
			bestSize = (useLargestPossible ? gemSize : gemSize2);
		}
		return bestSize != null;
	}

	public static bool GetGemSizeFromTableX(CSGemData gemData, double requestedSizeX, bool useLargestPossible, bool useIterationOnly, out GemSize bestSize)
	{
		return GetGemSizeFromTable(gemData, requestedSizeX, -1.23432101234321E+308, useLargestPossible, useIterationOnly, out bestSize);
	}

	public static bool GetGemSizeFromTableY(CSGemData gemData, double requestedSizeY, bool useLargestPossible, bool useIterationOnly, out GemSize bestSize)
	{
		return GetGemSizeFromTable(gemData, -1.23432101234321E+308, requestedSizeY, useLargestPossible, useIterationOnly, out bestSize);
	}

	public static bool GetGemZOffset(CSGemData gem, ValueListItem method, ref double zOffset)
	{
		return method switch
		{
			ValueListItem.GemPlacementMethodGirdle => GetGemZOffset(gem, EGemPlacementMethod.Girdle, ref zOffset), 
			ValueListItem.GemPlacementMethodCulet => GetGemZOffset(gem, EGemPlacementMethod.Culet, ref zOffset), 
			ValueListItem.GemPlacementMethodTable => GetGemZOffset(gem, EGemPlacementMethod.Table, ref zOffset), 
			ValueListItem.GemPlacementMethodGirdleScaled => GetGemZOffset(gem, EGemPlacementMethod.GirdleScaled, ref zOffset), 
			ValueListItem.GemPlacementMethodCuletScaled => GetGemZOffset(gem, EGemPlacementMethod.CuletScaled, ref zOffset), 
			ValueListItem.GemPlacementMethodTableScaled => GetGemZOffset(gem, EGemPlacementMethod.TableScaled, ref zOffset), 
			_ => false, 
		};
	}

	public static bool GetGemZOffset(CSGemData gem, EGemPlacementMethod method, ref double zOffset)
	{
		switch (method)
		{
		case EGemPlacementMethod.Culet:
			zOffset += Math.Abs(gem.GemZMin);
			break;
		case EGemPlacementMethod.Table:
			zOffset -= Math.Abs(gem.GemZMax);
			break;
		case EGemPlacementMethod.GirdleScaled:
			zOffset *= gem.GemZMin;
			break;
		default:
			return false;
		case EGemPlacementMethod.Girdle:
		case EGemPlacementMethod.CuletScaled:
		case EGemPlacementMethod.TableScaled:
			break;
		}
		return true;
	}
}
