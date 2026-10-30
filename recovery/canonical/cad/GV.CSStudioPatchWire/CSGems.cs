using CS;
using CS.CSValues;
using CS.CSValues.CSValueHandle;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlocks.Core;
using CSBlocks.Settings;
using CSBlocks.Settings.Bezels;
using CSBlocks.Settings.Heads.CSHeadBuilderV1;
using CSBlocks.Transforms;
using CSBlocks.Utilities;
using CSGemInstance.GemTableData;
using CSMaterials;

namespace GV.CSStudioPatchWire;

public static class CSGems
{
	[CSPatchWire("{01E1D329-BA89-47F6-94E4-74905A0CC137}", 1, "Gems", "Gem", "Create a Gem", "Images\\PatchWire\\Gems_Gem.png", "CSGems\\Gem\\")]
	public static CSPatchWireFunctionResult Gem()
	{
		CSUICategory cSUICategory = SetupAddGem();
		int sortIndex = 0;
		PatchWire.CSMirror(CSAddGemHandles(cSUICategory, ref sortIndex, out var _), cSUICategory, ref sortIndex);
		PartHelper.CurrentPartName = cSUICategory.CategoryName;
		return CSPatchWireFunctionResult.Success;
	}

	[CSPatchWire("{345e2a6d-16ad-420d-b72f-4cada011426e}", 1, "Gems", "Gem with Bezel", "Create a Gem with Bezel", "Images\\PatchWire\\Settings_Bezel.png", "CSGems\\GemWithBezel\\")]
	public static CSPatchWireFunctionResult GemWithBezel()
	{
		CSUICategory cSUICategory = SetupAddGem();
		int sortIndex = 0;
		CSOutputChannel gemDefinition;
		CSOutputChannel cSOutputChannel = CSAddGemHandles(cSUICategory, ref sortIndex, out gemDefinition);
		CSOutputChannel outputChannel = CSAddBezel(cSOutputChannel, cSUICategory, ref sortIndex);
		CSArray cSArray = new CSArray("Array");
		cSArray.AddLink(cSOutputChannel);
		cSArray.AddLink(outputChannel);
		PatchWire.CSMirror(cSArray.OutArray, cSUICategory, ref sortIndex);
		PartHelper.CurrentPartName = cSUICategory.CategoryName;
		return CSPatchWireFunctionResult.Success;
	}

	[CSPatchWire("{c5b773d4-02d8-4926-966e-80f121f5f034}", 2, "Gems", "Gem with Custom Head", "Create a Gem with a Custom Head", "Images\\PatchWire\\Settings_Head.png", "CSGems\\GemWithCustomHead\\")]
	public static CSPatchWireFunctionResult GemWithHeadCustom()
	{
		CSUICategory cSUICategory = SetupAddGem();
		int sortIndex = 0;
		CSOutputChannel gemDefinition;
		CSOutputChannel cSOutputChannel = CSAddGemHandles(cSUICategory, ref sortIndex, out gemDefinition);
		CSOutputChannel outputChannel = CSAddHeadCustom(cSOutputChannel, gemDefinition, cSUICategory, ref sortIndex);
		CSArray cSArray = new CSArray("Array");
		cSArray.AddLink(cSOutputChannel);
		cSArray.AddLink(outputChannel);
		PatchWire.CSMirror(cSArray.OutArray, cSUICategory, ref sortIndex);
		PartHelper.CurrentPartName = cSUICategory.CategoryName;
		return CSPatchWireFunctionResult.Success;
	}

