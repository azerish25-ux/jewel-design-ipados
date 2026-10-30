using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues.CSValueHandle;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.MaterialGroup;
using CS.Parametric;
using CSDatas;
using CSMaterials;
using GV.CSStudioPatchWire;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using Rhino.Display;
using Rhino.Geometry;

namespace CounterSketchServer.Utilities.Helpers;

public static class ImportGroupHelper
{
	private static readonly Size _captureSize = new Size(300, 300);

	private static Guid _activeImportGroupIdentifier;

	public static Guid ActiveImportGroupIdentifier
	{
		get
		{
			return _activeImportGroupIdentifier;
		}
		set
		{
			_activeImportGroupIdentifier = value;
			GetGraphImportGroups().ForEach(delegate(ImportGroup importGroup)
			{
				importGroup.IsSelectedImportGroup = importGroup.Identifier == value;
			});
		}
	}

	public static List<UIControl> DeleteImportedGroupFromGraph(List<Guid> targetImportGroupIdentifiers)
	{
		GetDocument().Graph.IncrementUpdateCounter();
		GetImportGroupsByIdentifiers(targetImportGroupIdentifiers)?.ForEach(delegate(ImportGroup importGroup)
		{
			GetDocument().UndoTable.Pause(isPausing: true);
			foreach (CSBlock item in (from activeObject in GetGraphBlocks()
				where importGroup.BlockIdentifiers.Contains(activeObject.InstanceGuid)
				select activeObject).ToList())
			{
				CSObject[] range = CSUtil.GetRange(item);
				if (range != null)
				{
					CSObject[] array = range;
					foreach (CSObject obj in array)
					{
						obj.Deleted = true;
						(obj as CSBlock)?.OutputChannelDatas.Where((CSData b) => b != null).ToList().ForEach(delegate(CSData b)
						{
							b.Dispose();
						});
					}
				}
			}
			GetDocument().UndoTable.Pause(isPausing: false);
			GetGraphImportGroups()?.Remove(importGroup);
			GetDocument().Graph.CategoryTable.Categories.Where((CSUICategory category) => category.ImportGroupIdentifier == importGroup.Identifier).ToList().ForEach(delegate(CSUICategory csuiCategory)
			{
				GetDocument().Graph.CategoryTable.Categories.Remove(csuiCategory);
			});
			GetDocument().Graph.RemoveDeleted();
			GetDocument().GemTable.RemovedUnusedGems();
			GetDocument().Graph.ClearIndexesOfGraphItems();
			GetDocument().UndoTable.Reset();
			GetDocument().Graph.Generate();
			CSUtil.Redraw();
		});
		return UIControlHelper.GetUIControlsFromGraphObjects();
	}

	public static void DocumentDuplicateCSGPart(List<Guid> targetImportGroupIdentifiers)
	{
		List<ImportGroup> second = GetGraphImportGroups().ToList();
		Guid partGroupIdentifier = Guid.Empty;
		Guid newPartGroupIdentifier = Guid.Empty;
		GetImportGroupsByIdentifiers(targetImportGroupIdentifiers)?.OrderBy((ImportGroup _) => _.PartGroupId).ToList().ForEach(delegate(ImportGroup importGroup)
		{
			if (importGroup.PartGroupId != partGroupIdentifier)
			{
				partGroupIdentifier = importGroup.PartGroupId;
				newPartGroupIdentifier = ((partGroupIdentifier != Guid.Empty) ? Guid.NewGuid() : Guid.Empty);
			}
			CSUtil.DuplicateImportGroup(importGroup, null, newPartGroupIdentifier);
		});
		GetDocument().UndoTable.Reset();
		List<ImportGroup> source = GetGraphImportGroups().ToList().Except(second).ToList();
		if (source.Any())
		{
			ImportGroup firstParentPart = source.OrderByDescending((ImportGroup _) => _.IsParentPart).FirstOrDefault();
			UpdateActiveImportGroupUsingGroup(firstParentPart);
			SelectImportedGroupsByIdentifierSecondary((from _ in source.Where(delegate(ImportGroup _)
				{
					Guid identifier = _.Identifier;
					Guid? obj = firstParentPart?.Identifier;
					return identifier != obj;
				})
				select _.Identifier).ToList());
			UpdateImportGroups(updateAllImportGroupImages: true);
			UpdateFreehandStackControlsForImportGroup(source.Select((ImportGroup _) => _.Identifier).ToList());
		}
	}

