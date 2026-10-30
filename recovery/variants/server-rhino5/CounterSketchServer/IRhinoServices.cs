using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
using CSMaterials;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.CAD;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Pricing;

namespace CounterSketchServer;

[ServiceContract(CallbackContract = typeof(IStudioUICallback))]
public interface IRhinoServices
{
	[OperationContract]
	string AbortPrintJob();

	[OperationContract]
	void AddPropToDisplay(string renderPropSwatchCollectionItem);

	[OperationContract]
	bool AreSecondaryMetalObjectsVisible();

	[OperationContract]
	bool AreSecondaryMetalObjectsVisibleForComponent(ComponentsEnum whichComponent);

	[OperationContract]
	bool CheckConnectionToProJet1200(string connectionAddress, out string sError, out string sStatus);

	[OperationContract]
	bool CheckFirmwareVersion();

	[OperationContract]
	void ClearExpandedSliders();

	[OperationContract]
	void ClearHighlightedGems();

	[OperationContract]
	bool ClearUnifiedMesh();

	[OperationContract]
	void ConduitEnabled(bool isEnabled);

	[OperationContract]
	void ConduitResetTryOn();

	[OperationContract]
	void ConduitSetFingernailObjectMaterial(string fingernailPolishMaterialName);

	[OperationContract]
	void ConduitSetFlipRing(bool flipRing);

	[OperationContract]
	void ConduitSetHandObjectMaterial(string handMaterialName);

	[OperationContract]
	void ConduitSetScaleToRingRailObjectFlip(bool flipObject);

	[OperationContract]
	void ConduitSetScaleToRingRailYOffset(double ringOffset);

	[OperationContract]
	bool ConnectionValid();

	[OperationContract]
	bool CSCreateSupportsForCSItem(out string ErrorMessage);

	[OperationContract]
	List<UIControl> DeleteImportedGroupFromGraph(List<Guid> importGroupIdentifiers);

	[OperationContract]
	bool DeleteSavedJob(int whatjob);

	[OperationContract]
	void DeselectViewportControls();

	[OperationContract]
	void DisplayConduitSetEnabled(bool enabled);

	[OperationContract]
	void DocumentCenterDesign();

	[OperationContract]
	void DocumentClearAllAnyGemAnyColor();

	[OperationContract]
	void DocumentClearUndo();

	[OperationContract]
	bool DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(GemMaterial gemMaterial, bool isCenterStoneAssigned = false);

	[OperationContract]
	void DocumentDuplicateCSGPart(List<Guid> importGroupIdentifiers);

	[OperationContract]
	bool DocumentGetModified();

	[OperationContract]
	double DocumentGetRingSizeIndexFromSizeText(string ringSizeText);

	[OperationContract]
	string DocumentGetRingSizeTextFromSizeIndex(double ringSizeIndex);

	[OperationContract]
	double DocumentGetRingSizeTranslated(double ringSizeInMillimeters);

	[OperationContract]
	List<StudioGem> DocumentGetStudioGemList();

	[OperationContract]
	void DocumentGraphGenerate();

	[OperationContract]
	void DocumentImportCSGPart(string pathToCSGPart, int? centerGemGraphIndex = null, bool doShowFontSourceErrors = true);

	[OperationContract]
	void DocumentReset();

	[OperationContract]
	void DocumentResetCenterGemAnyGemAnyColorMaterialAssignment();

	[OperationContract]
	void DocumentSetCameraView(string cameraViewName);

	[OperationContract]
	void DocumentSetCurvesAndPointsVisibility(bool visible);

	[OperationContract]
	void DocumentSetFilename(string filename);

	[OperationContract]
	void DocumentSetModified(bool modified);

	[OperationContract]
	void DocumentSetRingSizeSetting(string ringSizeSetting);

	[OperationContract]
	void DocumentSetShowCurves(bool visible);

	[OperationContract]
	void DocumentSetShowStarts(bool visible);

	[OperationContract]
	void DocumentShowAxis(bool visible);

	[OperationContract]
	void DocumentShowGrid(bool visible);

	[OperationContract]
	void DocumentSpinDesign(bool isEnabled);

	[OperationContract]
	void DocumentToggleGemSelectionMode(bool isEnabled);

	[OperationContract]
	void DocumentUndoBeginRecord();