	[CSPatchWire("{232e3cd4-a3dc-47fe-8bdf-b208eb86aa6d}", 2, "Gems", "Gem with Head", "Create a Gem with Head", "Images\\PatchWire\\Settings_ChooseHeadToGem.png", "CSGems\\GemWithHead\\")]
	public static CSPatchWireFunctionResult GemWithHeadLibrary()
	{
		CSUICategory cSUICategory = SetupAddGem();
		int sortIndex = 0;
		CSOutputChannel gemDefinition;
		CSOutputChannel cSOutputChannel = CSAddGemHandles(cSUICategory, ref sortIndex, out gemDefinition);
		CSOutputChannel outputChannel = CSAddHeadLibrary(gemDefinition, cSOutputChannel, cSUICategory, ref sortIndex);
		CSArray cSArray = new CSArray("Array");
		cSArray.AddLink(cSOutputChannel);
		cSArray.AddLink(outputChannel);
		PatchWire.CSMirror(cSArray.OutArray, cSUICategory, ref sortIndex);
		PartHelper.CurrentPartName = cSUICategory.CategoryName;
		return CSPatchWireFunctionResult.Success;
	}

	private static CSOutputChannel CSAddBezel(CSOutputChannel gemWithHandle, CSUICategory cat, ref int sortIndex)
	{
		CSValueChooser cSValueChooser = new CSValueChooser("Profile Chooser")
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueChooser.SetChooserSource(CSProfiles.GetBezelBlock());
		cSValueChooser.Categories.Add(cat);
		cSValueChooser.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange = new CSValueRange("Girdle Spacing", 0.0, -1.0, 2.0, 0.05)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange.Categories.Add(cat);
		cSValueRange.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange2 = new CSValueRange("Placement", 0.0, -2.0, 2.0, 0.05)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange2.Categories.Add(cat);
		cSValueRange2.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange3 = new CSValueRange("Angle", -17.0, -45.0, 45.0, 1.0)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange3.Categories.Add(cat);
		cSValueRange3.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange4 = new CSValueRange("Height", 5.0, 0.0, 10.0, 0.1)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange4.Categories.Add(cat);
		cSValueRange4.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange5 = new CSValueRange("Top Thickness", 0.8, 0.0, 2.0, 0.05)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange5.Categories.Add(cat);
		cSValueRange5.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange6 = new CSValueRange("Bottom Thickness", 0.8, 0.0, 2.0, 0.05)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange6.Categories.Add(cat);
		cSValueRange6.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange7 = new CSValueRange("Seat Height", 0.6, 0.0, 2.0, 0.05)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange7.Categories.Add(cat);
		cSValueRange7.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValue cSValue = new CSValue("Seat Angle", 45.0);
		CSValue cSValue2 = new CSValue("Seat Length", 0.6);
		CSValue cSValue3 = new CSValue("Fillet", 75.0);
		CSBezel cSBezel = new CSBezel("Bezel");
		cSBezel.InGems.SetLink(gemWithHandle);
		cSBezel.InProfile.SetLink(cSValueChooser.OutObject);
		cSBezel.InGirldeSpacing.SetLink(cSValueRange.OutValue);
		cSBezel.InPlacement.SetLink(cSValueRange2.OutValue);
		cSBezel.InAngle.SetLink(cSValueRange3.OutValue);
		cSBezel.InHeight.SetLink(cSValueRange4.OutValue);
		cSBezel.InTopThickness.SetLink(cSValueRange5.OutValue);
		cSBezel.InBottomThickness.SetLink(cSValueRange6.OutValue);
		cSBezel.InSeatAngle.SetLink(cSValue.OutValue);
		cSBezel.InSeatHeight.SetLink(cSValueRange7.OutValue);
		cSBezel.InSeatLength.SetLink(cSValue2.OutValue);
		cSBezel.InFillet.SetLink(cSValue3.OutValue);
		CSUtil.AddToDisplayAndGenerate(cSBezel, MaterialClassification.MetalSecondary);
		return cSBezel.OutBezel;
	}

