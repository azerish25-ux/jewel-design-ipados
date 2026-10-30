using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CSDatas;
using CSDatas.Interfaces;
using CSMaterials;
using GV.CounterSketch.Shared;
using Newtonsoft.Json.Linq;

namespace CS.MaterialGroup;

public static class MaterialGroupHelper
{
	public delegate void SelectionMaterialGroupChangedHandler(object sender, EventArgs eventArgs);

	private static readonly Dictionary<Guid, string> _MaterialGroupAlphaIdentifiers = new Dictionary<Guid, string>();

	private static List<CSMaterials.MaterialGroup> _originalMaterialGroups;

	public static List<CSMaterials.MaterialGroup> ImportedDocumentMaterialGroups { get; set; }

	public static bool IsGenerateMaterialGroupsFromGraphNeeded { get; set; }

	public static bool IsVerificationMaterialGroupEnabled
	{
		get
		{
			return CSMaterials.MaterialGroupHelper.IsVerificationMaterialGroupEnabled;
		}
		set
		{
			CSMaterials.MaterialGroupHelper.IsVerificationMaterialGroupEnabled = value;
		}
	}

	public static event SelectionMaterialGroupChangedHandler SelectionMaterialGroupChanged;

	public static void AddBlockToDefaultMaterialGroups(CSBlock csBlock, bool redraw = true)
	{
		MaterialClassification outputDisplayMaterialClassification = csBlock.OutputDisplayMaterialClassification;
		if ((uint)(outputDisplayMaterialClassification - 2) <= 4u || (uint)(outputDisplayMaterialClassification - 19) <= 2u)
		{
			AddItemToMaterialGroupByClassification(csBlock.OutputDisplayMaterialClassification, csBlock.InstanceGuid.ToString(), redraw);
		}
	}

