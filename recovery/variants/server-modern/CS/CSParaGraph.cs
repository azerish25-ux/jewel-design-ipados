using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues;
using CS.CSValues.Interfaces;
using CS.Iteration;
using CS.MaterialGroup;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Parametric.Interfaces;
using CS.Utilities.Helpers;
using CSGemInstance;
using CSMaterials;
using GV.CounterSketch.Shared.Model.Objects;
using GemvisionCommon.Utilities.Slack;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.Geometry;

namespace CS;

public class CSParaGraph
{
	private readonly ObservableCollection<CSObject> _unusedObjects = new ObservableCollection<CSObject>();

	public List<CSObject> AllObjectsList => new List<CSObject>(GraphList);

	public CSUICategoryTable CategoryTable { get; } = new CSUICategoryTable();

	public IEnumerable<CSBlock> CSBlocks => GraphList.OfType<CSBlock>();

	public bool GenerateDisabled { get; set; }

	public List<CSObject> GraphList { get; private set; } = new List<CSObject>();

	public bool MouseDown { get; set; }

	public int UpdateCount { get; private set; }

	public long GenerateTime { get; set; }

	public List<ImportGroup> ImportedCSGParts { get; set; } = new List<ImportGroup>();

	public bool IsConcurrentProcessingEnabled { get; set; }

	public bool IsPerformingGenerate { get; private set; }

	public IEnumerable<CSObject> ActiveObjects()
	{
		return from obj in AllObjects()
			where obj != null
			where !obj.Deleted
			select obj;
	}

	public IEnumerable<CSBlock> ActiveOutputObjects()
	{
		return ActiveObjects().OfType<CSBlock>();
	}

	public void Add(CSObject obj)
	{
		if (obj != null)
		{
			if (!IsValidBlockClass(obj, CSApplicationHelper.Application.BlockList))
			{
				RhUtil.RhinoMessageBox("Developer Error: Creating unsaveable block class:\n" + obj.GetType().FullName, "Error", 0u);
			}
			GraphList.Add(obj);
		}
	}

	public IEnumerable<CSObject> AllObjects()
	{
		return GraphList;
	}

	public void Clean()
	{
		foreach (CSObject graph in GraphList)
		{
			graph.CleanAfterTableChange(this);
		}
	}

	public void ClearIndexesOfGraphItems()
	{
		foreach (CSObject graph in GraphList)
		{
			graph.TableIndex = -1;
		}
	}

	public IEnumerable<CSBlock> DisplayObjects()
	{
		List<CSBlock> list = (from _ in GraphList.OfType<CSBlock>()
			where !_.Deleted && _.DisplayVisible
			select _).ToList();
		foreach (CSBlock csBlock in list)
		{
			bool flag = true;
			bool isImportVisible = true;
			if (csBlock.ImportGroup != null)
			{
				if (!CSApplicationHelper.Application.IsFreehandPartsVisible || !csBlock.ImportGroup.IsViewportVisible)
				{
					isImportVisible = false;
				}
			}
			else
			{
				isImportVisible = false;
			}
			CSPatchWireStart parentPatchWire = csBlock.ParentPatchWire;
			bool flag2;
			if (parentPatchWire != null)
			{
				flag2 = parentPatchWire.DisplayVisible;
				if (flag2 && parentPatchWire.ImportGroup == null && parentPatchWire.DisplayComponent != DisplayComponents.None && (CSApplicationHelper.Application.Document.CurrentDisplayComponents & parentPatchWire.DisplayComponent) == 0)
				{
					flag = false;
				}
			}
			else
			{
				flag2 = false;
			}
			if (flag2)
			{
				if (flag && csBlock.ImportGroup == null)
				{
					yield return csBlock;
				}
				if (isImportVisible)
				{
					yield return csBlock;
				}
				if (CSApplicationHelper.Application.Document.Graph.MouseDown && (csBlock.MouseDownObject || (csBlock.ForceVisible && isImportVisible)))
				{
					yield return csBlock;
				}
			}
		}
	}

	public void Generate(bool isQuick = false)
	{
		if (!GenerateDisabled)
		{
			ActivityHelper.DesignGenerating();
			IsPerformingGenerate = true;
			Stopwatch stopwatch = Stopwatch.StartNew();
			if (IsConcurrentProcessingEnabled)
			{
				GenerateConcurrent(isQuick);
			}
			else
			{
				GenerateSynchronous(isQuick);
			}
			stopwatch.Stop();
			GenerateTime = stopwatch.ElapsedMilliseconds;
			PrintGenerateTime(isQuick);
			IsPerformingGenerate = false;
			CSApplicationHelper.Application.RaiseGraphGenerateComplete();
		}
	}

	public void GenerateAll()
	{
		if (!GenerateDisabled)
		{
			ActivityHelper.DesignGenerating();
			IsPerformingGenerate = true;
			Stopwatch stopwatch = Stopwatch.StartNew();
			if (IsConcurrentProcessingEnabled)
			{
				GenerateConcurrentAll();
			}
			else
			{
				GenerateSynchronousAll();
			}
			stopwatch.Stop();
			GenerateTime = stopwatch.ElapsedMilliseconds;
			PrintGenerateTime(isQuick: false);
			IsPerformingGenerate = false;
			CSApplicationHelper.Application.RaiseGraphGenerateComplete();
		}
	}

	public static string GetSignature(out int blockLength)
	{
		return GetSignatureAtBlock(-1, out blockLength);
	}

	public static string GetSignatureAtBlock(int nStopAtBlock)
	{
		int blockLength;
		return GetSignatureAtBlock(nStopAtBlock, out blockLength);
	}

