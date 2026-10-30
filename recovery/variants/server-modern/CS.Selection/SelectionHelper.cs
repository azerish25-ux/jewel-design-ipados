using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using CS.CSObjects.CSBlocks;
using CS.MaterialGroup;
using CSCompatibility;
using CSDatas;
using CSMaterials;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CS.Selection;

public static class SelectionHelper
{
	private static SelectionMouseCallback _mouseCallback;

	private static CSMaterials.MaterialGroup _originalSelectionGroup;

	public static bool IsSelectionEnabled { get; private set; }

	public static bool IsMouseOverEnabled { get; private set; }

	public static bool IncludeAllMeshes { get; set; }

	public static SelectionMode CurrentSelectionMode { get; set; }

	public static bool IsReturningDirectHits { get; set; }

	public static void EnableSelectionMode(bool enable, bool isReturningDirectHits = true, bool isStoreOriginalSelectionEnabled = false, bool isMouseOverEnabled = true, bool includeAllMeshes = false)
	{
		IsSelectionEnabled = enable;
		IsMouseOverEnabled = isMouseOverEnabled;
		IncludeAllMeshes = includeAllMeshes;
		IsReturningDirectHits = isReturningDirectHits;
		if (_mouseCallback == null)
		{
			_mouseCallback = new SelectionMouseCallback();
		}
		if (!enable)
		{
			CS.MaterialGroup.MaterialGroupHelper.ClearMouseOverGroupItems();
			CS.MaterialGroup.MaterialGroupHelper.ClearSelectedGroupItems();
		}
		if (isStoreOriginalSelectionEnabled)
		{
			_originalSelectionGroup = CS.MaterialGroup.MaterialGroupHelper.GetSelectionMaterialGroup().Clone() as CSMaterials.MaterialGroup;
		}
		((MRhinoMouseCallback)_mouseCallback).Enable(enable);
		Mouse.OverrideCursor = null;
	}

