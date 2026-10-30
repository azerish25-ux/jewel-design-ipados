using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CS.MaterialGroup;
using CSMaterials;
using RMA.Rhino;

namespace CS.Selection;

public sealed class SelectionMouseCallback : MRhinoMouseCallback
{
	public override bool OnMouseDown(MRhinoView view, mouse_button button, uint flags, Point point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		if ((int)button == 1)
		{
			foreach (string itemUnderMouse in SelectionHelper.GetItemsUnderMouse(view, point))
			{
				if (CS.MaterialGroup.MaterialGroupHelper.CheckSelectedGroupForItem(itemUnderMouse))
				{
					CS.MaterialGroup.MaterialGroupHelper.RemoveItemFromSelectedGroup(itemUnderMouse);
					continue;
				}
				IEnumerable<MaterialGroupItem> enumerable = CS.MaterialGroup.MaterialGroupHelper.GetSelectionMaterialGroup().MaterialGroupItems.Where((MaterialGroupItem materialGroupItem) => materialGroupItem.Id.StartsWith(itemUnderMouse.Split(':').FirstOrDefault()));
				switch (SelectionHelper.CurrentSelectionMode)
				{
				case SelectionMode.Block:
					enumerable = enumerable.Where((MaterialGroupItem materialItem) => materialItem.Id.Split(':').Count() == 2 || materialItem.Id.Split(':').Count() == 3);
					break;
				case SelectionMode.BlockOutput:
					enumerable = enumerable.Where((MaterialGroupItem materialItem) => materialItem.Id.Split(':').Count() == 1 || materialItem.Id.Split(':').Count() == 3);
					break;
				case SelectionMode.BlockOutputIndex:
					enumerable = enumerable.Where((MaterialGroupItem materialItem) => materialItem.Id.Split(':').Count() == 1 || materialItem.Id.Split(':').Count() == 2);
					break;
				}
				CS.MaterialGroup.MaterialGroupHelper.RemoveRangeFromMaterialGroupByClassification(MaterialClassification.Selected, enumerable);
				CS.MaterialGroup.MaterialGroupHelper.AddItemToSelectedGroup(itemUnderMouse);
			}
		}
		return ((MRhinoMouseCallback)this).OnMouseDown(view, button, flags, point);
	}

	public override bool OnMouseMove(MRhinoView view, uint flags, Point point)
	{
		if (SelectionHelper.IsMouseOverEnabled)
		{
			CSApplicationHelper.Application.Document.Graph.SetIndexesOfGraphItems();
			List<string> itemsUnderMouse = SelectionHelper.GetItemsUnderMouse(view, point);
			if (IsMouseOverCollectionDifferent(itemsUnderMouse))
			{
				CS.MaterialGroup.MaterialGroupHelper.ClearMouseOverGroupItems();
				foreach (string item in itemsUnderMouse)
				{
					CS.MaterialGroup.MaterialGroupHelper.AddItemToMouseOverGroup(item);
				}
			}
		}
		return true;
	}

	private static bool IsMouseOverCollectionDifferent(List<string> itemsUnderMouse)
	{
		bool result = false;
		foreach (string item in itemsUnderMouse)
		{
			if (!CS.MaterialGroup.MaterialGroupHelper.CheckMouseOverGroupForItem(item))
			{
				result = true;
			}
		}
		foreach (MaterialGroupItem materialGroupItem in CS.MaterialGroup.MaterialGroupHelper.GetMouseOverMaterialGroup().MaterialGroupItems)
		{
			if (!itemsUnderMouse.Contains(materialGroupItem.Id))
			{
				result = true;
			}
		}
		return result;
	}
}