	private static CSOutputChannel CSAddGemHandles(CSUICategory cat, ref int sortIndex, out CSOutputChannel gemDefinition)
	{
		CSValueGemShape cSValueGemShape = new CSValueGemShape("Gem Shape")
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueGemShape.SetChooserSource(GetGemDefinitionBlock());
		cSValueGemShape.Categories.Add(cat);
		cSValueGemShape.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueGemSize cSValueGemSize = new CSValueGemSize("Gem Size")
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueGemSize.InGemShape.SetLink(cSValueGemShape.OutGem);
		cSValueGemSize.Categories.Add(cat);
		cSValueGemSize.SetSizeValuesFromCarat(1.0);
		cSValueGemSize.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueXFormObject cSValueXFormObject = new CSValueXFormObject("Handle");
		cSValueXFormObject.SetVisible(showMove: true, showRotate: true, showScale: false, showBend: false);
		CSValueOnOff cSValueOnOff = new CSValueOnOff("Gem On Off", onOffvalue: true)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueOnOff.Categories.Add(cat);
		cSValueOnOff.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		cSValueGemShape.InOnOff.SetLink(cSValueOnOff.OutOnOff);
		cSValueGemSize.InOnOff.SetLink(cSValueOnOff.OutOnOff);
		cSValueXFormObject.InOnOff.SetLink(cSValueOnOff.OutOnOff);
		CSTransformObject cSTransformObject = new CSTransformObject("Gem Transform");
		cSTransformObject.InObject.SetLink(cSValueGemSize.OutGem);
		cSTransformObject.AddLink(cSValueXFormObject.OutValue);
		CSUtil.AddToDisplayAndGenerate(cSTransformObject, MaterialClassification.GemCenter);
		gemDefinition = cSValueGemSize.OutGem;
		return cSTransformObject.OutObjectTransformed;
	}