	public static void AddItemToMaterialGroup(CSMaterials.MaterialGroup materialGroup, string identifier, bool redraw = true, bool isInherited = false)
	{
		if (materialGroup == null)
		{
			return;
		}
		if (isInherited)
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.ToList().ForEach(delegate(CSMaterials.MaterialGroup inheritedMaterialGroup)
			{
				MaterialGroupItem materialGroupItem = inheritedMaterialGroup.MaterialGroupItems.FirstOrDefault((MaterialGroupItem materialGroupItem2) => materialGroupItem2.Id == identifier && materialGroupItem2.IsInherited);
				if (materialGroupItem != null)
				{
					inheritedMaterialGroup.MaterialGroupItems.Remove(materialGroupItem);
				}
			});
		}
		if (materialGroup.MaterialGroupItems.All((MaterialGroupItem materialGroupItem) => materialGroupItem.Id != identifier))
		{
			materialGroup.MaterialGroupItems.Add(new MaterialGroupItem
			{
				Id = identifier,
				IsInherited = isInherited
			});
		}
		if (redraw)
		{
			if (isInherited)
			{
				CSUtil.ForceGraphToRedraw();
			}
			else
			{
				CSUtil.Redraw();
			}
		}
	}

	public static void AddItemToMaterialGroupByClassification(MaterialClassification materialClassification, string identifier, bool redraw = true)
	{
		CSMaterials.MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup group) => group.Classification == materialClassification);
		if (materialGroup != null)
		{
			AddItemToMaterialGroup(materialGroup, identifier, redraw);
		}
	}

	public static void AddItemToMouseOverGroup(string identifier)
	{
		AddItemToMaterialGroupByClassification(MaterialClassification.MouseOver, identifier);
	}

	public static void AddItemToSelectedGroup(string identifier)
	{
		AddItemToMaterialGroupByClassification(MaterialClassification.Selected, identifier);
		OnSelectionMaterialGroupChanged(null);
	}

	public static void AddRangeToMaterialGroup(CSMaterials.MaterialGroup materialGroup, IEnumerable<string> items)
	{
		foreach (string item in items)
		{
			if (materialGroup.MaterialGroupItems.All((MaterialGroupItem materialGroupItem) => materialGroupItem.Id != item))
			{
				AddItemToMaterialGroup(materialGroup, item, redraw: false);
			}
		}
		CSUtil.Redraw();
	}

	public static void AddRangeToMaterialGroup(CSMaterials.MaterialGroup materialGroup, IEnumerable<MaterialGroupItem> items)
	{
		foreach (MaterialGroupItem item in items)
		{
			if (materialGroup.MaterialGroupItems.All((MaterialGroupItem materialGroupItem) => !item.IsInherited && materialGroupItem.Id != item.Id))
			{
				materialGroup.MaterialGroupItems.Add(item);
			}
		}
		CSUtil.Redraw();
	}

	public static void AddRangeToMaterialGroupByClassification(MaterialClassification materialClassification, IEnumerable<string> identifiers)
	{
		CSMaterials.MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup group) => group.Classification == materialClassification);
		if (materialGroup != null)
		{
			AddRangeToMaterialGroup(materialGroup, identifiers);
		}
	}

	public static void AddRangeToMaterialGroupByClassification(MaterialClassification materialClassification, ObservableCollection<MaterialGroupItem> materialGroupItems)
	{
		CSMaterials.MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup group) => group.Classification == materialClassification);
		if (materialGroup != null)
		{
			AddRangeToMaterialGroup(materialGroup, materialGroupItems);
		}
	}

	public static void AddSelectedGroupItemsToMaterialGroup(CSMaterials.MaterialGroup materialGroup)
	{
		CSMaterials.MaterialGroup materialGroup2 = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.Selected).FirstOrDefault();
		if (materialGroup2 == null)
		{
			return;
		}
		if (materialGroup.IsDefaultMaterialGroup)
		{
			if (materialGroup.IsGemMaterialGroup)
			{
				RemoveItemsFromGemDefaultMaterialGroups(materialGroup2.MaterialGroupItems);
			}
			else if (materialGroup.IsMetalMaterialGroup)
			{
				RemoveItemsFromMetalDefaultMaterialGroups(materialGroup2.MaterialGroupItems);
			}
		}
		else if (materialGroup.IsDesignerMaterialGroup)
		{
			if (materialGroup.IsGemMaterialGroup)
			{
				RemoveItemsFromGemDesignerMaterialGroups(materialGroup2.MaterialGroupItems);
			}
			else if (materialGroup.IsMetalMaterialGroup)
			{
				RemoveItemsFromMetalDesignerMaterialGroups(materialGroup2.MaterialGroupItems);
			}
		}
		else if (materialGroup.IsUserMaterialGroup && materialGroup.IsGemMaterialGroup)
		{
			RemoveItemsFromGemUserMaterialGroups(materialGroup2.MaterialGroupItems);
		}
		AddRangeToMaterialGroup(materialGroup, materialGroup2.MaterialGroupItems);
	}

	public static bool AssignMaterialToMaterialGroup(CSMaterials.MaterialGroup materialGroup, string materialName)
	{
		bool result = false;
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup group) => materialGroup.Id == group.Id) != null)
		{
			materialGroup.MaterialName = materialName;
			result = true;
		}
		CSUtil.Redraw();
		return result;
	}

	public static bool CheckMaterialGroupForItem(CSMaterials.MaterialGroup materialGroup, string identifier)
	{
		return materialGroup?.MaterialGroupItems.Any((MaterialGroupItem materialGroupItem) => materialGroupItem.Id == identifier) ?? false;
	}

	public static bool CheckMaterialGroupForItemByClassification(MaterialClassification materialClassification, string identifier)
	{
		CSMaterials.MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup group) => group.Classification == materialClassification);
		if (materialGroup != null)
		{
			return CheckMaterialGroupForItem(materialGroup, identifier);
		}
		return false;
	}

	public static bool CheckMouseOverGroupForItem(string identifier)
	{
		return CheckMaterialGroupForItemByClassification(MaterialClassification.MouseOver, identifier);
	}

	public static bool CheckSelectedGroupForItem(string identifier)
	{
		return CheckMaterialGroupForItemByClassification(MaterialClassification.Selected, identifier);
	}

	public static void ClearGroupItemsForMaterialGroup(CSMaterials.MaterialGroup materialGroup)
	{
		materialGroup.MaterialGroupItems.Clear();
		if (materialGroup.Classification != MaterialClassification.Highlighted && materialGroup.Classification != MaterialClassification.Selected)
		{
			_ = materialGroup.Classification;
			_ = 16;
		}
		CSUtil.Redraw();
	}

	public static void ClearGroupItemsForMaterialGroupByClassification(MaterialClassification materialClassification)
	{
		CSMaterials.MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup group) => group.Classification == materialClassification);
		if (materialGroup != null)
		{
			ClearGroupItemsForMaterialGroup(materialGroup);
		}
	}

	public static void ClearHighlightGroupItems()
	{
		ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.Highlighted);
	}

	public static void ClearMouseOverGroupItems()
	{
		ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.MouseOver);
	}

	public static void ClearSelectedGroupItems()
	{
		ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.Selected);
	}

	public static void CreateDefaultMaterialGroups()
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups = new List<CSMaterials.MaterialGroup>
		{
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.GemCenter,
				Name = "Default - GemCenter",
				MaterialName = Enum.GetName(typeof(GemMaterial), GemMaterial.Diamond)
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.GemSide,
				Name = "Default - GemSide",
				MaterialName = Enum.GetName(typeof(GemMaterial), GemMaterial.SapphireBlue)
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.GemAccent,
				Name = "Default - GemAccent",
				MaterialName = Enum.GetName(typeof(GemMaterial), GemMaterial.Ruby)
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.MetalPrimary,
				Name = "Default - MetalPrimary",
				MaterialName = Enum.GetName(typeof(MetalMaterial), MetalMaterial.YellowGold)
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.MetalSecondary,
				Name = "Default - MetalSecondary",
				MaterialName = Enum.GetName(typeof(MetalMaterial), MetalMaterial.WhiteGold)
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Finger,
				Name = "Misc - Finger",
				MaterialName = "Finger"
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Cutting,
				Name = "Misc - Cutting",
				MaterialName = "Cutting"
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Creation,
				Name = "Misc - Creation",
				MaterialName = "Creation"
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.MouseOver,
				Name = "Selection - MouseOver",
				MaterialName = "MouseOver"
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Selected,
				Name = "Selection - Selected",
				MaterialName = "Selected"
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Highlighted,
				Name = "Selection - Highlighted",
				MaterialName = "Highlighted"
			},
			new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Undefined,
				Name = "Undefined",
				MaterialName = "Undefined"
			}
		};
	}

	public static void DuplicateMaterialGroupAssignments(CSBlock sourceBlock, CSBlock destinationBlock)
	{
		DuplicateMaterialGroupAssignmentsForIdentifier($"{sourceBlock.InstanceGuid}", $"{destinationBlock.InstanceGuid}");
		List<CSData> outputChannelDatas = destinationBlock.OutputChannelDatas;
		for (int i = 0; i < outputChannelDatas.Count; i++)
		{
			DuplicateMaterialGroupAssignmentsForIdentifier($"{sourceBlock.InstanceGuid},{i}", $"{destinationBlock.InstanceGuid},{i}");
			if (outputChannelDatas[i] is ICSArray iCSArray)
			{
				CSDataArray flattenArray = iCSArray.GetFlattenArray();
				for (int j = 0; j < flattenArray.GetArrayCount(); j++)
				{
					DuplicateMaterialGroupAssignmentsForIdentifier($"{sourceBlock.InstanceGuid},{i},{j}", $"{destinationBlock.InstanceGuid},{i},{j}");
				}
			}
		}
	}

	public static string GetAlphaIdentifierForMaterialGroup(CSMaterials.MaterialGroup materialGroup)
	{
		_MaterialGroupAlphaIdentifiers.TryGetValue(materialGroup.Id, out var value);
		return value;
	}

	public static CSMaterials.MaterialGroup GetAssignedMaterialGroup(string materialGroupItemIdentifier)
	{
		List<CSMaterials.MaterialGroup> source = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup8) => materialGroup8.MaterialGroupItems.Any((MaterialGroupItem materialGroupItem) => materialGroupItem.Id == materialGroupItemIdentifier && !materialGroupItem.IsInherited)).ToList();
		if (!IsVerificationMaterialGroupEnabled)
		{
			CSMaterials.MaterialGroup materialGroup = source.FirstOrDefault((CSMaterials.MaterialGroup group) => group.IsUserMaterialGroup);
			if (materialGroup != null)
			{
				return materialGroup;
			}
		}
		if (!IsVerificationMaterialGroupEnabled)
		{
			CSMaterials.MaterialGroup materialGroup2 = source.FirstOrDefault((CSMaterials.MaterialGroup group) => group.IsUserMaterialGroup && group.IsGemMaterialGroup);
			if (materialGroup2 != null)
			{
				return materialGroup2;
			}
		}
		CSMaterials.MaterialGroup materialGroup3 = source.FirstOrDefault((CSMaterials.MaterialGroup group) => group.IsDesignerMaterialGroup && group.IsMetalMaterialGroup);
		if (materialGroup3 != null)
		{
			return materialGroup3;
		}
		if (!IsVerificationMaterialGroupEnabled && !CSApplicationHelper.IsCounterSketchStudio)
		{
			CSMaterials.MaterialGroup materialGroup4 = source.FirstOrDefault((CSMaterials.MaterialGroup group) => group.IsDesignerMaterialGroup && group.IsGemMaterialGroup);
			if (materialGroup4 != null)
			{
				return materialGroup4;
			}
		}
		CSMaterials.MaterialGroup materialGroup5 = source.FirstOrDefault((CSMaterials.MaterialGroup group) => group.IsDefaultMaterialGroup && group.IsMetalMaterialGroup);
		if (materialGroup5 != null)
		{
			return materialGroup5;
		}
		CSMaterials.MaterialGroup materialGroup6 = source.FirstOrDefault((CSMaterials.MaterialGroup group) => group.IsDefaultMaterialGroup && group.IsGemMaterialGroup);
		if (materialGroup6 != null)
		{
			return materialGroup6;
		}
		CSMaterials.MaterialGroup materialGroup7 = source.FirstOrDefault((CSMaterials.MaterialGroup group) => group.IsConstructionGroup);
		if (materialGroup7 != null)
		{
			return materialGroup7;
		}
		return null;
	}

	public static MaterialClassification GetMaterialClassifictionByLayer(LayerNumber whichLayer)
	{
		MaterialClassification result = MaterialClassification.Undefined;
		switch (whichLayer)
		{
		case LayerNumber.Metal01:
			result = MaterialClassification.MetalPrimary;
			break;
		case LayerNumber.Metal02:
			result = MaterialClassification.MetalSecondary;
			break;
		case LayerNumber.Gem01:
			result = MaterialClassification.GemCenter;
			break;
		case LayerNumber.Gem02:
			result = MaterialClassification.GemSide;
			break;
		case LayerNumber.Gem03:
			result = MaterialClassification.GemAccent;
			break;
		case LayerNumber.Finger:
			result = MaterialClassification.Finger;
			break;
		case LayerNumber.Cutting:
			result = MaterialClassification.Cutting;
			break;
		case LayerNumber.Creation:
			result = MaterialClassification.Creation;
			break;
		}
		return result;
	}

	public static IEnumerable<CSMaterials.MaterialGroup> GetMaterialGroups()
	{
		return CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups;
	}

	public static IEnumerable<CSMaterials.MaterialGroup> GetMaterialGroupsForIdentifier(string materialGroupItemIdentifier, bool includedInherited = true)
	{
		return CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup) => materialGroup.MaterialGroupItems.Any((MaterialGroupItem materialGroupItem) => materialGroupItem.Id == materialGroupItemIdentifier && materialGroupItem.IsInherited == includedInherited));
	}

	public static CSMaterials.MaterialGroup GetMouseOverMaterialGroup()
	{
		return CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MouseOver).FirstOrDefault();
	}

	public static CSMaterials.MaterialGroup GetSelectionMaterialGroup()
	{
		return CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.Selected).FirstOrDefault();
	}

	public static bool ItemInAnyMateralGroups(CSBlock csBlock)
	{
		bool found = false;
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			if (materialGroup.MaterialGroupItems.Where((MaterialGroupItem materialGroupItem) => materialGroupItem.Id.Split(':').FirstOrDefault() == csBlock.InstanceGuid.ToString()).Any())
			{
				found = true;
			}
		});
		return found;
	}

	public static void LoadImportedMaterialGroups(CSUserData userData)
	{
		string text = userData.FindValue("Materials", "MaterialGroups");
		if (!string.IsNullOrWhiteSpace(text))
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CSMaterials.MaterialGroup>));
				StringReader textReader = new StringReader(text);
				if (xmlSerializer.Deserialize(textReader) is List<CSMaterials.MaterialGroup> collection)
				{
					ImportedDocumentMaterialGroups = new List<CSMaterials.MaterialGroup>(collection);
					ImportedDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification == MaterialClassification.Selected || materialGroup.Classification == MaterialClassification.MouseOver || materialGroup.Classification == MaterialClassification.Highlighted || materialGroup.Classification == MaterialClassification.Undefined).ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
					{
						materialGroup.MaterialGroupItems.Clear();
					});
				}
			}
			catch (Exception)
			{
			}
		}
		_originalMaterialGroups = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups;
	}

	public static void LoadMaterialGroups(CSUserData userData)
	{
		string text = userData.FindValue("Materials", "MaterialGroups");
		if (!string.IsNullOrWhiteSpace(text))
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CSMaterials.MaterialGroup>));
				StringReader textReader = new StringReader(text);
				if (xmlSerializer.Deserialize(textReader) is List<CSMaterials.MaterialGroup> collection)
				{
					CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups = new List<CSMaterials.MaterialGroup>(collection);
					UpdateUtilityMaterialGroups();
				}
			}
			catch (Exception)
			{
			}
		}
		else
		{
			IsGenerateMaterialGroupsFromGraphNeeded = true;
			CreateDefaultMaterialGroups();
		}
		_originalMaterialGroups = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups;
	}

	public static LayerNumber LookupLayerForMaterialClassification(MaterialClassification materialGroupClassification)
	{
		LayerNumber result = LayerNumber.User01;
		switch (materialGroupClassification)
		{
		case MaterialClassification.MetalPrimary:
			result = LayerNumber.Metal01;
			break;
		case MaterialClassification.MetalSecondary:
			result = LayerNumber.Metal02;
			break;
		case MaterialClassification.GemCenter:
			result = LayerNumber.Gem01;
			break;
		case MaterialClassification.GemSide:
			result = LayerNumber.Gem02;
			break;
		case MaterialClassification.GemAccent:
			result = LayerNumber.Gem03;
			break;
		case MaterialClassification.Finger:
			result = LayerNumber.Finger;
			break;
		case MaterialClassification.Cutting:
			result = LayerNumber.Cutting;
			break;
		case MaterialClassification.Creation:
			result = LayerNumber.Creation;
			break;
		}
		return result;
	}

	public static void ReadJson(JToken jSon)
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups = jSon.ToObject<List<CSMaterials.MaterialGroup>>();
		UpdateUtilityMaterialGroups();
		_originalMaterialGroups = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups;
	}

	public static void RemoveItemFromAllMaterialGroups(CSBlock csBlock, bool redraw = true)
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			materialGroup.MaterialGroupItems.Where((MaterialGroupItem materialGroupItem) => materialGroupItem.Id.Split(':').FirstOrDefault() == csBlock.InstanceGuid.ToString()).ToList().ForEach(delegate(MaterialGroupItem itemToRemove)
			{
				materialGroup.MaterialGroupItems.Remove(itemToRemove);
			});
		});
		if (redraw)
		{
			CSUtil.Redraw();
		}
	}

	public static void RemoveItemFromMaterialGroup(CSMaterials.MaterialGroup materialGroup, string identifier, bool includeRelatedIdentifiers = false)
	{
		if (materialGroup == null)
		{
			return;
		}
		if (!includeRelatedIdentifiers)
		{
			MaterialGroupItem materialGroupItem = materialGroup.MaterialGroupItems.FirstOrDefault((MaterialGroupItem groupItem) => groupItem.Id == identifier);
			if (materialGroupItem != null)
			{
				materialGroup.MaterialGroupItems.Remove(materialGroupItem);
			}
		}
		else
		{
			materialGroup.MaterialGroupItems.Where((MaterialGroupItem groupItem) => groupItem.Id.StartsWith(identifier)).ToList().ForEach(delegate(MaterialGroupItem item)
			{
				materialGroup.MaterialGroupItems.Remove(item);
			});
		}
		CSUtil.Redraw();
	}

	public static void RemoveItemFromMaterialGroupByClassification(MaterialClassification materialClassification, string identifier, bool includeRelatedIdentifiers = false)
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup group) => group.Classification == materialClassification).ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			RemoveItemFromMaterialGroup(materialGroup, identifier, includeRelatedIdentifiers);
		});
	}

	public static void RemoveItemFromMaterialGroupByLayer(CSBlock csBlock, bool redraw = true)
	{
		MaterialClassification outputDisplayMaterialClassification = csBlock.OutputDisplayMaterialClassification;
		if ((uint)(outputDisplayMaterialClassification - 2) <= 4u || (uint)(outputDisplayMaterialClassification - 19) <= 2u)
		{
			RemoveItemFromMaterialGroupByClassification(csBlock.OutputDisplayMaterialClassification, csBlock.InstanceGuid.ToString(), redraw);
		}
	}

	public static void RemoveItemFromSelectedGroup(string identifier, bool includeRelatedIdentifiers = false)
	{
		RemoveItemFromMaterialGroupByClassification(MaterialClassification.Selected, identifier, includeRelatedIdentifiers);
		OnSelectionMaterialGroupChanged(null);
	}

	public static void RemoveItemsFromGemDefaultMaterialGroups(ObservableCollection<MaterialGroupItem> materialGroupItems, bool includeRelatedIdentifiers = false)
	{
		foreach (MaterialGroupItem item in materialGroupItems.ToList())
		{
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.GemAccent, item.Id, includeRelatedIdentifiers);
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.GemSide, item.Id, includeRelatedIdentifiers);
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.GemCenter, item.Id, includeRelatedIdentifiers);
		}
	}

	public static void RemoveItemsFromGemDesignerMaterialGroups(ObservableCollection<MaterialGroupItem> materialGroupItems, bool includeRelatedIdentifiers = false)
	{
		foreach (MaterialGroupItem item in materialGroupItems.ToList())
		{
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.GemDesigner, item.Id, includeRelatedIdentifiers);
		}
	}

	public static void RemoveItemsFromGemUserMaterialGroups(ObservableCollection<MaterialGroupItem> materialGroupItems, bool includeRelatedIdentifiers = false)
	{
		foreach (MaterialGroupItem item in materialGroupItems.ToList())
		{
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.GemUser, item.Id, includeRelatedIdentifiers);
		}
	}

	public static void RemoveItemsFromMetalDefaultMaterialGroups(ObservableCollection<MaterialGroupItem> materialGroupItems, bool includeRelatedIdentifiers = false)
	{
		foreach (MaterialGroupItem item in materialGroupItems.ToList())
		{
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.MetalPrimary, item.Id, includeRelatedIdentifiers);
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.MetalSecondary, item.Id, includeRelatedIdentifiers);
		}
	}

	public static void RemoveItemsFromMetalDesignerMaterialGroups(ObservableCollection<MaterialGroupItem> materialGroupItems, bool includeRelatedIdentifiers = false)
	{
		foreach (MaterialGroupItem item in materialGroupItems.ToList())
		{
			RemoveItemFromMaterialGroupByClassification(MaterialClassification.MetalDesigner, item.Id, includeRelatedIdentifiers);
		}
	}

	public static void RemoveRangeFromMaterialGroupByClassification(MaterialClassification classification, IEnumerable<MaterialGroupItem> materialGroupItems)
	{
		CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(classification).ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			foreach (MaterialGroupItem item in materialGroupItems.ToList())
			{
				materialGroup.MaterialGroupItems.Remove(item);
			}
		});
		CSUtil.Redraw();
	}

	public static void ReplaceEditableMaterialGroups(List<CSMaterials.MaterialGroup> materialGroups)
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup) => materialGroups.Any((CSMaterials.MaterialGroup group) => group.IsEditableMaterialGroup)).ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Remove(materialGroup);
		});
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.AddRange(materialGroups);
		UpdateDesignerMaterialGroupGemDisplayNumbers();
		CSUtil.Redraw();
	}

	public static void ResetDefaultMaterialGroups()
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Clear();
		CreateDefaultMaterialGroups();
		CSApplicationHelper.Application.Document.Graph.DisplayObjects().ToList().ForEach(delegate(CSBlock csBlock)
		{
			foreach (CSData outputChannelData in csBlock.OutputChannelDatas)
			{
				if (outputChannelData != null)
				{
					if (outputChannelData is CSDataArray cSDataArray)
					{
						foreach (CSData item in cSDataArray.GetFlattenArray().Array)
						{
							item.CurrentMaterialGroup = null;
						}
					}
					else
					{
						outputChannelData.CurrentMaterialGroup = null;
					}
				}
			}
		});
		CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<CSBlock>().ToList()
			.ForEach(delegate(CSBlock csBlock)
			{
				AddBlockToDefaultMaterialGroups(csBlock, redraw: false);
			});
		if (IsVerificationMaterialGroupEnabled)
		{
			SetBackupMaterialGroups();
			SetVerificationMaterialsToDefaultMaterialGroups();
		}
		CSUtil.ForceGraphToRedraw();
		UpdateGemDisplayNumbers();
	}

	public static void RestoreBackupMaterialGroups()
	{
		if (_originalMaterialGroups != null && _originalMaterialGroups.Any())
		{
			ReplaceEditableMaterialGroups(_originalMaterialGroups);
			if (IsVerificationMaterialGroupEnabled)
			{
				IsVerificationMaterialGroupEnabled = !IsVerificationMaterialGroupEnabled;
			}
			UpdateGemDisplayNumbers();
		}
	}

	public static void SaveMaterialGroups(CSUserData userData)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CSMaterials.MaterialGroup>));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups);
			userData.AddKey("Materials", "MaterialGroups", stringWriter.ToString());
		}
		catch (Exception)
		{
		}
	}

	public static void SetBackupMaterialGroups()
	{
		_originalMaterialGroups = new List<CSMaterials.MaterialGroup>(CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Select((CSMaterials.MaterialGroup materialGroup) => materialGroup.Clone() as CSMaterials.MaterialGroup));
	}

	public static void SetDesignerMaterialsToMaterialGroups()
	{
		IsVerificationMaterialGroupEnabled = false;
		if (_originalMaterialGroups != null && _originalMaterialGroups.Any())
		{
			CSMaterials.MaterialGroup materialGroup = _originalMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup materialGroup6) => materialGroup6.Classification == MaterialClassification.GemCenter);
			if (materialGroup != null)
			{
				AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault(), (materialGroup.MaterialName != "Verification_GemCenter") ? materialGroup.MaterialName : Enum.GetName(typeof(GemMaterial), GemMaterial.Diamond));
			}
			CSMaterials.MaterialGroup materialGroup2 = _originalMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup materialGroup6) => materialGroup6.Classification == MaterialClassification.GemSide);
			if (materialGroup2 != null)
			{
				AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemSide).FirstOrDefault(), (materialGroup2.MaterialName != "Verification_GemSide") ? materialGroup2.MaterialName : Enum.GetName(typeof(GemMaterial), GemMaterial.SapphireBlue));
			}
			CSMaterials.MaterialGroup materialGroup3 = _originalMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup materialGroup6) => materialGroup6.Classification == MaterialClassification.GemAccent);
			if (materialGroup3 != null)
			{
				AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemAccent).FirstOrDefault(), (materialGroup3.MaterialName != "Verification_GemAccent") ? materialGroup3.MaterialName : Enum.GetName(typeof(GemMaterial), GemMaterial.Ruby));
			}
			CSMaterials.MaterialGroup materialGroup4 = _originalMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup materialGroup6) => materialGroup6.Classification == MaterialClassification.MetalPrimary);
			if (materialGroup4 != null)
			{
				AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalPrimary).FirstOrDefault(), (materialGroup4.MaterialName != "Verification_MetalPrimary") ? materialGroup4.MaterialName : Enum.GetName(typeof(MetalMaterial), MetalMaterial.YellowGold));
			}
			CSMaterials.MaterialGroup materialGroup5 = _originalMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup materialGroup6) => materialGroup6.Classification == MaterialClassification.MetalSecondary);
			if (materialGroup5 != null)
			{
				AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalSecondary).FirstOrDefault(), (materialGroup5.MaterialName != "Verification_MetalSecondary") ? materialGroup5.MaterialName : Enum.GetName(typeof(MetalMaterial), MetalMaterial.WhiteGold));
			}
		}
		else
		{
			AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault(), Enum.GetName(typeof(GemMaterial), GemMaterial.Diamond));
			AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemSide).FirstOrDefault(), Enum.GetName(typeof(GemMaterial), GemMaterial.SapphireBlue));
			AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemAccent).FirstOrDefault(), Enum.GetName(typeof(GemMaterial), GemMaterial.Ruby));
			AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalPrimary).FirstOrDefault(), Enum.GetName(typeof(MetalMaterial), MetalMaterial.YellowGold));
			AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalSecondary).FirstOrDefault(), Enum.GetName(typeof(MetalMaterial), MetalMaterial.WhiteGold));
		}
	}

	public static void SetVerificationMaterialsToDefaultMaterialGroups()
	{
		IsVerificationMaterialGroupEnabled = true;
		AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault(), "Verification_GemCenter");
		AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemSide).FirstOrDefault(), "Verification_GemSide");
		AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemAccent).FirstOrDefault(), "Verification_GemAccent");
		AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalPrimary).FirstOrDefault(), "Verification_MetalPrimary");
		AssignMaterialToMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalSecondary).FirstOrDefault(), "Verification_MetalSecondary");
	}

	public static void UpdateDesignerMaterialGroupGemDisplayNumbers()
	{
		UpdateMaterialGroupAlphaIdentifiers();
		(from materialGroup in GetMaterialGroups()
			where materialGroup.IsGemMaterialGroup && materialGroup.IsDesignerMaterialGroup
			select materialGroup).ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			materialGroup.MaterialGroupItems.Aggregate(1, (int current, MaterialGroupItem materialGroupItem) => UpdateGemNumber(materialGroupItem, current, materialGroup));
		});
	}

	public static void UpdateGemDisplayNumbers(bool isRedrawNeeded = true)
	{
		UpdateDesignerMaterialGroupGemDisplayNumbers();
		UpdateDefaultMaterialGroupGemNumbers();
		if (isRedrawNeeded)
		{
			CSUtil.Redraw();
		}
	}

	public static void UpdateTryOnMaterialGroups()
	{
		if (!CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnHand).Any())
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.AddRange(new List<CSMaterials.MaterialGroup>
			{
				new CSMaterials.MaterialGroup
				{
					Classification = MaterialClassification.TryOnHand,
					Name = "TryOn - Hand",
					MaterialName = ""
				},
				new CSMaterials.MaterialGroup
				{
					Classification = MaterialClassification.TryOnFingernails,
					Name = "TryOn - Fingernails",
					MaterialName = ""
				},
				new CSMaterials.MaterialGroup
				{
					Classification = MaterialClassification.TryOnShirt,
					Name = "TryOn - Shirt",
					MaterialName = ""
				}
			});
		}
		else
		{
			ClearGroupItemsForMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnHand));
			ClearGroupItemsForMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnFingernails));
			ClearGroupItemsForMaterialGroup(CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnShirt));
		}
		if (CSApplicationHelper.Application.Conduit._handObjects != null)
		{
			AddItemToMaterialGroupByClassification(MaterialClassification.TryOnHand, CSApplicationHelper.Application.Conduit._handObjects.InstanceGuid.ToString());
		}
		if (CSApplicationHelper.Application.Conduit._fingernailObjects != null)
		{
			AddItemToMaterialGroupByClassification(MaterialClassification.TryOnFingernails, CSApplicationHelper.Application.Conduit._fingernailObjects.InstanceGuid.ToString());
		}
		if (CSApplicationHelper.Application.Conduit._shirtObjects != null)
		{
			AddItemToMaterialGroupByClassification(MaterialClassification.TryOnShirt, CSApplicationHelper.Application.Conduit._shirtObjects.InstanceGuid.ToString());
		}
	}

	public static JArray WriteJson()
	{
		return JArray.FromObject(CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups);
	}

	private static void ClearGroupItemsForMaterialGroup(ObservableCollection<CSMaterials.MaterialGroup> materialGroups)
	{
		materialGroups.ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			materialGroup.MaterialGroupItems.Clear();
		});
		CSUtil.Redraw();
	}

	private static string ConvertNumberToAlpha(int numberToConvert)
	{
		string text = "";
		decimal num = numberToConvert;
		while (num > 0m)
		{
			decimal num2 = (num - 1m) % 26m;
			text = (char)(num2 + 65m) + text;
			num = (num - (num2 + 1m)) / 26m;
		}
		return text;
	}

	private static void DuplicateMaterialGroupAssignmentsForIdentifier(string originalIdentifier, string identifier)
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup) => materialGroup.IsEditableMaterialGroup || materialGroup.IsConstructionGroup).ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			if (materialGroup.MaterialGroupItems.Any((MaterialGroupItem materialGroupItem) => materialGroupItem.Id == originalIdentifier))
			{
				AddItemToMaterialGroup(materialGroup, identifier, redraw: false);
			}
		});
	}

	private static void OnSelectionMaterialGroupChanged(EventArgs eventArgs)
	{
		if (MaterialGroupHelper.SelectionMaterialGroupChanged != null)
		{
			MaterialGroupHelper.SelectionMaterialGroupChanged(null, eventArgs);
		}
	}

	private static void UpdateDefaultMaterialGroupGemNumbers()
	{
		List<string> exclusionIdentifiers = (from designerMaterialGroup in GetMaterialGroups()
			where designerMaterialGroup.IsGemMaterialGroup && designerMaterialGroup.IsDesignerMaterialGroup
			select designerMaterialGroup).SelectMany((CSMaterials.MaterialGroup group) => group.MaterialGroupItems.Select((MaterialGroupItem materialGroupItem) => materialGroupItem.Id)).ToList();
		(from defaultMaterialGroup in GetMaterialGroups()
			where defaultMaterialGroup.IsGemMaterialGroup && defaultMaterialGroup.IsDefaultMaterialGroup
			select defaultMaterialGroup).ToList().ForEach(delegate(CSMaterials.MaterialGroup defaultMaterialGroup)
		{
			defaultMaterialGroup.MaterialGroupItems.Aggregate(1, (int current, MaterialGroupItem materialGroupItem) => UpdateGemNumber(materialGroupItem, current, defaultMaterialGroup, exclusionIdentifiers));
		});
	}

	private static int UpdateGemNumber(MaterialGroupItem materialGroupItem, int gemNumber, CSMaterials.MaterialGroup materialGroup, List<string> exclusionIdentifiers = null)
	{
		if (exclusionIdentifiers == null)
		{
			exclusionIdentifiers = new List<string>();
		}
		string instanceGuid = materialGroupItem.Id;
		int num = instanceGuid.Split(':').Count();
		if (!string.IsNullOrWhiteSpace(instanceGuid) && !exclusionIdentifiers.Any((string exclusionIdentifier) => instanceGuid.StartsWith(exclusionIdentifier)))
		{
			CSBlock cSBlock = CSApplicationHelper.Application.Document.Graph.DisplayObjects().FirstOrDefault((CSBlock block) => block.InstanceGuid.ToString() == instanceGuid.Split(':').FirstOrDefault());
			if (cSBlock != null)
			{
				switch (num)
				{
				case 1:
				{
					if (exclusionIdentifiers.Contains(instanceGuid))
					{
						break;
					}
					List<CSData> outputChannelDatas = cSBlock.OutputChannelDatas;
					if (materialGroupItem.IsReversed)
					{
						outputChannelDatas.Reverse();
					}
					for (int num3 = 0; num3 < outputChannelDatas.Count; num3++)
					{
						string text2 = $"{instanceGuid}:{num3}";
						if (exclusionIdentifiers.Contains(text2) || GetMaterialGroupsForIdentifier(text2).Any((CSMaterials.MaterialGroup otherMaterialGroup) => otherMaterialGroup.Classification == materialGroup.Classification && materialGroup.Id != otherMaterialGroup.Id))
						{
							continue;
						}
						CSData cSData3 = outputChannelDatas[num3];
						if (cSData3 == null)
						{
							continue;
						}
						if (cSData3 is CSDataArray cSDataArray3)
						{
							CSDataArray flattenArray3 = cSDataArray3.GetFlattenArray();
							List<CSData> array2 = flattenArray3.Array;
							if (materialGroupItem.IsReversed)
							{
								array2.Reverse();
							}
							for (int num4 = 0; num4 < array2.Count; num4++)
							{
								string text3 = $"{instanceGuid}:{num3}:{num4}";
								if (!exclusionIdentifiers.Contains(text3) && !GetMaterialGroupsForIdentifier(text3).Any((CSMaterials.MaterialGroup otherMaterialGroup) => otherMaterialGroup.Classification == materialGroup.Classification && materialGroup.Id != otherMaterialGroup.Id))
								{
									CSData cSData4 = array2[num4];
									if (cSData4 != null && cSData4 is CSGemData cSGemData5)
									{
										cSGemData5.GemNumber = $"{GetAlphaIdentifierForMaterialGroup(materialGroup)}:{gemNumber++}";
										cSGemData5.GemGroupName = materialGroup.Name;
									}
								}
							}
						}
						else if (cSData3 is CSGemData cSGemData6)
						{
							cSGemData6.GemNumber = $"{GetAlphaIdentifierForMaterialGroup(materialGroup)}:{gemNumber++}";
							cSGemData6.GemGroupName = materialGroup.Name;
						}
					}
					break;
				}
				case 2:
				{
					if (exclusionIdentifiers.Contains(instanceGuid))
					{
						break;
					}
					int result3 = -1;
					if (!int.TryParse(instanceGuid.Split(':')[1], out result3) || result3 < 0 || exclusionIdentifiers.Contains($"{instanceGuid}:{result3}") || result3 >= cSBlock.OutputChannelCount)
					{
						break;
					}
					CSData outputChannelData2 = cSBlock.GetOutputChannelData(result3);
					if (outputChannelData2 == null)
					{
						break;
					}
					if (outputChannelData2 is CSDataArray cSDataArray2)
					{
						CSDataArray flattenArray2 = cSDataArray2.GetFlattenArray();
						List<CSData> array = flattenArray2.Array;
						if (materialGroupItem.IsReversed)
						{
							array.Reverse();
						}
						for (int num2 = 0; num2 < array.Count; num2++)
						{
							string text = $"{instanceGuid}:{result3}:{num2}";
							if (!exclusionIdentifiers.Contains(text) && !GetMaterialGroupsForIdentifier(text).Any((CSMaterials.MaterialGroup otherMaterialGroup) => otherMaterialGroup.Classification == materialGroup.Classification && materialGroup.Id != otherMaterialGroup.Id))
							{
								CSData cSData2 = array[num2];
								if (cSData2 != null && cSData2 is CSGemData cSGemData3)
								{
									cSGemData3.GemNumber = $"{GetAlphaIdentifierForMaterialGroup(materialGroup)}:{gemNumber++}";
									cSGemData3.GemGroupName = materialGroup.Name;
								}
							}
						}
					}
					else if (outputChannelData2 is CSGemData cSGemData4)
					{
						cSGemData4.GemNumber = $"{GetAlphaIdentifierForMaterialGroup(materialGroup)}:{gemNumber++}";
						cSGemData4.GemGroupName = materialGroup.Name;
					}
					break;
				}
				case 3:
				{
					if (exclusionIdentifiers.Contains(instanceGuid))
					{
						break;
					}
					int result = -1;
					if (!int.TryParse(instanceGuid.Split(':')[1], out result) || result < 0)
					{
						break;
					}
					int result2 = -1;
					if (!int.TryParse(instanceGuid.Split(':')[2], out result2) || result2 < 0 || exclusionIdentifiers.Contains($"{instanceGuid}:{result}") || result >= cSBlock.OutputChannelCount)
					{
						break;
					}
					CSData outputChannelData = cSBlock.GetOutputChannelData(result);
					if (outputChannelData == null)
					{
						break;
					}
					if (outputChannelData is CSDataArray cSDataArray)
					{
						if (exclusionIdentifiers.Contains($"{instanceGuid}:{result}:{result2}"))
						{
							break;
						}
						CSDataArray flattenArray = cSDataArray.GetFlattenArray();
						if (result2 < flattenArray.Array.Count())
						{
							CSData cSData = flattenArray.Array[result2];
							if (cSData != null && cSData is CSGemData cSGemData)
							{
								cSGemData.GemNumber = $"{GetAlphaIdentifierForMaterialGroup(materialGroup)}:{gemNumber++}";
								cSGemData.GemGroupName = materialGroup.Name;
							}
						}
					}
					else if (outputChannelData is CSGemData cSGemData2)
					{
						cSGemData2.GemNumber = $"{GetAlphaIdentifierForMaterialGroup(materialGroup)}:{gemNumber++}";
						cSGemData2.GemGroupName = materialGroup.Name;
					}
					break;
				}
				}
			}
		}
		return gemNumber;
	}

	private static void UpdateMaterialGroupAlphaIdentifiers()
	{
		List<CSMaterials.MaterialGroup> gemGroups = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup) => materialGroup.IsGemMaterialGroup && materialGroup.MaterialGroupItems.Any()).ToList();
		_MaterialGroupAlphaIdentifiers.Clear();
		gemGroups.ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			_MaterialGroupAlphaIdentifiers.Add(materialGroup.Id, ConvertNumberToAlpha(gemGroups.IndexOf(materialGroup) + 1));
		});
	}

	private static void UpdateUtilityMaterialGroups()
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification == MaterialClassification.Selected || materialGroup.Classification == MaterialClassification.MouseOver || materialGroup.Classification == MaterialClassification.Highlighted || materialGroup.Classification == MaterialClassification.Undefined).ToList().ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
		{
			ClearGroupItemsForMaterialGroupByClassification(materialGroup.Classification);
		});
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.All((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification != MaterialClassification.Finger))
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Finger,
				Name = "Misc - Finger",
				MaterialName = "Finger"
			});
		}
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.All((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification != MaterialClassification.Cutting))
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Cutting,
				Name = "Misc - Cutting",
				MaterialName = "Cutting"
			});
		}
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.All((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification != MaterialClassification.Creation))
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Creation,
				Name = "Misc - Creation",
				MaterialName = "Creation"
			});
		}
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.All((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification != MaterialClassification.MouseOver))
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.MouseOver,
				Name = "Selection - MouseOver",
				MaterialName = "MouseOver"
			});
		}
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.All((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification != MaterialClassification.Selected))
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Selected,
				Name = "Selection - Selected",
				MaterialName = "Selected"
			});
		}
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.All((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification != MaterialClassification.Highlighted))
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Highlighted,
				Name = "Selection - Highlighted",
				MaterialName = "Highlighted"
			});
		}
		if (CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.All((CSMaterials.MaterialGroup materialGroup) => materialGroup.Classification != MaterialClassification.Undefined))
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(new CSMaterials.MaterialGroup
			{
				Classification = MaterialClassification.Undefined,
				Name = "Undefined",
				MaterialName = "Undefined"
			});
		}
	}
}
