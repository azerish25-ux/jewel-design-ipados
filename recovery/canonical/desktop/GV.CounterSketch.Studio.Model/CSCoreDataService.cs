using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.CAD;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Pricing;
using GV.CounterSketch.Studio.LegacyUIControl;
using GV.CounterSketch.Studio.RhinoServicesReference;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.Model;

public class CSCoreDataService
{
	private static RhinoServicesHelper _rhinoServicesHelper;

	public List<StudioGem> GetManualSizedStones()
	{
		return _rhinoServicesHelper.Client.GetManualSizedStones().ToList();
	}

	public string AbortPrintJob()
	{
		return _rhinoServicesHelper.Client.AbortPrintJob();
	}

	public void AddPropToDisplay(string pathToProp)
	{
		_rhinoServicesHelper.Client.AddPropToDisplay(pathToProp);
	}

	public bool AreSecondaryMetalObjectsVisible()
	{
		return _rhinoServicesHelper.Client.AreSecondaryMetalObjectsVisible();
	}

	public bool AreSecondaryMetalObjectsVisibleForComponent(ComponentsEnum whichComponent)
	{
		return _rhinoServicesHelper.Client.AreSecondaryMetalObjectsVisibleForComponent(whichComponent);
	}

	public void CaptureRhinoScreen()
	{
		_rhinoServicesHelper.Client.SaveScreen("D:\\Screen_" + DateTime.Now.Ticks + ".jpg");
	}

	public bool CheckConnectionToProJet1200(string connectionAddress, out string sError, out string sStatus)
	{
		return _rhinoServicesHelper.Client.CheckConnectionToProJet1200(out sError, out sStatus, connectionAddress);
	}

	public bool CheckFirmwareVersion()
	{
		return _rhinoServicesHelper.Client.CheckFirmwareVersion();
	}

	public void CheckRhinoDocumentModified(Action<bool, Exception> callback)
	{
		callback(_rhinoServicesHelper.Client.DocumentGetModified(), null);
	}

	public void ClearExpandedSliders()
	{
		_rhinoServicesHelper.Client.ClearExpandedSliders();
	}

	public void ClearHighlightedGems()
	{
		_rhinoServicesHelper.Client.ClearHighlightedGemsAsync();
	}

	public bool ClearUnifiedMesh()
	{
		return _rhinoServicesHelper.Client.ClearUnifiedMesh();
	}

	public void ConduitEnabled(bool isEnabled)
	{
		_rhinoServicesHelper.Client.ConduitEnabled(isEnabled);
	}

	public void ConduitResetTryOn()
	{
		_rhinoServicesHelper.Client.ConduitResetTryOn();
	}

	public void ConduitSetFingernailObjectMaterial(string displayObjectMaterial)
	{
		_rhinoServicesHelper.Client.ConduitSetFingernailObjectMaterial(displayObjectMaterial);
	}

	public void ConduitSetFlipRing(bool flipRing)
	{
		_rhinoServicesHelper.Client.ConduitSetFlipRing(flipRing);
	}

	public void ConduitSetHandObjectMaterial(string objectMaterialName)
	{
		_rhinoServicesHelper.Client.ConduitSetHandObjectMaterial(objectMaterialName);
	}

	public void ConduitSetScaleToRingRailObjectFlip(bool flipObject)
	{
		_rhinoServicesHelper.Client.ConduitSetScaleToRingRailObjectFlip(flipObject);
	}

	public void ConduitSetScaleToRingRailYOffset(double ringOffset)
	{
		_rhinoServicesHelper.Client.ConduitSetScaleToRingRailYOffset(ringOffset);
	}

	public bool CSCreateSupportsForCSItem(out string ErrorMessage)
	{
		return _rhinoServicesHelper.Client.CSCreateSupportsForCSItem(out ErrorMessage);
	}

	public void CurveEditorAccessCurveEditor(UIControl uiControl, bool createAsCopy, Action<IEnumerable<UIControl>, Exception> callback)
	{
		UIHelper.FocusRequestDisabled = true;
		if (Keyboard.Modifiers == ModifierKeys.Shift)
		{
			createAsCopy = true;
		}
		bool sweepPointsShow = Keyboard.Modifiers == ModifierKeys.Control;
		double chooserValue;
		NurbsCurve curve;
		double[] collection = _rhinoServicesHelper.Client.EditorGetPartsForCurveEditrFromGraphObject(out chooserValue, out curve, uiControl.BlockInstanceGuid, createAsCopy);
		CurveEditorForm curveEditorForm = new CurveEditorForm();
		curveEditorForm.SweepPointsShow = sweepPointsShow;
		curveEditorForm.CurveValue = (int)chooserValue;
		curveEditorForm.SetCurve(curve, new List<double>(collection));
		curveEditorForm.BlockInstanceGuid = uiControl.BlockInstanceGuid;
		curveEditorForm.DrawCurve();
		GlobalErrorHandler.WriteInfoToEventLog("Opening Curve Editor Window");
		curveEditorForm.ShowDialog();
		ProjectManagementHelper.IsFileModified = true;
		_rhinoServicesHelper.Client.UpdateValueChooserFromUI(uiControl.BlockInstanceGuid);
		UIControl[] arg = _rhinoServicesHelper.Client.UpdateUIControl(uiControl);
		callback(arg, null);
		UIHelper.FocusRequestDisabled = false;
	}

	public BoundingBox CurveEditorNurbsGetBoundingBox(NurbsCurve nc)
	{
		return _rhinoServicesHelper.Client.NurbsGetBoundingBox(nc);
	}

	public double CurveEditorNurbsGetClosestPoint(NurbsCurve nc, Point3D testHit)
	{
		return _rhinoServicesHelper.Client.NurbsGetClosestPoint(nc, testHit);
	}

	public Point3D CurveEditorNurbsGetNormalizedPointAt(NurbsCurve nc, double editPointParam)
	{
		return _rhinoServicesHelper.Client.NurbsGetNormalizedPointAt(nc, editPointParam);
	}