	private static CSOutputChannel CSAddHeadCustom(CSOutputChannel gemWithHandle, CSOutputChannel gemDefinition, CSUICategory cat, ref int sortIndex)
	{
		CSGroupOfMatrixProfiles prongBlock = CSProfiles.GetProngBlock();
		CSGroupOfMatrixProfiles galleryRailBlock = CSProfiles.GetGalleryRailBlock();
		CSValueHeadStyleList cSValueHeadStyleList = new CSValueHeadStyleList("Setting : Style")
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueHeadStyleList.Categories.Add(cat);
		cSValueHeadStyleList.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		cSValueHeadStyleList.InGem.SetLink(gemDefinition);
		CSValueRange cSValueRange = new CSValueRange("Top Height", 0.3, 0.05, 2.0, 0.05)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange.Categories.Add(cat);
		cSValueRange.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange2 = new CSValueRange("Base Drop", -5.0, -10.0, 2.0, 0.05)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange2.Categories.Add(cat);
		cSValueRange2.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange3 = new CSValueRange("Head Angle", 10.0, 0.0, 35.0, 1.0)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange3.Categories.Add(cat);
		cSValueRange3.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange4 = new CSValueRange("Head Top Angle", 0.0, -10.0, 10.0, 0.1)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange4.Categories.Add(cat);
		cSValueRange4.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange5 = new CSValueRange("Prong Move", 0.0, -20.0, 20.0, 0.1);
		CSValueChooser cSValueChooser = new CSValueChooser("Profile Chooser")
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueChooser.SetChooserSource(prongBlock);
		cSValueChooser.Categories.Add(cat);
		cSValueChooser.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange6 = new CSValueRange("Prong X", 1.0, 0.1, 3.0, 0.01)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange6.Categories.Add(cat);
		cSValueRange6.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange7 = new CSValueRange("Prong Y", 1.0, 0.1, 3.0, 0.01)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange7.Categories.Add(cat);
		cSValueRange7.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange8 = new CSValueRange("Prong Nudge", 0.1, 0.0, 1.0, 0.01);
		CSValueOnOff cSValueOnOff = new CSValueOnOff("Enable Base", onOffvalue: false);
		CSValueChooser cSValueChooser2 = new CSValueChooser("Profile Chooser");
		cSValueChooser2.SetChooserSource(galleryRailBlock);
		CSValueRange cSValueRange9 = new CSValueRange("Rail X", 1.0, 0.1, 2.0, 0.01)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange9.Categories.Add(cat);
		cSValueRange9.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueRange cSValueRange10 = new CSValueRange("Rail Y", 1.0, 0.1, 2.0, 0.01)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange10.Categories.Add(cat);
		cSValueRange10.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueOnOff cSValueOnOff2 = new CSValueOnOff("Enable Rail 1", onOffvalue: true);
		CSValueRange cSValueRange11 = new CSValueRange("Rail 1 Height", 0.0, 0.0, 1.0, 0.01)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange11.Categories.Add(cat);
		cSValueRange11.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueOnOff cSValueOnOff3 = new CSValueOnOff("Enable Rail 1 Extra Controls", onOffvalue: false);
		CSValueOnOff cSValueOnOff4 = new CSValueOnOff("Enable Rail 2", onOffvalue: true);
		CSValueRange cSValueRange12 = new CSValueRange("Rail 2 Height", 0.66, 0.0, 1.0, 0.01)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueRange12.Categories.Add(cat);
		cSValueRange12.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueOnOff cSValueOnOff5 = new CSValueOnOff("Enable Rail 2 Extra Controls", onOffvalue: false);
		CSHeadShape cSHeadShape = new CSHeadShape("Head Shape");
		cSHeadShape.InGems.SetLink(gemWithHandle);
		cSHeadShape.InStyle.SetLink(cSValueHeadStyleList.OutListValue);
		cSHeadShape.InAbove.SetLink(cSValueRange.OutValue);
		cSHeadShape.InBelow.SetLink(cSValueRange2.OutValue);
		cSHeadShape.InAngleX.SetLink(cSValueRange3.OutValue);
		cSHeadShape.InTopAngle.SetLink(cSValueRange4.OutValue);
		cSHeadShape.InProngRotateAngle.SetLink(cSValueRange5.OutValue);
		CSHeadProngs cSHeadProngs = new CSHeadProngs("Head Prongs");
		cSHeadProngs.InGems.SetLink(gemWithHandle);
		cSHeadProngs.InProngLayout.SetLink(cSValueHeadStyleList.OutListValue);
		cSHeadProngs.InProngRails.SetLink(cSHeadShape.OutHeadProngLines);
		cSHeadProngs.InHeadTop.SetLink(cSHeadShape.OutHeadTop);
		cSHeadProngs.InHeadBase.SetLink(cSHeadShape.OutHeadBase);
		cSHeadProngs.InProngTopProfile.SetLink(cSValueChooser.OutObject);
		cSHeadProngs.InProngTopX.SetLink(cSValueRange6.OutValue);
		cSHeadProngs.InProngTopY.SetLink(cSValueRange7.OutValue);
		cSHeadProngs.InProngTopNudge.SetLink(cSValueRange8.OutValue);
		cSHeadProngs.InEnableBase.SetLink(cSValueOnOff.OutValue);
		cSHeadProngs.InProngBaseProfile.SetLink(cSValueChooser.OutObject);
		cSHeadProngs.InProngBaseX.SetLink(cSValueRange6.OutValue);
		cSHeadProngs.InProngBaseY.SetLink(cSValueRange7.OutValue);
		cSHeadProngs.InProngBaseNudge.SetLink(cSValueRange8.OutValue);
		CSHeadRail cSHeadRail = new CSHeadRail("Head Rail (Lower)");
		cSHeadRail.InGem.SetLink(gemWithHandle);
		cSHeadRail.InHeadSideX.SetLink(cSHeadShape.OutHeadSideX);
		cSHeadRail.InHeadSideY.SetLink(cSHeadShape.OutHeadSideY);
		cSHeadRail.InEnableRail.SetLink(cSValueOnOff2.OutValue);
		cSHeadRail.InRailZPos.SetLink(cSValueRange11.OutValue);
		cSHeadRail.InRailProfile.SetLink(cSValueChooser2.OutObject);
		cSHeadRail.InRailX.SetLink(cSValueRange9.OutValue);
		cSHeadRail.InRailY.SetLink(cSValueRange10.OutValue);
		cSHeadRail.InEnableSecondControl.SetLink(cSValueOnOff3.OutValue);
		CSHeadRail cSHeadRail2 = new CSHeadRail("Head Rail (Upper)");
		cSHeadRail2.InGem.SetLink(gemWithHandle);
		cSHeadRail2.InHeadSideX.SetLink(cSHeadShape.OutHeadSideX);
		cSHeadRail2.InHeadSideY.SetLink(cSHeadShape.OutHeadSideY);
		cSHeadRail2.InEnableRail.SetLink(cSValueOnOff4.OutValue);
		cSHeadRail2.InRailZPos.SetLink(cSValueRange12.OutValue);
		cSHeadRail2.InRailProfile.SetLink(cSValueChooser2.OutObject);
		cSHeadRail2.InRailX.SetLink(cSValueRange9.OutValue);
		cSHeadRail2.InRailY.SetLink(cSValueRange10.OutValue);
		cSHeadRail2.InEnableSecondControl.SetLink(cSValueOnOff5.OutValue);
		CSAssignObjectToGem cSAssignObjectToGem = new CSAssignObjectToGem("Object To Gem");
		cSAssignObjectToGem.InGem.SetLink(gemWithHandle);
		cSAssignObjectToGem.InObjects.SetLink(cSHeadRail2.OutObjects);
		CSUtil.AddToDisplayAndGenerate(cSAssignObjectToGem, MaterialClassification.MetalSecondary);
		return cSAssignObjectToGem.OutObjects;
	}