	[OperationContract]
	void DocumentUserDataAddValue(string typeName, string keyName, string value);

	[OperationContract]
	string DocumentUserDataFindValue(string typeName, string keyName);

	[OperationContract]
	string DocumentUserDataGetOrderData();

	[OperationContract]
	List<Tuple<string, string, string>> DocumentUserDataGetAllDataElements();

	[OperationContract]
	void DocumentWrite(string path, string licenseInfo);

	[OperationContract]
	void DocumentZoomCameraToBoundingBox();

	[OperationContract]
	void EditorGetPartsForCurveEditrFromGraphObject(Guid blockInstanceGuid, bool createAsCopy, out List<double> sweepPoints, out double chooserValue, out NurbsCurve curve);

	[OperationContract]
	void EnableDrawing(bool isEnabled);

	[OperationContract]
	bool ExportDesignAs3DM(string savePath, bool includeCenter, bool includeSide, bool includeAccent);

	[OperationContract]
	bool ExportDesignAsSTL(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh);

	[OperationContract]
	void ExportImportGroups(List<Guid> targetImportGroupIdentifiers, string path);

	[OperationContract]
	bool FindProjetPrinter(out string FindAddress);

	[OperationContract]
	bool GetActivationState();

	[OperationContract]
	IEnumerable<KeyValuePair<int, string>> GetAllRingSizesForCurrentRingSizeRegion();

	[OperationContract]
	List<ComponentsEnum> GetComponents();

	[OperationContract]
	byte[] GetCurrentViewportImageBitmap();

	[OperationContract]
	string GetDesignerFromUserData();

	[OperationContract]
	int GetDesignIterationCount();

	[OperationContract]
	byte[] GetDisplayObjectBinObjByComponentAndMaterialClassification(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications);

	[OperationContract]
	string GetDocumentFilename();

	[OperationContract]
	List<int> GetFeaturedDesignIterations();

	[OperationContract]
	GemMaterial[] GetGemMaterials();

	[OperationContract]
	MetalMaterial[] GetMetalMaterials();

	[OperationContract]
	Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> GetMetalVolumesPerComponent(bool hasSingleMetalCastingCount, int singleMetalCastingCount);

	[OperationContract]
	string GetNotesToCustomerForCurrentDesign();

	[OperationContract]
	int GetNumberofVisibleFreehandParts();

	[OperationContract]
	bool GetPrinterMaterial(out string message);

	[OperationContract]
	string GetRhinoSerialCode();

	[OperationContract]
	int GetRhinoThreadId();

	[OperationContract]
	IntPtr GetRhinoWindowHandle();

	[OperationContract]
	List<int> GetSavedJobs();

	[OperationContract]
	string GetSPINumberFromUserData();

	[OperationContract]
	BitmapSourceByteArrayInfo GetThumbnailFromFile(string filename);

	[OperationContract]
	List<ComponentsEnum> GetVisibleComponents();