	public static List<string> GetItemsUnderMouse(MRhinoView view, Point point)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<string, double> itemsUnderMouse = new Dictionary<string, double>();
		List<string> result = new List<string>();
		OnLine line = new OnLine();
		if (!view.ActiveViewport().VP().GetFrustumLine((double)point.X, (double)point.Y, ref line))
		{
			return result;
		}
		Transform flipRingTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList().ForEach(delegate(CSBlock csblock)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			double sortValue = double.NaN;
			for (int i = 0; i < csblock.OutputChannelCount; i++)
			{
				CSData outputChannelData = csblock.GetOutputChannelData(i);
				Transform transformation = flipRingTransform;
				if (outputChannelData is CSDataArray cSDataArray)
				{
					CSDataArray flattenArray = cSDataArray.GetFlattenArray();
					for (int j = 0; j < flattenArray.GetArrayCount(); j++)
					{
						if (flattenArray.GetArrayItem(j) != null)
						{
							flag = CheckItemUnderMouse(flattenArray.GetArrayItem(j), line.Convert(), transformation, ref sortValue);
							if (flag && CurrentSelectionMode == SelectionMode.BlockOutputIndex)
							{
								itemsUnderMouse.Add($"{csblock.InstanceGuid}:{i}:{j}", sortValue);
							}
							else if (flag && (CurrentSelectionMode == SelectionMode.BlockOutput || CurrentSelectionMode == SelectionMode.Block))
							{
								break;
							}
						}
					}
					if (flag && CurrentSelectionMode == SelectionMode.BlockOutput)
					{
						itemsUnderMouse.Add($"{csblock.InstanceGuid}:{i}", sortValue);
					}
					else if (flag && CurrentSelectionMode == SelectionMode.Block)
					{
						break;
					}
				}
				else
				{
					flag = CheckItemUnderMouse(outputChannelData, line.Convert(), transformation, ref sortValue);
					if (flag && CurrentSelectionMode == SelectionMode.BlockOutputIndex)
					{
						itemsUnderMouse.Add($"{csblock.InstanceGuid}:{i}:{0}", sortValue);
					}
					else if (flag && CurrentSelectionMode == SelectionMode.BlockOutput)
					{
						itemsUnderMouse.Add($"{csblock.InstanceGuid}:{i}", sortValue);
					}
					else if (flag && CurrentSelectionMode == SelectionMode.Block)
					{
						break;
					}
				}
			}
			if (flag && CurrentSelectionMode == SelectionMode.Block)
			{
				itemsUnderMouse.Add($"{csblock.InstanceGuid}", sortValue);
			}
		});
		result = SortItemsBySelectionMode(itemsUnderMouse.OrderByDescending((KeyValuePair<string, double> mouseItem) => mouseItem.Value));
		if (result != null && result.Any())
		{
			Mouse.OverrideCursor = Cursors.Hand;
		}
		else
		{
			Mouse.OverrideCursor = null;
		}
		return result;
	}

	public static List<string> GetItemsUnderMouse(RhinoView view, Point point, List<CSBlock> blockSampleSet = null)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<string, double> dictionary = new Dictionary<string, double>();
		List<string> result = new List<string>();
		if (blockSampleSet == null)
		{
			blockSampleSet = CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList();
		}
		Line line = default(Line);
		if (!view.ActiveViewport.GetFrustumLine((double)point.X, (double)point.Y, ref line))
		{
			return result;
		}
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		foreach (CSBlock item in blockSampleSet)
		{
			bool flag = false;
			double sortValue = double.NaN;
			for (int i = 0; i < item.OutputChannelCount; i++)
			{
				CSData outputChannelData = item.GetOutputChannelData(i);
				Transform transformation = designTransform;
				if (outputChannelData is CSDataArray cSDataArray)
				{
					CSDataArray flattenArray = cSDataArray.GetFlattenArray();
					for (int j = 0; j < flattenArray.GetArrayCount(); j++)
					{
						if (flattenArray.GetArrayItem(j) != null)
						{
							flag = CheckItemUnderMouse(flattenArray.GetArrayItem(j), line, transformation, ref sortValue);
							if (flag && CurrentSelectionMode == SelectionMode.BlockOutputIndex)
							{
								dictionary.Add($"{item.InstanceGuid}:{i}:{j}", sortValue);
							}
							else if (flag && (CurrentSelectionMode == SelectionMode.BlockOutput || CurrentSelectionMode == SelectionMode.Block))
							{
								break;
							}
						}
					}
					if (flag && CurrentSelectionMode == SelectionMode.BlockOutput)
					{
						dictionary.Add($"{item.InstanceGuid}:{i}", sortValue);
					}
					else if (flag && CurrentSelectionMode == SelectionMode.Block)
					{
						break;
					}
				}
				else
				{
					flag = CheckItemUnderMouse(outputChannelData, line, transformation, ref sortValue);
					if (flag && CurrentSelectionMode == SelectionMode.BlockOutputIndex)
					{
						dictionary.Add($"{item.InstanceGuid}:{i}:{0}", sortValue);
					}
					else if (flag && CurrentSelectionMode == SelectionMode.BlockOutput)
					{
						dictionary.Add($"{item.InstanceGuid}:{i}", sortValue);
					}
					else if (flag && CurrentSelectionMode == SelectionMode.Block)
					{
						break;
					}
				}
			}
			if (flag && CurrentSelectionMode == SelectionMode.Block)
			{
				dictionary.Add($"{item.InstanceGuid}", sortValue);
			}
		}
		result = SortItemsBySelectionMode(dictionary.OrderByDescending((KeyValuePair<string, double> mouseItem) => mouseItem.Value));
		if (result != null && result.Any())
		{
			Mouse.OverrideCursor = Cursors.Hand;
		}
		else
		{
			Mouse.OverrideCursor = null;
		}
		return result;
	}

	public static bool IntersectMesh(Mesh mesh, Line line, ref double t)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		int[] array2 = default(int[]);
		Point3d[] array = Intersection.MeshLine(mesh, line, ref array2);
		if (array.Length == 0)
		{
			return false;
		}
		List<double> list = ((IEnumerable<Point3d>)array).Select((Func<Point3d, double>)((Line)(object)line).ClosestParameter).ToList();
		if (list.Count <= 0)
		{
			return false;
		}
		t = list.Max();
		return true;
	}

	private static bool CheckItemUnderMouse(CSData csData, Line line, Transform transformation, ref double sortValue)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (csData is CSGemData cSGemData)
		{
			Mesh meshFromDefinition = cSGemData.GetMeshFromDefinition(transformation);
			BoundingBox boundingBox = ((GeometryBase)meshFromDefinition).GetBoundingBox(false);
			Interval val = default(Interval);
			if (Intersection.LineBox(line, boundingBox, 0.01, ref val) && IntersectMesh(meshFromDefinition, line, ref sortValue))
			{
				return true;
			}
			return false;
		}
		if (csData is CSDataArray cSDataArray)
		{
			for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
			{
				if (CheckItemUnderMouse(cSDataArray.GetArrayItem(i), line, transformation, ref sortValue))
				{
					return true;
				}
			}
			return false;
		}
		if (IncludeAllMeshes && csData is CSOnGeometryData cSOnGeometryData)
		{
			Mesh mesh = cSOnGeometryData.GetMesh();
			if (mesh != null)
			{
				((GeometryBase)mesh).Transform(transformation);
				BoundingBox boundingBox2 = ((GeometryBase)mesh).GetBoundingBox(false);
				Interval val2 = default(Interval);
				if (Intersection.LineBox(line, boundingBox2, 0.01, ref val2) && IntersectMesh(mesh, line, ref sortValue))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	private static List<string> SortItemsBySelectionMode(IEnumerable<KeyValuePair<string, double>> itemsUnderMouse)
	{
		List<string> list = new List<string>();
		IList<KeyValuePair<string, double>> source = (itemsUnderMouse as IList<KeyValuePair<string, double>>) ?? itemsUnderMouse.ToList();
		if (source.Any())
		{
			switch (CurrentSelectionMode)
			{
			case SelectionMode.Block:
			{
				string text3 = $"{source.FirstOrDefault().Key.Split(':')[0]}";
				foreach (KeyValuePair<string, double> item in source.Where(delegate(KeyValuePair<string, double> mouseItem)
				{
					string key = mouseItem.Key;
					return key != null && key.Split(':').Count() == 1;
				}))
				{
					string text4 = $"{item.Key.Split(':')[0]}";
					if (IsReturningDirectHits)
					{
						if (text4 == text3)
						{
							list.Add(text4);
						}
					}
					else
					{
						list.Add(text4);
					}
				}
				break;
			}
			case SelectionMode.BlockOutput:
			{
				KeyValuePair<string, double> keyValuePair2 = source.FirstOrDefault();
				string text5 = $"{keyValuePair2.Key.Split(':')[0]}:{keyValuePair2.Key.Split(':')[1]}";
				foreach (KeyValuePair<string, double> item2 in source.Where((KeyValuePair<string, double> mouseItem) => mouseItem.Key.Split(':').Count() == 2))
				{
					string text6 = $"{item2.Key.Split(':')[0]}:{item2.Key.Split(':')[1]}";
					if (IsReturningDirectHits)
					{
						if (text6 == text5)
						{
							list.Add(text6);
						}
					}
					else
					{
						list.Add(text6);
					}
				}
				break;
			}
			case SelectionMode.BlockOutputIndex:
			{
				KeyValuePair<string, double> keyValuePair = source.FirstOrDefault();
				string text = $"{keyValuePair.Key.Split(':')[0]}:{keyValuePair.Key.Split(':')[1]}:{keyValuePair.Key.Split(':')[2]}";
				foreach (KeyValuePair<string, double> item3 in source.Where((KeyValuePair<string, double> mouseItem) => mouseItem.Key.Split(':').Count() == 3))
				{
					string text2 = $"{item3.Key.Split(':')[0]}:{item3.Key.Split(':')[1]}:{item3.Key.Split(':')[2]}";
					if (IsReturningDirectHits)
					{
						if (text2 == text)
						{
							list.Add(text2);
						}
					}
					else
					{
						list.Add(text2);
					}
				}
				break;
			}
			}
		}
		return list;
	}
}
