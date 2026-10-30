using System.Windows.Forms;
using CS;
using CS.CSValues;
using CS.CSValues.CSValueHandle;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlocks.Core;

namespace GV.CSStudioPatchWire;

public static class CSParts
{
	[CSPatchWire("{C04CA91E-FCE5-43DF-A997-F5327316D857}", 1, "Parts", "Part", "Create a Part", "Images\\PatchWire\\Parts_Part.png", "Parts\\Part\\")]
	public static CSPatchWireFunctionResult Part()
	{
		string text;
		if (CSApplicationHelper.IsCounterSketchStudio)
		{
			text = CSApplicationHelper.Application.InvokePartPath;
		}
		else
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.CheckFileExists = true;
			openFileDialog.Filter = "Rhino 3D Models (*.3dm)|*.3dm|All (*.*)|*.*";
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return CSPatchWireFunctionResult.Failure;
			}
			text = openFileDialog.FileName;
		}
		if (string.IsNullOrEmpty(text))
		{
			return CSPatchWireFunctionResult.Failure;
		}
		CSUICategory cSUICategory = SetupAddPart();
		int sortIndex = 0;
		if (CSAddPart(text, cSUICategory, ref sortIndex) == null)
		{
			return CSPatchWireFunctionResult.Failure;
		}
		PartHelper.CurrentPartName = cSUICategory.CategoryName;
		return CSPatchWireFunctionResult.Success;
	}

	private static CSOutputChannel CSAddPart(string path, CSUICategory cat, ref int sortIndex)
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		CSValueHandleHelper.IsHandleMouseWatcherEnabled = true;
		CSValueOnOff cSValueOnOff = new CSValueOnOff("Part On Off", onOffvalue: true);
		cSValueOnOff.Categories.Add(cat);
		cSValueOnOff.UIVisible = false;
		cSValueOnOff.StudioVisible = false;
		cSValueOnOff.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSGroupOfRhinoObjectsStudioParts cSGroupOfRhinoObjectsStudioParts = new CSGroupOfRhinoObjectsStudioParts("Part");
		cSGroupOfRhinoObjectsStudioParts.AddObjectFromFile(path);
		cSGroupOfRhinoObjectsStudioParts.InOnOff.SetLink(cSValueOnOff.OutValue);
		CSValue cSValue = new CSValue("Center Part", 1.0);
		CSValueXFormObject cSValueXFormObject = new CSValueXFormObject("Handle");
		cSValueXFormObject.InOnOff.SetLink(cSValueOnOff.OutOnOff);
		cSValueXFormObject.InAutoCenter.SetLink(cSValue.OutValue);
		cSValueXFormObject.UIVisible = true;
		cSValueXFormObject.StudioVisible = true;
		cSValueXFormObject.InObjects.SetLink(cSGroupOfRhinoObjectsStudioParts.GetOutputChannel(0));
		CSUtil.AddToDisplayAndGenerate(cSValueXFormObject);
		CSValueHandleHelper.CurrentHandle = cSValueXFormObject;
		return cSValueXFormObject.OutTransformedObjects;
	}

	private static CSUICategory SetupAddPart()
	{
		int i;
		for (i = 1; i < 30 && 0 <= CSApplicationHelper.Application.Document.Graph.CategoryTable.FindCategoryByName(PartHelper.CurrentPartName + " " + i); i++)
		{
		}
		int i2 = ((i < 30) ? CSApplicationHelper.Application.Document.Graph.CategoryTable.AddCategoryDeletable(PartHelper.CurrentPartName + " " + i) : CSApplicationHelper.Application.Document.Graph.CategoryTable.AddCategoryDeletable(PartHelper.CurrentPartName));
		return CSApplicationHelper.Application.Document.Graph.CategoryTable.GetCategory(i2);
	}
}
