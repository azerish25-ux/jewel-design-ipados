using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.CAD;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Pricing;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RhinoServicesClient : DuplexClientBase<IRhinoServices>, IRhinoServices
{
	private BeginOperationDelegate onBeginToggleFreehandCommonCenterModeDelegate;

	private EndOperationDelegate onEndToggleFreehandCommonCenterModeDelegate;

	private SendOrPostCallback onToggleFreehandCommonCenterModeCompletedDelegate;

	private BeginOperationDelegate onBeginToggleFreehandVisibilityDelegate;

	private EndOperationDelegate onEndToggleFreehandVisibilityDelegate;

	private SendOrPostCallback onToggleFreehandVisibilityCompletedDelegate;

	private BeginOperationDelegate onBeginGetFreehandComponentVisibilityDelegate;

	private EndOperationDelegate onEndGetFreehandComponentVisibilityDelegate;

	private SendOrPostCallback onGetFreehandComponentVisibilityCompletedDelegate;

	private BeginOperationDelegate onBeginGetManualSizedStonesDelegate;

	private EndOperationDelegate onEndGetManualSizedStonesDelegate;

	private SendOrPostCallback onGetManualSizedStonesCompletedDelegate;

	private BeginOperationDelegate onBeginToggleQuarterCaratGemRoundingDelegate;

	private EndOperationDelegate onEndToggleQuarterCaratGemRoundingDelegate;

	private SendOrPostCallback onToggleQuarterCaratGemRoundingCompletedDelegate;

	private BeginOperationDelegate onBeginToggleViewportControlsDelegate;

	private EndOperationDelegate onEndToggleViewportControlsDelegate;

	private SendOrPostCallback onToggleViewportControlsCompletedDelegate;

	private BeginOperationDelegate onBeginTurnHandOffDelegate;

	private EndOperationDelegate onEndTurnHandOffDelegate;

	private SendOrPostCallback onTurnHandOffCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateCurveObjectFromUIDelegate;

	private EndOperationDelegate onEndUpdateCurveObjectFromUIDelegate;

	private SendOrPostCallback onUpdateCurveObjectFromUICompletedDelegate;

	private BeginOperationDelegate onBeginUpdateFirmwareAndActivateDelegate;

	private EndOperationDelegate onEndUpdateFirmwareAndActivateDelegate;

	private SendOrPostCallback onUpdateFirmwareAndActivateCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateImportGroupPartListGroupAssignmentDelegate;

	private EndOperationDelegate onEndUpdateImportGroupPartListGroupAssignmentDelegate;

	private SendOrPostCallback onUpdateImportGroupPartListGroupAssignmentCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateImportGroupSortOrderDelegate;

	private EndOperationDelegate onEndUpdateImportGroupSortOrderDelegate;

	private SendOrPostCallback onUpdateImportGroupSortOrderCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateRhinoAppearanceMaterialsDelegate;

	private EndOperationDelegate onEndUpdateRhinoAppearanceMaterialsDelegate;

	private SendOrPostCallback onUpdateRhinoAppearanceMaterialsCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateRhinoHandDelegate;

	private EndOperationDelegate onEndUpdateRhinoHandDelegate;

	private SendOrPostCallback onUpdateRhinoHandCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateStudioUICallbackChannelDelegate;

	private EndOperationDelegate onEndUpdateStudioUICallbackChannelDelegate;

	private SendOrPostCallback onUpdateStudioUICallbackChannelCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateUIControlDelegate;

	private EndOperationDelegate onEndUpdateUIControlDelegate;

	private SendOrPostCallback onUpdateUIControlCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateUIControlQuickDelegate;

	private EndOperationDelegate onEndUpdateUIControlQuickDelegate;

	private SendOrPostCallback onUpdateUIControlQuickCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateValueChooserFromUIDelegate;

	private EndOperationDelegate onEndUpdateValueChooserFromUIDelegate;

	private SendOrPostCallback onUpdateValueChooserFromUICompletedDelegate;

	private BeginOperationDelegate onBeginUpdateViewPortHandleDrawScaleDelegate;

	private EndOperationDelegate onEndUpdateViewPortHandleDrawScaleDelegate;

	private SendOrPostCallback onUpdateViewPortHandleDrawScaleCompletedDelegate;

	private BeginOperationDelegate onBeginViewportSetBackgroundAndColorDelegate;

	private EndOperationDelegate onEndViewportSetBackgroundAndColorDelegate;

	private SendOrPostCallback onViewportSetBackgroundAndColorCompletedDelegate;

	private BeginOperationDelegate onBeginViewportSetHighlightColorDelegate;

	private EndOperationDelegate onEndViewportSetHighlightColorDelegate;

	private SendOrPostCallback onViewportSetHighlightColorCompletedDelegate;

	private BeginOperationDelegate onBeginViewportSetTargetAndCameraLocationDelegate;

	private EndOperationDelegate onEndViewportSetTargetAndCameraLocationDelegate;

	private SendOrPostCallback onViewportSetTargetAndCameraLocationCompletedDelegate;

	private BeginOperationDelegate onBeginWatcherSetEnabledDelegate;

	private EndOperationDelegate onEndWatcherSetEnabledDelegate;

	private SendOrPostCallback onWatcherSetEnabledCompletedDelegate;

	private BeginOperationDelegate onBeginGraphItemRemoveUserDataValueDelegate;

	private EndOperationDelegate onEndGraphItemRemoveUserDataValueDelegate;

	private SendOrPostCallback onGraphItemRemoveUserDataValueCompletedDelegate;

	private BeginOperationDelegate onBeginGraphUndoChangeDelegate;

	private EndOperationDelegate onEndGraphUndoChangeDelegate;

	private SendOrPostCallback onGraphUndoChangeCompletedDelegate;

	private BeginOperationDelegate onBeginHighlightAccentGemsDelegate;

	private EndOperationDelegate onEndHighlightAccentGemsDelegate;

	private SendOrPostCallback onHighlightAccentGemsCompletedDelegate;

	private BeginOperationDelegate onBeginHighlightGemFromGemReportByGemNumberDelegate;

	private EndOperationDelegate onEndHighlightGemFromGemReportByGemNumberDelegate;

	private SendOrPostCallback onHighlightGemFromGemReportByGemNumberCompletedDelegate;

	private BeginOperationDelegate onBeginHighlightGemFromGemReportByIndexDelegate;

	private EndOperationDelegate onEndHighlightGemFromGemReportByIndexDelegate;

	private SendOrPostCallback onHighlightGemFromGemReportByIndexCompletedDelegate;

	private BeginOperationDelegate onBeginHighlightGraphItemDelegate;

	private EndOperationDelegate onEndHighlightGraphItemDelegate;

	private SendOrPostCallback onHighlightGraphItemCompletedDelegate;

	private BeginOperationDelegate onBeginHighlightItemsByBlockInstanceGuidDelegate;

	private EndOperationDelegate onEndHighlightItemsByBlockInstanceGuidDelegate;

	private SendOrPostCallback onHighlightItemsByBlockInstanceGuidCompletedDelegate;

	private BeginOperationDelegate onBeginImportGroupAddRingRailGuideDelegate;

	private EndOperationDelegate onEndImportGroupAddRingRailGuideDelegate;

	private SendOrPostCallback onImportGroupAddRingRailGuideCompletedDelegate;

	private BeginOperationDelegate onBeginImportGroupAddToPartGroupDelegate;

	private EndOperationDelegate onEndImportGroupAddToPartGroupDelegate;

	private SendOrPostCallback onImportGroupAddToPartGroupCompletedDelegate;

	private BeginOperationDelegate onBeginImportGroupBreakPartGroupDelegate;

	private EndOperationDelegate onEndImportGroupBreakPartGroupDelegate;

	private SendOrPostCallback onImportGroupBreakPartGroupCompletedDelegate;

	private BeginOperationDelegate onBeginImportGroupRenameDelegate;

	private EndOperationDelegate onEndImportGroupRenameDelegate;

	private SendOrPostCallback onImportGroupRenameCompletedDelegate;

	private BeginOperationDelegate onBeginInitializeRhinoPropertiesDelegate;

	private EndOperationDelegate onEndInitializeRhinoPropertiesDelegate;

	private SendOrPostCallback onInitializeRhinoPropertiesCompletedDelegate;

	private BeginOperationDelegate onBeginInvokeAddAPartPatchwireDelegate;

	private EndOperationDelegate onEndInvokeAddAPartPatchwireDelegate;

	private SendOrPostCallback onInvokeAddAPartPatchwireCompletedDelegate;

	private BeginOperationDelegate onBeginInvokeGemPatchWireDelegate;

	private EndOperationDelegate onEndInvokeGemPatchWireDelegate;

	private SendOrPostCallback onInvokeGemPatchWireCompletedDelegate;

	private BeginOperationDelegate onBeginIsComponentsEnumVisibleDelegate;

	private EndOperationDelegate onEndIsComponentsEnumVisibleDelegate;

	private SendOrPostCallback onIsComponentsEnumVisibleCompletedDelegate;

	private BeginOperationDelegate onBeginIsDisplayConduitEnabledDelegate;

	private EndOperationDelegate onEndIsDisplayConduitEnabledDelegate;

	private SendOrPostCallback onIsDisplayConduitEnabledCompletedDelegate;

	private BeginOperationDelegate onBeginIsDocumentCanvasEmptyDelegate;

	private EndOperationDelegate onEndIsDocumentCanvasEmptyDelegate;

	private SendOrPostCallback onIsDocumentCanvasEmptyCompletedDelegate;

	private BeginOperationDelegate onBeginIsManualSizingModeOnDelegate;

	private EndOperationDelegate onEndIsManualSizingModeOnDelegate;

	private SendOrPostCallback onIsManualSizingModeOnCompletedDelegate;

	private BeginOperationDelegate onBeginIsProjetDialogLoadedDelegate;

	private EndOperationDelegate onEndIsProjetDialogLoadedDelegate;

	private SendOrPostCallback onIsProjetDialogLoadedCompletedDelegate;

	private BeginOperationDelegate onBeginIsRhino64BitDelegate;

	private EndOperationDelegate onEndIsRhino64BitDelegate;

	private SendOrPostCallback onIsRhino64BitCompletedDelegate;

	private BeginOperationDelegate onBeginIsUsbKeyBeingUsedDelegate;

	private EndOperationDelegate onEndIsUsbKeyBeingUsedDelegate;

	private SendOrPostCallback onIsUsbKeyBeingUsedCompletedDelegate;

	private BeginOperationDelegate onBeginNurbsGetBoundingBoxDelegate;

	private EndOperationDelegate onEndNurbsGetBoundingBoxDelegate;

	private SendOrPostCallback onNurbsGetBoundingBoxCompletedDelegate;

	private BeginOperationDelegate onBeginNurbsGetClosestPointDelegate;

	private EndOperationDelegate onEndNurbsGetClosestPointDelegate;

	private SendOrPostCallback onNurbsGetClosestPointCompletedDelegate;

	private BeginOperationDelegate onBeginNurbsGetNormalizedPointAtDelegate;

	private EndOperationDelegate onEndNurbsGetNormalizedPointAtDelegate;

	private SendOrPostCallback onNurbsGetNormalizedPointAtCompletedDelegate;

	private BeginOperationDelegate onBeginNurbsGetPolylineDelegate;

	private EndOperationDelegate onEndNurbsGetPolylineDelegate;

	private SendOrPostCallback onNurbsGetPolylineCompletedDelegate;

	private BeginOperationDelegate onBeginNurbsRebuildDelegate;

	private EndOperationDelegate onEndNurbsRebuildDelegate;

	private SendOrPostCallback onNurbsRebuildCompletedDelegate;

	private BeginOperationDelegate onBeginNurbsSetDomainDelegate;

	private EndOperationDelegate onEndNurbsSetDomainDelegate;

	private SendOrPostCallback onNurbsSetDomainCompletedDelegate;

	private BeginOperationDelegate onBeginOpenDocumentDelegate;

	private EndOperationDelegate onEndOpenDocumentDelegate;

	private SendOrPostCallback onOpenDocumentCompletedDelegate;

	private BeginOperationDelegate onBeginOrderCheckNonCenterGemsExistDelegate;

	private EndOperationDelegate onEndOrderCheckNonCenterGemsExistDelegate;

	private SendOrPostCallback onOrderCheckNonCenterGemsExistCompletedDelegate;

	private BeginOperationDelegate onBeginOrderGetAccentGemDataDelegate;

	private EndOperationDelegate onEndOrderGetAccentGemDataDelegate;

	private SendOrPostCallback onOrderGetAccentGemDataCompletedDelegate;

	private BeginOperationDelegate onBeginOrderGetAllGemDataDelegate;

	private EndOperationDelegate onEndOrderGetAllGemDataDelegate;

	private SendOrPostCallback onOrderGetAllGemDataCompletedDelegate;

	private BeginOperationDelegate onBeginOrderGetCurrentRingSizesDelegate;

	private EndOperationDelegate onEndOrderGetCurrentRingSizesDelegate;

	private SendOrPostCallback onOrderGetCurrentRingSizesCompletedDelegate;

	private BeginOperationDelegate onBeginOrderGetCurrentRingSizesDiameterDelegate;

	private EndOperationDelegate onEndOrderGetCurrentRingSizesDiameterDelegate;

	private SendOrPostCallback onOrderGetCurrentRingSizesDiameterCompletedDelegate;

	private BeginOperationDelegate onBeginOrderGetCurrentRingSizesUomDelegate;

	private EndOperationDelegate onEndOrderGetCurrentRingSizesUomDelegate;

	private SendOrPostCallback onOrderGetCurrentRingSizesUomCompletedDelegate;

	private BeginOperationDelegate onBeginOrderGetExpandedSliderListDelegate;

	private EndOperationDelegate onEndOrderGetExpandedSliderListDelegate;

	private SendOrPostCallback onOrderGetExpandedSliderListCompletedDelegate;

	private BeginOperationDelegate onBeginPerformDistanceMeasurementDelegate;

	private EndOperationDelegate onEndPerformDistanceMeasurementDelegate;

	private SendOrPostCallback onPerformDistanceMeasurementCompletedDelegate;

	private BeginOperationDelegate onBeginPrintingSupportDisplayConduitEnabledDelegate;

	private EndOperationDelegate onEndPrintingSupportDisplayConduitEnabledDelegate;

	private SendOrPostCallback onPrintingSupportDisplayConduitEnabledCompletedDelegate;

	private BeginOperationDelegate onBeginPrintMultipleJobsDelegate;

	private EndOperationDelegate onEndPrintMultipleJobsDelegate;

	private SendOrPostCallback onPrintMultipleJobsCompletedDelegate;

	private BeginOperationDelegate onBeginPrintUnifiedMeshToProJet1200Delegate;

	private EndOperationDelegate onEndPrintUnifiedMeshToProJet1200Delegate;

	private SendOrPostCallback onPrintUnifiedMeshToProJet1200CompletedDelegate;

	private BeginOperationDelegate onBeginRedrawRhinoDelegate;

	private EndOperationDelegate onEndRedrawRhinoDelegate;

	private SendOrPostCallback onRedrawRhinoCompletedDelegate;

	private BeginOperationDelegate onBeginRemovePropFromDisplayDelegate;

	private EndOperationDelegate onEndRemovePropFromDisplayDelegate;

	private SendOrPostCallback onRemovePropFromDisplayCompletedDelegate;

	private BeginOperationDelegate onBeginRenderGenerateRenderDelegate;

	private EndOperationDelegate onEndRenderGenerateRenderDelegate;

	private SendOrPostCallback onRenderGenerateRenderCompletedDelegate;

	private BeginOperationDelegate onBeginRepaintRhinoDelegate;

	private EndOperationDelegate onEndRepaintRhinoDelegate;

	private SendOrPostCallback onRepaintRhinoCompletedDelegate;

	private BeginOperationDelegate onBeginRequestScreenCaptureDelegate;

	private EndOperationDelegate onEndRequestScreenCaptureDelegate;

	private SendOrPostCallback onRequestScreenCaptureCompletedDelegate;

	private BeginOperationDelegate onBeginRequestScreenCaptureAndFreezeDelegate;

	private EndOperationDelegate onEndRequestScreenCaptureAndFreezeDelegate;

	private SendOrPostCallback onRequestScreenCaptureAndFreezeCompletedDelegate;

	private BeginOperationDelegate onBeginRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate;

	private EndOperationDelegate onEndRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate;

	private SendOrPostCallback onRequestScreenCaptureAtPerspectivePositionForImportGroupCompletedDelegate;

	private BeginOperationDelegate onBeginRequestScreenCaptureDefaultDelegate;

	private EndOperationDelegate onEndRequestScreenCaptureDefaultDelegate;

	private SendOrPostCallback onRequestScreenCaptureDefaultCompletedDelegate;

	private BeginOperationDelegate onBeginRequestScreenCaptureFrontDelegate;

	private EndOperationDelegate onEndRequestScreenCaptureFrontDelegate;

	private SendOrPostCallback onRequestScreenCaptureFrontCompletedDelegate;

	private BeginOperationDelegate onBeginRequestScreenCaptureSideDelegate;

	private EndOperationDelegate onEndRequestScreenCaptureSideDelegate;

	private SendOrPostCallback onRequestScreenCaptureSideCompletedDelegate;

	private BeginOperationDelegate onBeginRequestScreenCaptureTopDelegate;

	private EndOperationDelegate onEndRequestScreenCaptureTopDelegate;

	private SendOrPostCallback onRequestScreenCaptureTopCompletedDelegate;

	private BeginOperationDelegate onBeginReselectViewportControlsDelegate;

	private EndOperationDelegate onEndReselectViewportControlsDelegate;

	private SendOrPostCallback onReselectViewportControlsCompletedDelegate;

	private BeginOperationDelegate onBeginResetHandleForImportGroupDelegate;

	private EndOperationDelegate onEndResetHandleForImportGroupDelegate;

	private SendOrPostCallback onResetHandleForImportGroupCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoAbortScriptsDelegate;

	private EndOperationDelegate onEndRhinoAbortScriptsDelegate;

	private SendOrPostCallback onRhinoAbortScriptsCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoCloseViewPortDelegate;

	private EndOperationDelegate onEndRhinoCloseViewPortDelegate;

	private SendOrPostCallback onRhinoCloseViewPortCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoCreateViewPortDelegate;

	private EndOperationDelegate onEndRhinoCreateViewPortDelegate;

	private SendOrPostCallback onRhinoCreateViewPortCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoGetActiveViewWindowHandleDelegate;

	private EndOperationDelegate onEndRhinoGetActiveViewWindowHandleDelegate;

	private SendOrPostCallback onRhinoGetActiveViewWindowHandleCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoGetVersionDelegate;

	private EndOperationDelegate onEndRhinoGetVersionDelegate;

	private SendOrPostCallback onRhinoGetVersionCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoGetWindowHandleDelegate;

	private EndOperationDelegate onEndRhinoGetWindowHandleDelegate;

	private SendOrPostCallback onRhinoGetWindowHandleCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoLoadBlankDocumentDelegate;

	private EndOperationDelegate onEndRhinoLoadBlankDocumentDelegate;

	private SendOrPostCallback onRhinoLoadBlankDocumentCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoRunScriptDelegate;

	private EndOperationDelegate onEndRhinoRunScriptDelegate;

	private SendOrPostCallback onRhinoRunScriptCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoSetFocusToDelegate;

	private EndOperationDelegate onEndRhinoSetFocusToDelegate;

	private SendOrPostCallback onRhinoSetFocusToCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoWindowReparentDelegate;

	private EndOperationDelegate onEndRhinoWindowReparentDelegate;

	private SendOrPostCallback onRhinoWindowReparentCompletedDelegate;

	private BeginOperationDelegate onBeginRhinoWindowRepositionDelegate;

	private EndOperationDelegate onEndRhinoWindowRepositionDelegate;

	private SendOrPostCallback onRhinoWindowRepositionCompletedDelegate;

	private BeginOperationDelegate onBeginSavePrintJobDelegate;

	private EndOperationDelegate onEndSavePrintJobDelegate;

	private SendOrPostCallback onSavePrintJobCompletedDelegate;

	private BeginOperationDelegate onBeginSaveScreenDelegate;

	private EndOperationDelegate onEndSaveScreenDelegate;

	private SendOrPostCallback onSaveScreenCompletedDelegate;

	private BeginOperationDelegate onBeginSaveViewportToImageDelegate;

	private EndOperationDelegate onEndSaveViewportToImageDelegate;

	private SendOrPostCallback onSaveViewportToImageCompletedDelegate;

	private BeginOperationDelegate onBeginSelectImportedGroupByIdentifierDelegate;

	private EndOperationDelegate onEndSelectImportedGroupByIdentifierDelegate;

	private SendOrPostCallback onSelectImportedGroupByIdentifierCompletedDelegate;

	private BeginOperationDelegate onBeginSelectImportedGroupByIdentifierSecondaryDelegate;

	private EndOperationDelegate onEndSelectImportedGroupByIdentifierSecondaryDelegate;

	private SendOrPostCallback onSelectImportedGroupByIdentifierSecondaryCompletedDelegate;

	private BeginOperationDelegate onBeginSetIsBlockParallelProcessingEnabledDelegate;

	private EndOperationDelegate onEndSetIsBlockParallelProcessingEnabledDelegate;

	private SendOrPostCallback onSetIsBlockParallelProcessingEnabledCompletedDelegate;

	private BeginOperationDelegate onBeginSetCAMModeStateDelegate;

	private EndOperationDelegate onEndSetCAMModeStateDelegate;

	private SendOrPostCallback onSetCAMModeStateCompletedDelegate;

	private BeginOperationDelegate onBeginSetComponentVisibilityDelegate;

	private EndOperationDelegate onEndSetComponentVisibilityDelegate;

	private SendOrPostCallback onSetComponentVisibilityCompletedDelegate;

	private BeginOperationDelegate onBeginSetDesignIterationDelegate;

	private EndOperationDelegate onEndSetDesignIterationDelegate;

	private SendOrPostCallback onSetDesignIterationCompletedDelegate;

	private BeginOperationDelegate onBeginSetGemMaterialsDelegate;

	private EndOperationDelegate onEndSetGemMaterialsDelegate;

	private SendOrPostCallback onSetGemMaterialsCompletedDelegate;

	private BeginOperationDelegate onBeginSetImportGroupToPrimaryMetalDelegate;

	private EndOperationDelegate onEndSetImportGroupToPrimaryMetalDelegate;

	private SendOrPostCallback onSetImportGroupToPrimaryMetalCompletedDelegate;

	private BeginOperationDelegate onBeginSetImportGroupToSecondaryMetalDelegate;

	private EndOperationDelegate onEndSetImportGroupToSecondaryMetalDelegate;

	private SendOrPostCallback onSetImportGroupToSecondaryMetalCompletedDelegate;

	private BeginOperationDelegate onBeginSetImportGroupVisibilityByIdentifierDelegate;

	private EndOperationDelegate onEndSetImportGroupVisibilityByIdentifierDelegate;

	private SendOrPostCallback onSetImportGroupVisibilityByIdentifierCompletedDelegate;

	private BeginOperationDelegate onBeginSetKeepAliveProcessIdsDelegate;

	private EndOperationDelegate onEndSetKeepAliveProcessIdsDelegate;

	private SendOrPostCallback onSetKeepAliveProcessIdsCompletedDelegate;

	private BeginOperationDelegate onBeginSetLoadedDialogStateDelegate;

	private EndOperationDelegate onEndSetLoadedDialogStateDelegate;

	private SendOrPostCallback onSetLoadedDialogStateCompletedDelegate;

	private BeginOperationDelegate onBeginSetMeshFromComponentDelegate;

	private EndOperationDelegate onEndSetMeshFromComponentDelegate;

	private SendOrPostCallback onSetMeshFromComponentCompletedDelegate;

	private BeginOperationDelegate onBeginSetMetalMaterialsDelegate;

	private EndOperationDelegate onEndSetMetalMaterialsDelegate;

	private SendOrPostCallback onSetMetalMaterialsCompletedDelegate;

	private BeginOperationDelegate onBeginSetPerspectiveViewActiveAndMaximizeDelegate;

	private EndOperationDelegate onEndSetPerspectiveViewActiveAndMaximizeDelegate;

	private SendOrPostCallback onSetPerspectiveViewActiveAndMaximizeCompletedDelegate;

	private BeginOperationDelegate onBeginSetSupportDensityDelegate;

	private EndOperationDelegate onEndSetSupportDensityDelegate;

	private SendOrPostCallback onSetSupportDensityCompletedDelegate;

	private BeginOperationDelegate onBeginSetSupportScalesDelegate;

	private EndOperationDelegate onEndSetSupportScalesDelegate;

	private SendOrPostCallback onSetSupportScalesCompletedDelegate;

	private BeginOperationDelegate onBeginSetUnifiedMeshLocationDelegate;

	private EndOperationDelegate onEndSetUnifiedMeshLocationDelegate;

	private SendOrPostCallback onSetUnifiedMeshLocationCompletedDelegate;

	private BeginOperationDelegate onBeginSetUnifiedMeshRotationDelegate;

	private EndOperationDelegate onEndSetUnifiedMeshRotationDelegate;

	private SendOrPostCallback onSetUnifiedMeshRotationCompletedDelegate;

	private BeginOperationDelegate onBeginShutdownDelegate;

	private EndOperationDelegate onEndShutdownDelegate;

	private SendOrPostCallback onShutdownCompletedDelegate;

	private BeginOperationDelegate onBeginAbortPrintJobDelegate;

	private EndOperationDelegate onEndAbortPrintJobDelegate;

	private SendOrPostCallback onAbortPrintJobCompletedDelegate;

	private BeginOperationDelegate onBeginAddPropToDisplayDelegate;

	private EndOperationDelegate onEndAddPropToDisplayDelegate;

	private SendOrPostCallback onAddPropToDisplayCompletedDelegate;

	private BeginOperationDelegate onBeginAreSecondaryMetalObjectsVisibleDelegate;

	private EndOperationDelegate onEndAreSecondaryMetalObjectsVisibleDelegate;

	private SendOrPostCallback onAreSecondaryMetalObjectsVisibleCompletedDelegate;

	private BeginOperationDelegate onBeginAreSecondaryMetalObjectsVisibleForComponentDelegate;

	private EndOperationDelegate onEndAreSecondaryMetalObjectsVisibleForComponentDelegate;

	private SendOrPostCallback onAreSecondaryMetalObjectsVisibleForComponentCompletedDelegate;

	private BeginOperationDelegate onBeginCheckConnectionToProJet1200Delegate;

	private EndOperationDelegate onEndCheckConnectionToProJet1200Delegate;

	private SendOrPostCallback onCheckConnectionToProJet1200CompletedDelegate;

	private BeginOperationDelegate onBeginCheckFirmwareVersionDelegate;

	private EndOperationDelegate onEndCheckFirmwareVersionDelegate;

	private SendOrPostCallback onCheckFirmwareVersionCompletedDelegate;

	private BeginOperationDelegate onBeginClearExpandedSlidersDelegate;

	private EndOperationDelegate onEndClearExpandedSlidersDelegate;

	private SendOrPostCallback onClearExpandedSlidersCompletedDelegate;

	private BeginOperationDelegate onBeginClearHighlightedGemsDelegate;

	private EndOperationDelegate onEndClearHighlightedGemsDelegate;

	private SendOrPostCallback onClearHighlightedGemsCompletedDelegate;

	private BeginOperationDelegate onBeginClearUnifiedMeshDelegate;

	private EndOperationDelegate onEndClearUnifiedMeshDelegate;

	private SendOrPostCallback onClearUnifiedMeshCompletedDelegate;

	private BeginOperationDelegate onBeginConduitEnabledDelegate;

	private EndOperationDelegate onEndConduitEnabledDelegate;

	private SendOrPostCallback onConduitEnabledCompletedDelegate;

	private BeginOperationDelegate onBeginConduitResetTryOnDelegate;

	private EndOperationDelegate onEndConduitResetTryOnDelegate;

	private SendOrPostCallback onConduitResetTryOnCompletedDelegate;

	private BeginOperationDelegate onBeginConduitSetFingernailObjectMaterialDelegate;

	private EndOperationDelegate onEndConduitSetFingernailObjectMaterialDelegate;

	private SendOrPostCallback onConduitSetFingernailObjectMaterialCompletedDelegate;

	private BeginOperationDelegate onBeginConduitSetFlipRingDelegate;

	private EndOperationDelegate onEndConduitSetFlipRingDelegate;

	private SendOrPostCallback onConduitSetFlipRingCompletedDelegate;

	private BeginOperationDelegate onBeginConduitSetHandObjectMaterialDelegate;

	private EndOperationDelegate onEndConduitSetHandObjectMaterialDelegate;

	private SendOrPostCallback onConduitSetHandObjectMaterialCompletedDelegate;

	private BeginOperationDelegate onBeginConduitSetScaleToRingRailObjectFlipDelegate;

	private EndOperationDelegate onEndConduitSetScaleToRingRailObjectFlipDelegate;

	private SendOrPostCallback onConduitSetScaleToRingRailObjectFlipCompletedDelegate;

	private BeginOperationDelegate onBeginConduitSetScaleToRingRailYOffsetDelegate;

	private EndOperationDelegate onEndConduitSetScaleToRingRailYOffsetDelegate;

	private SendOrPostCallback onConduitSetScaleToRingRailYOffsetCompletedDelegate;

	private BeginOperationDelegate onBeginConnectionValidDelegate;

	private EndOperationDelegate onEndConnectionValidDelegate;

	private SendOrPostCallback onConnectionValidCompletedDelegate;

	private BeginOperationDelegate onBeginCSCreateSupportsForCSItemDelegate;

	private EndOperationDelegate onEndCSCreateSupportsForCSItemDelegate;

	private SendOrPostCallback onCSCreateSupportsForCSItemCompletedDelegate;

	private BeginOperationDelegate onBeginDeleteImportedGroupFromGraphDelegate;

	private EndOperationDelegate onEndDeleteImportedGroupFromGraphDelegate;

	private SendOrPostCallback onDeleteImportedGroupFromGraphCompletedDelegate;

	private BeginOperationDelegate onBeginDeleteSavedJobDelegate;

	private EndOperationDelegate onEndDeleteSavedJobDelegate;

	private SendOrPostCallback onDeleteSavedJobCompletedDelegate;

	private BeginOperationDelegate onBeginDeselectViewportControlsDelegate;

	private EndOperationDelegate onEndDeselectViewportControlsDelegate;

	private SendOrPostCallback onDeselectViewportControlsCompletedDelegate;

	private BeginOperationDelegate onBeginDisplayConduitSetEnabledDelegate;

	private EndOperationDelegate onEndDisplayConduitSetEnabledDelegate;

	private SendOrPostCallback onDisplayConduitSetEnabledCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentCenterDesignDelegate;

	private EndOperationDelegate onEndDocumentCenterDesignDelegate;

	private SendOrPostCallback onDocumentCenterDesignCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentClearAllAnyGemAnyColorDelegate;

	private EndOperationDelegate onEndDocumentClearAllAnyGemAnyColorDelegate;

	private SendOrPostCallback onDocumentClearAllAnyGemAnyColorCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentClearUndoDelegate;

	private EndOperationDelegate onEndDocumentClearUndoDelegate;

	private SendOrPostCallback onDocumentClearUndoCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate;

	private EndOperationDelegate onEndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate;

	private SendOrPostCallback onDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentDuplicateCSGPartDelegate;

	private EndOperationDelegate onEndDocumentDuplicateCSGPartDelegate;

	private SendOrPostCallback onDocumentDuplicateCSGPartCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentGetModifiedDelegate;

	private EndOperationDelegate onEndDocumentGetModifiedDelegate;

	private SendOrPostCallback onDocumentGetModifiedCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentGetRingSizeIndexFromSizeTextDelegate;

	private EndOperationDelegate onEndDocumentGetRingSizeIndexFromSizeTextDelegate;

	private SendOrPostCallback onDocumentGetRingSizeIndexFromSizeTextCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentGetRingSizeTextFromSizeIndexDelegate;

	private EndOperationDelegate onEndDocumentGetRingSizeTextFromSizeIndexDelegate;

	private SendOrPostCallback onDocumentGetRingSizeTextFromSizeIndexCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentGetRingSizeTranslatedDelegate;

	private EndOperationDelegate onEndDocumentGetRingSizeTranslatedDelegate;

	private SendOrPostCallback onDocumentGetRingSizeTranslatedCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentGetStudioGemListDelegate;

	private EndOperationDelegate onEndDocumentGetStudioGemListDelegate;

	private SendOrPostCallback onDocumentGetStudioGemListCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentGraphGenerateDelegate;

	private EndOperationDelegate onEndDocumentGraphGenerateDelegate;

	private SendOrPostCallback onDocumentGraphGenerateCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentImportCSGPartDelegate;

	private EndOperationDelegate onEndDocumentImportCSGPartDelegate;

	private SendOrPostCallback onDocumentImportCSGPartCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentResetDelegate;

	private EndOperationDelegate onEndDocumentResetDelegate;

	private SendOrPostCallback onDocumentResetCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate;

	private EndOperationDelegate onEndDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate;

	private SendOrPostCallback onDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSetCameraViewDelegate;

	private EndOperationDelegate onEndDocumentSetCameraViewDelegate;

	private SendOrPostCallback onDocumentSetCameraViewCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSetCurvesAndPointsVisibilityDelegate;

	private EndOperationDelegate onEndDocumentSetCurvesAndPointsVisibilityDelegate;

	private SendOrPostCallback onDocumentSetCurvesAndPointsVisibilityCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSetFilenameDelegate;

	private EndOperationDelegate onEndDocumentSetFilenameDelegate;

	private SendOrPostCallback onDocumentSetFilenameCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSetModifiedDelegate;

	private EndOperationDelegate onEndDocumentSetModifiedDelegate;

	private SendOrPostCallback onDocumentSetModifiedCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSetRingSizeSettingDelegate;

	private EndOperationDelegate onEndDocumentSetRingSizeSettingDelegate;

	private SendOrPostCallback onDocumentSetRingSizeSettingCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSetShowCurvesDelegate;

	private EndOperationDelegate onEndDocumentSetShowCurvesDelegate;

	private SendOrPostCallback onDocumentSetShowCurvesCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSetShowStartsDelegate;

	private EndOperationDelegate onEndDocumentSetShowStartsDelegate;

	private SendOrPostCallback onDocumentSetShowStartsCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentShowAxisDelegate;

	private EndOperationDelegate onEndDocumentShowAxisDelegate;

	private SendOrPostCallback onDocumentShowAxisCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentShowGridDelegate;

	private EndOperationDelegate onEndDocumentShowGridDelegate;

	private SendOrPostCallback onDocumentShowGridCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentSpinDesignDelegate;

	private EndOperationDelegate onEndDocumentSpinDesignDelegate;

	private SendOrPostCallback onDocumentSpinDesignCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentToggleGemSelectionModeDelegate;

	private EndOperationDelegate onEndDocumentToggleGemSelectionModeDelegate;

	private SendOrPostCallback onDocumentToggleGemSelectionModeCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentUndoBeginRecordDelegate;

	private EndOperationDelegate onEndDocumentUndoBeginRecordDelegate;

	private SendOrPostCallback onDocumentUndoBeginRecordCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentUserDataAddValueDelegate;

	private EndOperationDelegate onEndDocumentUserDataAddValueDelegate;

	private SendOrPostCallback onDocumentUserDataAddValueCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentUserDataFindValueDelegate;

	private EndOperationDelegate onEndDocumentUserDataFindValueDelegate;

	private SendOrPostCallback onDocumentUserDataFindValueCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentUserDataGetOrderDataDelegate;

	private EndOperationDelegate onEndDocumentUserDataGetOrderDataDelegate;

	private SendOrPostCallback onDocumentUserDataGetOrderDataCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentUserDataGetAllDataElementsDelegate;

	private EndOperationDelegate onEndDocumentUserDataGetAllDataElementsDelegate;

	private SendOrPostCallback onDocumentUserDataGetAllDataElementsCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentWriteDelegate;

	private EndOperationDelegate onEndDocumentWriteDelegate;

	private SendOrPostCallback onDocumentWriteCompletedDelegate;

	private BeginOperationDelegate onBeginDocumentZoomCameraToBoundingBoxDelegate;

	private EndOperationDelegate onEndDocumentZoomCameraToBoundingBoxDelegate;

	private SendOrPostCallback onDocumentZoomCameraToBoundingBoxCompletedDelegate;

	private BeginOperationDelegate onBeginEditorGetPartsForCurveEditrFromGraphObjectDelegate;

	private EndOperationDelegate onEndEditorGetPartsForCurveEditrFromGraphObjectDelegate;

	private SendOrPostCallback onEditorGetPartsForCurveEditrFromGraphObjectCompletedDelegate;

	private BeginOperationDelegate onBeginEnableDrawingDelegate;

	private EndOperationDelegate onEndEnableDrawingDelegate;

	private SendOrPostCallback onEnableDrawingCompletedDelegate;

	private BeginOperationDelegate onBeginExportDesignAs3DMDelegate;

	private EndOperationDelegate onEndExportDesignAs3DMDelegate;

	private SendOrPostCallback onExportDesignAs3DMCompletedDelegate;

	private BeginOperationDelegate onBeginExportDesignAsSTLDelegate;

	private EndOperationDelegate onEndExportDesignAsSTLDelegate;

	private SendOrPostCallback onExportDesignAsSTLCompletedDelegate;

	private BeginOperationDelegate onBeginExportImportGroupsDelegate;

	private EndOperationDelegate onEndExportImportGroupsDelegate;

	private SendOrPostCallback onExportImportGroupsCompletedDelegate;

	private BeginOperationDelegate onBeginFindProjetPrinterDelegate;

	private EndOperationDelegate onEndFindProjetPrinterDelegate;

	private SendOrPostCallback onFindProjetPrinterCompletedDelegate;

	private BeginOperationDelegate onBeginGetActivationStateDelegate;

	private EndOperationDelegate onEndGetActivationStateDelegate;

	private SendOrPostCallback onGetActivationStateCompletedDelegate;

	private BeginOperationDelegate onBeginGetAllRingSizesForCurrentRingSizeRegionDelegate;

	private EndOperationDelegate onEndGetAllRingSizesForCurrentRingSizeRegionDelegate;

	private SendOrPostCallback onGetAllRingSizesForCurrentRingSizeRegionCompletedDelegate;

	private BeginOperationDelegate onBeginGetComponentsDelegate;

	private EndOperationDelegate onEndGetComponentsDelegate;

	private SendOrPostCallback onGetComponentsCompletedDelegate;

	private BeginOperationDelegate onBeginGetCurrentViewportImageBitmapDelegate;

	private EndOperationDelegate onEndGetCurrentViewportImageBitmapDelegate;

	private SendOrPostCallback onGetCurrentViewportImageBitmapCompletedDelegate;

	private BeginOperationDelegate onBeginGetDesignerFromUserDataDelegate;

	private EndOperationDelegate onEndGetDesignerFromUserDataDelegate;

	private SendOrPostCallback onGetDesignerFromUserDataCompletedDelegate;

	private BeginOperationDelegate onBeginGetDesignIterationCountDelegate;

	private EndOperationDelegate onEndGetDesignIterationCountDelegate;

	private SendOrPostCallback onGetDesignIterationCountCompletedDelegate;

	private BeginOperationDelegate onBeginGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate;

	private EndOperationDelegate onEndGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate;

	private SendOrPostCallback onGetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedDelegate;

	private BeginOperationDelegate onBeginGetDocumentFilenameDelegate;

	private EndOperationDelegate onEndGetDocumentFilenameDelegate;

	private SendOrPostCallback onGetDocumentFilenameCompletedDelegate;

	private BeginOperationDelegate onBeginGetFeaturedDesignIterationsDelegate;

	private EndOperationDelegate onEndGetFeaturedDesignIterationsDelegate;

	private SendOrPostCallback onGetFeaturedDesignIterationsCompletedDelegate;

	private BeginOperationDelegate onBeginGetGemMaterialsDelegate;

	private EndOperationDelegate onEndGetGemMaterialsDelegate;

	private SendOrPostCallback onGetGemMaterialsCompletedDelegate;

	private BeginOperationDelegate onBeginGetMetalMaterialsDelegate;

	private EndOperationDelegate onEndGetMetalMaterialsDelegate;

	private SendOrPostCallback onGetMetalMaterialsCompletedDelegate;

	private BeginOperationDelegate onBeginGetMetalVolumesPerComponentDelegate;

	private EndOperationDelegate onEndGetMetalVolumesPerComponentDelegate;

	private SendOrPostCallback onGetMetalVolumesPerComponentCompletedDelegate;

	private BeginOperationDelegate onBeginGetNotesToCustomerForCurrentDesignDelegate;

	private EndOperationDelegate onEndGetNotesToCustomerForCurrentDesignDelegate;

	private SendOrPostCallback onGetNotesToCustomerForCurrentDesignCompletedDelegate;

	private BeginOperationDelegate onBeginGetNumberofVisibleFreehandPartsDelegate;

	private EndOperationDelegate onEndGetNumberofVisibleFreehandPartsDelegate;

	private SendOrPostCallback onGetNumberofVisibleFreehandPartsCompletedDelegate;

	private BeginOperationDelegate onBeginGetPrinterMaterialDelegate;

	private EndOperationDelegate onEndGetPrinterMaterialDelegate;

	private SendOrPostCallback onGetPrinterMaterialCompletedDelegate;

	private BeginOperationDelegate onBeginGetRhinoSerialCodeDelegate;

	private EndOperationDelegate onEndGetRhinoSerialCodeDelegate;

	private SendOrPostCallback onGetRhinoSerialCodeCompletedDelegate;

	private BeginOperationDelegate onBeginGetRhinoThreadIdDelegate;

	private EndOperationDelegate onEndGetRhinoThreadIdDelegate;

	private SendOrPostCallback onGetRhinoThreadIdCompletedDelegate;

	private BeginOperationDelegate onBeginGetRhinoWindowHandleDelegate;

	private EndOperationDelegate onEndGetRhinoWindowHandleDelegate;

	private SendOrPostCallback onGetRhinoWindowHandleCompletedDelegate;

	private BeginOperationDelegate onBeginGetSavedJobsDelegate;

	private EndOperationDelegate onEndGetSavedJobsDelegate;

	private SendOrPostCallback onGetSavedJobsCompletedDelegate;

	private BeginOperationDelegate onBeginGetSPINumberFromUserDataDelegate;

	private EndOperationDelegate onEndGetSPINumberFromUserDataDelegate;

	private SendOrPostCallback onGetSPINumberFromUserDataCompletedDelegate;

	private BeginOperationDelegate onBeginGetThumbnailFromFileDelegate;

	private EndOperationDelegate onEndGetThumbnailFromFileDelegate;

	private SendOrPostCallback onGetThumbnailFromFileCompletedDelegate;

	private BeginOperationDelegate onBeginGetVisibleComponentsDelegate;

	private EndOperationDelegate onEndGetVisibleComponentsDelegate;

	private SendOrPostCallback onGetVisibleComponentsCompletedDelegate;

	private BeginOperationDelegate onBeginGraphItemAddUserDataValueDelegate;

	private EndOperationDelegate onEndGraphItemAddUserDataValueDelegate;

	private SendOrPostCallback onGraphItemAddUserDataValueCompletedDelegate;

	private BeginOperationDelegate onBeginGraphItemFindUserDataValueDelegate;

	private EndOperationDelegate onEndGraphItemFindUserDataValueDelegate;

	private SendOrPostCallback onGraphItemFindUserDataValueCompletedDelegate;

	private BeginOperationDelegate onBeginGraphRedoChangeDelegate;

	private EndOperationDelegate onEndGraphRedoChangeDelegate;

	private SendOrPostCallback onGraphRedoChangeCompletedDelegate;

	public event EventHandler<AsyncCompletedEventArgs> ToggleFreehandCommonCenterModeCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ToggleFreehandVisibilityCompleted;

	public event EventHandler<GetFreehandComponentVisibilityCompletedEventArgs> GetFreehandComponentVisibilityCompleted;

	public event EventHandler<GetManualSizedStonesCompletedEventArgs> GetManualSizedStonesCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ToggleQuarterCaratGemRoundingCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ToggleViewportControlsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> TurnHandOffCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateCurveObjectFromUICompleted;

	public event EventHandler<UpdateFirmwareAndActivateCompletedEventArgs> UpdateFirmwareAndActivateCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateImportGroupPartListGroupAssignmentCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateImportGroupSortOrderCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateRhinoAppearanceMaterialsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateRhinoHandCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateStudioUICallbackChannelCompleted;

	public event EventHandler<UpdateUIControlCompletedEventArgs> UpdateUIControlCompleted;

	public event EventHandler<UpdateUIControlQuickCompletedEventArgs> UpdateUIControlQuickCompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateValueChooserFromUICompleted;

	public event EventHandler<AsyncCompletedEventArgs> UpdateViewPortHandleDrawScaleCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ViewportSetBackgroundAndColorCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ViewportSetHighlightColorCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ViewportSetTargetAndCameraLocationCompleted;

	public event EventHandler<AsyncCompletedEventArgs> WatcherSetEnabledCompleted;

	public event EventHandler<AsyncCompletedEventArgs> GraphItemRemoveUserDataValueCompleted;

	public event EventHandler<GraphUndoChangeCompletedEventArgs> GraphUndoChangeCompleted;

	public event EventHandler<AsyncCompletedEventArgs> HighlightAccentGemsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> HighlightGemFromGemReportByGemNumberCompleted;

	public event EventHandler<AsyncCompletedEventArgs> HighlightGemFromGemReportByIndexCompleted;

	public event EventHandler<AsyncCompletedEventArgs> HighlightGraphItemCompleted;

	public event EventHandler<AsyncCompletedEventArgs> HighlightItemsByBlockInstanceGuidCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ImportGroupAddRingRailGuideCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ImportGroupAddToPartGroupCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ImportGroupBreakPartGroupCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ImportGroupRenameCompleted;

	public event EventHandler<AsyncCompletedEventArgs> InitializeRhinoPropertiesCompleted;

	public event EventHandler<InvokeAddAPartPatchwireCompletedEventArgs> InvokeAddAPartPatchwireCompleted;

	public event EventHandler<InvokeGemPatchWireCompletedEventArgs> InvokeGemPatchWireCompleted;

	public event EventHandler<IsComponentsEnumVisibleCompletedEventArgs> IsComponentsEnumVisibleCompleted;

	public event EventHandler<IsDisplayConduitEnabledCompletedEventArgs> IsDisplayConduitEnabledCompleted;

	public event EventHandler<IsDocumentCanvasEmptyCompletedEventArgs> IsDocumentCanvasEmptyCompleted;

	public event EventHandler<IsManualSizingModeOnCompletedEventArgs> IsManualSizingModeOnCompleted;

	public event EventHandler<IsProjetDialogLoadedCompletedEventArgs> IsProjetDialogLoadedCompleted;

	public event EventHandler<IsRhino64BitCompletedEventArgs> IsRhino64BitCompleted;

	public event EventHandler<IsUsbKeyBeingUsedCompletedEventArgs> IsUsbKeyBeingUsedCompleted;

	public event EventHandler<NurbsGetBoundingBoxCompletedEventArgs> NurbsGetBoundingBoxCompleted;

	public event EventHandler<NurbsGetClosestPointCompletedEventArgs> NurbsGetClosestPointCompleted;

	public event EventHandler<NurbsGetNormalizedPointAtCompletedEventArgs> NurbsGetNormalizedPointAtCompleted;

	public event EventHandler<NurbsGetPolylineCompletedEventArgs> NurbsGetPolylineCompleted;

	public event EventHandler<NurbsRebuildCompletedEventArgs> NurbsRebuildCompleted;

	public event EventHandler<NurbsSetDomainCompletedEventArgs> NurbsSetDomainCompleted;

	public event EventHandler<OpenDocumentCompletedEventArgs> OpenDocumentCompleted;

	public event EventHandler<OrderCheckNonCenterGemsExistCompletedEventArgs> OrderCheckNonCenterGemsExistCompleted;

	public event EventHandler<OrderGetAccentGemDataCompletedEventArgs> OrderGetAccentGemDataCompleted;

	public event EventHandler<OrderGetAllGemDataCompletedEventArgs> OrderGetAllGemDataCompleted;

	public event EventHandler<OrderGetCurrentRingSizesCompletedEventArgs> OrderGetCurrentRingSizesCompleted;

	public event EventHandler<OrderGetCurrentRingSizesDiameterCompletedEventArgs> OrderGetCurrentRingSizesDiameterCompleted;

	public event EventHandler<OrderGetCurrentRingSizesUomCompletedEventArgs> OrderGetCurrentRingSizesUomCompleted;

	public event EventHandler<OrderGetExpandedSliderListCompletedEventArgs> OrderGetExpandedSliderListCompleted;

	public event EventHandler<AsyncCompletedEventArgs> PerformDistanceMeasurementCompleted;

	public event EventHandler<AsyncCompletedEventArgs> PrintingSupportDisplayConduitEnabledCompleted;

	public event EventHandler<PrintMultipleJobsCompletedEventArgs> PrintMultipleJobsCompleted;

	public event EventHandler<PrintUnifiedMeshToProJet1200CompletedEventArgs> PrintUnifiedMeshToProJet1200Completed;

	public event EventHandler<AsyncCompletedEventArgs> RedrawRhinoCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RemovePropFromDisplayCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RenderGenerateRenderCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RepaintRhinoCompleted;

	public event EventHandler<RequestScreenCaptureCompletedEventArgs> RequestScreenCaptureCompleted;

	public event EventHandler<RequestScreenCaptureAndFreezeCompletedEventArgs> RequestScreenCaptureAndFreezeCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RequestScreenCaptureAtPerspectivePositionForImportGroupCompleted;

	public event EventHandler<RequestScreenCaptureDefaultCompletedEventArgs> RequestScreenCaptureDefaultCompleted;

	public event EventHandler<RequestScreenCaptureFrontCompletedEventArgs> RequestScreenCaptureFrontCompleted;

	public event EventHandler<RequestScreenCaptureSideCompletedEventArgs> RequestScreenCaptureSideCompleted;

	public event EventHandler<RequestScreenCaptureTopCompletedEventArgs> RequestScreenCaptureTopCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ReselectViewportControlsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ResetHandleForImportGroupCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RhinoAbortScriptsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RhinoCloseViewPortCompleted;

	public event EventHandler<RhinoCreateViewPortCompletedEventArgs> RhinoCreateViewPortCompleted;

	public event EventHandler<RhinoGetActiveViewWindowHandleCompletedEventArgs> RhinoGetActiveViewWindowHandleCompleted;

	public event EventHandler<RhinoGetVersionCompletedEventArgs> RhinoGetVersionCompleted;

	public event EventHandler<RhinoGetWindowHandleCompletedEventArgs> RhinoGetWindowHandleCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RhinoLoadBlankDocumentCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RhinoRunScriptCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RhinoSetFocusToCompleted;

	public event EventHandler<RhinoWindowReparentCompletedEventArgs> RhinoWindowReparentCompleted;

	public event EventHandler<AsyncCompletedEventArgs> RhinoWindowRepositionCompleted;

	public event EventHandler<SavePrintJobCompletedEventArgs> SavePrintJobCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SaveScreenCompleted;

	public event EventHandler<SaveViewportToImageCompletedEventArgs> SaveViewportToImageCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SelectImportedGroupByIdentifierCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SelectImportedGroupByIdentifierSecondaryCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetIsBlockParallelProcessingEnabledCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetCAMModeStateCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetComponentVisibilityCompleted;

	public event EventHandler<SetDesignIterationCompletedEventArgs> SetDesignIterationCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetGemMaterialsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetImportGroupToPrimaryMetalCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetImportGroupToSecondaryMetalCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetImportGroupVisibilityByIdentifierCompleted;

	public event EventHandler<SetKeepAliveProcessIdsCompletedEventArgs> SetKeepAliveProcessIdsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetLoadedDialogStateCompleted;

	public event EventHandler<SetMeshFromComponentCompletedEventArgs> SetMeshFromComponentCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetMetalMaterialsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetPerspectiveViewActiveAndMaximizeCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetSupportDensityCompleted;

	public event EventHandler<SetSupportScalesCompletedEventArgs> SetSupportScalesCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetUnifiedMeshLocationCompleted;

	public event EventHandler<AsyncCompletedEventArgs> SetUnifiedMeshRotationCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ShutdownCompleted;

	public event EventHandler<AbortPrintJobCompletedEventArgs> AbortPrintJobCompleted;

	public event EventHandler<AsyncCompletedEventArgs> AddPropToDisplayCompleted;

	public event EventHandler<AreSecondaryMetalObjectsVisibleCompletedEventArgs> AreSecondaryMetalObjectsVisibleCompleted;

	public event EventHandler<AreSecondaryMetalObjectsVisibleForComponentCompletedEventArgs> AreSecondaryMetalObjectsVisibleForComponentCompleted;

	public event EventHandler<CheckConnectionToProJet1200CompletedEventArgs> CheckConnectionToProJet1200Completed;

	public event EventHandler<CheckFirmwareVersionCompletedEventArgs> CheckFirmwareVersionCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ClearExpandedSlidersCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ClearHighlightedGemsCompleted;

	public event EventHandler<ClearUnifiedMeshCompletedEventArgs> ClearUnifiedMeshCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ConduitEnabledCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ConduitResetTryOnCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ConduitSetFingernailObjectMaterialCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ConduitSetFlipRingCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ConduitSetHandObjectMaterialCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ConduitSetScaleToRingRailObjectFlipCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ConduitSetScaleToRingRailYOffsetCompleted;

	public event EventHandler<ConnectionValidCompletedEventArgs> ConnectionValidCompleted;

	public event EventHandler<CSCreateSupportsForCSItemCompletedEventArgs> CSCreateSupportsForCSItemCompleted;

	public event EventHandler<DeleteImportedGroupFromGraphCompletedEventArgs> DeleteImportedGroupFromGraphCompleted;

	public event EventHandler<DeleteSavedJobCompletedEventArgs> DeleteSavedJobCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DeselectViewportControlsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DisplayConduitSetEnabledCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentCenterDesignCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentClearAllAnyGemAnyColorCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentClearUndoCompleted;

	public event EventHandler<DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompletedEventArgs> DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentDuplicateCSGPartCompleted;

	public event EventHandler<DocumentGetModifiedCompletedEventArgs> DocumentGetModifiedCompleted;

	public event EventHandler<DocumentGetRingSizeIndexFromSizeTextCompletedEventArgs> DocumentGetRingSizeIndexFromSizeTextCompleted;

	public event EventHandler<DocumentGetRingSizeTextFromSizeIndexCompletedEventArgs> DocumentGetRingSizeTextFromSizeIndexCompleted;

	public event EventHandler<DocumentGetRingSizeTranslatedCompletedEventArgs> DocumentGetRingSizeTranslatedCompleted;

	public event EventHandler<DocumentGetStudioGemListCompletedEventArgs> DocumentGetStudioGemListCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentGraphGenerateCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentImportCSGPartCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentResetCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSetCameraViewCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSetCurvesAndPointsVisibilityCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSetFilenameCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSetModifiedCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSetRingSizeSettingCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSetShowCurvesCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSetShowStartsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentShowAxisCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentShowGridCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentSpinDesignCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentToggleGemSelectionModeCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentUndoBeginRecordCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentUserDataAddValueCompleted;

	public event EventHandler<DocumentUserDataFindValueCompletedEventArgs> DocumentUserDataFindValueCompleted;

	public event EventHandler<DocumentUserDataGetOrderDataCompletedEventArgs> DocumentUserDataGetOrderDataCompleted;

	public event EventHandler<DocumentUserDataGetAllDataElementsCompletedEventArgs> DocumentUserDataGetAllDataElementsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentWriteCompleted;

	public event EventHandler<AsyncCompletedEventArgs> DocumentZoomCameraToBoundingBoxCompleted;

	public event EventHandler<EditorGetPartsForCurveEditrFromGraphObjectCompletedEventArgs> EditorGetPartsForCurveEditrFromGraphObjectCompleted;

	public event EventHandler<AsyncCompletedEventArgs> EnableDrawingCompleted;

	public event EventHandler<ExportDesignAs3DMCompletedEventArgs> ExportDesignAs3DMCompleted;

	public event EventHandler<ExportDesignAsSTLCompletedEventArgs> ExportDesignAsSTLCompleted;

	public event EventHandler<AsyncCompletedEventArgs> ExportImportGroupsCompleted;

	public event EventHandler<FindProjetPrinterCompletedEventArgs> FindProjetPrinterCompleted;

	public event EventHandler<GetActivationStateCompletedEventArgs> GetActivationStateCompleted;

	public event EventHandler<GetAllRingSizesForCurrentRingSizeRegionCompletedEventArgs> GetAllRingSizesForCurrentRingSizeRegionCompleted;

	public event EventHandler<GetComponentsCompletedEventArgs> GetComponentsCompleted;

	public event EventHandler<GetCurrentViewportImageBitmapCompletedEventArgs> GetCurrentViewportImageBitmapCompleted;

	public event EventHandler<GetDesignerFromUserDataCompletedEventArgs> GetDesignerFromUserDataCompleted;

	public event EventHandler<GetDesignIterationCountCompletedEventArgs> GetDesignIterationCountCompleted;

	public event EventHandler<GetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedEventArgs> GetDisplayObjectBinObjByComponentAndMaterialClassificationCompleted;

	public event EventHandler<GetDocumentFilenameCompletedEventArgs> GetDocumentFilenameCompleted;

	public event EventHandler<GetFeaturedDesignIterationsCompletedEventArgs> GetFeaturedDesignIterationsCompleted;

	public event EventHandler<GetGemMaterialsCompletedEventArgs> GetGemMaterialsCompleted;

	public event EventHandler<GetMetalMaterialsCompletedEventArgs> GetMetalMaterialsCompleted;

	public event EventHandler<GetMetalVolumesPerComponentCompletedEventArgs> GetMetalVolumesPerComponentCompleted;

	public event EventHandler<GetNotesToCustomerForCurrentDesignCompletedEventArgs> GetNotesToCustomerForCurrentDesignCompleted;

	public event EventHandler<GetNumberofVisibleFreehandPartsCompletedEventArgs> GetNumberofVisibleFreehandPartsCompleted;

	public event EventHandler<GetPrinterMaterialCompletedEventArgs> GetPrinterMaterialCompleted;

	public event EventHandler<GetRhinoSerialCodeCompletedEventArgs> GetRhinoSerialCodeCompleted;

	public event EventHandler<GetRhinoThreadIdCompletedEventArgs> GetRhinoThreadIdCompleted;

	public event EventHandler<GetRhinoWindowHandleCompletedEventArgs> GetRhinoWindowHandleCompleted;

	public event EventHandler<GetSavedJobsCompletedEventArgs> GetSavedJobsCompleted;

	public event EventHandler<GetSPINumberFromUserDataCompletedEventArgs> GetSPINumberFromUserDataCompleted;

	public event EventHandler<GetThumbnailFromFileCompletedEventArgs> GetThumbnailFromFileCompleted;

	public event EventHandler<GetVisibleComponentsCompletedEventArgs> GetVisibleComponentsCompleted;

	public event EventHandler<AsyncCompletedEventArgs> GraphItemAddUserDataValueCompleted;

	public event EventHandler<GraphItemFindUserDataValueCompletedEventArgs> GraphItemFindUserDataValueCompleted;

	public event EventHandler<GraphRedoChangeCompletedEventArgs> GraphRedoChangeCompleted;

	public RhinoServicesClient(InstanceContext callbackInstance)
		: base(callbackInstance)
	{
	}

	public RhinoServicesClient(InstanceContext callbackInstance, string endpointConfigurationName)
		: base(callbackInstance, endpointConfigurationName)
	{
	}

	public RhinoServicesClient(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public RhinoServicesClient(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(callbackInstance, endpointConfigurationName, remoteAddress)
	{
	}

	public RhinoServicesClient(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
		: base(callbackInstance, binding, remoteAddress)
	{
	}

	public void ToggleFreehandCommonCenterMode(bool isFreehandCommonCenterModeEnabled)
	{
		base.Channel.ToggleFreehandCommonCenterMode(isFreehandCommonCenterModeEnabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginToggleFreehandCommonCenterMode(bool isFreehandCommonCenterModeEnabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginToggleFreehandCommonCenterMode(isFreehandCommonCenterModeEnabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndToggleFreehandCommonCenterMode(IAsyncResult result)
	{
		base.Channel.EndToggleFreehandCommonCenterMode(result);
	}

	private IAsyncResult OnBeginToggleFreehandCommonCenterMode(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isFreehandCommonCenterModeEnabled = (bool)inValues[0];
		return BeginToggleFreehandCommonCenterMode(isFreehandCommonCenterModeEnabled, callback, asyncState);
	}

	private object[] OnEndToggleFreehandCommonCenterMode(IAsyncResult result)
	{
		EndToggleFreehandCommonCenterMode(result);
		return null;
	}

	private void OnToggleFreehandCommonCenterModeCompleted(object state)
	{
		if (this.ToggleFreehandCommonCenterModeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ToggleFreehandCommonCenterModeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ToggleFreehandCommonCenterModeAsync(bool isFreehandCommonCenterModeEnabled)
	{
		ToggleFreehandCommonCenterModeAsync(isFreehandCommonCenterModeEnabled, null);
	}

	public void ToggleFreehandCommonCenterModeAsync(bool isFreehandCommonCenterModeEnabled, object userState)
	{
		if (onBeginToggleFreehandCommonCenterModeDelegate == null)
		{
			onBeginToggleFreehandCommonCenterModeDelegate = OnBeginToggleFreehandCommonCenterMode;
		}
		if (onEndToggleFreehandCommonCenterModeDelegate == null)
		{
			onEndToggleFreehandCommonCenterModeDelegate = OnEndToggleFreehandCommonCenterMode;
		}
		if (onToggleFreehandCommonCenterModeCompletedDelegate == null)
		{
			onToggleFreehandCommonCenterModeCompletedDelegate = OnToggleFreehandCommonCenterModeCompleted;
		}
		InvokeAsync(onBeginToggleFreehandCommonCenterModeDelegate, new object[1] { isFreehandCommonCenterModeEnabled }, onEndToggleFreehandCommonCenterModeDelegate, onToggleFreehandCommonCenterModeCompletedDelegate, userState);
	}

	public void ToggleFreehandVisibility(bool isVisible)
	{
		base.Channel.ToggleFreehandVisibility(isVisible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginToggleFreehandVisibility(bool isVisible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginToggleFreehandVisibility(isVisible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndToggleFreehandVisibility(IAsyncResult result)
	{
		base.Channel.EndToggleFreehandVisibility(result);
	}

	private IAsyncResult OnBeginToggleFreehandVisibility(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isVisible = (bool)inValues[0];
		return BeginToggleFreehandVisibility(isVisible, callback, asyncState);
	}

	private object[] OnEndToggleFreehandVisibility(IAsyncResult result)
	{
		EndToggleFreehandVisibility(result);
		return null;
	}

	private void OnToggleFreehandVisibilityCompleted(object state)
	{
		if (this.ToggleFreehandVisibilityCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ToggleFreehandVisibilityCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ToggleFreehandVisibilityAsync(bool isVisible)
	{
		ToggleFreehandVisibilityAsync(isVisible, null);
	}

	public void ToggleFreehandVisibilityAsync(bool isVisible, object userState)
	{
		if (onBeginToggleFreehandVisibilityDelegate == null)
		{
			onBeginToggleFreehandVisibilityDelegate = OnBeginToggleFreehandVisibility;
		}
		if (onEndToggleFreehandVisibilityDelegate == null)
		{
			onEndToggleFreehandVisibilityDelegate = OnEndToggleFreehandVisibility;
		}
		if (onToggleFreehandVisibilityCompletedDelegate == null)
		{
			onToggleFreehandVisibilityCompletedDelegate = OnToggleFreehandVisibilityCompleted;
		}
		InvokeAsync(onBeginToggleFreehandVisibilityDelegate, new object[1] { isVisible }, onEndToggleFreehandVisibilityDelegate, onToggleFreehandVisibilityCompletedDelegate, userState);
	}

	public bool GetFreehandComponentVisibility()
	{
		return base.Channel.GetFreehandComponentVisibility();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetFreehandComponentVisibility(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetFreehandComponentVisibility(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndGetFreehandComponentVisibility(IAsyncResult result)
	{
		return base.Channel.EndGetFreehandComponentVisibility(result);
	}

	private IAsyncResult OnBeginGetFreehandComponentVisibility(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetFreehandComponentVisibility(callback, asyncState);
	}

	private object[] OnEndGetFreehandComponentVisibility(IAsyncResult result)
	{
		bool flag = EndGetFreehandComponentVisibility(result);
		return new object[1] { flag };
	}

	private void OnGetFreehandComponentVisibilityCompleted(object state)
	{
		if (this.GetFreehandComponentVisibilityCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetFreehandComponentVisibilityCompleted(this, new GetFreehandComponentVisibilityCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetFreehandComponentVisibilityAsync()
	{
		GetFreehandComponentVisibilityAsync(null);
	}

	public void GetFreehandComponentVisibilityAsync(object userState)
	{
		if (onBeginGetFreehandComponentVisibilityDelegate == null)
		{
			onBeginGetFreehandComponentVisibilityDelegate = OnBeginGetFreehandComponentVisibility;
		}
		if (onEndGetFreehandComponentVisibilityDelegate == null)
		{
			onEndGetFreehandComponentVisibilityDelegate = OnEndGetFreehandComponentVisibility;
		}
		if (onGetFreehandComponentVisibilityCompletedDelegate == null)
		{
			onGetFreehandComponentVisibilityCompletedDelegate = OnGetFreehandComponentVisibilityCompleted;
		}
		InvokeAsync(onBeginGetFreehandComponentVisibilityDelegate, null, onEndGetFreehandComponentVisibilityDelegate, onGetFreehandComponentVisibilityCompletedDelegate, userState);
	}

	public StudioGem[] GetManualSizedStones()
	{
		return base.Channel.GetManualSizedStones();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetManualSizedStones(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetManualSizedStones(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public StudioGem[] EndGetManualSizedStones(IAsyncResult result)
	{
		return base.Channel.EndGetManualSizedStones(result);
	}

	private IAsyncResult OnBeginGetManualSizedStones(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetManualSizedStones(callback, asyncState);
	}

	private object[] OnEndGetManualSizedStones(IAsyncResult result)
	{
		StudioGem[] array = EndGetManualSizedStones(result);
		return new object[1] { array };
	}

	private void OnGetManualSizedStonesCompleted(object state)
	{
		if (this.GetManualSizedStonesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetManualSizedStonesCompleted(this, new GetManualSizedStonesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetManualSizedStonesAsync()
	{
		GetManualSizedStonesAsync(null);
	}

	public void GetManualSizedStonesAsync(object userState)
	{
		if (onBeginGetManualSizedStonesDelegate == null)
		{
			onBeginGetManualSizedStonesDelegate = OnBeginGetManualSizedStones;
		}
		if (onEndGetManualSizedStonesDelegate == null)
		{
			onEndGetManualSizedStonesDelegate = OnEndGetManualSizedStones;
		}
		if (onGetManualSizedStonesCompletedDelegate == null)
		{
			onGetManualSizedStonesCompletedDelegate = OnGetManualSizedStonesCompleted;
		}
		InvokeAsync(onBeginGetManualSizedStonesDelegate, null, onEndGetManualSizedStonesDelegate, onGetManualSizedStonesCompletedDelegate, userState);
	}

	public void ToggleQuarterCaratGemRounding(bool isQuarterCaratRoundingEnabled)
	{
		base.Channel.ToggleQuarterCaratGemRounding(isQuarterCaratRoundingEnabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginToggleQuarterCaratGemRounding(bool isQuarterCaratRoundingEnabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginToggleQuarterCaratGemRounding(isQuarterCaratRoundingEnabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndToggleQuarterCaratGemRounding(IAsyncResult result)
	{
		base.Channel.EndToggleQuarterCaratGemRounding(result);
	}

	private IAsyncResult OnBeginToggleQuarterCaratGemRounding(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isQuarterCaratRoundingEnabled = (bool)inValues[0];
		return BeginToggleQuarterCaratGemRounding(isQuarterCaratRoundingEnabled, callback, asyncState);
	}

	private object[] OnEndToggleQuarterCaratGemRounding(IAsyncResult result)
	{
		EndToggleQuarterCaratGemRounding(result);
		return null;
	}

	private void OnToggleQuarterCaratGemRoundingCompleted(object state)
	{
		if (this.ToggleQuarterCaratGemRoundingCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ToggleQuarterCaratGemRoundingCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ToggleQuarterCaratGemRoundingAsync(bool isQuarterCaratRoundingEnabled)
	{
		ToggleQuarterCaratGemRoundingAsync(isQuarterCaratRoundingEnabled, null);
	}

	public void ToggleQuarterCaratGemRoundingAsync(bool isQuarterCaratRoundingEnabled, object userState)
	{
		if (onBeginToggleQuarterCaratGemRoundingDelegate == null)
		{
			onBeginToggleQuarterCaratGemRoundingDelegate = OnBeginToggleQuarterCaratGemRounding;
		}
		if (onEndToggleQuarterCaratGemRoundingDelegate == null)
		{
			onEndToggleQuarterCaratGemRoundingDelegate = OnEndToggleQuarterCaratGemRounding;
		}
		if (onToggleQuarterCaratGemRoundingCompletedDelegate == null)
		{
			onToggleQuarterCaratGemRoundingCompletedDelegate = OnToggleQuarterCaratGemRoundingCompleted;
		}
		InvokeAsync(onBeginToggleQuarterCaratGemRoundingDelegate, new object[1] { isQuarterCaratRoundingEnabled }, onEndToggleQuarterCaratGemRoundingDelegate, onToggleQuarterCaratGemRoundingCompletedDelegate, userState);
	}

	public void ToggleViewportControls(bool isVisible)
	{
		base.Channel.ToggleViewportControls(isVisible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginToggleViewportControls(bool isVisible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginToggleViewportControls(isVisible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndToggleViewportControls(IAsyncResult result)
	{
		base.Channel.EndToggleViewportControls(result);
	}

	private IAsyncResult OnBeginToggleViewportControls(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isVisible = (bool)inValues[0];
		return BeginToggleViewportControls(isVisible, callback, asyncState);
	}

	private object[] OnEndToggleViewportControls(IAsyncResult result)
	{
		EndToggleViewportControls(result);
		return null;
	}

	private void OnToggleViewportControlsCompleted(object state)
	{
		if (this.ToggleViewportControlsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ToggleViewportControlsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ToggleViewportControlsAsync(bool isVisible)
	{
		ToggleViewportControlsAsync(isVisible, null);
	}

	public void ToggleViewportControlsAsync(bool isVisible, object userState)
	{
		if (onBeginToggleViewportControlsDelegate == null)
		{
			onBeginToggleViewportControlsDelegate = OnBeginToggleViewportControls;
		}
		if (onEndToggleViewportControlsDelegate == null)
		{
			onEndToggleViewportControlsDelegate = OnEndToggleViewportControls;
		}
		if (onToggleViewportControlsCompletedDelegate == null)
		{
			onToggleViewportControlsCompletedDelegate = OnToggleViewportControlsCompleted;
		}
		InvokeAsync(onBeginToggleViewportControlsDelegate, new object[1] { isVisible }, onEndToggleViewportControlsDelegate, onToggleViewportControlsCompletedDelegate, userState);
	}

	public void TurnHandOff()
	{
		base.Channel.TurnHandOff();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginTurnHandOff(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginTurnHandOff(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndTurnHandOff(IAsyncResult result)
	{
		base.Channel.EndTurnHandOff(result);
	}

	private IAsyncResult OnBeginTurnHandOff(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginTurnHandOff(callback, asyncState);
	}

	private object[] OnEndTurnHandOff(IAsyncResult result)
	{
		EndTurnHandOff(result);
		return null;
	}

	private void OnTurnHandOffCompleted(object state)
	{
		if (this.TurnHandOffCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.TurnHandOffCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void TurnHandOffAsync()
	{
		TurnHandOffAsync(null);
	}

	public void TurnHandOffAsync(object userState)
	{
		if (onBeginTurnHandOffDelegate == null)
		{
			onBeginTurnHandOffDelegate = OnBeginTurnHandOff;
		}
		if (onEndTurnHandOffDelegate == null)
		{
			onEndTurnHandOffDelegate = OnEndTurnHandOff;
		}
		if (onTurnHandOffCompletedDelegate == null)
		{
			onTurnHandOffCompletedDelegate = OnTurnHandOffCompleted;
		}
		InvokeAsync(onBeginTurnHandOffDelegate, null, onEndTurnHandOffDelegate, onTurnHandOffCompletedDelegate, userState);
	}

	public void UpdateCurveObjectFromUI(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, double[] points, bool mouseDown)
	{
		base.Channel.UpdateCurveObjectFromUI(blockInstanceGuid, whichCurveIndex, curve, points, mouseDown);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateCurveObjectFromUI(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, double[] points, bool mouseDown, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateCurveObjectFromUI(blockInstanceGuid, whichCurveIndex, curve, points, mouseDown, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateCurveObjectFromUI(IAsyncResult result)
	{
		base.Channel.EndUpdateCurveObjectFromUI(result);
	}

	private IAsyncResult OnBeginUpdateCurveObjectFromUI(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid blockInstanceGuid = (Guid)inValues[0];
		int whichCurveIndex = (int)inValues[1];
		NurbsCurve curve = (NurbsCurve)inValues[2];
		double[] points = (double[])inValues[3];
		bool mouseDown = (bool)inValues[4];
		return BeginUpdateCurveObjectFromUI(blockInstanceGuid, whichCurveIndex, curve, points, mouseDown, callback, asyncState);
	}

	private object[] OnEndUpdateCurveObjectFromUI(IAsyncResult result)
	{
		EndUpdateCurveObjectFromUI(result);
		return null;
	}

	private void OnUpdateCurveObjectFromUICompleted(object state)
	{
		if (this.UpdateCurveObjectFromUICompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateCurveObjectFromUICompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateCurveObjectFromUIAsync(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, double[] points, bool mouseDown)
	{
		UpdateCurveObjectFromUIAsync(blockInstanceGuid, whichCurveIndex, curve, points, mouseDown, null);
	}

	public void UpdateCurveObjectFromUIAsync(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, double[] points, bool mouseDown, object userState)
	{
		if (onBeginUpdateCurveObjectFromUIDelegate == null)
		{
			onBeginUpdateCurveObjectFromUIDelegate = OnBeginUpdateCurveObjectFromUI;
		}
		if (onEndUpdateCurveObjectFromUIDelegate == null)
		{
			onEndUpdateCurveObjectFromUIDelegate = OnEndUpdateCurveObjectFromUI;
		}
		if (onUpdateCurveObjectFromUICompletedDelegate == null)
		{
			onUpdateCurveObjectFromUICompletedDelegate = OnUpdateCurveObjectFromUICompleted;
		}
		InvokeAsync(onBeginUpdateCurveObjectFromUIDelegate, new object[5] { blockInstanceGuid, whichCurveIndex, curve, points, mouseDown }, onEndUpdateCurveObjectFromUIDelegate, onUpdateCurveObjectFromUICompletedDelegate, userState);
	}

	public bool UpdateFirmwareAndActivate(out string sError, bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany)
	{
		return base.Channel.UpdateFirmwareAndActivate(out sError, bDoFirmware, bDoActivate, sEmail, sCompany);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateFirmwareAndActivate(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateFirmwareAndActivate(bDoFirmware, bDoActivate, sEmail, sCompany, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndUpdateFirmwareAndActivate(out string sError, IAsyncResult result)
	{
		return base.Channel.EndUpdateFirmwareAndActivate(out sError, result);
	}

	private IAsyncResult OnBeginUpdateFirmwareAndActivate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool bDoFirmware = (bool)inValues[0];
		bool bDoActivate = (bool)inValues[1];
		string sEmail = (string)inValues[2];
		string sCompany = (string)inValues[3];
		return BeginUpdateFirmwareAndActivate(bDoFirmware, bDoActivate, sEmail, sCompany, callback, asyncState);
	}

	private object[] OnEndUpdateFirmwareAndActivate(IAsyncResult result)
	{
		string sError = GetDefaultValueForInitialization<string>();
		bool flag = EndUpdateFirmwareAndActivate(out sError, result);
		return new object[2] { sError, flag };
	}

	private void OnUpdateFirmwareAndActivateCompleted(object state)
	{
		if (this.UpdateFirmwareAndActivateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateFirmwareAndActivateCompleted(this, new UpdateFirmwareAndActivateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateFirmwareAndActivateAsync(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany)
	{
		UpdateFirmwareAndActivateAsync(bDoFirmware, bDoActivate, sEmail, sCompany, null);
	}

	public void UpdateFirmwareAndActivateAsync(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany, object userState)
	{
		if (onBeginUpdateFirmwareAndActivateDelegate == null)
		{
			onBeginUpdateFirmwareAndActivateDelegate = OnBeginUpdateFirmwareAndActivate;
		}
		if (onEndUpdateFirmwareAndActivateDelegate == null)
		{
			onEndUpdateFirmwareAndActivateDelegate = OnEndUpdateFirmwareAndActivate;
		}
		if (onUpdateFirmwareAndActivateCompletedDelegate == null)
		{
			onUpdateFirmwareAndActivateCompletedDelegate = OnUpdateFirmwareAndActivateCompleted;
		}
		InvokeAsync(onBeginUpdateFirmwareAndActivateDelegate, new object[4] { bDoFirmware, bDoActivate, sEmail, sCompany }, onEndUpdateFirmwareAndActivateDelegate, onUpdateFirmwareAndActivateCompletedDelegate, userState);
	}

	public void UpdateImportGroupPartListGroupAssignment(Guid[] importGroupIdentifiers, string groupName)
	{
		base.Channel.UpdateImportGroupPartListGroupAssignment(importGroupIdentifiers, groupName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateImportGroupPartListGroupAssignment(Guid[] importGroupIdentifiers, string groupName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateImportGroupPartListGroupAssignment(importGroupIdentifiers, groupName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateImportGroupPartListGroupAssignment(IAsyncResult result)
	{
		base.Channel.EndUpdateImportGroupPartListGroupAssignment(result);
	}

	private IAsyncResult OnBeginUpdateImportGroupPartListGroupAssignment(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiers = (Guid[])inValues[0];
		string groupName = (string)inValues[1];
		return BeginUpdateImportGroupPartListGroupAssignment(importGroupIdentifiers, groupName, callback, asyncState);
	}

	private object[] OnEndUpdateImportGroupPartListGroupAssignment(IAsyncResult result)
	{
		EndUpdateImportGroupPartListGroupAssignment(result);
		return null;
	}

	private void OnUpdateImportGroupPartListGroupAssignmentCompleted(object state)
	{
		if (this.UpdateImportGroupPartListGroupAssignmentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateImportGroupPartListGroupAssignmentCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateImportGroupPartListGroupAssignmentAsync(Guid[] importGroupIdentifiers, string groupName)
	{
		UpdateImportGroupPartListGroupAssignmentAsync(importGroupIdentifiers, groupName, null);
	}

	public void UpdateImportGroupPartListGroupAssignmentAsync(Guid[] importGroupIdentifiers, string groupName, object userState)
	{
		if (onBeginUpdateImportGroupPartListGroupAssignmentDelegate == null)
		{
			onBeginUpdateImportGroupPartListGroupAssignmentDelegate = OnBeginUpdateImportGroupPartListGroupAssignment;
		}
		if (onEndUpdateImportGroupPartListGroupAssignmentDelegate == null)
		{
			onEndUpdateImportGroupPartListGroupAssignmentDelegate = OnEndUpdateImportGroupPartListGroupAssignment;
		}
		if (onUpdateImportGroupPartListGroupAssignmentCompletedDelegate == null)
		{
			onUpdateImportGroupPartListGroupAssignmentCompletedDelegate = OnUpdateImportGroupPartListGroupAssignmentCompleted;
		}
		InvokeAsync(onBeginUpdateImportGroupPartListGroupAssignmentDelegate, new object[2] { importGroupIdentifiers, groupName }, onEndUpdateImportGroupPartListGroupAssignmentDelegate, onUpdateImportGroupPartListGroupAssignmentCompletedDelegate, userState);
	}

	public void UpdateImportGroupSortOrder(Guid[] importGroupGuids)
	{
		base.Channel.UpdateImportGroupSortOrder(importGroupGuids);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateImportGroupSortOrder(Guid[] importGroupGuids, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateImportGroupSortOrder(importGroupGuids, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateImportGroupSortOrder(IAsyncResult result)
	{
		base.Channel.EndUpdateImportGroupSortOrder(result);
	}

	private IAsyncResult OnBeginUpdateImportGroupSortOrder(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupGuids = (Guid[])inValues[0];
		return BeginUpdateImportGroupSortOrder(importGroupGuids, callback, asyncState);
	}

	private object[] OnEndUpdateImportGroupSortOrder(IAsyncResult result)
	{
		EndUpdateImportGroupSortOrder(result);
		return null;
	}

	private void OnUpdateImportGroupSortOrderCompleted(object state)
	{
		if (this.UpdateImportGroupSortOrderCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateImportGroupSortOrderCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateImportGroupSortOrderAsync(Guid[] importGroupGuids)
	{
		UpdateImportGroupSortOrderAsync(importGroupGuids, null);
	}

	public void UpdateImportGroupSortOrderAsync(Guid[] importGroupGuids, object userState)
	{
		if (onBeginUpdateImportGroupSortOrderDelegate == null)
		{
			onBeginUpdateImportGroupSortOrderDelegate = OnBeginUpdateImportGroupSortOrder;
		}
		if (onEndUpdateImportGroupSortOrderDelegate == null)
		{
			onEndUpdateImportGroupSortOrderDelegate = OnEndUpdateImportGroupSortOrder;
		}
		if (onUpdateImportGroupSortOrderCompletedDelegate == null)
		{
			onUpdateImportGroupSortOrderCompletedDelegate = OnUpdateImportGroupSortOrderCompleted;
		}
		InvokeAsync(onBeginUpdateImportGroupSortOrderDelegate, new object[1] { importGroupGuids }, onEndUpdateImportGroupSortOrderDelegate, onUpdateImportGroupSortOrderCompletedDelegate, userState);
	}

	public void UpdateRhinoAppearanceMaterials(string skinName, string polishName, bool isRenderLayDesignFlat)
	{
		base.Channel.UpdateRhinoAppearanceMaterials(skinName, polishName, isRenderLayDesignFlat);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateRhinoAppearanceMaterials(string skinName, string polishName, bool isRenderLayDesignFlat, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateRhinoAppearanceMaterials(skinName, polishName, isRenderLayDesignFlat, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateRhinoAppearanceMaterials(IAsyncResult result)
	{
		base.Channel.EndUpdateRhinoAppearanceMaterials(result);
	}

	private IAsyncResult OnBeginUpdateRhinoAppearanceMaterials(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string skinName = (string)inValues[0];
		string polishName = (string)inValues[1];
		bool isRenderLayDesignFlat = (bool)inValues[2];
		return BeginUpdateRhinoAppearanceMaterials(skinName, polishName, isRenderLayDesignFlat, callback, asyncState);
	}

	private object[] OnEndUpdateRhinoAppearanceMaterials(IAsyncResult result)
	{
		EndUpdateRhinoAppearanceMaterials(result);
		return null;
	}

	private void OnUpdateRhinoAppearanceMaterialsCompleted(object state)
	{
		if (this.UpdateRhinoAppearanceMaterialsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateRhinoAppearanceMaterialsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateRhinoAppearanceMaterialsAsync(string skinName, string polishName, bool isRenderLayDesignFlat)
	{
		UpdateRhinoAppearanceMaterialsAsync(skinName, polishName, isRenderLayDesignFlat, null);
	}

	public void UpdateRhinoAppearanceMaterialsAsync(string skinName, string polishName, bool isRenderLayDesignFlat, object userState)
	{
		if (onBeginUpdateRhinoAppearanceMaterialsDelegate == null)
		{
			onBeginUpdateRhinoAppearanceMaterialsDelegate = OnBeginUpdateRhinoAppearanceMaterials;
		}
		if (onEndUpdateRhinoAppearanceMaterialsDelegate == null)
		{
			onEndUpdateRhinoAppearanceMaterialsDelegate = OnEndUpdateRhinoAppearanceMaterials;
		}
		if (onUpdateRhinoAppearanceMaterialsCompletedDelegate == null)
		{
			onUpdateRhinoAppearanceMaterialsCompletedDelegate = OnUpdateRhinoAppearanceMaterialsCompleted;
		}
		InvokeAsync(onBeginUpdateRhinoAppearanceMaterialsDelegate, new object[3] { skinName, polishName, isRenderLayDesignFlat }, onEndUpdateRhinoAppearanceMaterialsDelegate, onUpdateRhinoAppearanceMaterialsCompletedDelegate, userState);
	}

	public void UpdateRhinoHand(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset)
	{
		base.Channel.UpdateRhinoHand(selectedRing, isRingOnRightHand, isRingFlipped, ringPositionOffset);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateRhinoHand(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateRhinoHand(selectedRing, isRingOnRightHand, isRingFlipped, ringPositionOffset, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateRhinoHand(IAsyncResult result)
	{
		base.Channel.EndUpdateRhinoHand(result);
	}

	private IAsyncResult OnBeginUpdateRhinoHand(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string selectedRing = (string)inValues[0];
		bool isRingOnRightHand = (bool)inValues[1];
		bool isRingFlipped = (bool)inValues[2];
		double ringPositionOffset = (double)inValues[3];
		return BeginUpdateRhinoHand(selectedRing, isRingOnRightHand, isRingFlipped, ringPositionOffset, callback, asyncState);
	}

	private object[] OnEndUpdateRhinoHand(IAsyncResult result)
	{
		EndUpdateRhinoHand(result);
		return null;
	}

	private void OnUpdateRhinoHandCompleted(object state)
	{
		if (this.UpdateRhinoHandCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateRhinoHandCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateRhinoHandAsync(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset)
	{
		UpdateRhinoHandAsync(selectedRing, isRingOnRightHand, isRingFlipped, ringPositionOffset, null);
	}

	public void UpdateRhinoHandAsync(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset, object userState)
	{
		if (onBeginUpdateRhinoHandDelegate == null)
		{
			onBeginUpdateRhinoHandDelegate = OnBeginUpdateRhinoHand;
		}
		if (onEndUpdateRhinoHandDelegate == null)
		{
			onEndUpdateRhinoHandDelegate = OnEndUpdateRhinoHand;
		}
		if (onUpdateRhinoHandCompletedDelegate == null)
		{
			onUpdateRhinoHandCompletedDelegate = OnUpdateRhinoHandCompleted;
		}
		InvokeAsync(onBeginUpdateRhinoHandDelegate, new object[4] { selectedRing, isRingOnRightHand, isRingFlipped, ringPositionOffset }, onEndUpdateRhinoHandDelegate, onUpdateRhinoHandCompletedDelegate, userState);
	}

	public void UpdateStudioUICallbackChannel()
	{
		base.Channel.UpdateStudioUICallbackChannel();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateStudioUICallbackChannel(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateStudioUICallbackChannel(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateStudioUICallbackChannel(IAsyncResult result)
	{
		base.Channel.EndUpdateStudioUICallbackChannel(result);
	}

	private IAsyncResult OnBeginUpdateStudioUICallbackChannel(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginUpdateStudioUICallbackChannel(callback, asyncState);
	}

	private object[] OnEndUpdateStudioUICallbackChannel(IAsyncResult result)
	{
		EndUpdateStudioUICallbackChannel(result);
		return null;
	}

	private void OnUpdateStudioUICallbackChannelCompleted(object state)
	{
		if (this.UpdateStudioUICallbackChannelCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateStudioUICallbackChannelCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateStudioUICallbackChannelAsync()
	{
		UpdateStudioUICallbackChannelAsync(null);
	}

	public void UpdateStudioUICallbackChannelAsync(object userState)
	{
		if (onBeginUpdateStudioUICallbackChannelDelegate == null)
		{
			onBeginUpdateStudioUICallbackChannelDelegate = OnBeginUpdateStudioUICallbackChannel;
		}
		if (onEndUpdateStudioUICallbackChannelDelegate == null)
		{
			onEndUpdateStudioUICallbackChannelDelegate = OnEndUpdateStudioUICallbackChannel;
		}
		if (onUpdateStudioUICallbackChannelCompletedDelegate == null)
		{
			onUpdateStudioUICallbackChannelCompletedDelegate = OnUpdateStudioUICallbackChannelCompleted;
		}
		InvokeAsync(onBeginUpdateStudioUICallbackChannelDelegate, null, onEndUpdateStudioUICallbackChannelDelegate, onUpdateStudioUICallbackChannelCompletedDelegate, userState);
	}

	public UIControl[] UpdateUIControl(UIControl uiControl)
	{
		return base.Channel.UpdateUIControl(uiControl);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateUIControl(UIControl uiControl, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateUIControl(uiControl, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndUpdateUIControl(IAsyncResult result)
	{
		return base.Channel.EndUpdateUIControl(result);
	}

	private IAsyncResult OnBeginUpdateUIControl(object[] inValues, AsyncCallback callback, object asyncState)
	{
		UIControl uiControl = (UIControl)inValues[0];
		return BeginUpdateUIControl(uiControl, callback, asyncState);
	}

	private object[] OnEndUpdateUIControl(IAsyncResult result)
	{
		UIControl[] array = EndUpdateUIControl(result);
		return new object[1] { array };
	}

	private void OnUpdateUIControlCompleted(object state)
	{
		if (this.UpdateUIControlCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateUIControlCompleted(this, new UpdateUIControlCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateUIControlAsync(UIControl uiControl)
	{
		UpdateUIControlAsync(uiControl, null);
	}

	public void UpdateUIControlAsync(UIControl uiControl, object userState)
	{
		if (onBeginUpdateUIControlDelegate == null)
		{
			onBeginUpdateUIControlDelegate = OnBeginUpdateUIControl;
		}
		if (onEndUpdateUIControlDelegate == null)
		{
			onEndUpdateUIControlDelegate = OnEndUpdateUIControl;
		}
		if (onUpdateUIControlCompletedDelegate == null)
		{
			onUpdateUIControlCompletedDelegate = OnUpdateUIControlCompleted;
		}
		InvokeAsync(onBeginUpdateUIControlDelegate, new object[1] { uiControl }, onEndUpdateUIControlDelegate, onUpdateUIControlCompletedDelegate, userState);
	}

	public UIControl[] UpdateUIControlQuick(UIControl uiControl)
	{
		return base.Channel.UpdateUIControlQuick(uiControl);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateUIControlQuick(UIControl uiControl, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateUIControlQuick(uiControl, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndUpdateUIControlQuick(IAsyncResult result)
	{
		return base.Channel.EndUpdateUIControlQuick(result);
	}

	private IAsyncResult OnBeginUpdateUIControlQuick(object[] inValues, AsyncCallback callback, object asyncState)
	{
		UIControl uiControl = (UIControl)inValues[0];
		return BeginUpdateUIControlQuick(uiControl, callback, asyncState);
	}

	private object[] OnEndUpdateUIControlQuick(IAsyncResult result)
	{
		UIControl[] array = EndUpdateUIControlQuick(result);
		return new object[1] { array };
	}

	private void OnUpdateUIControlQuickCompleted(object state)
	{
		if (this.UpdateUIControlQuickCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateUIControlQuickCompleted(this, new UpdateUIControlQuickCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateUIControlQuickAsync(UIControl uiControl)
	{
		UpdateUIControlQuickAsync(uiControl, null);
	}

	public void UpdateUIControlQuickAsync(UIControl uiControl, object userState)
	{
		if (onBeginUpdateUIControlQuickDelegate == null)
		{
			onBeginUpdateUIControlQuickDelegate = OnBeginUpdateUIControlQuick;
		}
		if (onEndUpdateUIControlQuickDelegate == null)
		{
			onEndUpdateUIControlQuickDelegate = OnEndUpdateUIControlQuick;
		}
		if (onUpdateUIControlQuickCompletedDelegate == null)
		{
			onUpdateUIControlQuickCompletedDelegate = OnUpdateUIControlQuickCompleted;
		}
		InvokeAsync(onBeginUpdateUIControlQuickDelegate, new object[1] { uiControl }, onEndUpdateUIControlQuickDelegate, onUpdateUIControlQuickCompletedDelegate, userState);
	}

	public void UpdateValueChooserFromUI(Guid blockInstanceGuid)
	{
		base.Channel.UpdateValueChooserFromUI(blockInstanceGuid);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateValueChooserFromUI(Guid blockInstanceGuid, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateValueChooserFromUI(blockInstanceGuid, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateValueChooserFromUI(IAsyncResult result)
	{
		base.Channel.EndUpdateValueChooserFromUI(result);
	}

	private IAsyncResult OnBeginUpdateValueChooserFromUI(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid blockInstanceGuid = (Guid)inValues[0];
		return BeginUpdateValueChooserFromUI(blockInstanceGuid, callback, asyncState);
	}

	private object[] OnEndUpdateValueChooserFromUI(IAsyncResult result)
	{
		EndUpdateValueChooserFromUI(result);
		return null;
	}

	private void OnUpdateValueChooserFromUICompleted(object state)
	{
		if (this.UpdateValueChooserFromUICompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateValueChooserFromUICompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateValueChooserFromUIAsync(Guid blockInstanceGuid)
	{
		UpdateValueChooserFromUIAsync(blockInstanceGuid, null);
	}

	public void UpdateValueChooserFromUIAsync(Guid blockInstanceGuid, object userState)
	{
		if (onBeginUpdateValueChooserFromUIDelegate == null)
		{
			onBeginUpdateValueChooserFromUIDelegate = OnBeginUpdateValueChooserFromUI;
		}
		if (onEndUpdateValueChooserFromUIDelegate == null)
		{
			onEndUpdateValueChooserFromUIDelegate = OnEndUpdateValueChooserFromUI;
		}
		if (onUpdateValueChooserFromUICompletedDelegate == null)
		{
			onUpdateValueChooserFromUICompletedDelegate = OnUpdateValueChooserFromUICompleted;
		}
		InvokeAsync(onBeginUpdateValueChooserFromUIDelegate, new object[1] { blockInstanceGuid }, onEndUpdateValueChooserFromUIDelegate, onUpdateValueChooserFromUICompletedDelegate, userState);
	}

	public void UpdateViewPortHandleDrawScale(double scaleFactor)
	{
		base.Channel.UpdateViewPortHandleDrawScale(scaleFactor);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateViewPortHandleDrawScale(double scaleFactor, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateViewPortHandleDrawScale(scaleFactor, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndUpdateViewPortHandleDrawScale(IAsyncResult result)
	{
		base.Channel.EndUpdateViewPortHandleDrawScale(result);
	}

	private IAsyncResult OnBeginUpdateViewPortHandleDrawScale(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double scaleFactor = (double)inValues[0];
		return BeginUpdateViewPortHandleDrawScale(scaleFactor, callback, asyncState);
	}

	private object[] OnEndUpdateViewPortHandleDrawScale(IAsyncResult result)
	{
		EndUpdateViewPortHandleDrawScale(result);
		return null;
	}

	private void OnUpdateViewPortHandleDrawScaleCompleted(object state)
	{
		if (this.UpdateViewPortHandleDrawScaleCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateViewPortHandleDrawScaleCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateViewPortHandleDrawScaleAsync(double scaleFactor)
	{
		UpdateViewPortHandleDrawScaleAsync(scaleFactor, null);
	}

	public void UpdateViewPortHandleDrawScaleAsync(double scaleFactor, object userState)
	{
		if (onBeginUpdateViewPortHandleDrawScaleDelegate == null)
		{
			onBeginUpdateViewPortHandleDrawScaleDelegate = OnBeginUpdateViewPortHandleDrawScale;
		}
		if (onEndUpdateViewPortHandleDrawScaleDelegate == null)
		{
			onEndUpdateViewPortHandleDrawScaleDelegate = OnEndUpdateViewPortHandleDrawScale;
		}
		if (onUpdateViewPortHandleDrawScaleCompletedDelegate == null)
		{
			onUpdateViewPortHandleDrawScaleCompletedDelegate = OnUpdateViewPortHandleDrawScaleCompleted;
		}
		InvokeAsync(onBeginUpdateViewPortHandleDrawScaleDelegate, new object[1] { scaleFactor }, onEndUpdateViewPortHandleDrawScaleDelegate, onUpdateViewPortHandleDrawScaleCompletedDelegate, userState);
	}

	public void ViewportSetBackgroundAndColor(string backgroundColor, string backgroundGraphicPath)
	{
		base.Channel.ViewportSetBackgroundAndColor(backgroundColor, backgroundGraphicPath);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginViewportSetBackgroundAndColor(string backgroundColor, string backgroundGraphicPath, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginViewportSetBackgroundAndColor(backgroundColor, backgroundGraphicPath, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndViewportSetBackgroundAndColor(IAsyncResult result)
	{
		base.Channel.EndViewportSetBackgroundAndColor(result);
	}

	private IAsyncResult OnBeginViewportSetBackgroundAndColor(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string backgroundColor = (string)inValues[0];
		string backgroundGraphicPath = (string)inValues[1];
		return BeginViewportSetBackgroundAndColor(backgroundColor, backgroundGraphicPath, callback, asyncState);
	}

	private object[] OnEndViewportSetBackgroundAndColor(IAsyncResult result)
	{
		EndViewportSetBackgroundAndColor(result);
		return null;
	}

	private void OnViewportSetBackgroundAndColorCompleted(object state)
	{
		if (this.ViewportSetBackgroundAndColorCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ViewportSetBackgroundAndColorCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ViewportSetBackgroundAndColorAsync(string backgroundColor, string backgroundGraphicPath)
	{
		ViewportSetBackgroundAndColorAsync(backgroundColor, backgroundGraphicPath, null);
	}

	public void ViewportSetBackgroundAndColorAsync(string backgroundColor, string backgroundGraphicPath, object userState)
	{
		if (onBeginViewportSetBackgroundAndColorDelegate == null)
		{
			onBeginViewportSetBackgroundAndColorDelegate = OnBeginViewportSetBackgroundAndColor;
		}
		if (onEndViewportSetBackgroundAndColorDelegate == null)
		{
			onEndViewportSetBackgroundAndColorDelegate = OnEndViewportSetBackgroundAndColor;
		}
		if (onViewportSetBackgroundAndColorCompletedDelegate == null)
		{
			onViewportSetBackgroundAndColorCompletedDelegate = OnViewportSetBackgroundAndColorCompleted;
		}
		InvokeAsync(onBeginViewportSetBackgroundAndColorDelegate, new object[2] { backgroundColor, backgroundGraphicPath }, onEndViewportSetBackgroundAndColorDelegate, onViewportSetBackgroundAndColorCompletedDelegate, userState);
	}

	public void ViewportSetHighlightColor(Color color)
	{
		base.Channel.ViewportSetHighlightColor(color);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginViewportSetHighlightColor(Color color, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginViewportSetHighlightColor(color, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndViewportSetHighlightColor(IAsyncResult result)
	{
		base.Channel.EndViewportSetHighlightColor(result);
	}

	private IAsyncResult OnBeginViewportSetHighlightColor(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Color color = (Color)inValues[0];
		return BeginViewportSetHighlightColor(color, callback, asyncState);
	}

	private object[] OnEndViewportSetHighlightColor(IAsyncResult result)
	{
		EndViewportSetHighlightColor(result);
		return null;
	}

	private void OnViewportSetHighlightColorCompleted(object state)
	{
		if (this.ViewportSetHighlightColorCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ViewportSetHighlightColorCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ViewportSetHighlightColorAsync(Color color)
	{
		ViewportSetHighlightColorAsync(color, null);
	}

	public void ViewportSetHighlightColorAsync(Color color, object userState)
	{
		if (onBeginViewportSetHighlightColorDelegate == null)
		{
			onBeginViewportSetHighlightColorDelegate = OnBeginViewportSetHighlightColor;
		}
		if (onEndViewportSetHighlightColorDelegate == null)
		{
			onEndViewportSetHighlightColorDelegate = OnEndViewportSetHighlightColor;
		}
		if (onViewportSetHighlightColorCompletedDelegate == null)
		{
			onViewportSetHighlightColorCompletedDelegate = OnViewportSetHighlightColorCompleted;
		}
		InvokeAsync(onBeginViewportSetHighlightColorDelegate, new object[1] { color }, onEndViewportSetHighlightColorDelegate, onViewportSetHighlightColorCompletedDelegate, userState);
	}

	public void ViewportSetTargetAndCameraLocation(double x, double y, double z)
	{
		base.Channel.ViewportSetTargetAndCameraLocation(x, y, z);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginViewportSetTargetAndCameraLocation(double x, double y, double z, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginViewportSetTargetAndCameraLocation(x, y, z, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndViewportSetTargetAndCameraLocation(IAsyncResult result)
	{
		base.Channel.EndViewportSetTargetAndCameraLocation(result);
	}

	private IAsyncResult OnBeginViewportSetTargetAndCameraLocation(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double x = (double)inValues[0];
		double y = (double)inValues[1];
		double z = (double)inValues[2];
		return BeginViewportSetTargetAndCameraLocation(x, y, z, callback, asyncState);
	}

	private object[] OnEndViewportSetTargetAndCameraLocation(IAsyncResult result)
	{
		EndViewportSetTargetAndCameraLocation(result);
		return null;
	}

	private void OnViewportSetTargetAndCameraLocationCompleted(object state)
	{
		if (this.ViewportSetTargetAndCameraLocationCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ViewportSetTargetAndCameraLocationCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ViewportSetTargetAndCameraLocationAsync(double x, double y, double z)
	{
		ViewportSetTargetAndCameraLocationAsync(x, y, z, null);
	}

	public void ViewportSetTargetAndCameraLocationAsync(double x, double y, double z, object userState)
	{
		if (onBeginViewportSetTargetAndCameraLocationDelegate == null)
		{
			onBeginViewportSetTargetAndCameraLocationDelegate = OnBeginViewportSetTargetAndCameraLocation;
		}
		if (onEndViewportSetTargetAndCameraLocationDelegate == null)
		{
			onEndViewportSetTargetAndCameraLocationDelegate = OnEndViewportSetTargetAndCameraLocation;
		}
		if (onViewportSetTargetAndCameraLocationCompletedDelegate == null)
		{
			onViewportSetTargetAndCameraLocationCompletedDelegate = OnViewportSetTargetAndCameraLocationCompleted;
		}
		InvokeAsync(onBeginViewportSetTargetAndCameraLocationDelegate, new object[3] { x, y, z }, onEndViewportSetTargetAndCameraLocationDelegate, onViewportSetTargetAndCameraLocationCompletedDelegate, userState);
	}

	public void WatcherSetEnabled(bool enabled)
	{
		base.Channel.WatcherSetEnabled(enabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginWatcherSetEnabled(bool enabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginWatcherSetEnabled(enabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndWatcherSetEnabled(IAsyncResult result)
	{
		base.Channel.EndWatcherSetEnabled(result);
	}

	private IAsyncResult OnBeginWatcherSetEnabled(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool enabled = (bool)inValues[0];
		return BeginWatcherSetEnabled(enabled, callback, asyncState);
	}

	private object[] OnEndWatcherSetEnabled(IAsyncResult result)
	{
		EndWatcherSetEnabled(result);
		return null;
	}

	private void OnWatcherSetEnabledCompleted(object state)
	{
		if (this.WatcherSetEnabledCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.WatcherSetEnabledCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void WatcherSetEnabledAsync(bool enabled)
	{
		WatcherSetEnabledAsync(enabled, null);
	}

	public void WatcherSetEnabledAsync(bool enabled, object userState)
	{
		if (onBeginWatcherSetEnabledDelegate == null)
		{
			onBeginWatcherSetEnabledDelegate = OnBeginWatcherSetEnabled;
		}
		if (onEndWatcherSetEnabledDelegate == null)
		{
			onEndWatcherSetEnabledDelegate = OnEndWatcherSetEnabled;
		}
		if (onWatcherSetEnabledCompletedDelegate == null)
		{
			onWatcherSetEnabledCompletedDelegate = OnWatcherSetEnabledCompleted;
		}
		InvokeAsync(onBeginWatcherSetEnabledDelegate, new object[1] { enabled }, onEndWatcherSetEnabledDelegate, onWatcherSetEnabledCompletedDelegate, userState);
	}

	public void GraphItemRemoveUserDataValue(Guid blockInstanceGuid, string typeName, string keyName)
	{
		base.Channel.GraphItemRemoveUserDataValue(blockInstanceGuid, typeName, keyName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGraphItemRemoveUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGraphItemRemoveUserDataValue(blockInstanceGuid, typeName, keyName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndGraphItemRemoveUserDataValue(IAsyncResult result)
	{
		base.Channel.EndGraphItemRemoveUserDataValue(result);
	}

	private IAsyncResult OnBeginGraphItemRemoveUserDataValue(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid blockInstanceGuid = (Guid)inValues[0];
		string typeName = (string)inValues[1];
		string keyName = (string)inValues[2];
		return BeginGraphItemRemoveUserDataValue(blockInstanceGuid, typeName, keyName, callback, asyncState);
	}

	private object[] OnEndGraphItemRemoveUserDataValue(IAsyncResult result)
	{
		EndGraphItemRemoveUserDataValue(result);
		return null;
	}

	private void OnGraphItemRemoveUserDataValueCompleted(object state)
	{
		if (this.GraphItemRemoveUserDataValueCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GraphItemRemoveUserDataValueCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GraphItemRemoveUserDataValueAsync(Guid blockInstanceGuid, string typeName, string keyName)
	{
		GraphItemRemoveUserDataValueAsync(blockInstanceGuid, typeName, keyName, null);
	}

	public void GraphItemRemoveUserDataValueAsync(Guid blockInstanceGuid, string typeName, string keyName, object userState)
	{
		if (onBeginGraphItemRemoveUserDataValueDelegate == null)
		{
			onBeginGraphItemRemoveUserDataValueDelegate = OnBeginGraphItemRemoveUserDataValue;
		}
		if (onEndGraphItemRemoveUserDataValueDelegate == null)
		{
			onEndGraphItemRemoveUserDataValueDelegate = OnEndGraphItemRemoveUserDataValue;
		}
		if (onGraphItemRemoveUserDataValueCompletedDelegate == null)
		{
			onGraphItemRemoveUserDataValueCompletedDelegate = OnGraphItemRemoveUserDataValueCompleted;
		}
		InvokeAsync(onBeginGraphItemRemoveUserDataValueDelegate, new object[3] { blockInstanceGuid, typeName, keyName }, onEndGraphItemRemoveUserDataValueDelegate, onGraphItemRemoveUserDataValueCompletedDelegate, userState);
	}

	public UIControl[] GraphUndoChange()
	{
		return base.Channel.GraphUndoChange();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGraphUndoChange(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGraphUndoChange(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndGraphUndoChange(IAsyncResult result)
	{
		return base.Channel.EndGraphUndoChange(result);
	}

	private IAsyncResult OnBeginGraphUndoChange(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGraphUndoChange(callback, asyncState);
	}

	private object[] OnEndGraphUndoChange(IAsyncResult result)
	{
		UIControl[] array = EndGraphUndoChange(result);
		return new object[1] { array };
	}

	private void OnGraphUndoChangeCompleted(object state)
	{
		if (this.GraphUndoChangeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GraphUndoChangeCompleted(this, new GraphUndoChangeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GraphUndoChangeAsync()
	{
		GraphUndoChangeAsync(null);
	}

	public void GraphUndoChangeAsync(object userState)
	{
		if (onBeginGraphUndoChangeDelegate == null)
		{
			onBeginGraphUndoChangeDelegate = OnBeginGraphUndoChange;
		}
		if (onEndGraphUndoChangeDelegate == null)
		{
			onEndGraphUndoChangeDelegate = OnEndGraphUndoChange;
		}
		if (onGraphUndoChangeCompletedDelegate == null)
		{
			onGraphUndoChangeCompletedDelegate = OnGraphUndoChangeCompleted;
		}
		InvokeAsync(onBeginGraphUndoChangeDelegate, null, onEndGraphUndoChangeDelegate, onGraphUndoChangeCompletedDelegate, userState);
	}

	public void HighlightAccentGems(int gemId, bool highlightAll)
	{
		base.Channel.HighlightAccentGems(gemId, highlightAll);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginHighlightAccentGems(int gemId, bool highlightAll, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginHighlightAccentGems(gemId, highlightAll, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndHighlightAccentGems(IAsyncResult result)
	{
		base.Channel.EndHighlightAccentGems(result);
	}

	private IAsyncResult OnBeginHighlightAccentGems(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int gemId = (int)inValues[0];
		bool highlightAll = (bool)inValues[1];
		return BeginHighlightAccentGems(gemId, highlightAll, callback, asyncState);
	}

	private object[] OnEndHighlightAccentGems(IAsyncResult result)
	{
		EndHighlightAccentGems(result);
		return null;
	}

	private void OnHighlightAccentGemsCompleted(object state)
	{
		if (this.HighlightAccentGemsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.HighlightAccentGemsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void HighlightAccentGemsAsync(int gemId, bool highlightAll)
	{
		HighlightAccentGemsAsync(gemId, highlightAll, null);
	}

	public void HighlightAccentGemsAsync(int gemId, bool highlightAll, object userState)
	{
		if (onBeginHighlightAccentGemsDelegate == null)
		{
			onBeginHighlightAccentGemsDelegate = OnBeginHighlightAccentGems;
		}
		if (onEndHighlightAccentGemsDelegate == null)
		{
			onEndHighlightAccentGemsDelegate = OnEndHighlightAccentGems;
		}
		if (onHighlightAccentGemsCompletedDelegate == null)
		{
			onHighlightAccentGemsCompletedDelegate = OnHighlightAccentGemsCompleted;
		}
		InvokeAsync(onBeginHighlightAccentGemsDelegate, new object[2] { gemId, highlightAll }, onEndHighlightAccentGemsDelegate, onHighlightAccentGemsCompletedDelegate, userState);
	}

	public void HighlightGemFromGemReportByGemNumber(string[] gemNumbers)
	{
		base.Channel.HighlightGemFromGemReportByGemNumber(gemNumbers);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginHighlightGemFromGemReportByGemNumber(string[] gemNumbers, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginHighlightGemFromGemReportByGemNumber(gemNumbers, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndHighlightGemFromGemReportByGemNumber(IAsyncResult result)
	{
		base.Channel.EndHighlightGemFromGemReportByGemNumber(result);
	}

	private IAsyncResult OnBeginHighlightGemFromGemReportByGemNumber(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string[] gemNumbers = (string[])inValues[0];
		return BeginHighlightGemFromGemReportByGemNumber(gemNumbers, callback, asyncState);
	}

	private object[] OnEndHighlightGemFromGemReportByGemNumber(IAsyncResult result)
	{
		EndHighlightGemFromGemReportByGemNumber(result);
		return null;
	}

	private void OnHighlightGemFromGemReportByGemNumberCompleted(object state)
	{
		if (this.HighlightGemFromGemReportByGemNumberCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.HighlightGemFromGemReportByGemNumberCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void HighlightGemFromGemReportByGemNumberAsync(string[] gemNumbers)
	{
		HighlightGemFromGemReportByGemNumberAsync(gemNumbers, null);
	}

	public void HighlightGemFromGemReportByGemNumberAsync(string[] gemNumbers, object userState)
	{
		if (onBeginHighlightGemFromGemReportByGemNumberDelegate == null)
		{
			onBeginHighlightGemFromGemReportByGemNumberDelegate = OnBeginHighlightGemFromGemReportByGemNumber;
		}
		if (onEndHighlightGemFromGemReportByGemNumberDelegate == null)
		{
			onEndHighlightGemFromGemReportByGemNumberDelegate = OnEndHighlightGemFromGemReportByGemNumber;
		}
		if (onHighlightGemFromGemReportByGemNumberCompletedDelegate == null)
		{
			onHighlightGemFromGemReportByGemNumberCompletedDelegate = OnHighlightGemFromGemReportByGemNumberCompleted;
		}
		InvokeAsync(onBeginHighlightGemFromGemReportByGemNumberDelegate, new object[1] { gemNumbers }, onEndHighlightGemFromGemReportByGemNumberDelegate, onHighlightGemFromGemReportByGemNumberCompletedDelegate, userState);
	}

	public void HighlightGemFromGemReportByIndex(int[] gemIndexes)
	{
		base.Channel.HighlightGemFromGemReportByIndex(gemIndexes);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginHighlightGemFromGemReportByIndex(int[] gemIndexes, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginHighlightGemFromGemReportByIndex(gemIndexes, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndHighlightGemFromGemReportByIndex(IAsyncResult result)
	{
		base.Channel.EndHighlightGemFromGemReportByIndex(result);
	}

	private IAsyncResult OnBeginHighlightGemFromGemReportByIndex(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int[] gemIndexes = (int[])inValues[0];
		return BeginHighlightGemFromGemReportByIndex(gemIndexes, callback, asyncState);
	}

	private object[] OnEndHighlightGemFromGemReportByIndex(IAsyncResult result)
	{
		EndHighlightGemFromGemReportByIndex(result);
		return null;
	}

	private void OnHighlightGemFromGemReportByIndexCompleted(object state)
	{
		if (this.HighlightGemFromGemReportByIndexCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.HighlightGemFromGemReportByIndexCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void HighlightGemFromGemReportByIndexAsync(int[] gemIndexes)
	{
		HighlightGemFromGemReportByIndexAsync(gemIndexes, null);
	}

	public void HighlightGemFromGemReportByIndexAsync(int[] gemIndexes, object userState)
	{
		if (onBeginHighlightGemFromGemReportByIndexDelegate == null)
		{
			onBeginHighlightGemFromGemReportByIndexDelegate = OnBeginHighlightGemFromGemReportByIndex;
		}
		if (onEndHighlightGemFromGemReportByIndexDelegate == null)
		{
			onEndHighlightGemFromGemReportByIndexDelegate = OnEndHighlightGemFromGemReportByIndex;
		}
		if (onHighlightGemFromGemReportByIndexCompletedDelegate == null)
		{
			onHighlightGemFromGemReportByIndexCompletedDelegate = OnHighlightGemFromGemReportByIndexCompleted;
		}
		InvokeAsync(onBeginHighlightGemFromGemReportByIndexDelegate, new object[1] { gemIndexes }, onEndHighlightGemFromGemReportByIndexDelegate, onHighlightGemFromGemReportByIndexCompletedDelegate, userState);
	}

	public void HighlightGraphItem(Guid blockInstanceGuid, bool showHighlight)
	{
		base.Channel.HighlightGraphItem(blockInstanceGuid, showHighlight);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginHighlightGraphItem(Guid blockInstanceGuid, bool showHighlight, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginHighlightGraphItem(blockInstanceGuid, showHighlight, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndHighlightGraphItem(IAsyncResult result)
	{
		base.Channel.EndHighlightGraphItem(result);
	}

	private IAsyncResult OnBeginHighlightGraphItem(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid blockInstanceGuid = (Guid)inValues[0];
		bool showHighlight = (bool)inValues[1];
		return BeginHighlightGraphItem(blockInstanceGuid, showHighlight, callback, asyncState);
	}

	private object[] OnEndHighlightGraphItem(IAsyncResult result)
	{
		EndHighlightGraphItem(result);
		return null;
	}

	private void OnHighlightGraphItemCompleted(object state)
	{
		if (this.HighlightGraphItemCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.HighlightGraphItemCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void HighlightGraphItemAsync(Guid blockInstanceGuid, bool showHighlight)
	{
		HighlightGraphItemAsync(blockInstanceGuid, showHighlight, null);
	}

	public void HighlightGraphItemAsync(Guid blockInstanceGuid, bool showHighlight, object userState)
	{
		if (onBeginHighlightGraphItemDelegate == null)
		{
			onBeginHighlightGraphItemDelegate = OnBeginHighlightGraphItem;
		}
		if (onEndHighlightGraphItemDelegate == null)
		{
			onEndHighlightGraphItemDelegate = OnEndHighlightGraphItem;
		}
		if (onHighlightGraphItemCompletedDelegate == null)
		{
			onHighlightGraphItemCompletedDelegate = OnHighlightGraphItemCompleted;
		}
		InvokeAsync(onBeginHighlightGraphItemDelegate, new object[2] { blockInstanceGuid, showHighlight }, onEndHighlightGraphItemDelegate, onHighlightGraphItemCompletedDelegate, userState);
	}

	public void HighlightItemsByBlockInstanceGuid(Guid[] instanceGuids)
	{
		base.Channel.HighlightItemsByBlockInstanceGuid(instanceGuids);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginHighlightItemsByBlockInstanceGuid(Guid[] instanceGuids, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginHighlightItemsByBlockInstanceGuid(instanceGuids, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndHighlightItemsByBlockInstanceGuid(IAsyncResult result)
	{
		base.Channel.EndHighlightItemsByBlockInstanceGuid(result);
	}

	private IAsyncResult OnBeginHighlightItemsByBlockInstanceGuid(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] instanceGuids = (Guid[])inValues[0];
		return BeginHighlightItemsByBlockInstanceGuid(instanceGuids, callback, asyncState);
	}

	private object[] OnEndHighlightItemsByBlockInstanceGuid(IAsyncResult result)
	{
		EndHighlightItemsByBlockInstanceGuid(result);
		return null;
	}

	private void OnHighlightItemsByBlockInstanceGuidCompleted(object state)
	{
		if (this.HighlightItemsByBlockInstanceGuidCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.HighlightItemsByBlockInstanceGuidCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void HighlightItemsByBlockInstanceGuidAsync(Guid[] instanceGuids)
	{
		HighlightItemsByBlockInstanceGuidAsync(instanceGuids, null);
	}

	public void HighlightItemsByBlockInstanceGuidAsync(Guid[] instanceGuids, object userState)
	{
		if (onBeginHighlightItemsByBlockInstanceGuidDelegate == null)
		{
			onBeginHighlightItemsByBlockInstanceGuidDelegate = OnBeginHighlightItemsByBlockInstanceGuid;
		}
		if (onEndHighlightItemsByBlockInstanceGuidDelegate == null)
		{
			onEndHighlightItemsByBlockInstanceGuidDelegate = OnEndHighlightItemsByBlockInstanceGuid;
		}
		if (onHighlightItemsByBlockInstanceGuidCompletedDelegate == null)
		{
			onHighlightItemsByBlockInstanceGuidCompletedDelegate = OnHighlightItemsByBlockInstanceGuidCompleted;
		}
		InvokeAsync(onBeginHighlightItemsByBlockInstanceGuidDelegate, new object[1] { instanceGuids }, onEndHighlightItemsByBlockInstanceGuidDelegate, onHighlightItemsByBlockInstanceGuidCompletedDelegate, userState);
	}

	public void ImportGroupAddRingRailGuide()
	{
		base.Channel.ImportGroupAddRingRailGuide();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginImportGroupAddRingRailGuide(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginImportGroupAddRingRailGuide(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndImportGroupAddRingRailGuide(IAsyncResult result)
	{
		base.Channel.EndImportGroupAddRingRailGuide(result);
	}

	private IAsyncResult OnBeginImportGroupAddRingRailGuide(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginImportGroupAddRingRailGuide(callback, asyncState);
	}

	private object[] OnEndImportGroupAddRingRailGuide(IAsyncResult result)
	{
		EndImportGroupAddRingRailGuide(result);
		return null;
	}

	private void OnImportGroupAddRingRailGuideCompleted(object state)
	{
		if (this.ImportGroupAddRingRailGuideCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ImportGroupAddRingRailGuideCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ImportGroupAddRingRailGuideAsync()
	{
		ImportGroupAddRingRailGuideAsync(null);
	}

	public void ImportGroupAddRingRailGuideAsync(object userState)
	{
		if (onBeginImportGroupAddRingRailGuideDelegate == null)
		{
			onBeginImportGroupAddRingRailGuideDelegate = OnBeginImportGroupAddRingRailGuide;
		}
		if (onEndImportGroupAddRingRailGuideDelegate == null)
		{
			onEndImportGroupAddRingRailGuideDelegate = OnEndImportGroupAddRingRailGuide;
		}
		if (onImportGroupAddRingRailGuideCompletedDelegate == null)
		{
			onImportGroupAddRingRailGuideCompletedDelegate = OnImportGroupAddRingRailGuideCompleted;
		}
		InvokeAsync(onBeginImportGroupAddRingRailGuideDelegate, null, onEndImportGroupAddRingRailGuideDelegate, onImportGroupAddRingRailGuideCompletedDelegate, userState);
	}

	public void ImportGroupAddToPartGroup(Guid[] importGroupIdentifiers)
	{
		base.Channel.ImportGroupAddToPartGroup(importGroupIdentifiers);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginImportGroupAddToPartGroup(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginImportGroupAddToPartGroup(importGroupIdentifiers, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndImportGroupAddToPartGroup(IAsyncResult result)
	{
		base.Channel.EndImportGroupAddToPartGroup(result);
	}

	private IAsyncResult OnBeginImportGroupAddToPartGroup(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiers = (Guid[])inValues[0];
		return BeginImportGroupAddToPartGroup(importGroupIdentifiers, callback, asyncState);
	}

	private object[] OnEndImportGroupAddToPartGroup(IAsyncResult result)
	{
		EndImportGroupAddToPartGroup(result);
		return null;
	}

	private void OnImportGroupAddToPartGroupCompleted(object state)
	{
		if (this.ImportGroupAddToPartGroupCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ImportGroupAddToPartGroupCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ImportGroupAddToPartGroupAsync(Guid[] importGroupIdentifiers)
	{
		ImportGroupAddToPartGroupAsync(importGroupIdentifiers, null);
	}

	public void ImportGroupAddToPartGroupAsync(Guid[] importGroupIdentifiers, object userState)
	{
		if (onBeginImportGroupAddToPartGroupDelegate == null)
		{
			onBeginImportGroupAddToPartGroupDelegate = OnBeginImportGroupAddToPartGroup;
		}
		if (onEndImportGroupAddToPartGroupDelegate == null)
		{
			onEndImportGroupAddToPartGroupDelegate = OnEndImportGroupAddToPartGroup;
		}
		if (onImportGroupAddToPartGroupCompletedDelegate == null)
		{
			onImportGroupAddToPartGroupCompletedDelegate = OnImportGroupAddToPartGroupCompleted;
		}
		InvokeAsync(onBeginImportGroupAddToPartGroupDelegate, new object[1] { importGroupIdentifiers }, onEndImportGroupAddToPartGroupDelegate, onImportGroupAddToPartGroupCompletedDelegate, userState);
	}

	public void ImportGroupBreakPartGroup(Guid importGroupIdentifier)
	{
		base.Channel.ImportGroupBreakPartGroup(importGroupIdentifier);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginImportGroupBreakPartGroup(Guid importGroupIdentifier, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginImportGroupBreakPartGroup(importGroupIdentifier, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndImportGroupBreakPartGroup(IAsyncResult result)
	{
		base.Channel.EndImportGroupBreakPartGroup(result);
	}

	private IAsyncResult OnBeginImportGroupBreakPartGroup(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid importGroupIdentifier = (Guid)inValues[0];
		return BeginImportGroupBreakPartGroup(importGroupIdentifier, callback, asyncState);
	}

	private object[] OnEndImportGroupBreakPartGroup(IAsyncResult result)
	{
		EndImportGroupBreakPartGroup(result);
		return null;
	}

	private void OnImportGroupBreakPartGroupCompleted(object state)
	{
		if (this.ImportGroupBreakPartGroupCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ImportGroupBreakPartGroupCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ImportGroupBreakPartGroupAsync(Guid importGroupIdentifier)
	{
		ImportGroupBreakPartGroupAsync(importGroupIdentifier, null);
	}

	public void ImportGroupBreakPartGroupAsync(Guid importGroupIdentifier, object userState)
	{
		if (onBeginImportGroupBreakPartGroupDelegate == null)
		{
			onBeginImportGroupBreakPartGroupDelegate = OnBeginImportGroupBreakPartGroup;
		}
		if (onEndImportGroupBreakPartGroupDelegate == null)
		{
			onEndImportGroupBreakPartGroupDelegate = OnEndImportGroupBreakPartGroup;
		}
		if (onImportGroupBreakPartGroupCompletedDelegate == null)
		{
			onImportGroupBreakPartGroupCompletedDelegate = OnImportGroupBreakPartGroupCompleted;
		}
		InvokeAsync(onBeginImportGroupBreakPartGroupDelegate, new object[1] { importGroupIdentifier }, onEndImportGroupBreakPartGroupDelegate, onImportGroupBreakPartGroupCompletedDelegate, userState);
	}

	public void ImportGroupRename(Guid importGroupIdentifier, string updatedName)
	{
		base.Channel.ImportGroupRename(importGroupIdentifier, updatedName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginImportGroupRename(Guid importGroupIdentifier, string updatedName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginImportGroupRename(importGroupIdentifier, updatedName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndImportGroupRename(IAsyncResult result)
	{
		base.Channel.EndImportGroupRename(result);
	}

	private IAsyncResult OnBeginImportGroupRename(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid importGroupIdentifier = (Guid)inValues[0];
		string updatedName = (string)inValues[1];
		return BeginImportGroupRename(importGroupIdentifier, updatedName, callback, asyncState);
	}

	private object[] OnEndImportGroupRename(IAsyncResult result)
	{
		EndImportGroupRename(result);
		return null;
	}

	private void OnImportGroupRenameCompleted(object state)
	{
		if (this.ImportGroupRenameCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ImportGroupRenameCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ImportGroupRenameAsync(Guid importGroupIdentifier, string updatedName)
	{
		ImportGroupRenameAsync(importGroupIdentifier, updatedName, null);
	}

	public void ImportGroupRenameAsync(Guid importGroupIdentifier, string updatedName, object userState)
	{
		if (onBeginImportGroupRenameDelegate == null)
		{
			onBeginImportGroupRenameDelegate = OnBeginImportGroupRename;
		}
		if (onEndImportGroupRenameDelegate == null)
		{
			onEndImportGroupRenameDelegate = OnEndImportGroupRename;
		}
		if (onImportGroupRenameCompletedDelegate == null)
		{
			onImportGroupRenameCompletedDelegate = OnImportGroupRenameCompleted;
		}
		InvokeAsync(onBeginImportGroupRenameDelegate, new object[2] { importGroupIdentifier, updatedName }, onEndImportGroupRenameDelegate, onImportGroupRenameCompletedDelegate, userState);
	}

	public void InitializeRhinoProperties(string csLibraryPath)
	{
		base.Channel.InitializeRhinoProperties(csLibraryPath);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginInitializeRhinoProperties(string csLibraryPath, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginInitializeRhinoProperties(csLibraryPath, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndInitializeRhinoProperties(IAsyncResult result)
	{
		base.Channel.EndInitializeRhinoProperties(result);
	}

	private IAsyncResult OnBeginInitializeRhinoProperties(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string csLibraryPath = (string)inValues[0];
		return BeginInitializeRhinoProperties(csLibraryPath, callback, asyncState);
	}

	private object[] OnEndInitializeRhinoProperties(IAsyncResult result)
	{
		EndInitializeRhinoProperties(result);
		return null;
	}

	private void OnInitializeRhinoPropertiesCompleted(object state)
	{
		if (this.InitializeRhinoPropertiesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.InitializeRhinoPropertiesCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void InitializeRhinoPropertiesAsync(string csLibraryPath)
	{
		InitializeRhinoPropertiesAsync(csLibraryPath, null);
	}

	public void InitializeRhinoPropertiesAsync(string csLibraryPath, object userState)
	{
		if (onBeginInitializeRhinoPropertiesDelegate == null)
		{
			onBeginInitializeRhinoPropertiesDelegate = OnBeginInitializeRhinoProperties;
		}
		if (onEndInitializeRhinoPropertiesDelegate == null)
		{
			onEndInitializeRhinoPropertiesDelegate = OnEndInitializeRhinoProperties;
		}
		if (onInitializeRhinoPropertiesCompletedDelegate == null)
		{
			onInitializeRhinoPropertiesCompletedDelegate = OnInitializeRhinoPropertiesCompleted;
		}
		InvokeAsync(onBeginInitializeRhinoPropertiesDelegate, new object[1] { csLibraryPath }, onEndInitializeRhinoPropertiesDelegate, onInitializeRhinoPropertiesCompletedDelegate, userState);
	}

	public UIControl[] InvokeAddAPartPatchwire(string path, string name)
	{
		return base.Channel.InvokeAddAPartPatchwire(path, name);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginInvokeAddAPartPatchwire(string path, string name, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginInvokeAddAPartPatchwire(path, name, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndInvokeAddAPartPatchwire(IAsyncResult result)
	{
		return base.Channel.EndInvokeAddAPartPatchwire(result);
	}

	private IAsyncResult OnBeginInvokeAddAPartPatchwire(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string path = (string)inValues[0];
		string name = (string)inValues[1];
		return BeginInvokeAddAPartPatchwire(path, name, callback, asyncState);
	}

	private object[] OnEndInvokeAddAPartPatchwire(IAsyncResult result)
	{
		UIControl[] array = EndInvokeAddAPartPatchwire(result);
		return new object[1] { array };
	}

	private void OnInvokeAddAPartPatchwireCompleted(object state)
	{
		if (this.InvokeAddAPartPatchwireCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.InvokeAddAPartPatchwireCompleted(this, new InvokeAddAPartPatchwireCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void InvokeAddAPartPatchwireAsync(string path, string name)
	{
		InvokeAddAPartPatchwireAsync(path, name, null);
	}

	public void InvokeAddAPartPatchwireAsync(string path, string name, object userState)
	{
		if (onBeginInvokeAddAPartPatchwireDelegate == null)
		{
			onBeginInvokeAddAPartPatchwireDelegate = OnBeginInvokeAddAPartPatchwire;
		}
		if (onEndInvokeAddAPartPatchwireDelegate == null)
		{
			onEndInvokeAddAPartPatchwireDelegate = OnEndInvokeAddAPartPatchwire;
		}
		if (onInvokeAddAPartPatchwireCompletedDelegate == null)
		{
			onInvokeAddAPartPatchwireCompletedDelegate = OnInvokeAddAPartPatchwireCompleted;
		}
		InvokeAsync(onBeginInvokeAddAPartPatchwireDelegate, new object[2] { path, name }, onEndInvokeAddAPartPatchwireDelegate, onInvokeAddAPartPatchwireCompletedDelegate, userState);
	}

	public UIControl[] InvokeGemPatchWire(GemActions action)
	{
		return base.Channel.InvokeGemPatchWire(action);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginInvokeGemPatchWire(GemActions action, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginInvokeGemPatchWire(action, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndInvokeGemPatchWire(IAsyncResult result)
	{
		return base.Channel.EndInvokeGemPatchWire(result);
	}

	private IAsyncResult OnBeginInvokeGemPatchWire(object[] inValues, AsyncCallback callback, object asyncState)
	{
		GemActions action = (GemActions)inValues[0];
		return BeginInvokeGemPatchWire(action, callback, asyncState);
	}

	private object[] OnEndInvokeGemPatchWire(IAsyncResult result)
	{
		UIControl[] array = EndInvokeGemPatchWire(result);
		return new object[1] { array };
	}

	private void OnInvokeGemPatchWireCompleted(object state)
	{
		if (this.InvokeGemPatchWireCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.InvokeGemPatchWireCompleted(this, new InvokeGemPatchWireCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void InvokeGemPatchWireAsync(GemActions action)
	{
		InvokeGemPatchWireAsync(action, null);
	}

	public void InvokeGemPatchWireAsync(GemActions action, object userState)
	{
		if (onBeginInvokeGemPatchWireDelegate == null)
		{
			onBeginInvokeGemPatchWireDelegate = OnBeginInvokeGemPatchWire;
		}
		if (onEndInvokeGemPatchWireDelegate == null)
		{
			onEndInvokeGemPatchWireDelegate = OnEndInvokeGemPatchWire;
		}
		if (onInvokeGemPatchWireCompletedDelegate == null)
		{
			onInvokeGemPatchWireCompletedDelegate = OnInvokeGemPatchWireCompleted;
		}
		InvokeAsync(onBeginInvokeGemPatchWireDelegate, new object[1] { action }, onEndInvokeGemPatchWireDelegate, onInvokeGemPatchWireCompletedDelegate, userState);
	}

	public bool IsComponentsEnumVisible(ComponentsEnum componentsEnum)
	{
		return base.Channel.IsComponentsEnumVisible(componentsEnum);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsComponentsEnumVisible(ComponentsEnum componentsEnum, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsComponentsEnumVisible(componentsEnum, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsComponentsEnumVisible(IAsyncResult result)
	{
		return base.Channel.EndIsComponentsEnumVisible(result);
	}

	private IAsyncResult OnBeginIsComponentsEnumVisible(object[] inValues, AsyncCallback callback, object asyncState)
	{
		ComponentsEnum componentsEnum = (ComponentsEnum)inValues[0];
		return BeginIsComponentsEnumVisible(componentsEnum, callback, asyncState);
	}

	private object[] OnEndIsComponentsEnumVisible(IAsyncResult result)
	{
		bool flag = EndIsComponentsEnumVisible(result);
		return new object[1] { flag };
	}

	private void OnIsComponentsEnumVisibleCompleted(object state)
	{
		if (this.IsComponentsEnumVisibleCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsComponentsEnumVisibleCompleted(this, new IsComponentsEnumVisibleCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsComponentsEnumVisibleAsync(ComponentsEnum componentsEnum)
	{
		IsComponentsEnumVisibleAsync(componentsEnum, null);
	}

	public void IsComponentsEnumVisibleAsync(ComponentsEnum componentsEnum, object userState)
	{
		if (onBeginIsComponentsEnumVisibleDelegate == null)
		{
			onBeginIsComponentsEnumVisibleDelegate = OnBeginIsComponentsEnumVisible;
		}
		if (onEndIsComponentsEnumVisibleDelegate == null)
		{
			onEndIsComponentsEnumVisibleDelegate = OnEndIsComponentsEnumVisible;
		}
		if (onIsComponentsEnumVisibleCompletedDelegate == null)
		{
			onIsComponentsEnumVisibleCompletedDelegate = OnIsComponentsEnumVisibleCompleted;
		}
		InvokeAsync(onBeginIsComponentsEnumVisibleDelegate, new object[1] { componentsEnum }, onEndIsComponentsEnumVisibleDelegate, onIsComponentsEnumVisibleCompletedDelegate, userState);
	}

	public bool IsDisplayConduitEnabled()
	{
		return base.Channel.IsDisplayConduitEnabled();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsDisplayConduitEnabled(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsDisplayConduitEnabled(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsDisplayConduitEnabled(IAsyncResult result)
	{
		return base.Channel.EndIsDisplayConduitEnabled(result);
	}

	private IAsyncResult OnBeginIsDisplayConduitEnabled(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginIsDisplayConduitEnabled(callback, asyncState);
	}

	private object[] OnEndIsDisplayConduitEnabled(IAsyncResult result)
	{
		bool flag = EndIsDisplayConduitEnabled(result);
		return new object[1] { flag };
	}

	private void OnIsDisplayConduitEnabledCompleted(object state)
	{
		if (this.IsDisplayConduitEnabledCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsDisplayConduitEnabledCompleted(this, new IsDisplayConduitEnabledCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsDisplayConduitEnabledAsync()
	{
		IsDisplayConduitEnabledAsync(null);
	}

	public void IsDisplayConduitEnabledAsync(object userState)
	{
		if (onBeginIsDisplayConduitEnabledDelegate == null)
		{
			onBeginIsDisplayConduitEnabledDelegate = OnBeginIsDisplayConduitEnabled;
		}
		if (onEndIsDisplayConduitEnabledDelegate == null)
		{
			onEndIsDisplayConduitEnabledDelegate = OnEndIsDisplayConduitEnabled;
		}
		if (onIsDisplayConduitEnabledCompletedDelegate == null)
		{
			onIsDisplayConduitEnabledCompletedDelegate = OnIsDisplayConduitEnabledCompleted;
		}
		InvokeAsync(onBeginIsDisplayConduitEnabledDelegate, null, onEndIsDisplayConduitEnabledDelegate, onIsDisplayConduitEnabledCompletedDelegate, userState);
	}

	public bool IsDocumentCanvasEmpty()
	{
		return base.Channel.IsDocumentCanvasEmpty();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsDocumentCanvasEmpty(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsDocumentCanvasEmpty(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsDocumentCanvasEmpty(IAsyncResult result)
	{
		return base.Channel.EndIsDocumentCanvasEmpty(result);
	}

	private IAsyncResult OnBeginIsDocumentCanvasEmpty(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginIsDocumentCanvasEmpty(callback, asyncState);
	}

	private object[] OnEndIsDocumentCanvasEmpty(IAsyncResult result)
	{
		bool flag = EndIsDocumentCanvasEmpty(result);
		return new object[1] { flag };
	}

	private void OnIsDocumentCanvasEmptyCompleted(object state)
	{
		if (this.IsDocumentCanvasEmptyCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsDocumentCanvasEmptyCompleted(this, new IsDocumentCanvasEmptyCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsDocumentCanvasEmptyAsync()
	{
		IsDocumentCanvasEmptyAsync(null);
	}

	public void IsDocumentCanvasEmptyAsync(object userState)
	{
		if (onBeginIsDocumentCanvasEmptyDelegate == null)
		{
			onBeginIsDocumentCanvasEmptyDelegate = OnBeginIsDocumentCanvasEmpty;
		}
		if (onEndIsDocumentCanvasEmptyDelegate == null)
		{
			onEndIsDocumentCanvasEmptyDelegate = OnEndIsDocumentCanvasEmpty;
		}
		if (onIsDocumentCanvasEmptyCompletedDelegate == null)
		{
			onIsDocumentCanvasEmptyCompletedDelegate = OnIsDocumentCanvasEmptyCompleted;
		}
		InvokeAsync(onBeginIsDocumentCanvasEmptyDelegate, null, onEndIsDocumentCanvasEmptyDelegate, onIsDocumentCanvasEmptyCompletedDelegate, userState);
	}

	public bool IsManualSizingModeOn(bool excludeCenterStone)
	{
		return base.Channel.IsManualSizingModeOn(excludeCenterStone);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsManualSizingModeOn(bool excludeCenterStone, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsManualSizingModeOn(excludeCenterStone, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsManualSizingModeOn(IAsyncResult result)
	{
		return base.Channel.EndIsManualSizingModeOn(result);
	}

	private IAsyncResult OnBeginIsManualSizingModeOn(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool excludeCenterStone = (bool)inValues[0];
		return BeginIsManualSizingModeOn(excludeCenterStone, callback, asyncState);
	}

	private object[] OnEndIsManualSizingModeOn(IAsyncResult result)
	{
		bool flag = EndIsManualSizingModeOn(result);
		return new object[1] { flag };
	}

	private void OnIsManualSizingModeOnCompleted(object state)
	{
		if (this.IsManualSizingModeOnCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsManualSizingModeOnCompleted(this, new IsManualSizingModeOnCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsManualSizingModeOnAsync(bool excludeCenterStone)
	{
		IsManualSizingModeOnAsync(excludeCenterStone, null);
	}

	public void IsManualSizingModeOnAsync(bool excludeCenterStone, object userState)
	{
		if (onBeginIsManualSizingModeOnDelegate == null)
		{
			onBeginIsManualSizingModeOnDelegate = OnBeginIsManualSizingModeOn;
		}
		if (onEndIsManualSizingModeOnDelegate == null)
		{
			onEndIsManualSizingModeOnDelegate = OnEndIsManualSizingModeOn;
		}
		if (onIsManualSizingModeOnCompletedDelegate == null)
		{
			onIsManualSizingModeOnCompletedDelegate = OnIsManualSizingModeOnCompleted;
		}
		InvokeAsync(onBeginIsManualSizingModeOnDelegate, new object[1] { excludeCenterStone }, onEndIsManualSizingModeOnDelegate, onIsManualSizingModeOnCompletedDelegate, userState);
	}

	public bool IsProjetDialogLoaded()
	{
		return base.Channel.IsProjetDialogLoaded();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsProjetDialogLoaded(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsProjetDialogLoaded(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsProjetDialogLoaded(IAsyncResult result)
	{
		return base.Channel.EndIsProjetDialogLoaded(result);
	}

	private IAsyncResult OnBeginIsProjetDialogLoaded(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginIsProjetDialogLoaded(callback, asyncState);
	}

	private object[] OnEndIsProjetDialogLoaded(IAsyncResult result)
	{
		bool flag = EndIsProjetDialogLoaded(result);
		return new object[1] { flag };
	}

	private void OnIsProjetDialogLoadedCompleted(object state)
	{
		if (this.IsProjetDialogLoadedCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsProjetDialogLoadedCompleted(this, new IsProjetDialogLoadedCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsProjetDialogLoadedAsync()
	{
		IsProjetDialogLoadedAsync(null);
	}

	public void IsProjetDialogLoadedAsync(object userState)
	{
		if (onBeginIsProjetDialogLoadedDelegate == null)
		{
			onBeginIsProjetDialogLoadedDelegate = OnBeginIsProjetDialogLoaded;
		}
		if (onEndIsProjetDialogLoadedDelegate == null)
		{
			onEndIsProjetDialogLoadedDelegate = OnEndIsProjetDialogLoaded;
		}
		if (onIsProjetDialogLoadedCompletedDelegate == null)
		{
			onIsProjetDialogLoadedCompletedDelegate = OnIsProjetDialogLoadedCompleted;
		}
		InvokeAsync(onBeginIsProjetDialogLoadedDelegate, null, onEndIsProjetDialogLoadedDelegate, onIsProjetDialogLoadedCompletedDelegate, userState);
	}

	public bool IsRhino64Bit()
	{
		return base.Channel.IsRhino64Bit();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsRhino64Bit(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsRhino64Bit(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsRhino64Bit(IAsyncResult result)
	{
		return base.Channel.EndIsRhino64Bit(result);
	}

	private IAsyncResult OnBeginIsRhino64Bit(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginIsRhino64Bit(callback, asyncState);
	}

	private object[] OnEndIsRhino64Bit(IAsyncResult result)
	{
		bool flag = EndIsRhino64Bit(result);
		return new object[1] { flag };
	}

	private void OnIsRhino64BitCompleted(object state)
	{
		if (this.IsRhino64BitCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsRhino64BitCompleted(this, new IsRhino64BitCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsRhino64BitAsync()
	{
		IsRhino64BitAsync(null);
	}

	public void IsRhino64BitAsync(object userState)
	{
		if (onBeginIsRhino64BitDelegate == null)
		{
			onBeginIsRhino64BitDelegate = OnBeginIsRhino64Bit;
		}
		if (onEndIsRhino64BitDelegate == null)
		{
			onEndIsRhino64BitDelegate = OnEndIsRhino64Bit;
		}
		if (onIsRhino64BitCompletedDelegate == null)
		{
			onIsRhino64BitCompletedDelegate = OnIsRhino64BitCompleted;
		}
		InvokeAsync(onBeginIsRhino64BitDelegate, null, onEndIsRhino64BitDelegate, onIsRhino64BitCompletedDelegate, userState);
	}

	public bool IsUsbKeyBeingUsed()
	{
		return base.Channel.IsUsbKeyBeingUsed();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginIsUsbKeyBeingUsed(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginIsUsbKeyBeingUsed(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndIsUsbKeyBeingUsed(IAsyncResult result)
	{
		return base.Channel.EndIsUsbKeyBeingUsed(result);
	}

	private IAsyncResult OnBeginIsUsbKeyBeingUsed(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginIsUsbKeyBeingUsed(callback, asyncState);
	}

	private object[] OnEndIsUsbKeyBeingUsed(IAsyncResult result)
	{
		bool flag = EndIsUsbKeyBeingUsed(result);
		return new object[1] { flag };
	}

	private void OnIsUsbKeyBeingUsedCompleted(object state)
	{
		if (this.IsUsbKeyBeingUsedCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.IsUsbKeyBeingUsedCompleted(this, new IsUsbKeyBeingUsedCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void IsUsbKeyBeingUsedAsync()
	{
		IsUsbKeyBeingUsedAsync(null);
	}

	public void IsUsbKeyBeingUsedAsync(object userState)
	{
		if (onBeginIsUsbKeyBeingUsedDelegate == null)
		{
			onBeginIsUsbKeyBeingUsedDelegate = OnBeginIsUsbKeyBeingUsed;
		}
		if (onEndIsUsbKeyBeingUsedDelegate == null)
		{
			onEndIsUsbKeyBeingUsedDelegate = OnEndIsUsbKeyBeingUsed;
		}
		if (onIsUsbKeyBeingUsedCompletedDelegate == null)
		{
			onIsUsbKeyBeingUsedCompletedDelegate = OnIsUsbKeyBeingUsedCompleted;
		}
		InvokeAsync(onBeginIsUsbKeyBeingUsedDelegate, null, onEndIsUsbKeyBeingUsedDelegate, onIsUsbKeyBeingUsedCompletedDelegate, userState);
	}

	public BoundingBox NurbsGetBoundingBox(NurbsCurve nc)
	{
		return base.Channel.NurbsGetBoundingBox(nc);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginNurbsGetBoundingBox(NurbsCurve nc, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginNurbsGetBoundingBox(nc, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public BoundingBox EndNurbsGetBoundingBox(IAsyncResult result)
	{
		return base.Channel.EndNurbsGetBoundingBox(result);
	}

	private IAsyncResult OnBeginNurbsGetBoundingBox(object[] inValues, AsyncCallback callback, object asyncState)
	{
		NurbsCurve nc = (NurbsCurve)inValues[0];
		return BeginNurbsGetBoundingBox(nc, callback, asyncState);
	}

	private object[] OnEndNurbsGetBoundingBox(IAsyncResult result)
	{
		BoundingBox boundingBox = EndNurbsGetBoundingBox(result);
		return new object[1] { boundingBox };
	}

	private void OnNurbsGetBoundingBoxCompleted(object state)
	{
		if (this.NurbsGetBoundingBoxCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.NurbsGetBoundingBoxCompleted(this, new NurbsGetBoundingBoxCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void NurbsGetBoundingBoxAsync(NurbsCurve nc)
	{
		NurbsGetBoundingBoxAsync(nc, null);
	}

	public void NurbsGetBoundingBoxAsync(NurbsCurve nc, object userState)
	{
		if (onBeginNurbsGetBoundingBoxDelegate == null)
		{
			onBeginNurbsGetBoundingBoxDelegate = OnBeginNurbsGetBoundingBox;
		}
		if (onEndNurbsGetBoundingBoxDelegate == null)
		{
			onEndNurbsGetBoundingBoxDelegate = OnEndNurbsGetBoundingBox;
		}
		if (onNurbsGetBoundingBoxCompletedDelegate == null)
		{
			onNurbsGetBoundingBoxCompletedDelegate = OnNurbsGetBoundingBoxCompleted;
		}
		InvokeAsync(onBeginNurbsGetBoundingBoxDelegate, new object[1] { nc }, onEndNurbsGetBoundingBoxDelegate, onNurbsGetBoundingBoxCompletedDelegate, userState);
	}

	public double NurbsGetClosestPoint(NurbsCurve nc, Point3D testHit)
	{
		return base.Channel.NurbsGetClosestPoint(nc, testHit);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginNurbsGetClosestPoint(NurbsCurve nc, Point3D testHit, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginNurbsGetClosestPoint(nc, testHit, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public double EndNurbsGetClosestPoint(IAsyncResult result)
	{
		return base.Channel.EndNurbsGetClosestPoint(result);
	}

	private IAsyncResult OnBeginNurbsGetClosestPoint(object[] inValues, AsyncCallback callback, object asyncState)
	{
		NurbsCurve nc = (NurbsCurve)inValues[0];
		Point3D testHit = (Point3D)inValues[1];
		return BeginNurbsGetClosestPoint(nc, testHit, callback, asyncState);
	}

	private object[] OnEndNurbsGetClosestPoint(IAsyncResult result)
	{
		double num = EndNurbsGetClosestPoint(result);
		return new object[1] { num };
	}

	private void OnNurbsGetClosestPointCompleted(object state)
	{
		if (this.NurbsGetClosestPointCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.NurbsGetClosestPointCompleted(this, new NurbsGetClosestPointCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void NurbsGetClosestPointAsync(NurbsCurve nc, Point3D testHit)
	{
		NurbsGetClosestPointAsync(nc, testHit, null);
	}

	public void NurbsGetClosestPointAsync(NurbsCurve nc, Point3D testHit, object userState)
	{
		if (onBeginNurbsGetClosestPointDelegate == null)
		{
			onBeginNurbsGetClosestPointDelegate = OnBeginNurbsGetClosestPoint;
		}
		if (onEndNurbsGetClosestPointDelegate == null)
		{
			onEndNurbsGetClosestPointDelegate = OnEndNurbsGetClosestPoint;
		}
		if (onNurbsGetClosestPointCompletedDelegate == null)
		{
			onNurbsGetClosestPointCompletedDelegate = OnNurbsGetClosestPointCompleted;
		}
		InvokeAsync(onBeginNurbsGetClosestPointDelegate, new object[2] { nc, testHit }, onEndNurbsGetClosestPointDelegate, onNurbsGetClosestPointCompletedDelegate, userState);
	}

	public Point3D NurbsGetNormalizedPointAt(NurbsCurve nc, double editPointParam)
	{
		return base.Channel.NurbsGetNormalizedPointAt(nc, editPointParam);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginNurbsGetNormalizedPointAt(NurbsCurve nc, double editPointParam, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginNurbsGetNormalizedPointAt(nc, editPointParam, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Point3D EndNurbsGetNormalizedPointAt(IAsyncResult result)
	{
		return base.Channel.EndNurbsGetNormalizedPointAt(result);
	}

	private IAsyncResult OnBeginNurbsGetNormalizedPointAt(object[] inValues, AsyncCallback callback, object asyncState)
	{
		NurbsCurve nc = (NurbsCurve)inValues[0];
		double editPointParam = (double)inValues[1];
		return BeginNurbsGetNormalizedPointAt(nc, editPointParam, callback, asyncState);
	}

	private object[] OnEndNurbsGetNormalizedPointAt(IAsyncResult result)
	{
		Point3D point3D = EndNurbsGetNormalizedPointAt(result);
		return new object[1] { point3D };
	}

	private void OnNurbsGetNormalizedPointAtCompleted(object state)
	{
		if (this.NurbsGetNormalizedPointAtCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.NurbsGetNormalizedPointAtCompleted(this, new NurbsGetNormalizedPointAtCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void NurbsGetNormalizedPointAtAsync(NurbsCurve nc, double editPointParam)
	{
		NurbsGetNormalizedPointAtAsync(nc, editPointParam, null);
	}

	public void NurbsGetNormalizedPointAtAsync(NurbsCurve nc, double editPointParam, object userState)
	{
		if (onBeginNurbsGetNormalizedPointAtDelegate == null)
		{
			onBeginNurbsGetNormalizedPointAtDelegate = OnBeginNurbsGetNormalizedPointAt;
		}
		if (onEndNurbsGetNormalizedPointAtDelegate == null)
		{
			onEndNurbsGetNormalizedPointAtDelegate = OnEndNurbsGetNormalizedPointAt;
		}
		if (onNurbsGetNormalizedPointAtCompletedDelegate == null)
		{
			onNurbsGetNormalizedPointAtCompletedDelegate = OnNurbsGetNormalizedPointAtCompleted;
		}
		InvokeAsync(onBeginNurbsGetNormalizedPointAtDelegate, new object[2] { nc, editPointParam }, onEndNurbsGetNormalizedPointAtDelegate, onNurbsGetNormalizedPointAtCompletedDelegate, userState);
	}

	public Point2D[] NurbsGetPolyline(NurbsCurve nc)
	{
		return base.Channel.NurbsGetPolyline(nc);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginNurbsGetPolyline(NurbsCurve nc, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginNurbsGetPolyline(nc, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Point2D[] EndNurbsGetPolyline(IAsyncResult result)
	{
		return base.Channel.EndNurbsGetPolyline(result);
	}

	private IAsyncResult OnBeginNurbsGetPolyline(object[] inValues, AsyncCallback callback, object asyncState)
	{
		NurbsCurve nc = (NurbsCurve)inValues[0];
		return BeginNurbsGetPolyline(nc, callback, asyncState);
	}

	private object[] OnEndNurbsGetPolyline(IAsyncResult result)
	{
		Point2D[] array = EndNurbsGetPolyline(result);
		return new object[1] { array };
	}

	private void OnNurbsGetPolylineCompleted(object state)
	{
		if (this.NurbsGetPolylineCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.NurbsGetPolylineCompleted(this, new NurbsGetPolylineCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void NurbsGetPolylineAsync(NurbsCurve nc)
	{
		NurbsGetPolylineAsync(nc, null);
	}

	public void NurbsGetPolylineAsync(NurbsCurve nc, object userState)
	{
		if (onBeginNurbsGetPolylineDelegate == null)
		{
			onBeginNurbsGetPolylineDelegate = OnBeginNurbsGetPolyline;
		}
		if (onEndNurbsGetPolylineDelegate == null)
		{
			onEndNurbsGetPolylineDelegate = OnEndNurbsGetPolyline;
		}
		if (onNurbsGetPolylineCompletedDelegate == null)
		{
			onNurbsGetPolylineCompletedDelegate = OnNurbsGetPolylineCompleted;
		}
		InvokeAsync(onBeginNurbsGetPolylineDelegate, new object[1] { nc }, onEndNurbsGetPolylineDelegate, onNurbsGetPolylineCompletedDelegate, userState);
	}

	public NurbsCurve NurbsRebuild(NurbsCurve nc, int i, int i1)
	{
		return base.Channel.NurbsRebuild(nc, i, i1);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginNurbsRebuild(NurbsCurve nc, int i, int i1, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginNurbsRebuild(nc, i, i1, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public NurbsCurve EndNurbsRebuild(IAsyncResult result)
	{
		return base.Channel.EndNurbsRebuild(result);
	}

	private IAsyncResult OnBeginNurbsRebuild(object[] inValues, AsyncCallback callback, object asyncState)
	{
		NurbsCurve nc = (NurbsCurve)inValues[0];
		int i = (int)inValues[1];
		int i2 = (int)inValues[2];
		return BeginNurbsRebuild(nc, i, i2, callback, asyncState);
	}

	private object[] OnEndNurbsRebuild(IAsyncResult result)
	{
		NurbsCurve nurbsCurve = EndNurbsRebuild(result);
		return new object[1] { nurbsCurve };
	}

	private void OnNurbsRebuildCompleted(object state)
	{
		if (this.NurbsRebuildCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.NurbsRebuildCompleted(this, new NurbsRebuildCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void NurbsRebuildAsync(NurbsCurve nc, int i, int i1)
	{
		NurbsRebuildAsync(nc, i, i1, null);
	}

	public void NurbsRebuildAsync(NurbsCurve nc, int i, int i1, object userState)
	{
		if (onBeginNurbsRebuildDelegate == null)
		{
			onBeginNurbsRebuildDelegate = OnBeginNurbsRebuild;
		}
		if (onEndNurbsRebuildDelegate == null)
		{
			onEndNurbsRebuildDelegate = OnEndNurbsRebuild;
		}
		if (onNurbsRebuildCompletedDelegate == null)
		{
			onNurbsRebuildCompletedDelegate = OnNurbsRebuildCompleted;
		}
		InvokeAsync(onBeginNurbsRebuildDelegate, new object[3] { nc, i, i1 }, onEndNurbsRebuildDelegate, onNurbsRebuildCompletedDelegate, userState);
	}

	public NurbsCurve NurbsSetDomain(NurbsCurve nc, int i, int i1)
	{
		return base.Channel.NurbsSetDomain(nc, i, i1);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginNurbsSetDomain(NurbsCurve nc, int i, int i1, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginNurbsSetDomain(nc, i, i1, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public NurbsCurve EndNurbsSetDomain(IAsyncResult result)
	{
		return base.Channel.EndNurbsSetDomain(result);
	}

	private IAsyncResult OnBeginNurbsSetDomain(object[] inValues, AsyncCallback callback, object asyncState)
	{
		NurbsCurve nc = (NurbsCurve)inValues[0];
		int i = (int)inValues[1];
		int i2 = (int)inValues[2];
		return BeginNurbsSetDomain(nc, i, i2, callback, asyncState);
	}

	private object[] OnEndNurbsSetDomain(IAsyncResult result)
	{
		NurbsCurve nurbsCurve = EndNurbsSetDomain(result);
		return new object[1] { nurbsCurve };
	}

	private void OnNurbsSetDomainCompleted(object state)
	{
		if (this.NurbsSetDomainCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.NurbsSetDomainCompleted(this, new NurbsSetDomainCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void NurbsSetDomainAsync(NurbsCurve nc, int i, int i1)
	{
		NurbsSetDomainAsync(nc, i, i1, null);
	}

	public void NurbsSetDomainAsync(NurbsCurve nc, int i, int i1, object userState)
	{
		if (onBeginNurbsSetDomainDelegate == null)
		{
			onBeginNurbsSetDomainDelegate = OnBeginNurbsSetDomain;
		}
		if (onEndNurbsSetDomainDelegate == null)
		{
			onEndNurbsSetDomainDelegate = OnEndNurbsSetDomain;
		}
		if (onNurbsSetDomainCompletedDelegate == null)
		{
			onNurbsSetDomainCompletedDelegate = OnNurbsSetDomainCompleted;
		}
		InvokeAsync(onBeginNurbsSetDomainDelegate, new object[3] { nc, i, i1 }, onEndNurbsSetDomainDelegate, onNurbsSetDomainCompletedDelegate, userState);
	}

	public UIControl[] OpenDocument(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid, int? centerGemGraphIndex)
	{
		return base.Channel.OpenDocument(pathToFile, isQuarterCaratLargeStoneRoundingEnabled, largeStoneCaratMax, centerGemControlGuid, centerGemGraphIndex);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOpenDocument(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid, int? centerGemGraphIndex, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOpenDocument(pathToFile, isQuarterCaratLargeStoneRoundingEnabled, largeStoneCaratMax, centerGemControlGuid, centerGemGraphIndex, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndOpenDocument(IAsyncResult result)
	{
		return base.Channel.EndOpenDocument(result);
	}

	private IAsyncResult OnBeginOpenDocument(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string pathToFile = (string)inValues[0];
		bool isQuarterCaratLargeStoneRoundingEnabled = (bool)inValues[1];
		double largeStoneCaratMax = (double)inValues[2];
		Guid centerGemControlGuid = (Guid)inValues[3];
		int? centerGemGraphIndex = (int?)inValues[4];
		return BeginOpenDocument(pathToFile, isQuarterCaratLargeStoneRoundingEnabled, largeStoneCaratMax, centerGemControlGuid, centerGemGraphIndex, callback, asyncState);
	}

	private object[] OnEndOpenDocument(IAsyncResult result)
	{
		UIControl[] array = EndOpenDocument(result);
		return new object[1] { array };
	}

	private void OnOpenDocumentCompleted(object state)
	{
		if (this.OpenDocumentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OpenDocumentCompleted(this, new OpenDocumentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OpenDocumentAsync(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid, int? centerGemGraphIndex)
	{
		OpenDocumentAsync(pathToFile, isQuarterCaratLargeStoneRoundingEnabled, largeStoneCaratMax, centerGemControlGuid, centerGemGraphIndex, null);
	}

	public void OpenDocumentAsync(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid, int? centerGemGraphIndex, object userState)
	{
		if (onBeginOpenDocumentDelegate == null)
		{
			onBeginOpenDocumentDelegate = OnBeginOpenDocument;
		}
		if (onEndOpenDocumentDelegate == null)
		{
			onEndOpenDocumentDelegate = OnEndOpenDocument;
		}
		if (onOpenDocumentCompletedDelegate == null)
		{
			onOpenDocumentCompletedDelegate = OnOpenDocumentCompleted;
		}
		InvokeAsync(onBeginOpenDocumentDelegate, new object[5] { pathToFile, isQuarterCaratLargeStoneRoundingEnabled, largeStoneCaratMax, centerGemControlGuid, centerGemGraphIndex }, onEndOpenDocumentDelegate, onOpenDocumentCompletedDelegate, userState);
	}

	public bool OrderCheckNonCenterGemsExist()
	{
		return base.Channel.OrderCheckNonCenterGemsExist();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderCheckNonCenterGemsExist(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderCheckNonCenterGemsExist(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndOrderCheckNonCenterGemsExist(IAsyncResult result)
	{
		return base.Channel.EndOrderCheckNonCenterGemsExist(result);
	}

	private IAsyncResult OnBeginOrderCheckNonCenterGemsExist(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginOrderCheckNonCenterGemsExist(callback, asyncState);
	}

	private object[] OnEndOrderCheckNonCenterGemsExist(IAsyncResult result)
	{
		bool flag = EndOrderCheckNonCenterGemsExist(result);
		return new object[1] { flag };
	}

	private void OnOrderCheckNonCenterGemsExistCompleted(object state)
	{
		if (this.OrderCheckNonCenterGemsExistCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderCheckNonCenterGemsExistCompleted(this, new OrderCheckNonCenterGemsExistCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderCheckNonCenterGemsExistAsync()
	{
		OrderCheckNonCenterGemsExistAsync(null);
	}

	public void OrderCheckNonCenterGemsExistAsync(object userState)
	{
		if (onBeginOrderCheckNonCenterGemsExistDelegate == null)
		{
			onBeginOrderCheckNonCenterGemsExistDelegate = OnBeginOrderCheckNonCenterGemsExist;
		}
		if (onEndOrderCheckNonCenterGemsExistDelegate == null)
		{
			onEndOrderCheckNonCenterGemsExistDelegate = OnEndOrderCheckNonCenterGemsExist;
		}
		if (onOrderCheckNonCenterGemsExistCompletedDelegate == null)
		{
			onOrderCheckNonCenterGemsExistCompletedDelegate = OnOrderCheckNonCenterGemsExistCompleted;
		}
		InvokeAsync(onBeginOrderCheckNonCenterGemsExistDelegate, null, onEndOrderCheckNonCenterGemsExistDelegate, onOrderCheckNonCenterGemsExistCompletedDelegate, userState);
	}

	public OrderGemData[] OrderGetAccentGemData()
	{
		return base.Channel.OrderGetAccentGemData();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderGetAccentGemData(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderGetAccentGemData(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public OrderGemData[] EndOrderGetAccentGemData(IAsyncResult result)
	{
		return base.Channel.EndOrderGetAccentGemData(result);
	}

	private IAsyncResult OnBeginOrderGetAccentGemData(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginOrderGetAccentGemData(callback, asyncState);
	}

	private object[] OnEndOrderGetAccentGemData(IAsyncResult result)
	{
		OrderGemData[] array = EndOrderGetAccentGemData(result);
		return new object[1] { array };
	}

	private void OnOrderGetAccentGemDataCompleted(object state)
	{
		if (this.OrderGetAccentGemDataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderGetAccentGemDataCompleted(this, new OrderGetAccentGemDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderGetAccentGemDataAsync()
	{
		OrderGetAccentGemDataAsync(null);
	}

	public void OrderGetAccentGemDataAsync(object userState)
	{
		if (onBeginOrderGetAccentGemDataDelegate == null)
		{
			onBeginOrderGetAccentGemDataDelegate = OnBeginOrderGetAccentGemData;
		}
		if (onEndOrderGetAccentGemDataDelegate == null)
		{
			onEndOrderGetAccentGemDataDelegate = OnEndOrderGetAccentGemData;
		}
		if (onOrderGetAccentGemDataCompletedDelegate == null)
		{
			onOrderGetAccentGemDataCompletedDelegate = OnOrderGetAccentGemDataCompleted;
		}
		InvokeAsync(onBeginOrderGetAccentGemDataDelegate, null, onEndOrderGetAccentGemDataDelegate, onOrderGetAccentGemDataCompletedDelegate, userState);
	}

	public OrderGemData[] OrderGetAllGemData()
	{
		return base.Channel.OrderGetAllGemData();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderGetAllGemData(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderGetAllGemData(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public OrderGemData[] EndOrderGetAllGemData(IAsyncResult result)
	{
		return base.Channel.EndOrderGetAllGemData(result);
	}

	private IAsyncResult OnBeginOrderGetAllGemData(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginOrderGetAllGemData(callback, asyncState);
	}

	private object[] OnEndOrderGetAllGemData(IAsyncResult result)
	{
		OrderGemData[] array = EndOrderGetAllGemData(result);
		return new object[1] { array };
	}

	private void OnOrderGetAllGemDataCompleted(object state)
	{
		if (this.OrderGetAllGemDataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderGetAllGemDataCompleted(this, new OrderGetAllGemDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderGetAllGemDataAsync()
	{
		OrderGetAllGemDataAsync(null);
	}

	public void OrderGetAllGemDataAsync(object userState)
	{
		if (onBeginOrderGetAllGemDataDelegate == null)
		{
			onBeginOrderGetAllGemDataDelegate = OnBeginOrderGetAllGemData;
		}
		if (onEndOrderGetAllGemDataDelegate == null)
		{
			onEndOrderGetAllGemDataDelegate = OnEndOrderGetAllGemData;
		}
		if (onOrderGetAllGemDataCompletedDelegate == null)
		{
			onOrderGetAllGemDataCompletedDelegate = OnOrderGetAllGemDataCompleted;
		}
		InvokeAsync(onBeginOrderGetAllGemDataDelegate, null, onEndOrderGetAllGemDataDelegate, onOrderGetAllGemDataCompletedDelegate, userState);
	}

	public string[] OrderGetCurrentRingSizes()
	{
		return base.Channel.OrderGetCurrentRingSizes();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderGetCurrentRingSizes(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderGetCurrentRingSizes(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string[] EndOrderGetCurrentRingSizes(IAsyncResult result)
	{
		return base.Channel.EndOrderGetCurrentRingSizes(result);
	}

	private IAsyncResult OnBeginOrderGetCurrentRingSizes(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginOrderGetCurrentRingSizes(callback, asyncState);
	}

	private object[] OnEndOrderGetCurrentRingSizes(IAsyncResult result)
	{
		string[] array = EndOrderGetCurrentRingSizes(result);
		return new object[1] { array };
	}

	private void OnOrderGetCurrentRingSizesCompleted(object state)
	{
		if (this.OrderGetCurrentRingSizesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderGetCurrentRingSizesCompleted(this, new OrderGetCurrentRingSizesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderGetCurrentRingSizesAsync()
	{
		OrderGetCurrentRingSizesAsync(null);
	}

	public void OrderGetCurrentRingSizesAsync(object userState)
	{
		if (onBeginOrderGetCurrentRingSizesDelegate == null)
		{
			onBeginOrderGetCurrentRingSizesDelegate = OnBeginOrderGetCurrentRingSizes;
		}
		if (onEndOrderGetCurrentRingSizesDelegate == null)
		{
			onEndOrderGetCurrentRingSizesDelegate = OnEndOrderGetCurrentRingSizes;
		}
		if (onOrderGetCurrentRingSizesCompletedDelegate == null)
		{
			onOrderGetCurrentRingSizesCompletedDelegate = OnOrderGetCurrentRingSizesCompleted;
		}
		InvokeAsync(onBeginOrderGetCurrentRingSizesDelegate, null, onEndOrderGetCurrentRingSizesDelegate, onOrderGetCurrentRingSizesCompletedDelegate, userState);
	}

	public double[] OrderGetCurrentRingSizesDiameter()
	{
		return base.Channel.OrderGetCurrentRingSizesDiameter();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderGetCurrentRingSizesDiameter(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderGetCurrentRingSizesDiameter(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public double[] EndOrderGetCurrentRingSizesDiameter(IAsyncResult result)
	{
		return base.Channel.EndOrderGetCurrentRingSizesDiameter(result);
	}

	private IAsyncResult OnBeginOrderGetCurrentRingSizesDiameter(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginOrderGetCurrentRingSizesDiameter(callback, asyncState);
	}

	private object[] OnEndOrderGetCurrentRingSizesDiameter(IAsyncResult result)
	{
		double[] array = EndOrderGetCurrentRingSizesDiameter(result);
		return new object[1] { array };
	}

	private void OnOrderGetCurrentRingSizesDiameterCompleted(object state)
	{
		if (this.OrderGetCurrentRingSizesDiameterCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderGetCurrentRingSizesDiameterCompleted(this, new OrderGetCurrentRingSizesDiameterCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderGetCurrentRingSizesDiameterAsync()
	{
		OrderGetCurrentRingSizesDiameterAsync(null);
	}

	public void OrderGetCurrentRingSizesDiameterAsync(object userState)
	{
		if (onBeginOrderGetCurrentRingSizesDiameterDelegate == null)
		{
			onBeginOrderGetCurrentRingSizesDiameterDelegate = OnBeginOrderGetCurrentRingSizesDiameter;
		}
		if (onEndOrderGetCurrentRingSizesDiameterDelegate == null)
		{
			onEndOrderGetCurrentRingSizesDiameterDelegate = OnEndOrderGetCurrentRingSizesDiameter;
		}
		if (onOrderGetCurrentRingSizesDiameterCompletedDelegate == null)
		{
			onOrderGetCurrentRingSizesDiameterCompletedDelegate = OnOrderGetCurrentRingSizesDiameterCompleted;
		}
		InvokeAsync(onBeginOrderGetCurrentRingSizesDiameterDelegate, null, onEndOrderGetCurrentRingSizesDiameterDelegate, onOrderGetCurrentRingSizesDiameterCompletedDelegate, userState);
	}

	public string[] OrderGetCurrentRingSizesUom()
	{
		return base.Channel.OrderGetCurrentRingSizesUom();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderGetCurrentRingSizesUom(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderGetCurrentRingSizesUom(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string[] EndOrderGetCurrentRingSizesUom(IAsyncResult result)
	{
		return base.Channel.EndOrderGetCurrentRingSizesUom(result);
	}

	private IAsyncResult OnBeginOrderGetCurrentRingSizesUom(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginOrderGetCurrentRingSizesUom(callback, asyncState);
	}

	private object[] OnEndOrderGetCurrentRingSizesUom(IAsyncResult result)
	{
		string[] array = EndOrderGetCurrentRingSizesUom(result);
		return new object[1] { array };
	}

	private void OnOrderGetCurrentRingSizesUomCompleted(object state)
	{
		if (this.OrderGetCurrentRingSizesUomCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderGetCurrentRingSizesUomCompleted(this, new OrderGetCurrentRingSizesUomCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderGetCurrentRingSizesUomAsync()
	{
		OrderGetCurrentRingSizesUomAsync(null);
	}

	public void OrderGetCurrentRingSizesUomAsync(object userState)
	{
		if (onBeginOrderGetCurrentRingSizesUomDelegate == null)
		{
			onBeginOrderGetCurrentRingSizesUomDelegate = OnBeginOrderGetCurrentRingSizesUom;
		}
		if (onEndOrderGetCurrentRingSizesUomDelegate == null)
		{
			onEndOrderGetCurrentRingSizesUomDelegate = OnEndOrderGetCurrentRingSizesUom;
		}
		if (onOrderGetCurrentRingSizesUomCompletedDelegate == null)
		{
			onOrderGetCurrentRingSizesUomCompletedDelegate = OnOrderGetCurrentRingSizesUomCompleted;
		}
		InvokeAsync(onBeginOrderGetCurrentRingSizesUomDelegate, null, onEndOrderGetCurrentRingSizesUomDelegate, onOrderGetCurrentRingSizesUomCompletedDelegate, userState);
	}

	public string[] OrderGetExpandedSliderList()
	{
		return base.Channel.OrderGetExpandedSliderList();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginOrderGetExpandedSliderList(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginOrderGetExpandedSliderList(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string[] EndOrderGetExpandedSliderList(IAsyncResult result)
	{
		return base.Channel.EndOrderGetExpandedSliderList(result);
	}

	private IAsyncResult OnBeginOrderGetExpandedSliderList(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginOrderGetExpandedSliderList(callback, asyncState);
	}

	private object[] OnEndOrderGetExpandedSliderList(IAsyncResult result)
	{
		string[] array = EndOrderGetExpandedSliderList(result);
		return new object[1] { array };
	}

	private void OnOrderGetExpandedSliderListCompleted(object state)
	{
		if (this.OrderGetExpandedSliderListCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.OrderGetExpandedSliderListCompleted(this, new OrderGetExpandedSliderListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void OrderGetExpandedSliderListAsync()
	{
		OrderGetExpandedSliderListAsync(null);
	}

	public void OrderGetExpandedSliderListAsync(object userState)
	{
		if (onBeginOrderGetExpandedSliderListDelegate == null)
		{
			onBeginOrderGetExpandedSliderListDelegate = OnBeginOrderGetExpandedSliderList;
		}
		if (onEndOrderGetExpandedSliderListDelegate == null)
		{
			onEndOrderGetExpandedSliderListDelegate = OnEndOrderGetExpandedSliderList;
		}
		if (onOrderGetExpandedSliderListCompletedDelegate == null)
		{
			onOrderGetExpandedSliderListCompletedDelegate = OnOrderGetExpandedSliderListCompleted;
		}
		InvokeAsync(onBeginOrderGetExpandedSliderListDelegate, null, onEndOrderGetExpandedSliderListDelegate, onOrderGetExpandedSliderListCompletedDelegate, userState);
	}

	public void PerformDistanceMeasurement()
	{
		base.Channel.PerformDistanceMeasurement();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginPerformDistanceMeasurement(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginPerformDistanceMeasurement(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndPerformDistanceMeasurement(IAsyncResult result)
	{
		base.Channel.EndPerformDistanceMeasurement(result);
	}

	private IAsyncResult OnBeginPerformDistanceMeasurement(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginPerformDistanceMeasurement(callback, asyncState);
	}

	private object[] OnEndPerformDistanceMeasurement(IAsyncResult result)
	{
		EndPerformDistanceMeasurement(result);
		return null;
	}

	private void OnPerformDistanceMeasurementCompleted(object state)
	{
		if (this.PerformDistanceMeasurementCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.PerformDistanceMeasurementCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void PerformDistanceMeasurementAsync()
	{
		PerformDistanceMeasurementAsync(null);
	}

	public void PerformDistanceMeasurementAsync(object userState)
	{
		if (onBeginPerformDistanceMeasurementDelegate == null)
		{
			onBeginPerformDistanceMeasurementDelegate = OnBeginPerformDistanceMeasurement;
		}
		if (onEndPerformDistanceMeasurementDelegate == null)
		{
			onEndPerformDistanceMeasurementDelegate = OnEndPerformDistanceMeasurement;
		}
		if (onPerformDistanceMeasurementCompletedDelegate == null)
		{
			onPerformDistanceMeasurementCompletedDelegate = OnPerformDistanceMeasurementCompleted;
		}
		InvokeAsync(onBeginPerformDistanceMeasurementDelegate, null, onEndPerformDistanceMeasurementDelegate, onPerformDistanceMeasurementCompletedDelegate, userState);
	}

	public void PrintingSupportDisplayConduitEnabled(bool isEnabled)
	{
		base.Channel.PrintingSupportDisplayConduitEnabled(isEnabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginPrintingSupportDisplayConduitEnabled(bool isEnabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginPrintingSupportDisplayConduitEnabled(isEnabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndPrintingSupportDisplayConduitEnabled(IAsyncResult result)
	{
		base.Channel.EndPrintingSupportDisplayConduitEnabled(result);
	}

	private IAsyncResult OnBeginPrintingSupportDisplayConduitEnabled(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isEnabled = (bool)inValues[0];
		return BeginPrintingSupportDisplayConduitEnabled(isEnabled, callback, asyncState);
	}

	private object[] OnEndPrintingSupportDisplayConduitEnabled(IAsyncResult result)
	{
		EndPrintingSupportDisplayConduitEnabled(result);
		return null;
	}

	private void OnPrintingSupportDisplayConduitEnabledCompleted(object state)
	{
		if (this.PrintingSupportDisplayConduitEnabledCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.PrintingSupportDisplayConduitEnabledCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void PrintingSupportDisplayConduitEnabledAsync(bool isEnabled)
	{
		PrintingSupportDisplayConduitEnabledAsync(isEnabled, null);
	}

	public void PrintingSupportDisplayConduitEnabledAsync(bool isEnabled, object userState)
	{
		if (onBeginPrintingSupportDisplayConduitEnabledDelegate == null)
		{
			onBeginPrintingSupportDisplayConduitEnabledDelegate = OnBeginPrintingSupportDisplayConduitEnabled;
		}
		if (onEndPrintingSupportDisplayConduitEnabledDelegate == null)
		{
			onEndPrintingSupportDisplayConduitEnabledDelegate = OnEndPrintingSupportDisplayConduitEnabled;
		}
		if (onPrintingSupportDisplayConduitEnabledCompletedDelegate == null)
		{
			onPrintingSupportDisplayConduitEnabledCompletedDelegate = OnPrintingSupportDisplayConduitEnabledCompleted;
		}
		InvokeAsync(onBeginPrintingSupportDisplayConduitEnabledDelegate, new object[1] { isEnabled }, onEndPrintingSupportDisplayConduitEnabledDelegate, onPrintingSupportDisplayConduitEnabledCompletedDelegate, userState);
	}

	public bool PrintMultipleJobs(out string sStatus, int[] lstJobsToPrint)
	{
		return base.Channel.PrintMultipleJobs(out sStatus, lstJobsToPrint);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginPrintMultipleJobs(int[] lstJobsToPrint, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginPrintMultipleJobs(lstJobsToPrint, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndPrintMultipleJobs(out string sStatus, IAsyncResult result)
	{
		return base.Channel.EndPrintMultipleJobs(out sStatus, result);
	}

	private IAsyncResult OnBeginPrintMultipleJobs(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int[] lstJobsToPrint = (int[])inValues[0];
		return BeginPrintMultipleJobs(lstJobsToPrint, callback, asyncState);
	}

	private object[] OnEndPrintMultipleJobs(IAsyncResult result)
	{
		string sStatus = GetDefaultValueForInitialization<string>();
		bool flag = EndPrintMultipleJobs(out sStatus, result);
		return new object[2] { sStatus, flag };
	}

	private void OnPrintMultipleJobsCompleted(object state)
	{
		if (this.PrintMultipleJobsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.PrintMultipleJobsCompleted(this, new PrintMultipleJobsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void PrintMultipleJobsAsync(int[] lstJobsToPrint)
	{
		PrintMultipleJobsAsync(lstJobsToPrint, null);
	}

	public void PrintMultipleJobsAsync(int[] lstJobsToPrint, object userState)
	{
		if (onBeginPrintMultipleJobsDelegate == null)
		{
			onBeginPrintMultipleJobsDelegate = OnBeginPrintMultipleJobs;
		}
		if (onEndPrintMultipleJobsDelegate == null)
		{
			onEndPrintMultipleJobsDelegate = OnEndPrintMultipleJobs;
		}
		if (onPrintMultipleJobsCompletedDelegate == null)
		{
			onPrintMultipleJobsCompletedDelegate = OnPrintMultipleJobsCompleted;
		}
		InvokeAsync(onBeginPrintMultipleJobsDelegate, new object[1] { lstJobsToPrint }, onEndPrintMultipleJobsDelegate, onPrintMultipleJobsCompletedDelegate, userState);
	}

	public bool PrintUnifiedMeshToProJet1200(out string ErrorMessage)
	{
		return base.Channel.PrintUnifiedMeshToProJet1200(out ErrorMessage);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginPrintUnifiedMeshToProJet1200(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginPrintUnifiedMeshToProJet1200(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndPrintUnifiedMeshToProJet1200(out string ErrorMessage, IAsyncResult result)
	{
		return base.Channel.EndPrintUnifiedMeshToProJet1200(out ErrorMessage, result);
	}

	private IAsyncResult OnBeginPrintUnifiedMeshToProJet1200(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginPrintUnifiedMeshToProJet1200(callback, asyncState);
	}

	private object[] OnEndPrintUnifiedMeshToProJet1200(IAsyncResult result)
	{
		string ErrorMessage = GetDefaultValueForInitialization<string>();
		bool flag = EndPrintUnifiedMeshToProJet1200(out ErrorMessage, result);
		return new object[2] { ErrorMessage, flag };
	}

	private void OnPrintUnifiedMeshToProJet1200Completed(object state)
	{
		if (this.PrintUnifiedMeshToProJet1200Completed != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.PrintUnifiedMeshToProJet1200Completed(this, new PrintUnifiedMeshToProJet1200CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void PrintUnifiedMeshToProJet1200Async()
	{
		PrintUnifiedMeshToProJet1200Async(null);
	}

	public void PrintUnifiedMeshToProJet1200Async(object userState)
	{
		if (onBeginPrintUnifiedMeshToProJet1200Delegate == null)
		{
			onBeginPrintUnifiedMeshToProJet1200Delegate = OnBeginPrintUnifiedMeshToProJet1200;
		}
		if (onEndPrintUnifiedMeshToProJet1200Delegate == null)
		{
			onEndPrintUnifiedMeshToProJet1200Delegate = OnEndPrintUnifiedMeshToProJet1200;
		}
		if (onPrintUnifiedMeshToProJet1200CompletedDelegate == null)
		{
			onPrintUnifiedMeshToProJet1200CompletedDelegate = OnPrintUnifiedMeshToProJet1200Completed;
		}
		InvokeAsync(onBeginPrintUnifiedMeshToProJet1200Delegate, null, onEndPrintUnifiedMeshToProJet1200Delegate, onPrintUnifiedMeshToProJet1200CompletedDelegate, userState);
	}

	public void RedrawRhino()
	{
		base.Channel.RedrawRhino();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRedrawRhino(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRedrawRhino(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRedrawRhino(IAsyncResult result)
	{
		base.Channel.EndRedrawRhino(result);
	}

	private IAsyncResult OnBeginRedrawRhino(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRedrawRhino(callback, asyncState);
	}

	private object[] OnEndRedrawRhino(IAsyncResult result)
	{
		EndRedrawRhino(result);
		return null;
	}

	private void OnRedrawRhinoCompleted(object state)
	{
		if (this.RedrawRhinoCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RedrawRhinoCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RedrawRhinoAsync()
	{
		RedrawRhinoAsync(null);
	}

	public void RedrawRhinoAsync(object userState)
	{
		if (onBeginRedrawRhinoDelegate == null)
		{
			onBeginRedrawRhinoDelegate = OnBeginRedrawRhino;
		}
		if (onEndRedrawRhinoDelegate == null)
		{
			onEndRedrawRhinoDelegate = OnEndRedrawRhino;
		}
		if (onRedrawRhinoCompletedDelegate == null)
		{
			onRedrawRhinoCompletedDelegate = OnRedrawRhinoCompleted;
		}
		InvokeAsync(onBeginRedrawRhinoDelegate, null, onEndRedrawRhinoDelegate, onRedrawRhinoCompletedDelegate, userState);
	}

	public void RemovePropFromDisplay()
	{
		base.Channel.RemovePropFromDisplay();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRemovePropFromDisplay(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRemovePropFromDisplay(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRemovePropFromDisplay(IAsyncResult result)
	{
		base.Channel.EndRemovePropFromDisplay(result);
	}

	private IAsyncResult OnBeginRemovePropFromDisplay(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRemovePropFromDisplay(callback, asyncState);
	}

	private object[] OnEndRemovePropFromDisplay(IAsyncResult result)
	{
		EndRemovePropFromDisplay(result);
		return null;
	}

	private void OnRemovePropFromDisplayCompleted(object state)
	{
		if (this.RemovePropFromDisplayCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RemovePropFromDisplayCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RemovePropFromDisplayAsync()
	{
		RemovePropFromDisplayAsync(null);
	}

	public void RemovePropFromDisplayAsync(object userState)
	{
		if (onBeginRemovePropFromDisplayDelegate == null)
		{
			onBeginRemovePropFromDisplayDelegate = OnBeginRemovePropFromDisplay;
		}
		if (onEndRemovePropFromDisplayDelegate == null)
		{
			onEndRemovePropFromDisplayDelegate = OnEndRemovePropFromDisplay;
		}
		if (onRemovePropFromDisplayCompletedDelegate == null)
		{
			onRemovePropFromDisplayCompletedDelegate = OnRemovePropFromDisplayCompleted;
		}
		InvokeAsync(onBeginRemovePropFromDisplayDelegate, null, onEndRemovePropFromDisplayDelegate, onRemovePropFromDisplayCompletedDelegate, userState);
	}

	public void RenderGenerateRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane)
	{
		base.Channel.RenderGenerateRender(isRenderLayDesignFlat, isRenderUsePropsSelected, renderPropSwatchCollectionItem, renderPropMaterialSwatchCollectionItem, renderPropMaterialSwatchCollectionSecondaryItem, groundPlaneMaterial, renderResolution, renderEnvironment, saveRender, saveRenderLocation, alphaProps, alphaGroundPlane);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRenderGenerateRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRenderGenerateRender(isRenderLayDesignFlat, isRenderUsePropsSelected, renderPropSwatchCollectionItem, renderPropMaterialSwatchCollectionItem, renderPropMaterialSwatchCollectionSecondaryItem, groundPlaneMaterial, renderResolution, renderEnvironment, saveRender, saveRenderLocation, alphaProps, alphaGroundPlane, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRenderGenerateRender(IAsyncResult result)
	{
		base.Channel.EndRenderGenerateRender(result);
	}

	private IAsyncResult OnBeginRenderGenerateRender(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isRenderLayDesignFlat = (bool)inValues[0];
		bool isRenderUsePropsSelected = (bool)inValues[1];
		string renderPropSwatchCollectionItem = (string)inValues[2];
		string renderPropMaterialSwatchCollectionItem = (string)inValues[3];
		string renderPropMaterialSwatchCollectionSecondaryItem = (string)inValues[4];
		string groundPlaneMaterial = (string)inValues[5];
		string renderResolution = (string)inValues[6];
		string renderEnvironment = (string)inValues[7];
		bool saveRender = (bool)inValues[8];
		string saveRenderLocation = (string)inValues[9];
		bool alphaProps = (bool)inValues[10];
		bool alphaGroundPlane = (bool)inValues[11];
		return BeginRenderGenerateRender(isRenderLayDesignFlat, isRenderUsePropsSelected, renderPropSwatchCollectionItem, renderPropMaterialSwatchCollectionItem, renderPropMaterialSwatchCollectionSecondaryItem, groundPlaneMaterial, renderResolution, renderEnvironment, saveRender, saveRenderLocation, alphaProps, alphaGroundPlane, callback, asyncState);
	}

	private object[] OnEndRenderGenerateRender(IAsyncResult result)
	{
		EndRenderGenerateRender(result);
		return null;
	}

	private void OnRenderGenerateRenderCompleted(object state)
	{
		if (this.RenderGenerateRenderCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RenderGenerateRenderCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RenderGenerateRenderAsync(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane)
	{
		RenderGenerateRenderAsync(isRenderLayDesignFlat, isRenderUsePropsSelected, renderPropSwatchCollectionItem, renderPropMaterialSwatchCollectionItem, renderPropMaterialSwatchCollectionSecondaryItem, groundPlaneMaterial, renderResolution, renderEnvironment, saveRender, saveRenderLocation, alphaProps, alphaGroundPlane, null);
	}

	public void RenderGenerateRenderAsync(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane, object userState)
	{
		if (onBeginRenderGenerateRenderDelegate == null)
		{
			onBeginRenderGenerateRenderDelegate = OnBeginRenderGenerateRender;
		}
		if (onEndRenderGenerateRenderDelegate == null)
		{
			onEndRenderGenerateRenderDelegate = OnEndRenderGenerateRender;
		}
		if (onRenderGenerateRenderCompletedDelegate == null)
		{
			onRenderGenerateRenderCompletedDelegate = OnRenderGenerateRenderCompleted;
		}
		InvokeAsync(onBeginRenderGenerateRenderDelegate, new object[12]
		{
			isRenderLayDesignFlat, isRenderUsePropsSelected, renderPropSwatchCollectionItem, renderPropMaterialSwatchCollectionItem, renderPropMaterialSwatchCollectionSecondaryItem, groundPlaneMaterial, renderResolution, renderEnvironment, saveRender, saveRenderLocation,
			alphaProps, alphaGroundPlane
		}, onEndRenderGenerateRenderDelegate, onRenderGenerateRenderCompletedDelegate, userState);
	}

	public void RepaintRhino()
	{
		base.Channel.RepaintRhino();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRepaintRhino(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRepaintRhino(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRepaintRhino(IAsyncResult result)
	{
		base.Channel.EndRepaintRhino(result);
	}

	private IAsyncResult OnBeginRepaintRhino(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRepaintRhino(callback, asyncState);
	}

	private object[] OnEndRepaintRhino(IAsyncResult result)
	{
		EndRepaintRhino(result);
		return null;
	}

	private void OnRepaintRhinoCompleted(object state)
	{
		if (this.RepaintRhinoCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RepaintRhinoCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RepaintRhinoAsync()
	{
		RepaintRhinoAsync(null);
	}

	public void RepaintRhinoAsync(object userState)
	{
		if (onBeginRepaintRhinoDelegate == null)
		{
			onBeginRepaintRhinoDelegate = OnBeginRepaintRhino;
		}
		if (onEndRepaintRhinoDelegate == null)
		{
			onEndRepaintRhinoDelegate = OnEndRepaintRhino;
		}
		if (onRepaintRhinoCompletedDelegate == null)
		{
			onRepaintRhinoCompletedDelegate = OnRepaintRhinoCompleted;
		}
		InvokeAsync(onBeginRepaintRhinoDelegate, null, onEndRepaintRhinoDelegate, onRepaintRhinoCompletedDelegate, userState);
	}

	public Bitmap RequestScreenCapture(Size captureSize)
	{
		return base.Channel.RequestScreenCapture(captureSize);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRequestScreenCapture(Size captureSize, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRequestScreenCapture(captureSize, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bitmap EndRequestScreenCapture(IAsyncResult result)
	{
		return base.Channel.EndRequestScreenCapture(result);
	}

	private IAsyncResult OnBeginRequestScreenCapture(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Size captureSize = (Size)inValues[0];
		return BeginRequestScreenCapture(captureSize, callback, asyncState);
	}

	private object[] OnEndRequestScreenCapture(IAsyncResult result)
	{
		Bitmap bitmap = EndRequestScreenCapture(result);
		return new object[1] { bitmap };
	}

	private void OnRequestScreenCaptureCompleted(object state)
	{
		if (this.RequestScreenCaptureCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RequestScreenCaptureCompleted(this, new RequestScreenCaptureCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RequestScreenCaptureAsync(Size captureSize)
	{
		RequestScreenCaptureAsync(captureSize, null);
	}

	public void RequestScreenCaptureAsync(Size captureSize, object userState)
	{
		if (onBeginRequestScreenCaptureDelegate == null)
		{
			onBeginRequestScreenCaptureDelegate = OnBeginRequestScreenCapture;
		}
		if (onEndRequestScreenCaptureDelegate == null)
		{
			onEndRequestScreenCaptureDelegate = OnEndRequestScreenCapture;
		}
		if (onRequestScreenCaptureCompletedDelegate == null)
		{
			onRequestScreenCaptureCompletedDelegate = OnRequestScreenCaptureCompleted;
		}
		InvokeAsync(onBeginRequestScreenCaptureDelegate, new object[1] { captureSize }, onEndRequestScreenCaptureDelegate, onRequestScreenCaptureCompletedDelegate, userState);
	}

	public Bitmap RequestScreenCaptureAndFreeze(Size captureSize)
	{
		return base.Channel.RequestScreenCaptureAndFreeze(captureSize);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRequestScreenCaptureAndFreeze(Size captureSize, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRequestScreenCaptureAndFreeze(captureSize, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bitmap EndRequestScreenCaptureAndFreeze(IAsyncResult result)
	{
		return base.Channel.EndRequestScreenCaptureAndFreeze(result);
	}

	private IAsyncResult OnBeginRequestScreenCaptureAndFreeze(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Size captureSize = (Size)inValues[0];
		return BeginRequestScreenCaptureAndFreeze(captureSize, callback, asyncState);
	}

	private object[] OnEndRequestScreenCaptureAndFreeze(IAsyncResult result)
	{
		Bitmap bitmap = EndRequestScreenCaptureAndFreeze(result);
		return new object[1] { bitmap };
	}

	private void OnRequestScreenCaptureAndFreezeCompleted(object state)
	{
		if (this.RequestScreenCaptureAndFreezeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RequestScreenCaptureAndFreezeCompleted(this, new RequestScreenCaptureAndFreezeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RequestScreenCaptureAndFreezeAsync(Size captureSize)
	{
		RequestScreenCaptureAndFreezeAsync(captureSize, null);
	}

	public void RequestScreenCaptureAndFreezeAsync(Size captureSize, object userState)
	{
		if (onBeginRequestScreenCaptureAndFreezeDelegate == null)
		{
			onBeginRequestScreenCaptureAndFreezeDelegate = OnBeginRequestScreenCaptureAndFreeze;
		}
		if (onEndRequestScreenCaptureAndFreezeDelegate == null)
		{
			onEndRequestScreenCaptureAndFreezeDelegate = OnEndRequestScreenCaptureAndFreeze;
		}
		if (onRequestScreenCaptureAndFreezeCompletedDelegate == null)
		{
			onRequestScreenCaptureAndFreezeCompletedDelegate = OnRequestScreenCaptureAndFreezeCompleted;
		}
		InvokeAsync(onBeginRequestScreenCaptureAndFreezeDelegate, new object[1] { captureSize }, onEndRequestScreenCaptureAndFreezeDelegate, onRequestScreenCaptureAndFreezeCompletedDelegate, userState);
	}

	public void RequestScreenCaptureAtPerspectivePositionForImportGroup(Size captureSize, bool zoom, Guid[] importGroupIdentifiers)
	{
		base.Channel.RequestScreenCaptureAtPerspectivePositionForImportGroup(captureSize, zoom, importGroupIdentifiers);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRequestScreenCaptureAtPerspectivePositionForImportGroup(Size captureSize, bool zoom, Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRequestScreenCaptureAtPerspectivePositionForImportGroup(captureSize, zoom, importGroupIdentifiers, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRequestScreenCaptureAtPerspectivePositionForImportGroup(IAsyncResult result)
	{
		base.Channel.EndRequestScreenCaptureAtPerspectivePositionForImportGroup(result);
	}

	private IAsyncResult OnBeginRequestScreenCaptureAtPerspectivePositionForImportGroup(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Size captureSize = (Size)inValues[0];
		bool zoom = (bool)inValues[1];
		Guid[] importGroupIdentifiers = (Guid[])inValues[2];
		return BeginRequestScreenCaptureAtPerspectivePositionForImportGroup(captureSize, zoom, importGroupIdentifiers, callback, asyncState);
	}

	private object[] OnEndRequestScreenCaptureAtPerspectivePositionForImportGroup(IAsyncResult result)
	{
		EndRequestScreenCaptureAtPerspectivePositionForImportGroup(result);
		return null;
	}

	private void OnRequestScreenCaptureAtPerspectivePositionForImportGroupCompleted(object state)
	{
		if (this.RequestScreenCaptureAtPerspectivePositionForImportGroupCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RequestScreenCaptureAtPerspectivePositionForImportGroupCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RequestScreenCaptureAtPerspectivePositionForImportGroupAsync(Size captureSize, bool zoom, Guid[] importGroupIdentifiers)
	{
		RequestScreenCaptureAtPerspectivePositionForImportGroupAsync(captureSize, zoom, importGroupIdentifiers, null);
	}

	public void RequestScreenCaptureAtPerspectivePositionForImportGroupAsync(Size captureSize, bool zoom, Guid[] importGroupIdentifiers, object userState)
	{
		if (onBeginRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate == null)
		{
			onBeginRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate = OnBeginRequestScreenCaptureAtPerspectivePositionForImportGroup;
		}
		if (onEndRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate == null)
		{
			onEndRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate = OnEndRequestScreenCaptureAtPerspectivePositionForImportGroup;
		}
		if (onRequestScreenCaptureAtPerspectivePositionForImportGroupCompletedDelegate == null)
		{
			onRequestScreenCaptureAtPerspectivePositionForImportGroupCompletedDelegate = OnRequestScreenCaptureAtPerspectivePositionForImportGroupCompleted;
		}
		InvokeAsync(onBeginRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate, new object[3] { captureSize, zoom, importGroupIdentifiers }, onEndRequestScreenCaptureAtPerspectivePositionForImportGroupDelegate, onRequestScreenCaptureAtPerspectivePositionForImportGroupCompletedDelegate, userState);
	}

	public Bitmap RequestScreenCaptureDefault(Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly)
	{
		return base.Channel.RequestScreenCaptureDefault(captureSize, zoom, zoomPadding, isMainDesignOnly);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRequestScreenCaptureDefault(Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRequestScreenCaptureDefault(captureSize, zoom, zoomPadding, isMainDesignOnly, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bitmap EndRequestScreenCaptureDefault(IAsyncResult result)
	{
		return base.Channel.EndRequestScreenCaptureDefault(result);
	}

	private IAsyncResult OnBeginRequestScreenCaptureDefault(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Size captureSize = (Size)inValues[0];
		bool zoom = (bool)inValues[1];
		double zoomPadding = (double)inValues[2];
		bool isMainDesignOnly = (bool)inValues[3];
		return BeginRequestScreenCaptureDefault(captureSize, zoom, zoomPadding, isMainDesignOnly, callback, asyncState);
	}

	private object[] OnEndRequestScreenCaptureDefault(IAsyncResult result)
	{
		Bitmap bitmap = EndRequestScreenCaptureDefault(result);
		return new object[1] { bitmap };
	}

	private void OnRequestScreenCaptureDefaultCompleted(object state)
	{
		if (this.RequestScreenCaptureDefaultCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RequestScreenCaptureDefaultCompleted(this, new RequestScreenCaptureDefaultCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RequestScreenCaptureDefaultAsync(Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly)
	{
		RequestScreenCaptureDefaultAsync(captureSize, zoom, zoomPadding, isMainDesignOnly, null);
	}

	public void RequestScreenCaptureDefaultAsync(Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly, object userState)
	{
		if (onBeginRequestScreenCaptureDefaultDelegate == null)
		{
			onBeginRequestScreenCaptureDefaultDelegate = OnBeginRequestScreenCaptureDefault;
		}
		if (onEndRequestScreenCaptureDefaultDelegate == null)
		{
			onEndRequestScreenCaptureDefaultDelegate = OnEndRequestScreenCaptureDefault;
		}
		if (onRequestScreenCaptureDefaultCompletedDelegate == null)
		{
			onRequestScreenCaptureDefaultCompletedDelegate = OnRequestScreenCaptureDefaultCompleted;
		}
		InvokeAsync(onBeginRequestScreenCaptureDefaultDelegate, new object[4] { captureSize, zoom, zoomPadding, isMainDesignOnly }, onEndRequestScreenCaptureDefaultDelegate, onRequestScreenCaptureDefaultCompletedDelegate, userState);
	}

	public Bitmap RequestScreenCaptureFront(Size captureSize)
	{
		return base.Channel.RequestScreenCaptureFront(captureSize);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRequestScreenCaptureFront(Size captureSize, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRequestScreenCaptureFront(captureSize, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bitmap EndRequestScreenCaptureFront(IAsyncResult result)
	{
		return base.Channel.EndRequestScreenCaptureFront(result);
	}

	private IAsyncResult OnBeginRequestScreenCaptureFront(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Size captureSize = (Size)inValues[0];
		return BeginRequestScreenCaptureFront(captureSize, callback, asyncState);
	}

	private object[] OnEndRequestScreenCaptureFront(IAsyncResult result)
	{
		Bitmap bitmap = EndRequestScreenCaptureFront(result);
		return new object[1] { bitmap };
	}

	private void OnRequestScreenCaptureFrontCompleted(object state)
	{
		if (this.RequestScreenCaptureFrontCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RequestScreenCaptureFrontCompleted(this, new RequestScreenCaptureFrontCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RequestScreenCaptureFrontAsync(Size captureSize)
	{
		RequestScreenCaptureFrontAsync(captureSize, null);
	}

	public void RequestScreenCaptureFrontAsync(Size captureSize, object userState)
	{
		if (onBeginRequestScreenCaptureFrontDelegate == null)
		{
			onBeginRequestScreenCaptureFrontDelegate = OnBeginRequestScreenCaptureFront;
		}
		if (onEndRequestScreenCaptureFrontDelegate == null)
		{
			onEndRequestScreenCaptureFrontDelegate = OnEndRequestScreenCaptureFront;
		}
		if (onRequestScreenCaptureFrontCompletedDelegate == null)
		{
			onRequestScreenCaptureFrontCompletedDelegate = OnRequestScreenCaptureFrontCompleted;
		}
		InvokeAsync(onBeginRequestScreenCaptureFrontDelegate, new object[1] { captureSize }, onEndRequestScreenCaptureFrontDelegate, onRequestScreenCaptureFrontCompletedDelegate, userState);
	}

	public Bitmap RequestScreenCaptureSide(Size captureSize)
	{
		return base.Channel.RequestScreenCaptureSide(captureSize);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRequestScreenCaptureSide(Size captureSize, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRequestScreenCaptureSide(captureSize, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bitmap EndRequestScreenCaptureSide(IAsyncResult result)
	{
		return base.Channel.EndRequestScreenCaptureSide(result);
	}

	private IAsyncResult OnBeginRequestScreenCaptureSide(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Size captureSize = (Size)inValues[0];
		return BeginRequestScreenCaptureSide(captureSize, callback, asyncState);
	}

	private object[] OnEndRequestScreenCaptureSide(IAsyncResult result)
	{
		Bitmap bitmap = EndRequestScreenCaptureSide(result);
		return new object[1] { bitmap };
	}

	private void OnRequestScreenCaptureSideCompleted(object state)
	{
		if (this.RequestScreenCaptureSideCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RequestScreenCaptureSideCompleted(this, new RequestScreenCaptureSideCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RequestScreenCaptureSideAsync(Size captureSize)
	{
		RequestScreenCaptureSideAsync(captureSize, null);
	}

	public void RequestScreenCaptureSideAsync(Size captureSize, object userState)
	{
		if (onBeginRequestScreenCaptureSideDelegate == null)
		{
			onBeginRequestScreenCaptureSideDelegate = OnBeginRequestScreenCaptureSide;
		}
		if (onEndRequestScreenCaptureSideDelegate == null)
		{
			onEndRequestScreenCaptureSideDelegate = OnEndRequestScreenCaptureSide;
		}
		if (onRequestScreenCaptureSideCompletedDelegate == null)
		{
			onRequestScreenCaptureSideCompletedDelegate = OnRequestScreenCaptureSideCompleted;
		}
		InvokeAsync(onBeginRequestScreenCaptureSideDelegate, new object[1] { captureSize }, onEndRequestScreenCaptureSideDelegate, onRequestScreenCaptureSideCompletedDelegate, userState);
	}

	public Bitmap RequestScreenCaptureTop(Size captureSize)
	{
		return base.Channel.RequestScreenCaptureTop(captureSize);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRequestScreenCaptureTop(Size captureSize, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRequestScreenCaptureTop(captureSize, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Bitmap EndRequestScreenCaptureTop(IAsyncResult result)
	{
		return base.Channel.EndRequestScreenCaptureTop(result);
	}

	private IAsyncResult OnBeginRequestScreenCaptureTop(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Size captureSize = (Size)inValues[0];
		return BeginRequestScreenCaptureTop(captureSize, callback, asyncState);
	}

	private object[] OnEndRequestScreenCaptureTop(IAsyncResult result)
	{
		Bitmap bitmap = EndRequestScreenCaptureTop(result);
		return new object[1] { bitmap };
	}

	private void OnRequestScreenCaptureTopCompleted(object state)
	{
		if (this.RequestScreenCaptureTopCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RequestScreenCaptureTopCompleted(this, new RequestScreenCaptureTopCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RequestScreenCaptureTopAsync(Size captureSize)
	{
		RequestScreenCaptureTopAsync(captureSize, null);
	}

	public void RequestScreenCaptureTopAsync(Size captureSize, object userState)
	{
		if (onBeginRequestScreenCaptureTopDelegate == null)
		{
			onBeginRequestScreenCaptureTopDelegate = OnBeginRequestScreenCaptureTop;
		}
		if (onEndRequestScreenCaptureTopDelegate == null)
		{
			onEndRequestScreenCaptureTopDelegate = OnEndRequestScreenCaptureTop;
		}
		if (onRequestScreenCaptureTopCompletedDelegate == null)
		{
			onRequestScreenCaptureTopCompletedDelegate = OnRequestScreenCaptureTopCompleted;
		}
		InvokeAsync(onBeginRequestScreenCaptureTopDelegate, new object[1] { captureSize }, onEndRequestScreenCaptureTopDelegate, onRequestScreenCaptureTopCompletedDelegate, userState);
	}

	public void ReselectViewportControls()
	{
		base.Channel.ReselectViewportControls();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginReselectViewportControls(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginReselectViewportControls(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndReselectViewportControls(IAsyncResult result)
	{
		base.Channel.EndReselectViewportControls(result);
	}

	private IAsyncResult OnBeginReselectViewportControls(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginReselectViewportControls(callback, asyncState);
	}

	private object[] OnEndReselectViewportControls(IAsyncResult result)
	{
		EndReselectViewportControls(result);
		return null;
	}

	private void OnReselectViewportControlsCompleted(object state)
	{
		if (this.ReselectViewportControlsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ReselectViewportControlsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ReselectViewportControlsAsync()
	{
		ReselectViewportControlsAsync(null);
	}

	public void ReselectViewportControlsAsync(object userState)
	{
		if (onBeginReselectViewportControlsDelegate == null)
		{
			onBeginReselectViewportControlsDelegate = OnBeginReselectViewportControls;
		}
		if (onEndReselectViewportControlsDelegate == null)
		{
			onEndReselectViewportControlsDelegate = OnEndReselectViewportControls;
		}
		if (onReselectViewportControlsCompletedDelegate == null)
		{
			onReselectViewportControlsCompletedDelegate = OnReselectViewportControlsCompleted;
		}
		InvokeAsync(onBeginReselectViewportControlsDelegate, null, onEndReselectViewportControlsDelegate, onReselectViewportControlsCompletedDelegate, userState);
	}

	public void ResetHandleForImportGroup(Guid importGroupIdentifier)
	{
		base.Channel.ResetHandleForImportGroup(importGroupIdentifier);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginResetHandleForImportGroup(Guid importGroupIdentifier, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginResetHandleForImportGroup(importGroupIdentifier, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndResetHandleForImportGroup(IAsyncResult result)
	{
		base.Channel.EndResetHandleForImportGroup(result);
	}

	private IAsyncResult OnBeginResetHandleForImportGroup(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid importGroupIdentifier = (Guid)inValues[0];
		return BeginResetHandleForImportGroup(importGroupIdentifier, callback, asyncState);
	}

	private object[] OnEndResetHandleForImportGroup(IAsyncResult result)
	{
		EndResetHandleForImportGroup(result);
		return null;
	}

	private void OnResetHandleForImportGroupCompleted(object state)
	{
		if (this.ResetHandleForImportGroupCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ResetHandleForImportGroupCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ResetHandleForImportGroupAsync(Guid importGroupIdentifier)
	{
		ResetHandleForImportGroupAsync(importGroupIdentifier, null);
	}

	public void ResetHandleForImportGroupAsync(Guid importGroupIdentifier, object userState)
	{
		if (onBeginResetHandleForImportGroupDelegate == null)
		{
			onBeginResetHandleForImportGroupDelegate = OnBeginResetHandleForImportGroup;
		}
		if (onEndResetHandleForImportGroupDelegate == null)
		{
			onEndResetHandleForImportGroupDelegate = OnEndResetHandleForImportGroup;
		}
		if (onResetHandleForImportGroupCompletedDelegate == null)
		{
			onResetHandleForImportGroupCompletedDelegate = OnResetHandleForImportGroupCompleted;
		}
		InvokeAsync(onBeginResetHandleForImportGroupDelegate, new object[1] { importGroupIdentifier }, onEndResetHandleForImportGroupDelegate, onResetHandleForImportGroupCompletedDelegate, userState);
	}

	public void RhinoAbortScripts()
	{
		base.Channel.RhinoAbortScripts();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoAbortScripts(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoAbortScripts(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRhinoAbortScripts(IAsyncResult result)
	{
		base.Channel.EndRhinoAbortScripts(result);
	}

	private IAsyncResult OnBeginRhinoAbortScripts(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRhinoAbortScripts(callback, asyncState);
	}

	private object[] OnEndRhinoAbortScripts(IAsyncResult result)
	{
		EndRhinoAbortScripts(result);
		return null;
	}

	private void OnRhinoAbortScriptsCompleted(object state)
	{
		if (this.RhinoAbortScriptsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoAbortScriptsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoAbortScriptsAsync()
	{
		RhinoAbortScriptsAsync(null);
	}

	public void RhinoAbortScriptsAsync(object userState)
	{
		if (onBeginRhinoAbortScriptsDelegate == null)
		{
			onBeginRhinoAbortScriptsDelegate = OnBeginRhinoAbortScripts;
		}
		if (onEndRhinoAbortScriptsDelegate == null)
		{
			onEndRhinoAbortScriptsDelegate = OnEndRhinoAbortScripts;
		}
		if (onRhinoAbortScriptsCompletedDelegate == null)
		{
			onRhinoAbortScriptsCompletedDelegate = OnRhinoAbortScriptsCompleted;
		}
		InvokeAsync(onBeginRhinoAbortScriptsDelegate, null, onEndRhinoAbortScriptsDelegate, onRhinoAbortScriptsCompletedDelegate, userState);
	}

	public void RhinoCloseViewPort(string viewportName)
	{
		base.Channel.RhinoCloseViewPort(viewportName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoCloseViewPort(string viewportName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoCloseViewPort(viewportName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRhinoCloseViewPort(IAsyncResult result)
	{
		base.Channel.EndRhinoCloseViewPort(result);
	}

	private IAsyncResult OnBeginRhinoCloseViewPort(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string viewportName = (string)inValues[0];
		return BeginRhinoCloseViewPort(viewportName, callback, asyncState);
	}

	private object[] OnEndRhinoCloseViewPort(IAsyncResult result)
	{
		EndRhinoCloseViewPort(result);
		return null;
	}

	private void OnRhinoCloseViewPortCompleted(object state)
	{
		if (this.RhinoCloseViewPortCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoCloseViewPortCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoCloseViewPortAsync(string viewportName)
	{
		RhinoCloseViewPortAsync(viewportName, null);
	}

	public void RhinoCloseViewPortAsync(string viewportName, object userState)
	{
		if (onBeginRhinoCloseViewPortDelegate == null)
		{
			onBeginRhinoCloseViewPortDelegate = OnBeginRhinoCloseViewPort;
		}
		if (onEndRhinoCloseViewPortDelegate == null)
		{
			onEndRhinoCloseViewPortDelegate = OnEndRhinoCloseViewPort;
		}
		if (onRhinoCloseViewPortCompletedDelegate == null)
		{
			onRhinoCloseViewPortCompletedDelegate = OnRhinoCloseViewPortCompleted;
		}
		InvokeAsync(onBeginRhinoCloseViewPortDelegate, new object[1] { viewportName }, onEndRhinoCloseViewPortDelegate, onRhinoCloseViewPortCompletedDelegate, userState);
	}

	public IntPtr RhinoCreateViewPort(string viewportName)
	{
		return base.Channel.RhinoCreateViewPort(viewportName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoCreateViewPort(string viewportName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoCreateViewPort(viewportName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IntPtr EndRhinoCreateViewPort(IAsyncResult result)
	{
		return base.Channel.EndRhinoCreateViewPort(result);
	}

	private IAsyncResult OnBeginRhinoCreateViewPort(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string viewportName = (string)inValues[0];
		return BeginRhinoCreateViewPort(viewportName, callback, asyncState);
	}

	private object[] OnEndRhinoCreateViewPort(IAsyncResult result)
	{
		IntPtr intPtr = EndRhinoCreateViewPort(result);
		return new object[1] { intPtr };
	}

	private void OnRhinoCreateViewPortCompleted(object state)
	{
		if (this.RhinoCreateViewPortCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoCreateViewPortCompleted(this, new RhinoCreateViewPortCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoCreateViewPortAsync(string viewportName)
	{
		RhinoCreateViewPortAsync(viewportName, null);
	}

	public void RhinoCreateViewPortAsync(string viewportName, object userState)
	{
		if (onBeginRhinoCreateViewPortDelegate == null)
		{
			onBeginRhinoCreateViewPortDelegate = OnBeginRhinoCreateViewPort;
		}
		if (onEndRhinoCreateViewPortDelegate == null)
		{
			onEndRhinoCreateViewPortDelegate = OnEndRhinoCreateViewPort;
		}
		if (onRhinoCreateViewPortCompletedDelegate == null)
		{
			onRhinoCreateViewPortCompletedDelegate = OnRhinoCreateViewPortCompleted;
		}
		InvokeAsync(onBeginRhinoCreateViewPortDelegate, new object[1] { viewportName }, onEndRhinoCreateViewPortDelegate, onRhinoCreateViewPortCompletedDelegate, userState);
	}

	public IntPtr RhinoGetActiveViewWindowHandle()
	{
		return base.Channel.RhinoGetActiveViewWindowHandle();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoGetActiveViewWindowHandle(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoGetActiveViewWindowHandle(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IntPtr EndRhinoGetActiveViewWindowHandle(IAsyncResult result)
	{
		return base.Channel.EndRhinoGetActiveViewWindowHandle(result);
	}

	private IAsyncResult OnBeginRhinoGetActiveViewWindowHandle(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRhinoGetActiveViewWindowHandle(callback, asyncState);
	}

	private object[] OnEndRhinoGetActiveViewWindowHandle(IAsyncResult result)
	{
		IntPtr intPtr = EndRhinoGetActiveViewWindowHandle(result);
		return new object[1] { intPtr };
	}

	private void OnRhinoGetActiveViewWindowHandleCompleted(object state)
	{
		if (this.RhinoGetActiveViewWindowHandleCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoGetActiveViewWindowHandleCompleted(this, new RhinoGetActiveViewWindowHandleCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoGetActiveViewWindowHandleAsync()
	{
		RhinoGetActiveViewWindowHandleAsync(null);
	}

	public void RhinoGetActiveViewWindowHandleAsync(object userState)
	{
		if (onBeginRhinoGetActiveViewWindowHandleDelegate == null)
		{
			onBeginRhinoGetActiveViewWindowHandleDelegate = OnBeginRhinoGetActiveViewWindowHandle;
		}
		if (onEndRhinoGetActiveViewWindowHandleDelegate == null)
		{
			onEndRhinoGetActiveViewWindowHandleDelegate = OnEndRhinoGetActiveViewWindowHandle;
		}
		if (onRhinoGetActiveViewWindowHandleCompletedDelegate == null)
		{
			onRhinoGetActiveViewWindowHandleCompletedDelegate = OnRhinoGetActiveViewWindowHandleCompleted;
		}
		InvokeAsync(onBeginRhinoGetActiveViewWindowHandleDelegate, null, onEndRhinoGetActiveViewWindowHandleDelegate, onRhinoGetActiveViewWindowHandleCompletedDelegate, userState);
	}

	public string RhinoGetVersion()
	{
		return base.Channel.RhinoGetVersion();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoGetVersion(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoGetVersion(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndRhinoGetVersion(IAsyncResult result)
	{
		return base.Channel.EndRhinoGetVersion(result);
	}

	private IAsyncResult OnBeginRhinoGetVersion(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRhinoGetVersion(callback, asyncState);
	}

	private object[] OnEndRhinoGetVersion(IAsyncResult result)
	{
		string text = EndRhinoGetVersion(result);
		return new object[1] { text };
	}

	private void OnRhinoGetVersionCompleted(object state)
	{
		if (this.RhinoGetVersionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoGetVersionCompleted(this, new RhinoGetVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoGetVersionAsync()
	{
		RhinoGetVersionAsync(null);
	}

	public void RhinoGetVersionAsync(object userState)
	{
		if (onBeginRhinoGetVersionDelegate == null)
		{
			onBeginRhinoGetVersionDelegate = OnBeginRhinoGetVersion;
		}
		if (onEndRhinoGetVersionDelegate == null)
		{
			onEndRhinoGetVersionDelegate = OnEndRhinoGetVersion;
		}
		if (onRhinoGetVersionCompletedDelegate == null)
		{
			onRhinoGetVersionCompletedDelegate = OnRhinoGetVersionCompleted;
		}
		InvokeAsync(onBeginRhinoGetVersionDelegate, null, onEndRhinoGetVersionDelegate, onRhinoGetVersionCompletedDelegate, userState);
	}

	public IntPtr RhinoGetWindowHandle()
	{
		return base.Channel.RhinoGetWindowHandle();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoGetWindowHandle(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoGetWindowHandle(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IntPtr EndRhinoGetWindowHandle(IAsyncResult result)
	{
		return base.Channel.EndRhinoGetWindowHandle(result);
	}

	private IAsyncResult OnBeginRhinoGetWindowHandle(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRhinoGetWindowHandle(callback, asyncState);
	}

	private object[] OnEndRhinoGetWindowHandle(IAsyncResult result)
	{
		IntPtr intPtr = EndRhinoGetWindowHandle(result);
		return new object[1] { intPtr };
	}

	private void OnRhinoGetWindowHandleCompleted(object state)
	{
		if (this.RhinoGetWindowHandleCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoGetWindowHandleCompleted(this, new RhinoGetWindowHandleCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoGetWindowHandleAsync()
	{
		RhinoGetWindowHandleAsync(null);
	}

	public void RhinoGetWindowHandleAsync(object userState)
	{
		if (onBeginRhinoGetWindowHandleDelegate == null)
		{
			onBeginRhinoGetWindowHandleDelegate = OnBeginRhinoGetWindowHandle;
		}
		if (onEndRhinoGetWindowHandleDelegate == null)
		{
			onEndRhinoGetWindowHandleDelegate = OnEndRhinoGetWindowHandle;
		}
		if (onRhinoGetWindowHandleCompletedDelegate == null)
		{
			onRhinoGetWindowHandleCompletedDelegate = OnRhinoGetWindowHandleCompleted;
		}
		InvokeAsync(onBeginRhinoGetWindowHandleDelegate, null, onEndRhinoGetWindowHandleDelegate, onRhinoGetWindowHandleCompletedDelegate, userState);
	}

	public void RhinoLoadBlankDocument()
	{
		base.Channel.RhinoLoadBlankDocument();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoLoadBlankDocument(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoLoadBlankDocument(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRhinoLoadBlankDocument(IAsyncResult result)
	{
		base.Channel.EndRhinoLoadBlankDocument(result);
	}

	private IAsyncResult OnBeginRhinoLoadBlankDocument(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRhinoLoadBlankDocument(callback, asyncState);
	}

	private object[] OnEndRhinoLoadBlankDocument(IAsyncResult result)
	{
		EndRhinoLoadBlankDocument(result);
		return null;
	}

	private void OnRhinoLoadBlankDocumentCompleted(object state)
	{
		if (this.RhinoLoadBlankDocumentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoLoadBlankDocumentCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoLoadBlankDocumentAsync()
	{
		RhinoLoadBlankDocumentAsync(null);
	}

	public void RhinoLoadBlankDocumentAsync(object userState)
	{
		if (onBeginRhinoLoadBlankDocumentDelegate == null)
		{
			onBeginRhinoLoadBlankDocumentDelegate = OnBeginRhinoLoadBlankDocument;
		}
		if (onEndRhinoLoadBlankDocumentDelegate == null)
		{
			onEndRhinoLoadBlankDocumentDelegate = OnEndRhinoLoadBlankDocument;
		}
		if (onRhinoLoadBlankDocumentCompletedDelegate == null)
		{
			onRhinoLoadBlankDocumentCompletedDelegate = OnRhinoLoadBlankDocumentCompleted;
		}
		InvokeAsync(onBeginRhinoLoadBlankDocumentDelegate, null, onEndRhinoLoadBlankDocumentDelegate, onRhinoLoadBlankDocumentCompletedDelegate, userState);
	}

	public void RhinoRunScript(string scriptText)
	{
		base.Channel.RhinoRunScript(scriptText);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoRunScript(string scriptText, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoRunScript(scriptText, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRhinoRunScript(IAsyncResult result)
	{
		base.Channel.EndRhinoRunScript(result);
	}

	private IAsyncResult OnBeginRhinoRunScript(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string scriptText = (string)inValues[0];
		return BeginRhinoRunScript(scriptText, callback, asyncState);
	}

	private object[] OnEndRhinoRunScript(IAsyncResult result)
	{
		EndRhinoRunScript(result);
		return null;
	}

	private void OnRhinoRunScriptCompleted(object state)
	{
		if (this.RhinoRunScriptCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoRunScriptCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoRunScriptAsync(string scriptText)
	{
		RhinoRunScriptAsync(scriptText, null);
	}

	public void RhinoRunScriptAsync(string scriptText, object userState)
	{
		if (onBeginRhinoRunScriptDelegate == null)
		{
			onBeginRhinoRunScriptDelegate = OnBeginRhinoRunScript;
		}
		if (onEndRhinoRunScriptDelegate == null)
		{
			onEndRhinoRunScriptDelegate = OnEndRhinoRunScript;
		}
		if (onRhinoRunScriptCompletedDelegate == null)
		{
			onRhinoRunScriptCompletedDelegate = OnRhinoRunScriptCompleted;
		}
		InvokeAsync(onBeginRhinoRunScriptDelegate, new object[1] { scriptText }, onEndRhinoRunScriptDelegate, onRhinoRunScriptCompletedDelegate, userState);
	}

	public void RhinoSetFocusTo()
	{
		base.Channel.RhinoSetFocusTo();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoSetFocusTo(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoSetFocusTo(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRhinoSetFocusTo(IAsyncResult result)
	{
		base.Channel.EndRhinoSetFocusTo(result);
	}

	private IAsyncResult OnBeginRhinoSetFocusTo(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginRhinoSetFocusTo(callback, asyncState);
	}

	private object[] OnEndRhinoSetFocusTo(IAsyncResult result)
	{
		EndRhinoSetFocusTo(result);
		return null;
	}

	private void OnRhinoSetFocusToCompleted(object state)
	{
		if (this.RhinoSetFocusToCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoSetFocusToCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoSetFocusToAsync()
	{
		RhinoSetFocusToAsync(null);
	}

	public void RhinoSetFocusToAsync(object userState)
	{
		if (onBeginRhinoSetFocusToDelegate == null)
		{
			onBeginRhinoSetFocusToDelegate = OnBeginRhinoSetFocusTo;
		}
		if (onEndRhinoSetFocusToDelegate == null)
		{
			onEndRhinoSetFocusToDelegate = OnEndRhinoSetFocusTo;
		}
		if (onRhinoSetFocusToCompletedDelegate == null)
		{
			onRhinoSetFocusToCompletedDelegate = OnRhinoSetFocusToCompleted;
		}
		InvokeAsync(onBeginRhinoSetFocusToDelegate, null, onEndRhinoSetFocusToDelegate, onRhinoSetFocusToCompletedDelegate, userState);
	}

	public bool RhinoWindowReparent(IntPtr intPtrParent)
	{
		return base.Channel.RhinoWindowReparent(intPtrParent);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoWindowReparent(IntPtr intPtrParent, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoWindowReparent(intPtrParent, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndRhinoWindowReparent(IAsyncResult result)
	{
		return base.Channel.EndRhinoWindowReparent(result);
	}

	private IAsyncResult OnBeginRhinoWindowReparent(object[] inValues, AsyncCallback callback, object asyncState)
	{
		IntPtr intPtrParent = (IntPtr)inValues[0];
		return BeginRhinoWindowReparent(intPtrParent, callback, asyncState);
	}

	private object[] OnEndRhinoWindowReparent(IAsyncResult result)
	{
		bool flag = EndRhinoWindowReparent(result);
		return new object[1] { flag };
	}

	private void OnRhinoWindowReparentCompleted(object state)
	{
		if (this.RhinoWindowReparentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoWindowReparentCompleted(this, new RhinoWindowReparentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoWindowReparentAsync(IntPtr intPtrParent)
	{
		RhinoWindowReparentAsync(intPtrParent, null);
	}

	public void RhinoWindowReparentAsync(IntPtr intPtrParent, object userState)
	{
		if (onBeginRhinoWindowReparentDelegate == null)
		{
			onBeginRhinoWindowReparentDelegate = OnBeginRhinoWindowReparent;
		}
		if (onEndRhinoWindowReparentDelegate == null)
		{
			onEndRhinoWindowReparentDelegate = OnEndRhinoWindowReparent;
		}
		if (onRhinoWindowReparentCompletedDelegate == null)
		{
			onRhinoWindowReparentCompletedDelegate = OnRhinoWindowReparentCompleted;
		}
		InvokeAsync(onBeginRhinoWindowReparentDelegate, new object[1] { intPtrParent }, onEndRhinoWindowReparentDelegate, onRhinoWindowReparentCompletedDelegate, userState);
	}

	public void RhinoWindowReposition(int startPointX, int startPointY, int width, int height)
	{
		base.Channel.RhinoWindowReposition(startPointX, startPointY, width, height);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRhinoWindowReposition(int startPointX, int startPointY, int width, int height, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRhinoWindowReposition(startPointX, startPointY, width, height, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndRhinoWindowReposition(IAsyncResult result)
	{
		base.Channel.EndRhinoWindowReposition(result);
	}

	private IAsyncResult OnBeginRhinoWindowReposition(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int startPointX = (int)inValues[0];
		int startPointY = (int)inValues[1];
		int width = (int)inValues[2];
		int height = (int)inValues[3];
		return BeginRhinoWindowReposition(startPointX, startPointY, width, height, callback, asyncState);
	}

	private object[] OnEndRhinoWindowReposition(IAsyncResult result)
	{
		EndRhinoWindowReposition(result);
		return null;
	}

	private void OnRhinoWindowRepositionCompleted(object state)
	{
		if (this.RhinoWindowRepositionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RhinoWindowRepositionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RhinoWindowRepositionAsync(int startPointX, int startPointY, int width, int height)
	{
		RhinoWindowRepositionAsync(startPointX, startPointY, width, height, null);
	}

	public void RhinoWindowRepositionAsync(int startPointX, int startPointY, int width, int height, object userState)
	{
		if (onBeginRhinoWindowRepositionDelegate == null)
		{
			onBeginRhinoWindowRepositionDelegate = OnBeginRhinoWindowReposition;
		}
		if (onEndRhinoWindowRepositionDelegate == null)
		{
			onEndRhinoWindowRepositionDelegate = OnEndRhinoWindowReposition;
		}
		if (onRhinoWindowRepositionCompletedDelegate == null)
		{
			onRhinoWindowRepositionCompletedDelegate = OnRhinoWindowRepositionCompleted;
		}
		InvokeAsync(onBeginRhinoWindowRepositionDelegate, new object[4] { startPointX, startPointY, width, height }, onEndRhinoWindowRepositionDelegate, onRhinoWindowRepositionCompletedDelegate, userState);
	}

	public bool SavePrintJob(out string sStatus, int whatslot)
	{
		return base.Channel.SavePrintJob(out sStatus, whatslot);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSavePrintJob(int whatslot, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSavePrintJob(whatslot, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndSavePrintJob(out string sStatus, IAsyncResult result)
	{
		return base.Channel.EndSavePrintJob(out sStatus, result);
	}

	private IAsyncResult OnBeginSavePrintJob(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int whatslot = (int)inValues[0];
		return BeginSavePrintJob(whatslot, callback, asyncState);
	}

	private object[] OnEndSavePrintJob(IAsyncResult result)
	{
		string sStatus = GetDefaultValueForInitialization<string>();
		bool flag = EndSavePrintJob(out sStatus, result);
		return new object[2] { sStatus, flag };
	}

	private void OnSavePrintJobCompleted(object state)
	{
		if (this.SavePrintJobCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SavePrintJobCompleted(this, new SavePrintJobCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SavePrintJobAsync(int whatslot)
	{
		SavePrintJobAsync(whatslot, null);
	}

	public void SavePrintJobAsync(int whatslot, object userState)
	{
		if (onBeginSavePrintJobDelegate == null)
		{
			onBeginSavePrintJobDelegate = OnBeginSavePrintJob;
		}
		if (onEndSavePrintJobDelegate == null)
		{
			onEndSavePrintJobDelegate = OnEndSavePrintJob;
		}
		if (onSavePrintJobCompletedDelegate == null)
		{
			onSavePrintJobCompletedDelegate = OnSavePrintJobCompleted;
		}
		InvokeAsync(onBeginSavePrintJobDelegate, new object[1] { whatslot }, onEndSavePrintJobDelegate, onSavePrintJobCompletedDelegate, userState);
	}

	public void SaveScreen(string saveImagePath)
	{
		base.Channel.SaveScreen(saveImagePath);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSaveScreen(string saveImagePath, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSaveScreen(saveImagePath, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSaveScreen(IAsyncResult result)
	{
		base.Channel.EndSaveScreen(result);
	}

	private IAsyncResult OnBeginSaveScreen(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string saveImagePath = (string)inValues[0];
		return BeginSaveScreen(saveImagePath, callback, asyncState);
	}

	private object[] OnEndSaveScreen(IAsyncResult result)
	{
		EndSaveScreen(result);
		return null;
	}

	private void OnSaveScreenCompleted(object state)
	{
		if (this.SaveScreenCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SaveScreenCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SaveScreenAsync(string saveImagePath)
	{
		SaveScreenAsync(saveImagePath, null);
	}

	public void SaveScreenAsync(string saveImagePath, object userState)
	{
		if (onBeginSaveScreenDelegate == null)
		{
			onBeginSaveScreenDelegate = OnBeginSaveScreen;
		}
		if (onEndSaveScreenDelegate == null)
		{
			onEndSaveScreenDelegate = OnEndSaveScreen;
		}
		if (onSaveScreenCompletedDelegate == null)
		{
			onSaveScreenCompletedDelegate = OnSaveScreenCompleted;
		}
		InvokeAsync(onBeginSaveScreenDelegate, new object[1] { saveImagePath }, onEndSaveScreenDelegate, onSaveScreenCompletedDelegate, userState);
	}

	public bool SaveViewportToImage(string viewportName, string pathToSaveImage, double maxHeightWidth)
	{
		return base.Channel.SaveViewportToImage(viewportName, pathToSaveImage, maxHeightWidth);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSaveViewportToImage(string viewportName, string pathToSaveImage, double maxHeightWidth, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSaveViewportToImage(viewportName, pathToSaveImage, maxHeightWidth, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndSaveViewportToImage(IAsyncResult result)
	{
		return base.Channel.EndSaveViewportToImage(result);
	}

	private IAsyncResult OnBeginSaveViewportToImage(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string viewportName = (string)inValues[0];
		string pathToSaveImage = (string)inValues[1];
		double maxHeightWidth = (double)inValues[2];
		return BeginSaveViewportToImage(viewportName, pathToSaveImage, maxHeightWidth, callback, asyncState);
	}

	private object[] OnEndSaveViewportToImage(IAsyncResult result)
	{
		bool flag = EndSaveViewportToImage(result);
		return new object[1] { flag };
	}

	private void OnSaveViewportToImageCompleted(object state)
	{
		if (this.SaveViewportToImageCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SaveViewportToImageCompleted(this, new SaveViewportToImageCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SaveViewportToImageAsync(string viewportName, string pathToSaveImage, double maxHeightWidth)
	{
		SaveViewportToImageAsync(viewportName, pathToSaveImage, maxHeightWidth, null);
	}

	public void SaveViewportToImageAsync(string viewportName, string pathToSaveImage, double maxHeightWidth, object userState)
	{
		if (onBeginSaveViewportToImageDelegate == null)
		{
			onBeginSaveViewportToImageDelegate = OnBeginSaveViewportToImage;
		}
		if (onEndSaveViewportToImageDelegate == null)
		{
			onEndSaveViewportToImageDelegate = OnEndSaveViewportToImage;
		}
		if (onSaveViewportToImageCompletedDelegate == null)
		{
			onSaveViewportToImageCompletedDelegate = OnSaveViewportToImageCompleted;
		}
		InvokeAsync(onBeginSaveViewportToImageDelegate, new object[3] { viewportName, pathToSaveImage, maxHeightWidth }, onEndSaveViewportToImageDelegate, onSaveViewportToImageCompletedDelegate, userState);
	}

	public void SelectImportedGroupByIdentifier(Guid importGroupIdentifier)
	{
		base.Channel.SelectImportedGroupByIdentifier(importGroupIdentifier);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSelectImportedGroupByIdentifier(Guid importGroupIdentifier, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSelectImportedGroupByIdentifier(importGroupIdentifier, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSelectImportedGroupByIdentifier(IAsyncResult result)
	{
		base.Channel.EndSelectImportedGroupByIdentifier(result);
	}

	private IAsyncResult OnBeginSelectImportedGroupByIdentifier(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid importGroupIdentifier = (Guid)inValues[0];
		return BeginSelectImportedGroupByIdentifier(importGroupIdentifier, callback, asyncState);
	}

	private object[] OnEndSelectImportedGroupByIdentifier(IAsyncResult result)
	{
		EndSelectImportedGroupByIdentifier(result);
		return null;
	}

	private void OnSelectImportedGroupByIdentifierCompleted(object state)
	{
		if (this.SelectImportedGroupByIdentifierCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SelectImportedGroupByIdentifierCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SelectImportedGroupByIdentifierAsync(Guid importGroupIdentifier)
	{
		SelectImportedGroupByIdentifierAsync(importGroupIdentifier, null);
	}

	public void SelectImportedGroupByIdentifierAsync(Guid importGroupIdentifier, object userState)
	{
		if (onBeginSelectImportedGroupByIdentifierDelegate == null)
		{
			onBeginSelectImportedGroupByIdentifierDelegate = OnBeginSelectImportedGroupByIdentifier;
		}
		if (onEndSelectImportedGroupByIdentifierDelegate == null)
		{
			onEndSelectImportedGroupByIdentifierDelegate = OnEndSelectImportedGroupByIdentifier;
		}
		if (onSelectImportedGroupByIdentifierCompletedDelegate == null)
		{
			onSelectImportedGroupByIdentifierCompletedDelegate = OnSelectImportedGroupByIdentifierCompleted;
		}
		InvokeAsync(onBeginSelectImportedGroupByIdentifierDelegate, new object[1] { importGroupIdentifier }, onEndSelectImportedGroupByIdentifierDelegate, onSelectImportedGroupByIdentifierCompletedDelegate, userState);
	}

	public void SelectImportedGroupByIdentifierSecondary(Guid[] importGroupIdentifiersd)
	{
		base.Channel.SelectImportedGroupByIdentifierSecondary(importGroupIdentifiersd);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSelectImportedGroupByIdentifierSecondary(Guid[] importGroupIdentifiersd, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSelectImportedGroupByIdentifierSecondary(importGroupIdentifiersd, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSelectImportedGroupByIdentifierSecondary(IAsyncResult result)
	{
		base.Channel.EndSelectImportedGroupByIdentifierSecondary(result);
	}

	private IAsyncResult OnBeginSelectImportedGroupByIdentifierSecondary(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiersd = (Guid[])inValues[0];
		return BeginSelectImportedGroupByIdentifierSecondary(importGroupIdentifiersd, callback, asyncState);
	}

	private object[] OnEndSelectImportedGroupByIdentifierSecondary(IAsyncResult result)
	{
		EndSelectImportedGroupByIdentifierSecondary(result);
		return null;
	}

	private void OnSelectImportedGroupByIdentifierSecondaryCompleted(object state)
	{
		if (this.SelectImportedGroupByIdentifierSecondaryCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SelectImportedGroupByIdentifierSecondaryCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SelectImportedGroupByIdentifierSecondaryAsync(Guid[] importGroupIdentifiersd)
	{
		SelectImportedGroupByIdentifierSecondaryAsync(importGroupIdentifiersd, null);
	}

	public void SelectImportedGroupByIdentifierSecondaryAsync(Guid[] importGroupIdentifiersd, object userState)
	{
		if (onBeginSelectImportedGroupByIdentifierSecondaryDelegate == null)
		{
			onBeginSelectImportedGroupByIdentifierSecondaryDelegate = OnBeginSelectImportedGroupByIdentifierSecondary;
		}
		if (onEndSelectImportedGroupByIdentifierSecondaryDelegate == null)
		{
			onEndSelectImportedGroupByIdentifierSecondaryDelegate = OnEndSelectImportedGroupByIdentifierSecondary;
		}
		if (onSelectImportedGroupByIdentifierSecondaryCompletedDelegate == null)
		{
			onSelectImportedGroupByIdentifierSecondaryCompletedDelegate = OnSelectImportedGroupByIdentifierSecondaryCompleted;
		}
		InvokeAsync(onBeginSelectImportedGroupByIdentifierSecondaryDelegate, new object[1] { importGroupIdentifiersd }, onEndSelectImportedGroupByIdentifierSecondaryDelegate, onSelectImportedGroupByIdentifierSecondaryCompletedDelegate, userState);
	}

	public void SetIsBlockParallelProcessingEnabled(bool enabled)
	{
		base.Channel.SetIsBlockParallelProcessingEnabled(enabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetIsBlockParallelProcessingEnabled(bool enabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetIsBlockParallelProcessingEnabled(enabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetIsBlockParallelProcessingEnabled(IAsyncResult result)
	{
		base.Channel.EndSetIsBlockParallelProcessingEnabled(result);
	}

	private IAsyncResult OnBeginSetIsBlockParallelProcessingEnabled(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool enabled = (bool)inValues[0];
		return BeginSetIsBlockParallelProcessingEnabled(enabled, callback, asyncState);
	}

	private object[] OnEndSetIsBlockParallelProcessingEnabled(IAsyncResult result)
	{
		EndSetIsBlockParallelProcessingEnabled(result);
		return null;
	}

	private void OnSetIsBlockParallelProcessingEnabledCompleted(object state)
	{
		if (this.SetIsBlockParallelProcessingEnabledCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetIsBlockParallelProcessingEnabledCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetIsBlockParallelProcessingEnabledAsync(bool enabled)
	{
		SetIsBlockParallelProcessingEnabledAsync(enabled, null);
	}

	public void SetIsBlockParallelProcessingEnabledAsync(bool enabled, object userState)
	{
		if (onBeginSetIsBlockParallelProcessingEnabledDelegate == null)
		{
			onBeginSetIsBlockParallelProcessingEnabledDelegate = OnBeginSetIsBlockParallelProcessingEnabled;
		}
		if (onEndSetIsBlockParallelProcessingEnabledDelegate == null)
		{
			onEndSetIsBlockParallelProcessingEnabledDelegate = OnEndSetIsBlockParallelProcessingEnabled;
		}
		if (onSetIsBlockParallelProcessingEnabledCompletedDelegate == null)
		{
			onSetIsBlockParallelProcessingEnabledCompletedDelegate = OnSetIsBlockParallelProcessingEnabledCompleted;
		}
		InvokeAsync(onBeginSetIsBlockParallelProcessingEnabledDelegate, new object[1] { enabled }, onEndSetIsBlockParallelProcessingEnabledDelegate, onSetIsBlockParallelProcessingEnabledCompletedDelegate, userState);
	}

	public void SetCAMModeState(bool CAMModeOn)
	{
		base.Channel.SetCAMModeState(CAMModeOn);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetCAMModeState(bool CAMModeOn, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetCAMModeState(CAMModeOn, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetCAMModeState(IAsyncResult result)
	{
		base.Channel.EndSetCAMModeState(result);
	}

	private IAsyncResult OnBeginSetCAMModeState(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool cAMModeOn = (bool)inValues[0];
		return BeginSetCAMModeState(cAMModeOn, callback, asyncState);
	}

	private object[] OnEndSetCAMModeState(IAsyncResult result)
	{
		EndSetCAMModeState(result);
		return null;
	}

	private void OnSetCAMModeStateCompleted(object state)
	{
		if (this.SetCAMModeStateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetCAMModeStateCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetCAMModeStateAsync(bool CAMModeOn)
	{
		SetCAMModeStateAsync(CAMModeOn, null);
	}

	public void SetCAMModeStateAsync(bool CAMModeOn, object userState)
	{
		if (onBeginSetCAMModeStateDelegate == null)
		{
			onBeginSetCAMModeStateDelegate = OnBeginSetCAMModeState;
		}
		if (onEndSetCAMModeStateDelegate == null)
		{
			onEndSetCAMModeStateDelegate = OnEndSetCAMModeState;
		}
		if (onSetCAMModeStateCompletedDelegate == null)
		{
			onSetCAMModeStateCompletedDelegate = OnSetCAMModeStateCompleted;
		}
		InvokeAsync(onBeginSetCAMModeStateDelegate, new object[1] { CAMModeOn }, onEndSetCAMModeStateDelegate, onSetCAMModeStateCompletedDelegate, userState);
	}

	public void SetComponentVisibility(ComponentsEnum componentsEnum, bool visible)
	{
		base.Channel.SetComponentVisibility(componentsEnum, visible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetComponentVisibility(ComponentsEnum componentsEnum, bool visible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetComponentVisibility(componentsEnum, visible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetComponentVisibility(IAsyncResult result)
	{
		base.Channel.EndSetComponentVisibility(result);
	}

	private IAsyncResult OnBeginSetComponentVisibility(object[] inValues, AsyncCallback callback, object asyncState)
	{
		ComponentsEnum componentsEnum = (ComponentsEnum)inValues[0];
		bool visible = (bool)inValues[1];
		return BeginSetComponentVisibility(componentsEnum, visible, callback, asyncState);
	}

	private object[] OnEndSetComponentVisibility(IAsyncResult result)
	{
		EndSetComponentVisibility(result);
		return null;
	}

	private void OnSetComponentVisibilityCompleted(object state)
	{
		if (this.SetComponentVisibilityCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetComponentVisibilityCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetComponentVisibilityAsync(ComponentsEnum componentsEnum, bool visible)
	{
		SetComponentVisibilityAsync(componentsEnum, visible, null);
	}

	public void SetComponentVisibilityAsync(ComponentsEnum componentsEnum, bool visible, object userState)
	{
		if (onBeginSetComponentVisibilityDelegate == null)
		{
			onBeginSetComponentVisibilityDelegate = OnBeginSetComponentVisibility;
		}
		if (onEndSetComponentVisibilityDelegate == null)
		{
			onEndSetComponentVisibilityDelegate = OnEndSetComponentVisibility;
		}
		if (onSetComponentVisibilityCompletedDelegate == null)
		{
			onSetComponentVisibilityCompletedDelegate = OnSetComponentVisibilityCompleted;
		}
		InvokeAsync(onBeginSetComponentVisibilityDelegate, new object[2] { componentsEnum, visible }, onEndSetComponentVisibilityDelegate, onSetComponentVisibilityCompletedDelegate, userState);
	}

	public UIControl[] SetDesignIteration(int iterationId)
	{
		return base.Channel.SetDesignIteration(iterationId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetDesignIteration(int iterationId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetDesignIteration(iterationId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndSetDesignIteration(IAsyncResult result)
	{
		return base.Channel.EndSetDesignIteration(result);
	}

	private IAsyncResult OnBeginSetDesignIteration(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int iterationId = (int)inValues[0];
		return BeginSetDesignIteration(iterationId, callback, asyncState);
	}

	private object[] OnEndSetDesignIteration(IAsyncResult result)
	{
		UIControl[] array = EndSetDesignIteration(result);
		return new object[1] { array };
	}

	private void OnSetDesignIterationCompleted(object state)
	{
		if (this.SetDesignIterationCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetDesignIterationCompleted(this, new SetDesignIterationCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetDesignIterationAsync(int iterationId)
	{
		SetDesignIterationAsync(iterationId, null);
	}

	public void SetDesignIterationAsync(int iterationId, object userState)
	{
		if (onBeginSetDesignIterationDelegate == null)
		{
			onBeginSetDesignIterationDelegate = OnBeginSetDesignIteration;
		}
		if (onEndSetDesignIterationDelegate == null)
		{
			onEndSetDesignIterationDelegate = OnEndSetDesignIteration;
		}
		if (onSetDesignIterationCompletedDelegate == null)
		{
			onSetDesignIterationCompletedDelegate = OnSetDesignIterationCompleted;
		}
		InvokeAsync(onBeginSetDesignIterationDelegate, new object[1] { iterationId }, onEndSetDesignIterationDelegate, onSetDesignIterationCompletedDelegate, userState);
	}

	public void SetGemMaterials(GemMaterial[] gemMaterials)
	{
		base.Channel.SetGemMaterials(gemMaterials);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetGemMaterials(GemMaterial[] gemMaterials, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetGemMaterials(gemMaterials, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetGemMaterials(IAsyncResult result)
	{
		base.Channel.EndSetGemMaterials(result);
	}

	private IAsyncResult OnBeginSetGemMaterials(object[] inValues, AsyncCallback callback, object asyncState)
	{
		GemMaterial[] gemMaterials = (GemMaterial[])inValues[0];
		return BeginSetGemMaterials(gemMaterials, callback, asyncState);
	}

	private object[] OnEndSetGemMaterials(IAsyncResult result)
	{
		EndSetGemMaterials(result);
		return null;
	}

	private void OnSetGemMaterialsCompleted(object state)
	{
		if (this.SetGemMaterialsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetGemMaterialsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetGemMaterialsAsync(GemMaterial[] gemMaterials)
	{
		SetGemMaterialsAsync(gemMaterials, null);
	}

	public void SetGemMaterialsAsync(GemMaterial[] gemMaterials, object userState)
	{
		if (onBeginSetGemMaterialsDelegate == null)
		{
			onBeginSetGemMaterialsDelegate = OnBeginSetGemMaterials;
		}
		if (onEndSetGemMaterialsDelegate == null)
		{
			onEndSetGemMaterialsDelegate = OnEndSetGemMaterials;
		}
		if (onSetGemMaterialsCompletedDelegate == null)
		{
			onSetGemMaterialsCompletedDelegate = OnSetGemMaterialsCompleted;
		}
		InvokeAsync(onBeginSetGemMaterialsDelegate, new object[1] { gemMaterials }, onEndSetGemMaterialsDelegate, onSetGemMaterialsCompletedDelegate, userState);
	}

	public void SetImportGroupToPrimaryMetal(Guid[] importGroupIdentifiers)
	{
		base.Channel.SetImportGroupToPrimaryMetal(importGroupIdentifiers);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetImportGroupToPrimaryMetal(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetImportGroupToPrimaryMetal(importGroupIdentifiers, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetImportGroupToPrimaryMetal(IAsyncResult result)
	{
		base.Channel.EndSetImportGroupToPrimaryMetal(result);
	}

	private IAsyncResult OnBeginSetImportGroupToPrimaryMetal(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiers = (Guid[])inValues[0];
		return BeginSetImportGroupToPrimaryMetal(importGroupIdentifiers, callback, asyncState);
	}

	private object[] OnEndSetImportGroupToPrimaryMetal(IAsyncResult result)
	{
		EndSetImportGroupToPrimaryMetal(result);
		return null;
	}

	private void OnSetImportGroupToPrimaryMetalCompleted(object state)
	{
		if (this.SetImportGroupToPrimaryMetalCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetImportGroupToPrimaryMetalCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetImportGroupToPrimaryMetalAsync(Guid[] importGroupIdentifiers)
	{
		SetImportGroupToPrimaryMetalAsync(importGroupIdentifiers, null);
	}

	public void SetImportGroupToPrimaryMetalAsync(Guid[] importGroupIdentifiers, object userState)
	{
		if (onBeginSetImportGroupToPrimaryMetalDelegate == null)
		{
			onBeginSetImportGroupToPrimaryMetalDelegate = OnBeginSetImportGroupToPrimaryMetal;
		}
		if (onEndSetImportGroupToPrimaryMetalDelegate == null)
		{
			onEndSetImportGroupToPrimaryMetalDelegate = OnEndSetImportGroupToPrimaryMetal;
		}
		if (onSetImportGroupToPrimaryMetalCompletedDelegate == null)
		{
			onSetImportGroupToPrimaryMetalCompletedDelegate = OnSetImportGroupToPrimaryMetalCompleted;
		}
		InvokeAsync(onBeginSetImportGroupToPrimaryMetalDelegate, new object[1] { importGroupIdentifiers }, onEndSetImportGroupToPrimaryMetalDelegate, onSetImportGroupToPrimaryMetalCompletedDelegate, userState);
	}

	public void SetImportGroupToSecondaryMetal(Guid[] importGroupIdentifiers)
	{
		base.Channel.SetImportGroupToSecondaryMetal(importGroupIdentifiers);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetImportGroupToSecondaryMetal(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetImportGroupToSecondaryMetal(importGroupIdentifiers, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetImportGroupToSecondaryMetal(IAsyncResult result)
	{
		base.Channel.EndSetImportGroupToSecondaryMetal(result);
	}

	private IAsyncResult OnBeginSetImportGroupToSecondaryMetal(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiers = (Guid[])inValues[0];
		return BeginSetImportGroupToSecondaryMetal(importGroupIdentifiers, callback, asyncState);
	}

	private object[] OnEndSetImportGroupToSecondaryMetal(IAsyncResult result)
	{
		EndSetImportGroupToSecondaryMetal(result);
		return null;
	}

	private void OnSetImportGroupToSecondaryMetalCompleted(object state)
	{
		if (this.SetImportGroupToSecondaryMetalCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetImportGroupToSecondaryMetalCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetImportGroupToSecondaryMetalAsync(Guid[] importGroupIdentifiers)
	{
		SetImportGroupToSecondaryMetalAsync(importGroupIdentifiers, null);
	}

	public void SetImportGroupToSecondaryMetalAsync(Guid[] importGroupIdentifiers, object userState)
	{
		if (onBeginSetImportGroupToSecondaryMetalDelegate == null)
		{
			onBeginSetImportGroupToSecondaryMetalDelegate = OnBeginSetImportGroupToSecondaryMetal;
		}
		if (onEndSetImportGroupToSecondaryMetalDelegate == null)
		{
			onEndSetImportGroupToSecondaryMetalDelegate = OnEndSetImportGroupToSecondaryMetal;
		}
		if (onSetImportGroupToSecondaryMetalCompletedDelegate == null)
		{
			onSetImportGroupToSecondaryMetalCompletedDelegate = OnSetImportGroupToSecondaryMetalCompleted;
		}
		InvokeAsync(onBeginSetImportGroupToSecondaryMetalDelegate, new object[1] { importGroupIdentifiers }, onEndSetImportGroupToSecondaryMetalDelegate, onSetImportGroupToSecondaryMetalCompletedDelegate, userState);
	}

	public void SetImportGroupVisibilityByIdentifier(Guid[] importGroupIdentifiers, bool isViewportVisible)
	{
		base.Channel.SetImportGroupVisibilityByIdentifier(importGroupIdentifiers, isViewportVisible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetImportGroupVisibilityByIdentifier(Guid[] importGroupIdentifiers, bool isViewportVisible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetImportGroupVisibilityByIdentifier(importGroupIdentifiers, isViewportVisible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetImportGroupVisibilityByIdentifier(IAsyncResult result)
	{
		base.Channel.EndSetImportGroupVisibilityByIdentifier(result);
	}

	private IAsyncResult OnBeginSetImportGroupVisibilityByIdentifier(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiers = (Guid[])inValues[0];
		bool isViewportVisible = (bool)inValues[1];
		return BeginSetImportGroupVisibilityByIdentifier(importGroupIdentifiers, isViewportVisible, callback, asyncState);
	}

	private object[] OnEndSetImportGroupVisibilityByIdentifier(IAsyncResult result)
	{
		EndSetImportGroupVisibilityByIdentifier(result);
		return null;
	}

	private void OnSetImportGroupVisibilityByIdentifierCompleted(object state)
	{
		if (this.SetImportGroupVisibilityByIdentifierCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetImportGroupVisibilityByIdentifierCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetImportGroupVisibilityByIdentifierAsync(Guid[] importGroupIdentifiers, bool isViewportVisible)
	{
		SetImportGroupVisibilityByIdentifierAsync(importGroupIdentifiers, isViewportVisible, null);
	}

	public void SetImportGroupVisibilityByIdentifierAsync(Guid[] importGroupIdentifiers, bool isViewportVisible, object userState)
	{
		if (onBeginSetImportGroupVisibilityByIdentifierDelegate == null)
		{
			onBeginSetImportGroupVisibilityByIdentifierDelegate = OnBeginSetImportGroupVisibilityByIdentifier;
		}
		if (onEndSetImportGroupVisibilityByIdentifierDelegate == null)
		{
			onEndSetImportGroupVisibilityByIdentifierDelegate = OnEndSetImportGroupVisibilityByIdentifier;
		}
		if (onSetImportGroupVisibilityByIdentifierCompletedDelegate == null)
		{
			onSetImportGroupVisibilityByIdentifierCompletedDelegate = OnSetImportGroupVisibilityByIdentifierCompleted;
		}
		InvokeAsync(onBeginSetImportGroupVisibilityByIdentifierDelegate, new object[2] { importGroupIdentifiers, isViewportVisible }, onEndSetImportGroupVisibilityByIdentifierDelegate, onSetImportGroupVisibilityByIdentifierCompletedDelegate, userState);
	}

	public int SetKeepAliveProcessIds(int id)
	{
		return base.Channel.SetKeepAliveProcessIds(id);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetKeepAliveProcessIds(int id, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetKeepAliveProcessIds(id, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int EndSetKeepAliveProcessIds(IAsyncResult result)
	{
		return base.Channel.EndSetKeepAliveProcessIds(result);
	}

	private IAsyncResult OnBeginSetKeepAliveProcessIds(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int id = (int)inValues[0];
		return BeginSetKeepAliveProcessIds(id, callback, asyncState);
	}

	private object[] OnEndSetKeepAliveProcessIds(IAsyncResult result)
	{
		int num = EndSetKeepAliveProcessIds(result);
		return new object[1] { num };
	}

	private void OnSetKeepAliveProcessIdsCompleted(object state)
	{
		if (this.SetKeepAliveProcessIdsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetKeepAliveProcessIdsCompleted(this, new SetKeepAliveProcessIdsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetKeepAliveProcessIdsAsync(int id)
	{
		SetKeepAliveProcessIdsAsync(id, null);
	}

	public void SetKeepAliveProcessIdsAsync(int id, object userState)
	{
		if (onBeginSetKeepAliveProcessIdsDelegate == null)
		{
			onBeginSetKeepAliveProcessIdsDelegate = OnBeginSetKeepAliveProcessIds;
		}
		if (onEndSetKeepAliveProcessIdsDelegate == null)
		{
			onEndSetKeepAliveProcessIdsDelegate = OnEndSetKeepAliveProcessIds;
		}
		if (onSetKeepAliveProcessIdsCompletedDelegate == null)
		{
			onSetKeepAliveProcessIdsCompletedDelegate = OnSetKeepAliveProcessIdsCompleted;
		}
		InvokeAsync(onBeginSetKeepAliveProcessIdsDelegate, new object[1] { id }, onEndSetKeepAliveProcessIdsDelegate, onSetKeepAliveProcessIdsCompletedDelegate, userState);
	}

	public void SetLoadedDialogState(bool bState)
	{
		base.Channel.SetLoadedDialogState(bState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetLoadedDialogState(bool bState, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetLoadedDialogState(bState, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetLoadedDialogState(IAsyncResult result)
	{
		base.Channel.EndSetLoadedDialogState(result);
	}

	private IAsyncResult OnBeginSetLoadedDialogState(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool bState = (bool)inValues[0];
		return BeginSetLoadedDialogState(bState, callback, asyncState);
	}

	private object[] OnEndSetLoadedDialogState(IAsyncResult result)
	{
		EndSetLoadedDialogState(result);
		return null;
	}

	private void OnSetLoadedDialogStateCompleted(object state)
	{
		if (this.SetLoadedDialogStateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetLoadedDialogStateCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetLoadedDialogStateAsync(bool bState)
	{
		SetLoadedDialogStateAsync(bState, null);
	}

	public void SetLoadedDialogStateAsync(bool bState, object userState)
	{
		if (onBeginSetLoadedDialogStateDelegate == null)
		{
			onBeginSetLoadedDialogStateDelegate = OnBeginSetLoadedDialogState;
		}
		if (onEndSetLoadedDialogStateDelegate == null)
		{
			onEndSetLoadedDialogStateDelegate = OnEndSetLoadedDialogState;
		}
		if (onSetLoadedDialogStateCompletedDelegate == null)
		{
			onSetLoadedDialogStateCompletedDelegate = OnSetLoadedDialogStateCompleted;
		}
		InvokeAsync(onBeginSetLoadedDialogStateDelegate, new object[1] { bState }, onEndSetLoadedDialogStateDelegate, onSetLoadedDialogStateCompletedDelegate, userState);
	}

	public bool SetMeshFromComponent(out string sError, ComponentsEnum[] whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed)
	{
		return base.Channel.SetMeshFromComponent(out sError, whichComponents, isCenterGemAllowed, isSideGemAllowed, isAccentGemAllowed);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetMeshFromComponent(ComponentsEnum[] whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetMeshFromComponent(whichComponents, isCenterGemAllowed, isSideGemAllowed, isAccentGemAllowed, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndSetMeshFromComponent(out string sError, IAsyncResult result)
	{
		return base.Channel.EndSetMeshFromComponent(out sError, result);
	}

	private IAsyncResult OnBeginSetMeshFromComponent(object[] inValues, AsyncCallback callback, object asyncState)
	{
		ComponentsEnum[] whichComponents = (ComponentsEnum[])inValues[0];
		bool isCenterGemAllowed = (bool)inValues[1];
		bool isSideGemAllowed = (bool)inValues[2];
		bool isAccentGemAllowed = (bool)inValues[3];
		return BeginSetMeshFromComponent(whichComponents, isCenterGemAllowed, isSideGemAllowed, isAccentGemAllowed, callback, asyncState);
	}

	private object[] OnEndSetMeshFromComponent(IAsyncResult result)
	{
		string sError = GetDefaultValueForInitialization<string>();
		bool flag = EndSetMeshFromComponent(out sError, result);
		return new object[2] { sError, flag };
	}

	private void OnSetMeshFromComponentCompleted(object state)
	{
		if (this.SetMeshFromComponentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetMeshFromComponentCompleted(this, new SetMeshFromComponentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetMeshFromComponentAsync(ComponentsEnum[] whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed)
	{
		SetMeshFromComponentAsync(whichComponents, isCenterGemAllowed, isSideGemAllowed, isAccentGemAllowed, null);
	}

	public void SetMeshFromComponentAsync(ComponentsEnum[] whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed, object userState)
	{
		if (onBeginSetMeshFromComponentDelegate == null)
		{
			onBeginSetMeshFromComponentDelegate = OnBeginSetMeshFromComponent;
		}
		if (onEndSetMeshFromComponentDelegate == null)
		{
			onEndSetMeshFromComponentDelegate = OnEndSetMeshFromComponent;
		}
		if (onSetMeshFromComponentCompletedDelegate == null)
		{
			onSetMeshFromComponentCompletedDelegate = OnSetMeshFromComponentCompleted;
		}
		InvokeAsync(onBeginSetMeshFromComponentDelegate, new object[4] { whichComponents, isCenterGemAllowed, isSideGemAllowed, isAccentGemAllowed }, onEndSetMeshFromComponentDelegate, onSetMeshFromComponentCompletedDelegate, userState);
	}

	public void SetMetalMaterials(MetalMaterial[] metalMaterials)
	{
		base.Channel.SetMetalMaterials(metalMaterials);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetMetalMaterials(MetalMaterial[] metalMaterials, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetMetalMaterials(metalMaterials, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetMetalMaterials(IAsyncResult result)
	{
		base.Channel.EndSetMetalMaterials(result);
	}

	private IAsyncResult OnBeginSetMetalMaterials(object[] inValues, AsyncCallback callback, object asyncState)
	{
		MetalMaterial[] metalMaterials = (MetalMaterial[])inValues[0];
		return BeginSetMetalMaterials(metalMaterials, callback, asyncState);
	}

	private object[] OnEndSetMetalMaterials(IAsyncResult result)
	{
		EndSetMetalMaterials(result);
		return null;
	}

	private void OnSetMetalMaterialsCompleted(object state)
	{
		if (this.SetMetalMaterialsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetMetalMaterialsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetMetalMaterialsAsync(MetalMaterial[] metalMaterials)
	{
		SetMetalMaterialsAsync(metalMaterials, null);
	}

	public void SetMetalMaterialsAsync(MetalMaterial[] metalMaterials, object userState)
	{
		if (onBeginSetMetalMaterialsDelegate == null)
		{
			onBeginSetMetalMaterialsDelegate = OnBeginSetMetalMaterials;
		}
		if (onEndSetMetalMaterialsDelegate == null)
		{
			onEndSetMetalMaterialsDelegate = OnEndSetMetalMaterials;
		}
		if (onSetMetalMaterialsCompletedDelegate == null)
		{
			onSetMetalMaterialsCompletedDelegate = OnSetMetalMaterialsCompleted;
		}
		InvokeAsync(onBeginSetMetalMaterialsDelegate, new object[1] { metalMaterials }, onEndSetMetalMaterialsDelegate, onSetMetalMaterialsCompletedDelegate, userState);
	}

	public void SetPerspectiveViewActiveAndMaximize()
	{
		base.Channel.SetPerspectiveViewActiveAndMaximize();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetPerspectiveViewActiveAndMaximize(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetPerspectiveViewActiveAndMaximize(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetPerspectiveViewActiveAndMaximize(IAsyncResult result)
	{
		base.Channel.EndSetPerspectiveViewActiveAndMaximize(result);
	}

	private IAsyncResult OnBeginSetPerspectiveViewActiveAndMaximize(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginSetPerspectiveViewActiveAndMaximize(callback, asyncState);
	}

	private object[] OnEndSetPerspectiveViewActiveAndMaximize(IAsyncResult result)
	{
		EndSetPerspectiveViewActiveAndMaximize(result);
		return null;
	}

	private void OnSetPerspectiveViewActiveAndMaximizeCompleted(object state)
	{
		if (this.SetPerspectiveViewActiveAndMaximizeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetPerspectiveViewActiveAndMaximizeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetPerspectiveViewActiveAndMaximizeAsync()
	{
		SetPerspectiveViewActiveAndMaximizeAsync(null);
	}

	public void SetPerspectiveViewActiveAndMaximizeAsync(object userState)
	{
		if (onBeginSetPerspectiveViewActiveAndMaximizeDelegate == null)
		{
			onBeginSetPerspectiveViewActiveAndMaximizeDelegate = OnBeginSetPerspectiveViewActiveAndMaximize;
		}
		if (onEndSetPerspectiveViewActiveAndMaximizeDelegate == null)
		{
			onEndSetPerspectiveViewActiveAndMaximizeDelegate = OnEndSetPerspectiveViewActiveAndMaximize;
		}
		if (onSetPerspectiveViewActiveAndMaximizeCompletedDelegate == null)
		{
			onSetPerspectiveViewActiveAndMaximizeCompletedDelegate = OnSetPerspectiveViewActiveAndMaximizeCompleted;
		}
		InvokeAsync(onBeginSetPerspectiveViewActiveAndMaximizeDelegate, null, onEndSetPerspectiveViewActiveAndMaximizeDelegate, onSetPerspectiveViewActiveAndMaximizeCompletedDelegate, userState);
	}

	public void SetSupportDensity(int iSupportDensity)
	{
		base.Channel.SetSupportDensity(iSupportDensity);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetSupportDensity(int iSupportDensity, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetSupportDensity(iSupportDensity, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetSupportDensity(IAsyncResult result)
	{
		base.Channel.EndSetSupportDensity(result);
	}

	private IAsyncResult OnBeginSetSupportDensity(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int iSupportDensity = (int)inValues[0];
		return BeginSetSupportDensity(iSupportDensity, callback, asyncState);
	}

	private object[] OnEndSetSupportDensity(IAsyncResult result)
	{
		EndSetSupportDensity(result);
		return null;
	}

	private void OnSetSupportDensityCompleted(object state)
	{
		if (this.SetSupportDensityCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetSupportDensityCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetSupportDensityAsync(int iSupportDensity)
	{
		SetSupportDensityAsync(iSupportDensity, null);
	}

	public void SetSupportDensityAsync(int iSupportDensity, object userState)
	{
		if (onBeginSetSupportDensityDelegate == null)
		{
			onBeginSetSupportDensityDelegate = OnBeginSetSupportDensity;
		}
		if (onEndSetSupportDensityDelegate == null)
		{
			onEndSetSupportDensityDelegate = OnEndSetSupportDensity;
		}
		if (onSetSupportDensityCompletedDelegate == null)
		{
			onSetSupportDensityCompletedDelegate = OnSetSupportDensityCompleted;
		}
		InvokeAsync(onBeginSetSupportDensityDelegate, new object[1] { iSupportDensity }, onEndSetSupportDensityDelegate, onSetSupportDensityCompletedDelegate, userState);
	}

	public bool SetSupportScales(double SupScale, int SupType)
	{
		return base.Channel.SetSupportScales(SupScale, SupType);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetSupportScales(double SupScale, int SupType, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetSupportScales(SupScale, SupType, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndSetSupportScales(IAsyncResult result)
	{
		return base.Channel.EndSetSupportScales(result);
	}

	private IAsyncResult OnBeginSetSupportScales(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double supScale = (double)inValues[0];
		int supType = (int)inValues[1];
		return BeginSetSupportScales(supScale, supType, callback, asyncState);
	}

	private object[] OnEndSetSupportScales(IAsyncResult result)
	{
		bool flag = EndSetSupportScales(result);
		return new object[1] { flag };
	}

	private void OnSetSupportScalesCompleted(object state)
	{
		if (this.SetSupportScalesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetSupportScalesCompleted(this, new SetSupportScalesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetSupportScalesAsync(double SupScale, int SupType)
	{
		SetSupportScalesAsync(SupScale, SupType, null);
	}

	public void SetSupportScalesAsync(double SupScale, int SupType, object userState)
	{
		if (onBeginSetSupportScalesDelegate == null)
		{
			onBeginSetSupportScalesDelegate = OnBeginSetSupportScales;
		}
		if (onEndSetSupportScalesDelegate == null)
		{
			onEndSetSupportScalesDelegate = OnEndSetSupportScales;
		}
		if (onSetSupportScalesCompletedDelegate == null)
		{
			onSetSupportScalesCompletedDelegate = OnSetSupportScalesCompleted;
		}
		InvokeAsync(onBeginSetSupportScalesDelegate, new object[2] { SupScale, SupType }, onEndSetSupportScalesDelegate, onSetSupportScalesCompletedDelegate, userState);
	}

	public void SetUnifiedMeshLocation(double dHeight)
	{
		base.Channel.SetUnifiedMeshLocation(dHeight);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetUnifiedMeshLocation(double dHeight, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetUnifiedMeshLocation(dHeight, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetUnifiedMeshLocation(IAsyncResult result)
	{
		base.Channel.EndSetUnifiedMeshLocation(result);
	}

	private IAsyncResult OnBeginSetUnifiedMeshLocation(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double dHeight = (double)inValues[0];
		return BeginSetUnifiedMeshLocation(dHeight, callback, asyncState);
	}

	private object[] OnEndSetUnifiedMeshLocation(IAsyncResult result)
	{
		EndSetUnifiedMeshLocation(result);
		return null;
	}

	private void OnSetUnifiedMeshLocationCompleted(object state)
	{
		if (this.SetUnifiedMeshLocationCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetUnifiedMeshLocationCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetUnifiedMeshLocationAsync(double dHeight)
	{
		SetUnifiedMeshLocationAsync(dHeight, null);
	}

	public void SetUnifiedMeshLocationAsync(double dHeight, object userState)
	{
		if (onBeginSetUnifiedMeshLocationDelegate == null)
		{
			onBeginSetUnifiedMeshLocationDelegate = OnBeginSetUnifiedMeshLocation;
		}
		if (onEndSetUnifiedMeshLocationDelegate == null)
		{
			onEndSetUnifiedMeshLocationDelegate = OnEndSetUnifiedMeshLocation;
		}
		if (onSetUnifiedMeshLocationCompletedDelegate == null)
		{
			onSetUnifiedMeshLocationCompletedDelegate = OnSetUnifiedMeshLocationCompleted;
		}
		InvokeAsync(onBeginSetUnifiedMeshLocationDelegate, new object[1] { dHeight }, onEndSetUnifiedMeshLocationDelegate, onSetUnifiedMeshLocationCompletedDelegate, userState);
	}

	public void SetUnifiedMeshRotation(double angleX, double angleY, double angleZ)
	{
		base.Channel.SetUnifiedMeshRotation(angleX, angleY, angleZ);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginSetUnifiedMeshRotation(double angleX, double angleY, double angleZ, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginSetUnifiedMeshRotation(angleX, angleY, angleZ, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndSetUnifiedMeshRotation(IAsyncResult result)
	{
		base.Channel.EndSetUnifiedMeshRotation(result);
	}

	private IAsyncResult OnBeginSetUnifiedMeshRotation(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double angleX = (double)inValues[0];
		double angleY = (double)inValues[1];
		double angleZ = (double)inValues[2];
		return BeginSetUnifiedMeshRotation(angleX, angleY, angleZ, callback, asyncState);
	}

	private object[] OnEndSetUnifiedMeshRotation(IAsyncResult result)
	{
		EndSetUnifiedMeshRotation(result);
		return null;
	}

	private void OnSetUnifiedMeshRotationCompleted(object state)
	{
		if (this.SetUnifiedMeshRotationCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.SetUnifiedMeshRotationCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void SetUnifiedMeshRotationAsync(double angleX, double angleY, double angleZ)
	{
		SetUnifiedMeshRotationAsync(angleX, angleY, angleZ, null);
	}

	public void SetUnifiedMeshRotationAsync(double angleX, double angleY, double angleZ, object userState)
	{
		if (onBeginSetUnifiedMeshRotationDelegate == null)
		{
			onBeginSetUnifiedMeshRotationDelegate = OnBeginSetUnifiedMeshRotation;
		}
		if (onEndSetUnifiedMeshRotationDelegate == null)
		{
			onEndSetUnifiedMeshRotationDelegate = OnEndSetUnifiedMeshRotation;
		}
		if (onSetUnifiedMeshRotationCompletedDelegate == null)
		{
			onSetUnifiedMeshRotationCompletedDelegate = OnSetUnifiedMeshRotationCompleted;
		}
		InvokeAsync(onBeginSetUnifiedMeshRotationDelegate, new object[3] { angleX, angleY, angleZ }, onEndSetUnifiedMeshRotationDelegate, onSetUnifiedMeshRotationCompletedDelegate, userState);
	}

	public void Shutdown()
	{
		base.Channel.Shutdown();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginShutdown(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginShutdown(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndShutdown(IAsyncResult result)
	{
		base.Channel.EndShutdown(result);
	}

	private IAsyncResult OnBeginShutdown(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginShutdown(callback, asyncState);
	}

	private object[] OnEndShutdown(IAsyncResult result)
	{
		EndShutdown(result);
		return null;
	}

	private void OnShutdownCompleted(object state)
	{
		if (this.ShutdownCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ShutdownCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ShutdownAsync()
	{
		ShutdownAsync(null);
	}

	public void ShutdownAsync(object userState)
	{
		if (onBeginShutdownDelegate == null)
		{
			onBeginShutdownDelegate = OnBeginShutdown;
		}
		if (onEndShutdownDelegate == null)
		{
			onEndShutdownDelegate = OnEndShutdown;
		}
		if (onShutdownCompletedDelegate == null)
		{
			onShutdownCompletedDelegate = OnShutdownCompleted;
		}
		InvokeAsync(onBeginShutdownDelegate, null, onEndShutdownDelegate, onShutdownCompletedDelegate, userState);
	}

	public string AbortPrintJob()
	{
		return base.Channel.AbortPrintJob();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginAbortPrintJob(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginAbortPrintJob(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndAbortPrintJob(IAsyncResult result)
	{
		return base.Channel.EndAbortPrintJob(result);
	}

	private IAsyncResult OnBeginAbortPrintJob(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginAbortPrintJob(callback, asyncState);
	}

	private object[] OnEndAbortPrintJob(IAsyncResult result)
	{
		string text = EndAbortPrintJob(result);
		return new object[1] { text };
	}

	private void OnAbortPrintJobCompleted(object state)
	{
		if (this.AbortPrintJobCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.AbortPrintJobCompleted(this, new AbortPrintJobCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void AbortPrintJobAsync()
	{
		AbortPrintJobAsync(null);
	}

	public void AbortPrintJobAsync(object userState)
	{
		if (onBeginAbortPrintJobDelegate == null)
		{
			onBeginAbortPrintJobDelegate = OnBeginAbortPrintJob;
		}
		if (onEndAbortPrintJobDelegate == null)
		{
			onEndAbortPrintJobDelegate = OnEndAbortPrintJob;
		}
		if (onAbortPrintJobCompletedDelegate == null)
		{
			onAbortPrintJobCompletedDelegate = OnAbortPrintJobCompleted;
		}
		InvokeAsync(onBeginAbortPrintJobDelegate, null, onEndAbortPrintJobDelegate, onAbortPrintJobCompletedDelegate, userState);
	}

	public void AddPropToDisplay(string renderPropSwatchCollectionItem)
	{
		base.Channel.AddPropToDisplay(renderPropSwatchCollectionItem);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginAddPropToDisplay(string renderPropSwatchCollectionItem, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginAddPropToDisplay(renderPropSwatchCollectionItem, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndAddPropToDisplay(IAsyncResult result)
	{
		base.Channel.EndAddPropToDisplay(result);
	}

	private IAsyncResult OnBeginAddPropToDisplay(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string renderPropSwatchCollectionItem = (string)inValues[0];
		return BeginAddPropToDisplay(renderPropSwatchCollectionItem, callback, asyncState);
	}

	private object[] OnEndAddPropToDisplay(IAsyncResult result)
	{
		EndAddPropToDisplay(result);
		return null;
	}

	private void OnAddPropToDisplayCompleted(object state)
	{
		if (this.AddPropToDisplayCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.AddPropToDisplayCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void AddPropToDisplayAsync(string renderPropSwatchCollectionItem)
	{
		AddPropToDisplayAsync(renderPropSwatchCollectionItem, null);
	}

	public void AddPropToDisplayAsync(string renderPropSwatchCollectionItem, object userState)
	{
		if (onBeginAddPropToDisplayDelegate == null)
		{
			onBeginAddPropToDisplayDelegate = OnBeginAddPropToDisplay;
		}
		if (onEndAddPropToDisplayDelegate == null)
		{
			onEndAddPropToDisplayDelegate = OnEndAddPropToDisplay;
		}
		if (onAddPropToDisplayCompletedDelegate == null)
		{
			onAddPropToDisplayCompletedDelegate = OnAddPropToDisplayCompleted;
		}
		InvokeAsync(onBeginAddPropToDisplayDelegate, new object[1] { renderPropSwatchCollectionItem }, onEndAddPropToDisplayDelegate, onAddPropToDisplayCompletedDelegate, userState);
	}

	public bool AreSecondaryMetalObjectsVisible()
	{
		return base.Channel.AreSecondaryMetalObjectsVisible();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginAreSecondaryMetalObjectsVisible(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginAreSecondaryMetalObjectsVisible(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndAreSecondaryMetalObjectsVisible(IAsyncResult result)
	{
		return base.Channel.EndAreSecondaryMetalObjectsVisible(result);
	}

	private IAsyncResult OnBeginAreSecondaryMetalObjectsVisible(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginAreSecondaryMetalObjectsVisible(callback, asyncState);
	}

	private object[] OnEndAreSecondaryMetalObjectsVisible(IAsyncResult result)
	{
		bool flag = EndAreSecondaryMetalObjectsVisible(result);
		return new object[1] { flag };
	}

	private void OnAreSecondaryMetalObjectsVisibleCompleted(object state)
	{
		if (this.AreSecondaryMetalObjectsVisibleCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.AreSecondaryMetalObjectsVisibleCompleted(this, new AreSecondaryMetalObjectsVisibleCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void AreSecondaryMetalObjectsVisibleAsync()
	{
		AreSecondaryMetalObjectsVisibleAsync(null);
	}

	public void AreSecondaryMetalObjectsVisibleAsync(object userState)
	{
		if (onBeginAreSecondaryMetalObjectsVisibleDelegate == null)
		{
			onBeginAreSecondaryMetalObjectsVisibleDelegate = OnBeginAreSecondaryMetalObjectsVisible;
		}
		if (onEndAreSecondaryMetalObjectsVisibleDelegate == null)
		{
			onEndAreSecondaryMetalObjectsVisibleDelegate = OnEndAreSecondaryMetalObjectsVisible;
		}
		if (onAreSecondaryMetalObjectsVisibleCompletedDelegate == null)
		{
			onAreSecondaryMetalObjectsVisibleCompletedDelegate = OnAreSecondaryMetalObjectsVisibleCompleted;
		}
		InvokeAsync(onBeginAreSecondaryMetalObjectsVisibleDelegate, null, onEndAreSecondaryMetalObjectsVisibleDelegate, onAreSecondaryMetalObjectsVisibleCompletedDelegate, userState);
	}

	public bool AreSecondaryMetalObjectsVisibleForComponent(ComponentsEnum whichComponent)
	{
		return base.Channel.AreSecondaryMetalObjectsVisibleForComponent(whichComponent);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginAreSecondaryMetalObjectsVisibleForComponent(ComponentsEnum whichComponent, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginAreSecondaryMetalObjectsVisibleForComponent(whichComponent, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndAreSecondaryMetalObjectsVisibleForComponent(IAsyncResult result)
	{
		return base.Channel.EndAreSecondaryMetalObjectsVisibleForComponent(result);
	}

	private IAsyncResult OnBeginAreSecondaryMetalObjectsVisibleForComponent(object[] inValues, AsyncCallback callback, object asyncState)
	{
		ComponentsEnum whichComponent = (ComponentsEnum)inValues[0];
		return BeginAreSecondaryMetalObjectsVisibleForComponent(whichComponent, callback, asyncState);
	}

	private object[] OnEndAreSecondaryMetalObjectsVisibleForComponent(IAsyncResult result)
	{
		bool flag = EndAreSecondaryMetalObjectsVisibleForComponent(result);
		return new object[1] { flag };
	}

	private void OnAreSecondaryMetalObjectsVisibleForComponentCompleted(object state)
	{
		if (this.AreSecondaryMetalObjectsVisibleForComponentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.AreSecondaryMetalObjectsVisibleForComponentCompleted(this, new AreSecondaryMetalObjectsVisibleForComponentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void AreSecondaryMetalObjectsVisibleForComponentAsync(ComponentsEnum whichComponent)
	{
		AreSecondaryMetalObjectsVisibleForComponentAsync(whichComponent, null);
	}

	public void AreSecondaryMetalObjectsVisibleForComponentAsync(ComponentsEnum whichComponent, object userState)
	{
		if (onBeginAreSecondaryMetalObjectsVisibleForComponentDelegate == null)
		{
			onBeginAreSecondaryMetalObjectsVisibleForComponentDelegate = OnBeginAreSecondaryMetalObjectsVisibleForComponent;
		}
		if (onEndAreSecondaryMetalObjectsVisibleForComponentDelegate == null)
		{
			onEndAreSecondaryMetalObjectsVisibleForComponentDelegate = OnEndAreSecondaryMetalObjectsVisibleForComponent;
		}
		if (onAreSecondaryMetalObjectsVisibleForComponentCompletedDelegate == null)
		{
			onAreSecondaryMetalObjectsVisibleForComponentCompletedDelegate = OnAreSecondaryMetalObjectsVisibleForComponentCompleted;
		}
		InvokeAsync(onBeginAreSecondaryMetalObjectsVisibleForComponentDelegate, new object[1] { whichComponent }, onEndAreSecondaryMetalObjectsVisibleForComponentDelegate, onAreSecondaryMetalObjectsVisibleForComponentCompletedDelegate, userState);
	}

	public bool CheckConnectionToProJet1200(out string sError, out string sStatus, string connectionAddress)
	{
		return base.Channel.CheckConnectionToProJet1200(out sError, out sStatus, connectionAddress);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginCheckConnectionToProJet1200(string connectionAddress, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginCheckConnectionToProJet1200(connectionAddress, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndCheckConnectionToProJet1200(out string sError, out string sStatus, IAsyncResult result)
	{
		return base.Channel.EndCheckConnectionToProJet1200(out sError, out sStatus, result);
	}

	private IAsyncResult OnBeginCheckConnectionToProJet1200(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string connectionAddress = (string)inValues[0];
		return BeginCheckConnectionToProJet1200(connectionAddress, callback, asyncState);
	}

	private object[] OnEndCheckConnectionToProJet1200(IAsyncResult result)
	{
		string sError = GetDefaultValueForInitialization<string>();
		string sStatus = GetDefaultValueForInitialization<string>();
		bool flag = EndCheckConnectionToProJet1200(out sError, out sStatus, result);
		return new object[3] { sError, sStatus, flag };
	}

	private void OnCheckConnectionToProJet1200Completed(object state)
	{
		if (this.CheckConnectionToProJet1200Completed != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.CheckConnectionToProJet1200Completed(this, new CheckConnectionToProJet1200CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void CheckConnectionToProJet1200Async(string connectionAddress)
	{
		CheckConnectionToProJet1200Async(connectionAddress, null);
	}

	public void CheckConnectionToProJet1200Async(string connectionAddress, object userState)
	{
		if (onBeginCheckConnectionToProJet1200Delegate == null)
		{
			onBeginCheckConnectionToProJet1200Delegate = OnBeginCheckConnectionToProJet1200;
		}
		if (onEndCheckConnectionToProJet1200Delegate == null)
		{
			onEndCheckConnectionToProJet1200Delegate = OnEndCheckConnectionToProJet1200;
		}
		if (onCheckConnectionToProJet1200CompletedDelegate == null)
		{
			onCheckConnectionToProJet1200CompletedDelegate = OnCheckConnectionToProJet1200Completed;
		}
		InvokeAsync(onBeginCheckConnectionToProJet1200Delegate, new object[1] { connectionAddress }, onEndCheckConnectionToProJet1200Delegate, onCheckConnectionToProJet1200CompletedDelegate, userState);
	}

	public bool CheckFirmwareVersion()
	{
		return base.Channel.CheckFirmwareVersion();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginCheckFirmwareVersion(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginCheckFirmwareVersion(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndCheckFirmwareVersion(IAsyncResult result)
	{
		return base.Channel.EndCheckFirmwareVersion(result);
	}

	private IAsyncResult OnBeginCheckFirmwareVersion(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginCheckFirmwareVersion(callback, asyncState);
	}

	private object[] OnEndCheckFirmwareVersion(IAsyncResult result)
	{
		bool flag = EndCheckFirmwareVersion(result);
		return new object[1] { flag };
	}

	private void OnCheckFirmwareVersionCompleted(object state)
	{
		if (this.CheckFirmwareVersionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.CheckFirmwareVersionCompleted(this, new CheckFirmwareVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void CheckFirmwareVersionAsync()
	{
		CheckFirmwareVersionAsync(null);
	}

	public void CheckFirmwareVersionAsync(object userState)
	{
		if (onBeginCheckFirmwareVersionDelegate == null)
		{
			onBeginCheckFirmwareVersionDelegate = OnBeginCheckFirmwareVersion;
		}
		if (onEndCheckFirmwareVersionDelegate == null)
		{
			onEndCheckFirmwareVersionDelegate = OnEndCheckFirmwareVersion;
		}
		if (onCheckFirmwareVersionCompletedDelegate == null)
		{
			onCheckFirmwareVersionCompletedDelegate = OnCheckFirmwareVersionCompleted;
		}
		InvokeAsync(onBeginCheckFirmwareVersionDelegate, null, onEndCheckFirmwareVersionDelegate, onCheckFirmwareVersionCompletedDelegate, userState);
	}

	public void ClearExpandedSliders()
	{
		base.Channel.ClearExpandedSliders();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginClearExpandedSliders(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginClearExpandedSliders(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndClearExpandedSliders(IAsyncResult result)
	{
		base.Channel.EndClearExpandedSliders(result);
	}

	private IAsyncResult OnBeginClearExpandedSliders(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginClearExpandedSliders(callback, asyncState);
	}

	private object[] OnEndClearExpandedSliders(IAsyncResult result)
	{
		EndClearExpandedSliders(result);
		return null;
	}

	private void OnClearExpandedSlidersCompleted(object state)
	{
		if (this.ClearExpandedSlidersCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ClearExpandedSlidersCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ClearExpandedSlidersAsync()
	{
		ClearExpandedSlidersAsync(null);
	}

	public void ClearExpandedSlidersAsync(object userState)
	{
		if (onBeginClearExpandedSlidersDelegate == null)
		{
			onBeginClearExpandedSlidersDelegate = OnBeginClearExpandedSliders;
		}
		if (onEndClearExpandedSlidersDelegate == null)
		{
			onEndClearExpandedSlidersDelegate = OnEndClearExpandedSliders;
		}
		if (onClearExpandedSlidersCompletedDelegate == null)
		{
			onClearExpandedSlidersCompletedDelegate = OnClearExpandedSlidersCompleted;
		}
		InvokeAsync(onBeginClearExpandedSlidersDelegate, null, onEndClearExpandedSlidersDelegate, onClearExpandedSlidersCompletedDelegate, userState);
	}

	public void ClearHighlightedGems()
	{
		base.Channel.ClearHighlightedGems();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginClearHighlightedGems(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginClearHighlightedGems(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndClearHighlightedGems(IAsyncResult result)
	{
		base.Channel.EndClearHighlightedGems(result);
	}

	private IAsyncResult OnBeginClearHighlightedGems(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginClearHighlightedGems(callback, asyncState);
	}

	private object[] OnEndClearHighlightedGems(IAsyncResult result)
	{
		EndClearHighlightedGems(result);
		return null;
	}

	private void OnClearHighlightedGemsCompleted(object state)
	{
		if (this.ClearHighlightedGemsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ClearHighlightedGemsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ClearHighlightedGemsAsync()
	{
		ClearHighlightedGemsAsync(null);
	}

	public void ClearHighlightedGemsAsync(object userState)
	{
		if (onBeginClearHighlightedGemsDelegate == null)
		{
			onBeginClearHighlightedGemsDelegate = OnBeginClearHighlightedGems;
		}
		if (onEndClearHighlightedGemsDelegate == null)
		{
			onEndClearHighlightedGemsDelegate = OnEndClearHighlightedGems;
		}
		if (onClearHighlightedGemsCompletedDelegate == null)
		{
			onClearHighlightedGemsCompletedDelegate = OnClearHighlightedGemsCompleted;
		}
		InvokeAsync(onBeginClearHighlightedGemsDelegate, null, onEndClearHighlightedGemsDelegate, onClearHighlightedGemsCompletedDelegate, userState);
	}

	public bool ClearUnifiedMesh()
	{
		return base.Channel.ClearUnifiedMesh();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginClearUnifiedMesh(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginClearUnifiedMesh(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndClearUnifiedMesh(IAsyncResult result)
	{
		return base.Channel.EndClearUnifiedMesh(result);
	}

	private IAsyncResult OnBeginClearUnifiedMesh(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginClearUnifiedMesh(callback, asyncState);
	}

	private object[] OnEndClearUnifiedMesh(IAsyncResult result)
	{
		bool flag = EndClearUnifiedMesh(result);
		return new object[1] { flag };
	}

	private void OnClearUnifiedMeshCompleted(object state)
	{
		if (this.ClearUnifiedMeshCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ClearUnifiedMeshCompleted(this, new ClearUnifiedMeshCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ClearUnifiedMeshAsync()
	{
		ClearUnifiedMeshAsync(null);
	}

	public void ClearUnifiedMeshAsync(object userState)
	{
		if (onBeginClearUnifiedMeshDelegate == null)
		{
			onBeginClearUnifiedMeshDelegate = OnBeginClearUnifiedMesh;
		}
		if (onEndClearUnifiedMeshDelegate == null)
		{
			onEndClearUnifiedMeshDelegate = OnEndClearUnifiedMesh;
		}
		if (onClearUnifiedMeshCompletedDelegate == null)
		{
			onClearUnifiedMeshCompletedDelegate = OnClearUnifiedMeshCompleted;
		}
		InvokeAsync(onBeginClearUnifiedMeshDelegate, null, onEndClearUnifiedMeshDelegate, onClearUnifiedMeshCompletedDelegate, userState);
	}

	public void ConduitEnabled(bool isEnabled)
	{
		base.Channel.ConduitEnabled(isEnabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConduitEnabled(bool isEnabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConduitEnabled(isEnabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndConduitEnabled(IAsyncResult result)
	{
		base.Channel.EndConduitEnabled(result);
	}

	private IAsyncResult OnBeginConduitEnabled(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isEnabled = (bool)inValues[0];
		return BeginConduitEnabled(isEnabled, callback, asyncState);
	}

	private object[] OnEndConduitEnabled(IAsyncResult result)
	{
		EndConduitEnabled(result);
		return null;
	}

	private void OnConduitEnabledCompleted(object state)
	{
		if (this.ConduitEnabledCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConduitEnabledCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConduitEnabledAsync(bool isEnabled)
	{
		ConduitEnabledAsync(isEnabled, null);
	}

	public void ConduitEnabledAsync(bool isEnabled, object userState)
	{
		if (onBeginConduitEnabledDelegate == null)
		{
			onBeginConduitEnabledDelegate = OnBeginConduitEnabled;
		}
		if (onEndConduitEnabledDelegate == null)
		{
			onEndConduitEnabledDelegate = OnEndConduitEnabled;
		}
		if (onConduitEnabledCompletedDelegate == null)
		{
			onConduitEnabledCompletedDelegate = OnConduitEnabledCompleted;
		}
		InvokeAsync(onBeginConduitEnabledDelegate, new object[1] { isEnabled }, onEndConduitEnabledDelegate, onConduitEnabledCompletedDelegate, userState);
	}

	public void ConduitResetTryOn()
	{
		base.Channel.ConduitResetTryOn();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConduitResetTryOn(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConduitResetTryOn(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndConduitResetTryOn(IAsyncResult result)
	{
		base.Channel.EndConduitResetTryOn(result);
	}

	private IAsyncResult OnBeginConduitResetTryOn(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginConduitResetTryOn(callback, asyncState);
	}

	private object[] OnEndConduitResetTryOn(IAsyncResult result)
	{
		EndConduitResetTryOn(result);
		return null;
	}

	private void OnConduitResetTryOnCompleted(object state)
	{
		if (this.ConduitResetTryOnCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConduitResetTryOnCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConduitResetTryOnAsync()
	{
		ConduitResetTryOnAsync(null);
	}

	public void ConduitResetTryOnAsync(object userState)
	{
		if (onBeginConduitResetTryOnDelegate == null)
		{
			onBeginConduitResetTryOnDelegate = OnBeginConduitResetTryOn;
		}
		if (onEndConduitResetTryOnDelegate == null)
		{
			onEndConduitResetTryOnDelegate = OnEndConduitResetTryOn;
		}
		if (onConduitResetTryOnCompletedDelegate == null)
		{
			onConduitResetTryOnCompletedDelegate = OnConduitResetTryOnCompleted;
		}
		InvokeAsync(onBeginConduitResetTryOnDelegate, null, onEndConduitResetTryOnDelegate, onConduitResetTryOnCompletedDelegate, userState);
	}

	public void ConduitSetFingernailObjectMaterial(string fingernailPolishMaterialName)
	{
		base.Channel.ConduitSetFingernailObjectMaterial(fingernailPolishMaterialName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConduitSetFingernailObjectMaterial(string fingernailPolishMaterialName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConduitSetFingernailObjectMaterial(fingernailPolishMaterialName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndConduitSetFingernailObjectMaterial(IAsyncResult result)
	{
		base.Channel.EndConduitSetFingernailObjectMaterial(result);
	}

	private IAsyncResult OnBeginConduitSetFingernailObjectMaterial(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string fingernailPolishMaterialName = (string)inValues[0];
		return BeginConduitSetFingernailObjectMaterial(fingernailPolishMaterialName, callback, asyncState);
	}

	private object[] OnEndConduitSetFingernailObjectMaterial(IAsyncResult result)
	{
		EndConduitSetFingernailObjectMaterial(result);
		return null;
	}

	private void OnConduitSetFingernailObjectMaterialCompleted(object state)
	{
		if (this.ConduitSetFingernailObjectMaterialCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConduitSetFingernailObjectMaterialCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConduitSetFingernailObjectMaterialAsync(string fingernailPolishMaterialName)
	{
		ConduitSetFingernailObjectMaterialAsync(fingernailPolishMaterialName, null);
	}

	public void ConduitSetFingernailObjectMaterialAsync(string fingernailPolishMaterialName, object userState)
	{
		if (onBeginConduitSetFingernailObjectMaterialDelegate == null)
		{
			onBeginConduitSetFingernailObjectMaterialDelegate = OnBeginConduitSetFingernailObjectMaterial;
		}
		if (onEndConduitSetFingernailObjectMaterialDelegate == null)
		{
			onEndConduitSetFingernailObjectMaterialDelegate = OnEndConduitSetFingernailObjectMaterial;
		}
		if (onConduitSetFingernailObjectMaterialCompletedDelegate == null)
		{
			onConduitSetFingernailObjectMaterialCompletedDelegate = OnConduitSetFingernailObjectMaterialCompleted;
		}
		InvokeAsync(onBeginConduitSetFingernailObjectMaterialDelegate, new object[1] { fingernailPolishMaterialName }, onEndConduitSetFingernailObjectMaterialDelegate, onConduitSetFingernailObjectMaterialCompletedDelegate, userState);
	}

	public void ConduitSetFlipRing(bool flipRing)
	{
		base.Channel.ConduitSetFlipRing(flipRing);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConduitSetFlipRing(bool flipRing, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConduitSetFlipRing(flipRing, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndConduitSetFlipRing(IAsyncResult result)
	{
		base.Channel.EndConduitSetFlipRing(result);
	}

	private IAsyncResult OnBeginConduitSetFlipRing(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool flipRing = (bool)inValues[0];
		return BeginConduitSetFlipRing(flipRing, callback, asyncState);
	}

	private object[] OnEndConduitSetFlipRing(IAsyncResult result)
	{
		EndConduitSetFlipRing(result);
		return null;
	}

	private void OnConduitSetFlipRingCompleted(object state)
	{
		if (this.ConduitSetFlipRingCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConduitSetFlipRingCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConduitSetFlipRingAsync(bool flipRing)
	{
		ConduitSetFlipRingAsync(flipRing, null);
	}

	public void ConduitSetFlipRingAsync(bool flipRing, object userState)
	{
		if (onBeginConduitSetFlipRingDelegate == null)
		{
			onBeginConduitSetFlipRingDelegate = OnBeginConduitSetFlipRing;
		}
		if (onEndConduitSetFlipRingDelegate == null)
		{
			onEndConduitSetFlipRingDelegate = OnEndConduitSetFlipRing;
		}
		if (onConduitSetFlipRingCompletedDelegate == null)
		{
			onConduitSetFlipRingCompletedDelegate = OnConduitSetFlipRingCompleted;
		}
		InvokeAsync(onBeginConduitSetFlipRingDelegate, new object[1] { flipRing }, onEndConduitSetFlipRingDelegate, onConduitSetFlipRingCompletedDelegate, userState);
	}

	public void ConduitSetHandObjectMaterial(string handMaterialName)
	{
		base.Channel.ConduitSetHandObjectMaterial(handMaterialName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConduitSetHandObjectMaterial(string handMaterialName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConduitSetHandObjectMaterial(handMaterialName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndConduitSetHandObjectMaterial(IAsyncResult result)
	{
		base.Channel.EndConduitSetHandObjectMaterial(result);
	}

	private IAsyncResult OnBeginConduitSetHandObjectMaterial(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string handMaterialName = (string)inValues[0];
		return BeginConduitSetHandObjectMaterial(handMaterialName, callback, asyncState);
	}

	private object[] OnEndConduitSetHandObjectMaterial(IAsyncResult result)
	{
		EndConduitSetHandObjectMaterial(result);
		return null;
	}

	private void OnConduitSetHandObjectMaterialCompleted(object state)
	{
		if (this.ConduitSetHandObjectMaterialCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConduitSetHandObjectMaterialCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConduitSetHandObjectMaterialAsync(string handMaterialName)
	{
		ConduitSetHandObjectMaterialAsync(handMaterialName, null);
	}

	public void ConduitSetHandObjectMaterialAsync(string handMaterialName, object userState)
	{
		if (onBeginConduitSetHandObjectMaterialDelegate == null)
		{
			onBeginConduitSetHandObjectMaterialDelegate = OnBeginConduitSetHandObjectMaterial;
		}
		if (onEndConduitSetHandObjectMaterialDelegate == null)
		{
			onEndConduitSetHandObjectMaterialDelegate = OnEndConduitSetHandObjectMaterial;
		}
		if (onConduitSetHandObjectMaterialCompletedDelegate == null)
		{
			onConduitSetHandObjectMaterialCompletedDelegate = OnConduitSetHandObjectMaterialCompleted;
		}
		InvokeAsync(onBeginConduitSetHandObjectMaterialDelegate, new object[1] { handMaterialName }, onEndConduitSetHandObjectMaterialDelegate, onConduitSetHandObjectMaterialCompletedDelegate, userState);
	}

	public void ConduitSetScaleToRingRailObjectFlip(bool flipObject)
	{
		base.Channel.ConduitSetScaleToRingRailObjectFlip(flipObject);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConduitSetScaleToRingRailObjectFlip(bool flipObject, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConduitSetScaleToRingRailObjectFlip(flipObject, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndConduitSetScaleToRingRailObjectFlip(IAsyncResult result)
	{
		base.Channel.EndConduitSetScaleToRingRailObjectFlip(result);
	}

	private IAsyncResult OnBeginConduitSetScaleToRingRailObjectFlip(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool flipObject = (bool)inValues[0];
		return BeginConduitSetScaleToRingRailObjectFlip(flipObject, callback, asyncState);
	}

	private object[] OnEndConduitSetScaleToRingRailObjectFlip(IAsyncResult result)
	{
		EndConduitSetScaleToRingRailObjectFlip(result);
		return null;
	}

	private void OnConduitSetScaleToRingRailObjectFlipCompleted(object state)
	{
		if (this.ConduitSetScaleToRingRailObjectFlipCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConduitSetScaleToRingRailObjectFlipCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConduitSetScaleToRingRailObjectFlipAsync(bool flipObject)
	{
		ConduitSetScaleToRingRailObjectFlipAsync(flipObject, null);
	}

	public void ConduitSetScaleToRingRailObjectFlipAsync(bool flipObject, object userState)
	{
		if (onBeginConduitSetScaleToRingRailObjectFlipDelegate == null)
		{
			onBeginConduitSetScaleToRingRailObjectFlipDelegate = OnBeginConduitSetScaleToRingRailObjectFlip;
		}
		if (onEndConduitSetScaleToRingRailObjectFlipDelegate == null)
		{
			onEndConduitSetScaleToRingRailObjectFlipDelegate = OnEndConduitSetScaleToRingRailObjectFlip;
		}
		if (onConduitSetScaleToRingRailObjectFlipCompletedDelegate == null)
		{
			onConduitSetScaleToRingRailObjectFlipCompletedDelegate = OnConduitSetScaleToRingRailObjectFlipCompleted;
		}
		InvokeAsync(onBeginConduitSetScaleToRingRailObjectFlipDelegate, new object[1] { flipObject }, onEndConduitSetScaleToRingRailObjectFlipDelegate, onConduitSetScaleToRingRailObjectFlipCompletedDelegate, userState);
	}

	public void ConduitSetScaleToRingRailYOffset(double ringOffset)
	{
		base.Channel.ConduitSetScaleToRingRailYOffset(ringOffset);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConduitSetScaleToRingRailYOffset(double ringOffset, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConduitSetScaleToRingRailYOffset(ringOffset, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndConduitSetScaleToRingRailYOffset(IAsyncResult result)
	{
		base.Channel.EndConduitSetScaleToRingRailYOffset(result);
	}

	private IAsyncResult OnBeginConduitSetScaleToRingRailYOffset(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double ringOffset = (double)inValues[0];
		return BeginConduitSetScaleToRingRailYOffset(ringOffset, callback, asyncState);
	}

	private object[] OnEndConduitSetScaleToRingRailYOffset(IAsyncResult result)
	{
		EndConduitSetScaleToRingRailYOffset(result);
		return null;
	}

	private void OnConduitSetScaleToRingRailYOffsetCompleted(object state)
	{
		if (this.ConduitSetScaleToRingRailYOffsetCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConduitSetScaleToRingRailYOffsetCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConduitSetScaleToRingRailYOffsetAsync(double ringOffset)
	{
		ConduitSetScaleToRingRailYOffsetAsync(ringOffset, null);
	}

	public void ConduitSetScaleToRingRailYOffsetAsync(double ringOffset, object userState)
	{
		if (onBeginConduitSetScaleToRingRailYOffsetDelegate == null)
		{
			onBeginConduitSetScaleToRingRailYOffsetDelegate = OnBeginConduitSetScaleToRingRailYOffset;
		}
		if (onEndConduitSetScaleToRingRailYOffsetDelegate == null)
		{
			onEndConduitSetScaleToRingRailYOffsetDelegate = OnEndConduitSetScaleToRingRailYOffset;
		}
		if (onConduitSetScaleToRingRailYOffsetCompletedDelegate == null)
		{
			onConduitSetScaleToRingRailYOffsetCompletedDelegate = OnConduitSetScaleToRingRailYOffsetCompleted;
		}
		InvokeAsync(onBeginConduitSetScaleToRingRailYOffsetDelegate, new object[1] { ringOffset }, onEndConduitSetScaleToRingRailYOffsetDelegate, onConduitSetScaleToRingRailYOffsetCompletedDelegate, userState);
	}

	public bool ConnectionValid()
	{
		return base.Channel.ConnectionValid();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginConnectionValid(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginConnectionValid(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndConnectionValid(IAsyncResult result)
	{
		return base.Channel.EndConnectionValid(result);
	}

	private IAsyncResult OnBeginConnectionValid(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginConnectionValid(callback, asyncState);
	}

	private object[] OnEndConnectionValid(IAsyncResult result)
	{
		bool flag = EndConnectionValid(result);
		return new object[1] { flag };
	}

	private void OnConnectionValidCompleted(object state)
	{
		if (this.ConnectionValidCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ConnectionValidCompleted(this, new ConnectionValidCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ConnectionValidAsync()
	{
		ConnectionValidAsync(null);
	}

	public void ConnectionValidAsync(object userState)
	{
		if (onBeginConnectionValidDelegate == null)
		{
			onBeginConnectionValidDelegate = OnBeginConnectionValid;
		}
		if (onEndConnectionValidDelegate == null)
		{
			onEndConnectionValidDelegate = OnEndConnectionValid;
		}
		if (onConnectionValidCompletedDelegate == null)
		{
			onConnectionValidCompletedDelegate = OnConnectionValidCompleted;
		}
		InvokeAsync(onBeginConnectionValidDelegate, null, onEndConnectionValidDelegate, onConnectionValidCompletedDelegate, userState);
	}

	public bool CSCreateSupportsForCSItem(out string ErrorMessage)
	{
		return base.Channel.CSCreateSupportsForCSItem(out ErrorMessage);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginCSCreateSupportsForCSItem(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginCSCreateSupportsForCSItem(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndCSCreateSupportsForCSItem(out string ErrorMessage, IAsyncResult result)
	{
		return base.Channel.EndCSCreateSupportsForCSItem(out ErrorMessage, result);
	}

	private IAsyncResult OnBeginCSCreateSupportsForCSItem(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginCSCreateSupportsForCSItem(callback, asyncState);
	}

	private object[] OnEndCSCreateSupportsForCSItem(IAsyncResult result)
	{
		string ErrorMessage = GetDefaultValueForInitialization<string>();
		bool flag = EndCSCreateSupportsForCSItem(out ErrorMessage, result);
		return new object[2] { ErrorMessage, flag };
	}

	private void OnCSCreateSupportsForCSItemCompleted(object state)
	{
		if (this.CSCreateSupportsForCSItemCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.CSCreateSupportsForCSItemCompleted(this, new CSCreateSupportsForCSItemCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void CSCreateSupportsForCSItemAsync()
	{
		CSCreateSupportsForCSItemAsync(null);
	}

	public void CSCreateSupportsForCSItemAsync(object userState)
	{
		if (onBeginCSCreateSupportsForCSItemDelegate == null)
		{
			onBeginCSCreateSupportsForCSItemDelegate = OnBeginCSCreateSupportsForCSItem;
		}
		if (onEndCSCreateSupportsForCSItemDelegate == null)
		{
			onEndCSCreateSupportsForCSItemDelegate = OnEndCSCreateSupportsForCSItem;
		}
		if (onCSCreateSupportsForCSItemCompletedDelegate == null)
		{
			onCSCreateSupportsForCSItemCompletedDelegate = OnCSCreateSupportsForCSItemCompleted;
		}
		InvokeAsync(onBeginCSCreateSupportsForCSItemDelegate, null, onEndCSCreateSupportsForCSItemDelegate, onCSCreateSupportsForCSItemCompletedDelegate, userState);
	}

	public UIControl[] DeleteImportedGroupFromGraph(Guid[] importGroupIdentifiers)
	{
		return base.Channel.DeleteImportedGroupFromGraph(importGroupIdentifiers);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDeleteImportedGroupFromGraph(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDeleteImportedGroupFromGraph(importGroupIdentifiers, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndDeleteImportedGroupFromGraph(IAsyncResult result)
	{
		return base.Channel.EndDeleteImportedGroupFromGraph(result);
	}

	private IAsyncResult OnBeginDeleteImportedGroupFromGraph(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiers = (Guid[])inValues[0];
		return BeginDeleteImportedGroupFromGraph(importGroupIdentifiers, callback, asyncState);
	}

	private object[] OnEndDeleteImportedGroupFromGraph(IAsyncResult result)
	{
		UIControl[] array = EndDeleteImportedGroupFromGraph(result);
		return new object[1] { array };
	}

	private void OnDeleteImportedGroupFromGraphCompleted(object state)
	{
		if (this.DeleteImportedGroupFromGraphCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DeleteImportedGroupFromGraphCompleted(this, new DeleteImportedGroupFromGraphCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DeleteImportedGroupFromGraphAsync(Guid[] importGroupIdentifiers)
	{
		DeleteImportedGroupFromGraphAsync(importGroupIdentifiers, null);
	}

	public void DeleteImportedGroupFromGraphAsync(Guid[] importGroupIdentifiers, object userState)
	{
		if (onBeginDeleteImportedGroupFromGraphDelegate == null)
		{
			onBeginDeleteImportedGroupFromGraphDelegate = OnBeginDeleteImportedGroupFromGraph;
		}
		if (onEndDeleteImportedGroupFromGraphDelegate == null)
		{
			onEndDeleteImportedGroupFromGraphDelegate = OnEndDeleteImportedGroupFromGraph;
		}
		if (onDeleteImportedGroupFromGraphCompletedDelegate == null)
		{
			onDeleteImportedGroupFromGraphCompletedDelegate = OnDeleteImportedGroupFromGraphCompleted;
		}
		InvokeAsync(onBeginDeleteImportedGroupFromGraphDelegate, new object[1] { importGroupIdentifiers }, onEndDeleteImportedGroupFromGraphDelegate, onDeleteImportedGroupFromGraphCompletedDelegate, userState);
	}

	public bool DeleteSavedJob(int whatjob)
	{
		return base.Channel.DeleteSavedJob(whatjob);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDeleteSavedJob(int whatjob, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDeleteSavedJob(whatjob, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndDeleteSavedJob(IAsyncResult result)
	{
		return base.Channel.EndDeleteSavedJob(result);
	}

	private IAsyncResult OnBeginDeleteSavedJob(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int whatjob = (int)inValues[0];
		return BeginDeleteSavedJob(whatjob, callback, asyncState);
	}

	private object[] OnEndDeleteSavedJob(IAsyncResult result)
	{
		bool flag = EndDeleteSavedJob(result);
		return new object[1] { flag };
	}

	private void OnDeleteSavedJobCompleted(object state)
	{
		if (this.DeleteSavedJobCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DeleteSavedJobCompleted(this, new DeleteSavedJobCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DeleteSavedJobAsync(int whatjob)
	{
		DeleteSavedJobAsync(whatjob, null);
	}

	public void DeleteSavedJobAsync(int whatjob, object userState)
	{
		if (onBeginDeleteSavedJobDelegate == null)
		{
			onBeginDeleteSavedJobDelegate = OnBeginDeleteSavedJob;
		}
		if (onEndDeleteSavedJobDelegate == null)
		{
			onEndDeleteSavedJobDelegate = OnEndDeleteSavedJob;
		}
		if (onDeleteSavedJobCompletedDelegate == null)
		{
			onDeleteSavedJobCompletedDelegate = OnDeleteSavedJobCompleted;
		}
		InvokeAsync(onBeginDeleteSavedJobDelegate, new object[1] { whatjob }, onEndDeleteSavedJobDelegate, onDeleteSavedJobCompletedDelegate, userState);
	}

	public void DeselectViewportControls()
	{
		base.Channel.DeselectViewportControls();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDeselectViewportControls(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDeselectViewportControls(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDeselectViewportControls(IAsyncResult result)
	{
		base.Channel.EndDeselectViewportControls(result);
	}

	private IAsyncResult OnBeginDeselectViewportControls(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDeselectViewportControls(callback, asyncState);
	}

	private object[] OnEndDeselectViewportControls(IAsyncResult result)
	{
		EndDeselectViewportControls(result);
		return null;
	}

	private void OnDeselectViewportControlsCompleted(object state)
	{
		if (this.DeselectViewportControlsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DeselectViewportControlsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DeselectViewportControlsAsync()
	{
		DeselectViewportControlsAsync(null);
	}

	public void DeselectViewportControlsAsync(object userState)
	{
		if (onBeginDeselectViewportControlsDelegate == null)
		{
			onBeginDeselectViewportControlsDelegate = OnBeginDeselectViewportControls;
		}
		if (onEndDeselectViewportControlsDelegate == null)
		{
			onEndDeselectViewportControlsDelegate = OnEndDeselectViewportControls;
		}
		if (onDeselectViewportControlsCompletedDelegate == null)
		{
			onDeselectViewportControlsCompletedDelegate = OnDeselectViewportControlsCompleted;
		}
		InvokeAsync(onBeginDeselectViewportControlsDelegate, null, onEndDeselectViewportControlsDelegate, onDeselectViewportControlsCompletedDelegate, userState);
	}

	public void DisplayConduitSetEnabled(bool enabled)
	{
		base.Channel.DisplayConduitSetEnabled(enabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDisplayConduitSetEnabled(bool enabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDisplayConduitSetEnabled(enabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDisplayConduitSetEnabled(IAsyncResult result)
	{
		base.Channel.EndDisplayConduitSetEnabled(result);
	}

	private IAsyncResult OnBeginDisplayConduitSetEnabled(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool enabled = (bool)inValues[0];
		return BeginDisplayConduitSetEnabled(enabled, callback, asyncState);
	}

	private object[] OnEndDisplayConduitSetEnabled(IAsyncResult result)
	{
		EndDisplayConduitSetEnabled(result);
		return null;
	}

	private void OnDisplayConduitSetEnabledCompleted(object state)
	{
		if (this.DisplayConduitSetEnabledCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DisplayConduitSetEnabledCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DisplayConduitSetEnabledAsync(bool enabled)
	{
		DisplayConduitSetEnabledAsync(enabled, null);
	}

	public void DisplayConduitSetEnabledAsync(bool enabled, object userState)
	{
		if (onBeginDisplayConduitSetEnabledDelegate == null)
		{
			onBeginDisplayConduitSetEnabledDelegate = OnBeginDisplayConduitSetEnabled;
		}
		if (onEndDisplayConduitSetEnabledDelegate == null)
		{
			onEndDisplayConduitSetEnabledDelegate = OnEndDisplayConduitSetEnabled;
		}
		if (onDisplayConduitSetEnabledCompletedDelegate == null)
		{
			onDisplayConduitSetEnabledCompletedDelegate = OnDisplayConduitSetEnabledCompleted;
		}
		InvokeAsync(onBeginDisplayConduitSetEnabledDelegate, new object[1] { enabled }, onEndDisplayConduitSetEnabledDelegate, onDisplayConduitSetEnabledCompletedDelegate, userState);
	}

	public void DocumentCenterDesign()
	{
		base.Channel.DocumentCenterDesign();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentCenterDesign(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentCenterDesign(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentCenterDesign(IAsyncResult result)
	{
		base.Channel.EndDocumentCenterDesign(result);
	}

	private IAsyncResult OnBeginDocumentCenterDesign(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentCenterDesign(callback, asyncState);
	}

	private object[] OnEndDocumentCenterDesign(IAsyncResult result)
	{
		EndDocumentCenterDesign(result);
		return null;
	}

	private void OnDocumentCenterDesignCompleted(object state)
	{
		if (this.DocumentCenterDesignCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentCenterDesignCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentCenterDesignAsync()
	{
		DocumentCenterDesignAsync(null);
	}

	public void DocumentCenterDesignAsync(object userState)
	{
		if (onBeginDocumentCenterDesignDelegate == null)
		{
			onBeginDocumentCenterDesignDelegate = OnBeginDocumentCenterDesign;
		}
		if (onEndDocumentCenterDesignDelegate == null)
		{
			onEndDocumentCenterDesignDelegate = OnEndDocumentCenterDesign;
		}
		if (onDocumentCenterDesignCompletedDelegate == null)
		{
			onDocumentCenterDesignCompletedDelegate = OnDocumentCenterDesignCompleted;
		}
		InvokeAsync(onBeginDocumentCenterDesignDelegate, null, onEndDocumentCenterDesignDelegate, onDocumentCenterDesignCompletedDelegate, userState);
	}

	public void DocumentClearAllAnyGemAnyColor()
	{
		base.Channel.DocumentClearAllAnyGemAnyColor();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentClearAllAnyGemAnyColor(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentClearAllAnyGemAnyColor(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentClearAllAnyGemAnyColor(IAsyncResult result)
	{
		base.Channel.EndDocumentClearAllAnyGemAnyColor(result);
	}

	private IAsyncResult OnBeginDocumentClearAllAnyGemAnyColor(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentClearAllAnyGemAnyColor(callback, asyncState);
	}

	private object[] OnEndDocumentClearAllAnyGemAnyColor(IAsyncResult result)
	{
		EndDocumentClearAllAnyGemAnyColor(result);
		return null;
	}

	private void OnDocumentClearAllAnyGemAnyColorCompleted(object state)
	{
		if (this.DocumentClearAllAnyGemAnyColorCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentClearAllAnyGemAnyColorCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentClearAllAnyGemAnyColorAsync()
	{
		DocumentClearAllAnyGemAnyColorAsync(null);
	}

	public void DocumentClearAllAnyGemAnyColorAsync(object userState)
	{
		if (onBeginDocumentClearAllAnyGemAnyColorDelegate == null)
		{
			onBeginDocumentClearAllAnyGemAnyColorDelegate = OnBeginDocumentClearAllAnyGemAnyColor;
		}
		if (onEndDocumentClearAllAnyGemAnyColorDelegate == null)
		{
			onEndDocumentClearAllAnyGemAnyColorDelegate = OnEndDocumentClearAllAnyGemAnyColor;
		}
		if (onDocumentClearAllAnyGemAnyColorCompletedDelegate == null)
		{
			onDocumentClearAllAnyGemAnyColorCompletedDelegate = OnDocumentClearAllAnyGemAnyColorCompleted;
		}
		InvokeAsync(onBeginDocumentClearAllAnyGemAnyColorDelegate, null, onEndDocumentClearAllAnyGemAnyColorDelegate, onDocumentClearAllAnyGemAnyColorCompletedDelegate, userState);
	}

	public void DocumentClearUndo()
	{
		base.Channel.DocumentClearUndo();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentClearUndo(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentClearUndo(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentClearUndo(IAsyncResult result)
	{
		base.Channel.EndDocumentClearUndo(result);
	}

	private IAsyncResult OnBeginDocumentClearUndo(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentClearUndo(callback, asyncState);
	}

	private object[] OnEndDocumentClearUndo(IAsyncResult result)
	{
		EndDocumentClearUndo(result);
		return null;
	}

	private void OnDocumentClearUndoCompleted(object state)
	{
		if (this.DocumentClearUndoCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentClearUndoCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentClearUndoAsync()
	{
		DocumentClearUndoAsync(null);
	}

	public void DocumentClearUndoAsync(object userState)
	{
		if (onBeginDocumentClearUndoDelegate == null)
		{
			onBeginDocumentClearUndoDelegate = OnBeginDocumentClearUndo;
		}
		if (onEndDocumentClearUndoDelegate == null)
		{
			onEndDocumentClearUndoDelegate = OnEndDocumentClearUndo;
		}
		if (onDocumentClearUndoCompletedDelegate == null)
		{
			onDocumentClearUndoCompletedDelegate = OnDocumentClearUndoCompleted;
		}
		InvokeAsync(onBeginDocumentClearUndoDelegate, null, onEndDocumentClearUndoDelegate, onDocumentClearUndoCompletedDelegate, userState);
	}

	public bool DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(GemMaterial gemMaterial, bool isCenterStoneAssigned)
	{
		return base.Channel.DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(gemMaterial, isCenterStoneAssigned);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(GemMaterial gemMaterial, bool isCenterStoneAssigned, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(gemMaterial, isCenterStoneAssigned, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(IAsyncResult result)
	{
		return base.Channel.EndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(result);
	}

	private IAsyncResult OnBeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(object[] inValues, AsyncCallback callback, object asyncState)
	{
		GemMaterial gemMaterial = (GemMaterial)inValues[0];
		bool isCenterStoneAssigned = (bool)inValues[1];
		return BeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(gemMaterial, isCenterStoneAssigned, callback, asyncState);
	}

	private object[] OnEndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(IAsyncResult result)
	{
		bool flag = EndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(result);
		return new object[1] { flag };
	}

	private void OnDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompleted(object state)
	{
		if (this.DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompleted(this, new DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsAsync(GemMaterial gemMaterial, bool isCenterStoneAssigned)
	{
		DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsAsync(gemMaterial, isCenterStoneAssigned, null);
	}

	public void DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsAsync(GemMaterial gemMaterial, bool isCenterStoneAssigned, object userState)
	{
		if (onBeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate == null)
		{
			onBeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate = OnBeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems;
		}
		if (onEndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate == null)
		{
			onEndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate = OnEndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems;
		}
		if (onDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompletedDelegate == null)
		{
			onDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompletedDelegate = OnDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompleted;
		}
		InvokeAsync(onBeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate, new object[2] { gemMaterial, isCenterStoneAssigned }, onEndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsDelegate, onDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsCompletedDelegate, userState);
	}

	public void DocumentDuplicateCSGPart(Guid[] importGroupIdentifiers)
	{
		base.Channel.DocumentDuplicateCSGPart(importGroupIdentifiers);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentDuplicateCSGPart(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentDuplicateCSGPart(importGroupIdentifiers, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentDuplicateCSGPart(IAsyncResult result)
	{
		base.Channel.EndDocumentDuplicateCSGPart(result);
	}

	private IAsyncResult OnBeginDocumentDuplicateCSGPart(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] importGroupIdentifiers = (Guid[])inValues[0];
		return BeginDocumentDuplicateCSGPart(importGroupIdentifiers, callback, asyncState);
	}

	private object[] OnEndDocumentDuplicateCSGPart(IAsyncResult result)
	{
		EndDocumentDuplicateCSGPart(result);
		return null;
	}

	private void OnDocumentDuplicateCSGPartCompleted(object state)
	{
		if (this.DocumentDuplicateCSGPartCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentDuplicateCSGPartCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentDuplicateCSGPartAsync(Guid[] importGroupIdentifiers)
	{
		DocumentDuplicateCSGPartAsync(importGroupIdentifiers, null);
	}

	public void DocumentDuplicateCSGPartAsync(Guid[] importGroupIdentifiers, object userState)
	{
		if (onBeginDocumentDuplicateCSGPartDelegate == null)
		{
			onBeginDocumentDuplicateCSGPartDelegate = OnBeginDocumentDuplicateCSGPart;
		}
		if (onEndDocumentDuplicateCSGPartDelegate == null)
		{
			onEndDocumentDuplicateCSGPartDelegate = OnEndDocumentDuplicateCSGPart;
		}
		if (onDocumentDuplicateCSGPartCompletedDelegate == null)
		{
			onDocumentDuplicateCSGPartCompletedDelegate = OnDocumentDuplicateCSGPartCompleted;
		}
		InvokeAsync(onBeginDocumentDuplicateCSGPartDelegate, new object[1] { importGroupIdentifiers }, onEndDocumentDuplicateCSGPartDelegate, onDocumentDuplicateCSGPartCompletedDelegate, userState);
	}

	public bool DocumentGetModified()
	{
		return base.Channel.DocumentGetModified();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentGetModified(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentGetModified(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndDocumentGetModified(IAsyncResult result)
	{
		return base.Channel.EndDocumentGetModified(result);
	}

	private IAsyncResult OnBeginDocumentGetModified(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentGetModified(callback, asyncState);
	}

	private object[] OnEndDocumentGetModified(IAsyncResult result)
	{
		bool flag = EndDocumentGetModified(result);
		return new object[1] { flag };
	}

	private void OnDocumentGetModifiedCompleted(object state)
	{
		if (this.DocumentGetModifiedCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentGetModifiedCompleted(this, new DocumentGetModifiedCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentGetModifiedAsync()
	{
		DocumentGetModifiedAsync(null);
	}

	public void DocumentGetModifiedAsync(object userState)
	{
		if (onBeginDocumentGetModifiedDelegate == null)
		{
			onBeginDocumentGetModifiedDelegate = OnBeginDocumentGetModified;
		}
		if (onEndDocumentGetModifiedDelegate == null)
		{
			onEndDocumentGetModifiedDelegate = OnEndDocumentGetModified;
		}
		if (onDocumentGetModifiedCompletedDelegate == null)
		{
			onDocumentGetModifiedCompletedDelegate = OnDocumentGetModifiedCompleted;
		}
		InvokeAsync(onBeginDocumentGetModifiedDelegate, null, onEndDocumentGetModifiedDelegate, onDocumentGetModifiedCompletedDelegate, userState);
	}

	public double DocumentGetRingSizeIndexFromSizeText(string ringSizeText)
	{
		return base.Channel.DocumentGetRingSizeIndexFromSizeText(ringSizeText);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentGetRingSizeIndexFromSizeText(string ringSizeText, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentGetRingSizeIndexFromSizeText(ringSizeText, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public double EndDocumentGetRingSizeIndexFromSizeText(IAsyncResult result)
	{
		return base.Channel.EndDocumentGetRingSizeIndexFromSizeText(result);
	}

	private IAsyncResult OnBeginDocumentGetRingSizeIndexFromSizeText(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string ringSizeText = (string)inValues[0];
		return BeginDocumentGetRingSizeIndexFromSizeText(ringSizeText, callback, asyncState);
	}

	private object[] OnEndDocumentGetRingSizeIndexFromSizeText(IAsyncResult result)
	{
		double num = EndDocumentGetRingSizeIndexFromSizeText(result);
		return new object[1] { num };
	}

	private void OnDocumentGetRingSizeIndexFromSizeTextCompleted(object state)
	{
		if (this.DocumentGetRingSizeIndexFromSizeTextCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentGetRingSizeIndexFromSizeTextCompleted(this, new DocumentGetRingSizeIndexFromSizeTextCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentGetRingSizeIndexFromSizeTextAsync(string ringSizeText)
	{
		DocumentGetRingSizeIndexFromSizeTextAsync(ringSizeText, null);
	}

	public void DocumentGetRingSizeIndexFromSizeTextAsync(string ringSizeText, object userState)
	{
		if (onBeginDocumentGetRingSizeIndexFromSizeTextDelegate == null)
		{
			onBeginDocumentGetRingSizeIndexFromSizeTextDelegate = OnBeginDocumentGetRingSizeIndexFromSizeText;
		}
		if (onEndDocumentGetRingSizeIndexFromSizeTextDelegate == null)
		{
			onEndDocumentGetRingSizeIndexFromSizeTextDelegate = OnEndDocumentGetRingSizeIndexFromSizeText;
		}
		if (onDocumentGetRingSizeIndexFromSizeTextCompletedDelegate == null)
		{
			onDocumentGetRingSizeIndexFromSizeTextCompletedDelegate = OnDocumentGetRingSizeIndexFromSizeTextCompleted;
		}
		InvokeAsync(onBeginDocumentGetRingSizeIndexFromSizeTextDelegate, new object[1] { ringSizeText }, onEndDocumentGetRingSizeIndexFromSizeTextDelegate, onDocumentGetRingSizeIndexFromSizeTextCompletedDelegate, userState);
	}

	public string DocumentGetRingSizeTextFromSizeIndex(double ringSizeIndex)
	{
		return base.Channel.DocumentGetRingSizeTextFromSizeIndex(ringSizeIndex);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentGetRingSizeTextFromSizeIndex(double ringSizeIndex, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentGetRingSizeTextFromSizeIndex(ringSizeIndex, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndDocumentGetRingSizeTextFromSizeIndex(IAsyncResult result)
	{
		return base.Channel.EndDocumentGetRingSizeTextFromSizeIndex(result);
	}

	private IAsyncResult OnBeginDocumentGetRingSizeTextFromSizeIndex(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double ringSizeIndex = (double)inValues[0];
		return BeginDocumentGetRingSizeTextFromSizeIndex(ringSizeIndex, callback, asyncState);
	}

	private object[] OnEndDocumentGetRingSizeTextFromSizeIndex(IAsyncResult result)
	{
		string text = EndDocumentGetRingSizeTextFromSizeIndex(result);
		return new object[1] { text };
	}

	private void OnDocumentGetRingSizeTextFromSizeIndexCompleted(object state)
	{
		if (this.DocumentGetRingSizeTextFromSizeIndexCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentGetRingSizeTextFromSizeIndexCompleted(this, new DocumentGetRingSizeTextFromSizeIndexCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentGetRingSizeTextFromSizeIndexAsync(double ringSizeIndex)
	{
		DocumentGetRingSizeTextFromSizeIndexAsync(ringSizeIndex, null);
	}

	public void DocumentGetRingSizeTextFromSizeIndexAsync(double ringSizeIndex, object userState)
	{
		if (onBeginDocumentGetRingSizeTextFromSizeIndexDelegate == null)
		{
			onBeginDocumentGetRingSizeTextFromSizeIndexDelegate = OnBeginDocumentGetRingSizeTextFromSizeIndex;
		}
		if (onEndDocumentGetRingSizeTextFromSizeIndexDelegate == null)
		{
			onEndDocumentGetRingSizeTextFromSizeIndexDelegate = OnEndDocumentGetRingSizeTextFromSizeIndex;
		}
		if (onDocumentGetRingSizeTextFromSizeIndexCompletedDelegate == null)
		{
			onDocumentGetRingSizeTextFromSizeIndexCompletedDelegate = OnDocumentGetRingSizeTextFromSizeIndexCompleted;
		}
		InvokeAsync(onBeginDocumentGetRingSizeTextFromSizeIndexDelegate, new object[1] { ringSizeIndex }, onEndDocumentGetRingSizeTextFromSizeIndexDelegate, onDocumentGetRingSizeTextFromSizeIndexCompletedDelegate, userState);
	}

	public double DocumentGetRingSizeTranslated(double ringSizeInMillimeters)
	{
		return base.Channel.DocumentGetRingSizeTranslated(ringSizeInMillimeters);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentGetRingSizeTranslated(double ringSizeInMillimeters, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentGetRingSizeTranslated(ringSizeInMillimeters, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public double EndDocumentGetRingSizeTranslated(IAsyncResult result)
	{
		return base.Channel.EndDocumentGetRingSizeTranslated(result);
	}

	private IAsyncResult OnBeginDocumentGetRingSizeTranslated(object[] inValues, AsyncCallback callback, object asyncState)
	{
		double ringSizeInMillimeters = (double)inValues[0];
		return BeginDocumentGetRingSizeTranslated(ringSizeInMillimeters, callback, asyncState);
	}

	private object[] OnEndDocumentGetRingSizeTranslated(IAsyncResult result)
	{
		double num = EndDocumentGetRingSizeTranslated(result);
		return new object[1] { num };
	}

	private void OnDocumentGetRingSizeTranslatedCompleted(object state)
	{
		if (this.DocumentGetRingSizeTranslatedCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentGetRingSizeTranslatedCompleted(this, new DocumentGetRingSizeTranslatedCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentGetRingSizeTranslatedAsync(double ringSizeInMillimeters)
	{
		DocumentGetRingSizeTranslatedAsync(ringSizeInMillimeters, null);
	}

	public void DocumentGetRingSizeTranslatedAsync(double ringSizeInMillimeters, object userState)
	{
		if (onBeginDocumentGetRingSizeTranslatedDelegate == null)
		{
			onBeginDocumentGetRingSizeTranslatedDelegate = OnBeginDocumentGetRingSizeTranslated;
		}
		if (onEndDocumentGetRingSizeTranslatedDelegate == null)
		{
			onEndDocumentGetRingSizeTranslatedDelegate = OnEndDocumentGetRingSizeTranslated;
		}
		if (onDocumentGetRingSizeTranslatedCompletedDelegate == null)
		{
			onDocumentGetRingSizeTranslatedCompletedDelegate = OnDocumentGetRingSizeTranslatedCompleted;
		}
		InvokeAsync(onBeginDocumentGetRingSizeTranslatedDelegate, new object[1] { ringSizeInMillimeters }, onEndDocumentGetRingSizeTranslatedDelegate, onDocumentGetRingSizeTranslatedCompletedDelegate, userState);
	}

	public StudioGem[] DocumentGetStudioGemList()
	{
		return base.Channel.DocumentGetStudioGemList();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentGetStudioGemList(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentGetStudioGemList(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public StudioGem[] EndDocumentGetStudioGemList(IAsyncResult result)
	{
		return base.Channel.EndDocumentGetStudioGemList(result);
	}

	private IAsyncResult OnBeginDocumentGetStudioGemList(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentGetStudioGemList(callback, asyncState);
	}

	private object[] OnEndDocumentGetStudioGemList(IAsyncResult result)
	{
		StudioGem[] array = EndDocumentGetStudioGemList(result);
		return new object[1] { array };
	}

	private void OnDocumentGetStudioGemListCompleted(object state)
	{
		if (this.DocumentGetStudioGemListCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentGetStudioGemListCompleted(this, new DocumentGetStudioGemListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentGetStudioGemListAsync()
	{
		DocumentGetStudioGemListAsync(null);
	}

	public void DocumentGetStudioGemListAsync(object userState)
	{
		if (onBeginDocumentGetStudioGemListDelegate == null)
		{
			onBeginDocumentGetStudioGemListDelegate = OnBeginDocumentGetStudioGemList;
		}
		if (onEndDocumentGetStudioGemListDelegate == null)
		{
			onEndDocumentGetStudioGemListDelegate = OnEndDocumentGetStudioGemList;
		}
		if (onDocumentGetStudioGemListCompletedDelegate == null)
		{
			onDocumentGetStudioGemListCompletedDelegate = OnDocumentGetStudioGemListCompleted;
		}
		InvokeAsync(onBeginDocumentGetStudioGemListDelegate, null, onEndDocumentGetStudioGemListDelegate, onDocumentGetStudioGemListCompletedDelegate, userState);
	}

	public void DocumentGraphGenerate()
	{
		base.Channel.DocumentGraphGenerate();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentGraphGenerate(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentGraphGenerate(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentGraphGenerate(IAsyncResult result)
	{
		base.Channel.EndDocumentGraphGenerate(result);
	}

	private IAsyncResult OnBeginDocumentGraphGenerate(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentGraphGenerate(callback, asyncState);
	}

	private object[] OnEndDocumentGraphGenerate(IAsyncResult result)
	{
		EndDocumentGraphGenerate(result);
		return null;
	}

	private void OnDocumentGraphGenerateCompleted(object state)
	{
		if (this.DocumentGraphGenerateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentGraphGenerateCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentGraphGenerateAsync()
	{
		DocumentGraphGenerateAsync(null);
	}

	public void DocumentGraphGenerateAsync(object userState)
	{
		if (onBeginDocumentGraphGenerateDelegate == null)
		{
			onBeginDocumentGraphGenerateDelegate = OnBeginDocumentGraphGenerate;
		}
		if (onEndDocumentGraphGenerateDelegate == null)
		{
			onEndDocumentGraphGenerateDelegate = OnEndDocumentGraphGenerate;
		}
		if (onDocumentGraphGenerateCompletedDelegate == null)
		{
			onDocumentGraphGenerateCompletedDelegate = OnDocumentGraphGenerateCompleted;
		}
		InvokeAsync(onBeginDocumentGraphGenerateDelegate, null, onEndDocumentGraphGenerateDelegate, onDocumentGraphGenerateCompletedDelegate, userState);
	}

	public void DocumentImportCSGPart(string pathToCSGPart, int? centerGemGraphIndex, bool doShowFontSourceErrors)
	{
		base.Channel.DocumentImportCSGPart(pathToCSGPart, centerGemGraphIndex, doShowFontSourceErrors);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentImportCSGPart(string pathToCSGPart, int? centerGemGraphIndex, bool doShowFontSourceErrors, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentImportCSGPart(pathToCSGPart, centerGemGraphIndex, doShowFontSourceErrors, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentImportCSGPart(IAsyncResult result)
	{
		base.Channel.EndDocumentImportCSGPart(result);
	}

	private IAsyncResult OnBeginDocumentImportCSGPart(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string pathToCSGPart = (string)inValues[0];
		int? centerGemGraphIndex = (int?)inValues[1];
		bool doShowFontSourceErrors = (bool)inValues[2];
		return BeginDocumentImportCSGPart(pathToCSGPart, centerGemGraphIndex, doShowFontSourceErrors, callback, asyncState);
	}

	private object[] OnEndDocumentImportCSGPart(IAsyncResult result)
	{
		EndDocumentImportCSGPart(result);
		return null;
	}

	private void OnDocumentImportCSGPartCompleted(object state)
	{
		if (this.DocumentImportCSGPartCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentImportCSGPartCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentImportCSGPartAsync(string pathToCSGPart, int? centerGemGraphIndex, bool doShowFontSourceErrors)
	{
		DocumentImportCSGPartAsync(pathToCSGPart, centerGemGraphIndex, doShowFontSourceErrors, null);
	}

	public void DocumentImportCSGPartAsync(string pathToCSGPart, int? centerGemGraphIndex, bool doShowFontSourceErrors, object userState)
	{
		if (onBeginDocumentImportCSGPartDelegate == null)
		{
			onBeginDocumentImportCSGPartDelegate = OnBeginDocumentImportCSGPart;
		}
		if (onEndDocumentImportCSGPartDelegate == null)
		{
			onEndDocumentImportCSGPartDelegate = OnEndDocumentImportCSGPart;
		}
		if (onDocumentImportCSGPartCompletedDelegate == null)
		{
			onDocumentImportCSGPartCompletedDelegate = OnDocumentImportCSGPartCompleted;
		}
		InvokeAsync(onBeginDocumentImportCSGPartDelegate, new object[3] { pathToCSGPart, centerGemGraphIndex, doShowFontSourceErrors }, onEndDocumentImportCSGPartDelegate, onDocumentImportCSGPartCompletedDelegate, userState);
	}

	public void DocumentReset()
	{
		base.Channel.DocumentReset();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentReset(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentReset(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentReset(IAsyncResult result)
	{
		base.Channel.EndDocumentReset(result);
	}

	private IAsyncResult OnBeginDocumentReset(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentReset(callback, asyncState);
	}

	private object[] OnEndDocumentReset(IAsyncResult result)
	{
		EndDocumentReset(result);
		return null;
	}

	private void OnDocumentResetCompleted(object state)
	{
		if (this.DocumentResetCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentResetCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentResetAsync()
	{
		DocumentResetAsync(null);
	}

	public void DocumentResetAsync(object userState)
	{
		if (onBeginDocumentResetDelegate == null)
		{
			onBeginDocumentResetDelegate = OnBeginDocumentReset;
		}
		if (onEndDocumentResetDelegate == null)
		{
			onEndDocumentResetDelegate = OnEndDocumentReset;
		}
		if (onDocumentResetCompletedDelegate == null)
		{
			onDocumentResetCompletedDelegate = OnDocumentResetCompleted;
		}
		InvokeAsync(onBeginDocumentResetDelegate, null, onEndDocumentResetDelegate, onDocumentResetCompletedDelegate, userState);
	}

	public void DocumentResetCenterGemAnyGemAnyColorMaterialAssignment()
	{
		base.Channel.DocumentResetCenterGemAnyGemAnyColorMaterialAssignment();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(IAsyncResult result)
	{
		base.Channel.EndDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(result);
	}

	private IAsyncResult OnBeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(callback, asyncState);
	}

	private object[] OnEndDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(IAsyncResult result)
	{
		EndDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(result);
		return null;
	}

	private void OnDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompleted(object state)
	{
		if (this.DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentAsync()
	{
		DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentAsync(null);
	}

	public void DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentAsync(object userState)
	{
		if (onBeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate == null)
		{
			onBeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate = OnBeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignment;
		}
		if (onEndDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate == null)
		{
			onEndDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate = OnEndDocumentResetCenterGemAnyGemAnyColorMaterialAssignment;
		}
		if (onDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompletedDelegate == null)
		{
			onDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompletedDelegate = OnDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompleted;
		}
		InvokeAsync(onBeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate, null, onEndDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentDelegate, onDocumentResetCenterGemAnyGemAnyColorMaterialAssignmentCompletedDelegate, userState);
	}

	public void DocumentSetCameraView(string cameraViewName)
	{
		base.Channel.DocumentSetCameraView(cameraViewName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSetCameraView(string cameraViewName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSetCameraView(cameraViewName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSetCameraView(IAsyncResult result)
	{
		base.Channel.EndDocumentSetCameraView(result);
	}

	private IAsyncResult OnBeginDocumentSetCameraView(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string cameraViewName = (string)inValues[0];
		return BeginDocumentSetCameraView(cameraViewName, callback, asyncState);
	}

	private object[] OnEndDocumentSetCameraView(IAsyncResult result)
	{
		EndDocumentSetCameraView(result);
		return null;
	}

	private void OnDocumentSetCameraViewCompleted(object state)
	{
		if (this.DocumentSetCameraViewCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSetCameraViewCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSetCameraViewAsync(string cameraViewName)
	{
		DocumentSetCameraViewAsync(cameraViewName, null);
	}

	public void DocumentSetCameraViewAsync(string cameraViewName, object userState)
	{
		if (onBeginDocumentSetCameraViewDelegate == null)
		{
			onBeginDocumentSetCameraViewDelegate = OnBeginDocumentSetCameraView;
		}
		if (onEndDocumentSetCameraViewDelegate == null)
		{
			onEndDocumentSetCameraViewDelegate = OnEndDocumentSetCameraView;
		}
		if (onDocumentSetCameraViewCompletedDelegate == null)
		{
			onDocumentSetCameraViewCompletedDelegate = OnDocumentSetCameraViewCompleted;
		}
		InvokeAsync(onBeginDocumentSetCameraViewDelegate, new object[1] { cameraViewName }, onEndDocumentSetCameraViewDelegate, onDocumentSetCameraViewCompletedDelegate, userState);
	}

	public void DocumentSetCurvesAndPointsVisibility(bool visible)
	{
		base.Channel.DocumentSetCurvesAndPointsVisibility(visible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSetCurvesAndPointsVisibility(bool visible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSetCurvesAndPointsVisibility(visible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSetCurvesAndPointsVisibility(IAsyncResult result)
	{
		base.Channel.EndDocumentSetCurvesAndPointsVisibility(result);
	}

	private IAsyncResult OnBeginDocumentSetCurvesAndPointsVisibility(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool visible = (bool)inValues[0];
		return BeginDocumentSetCurvesAndPointsVisibility(visible, callback, asyncState);
	}

	private object[] OnEndDocumentSetCurvesAndPointsVisibility(IAsyncResult result)
	{
		EndDocumentSetCurvesAndPointsVisibility(result);
		return null;
	}

	private void OnDocumentSetCurvesAndPointsVisibilityCompleted(object state)
	{
		if (this.DocumentSetCurvesAndPointsVisibilityCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSetCurvesAndPointsVisibilityCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSetCurvesAndPointsVisibilityAsync(bool visible)
	{
		DocumentSetCurvesAndPointsVisibilityAsync(visible, null);
	}

	public void DocumentSetCurvesAndPointsVisibilityAsync(bool visible, object userState)
	{
		if (onBeginDocumentSetCurvesAndPointsVisibilityDelegate == null)
		{
			onBeginDocumentSetCurvesAndPointsVisibilityDelegate = OnBeginDocumentSetCurvesAndPointsVisibility;
		}
		if (onEndDocumentSetCurvesAndPointsVisibilityDelegate == null)
		{
			onEndDocumentSetCurvesAndPointsVisibilityDelegate = OnEndDocumentSetCurvesAndPointsVisibility;
		}
		if (onDocumentSetCurvesAndPointsVisibilityCompletedDelegate == null)
		{
			onDocumentSetCurvesAndPointsVisibilityCompletedDelegate = OnDocumentSetCurvesAndPointsVisibilityCompleted;
		}
		InvokeAsync(onBeginDocumentSetCurvesAndPointsVisibilityDelegate, new object[1] { visible }, onEndDocumentSetCurvesAndPointsVisibilityDelegate, onDocumentSetCurvesAndPointsVisibilityCompletedDelegate, userState);
	}

	public void DocumentSetFilename(string filename)
	{
		base.Channel.DocumentSetFilename(filename);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSetFilename(string filename, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSetFilename(filename, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSetFilename(IAsyncResult result)
	{
		base.Channel.EndDocumentSetFilename(result);
	}

	private IAsyncResult OnBeginDocumentSetFilename(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string filename = (string)inValues[0];
		return BeginDocumentSetFilename(filename, callback, asyncState);
	}

	private object[] OnEndDocumentSetFilename(IAsyncResult result)
	{
		EndDocumentSetFilename(result);
		return null;
	}

	private void OnDocumentSetFilenameCompleted(object state)
	{
		if (this.DocumentSetFilenameCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSetFilenameCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSetFilenameAsync(string filename)
	{
		DocumentSetFilenameAsync(filename, null);
	}

	public void DocumentSetFilenameAsync(string filename, object userState)
	{
		if (onBeginDocumentSetFilenameDelegate == null)
		{
			onBeginDocumentSetFilenameDelegate = OnBeginDocumentSetFilename;
		}
		if (onEndDocumentSetFilenameDelegate == null)
		{
			onEndDocumentSetFilenameDelegate = OnEndDocumentSetFilename;
		}
		if (onDocumentSetFilenameCompletedDelegate == null)
		{
			onDocumentSetFilenameCompletedDelegate = OnDocumentSetFilenameCompleted;
		}
		InvokeAsync(onBeginDocumentSetFilenameDelegate, new object[1] { filename }, onEndDocumentSetFilenameDelegate, onDocumentSetFilenameCompletedDelegate, userState);
	}

	public void DocumentSetModified(bool modified)
	{
		base.Channel.DocumentSetModified(modified);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSetModified(bool modified, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSetModified(modified, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSetModified(IAsyncResult result)
	{
		base.Channel.EndDocumentSetModified(result);
	}

	private IAsyncResult OnBeginDocumentSetModified(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool modified = (bool)inValues[0];
		return BeginDocumentSetModified(modified, callback, asyncState);
	}

	private object[] OnEndDocumentSetModified(IAsyncResult result)
	{
		EndDocumentSetModified(result);
		return null;
	}

	private void OnDocumentSetModifiedCompleted(object state)
	{
		if (this.DocumentSetModifiedCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSetModifiedCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSetModifiedAsync(bool modified)
	{
		DocumentSetModifiedAsync(modified, null);
	}

	public void DocumentSetModifiedAsync(bool modified, object userState)
	{
		if (onBeginDocumentSetModifiedDelegate == null)
		{
			onBeginDocumentSetModifiedDelegate = OnBeginDocumentSetModified;
		}
		if (onEndDocumentSetModifiedDelegate == null)
		{
			onEndDocumentSetModifiedDelegate = OnEndDocumentSetModified;
		}
		if (onDocumentSetModifiedCompletedDelegate == null)
		{
			onDocumentSetModifiedCompletedDelegate = OnDocumentSetModifiedCompleted;
		}
		InvokeAsync(onBeginDocumentSetModifiedDelegate, new object[1] { modified }, onEndDocumentSetModifiedDelegate, onDocumentSetModifiedCompletedDelegate, userState);
	}

	public void DocumentSetRingSizeSetting(string ringSizeSetting)
	{
		base.Channel.DocumentSetRingSizeSetting(ringSizeSetting);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSetRingSizeSetting(string ringSizeSetting, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSetRingSizeSetting(ringSizeSetting, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSetRingSizeSetting(IAsyncResult result)
	{
		base.Channel.EndDocumentSetRingSizeSetting(result);
	}

	private IAsyncResult OnBeginDocumentSetRingSizeSetting(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string ringSizeSetting = (string)inValues[0];
		return BeginDocumentSetRingSizeSetting(ringSizeSetting, callback, asyncState);
	}

	private object[] OnEndDocumentSetRingSizeSetting(IAsyncResult result)
	{
		EndDocumentSetRingSizeSetting(result);
		return null;
	}

	private void OnDocumentSetRingSizeSettingCompleted(object state)
	{
		if (this.DocumentSetRingSizeSettingCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSetRingSizeSettingCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSetRingSizeSettingAsync(string ringSizeSetting)
	{
		DocumentSetRingSizeSettingAsync(ringSizeSetting, null);
	}

	public void DocumentSetRingSizeSettingAsync(string ringSizeSetting, object userState)
	{
		if (onBeginDocumentSetRingSizeSettingDelegate == null)
		{
			onBeginDocumentSetRingSizeSettingDelegate = OnBeginDocumentSetRingSizeSetting;
		}
		if (onEndDocumentSetRingSizeSettingDelegate == null)
		{
			onEndDocumentSetRingSizeSettingDelegate = OnEndDocumentSetRingSizeSetting;
		}
		if (onDocumentSetRingSizeSettingCompletedDelegate == null)
		{
			onDocumentSetRingSizeSettingCompletedDelegate = OnDocumentSetRingSizeSettingCompleted;
		}
		InvokeAsync(onBeginDocumentSetRingSizeSettingDelegate, new object[1] { ringSizeSetting }, onEndDocumentSetRingSizeSettingDelegate, onDocumentSetRingSizeSettingCompletedDelegate, userState);
	}

	public void DocumentSetShowCurves(bool visible)
	{
		base.Channel.DocumentSetShowCurves(visible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSetShowCurves(bool visible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSetShowCurves(visible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSetShowCurves(IAsyncResult result)
	{
		base.Channel.EndDocumentSetShowCurves(result);
	}

	private IAsyncResult OnBeginDocumentSetShowCurves(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool visible = (bool)inValues[0];
		return BeginDocumentSetShowCurves(visible, callback, asyncState);
	}

	private object[] OnEndDocumentSetShowCurves(IAsyncResult result)
	{
		EndDocumentSetShowCurves(result);
		return null;
	}

	private void OnDocumentSetShowCurvesCompleted(object state)
	{
		if (this.DocumentSetShowCurvesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSetShowCurvesCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSetShowCurvesAsync(bool visible)
	{
		DocumentSetShowCurvesAsync(visible, null);
	}

	public void DocumentSetShowCurvesAsync(bool visible, object userState)
	{
		if (onBeginDocumentSetShowCurvesDelegate == null)
		{
			onBeginDocumentSetShowCurvesDelegate = OnBeginDocumentSetShowCurves;
		}
		if (onEndDocumentSetShowCurvesDelegate == null)
		{
			onEndDocumentSetShowCurvesDelegate = OnEndDocumentSetShowCurves;
		}
		if (onDocumentSetShowCurvesCompletedDelegate == null)
		{
			onDocumentSetShowCurvesCompletedDelegate = OnDocumentSetShowCurvesCompleted;
		}
		InvokeAsync(onBeginDocumentSetShowCurvesDelegate, new object[1] { visible }, onEndDocumentSetShowCurvesDelegate, onDocumentSetShowCurvesCompletedDelegate, userState);
	}

	public void DocumentSetShowStarts(bool visible)
	{
		base.Channel.DocumentSetShowStarts(visible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSetShowStarts(bool visible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSetShowStarts(visible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSetShowStarts(IAsyncResult result)
	{
		base.Channel.EndDocumentSetShowStarts(result);
	}

	private IAsyncResult OnBeginDocumentSetShowStarts(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool visible = (bool)inValues[0];
		return BeginDocumentSetShowStarts(visible, callback, asyncState);
	}

	private object[] OnEndDocumentSetShowStarts(IAsyncResult result)
	{
		EndDocumentSetShowStarts(result);
		return null;
	}

	private void OnDocumentSetShowStartsCompleted(object state)
	{
		if (this.DocumentSetShowStartsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSetShowStartsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSetShowStartsAsync(bool visible)
	{
		DocumentSetShowStartsAsync(visible, null);
	}

	public void DocumentSetShowStartsAsync(bool visible, object userState)
	{
		if (onBeginDocumentSetShowStartsDelegate == null)
		{
			onBeginDocumentSetShowStartsDelegate = OnBeginDocumentSetShowStarts;
		}
		if (onEndDocumentSetShowStartsDelegate == null)
		{
			onEndDocumentSetShowStartsDelegate = OnEndDocumentSetShowStarts;
		}
		if (onDocumentSetShowStartsCompletedDelegate == null)
		{
			onDocumentSetShowStartsCompletedDelegate = OnDocumentSetShowStartsCompleted;
		}
		InvokeAsync(onBeginDocumentSetShowStartsDelegate, new object[1] { visible }, onEndDocumentSetShowStartsDelegate, onDocumentSetShowStartsCompletedDelegate, userState);
	}

	public void DocumentShowAxis(bool visible)
	{
		base.Channel.DocumentShowAxis(visible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentShowAxis(bool visible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentShowAxis(visible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentShowAxis(IAsyncResult result)
	{
		base.Channel.EndDocumentShowAxis(result);
	}

	private IAsyncResult OnBeginDocumentShowAxis(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool visible = (bool)inValues[0];
		return BeginDocumentShowAxis(visible, callback, asyncState);
	}

	private object[] OnEndDocumentShowAxis(IAsyncResult result)
	{
		EndDocumentShowAxis(result);
		return null;
	}

	private void OnDocumentShowAxisCompleted(object state)
	{
		if (this.DocumentShowAxisCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentShowAxisCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentShowAxisAsync(bool visible)
	{
		DocumentShowAxisAsync(visible, null);
	}

	public void DocumentShowAxisAsync(bool visible, object userState)
	{
		if (onBeginDocumentShowAxisDelegate == null)
		{
			onBeginDocumentShowAxisDelegate = OnBeginDocumentShowAxis;
		}
		if (onEndDocumentShowAxisDelegate == null)
		{
			onEndDocumentShowAxisDelegate = OnEndDocumentShowAxis;
		}
		if (onDocumentShowAxisCompletedDelegate == null)
		{
			onDocumentShowAxisCompletedDelegate = OnDocumentShowAxisCompleted;
		}
		InvokeAsync(onBeginDocumentShowAxisDelegate, new object[1] { visible }, onEndDocumentShowAxisDelegate, onDocumentShowAxisCompletedDelegate, userState);
	}

	public void DocumentShowGrid(bool visible)
	{
		base.Channel.DocumentShowGrid(visible);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentShowGrid(bool visible, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentShowGrid(visible, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentShowGrid(IAsyncResult result)
	{
		base.Channel.EndDocumentShowGrid(result);
	}

	private IAsyncResult OnBeginDocumentShowGrid(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool visible = (bool)inValues[0];
		return BeginDocumentShowGrid(visible, callback, asyncState);
	}

	private object[] OnEndDocumentShowGrid(IAsyncResult result)
	{
		EndDocumentShowGrid(result);
		return null;
	}

	private void OnDocumentShowGridCompleted(object state)
	{
		if (this.DocumentShowGridCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentShowGridCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentShowGridAsync(bool visible)
	{
		DocumentShowGridAsync(visible, null);
	}

	public void DocumentShowGridAsync(bool visible, object userState)
	{
		if (onBeginDocumentShowGridDelegate == null)
		{
			onBeginDocumentShowGridDelegate = OnBeginDocumentShowGrid;
		}
		if (onEndDocumentShowGridDelegate == null)
		{
			onEndDocumentShowGridDelegate = OnEndDocumentShowGrid;
		}
		if (onDocumentShowGridCompletedDelegate == null)
		{
			onDocumentShowGridCompletedDelegate = OnDocumentShowGridCompleted;
		}
		InvokeAsync(onBeginDocumentShowGridDelegate, new object[1] { visible }, onEndDocumentShowGridDelegate, onDocumentShowGridCompletedDelegate, userState);
	}

	public void DocumentSpinDesign(bool isEnabled)
	{
		base.Channel.DocumentSpinDesign(isEnabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentSpinDesign(bool isEnabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentSpinDesign(isEnabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentSpinDesign(IAsyncResult result)
	{
		base.Channel.EndDocumentSpinDesign(result);
	}

	private IAsyncResult OnBeginDocumentSpinDesign(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isEnabled = (bool)inValues[0];
		return BeginDocumentSpinDesign(isEnabled, callback, asyncState);
	}

	private object[] OnEndDocumentSpinDesign(IAsyncResult result)
	{
		EndDocumentSpinDesign(result);
		return null;
	}

	private void OnDocumentSpinDesignCompleted(object state)
	{
		if (this.DocumentSpinDesignCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentSpinDesignCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentSpinDesignAsync(bool isEnabled)
	{
		DocumentSpinDesignAsync(isEnabled, null);
	}

	public void DocumentSpinDesignAsync(bool isEnabled, object userState)
	{
		if (onBeginDocumentSpinDesignDelegate == null)
		{
			onBeginDocumentSpinDesignDelegate = OnBeginDocumentSpinDesign;
		}
		if (onEndDocumentSpinDesignDelegate == null)
		{
			onEndDocumentSpinDesignDelegate = OnEndDocumentSpinDesign;
		}
		if (onDocumentSpinDesignCompletedDelegate == null)
		{
			onDocumentSpinDesignCompletedDelegate = OnDocumentSpinDesignCompleted;
		}
		InvokeAsync(onBeginDocumentSpinDesignDelegate, new object[1] { isEnabled }, onEndDocumentSpinDesignDelegate, onDocumentSpinDesignCompletedDelegate, userState);
	}

	public void DocumentToggleGemSelectionMode(bool isEnabled)
	{
		base.Channel.DocumentToggleGemSelectionMode(isEnabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentToggleGemSelectionMode(bool isEnabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentToggleGemSelectionMode(isEnabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentToggleGemSelectionMode(IAsyncResult result)
	{
		base.Channel.EndDocumentToggleGemSelectionMode(result);
	}

	private IAsyncResult OnBeginDocumentToggleGemSelectionMode(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isEnabled = (bool)inValues[0];
		return BeginDocumentToggleGemSelectionMode(isEnabled, callback, asyncState);
	}

	private object[] OnEndDocumentToggleGemSelectionMode(IAsyncResult result)
	{
		EndDocumentToggleGemSelectionMode(result);
		return null;
	}

	private void OnDocumentToggleGemSelectionModeCompleted(object state)
	{
		if (this.DocumentToggleGemSelectionModeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentToggleGemSelectionModeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentToggleGemSelectionModeAsync(bool isEnabled)
	{
		DocumentToggleGemSelectionModeAsync(isEnabled, null);
	}

	public void DocumentToggleGemSelectionModeAsync(bool isEnabled, object userState)
	{
		if (onBeginDocumentToggleGemSelectionModeDelegate == null)
		{
			onBeginDocumentToggleGemSelectionModeDelegate = OnBeginDocumentToggleGemSelectionMode;
		}
		if (onEndDocumentToggleGemSelectionModeDelegate == null)
		{
			onEndDocumentToggleGemSelectionModeDelegate = OnEndDocumentToggleGemSelectionMode;
		}
		if (onDocumentToggleGemSelectionModeCompletedDelegate == null)
		{
			onDocumentToggleGemSelectionModeCompletedDelegate = OnDocumentToggleGemSelectionModeCompleted;
		}
		InvokeAsync(onBeginDocumentToggleGemSelectionModeDelegate, new object[1] { isEnabled }, onEndDocumentToggleGemSelectionModeDelegate, onDocumentToggleGemSelectionModeCompletedDelegate, userState);
	}

	public void DocumentUndoBeginRecord()
	{
		base.Channel.DocumentUndoBeginRecord();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentUndoBeginRecord(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentUndoBeginRecord(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentUndoBeginRecord(IAsyncResult result)
	{
		base.Channel.EndDocumentUndoBeginRecord(result);
	}

	private IAsyncResult OnBeginDocumentUndoBeginRecord(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentUndoBeginRecord(callback, asyncState);
	}

	private object[] OnEndDocumentUndoBeginRecord(IAsyncResult result)
	{
		EndDocumentUndoBeginRecord(result);
		return null;
	}

	private void OnDocumentUndoBeginRecordCompleted(object state)
	{
		if (this.DocumentUndoBeginRecordCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentUndoBeginRecordCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentUndoBeginRecordAsync()
	{
		DocumentUndoBeginRecordAsync(null);
	}

	public void DocumentUndoBeginRecordAsync(object userState)
	{
		if (onBeginDocumentUndoBeginRecordDelegate == null)
		{
			onBeginDocumentUndoBeginRecordDelegate = OnBeginDocumentUndoBeginRecord;
		}
		if (onEndDocumentUndoBeginRecordDelegate == null)
		{
			onEndDocumentUndoBeginRecordDelegate = OnEndDocumentUndoBeginRecord;
		}
		if (onDocumentUndoBeginRecordCompletedDelegate == null)
		{
			onDocumentUndoBeginRecordCompletedDelegate = OnDocumentUndoBeginRecordCompleted;
		}
		InvokeAsync(onBeginDocumentUndoBeginRecordDelegate, null, onEndDocumentUndoBeginRecordDelegate, onDocumentUndoBeginRecordCompletedDelegate, userState);
	}

	public void DocumentUserDataAddValue(string typeName, string keyName, string value)
	{
		base.Channel.DocumentUserDataAddValue(typeName, keyName, value);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentUserDataAddValue(string typeName, string keyName, string value, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentUserDataAddValue(typeName, keyName, value, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentUserDataAddValue(IAsyncResult result)
	{
		base.Channel.EndDocumentUserDataAddValue(result);
	}

	private IAsyncResult OnBeginDocumentUserDataAddValue(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string typeName = (string)inValues[0];
		string keyName = (string)inValues[1];
		string value = (string)inValues[2];
		return BeginDocumentUserDataAddValue(typeName, keyName, value, callback, asyncState);
	}

	private object[] OnEndDocumentUserDataAddValue(IAsyncResult result)
	{
		EndDocumentUserDataAddValue(result);
		return null;
	}

	private void OnDocumentUserDataAddValueCompleted(object state)
	{
		if (this.DocumentUserDataAddValueCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentUserDataAddValueCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentUserDataAddValueAsync(string typeName, string keyName, string value)
	{
		DocumentUserDataAddValueAsync(typeName, keyName, value, null);
	}

	public void DocumentUserDataAddValueAsync(string typeName, string keyName, string value, object userState)
	{
		if (onBeginDocumentUserDataAddValueDelegate == null)
		{
			onBeginDocumentUserDataAddValueDelegate = OnBeginDocumentUserDataAddValue;
		}
		if (onEndDocumentUserDataAddValueDelegate == null)
		{
			onEndDocumentUserDataAddValueDelegate = OnEndDocumentUserDataAddValue;
		}
		if (onDocumentUserDataAddValueCompletedDelegate == null)
		{
			onDocumentUserDataAddValueCompletedDelegate = OnDocumentUserDataAddValueCompleted;
		}
		InvokeAsync(onBeginDocumentUserDataAddValueDelegate, new object[3] { typeName, keyName, value }, onEndDocumentUserDataAddValueDelegate, onDocumentUserDataAddValueCompletedDelegate, userState);
	}

	public string DocumentUserDataFindValue(string typeName, string keyName)
	{
		return base.Channel.DocumentUserDataFindValue(typeName, keyName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentUserDataFindValue(string typeName, string keyName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentUserDataFindValue(typeName, keyName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndDocumentUserDataFindValue(IAsyncResult result)
	{
		return base.Channel.EndDocumentUserDataFindValue(result);
	}

	private IAsyncResult OnBeginDocumentUserDataFindValue(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string typeName = (string)inValues[0];
		string keyName = (string)inValues[1];
		return BeginDocumentUserDataFindValue(typeName, keyName, callback, asyncState);
	}

	private object[] OnEndDocumentUserDataFindValue(IAsyncResult result)
	{
		string text = EndDocumentUserDataFindValue(result);
		return new object[1] { text };
	}

	private void OnDocumentUserDataFindValueCompleted(object state)
	{
		if (this.DocumentUserDataFindValueCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentUserDataFindValueCompleted(this, new DocumentUserDataFindValueCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentUserDataFindValueAsync(string typeName, string keyName)
	{
		DocumentUserDataFindValueAsync(typeName, keyName, null);
	}

	public void DocumentUserDataFindValueAsync(string typeName, string keyName, object userState)
	{
		if (onBeginDocumentUserDataFindValueDelegate == null)
		{
			onBeginDocumentUserDataFindValueDelegate = OnBeginDocumentUserDataFindValue;
		}
		if (onEndDocumentUserDataFindValueDelegate == null)
		{
			onEndDocumentUserDataFindValueDelegate = OnEndDocumentUserDataFindValue;
		}
		if (onDocumentUserDataFindValueCompletedDelegate == null)
		{
			onDocumentUserDataFindValueCompletedDelegate = OnDocumentUserDataFindValueCompleted;
		}
		InvokeAsync(onBeginDocumentUserDataFindValueDelegate, new object[2] { typeName, keyName }, onEndDocumentUserDataFindValueDelegate, onDocumentUserDataFindValueCompletedDelegate, userState);
	}

	public string DocumentUserDataGetOrderData()
	{
		return base.Channel.DocumentUserDataGetOrderData();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentUserDataGetOrderData(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentUserDataGetOrderData(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndDocumentUserDataGetOrderData(IAsyncResult result)
	{
		return base.Channel.EndDocumentUserDataGetOrderData(result);
	}

	private IAsyncResult OnBeginDocumentUserDataGetOrderData(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentUserDataGetOrderData(callback, asyncState);
	}

	private object[] OnEndDocumentUserDataGetOrderData(IAsyncResult result)
	{
		string text = EndDocumentUserDataGetOrderData(result);
		return new object[1] { text };
	}

	private void OnDocumentUserDataGetOrderDataCompleted(object state)
	{
		if (this.DocumentUserDataGetOrderDataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentUserDataGetOrderDataCompleted(this, new DocumentUserDataGetOrderDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentUserDataGetOrderDataAsync()
	{
		DocumentUserDataGetOrderDataAsync(null);
	}

	public void DocumentUserDataGetOrderDataAsync(object userState)
	{
		if (onBeginDocumentUserDataGetOrderDataDelegate == null)
		{
			onBeginDocumentUserDataGetOrderDataDelegate = OnBeginDocumentUserDataGetOrderData;
		}
		if (onEndDocumentUserDataGetOrderDataDelegate == null)
		{
			onEndDocumentUserDataGetOrderDataDelegate = OnEndDocumentUserDataGetOrderData;
		}
		if (onDocumentUserDataGetOrderDataCompletedDelegate == null)
		{
			onDocumentUserDataGetOrderDataCompletedDelegate = OnDocumentUserDataGetOrderDataCompleted;
		}
		InvokeAsync(onBeginDocumentUserDataGetOrderDataDelegate, null, onEndDocumentUserDataGetOrderDataDelegate, onDocumentUserDataGetOrderDataCompletedDelegate, userState);
	}

	public Tuple<string, string, string>[] DocumentUserDataGetAllDataElements()
	{
		return base.Channel.DocumentUserDataGetAllDataElements();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentUserDataGetAllDataElements(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentUserDataGetAllDataElements(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Tuple<string, string, string>[] EndDocumentUserDataGetAllDataElements(IAsyncResult result)
	{
		return base.Channel.EndDocumentUserDataGetAllDataElements(result);
	}

	private IAsyncResult OnBeginDocumentUserDataGetAllDataElements(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentUserDataGetAllDataElements(callback, asyncState);
	}

	private object[] OnEndDocumentUserDataGetAllDataElements(IAsyncResult result)
	{
		Tuple<string, string, string>[] array = EndDocumentUserDataGetAllDataElements(result);
		return new object[1] { array };
	}

	private void OnDocumentUserDataGetAllDataElementsCompleted(object state)
	{
		if (this.DocumentUserDataGetAllDataElementsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentUserDataGetAllDataElementsCompleted(this, new DocumentUserDataGetAllDataElementsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentUserDataGetAllDataElementsAsync()
	{
		DocumentUserDataGetAllDataElementsAsync(null);
	}

	public void DocumentUserDataGetAllDataElementsAsync(object userState)
	{
		if (onBeginDocumentUserDataGetAllDataElementsDelegate == null)
		{
			onBeginDocumentUserDataGetAllDataElementsDelegate = OnBeginDocumentUserDataGetAllDataElements;
		}
		if (onEndDocumentUserDataGetAllDataElementsDelegate == null)
		{
			onEndDocumentUserDataGetAllDataElementsDelegate = OnEndDocumentUserDataGetAllDataElements;
		}
		if (onDocumentUserDataGetAllDataElementsCompletedDelegate == null)
		{
			onDocumentUserDataGetAllDataElementsCompletedDelegate = OnDocumentUserDataGetAllDataElementsCompleted;
		}
		InvokeAsync(onBeginDocumentUserDataGetAllDataElementsDelegate, null, onEndDocumentUserDataGetAllDataElementsDelegate, onDocumentUserDataGetAllDataElementsCompletedDelegate, userState);
	}

	public void DocumentWrite(string path, string licenseInfo)
	{
		base.Channel.DocumentWrite(path, licenseInfo);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentWrite(string path, string licenseInfo, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentWrite(path, licenseInfo, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentWrite(IAsyncResult result)
	{
		base.Channel.EndDocumentWrite(result);
	}

	private IAsyncResult OnBeginDocumentWrite(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string path = (string)inValues[0];
		string licenseInfo = (string)inValues[1];
		return BeginDocumentWrite(path, licenseInfo, callback, asyncState);
	}

	private object[] OnEndDocumentWrite(IAsyncResult result)
	{
		EndDocumentWrite(result);
		return null;
	}

	private void OnDocumentWriteCompleted(object state)
	{
		if (this.DocumentWriteCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentWriteCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentWriteAsync(string path, string licenseInfo)
	{
		DocumentWriteAsync(path, licenseInfo, null);
	}

	public void DocumentWriteAsync(string path, string licenseInfo, object userState)
	{
		if (onBeginDocumentWriteDelegate == null)
		{
			onBeginDocumentWriteDelegate = OnBeginDocumentWrite;
		}
		if (onEndDocumentWriteDelegate == null)
		{
			onEndDocumentWriteDelegate = OnEndDocumentWrite;
		}
		if (onDocumentWriteCompletedDelegate == null)
		{
			onDocumentWriteCompletedDelegate = OnDocumentWriteCompleted;
		}
		InvokeAsync(onBeginDocumentWriteDelegate, new object[2] { path, licenseInfo }, onEndDocumentWriteDelegate, onDocumentWriteCompletedDelegate, userState);
	}

	public void DocumentZoomCameraToBoundingBox()
	{
		base.Channel.DocumentZoomCameraToBoundingBox();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDocumentZoomCameraToBoundingBox(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDocumentZoomCameraToBoundingBox(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndDocumentZoomCameraToBoundingBox(IAsyncResult result)
	{
		base.Channel.EndDocumentZoomCameraToBoundingBox(result);
	}

	private IAsyncResult OnBeginDocumentZoomCameraToBoundingBox(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginDocumentZoomCameraToBoundingBox(callback, asyncState);
	}

	private object[] OnEndDocumentZoomCameraToBoundingBox(IAsyncResult result)
	{
		EndDocumentZoomCameraToBoundingBox(result);
		return null;
	}

	private void OnDocumentZoomCameraToBoundingBoxCompleted(object state)
	{
		if (this.DocumentZoomCameraToBoundingBoxCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DocumentZoomCameraToBoundingBoxCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DocumentZoomCameraToBoundingBoxAsync()
	{
		DocumentZoomCameraToBoundingBoxAsync(null);
	}

	public void DocumentZoomCameraToBoundingBoxAsync(object userState)
	{
		if (onBeginDocumentZoomCameraToBoundingBoxDelegate == null)
		{
			onBeginDocumentZoomCameraToBoundingBoxDelegate = OnBeginDocumentZoomCameraToBoundingBox;
		}
		if (onEndDocumentZoomCameraToBoundingBoxDelegate == null)
		{
			onEndDocumentZoomCameraToBoundingBoxDelegate = OnEndDocumentZoomCameraToBoundingBox;
		}
		if (onDocumentZoomCameraToBoundingBoxCompletedDelegate == null)
		{
			onDocumentZoomCameraToBoundingBoxCompletedDelegate = OnDocumentZoomCameraToBoundingBoxCompleted;
		}
		InvokeAsync(onBeginDocumentZoomCameraToBoundingBoxDelegate, null, onEndDocumentZoomCameraToBoundingBoxDelegate, onDocumentZoomCameraToBoundingBoxCompletedDelegate, userState);
	}

	public double[] EditorGetPartsForCurveEditrFromGraphObject(out double chooserValue, out NurbsCurve curve, Guid blockInstanceGuid, bool createAsCopy)
	{
		return base.Channel.EditorGetPartsForCurveEditrFromGraphObject(out chooserValue, out curve, blockInstanceGuid, createAsCopy);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginEditorGetPartsForCurveEditrFromGraphObject(Guid blockInstanceGuid, bool createAsCopy, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginEditorGetPartsForCurveEditrFromGraphObject(blockInstanceGuid, createAsCopy, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public double[] EndEditorGetPartsForCurveEditrFromGraphObject(out double chooserValue, out NurbsCurve curve, IAsyncResult result)
	{
		return base.Channel.EndEditorGetPartsForCurveEditrFromGraphObject(out chooserValue, out curve, result);
	}

	private IAsyncResult OnBeginEditorGetPartsForCurveEditrFromGraphObject(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid blockInstanceGuid = (Guid)inValues[0];
		bool createAsCopy = (bool)inValues[1];
		return BeginEditorGetPartsForCurveEditrFromGraphObject(blockInstanceGuid, createAsCopy, callback, asyncState);
	}

	private object[] OnEndEditorGetPartsForCurveEditrFromGraphObject(IAsyncResult result)
	{
		double chooserValue = GetDefaultValueForInitialization<double>();
		NurbsCurve curve = GetDefaultValueForInitialization<NurbsCurve>();
		double[] array = EndEditorGetPartsForCurveEditrFromGraphObject(out chooserValue, out curve, result);
		return new object[3] { chooserValue, curve, array };
	}

	private void OnEditorGetPartsForCurveEditrFromGraphObjectCompleted(object state)
	{
		if (this.EditorGetPartsForCurveEditrFromGraphObjectCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.EditorGetPartsForCurveEditrFromGraphObjectCompleted(this, new EditorGetPartsForCurveEditrFromGraphObjectCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void EditorGetPartsForCurveEditrFromGraphObjectAsync(Guid blockInstanceGuid, bool createAsCopy)
	{
		EditorGetPartsForCurveEditrFromGraphObjectAsync(blockInstanceGuid, createAsCopy, null);
	}

	public void EditorGetPartsForCurveEditrFromGraphObjectAsync(Guid blockInstanceGuid, bool createAsCopy, object userState)
	{
		if (onBeginEditorGetPartsForCurveEditrFromGraphObjectDelegate == null)
		{
			onBeginEditorGetPartsForCurveEditrFromGraphObjectDelegate = OnBeginEditorGetPartsForCurveEditrFromGraphObject;
		}
		if (onEndEditorGetPartsForCurveEditrFromGraphObjectDelegate == null)
		{
			onEndEditorGetPartsForCurveEditrFromGraphObjectDelegate = OnEndEditorGetPartsForCurveEditrFromGraphObject;
		}
		if (onEditorGetPartsForCurveEditrFromGraphObjectCompletedDelegate == null)
		{
			onEditorGetPartsForCurveEditrFromGraphObjectCompletedDelegate = OnEditorGetPartsForCurveEditrFromGraphObjectCompleted;
		}
		InvokeAsync(onBeginEditorGetPartsForCurveEditrFromGraphObjectDelegate, new object[2] { blockInstanceGuid, createAsCopy }, onEndEditorGetPartsForCurveEditrFromGraphObjectDelegate, onEditorGetPartsForCurveEditrFromGraphObjectCompletedDelegate, userState);
	}

	public void EnableDrawing(bool isEnabled)
	{
		base.Channel.EnableDrawing(isEnabled);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginEnableDrawing(bool isEnabled, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginEnableDrawing(isEnabled, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndEnableDrawing(IAsyncResult result)
	{
		base.Channel.EndEnableDrawing(result);
	}

	private IAsyncResult OnBeginEnableDrawing(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool isEnabled = (bool)inValues[0];
		return BeginEnableDrawing(isEnabled, callback, asyncState);
	}

	private object[] OnEndEnableDrawing(IAsyncResult result)
	{
		EndEnableDrawing(result);
		return null;
	}

	private void OnEnableDrawingCompleted(object state)
	{
		if (this.EnableDrawingCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.EnableDrawingCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void EnableDrawingAsync(bool isEnabled)
	{
		EnableDrawingAsync(isEnabled, null);
	}

	public void EnableDrawingAsync(bool isEnabled, object userState)
	{
		if (onBeginEnableDrawingDelegate == null)
		{
			onBeginEnableDrawingDelegate = OnBeginEnableDrawing;
		}
		if (onEndEnableDrawingDelegate == null)
		{
			onEndEnableDrawingDelegate = OnEndEnableDrawing;
		}
		if (onEnableDrawingCompletedDelegate == null)
		{
			onEnableDrawingCompletedDelegate = OnEnableDrawingCompleted;
		}
		InvokeAsync(onBeginEnableDrawingDelegate, new object[1] { isEnabled }, onEndEnableDrawingDelegate, onEnableDrawingCompletedDelegate, userState);
	}

	public bool ExportDesignAs3DM(string savePath, bool includeCenter, bool includeSide, bool includeAccent)
	{
		return base.Channel.ExportDesignAs3DM(savePath, includeCenter, includeSide, includeAccent);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginExportDesignAs3DM(string savePath, bool includeCenter, bool includeSide, bool includeAccent, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginExportDesignAs3DM(savePath, includeCenter, includeSide, includeAccent, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndExportDesignAs3DM(IAsyncResult result)
	{
		return base.Channel.EndExportDesignAs3DM(result);
	}

	private IAsyncResult OnBeginExportDesignAs3DM(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string savePath = (string)inValues[0];
		bool includeCenter = (bool)inValues[1];
		bool includeSide = (bool)inValues[2];
		bool includeAccent = (bool)inValues[3];
		return BeginExportDesignAs3DM(savePath, includeCenter, includeSide, includeAccent, callback, asyncState);
	}

	private object[] OnEndExportDesignAs3DM(IAsyncResult result)
	{
		bool flag = EndExportDesignAs3DM(result);
		return new object[1] { flag };
	}

	private void OnExportDesignAs3DMCompleted(object state)
	{
		if (this.ExportDesignAs3DMCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ExportDesignAs3DMCompleted(this, new ExportDesignAs3DMCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ExportDesignAs3DMAsync(string savePath, bool includeCenter, bool includeSide, bool includeAccent)
	{
		ExportDesignAs3DMAsync(savePath, includeCenter, includeSide, includeAccent, null);
	}

	public void ExportDesignAs3DMAsync(string savePath, bool includeCenter, bool includeSide, bool includeAccent, object userState)
	{
		if (onBeginExportDesignAs3DMDelegate == null)
		{
			onBeginExportDesignAs3DMDelegate = OnBeginExportDesignAs3DM;
		}
		if (onEndExportDesignAs3DMDelegate == null)
		{
			onEndExportDesignAs3DMDelegate = OnEndExportDesignAs3DM;
		}
		if (onExportDesignAs3DMCompletedDelegate == null)
		{
			onExportDesignAs3DMCompletedDelegate = OnExportDesignAs3DMCompleted;
		}
		InvokeAsync(onBeginExportDesignAs3DMDelegate, new object[4] { savePath, includeCenter, includeSide, includeAccent }, onEndExportDesignAs3DMDelegate, onExportDesignAs3DMCompletedDelegate, userState);
	}

	public bool ExportDesignAsSTL(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh)
	{
		return base.Channel.ExportDesignAsSTL(savePath, includeCenter, includeSide, includeAccent, repairMesh);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginExportDesignAsSTL(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginExportDesignAsSTL(savePath, includeCenter, includeSide, includeAccent, repairMesh, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndExportDesignAsSTL(IAsyncResult result)
	{
		return base.Channel.EndExportDesignAsSTL(result);
	}

	private IAsyncResult OnBeginExportDesignAsSTL(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string savePath = (string)inValues[0];
		bool includeCenter = (bool)inValues[1];
		bool includeSide = (bool)inValues[2];
		bool includeAccent = (bool)inValues[3];
		bool repairMesh = (bool)inValues[4];
		return BeginExportDesignAsSTL(savePath, includeCenter, includeSide, includeAccent, repairMesh, callback, asyncState);
	}

	private object[] OnEndExportDesignAsSTL(IAsyncResult result)
	{
		bool flag = EndExportDesignAsSTL(result);
		return new object[1] { flag };
	}

	private void OnExportDesignAsSTLCompleted(object state)
	{
		if (this.ExportDesignAsSTLCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ExportDesignAsSTLCompleted(this, new ExportDesignAsSTLCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ExportDesignAsSTLAsync(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh)
	{
		ExportDesignAsSTLAsync(savePath, includeCenter, includeSide, includeAccent, repairMesh, null);
	}

	public void ExportDesignAsSTLAsync(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh, object userState)
	{
		if (onBeginExportDesignAsSTLDelegate == null)
		{
			onBeginExportDesignAsSTLDelegate = OnBeginExportDesignAsSTL;
		}
		if (onEndExportDesignAsSTLDelegate == null)
		{
			onEndExportDesignAsSTLDelegate = OnEndExportDesignAsSTL;
		}
		if (onExportDesignAsSTLCompletedDelegate == null)
		{
			onExportDesignAsSTLCompletedDelegate = OnExportDesignAsSTLCompleted;
		}
		InvokeAsync(onBeginExportDesignAsSTLDelegate, new object[5] { savePath, includeCenter, includeSide, includeAccent, repairMesh }, onEndExportDesignAsSTLDelegate, onExportDesignAsSTLCompletedDelegate, userState);
	}

	public void ExportImportGroups(Guid[] targetImportGroupIdentifiers, string path)
	{
		base.Channel.ExportImportGroups(targetImportGroupIdentifiers, path);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginExportImportGroups(Guid[] targetImportGroupIdentifiers, string path, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginExportImportGroups(targetImportGroupIdentifiers, path, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndExportImportGroups(IAsyncResult result)
	{
		base.Channel.EndExportImportGroups(result);
	}

	private IAsyncResult OnBeginExportImportGroups(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid[] targetImportGroupIdentifiers = (Guid[])inValues[0];
		string path = (string)inValues[1];
		return BeginExportImportGroups(targetImportGroupIdentifiers, path, callback, asyncState);
	}

	private object[] OnEndExportImportGroups(IAsyncResult result)
	{
		EndExportImportGroups(result);
		return null;
	}

	private void OnExportImportGroupsCompleted(object state)
	{
		if (this.ExportImportGroupsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.ExportImportGroupsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void ExportImportGroupsAsync(Guid[] targetImportGroupIdentifiers, string path)
	{
		ExportImportGroupsAsync(targetImportGroupIdentifiers, path, null);
	}

	public void ExportImportGroupsAsync(Guid[] targetImportGroupIdentifiers, string path, object userState)
	{
		if (onBeginExportImportGroupsDelegate == null)
		{
			onBeginExportImportGroupsDelegate = OnBeginExportImportGroups;
		}
		if (onEndExportImportGroupsDelegate == null)
		{
			onEndExportImportGroupsDelegate = OnEndExportImportGroups;
		}
		if (onExportImportGroupsCompletedDelegate == null)
		{
			onExportImportGroupsCompletedDelegate = OnExportImportGroupsCompleted;
		}
		InvokeAsync(onBeginExportImportGroupsDelegate, new object[2] { targetImportGroupIdentifiers, path }, onEndExportImportGroupsDelegate, onExportImportGroupsCompletedDelegate, userState);
	}

	public bool FindProjetPrinter(out string FindAddress)
	{
		return base.Channel.FindProjetPrinter(out FindAddress);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginFindProjetPrinter(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginFindProjetPrinter(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndFindProjetPrinter(out string FindAddress, IAsyncResult result)
	{
		return base.Channel.EndFindProjetPrinter(out FindAddress, result);
	}

	private IAsyncResult OnBeginFindProjetPrinter(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginFindProjetPrinter(callback, asyncState);
	}

	private object[] OnEndFindProjetPrinter(IAsyncResult result)
	{
		string FindAddress = GetDefaultValueForInitialization<string>();
		bool flag = EndFindProjetPrinter(out FindAddress, result);
		return new object[2] { FindAddress, flag };
	}

	private void OnFindProjetPrinterCompleted(object state)
	{
		if (this.FindProjetPrinterCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.FindProjetPrinterCompleted(this, new FindProjetPrinterCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void FindProjetPrinterAsync()
	{
		FindProjetPrinterAsync(null);
	}

	public void FindProjetPrinterAsync(object userState)
	{
		if (onBeginFindProjetPrinterDelegate == null)
		{
			onBeginFindProjetPrinterDelegate = OnBeginFindProjetPrinter;
		}
		if (onEndFindProjetPrinterDelegate == null)
		{
			onEndFindProjetPrinterDelegate = OnEndFindProjetPrinter;
		}
		if (onFindProjetPrinterCompletedDelegate == null)
		{
			onFindProjetPrinterCompletedDelegate = OnFindProjetPrinterCompleted;
		}
		InvokeAsync(onBeginFindProjetPrinterDelegate, null, onEndFindProjetPrinterDelegate, onFindProjetPrinterCompletedDelegate, userState);
	}

	public bool GetActivationState()
	{
		return base.Channel.GetActivationState();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetActivationState(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetActivationState(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndGetActivationState(IAsyncResult result)
	{
		return base.Channel.EndGetActivationState(result);
	}

	private IAsyncResult OnBeginGetActivationState(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetActivationState(callback, asyncState);
	}

	private object[] OnEndGetActivationState(IAsyncResult result)
	{
		bool flag = EndGetActivationState(result);
		return new object[1] { flag };
	}

	private void OnGetActivationStateCompleted(object state)
	{
		if (this.GetActivationStateCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetActivationStateCompleted(this, new GetActivationStateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetActivationStateAsync()
	{
		GetActivationStateAsync(null);
	}

	public void GetActivationStateAsync(object userState)
	{
		if (onBeginGetActivationStateDelegate == null)
		{
			onBeginGetActivationStateDelegate = OnBeginGetActivationState;
		}
		if (onEndGetActivationStateDelegate == null)
		{
			onEndGetActivationStateDelegate = OnEndGetActivationState;
		}
		if (onGetActivationStateCompletedDelegate == null)
		{
			onGetActivationStateCompletedDelegate = OnGetActivationStateCompleted;
		}
		InvokeAsync(onBeginGetActivationStateDelegate, null, onEndGetActivationStateDelegate, onGetActivationStateCompletedDelegate, userState);
	}

	public KeyValuePair<int, string>[] GetAllRingSizesForCurrentRingSizeRegion()
	{
		return base.Channel.GetAllRingSizesForCurrentRingSizeRegion();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetAllRingSizesForCurrentRingSizeRegion(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetAllRingSizesForCurrentRingSizeRegion(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public KeyValuePair<int, string>[] EndGetAllRingSizesForCurrentRingSizeRegion(IAsyncResult result)
	{
		return base.Channel.EndGetAllRingSizesForCurrentRingSizeRegion(result);
	}

	private IAsyncResult OnBeginGetAllRingSizesForCurrentRingSizeRegion(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetAllRingSizesForCurrentRingSizeRegion(callback, asyncState);
	}

	private object[] OnEndGetAllRingSizesForCurrentRingSizeRegion(IAsyncResult result)
	{
		KeyValuePair<int, string>[] array = EndGetAllRingSizesForCurrentRingSizeRegion(result);
		return new object[1] { array };
	}

	private void OnGetAllRingSizesForCurrentRingSizeRegionCompleted(object state)
	{
		if (this.GetAllRingSizesForCurrentRingSizeRegionCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetAllRingSizesForCurrentRingSizeRegionCompleted(this, new GetAllRingSizesForCurrentRingSizeRegionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetAllRingSizesForCurrentRingSizeRegionAsync()
	{
		GetAllRingSizesForCurrentRingSizeRegionAsync(null);
	}

	public void GetAllRingSizesForCurrentRingSizeRegionAsync(object userState)
	{
		if (onBeginGetAllRingSizesForCurrentRingSizeRegionDelegate == null)
		{
			onBeginGetAllRingSizesForCurrentRingSizeRegionDelegate = OnBeginGetAllRingSizesForCurrentRingSizeRegion;
		}
		if (onEndGetAllRingSizesForCurrentRingSizeRegionDelegate == null)
		{
			onEndGetAllRingSizesForCurrentRingSizeRegionDelegate = OnEndGetAllRingSizesForCurrentRingSizeRegion;
		}
		if (onGetAllRingSizesForCurrentRingSizeRegionCompletedDelegate == null)
		{
			onGetAllRingSizesForCurrentRingSizeRegionCompletedDelegate = OnGetAllRingSizesForCurrentRingSizeRegionCompleted;
		}
		InvokeAsync(onBeginGetAllRingSizesForCurrentRingSizeRegionDelegate, null, onEndGetAllRingSizesForCurrentRingSizeRegionDelegate, onGetAllRingSizesForCurrentRingSizeRegionCompletedDelegate, userState);
	}

	public ComponentsEnum[] GetComponents()
	{
		return base.Channel.GetComponents();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetComponents(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetComponents(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ComponentsEnum[] EndGetComponents(IAsyncResult result)
	{
		return base.Channel.EndGetComponents(result);
	}

	private IAsyncResult OnBeginGetComponents(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetComponents(callback, asyncState);
	}

	private object[] OnEndGetComponents(IAsyncResult result)
	{
		ComponentsEnum[] array = EndGetComponents(result);
		return new object[1] { array };
	}

	private void OnGetComponentsCompleted(object state)
	{
		if (this.GetComponentsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetComponentsCompleted(this, new GetComponentsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetComponentsAsync()
	{
		GetComponentsAsync(null);
	}

	public void GetComponentsAsync(object userState)
	{
		if (onBeginGetComponentsDelegate == null)
		{
			onBeginGetComponentsDelegate = OnBeginGetComponents;
		}
		if (onEndGetComponentsDelegate == null)
		{
			onEndGetComponentsDelegate = OnEndGetComponents;
		}
		if (onGetComponentsCompletedDelegate == null)
		{
			onGetComponentsCompletedDelegate = OnGetComponentsCompleted;
		}
		InvokeAsync(onBeginGetComponentsDelegate, null, onEndGetComponentsDelegate, onGetComponentsCompletedDelegate, userState);
	}

	public byte[] GetCurrentViewportImageBitmap()
	{
		return base.Channel.GetCurrentViewportImageBitmap();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetCurrentViewportImageBitmap(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetCurrentViewportImageBitmap(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public byte[] EndGetCurrentViewportImageBitmap(IAsyncResult result)
	{
		return base.Channel.EndGetCurrentViewportImageBitmap(result);
	}

	private IAsyncResult OnBeginGetCurrentViewportImageBitmap(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetCurrentViewportImageBitmap(callback, asyncState);
	}

	private object[] OnEndGetCurrentViewportImageBitmap(IAsyncResult result)
	{
		byte[] array = EndGetCurrentViewportImageBitmap(result);
		return new object[1] { array };
	}

	private void OnGetCurrentViewportImageBitmapCompleted(object state)
	{
		if (this.GetCurrentViewportImageBitmapCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetCurrentViewportImageBitmapCompleted(this, new GetCurrentViewportImageBitmapCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetCurrentViewportImageBitmapAsync()
	{
		GetCurrentViewportImageBitmapAsync(null);
	}

	public void GetCurrentViewportImageBitmapAsync(object userState)
	{
		if (onBeginGetCurrentViewportImageBitmapDelegate == null)
		{
			onBeginGetCurrentViewportImageBitmapDelegate = OnBeginGetCurrentViewportImageBitmap;
		}
		if (onEndGetCurrentViewportImageBitmapDelegate == null)
		{
			onEndGetCurrentViewportImageBitmapDelegate = OnEndGetCurrentViewportImageBitmap;
		}
		if (onGetCurrentViewportImageBitmapCompletedDelegate == null)
		{
			onGetCurrentViewportImageBitmapCompletedDelegate = OnGetCurrentViewportImageBitmapCompleted;
		}
		InvokeAsync(onBeginGetCurrentViewportImageBitmapDelegate, null, onEndGetCurrentViewportImageBitmapDelegate, onGetCurrentViewportImageBitmapCompletedDelegate, userState);
	}

	public string GetDesignerFromUserData()
	{
		return base.Channel.GetDesignerFromUserData();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetDesignerFromUserData(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetDesignerFromUserData(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetDesignerFromUserData(IAsyncResult result)
	{
		return base.Channel.EndGetDesignerFromUserData(result);
	}

	private IAsyncResult OnBeginGetDesignerFromUserData(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetDesignerFromUserData(callback, asyncState);
	}

	private object[] OnEndGetDesignerFromUserData(IAsyncResult result)
	{
		string text = EndGetDesignerFromUserData(result);
		return new object[1] { text };
	}

	private void OnGetDesignerFromUserDataCompleted(object state)
	{
		if (this.GetDesignerFromUserDataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetDesignerFromUserDataCompleted(this, new GetDesignerFromUserDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetDesignerFromUserDataAsync()
	{
		GetDesignerFromUserDataAsync(null);
	}

	public void GetDesignerFromUserDataAsync(object userState)
	{
		if (onBeginGetDesignerFromUserDataDelegate == null)
		{
			onBeginGetDesignerFromUserDataDelegate = OnBeginGetDesignerFromUserData;
		}
		if (onEndGetDesignerFromUserDataDelegate == null)
		{
			onEndGetDesignerFromUserDataDelegate = OnEndGetDesignerFromUserData;
		}
		if (onGetDesignerFromUserDataCompletedDelegate == null)
		{
			onGetDesignerFromUserDataCompletedDelegate = OnGetDesignerFromUserDataCompleted;
		}
		InvokeAsync(onBeginGetDesignerFromUserDataDelegate, null, onEndGetDesignerFromUserDataDelegate, onGetDesignerFromUserDataCompletedDelegate, userState);
	}

	public int GetDesignIterationCount()
	{
		return base.Channel.GetDesignIterationCount();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetDesignIterationCount(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetDesignIterationCount(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int EndGetDesignIterationCount(IAsyncResult result)
	{
		return base.Channel.EndGetDesignIterationCount(result);
	}

	private IAsyncResult OnBeginGetDesignIterationCount(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetDesignIterationCount(callback, asyncState);
	}

	private object[] OnEndGetDesignIterationCount(IAsyncResult result)
	{
		int num = EndGetDesignIterationCount(result);
		return new object[1] { num };
	}

	private void OnGetDesignIterationCountCompleted(object state)
	{
		if (this.GetDesignIterationCountCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetDesignIterationCountCompleted(this, new GetDesignIterationCountCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetDesignIterationCountAsync()
	{
		GetDesignIterationCountAsync(null);
	}

	public void GetDesignIterationCountAsync(object userState)
	{
		if (onBeginGetDesignIterationCountDelegate == null)
		{
			onBeginGetDesignIterationCountDelegate = OnBeginGetDesignIterationCount;
		}
		if (onEndGetDesignIterationCountDelegate == null)
		{
			onEndGetDesignIterationCountDelegate = OnEndGetDesignIterationCount;
		}
		if (onGetDesignIterationCountCompletedDelegate == null)
		{
			onGetDesignIterationCountCompletedDelegate = OnGetDesignIterationCountCompleted;
		}
		InvokeAsync(onBeginGetDesignIterationCountDelegate, null, onEndGetDesignIterationCountDelegate, onGetDesignIterationCountCompletedDelegate, userState);
	}

	public byte[] GetDisplayObjectBinObjByComponentAndMaterialClassification(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications)
	{
		return base.Channel.GetDisplayObjectBinObjByComponentAndMaterialClassification(whichComponent, whichMaterialClassifications);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetDisplayObjectBinObjByComponentAndMaterialClassification(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetDisplayObjectBinObjByComponentAndMaterialClassification(whichComponent, whichMaterialClassifications, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public byte[] EndGetDisplayObjectBinObjByComponentAndMaterialClassification(IAsyncResult result)
	{
		return base.Channel.EndGetDisplayObjectBinObjByComponentAndMaterialClassification(result);
	}

	private IAsyncResult OnBeginGetDisplayObjectBinObjByComponentAndMaterialClassification(object[] inValues, AsyncCallback callback, object asyncState)
	{
		ComponentsEnum whichComponent = (ComponentsEnum)inValues[0];
		MaterialClassification[] whichMaterialClassifications = (MaterialClassification[])inValues[1];
		return BeginGetDisplayObjectBinObjByComponentAndMaterialClassification(whichComponent, whichMaterialClassifications, callback, asyncState);
	}

	private object[] OnEndGetDisplayObjectBinObjByComponentAndMaterialClassification(IAsyncResult result)
	{
		byte[] array = EndGetDisplayObjectBinObjByComponentAndMaterialClassification(result);
		return new object[1] { array };
	}

	private void OnGetDisplayObjectBinObjByComponentAndMaterialClassificationCompleted(object state)
	{
		if (this.GetDisplayObjectBinObjByComponentAndMaterialClassificationCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetDisplayObjectBinObjByComponentAndMaterialClassificationCompleted(this, new GetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetDisplayObjectBinObjByComponentAndMaterialClassificationAsync(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications)
	{
		GetDisplayObjectBinObjByComponentAndMaterialClassificationAsync(whichComponent, whichMaterialClassifications, null);
	}

	public void GetDisplayObjectBinObjByComponentAndMaterialClassificationAsync(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications, object userState)
	{
		if (onBeginGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate == null)
		{
			onBeginGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate = OnBeginGetDisplayObjectBinObjByComponentAndMaterialClassification;
		}
		if (onEndGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate == null)
		{
			onEndGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate = OnEndGetDisplayObjectBinObjByComponentAndMaterialClassification;
		}
		if (onGetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedDelegate == null)
		{
			onGetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedDelegate = OnGetDisplayObjectBinObjByComponentAndMaterialClassificationCompleted;
		}
		InvokeAsync(onBeginGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate, new object[2] { whichComponent, whichMaterialClassifications }, onEndGetDisplayObjectBinObjByComponentAndMaterialClassificationDelegate, onGetDisplayObjectBinObjByComponentAndMaterialClassificationCompletedDelegate, userState);
	}

	public string GetDocumentFilename()
	{
		return base.Channel.GetDocumentFilename();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetDocumentFilename(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetDocumentFilename(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetDocumentFilename(IAsyncResult result)
	{
		return base.Channel.EndGetDocumentFilename(result);
	}

	private IAsyncResult OnBeginGetDocumentFilename(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetDocumentFilename(callback, asyncState);
	}

	private object[] OnEndGetDocumentFilename(IAsyncResult result)
	{
		string text = EndGetDocumentFilename(result);
		return new object[1] { text };
	}

	private void OnGetDocumentFilenameCompleted(object state)
	{
		if (this.GetDocumentFilenameCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetDocumentFilenameCompleted(this, new GetDocumentFilenameCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetDocumentFilenameAsync()
	{
		GetDocumentFilenameAsync(null);
	}

	public void GetDocumentFilenameAsync(object userState)
	{
		if (onBeginGetDocumentFilenameDelegate == null)
		{
			onBeginGetDocumentFilenameDelegate = OnBeginGetDocumentFilename;
		}
		if (onEndGetDocumentFilenameDelegate == null)
		{
			onEndGetDocumentFilenameDelegate = OnEndGetDocumentFilename;
		}
		if (onGetDocumentFilenameCompletedDelegate == null)
		{
			onGetDocumentFilenameCompletedDelegate = OnGetDocumentFilenameCompleted;
		}
		InvokeAsync(onBeginGetDocumentFilenameDelegate, null, onEndGetDocumentFilenameDelegate, onGetDocumentFilenameCompletedDelegate, userState);
	}

	public int[] GetFeaturedDesignIterations()
	{
		return base.Channel.GetFeaturedDesignIterations();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetFeaturedDesignIterations(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetFeaturedDesignIterations(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int[] EndGetFeaturedDesignIterations(IAsyncResult result)
	{
		return base.Channel.EndGetFeaturedDesignIterations(result);
	}

	private IAsyncResult OnBeginGetFeaturedDesignIterations(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetFeaturedDesignIterations(callback, asyncState);
	}

	private object[] OnEndGetFeaturedDesignIterations(IAsyncResult result)
	{
		int[] array = EndGetFeaturedDesignIterations(result);
		return new object[1] { array };
	}

	private void OnGetFeaturedDesignIterationsCompleted(object state)
	{
		if (this.GetFeaturedDesignIterationsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetFeaturedDesignIterationsCompleted(this, new GetFeaturedDesignIterationsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetFeaturedDesignIterationsAsync()
	{
		GetFeaturedDesignIterationsAsync(null);
	}

	public void GetFeaturedDesignIterationsAsync(object userState)
	{
		if (onBeginGetFeaturedDesignIterationsDelegate == null)
		{
			onBeginGetFeaturedDesignIterationsDelegate = OnBeginGetFeaturedDesignIterations;
		}
		if (onEndGetFeaturedDesignIterationsDelegate == null)
		{
			onEndGetFeaturedDesignIterationsDelegate = OnEndGetFeaturedDesignIterations;
		}
		if (onGetFeaturedDesignIterationsCompletedDelegate == null)
		{
			onGetFeaturedDesignIterationsCompletedDelegate = OnGetFeaturedDesignIterationsCompleted;
		}
		InvokeAsync(onBeginGetFeaturedDesignIterationsDelegate, null, onEndGetFeaturedDesignIterationsDelegate, onGetFeaturedDesignIterationsCompletedDelegate, userState);
	}

	public GemMaterial[] GetGemMaterials()
	{
		return base.Channel.GetGemMaterials();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetGemMaterials(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetGemMaterials(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public GemMaterial[] EndGetGemMaterials(IAsyncResult result)
	{
		return base.Channel.EndGetGemMaterials(result);
	}

	private IAsyncResult OnBeginGetGemMaterials(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetGemMaterials(callback, asyncState);
	}

	private object[] OnEndGetGemMaterials(IAsyncResult result)
	{
		GemMaterial[] array = EndGetGemMaterials(result);
		return new object[1] { array };
	}

	private void OnGetGemMaterialsCompleted(object state)
	{
		if (this.GetGemMaterialsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetGemMaterialsCompleted(this, new GetGemMaterialsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetGemMaterialsAsync()
	{
		GetGemMaterialsAsync(null);
	}

	public void GetGemMaterialsAsync(object userState)
	{
		if (onBeginGetGemMaterialsDelegate == null)
		{
			onBeginGetGemMaterialsDelegate = OnBeginGetGemMaterials;
		}
		if (onEndGetGemMaterialsDelegate == null)
		{
			onEndGetGemMaterialsDelegate = OnEndGetGemMaterials;
		}
		if (onGetGemMaterialsCompletedDelegate == null)
		{
			onGetGemMaterialsCompletedDelegate = OnGetGemMaterialsCompleted;
		}
		InvokeAsync(onBeginGetGemMaterialsDelegate, null, onEndGetGemMaterialsDelegate, onGetGemMaterialsCompletedDelegate, userState);
	}

	public MetalMaterial[] GetMetalMaterials()
	{
		return base.Channel.GetMetalMaterials();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetMetalMaterials(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetMetalMaterials(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public MetalMaterial[] EndGetMetalMaterials(IAsyncResult result)
	{
		return base.Channel.EndGetMetalMaterials(result);
	}

	private IAsyncResult OnBeginGetMetalMaterials(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetMetalMaterials(callback, asyncState);
	}

	private object[] OnEndGetMetalMaterials(IAsyncResult result)
	{
		MetalMaterial[] array = EndGetMetalMaterials(result);
		return new object[1] { array };
	}

	private void OnGetMetalMaterialsCompleted(object state)
	{
		if (this.GetMetalMaterialsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetMetalMaterialsCompleted(this, new GetMetalMaterialsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetMetalMaterialsAsync()
	{
		GetMetalMaterialsAsync(null);
	}

	public void GetMetalMaterialsAsync(object userState)
	{
		if (onBeginGetMetalMaterialsDelegate == null)
		{
			onBeginGetMetalMaterialsDelegate = OnBeginGetMetalMaterials;
		}
		if (onEndGetMetalMaterialsDelegate == null)
		{
			onEndGetMetalMaterialsDelegate = OnEndGetMetalMaterials;
		}
		if (onGetMetalMaterialsCompletedDelegate == null)
		{
			onGetMetalMaterialsCompletedDelegate = OnGetMetalMaterialsCompleted;
		}
		InvokeAsync(onBeginGetMetalMaterialsDelegate, null, onEndGetMetalMaterialsDelegate, onGetMetalMaterialsCompletedDelegate, userState);
	}

	public Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> GetMetalVolumesPerComponent(bool hasSingleMetalCastingCount, int singleMetalCastingCount)
	{
		return base.Channel.GetMetalVolumesPerComponent(hasSingleMetalCastingCount, singleMetalCastingCount);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetMetalVolumesPerComponent(bool hasSingleMetalCastingCount, int singleMetalCastingCount, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetMetalVolumesPerComponent(hasSingleMetalCastingCount, singleMetalCastingCount, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> EndGetMetalVolumesPerComponent(IAsyncResult result)
	{
		return base.Channel.EndGetMetalVolumesPerComponent(result);
	}

	private IAsyncResult OnBeginGetMetalVolumesPerComponent(object[] inValues, AsyncCallback callback, object asyncState)
	{
		bool hasSingleMetalCastingCount = (bool)inValues[0];
		int singleMetalCastingCount = (int)inValues[1];
		return BeginGetMetalVolumesPerComponent(hasSingleMetalCastingCount, singleMetalCastingCount, callback, asyncState);
	}

	private object[] OnEndGetMetalVolumesPerComponent(IAsyncResult result)
	{
		Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> dictionary = EndGetMetalVolumesPerComponent(result);
		return new object[1] { dictionary };
	}

	private void OnGetMetalVolumesPerComponentCompleted(object state)
	{
		if (this.GetMetalVolumesPerComponentCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetMetalVolumesPerComponentCompleted(this, new GetMetalVolumesPerComponentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetMetalVolumesPerComponentAsync(bool hasSingleMetalCastingCount, int singleMetalCastingCount)
	{
		GetMetalVolumesPerComponentAsync(hasSingleMetalCastingCount, singleMetalCastingCount, null);
	}

	public void GetMetalVolumesPerComponentAsync(bool hasSingleMetalCastingCount, int singleMetalCastingCount, object userState)
	{
		if (onBeginGetMetalVolumesPerComponentDelegate == null)
		{
			onBeginGetMetalVolumesPerComponentDelegate = OnBeginGetMetalVolumesPerComponent;
		}
		if (onEndGetMetalVolumesPerComponentDelegate == null)
		{
			onEndGetMetalVolumesPerComponentDelegate = OnEndGetMetalVolumesPerComponent;
		}
		if (onGetMetalVolumesPerComponentCompletedDelegate == null)
		{
			onGetMetalVolumesPerComponentCompletedDelegate = OnGetMetalVolumesPerComponentCompleted;
		}
		InvokeAsync(onBeginGetMetalVolumesPerComponentDelegate, new object[2] { hasSingleMetalCastingCount, singleMetalCastingCount }, onEndGetMetalVolumesPerComponentDelegate, onGetMetalVolumesPerComponentCompletedDelegate, userState);
	}

	public string GetNotesToCustomerForCurrentDesign()
	{
		return base.Channel.GetNotesToCustomerForCurrentDesign();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetNotesToCustomerForCurrentDesign(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetNotesToCustomerForCurrentDesign(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetNotesToCustomerForCurrentDesign(IAsyncResult result)
	{
		return base.Channel.EndGetNotesToCustomerForCurrentDesign(result);
	}

	private IAsyncResult OnBeginGetNotesToCustomerForCurrentDesign(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetNotesToCustomerForCurrentDesign(callback, asyncState);
	}

	private object[] OnEndGetNotesToCustomerForCurrentDesign(IAsyncResult result)
	{
		string text = EndGetNotesToCustomerForCurrentDesign(result);
		return new object[1] { text };
	}

	private void OnGetNotesToCustomerForCurrentDesignCompleted(object state)
	{
		if (this.GetNotesToCustomerForCurrentDesignCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetNotesToCustomerForCurrentDesignCompleted(this, new GetNotesToCustomerForCurrentDesignCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetNotesToCustomerForCurrentDesignAsync()
	{
		GetNotesToCustomerForCurrentDesignAsync(null);
	}

	public void GetNotesToCustomerForCurrentDesignAsync(object userState)
	{
		if (onBeginGetNotesToCustomerForCurrentDesignDelegate == null)
		{
			onBeginGetNotesToCustomerForCurrentDesignDelegate = OnBeginGetNotesToCustomerForCurrentDesign;
		}
		if (onEndGetNotesToCustomerForCurrentDesignDelegate == null)
		{
			onEndGetNotesToCustomerForCurrentDesignDelegate = OnEndGetNotesToCustomerForCurrentDesign;
		}
		if (onGetNotesToCustomerForCurrentDesignCompletedDelegate == null)
		{
			onGetNotesToCustomerForCurrentDesignCompletedDelegate = OnGetNotesToCustomerForCurrentDesignCompleted;
		}
		InvokeAsync(onBeginGetNotesToCustomerForCurrentDesignDelegate, null, onEndGetNotesToCustomerForCurrentDesignDelegate, onGetNotesToCustomerForCurrentDesignCompletedDelegate, userState);
	}

	public int GetNumberofVisibleFreehandParts()
	{
		return base.Channel.GetNumberofVisibleFreehandParts();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetNumberofVisibleFreehandParts(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetNumberofVisibleFreehandParts(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int EndGetNumberofVisibleFreehandParts(IAsyncResult result)
	{
		return base.Channel.EndGetNumberofVisibleFreehandParts(result);
	}

	private IAsyncResult OnBeginGetNumberofVisibleFreehandParts(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetNumberofVisibleFreehandParts(callback, asyncState);
	}

	private object[] OnEndGetNumberofVisibleFreehandParts(IAsyncResult result)
	{
		int num = EndGetNumberofVisibleFreehandParts(result);
		return new object[1] { num };
	}

	private void OnGetNumberofVisibleFreehandPartsCompleted(object state)
	{
		if (this.GetNumberofVisibleFreehandPartsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetNumberofVisibleFreehandPartsCompleted(this, new GetNumberofVisibleFreehandPartsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetNumberofVisibleFreehandPartsAsync()
	{
		GetNumberofVisibleFreehandPartsAsync(null);
	}

	public void GetNumberofVisibleFreehandPartsAsync(object userState)
	{
		if (onBeginGetNumberofVisibleFreehandPartsDelegate == null)
		{
			onBeginGetNumberofVisibleFreehandPartsDelegate = OnBeginGetNumberofVisibleFreehandParts;
		}
		if (onEndGetNumberofVisibleFreehandPartsDelegate == null)
		{
			onEndGetNumberofVisibleFreehandPartsDelegate = OnEndGetNumberofVisibleFreehandParts;
		}
		if (onGetNumberofVisibleFreehandPartsCompletedDelegate == null)
		{
			onGetNumberofVisibleFreehandPartsCompletedDelegate = OnGetNumberofVisibleFreehandPartsCompleted;
		}
		InvokeAsync(onBeginGetNumberofVisibleFreehandPartsDelegate, null, onEndGetNumberofVisibleFreehandPartsDelegate, onGetNumberofVisibleFreehandPartsCompletedDelegate, userState);
	}

	public bool GetPrinterMaterial(out string message)
	{
		return base.Channel.GetPrinterMaterial(out message);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetPrinterMaterial(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetPrinterMaterial(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndGetPrinterMaterial(out string message, IAsyncResult result)
	{
		return base.Channel.EndGetPrinterMaterial(out message, result);
	}

	private IAsyncResult OnBeginGetPrinterMaterial(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetPrinterMaterial(callback, asyncState);
	}

	private object[] OnEndGetPrinterMaterial(IAsyncResult result)
	{
		string message = GetDefaultValueForInitialization<string>();
		bool flag = EndGetPrinterMaterial(out message, result);
		return new object[2] { message, flag };
	}

	private void OnGetPrinterMaterialCompleted(object state)
	{
		if (this.GetPrinterMaterialCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetPrinterMaterialCompleted(this, new GetPrinterMaterialCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetPrinterMaterialAsync()
	{
		GetPrinterMaterialAsync(null);
	}

	public void GetPrinterMaterialAsync(object userState)
	{
		if (onBeginGetPrinterMaterialDelegate == null)
		{
			onBeginGetPrinterMaterialDelegate = OnBeginGetPrinterMaterial;
		}
		if (onEndGetPrinterMaterialDelegate == null)
		{
			onEndGetPrinterMaterialDelegate = OnEndGetPrinterMaterial;
		}
		if (onGetPrinterMaterialCompletedDelegate == null)
		{
			onGetPrinterMaterialCompletedDelegate = OnGetPrinterMaterialCompleted;
		}
		InvokeAsync(onBeginGetPrinterMaterialDelegate, null, onEndGetPrinterMaterialDelegate, onGetPrinterMaterialCompletedDelegate, userState);
	}

	public string GetRhinoSerialCode()
	{
		return base.Channel.GetRhinoSerialCode();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetRhinoSerialCode(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetRhinoSerialCode(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetRhinoSerialCode(IAsyncResult result)
	{
		return base.Channel.EndGetRhinoSerialCode(result);
	}

	private IAsyncResult OnBeginGetRhinoSerialCode(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetRhinoSerialCode(callback, asyncState);
	}

	private object[] OnEndGetRhinoSerialCode(IAsyncResult result)
	{
		string text = EndGetRhinoSerialCode(result);
		return new object[1] { text };
	}

	private void OnGetRhinoSerialCodeCompleted(object state)
	{
		if (this.GetRhinoSerialCodeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetRhinoSerialCodeCompleted(this, new GetRhinoSerialCodeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetRhinoSerialCodeAsync()
	{
		GetRhinoSerialCodeAsync(null);
	}

	public void GetRhinoSerialCodeAsync(object userState)
	{
		if (onBeginGetRhinoSerialCodeDelegate == null)
		{
			onBeginGetRhinoSerialCodeDelegate = OnBeginGetRhinoSerialCode;
		}
		if (onEndGetRhinoSerialCodeDelegate == null)
		{
			onEndGetRhinoSerialCodeDelegate = OnEndGetRhinoSerialCode;
		}
		if (onGetRhinoSerialCodeCompletedDelegate == null)
		{
			onGetRhinoSerialCodeCompletedDelegate = OnGetRhinoSerialCodeCompleted;
		}
		InvokeAsync(onBeginGetRhinoSerialCodeDelegate, null, onEndGetRhinoSerialCodeDelegate, onGetRhinoSerialCodeCompletedDelegate, userState);
	}

	public int GetRhinoThreadId()
	{
		return base.Channel.GetRhinoThreadId();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetRhinoThreadId(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetRhinoThreadId(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int EndGetRhinoThreadId(IAsyncResult result)
	{
		return base.Channel.EndGetRhinoThreadId(result);
	}

	private IAsyncResult OnBeginGetRhinoThreadId(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetRhinoThreadId(callback, asyncState);
	}

	private object[] OnEndGetRhinoThreadId(IAsyncResult result)
	{
		int num = EndGetRhinoThreadId(result);
		return new object[1] { num };
	}

	private void OnGetRhinoThreadIdCompleted(object state)
	{
		if (this.GetRhinoThreadIdCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetRhinoThreadIdCompleted(this, new GetRhinoThreadIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetRhinoThreadIdAsync()
	{
		GetRhinoThreadIdAsync(null);
	}

	public void GetRhinoThreadIdAsync(object userState)
	{
		if (onBeginGetRhinoThreadIdDelegate == null)
		{
			onBeginGetRhinoThreadIdDelegate = OnBeginGetRhinoThreadId;
		}
		if (onEndGetRhinoThreadIdDelegate == null)
		{
			onEndGetRhinoThreadIdDelegate = OnEndGetRhinoThreadId;
		}
		if (onGetRhinoThreadIdCompletedDelegate == null)
		{
			onGetRhinoThreadIdCompletedDelegate = OnGetRhinoThreadIdCompleted;
		}
		InvokeAsync(onBeginGetRhinoThreadIdDelegate, null, onEndGetRhinoThreadIdDelegate, onGetRhinoThreadIdCompletedDelegate, userState);
	}

	public IntPtr GetRhinoWindowHandle()
	{
		return base.Channel.GetRhinoWindowHandle();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetRhinoWindowHandle(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetRhinoWindowHandle(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IntPtr EndGetRhinoWindowHandle(IAsyncResult result)
	{
		return base.Channel.EndGetRhinoWindowHandle(result);
	}

	private IAsyncResult OnBeginGetRhinoWindowHandle(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetRhinoWindowHandle(callback, asyncState);
	}

	private object[] OnEndGetRhinoWindowHandle(IAsyncResult result)
	{
		IntPtr intPtr = EndGetRhinoWindowHandle(result);
		return new object[1] { intPtr };
	}

	private void OnGetRhinoWindowHandleCompleted(object state)
	{
		if (this.GetRhinoWindowHandleCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetRhinoWindowHandleCompleted(this, new GetRhinoWindowHandleCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetRhinoWindowHandleAsync()
	{
		GetRhinoWindowHandleAsync(null);
	}

	public void GetRhinoWindowHandleAsync(object userState)
	{
		if (onBeginGetRhinoWindowHandleDelegate == null)
		{
			onBeginGetRhinoWindowHandleDelegate = OnBeginGetRhinoWindowHandle;
		}
		if (onEndGetRhinoWindowHandleDelegate == null)
		{
			onEndGetRhinoWindowHandleDelegate = OnEndGetRhinoWindowHandle;
		}
		if (onGetRhinoWindowHandleCompletedDelegate == null)
		{
			onGetRhinoWindowHandleCompletedDelegate = OnGetRhinoWindowHandleCompleted;
		}
		InvokeAsync(onBeginGetRhinoWindowHandleDelegate, null, onEndGetRhinoWindowHandleDelegate, onGetRhinoWindowHandleCompletedDelegate, userState);
	}

	public int[] GetSavedJobs()
	{
		return base.Channel.GetSavedJobs();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetSavedJobs(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetSavedJobs(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int[] EndGetSavedJobs(IAsyncResult result)
	{
		return base.Channel.EndGetSavedJobs(result);
	}

	private IAsyncResult OnBeginGetSavedJobs(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetSavedJobs(callback, asyncState);
	}

	private object[] OnEndGetSavedJobs(IAsyncResult result)
	{
		int[] array = EndGetSavedJobs(result);
		return new object[1] { array };
	}

	private void OnGetSavedJobsCompleted(object state)
	{
		if (this.GetSavedJobsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetSavedJobsCompleted(this, new GetSavedJobsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetSavedJobsAsync()
	{
		GetSavedJobsAsync(null);
	}

	public void GetSavedJobsAsync(object userState)
	{
		if (onBeginGetSavedJobsDelegate == null)
		{
			onBeginGetSavedJobsDelegate = OnBeginGetSavedJobs;
		}
		if (onEndGetSavedJobsDelegate == null)
		{
			onEndGetSavedJobsDelegate = OnEndGetSavedJobs;
		}
		if (onGetSavedJobsCompletedDelegate == null)
		{
			onGetSavedJobsCompletedDelegate = OnGetSavedJobsCompleted;
		}
		InvokeAsync(onBeginGetSavedJobsDelegate, null, onEndGetSavedJobsDelegate, onGetSavedJobsCompletedDelegate, userState);
	}

	public string GetSPINumberFromUserData()
	{
		return base.Channel.GetSPINumberFromUserData();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetSPINumberFromUserData(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetSPINumberFromUserData(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGetSPINumberFromUserData(IAsyncResult result)
	{
		return base.Channel.EndGetSPINumberFromUserData(result);
	}

	private IAsyncResult OnBeginGetSPINumberFromUserData(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetSPINumberFromUserData(callback, asyncState);
	}

	private object[] OnEndGetSPINumberFromUserData(IAsyncResult result)
	{
		string text = EndGetSPINumberFromUserData(result);
		return new object[1] { text };
	}

	private void OnGetSPINumberFromUserDataCompleted(object state)
	{
		if (this.GetSPINumberFromUserDataCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetSPINumberFromUserDataCompleted(this, new GetSPINumberFromUserDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetSPINumberFromUserDataAsync()
	{
		GetSPINumberFromUserDataAsync(null);
	}

	public void GetSPINumberFromUserDataAsync(object userState)
	{
		if (onBeginGetSPINumberFromUserDataDelegate == null)
		{
			onBeginGetSPINumberFromUserDataDelegate = OnBeginGetSPINumberFromUserData;
		}
		if (onEndGetSPINumberFromUserDataDelegate == null)
		{
			onEndGetSPINumberFromUserDataDelegate = OnEndGetSPINumberFromUserData;
		}
		if (onGetSPINumberFromUserDataCompletedDelegate == null)
		{
			onGetSPINumberFromUserDataCompletedDelegate = OnGetSPINumberFromUserDataCompleted;
		}
		InvokeAsync(onBeginGetSPINumberFromUserDataDelegate, null, onEndGetSPINumberFromUserDataDelegate, onGetSPINumberFromUserDataCompletedDelegate, userState);
	}

	public BitmapSourceByteArrayInfo GetThumbnailFromFile(string filename)
	{
		return base.Channel.GetThumbnailFromFile(filename);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetThumbnailFromFile(string filename, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetThumbnailFromFile(filename, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public BitmapSourceByteArrayInfo EndGetThumbnailFromFile(IAsyncResult result)
	{
		return base.Channel.EndGetThumbnailFromFile(result);
	}

	private IAsyncResult OnBeginGetThumbnailFromFile(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string filename = (string)inValues[0];
		return BeginGetThumbnailFromFile(filename, callback, asyncState);
	}

	private object[] OnEndGetThumbnailFromFile(IAsyncResult result)
	{
		BitmapSourceByteArrayInfo bitmapSourceByteArrayInfo = EndGetThumbnailFromFile(result);
		return new object[1] { bitmapSourceByteArrayInfo };
	}

	private void OnGetThumbnailFromFileCompleted(object state)
	{
		if (this.GetThumbnailFromFileCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetThumbnailFromFileCompleted(this, new GetThumbnailFromFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetThumbnailFromFileAsync(string filename)
	{
		GetThumbnailFromFileAsync(filename, null);
	}

	public void GetThumbnailFromFileAsync(string filename, object userState)
	{
		if (onBeginGetThumbnailFromFileDelegate == null)
		{
			onBeginGetThumbnailFromFileDelegate = OnBeginGetThumbnailFromFile;
		}
		if (onEndGetThumbnailFromFileDelegate == null)
		{
			onEndGetThumbnailFromFileDelegate = OnEndGetThumbnailFromFile;
		}
		if (onGetThumbnailFromFileCompletedDelegate == null)
		{
			onGetThumbnailFromFileCompletedDelegate = OnGetThumbnailFromFileCompleted;
		}
		InvokeAsync(onBeginGetThumbnailFromFileDelegate, new object[1] { filename }, onEndGetThumbnailFromFileDelegate, onGetThumbnailFromFileCompletedDelegate, userState);
	}

	public ComponentsEnum[] GetVisibleComponents()
	{
		return base.Channel.GetVisibleComponents();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetVisibleComponents(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetVisibleComponents(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public ComponentsEnum[] EndGetVisibleComponents(IAsyncResult result)
	{
		return base.Channel.EndGetVisibleComponents(result);
	}

	private IAsyncResult OnBeginGetVisibleComponents(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetVisibleComponents(callback, asyncState);
	}

	private object[] OnEndGetVisibleComponents(IAsyncResult result)
	{
		ComponentsEnum[] array = EndGetVisibleComponents(result);
		return new object[1] { array };
	}

	private void OnGetVisibleComponentsCompleted(object state)
	{
		if (this.GetVisibleComponentsCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetVisibleComponentsCompleted(this, new GetVisibleComponentsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetVisibleComponentsAsync()
	{
		GetVisibleComponentsAsync(null);
	}

	public void GetVisibleComponentsAsync(object userState)
	{
		if (onBeginGetVisibleComponentsDelegate == null)
		{
			onBeginGetVisibleComponentsDelegate = OnBeginGetVisibleComponents;
		}
		if (onEndGetVisibleComponentsDelegate == null)
		{
			onEndGetVisibleComponentsDelegate = OnEndGetVisibleComponents;
		}
		if (onGetVisibleComponentsCompletedDelegate == null)
		{
			onGetVisibleComponentsCompletedDelegate = OnGetVisibleComponentsCompleted;
		}
		InvokeAsync(onBeginGetVisibleComponentsDelegate, null, onEndGetVisibleComponentsDelegate, onGetVisibleComponentsCompletedDelegate, userState);
	}

	public void GraphItemAddUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue)
	{
		base.Channel.GraphItemAddUserDataValue(blockInstanceGuid, typeName, keyName, value, replaceCurrentValue);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGraphItemAddUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGraphItemAddUserDataValue(blockInstanceGuid, typeName, keyName, value, replaceCurrentValue, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void EndGraphItemAddUserDataValue(IAsyncResult result)
	{
		base.Channel.EndGraphItemAddUserDataValue(result);
	}

	private IAsyncResult OnBeginGraphItemAddUserDataValue(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid blockInstanceGuid = (Guid)inValues[0];
		string typeName = (string)inValues[1];
		string keyName = (string)inValues[2];
		string value = (string)inValues[3];
		bool replaceCurrentValue = (bool)inValues[4];
		return BeginGraphItemAddUserDataValue(blockInstanceGuid, typeName, keyName, value, replaceCurrentValue, callback, asyncState);
	}

	private object[] OnEndGraphItemAddUserDataValue(IAsyncResult result)
	{
		EndGraphItemAddUserDataValue(result);
		return null;
	}

	private void OnGraphItemAddUserDataValueCompleted(object state)
	{
		if (this.GraphItemAddUserDataValueCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GraphItemAddUserDataValueCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GraphItemAddUserDataValueAsync(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue)
	{
		GraphItemAddUserDataValueAsync(blockInstanceGuid, typeName, keyName, value, replaceCurrentValue, null);
	}

	public void GraphItemAddUserDataValueAsync(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue, object userState)
	{
		if (onBeginGraphItemAddUserDataValueDelegate == null)
		{
			onBeginGraphItemAddUserDataValueDelegate = OnBeginGraphItemAddUserDataValue;
		}
		if (onEndGraphItemAddUserDataValueDelegate == null)
		{
			onEndGraphItemAddUserDataValueDelegate = OnEndGraphItemAddUserDataValue;
		}
		if (onGraphItemAddUserDataValueCompletedDelegate == null)
		{
			onGraphItemAddUserDataValueCompletedDelegate = OnGraphItemAddUserDataValueCompleted;
		}
		InvokeAsync(onBeginGraphItemAddUserDataValueDelegate, new object[5] { blockInstanceGuid, typeName, keyName, value, replaceCurrentValue }, onEndGraphItemAddUserDataValueDelegate, onGraphItemAddUserDataValueCompletedDelegate, userState);
	}

	public string GraphItemFindUserDataValue(Guid blockInstanceGuid, string typeName, string keyName)
	{
		return base.Channel.GraphItemFindUserDataValue(blockInstanceGuid, typeName, keyName);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGraphItemFindUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGraphItemFindUserDataValue(blockInstanceGuid, typeName, keyName, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string EndGraphItemFindUserDataValue(IAsyncResult result)
	{
		return base.Channel.EndGraphItemFindUserDataValue(result);
	}

	private IAsyncResult OnBeginGraphItemFindUserDataValue(object[] inValues, AsyncCallback callback, object asyncState)
	{
		Guid blockInstanceGuid = (Guid)inValues[0];
		string typeName = (string)inValues[1];
		string keyName = (string)inValues[2];
		return BeginGraphItemFindUserDataValue(blockInstanceGuid, typeName, keyName, callback, asyncState);
	}

	private object[] OnEndGraphItemFindUserDataValue(IAsyncResult result)
	{
		string text = EndGraphItemFindUserDataValue(result);
		return new object[1] { text };
	}

	private void OnGraphItemFindUserDataValueCompleted(object state)
	{
		if (this.GraphItemFindUserDataValueCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GraphItemFindUserDataValueCompleted(this, new GraphItemFindUserDataValueCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GraphItemFindUserDataValueAsync(Guid blockInstanceGuid, string typeName, string keyName)
	{
		GraphItemFindUserDataValueAsync(blockInstanceGuid, typeName, keyName, null);
	}

	public void GraphItemFindUserDataValueAsync(Guid blockInstanceGuid, string typeName, string keyName, object userState)
	{
		if (onBeginGraphItemFindUserDataValueDelegate == null)
		{
			onBeginGraphItemFindUserDataValueDelegate = OnBeginGraphItemFindUserDataValue;
		}
		if (onEndGraphItemFindUserDataValueDelegate == null)
		{
			onEndGraphItemFindUserDataValueDelegate = OnEndGraphItemFindUserDataValue;
		}
		if (onGraphItemFindUserDataValueCompletedDelegate == null)
		{
			onGraphItemFindUserDataValueCompletedDelegate = OnGraphItemFindUserDataValueCompleted;
		}
		InvokeAsync(onBeginGraphItemFindUserDataValueDelegate, new object[3] { blockInstanceGuid, typeName, keyName }, onEndGraphItemFindUserDataValueDelegate, onGraphItemFindUserDataValueCompletedDelegate, userState);
	}

	public UIControl[] GraphRedoChange()
	{
		return base.Channel.GraphRedoChange();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGraphRedoChange(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGraphRedoChange(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public UIControl[] EndGraphRedoChange(IAsyncResult result)
	{
		return base.Channel.EndGraphRedoChange(result);
	}

	private IAsyncResult OnBeginGraphRedoChange(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGraphRedoChange(callback, asyncState);
	}

	private object[] OnEndGraphRedoChange(IAsyncResult result)
	{
		UIControl[] array = EndGraphRedoChange(result);
		return new object[1] { array };
	}

	private void OnGraphRedoChangeCompleted(object state)
	{
		if (this.GraphRedoChangeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GraphRedoChangeCompleted(this, new GraphRedoChangeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GraphRedoChangeAsync()
	{
		GraphRedoChangeAsync(null);
	}

	public void GraphRedoChangeAsync(object userState)
	{
		if (onBeginGraphRedoChangeDelegate == null)
		{
			onBeginGraphRedoChangeDelegate = OnBeginGraphRedoChange;
		}
		if (onEndGraphRedoChangeDelegate == null)
		{
			onEndGraphRedoChangeDelegate = OnEndGraphRedoChange;
		}
		if (onGraphRedoChangeCompletedDelegate == null)
		{
			onGraphRedoChangeCompletedDelegate = OnGraphRedoChangeCompleted;
		}
		InvokeAsync(onBeginGraphRedoChangeDelegate, null, onEndGraphRedoChangeDelegate, onGraphRedoChangeCompletedDelegate, userState);
	}
}