	public IEnumerable<Point2D> CurveEditorNurbsGetPolyline(NurbsCurve nc)
	{
		return _rhinoServicesHelper.Client.NurbsGetPolyline(nc);
	}

	public NurbsCurve CurveEditorNurbsRebuild(NurbsCurve nc, int i, int i1)
	{
		return _rhinoServicesHelper.Client.NurbsRebuild(nc, i, i1);
	}

	public NurbsCurve CurveEditorNurbsSetDomain(NurbsCurve nc, int i, int i1)
	{
		return _rhinoServicesHelper.Client.NurbsSetDomain(nc, i, i1);
	}

	public void CurveEditorUpdateCurveObjectFromUI(Guid blockInstanceGuid, int curveValue, NurbsCurve nc2, double[] toArray, bool b)
	{
		_rhinoServicesHelper.Client.UpdateCurveObjectFromUI(blockInstanceGuid, curveValue, nc2, toArray, b);
	}

	public bool DeleteSavedJob(int whatjob)
	{
		return _rhinoServicesHelper.Client.DeleteSavedJob(whatjob);
	}

	public void DeselectViewportControls()
	{
		_rhinoServicesHelper.Client.DeselectViewportControlsAsync();
		RedrawRhino();
	}

	public void DisplayConduitSetEnabled(bool enabled)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			_rhinoServicesHelper.Client.DisplayConduitSetEnabled(enabled);
		});
	}

	public void Dispose()
	{
		if (_rhinoServicesHelper != null)
		{
			if (_rhinoServicesHelper.Client != null && _rhinoServicesHelper.Client.State != CommunicationState.Faulted)
			{
				_rhinoServicesHelper.Client.ShutdownAsync();
			}
			_rhinoServicesHelper.Dispose();
		}
	}

	public void DocumentCenterDesign()
	{
		_rhinoServicesHelper.Client.DocumentSetCameraView("Perspective");
	}

	public void DocumentClearAllAnyGemAnyColor()
	{
		_rhinoServicesHelper.Client.DocumentClearAllAnyGemAnyColorCompleted += Client_DocumentClearAllAnyGemAnyColorCompleted;
		ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: true);
		ProjectManagementHelper.IsFileModified = true;
		_rhinoServicesHelper.Client.DocumentClearAllAnyGemAnyColorAsync();
	}

	public void DocumentClearUndo()
	{
		_rhinoServicesHelper.Client.DocumentClearUndo();
	}

	public bool DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(GemMaterial gemMaterial)
	{
		ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: true);
		Application.Current.Dispatcher.Invoke(delegate
		{
			ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: true);
		});
		ProjectManagementHelper.IsFileModified = true;
		bool isCenterStoneAssigned = ViewModelLocator.Current?.StoneTrayVM != null && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned;
		bool result = _rhinoServicesHelper.Client.DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(gemMaterial, isCenterStoneAssigned);
		ViewModelLocator current = ViewModelLocator.Current;
		if (current != null)
		{
			current.MainVM.SetIsRhinoBusy(isBusy: false);
			return result;
		}
		return result;
	}

	public double DocumentGetRingSizeIndexFromSizeText(string ringSizeText)
	{
		return _rhinoServicesHelper.Client.DocumentGetRingSizeIndexFromSizeText(ringSizeText);
	}

	public string DocumentGetRingSizeTextFromSizeIndex(double ringSizeIndex)
	{
		return _rhinoServicesHelper.Client.DocumentGetRingSizeTextFromSizeIndex(ringSizeIndex);
	}

	public double DocumentGetRingSizeTranslated(double ringSizeInMillimeters)
	{
		return _rhinoServicesHelper.Client.DocumentGetRingSizeTranslated(ringSizeInMillimeters);
	}

	public List<StudioGem> DocumentGetStudioGemList()
	{
		return _rhinoServicesHelper.Client.DocumentGetStudioGemList().ToList();
	}

	public void DocumentGraphGenerate()
	{
		_rhinoServicesHelper.Client.DocumentGraphGenerate();
	}

	public void DocumentImportCSGPart(string pathToCSGPart, int? centerGemControlGraphIndex = null, bool doShowFontSourceErrors = true)
	{
		ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: true);
		ProjectManagementHelper.IsFileModified = true;
		_rhinoServicesHelper.Client.DocumentImportCSGPartAsync(pathToCSGPart, centerGemControlGraphIndex, doShowFontSourceErrors);
		if (ViewModelLocator.Current != null)
		{
			ViewModelLocator.Current.FreehandStackVM.IsFreehandCheckBoxActive = true;
		}
	}

	public void DocumentReset()
	{
		_rhinoServicesHelper.Client.DocumentReset();
	}

	public void DocumentResetCenterGemAnyGemAnyColorMaterialAssignment()
	{
		_rhinoServicesHelper.Client.DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentAsync();
	}

	public bool DocumentRhino5Check()
	{
		string text = _rhinoServicesHelper.Client.DocumentUserDataFindValue("Version", "Rhino");
		if (!string.IsNullOrWhiteSpace(text))
		{
			string[] source = text.Split('.');
			if (source.Any())
			{
				string text2 = source.FirstOrDefault();
				if (!string.IsNullOrWhiteSpace(text2))
				{
					return text2 == "5";
				}
			}
		}
		return false;
	}

	public void DocumentSetCameraView(string documentSetCameraView)
	{
		_rhinoServicesHelper.Client.DocumentSetCameraView(documentSetCameraView);
	}

	public void DocumentSetCurvesAndPointsVisibility(bool visible)
	{
		_rhinoServicesHelper.Client.DocumentSetCurvesAndPointsVisibilityAsync(visible);
	}

	public void DocumentSetFilename(string filename)
	{
		_rhinoServicesHelper.Client.DocumentSetFilenameAsync(filename);
	}

	public void DocumentSetModified(bool modified)
	{
		_rhinoServicesHelper.Client.DocumentSetModifiedAsync(modified);
	}

	public void DocumentSetRingSizeSetting(string ringSizeSetting)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			_rhinoServicesHelper.Client.DocumentSetRingSizeSetting(ringSizeSetting);
		});
	}

	public void DocumentSetShowCurves(bool visible)
	{
		_rhinoServicesHelper.Client.DocumentSetShowCurves(visible);
	}

	public void DocumentSetShowStarts(bool visible)
	{
		_rhinoServicesHelper.Client.DocumentSetShowStarts(visible);
	}

	public void DocumentSpinDesign(bool isEnabled)
	{
		_rhinoServicesHelper.Client.DocumentSpinDesign(isEnabled);
	}

	public void DocumentToggleGemSelectionMode(bool isEnabled)
	{
		_rhinoServicesHelper.Client.DocumentToggleGemSelectionModeAsync(isEnabled);
	}

	public void DocumentUndoBeginRecord()
	{
		_rhinoServicesHelper.Client.DocumentUndoBeginRecord();
	}

	public void DocumentUserDataAddValue(string typeName, string keyName, string value)
	{
		_rhinoServicesHelper.Client.DocumentUserDataAddValue(typeName, keyName, value);
	}

	public string DocumentUserDataFindValue(string typeName, string keyName)
	{
		return _rhinoServicesHelper.Client.DocumentUserDataFindValue(typeName, keyName);
	}

	public string DocumentUserDataGetOrderData()
	{
		return _rhinoServicesHelper.Client.DocumentUserDataGetOrderData();
	}

	public List<Tuple<string, string, string>> DocumentUserDataGetAllElements()
	{
		return _rhinoServicesHelper.Client.DocumentUserDataGetAllDataElements().ToList();
	}

	public void DocumentWrite(string path)
	{
		_rhinoServicesHelper.Client.DocumentWrite(path, DebuggingHelper.GetCurrentUserLicenseInfo());
	}

	public void DocumentZoomCameraToBoundingBox()
	{
		_rhinoServicesHelper.Client.DocumentZoomCameraToBoundingBoxAsync();
	}

	public void EnableDrawing(bool isEnabled)
	{
		_rhinoServicesHelper.Client.EnableDrawing(isEnabled);
	}

	public bool ExportDesignAs3DM(string savePath, bool includeCenter, bool includeSide, bool includeAccent)
	{
		return _rhinoServicesHelper.Client.ExportDesignAs3DM(savePath, includeCenter, includeSide, includeAccent);
	}

	public bool ExportDesignAsSTL(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh)
	{
		return _rhinoServicesHelper.Client.ExportDesignAsSTL(savePath, includeCenter, includeSide, includeAccent, repairMesh);
	}

	public bool FindProjetPrinter(out string FindAddress)
	{
		return _rhinoServicesHelper.Client.FindProjetPrinter(out FindAddress);
	}

	public bool GetActivationState()
	{
		return _rhinoServicesHelper.Client.GetActivationState();
	}

	public IEnumerable<KeyValuePair<int, string>> GetAllRingSizesForCurrentRingSizeRegion()
	{
		return _rhinoServicesHelper.Client.GetAllRingSizesForCurrentRingSizeRegion();
	}

	public void GetComponents(Action<IEnumerable<ComponentsEnum>> callback)
	{
		callback(_rhinoServicesHelper.Client.GetComponents());
	}

	public void GetCurrentViewportImageBitmap(Action<byte[]> callback)
	{
		callback(_rhinoServicesHelper.Client.GetCurrentViewportImageBitmap());
	}

	public string GetDesignerFromUserData()
	{
		return _rhinoServicesHelper.Client.GetDesignerFromUserData();
	}

	public int GetDesignIterationCount()
	{
		return _rhinoServicesHelper.Client.GetDesignIterationCount();
	}

	public string GetDocumentFilename()
	{
		return _rhinoServicesHelper.Client.GetDocumentFilename();
	}

	public List<int> GetFeaturedDesignIterations()
	{
		return _rhinoServicesHelper.Client.GetFeaturedDesignIterations().ToList();
	}

	public Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> GetMetalVolumesPerComponent(bool hasSingleMetalCastingCount, int singleMetalCastingCount)
	{
		return _rhinoServicesHelper.Client.GetMetalVolumesPerComponent(hasSingleMetalCastingCount, singleMetalCastingCount);
	}

	public string GetNotesToCustomerForCurrentDesign()
	{
		return _rhinoServicesHelper.Client.GetNotesToCustomerForCurrentDesign();
	}

	public int GetNumberofVisibleFreehandParts()
	{
		return _rhinoServicesHelper.Client.GetNumberofVisibleFreehandParts();
	}

	public bool GetPrinterMaterial(out string message)
	{
		return _rhinoServicesHelper.Client.GetPrinterMaterial(out message);
	}

	public string GetRhinoSerialCode()
	{
		return _rhinoServicesHelper.Client.GetRhinoSerialCode();
	}

	public int GetRhinoThreadId()
	{
		return _rhinoServicesHelper.Client.GetRhinoThreadId();
	}

	public IntPtr GetRhinoWindowHandle()
	{
		return _rhinoServicesHelper.Client.GetRhinoWindowHandle();
	}

	public List<int> GetSavedJobs()
	{
		return _rhinoServicesHelper.Client.GetSavedJobs().ToList();
	}

	public string GetSPINumberFromUserData()
	{
		return _rhinoServicesHelper.Client.GetSPINumberFromUserData();
	}

	public void GetThumbnailFromFile(string filename, Action<BitmapSourceByteArrayInfo> callback)
	{
		Application.Current.Dispatcher.Invoke(delegate
		{
			callback(_rhinoServicesHelper.Client.GetThumbnailFromFile(filename));
		});
	}

	public void GetVisibleComponents(Action<IEnumerable<ComponentsEnum>> callback)
	{
		callback(_rhinoServicesHelper.Client.GetVisibleComponents());
	}

	public string GraphItemFindUserDataValue(Guid blockInstanceGuid, string typeName, string keyName)
	{
		return _rhinoServicesHelper.Client.GraphItemFindUserDataValue(blockInstanceGuid, typeName, keyName);
	}

	public void GraphItemAddUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue)
	{
		_rhinoServicesHelper.Client.GraphItemAddUserDataValueAsync(blockInstanceGuid, typeName, keyName, value, replaceCurrentValue);
	}

	public void GraphItemRemoveUserDataValue(Guid blockInstanceGuid, string typeName, string keyName)
	{
		_rhinoServicesHelper.Client.GraphItemRemoveUserDataValue(blockInstanceGuid, typeName, keyName);
	}

	public void GraphRedoChange(Action<IEnumerable<UIControl>, Exception> callback)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			callback(_rhinoServicesHelper.Client.GraphRedoChange(), null);
		});
	}

	public void GraphUndoChange(Action<IEnumerable<UIControl>, Exception> callback)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			callback(_rhinoServicesHelper.Client.GraphUndoChange(), null);
		});
	}

	public void HighlightAccentGems(int gemId, bool highlightAll)
	{
		if (!UIHelper.CounterSketchOptions.DisableViewportHighlight)
		{
			_rhinoServicesHelper.Client.HighlightAccentGemsAsync(gemId, highlightAll);
		}
	}

	public void HighlightGemFromGemReportByGemNumber(List<string> gemNumbers)
	{
		if (!UIHelper.CounterSketchOptions.DisableViewportHighlight)
		{
			ViewportSetHighlightColor(Color.FromArgb(255, 6, 98, 235));
			_rhinoServicesHelper.Client.ClearHighlightedGemsAsync();
			_rhinoServicesHelper.Client.HighlightGemFromGemReportByGemNumberAsync(gemNumbers.ToArray());
		}
	}

	public void HighlightGemFromGemReportByIndex(List<int> gemIndex)
	{
		if (!UIHelper.CounterSketchOptions.DisableViewportHighlight)
		{
			ViewportSetHighlightColor(Color.FromArgb(255, 6, 98, 235));
			_rhinoServicesHelper.Client.ClearHighlightedGemsAsync();
			_rhinoServicesHelper.Client.HighlightGemFromGemReportByIndexAsync(gemIndex.ToArray());
		}
	}

	public void HighlightItemsByBlockInstanceGuid(IEnumerable<Guid> instanceGuids)
	{
		if (!UIHelper.CounterSketchOptions.DisableViewportHighlight)
		{
			_rhinoServicesHelper.Client.HighlightItemsByBlockInstanceGuidAsync(instanceGuids?.ToArray());
		}
	}

	public void HighlightRhinoDocumentGraphItem(Guid blockInstanceGuid, bool isHighlighted)
	{
		if (!UIHelper.CounterSketchOptions.DisableViewportHighlight)
		{
			_rhinoServicesHelper.Client.HighlightGraphItemAsync(blockInstanceGuid, isHighlighted);
		}
	}

	public void ImportGroupAddRingRailGuide()
	{
		_rhinoServicesHelper.Client.ImportGroupAddRingRailGuideAsync();
		ViewModelLocator.Current.FreehandStackVM.IsFreehandCheckBoxActive = true;
	}

	public void ImportGroupAddToPartGroup(List<Guid> importGroupIdentifiers)
	{
		_rhinoServicesHelper.Client.ImportGroupAddToPartGroup(importGroupIdentifiers.ToArray());
	}

	public void ImportGroupBreakPartGroup(Guid importGroupIdentifier)
	{
		_rhinoServicesHelper.Client.ImportGroupBreakPartGroup(importGroupIdentifier);
	}

	public void ImportGroupDeleteFromGraph(List<Guid> importGroupIdentifiers, Action<IEnumerable<UIControl>, Exception> callback)
	{
		ProjectManagementHelper.IsFileModified = true;
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			callback(_rhinoServicesHelper.Client.DeleteImportedGroupFromGraph(importGroupIdentifiers.ToArray()), null);
		});
	}

	public void ImportGroupDuplicate(List<Guid> importGroupIdentifiers)
	{
		ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: true);
		ProjectManagementHelper.IsFileModified = true;
		_rhinoServicesHelper.Client.DocumentDuplicateCSGPartAsync(importGroupIdentifiers.ToArray());
	}

	public void ImportGroupExport(List<Guid> targetImportGroupIdentifiers, string path)
	{
		_rhinoServicesHelper.Client.ExportImportGroups(targetImportGroupIdentifiers.ToArray(), path);
	}

	public void ImportGroupRename(Guid importGroupIdentifier, string updatedName)
	{
		_rhinoServicesHelper.Client.ImportGroupRename(importGroupIdentifier, updatedName);
	}

	public void ImportGroupRequestScreenCaptureAtPerspectivePosition(System.Drawing.Size captureSize, bool zoom, List<Guid> importGroupIdentifiers)
	{
		_rhinoServicesHelper.Client.RequestScreenCaptureAtPerspectivePositionForImportGroupAsync(captureSize, zoom, importGroupIdentifiers.ToArray());
	}

	public void ImportGroupSelectByIdentifier(Guid importGroupIdentifier)
	{
		_rhinoServicesHelper.Client.SelectImportedGroupByIdentifierAsync(importGroupIdentifier);
	}

	public void ImportGroupSelectSecondaryByIdentifier(List<Guid> importGroupIdentifiers)
	{
		_rhinoServicesHelper.Client.SelectImportedGroupByIdentifierSecondaryAsync(importGroupIdentifiers.ToArray());
	}

	public void ImportGroupSetToPrimaryMetal(List<Guid> importGroupIdentifiers)
	{
		_rhinoServicesHelper.Client.SetImportGroupToPrimaryMetal(importGroupIdentifiers.ToArray());
	}

	public void ImportGroupSetToSecondaryMetal(List<Guid> importGroupIdentifiers)
	{
		_rhinoServicesHelper.Client.SetImportGroupToSecondaryMetal(importGroupIdentifiers.ToArray());
	}

	public void ImportGroupSetVisibilityByIdentifier(List<Guid> importGroupIdentifiers, bool isViewportVisible)
	{
		_rhinoServicesHelper.Client.SetImportGroupVisibilityByIdentifier(importGroupIdentifiers.ToArray(), isViewportVisible);
	}

	public void ImportGroupUpdateSortOrder(List<Guid> importGroupGuids)
	{
		_rhinoServicesHelper.Client.UpdateImportGroupSortOrderAsync(importGroupGuids.ToArray());
	}

	public void InitializeProjet()
	{
	}

	public void InitializeRhinoProperties(string csLibraryPath, string csRootPath)
	{
		_rhinoServicesHelper.Client.InitializeRhinoProperties(csLibraryPath);
	}

	public void InvokeGemPatchWire(GemActions action, Action<IEnumerable<UIControl>, Exception> callback)
	{
		ProjectManagementHelper.IsFileModified = true;
	}

	public void InvokePartPatchWire(string path, string name, Action<IEnumerable<UIControl>, Exception> callback)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			callback(_rhinoServicesHelper.Client.InvokeAddAPartPatchwire(path, name), null);
		});
	}

	public bool IsComponentsEnumVisible(ComponentsEnum componentEnum)
	{
		return _rhinoServicesHelper.Client.IsComponentsEnumVisible(componentEnum);
	}

	public bool IsDisplayConduitEnabled()
	{
		return _rhinoServicesHelper.Client.IsDisplayConduitEnabled();
	}

	public bool IsDocumentCanvasEmpty()
	{
		return _rhinoServicesHelper.Client.IsDocumentCanvasEmpty();
	}

	public bool IsManualSizingModeOn(bool excludeCenterStone)
	{
		return _rhinoServicesHelper.Client.IsManualSizingModeOn(excludeCenterStone);
	}

	public bool IsProjetDialogLoaded()
	{
		return _rhinoServicesHelper.Client.IsProjetDialogLoaded();
	}

	public bool IsRhino64Bit()
	{
		return _rhinoServicesHelper.Client.IsRhino64Bit();
	}

	public void MaterialsGetGem(Action<GemMaterial[], Exception> callback)
	{
		callback(_rhinoServicesHelper.Client.GetGemMaterials(), null);
	}

	public void MaterialsGetMetal(Action<MetalMaterial[], Exception> callback)
	{
		callback(_rhinoServicesHelper.Client.GetMetalMaterials(), null);
	}

	public Dictionary<Tuple<ComponentsEnum, MaterialClassification>, byte[]> MaterialsGetMetalDisplayObjectBinObjFiles()
	{
		MetalMaterial[] metalMaterials = _rhinoServicesHelper.Client.GetMetalMaterials();
		if (metalMaterials == null)
		{
			return null;
		}
		MetalMaterial metalMaterial = metalMaterials[0];
		MetalMaterial metalMaterial2 = metalMaterials[1];
		Dictionary<Tuple<ComponentsEnum, MaterialClassification>, byte[]> dictionary = new Dictionary<Tuple<ComponentsEnum, MaterialClassification>, byte[]>();
		ComponentsEnum[] visibleComponents = _rhinoServicesHelper.Client.GetVisibleComponents();
		foreach (ComponentsEnum componentsEnum in visibleComponents)
		{
			switch (componentsEnum)
			{
			case ComponentsEnum.Ring:
			case ComponentsEnum.Pendant:
			{
				byte[] binObjFile10;
				if (UIHelper.CoreDataService.AreSecondaryMetalObjectsVisibleForComponent(componentsEnum))
				{
					if (metalMaterial == metalMaterial2)
					{
						byte[] binObjFile7;
						if (CastingHelper.TryGetSingleMetalCastingCount(UIHelper.LoadedDesign.DesignId, out var singleMetalCastingCount))
						{
							if (singleMetalCastingCount == 1)
							{
								if (MaterialsGetMetalDisplayObjectBinObjFile(out var binObjFile4, componentsEnum, MaterialClassification.MetalPrimary, MaterialClassification.MetalSecondary))
								{
									dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalPrimary), binObjFile4);
								}
								break;
							}
							if (MaterialsGetMetalDisplayObjectBinObjFile(out var binObjFile5, componentsEnum, MaterialClassification.MetalPrimary))
							{
								dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalPrimary), binObjFile5);
							}
							if (MaterialsGetMetalDisplayObjectBinObjFile(out var binObjFile6, componentsEnum, MaterialClassification.MetalSecondary))
							{
								dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalSecondary), binObjFile6);
							}
						}
						else if (MaterialsGetMetalDisplayObjectBinObjFile(out binObjFile7, componentsEnum, MaterialClassification.MetalPrimary, MaterialClassification.MetalSecondary))
						{
							dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalPrimary), binObjFile7);
						}
					}
					else
					{
						if (MaterialsGetMetalDisplayObjectBinObjFile(out var binObjFile8, componentsEnum, MaterialClassification.MetalPrimary))
						{
							dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalPrimary), binObjFile8);
						}
						if (MaterialsGetMetalDisplayObjectBinObjFile(out var binObjFile9, componentsEnum, MaterialClassification.MetalSecondary))
						{
							dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalSecondary), binObjFile9);
						}
					}
				}
				else if (MaterialsGetMetalDisplayObjectBinObjFile(out binObjFile10, componentsEnum, MaterialClassification.MetalPrimary))
				{
					dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalPrimary), binObjFile10);
				}
				break;
			}
			case ComponentsEnum.Band1:
			case ComponentsEnum.Band2:
			{
				byte[] binObjFile3;
				if (UIHelper.CoreDataService.AreSecondaryMetalObjectsVisibleForComponent(componentsEnum))
				{
					if (MaterialsGetMetalDisplayObjectBinObjFile(out var binObjFile, componentsEnum, MaterialClassification.MetalPrimary))
					{
						dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalPrimary), binObjFile);
					}
					if (MaterialsGetMetalDisplayObjectBinObjFile(out var binObjFile2, componentsEnum, MaterialClassification.MetalSecondary))
					{
						dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalSecondary), binObjFile2);
					}
				}
				else if (MaterialsGetMetalDisplayObjectBinObjFile(out binObjFile3, componentsEnum, MaterialClassification.MetalPrimary))
				{
					dictionary.Add(new Tuple<ComponentsEnum, MaterialClassification>(componentsEnum, MaterialClassification.MetalPrimary), binObjFile3);
				}
				break;
			}
			default:
				GlobalErrorHandler.WriteInfoToEventLog($"Invalid ComponentEnum type of {componentsEnum}", 2);
				break;
			}
		}
		return dictionary;
	}

	public void MaterialsSetGem(GemMaterial[] gemMaterials)
	{
		_rhinoServicesHelper.Client.SetGemMaterials(gemMaterials);
		ProjectManagementHelper.IsFileModified = true;
	}

	public void MaterialsSetMetal(MetalMaterial[] metalMaterials)
	{
		_rhinoServicesHelper.Client.SetMetalMaterials(metalMaterials);
		ProjectManagementHelper.IsFileModified = true;
	}

	public void OpenDocument(string pathToCSG, Action<IEnumerable<UIControl>, Exception> callback, Guid centerGemControlGuid = default(Guid), int? centerGemControlGraphIndex = null)
	{
		double largeStoneCaratMax = ((UIHelper.KeyProfile.KeyProfileId == 2) ? 3.0 : 100.0);
		Application.Current.Dispatcher.Invoke(delegate
		{
			callback(_rhinoServicesHelper.Client.OpenDocument(pathToCSG, UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled, largeStoneCaratMax, centerGemControlGuid, centerGemControlGraphIndex), null);
		});
	}

	public void OpenDocumentNoDisplay(string pathToCSG, Guid centerGemControlGuid = default(Guid), int? centerGemControlGraphIndex = null)
	{
		double largeStoneCaratMax = ((UIHelper.KeyProfile.KeyProfileId == 2) ? 3.0 : 100.0);
		_rhinoServicesHelper.Client.OpenDocument(pathToCSG, UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled, largeStoneCaratMax, centerGemControlGuid, centerGemControlGraphIndex);
	}

	public void OrderCheckNonCenterGemsExist(Action<bool> callback)
	{
		callback(_rhinoServicesHelper.Client.OrderCheckNonCenterGemsExist());
	}

	public void OrderGetAccentGemData(Action<IEnumerable<OrderGemData>> callback)
	{
		callback(_rhinoServicesHelper.Client.OrderGetAccentGemData());
	}

	public IEnumerable<OrderGemData> OrderGetAllGemData()
	{
		return _rhinoServicesHelper.Client.OrderGetAllGemData();
	}

	public void OrderGetCurrentRingSizes(Action<List<string>> callback)
	{
		callback(_rhinoServicesHelper.Client.OrderGetCurrentRingSizes().ToList());
	}

	public void OrderGetCurrentRingSizesDiameter(Action<List<double>> callback)
	{
		callback(_rhinoServicesHelper.Client.OrderGetCurrentRingSizesDiameter().ToList());
	}

	public void OrderGetCurrentRingSizesUom(Action<List<string>> callback)
	{
		callback(_rhinoServicesHelper.Client.OrderGetCurrentRingSizesUom().ToList());
	}

	public void OrderGetExpandedSliderList(Action<IEnumerable<string>> callback)
	{
		callback(_rhinoServicesHelper.Client.OrderGetExpandedSliderList());
	}

	public void PerformDistanceMeasurement()
	{
		_rhinoServicesHelper.Client.PerformDistanceMeasurementAsync();
	}

	public void PrintingSupportDisplayConduitEnabled(bool isEnabled)
	{
		_rhinoServicesHelper.Client.PrintingSupportDisplayConduitEnabled(isEnabled);
	}

	public bool PrintMultipleJobs(out string sStatus, int[] lstJobsToPrin)
	{
		return _rhinoServicesHelper.Client.PrintMultipleJobs(out sStatus, lstJobsToPrin);
	}

	public bool PrintUnifiedMeshToProJet1200(out string sErr)
	{
		return _rhinoServicesHelper.Client.PrintUnifiedMeshToProJet1200(out sErr);
	}

	public void RedrawRhino()
	{
		_rhinoServicesHelper.Client.RedrawRhinoAsync();
	}

	public void RemovePropFromDisplay()
	{
		_rhinoServicesHelper.Client.RemovePropFromDisplay();
	}

	public void RenderAbort()
	{
	}

	public void RenderGenerateRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane)
	{
		_rhinoServicesHelper.Client.RenderGenerateRender(isRenderLayDesignFlat, isRenderUsePropsSelected, renderPropSwatchCollectionItem, renderPropMaterialSwatchCollectionItem, renderPropMaterialSwatchCollectionSecondaryItem, groundPlaneMaterial, renderResolution, renderEnvironment, saveRender, saveRenderLocation, alphaProps, alphaGroundPlane);
	}

	public void RepaintRhino()
	{
		_rhinoServicesHelper.Client.RepaintRhino();
	}

	public Bitmap RequestScreenCapture(System.Drawing.Size captureSize)
	{
		return _rhinoServicesHelper.Client.RequestScreenCapture(captureSize);
	}

	public Bitmap RequestScreenCaptureAndFreeze(System.Drawing.Size captureSize)
	{
		return _rhinoServicesHelper.Client.RequestScreenCaptureAndFreeze(captureSize);
	}

	public Bitmap RequestScreenCaptureDefault(System.Drawing.Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly = false)
	{
		return _rhinoServicesHelper.Client.RequestScreenCaptureDefault(captureSize, zoom, zoomPadding, isMainDesignOnly);
	}

	public Bitmap RequestScreenCaptureFront(System.Drawing.Size captureSize)
	{
		return _rhinoServicesHelper.Client.RequestScreenCaptureFront(captureSize);
	}

	public Bitmap RequestScreenCaptureSide(System.Drawing.Size captureSize)
	{
		return _rhinoServicesHelper.Client.RequestScreenCaptureSide(captureSize);
	}

	public Bitmap RequestScreenCaptureTop(System.Drawing.Size captureSize)
	{
		return _rhinoServicesHelper.Client.RequestScreenCaptureTop(captureSize);
	}

	public void ReselectViewportControls()
	{
		_rhinoServicesHelper.Client.ReselectViewportControlsAsync();
		RedrawRhino();
	}

	public void ResetHandleForImportGroup(Guid importGroupIdentifier)
	{
		_rhinoServicesHelper.Client.ResetHandleForImportGroup(importGroupIdentifier);
	}

	public void RhinoAbortScripts()
	{
		_rhinoServicesHelper.Client.RhinoAbortScripts();
	}

	public void RhinoCloseViewPort(string nameNewViewport)
	{
		_rhinoServicesHelper.Client.RhinoCloseViewPort(nameNewViewport);
	}

	public IntPtr RhinoCreateViewPort(string nameNewViewport)
	{
		return _rhinoServicesHelper.Client.RhinoCreateViewPort(nameNewViewport);
	}

	public IntPtr RhinoGetActiveViewWindowHandle()
	{
		return _rhinoServicesHelper.Client.RhinoGetActiveViewWindowHandle();
	}

	public void RhinoGetVersion(Action<string> callback)
	{
		callback(_rhinoServicesHelper.Client.RhinoGetVersion());
	}

	public void RhinoLoadBlankDocument()
	{
		_rhinoServicesHelper.Client.RhinoLoadBlankDocument();
	}

	public void RhinoRunScript(string scriptText)
	{
		_rhinoServicesHelper.Client.RhinoRunScript(scriptText);
	}

	public void RhinoSetFocusTo()
	{
		_rhinoServicesHelper.Client.RhinoSetFocusTo();
	}

	public bool RhinoWindowReparent(IntPtr intPtrParent)
	{
		return _rhinoServicesHelper.Client.RhinoWindowReparent(intPtrParent);
	}

	public void RhinoWindowReposition(int startPointX, int startPointY, int width, int height)
	{
	}

	public bool SavePrintJob(out string sStatus, int whatslot)
	{
		return _rhinoServicesHelper.Client.SavePrintJob(out sStatus, whatslot);
	}

	public bool SaveViewportToImage(string viewportName, string pathToSaveImage, double maxHeightWidth)
	{
		return _rhinoServicesHelper.Client.SaveViewportToImage(viewportName, pathToSaveImage, maxHeightWidth);
	}

	public void SetCAMModeState(bool CAMModeOn)
	{
		_rhinoServicesHelper.Client.SetCAMModeStateAsync(CAMModeOn);
	}

	public void SetComponentVisibility(ComponentsEnum componentsEnum, bool visible)
	{
		if (visible)
		{
			if (!UIHelper.VisibleComponents.Contains(componentsEnum))
			{
				UIHelper.VisibleComponents.Add(componentsEnum);
			}
		}
		else
		{
			UIHelper.VisibleComponents.Remove(componentsEnum);
		}
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			_rhinoServicesHelper.Client.SetComponentVisibility(componentsEnum, visible);
		});
	}

	public void SetDesignIteration(int? iterationId, Action<IEnumerable<UIControl>, Exception> callback)
	{
		if (iterationId.HasValue)
		{
			Application.Current.Dispatcher.BeginInvoke((Action)delegate
			{
				callback(_rhinoServicesHelper.Client.SetDesignIteration(iterationId.Value), null);
			});
		}
	}

	public void SetDesignIteration(int? iterationId)
	{
		if (iterationId.HasValue)
		{
			_rhinoServicesHelper.Client.SetDesignIteration(iterationId.Value);
		}
	}

	public void SetKeepAliveProcessIds(int id)
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			RhinoServicesHelper.ServerProcessId = _rhinoServicesHelper.Client.SetKeepAliveProcessIds(id);
		});
	}

	public void SetLoadedDialogState(bool bState)
	{
		_rhinoServicesHelper.Client.SetLoadedDialogState(bState);
	}

	public bool SetMeshFromComponent(List<ComponentsEnum> whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed, out string sError)
	{
		return _rhinoServicesHelper.Client.SetMeshFromComponent(out sError, whichComponents.ToArray(), isCenterGemAllowed, isSideGemAllowed, isAccentGemAllowed);
	}

	public void SetPerspectiveViewActiveAndMaximize()
	{
		_rhinoServicesHelper.Client.SetPerspectiveViewActiveAndMaximize();
	}

	public void SetSupportDensity(int iSupportDensity)
	{
		_rhinoServicesHelper.Client.SetSupportDensity(iSupportDensity);
	}

	public bool SetSupportScales(double SupScale, int SupType)
	{
		return _rhinoServicesHelper.Client.SetSupportScales(SupScale, SupType);
	}

	public void SetUIControl(UIControl uiControl, bool quickSet, Action<IEnumerable<UIControl>, Exception> callback)
	{
		if (!_rhinoServicesHelper.IsSetUIQuickBusy || !quickSet)
		{
			if (quickSet)
			{
				_rhinoServicesHelper.IsSetUIQuickBusy = true;
				_rhinoServicesHelper.Client.UpdateUIControlQuickAsync(uiControl);
				_rhinoServicesHelper.IsSetUIQuickBusy = false;
			}
			else
			{
				ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: true);
				_rhinoServicesHelper.Client.UpdateUIControlAsync(uiControl);
			}
		}
	}

	public void SetUnifiedMeshLocation(double dHeight)
	{
		_rhinoServicesHelper.Client.SetUnifiedMeshLocation(dHeight);
	}

	public void SetUnifiedMeshRotation(double angleX, double angleY, double angleZ)
	{
		_rhinoServicesHelper.Client.SetUnifiedMeshRotation(angleX, angleY, angleZ);
	}

	public void ShutdownRhino()
	{
		try
		{
			_rhinoServicesHelper.Client.ShutdownAsync();
		}
		catch (Exception)
		{
		}
	}

	public bool TestServerAccess()
	{
		if (("Z" + _rhinoServicesHelper.Client.RhinoGetVersion()).Length > 1)
		{
			return true;
		}
		return false;
	}

	public void ToggleFreehandCommonCenterMode(bool isFreehandCommonCenterModeEnabled)
	{
		_rhinoServicesHelper.Client.ToggleFreehandCommonCenterMode(isFreehandCommonCenterModeEnabled);
	}

	public void ToggleFreehandVisibility(bool isVisible)
	{
		_rhinoServicesHelper.Client.ToggleFreehandVisibility(isVisible);
	}

	public bool GetFreehandComponentVisibility()
	{
		return _rhinoServicesHelper.Client.GetFreehandComponentVisibility();
	}

	public void ToggleQuarterCaratGemRounding(bool isQuarterCaratRoundingEnabled)
	{
		_rhinoServicesHelper.Client.ToggleQuarterCaratGemRoundingAsync(isQuarterCaratRoundingEnabled);
	}

	public void ToggleRhinoViewportAxis(bool enableAxis)
	{
		_rhinoServicesHelper.Client.DocumentShowAxis(enableAxis);
	}

	public void ToggleRhinoViewportGridlines(bool enableGridlines)
	{
		_rhinoServicesHelper.Client.DocumentShowGrid(enableGridlines);
	}

	public void ToggleViewportControls(bool isVisible)
	{
		_rhinoServicesHelper.Client.ToggleViewportControlsAsync(isVisible);
	}

	public void TurnHandOff()
	{
		_rhinoServicesHelper.Client.TurnHandOff();
	}

	public bool UpdateFirmwareAndActivate(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany, out string sError)
	{
		return _rhinoServicesHelper.Client.UpdateFirmwareAndActivate(out sError, bDoFirmware, bDoActivate, sEmail, sCompany);
	}

	public void UpdateRhinoAppearanceMaterials(string skinName, string polishName, bool isRenderLayDesignFlat)
	{
		_rhinoServicesHelper.Client.UpdateRhinoAppearanceMaterialsAsync(skinName, polishName, isRenderLayDesignFlat);
	}

	public void UpdateRhinoHand(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset)
	{
		UIHelper.CoreDataService.DisplayConduitSetEnabled(enabled: false);
		_rhinoServicesHelper.Client.UpdateRhinoHand(selectedRing, isRingOnRightHand, isRingFlipped, ringPositionOffset);
		UIHelper.CoreDataService.DisplayConduitSetEnabled(enabled: true);
		Application.Current.MainWindow.Activate();
	}

	public void UpdateViewPortHandleDrawScale(double scaleFactor)
	{
		Application.Current.Dispatcher.InvokeAsync(delegate
		{
			_rhinoServicesHelper.Client.UpdateViewPortHandleDrawScale(scaleFactor);
		}, DispatcherPriority.Input);
	}

	public void ViewportSetBackgroundAndColor(string backgroundColor, string backgroundGraphicPath)
	{
		_rhinoServicesHelper.Client.ViewportSetBackgroundAndColorAsync(backgroundColor, backgroundGraphicPath);
	}

	public void ViewportSetHighlightColor(Color color)
	{
		_rhinoServicesHelper.Client.ViewportSetHighlightColorAsync(color);
	}

	public void ViewportSetTargetAndCameraLocation(double x, double y, double z)
	{
		_rhinoServicesHelper.Client.ViewportSetTargetAndCameraLocation(x, y, z);
	}

	public void WatcherSetEnabled(bool enabled)
	{
		_rhinoServicesHelper.Client.WatcherSetEnabled(enabled);
	}

	private void Client_DocumentClearAllAnyGemAnyColorCompleted(object sender, AsyncCompletedEventArgs e)
	{
		ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: false);
		_rhinoServicesHelper.Client.DocumentClearAllAnyGemAnyColorCompleted -= Client_DocumentClearAllAnyGemAnyColorCompleted;
	}

	private bool MaterialsGetMetalDisplayObjectBinObjFile(out byte[] binObjFile, ComponentsEnum whichComponent, params MaterialClassification[] materialClassifications)
	{
		binObjFile = _rhinoServicesHelper.Client.GetDisplayObjectBinObjByComponentAndMaterialClassification(whichComponent, materialClassifications);
		if (binObjFile == null)
		{
			return false;
		}
		return binObjFile.Length >= 0;
	}

	private static void OnRhinoServiceOnUpdateUIControlCompleted(object s, UpdateUIControlCompletedEventArgs e)
	{
		ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: false);
		ViewModelLocator.Current?.DesignStackVM.UpdateDesignStackUIControls(e.Result, e.Error);
	}

	~CSCoreDataService()
	{
		Dispose();
	}

	public void InitializeRhinoConnection()
	{
		GlobalErrorHandler.WriteMethodInfo("InitializeRhinoConnection");
		int num = 0;
		bool flag = false;
		_rhinoServicesHelper = new RhinoServicesHelper();
		while (num < 4 && !flag)
		{
			num++;
			_rhinoServicesHelper.Open("InitializeRhinoConnection");
			flag = _rhinoServicesHelper.IsConnectedToRhino;
			GlobalErrorHandler.WriteInfoToEventLog($"Connecting To Rhino, attempt {num}.");
		}
		ViewModelLocator.Current?.MainVM.SetIsRhinoBusy(isBusy: false);
		_rhinoServicesHelper.UIControlsUpdated = OnRhinoServiceOnUpdateUIControlCompleted;
	}

	public void SetIsBlockParallelProcessingEnabled(bool isBlockParallelProcessingEnabled)
	{
		_rhinoServicesHelper.Client.SetIsBlockParallelProcessingEnabledAsync(isBlockParallelProcessingEnabled);
	}
}
