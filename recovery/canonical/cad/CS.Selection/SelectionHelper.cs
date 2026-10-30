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
		_mouseCallback.Enable(enable);
		Mouse.OverrideCursor = null;
	}

	public static List<string> GetItemsUnderMouse(MRhinoView view, System.Drawing.Point point)
	{
		Dictionary<string, double> itemsUnderMouse = new Dictionary<string, double>();
		List<string> result = new List<string>();
		OnLine line = new OnLine();
		if (!view.ActiveViewport().VP().GetFrustumLine(point.X, point.Y, ref line))
		{
			return result;
		}
		Transform flipRingTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList().ForEach(delegate(CSBlock csblock)
		{
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

	public static List<string> GetItemsUnderMouse(RhinoView view, System.Drawing.Point point, List<CSBlock> blockSampleSet = null)
	{
		Dictionary<string, double> dictionary = new Dictionary<string, double>();
		List<string> result = new List<string>();
		if (blockSampleSet == null)
		{
			blockSampleSet = CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList();
		}
		if (!view.ActiveViewport.GetFrustumLine(point.X, point.Y, out var worldLine))
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
							flag = CheckItemUnderMouse(flattenArray.GetArrayItem(j), worldLine, transformation, ref sortValue);
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
					flag = CheckItemUnderMouse(outputChannelData, worldLine, transformation, ref sortValue);
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
		int[] faceIds;
		Point3d[] array = Intersection.MeshLine(mesh, line, out faceIds);
		if (array.Length == 0)
		{
			return false;
		}
		List<double> list = array.Select(((Line)line).ClosestParameter).ToList();
		if (list.Count <= 0)
		{
			return false;
		}
		t = list.Max();
		return true;
	}

	private static bool CheckItemUnderMouse(CSData csData, Line line, Transform transformation, ref double sortValue)
	{
		if (csData is CSGemData cSGemData)
		{
			Mesh meshFromDefinition = cSGemData.GetMeshFromDefinition(transformation);
			BoundingBox boundingBox = meshFromDefinition.GetBoundingBox(accurate: false);
			if (Intersection.LineBox(line, boundingBox, 0.01, out var _) && IntersectMesh(meshFromDefinition, line, ref sortValue))
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
				mesh.Transform(transformation);
				BoundingBox boundingBox2 = mesh.GetBoundingBox(accurate: false);
				if (Intersection.LineBox(line, boundingBox2, 0.01, out var _) && IntersectMesh(mesh, line, ref sortValue))
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