	public int GetUnusedObjects()
	{
		_unusedObjects.Clear();
		foreach (CSObject graph in GraphList)
		{
			if (graph is CSPatchWireStart { Deleted: false, OrphanedPatchwire: not false } cSPatchWireStart)
			{
				cSPatchWireStart.IsBlockDeletable = true;
				_unusedObjects.Add(graph);
			}
			if (!(graph is CSSuperViewGroup { Deleted: false } cSSuperViewGroup))
			{
				continue;
			}
			bool flag = true;
			for (int i = 0; i < cSSuperViewGroup.ChildCount(); i++)
			{
				if (cSSuperViewGroup.Child(i) is CSPatchWireStart cSPatchWireStart2)
				{
					flag = cSPatchWireStart2.Deleted || cSPatchWireStart2.OrphanedPatchwire;
				}
				else if (cSSuperViewGroup.Child(i) is CSTreeObject)
				{
					flag = cSSuperViewGroup.Child(i).Deleted;
				}
				if (!flag)
				{
					break;
				}
			}
			if (flag)
			{
				_unusedObjects.Add(graph);
			}
		}
		return _unusedObjects.Count;
	}

	public bool ImportReadGraphItems(OnBinaryArchive archive, List<CSBlockDescription> blockList, string importFileName = "", bool isFreehandImport = false, CSUICategoryTable csuiCategoryTable = null, CSUserData documentUserData = null)
	{
		int num = 1;
		int num2 = 0;
		CSUtil.Print("Begin loading graph\n");
		if (!archive.BeginRead3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, ref num, ref num2))
		{
			CSUtil.Print("Error Reading Chunk\n");
		}
		if (num != 1 || num2 != 0)
		{
			archive.EndRead3dmChunk();
			CSUtil.Print("Read Graph Items Version Number Doesn't Match:" + num + "." + num2 + "\n");
			return false;
		}
		Guid[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			archive.EndRead3dmChunk();
			CSUtil.Print("TypeNames not found\n");
			return false;
		}
		string[] array = Path.GetFileNameWithoutExtension(importFileName).Split('_');
		string name = ((array.Length >= 7) ? array[6] : importFileName);
		ImportGroup importGroup = new ImportGroup
		{
			Identifier = Guid.NewGuid(),
			Name = name,
			IsViewportVisible = true,
			IsSelectedImportGroup = false,
			BlockIdentifiers = new List<Guid>()
		};
		if (isFreehandImport)
		{
			if (ImportedCSGParts == null)
			{
				ImportedCSGParts = new List<ImportGroup>();
			}
			int count = ImportedCSGParts.Count;
			ReadImportedObjectData(csuiCategoryTable, documentUserData);
			if (count == ImportedCSGParts.Count)
			{
				ImportedCSGParts.Add(importGroup);
				if (csuiCategoryTable != null)
				{
					foreach (CSUICategory category in csuiCategoryTable.Categories)
					{
						category.ImportGroupIdentifier = importGroup.Identifier;
					}
				}
			}
		}
		for (int i = 0; i < items.Length; i++)
		{
			bool flag = false;
			foreach (CSBlockDescription block in blockList)
			{
				if (!block.IsBlockClass(items[i]))
				{
					continue;
				}
				if (this == CSApplicationHelper.Application.Document.Graph)
				{
					if (block.GetNew(block.BlockName) == null)
					{
						GraphList.Add(null);
						CSUtil.Print($"Unable to create new:  {items[i]}\n");
					}
					flag = true;
					break;
				}
				CSObject newTemp = block.GetNewTemp(blockList[i].BlockName);
				GraphList.Add(newTemp);
				flag = true;
			}
			if (!flag)
			{
				CSUtil.Print($"Unable to find type:  {items[i]}\n");
				GraphList.Add(null);
			}
		}
		for (int j = CSApplicationHelper.Application.ImportOffset; j < GraphList.Count; j++)
		{
			if (GraphList[j] != null)
			{
				int major = 0;
				int minor = 0;
				if (GraphList[j].PreRead(ref archive, ref major, ref minor))
				{
					GraphList[j].Read(ref archive, this, major, minor);
					GraphList[j].PostRead(ref archive);
					Guid newInstanceGuid = Guid.NewGuid();
					Guid originalInstanceGuid = GraphList[j].InstanceGuid;
					GraphList[j].InstanceGuid = newInstanceGuid;
					if (isFreehandImport)
					{
						ImportGroup importGroup2 = ImportedCSGParts.FirstOrDefault((ImportGroup _) => _.BlockIdentifiers.Any((Guid blockIdentifier) => blockIdentifier == originalInstanceGuid));
						if (importGroup2 != null)
						{
							importGroup = importGroup2;
							importGroup.BlockIdentifiers.Remove(originalInstanceGuid);
							importGroup.BlockIdentifiers.Add(newInstanceGuid);
						}
						if (importGroup.BlockIdentifiers == null)
						{
							importGroup.BlockIdentifiers = new List<Guid>();
						}
						importGroup.BlockIdentifiers.Add(newInstanceGuid);
						if (CS.MaterialGroup.MaterialGroupHelper.ImportedDocumentMaterialGroups != null)
						{
							importGroup.IsPrimaryMetal = CS.MaterialGroup.MaterialGroupHelper.ImportedDocumentMaterialGroups.Any((CSMaterials.MaterialGroup _) => _.Classification == MaterialClassification.MetalPrimary && _.MaterialGroupItems.Any());
						}
					}
					CS.MaterialGroup.MaterialGroupHelper.ImportedDocumentMaterialGroups.ForEach(delegate(CSMaterials.MaterialGroup materialGroup)
					{
						materialGroup.MaterialGroupItems.Where((MaterialGroupItem materialGroupItem) => materialGroupItem.Id.Contains(originalInstanceGuid.ToString())).ToList().ForEach(delegate(MaterialGroupItem materialGroupItem)
						{
							string id = materialGroupItem.Id.Replace(originalInstanceGuid.ToString(), newInstanceGuid.ToString());
							materialGroupItem.Id = id;
							CSMaterials.MaterialGroup materialGroup2 = CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.FirstOrDefault((CSMaterials.MaterialGroup currentMaterialGroup) => materialGroup.Name == currentMaterialGroup.Name);
							if (materialGroup2 == null)
							{
								CSMaterials.MaterialGroup materialGroup3 = new CSMaterials.MaterialGroup
								{
									Id = Guid.NewGuid(),
									Name = materialGroup.Name,
									Classification = materialGroup.Classification,
									MaterialName = materialGroup.MaterialName,
									MaterialGroupItems = new ObservableCollection<MaterialGroupItem>()
								};
								CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(materialGroup3);
								materialGroup2 = materialGroup3;
							}
							materialGroup2.MaterialGroupItems.Add(materialGroupItem);
						});
					});
				}
				GraphList[j].UserData.RemoveKey(GCOption.UserDataType, GCOption.UserDataGuidName);
			}
			else
			{
				CSUtil.Print("Problem Reading Graph Item\n");
				int num3 = 0;
				int num4 = 0;
				archive.BeginRead3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, ref num4, ref num3);
				archive.EndRead3dmChunk();
			}
		}
		CSUtil.Print("Loaded graph of size:" + GraphList.Count + "\n");
		archive.EndRead3dmChunk();
		if (isFreehandImport)
		{
			importGroup.FirstGraphIdentifier = GraphList[CSApplicationHelper.Application.ImportOffset].InstanceGuid;
			importGroup.LastGraphIdentifier = GraphList[GraphList.Count - 1].InstanceGuid;
		}
		return true;
	}

	public List<CSInputChannel> InArrows(CSTreeObject treeObj)
	{
		return (from input in GraphList.Where((CSObject obj) => obj != null && !obj.Deleted && obj.ParentPatchWire == treeObj).OfType<CSBlock>().SelectMany((CSBlock block) => block.InputChannels)
			where input.OutputChannelBlock != null && input.OutputChannelBlock.ParentPatchWire != treeObj
			select input).ToList();
	}

	public void IncrementUpdateCounter()
	{
		CSUtil.SetUIModified(modifed: true);
		UpdateCount++;
	}

	public IEnumerable<CSPatchWireStart> PatchWireStartObjects()
	{
		return AllObjects().OfType<CSPatchWireStart>();
	}

	public bool ReadGraphItemsJson(JArray graph, IEnumerable<CSBlockDescription> blockList)
	{
		GraphList = null;
		int count = graph.Count;
		Guid[] array = new Guid[count];
		for (int i = 0; i < count; i++)
		{
			JObject jObject = (JObject)graph[i];
			array[i] = new Guid(jObject["TypeGuid"].ToString());
		}
		List<CSBlockDescription> list = SortBlockList(blockList);
		GraphList = new List<CSObject>(array.Length);
		Guid[] array2 = array;
		foreach (Guid guid in array2)
		{
			bool flag = false;
			int num = BlockDescriptionSearch(list, guid);
			if (num >= 0)
			{
				if (this == CSApplicationHelper.Application.Document.Graph)
				{
					if (list[num].GetNew(list[num].BlockName) == null)
					{
						GraphList.Add(null);
						CSUtil.PrintLine($"Unable to create new:  {guid}");
					}
					flag = true;
				}
				else
				{
					CSObject newTemp = list[num].GetNewTemp(list[num].BlockName);
					GraphList.Add(newTemp);
					flag = true;
				}
			}
			if (!flag)
			{
				CSUtil.PrintLine($"Unable to find type:  {guid}");
				return false;
			}
		}
		int num2 = 0;
		foreach (CSObject graph2 in GraphList)
		{
			if (graph2 != null)
			{
				JObject jObject2 = (JObject)graph[num2];
				int major = Convert.ToInt32(jObject2["majorVersion"]);
				int minor = Convert.ToInt32(jObject2["minorVersion"]);
				graph2.ReadJson(jObject2, this, major, minor);
			}
			num2++;
		}
		CSUtil.Print("Loaded graph of size:" + GraphList.Count + "\n");
		return true;
	}

	public bool ReadGraphItems(OnBinaryArchive archive, IEnumerable<CSBlockDescription> blockList)
	{
		int num = 1;
		int num2 = 0;
		CSUtil.PrintLine("Begin loading graph");
		if (!archive.BeginRead3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, ref num, ref num2))
		{
			CSUtil.PrintLine("Error Reading Chunk");
		}
		if (num != 1 || num2 != 0)
		{
			archive.EndRead3dmChunk();
			CSUtil.PrintLine($"Read Graph Items Version Number Doesn't Match:  {num}.{num2}");
			return false;
		}
		GraphList = null;
		Guid[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items == null)
		{
			GraphList = new List<CSObject>();
			archive.EndRead3dmChunk();
			CSUtil.PrintLine("TypeNames not found");
			return false;
		}
		List<CSBlockDescription> list = SortBlockList(blockList);
		GraphList = new List<CSObject>(items.Length);
		Guid[] array = items;
		foreach (Guid guid in array)
		{
			bool flag = false;
			int num3 = BlockDescriptionSearch(list, guid);
			if (num3 == -1)
			{
				string text = string.Concat("Expired Block loaded with Guid : ", guid, "\n And Version : ", CSApplicationHelper.Application.BuildRevisionString);
				string username = $"Null Block Check [{Environment.UserName}]";
				new SlackClient().PostMessage(text, username, "#dev-cadgeo");
				Guid guid2 = new Guid("{00000000-0000-0000-0000-000000000000}");
				num3 = BlockDescriptionSearch(list, guid2);
			}
			if (num3 >= 0)
			{
				if (this == CSApplicationHelper.Application.Document.Graph)
				{
					if (list[num3].GetNew(list[num3].BlockName) == null)
					{
						GraphList.Add(null);
						CSUtil.PrintLine($"Unable to create new:  {guid}");
					}
					flag = true;
				}
				else
				{
					CSObject newTemp = list[num3].GetNewTemp(list[num3].BlockName);
					GraphList.Add(newTemp);
					flag = true;
				}
			}
			if (!flag)
			{
				archive.EndRead3dmChunk();
				CSUtil.PrintLine($"Unable to find type:  {guid}");
				return false;
			}
		}
		foreach (CSObject graph in GraphList)
		{
			if (graph != null)
			{
				int major = 0;
				int minor = 0;
				if (graph.PreRead(ref archive, ref major, ref minor))
				{
					graph.Read(ref archive, this, major, minor);
					graph.PostRead(ref archive);
				}
			}
			else
			{
				CSUtil.PrintLine("Problem Reading Graph Item");
				int num4 = 0;
				int num5 = 0;
				archive.BeginRead3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, ref num5, ref num4);
				archive.EndRead3dmChunk();
			}
		}
		CSUtil.Print("Loaded graph of size:" + GraphList.Count + "\n");
		archive.EndRead3dmChunk();
		return true;
	}

	public void ReadImportedObjectDataJson(CSUICategoryTable categoryTable, JToken jSon)
	{
		try
		{
			List<ImportGroup> list = jSon.ToObject<List<ImportGroup>>();
			if (ImportedCSGParts == null)
			{
				ImportedCSGParts = new List<ImportGroup>();
			}
			if (list == null)
			{
				return;
			}
			list.ForEach(delegate(ImportGroup _)
			{
				Guid newIdentifier = Guid.NewGuid();
				List<CSUICategory> list2 = categoryTable?.Categories.Where((CSUICategory category) => category.ImportGroupIdentifier == _.Identifier).ToList();
				if (list2 != null && list2.Any())
				{
					list2.ForEach(delegate(CSUICategory category)
					{
						category.ImportGroupIdentifier = newIdentifier;
					});
				}
				_.Identifier = newIdentifier;
			});
			ImportedCSGParts.AddRange(list);
		}
		catch (Exception)
		{
		}
	}

	public void ReadImportedObjectData(CSUICategoryTable categoryTable, CSUserData documentUserData)
	{
		try
		{
			List<ImportGroup> list = new List<ImportGroup>();
			string text = documentUserData.FindValue("ImportedObjects", "Freehand");
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ImportGroup>));
			if (text != null)
			{
				StringReader textReader = new StringReader(text);
				list = xmlSerializer.Deserialize(textReader) as List<ImportGroup>;
			}
			if (ImportedCSGParts == null)
			{
				ImportedCSGParts = new List<ImportGroup>();
			}
			if (list == null)
			{
				return;
			}
			list.ForEach(delegate(ImportGroup _)
			{
				Guid newIdentifier = Guid.NewGuid();
				List<CSUICategory> list2 = categoryTable?.Categories.Where((CSUICategory category) => category.ImportGroupIdentifier == _.Identifier).ToList();
				if (list2 != null && list2.Any())
				{
					list2.ForEach(delegate(CSUICategory category)
					{
						category.ImportGroupIdentifier = newIdentifier;
					});
				}
				_.Identifier = newIdentifier;
			});
			ImportedCSGParts.AddRange(list);
		}
		catch (Exception)
		{
		}
	}

	public void ReloadGems()
	{
		foreach (CSObject graph in GraphList)
		{
			if (!(graph is CSBlock cSBlock))
			{
				continue;
			}
			CSValueGem csValueGem = graph as CSValueGem;
			if (csValueGem != null)
			{
				CSPatchWireStart patchwireStartUsingCSBlock = CSUtil.GetPatchwireStartUsingCSBlock(csValueGem);
				string patchwireName = string.Empty;
				if (patchwireStartUsingCSBlock != null)
				{
					patchwireName = patchwireStartUsingCSBlock.Identifier;
				}
				csValueGem.GemTableSizeExclusions?.ToList().ForEach(delegate(GemTableSizeExclusion gemTableSizeExclusion)
				{
					if (gemTableSizeExclusion.UpdatedCheckToClearExclusions())
					{
						csValueGem.GemTableSizeExclusions.Remove(gemTableSizeExclusion);
						if (gemTableSizeExclusion.ExcludedGemTableSizeIndexes.Any())
						{
							CSUtil.Print($"\nCleared Exclusions for Patchwire - {patchwireName}, Control - {csValueGem.Identifier}, Gem Type - {gemTableSizeExclusion.GemType}, Gem Shape - {gemTableSizeExclusion.GemShape}.");
							CSUtil.Print("\nPlease reassign exclusions using the new table sizes.");
						}
					}
				});
			}
			CSValueGemSize csValueGemSize = graph as CSValueGemSize;
			if (csValueGemSize != null)
			{
				CSPatchWireStart patchwireStartUsingCSBlock2 = CSUtil.GetPatchwireStartUsingCSBlock(csValueGemSize);
				string patchwireName2 = string.Empty;
				if (patchwireStartUsingCSBlock2 != null)
				{
					patchwireName2 = patchwireStartUsingCSBlock2.Identifier;
				}
				csValueGemSize.GemTableSizeExclusions?.ToList().ForEach(delegate(GemTableSizeExclusion gemTableSizeExclusion)
				{
					if (gemTableSizeExclusion.UpdatedCheckToClearExclusions())
					{
						csValueGemSize.GemTableSizeExclusions.Remove(gemTableSizeExclusion);
						if (gemTableSizeExclusion.ExcludedGemTableSizeIndexes.Any())
						{
							CSUtil.Print($"Cleared Gem Table Exclusions for Patchwire : {patchwireName2}, Control: {csValueGemSize.Identifier}, Gem Type: {gemTableSizeExclusion.GemType}, Gem Shape: {gemTableSizeExclusion.GemShape}.");
							CSUtil.Print("Please reassign exclusions using the new table sizes.");
						}
					}
				});
			}
			(cSBlock as IGemSource)?.ReloadGems();
		}
	}

	public int RemoveDeleted()
	{
		int num = 0;
		for (int num2 = GraphList.Count - 1; num2 >= 0; num2--)
		{
			if (GraphList[num2].Deleted)
			{
				if (GraphList[num2] is CSBlock csBlock)
				{
					CS.MaterialGroup.MaterialGroupHelper.RemoveItemFromAllMaterialGroups(csBlock, redraw: false);
				}
				GraphList.RemoveAt(num2);
				num++;
			}
		}
		return num;
	}

	public int RemoveUnusedObjects()
	{
		if (!_unusedObjects.Any())
		{
			return _unusedObjects.Count;
		}
		bool flag = false;
		bool flag2 = true;
		while (flag2)
		{
			flag2 = false;
			foreach (CSObject unusedObject in _unusedObjects)
			{
				if (unusedObject is CSPatchWireStart { Deleted: false, IsBlockDeletable: not false } cSPatchWireStart)
				{
					cSPatchWireStart.Delete();
					flag2 = (flag = true);
				}
			}
			if (flag2)
			{
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
			}
		}
		if (flag)
		{
			CSUtil.RedrawAllUI();
		}
		return _unusedObjects.Count;
	}

	public void ReplaceGeneratedGem(CSGemInstanceDefinition gemInstanceDefinition)
	{
		foreach (CSObject graph in GraphList)
		{
			if (graph is CSBlock cSBlock)
			{
				(cSBlock as IGemSource)?.ReplaceGem(gemInstanceDefinition);
			}
		}
	}

	public void SetIndexesOfGraphItems(bool activeOnly = false, ObservableCollection<CSObject> selection = null)
	{
		int num = 0;
		foreach (CSObject graph in GraphList)
		{
			if ((selection == null || selection.Contains(graph)) && !(graph.Deleted && activeOnly))
			{
				graph.TableIndex = num;
				num++;
			}
		}
	}

	public void SetParentUserData()
	{
		foreach (CSObject graph in GraphList)
		{
			if (graph.Parent == null)
			{
				graph.UserData.RemoveKey("SuperView", "Parent");
			}
			else
			{
				graph.UserData.AddKey("SuperView", "Parent", graph.Parent.TableIndex.ToString());
			}
		}
	}

	public void SetStaticData()
	{
		foreach (CSObject graph in GraphList)
		{
			if (graph is CSTreeObject cSTreeObject)
			{
				cSTreeObject.SetStaticUserData();
			}
			else
			{
				(graph as CSBlock)?.SetStaticUserData();
			}
		}
	}

	public void UpdateDeletable()
	{
		Dictionary<CSObject, CSPatchWireStart> dictionary = new Dictionary<CSObject, CSPatchWireStart>();
		foreach (CSObject item in ActiveObjects())
		{
			if (item != null)
			{
				CSPatchWireStart startObject = CSUtil.GetStartObject(item);
				if (startObject != null)
				{
					startObject.IsBlockDeletable = true;
					dictionary.Add(item, startObject);
				}
			}
		}
		foreach (CSObject item2 in ActiveObjects())
		{
			if (item2.Deleted || !(item2 is CSBlock cSBlock))
			{
				continue;
			}
			CSPatchWireStart startObject2 = CSUtil.GetStartObject(cSBlock);
			if (startObject2 == null || startObject2.Deleted)
			{
				continue;
			}
			int inputChannelCount = cSBlock.InputChannelCount;
			for (int i = 0; i < inputChannelCount; i++)
			{
				CSInputChannel inputChannel = cSBlock.GetInputChannel(i);
				if (inputChannel?.OutputChannelBlock != null)
				{
					dictionary.TryGetValue(inputChannel.OutputChannelBlock, out var value);
					if (value != null && startObject2 != value)
					{
						value.IsBlockDeletable = false;
					}
				}
			}
		}
	}

	public void UpdatePatchWireErrors()
	{
		CSPatchWireStart cSPatchWireStart = null;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		foreach (CSObject graph in GraphList)
		{
			if (graph is CSPatchWireStart cSPatchWireStart2)
			{
				if (cSPatchWireStart != null)
				{
					bool num = cSPatchWireStart.MissingInputInPatchwire != flag2 || cSPatchWireStart.ErrorInPatchwire != flag || cSPatchWireStart.ExceptionInPatchwire != flag3 || cSPatchWireStart.CAMAdjustmentInPatchwire != flag4;
					cSPatchWireStart.ExceptionInPatchwire = flag3;
					cSPatchWireStart.ErrorInPatchwire = flag;
					cSPatchWireStart.MissingInputInPatchwire = flag2;
					cSPatchWireStart.CAMAdjustmentInPatchwire = flag4;
					if (num)
					{
						cSPatchWireStart.OnErrorChanged(new EventArgs());
					}
					flag2 = false;
					flag = false;
					flag3 = false;
					flag4 = false;
				}
				cSPatchWireStart = cSPatchWireStart2;
				cSPatchWireStart.PatchWireUpdateTime = 0.0;
				cSPatchWireStart.CAMAdjustmentInPatchwire = false;
			}
			if (graph != null)
			{
				if (graph.LastUpdateTime > 0.0 && cSPatchWireStart != null)
				{
					cSPatchWireStart.PatchWireUpdateTime += graph.LastUpdateTime;
				}
				flag = graph.Error || flag;
				flag2 = graph.MissingInput || flag2;
				flag3 = graph.ExceptionThrown || flag3;
				if (graph is CSValue)
				{
					flag4 = (graph as CSValue).HasCAM() || flag4;
				}
			}
		}
		if (cSPatchWireStart != null)
		{
			bool num2 = cSPatchWireStart.MissingInputInPatchwire != flag2 || cSPatchWireStart.ErrorInPatchwire != flag || cSPatchWireStart.ExceptionInPatchwire != flag3 || cSPatchWireStart.CAMAdjustmentInPatchwire != flag4;
			cSPatchWireStart.ExceptionInPatchwire = flag3;
			cSPatchWireStart.ErrorInPatchwire = flag;
			cSPatchWireStart.MissingInputInPatchwire = flag2;
			cSPatchWireStart.CAMAdjustmentInPatchwire = flag4;
			if (num2)
			{
				cSPatchWireStart.OnErrorChanged(new EventArgs());
			}
		}
	}

	public bool WriteGraphItems(OnBinaryArchive archive, CSBlockList blockList, bool activeOnly = false, ObservableCollection<CSObject> selection = null)
	{
		archive.BeginWrite3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, 1, 0);
		long num = 0L;
		List<CSObject> list = ((selection == null) ? GraphList.Where((CSObject csObject) => !csObject.Deleted || !activeOnly).ToList() : GraphList.Where((CSObject csObject) => selection.Contains(csObject)).ToList());
		CSUtil.Print("Begin save of ActiveGraphList of items:" + list.Count + "\n");
		Guid[] array = new Guid[list.Count];
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			array[num2] = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			if (list[num2] != null)
			{
				CSBlockDescription cSBlockDescription = blockList.FindBlockFrom(list[num2]);
				if (cSBlockDescription != null)
				{
					array[num2] = cSBlockDescription.Guid;
				}
			}
		}
		CSUtilIO.WriteArray(array, ref archive);
		foreach (CSObject item in list)
		{
			if (item == null)
			{
				CSUtil.Print("Problem Writing Graph Item\n");
				continue;
			}
			item.PreWrite(ref archive);
			item.Write(ref archive);
			item.PostWrite(ref archive);
			num++;
		}
		CSUtil.Print("Saved activeGraphList of items:" + num + "\n");
		archive.EndWrite3dmChunk();
		return true;
	}

	public JArray WriteGraphItemsJSon(CSBlockList blockList, bool activeOnly = false, ObservableCollection<CSObject> selection = null)
	{
		long num = 0L;
		List<CSObject> list = ((selection == null) ? GraphList.Where((CSObject csObject) => !csObject.Deleted || !activeOnly).ToList() : GraphList.Where((CSObject csObject) => selection.Contains(csObject)).ToList());
		CSUtil.Print("Begin save of ActiveGraphList of items:" + list.Count + "\n");
		JArray jArray = new JArray();
		foreach (CSObject item in list)
		{
			if (item == null)
			{
				CSUtil.Print("Problem Writing Graph Item\n");
				continue;
			}
			Guid guid = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			CSBlockDescription cSBlockDescription = blockList.FindBlockFrom(item);
			if (cSBlockDescription != null)
			{
				guid = cSBlockDescription.Guid;
			}
			JObject jObj = new JObject();
			jObj.Add("TypeGuid", guid);
			jObj.Add("ClassType", item.ToString());
			jObj.Add("Index", item.TableIndex);
			item.PreWriteJson(ref jObj);
			item.WriteJson(ref jObj);
			jArray.Add(jObj);
			num++;
		}
		CSUtil.Print("Saved activeGraphList of items:" + num + "\n");
		return jArray;
	}

	public JArray WriteImportedObjectJson()
	{
		return JArray.FromObject(ImportedCSGParts);
	}

	public void WriteImportedObjectData()
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ImportGroup>));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, ImportedCSGParts);
			CSApplicationHelper.Application.Document.DocumentUserData.AddKey("ImportedObjects", "Freehand", stringWriter.ToString());
		}
		catch (Exception)
		{
		}
	}

	private static int BlockDescriptionSearch(IReadOnlyList<CSBlockDescription> blocks, Guid guid)
	{
		int num = 0;
		int num2 = blocks.Count;
		while (num < num2)
		{
			int num3 = (num + num2) / 2;
			int num4 = blocks[num3].Guid.CompareTo(guid);
			if (num4 == 0)
			{
				return num3;
			}
			if (num4 > 0)
			{
				num2 = num3;
			}
			else
			{
				num = num3 + 1;
			}
		}
		return -1;
	}

	private void GenerateConcurrent(bool quick = false)
	{
		if (CSApplicationHelper.Application.PrintTimingInformation)
		{
			CSUtil.Print("--Starting Generate-----------------\n");
		}
		ParallelGraphProcessingHelper.ResetWorkingQueue();
		CSApplicationHelper.Application.Document.StartGenerate(quick);
		GenerateConcurrentMarkBlocksForProcessing(GenerateObjects());
		GenerateConcurrentProcessBlocks(GenerateObjects());
		List<CSBlock> list = new List<CSBlock>();
		List<CSBlock> list2 = new List<CSBlock>();
		foreach (CSObject graph in GraphList)
		{
			(graph as ICSLoopUpdate)?.LoopUpdate();
			if (graph is CSBlock cSBlock)
			{
				if (cSBlock.JustUpdated() && cSBlock.LastUpdateTime > 0.0)
				{
					list.Add(cSBlock);
				}
				if (cSBlock.AvgLastUpdateTime > 0.1)
				{
					list2.Add(cSBlock);
				}
			}
		}
		list.Sort((CSBlock obj1, CSBlock obj2) => -obj1.LastUpdateTime.CompareTo(obj2.LastUpdateTime));
		list2.Sort((CSBlock obj1, CSBlock obj2) => -obj1.AvgLastUpdateTime.CompareTo(obj2.AvgLastUpdateTime));
		if (!quick && ((Transform)(ref CSApplicationHelper.Application.Conduit._layFlatForm)).IsValid)
		{
			CSUtil.LayFlat();
		}
		if (CSApplicationHelper.Application.PrintTimingInformation)
		{
			CSUtil.Print("--Block Update time-----------------\n");
			for (int num = 0; num < list.Count && num < 5; num++)
			{
				CSUtil.Print(string.Concat(num, ":", list[num], ":", list[num].LastUpdateTime, "\n"));
			}
			CSUtil.Print("--Avg Time In Block-----------------\n");
			for (int num2 = 0; num2 < list2.Count && num2 < 5; num2++)
			{
				CSUtil.Print(string.Concat(num2, ":", list2[num2], ":", list2[num2].AvgLastUpdateTime, "\n"));
			}
		}
		SetIndexesOfGraphItems();
		UpdatePatchWireErrors();
		if (!quick)
		{
			UpdateDeletable();
			if (CSDisplayConduit.IsGemMapEnabled)
			{
				CS.MaterialGroup.MaterialGroupHelper.UpdateGemDisplayNumbers();
			}
			UpdateOrphans();
		}
	}

	private void GenerateConcurrentAll()
	{
		ParallelGraphProcessingHelper.ResetWorkingQueue();
		CSApplicationHelper.Application.Document.StartGenerate(quickUpdate: false);
		GenerateConcurrentMarkBlocksForProcessing(ActiveObjects().OfType<CSBlock>());
		GenerateConcurrentProcessBlocks(ActiveObjects().OfType<CSBlock>());
		List<CSBlock> list = new List<CSBlock>();
		List<CSBlock> list2 = new List<CSBlock>();
		foreach (CSBlock item in GraphList.OfType<CSBlock>())
		{
			if (item.JustUpdated() && item.LastUpdateTime > 0.0)
			{
				list.Add(item);
			}
			if (item.AvgLastUpdateTime > 0.1)
			{
				list2.Add(item);
			}
		}
		list.Sort((CSBlock obj1, CSBlock obj2) => -obj1.LastUpdateTime.CompareTo(obj2.LastUpdateTime));
		list2.Sort((CSBlock obj1, CSBlock obj2) => -obj1.AvgLastUpdateTime.CompareTo(obj2.AvgLastUpdateTime));
		if (CSApplicationHelper.Application.PrintTimingInformation)
		{
			CSUtil.Print("--Block Update time-----------------\n");
			for (int num = 0; num < list.Count && num < 5; num++)
			{
				CSUtil.Print(string.Concat(num, ":", list[num], ":", list[num].LastUpdateTime, "\n"));
			}
			CSUtil.Print("--Avg Time In Block-----------------\n");
			for (int num2 = 0; num2 < list2.Count && num2 < 5; num2++)
			{
				CSUtil.Print(string.Concat(num2, ":", list2[num2], ":", list2[num2].AvgLastUpdateTime, "\n"));
			}
		}
		SetIndexesOfGraphItems();
		UpdatePatchWireErrors();
		UpdateDeletable();
		UpdateOrphans();
	}

	private void GenerateConcurrentMarkBlocksForProcessing(IEnumerable<CSBlock> blocksToMark)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		IList<CSBlock> source = (blocksToMark as IList<CSBlock>) ?? blocksToMark.ToList();
		CSBlocks.ToList().ForEach(delegate(CSBlock block)
		{
			block.LoopCheckStatus = BlockLoopCheckStatus.NeedsCheck;
		});
		source.Aggregate(seed: false, (bool current, CSBlock item) => current | item.UpdateStatusForProcessing());
		CSBlocks.Where((CSBlock csBlock) => csBlock.LoopCheckStatus == BlockLoopCheckStatus.NeedsCheck).ToList().ForEach(delegate(CSBlock csBlock)
		{
			csBlock.CheckIllegalLoops();
		});
		stopwatch.Stop();
	}

	private static void GenerateConcurrentProcessBlocks(IEnumerable<CSBlock> blocksToProcess)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		IList<CSBlock> list = (blocksToProcess as IList<CSBlock>) ?? blocksToProcess.Where((CSBlock block) => block.ProcessingStatus != BlockProcessingStatus.Processed).ToList();
		if (list.Any())
		{
			ParallelGraphProcessingHelper.GenerateChildRelationships(list);
			ParallelGraphProcessingHelper.ProcessGraph(list.ToList());
		}
		stopwatch.Stop();
		RhinoApp.WriteLine("Total Processing Time: {0}ms", (object)stopwatch.ElapsedMilliseconds);
	}

	private IEnumerable<CSBlock> GenerateObjects()
	{
		return from obj in DisplayObjects()
			where (obj.UIGenerate || MouseDown) && !obj.Deleted
			select obj;
	}

	private void GenerateSynchronous(bool quick)
	{
		List<CSBlock> list = GenerateObjects().ToList();
		CSApplicationHelper.Application.Document.StartGenerate(quick);
		bool flag = Enumerable.Aggregate(seed: list.Aggregate(seed: false, (bool current, CSBlock _) => current | _.MarkAllChanged()), source: list.Where((CSBlock _) => !CSApplicationHelper.Application.Document.InterruptUpdate()), func: (bool current, CSBlock _) => current | _.UpdateAllChanged());
		GraphList.OfType<ICSLoopUpdate>().ToList().ForEach(delegate(ICSLoopUpdate _)
		{
			_.LoopUpdate();
		});
		UpdateBlockTimings(list);
		if (!quick && ((Transform)(ref CSApplicationHelper.Application.Conduit._layFlatForm)).IsValid)
		{
			CSUtil.LayFlat();
		}
		SetIndexesOfGraphItems();
		UpdatePatchWireErrors();
		if (!quick && !CSApplicationHelper.IsCounterSketchStudio)
		{
			UpdateDeletable();
			if (CSDisplayConduit.IsGemMapEnabled)
			{
				CS.MaterialGroup.MaterialGroupHelper.UpdateGemDisplayNumbers();
			}
			UpdateOrphans();
		}
	}

	private void GenerateSynchronousAll()
	{
		List<CSBlock> list = ActiveObjects().OfType<CSBlock>().ToList();
		CSApplicationHelper.Application.Document.StartGenerate(quickUpdate: false);
		bool flag = Enumerable.Aggregate(seed: list.Aggregate(seed: false, (bool current, CSBlock _) => current | _.MarkAllChanged()), source: list.Where((CSBlock _) => !CSApplicationHelper.Application.Document.InterruptUpdate()), func: (bool current, CSBlock _) => current | _.UpdateAllChanged());
		GraphList.OfType<ICSLoopUpdate>().ToList().ForEach(delegate(ICSLoopUpdate _)
		{
			_.LoopUpdate();
		});
		UpdateBlockTimings(list);
		if (((Transform)(ref CSApplicationHelper.Application.Conduit._layFlatForm)).IsValid)
		{
			CSUtil.LayFlat();
		}
		SetIndexesOfGraphItems();
		UpdatePatchWireErrors();
		if (!CSApplicationHelper.IsCounterSketchStudio)
		{
			UpdateDeletable();
			if (CSDisplayConduit.IsGemMapEnabled)
			{
				CS.MaterialGroup.MaterialGroupHelper.UpdateGemDisplayNumbers();
			}
			UpdateOrphans();
		}
	}

	private static string GetSignatureAtBlock(int nStopAtBlock, out int blockLength)
	{
		int inputCount = default(Guid).ToByteArray().Length;
		List<Guid> list = new List<Guid>();
		foreach (CSObject graph in CSApplicationHelper.Application.Document.Graph.GraphList)
		{
			if (!graph.Deleted)
			{
				object[] customAttributes = graph.GetType().GetCustomAttributes(typeof(CSBlockAttribute), inherit: true);
				int num = 0;
				if (num < customAttributes.Length && customAttributes[num] is CSBlockAttribute cSBlockAttribute)
				{
					list.Add(cSBlockAttribute.Guid);
				}
			}
			if (nStopAtBlock != -1 && list.Count == nStopAtBlock)
			{
				break;
			}
		}
		SHA1 sHA = SHA1.Create();
		foreach (Guid item in list)
		{
			sHA.TransformBlock(item.ToByteArray(), 0, inputCount, null, 0);
		}
		sHA.TransformFinalBlock(new byte[0], 0, 0);
		blockLength = list.Count;
		return SHAToString(sHA.Hash);
	}

	private static bool IsValidBlockClass(CSObject obj, IEnumerable<CSBlockDescription> blockList)
	{
		if (obj == null)
		{
			return false;
		}
		string fullname = obj.GetType().FullName;
		return blockList.Any((CSBlockDescription desc) => desc.IsBlockClass(fullname));
	}

	private void OrphanTestBlock(CSBlock testObj)
	{
		CSPatchWireStart cSPatchWireStart = null;
		foreach (CSInputChannel inputChannel in testObj.InputChannels)
		{
			if (inputChannel.OutputChannelBlock != null && inputChannel.OutputChannelBlock.Parent is CSPatchWireStart testObj2)
			{
				OrphanTestBlock(testObj2, patchWire: false);
			}
		}
	}

	private void OrphanTestBlock(CSPatchWireStart testObj, bool patchWire)
	{
		if (!testObj.OrphanedPatchwire)
		{
			return;
		}
		if (patchWire)
		{
			string text = testObj.UserData.FindValue("UI", "ControlColorBrush");
			if (string.IsNullOrEmpty(text) || text != "Math")
			{
				testObj.OrphanedPatchwire = false;
			}
		}
		else
		{
			testObj.OrphanedPatchwire = false;
		}
		foreach (CSInputChannel item in (from input in CSBlocks.Where((CSBlock obj) => obj != null && !obj.Deleted && obj.ParentPatchWire == testObj).SelectMany((CSBlock block) => block.InputChannels)
			where input?.OutputChannelBlock != null && input.OutputChannelBlock.ParentPatchWire != testObj
			select input).ToList())
		{
			CSPatchWireStart cSPatchWireStart = item.OutputChannelBlock.Parent as CSPatchWireStart;
			CSPatchWireStart cSPatchWireStart2 = null;
			if (cSPatchWireStart != null)
			{
				cSPatchWireStart2 = cSPatchWireStart;
			}
			else if (item.OutputChannelBlock.ParentPatchWire != null)
			{
				cSPatchWireStart2 = item.OutputChannelBlock.ParentPatchWire;
			}
			if (cSPatchWireStart2 != null)
			{
				OrphanTestBlock(cSPatchWireStart2, patchWire: false);
			}
		}
	}

	private void PrintGenerateTime(bool isQuick)
	{
		string arg = (IsConcurrentProcessingEnabled ? $"Concurrent({CSApplicationHelper.MaxNumberOfAvailableCores})" : string.Empty);
		CSUtil.PrintLine();
		CSUtil.PrintLine("--------------------------------------");
		CSUtil.PrintLine($"Generate{arg} (Quick: {isQuick}) in {GenerateTime} ms");
		CSUtil.PrintLine("--------------------------------------");
	}

	private static string SHAToString(byte[] b)
	{
		if (b != null)
		{
			return BitConverter.ToString(b).ToLower().Replace("-", "");
		}
		return "";
	}

	private static List<CSBlockDescription> SortBlockList(IEnumerable<CSBlockDescription> blockList)
	{
		List<CSBlockDescription> list = new List<CSBlockDescription>(blockList);
		list.Sort((CSBlockDescription b1, CSBlockDescription b2) => b1.Guid.CompareTo(b2.Guid));
		return list;
	}

	private static void UpdateBlockTimings(List<CSBlock> generateObjects)
	{
		List<CSBlock> updatedItems = new List<CSBlock>();
		List<CSBlock> avgUpdateItems = new List<CSBlock>();
		generateObjects.ForEach(delegate(CSBlock _)
		{
			if (_.JustUpdated() && _.LastUpdateTime > 0.0)
			{
				updatedItems.Add(_);
			}
			if (_.AvgLastUpdateTime > 0.1)
			{
				avgUpdateItems.Add(_);
			}
		});
		updatedItems.Sort((CSBlock obj1, CSBlock obj2) => -obj1.LastUpdateTime.CompareTo(obj2.LastUpdateTime));
		avgUpdateItems.Sort((CSBlock obj1, CSBlock obj2) => -obj1.AvgLastUpdateTime.CompareTo(obj2.AvgLastUpdateTime));
		if (CSApplicationHelper.Application.PrintTimingInformation)
		{
			for (int num = 0; num < updatedItems.Count && num < 5; num++)
			{
			}
			for (int num2 = 0; num2 < avgUpdateItems.Count && num2 < 5; num2++)
			{
			}
		}
	}

	private void UpdateOrphans()
	{
		List<CSObject> graphList = GraphList;
		foreach (CSObject item in graphList)
		{
			if (!(item is CSPatchWireStart cSPatchWireStart))
			{
				continue;
			}
			cSPatchWireStart.OrphanedPatchwire = true;
			cSPatchWireStart.ObsoletePatchwire = false;
			int num = cSPatchWireStart.ChildCount();
			for (int i = 0; i < num; i++)
			{
				object[] customAttributes = cSPatchWireStart.Child(i).GetType().GetCustomAttributes(inherit: true);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					if (((Attribute)customAttributes[j]) is ObsoleteAttribute)
					{
						cSPatchWireStart.ObsoletePatchwire = true;
						break;
					}
				}
				if (cSPatchWireStart.ObsoletePatchwire)
				{
					break;
				}
			}
		}
		foreach (CSObject item2 in graphList)
		{
			if (item2 is CSPatchWireStart { Deleted: false, DisplayVisible: not false } cSPatchWireStart2)
			{
				OrphanTestBlock(cSPatchWireStart2, patchWire: true);
			}
		}
	}
}