	public static void DocumentImportCSGPart(string pathToCSGPart, int? centerGemGraphIndex = null, bool doShowFontSourceErrors = true)
	{
		List<ImportGroup> second = GetGraphImportGroups().ToList();
		GetDocument().Import(pathToCSGPart, isFreehandImport: true, doShowFontSourceErrors);
		List<ImportGroup> list = GetGraphImportGroups().ToList().Except(second).ToList();
		if (!list.Any())
		{
			return;
		}
		if (centerGemGraphIndex.HasValue)
		{
			list.ForEach(delegate(ImportGroup importGroup)
			{
				UIControlHelper.UpdateCenterGemControlIdentifierForImportGroup(importGroup, centerGemGraphIndex);
			});
		}
		UpdateActiveImportGroupUsingGroup(list.FirstOrDefault());
		if (list.Count() > 1)
		{
			SelectImportedGroupsByIdentifierSecondary((from _ in list.Skip(1)
				select _.Identifier).ToList());
		}
		UpdateImportGroups(updateAllImportGroupImages: true);
		UpdateFreehandStackControlsForImportGroup(list.Select((ImportGroup _) => _.Identifier).ToList());
	}

	public static void ExportImportGroups(List<Guid> targetImportGroupIdentifiers, string path)
	{
		CS.MaterialGroup.MaterialGroupHelper.SaveMaterialGroups(GetDocument().DocumentUserData);
		CSDocument temporaryDocument = new CSDocument
		{
			GemTable = GetDocument().GemTable,
			CurrentDisplayComponents = GetDocument().CurrentDisplayComponents,
			DocumentUserData = GetDocument().DocumentUserData
		};
		CS.MaterialGroup.MaterialGroupHelper.SaveMaterialGroups(temporaryDocument.DocumentUserData);
		List<ImportGroup> importGroups = GetImportGroupsByIdentifiers(targetImportGroupIdentifiers);
		Guid partGroupIdentifier = Guid.Empty;
		Guid newPartGroupIdentifier = Guid.Empty;
		importGroups?.OrderBy((ImportGroup _) => _.PartGroupId).ToList().ForEach(delegate(ImportGroup importGroup)
		{
			if (importGroup.PartGroupId != partGroupIdentifier)
			{
				partGroupIdentifier = importGroup.PartGroupId;
				newPartGroupIdentifier = ((partGroupIdentifier != Guid.Empty) ? Guid.NewGuid() : Guid.Empty);
			}
			CSUtil.DuplicateImportGroup(importGroup, temporaryDocument.Graph, newPartGroupIdentifier);
		});
		Bitmap bitmap = CSDisplayCaptureHelper.CaptureViewportToBitmapForTargetedBlocksAtDefaultPerspective((from _ in GetDocument().Graph.DisplayObjects()
			where _.ImportGroup != null && importGroups.Any((ImportGroup importGroup) => importGroup.Identifier == _.ImportGroup.Identifier)
			select _.InstanceGuid).ToList(), _captureSize, zoomToObjects: true, 0.0);
		string tempFileName = Path.GetTempFileName();
		bitmap.Save(tempFileName);
		temporaryDocument.Write(path, tempFileName);
	}

	public static void ImportGroupAddToPartGroup(List<Guid> importGroupIdentifiers)
	{
		List<ImportGroup> source = GetImportGroupsByIdentifiers(importGroupIdentifiers).ToList();
		ImportGroup firstGroupedPart = source.FirstOrDefault((ImportGroup _) => _.PartGroupId != Guid.Empty && _.IsParentPart);
		Guid partGroupId = firstGroupedPart?.PartGroupId ?? Guid.NewGuid();
		string partGroupName = firstGroupedPart?.PartGroupName ?? "Group";
		bool isParentAssigned = firstGroupedPart != null;
		source.ToList().ForEach(delegate(ImportGroup _)
		{
			_.PartGroupId = partGroupId;
			_.PartGroupName = partGroupName;
			_.IsParentPart = ((firstGroupedPart != null) ? (_ == firstGroupedPart) : (_.IsSelectedImportGroup && !isParentAssigned));
			if (_.IsParentPart)
			{
				isParentAssigned = true;
			}
		});
		SelectImportedGroupByIdentifier(partGroupId);
		UpdateImportGroups(updateAllImportGroupImages: true);
		UpdateFreehandStackControls();
	}

	public static void ImportGroupBreakPartGroup(Guid importGroupIdentifier)
	{
		List<ImportGroup> obj = GetGraphImportGroups()?.Where((ImportGroup _) => _.PartGroupId == importGroupIdentifier && !_.IsParentPart).ToList();
		ImportGroup importGroup = GetGraphImportGroups()?.FirstOrDefault((ImportGroup _) => _.PartGroupId == importGroupIdentifier && _.IsParentPart);
		if (importGroup != null)
		{
			importGroup.PartGroupId = Guid.Empty;
			importGroup.PartGroupName = string.Empty;
			UpdateActiveImportGroupUsingGroup(importGroup, requiresUICallback: false);
		}
		obj?.ForEach(delegate(ImportGroup _)
		{
			_.PartGroupId = Guid.Empty;
			_.PartGroupName = string.Empty;
		});
		SelectImportedGroupsByIdentifierSecondary(obj?.Select((ImportGroup _) => _.Identifier).ToList());
		UpdateImportGroups(updateAllImportGroupImages: true);
		UpdateFreehandStackControls();
	}

