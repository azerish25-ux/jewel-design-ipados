using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net.Cache;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.CAD;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Pricing;
using GV.CounterSketch.Shared.Pricing.Enums;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "RhinoServicesReference.IRhinoServices", CallbackContract = typeof(IRhinoServicesCallback))]
public interface IRhinoServices
{
	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ToggleFreehandCommonCenterMode", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleFreehandCommonCenterModeResponse")]
	void ToggleFreehandCommonCenterMode(bool isFreehandCommonCenterModeEnabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ToggleFreehandCommonCenterMode", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleFreehandCommonCenterModeResponse")]
	IAsyncResult BeginToggleFreehandCommonCenterMode(bool isFreehandCommonCenterModeEnabled, AsyncCallback callback, object asyncState);

	void EndToggleFreehandCommonCenterMode(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ToggleFreehandVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleFreehandVisibilityResponse")]
	void ToggleFreehandVisibility(bool isVisible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ToggleFreehandVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleFreehandVisibilityResponse")]
	IAsyncResult BeginToggleFreehandVisibility(bool isVisible, AsyncCallback callback, object asyncState);

	void EndToggleFreehandVisibility(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetFreehandComponentVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/GetFreehandComponentVisibilityResponse")]
	bool GetFreehandComponentVisibility();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetFreehandComponentVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/GetFreehandComponentVisibilityResponse")]
	IAsyncResult BeginGetFreehandComponentVisibility(AsyncCallback callback, object asyncState);

	bool EndGetFreehandComponentVisibility(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetManualSizedStones", ReplyAction = "http://tempuri.org/IRhinoServices/GetManualSizedStonesResponse")]
	StudioGem[] GetManualSizedStones();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetManualSizedStones", ReplyAction = "http://tempuri.org/IRhinoServices/GetManualSizedStonesResponse")]
	IAsyncResult BeginGetManualSizedStones(AsyncCallback callback, object asyncState);

	StudioGem[] EndGetManualSizedStones(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ToggleQuarterCaratGemRounding", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleQuarterCaratGemRoundingResponse")]
	void ToggleQuarterCaratGemRounding(bool isQuarterCaratRoundingEnabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ToggleQuarterCaratGemRounding", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleQuarterCaratGemRoundingResponse")]
	IAsyncResult BeginToggleQuarterCaratGemRounding(bool isQuarterCaratRoundingEnabled, AsyncCallback callback, object asyncState);

	void EndToggleQuarterCaratGemRounding(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ToggleViewportControls", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleViewportControlsResponse")]
	void ToggleViewportControls(bool isVisible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ToggleViewportControls", ReplyAction = "http://tempuri.org/IRhinoServices/ToggleViewportControlsResponse")]
	IAsyncResult BeginToggleViewportControls(bool isVisible, AsyncCallback callback, object asyncState);

	void EndToggleViewportControls(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/TurnHandOff", ReplyAction = "http://tempuri.org/IRhinoServices/TurnHandOffResponse")]
	void TurnHandOff();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/TurnHandOff", ReplyAction = "http://tempuri.org/IRhinoServices/TurnHandOffResponse")]
	IAsyncResult BeginTurnHandOff(AsyncCallback callback, object asyncState);

	void EndTurnHandOff(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateCurveObjectFromUI", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateCurveObjectFromUIResponse")]
	void UpdateCurveObjectFromUI(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, double[] points, bool mouseDown);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateCurveObjectFromUI", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateCurveObjectFromUIResponse")]
	IAsyncResult BeginUpdateCurveObjectFromUI(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, double[] points, bool mouseDown, AsyncCallback callback, object asyncState);

	void EndUpdateCurveObjectFromUI(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateFirmwareAndActivate", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateFirmwareAndActivateResponse")]
	bool UpdateFirmwareAndActivate(out string sError, bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateFirmwareAndActivate", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateFirmwareAndActivateResponse")]
	IAsyncResult BeginUpdateFirmwareAndActivate(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany, AsyncCallback callback, object asyncState);

	bool EndUpdateFirmwareAndActivate(out string sError, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateImportGroupPartListGroupAssignment", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateImportGroupPartListGroupAssignmentResponse")]
	void UpdateImportGroupPartListGroupAssignment(Guid[] importGroupIdentifiers, string groupName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateImportGroupPartListGroupAssignment", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateImportGroupPartListGroupAssignmentResponse")]
	IAsyncResult BeginUpdateImportGroupPartListGroupAssignment(Guid[] importGroupIdentifiers, string groupName, AsyncCallback callback, object asyncState);

	void EndUpdateImportGroupPartListGroupAssignment(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateImportGroupSortOrder", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateImportGroupSortOrderResponse")]
	void UpdateImportGroupSortOrder(Guid[] importGroupGuids);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateImportGroupSortOrder", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateImportGroupSortOrderResponse")]
	IAsyncResult BeginUpdateImportGroupSortOrder(Guid[] importGroupGuids, AsyncCallback callback, object asyncState);

	void EndUpdateImportGroupSortOrder(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateRhinoAppearanceMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateRhinoAppearanceMaterialsResponse")]
	void UpdateRhinoAppearanceMaterials(string skinName, string polishName, bool isRenderLayDesignFlat);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateRhinoAppearanceMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateRhinoAppearanceMaterialsResponse")]
	IAsyncResult BeginUpdateRhinoAppearanceMaterials(string skinName, string polishName, bool isRenderLayDesignFlat, AsyncCallback callback, object asyncState);

	void EndUpdateRhinoAppearanceMaterials(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateRhinoHand", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateRhinoHandResponse")]
	void UpdateRhinoHand(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateRhinoHand", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateRhinoHandResponse")]
	IAsyncResult BeginUpdateRhinoHand(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset, AsyncCallback callback, object asyncState);

	void EndUpdateRhinoHand(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateStudioUICallbackChannel", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateStudioUICallbackChannelResponse")]
	void UpdateStudioUICallbackChannel();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateStudioUICallbackChannel", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateStudioUICallbackChannelResponse")]
	IAsyncResult BeginUpdateStudioUICallbackChannel(AsyncCallback callback, object asyncState);

	void EndUpdateStudioUICallbackChannel(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateUIControl", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateUIControlResponse")]
	[ServiceKnownType(typeof(StudioGem[]))]
	[ServiceKnownType(typeof(StudioGem))]
	[ServiceKnownType(typeof(StudioGemColorQuality))]
	[ServiceKnownType(typeof(StudioGemShape))]
	[ServiceKnownType(typeof(StudioGemType))]
	[ServiceKnownType(typeof(StudioSetType))]
	[ServiceKnownType(typeof(GemMaterial))]
	[ServiceKnownType(typeof(ComponentsEnum[]))]
	[ServiceKnownType(typeof(ComponentsEnum))]
	[ServiceKnownType(typeof(HeadStyleListItemIndexPair[]))]
	[ServiceKnownType(typeof(HeadStyleListItemIndexPair))]
	[ServiceKnownType(typeof(HeadStyle))]
	[ServiceKnownType(typeof(ListItemIndexPair[]))]
	[ServiceKnownType(typeof(ListItemIndexPair))]
	[ServiceKnownType(typeof(ValueListItem))]
	[ServiceKnownType(typeof(GemActions))]
	[ServiceKnownType(typeof(GemMaterial[]))]
	[ServiceKnownType(typeof(MetalMaterial[]))]
	[ServiceKnownType(typeof(MetalMaterial))]
	[ServiceKnownType(typeof(NurbsCurve))]
	[ServiceKnownType(typeof(Point4D[]))]
	[ServiceKnownType(typeof(Point4D))]
	[ServiceKnownType(typeof(BoundingBox))]
	[ServiceKnownType(typeof(Point3D))]
	[ServiceKnownType(typeof(Point2D[]))]
	[ServiceKnownType(typeof(Point2D))]
	[ServiceKnownType(typeof(double[]))]
	[ServiceKnownType(typeof(Guid[]))]
	[ServiceKnownType(typeof(string[]))]
	[ServiceKnownType(typeof(byte[][]))]
	[ServiceKnownType(typeof(int[]))]
	[ServiceKnownType(typeof(Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double>))]
	[ServiceKnownType(typeof(UIControlStackLocation))]
	[ServiceKnownType(typeof(UIControlStatus))]
	[ServiceKnownType(typeof(UIControlType))]
	[ServiceKnownType(typeof(DataItem[]))]
	[ServiceKnownType(typeof(DataItem))]
	[ServiceKnownType(typeof(BitmapSourceByteArrayInfo))]
	[ServiceKnownType(typeof(UIControl[]))]
	[ServiceKnownType(typeof(System.Windows.Point[]))]
	[ServiceKnownType(typeof(System.Windows.Point))]
	[ServiceKnownType(typeof(Freezable))]
	[ServiceKnownType(typeof(DependencyObject))]
	[ServiceKnownType(typeof(Int32Rect))]
	[ServiceKnownType(typeof(GemTableSizeInfo[]))]
	[ServiceKnownType(typeof(GemTableSizeInfo))]
	[ServiceKnownType(typeof(OrderGemData[]))]
	[ServiceKnownType(typeof(OrderGemData))]
	[ServiceKnownType(typeof(ImportGroup[]))]
	[ServiceKnownType(typeof(ImportGroup))]
	[ServiceKnownType(typeof(System.Drawing.Color))]
	[ServiceKnownType(typeof(System.Drawing.Size))]
	[ServiceKnownType(typeof(Bitmap))]
	[ServiceKnownType(typeof(Image))]
	[ServiceKnownType(typeof(IntPtr))]
	[ServiceKnownType(typeof(Tuple<string, string, string>[]))]
	[ServiceKnownType(typeof(Tuple<string, string, string>))]
	[ServiceKnownType(typeof(Tuple<ComponentsEnum, MaterialClassification>))]
	[ServiceKnownType(typeof(MulticastDelegate))]
	[ServiceKnownType(typeof(Delegate))]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(KeyValuePair<int, string>[]))]
	[ServiceKnownType(typeof(KeyValuePair<int, string>))]
	[ServiceKnownType(typeof(KeyValuePair<Guid, Bitmap>))]
	[ServiceKnownType(typeof(MaterialClassification[]))]
	[ServiceKnownType(typeof(MaterialClassification))]
	[ServiceKnownType(typeof(PropertyChangedEventHandler))]
	[ServiceKnownType(typeof(BitmapImage))]
	[ServiceKnownType(typeof(BitmapSource))]
	[ServiceKnownType(typeof(BitmapCacheOption))]
	[ServiceKnownType(typeof(BitmapCreateOptions))]
	[ServiceKnownType(typeof(Rotation))]
	[ServiceKnownType(typeof(ImageSource))]
	[ServiceKnownType(typeof(Animatable))]
	[ServiceKnownType(typeof(DispatcherObject))]
	[ServiceKnownType(typeof(Stream))]
	[ServiceKnownType(typeof(RequestCachePolicy))]
	[ServiceKnownType(typeof(Key))]
	UIControl[] UpdateUIControl(UIControl uiControl);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateUIControl", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateUIControlResponse")]
	IAsyncResult BeginUpdateUIControl(UIControl uiControl, AsyncCallback callback, object asyncState);

	UIControl[] EndUpdateUIControl(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateUIControlQuick", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateUIControlQuickResponse")]
	[ServiceKnownType(typeof(StudioGem[]))]
	[ServiceKnownType(typeof(StudioGem))]
	[ServiceKnownType(typeof(StudioGemColorQuality))]
	[ServiceKnownType(typeof(StudioGemShape))]
	[ServiceKnownType(typeof(StudioGemType))]
	[ServiceKnownType(typeof(StudioSetType))]
	[ServiceKnownType(typeof(GemMaterial))]
	[ServiceKnownType(typeof(ComponentsEnum[]))]
	[ServiceKnownType(typeof(ComponentsEnum))]
	[ServiceKnownType(typeof(HeadStyleListItemIndexPair[]))]
	[ServiceKnownType(typeof(HeadStyleListItemIndexPair))]
	[ServiceKnownType(typeof(HeadStyle))]
	[ServiceKnownType(typeof(ListItemIndexPair[]))]
	[ServiceKnownType(typeof(ListItemIndexPair))]
	[ServiceKnownType(typeof(ValueListItem))]
	[ServiceKnownType(typeof(GemActions))]
	[ServiceKnownType(typeof(GemMaterial[]))]
	[ServiceKnownType(typeof(MetalMaterial[]))]
	[ServiceKnownType(typeof(MetalMaterial))]
	[ServiceKnownType(typeof(NurbsCurve))]
	[ServiceKnownType(typeof(Point4D[]))]
	[ServiceKnownType(typeof(Point4D))]
	[ServiceKnownType(typeof(BoundingBox))]
	[ServiceKnownType(typeof(Point3D))]
	[ServiceKnownType(typeof(Point2D[]))]
	[ServiceKnownType(typeof(Point2D))]
	[ServiceKnownType(typeof(double[]))]
	[ServiceKnownType(typeof(Guid[]))]
	[ServiceKnownType(typeof(string[]))]
	[ServiceKnownType(typeof(byte[][]))]
	[ServiceKnownType(typeof(int[]))]
	[ServiceKnownType(typeof(Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double>))]
	[ServiceKnownType(typeof(UIControlStackLocation))]
	[ServiceKnownType(typeof(UIControlStatus))]
	[ServiceKnownType(typeof(UIControlType))]
	[ServiceKnownType(typeof(DataItem[]))]
	[ServiceKnownType(typeof(DataItem))]
	[ServiceKnownType(typeof(BitmapSourceByteArrayInfo))]
	[ServiceKnownType(typeof(UIControl[]))]
	[ServiceKnownType(typeof(System.Windows.Point[]))]
	[ServiceKnownType(typeof(System.Windows.Point))]
	[ServiceKnownType(typeof(Freezable))]
	[ServiceKnownType(typeof(DependencyObject))]
	[ServiceKnownType(typeof(Int32Rect))]
	[ServiceKnownType(typeof(GemTableSizeInfo[]))]
	[ServiceKnownType(typeof(GemTableSizeInfo))]
	[ServiceKnownType(typeof(OrderGemData[]))]
	[ServiceKnownType(typeof(OrderGemData))]
	[ServiceKnownType(typeof(ImportGroup[]))]
	[ServiceKnownType(typeof(ImportGroup))]
	[ServiceKnownType(typeof(System.Drawing.Color))]
	[ServiceKnownType(typeof(System.Drawing.Size))]
	[ServiceKnownType(typeof(Bitmap))]
	[ServiceKnownType(typeof(Image))]
	[ServiceKnownType(typeof(IntPtr))]
	[ServiceKnownType(typeof(Tuple<string, string, string>[]))]
	[ServiceKnownType(typeof(Tuple<string, string, string>))]
	[ServiceKnownType(typeof(Tuple<ComponentsEnum, MaterialClassification>))]
	[ServiceKnownType(typeof(MulticastDelegate))]
	[ServiceKnownType(typeof(Delegate))]
	[ServiceKnownType(typeof(MarshalByRefObject))]
	[ServiceKnownType(typeof(KeyValuePair<int, string>[]))]
	[ServiceKnownType(typeof(KeyValuePair<int, string>))]
	[ServiceKnownType(typeof(KeyValuePair<Guid, Bitmap>))]
	[ServiceKnownType(typeof(MaterialClassification[]))]
	[ServiceKnownType(typeof(MaterialClassification))]
	[ServiceKnownType(typeof(PropertyChangedEventHandler))]
	[ServiceKnownType(typeof(BitmapImage))]
	[ServiceKnownType(typeof(BitmapSource))]
	[ServiceKnownType(typeof(BitmapCacheOption))]
	[ServiceKnownType(typeof(BitmapCreateOptions))]
	[ServiceKnownType(typeof(Rotation))]
	[ServiceKnownType(typeof(ImageSource))]
	[ServiceKnownType(typeof(Animatable))]
	[ServiceKnownType(typeof(DispatcherObject))]
	[ServiceKnownType(typeof(Stream))]
	[ServiceKnownType(typeof(RequestCachePolicy))]
	[ServiceKnownType(typeof(Key))]
	UIControl[] UpdateUIControlQuick(UIControl uiControl);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateUIControlQuick", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateUIControlQuickResponse")]
	IAsyncResult BeginUpdateUIControlQuick(UIControl uiControl, AsyncCallback callback, object asyncState);

	UIControl[] EndUpdateUIControlQuick(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateValueChooserFromUI", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateValueChooserFromUIResponse")]
	void UpdateValueChooserFromUI(Guid blockInstanceGuid);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateValueChooserFromUI", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateValueChooserFromUIResponse")]
	IAsyncResult BeginUpdateValueChooserFromUI(Guid blockInstanceGuid, AsyncCallback callback, object asyncState);

	void EndUpdateValueChooserFromUI(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/UpdateViewPortHandleDrawScale", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateViewPortHandleDrawScaleResponse")]
	void UpdateViewPortHandleDrawScale(double scaleFactor);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/UpdateViewPortHandleDrawScale", ReplyAction = "http://tempuri.org/IRhinoServices/UpdateViewPortHandleDrawScaleResponse")]
	IAsyncResult BeginUpdateViewPortHandleDrawScale(double scaleFactor, AsyncCallback callback, object asyncState);

	void EndUpdateViewPortHandleDrawScale(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ViewportSetBackgroundAndColor", ReplyAction = "http://tempuri.org/IRhinoServices/ViewportSetBackgroundAndColorResponse")]
	void ViewportSetBackgroundAndColor(string backgroundColor, string backgroundGraphicPath);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ViewportSetBackgroundAndColor", ReplyAction = "http://tempuri.org/IRhinoServices/ViewportSetBackgroundAndColorResponse")]
	IAsyncResult BeginViewportSetBackgroundAndColor(string backgroundColor, string backgroundGraphicPath, AsyncCallback callback, object asyncState);

	void EndViewportSetBackgroundAndColor(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ViewportSetHighlightColor", ReplyAction = "http://tempuri.org/IRhinoServices/ViewportSetHighlightColorResponse")]
	void ViewportSetHighlightColor(System.Drawing.Color color);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ViewportSetHighlightColor", ReplyAction = "http://tempuri.org/IRhinoServices/ViewportSetHighlightColorResponse")]
	IAsyncResult BeginViewportSetHighlightColor(System.Drawing.Color color, AsyncCallback callback, object asyncState);

	void EndViewportSetHighlightColor(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ViewportSetTargetAndCameraLocation", ReplyAction = "http://tempuri.org/IRhinoServices/ViewportSetTargetAndCameraLocationResponse")]
	void ViewportSetTargetAndCameraLocation(double x, double y, double z);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ViewportSetTargetAndCameraLocation", ReplyAction = "http://tempuri.org/IRhinoServices/ViewportSetTargetAndCameraLocationResponse")]
	IAsyncResult BeginViewportSetTargetAndCameraLocation(double x, double y, double z, AsyncCallback callback, object asyncState);

	void EndViewportSetTargetAndCameraLocation(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/WatcherSetEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/WatcherSetEnabledResponse")]
	void WatcherSetEnabled(bool enabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/WatcherSetEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/WatcherSetEnabledResponse")]
	IAsyncResult BeginWatcherSetEnabled(bool enabled, AsyncCallback callback, object asyncState);

	void EndWatcherSetEnabled(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GraphItemRemoveUserDataValue", ReplyAction = "http://tempuri.org/IRhinoServices/GraphItemRemoveUserDataValueResponse")]
	void GraphItemRemoveUserDataValue(Guid blockInstanceGuid, string typeName, string keyName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GraphItemRemoveUserDataValue", ReplyAction = "http://tempuri.org/IRhinoServices/GraphItemRemoveUserDataValueResponse")]
	IAsyncResult BeginGraphItemRemoveUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, AsyncCallback callback, object asyncState);

	void EndGraphItemRemoveUserDataValue(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GraphUndoChange", ReplyAction = "http://tempuri.org/IRhinoServices/GraphUndoChangeResponse")]
	UIControl[] GraphUndoChange();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GraphUndoChange", ReplyAction = "http://tempuri.org/IRhinoServices/GraphUndoChangeResponse")]
	IAsyncResult BeginGraphUndoChange(AsyncCallback callback, object asyncState);

	UIControl[] EndGraphUndoChange(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/HighlightAccentGems", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightAccentGemsResponse")]
	void HighlightAccentGems(int gemId, bool highlightAll);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/HighlightAccentGems", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightAccentGemsResponse")]
	IAsyncResult BeginHighlightAccentGems(int gemId, bool highlightAll, AsyncCallback callback, object asyncState);

	void EndHighlightAccentGems(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByGemNumber", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByGemNumberResponse")]
	void HighlightGemFromGemReportByGemNumber(string[] gemNumbers);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByGemNumber", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByGemNumberResponse")]
	IAsyncResult BeginHighlightGemFromGemReportByGemNumber(string[] gemNumbers, AsyncCallback callback, object asyncState);

	void EndHighlightGemFromGemReportByGemNumber(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByIndex", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByIndexResponse")]
	void HighlightGemFromGemReportByIndex(int[] gemIndexes);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByIndex", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightGemFromGemReportByIndexResponse")]
	IAsyncResult BeginHighlightGemFromGemReportByIndex(int[] gemIndexes, AsyncCallback callback, object asyncState);

	void EndHighlightGemFromGemReportByIndex(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/HighlightGraphItem", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightGraphItemResponse")]
	void HighlightGraphItem(Guid blockInstanceGuid, bool showHighlight);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/HighlightGraphItem", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightGraphItemResponse")]
	IAsyncResult BeginHighlightGraphItem(Guid blockInstanceGuid, bool showHighlight, AsyncCallback callback, object asyncState);

	void EndHighlightGraphItem(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/HighlightItemsByBlockInstanceGuid", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightItemsByBlockInstanceGuidResponse")]
	void HighlightItemsByBlockInstanceGuid(Guid[] instanceGuids);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/HighlightItemsByBlockInstanceGuid", ReplyAction = "http://tempuri.org/IRhinoServices/HighlightItemsByBlockInstanceGuidResponse")]
	IAsyncResult BeginHighlightItemsByBlockInstanceGuid(Guid[] instanceGuids, AsyncCallback callback, object asyncState);

	void EndHighlightItemsByBlockInstanceGuid(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ImportGroupAddRingRailGuide", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupAddRingRailGuideResponse")]
	void ImportGroupAddRingRailGuide();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ImportGroupAddRingRailGuide", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupAddRingRailGuideResponse")]
	IAsyncResult BeginImportGroupAddRingRailGuide(AsyncCallback callback, object asyncState);

	void EndImportGroupAddRingRailGuide(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ImportGroupAddToPartGroup", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupAddToPartGroupResponse")]
	void ImportGroupAddToPartGroup(Guid[] importGroupIdentifiers);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ImportGroupAddToPartGroup", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupAddToPartGroupResponse")]
	IAsyncResult BeginImportGroupAddToPartGroup(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState);

	void EndImportGroupAddToPartGroup(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ImportGroupBreakPartGroup", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupBreakPartGroupResponse")]
	void ImportGroupBreakPartGroup(Guid importGroupIdentifier);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ImportGroupBreakPartGroup", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupBreakPartGroupResponse")]
	IAsyncResult BeginImportGroupBreakPartGroup(Guid importGroupIdentifier, AsyncCallback callback, object asyncState);

	void EndImportGroupBreakPartGroup(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ImportGroupRename", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupRenameResponse")]
	void ImportGroupRename(Guid importGroupIdentifier, string updatedName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ImportGroupRename", ReplyAction = "http://tempuri.org/IRhinoServices/ImportGroupRenameResponse")]
	IAsyncResult BeginImportGroupRename(Guid importGroupIdentifier, string updatedName, AsyncCallback callback, object asyncState);

	void EndImportGroupRename(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/InitializeRhinoProperties", ReplyAction = "http://tempuri.org/IRhinoServices/InitializeRhinoPropertiesResponse")]
	void InitializeRhinoProperties(string csLibraryPath);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/InitializeRhinoProperties", ReplyAction = "http://tempuri.org/IRhinoServices/InitializeRhinoPropertiesResponse")]
	IAsyncResult BeginInitializeRhinoProperties(string csLibraryPath, AsyncCallback callback, object asyncState);

	void EndInitializeRhinoProperties(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/InvokeAddAPartPatchwire", ReplyAction = "http://tempuri.org/IRhinoServices/InvokeAddAPartPatchwireResponse")]
	UIControl[] InvokeAddAPartPatchwire(string path, string name);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/InvokeAddAPartPatchwire", ReplyAction = "http://tempuri.org/IRhinoServices/InvokeAddAPartPatchwireResponse")]
	IAsyncResult BeginInvokeAddAPartPatchwire(string path, string name, AsyncCallback callback, object asyncState);

	UIControl[] EndInvokeAddAPartPatchwire(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/InvokeGemPatchWire", ReplyAction = "http://tempuri.org/IRhinoServices/InvokeGemPatchWireResponse")]
	UIControl[] InvokeGemPatchWire(GemActions action);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/InvokeGemPatchWire", ReplyAction = "http://tempuri.org/IRhinoServices/InvokeGemPatchWireResponse")]
	IAsyncResult BeginInvokeGemPatchWire(GemActions action, AsyncCallback callback, object asyncState);

	UIControl[] EndInvokeGemPatchWire(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/IsComponentsEnumVisible", ReplyAction = "http://tempuri.org/IRhinoServices/IsComponentsEnumVisibleResponse")]
	bool IsComponentsEnumVisible(ComponentsEnum componentsEnum);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/IsComponentsEnumVisible", ReplyAction = "http://tempuri.org/IRhinoServices/IsComponentsEnumVisibleResponse")]
	IAsyncResult BeginIsComponentsEnumVisible(ComponentsEnum componentsEnum, AsyncCallback callback, object asyncState);

	bool EndIsComponentsEnumVisible(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/IsDisplayConduitEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/IsDisplayConduitEnabledResponse")]
	bool IsDisplayConduitEnabled();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/IsDisplayConduitEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/IsDisplayConduitEnabledResponse")]
	IAsyncResult BeginIsDisplayConduitEnabled(AsyncCallback callback, object asyncState);

	bool EndIsDisplayConduitEnabled(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/IsDocumentCanvasEmpty", ReplyAction = "http://tempuri.org/IRhinoServices/IsDocumentCanvasEmptyResponse")]
	bool IsDocumentCanvasEmpty();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/IsDocumentCanvasEmpty", ReplyAction = "http://tempuri.org/IRhinoServices/IsDocumentCanvasEmptyResponse")]
	IAsyncResult BeginIsDocumentCanvasEmpty(AsyncCallback callback, object asyncState);

	bool EndIsDocumentCanvasEmpty(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/IsManualSizingModeOn", ReplyAction = "http://tempuri.org/IRhinoServices/IsManualSizingModeOnResponse")]
	bool IsManualSizingModeOn(bool excludeCenterStone);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/IsManualSizingModeOn", ReplyAction = "http://tempuri.org/IRhinoServices/IsManualSizingModeOnResponse")]
	IAsyncResult BeginIsManualSizingModeOn(bool excludeCenterStone, AsyncCallback callback, object asyncState);

	bool EndIsManualSizingModeOn(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/IsProjetDialogLoaded", ReplyAction = "http://tempuri.org/IRhinoServices/IsProjetDialogLoadedResponse")]
	bool IsProjetDialogLoaded();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/IsProjetDialogLoaded", ReplyAction = "http://tempuri.org/IRhinoServices/IsProjetDialogLoadedResponse")]
	IAsyncResult BeginIsProjetDialogLoaded(AsyncCallback callback, object asyncState);

	bool EndIsProjetDialogLoaded(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/IsRhino64Bit", ReplyAction = "http://tempuri.org/IRhinoServices/IsRhino64BitResponse")]
	bool IsRhino64Bit();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/IsRhino64Bit", ReplyAction = "http://tempuri.org/IRhinoServices/IsRhino64BitResponse")]
	IAsyncResult BeginIsRhino64Bit(AsyncCallback callback, object asyncState);

	bool EndIsRhino64Bit(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/IsUsbKeyBeingUsed", ReplyAction = "http://tempuri.org/IRhinoServices/IsUsbKeyBeingUsedResponse")]
	bool IsUsbKeyBeingUsed();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/IsUsbKeyBeingUsed", ReplyAction = "http://tempuri.org/IRhinoServices/IsUsbKeyBeingUsedResponse")]
	IAsyncResult BeginIsUsbKeyBeingUsed(AsyncCallback callback, object asyncState);

	bool EndIsUsbKeyBeingUsed(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/NurbsGetBoundingBox", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetBoundingBoxResponse")]
	BoundingBox NurbsGetBoundingBox(NurbsCurve nc);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/NurbsGetBoundingBox", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetBoundingBoxResponse")]
	IAsyncResult BeginNurbsGetBoundingBox(NurbsCurve nc, AsyncCallback callback, object asyncState);

	BoundingBox EndNurbsGetBoundingBox(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/NurbsGetClosestPoint", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetClosestPointResponse")]
	double NurbsGetClosestPoint(NurbsCurve nc, Point3D testHit);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/NurbsGetClosestPoint", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetClosestPointResponse")]
	IAsyncResult BeginNurbsGetClosestPoint(NurbsCurve nc, Point3D testHit, AsyncCallback callback, object asyncState);

	double EndNurbsGetClosestPoint(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/NurbsGetNormalizedPointAt", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetNormalizedPointAtResponse")]
	Point3D NurbsGetNormalizedPointAt(NurbsCurve nc, double editPointParam);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/NurbsGetNormalizedPointAt", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetNormalizedPointAtResponse")]
	IAsyncResult BeginNurbsGetNormalizedPointAt(NurbsCurve nc, double editPointParam, AsyncCallback callback, object asyncState);

	Point3D EndNurbsGetNormalizedPointAt(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/NurbsGetPolyline", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetPolylineResponse")]
	Point2D[] NurbsGetPolyline(NurbsCurve nc);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/NurbsGetPolyline", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsGetPolylineResponse")]
	IAsyncResult BeginNurbsGetPolyline(NurbsCurve nc, AsyncCallback callback, object asyncState);

	Point2D[] EndNurbsGetPolyline(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/NurbsRebuild", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsRebuildResponse")]
	NurbsCurve NurbsRebuild(NurbsCurve nc, int i, int i1);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/NurbsRebuild", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsRebuildResponse")]
	IAsyncResult BeginNurbsRebuild(NurbsCurve nc, int i, int i1, AsyncCallback callback, object asyncState);

	NurbsCurve EndNurbsRebuild(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/NurbsSetDomain", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsSetDomainResponse")]
	NurbsCurve NurbsSetDomain(NurbsCurve nc, int i, int i1);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/NurbsSetDomain", ReplyAction = "http://tempuri.org/IRhinoServices/NurbsSetDomainResponse")]
	IAsyncResult BeginNurbsSetDomain(NurbsCurve nc, int i, int i1, AsyncCallback callback, object asyncState);

	NurbsCurve EndNurbsSetDomain(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OpenDocument", ReplyAction = "http://tempuri.org/IRhinoServices/OpenDocumentResponse")]
	UIControl[] OpenDocument(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid, int? centerGemGraphIndex);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OpenDocument", ReplyAction = "http://tempuri.org/IRhinoServices/OpenDocumentResponse")]
	IAsyncResult BeginOpenDocument(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid, int? centerGemGraphIndex, AsyncCallback callback, object asyncState);

	UIControl[] EndOpenDocument(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OrderCheckNonCenterGemsExist", ReplyAction = "http://tempuri.org/IRhinoServices/OrderCheckNonCenterGemsExistResponse")]
	bool OrderCheckNonCenterGemsExist();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OrderCheckNonCenterGemsExist", ReplyAction = "http://tempuri.org/IRhinoServices/OrderCheckNonCenterGemsExistResponse")]
	IAsyncResult BeginOrderCheckNonCenterGemsExist(AsyncCallback callback, object asyncState);

	bool EndOrderCheckNonCenterGemsExist(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OrderGetAccentGemData", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetAccentGemDataResponse")]
	OrderGemData[] OrderGetAccentGemData();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OrderGetAccentGemData", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetAccentGemDataResponse")]
	IAsyncResult BeginOrderGetAccentGemData(AsyncCallback callback, object asyncState);

	OrderGemData[] EndOrderGetAccentGemData(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OrderGetAllGemData", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetAllGemDataResponse")]
	OrderGemData[] OrderGetAllGemData();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OrderGetAllGemData", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetAllGemDataResponse")]
	IAsyncResult BeginOrderGetAllGemData(AsyncCallback callback, object asyncState);

	OrderGemData[] EndOrderGetAllGemData(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizes", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesResponse")]
	string[] OrderGetCurrentRingSizes();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizes", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesResponse")]
	IAsyncResult BeginOrderGetCurrentRingSizes(AsyncCallback callback, object asyncState);

	string[] EndOrderGetCurrentRingSizes(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesDiameter", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesDiameterResponse")]
	double[] OrderGetCurrentRingSizesDiameter();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesDiameter", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesDiameterResponse")]
	IAsyncResult BeginOrderGetCurrentRingSizesDiameter(AsyncCallback callback, object asyncState);

	double[] EndOrderGetCurrentRingSizesDiameter(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesUom", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesUomResponse")]
	string[] OrderGetCurrentRingSizesUom();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesUom", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetCurrentRingSizesUomResponse")]
	IAsyncResult BeginOrderGetCurrentRingSizesUom(AsyncCallback callback, object asyncState);

	string[] EndOrderGetCurrentRingSizesUom(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/OrderGetExpandedSliderList", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetExpandedSliderListResponse")]
	string[] OrderGetExpandedSliderList();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/OrderGetExpandedSliderList", ReplyAction = "http://tempuri.org/IRhinoServices/OrderGetExpandedSliderListResponse")]
	IAsyncResult BeginOrderGetExpandedSliderList(AsyncCallback callback, object asyncState);

	string[] EndOrderGetExpandedSliderList(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/PerformDistanceMeasurement", ReplyAction = "http://tempuri.org/IRhinoServices/PerformDistanceMeasurementResponse")]
	void PerformDistanceMeasurement();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/PerformDistanceMeasurement", ReplyAction = "http://tempuri.org/IRhinoServices/PerformDistanceMeasurementResponse")]
	IAsyncResult BeginPerformDistanceMeasurement(AsyncCallback callback, object asyncState);

	void EndPerformDistanceMeasurement(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/PrintingSupportDisplayConduitEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/PrintingSupportDisplayConduitEnabledResponse")]
	void PrintingSupportDisplayConduitEnabled(bool isEnabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/PrintingSupportDisplayConduitEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/PrintingSupportDisplayConduitEnabledResponse")]
	IAsyncResult BeginPrintingSupportDisplayConduitEnabled(bool isEnabled, AsyncCallback callback, object asyncState);

	void EndPrintingSupportDisplayConduitEnabled(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/PrintMultipleJobs", ReplyAction = "http://tempuri.org/IRhinoServices/PrintMultipleJobsResponse")]
	bool PrintMultipleJobs(out string sStatus, int[] lstJobsToPrint);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/PrintMultipleJobs", ReplyAction = "http://tempuri.org/IRhinoServices/PrintMultipleJobsResponse")]
	IAsyncResult BeginPrintMultipleJobs(int[] lstJobsToPrint, AsyncCallback callback, object asyncState);

	bool EndPrintMultipleJobs(out string sStatus, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/PrintUnifiedMeshToProJet1200", ReplyAction = "http://tempuri.org/IRhinoServices/PrintUnifiedMeshToProJet1200Response")]
	bool PrintUnifiedMeshToProJet1200(out string ErrorMessage);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/PrintUnifiedMeshToProJet1200", ReplyAction = "http://tempuri.org/IRhinoServices/PrintUnifiedMeshToProJet1200Response")]
	IAsyncResult BeginPrintUnifiedMeshToProJet1200(AsyncCallback callback, object asyncState);

	bool EndPrintUnifiedMeshToProJet1200(out string ErrorMessage, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RedrawRhino", ReplyAction = "http://tempuri.org/IRhinoServices/RedrawRhinoResponse")]
	void RedrawRhino();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RedrawRhino", ReplyAction = "http://tempuri.org/IRhinoServices/RedrawRhinoResponse")]
	IAsyncResult BeginRedrawRhino(AsyncCallback callback, object asyncState);

	void EndRedrawRhino(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RemovePropFromDisplay", ReplyAction = "http://tempuri.org/IRhinoServices/RemovePropFromDisplayResponse")]
	void RemovePropFromDisplay();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RemovePropFromDisplay", ReplyAction = "http://tempuri.org/IRhinoServices/RemovePropFromDisplayResponse")]
	IAsyncResult BeginRemovePropFromDisplay(AsyncCallback callback, object asyncState);

	void EndRemovePropFromDisplay(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RenderGenerateRender", ReplyAction = "http://tempuri.org/IRhinoServices/RenderGenerateRenderResponse")]
	void RenderGenerateRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RenderGenerateRender", ReplyAction = "http://tempuri.org/IRhinoServices/RenderGenerateRenderResponse")]
	IAsyncResult BeginRenderGenerateRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane, AsyncCallback callback, object asyncState);

	void EndRenderGenerateRender(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RepaintRhino", ReplyAction = "http://tempuri.org/IRhinoServices/RepaintRhinoResponse")]
	void RepaintRhino();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RepaintRhino", ReplyAction = "http://tempuri.org/IRhinoServices/RepaintRhinoResponse")]
	IAsyncResult BeginRepaintRhino(AsyncCallback callback, object asyncState);

	void EndRepaintRhino(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RequestScreenCapture", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureResponse")]
	Bitmap RequestScreenCapture(System.Drawing.Size captureSize);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RequestScreenCapture", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureResponse")]
	IAsyncResult BeginRequestScreenCapture(System.Drawing.Size captureSize, AsyncCallback callback, object asyncState);

	Bitmap EndRequestScreenCapture(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAndFreeze", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAndFreezeResponse")]
	Bitmap RequestScreenCaptureAndFreeze(System.Drawing.Size captureSize);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAndFreeze", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAndFreezeResponse")]
	IAsyncResult BeginRequestScreenCaptureAndFreeze(System.Drawing.Size captureSize, AsyncCallback callback, object asyncState);

	Bitmap EndRequestScreenCaptureAndFreeze(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAtPerspectivePositionForImportGroup", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAtPerspectivePositionForImportGroupResponse")]
	void RequestScreenCaptureAtPerspectivePositionForImportGroup(System.Drawing.Size captureSize, bool zoom, Guid[] importGroupIdentifiers);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAtPerspectivePositionForImportGroup", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureAtPerspectivePositionForImportGroupResponse")]
	IAsyncResult BeginRequestScreenCaptureAtPerspectivePositionForImportGroup(System.Drawing.Size captureSize, bool zoom, Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState);

	void EndRequestScreenCaptureAtPerspectivePositionForImportGroup(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureDefault", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureDefaultResponse")]
	Bitmap RequestScreenCaptureDefault(System.Drawing.Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureDefault", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureDefaultResponse")]
	IAsyncResult BeginRequestScreenCaptureDefault(System.Drawing.Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly, AsyncCallback callback, object asyncState);

	Bitmap EndRequestScreenCaptureDefault(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureFront", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureFrontResponse")]
	Bitmap RequestScreenCaptureFront(System.Drawing.Size captureSize);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureFront", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureFrontResponse")]
	IAsyncResult BeginRequestScreenCaptureFront(System.Drawing.Size captureSize, AsyncCallback callback, object asyncState);

	Bitmap EndRequestScreenCaptureFront(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureSide", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureSideResponse")]
	Bitmap RequestScreenCaptureSide(System.Drawing.Size captureSize);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureSide", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureSideResponse")]
	IAsyncResult BeginRequestScreenCaptureSide(System.Drawing.Size captureSize, AsyncCallback callback, object asyncState);

	Bitmap EndRequestScreenCaptureSide(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureTop", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureTopResponse")]
	Bitmap RequestScreenCaptureTop(System.Drawing.Size captureSize);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RequestScreenCaptureTop", ReplyAction = "http://tempuri.org/IRhinoServices/RequestScreenCaptureTopResponse")]
	IAsyncResult BeginRequestScreenCaptureTop(System.Drawing.Size captureSize, AsyncCallback callback, object asyncState);

	Bitmap EndRequestScreenCaptureTop(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ReselectViewportControls", ReplyAction = "http://tempuri.org/IRhinoServices/ReselectViewportControlsResponse")]
	void ReselectViewportControls();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ReselectViewportControls", ReplyAction = "http://tempuri.org/IRhinoServices/ReselectViewportControlsResponse")]
	IAsyncResult BeginReselectViewportControls(AsyncCallback callback, object asyncState);

	void EndReselectViewportControls(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ResetHandleForImportGroup", ReplyAction = "http://tempuri.org/IRhinoServices/ResetHandleForImportGroupResponse")]
	void ResetHandleForImportGroup(Guid importGroupIdentifier);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ResetHandleForImportGroup", ReplyAction = "http://tempuri.org/IRhinoServices/ResetHandleForImportGroupResponse")]
	IAsyncResult BeginResetHandleForImportGroup(Guid importGroupIdentifier, AsyncCallback callback, object asyncState);

	void EndResetHandleForImportGroup(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoAbortScripts", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoAbortScriptsResponse")]
	void RhinoAbortScripts();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoAbortScripts", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoAbortScriptsResponse")]
	IAsyncResult BeginRhinoAbortScripts(AsyncCallback callback, object asyncState);

	void EndRhinoAbortScripts(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoCloseViewPort", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoCloseViewPortResponse")]
	void RhinoCloseViewPort(string viewportName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoCloseViewPort", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoCloseViewPortResponse")]
	IAsyncResult BeginRhinoCloseViewPort(string viewportName, AsyncCallback callback, object asyncState);

	void EndRhinoCloseViewPort(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoCreateViewPort", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoCreateViewPortResponse")]
	IntPtr RhinoCreateViewPort(string viewportName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoCreateViewPort", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoCreateViewPortResponse")]
	IAsyncResult BeginRhinoCreateViewPort(string viewportName, AsyncCallback callback, object asyncState);

	IntPtr EndRhinoCreateViewPort(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoGetActiveViewWindowHandle", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoGetActiveViewWindowHandleResponse")]
	IntPtr RhinoGetActiveViewWindowHandle();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoGetActiveViewWindowHandle", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoGetActiveViewWindowHandleResponse")]
	IAsyncResult BeginRhinoGetActiveViewWindowHandle(AsyncCallback callback, object asyncState);

	IntPtr EndRhinoGetActiveViewWindowHandle(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoGetVersion", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoGetVersionResponse")]
	string RhinoGetVersion();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoGetVersion", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoGetVersionResponse")]
	IAsyncResult BeginRhinoGetVersion(AsyncCallback callback, object asyncState);

	string EndRhinoGetVersion(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoGetWindowHandle", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoGetWindowHandleResponse")]
	IntPtr RhinoGetWindowHandle();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoGetWindowHandle", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoGetWindowHandleResponse")]
	IAsyncResult BeginRhinoGetWindowHandle(AsyncCallback callback, object asyncState);

	IntPtr EndRhinoGetWindowHandle(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoLoadBlankDocument", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoLoadBlankDocumentResponse")]
	void RhinoLoadBlankDocument();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoLoadBlankDocument", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoLoadBlankDocumentResponse")]
	IAsyncResult BeginRhinoLoadBlankDocument(AsyncCallback callback, object asyncState);

	void EndRhinoLoadBlankDocument(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoRunScript", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoRunScriptResponse")]
	void RhinoRunScript(string scriptText);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoRunScript", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoRunScriptResponse")]
	IAsyncResult BeginRhinoRunScript(string scriptText, AsyncCallback callback, object asyncState);

	void EndRhinoRunScript(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoSetFocusTo", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoSetFocusToResponse")]
	void RhinoSetFocusTo();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoSetFocusTo", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoSetFocusToResponse")]
	IAsyncResult BeginRhinoSetFocusTo(AsyncCallback callback, object asyncState);

	void EndRhinoSetFocusTo(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoWindowReparent", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoWindowReparentResponse")]
	bool RhinoWindowReparent(IntPtr intPtrParent);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoWindowReparent", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoWindowReparentResponse")]
	IAsyncResult BeginRhinoWindowReparent(IntPtr intPtrParent, AsyncCallback callback, object asyncState);

	bool EndRhinoWindowReparent(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/RhinoWindowReposition", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoWindowRepositionResponse")]
	void RhinoWindowReposition(int startPointX, int startPointY, int width, int height);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/RhinoWindowReposition", ReplyAction = "http://tempuri.org/IRhinoServices/RhinoWindowRepositionResponse")]
	IAsyncResult BeginRhinoWindowReposition(int startPointX, int startPointY, int width, int height, AsyncCallback callback, object asyncState);

	void EndRhinoWindowReposition(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SavePrintJob", ReplyAction = "http://tempuri.org/IRhinoServices/SavePrintJobResponse")]
	bool SavePrintJob(out string sStatus, int whatslot);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SavePrintJob", ReplyAction = "http://tempuri.org/IRhinoServices/SavePrintJobResponse")]
	IAsyncResult BeginSavePrintJob(int whatslot, AsyncCallback callback, object asyncState);

	bool EndSavePrintJob(out string sStatus, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SaveScreen", ReplyAction = "http://tempuri.org/IRhinoServices/SaveScreenResponse")]
	void SaveScreen(string saveImagePath);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SaveScreen", ReplyAction = "http://tempuri.org/IRhinoServices/SaveScreenResponse")]
	IAsyncResult BeginSaveScreen(string saveImagePath, AsyncCallback callback, object asyncState);

	void EndSaveScreen(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SaveViewportToImage", ReplyAction = "http://tempuri.org/IRhinoServices/SaveViewportToImageResponse")]
	bool SaveViewportToImage(string viewportName, string pathToSaveImage, double maxHeightWidth);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SaveViewportToImage", ReplyAction = "http://tempuri.org/IRhinoServices/SaveViewportToImageResponse")]
	IAsyncResult BeginSaveViewportToImage(string viewportName, string pathToSaveImage, double maxHeightWidth, AsyncCallback callback, object asyncState);

	bool EndSaveViewportToImage(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifier", ReplyAction = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifierResponse")]
	void SelectImportedGroupByIdentifier(Guid importGroupIdentifier);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifier", ReplyAction = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifierResponse")]
	IAsyncResult BeginSelectImportedGroupByIdentifier(Guid importGroupIdentifier, AsyncCallback callback, object asyncState);

	void EndSelectImportedGroupByIdentifier(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifierSecondary", ReplyAction = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifierSecondaryResponse")]
	void SelectImportedGroupByIdentifierSecondary(Guid[] importGroupIdentifiersd);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifierSecondary", ReplyAction = "http://tempuri.org/IRhinoServices/SelectImportedGroupByIdentifierSecondaryResponse")]
	IAsyncResult BeginSelectImportedGroupByIdentifierSecondary(Guid[] importGroupIdentifiersd, AsyncCallback callback, object asyncState);

	void EndSelectImportedGroupByIdentifierSecondary(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetIsBlockParallelProcessingEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/SetIsBlockParallelProcessingEnabledResponse")]
	void SetIsBlockParallelProcessingEnabled(bool enabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetIsBlockParallelProcessingEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/SetIsBlockParallelProcessingEnabledResponse")]
	IAsyncResult BeginSetIsBlockParallelProcessingEnabled(bool enabled, AsyncCallback callback, object asyncState);

	void EndSetIsBlockParallelProcessingEnabled(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetCAMModeState", ReplyAction = "http://tempuri.org/IRhinoServices/SetCAMModeStateResponse")]
	void SetCAMModeState(bool CAMModeOn);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetCAMModeState", ReplyAction = "http://tempuri.org/IRhinoServices/SetCAMModeStateResponse")]
	IAsyncResult BeginSetCAMModeState(bool CAMModeOn, AsyncCallback callback, object asyncState);

	void EndSetCAMModeState(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetComponentVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/SetComponentVisibilityResponse")]
	void SetComponentVisibility(ComponentsEnum componentsEnum, bool visible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetComponentVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/SetComponentVisibilityResponse")]
	IAsyncResult BeginSetComponentVisibility(ComponentsEnum componentsEnum, bool visible, AsyncCallback callback, object asyncState);

	void EndSetComponentVisibility(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetDesignIteration", ReplyAction = "http://tempuri.org/IRhinoServices/SetDesignIterationResponse")]
	UIControl[] SetDesignIteration(int iterationId);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetDesignIteration", ReplyAction = "http://tempuri.org/IRhinoServices/SetDesignIterationResponse")]
	IAsyncResult BeginSetDesignIteration(int iterationId, AsyncCallback callback, object asyncState);

	UIControl[] EndSetDesignIteration(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetGemMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/SetGemMaterialsResponse")]
	void SetGemMaterials(GemMaterial[] gemMaterials);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetGemMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/SetGemMaterialsResponse")]
	IAsyncResult BeginSetGemMaterials(GemMaterial[] gemMaterials, AsyncCallback callback, object asyncState);

	void EndSetGemMaterials(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetImportGroupToPrimaryMetal", ReplyAction = "http://tempuri.org/IRhinoServices/SetImportGroupToPrimaryMetalResponse")]
	void SetImportGroupToPrimaryMetal(Guid[] importGroupIdentifiers);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetImportGroupToPrimaryMetal", ReplyAction = "http://tempuri.org/IRhinoServices/SetImportGroupToPrimaryMetalResponse")]
	IAsyncResult BeginSetImportGroupToPrimaryMetal(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState);

	void EndSetImportGroupToPrimaryMetal(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetImportGroupToSecondaryMetal", ReplyAction = "http://tempuri.org/IRhinoServices/SetImportGroupToSecondaryMetalResponse")]
	void SetImportGroupToSecondaryMetal(Guid[] importGroupIdentifiers);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetImportGroupToSecondaryMetal", ReplyAction = "http://tempuri.org/IRhinoServices/SetImportGroupToSecondaryMetalResponse")]
	IAsyncResult BeginSetImportGroupToSecondaryMetal(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState);

	void EndSetImportGroupToSecondaryMetal(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetImportGroupVisibilityByIdentifier", ReplyAction = "http://tempuri.org/IRhinoServices/SetImportGroupVisibilityByIdentifierResponse")]
	void SetImportGroupVisibilityByIdentifier(Guid[] importGroupIdentifiers, bool isViewportVisible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetImportGroupVisibilityByIdentifier", ReplyAction = "http://tempuri.org/IRhinoServices/SetImportGroupVisibilityByIdentifierResponse")]
	IAsyncResult BeginSetImportGroupVisibilityByIdentifier(Guid[] importGroupIdentifiers, bool isViewportVisible, AsyncCallback callback, object asyncState);

	void EndSetImportGroupVisibilityByIdentifier(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetKeepAliveProcessIds", ReplyAction = "http://tempuri.org/IRhinoServices/SetKeepAliveProcessIdsResponse")]
	int SetKeepAliveProcessIds(int id);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetKeepAliveProcessIds", ReplyAction = "http://tempuri.org/IRhinoServices/SetKeepAliveProcessIdsResponse")]
	IAsyncResult BeginSetKeepAliveProcessIds(int id, AsyncCallback callback, object asyncState);

	int EndSetKeepAliveProcessIds(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetLoadedDialogState", ReplyAction = "http://tempuri.org/IRhinoServices/SetLoadedDialogStateResponse")]
	void SetLoadedDialogState(bool bState);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetLoadedDialogState", ReplyAction = "http://tempuri.org/IRhinoServices/SetLoadedDialogStateResponse")]
	IAsyncResult BeginSetLoadedDialogState(bool bState, AsyncCallback callback, object asyncState);

	void EndSetLoadedDialogState(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetMeshFromComponent", ReplyAction = "http://tempuri.org/IRhinoServices/SetMeshFromComponentResponse")]
	bool SetMeshFromComponent(out string sError, ComponentsEnum[] whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetMeshFromComponent", ReplyAction = "http://tempuri.org/IRhinoServices/SetMeshFromComponentResponse")]
	IAsyncResult BeginSetMeshFromComponent(ComponentsEnum[] whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed, AsyncCallback callback, object asyncState);

	bool EndSetMeshFromComponent(out string sError, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetMetalMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/SetMetalMaterialsResponse")]
	void SetMetalMaterials(MetalMaterial[] metalMaterials);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetMetalMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/SetMetalMaterialsResponse")]
	IAsyncResult BeginSetMetalMaterials(MetalMaterial[] metalMaterials, AsyncCallback callback, object asyncState);

	void EndSetMetalMaterials(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetPerspectiveViewActiveAndMaximize", ReplyAction = "http://tempuri.org/IRhinoServices/SetPerspectiveViewActiveAndMaximizeResponse")]
	void SetPerspectiveViewActiveAndMaximize();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetPerspectiveViewActiveAndMaximize", ReplyAction = "http://tempuri.org/IRhinoServices/SetPerspectiveViewActiveAndMaximizeResponse")]
	IAsyncResult BeginSetPerspectiveViewActiveAndMaximize(AsyncCallback callback, object asyncState);

	void EndSetPerspectiveViewActiveAndMaximize(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetSupportDensity", ReplyAction = "http://tempuri.org/IRhinoServices/SetSupportDensityResponse")]
	void SetSupportDensity(int iSupportDensity);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetSupportDensity", ReplyAction = "http://tempuri.org/IRhinoServices/SetSupportDensityResponse")]
	IAsyncResult BeginSetSupportDensity(int iSupportDensity, AsyncCallback callback, object asyncState);

	void EndSetSupportDensity(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetSupportScales", ReplyAction = "http://tempuri.org/IRhinoServices/SetSupportScalesResponse")]
	bool SetSupportScales(double SupScale, int SupType);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetSupportScales", ReplyAction = "http://tempuri.org/IRhinoServices/SetSupportScalesResponse")]
	IAsyncResult BeginSetSupportScales(double SupScale, int SupType, AsyncCallback callback, object asyncState);

	bool EndSetSupportScales(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetUnifiedMeshLocation", ReplyAction = "http://tempuri.org/IRhinoServices/SetUnifiedMeshLocationResponse")]
	void SetUnifiedMeshLocation(double dHeight);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetUnifiedMeshLocation", ReplyAction = "http://tempuri.org/IRhinoServices/SetUnifiedMeshLocationResponse")]
	IAsyncResult BeginSetUnifiedMeshLocation(double dHeight, AsyncCallback callback, object asyncState);

	void EndSetUnifiedMeshLocation(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/SetUnifiedMeshRotation", ReplyAction = "http://tempuri.org/IRhinoServices/SetUnifiedMeshRotationResponse")]
	void SetUnifiedMeshRotation(double angleX, double angleY, double angleZ);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/SetUnifiedMeshRotation", ReplyAction = "http://tempuri.org/IRhinoServices/SetUnifiedMeshRotationResponse")]
	IAsyncResult BeginSetUnifiedMeshRotation(double angleX, double angleY, double angleZ, AsyncCallback callback, object asyncState);

	void EndSetUnifiedMeshRotation(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/Shutdown", ReplyAction = "http://tempuri.org/IRhinoServices/ShutdownResponse")]
	void Shutdown();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/Shutdown", ReplyAction = "http://tempuri.org/IRhinoServices/ShutdownResponse")]
	IAsyncResult BeginShutdown(AsyncCallback callback, object asyncState);

	void EndShutdown(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/AbortPrintJob", ReplyAction = "http://tempuri.org/IRhinoServices/AbortPrintJobResponse")]
	string AbortPrintJob();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/AbortPrintJob", ReplyAction = "http://tempuri.org/IRhinoServices/AbortPrintJobResponse")]
	IAsyncResult BeginAbortPrintJob(AsyncCallback callback, object asyncState);

	string EndAbortPrintJob(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/AddPropToDisplay", ReplyAction = "http://tempuri.org/IRhinoServices/AddPropToDisplayResponse")]
	void AddPropToDisplay(string renderPropSwatchCollectionItem);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/AddPropToDisplay", ReplyAction = "http://tempuri.org/IRhinoServices/AddPropToDisplayResponse")]
	IAsyncResult BeginAddPropToDisplay(string renderPropSwatchCollectionItem, AsyncCallback callback, object asyncState);

	void EndAddPropToDisplay(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisible", ReplyAction = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisibleResponse")]
	bool AreSecondaryMetalObjectsVisible();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisible", ReplyAction = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisibleResponse")]
	IAsyncResult BeginAreSecondaryMetalObjectsVisible(AsyncCallback callback, object asyncState);

	bool EndAreSecondaryMetalObjectsVisible(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisibleForComponent", ReplyAction = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisibleForComponentResponse")]
	bool AreSecondaryMetalObjectsVisibleForComponent(ComponentsEnum whichComponent);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisibleForComponent", ReplyAction = "http://tempuri.org/IRhinoServices/AreSecondaryMetalObjectsVisibleForComponentResponse")]
	IAsyncResult BeginAreSecondaryMetalObjectsVisibleForComponent(ComponentsEnum whichComponent, AsyncCallback callback, object asyncState);

	bool EndAreSecondaryMetalObjectsVisibleForComponent(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/CheckConnectionToProJet1200", ReplyAction = "http://tempuri.org/IRhinoServices/CheckConnectionToProJet1200Response")]
	bool CheckConnectionToProJet1200(out string sError, out string sStatus, string connectionAddress);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/CheckConnectionToProJet1200", ReplyAction = "http://tempuri.org/IRhinoServices/CheckConnectionToProJet1200Response")]
	IAsyncResult BeginCheckConnectionToProJet1200(string connectionAddress, AsyncCallback callback, object asyncState);

	bool EndCheckConnectionToProJet1200(out string sError, out string sStatus, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/CheckFirmwareVersion", ReplyAction = "http://tempuri.org/IRhinoServices/CheckFirmwareVersionResponse")]
	bool CheckFirmwareVersion();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/CheckFirmwareVersion", ReplyAction = "http://tempuri.org/IRhinoServices/CheckFirmwareVersionResponse")]
	IAsyncResult BeginCheckFirmwareVersion(AsyncCallback callback, object asyncState);

	bool EndCheckFirmwareVersion(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ClearExpandedSliders", ReplyAction = "http://tempuri.org/IRhinoServices/ClearExpandedSlidersResponse")]
	void ClearExpandedSliders();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ClearExpandedSliders", ReplyAction = "http://tempuri.org/IRhinoServices/ClearExpandedSlidersResponse")]
	IAsyncResult BeginClearExpandedSliders(AsyncCallback callback, object asyncState);

	void EndClearExpandedSliders(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ClearHighlightedGems", ReplyAction = "http://tempuri.org/IRhinoServices/ClearHighlightedGemsResponse")]
	void ClearHighlightedGems();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ClearHighlightedGems", ReplyAction = "http://tempuri.org/IRhinoServices/ClearHighlightedGemsResponse")]
	IAsyncResult BeginClearHighlightedGems(AsyncCallback callback, object asyncState);

	void EndClearHighlightedGems(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ClearUnifiedMesh", ReplyAction = "http://tempuri.org/IRhinoServices/ClearUnifiedMeshResponse")]
	bool ClearUnifiedMesh();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ClearUnifiedMesh", ReplyAction = "http://tempuri.org/IRhinoServices/ClearUnifiedMeshResponse")]
	IAsyncResult BeginClearUnifiedMesh(AsyncCallback callback, object asyncState);

	bool EndClearUnifiedMesh(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConduitEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitEnabledResponse")]
	void ConduitEnabled(bool isEnabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConduitEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitEnabledResponse")]
	IAsyncResult BeginConduitEnabled(bool isEnabled, AsyncCallback callback, object asyncState);

	void EndConduitEnabled(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConduitResetTryOn", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitResetTryOnResponse")]
	void ConduitResetTryOn();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConduitResetTryOn", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitResetTryOnResponse")]
	IAsyncResult BeginConduitResetTryOn(AsyncCallback callback, object asyncState);

	void EndConduitResetTryOn(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConduitSetFingernailObjectMaterial", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetFingernailObjectMaterialResponse")]
	void ConduitSetFingernailObjectMaterial(string fingernailPolishMaterialName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConduitSetFingernailObjectMaterial", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetFingernailObjectMaterialResponse")]
	IAsyncResult BeginConduitSetFingernailObjectMaterial(string fingernailPolishMaterialName, AsyncCallback callback, object asyncState);

	void EndConduitSetFingernailObjectMaterial(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConduitSetFlipRing", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetFlipRingResponse")]
	void ConduitSetFlipRing(bool flipRing);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConduitSetFlipRing", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetFlipRingResponse")]
	IAsyncResult BeginConduitSetFlipRing(bool flipRing, AsyncCallback callback, object asyncState);

	void EndConduitSetFlipRing(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConduitSetHandObjectMaterial", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetHandObjectMaterialResponse")]
	void ConduitSetHandObjectMaterial(string handMaterialName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConduitSetHandObjectMaterial", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetHandObjectMaterialResponse")]
	IAsyncResult BeginConduitSetHandObjectMaterial(string handMaterialName, AsyncCallback callback, object asyncState);

	void EndConduitSetHandObjectMaterial(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailObjectFlip", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailObjectFlipResponse")]
	void ConduitSetScaleToRingRailObjectFlip(bool flipObject);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailObjectFlip", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailObjectFlipResponse")]
	IAsyncResult BeginConduitSetScaleToRingRailObjectFlip(bool flipObject, AsyncCallback callback, object asyncState);

	void EndConduitSetScaleToRingRailObjectFlip(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailYOffset", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailYOffsetResponse")]
	void ConduitSetScaleToRingRailYOffset(double ringOffset);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailYOffset", ReplyAction = "http://tempuri.org/IRhinoServices/ConduitSetScaleToRingRailYOffsetResponse")]
	IAsyncResult BeginConduitSetScaleToRingRailYOffset(double ringOffset, AsyncCallback callback, object asyncState);

	void EndConduitSetScaleToRingRailYOffset(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ConnectionValid", ReplyAction = "http://tempuri.org/IRhinoServices/ConnectionValidResponse")]
	bool ConnectionValid();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ConnectionValid", ReplyAction = "http://tempuri.org/IRhinoServices/ConnectionValidResponse")]
	IAsyncResult BeginConnectionValid(AsyncCallback callback, object asyncState);

	bool EndConnectionValid(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/CSCreateSupportsForCSItem", ReplyAction = "http://tempuri.org/IRhinoServices/CSCreateSupportsForCSItemResponse")]
	bool CSCreateSupportsForCSItem(out string ErrorMessage);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/CSCreateSupportsForCSItem", ReplyAction = "http://tempuri.org/IRhinoServices/CSCreateSupportsForCSItemResponse")]
	IAsyncResult BeginCSCreateSupportsForCSItem(AsyncCallback callback, object asyncState);

	bool EndCSCreateSupportsForCSItem(out string ErrorMessage, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DeleteImportedGroupFromGraph", ReplyAction = "http://tempuri.org/IRhinoServices/DeleteImportedGroupFromGraphResponse")]
	UIControl[] DeleteImportedGroupFromGraph(Guid[] importGroupIdentifiers);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DeleteImportedGroupFromGraph", ReplyAction = "http://tempuri.org/IRhinoServices/DeleteImportedGroupFromGraphResponse")]
	IAsyncResult BeginDeleteImportedGroupFromGraph(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState);

	UIControl[] EndDeleteImportedGroupFromGraph(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DeleteSavedJob", ReplyAction = "http://tempuri.org/IRhinoServices/DeleteSavedJobResponse")]
	bool DeleteSavedJob(int whatjob);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DeleteSavedJob", ReplyAction = "http://tempuri.org/IRhinoServices/DeleteSavedJobResponse")]
	IAsyncResult BeginDeleteSavedJob(int whatjob, AsyncCallback callback, object asyncState);

	bool EndDeleteSavedJob(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DeselectViewportControls", ReplyAction = "http://tempuri.org/IRhinoServices/DeselectViewportControlsResponse")]
	void DeselectViewportControls();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DeselectViewportControls", ReplyAction = "http://tempuri.org/IRhinoServices/DeselectViewportControlsResponse")]
	IAsyncResult BeginDeselectViewportControls(AsyncCallback callback, object asyncState);

	void EndDeselectViewportControls(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DisplayConduitSetEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/DisplayConduitSetEnabledResponse")]
	void DisplayConduitSetEnabled(bool enabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DisplayConduitSetEnabled", ReplyAction = "http://tempuri.org/IRhinoServices/DisplayConduitSetEnabledResponse")]
	IAsyncResult BeginDisplayConduitSetEnabled(bool enabled, AsyncCallback callback, object asyncState);

	void EndDisplayConduitSetEnabled(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentCenterDesign", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentCenterDesignResponse")]
	void DocumentCenterDesign();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentCenterDesign", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentCenterDesignResponse")]
	IAsyncResult BeginDocumentCenterDesign(AsyncCallback callback, object asyncState);

	void EndDocumentCenterDesign(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentClearAllAnyGemAnyColor", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentClearAllAnyGemAnyColorResponse")]
	void DocumentClearAllAnyGemAnyColor();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentClearAllAnyGemAnyColor", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentClearAllAnyGemAnyColorResponse")]
	IAsyncResult BeginDocumentClearAllAnyGemAnyColor(AsyncCallback callback, object asyncState);

	void EndDocumentClearAllAnyGemAnyColor(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentClearUndo", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentClearUndoResponse")]
	void DocumentClearUndo();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentClearUndo", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentClearUndoResponse")]
	IAsyncResult BeginDocumentClearUndo(AsyncCallback callback, object asyncState);

	void EndDocumentClearUndo(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsResponse")]
	bool DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(GemMaterial gemMaterial, bool isCenterStoneAssigned);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGemsResponse")]
	IAsyncResult BeginDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(GemMaterial gemMaterial, bool isCenterStoneAssigned, AsyncCallback callback, object asyncState);

	bool EndDocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentDuplicateCSGPart", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentDuplicateCSGPartResponse")]
	void DocumentDuplicateCSGPart(Guid[] importGroupIdentifiers);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentDuplicateCSGPart", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentDuplicateCSGPartResponse")]
	IAsyncResult BeginDocumentDuplicateCSGPart(Guid[] importGroupIdentifiers, AsyncCallback callback, object asyncState);

	void EndDocumentDuplicateCSGPart(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentGetModified", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetModifiedResponse")]
	bool DocumentGetModified();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentGetModified", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetModifiedResponse")]
	IAsyncResult BeginDocumentGetModified(AsyncCallback callback, object asyncState);

	bool EndDocumentGetModified(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeIndexFromSizeText", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeIndexFromSizeTextResponse")]
	double DocumentGetRingSizeIndexFromSizeText(string ringSizeText);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeIndexFromSizeText", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeIndexFromSizeTextResponse")]
	IAsyncResult BeginDocumentGetRingSizeIndexFromSizeText(string ringSizeText, AsyncCallback callback, object asyncState);

	double EndDocumentGetRingSizeIndexFromSizeText(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTextFromSizeIndex", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTextFromSizeIndexResponse")]
	string DocumentGetRingSizeTextFromSizeIndex(double ringSizeIndex);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTextFromSizeIndex", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTextFromSizeIndexResponse")]
	IAsyncResult BeginDocumentGetRingSizeTextFromSizeIndex(double ringSizeIndex, AsyncCallback callback, object asyncState);

	string EndDocumentGetRingSizeTextFromSizeIndex(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTranslated", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTranslatedResponse")]
	double DocumentGetRingSizeTranslated(double ringSizeInMillimeters);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTranslated", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetRingSizeTranslatedResponse")]
	IAsyncResult BeginDocumentGetRingSizeTranslated(double ringSizeInMillimeters, AsyncCallback callback, object asyncState);

	double EndDocumentGetRingSizeTranslated(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentGetStudioGemList", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetStudioGemListResponse")]
	StudioGem[] DocumentGetStudioGemList();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentGetStudioGemList", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGetStudioGemListResponse")]
	IAsyncResult BeginDocumentGetStudioGemList(AsyncCallback callback, object asyncState);

	StudioGem[] EndDocumentGetStudioGemList(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentGraphGenerate", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGraphGenerateResponse")]
	void DocumentGraphGenerate();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentGraphGenerate", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentGraphGenerateResponse")]
	IAsyncResult BeginDocumentGraphGenerate(AsyncCallback callback, object asyncState);

	void EndDocumentGraphGenerate(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentImportCSGPart", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentImportCSGPartResponse")]
	void DocumentImportCSGPart(string pathToCSGPart, int? centerGemGraphIndex, bool doShowFontSourceErrors);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentImportCSGPart", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentImportCSGPartResponse")]
	IAsyncResult BeginDocumentImportCSGPart(string pathToCSGPart, int? centerGemGraphIndex, bool doShowFontSourceErrors, AsyncCallback callback, object asyncState);

	void EndDocumentImportCSGPart(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentReset", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentResetResponse")]
	void DocumentReset();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentReset", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentResetResponse")]
	IAsyncResult BeginDocumentReset(AsyncCallback callback, object asyncState);

	void EndDocumentReset(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentResetCenterGemAnyGemAnyColorMaterialAssignment", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentResponse")]
	void DocumentResetCenterGemAnyGemAnyColorMaterialAssignment();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentResetCenterGemAnyGemAnyColorMaterialAssignment", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentResetCenterGemAnyGemAnyColorMaterialAssignmentResponse")]
	IAsyncResult BeginDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(AsyncCallback callback, object asyncState);

	void EndDocumentResetCenterGemAnyGemAnyColorMaterialAssignment(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSetCameraView", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetCameraViewResponse")]
	void DocumentSetCameraView(string cameraViewName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSetCameraView", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetCameraViewResponse")]
	IAsyncResult BeginDocumentSetCameraView(string cameraViewName, AsyncCallback callback, object asyncState);

	void EndDocumentSetCameraView(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSetCurvesAndPointsVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetCurvesAndPointsVisibilityResponse")]
	void DocumentSetCurvesAndPointsVisibility(bool visible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSetCurvesAndPointsVisibility", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetCurvesAndPointsVisibilityResponse")]
	IAsyncResult BeginDocumentSetCurvesAndPointsVisibility(bool visible, AsyncCallback callback, object asyncState);

	void EndDocumentSetCurvesAndPointsVisibility(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSetFilename", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetFilenameResponse")]
	void DocumentSetFilename(string filename);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSetFilename", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetFilenameResponse")]
	IAsyncResult BeginDocumentSetFilename(string filename, AsyncCallback callback, object asyncState);

	void EndDocumentSetFilename(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSetModified", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetModifiedResponse")]
	void DocumentSetModified(bool modified);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSetModified", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetModifiedResponse")]
	IAsyncResult BeginDocumentSetModified(bool modified, AsyncCallback callback, object asyncState);

	void EndDocumentSetModified(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSetRingSizeSetting", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetRingSizeSettingResponse")]
	void DocumentSetRingSizeSetting(string ringSizeSetting);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSetRingSizeSetting", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetRingSizeSettingResponse")]
	IAsyncResult BeginDocumentSetRingSizeSetting(string ringSizeSetting, AsyncCallback callback, object asyncState);

	void EndDocumentSetRingSizeSetting(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSetShowCurves", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetShowCurvesResponse")]
	void DocumentSetShowCurves(bool visible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSetShowCurves", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetShowCurvesResponse")]
	IAsyncResult BeginDocumentSetShowCurves(bool visible, AsyncCallback callback, object asyncState);

	void EndDocumentSetShowCurves(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSetShowStarts", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetShowStartsResponse")]
	void DocumentSetShowStarts(bool visible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSetShowStarts", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSetShowStartsResponse")]
	IAsyncResult BeginDocumentSetShowStarts(bool visible, AsyncCallback callback, object asyncState);

	void EndDocumentSetShowStarts(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentShowAxis", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentShowAxisResponse")]
	void DocumentShowAxis(bool visible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentShowAxis", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentShowAxisResponse")]
	IAsyncResult BeginDocumentShowAxis(bool visible, AsyncCallback callback, object asyncState);

	void EndDocumentShowAxis(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentShowGrid", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentShowGridResponse")]
	void DocumentShowGrid(bool visible);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentShowGrid", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentShowGridResponse")]
	IAsyncResult BeginDocumentShowGrid(bool visible, AsyncCallback callback, object asyncState);

	void EndDocumentShowGrid(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentSpinDesign", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSpinDesignResponse")]
	void DocumentSpinDesign(bool isEnabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentSpinDesign", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentSpinDesignResponse")]
	IAsyncResult BeginDocumentSpinDesign(bool isEnabled, AsyncCallback callback, object asyncState);

	void EndDocumentSpinDesign(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentToggleGemSelectionMode", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentToggleGemSelectionModeResponse")]
	void DocumentToggleGemSelectionMode(bool isEnabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentToggleGemSelectionMode", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentToggleGemSelectionModeResponse")]
	IAsyncResult BeginDocumentToggleGemSelectionMode(bool isEnabled, AsyncCallback callback, object asyncState);

	void EndDocumentToggleGemSelectionMode(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentUndoBeginRecord", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUndoBeginRecordResponse")]
	void DocumentUndoBeginRecord();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentUndoBeginRecord", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUndoBeginRecordResponse")]
	IAsyncResult BeginDocumentUndoBeginRecord(AsyncCallback callback, object asyncState);

	void EndDocumentUndoBeginRecord(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentUserDataAddValue", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataAddValueResponse")]
	void DocumentUserDataAddValue(string typeName, string keyName, string value);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentUserDataAddValue", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataAddValueResponse")]
	IAsyncResult BeginDocumentUserDataAddValue(string typeName, string keyName, string value, AsyncCallback callback, object asyncState);

	void EndDocumentUserDataAddValue(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentUserDataFindValue", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataFindValueResponse")]
	string DocumentUserDataFindValue(string typeName, string keyName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentUserDataFindValue", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataFindValueResponse")]
	IAsyncResult BeginDocumentUserDataFindValue(string typeName, string keyName, AsyncCallback callback, object asyncState);

	string EndDocumentUserDataFindValue(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentUserDataGetOrderData", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataGetOrderDataResponse")]
	string DocumentUserDataGetOrderData();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentUserDataGetOrderData", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataGetOrderDataResponse")]
	IAsyncResult BeginDocumentUserDataGetOrderData(AsyncCallback callback, object asyncState);

	string EndDocumentUserDataGetOrderData(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentUserDataGetAllDataElements", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataGetAllDataElementsResponse")]
	Tuple<string, string, string>[] DocumentUserDataGetAllDataElements();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentUserDataGetAllDataElements", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentUserDataGetAllDataElementsResponse")]
	IAsyncResult BeginDocumentUserDataGetAllDataElements(AsyncCallback callback, object asyncState);

	Tuple<string, string, string>[] EndDocumentUserDataGetAllDataElements(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentWrite", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentWriteResponse")]
	void DocumentWrite(string path, string licenseInfo);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentWrite", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentWriteResponse")]
	IAsyncResult BeginDocumentWrite(string path, string licenseInfo, AsyncCallback callback, object asyncState);

	void EndDocumentWrite(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/DocumentZoomCameraToBoundingBox", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentZoomCameraToBoundingBoxResponse")]
	void DocumentZoomCameraToBoundingBox();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/DocumentZoomCameraToBoundingBox", ReplyAction = "http://tempuri.org/IRhinoServices/DocumentZoomCameraToBoundingBoxResponse")]
	IAsyncResult BeginDocumentZoomCameraToBoundingBox(AsyncCallback callback, object asyncState);

	void EndDocumentZoomCameraToBoundingBox(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/EditorGetPartsForCurveEditrFromGraphObject", ReplyAction = "http://tempuri.org/IRhinoServices/EditorGetPartsForCurveEditrFromGraphObjectResponse")]
	[return: MessageParameter(Name = "sweepPoints")]
	double[] EditorGetPartsForCurveEditrFromGraphObject(out double chooserValue, out NurbsCurve curve, Guid blockInstanceGuid, bool createAsCopy);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/EditorGetPartsForCurveEditrFromGraphObject", ReplyAction = "http://tempuri.org/IRhinoServices/EditorGetPartsForCurveEditrFromGraphObjectResponse")]
	IAsyncResult BeginEditorGetPartsForCurveEditrFromGraphObject(Guid blockInstanceGuid, bool createAsCopy, AsyncCallback callback, object asyncState);

	[return: MessageParameter(Name = "sweepPoints")]
	double[] EndEditorGetPartsForCurveEditrFromGraphObject(out double chooserValue, out NurbsCurve curve, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/EnableDrawing", ReplyAction = "http://tempuri.org/IRhinoServices/EnableDrawingResponse")]
	void EnableDrawing(bool isEnabled);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/EnableDrawing", ReplyAction = "http://tempuri.org/IRhinoServices/EnableDrawingResponse")]
	IAsyncResult BeginEnableDrawing(bool isEnabled, AsyncCallback callback, object asyncState);

	void EndEnableDrawing(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ExportDesignAs3DM", ReplyAction = "http://tempuri.org/IRhinoServices/ExportDesignAs3DMResponse")]
	bool ExportDesignAs3DM(string savePath, bool includeCenter, bool includeSide, bool includeAccent);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ExportDesignAs3DM", ReplyAction = "http://tempuri.org/IRhinoServices/ExportDesignAs3DMResponse")]
	IAsyncResult BeginExportDesignAs3DM(string savePath, bool includeCenter, bool includeSide, bool includeAccent, AsyncCallback callback, object asyncState);

	bool EndExportDesignAs3DM(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ExportDesignAsSTL", ReplyAction = "http://tempuri.org/IRhinoServices/ExportDesignAsSTLResponse")]
	bool ExportDesignAsSTL(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ExportDesignAsSTL", ReplyAction = "http://tempuri.org/IRhinoServices/ExportDesignAsSTLResponse")]
	IAsyncResult BeginExportDesignAsSTL(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh, AsyncCallback callback, object asyncState);

	bool EndExportDesignAsSTL(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/ExportImportGroups", ReplyAction = "http://tempuri.org/IRhinoServices/ExportImportGroupsResponse")]
	void ExportImportGroups(Guid[] targetImportGroupIdentifiers, string path);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/ExportImportGroups", ReplyAction = "http://tempuri.org/IRhinoServices/ExportImportGroupsResponse")]
	IAsyncResult BeginExportImportGroups(Guid[] targetImportGroupIdentifiers, string path, AsyncCallback callback, object asyncState);

	void EndExportImportGroups(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/FindProjetPrinter", ReplyAction = "http://tempuri.org/IRhinoServices/FindProjetPrinterResponse")]
	bool FindProjetPrinter(out string FindAddress);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/FindProjetPrinter", ReplyAction = "http://tempuri.org/IRhinoServices/FindProjetPrinterResponse")]
	IAsyncResult BeginFindProjetPrinter(AsyncCallback callback, object asyncState);

	bool EndFindProjetPrinter(out string FindAddress, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetActivationState", ReplyAction = "http://tempuri.org/IRhinoServices/GetActivationStateResponse")]
	bool GetActivationState();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetActivationState", ReplyAction = "http://tempuri.org/IRhinoServices/GetActivationStateResponse")]
	IAsyncResult BeginGetActivationState(AsyncCallback callback, object asyncState);

	bool EndGetActivationState(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetAllRingSizesForCurrentRingSizeRegion", ReplyAction = "http://tempuri.org/IRhinoServices/GetAllRingSizesForCurrentRingSizeRegionResponse")]
	KeyValuePair<int, string>[] GetAllRingSizesForCurrentRingSizeRegion();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetAllRingSizesForCurrentRingSizeRegion", ReplyAction = "http://tempuri.org/IRhinoServices/GetAllRingSizesForCurrentRingSizeRegionResponse")]
	IAsyncResult BeginGetAllRingSizesForCurrentRingSizeRegion(AsyncCallback callback, object asyncState);

	KeyValuePair<int, string>[] EndGetAllRingSizesForCurrentRingSizeRegion(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetComponents", ReplyAction = "http://tempuri.org/IRhinoServices/GetComponentsResponse")]
	ComponentsEnum[] GetComponents();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetComponents", ReplyAction = "http://tempuri.org/IRhinoServices/GetComponentsResponse")]
	IAsyncResult BeginGetComponents(AsyncCallback callback, object asyncState);

	ComponentsEnum[] EndGetComponents(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetCurrentViewportImageBitmap", ReplyAction = "http://tempuri.org/IRhinoServices/GetCurrentViewportImageBitmapResponse")]
	byte[] GetCurrentViewportImageBitmap();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetCurrentViewportImageBitmap", ReplyAction = "http://tempuri.org/IRhinoServices/GetCurrentViewportImageBitmapResponse")]
	IAsyncResult BeginGetCurrentViewportImageBitmap(AsyncCallback callback, object asyncState);

	byte[] EndGetCurrentViewportImageBitmap(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetDesignerFromUserData", ReplyAction = "http://tempuri.org/IRhinoServices/GetDesignerFromUserDataResponse")]
	string GetDesignerFromUserData();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetDesignerFromUserData", ReplyAction = "http://tempuri.org/IRhinoServices/GetDesignerFromUserDataResponse")]
	IAsyncResult BeginGetDesignerFromUserData(AsyncCallback callback, object asyncState);

	string EndGetDesignerFromUserData(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetDesignIterationCount", ReplyAction = "http://tempuri.org/IRhinoServices/GetDesignIterationCountResponse")]
	int GetDesignIterationCount();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetDesignIterationCount", ReplyAction = "http://tempuri.org/IRhinoServices/GetDesignIterationCountResponse")]
	IAsyncResult BeginGetDesignIterationCount(AsyncCallback callback, object asyncState);

	int EndGetDesignIterationCount(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetDisplayObjectBinObjByComponentAndMaterialClassification", ReplyAction = "http://tempuri.org/IRhinoServices/GetDisplayObjectBinObjByComponentAndMaterialClassificationResponse")]
	byte[] GetDisplayObjectBinObjByComponentAndMaterialClassification(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetDisplayObjectBinObjByComponentAndMaterialClassification", ReplyAction = "http://tempuri.org/IRhinoServices/GetDisplayObjectBinObjByComponentAndMaterialClassificationResponse")]
	IAsyncResult BeginGetDisplayObjectBinObjByComponentAndMaterialClassification(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications, AsyncCallback callback, object asyncState);

	byte[] EndGetDisplayObjectBinObjByComponentAndMaterialClassification(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetDocumentFilename", ReplyAction = "http://tempuri.org/IRhinoServices/GetDocumentFilenameResponse")]
	string GetDocumentFilename();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetDocumentFilename", ReplyAction = "http://tempuri.org/IRhinoServices/GetDocumentFilenameResponse")]
	IAsyncResult BeginGetDocumentFilename(AsyncCallback callback, object asyncState);

	string EndGetDocumentFilename(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetFeaturedDesignIterations", ReplyAction = "http://tempuri.org/IRhinoServices/GetFeaturedDesignIterationsResponse")]
	int[] GetFeaturedDesignIterations();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetFeaturedDesignIterations", ReplyAction = "http://tempuri.org/IRhinoServices/GetFeaturedDesignIterationsResponse")]
	IAsyncResult BeginGetFeaturedDesignIterations(AsyncCallback callback, object asyncState);

	int[] EndGetFeaturedDesignIterations(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetGemMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/GetGemMaterialsResponse")]
	GemMaterial[] GetGemMaterials();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetGemMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/GetGemMaterialsResponse")]
	IAsyncResult BeginGetGemMaterials(AsyncCallback callback, object asyncState);

	GemMaterial[] EndGetGemMaterials(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetMetalMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/GetMetalMaterialsResponse")]
	MetalMaterial[] GetMetalMaterials();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetMetalMaterials", ReplyAction = "http://tempuri.org/IRhinoServices/GetMetalMaterialsResponse")]
	IAsyncResult BeginGetMetalMaterials(AsyncCallback callback, object asyncState);

	MetalMaterial[] EndGetMetalMaterials(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetMetalVolumesPerComponent", ReplyAction = "http://tempuri.org/IRhinoServices/GetMetalVolumesPerComponentResponse")]
	Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> GetMetalVolumesPerComponent(bool hasSingleMetalCastingCount, int singleMetalCastingCount);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetMetalVolumesPerComponent", ReplyAction = "http://tempuri.org/IRhinoServices/GetMetalVolumesPerComponentResponse")]
	IAsyncResult BeginGetMetalVolumesPerComponent(bool hasSingleMetalCastingCount, int singleMetalCastingCount, AsyncCallback callback, object asyncState);

	Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> EndGetMetalVolumesPerComponent(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetNotesToCustomerForCurrentDesign", ReplyAction = "http://tempuri.org/IRhinoServices/GetNotesToCustomerForCurrentDesignResponse")]
	string GetNotesToCustomerForCurrentDesign();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetNotesToCustomerForCurrentDesign", ReplyAction = "http://tempuri.org/IRhinoServices/GetNotesToCustomerForCurrentDesignResponse")]
	IAsyncResult BeginGetNotesToCustomerForCurrentDesign(AsyncCallback callback, object asyncState);

	string EndGetNotesToCustomerForCurrentDesign(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetNumberofVisibleFreehandParts", ReplyAction = "http://tempuri.org/IRhinoServices/GetNumberofVisibleFreehandPartsResponse")]
	int GetNumberofVisibleFreehandParts();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetNumberofVisibleFreehandParts", ReplyAction = "http://tempuri.org/IRhinoServices/GetNumberofVisibleFreehandPartsResponse")]
	IAsyncResult BeginGetNumberofVisibleFreehandParts(AsyncCallback callback, object asyncState);

	int EndGetNumberofVisibleFreehandParts(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetPrinterMaterial", ReplyAction = "http://tempuri.org/IRhinoServices/GetPrinterMaterialResponse")]
	bool GetPrinterMaterial(out string message);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetPrinterMaterial", ReplyAction = "http://tempuri.org/IRhinoServices/GetPrinterMaterialResponse")]
	IAsyncResult BeginGetPrinterMaterial(AsyncCallback callback, object asyncState);

	bool EndGetPrinterMaterial(out string message, IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetRhinoSerialCode", ReplyAction = "http://tempuri.org/IRhinoServices/GetRhinoSerialCodeResponse")]
	string GetRhinoSerialCode();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetRhinoSerialCode", ReplyAction = "http://tempuri.org/IRhinoServices/GetRhinoSerialCodeResponse")]
	IAsyncResult BeginGetRhinoSerialCode(AsyncCallback callback, object asyncState);

	string EndGetRhinoSerialCode(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetRhinoThreadId", ReplyAction = "http://tempuri.org/IRhinoServices/GetRhinoThreadIdResponse")]
	int GetRhinoThreadId();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetRhinoThreadId", ReplyAction = "http://tempuri.org/IRhinoServices/GetRhinoThreadIdResponse")]
	IAsyncResult BeginGetRhinoThreadId(AsyncCallback callback, object asyncState);

	int EndGetRhinoThreadId(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetRhinoWindowHandle", ReplyAction = "http://tempuri.org/IRhinoServices/GetRhinoWindowHandleResponse")]
	IntPtr GetRhinoWindowHandle();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetRhinoWindowHandle", ReplyAction = "http://tempuri.org/IRhinoServices/GetRhinoWindowHandleResponse")]
	IAsyncResult BeginGetRhinoWindowHandle(AsyncCallback callback, object asyncState);

	IntPtr EndGetRhinoWindowHandle(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetSavedJobs", ReplyAction = "http://tempuri.org/IRhinoServices/GetSavedJobsResponse")]
	int[] GetSavedJobs();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetSavedJobs", ReplyAction = "http://tempuri.org/IRhinoServices/GetSavedJobsResponse")]
	IAsyncResult BeginGetSavedJobs(AsyncCallback callback, object asyncState);

	int[] EndGetSavedJobs(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetSPINumberFromUserData", ReplyAction = "http://tempuri.org/IRhinoServices/GetSPINumberFromUserDataResponse")]
	string GetSPINumberFromUserData();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetSPINumberFromUserData", ReplyAction = "http://tempuri.org/IRhinoServices/GetSPINumberFromUserDataResponse")]
	IAsyncResult BeginGetSPINumberFromUserData(AsyncCallback callback, object asyncState);

	string EndGetSPINumberFromUserData(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetThumbnailFromFile", ReplyAction = "http://tempuri.org/IRhinoServices/GetThumbnailFromFileResponse")]
	BitmapSourceByteArrayInfo GetThumbnailFromFile(string filename);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetThumbnailFromFile", ReplyAction = "http://tempuri.org/IRhinoServices/GetThumbnailFromFileResponse")]
	IAsyncResult BeginGetThumbnailFromFile(string filename, AsyncCallback callback, object asyncState);

	BitmapSourceByteArrayInfo EndGetThumbnailFromFile(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GetVisibleComponents", ReplyAction = "http://tempuri.org/IRhinoServices/GetVisibleComponentsResponse")]
	ComponentsEnum[] GetVisibleComponents();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GetVisibleComponents", ReplyAction = "http://tempuri.org/IRhinoServices/GetVisibleComponentsResponse")]
	IAsyncResult BeginGetVisibleComponents(AsyncCallback callback, object asyncState);

	ComponentsEnum[] EndGetVisibleComponents(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GraphItemAddUserDataValue", ReplyAction = "http://tempuri.org/IRhinoServices/GraphItemAddUserDataValueResponse")]
	void GraphItemAddUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GraphItemAddUserDataValue", ReplyAction = "http://tempuri.org/IRhinoServices/GraphItemAddUserDataValueResponse")]
	IAsyncResult BeginGraphItemAddUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue, AsyncCallback callback, object asyncState);

	void EndGraphItemAddUserDataValue(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GraphItemFindUserDataValue", ReplyAction = "http://tempuri.org/IRhinoServices/GraphItemFindUserDataValueResponse")]
	string GraphItemFindUserDataValue(Guid blockInstanceGuid, string typeName, string keyName);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GraphItemFindUserDataValue", ReplyAction = "http://tempuri.org/IRhinoServices/GraphItemFindUserDataValueResponse")]
	IAsyncResult BeginGraphItemFindUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, AsyncCallback callback, object asyncState);

	string EndGraphItemFindUserDataValue(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/IRhinoServices/GraphRedoChange", ReplyAction = "http://tempuri.org/IRhinoServices/GraphRedoChangeResponse")]
	UIControl[] GraphRedoChange();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IRhinoServices/GraphRedoChange", ReplyAction = "http://tempuri.org/IRhinoServices/GraphRedoChangeResponse")]
	IAsyncResult BeginGraphRedoChange(AsyncCallback callback, object asyncState);

	UIControl[] EndGraphRedoChange(IAsyncResult result);
}