	private static CSOutputChannel CSAddHeadLibrary(CSOutputChannel gemDefinition, CSOutputChannel gemWithHandle, CSUICategory cat, ref int sortIndex)
	{
		CSGroupOfRhinoObjectsFilePath cSGroupOfRhinoObjectsFilePath = new CSGroupOfRhinoObjectsFilePath("Head Objects");
		cSGroupOfRhinoObjectsFilePath.iRestrictTo.SetLink(gemDefinition);
		cSGroupOfRhinoObjectsFilePath.AddObjectsFromLibraryPath("Heads");
		cSGroupOfRhinoObjectsFilePath.Generate();
		CSValueChooser cSValueChooser = new CSValueChooser("Which Object")
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueChooser.SetChooserSource(cSGroupOfRhinoObjectsFilePath);
		cSValueChooser.Categories.Add(cat);
		cSValueChooser.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSGenerateMesh cSGenerateMesh = new CSGenerateMesh("MeshHead");
		cSGenerateMesh.InObjects.SetLink(cSValueChooser.OutObject);
		CSOrientToGem cSOrientToGem = new CSOrientToGem("Orient to Gem");
		cSOrientToGem.InObject.SetLink(cSGenerateMesh.OutObjects);
		cSOrientToGem.InGemDestination.SetLink(gemWithHandle);
		CSUtil.AddToDisplayAndGenerate(cSOrientToGem, MaterialClassification.MetalSecondary);
		return cSOrientToGem.OutObject;
	}

	private static CSGroupOfGems GetGemDefinitionBlock()
	{
		CSGroupOfGems cSGroupOfGems = new CSGroupOfGems("GroupOfGems");
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Round);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.BaguetteStraight);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Calf);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Cushion);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.CushionSquare);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Oval);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Emerald);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.EmeraldSquare);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.HalfMoon);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Heart);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Marquise);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Pear);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Princess);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.Radiant);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.TrillionCurved);
		cSGroupOfGems.AddGem(GemTableType.Diamond, GemTableShape.TrillionStraight);
		cSGroupOfGems.Update();
		return cSGroupOfGems;
	}

	private static CSUICategory SetupAddGem()
	{
		int i;
		for (i = 1; i < 30 && 0 <= CSApplicationHelper.Application.Document.Graph.CategoryTable.FindCategoryByName("CS Gems " + i); i++)
		{
		}
		int i2 = ((i < 30) ? CSApplicationHelper.Application.Document.Graph.CategoryTable.AddCategoryDeletable("CS Gems " + i) : CSApplicationHelper.Application.Document.Graph.CategoryTable.AddCategoryDeletable("CS Gems"));
		return CSApplicationHelper.Application.Document.Graph.CategoryTable.GetCategory(i2);
	}
}