	public static void ImportGroupRename(Guid importGroupIdentifier, string updatedName)
	{
		List<ImportGroup> importGroupsByIdentifiers = GetImportGroupsByIdentifiers(new List<Guid> { importGroupIdentifier });
		importGroupsByIdentifiers?.ForEach(delegate(ImportGroup _)
		{
			if (_.PartGroupId != Guid.Empty)
			{
				_.PartGroupName = updatedName;
			}
			else
			{
				_.Name = updatedName;
			}
		});
		UpdateFreehandStackControlsForImportGroup(importGroupsByIdentifiers.Select((ImportGroup _) => _.Identifier).ToList());
	}

	public static void InitializeStackControls()
	{
		if (RhinoServices.StudioUICallback == null || GetGraphImportGroups() == null)
		{
			return;
		}
		if (GetGraphImportGroups() != null)
		{
			UpdateActiveImportGroupUsingGroup(null);
			UpdateImportGroups(updateAllImportGroupImages: true);
		}
		List<UIControl> orderedControlList = new List<UIControl>();
		GetGraphImportGroups().ForEach(delegate(ImportGroup importedObjectGroup)
		{
			List<UIControl> list = (from csValueBase in GetDocument().Graph.AllObjects()
				where importedObjectGroup.BlockIdentifiers.Contains(csValueBase.InstanceGuid) && (csValueBase.StudioVisible || csValueBase is CSValueXFormObject)
				select csValueBase).AsEnumerable().Select(UIControlHelper.CreateUIControlFromGraphObject).ToList();
			list.ForEach(delegate(UIControl uiControl)
			{
				uiControl.ImportGroupIdentifier = importedObjectGroup.Identifier;
			});
			foreach (CSUICategory item in GetDocument().Graph.CategoryTable.Categories.OrderBy((CSUICategory category) => category.Index))
			{
				CSUICategory categoryLocal = item;
				List<UIControl> collection = list.Where((UIControl controls) => controls.CategoryName == categoryLocal.CategoryName && controls.ImportGroupIdentifier == categoryLocal.ImportGroupIdentifier).ToList();
				orderedControlList.AddRange(collection);
			}
			orderedControlList.AddRange(list.Where((UIControl freehandControl) => freehandControl.CurrentUIControlType == UIControlType.CSValueXFormObject));
			RequestScreenCaptureAtPerspectivePositionForImportGroup(_captureSize, zoom: true, new List<Guid> { importedObjectGroup.Identifier });
		});
		RhinoServices.StudioUICallback.FreehandControlUpdateCallback(orderedControlList);
	}

	public static List<UIControl> InvokeAddAPartPatchwire(string path, string name)
	{
		GlobalErrorHandler.WriteInfoToEventLog("InvokeAddAPartPatchwire Starting to Invoke:" + path);
		GetDocument().Graph.IncrementUpdateCounter();
		int count = GetDocument().Graph.GraphList.Count;
		if (CreatePart(path, name) != CSPatchWireFunctionResult.Success)
		{
			GlobalErrorHandler.WriteErrorToEventLog("InvokeAddAPartPatchwire Failed to Invoke Part: " + path);
			while (count < GetDocument().Graph.GraphList.Count)
			{
				GetDocument().Graph.GraphList.Remove(GetDocument().Graph.GraphList[count]);
			}
		}
		GlobalErrorHandler.WriteInfoToEventLog("InvokeAddAPartPatchwire Succeeded to Invoke Part: " + path);
		GetDocument().UndoTable.Reset();
		GetDocument().Graph.Generate();
		GlobalErrorHandler.WriteInfoToEventLog("InvokeAddAPartPatchwire Generated.");
		CSUtil.Redraw();
		GlobalErrorHandler.WriteInfoToEventLog("InvokeAddAPartPatchwire Redrawn");
		ImportGroup importGroup = GetGraphImportGroups()?.LastOrDefault();
		if (importGroup != null)
		{
			UpdateFreehandStackControlsForImportGroup(new List<Guid> { importGroup.Identifier });
			UpdateActiveImportGroupUsingGroup(importGroup);
		}
		return new List<UIControl>();
	}