	[OperationContract]
	void GraphItemAddUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue);

	[OperationContract]
	string GraphItemFindUserDataValue(Guid blockInstanceGuid, string typeName, string keyName);

	[OperationContract]
	List<UIControl> GraphRedoChange();

	[OperationContract]
	List<UIControl> GraphUndoChange();

	[OperationContract]
	void HighlightAccentGems(int gemId, bool highlightAll);

	[OperationContract]
	void HighlightGemFromGemReportByGemNumber(List<string> gemNumbers);

	[OperationContract]
	void HighlightGemFromGemReportByIndex(List<int> gemIndexes);

	[OperationContract]
	void HighlightGraphItem(Guid blockInstanceGuid, bool showHighlight);

	[OperationContract]
	void HighlightItemsByBlockInstanceGuid(IEnumerable<Guid> instanceGuids);

	[OperationContract]
	void ImportGroupAddRingRailGuide();

	[OperationContract]
	void ImportGroupAddToPartGroup(List<Guid> importGroupIdentifiers);

	[OperationContract]
	void ImportGroupBreakPartGroup(Guid importGroupIdentifier);

	[OperationContract]
	void ImportGroupRename(Guid importGroupIdentifier, string updatedName);

	[OperationContract]
	void InitializeRhinoProperties(string csLibraryPath);

	[OperationContract]
	List<UIControl> InvokeAddAPartPatchwire(string path, string name);

	[OperationContract]
	List<UIControl> InvokeGemPatchWire(GemActions action);

	[OperationContract]
	bool IsComponentsEnumVisible(ComponentsEnum componentsEnum);

	[OperationContract]
	bool IsDisplayConduitEnabled();

	[OperationContract]
	bool IsDocumentCanvasEmpty();

	[OperationContract]
	bool IsManualSizingModeOn(bool excludeCenterStone);

	[OperationContract]
	bool IsProjetDialogLoaded();

	[OperationContract]
	bool IsRhino64Bit();

	[OperationContract]
	bool IsUsbKeyBeingUsed();

	[OperationContract]
	BoundingBox NurbsGetBoundingBox(NurbsCurve nc);

	[OperationContract]
	double NurbsGetClosestPoint(NurbsCurve nc, Point3D testHit);

	[OperationContract]
	Point3D NurbsGetNormalizedPointAt(NurbsCurve nc, double editPointParam);

	[OperationContract]
	List<Point2D> NurbsGetPolyline(NurbsCurve nc);

	[OperationContract]
	NurbsCurve NurbsRebuild(NurbsCurve nc, int i, int i1);

	[OperationContract]
	NurbsCurve NurbsSetDomain(NurbsCurve nc, int i, int i1);

	[OperationContract]
	List<UIControl> OpenDocument(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid = default(Guid), int? centerGemGraphIndex = null);

	[OperationContract]
	bool OrderCheckNonCenterGemsExist();

	[OperationContract]
	List<OrderGemData> OrderGetAccentGemData();

	[OperationContract]
	List<OrderGemData> OrderGetAllGemData();

	[OperationContract]
	List<string> OrderGetCurrentRingSizes();

	[OperationContract]
	List<double> OrderGetCurrentRingSizesDiameter();

	[OperationContract]
	List<string> OrderGetCurrentRingSizesUom();

	[OperationContract]
	List<string> OrderGetExpandedSliderList();

	[OperationContract]
	void PerformDistanceMeasurement();

	[OperationContract]
	void PrintingSupportDisplayConduitEnabled(bool isEnabled);

	[OperationContract]
	bool PrintMultipleJobs(List<int> lstJobsToPrint, out string sStatus);

	[OperationContract]
	bool PrintUnifiedMeshToProJet1200(out string ErrorMessage);

	[OperationContract]
	void RedrawRhino();

	[OperationContract]
	void RemovePropFromDisplay();

	[OperationContract]
	void RenderGenerateRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane);

	[OperationContract]
	void RepaintRhino();

	[OperationContract]
	Bitmap RequestScreenCapture(Size captureSize);

	[OperationContract]
	Bitmap RequestScreenCaptureAndFreeze(Size captureSize);

	[OperationContract]
	void RequestScreenCaptureAtPerspectivePositionForImportGroup(Size captureSize, bool zoom, List<Guid> importGroupIdentifiers);

	[OperationContract]
	Bitmap RequestScreenCaptureDefault(Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly = false);

	[OperationContract]
	Bitmap RequestScreenCaptureFront(Size captureSize);

	[OperationContract]
	Bitmap RequestScreenCaptureSide(Size captureSize);

	[OperationContract]
	Bitmap RequestScreenCaptureTop(Size captureSize);

	[OperationContract]
	void ReselectViewportControls();

	[OperationContract]
	void ResetHandleForImportGroup(Guid importGroupIdentifier);

	[OperationContract]
	void RhinoAbortScripts();

	[OperationContract]
	void RhinoCloseViewPort(string viewportName);

	[OperationContract]
	IntPtr RhinoCreateViewPort(string viewportName);

	[OperationContract]
	IntPtr RhinoGetActiveViewWindowHandle();

	[OperationContract]
	string RhinoGetVersion();

	[OperationContract]
	IntPtr RhinoGetWindowHandle();

	[OperationContract]
	void RhinoLoadBlankDocument();

	[OperationContract]
	void RhinoRunScript(string scriptText);

	[OperationContract]
	void RhinoSetFocusTo();

	[OperationContract]
	bool RhinoWindowReparent(IntPtr intPtrParent);

	[OperationContract]
	void RhinoWindowReposition(int startPointX, int startPointY, int width, int height);

	[OperationContract]
	bool SavePrintJob(int whatslot, out string sStatus);

	[OperationContract]
	void SaveScreen(string saveImagePath);

	[OperationContract]
	bool SaveViewportToImage(string viewportName, string pathToSaveImage, double maxHeightWidth);

	[OperationContract]
	void SelectImportedGroupByIdentifier(Guid importGroupIdentifier);

	[OperationContract]
	void SelectImportedGroupByIdentifierSecondary(List<Guid> importGroupIdentifiersd);

	[OperationContract]
	void SetIsBlockParallelProcessingEnabled(bool enabled);

	[OperationContract]
	void SetCAMModeState(bool CAMModeOn);

	[OperationContract]
	void SetComponentVisibility(ComponentsEnum componentsEnum, bool visible);

	[OperationContract]
	List<UIControl> SetDesignIteration(int iterationId);

	[OperationContract]
	void SetGemMaterials(GemMaterial[] gemMaterials);

	[OperationContract]
	void SetImportGroupToPrimaryMetal(List<Guid> importGroupIdentifiers);

	[OperationContract]
	void SetImportGroupToSecondaryMetal(List<Guid> importGroupIdentifiers);

	[OperationContract]
	void SetImportGroupVisibilityByIdentifier(List<Guid> importGroupIdentifiers, bool isViewportVisible);

	[OperationContract]
	int SetKeepAliveProcessIds(int id);

	[OperationContract]
	void SetLoadedDialogState(bool bState);

	[OperationContract]
	bool SetMeshFromComponent(List<ComponentsEnum> whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed, out string sError);

	[OperationContract]
	void SetMetalMaterials(MetalMaterial[] metalMaterials);

	[OperationContract]
	void SetPerspectiveViewActiveAndMaximize();

	[OperationContract]
	void SetSupportDensity(int iSupportDensity);

	[OperationContract]
	bool SetSupportScales(double SupScale, int SupType);

	[OperationContract]
	void SetUnifiedMeshLocation(double dHeight);

	[OperationContract]
	void SetUnifiedMeshRotation(double angleX, double angleY, double angleZ);

	[OperationContract]
	void Shutdown();

	[OperationContract]
	void ToggleFreehandCommonCenterMode(bool isFreehandCommonCenterModeEnabled);

	[OperationContract]
	void ToggleFreehandVisibility(bool isVisible);

	[OperationContract]
	bool GetFreehandComponentVisibility();

	[OperationContract]
	List<StudioGem> GetManualSizedStones();

	[OperationContract]
	void ToggleQuarterCaratGemRounding(bool isQuarterCaratRoundingEnabled);

	[OperationContract]
	void ToggleViewportControls(bool isVisible);

	[OperationContract]
	void TurnHandOff();

	[OperationContract]
	void UpdateCurveObjectFromUI(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, List<double> points, bool mouseDown);

	[OperationContract]
	bool UpdateFirmwareAndActivate(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany, out string sError);

	[OperationContract]
	void UpdateImportGroupPartListGroupAssignment(List<Guid> importGroupIdentifiers, string groupName);

	[OperationContract]
	void UpdateImportGroupSortOrder(List<Guid> importGroupGuids);

	[OperationContract]
	void UpdateRhinoAppearanceMaterials(string skinName, string polishName, bool isRenderLayDesignFlat);

	[OperationContract]
	void UpdateRhinoHand(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset);

	[OperationContract]
	void UpdateStudioUICallbackChannel();

	[OperationContract]
	List<UIControl> UpdateUIControl(UIControl uiControl);

	[OperationContract]
	List<UIControl> UpdateUIControlQuick(UIControl uiControl);

	[OperationContract]
	void UpdateValueChooserFromUI(Guid blockInstanceGuid);

	[OperationContract]
	void UpdateViewPortHandleDrawScale(double scaleFactor);

	[OperationContract]
	void ViewportSetBackgroundAndColor(string backgroundColor, string backgroundGraphicPath);

	[OperationContract]
	void ViewportSetHighlightColor(Color color);

	[OperationContract]
	void ViewportSetTargetAndCameraLocation(double x, double y, double z);

	[OperationContract]
	void WatcherSetEnabled(bool enabled);

	[OperationContract]
	void GraphItemRemoveUserDataValue(Guid blockInstanceGuid, string typeName, string keyName);
}