	public static void RequestScreenCaptureAtPerspectivePositionForImportGroup(Size captureSize, bool zoom, List<Guid> importGroupIdentifiers)
	{
		if (GetGraphImportGroups() == null)
		{
			return;
		}
		((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = false;
		GetImportGroupsByIdentifiers(importGroupIdentifiers)?.GroupBy((ImportGroup importGroupKey) => new { importGroupKey.PartGroupId }, (key, IEnumerable<ImportGroup> values) => new
		{
			PartGroupId = key.PartGroupId,
			ImportGroups = values.ToList()
		}).ToList().ForEach(groupCollection =>
		{
			if (groupCollection.PartGroupId == Guid.Empty)
			{
				groupCollection.ImportGroups?.ForEach(delegate(ImportGroup importGroup)
				{
					List<Guid> list2 = (from csBlock in GetDocument().Graph.DisplayObjects()
						where importGroup == csBlock.ImportGroup
						select csBlock.InstanceGuid).ToList();
					if (RhinoServices.StudioUICallback != null && list2.Any())
					{
						Bitmap bitmap2 = CSDisplayCaptureHelper.CaptureViewportToBitmapForTargetedBlocksAtDefaultPerspective(list2, captureSize, zoom, -0.25);
						if (bitmap2 != null)
						{
							RhinoServices.StudioUICallback.FreehandImportGroupBitmapUpdateCallback(new KeyValuePair<Guid, Bitmap>(importGroup.Identifier, bitmap2));
						}
					}
				});
			}
			else
			{
				List<Guid> groupCollectionIdentifiers = groupCollection.ImportGroups.Select((ImportGroup _) => _.Identifier).ToList();
				List<Guid> list = (from csBlock in GetDocument().Graph.DisplayObjects()
					where groupCollectionIdentifiers.Any(delegate(Guid _)
					{
						Guid? obj = csBlock.ImportGroup?.Identifier;
						return _ == obj;
					})
					select csBlock.InstanceGuid).ToList();
				if (RhinoServices.StudioUICallback != null && list.Any())
				{
					Bitmap bitmap = CSDisplayCaptureHelper.CaptureViewportToBitmapForTargetedBlocksAtDefaultPerspective(list, captureSize, zoom, -0.25);
					if (bitmap != null)
					{
						RhinoServices.StudioUICallback.FreehandImportGroupBitmapUpdateCallback(new KeyValuePair<Guid, Bitmap>(groupCollection.PartGroupId, bitmap));
					}
				}
			}
		});
		((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = true;
	}

	public static void ResetHandleForImportGroup(Guid importGroupIdentifier)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		List<CSValueXFormBase> source = CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<CSValueXFormBase>().ToList();
		List<ImportGroup> list = GetImportGroupsByIdentifier(importGroupIdentifier).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		Transform transform = Transform.Unset;
		ImportGroup parentImportGroup = list.FirstOrDefault((ImportGroup _) => _.IsParentPart);
		if (parentImportGroup != null)
		{
			CSValueXFormBase cSValueXFormBase = source.FirstOrDefault((CSValueXFormBase _) => _.ImportGroup.Identifier == parentImportGroup.Identifier);
			if (cSValueXFormBase != null)
			{
				transform = cSValueXFormBase.GetResetTransform();
			}
		}
		IEnumerable<Guid> importGroupIdentifiers = list.Select((ImportGroup _) => _.Identifier);
		List<CSValueXFormBase> list2 = source.Where((CSValueXFormBase _) => importGroupIdentifiers.Contains(_.ImportGroup.Identifier)).ToList();
		using (new CSUndoTransaction())
		{
			foreach (CSValueXFormBase item in list2)
			{
				if (((Transform)(ref transform)).IsValid)
				{
					item.ApplyTransform(transform);
				}
				else
				{
					item.Reset();
				}
			}
		}
		CSUtil.ForceGraphToRedraw(list2.Cast<CSBlock>().ToList());
	}

	public static void SelectImportedGroupByIdentifier(Guid selectedGroupIdentifier)
	{
		List<ImportGroup> importGroupsByIdentifiers = GetImportGroupsByIdentifiers(new List<Guid> { selectedGroupIdentifier });
		if (importGroupsByIdentifiers != null && importGroupsByIdentifiers.Count() > 1 && importGroupsByIdentifiers.Any((ImportGroup _) => _.PartGroupId != Guid.Empty))
		{
			UpdateActiveImportGroupUsingGroup(importGroupsByIdentifiers?.FirstOrDefault((ImportGroup _) => _.IsParentPart), requiresUICallback: false);
			SelectImportedGroupsByIdentifierSecondary((from _ in importGroupsByIdentifiers?.Where((ImportGroup _) => !_.IsParentPart)
				select _.Identifier).ToList());
		}
		else
		{
			UpdateActiveImportGroupUsingGroup(importGroupsByIdentifiers?.FirstOrDefault(), requiresUICallback: false);
		}
	}

	public static void SelectImportedGroupsByIdentifierSecondary(List<Guid> importGroupIdentifiers = null)
	{
		List<ImportGroup> list = new List<ImportGroup>();
		((importGroupIdentifiers != null) ? GetImportGroupsByIdentifiers(importGroupIdentifiers) : (from _ in GetGraphImportGroups()
			where _.IsSecondarySelectedImportGroup
			select _).ToList())?.ForEach(delegate(ImportGroup importGroup)
		{
			CSBlock cSBlock = GetDocument().Graph.AllObjects().OfType<CSBlock>().FirstOrDefault((CSBlock graphObject) => graphObject is ICSVHandle && importGroup.BlockIdentifiers.Contains(graphObject.InstanceGuid));
			if (cSBlock != null)
			{
				ICSVHandle handle = cSBlock as ICSVHandle;
				List<Guid> list2 = importGroupIdentifiers;
				CSValueHandleHelper.UIUpdateSelectedHandles(handle, list2 != null && !list2.Any());
			}
		});
	}

	public static void SetImportGroupVisibilityByIdentifier(List<Guid> targetImportGroupIdentifiers, bool isViewportVisible)
	{
		GetImportGroupsByIdentifiers(targetImportGroupIdentifiers)?.ForEach(delegate(ImportGroup importGroup)
		{
			importGroup.IsViewportVisible = isViewportVisible;
		});
		GetDocument().Graph.Generate();
		CSUtil.Redraw();
	}

	public static void UpdateActiveImportGroupUsingGroup(ImportGroup importGroup, bool requiresUICallback = true)
	{
		if (RhinoServices.StudioUICallback == null)
		{
			return;
		}
		if (importGroup != null)
		{
			if (ActiveImportGroupIdentifier != importGroup.Identifier)
			{
				importGroup.IsSecondarySelectedImportGroup = false;
				ActiveImportGroupIdentifier = importGroup.Identifier;
			}
			GetGraphImportGroups().ForEach(delegate(ImportGroup secondaryGroup)
			{
				secondaryGroup.IsSecondarySelectedImportGroup = false;
			});
			CSBlock cSBlock = GetGraphBlocks().FirstOrDefault((CSBlock graphObject) => graphObject is ICSVHandle && importGroup.BlockIdentifiers.Contains(graphObject.InstanceGuid));
			if (cSBlock != null)
			{
				CSValueHandleHelper.TemporaryHandle = (CSValueHandleHelper.CurrentHandle = cSBlock as ICSVHandle);
				CSValueHandleHelper.SelectedHandles.Clear();
			}
		}
		else
		{
			ActiveImportGroupIdentifier = Guid.Empty;
			CSValueHandleHelper.CurrentHandle = null;
			CSValueHandleHelper.TemporaryHandle = null;
			CSValueHandleHelper.SelectedHandles.Clear();
		}
		CSUtil.Redraw();
		if (requiresUICallback)
		{
			UpdateImportGroups(updateAllImportGroupImages: false);
			_ = ActiveImportGroupIdentifier;
			UpdateImportGroupBitmapImage(new List<Guid> { ActiveImportGroupIdentifier });
			UpdateFreehandStackControlsForImportGroup(GetSelectedImportGroupIdentifiers());
		}
	}

	public static void UpdateActiveImportGroupUsingHandleArgs(CSApplication.SelectedHandlesEventArgs selectedHandlesEventArgs)
	{
		if (RhinoServices.StudioUICallback == null)
		{
			return;
		}
		CSPatchWireStart patchwireStart = CSUtil.GetPatchwireStartUsingCSBlock(selectedHandlesEventArgs.PrimarySelectedHandle as CSBlock);
		if (patchwireStart != null)
		{
			ImportGroup importGroup = GetGraphImportGroups().FirstOrDefault((ImportGroup importedObject) => importedObject.BlockIdentifiers.Contains(patchwireStart.InstanceGuid));
			if (importGroup != null)
			{
				GetGraphImportGroups().ForEach(delegate(ImportGroup secondaryGroup)
				{
					secondaryGroup.IsSecondarySelectedImportGroup = false;
				});
				selectedHandlesEventArgs.SecondarySelectedHandles.ForEach(delegate(ICSVHandle handle)
				{
					CSPatchWireStart pws = CSUtil.GetPatchwireStartUsingCSBlock(handle as CSBlock);
					ImportGroup importGroup2 = GetGraphImportGroups().FirstOrDefault((ImportGroup importedObject) => importedObject.BlockIdentifiers.Contains(pws.InstanceGuid));
					if (importGroup2 != null)
					{
						importGroup2.IsSecondarySelectedImportGroup = true;
					}
				});
				if (ActiveImportGroupIdentifier != importGroup.Identifier)
				{
					ActiveImportGroupIdentifier = importGroup.Identifier;
				}
			}
		}
		else
		{
			GetGraphImportGroups().ForEach(delegate(ImportGroup secondaryGroup)
			{
				secondaryGroup.IsSecondarySelectedImportGroup = false;
			});
			ActiveImportGroupIdentifier = Guid.Empty;
		}
		if (selectedHandlesEventArgs.RequiresUIUpdate)
		{
			UpdateImportGroups(updateAllImportGroupImages: false);
		}
		_ = ActiveImportGroupIdentifier;
		UpdateImportGroupBitmapImage(new List<Guid> { ActiveImportGroupIdentifier });
	}

	public static void UpdateAllImportGroupBitmapImages()
	{
		GetGraphImportGroups()?.GroupBy((ImportGroup importGroupKey) => new { importGroupKey.PartGroupId }, (key, IEnumerable<ImportGroup> values) => new
		{
			PartGroupId = key.PartGroupId,
			ImportGroups = values.ToList()
		}).ToList().ForEach(groupCollection =>
		{
			if (groupCollection.PartGroupId != Guid.Empty)
			{
				RequestScreenCaptureAtPerspectivePositionForImportGroup(_captureSize, zoom: true, groupCollection.ImportGroups.Select((ImportGroup _) => _.Identifier).ToList());
			}
			else
			{
				groupCollection.ImportGroups?.ForEach(delegate(ImportGroup importGroup)
				{
					RequestScreenCaptureAtPerspectivePositionForImportGroup(_captureSize, zoom: true, new List<Guid> { importGroup.Identifier });
				});
			}
		});
	}

	public static void UpdateFreehandStackControls()
	{
		if (GetGraphImportGroups() != null)
		{
			List<Guid> list = (from _ in GetGraphImportGroups()
				where _.IsSelected
				select _.Identifier).ToList();
			if (list.Any())
			{
				UpdateFreehandStackControlsForImportGroup(list);
			}
		}
	}

	public static void UpdateFreehandStackControlsForImportGroup(List<Guid> targetImportGroupIdentifiers)
	{
		if (RhinoServices.StudioUICallback == null)
		{
			return;
		}
		List<UIControl> orderedControlList = new List<UIControl>();
		GetImportGroupsByIdentifiers(targetImportGroupIdentifiers)?.ForEach(delegate(ImportGroup importGroup)
		{
			if (importGroup != null)
			{
				List<UIControl> list = (from csValueBase in GetDocument().Graph.AllObjects()
					where importGroup.BlockIdentifiers.Contains(csValueBase.InstanceGuid) && (csValueBase.StudioVisible || csValueBase is CSValueXFormObject)
					select csValueBase).AsEnumerable().Select(UIControlHelper.CreateUIControlFromGraphObject).ToList();
				list.ForEach(delegate(UIControl uiControl)
				{
					uiControl.ImportGroupIdentifier = importGroup.Identifier;
				});
				foreach (CSUICategory item in GetDocument().Graph.CategoryTable.Categories.OrderBy((CSUICategory category) => category.Index))
				{
					CSUICategory categoryLocal = item;
					orderedControlList.AddRange(list.Where((UIControl controls) => controls.CategoryName == categoryLocal.CategoryName && controls.ImportGroupIdentifier == categoryLocal.ImportGroupIdentifier));
				}
				orderedControlList.AddRange(list.Where((UIControl freehandControl) => freehandControl.CurrentUIControlType == UIControlType.CSValueXFormObject));
			}
		});
		RhinoServices.StudioUICallback.FreehandControlUpdateCallback(orderedControlList);
	}

	public static void UpdateFreehandStackControlsFromPatchwireStart(CSPatchWireStart csPatchWireStart)
	{
		if (csPatchWireStart != null)
		{
			ImportGroup importGroup = GetGraphImportGroups()?.FirstOrDefault((ImportGroup importedObject) => importedObject.BlockIdentifiers.Contains(csPatchWireStart.InstanceGuid));
			if (importGroup != null)
			{
				UpdateFreehandStackControlsForImportGroup(new List<Guid> { importGroup.Identifier });
			}
		}
	}

	public static void UpdateImportGroupBitmapImage(ImportGroup targetImportGroup)
	{
		if (targetImportGroup.PartGroupId == Guid.Empty)
		{
			RequestScreenCaptureAtPerspectivePositionForImportGroup(_captureSize, zoom: true, new List<Guid> { targetImportGroup.Identifier });
			return;
		}
		List<ImportGroup> importGroupsByIdentifiers = GetImportGroupsByIdentifiers(new List<Guid> { targetImportGroup.PartGroupId });
		RequestScreenCaptureAtPerspectivePositionForImportGroup(_captureSize, zoom: true, importGroupsByIdentifiers.Select((ImportGroup _) => _.Identifier).ToList());
	}

	public static void UpdateImportGroupBitmapImage(List<Guid> targetImportGroupIdentifiers)
	{
		GetImportGroupsByIdentifiers(targetImportGroupIdentifiers)?.ForEach(UpdateImportGroupBitmapImage);
	}

	public static void UpdateImportGroupMetalMaterial(List<Guid> targetImportGroupIdentifiers, bool isPrimaryMetal)
	{
		List<ImportGroup> list = GetImportGroupsByIdentifiers(targetImportGroupIdentifiers).ToList();
		if (!list.Any())
		{
			return;
		}
		list.ForEach(delegate(ImportGroup graphGroup)
		{
			CSUtil.IsRedrawDisabled = true;
			graphGroup.IsPrimaryMetal = isPrimaryMetal;
			List<string> blockInstanceCollection = graphGroup.BlockIdentifiers.Select((Guid identifier) => identifier.ToString()).ToList();
			if (isPrimaryMetal)
			{
				SetAllBlockMaterialClassificationToPrimaryMetal(blockInstanceCollection);
			}
			else
			{
				SetAllBlockMaterialClassificationToSecondaryMetal(blockInstanceCollection);
			}
			IEnumerable<CSBlock> source = from csBlock in GetGraphBlocks()
				where blockInstanceCollection.Any((string identifier) => identifier.Contains(csBlock.InstanceGuid.ToString()))
				select csBlock;
			CSUtil.IsRedrawDisabled = false;
			CSUtil.ForceGraphToRedraw(source.ToList());
			UpdateImportGroups(updateAllImportGroupImages: false);
			UpdateImportGroupBitmapImage(graphGroup);
		});
	}

	public static void UpdateImportGroups(bool updateAllImportGroupImages)
	{
		if (RhinoServices.StudioUICallback != null && GetGraphImportGroups() != null)
		{
			RhinoServices.StudioUICallback.FreehandImportGroupUpdateCallback(GetConsolidatedImportGroups());
			if (updateAllImportGroupImages)
			{
				UpdateAllImportGroupBitmapImages();
			}
		}
	}

	private static CSPatchWireFunctionResult CreatePart(string path, string name)
	{
		CSApplicationHelper.Application.InvokePartPath = path;
		CSPatchWireAttribute attributeFromDecoratedClass = CSPatchWireAttribute.GetAttributeFromDecoratedClass(typeof(CSParts), "Part");
		CSPatchWireStart cSPatchWireStart = CSUtil.CreatePatchWireStart("Add-A-Part", attributeFromDecoratedClass.Identifier, attributeFromDecoratedClass.VersionNumber);
		cSPatchWireStart.Identifier = name;
		cSPatchWireStart.UserData.AddKey("UI", "IconImagePath", "Images\\PatchWire\\Add-A-Part.png");
		cSPatchWireStart.UserData.AddKey("UI", "Description", "Add-A-Part");
		cSPatchWireStart.UserData.AddKey("UI", "ControlColorBrush", "Add-A-Part");
		PartHelper.CurrentPartName = name;
		CSPatchWireFunctionResult result = CSParts.Part();
		if (GetGraphImportGroups() == null)
		{
			CSApplicationHelper.Application.Document.Graph.ImportedCSGParts = new List<ImportGroup>();
		}
		List<Guid> blockIdentifiers = (from csObject in CSUtil.GetRange(cSPatchWireStart)
			select csObject.InstanceGuid).ToList();
		string[] array = PartHelper.CurrentPartName.Split('_');
		string name2 = ((array.Length >= 7) ? array[6] : PartHelper.CurrentPartName);
		ImportGroup importGroup = new ImportGroup
		{
			Name = name2,
			Identifier = Guid.NewGuid(),
			BlockIdentifiers = blockIdentifiers,
			IsViewportVisible = true
		};
		GetGraphImportGroups().Add(importGroup);
		CSUICategory cSUICategory = GetDocument().Graph.CategoryTable.Categories.LastOrDefault((CSUICategory category) => category.CategoryName == PartHelper.CurrentPartName);
		if (cSUICategory != null)
		{
			cSUICategory.ImportGroupIdentifier = importGroup.Identifier;
		}
		PartHelper.CurrentPartName = "Part";
		return result;
	}

	private static IEnumerable<ImportGroup> GetConsolidatedImportGroups()
	{
		List<ImportGroup> consolidatedImportGroups = new List<ImportGroup>();
		GetGraphImportGroups()?.GroupBy((ImportGroup importGroupKey) => new { importGroupKey.PartGroupId, importGroupKey.PartGroupName }, (key, IEnumerable<ImportGroup> values) => new
		{
			PartGroupId = key.PartGroupId,
			PartGroupName = key.PartGroupName,
			ImportGroups = values.ToList()
		}).ToList().ForEach(groupCollection =>
		{
			if (groupCollection.PartGroupId == Guid.Empty)
			{
				consolidatedImportGroups.AddRange(groupCollection.ImportGroups);
			}
			else
			{
				ImportGroup item = new ImportGroup
				{
					Identifier = groupCollection.PartGroupId,
					IsGroup = true,
					Name = groupCollection.PartGroupName,
					IsSelectedImportGroup = groupCollection.ImportGroups.Any((ImportGroup _) => _.IsSelectedImportGroup),
					IsSecondarySelectedImportGroup = groupCollection.ImportGroups.All((ImportGroup _) => _.IsSecondarySelectedImportGroup),
					IsViewportVisible = true,
					IsPrimaryMetal = false
				};
				consolidatedImportGroups.Add(item);
			}
		});
		return consolidatedImportGroups;
	}

	private static CSDocument GetDocument()
	{
		return CSApplicationHelper.Application.Document;
	}

	private static List<CSBlock> GetGraphBlocks()
	{
		return GetDocument().Graph.CSBlocks.ToList();
	}

	private static List<ImportGroup> GetGraphImportGroups()
	{
		return GetDocument().Graph.ImportedCSGParts;
	}

	private static IEnumerable<ImportGroup> GetImportGroupsByIdentifier(Guid targetImportGroupIdentifier)
	{
		return GetGraphImportGroups()?.Where((ImportGroup importGroup) => targetImportGroupIdentifier == importGroup.Identifier || targetImportGroupIdentifier == importGroup.PartGroupId) ?? new List<ImportGroup>();
	}

	private static List<ImportGroup> GetImportGroupsByIdentifiers(IEnumerable<Guid> targetImportGroupIdentifiers)
	{
		List<ImportGroup> list = new List<ImportGroup>();
		foreach (Guid targetImportGroupIdentifier in targetImportGroupIdentifiers)
		{
			IEnumerable<ImportGroup> importGroupsByIdentifier = GetImportGroupsByIdentifier(targetImportGroupIdentifier);
			if (importGroupsByIdentifier != null)
			{
				list.AddRange(importGroupsByIdentifier);
			}
		}
		return list;
	}

	private static List<Guid> GetSelectedImportGroupIdentifiers()
	{
		return (from part in GetSelectedImportGroups()
			select part.Identifier).ToList();
	}

	private static List<ImportGroup> GetSelectedImportGroups()
	{
		return (from _ in GetGraphImportGroups()
			where _.IsSelected
			select _).ToList();
	}

	private static void SetAllBlockMaterialClassificationToPrimaryMetal(List<string> blockInstanceCollection)
	{
		foreach (string item in blockInstanceCollection)
		{
			CS.MaterialGroup.MaterialGroupHelper.RemoveItemFromMaterialGroupByClassification(MaterialClassification.MetalSecondary, item, includeRelatedIdentifiers: true);
		}
		(from _ in GetGraphBlocks()
			where blockInstanceCollection.Contains(_.InstanceGuid.ToString()) && _.OutputDisplayMaterialClassification == MaterialClassification.MetalSecondary
			select _).ToList().ForEach(delegate(CSBlock _)
		{
			_.OutputDisplayMaterialClassification = MaterialClassification.MetalPrimary;
		});
		CS.MaterialGroup.MaterialGroupHelper.AddRangeToMaterialGroupByClassification(MaterialClassification.MetalPrimary, blockInstanceCollection);
	}

	private static void SetAllBlockMaterialClassificationToSecondaryMetal(List<string> blockInstanceCollection)
	{
		foreach (string item in blockInstanceCollection)
		{
			CS.MaterialGroup.MaterialGroupHelper.RemoveItemFromMaterialGroupByClassification(MaterialClassification.MetalPrimary, item, includeRelatedIdentifiers: true);
		}
		(from _ in GetGraphBlocks()
			where blockInstanceCollection.Contains(_.InstanceGuid.ToString()) && _.OutputDisplayMaterialClassification == MaterialClassification.MetalPrimary
			select _).ToList().ForEach(delegate(CSBlock _)
		{
			_.OutputDisplayMaterialClassification = MaterialClassification.MetalSecondary;
		});
		CS.MaterialGroup.MaterialGroupHelper.AddRangeToMaterialGroupByClassification(MaterialClassification.MetalSecondary, blockInstanceCollection);
	}
}
