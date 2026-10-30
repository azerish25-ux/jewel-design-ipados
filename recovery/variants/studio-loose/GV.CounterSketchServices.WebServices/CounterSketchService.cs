using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace GV.CounterSketchServices.WebServices;

[GeneratedCode("wsdl", "4.0.30319.18020")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[WebServiceBinding(Name = "CounterSketchServiceSoap", Namespace = "http://CounterSketch.biz/")]
[XmlInclude(typeof(RelatedEnd))]
[XmlInclude(typeof(StructuralObject))]
[XmlInclude(typeof(IterationAttribute[]))]
[XmlInclude(typeof(EntityKeyMember[]))]
public class CounterSketchService : SoapHttpClientProtocol
{
	private SendOrPostCallback DownloadFileOperationCompleted;

	private SendOrPostCallback GetDesignerByGuidOperationCompleted;

	private SendOrPostCallback GetAssignmentsByDesignerGuidOperationCompleted;

	private SendOrPostCallback GetAllDesignTypesOperationCompleted;

	private SendOrPostCallback GetAssignmentNumberByAssignmentIdOperationCompleted;

	private SendOrPostCallback HasAssignmentDocumentByAssignmentIdOperationCompleted;

	private SendOrPostCallback FindADesignWithGuidOperationCompleted;

	private SendOrPostCallback UploadFileOperationCompleted;

	private SendOrPostCallback ReviewerUploadFileTempOperationCompleted;

	private SendOrPostCallback GetDesignsByDesignerGuidOperationCompleted;

	private SendOrPostCallback GetDesignerByUsernameOperationCompleted;

	private SendOrPostCallback CSG_DesignVersionAddAttributeOperationCompleted;

	private SendOrPostCallback CSG_DesignVersionUpdateAttributeOperationCompleted;

	private SendOrPostCallback CSG_DesignVersionDeleteAttributeOperationCompleted;

	private SendOrPostCallback CSG_DesignVersionComponentAddAttributeOperationCompleted;

	private SendOrPostCallback CSG_DesignVersionComponentUpdateAttributeOperationCompleted;

	private SendOrPostCallback CSG_DesignVersionComponentDeleteAttributeOperationCompleted;

	private SendOrPostCallback CSG_AddAttributeToDesignComponentNoRepeatsOperationCompleted;

	private SendOrPostCallback CSG_RingRailSetDesignOperationCompleted;

	private SendOrPostCallback CSG_RingRailDeleteDesignOperationCompleted;

	private SendOrPostCallback CSG_GetDesignIdFromDesignVersionIDOperationCompleted;

	private SendOrPostCallback CSG_GetVersionFromDesignVersionIDOperationCompleted;

	private SendOrPostCallback CSG_GetMaxVersionFromDesignIdOperationCompleted;

	private SendOrPostCallback CSG_DownloadOriginalFileOperationCompleted;

	private SendOrPostCallback CSG_DownloadCleanFileOperationCompleted;

	private SendOrPostCallback CSG_UploadCleanedFileOperationCompleted;

	private SendOrPostCallback CSG_GetDesignAttributeValueByDesignIdAndAttributeIdOperationCompleted;

	private SendOrPostCallback CSG_SetComponentListOperationCompleted;

	private SendOrPostCallback CSG_DeleteGemsFromDesignVersionOperationCompleted;

	private SendOrPostCallback CSG_DeleteGemComponentDesignVersionIdOperationCompleted;

	private SendOrPostCallback CSG_AddGemToDesignOperationCompleted;

	private SendOrPostCallback CSG_SetNumberOfControlsOperationCompleted;

	private SendOrPostCallback CSG_AddVolumeToComponentOperationCompleted;

	private SendOrPostCallback CSG_AddMetalCountToComponentOperationCompleted;

	private SendOrPostCallback CSG_SetBlocksUsedOperationCompleted;

	private SendOrPostCallback CSG_GetDesignAndRelatedInfoByIdOperationCompleted;

	private SendOrPostCallback CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdOperationCompleted;

	private SendOrPostCallback CSD_SetDesignerBlocksOperationCompleted;

	private SendOrPostCallback GetSurveyQuestionsOperationCompleted;

	private SendOrPostCallback SaveFeedbackOperationCompleted;

	private SendOrPostCallback GetLatestPublicCounterSketchVersionStringOperationCompleted;

	private SendOrPostCallback GetMyLibraryFilesOperationCompleted;

	private SendOrPostCallback GetMyLibraryZBinOperationCompleted;

	private SendOrPostCallback GetMyLibraryVersionOperationCompleted;

	private SendOrPostCallback TransmitDetailedDebuggingReportOperationCompleted;

	private SendOrPostCallback TransmitDebuggingReportOperationCompleted;

	private SendOrPostCallback GeoCodeAddressOperationCompleted;

	private SendOrPostCallback UpdateKeyInformation_2OperationCompleted;

	private SendOrPostCallback GetDebugVersionStringOperationCompleted;

	private SendOrPostCallback GetMyCounterSketchVersionStringOperationCompleted;

	private SendOrPostCallback setVersionOperationCompleted;

	private SendOrPostCallback setVersion1OperationCompleted;

	private SendOrPostCallback GetUserTypeIdOperationCompleted;

	private SendOrPostCallback GetDesignVersionComponentIdOperationCompleted;

	private SendOrPostCallback AddAnswerOperationCompleted;

	private SendOrPostCallback AddAssignmentOperationCompleted;

	private SendOrPostCallback AddAssignmentNewOperationCompleted;

	private SendOrPostCallback AddQuestionOperationCompleted;

	private SendOrPostCallback DesignCanBeStullerApprovedOperationCompleted;

	private SendOrPostCallback DesignLockOperationCompleted;

	private SendOrPostCallback DesignLockInfoOperationCompleted;

	private SendOrPostCallback DesignReleaseLockOperationCompleted;

	private SendOrPostCallback DesignsSetStatusOperationCompleted;

	private SendOrPostCallback DownloadLatestCounterSketchVersionOperationCompleted;

	private SendOrPostCallback FindADesignOperationCompleted;

	private SendOrPostCallback FindADesign2OperationCompleted;

	private SendOrPostCallback GetAllAttributesOperationCompleted;

	private SendOrPostCallback GetAllAttributeTypesOperationCompleted;

	private SendOrPostCallback GetAllDesignsOperationCompleted;

	private SendOrPostCallback GetAllDesignsAndRelatedInfoOperationCompleted;

	private SendOrPostCallback GetAllDesignsAndRelatedInfoByKewordSearchOperationCompleted;

	private SendOrPostCallback GetAllDesignsByPaidStatusOperationCompleted;

	private SendOrPostCallback GetAllDesignsByStatusesOrderByDesignIdOperationCompleted;

	private SendOrPostCallback GetAllDesignsByStatusesOrderByDesignIdByKewordSearchOperationCompleted;

	private SendOrPostCallback GetAllDesignsByStatusOrderByDesignIdOperationCompleted;

	private SendOrPostCallback GetAllDesignsByStatusOrderByStatusDateOperationCompleted;

	private SendOrPostCallback GetAllDesignsByStatusOrderByStatusDateByKewordSearchOperationCompleted;

	private SendOrPostCallback GetAllQuestionTypesOperationCompleted;

	private SendOrPostCallback GetAllStullerComponentAttributeTypesOperationCompleted;

	private SendOrPostCallback GetAssignmentGroupByIdOperationCompleted;

	private SendOrPostCallback GetDesignMetalSpecificationByDesignComponentIdOperationCompleted;

	private SendOrPostCallback GetDesignRingRailSpecificationByDesignIdOperationCompleted;

	private SendOrPostCallback GetKeyCodeByKeyIdOperationCompleted;

	private SendOrPostCallback GetUserTypesOperationCompleted;

	private SendOrPostCallback UpdateUserTypeOnKeyCodeOperationCompleted;

	private SendOrPostCallback GetLatestCounterSketchVersionStringOperationCompleted;

	private SendOrPostCallback GetRequiredLibraryFilesOperationCompleted;

	private SendOrPostCallback GetServiceExceptionHistoryOperationCompleted;

	private SendOrPostCallback HasActiveSurveysOperationCompleted;

	private SendOrPostCallback UpdateAssignmentOnDesignerOperationCompleted;

	private SendOrPostCallback UpdateDesignPaidStatusOperationCompleted;

	private SendOrPostCallback UpdateKeyInformationOperationCompleted;

	private SendOrPostCallback ValidateDesignerInfoOperationCompleted;

	private SendOrPostCallback GetDesignComponentCountByDesignIdOperationCompleted;

	private SendOrPostCallback GetDesignComponentAttributesByDesignComponentIdOperationCompleted;

	private SendOrPostCallback GetLastNoticeIdOperationCompleted;

	private SendOrPostCallback GetNoticeTypeByNoticeIdOperationCompleted;

	private SendOrPostCallback GetLatestNoticeByKeyIdOperationCompleted;

	private SendOrPostCallback SendApplicationEventsOperationCompleted;

	private SendOrPostCallback SendImmediateEventOperationCompleted;

	private SendOrPostCallback SendInstallationSpecificApplicationEventsOperationCompleted;

	private SendOrPostCallback SendMailFromUserOperationCompleted;

	private SendOrPostCallback SendMailOperationCompleted;

	private SendOrPostCallback GetExchangeRateOperationCompleted;

	private SendOrPostCallback GetQuickEstimateOperationCompleted;

	private SendOrPostCallback GetQuickEstimateWeightCorrectionsByDesignIdOperationCompleted;

	private SendOrPostCallback GetQuotesByKeyIdOperationCompleted;

	private SendOrPostCallback SendPrototypeOrderOperationCompleted;

	private SendOrPostCallback SendQuoteOrderRequestOperationCompleted;

	private SendOrPostCallback SendQuoteRequestOperationCompleted;

	private SendOrPostCallback SendRequestForQuoteOperationCompleted;

	private SendOrPostCallback SendFullRequestForQuoteOperationCompleted;

	private SendOrPostCallback SendDetailedRequestForQuoteOperationCompleted;

	private SendOrPostCallback GetCatalogOrdersOperationCompleted;

	private SendOrPostCallback GetCatalogOrderByIdOperationCompleted;

	private SendOrPostCallback GetCatalogOrdersByKeyIdOperationCompleted;

	private SendOrPostCallback CreateCatalogOrderOperationCompleted;

	private SendOrPostCallback CreateCatalogOrderNewOperationCompleted;

	private SendOrPostCallback UpdateCatalogOrderOperationCompleted;

	private SendOrPostCallback UpdateCatalogOrderShipmentOperationCompleted;

	private SendOrPostCallback UpdateCatalogOrderCreationStatusOperationCompleted;

	private SendOrPostCallback DeleteCatalogOrderOperationCompleted;

	private SendOrPostCallback GetCatalogCostPerPageOperationCompleted;

	private SendOrPostCallback GetCatalogBaseShippingCostOperationCompleted;

	private SendOrPostCallback GetCatalogPerCopyShippingCostOperationCompleted;

	private SendOrPostCallback GetDesignSignatureOperationCompleted;

	private SendOrPostCallback GetDesignSignaturesOperationCompleted;

	private SendOrPostCallback UploadCAMSettingsOperationCompleted;

	private SendOrPostCallback UploadCAMSettingsFileOperationCompleted;

	private SendOrPostCallback GetCAMSettingFilesOperationCompleted;

	private SendOrPostCallback DownloadCAMSettingsOperationCompleted;

	private SendOrPostCallback DownloadCAMSettingsFileOperationCompleted;

	private SendOrPostCallback DownloadDesignForReviewOperationCompleted;

	private SendOrPostCallback DesignIteraionSetNotFoundOperationCompleted;

	private SendOrPostCallback GetDesignIterationValuesOperationCompleted;

	private SendOrPostCallback DesignIteraionComponentAddOperationCompleted;

	private SendOrPostCallback DesignIteraionComponentSetFoundAndDeleteAttributesOperationCompleted;

	private SendOrPostCallback IterationAttributeSetOperationCompleted;

	private SendOrPostCallback IterationComponentAttributeSetOperationCompleted;

	private SendOrPostCallback IterationComponentAttributeClearOperationCompleted;

	private SendOrPostCallback IterationComponentAttributeIndexSetListOperationCompleted;

	private SendOrPostCallback IterationComponentAttributeIndexSetOperationCompleted;

	private SendOrPostCallback IterationComponentAttributeGetOperationCompleted;

	private SendOrPostCallback DesignIterationComponantAttributesGetOperationCompleted;

	private SendOrPostCallback GetDesignInfoOperationCompleted;

	private SendOrPostCallback GetAllBandSizesOperationCompleted;

	private SendOrPostCallback CreateNewBandOperationCompleted;

	private SendOrPostCallback CopyAttributesFromIDToLinkedIDOperationCompleted;

	private SendOrPostCallback SetLinkedIDToOperationCompleted;

	private SendOrPostCallback GetDICIDsFromLinkedDICIDOperationCompleted;

	private SendOrPostCallback GetValidIterationsListOperationCompleted;

	private SendOrPostCallback getMatchingBandsOperationCompleted;

	private SendOrPostCallback DesignIteraionAddOperationCompleted;

	private SendOrPostCallback DesignNumbersGetOperationCompleted;

	private SendOrPostCallback GetIterationOptionsOperationCompleted;

	private SendOrPostCallback GetIterationAttributesByTypeIdOperationCompleted;

	private SendOrPostCallback IterationAttributeIdGetOperationCompleted;

	private SendOrPostCallback GetValidIterationsOperationCompleted;

	private SendOrPostCallback IterationComponentAttributeIndexGetOperationCompleted;

	private SendOrPostCallback GetIterationGemInfoOperationCompleted;

	private SendOrPostCallback GetAllDesignIterationComponentIdOperationCompleted;

	private SendOrPostCallback GetAllDesignIterationComponentIdFoundOperationCompleted;

	private SendOrPostCallback StullerServicesAreActiveOperationCompleted;

	private SendOrPostCallback GetLatestDesignerVersionStringOperationCompleted;

	private SendOrPostCallback GetDesignHistoryOperationCompleted;

	private SendOrPostCallback DownloadImageOperationCompleted;

	private SendOrPostCallback GetDesignFilenameByDesignIdOperationCompleted;

	public event DownloadFileCompletedEventHandler DownloadFileCompleted;

	public event GetDesignerByGuidCompletedEventHandler GetDesignerByGuidCompleted;

	public event GetAssignmentsByDesignerGuidCompletedEventHandler GetAssignmentsByDesignerGuidCompleted;

	public event GetAllDesignTypesCompletedEventHandler GetAllDesignTypesCompleted;

	public event GetAssignmentNumberByAssignmentIdCompletedEventHandler GetAssignmentNumberByAssignmentIdCompleted;

	public event HasAssignmentDocumentByAssignmentIdCompletedEventHandler HasAssignmentDocumentByAssignmentIdCompleted;

	public event FindADesignWithGuidCompletedEventHandler FindADesignWithGuidCompleted;

	public event UploadFileCompletedEventHandler UploadFileCompleted;

	public event ReviewerUploadFileTempCompletedEventHandler ReviewerUploadFileTempCompleted;

	public event GetDesignsByDesignerGuidCompletedEventHandler GetDesignsByDesignerGuidCompleted;

	public event GetDesignerByUsernameCompletedEventHandler GetDesignerByUsernameCompleted;

	public event CSG_DesignVersionAddAttributeCompletedEventHandler CSG_DesignVersionAddAttributeCompleted;

	public event CSG_DesignVersionUpdateAttributeCompletedEventHandler CSG_DesignVersionUpdateAttributeCompleted;

	public event CSG_DesignVersionDeleteAttributeCompletedEventHandler CSG_DesignVersionDeleteAttributeCompleted;

	public event CSG_DesignVersionComponentAddAttributeCompletedEventHandler CSG_DesignVersionComponentAddAttributeCompleted;

	public event CSG_DesignVersionComponentUpdateAttributeCompletedEventHandler CSG_DesignVersionComponentUpdateAttributeCompleted;

	public event CSG_DesignVersionComponentDeleteAttributeCompletedEventHandler CSG_DesignVersionComponentDeleteAttributeCompleted;

	public event CSG_AddAttributeToDesignComponentNoRepeatsCompletedEventHandler CSG_AddAttributeToDesignComponentNoRepeatsCompleted;

	public event CSG_RingRailSetDesignCompletedEventHandler CSG_RingRailSetDesignCompleted;

	public event CSG_RingRailDeleteDesignCompletedEventHandler CSG_RingRailDeleteDesignCompleted;

	public event CSG_GetDesignIdFromDesignVersionIDCompletedEventHandler CSG_GetDesignIdFromDesignVersionIDCompleted;

	public event CSG_GetVersionFromDesignVersionIDCompletedEventHandler CSG_GetVersionFromDesignVersionIDCompleted;

	public event CSG_GetMaxVersionFromDesignIdCompletedEventHandler CSG_GetMaxVersionFromDesignIdCompleted;

	public event CSG_DownloadOriginalFileCompletedEventHandler CSG_DownloadOriginalFileCompleted;

	public event CSG_DownloadCleanFileCompletedEventHandler CSG_DownloadCleanFileCompleted;

	public event CSG_UploadCleanedFileCompletedEventHandler CSG_UploadCleanedFileCompleted;

	public event CSG_GetDesignAttributeValueByDesignIdAndAttributeIdCompletedEventHandler CSG_GetDesignAttributeValueByDesignIdAndAttributeIdCompleted;

	public event CSG_SetComponentListCompletedEventHandler CSG_SetComponentListCompleted;

	public event CSG_DeleteGemsFromDesignVersionCompletedEventHandler CSG_DeleteGemsFromDesignVersionCompleted;

	public event CSG_DeleteGemComponentDesignVersionIdCompletedEventHandler CSG_DeleteGemComponentDesignVersionIdCompleted;

	public event CSG_AddGemToDesignCompletedEventHandler CSG_AddGemToDesignCompleted;

	public event CSG_SetNumberOfControlsCompletedEventHandler CSG_SetNumberOfControlsCompleted;

	public event CSG_AddVolumeToComponentCompletedEventHandler CSG_AddVolumeToComponentCompleted;

	public event CSG_AddMetalCountToComponentCompletedEventHandler CSG_AddMetalCountToComponentCompleted;

	public event CSG_SetBlocksUsedCompletedEventHandler CSG_SetBlocksUsedCompleted;

	public event CSG_GetDesignAndRelatedInfoByIdCompletedEventHandler CSG_GetDesignAndRelatedInfoByIdCompleted;

	public event CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdCompletedEventHandler CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdCompleted;

	public event CSD_SetDesignerBlocksCompletedEventHandler CSD_SetDesignerBlocksCompleted;

	public event GetSurveyQuestionsCompletedEventHandler GetSurveyQuestionsCompleted;

	public event SaveFeedbackCompletedEventHandler SaveFeedbackCompleted;

	public event GetLatestPublicCounterSketchVersionStringCompletedEventHandler GetLatestPublicCounterSketchVersionStringCompleted;

	public event GetMyLibraryFilesCompletedEventHandler GetMyLibraryFilesCompleted;

	public event GetMyLibraryZBinCompletedEventHandler GetMyLibraryZBinCompleted;

	public event GetMyLibraryVersionCompletedEventHandler GetMyLibraryVersionCompleted;

	public event TransmitDetailedDebuggingReportCompletedEventHandler TransmitDetailedDebuggingReportCompleted;

	public event TransmitDebuggingReportCompletedEventHandler TransmitDebuggingReportCompleted;

	public event GeoCodeAddressCompletedEventHandler GeoCodeAddressCompleted;

	public event UpdateKeyInformation_2CompletedEventHandler UpdateKeyInformation_2Completed;

	public event GetDebugVersionStringCompletedEventHandler GetDebugVersionStringCompleted;

	public event GetMyCounterSketchVersionStringCompletedEventHandler GetMyCounterSketchVersionStringCompleted;

	public event setVersionCompletedEventHandler setVersionCompleted;

	public event setVersion1CompletedEventHandler setVersion1Completed;

	public event GetUserTypeIdCompletedEventHandler GetUserTypeIdCompleted;

	public event GetDesignVersionComponentIdCompletedEventHandler GetDesignVersionComponentIdCompleted;

	public event AddAnswerCompletedEventHandler AddAnswerCompleted;

	public event AddAssignmentCompletedEventHandler AddAssignmentCompleted;

	public event AddAssignmentNewCompletedEventHandler AddAssignmentNewCompleted;

	public event AddQuestionCompletedEventHandler AddQuestionCompleted;

	public event DesignCanBeStullerApprovedCompletedEventHandler DesignCanBeStullerApprovedCompleted;

	public event DesignLockCompletedEventHandler DesignLockCompleted;

	public event DesignLockInfoCompletedEventHandler DesignLockInfoCompleted;

	public event DesignReleaseLockCompletedEventHandler DesignReleaseLockCompleted;

	public event DesignsSetStatusCompletedEventHandler DesignsSetStatusCompleted;

	public event DownloadLatestCounterSketchVersionCompletedEventHandler DownloadLatestCounterSketchVersionCompleted;

	public event FindADesignCompletedEventHandler FindADesignCompleted;

	public event FindADesign2CompletedEventHandler FindADesign2Completed;

	public event GetAllAttributesCompletedEventHandler GetAllAttributesCompleted;

	public event GetAllAttributeTypesCompletedEventHandler GetAllAttributeTypesCompleted;

	public event GetAllDesignsCompletedEventHandler GetAllDesignsCompleted;

	public event GetAllDesignsAndRelatedInfoCompletedEventHandler GetAllDesignsAndRelatedInfoCompleted;

	public event GetAllDesignsAndRelatedInfoByKewordSearchCompletedEventHandler GetAllDesignsAndRelatedInfoByKewordSearchCompleted;

	public event GetAllDesignsByPaidStatusCompletedEventHandler GetAllDesignsByPaidStatusCompleted;

	public event GetAllDesignsByStatusesOrderByDesignIdCompletedEventHandler GetAllDesignsByStatusesOrderByDesignIdCompleted;

	public event GetAllDesignsByStatusesOrderByDesignIdByKewordSearchCompletedEventHandler GetAllDesignsByStatusesOrderByDesignIdByKewordSearchCompleted;

	public event GetAllDesignsByStatusOrderByDesignIdCompletedEventHandler GetAllDesignsByStatusOrderByDesignIdCompleted;

	public event GetAllDesignsByStatusOrderByStatusDateCompletedEventHandler GetAllDesignsByStatusOrderByStatusDateCompleted;

	public event GetAllDesignsByStatusOrderByStatusDateByKewordSearchCompletedEventHandler GetAllDesignsByStatusOrderByStatusDateByKewordSearchCompleted;

	public event GetAllQuestionTypesCompletedEventHandler GetAllQuestionTypesCompleted;

	public event GetAllStullerComponentAttributeTypesCompletedEventHandler GetAllStullerComponentAttributeTypesCompleted;

	public event GetAssignmentGroupByIdCompletedEventHandler GetAssignmentGroupByIdCompleted;

	public event GetDesignMetalSpecificationByDesignComponentIdCompletedEventHandler GetDesignMetalSpecificationByDesignComponentIdCompleted;

	public event GetDesignRingRailSpecificationByDesignIdCompletedEventHandler GetDesignRingRailSpecificationByDesignIdCompleted;

	public event GetKeyCodeByKeyIdCompletedEventHandler GetKeyCodeByKeyIdCompleted;

	public event GetUserTypesCompletedEventHandler GetUserTypesCompleted;

	public event UpdateUserTypeOnKeyCodeCompletedEventHandler UpdateUserTypeOnKeyCodeCompleted;

	public event GetLatestCounterSketchVersionStringCompletedEventHandler GetLatestCounterSketchVersionStringCompleted;

	public event GetRequiredLibraryFilesCompletedEventHandler GetRequiredLibraryFilesCompleted;

	public event GetServiceExceptionHistoryCompletedEventHandler GetServiceExceptionHistoryCompleted;

	public event HasActiveSurveysCompletedEventHandler HasActiveSurveysCompleted;

	public event UpdateAssignmentOnDesignerCompletedEventHandler UpdateAssignmentOnDesignerCompleted;

	public event UpdateDesignPaidStatusCompletedEventHandler UpdateDesignPaidStatusCompleted;

	public event UpdateKeyInformationCompletedEventHandler UpdateKeyInformationCompleted;

	public event ValidateDesignerInfoCompletedEventHandler ValidateDesignerInfoCompleted;

	public event GetDesignComponentCountByDesignIdCompletedEventHandler GetDesignComponentCountByDesignIdCompleted;

	public event GetDesignComponentAttributesByDesignComponentIdCompletedEventHandler GetDesignComponentAttributesByDesignComponentIdCompleted;

	public event GetLastNoticeIdCompletedEventHandler GetLastNoticeIdCompleted;

	public event GetNoticeTypeByNoticeIdCompletedEventHandler GetNoticeTypeByNoticeIdCompleted;

	public event GetLatestNoticeByKeyIdCompletedEventHandler GetLatestNoticeByKeyIdCompleted;

	public event SendApplicationEventsCompletedEventHandler SendApplicationEventsCompleted;

	public event SendImmediateEventCompletedEventHandler SendImmediateEventCompleted;

	public event SendInstallationSpecificApplicationEventsCompletedEventHandler SendInstallationSpecificApplicationEventsCompleted;

	public event SendMailFromUserCompletedEventHandler SendMailFromUserCompleted;

	public event SendMailCompletedEventHandler SendMailCompleted;

	public event GetExchangeRateCompletedEventHandler GetExchangeRateCompleted;

	public event GetQuickEstimateCompletedEventHandler GetQuickEstimateCompleted;

	public event GetQuickEstimateWeightCorrectionsByDesignIdCompletedEventHandler GetQuickEstimateWeightCorrectionsByDesignIdCompleted;

	public event GetQuotesByKeyIdCompletedEventHandler GetQuotesByKeyIdCompleted;

	public event SendPrototypeOrderCompletedEventHandler SendPrototypeOrderCompleted;

	public event SendQuoteOrderRequestCompletedEventHandler SendQuoteOrderRequestCompleted;

	public event SendQuoteRequestCompletedEventHandler SendQuoteRequestCompleted;

	public event SendRequestForQuoteCompletedEventHandler SendRequestForQuoteCompleted;

	public event SendFullRequestForQuoteCompletedEventHandler SendFullRequestForQuoteCompleted;

	public event SendDetailedRequestForQuoteCompletedEventHandler SendDetailedRequestForQuoteCompleted;

	public event GetCatalogOrdersCompletedEventHandler GetCatalogOrdersCompleted;

	public event GetCatalogOrderByIdCompletedEventHandler GetCatalogOrderByIdCompleted;

	public event GetCatalogOrdersByKeyIdCompletedEventHandler GetCatalogOrdersByKeyIdCompleted;

	public event CreateCatalogOrderCompletedEventHandler CreateCatalogOrderCompleted;

	public event CreateCatalogOrderNewCompletedEventHandler CreateCatalogOrderNewCompleted;

	public event UpdateCatalogOrderCompletedEventHandler UpdateCatalogOrderCompleted;

	public event UpdateCatalogOrderShipmentCompletedEventHandler UpdateCatalogOrderShipmentCompleted;

	public event UpdateCatalogOrderCreationStatusCompletedEventHandler UpdateCatalogOrderCreationStatusCompleted;

	public event DeleteCatalogOrderCompletedEventHandler DeleteCatalogOrderCompleted;

	public event GetCatalogCostPerPageCompletedEventHandler GetCatalogCostPerPageCompleted;

	public event GetCatalogBaseShippingCostCompletedEventHandler GetCatalogBaseShippingCostCompleted;

	public event GetCatalogPerCopyShippingCostCompletedEventHandler GetCatalogPerCopyShippingCostCompleted;

	public event GetDesignSignatureCompletedEventHandler GetDesignSignatureCompleted;

	public event GetDesignSignaturesCompletedEventHandler GetDesignSignaturesCompleted;

	public event UploadCAMSettingsCompletedEventHandler UploadCAMSettingsCompleted;

	public event UploadCAMSettingsFileCompletedEventHandler UploadCAMSettingsFileCompleted;

	public event GetCAMSettingFilesCompletedEventHandler GetCAMSettingFilesCompleted;

	public event DownloadCAMSettingsCompletedEventHandler DownloadCAMSettingsCompleted;

	public event DownloadCAMSettingsFileCompletedEventHandler DownloadCAMSettingsFileCompleted;

	public event DownloadDesignForReviewCompletedEventHandler DownloadDesignForReviewCompleted;

	public event DesignIteraionSetNotFoundCompletedEventHandler DesignIteraionSetNotFoundCompleted;

	public event GetDesignIterationValuesCompletedEventHandler GetDesignIterationValuesCompleted;

	public event DesignIteraionComponentAddCompletedEventHandler DesignIteraionComponentAddCompleted;

	public event DesignIteraionComponentSetFoundAndDeleteAttributesCompletedEventHandler DesignIteraionComponentSetFoundAndDeleteAttributesCompleted;

	public event IterationAttributeSetCompletedEventHandler IterationAttributeSetCompleted;

	public event IterationComponentAttributeSetCompletedEventHandler IterationComponentAttributeSetCompleted;

	public event IterationComponentAttributeClearCompletedEventHandler IterationComponentAttributeClearCompleted;

	public event IterationComponentAttributeIndexSetListCompletedEventHandler IterationComponentAttributeIndexSetListCompleted;

	public event IterationComponentAttributeIndexSetCompletedEventHandler IterationComponentAttributeIndexSetCompleted;

	public event IterationComponentAttributeGetCompletedEventHandler IterationComponentAttributeGetCompleted;

	public event DesignIterationComponantAttributesGetCompletedEventHandler DesignIterationComponantAttributesGetCompleted;

	public event GetDesignInfoCompletedEventHandler GetDesignInfoCompleted;

	public event GetAllBandSizesCompletedEventHandler GetAllBandSizesCompleted;

	public event CreateNewBandCompletedEventHandler CreateNewBandCompleted;

	public event CopyAttributesFromIDToLinkedIDCompletedEventHandler CopyAttributesFromIDToLinkedIDCompleted;

	public event SetLinkedIDToCompletedEventHandler SetLinkedIDToCompleted;

	public event GetDICIDsFromLinkedDICIDCompletedEventHandler GetDICIDsFromLinkedDICIDCompleted;

	public event GetValidIterationsListCompletedEventHandler GetValidIterationsListCompleted;

	public event getMatchingBandsCompletedEventHandler getMatchingBandsCompleted;

	public event DesignIteraionAddCompletedEventHandler DesignIteraionAddCompleted;

	public event DesignNumbersGetCompletedEventHandler DesignNumbersGetCompleted;

	public event GetIterationOptionsCompletedEventHandler GetIterationOptionsCompleted;

	public event GetIterationAttributesByTypeIdCompletedEventHandler GetIterationAttributesByTypeIdCompleted;

	public event IterationAttributeIdGetCompletedEventHandler IterationAttributeIdGetCompleted;

	public event GetValidIterationsCompletedEventHandler GetValidIterationsCompleted;

	public event IterationComponentAttributeIndexGetCompletedEventHandler IterationComponentAttributeIndexGetCompleted;

	public event GetIterationGemInfoCompletedEventHandler GetIterationGemInfoCompleted;

	public event GetAllDesignIterationComponentIdCompletedEventHandler GetAllDesignIterationComponentIdCompleted;

	public event GetAllDesignIterationComponentIdFoundCompletedEventHandler GetAllDesignIterationComponentIdFoundCompleted;

	public event StullerServicesAreActiveCompletedEventHandler StullerServicesAreActiveCompleted;

	public event GetLatestDesignerVersionStringCompletedEventHandler GetLatestDesignerVersionStringCompleted;

	public event GetDesignHistoryCompletedEventHandler GetDesignHistoryCompleted;

	public event DownloadImageCompletedEventHandler DownloadImageCompleted;

	public event GetDesignFilenameByDesignIdCompletedEventHandler GetDesignFilenameByDesignIdCompleted;

	[SoapDocumentMethod("http://CounterSketch.biz/DownloadFile", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] DownloadFile(int designVersionId)
	{
		return (byte[])Invoke("DownloadFile", new object[1] { designVersionId })[0];
	}

	public IAsyncResult BeginDownloadFile(int designVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DownloadFile", new object[1] { designVersionId }, callback, asyncState);
	}

	public byte[] EndDownloadFile(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void DownloadFileAsync(int designVersionId)
	{
		DownloadFileAsync(designVersionId, null);
	}

	public void DownloadFileAsync(int designVersionId, object userState)
	{
		if (DownloadFileOperationCompleted == null)
		{
			DownloadFileOperationCompleted = OnDownloadFileOperationCompleted;
		}
		InvokeAsync("DownloadFile", new object[1] { designVersionId }, DownloadFileOperationCompleted, userState);
	}

	private void OnDownloadFileOperationCompleted(object arg)
	{
		if (this.DownloadFileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DownloadFileCompleted(this, new DownloadFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignerByGuid", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignerByGuid(Guid designerGuid)
	{
		return (DataTable)Invoke("GetDesignerByGuid", new object[1] { designerGuid })[0];
	}

	public IAsyncResult BeginGetDesignerByGuid(Guid designerGuid, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignerByGuid", new object[1] { designerGuid }, callback, asyncState);
	}

	public DataTable EndGetDesignerByGuid(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignerByGuidAsync(Guid designerGuid)
	{
		GetDesignerByGuidAsync(designerGuid, null);
	}

	public void GetDesignerByGuidAsync(Guid designerGuid, object userState)
	{
		if (GetDesignerByGuidOperationCompleted == null)
		{
			GetDesignerByGuidOperationCompleted = OnGetDesignerByGuidOperationCompleted;
		}
		InvokeAsync("GetDesignerByGuid", new object[1] { designerGuid }, GetDesignerByGuidOperationCompleted, userState);
	}

	private void OnGetDesignerByGuidOperationCompleted(object arg)
	{
		if (this.GetDesignerByGuidCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignerByGuidCompleted(this, new GetDesignerByGuidCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAssignmentsByDesignerGuid", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAssignmentsByDesignerGuid(string designerGuid)
	{
		return (DataTable)Invoke("GetAssignmentsByDesignerGuid", new object[1] { designerGuid })[0];
	}

	public IAsyncResult BeginGetAssignmentsByDesignerGuid(string designerGuid, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAssignmentsByDesignerGuid", new object[1] { designerGuid }, callback, asyncState);
	}

	public DataTable EndGetAssignmentsByDesignerGuid(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAssignmentsByDesignerGuidAsync(string designerGuid)
	{
		GetAssignmentsByDesignerGuidAsync(designerGuid, null);
	}

	public void GetAssignmentsByDesignerGuidAsync(string designerGuid, object userState)
	{
		if (GetAssignmentsByDesignerGuidOperationCompleted == null)
		{
			GetAssignmentsByDesignerGuidOperationCompleted = OnGetAssignmentsByDesignerGuidOperationCompleted;
		}
		InvokeAsync("GetAssignmentsByDesignerGuid", new object[1] { designerGuid }, GetAssignmentsByDesignerGuidOperationCompleted, userState);
	}

	private void OnGetAssignmentsByDesignerGuidOperationCompleted(object arg)
	{
		if (this.GetAssignmentsByDesignerGuidCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAssignmentsByDesignerGuidCompleted(this, new GetAssignmentsByDesignerGuidCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignTypes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignTypes()
	{
		return (DataTable)Invoke("GetAllDesignTypes", new object[0])[0];
	}

	public IAsyncResult BeginGetAllDesignTypes(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignTypes", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllDesignTypes(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignTypesAsync()
	{
		GetAllDesignTypesAsync(null);
	}

	public void GetAllDesignTypesAsync(object userState)
	{
		if (GetAllDesignTypesOperationCompleted == null)
		{
			GetAllDesignTypesOperationCompleted = OnGetAllDesignTypesOperationCompleted;
		}
		InvokeAsync("GetAllDesignTypes", new object[0], GetAllDesignTypesOperationCompleted, userState);
	}

	private void OnGetAllDesignTypesOperationCompleted(object arg)
	{
		if (this.GetAllDesignTypesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignTypesCompleted(this, new GetAllDesignTypesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAssignmentNumberByAssignmentId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetAssignmentNumberByAssignmentId(int assignmentId)
	{
		return (string)Invoke("GetAssignmentNumberByAssignmentId", new object[1] { assignmentId })[0];
	}

	public IAsyncResult BeginGetAssignmentNumberByAssignmentId(int assignmentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAssignmentNumberByAssignmentId", new object[1] { assignmentId }, callback, asyncState);
	}

	public string EndGetAssignmentNumberByAssignmentId(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetAssignmentNumberByAssignmentIdAsync(int assignmentId)
	{
		GetAssignmentNumberByAssignmentIdAsync(assignmentId, null);
	}

	public void GetAssignmentNumberByAssignmentIdAsync(int assignmentId, object userState)
	{
		if (GetAssignmentNumberByAssignmentIdOperationCompleted == null)
		{
			GetAssignmentNumberByAssignmentIdOperationCompleted = OnGetAssignmentNumberByAssignmentIdOperationCompleted;
		}
		InvokeAsync("GetAssignmentNumberByAssignmentId", new object[1] { assignmentId }, GetAssignmentNumberByAssignmentIdOperationCompleted, userState);
	}

	private void OnGetAssignmentNumberByAssignmentIdOperationCompleted(object arg)
	{
		if (this.GetAssignmentNumberByAssignmentIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAssignmentNumberByAssignmentIdCompleted(this, new GetAssignmentNumberByAssignmentIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/HasAssignmentDocumentByAssignmentId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool HasAssignmentDocumentByAssignmentId(int assignmentId)
	{
		return (bool)Invoke("HasAssignmentDocumentByAssignmentId", new object[1] { assignmentId })[0];
	}

	public IAsyncResult BeginHasAssignmentDocumentByAssignmentId(int assignmentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("HasAssignmentDocumentByAssignmentId", new object[1] { assignmentId }, callback, asyncState);
	}

	public bool EndHasAssignmentDocumentByAssignmentId(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void HasAssignmentDocumentByAssignmentIdAsync(int assignmentId)
	{
		HasAssignmentDocumentByAssignmentIdAsync(assignmentId, null);
	}

	public void HasAssignmentDocumentByAssignmentIdAsync(int assignmentId, object userState)
	{
		if (HasAssignmentDocumentByAssignmentIdOperationCompleted == null)
		{
			HasAssignmentDocumentByAssignmentIdOperationCompleted = OnHasAssignmentDocumentByAssignmentIdOperationCompleted;
		}
		InvokeAsync("HasAssignmentDocumentByAssignmentId", new object[1] { assignmentId }, HasAssignmentDocumentByAssignmentIdOperationCompleted, userState);
	}

	private void OnHasAssignmentDocumentByAssignmentIdOperationCompleted(object arg)
	{
		if (this.HasAssignmentDocumentByAssignmentIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.HasAssignmentDocumentByAssignmentIdCompleted(this, new HasAssignmentDocumentByAssignmentIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/FindADesignWithGuid", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int FindADesignWithGuid(int AssignmentId, Guid KeyGuid, int ModelNumber, int DesignTypeId)
	{
		return (int)Invoke("FindADesignWithGuid", new object[4] { AssignmentId, KeyGuid, ModelNumber, DesignTypeId })[0];
	}

	public IAsyncResult BeginFindADesignWithGuid(int AssignmentId, Guid KeyGuid, int ModelNumber, int DesignTypeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("FindADesignWithGuid", new object[4] { AssignmentId, KeyGuid, ModelNumber, DesignTypeId }, callback, asyncState);
	}

	public int EndFindADesignWithGuid(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void FindADesignWithGuidAsync(int AssignmentId, Guid KeyGuid, int ModelNumber, int DesignTypeId)
	{
		FindADesignWithGuidAsync(AssignmentId, KeyGuid, ModelNumber, DesignTypeId, null);
	}

	public void FindADesignWithGuidAsync(int AssignmentId, Guid KeyGuid, int ModelNumber, int DesignTypeId, object userState)
	{
		if (FindADesignWithGuidOperationCompleted == null)
		{
			FindADesignWithGuidOperationCompleted = OnFindADesignWithGuidOperationCompleted;
		}
		InvokeAsync("FindADesignWithGuid", new object[4] { AssignmentId, KeyGuid, ModelNumber, DesignTypeId }, FindADesignWithGuidOperationCompleted, userState);
	}

	private void OnFindADesignWithGuidOperationCompleted(object arg)
	{
		if (this.FindADesignWithGuidCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.FindADesignWithGuidCompleted(this, new FindADesignWithGuidCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UploadFile", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string UploadFile([XmlElement(DataType = "base64Binary")] byte[] f, Guid KeyGuid, int AssignmentId, int ModelNumber, int DesignTypeId, string Comments)
	{
		return (string)Invoke("UploadFile", new object[6] { f, KeyGuid, AssignmentId, ModelNumber, DesignTypeId, Comments })[0];
	}

	public IAsyncResult BeginUploadFile(byte[] f, Guid KeyGuid, int AssignmentId, int ModelNumber, int DesignTypeId, string Comments, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UploadFile", new object[6] { f, KeyGuid, AssignmentId, ModelNumber, DesignTypeId, Comments }, callback, asyncState);
	}

	public string EndUploadFile(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void UploadFileAsync(byte[] f, Guid KeyGuid, int AssignmentId, int ModelNumber, int DesignTypeId, string Comments)
	{
		UploadFileAsync(f, KeyGuid, AssignmentId, ModelNumber, DesignTypeId, Comments, null);
	}

	public void UploadFileAsync(byte[] f, Guid KeyGuid, int AssignmentId, int ModelNumber, int DesignTypeId, string Comments, object userState)
	{
		if (UploadFileOperationCompleted == null)
		{
			UploadFileOperationCompleted = OnUploadFileOperationCompleted;
		}
		InvokeAsync("UploadFile", new object[6] { f, KeyGuid, AssignmentId, ModelNumber, DesignTypeId, Comments }, UploadFileOperationCompleted, userState);
	}

	private void OnUploadFileOperationCompleted(object arg)
	{
		if (this.UploadFileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UploadFileCompleted(this, new UploadFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/ReviewerUploadFileTemp", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string ReviewerUploadFileTemp([XmlElement(DataType = "base64Binary")] byte[] byteArray, int DesignId, string Comments, string UserName)
	{
		return (string)Invoke("ReviewerUploadFileTemp", new object[4] { byteArray, DesignId, Comments, UserName })[0];
	}

	public IAsyncResult BeginReviewerUploadFileTemp(byte[] byteArray, int DesignId, string Comments, string UserName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("ReviewerUploadFileTemp", new object[4] { byteArray, DesignId, Comments, UserName }, callback, asyncState);
	}

	public string EndReviewerUploadFileTemp(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void ReviewerUploadFileTempAsync(byte[] byteArray, int DesignId, string Comments, string UserName)
	{
		ReviewerUploadFileTempAsync(byteArray, DesignId, Comments, UserName, null);
	}

	public void ReviewerUploadFileTempAsync(byte[] byteArray, int DesignId, string Comments, string UserName, object userState)
	{
		if (ReviewerUploadFileTempOperationCompleted == null)
		{
			ReviewerUploadFileTempOperationCompleted = OnReviewerUploadFileTempOperationCompleted;
		}
		InvokeAsync("ReviewerUploadFileTemp", new object[4] { byteArray, DesignId, Comments, UserName }, ReviewerUploadFileTempOperationCompleted, userState);
	}

	private void OnReviewerUploadFileTempOperationCompleted(object arg)
	{
		if (this.ReviewerUploadFileTempCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.ReviewerUploadFileTempCompleted(this, new ReviewerUploadFileTempCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignsByDesignerGuid", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignsByDesignerGuid(Guid KeyGuid)
	{
		return (DataTable)Invoke("GetDesignsByDesignerGuid", new object[1] { KeyGuid })[0];
	}

	public IAsyncResult BeginGetDesignsByDesignerGuid(Guid KeyGuid, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignsByDesignerGuid", new object[1] { KeyGuid }, callback, asyncState);
	}

	public DataTable EndGetDesignsByDesignerGuid(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignsByDesignerGuidAsync(Guid KeyGuid)
	{
		GetDesignsByDesignerGuidAsync(KeyGuid, null);
	}

	public void GetDesignsByDesignerGuidAsync(Guid KeyGuid, object userState)
	{
		if (GetDesignsByDesignerGuidOperationCompleted == null)
		{
			GetDesignsByDesignerGuidOperationCompleted = OnGetDesignsByDesignerGuidOperationCompleted;
		}
		InvokeAsync("GetDesignsByDesignerGuid", new object[1] { KeyGuid }, GetDesignsByDesignerGuidOperationCompleted, userState);
	}

	private void OnGetDesignsByDesignerGuidOperationCompleted(object arg)
	{
		if (this.GetDesignsByDesignerGuidCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignsByDesignerGuidCompleted(this, new GetDesignsByDesignerGuidCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignerByUsername", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignerByUsername(string username)
	{
		return (DataTable)Invoke("GetDesignerByUsername", new object[1] { username })[0];
	}

	public IAsyncResult BeginGetDesignerByUsername(string username, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignerByUsername", new object[1] { username }, callback, asyncState);
	}

	public DataTable EndGetDesignerByUsername(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignerByUsernameAsync(string username)
	{
		GetDesignerByUsernameAsync(username, null);
	}

	public void GetDesignerByUsernameAsync(string username, object userState)
	{
		if (GetDesignerByUsernameOperationCompleted == null)
		{
			GetDesignerByUsernameOperationCompleted = OnGetDesignerByUsernameOperationCompleted;
		}
		InvokeAsync("GetDesignerByUsername", new object[1] { username }, GetDesignerByUsernameOperationCompleted, userState);
	}

	private void OnGetDesignerByUsernameOperationCompleted(object arg)
	{
		if (this.GetDesignerByUsernameCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignerByUsernameCompleted(this, new GetDesignerByUsernameCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DesignVersionAddAttribute", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_DesignVersionAddAttribute(int designVersionId, int attributeId, string attributeValue)
	{
		Invoke("CSG_DesignVersionAddAttribute", new object[3] { designVersionId, attributeId, attributeValue });
	}

	public IAsyncResult BeginCSG_DesignVersionAddAttribute(int designVersionId, int attributeId, string attributeValue, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DesignVersionAddAttribute", new object[3] { designVersionId, attributeId, attributeValue }, callback, asyncState);
	}

	public void EndCSG_DesignVersionAddAttribute(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_DesignVersionAddAttributeAsync(int designVersionId, int attributeId, string attributeValue)
	{
		CSG_DesignVersionAddAttributeAsync(designVersionId, attributeId, attributeValue, null);
	}

	public void CSG_DesignVersionAddAttributeAsync(int designVersionId, int attributeId, string attributeValue, object userState)
	{
		if (CSG_DesignVersionAddAttributeOperationCompleted == null)
		{
			CSG_DesignVersionAddAttributeOperationCompleted = OnCSG_DesignVersionAddAttributeOperationCompleted;
		}
		InvokeAsync("CSG_DesignVersionAddAttribute", new object[3] { designVersionId, attributeId, attributeValue }, CSG_DesignVersionAddAttributeOperationCompleted, userState);
	}

	private void OnCSG_DesignVersionAddAttributeOperationCompleted(object arg)
	{
		if (this.CSG_DesignVersionAddAttributeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DesignVersionAddAttributeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DesignVersionUpdateAttribute", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_DesignVersionUpdateAttribute(int designVersionId, int attributeId, string attributeValue)
	{
		Invoke("CSG_DesignVersionUpdateAttribute", new object[3] { designVersionId, attributeId, attributeValue });
	}

	public IAsyncResult BeginCSG_DesignVersionUpdateAttribute(int designVersionId, int attributeId, string attributeValue, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DesignVersionUpdateAttribute", new object[3] { designVersionId, attributeId, attributeValue }, callback, asyncState);
	}

	public void EndCSG_DesignVersionUpdateAttribute(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_DesignVersionUpdateAttributeAsync(int designVersionId, int attributeId, string attributeValue)
	{
		CSG_DesignVersionUpdateAttributeAsync(designVersionId, attributeId, attributeValue, null);
	}

	public void CSG_DesignVersionUpdateAttributeAsync(int designVersionId, int attributeId, string attributeValue, object userState)
	{
		if (CSG_DesignVersionUpdateAttributeOperationCompleted == null)
		{
			CSG_DesignVersionUpdateAttributeOperationCompleted = OnCSG_DesignVersionUpdateAttributeOperationCompleted;
		}
		InvokeAsync("CSG_DesignVersionUpdateAttribute", new object[3] { designVersionId, attributeId, attributeValue }, CSG_DesignVersionUpdateAttributeOperationCompleted, userState);
	}

	private void OnCSG_DesignVersionUpdateAttributeOperationCompleted(object arg)
	{
		if (this.CSG_DesignVersionUpdateAttributeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DesignVersionUpdateAttributeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DesignVersionDeleteAttribute", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_DesignVersionDeleteAttribute(int designVersionId, int attributeId)
	{
		Invoke("CSG_DesignVersionDeleteAttribute", new object[2] { designVersionId, attributeId });
	}

	public IAsyncResult BeginCSG_DesignVersionDeleteAttribute(int designVersionId, int attributeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DesignVersionDeleteAttribute", new object[2] { designVersionId, attributeId }, callback, asyncState);
	}

	public void EndCSG_DesignVersionDeleteAttribute(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_DesignVersionDeleteAttributeAsync(int designVersionId, int attributeId)
	{
		CSG_DesignVersionDeleteAttributeAsync(designVersionId, attributeId, null);
	}

	public void CSG_DesignVersionDeleteAttributeAsync(int designVersionId, int attributeId, object userState)
	{
		if (CSG_DesignVersionDeleteAttributeOperationCompleted == null)
		{
			CSG_DesignVersionDeleteAttributeOperationCompleted = OnCSG_DesignVersionDeleteAttributeOperationCompleted;
		}
		InvokeAsync("CSG_DesignVersionDeleteAttribute", new object[2] { designVersionId, attributeId }, CSG_DesignVersionDeleteAttributeOperationCompleted, userState);
	}

	private void OnCSG_DesignVersionDeleteAttributeOperationCompleted(object arg)
	{
		if (this.CSG_DesignVersionDeleteAttributeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DesignVersionDeleteAttributeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DesignVersionComponentAddAttribute", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_DesignVersionComponentAddAttribute(int designVersionComponentId, int attributeId, string attributeValue)
	{
		Invoke("CSG_DesignVersionComponentAddAttribute", new object[3] { designVersionComponentId, attributeId, attributeValue });
	}

	public IAsyncResult BeginCSG_DesignVersionComponentAddAttribute(int designVersionComponentId, int attributeId, string attributeValue, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DesignVersionComponentAddAttribute", new object[3] { designVersionComponentId, attributeId, attributeValue }, callback, asyncState);
	}

	public void EndCSG_DesignVersionComponentAddAttribute(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_DesignVersionComponentAddAttributeAsync(int designVersionComponentId, int attributeId, string attributeValue)
	{
		CSG_DesignVersionComponentAddAttributeAsync(designVersionComponentId, attributeId, attributeValue, null);
	}

	public void CSG_DesignVersionComponentAddAttributeAsync(int designVersionComponentId, int attributeId, string attributeValue, object userState)
	{
		if (CSG_DesignVersionComponentAddAttributeOperationCompleted == null)
		{
			CSG_DesignVersionComponentAddAttributeOperationCompleted = OnCSG_DesignVersionComponentAddAttributeOperationCompleted;
		}
		InvokeAsync("CSG_DesignVersionComponentAddAttribute", new object[3] { designVersionComponentId, attributeId, attributeValue }, CSG_DesignVersionComponentAddAttributeOperationCompleted, userState);
	}

	private void OnCSG_DesignVersionComponentAddAttributeOperationCompleted(object arg)
	{
		if (this.CSG_DesignVersionComponentAddAttributeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DesignVersionComponentAddAttributeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DesignVersionComponentUpdateAttribute", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_DesignVersionComponentUpdateAttribute(int designVersionComponentId, int attributeId, string attributeValue)
	{
		Invoke("CSG_DesignVersionComponentUpdateAttribute", new object[3] { designVersionComponentId, attributeId, attributeValue });
	}

	public IAsyncResult BeginCSG_DesignVersionComponentUpdateAttribute(int designVersionComponentId, int attributeId, string attributeValue, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DesignVersionComponentUpdateAttribute", new object[3] { designVersionComponentId, attributeId, attributeValue }, callback, asyncState);
	}

	public void EndCSG_DesignVersionComponentUpdateAttribute(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_DesignVersionComponentUpdateAttributeAsync(int designVersionComponentId, int attributeId, string attributeValue)
	{
		CSG_DesignVersionComponentUpdateAttributeAsync(designVersionComponentId, attributeId, attributeValue, null);
	}

	public void CSG_DesignVersionComponentUpdateAttributeAsync(int designVersionComponentId, int attributeId, string attributeValue, object userState)
	{
		if (CSG_DesignVersionComponentUpdateAttributeOperationCompleted == null)
		{
			CSG_DesignVersionComponentUpdateAttributeOperationCompleted = OnCSG_DesignVersionComponentUpdateAttributeOperationCompleted;
		}
		InvokeAsync("CSG_DesignVersionComponentUpdateAttribute", new object[3] { designVersionComponentId, attributeId, attributeValue }, CSG_DesignVersionComponentUpdateAttributeOperationCompleted, userState);
	}

	private void OnCSG_DesignVersionComponentUpdateAttributeOperationCompleted(object arg)
	{
		if (this.CSG_DesignVersionComponentUpdateAttributeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DesignVersionComponentUpdateAttributeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DesignVersionComponentDeleteAttribute", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_DesignVersionComponentDeleteAttribute(int designVersionComponentId, int attributeId)
	{
		Invoke("CSG_DesignVersionComponentDeleteAttribute", new object[2] { designVersionComponentId, attributeId });
	}

	public IAsyncResult BeginCSG_DesignVersionComponentDeleteAttribute(int designVersionComponentId, int attributeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DesignVersionComponentDeleteAttribute", new object[2] { designVersionComponentId, attributeId }, callback, asyncState);
	}

	public void EndCSG_DesignVersionComponentDeleteAttribute(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_DesignVersionComponentDeleteAttributeAsync(int designVersionComponentId, int attributeId)
	{
		CSG_DesignVersionComponentDeleteAttributeAsync(designVersionComponentId, attributeId, null);
	}

	public void CSG_DesignVersionComponentDeleteAttributeAsync(int designVersionComponentId, int attributeId, object userState)
	{
		if (CSG_DesignVersionComponentDeleteAttributeOperationCompleted == null)
		{
			CSG_DesignVersionComponentDeleteAttributeOperationCompleted = OnCSG_DesignVersionComponentDeleteAttributeOperationCompleted;
		}
		InvokeAsync("CSG_DesignVersionComponentDeleteAttribute", new object[2] { designVersionComponentId, attributeId }, CSG_DesignVersionComponentDeleteAttributeOperationCompleted, userState);
	}

	private void OnCSG_DesignVersionComponentDeleteAttributeOperationCompleted(object arg)
	{
		if (this.CSG_DesignVersionComponentDeleteAttributeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DesignVersionComponentDeleteAttributeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_AddAttributeToDesignComponentNoRepeats", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_AddAttributeToDesignComponentNoRepeats(int designVersionId, int componentId, int attributeId, string attributeValue)
	{
		Invoke("CSG_AddAttributeToDesignComponentNoRepeats", new object[4] { designVersionId, componentId, attributeId, attributeValue });
	}

	public IAsyncResult BeginCSG_AddAttributeToDesignComponentNoRepeats(int designVersionId, int componentId, int attributeId, string attributeValue, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_AddAttributeToDesignComponentNoRepeats", new object[4] { designVersionId, componentId, attributeId, attributeValue }, callback, asyncState);
	}

	public void EndCSG_AddAttributeToDesignComponentNoRepeats(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_AddAttributeToDesignComponentNoRepeatsAsync(int designVersionId, int componentId, int attributeId, string attributeValue)
	{
		CSG_AddAttributeToDesignComponentNoRepeatsAsync(designVersionId, componentId, attributeId, attributeValue, null);
	}

	public void CSG_AddAttributeToDesignComponentNoRepeatsAsync(int designVersionId, int componentId, int attributeId, string attributeValue, object userState)
	{
		if (CSG_AddAttributeToDesignComponentNoRepeatsOperationCompleted == null)
		{
			CSG_AddAttributeToDesignComponentNoRepeatsOperationCompleted = OnCSG_AddAttributeToDesignComponentNoRepeatsOperationCompleted;
		}
		InvokeAsync("CSG_AddAttributeToDesignComponentNoRepeats", new object[4] { designVersionId, componentId, attributeId, attributeValue }, CSG_AddAttributeToDesignComponentNoRepeatsOperationCompleted, userState);
	}

	private void OnCSG_AddAttributeToDesignComponentNoRepeatsOperationCompleted(object arg)
	{
		if (this.CSG_AddAttributeToDesignComponentNoRepeatsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_AddAttributeToDesignComponentNoRepeatsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_RingRailSetDesign", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_RingRailSetDesign(int designVersionId, double RingSize, double RingMin, double RingMax)
	{
		Invoke("CSG_RingRailSetDesign", new object[4] { designVersionId, RingSize, RingMin, RingMax });
	}

	public IAsyncResult BeginCSG_RingRailSetDesign(int designVersionId, double RingSize, double RingMin, double RingMax, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_RingRailSetDesign", new object[4] { designVersionId, RingSize, RingMin, RingMax }, callback, asyncState);
	}

	public void EndCSG_RingRailSetDesign(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_RingRailSetDesignAsync(int designVersionId, double RingSize, double RingMin, double RingMax)
	{
		CSG_RingRailSetDesignAsync(designVersionId, RingSize, RingMin, RingMax, null);
	}

	public void CSG_RingRailSetDesignAsync(int designVersionId, double RingSize, double RingMin, double RingMax, object userState)
	{
		if (CSG_RingRailSetDesignOperationCompleted == null)
		{
			CSG_RingRailSetDesignOperationCompleted = OnCSG_RingRailSetDesignOperationCompleted;
		}
		InvokeAsync("CSG_RingRailSetDesign", new object[4] { designVersionId, RingSize, RingMin, RingMax }, CSG_RingRailSetDesignOperationCompleted, userState);
	}

	private void OnCSG_RingRailSetDesignOperationCompleted(object arg)
	{
		if (this.CSG_RingRailSetDesignCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_RingRailSetDesignCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_RingRailDeleteDesign", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_RingRailDeleteDesign(int designVersionId)
	{
		Invoke("CSG_RingRailDeleteDesign", new object[1] { designVersionId });
	}

	public IAsyncResult BeginCSG_RingRailDeleteDesign(int designVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_RingRailDeleteDesign", new object[1] { designVersionId }, callback, asyncState);
	}

	public void EndCSG_RingRailDeleteDesign(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_RingRailDeleteDesignAsync(int designVersionId)
	{
		CSG_RingRailDeleteDesignAsync(designVersionId, null);
	}

	public void CSG_RingRailDeleteDesignAsync(int designVersionId, object userState)
	{
		if (CSG_RingRailDeleteDesignOperationCompleted == null)
		{
			CSG_RingRailDeleteDesignOperationCompleted = OnCSG_RingRailDeleteDesignOperationCompleted;
		}
		InvokeAsync("CSG_RingRailDeleteDesign", new object[1] { designVersionId }, CSG_RingRailDeleteDesignOperationCompleted, userState);
	}

	private void OnCSG_RingRailDeleteDesignOperationCompleted(object arg)
	{
		if (this.CSG_RingRailDeleteDesignCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_RingRailDeleteDesignCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_GetDesignIdFromDesignVersionID", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CSG_GetDesignIdFromDesignVersionID(int designVersionId)
	{
		return (int)Invoke("CSG_GetDesignIdFromDesignVersionID", new object[1] { designVersionId })[0];
	}

	public IAsyncResult BeginCSG_GetDesignIdFromDesignVersionID(int designVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_GetDesignIdFromDesignVersionID", new object[1] { designVersionId }, callback, asyncState);
	}

	public int EndCSG_GetDesignIdFromDesignVersionID(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CSG_GetDesignIdFromDesignVersionIDAsync(int designVersionId)
	{
		CSG_GetDesignIdFromDesignVersionIDAsync(designVersionId, null);
	}

	public void CSG_GetDesignIdFromDesignVersionIDAsync(int designVersionId, object userState)
	{
		if (CSG_GetDesignIdFromDesignVersionIDOperationCompleted == null)
		{
			CSG_GetDesignIdFromDesignVersionIDOperationCompleted = OnCSG_GetDesignIdFromDesignVersionIDOperationCompleted;
		}
		InvokeAsync("CSG_GetDesignIdFromDesignVersionID", new object[1] { designVersionId }, CSG_GetDesignIdFromDesignVersionIDOperationCompleted, userState);
	}

	private void OnCSG_GetDesignIdFromDesignVersionIDOperationCompleted(object arg)
	{
		if (this.CSG_GetDesignIdFromDesignVersionIDCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_GetDesignIdFromDesignVersionIDCompleted(this, new CSG_GetDesignIdFromDesignVersionIDCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_GetVersionFromDesignVersionID", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CSG_GetVersionFromDesignVersionID(int designVersionId)
	{
		return (int)Invoke("CSG_GetVersionFromDesignVersionID", new object[1] { designVersionId })[0];
	}

	public IAsyncResult BeginCSG_GetVersionFromDesignVersionID(int designVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_GetVersionFromDesignVersionID", new object[1] { designVersionId }, callback, asyncState);
	}

	public int EndCSG_GetVersionFromDesignVersionID(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CSG_GetVersionFromDesignVersionIDAsync(int designVersionId)
	{
		CSG_GetVersionFromDesignVersionIDAsync(designVersionId, null);
	}

	public void CSG_GetVersionFromDesignVersionIDAsync(int designVersionId, object userState)
	{
		if (CSG_GetVersionFromDesignVersionIDOperationCompleted == null)
		{
			CSG_GetVersionFromDesignVersionIDOperationCompleted = OnCSG_GetVersionFromDesignVersionIDOperationCompleted;
		}
		InvokeAsync("CSG_GetVersionFromDesignVersionID", new object[1] { designVersionId }, CSG_GetVersionFromDesignVersionIDOperationCompleted, userState);
	}

	private void OnCSG_GetVersionFromDesignVersionIDOperationCompleted(object arg)
	{
		if (this.CSG_GetVersionFromDesignVersionIDCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_GetVersionFromDesignVersionIDCompleted(this, new CSG_GetVersionFromDesignVersionIDCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_GetMaxVersionFromDesignId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CSG_GetMaxVersionFromDesignId(int designId)
	{
		return (int)Invoke("CSG_GetMaxVersionFromDesignId", new object[1] { designId })[0];
	}

	public IAsyncResult BeginCSG_GetMaxVersionFromDesignId(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_GetMaxVersionFromDesignId", new object[1] { designId }, callback, asyncState);
	}

	public int EndCSG_GetMaxVersionFromDesignId(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CSG_GetMaxVersionFromDesignIdAsync(int designId)
	{
		CSG_GetMaxVersionFromDesignIdAsync(designId, null);
	}

	public void CSG_GetMaxVersionFromDesignIdAsync(int designId, object userState)
	{
		if (CSG_GetMaxVersionFromDesignIdOperationCompleted == null)
		{
			CSG_GetMaxVersionFromDesignIdOperationCompleted = OnCSG_GetMaxVersionFromDesignIdOperationCompleted;
		}
		InvokeAsync("CSG_GetMaxVersionFromDesignId", new object[1] { designId }, CSG_GetMaxVersionFromDesignIdOperationCompleted, userState);
	}

	private void OnCSG_GetMaxVersionFromDesignIdOperationCompleted(object arg)
	{
		if (this.CSG_GetMaxVersionFromDesignIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_GetMaxVersionFromDesignIdCompleted(this, new CSG_GetMaxVersionFromDesignIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DownloadOriginalFile", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] CSG_DownloadOriginalFile(int designVersionId)
	{
		return (byte[])Invoke("CSG_DownloadOriginalFile", new object[1] { designVersionId })[0];
	}

	public IAsyncResult BeginCSG_DownloadOriginalFile(int designVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DownloadOriginalFile", new object[1] { designVersionId }, callback, asyncState);
	}

	public byte[] EndCSG_DownloadOriginalFile(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void CSG_DownloadOriginalFileAsync(int designVersionId)
	{
		CSG_DownloadOriginalFileAsync(designVersionId, null);
	}

	public void CSG_DownloadOriginalFileAsync(int designVersionId, object userState)
	{
		if (CSG_DownloadOriginalFileOperationCompleted == null)
		{
			CSG_DownloadOriginalFileOperationCompleted = OnCSG_DownloadOriginalFileOperationCompleted;
		}
		InvokeAsync("CSG_DownloadOriginalFile", new object[1] { designVersionId }, CSG_DownloadOriginalFileOperationCompleted, userState);
	}

	private void OnCSG_DownloadOriginalFileOperationCompleted(object arg)
	{
		if (this.CSG_DownloadOriginalFileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DownloadOriginalFileCompleted(this, new CSG_DownloadOriginalFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DownloadCleanFile", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] CSG_DownloadCleanFile(int designVersionId)
	{
		return (byte[])Invoke("CSG_DownloadCleanFile", new object[1] { designVersionId })[0];
	}

	public IAsyncResult BeginCSG_DownloadCleanFile(int designVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DownloadCleanFile", new object[1] { designVersionId }, callback, asyncState);
	}

	public byte[] EndCSG_DownloadCleanFile(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void CSG_DownloadCleanFileAsync(int designVersionId)
	{
		CSG_DownloadCleanFileAsync(designVersionId, null);
	}

	public void CSG_DownloadCleanFileAsync(int designVersionId, object userState)
	{
		if (CSG_DownloadCleanFileOperationCompleted == null)
		{
			CSG_DownloadCleanFileOperationCompleted = OnCSG_DownloadCleanFileOperationCompleted;
		}
		InvokeAsync("CSG_DownloadCleanFile", new object[1] { designVersionId }, CSG_DownloadCleanFileOperationCompleted, userState);
	}

	private void OnCSG_DownloadCleanFileOperationCompleted(object arg)
	{
		if (this.CSG_DownloadCleanFileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DownloadCleanFileCompleted(this, new CSG_DownloadCleanFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_UploadCleanedFile", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_UploadCleanedFile(int DesignVersionId, [XmlElement(DataType = "base64Binary")] byte[] f, int blockLength, string identSHA)
	{
		Invoke("CSG_UploadCleanedFile", new object[4] { DesignVersionId, f, blockLength, identSHA });
	}

	public IAsyncResult BeginCSG_UploadCleanedFile(int DesignVersionId, byte[] f, int blockLength, string identSHA, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_UploadCleanedFile", new object[4] { DesignVersionId, f, blockLength, identSHA }, callback, asyncState);
	}

	public void EndCSG_UploadCleanedFile(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_UploadCleanedFileAsync(int DesignVersionId, byte[] f, int blockLength, string identSHA)
	{
		CSG_UploadCleanedFileAsync(DesignVersionId, f, blockLength, identSHA, null);
	}

	public void CSG_UploadCleanedFileAsync(int DesignVersionId, byte[] f, int blockLength, string identSHA, object userState)
	{
		if (CSG_UploadCleanedFileOperationCompleted == null)
		{
			CSG_UploadCleanedFileOperationCompleted = OnCSG_UploadCleanedFileOperationCompleted;
		}
		InvokeAsync("CSG_UploadCleanedFile", new object[4] { DesignVersionId, f, blockLength, identSHA }, CSG_UploadCleanedFileOperationCompleted, userState);
	}

	private void OnCSG_UploadCleanedFileOperationCompleted(object arg)
	{
		if (this.CSG_UploadCleanedFileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_UploadCleanedFileCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_GetDesignAttributeValueByDesignIdAndAttributeId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string CSG_GetDesignAttributeValueByDesignIdAndAttributeId(int designId, int attributeId)
	{
		return (string)Invoke("CSG_GetDesignAttributeValueByDesignIdAndAttributeId", new object[2] { designId, attributeId })[0];
	}

	public IAsyncResult BeginCSG_GetDesignAttributeValueByDesignIdAndAttributeId(int designId, int attributeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_GetDesignAttributeValueByDesignIdAndAttributeId", new object[2] { designId, attributeId }, callback, asyncState);
	}

	public string EndCSG_GetDesignAttributeValueByDesignIdAndAttributeId(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void CSG_GetDesignAttributeValueByDesignIdAndAttributeIdAsync(int designId, int attributeId)
	{
		CSG_GetDesignAttributeValueByDesignIdAndAttributeIdAsync(designId, attributeId, null);
	}

	public void CSG_GetDesignAttributeValueByDesignIdAndAttributeIdAsync(int designId, int attributeId, object userState)
	{
		if (CSG_GetDesignAttributeValueByDesignIdAndAttributeIdOperationCompleted == null)
		{
			CSG_GetDesignAttributeValueByDesignIdAndAttributeIdOperationCompleted = OnCSG_GetDesignAttributeValueByDesignIdAndAttributeIdOperationCompleted;
		}
		InvokeAsync("CSG_GetDesignAttributeValueByDesignIdAndAttributeId", new object[2] { designId, attributeId }, CSG_GetDesignAttributeValueByDesignIdAndAttributeIdOperationCompleted, userState);
	}

	private void OnCSG_GetDesignAttributeValueByDesignIdAndAttributeIdOperationCompleted(object arg)
	{
		if (this.CSG_GetDesignAttributeValueByDesignIdAndAttributeIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_GetDesignAttributeValueByDesignIdAndAttributeIdCompleted(this, new CSG_GetDesignAttributeValueByDesignIdAndAttributeIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_SetComponentList", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_SetComponentList(int DesignVersionId, int[] compList)
	{
		Invoke("CSG_SetComponentList", new object[2] { DesignVersionId, compList });
	}

	public IAsyncResult BeginCSG_SetComponentList(int DesignVersionId, int[] compList, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_SetComponentList", new object[2] { DesignVersionId, compList }, callback, asyncState);
	}

	public void EndCSG_SetComponentList(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_SetComponentListAsync(int DesignVersionId, int[] compList)
	{
		CSG_SetComponentListAsync(DesignVersionId, compList, null);
	}

	public void CSG_SetComponentListAsync(int DesignVersionId, int[] compList, object userState)
	{
		if (CSG_SetComponentListOperationCompleted == null)
		{
			CSG_SetComponentListOperationCompleted = OnCSG_SetComponentListOperationCompleted;
		}
		InvokeAsync("CSG_SetComponentList", new object[2] { DesignVersionId, compList }, CSG_SetComponentListOperationCompleted, userState);
	}

	private void OnCSG_SetComponentListOperationCompleted(object arg)
	{
		if (this.CSG_SetComponentListCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_SetComponentListCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DeleteGemsFromDesignVersion", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CSG_DeleteGemsFromDesignVersion(int DesignVersionId)
	{
		return (int)Invoke("CSG_DeleteGemsFromDesignVersion", new object[1] { DesignVersionId })[0];
	}

	public IAsyncResult BeginCSG_DeleteGemsFromDesignVersion(int DesignVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DeleteGemsFromDesignVersion", new object[1] { DesignVersionId }, callback, asyncState);
	}

	public int EndCSG_DeleteGemsFromDesignVersion(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CSG_DeleteGemsFromDesignVersionAsync(int DesignVersionId)
	{
		CSG_DeleteGemsFromDesignVersionAsync(DesignVersionId, null);
	}

	public void CSG_DeleteGemsFromDesignVersionAsync(int DesignVersionId, object userState)
	{
		if (CSG_DeleteGemsFromDesignVersionOperationCompleted == null)
		{
			CSG_DeleteGemsFromDesignVersionOperationCompleted = OnCSG_DeleteGemsFromDesignVersionOperationCompleted;
		}
		InvokeAsync("CSG_DeleteGemsFromDesignVersion", new object[1] { DesignVersionId }, CSG_DeleteGemsFromDesignVersionOperationCompleted, userState);
	}

	private void OnCSG_DeleteGemsFromDesignVersionOperationCompleted(object arg)
	{
		if (this.CSG_DeleteGemsFromDesignVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DeleteGemsFromDesignVersionCompleted(this, new CSG_DeleteGemsFromDesignVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_DeleteGemComponentDesignVersionId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_DeleteGemComponentDesignVersionId(int designVersionId)
	{
		Invoke("CSG_DeleteGemComponentDesignVersionId", new object[1] { designVersionId });
	}

	public IAsyncResult BeginCSG_DeleteGemComponentDesignVersionId(int designVersionId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_DeleteGemComponentDesignVersionId", new object[1] { designVersionId }, callback, asyncState);
	}

	public void EndCSG_DeleteGemComponentDesignVersionId(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_DeleteGemComponentDesignVersionIdAsync(int designVersionId)
	{
		CSG_DeleteGemComponentDesignVersionIdAsync(designVersionId, null);
	}

	public void CSG_DeleteGemComponentDesignVersionIdAsync(int designVersionId, object userState)
	{
		if (CSG_DeleteGemComponentDesignVersionIdOperationCompleted == null)
		{
			CSG_DeleteGemComponentDesignVersionIdOperationCompleted = OnCSG_DeleteGemComponentDesignVersionIdOperationCompleted;
		}
		InvokeAsync("CSG_DeleteGemComponentDesignVersionId", new object[1] { designVersionId }, CSG_DeleteGemComponentDesignVersionIdOperationCompleted, userState);
	}

	private void OnCSG_DeleteGemComponentDesignVersionIdOperationCompleted(object arg)
	{
		if (this.CSG_DeleteGemComponentDesignVersionIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_DeleteGemComponentDesignVersionIdCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_AddGemToDesign", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CSG_AddGemToDesign(int DesignVersionComponentId, string Name, string Shape, double GemX, double GemXMax, double GemXMin, double GemY, double GemYMax, double GemYMin, double GemZ, double FFactor, double GemW, double GemWMax, double GemWMin, string Mounting, int Flags, int GemPlacement, double MountingVolume)
	{
		return (int)Invoke("CSG_AddGemToDesign", new object[18]
		{
			DesignVersionComponentId, Name, Shape, GemX, GemXMax, GemXMin, GemY, GemYMax, GemYMin, GemZ,
			FFactor, GemW, GemWMax, GemWMin, Mounting, Flags, GemPlacement, MountingVolume
		})[0];
	}

	public IAsyncResult BeginCSG_AddGemToDesign(int DesignVersionComponentId, string Name, string Shape, double GemX, double GemXMax, double GemXMin, double GemY, double GemYMax, double GemYMin, double GemZ, double FFactor, double GemW, double GemWMax, double GemWMin, string Mounting, int Flags, int GemPlacement, double MountingVolume, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_AddGemToDesign", new object[18]
		{
			DesignVersionComponentId, Name, Shape, GemX, GemXMax, GemXMin, GemY, GemYMax, GemYMin, GemZ,
			FFactor, GemW, GemWMax, GemWMin, Mounting, Flags, GemPlacement, MountingVolume
		}, callback, asyncState);
	}

	public int EndCSG_AddGemToDesign(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CSG_AddGemToDesignAsync(int DesignVersionComponentId, string Name, string Shape, double GemX, double GemXMax, double GemXMin, double GemY, double GemYMax, double GemYMin, double GemZ, double FFactor, double GemW, double GemWMax, double GemWMin, string Mounting, int Flags, int GemPlacement, double MountingVolume)
	{
		CSG_AddGemToDesignAsync(DesignVersionComponentId, Name, Shape, GemX, GemXMax, GemXMin, GemY, GemYMax, GemYMin, GemZ, FFactor, GemW, GemWMax, GemWMin, Mounting, Flags, GemPlacement, MountingVolume, null);
	}

	public void CSG_AddGemToDesignAsync(int DesignVersionComponentId, string Name, string Shape, double GemX, double GemXMax, double GemXMin, double GemY, double GemYMax, double GemYMin, double GemZ, double FFactor, double GemW, double GemWMax, double GemWMin, string Mounting, int Flags, int GemPlacement, double MountingVolume, object userState)
	{
		if (CSG_AddGemToDesignOperationCompleted == null)
		{
			CSG_AddGemToDesignOperationCompleted = OnCSG_AddGemToDesignOperationCompleted;
		}
		InvokeAsync("CSG_AddGemToDesign", new object[18]
		{
			DesignVersionComponentId, Name, Shape, GemX, GemXMax, GemXMin, GemY, GemYMax, GemYMin, GemZ,
			FFactor, GemW, GemWMax, GemWMin, Mounting, Flags, GemPlacement, MountingVolume
		}, CSG_AddGemToDesignOperationCompleted, userState);
	}

	private void OnCSG_AddGemToDesignOperationCompleted(object arg)
	{
		if (this.CSG_AddGemToDesignCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_AddGemToDesignCompleted(this, new CSG_AddGemToDesignCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_SetNumberOfControls", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_SetNumberOfControls(int designVersionId, int Controls)
	{
		Invoke("CSG_SetNumberOfControls", new object[2] { designVersionId, Controls });
	}

	public IAsyncResult BeginCSG_SetNumberOfControls(int designVersionId, int Controls, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_SetNumberOfControls", new object[2] { designVersionId, Controls }, callback, asyncState);
	}

	public void EndCSG_SetNumberOfControls(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_SetNumberOfControlsAsync(int designVersionId, int Controls)
	{
		CSG_SetNumberOfControlsAsync(designVersionId, Controls, null);
	}

	public void CSG_SetNumberOfControlsAsync(int designVersionId, int Controls, object userState)
	{
		if (CSG_SetNumberOfControlsOperationCompleted == null)
		{
			CSG_SetNumberOfControlsOperationCompleted = OnCSG_SetNumberOfControlsOperationCompleted;
		}
		InvokeAsync("CSG_SetNumberOfControls", new object[2] { designVersionId, Controls }, CSG_SetNumberOfControlsOperationCompleted, userState);
	}

	private void OnCSG_SetNumberOfControlsOperationCompleted(object arg)
	{
		if (this.CSG_SetNumberOfControlsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_SetNumberOfControlsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_AddVolumeToComponent", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_AddVolumeToComponent(int designVersionId, int componentId, double Volume)
	{
		Invoke("CSG_AddVolumeToComponent", new object[3] { designVersionId, componentId, Volume });
	}

	public IAsyncResult BeginCSG_AddVolumeToComponent(int designVersionId, int componentId, double Volume, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_AddVolumeToComponent", new object[3] { designVersionId, componentId, Volume }, callback, asyncState);
	}

	public void EndCSG_AddVolumeToComponent(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_AddVolumeToComponentAsync(int designVersionId, int componentId, double Volume)
	{
		CSG_AddVolumeToComponentAsync(designVersionId, componentId, Volume, null);
	}

	public void CSG_AddVolumeToComponentAsync(int designVersionId, int componentId, double Volume, object userState)
	{
		if (CSG_AddVolumeToComponentOperationCompleted == null)
		{
			CSG_AddVolumeToComponentOperationCompleted = OnCSG_AddVolumeToComponentOperationCompleted;
		}
		InvokeAsync("CSG_AddVolumeToComponent", new object[3] { designVersionId, componentId, Volume }, CSG_AddVolumeToComponentOperationCompleted, userState);
	}

	private void OnCSG_AddVolumeToComponentOperationCompleted(object arg)
	{
		if (this.CSG_AddVolumeToComponentCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_AddVolumeToComponentCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_AddMetalCountToComponent", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_AddMetalCountToComponent(int designVersionId, int componentId, double Parts)
	{
		Invoke("CSG_AddMetalCountToComponent", new object[3] { designVersionId, componentId, Parts });
	}

	public IAsyncResult BeginCSG_AddMetalCountToComponent(int designVersionId, int componentId, double Parts, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_AddMetalCountToComponent", new object[3] { designVersionId, componentId, Parts }, callback, asyncState);
	}

	public void EndCSG_AddMetalCountToComponent(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_AddMetalCountToComponentAsync(int designVersionId, int componentId, double Parts)
	{
		CSG_AddMetalCountToComponentAsync(designVersionId, componentId, Parts, null);
	}

	public void CSG_AddMetalCountToComponentAsync(int designVersionId, int componentId, double Parts, object userState)
	{
		if (CSG_AddMetalCountToComponentOperationCompleted == null)
		{
			CSG_AddMetalCountToComponentOperationCompleted = OnCSG_AddMetalCountToComponentOperationCompleted;
		}
		InvokeAsync("CSG_AddMetalCountToComponent", new object[3] { designVersionId, componentId, Parts }, CSG_AddMetalCountToComponentOperationCompleted, userState);
	}

	private void OnCSG_AddMetalCountToComponentOperationCompleted(object arg)
	{
		if (this.CSG_AddMetalCountToComponentCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_AddMetalCountToComponentCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_SetBlocksUsed", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSG_SetBlocksUsed(int designVersionId, Guid[] blocksGuid, int[] blocksCount)
	{
		Invoke("CSG_SetBlocksUsed", new object[3] { designVersionId, blocksGuid, blocksCount });
	}

	public IAsyncResult BeginCSG_SetBlocksUsed(int designVersionId, Guid[] blocksGuid, int[] blocksCount, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_SetBlocksUsed", new object[3] { designVersionId, blocksGuid, blocksCount }, callback, asyncState);
	}

	public void EndCSG_SetBlocksUsed(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSG_SetBlocksUsedAsync(int designVersionId, Guid[] blocksGuid, int[] blocksCount)
	{
		CSG_SetBlocksUsedAsync(designVersionId, blocksGuid, blocksCount, null);
	}

	public void CSG_SetBlocksUsedAsync(int designVersionId, Guid[] blocksGuid, int[] blocksCount, object userState)
	{
		if (CSG_SetBlocksUsedOperationCompleted == null)
		{
			CSG_SetBlocksUsedOperationCompleted = OnCSG_SetBlocksUsedOperationCompleted;
		}
		InvokeAsync("CSG_SetBlocksUsed", new object[3] { designVersionId, blocksGuid, blocksCount }, CSG_SetBlocksUsedOperationCompleted, userState);
	}

	private void OnCSG_SetBlocksUsedOperationCompleted(object arg)
	{
		if (this.CSG_SetBlocksUsedCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_SetBlocksUsedCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_GetDesignAndRelatedInfoById", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable CSG_GetDesignAndRelatedInfoById(int designId)
	{
		return (DataTable)Invoke("CSG_GetDesignAndRelatedInfoById", new object[1] { designId })[0];
	}

	public IAsyncResult BeginCSG_GetDesignAndRelatedInfoById(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_GetDesignAndRelatedInfoById", new object[1] { designId }, callback, asyncState);
	}

	public DataTable EndCSG_GetDesignAndRelatedInfoById(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void CSG_GetDesignAndRelatedInfoByIdAsync(int designId)
	{
		CSG_GetDesignAndRelatedInfoByIdAsync(designId, null);
	}

	public void CSG_GetDesignAndRelatedInfoByIdAsync(int designId, object userState)
	{
		if (CSG_GetDesignAndRelatedInfoByIdOperationCompleted == null)
		{
			CSG_GetDesignAndRelatedInfoByIdOperationCompleted = OnCSG_GetDesignAndRelatedInfoByIdOperationCompleted;
		}
		InvokeAsync("CSG_GetDesignAndRelatedInfoById", new object[1] { designId }, CSG_GetDesignAndRelatedInfoByIdOperationCompleted, userState);
	}

	private void OnCSG_GetDesignAndRelatedInfoByIdOperationCompleted(object arg)
	{
		if (this.CSG_GetDesignAndRelatedInfoByIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_GetDesignAndRelatedInfoByIdCompleted(this, new CSG_GetDesignAndRelatedInfoByIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentId(int attributeTypeId, int designComponentId)
	{
		return (DataTable)Invoke("CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentId", new object[2] { attributeTypeId, designComponentId })[0];
	}

	public IAsyncResult BeginCSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentId(int attributeTypeId, int designComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentId", new object[2] { attributeTypeId, designComponentId }, callback, asyncState);
	}

	public DataTable EndCSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdAsync(int attributeTypeId, int designComponentId)
	{
		CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdAsync(attributeTypeId, designComponentId, null);
	}

	public void CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdAsync(int attributeTypeId, int designComponentId, object userState)
	{
		if (CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdOperationCompleted == null)
		{
			CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdOperationCompleted = OnCSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdOperationCompleted;
		}
		InvokeAsync("CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentId", new object[2] { attributeTypeId, designComponentId }, CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdOperationCompleted, userState);
	}

	private void OnCSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdOperationCompleted(object arg)
	{
		if (this.CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdCompleted(this, new CSG_GetDesignComponentAttributesByAttributeTypeIdAndDesignComponentIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CSD_SetDesignerBlocks", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CSD_SetDesignerBlocks(Guid[] blocksGuid, string[] blocksName)
	{
		Invoke("CSD_SetDesignerBlocks", new object[2] { blocksGuid, blocksName });
	}

	public IAsyncResult BeginCSD_SetDesignerBlocks(Guid[] blocksGuid, string[] blocksName, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CSD_SetDesignerBlocks", new object[2] { blocksGuid, blocksName }, callback, asyncState);
	}

	public void EndCSD_SetDesignerBlocks(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CSD_SetDesignerBlocksAsync(Guid[] blocksGuid, string[] blocksName)
	{
		CSD_SetDesignerBlocksAsync(blocksGuid, blocksName, null);
	}

	public void CSD_SetDesignerBlocksAsync(Guid[] blocksGuid, string[] blocksName, object userState)
	{
		if (CSD_SetDesignerBlocksOperationCompleted == null)
		{
			CSD_SetDesignerBlocksOperationCompleted = OnCSD_SetDesignerBlocksOperationCompleted;
		}
		InvokeAsync("CSD_SetDesignerBlocks", new object[2] { blocksGuid, blocksName }, CSD_SetDesignerBlocksOperationCompleted, userState);
	}

	private void OnCSD_SetDesignerBlocksOperationCompleted(object arg)
	{
		if (this.CSD_SetDesignerBlocksCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CSD_SetDesignerBlocksCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetSurveyQuestions", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public CSFeedbackQuestion[] GetSurveyQuestions()
	{
		return (CSFeedbackQuestion[])Invoke("GetSurveyQuestions", new object[0])[0];
	}

	public IAsyncResult BeginGetSurveyQuestions(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetSurveyQuestions", new object[0], callback, asyncState);
	}

	public CSFeedbackQuestion[] EndGetSurveyQuestions(IAsyncResult asyncResult)
	{
		return (CSFeedbackQuestion[])EndInvoke(asyncResult)[0];
	}

	public void GetSurveyQuestionsAsync()
	{
		GetSurveyQuestionsAsync(null);
	}

	public void GetSurveyQuestionsAsync(object userState)
	{
		if (GetSurveyQuestionsOperationCompleted == null)
		{
			GetSurveyQuestionsOperationCompleted = OnGetSurveyQuestionsOperationCompleted;
		}
		InvokeAsync("GetSurveyQuestions", new object[0], GetSurveyQuestionsOperationCompleted, userState);
	}

	private void OnGetSurveyQuestionsOperationCompleted(object arg)
	{
		if (this.GetSurveyQuestionsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetSurveyQuestionsCompleted(this, new GetSurveyQuestionsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SaveFeedback", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SaveFeedback(CSFeedbackResult csFeedbackResult)
	{
		Invoke("SaveFeedback", new object[1] { csFeedbackResult });
	}

	public IAsyncResult BeginSaveFeedback(CSFeedbackResult csFeedbackResult, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SaveFeedback", new object[1] { csFeedbackResult }, callback, asyncState);
	}

	public void EndSaveFeedback(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SaveFeedbackAsync(CSFeedbackResult csFeedbackResult)
	{
		SaveFeedbackAsync(csFeedbackResult, null);
	}

	public void SaveFeedbackAsync(CSFeedbackResult csFeedbackResult, object userState)
	{
		if (SaveFeedbackOperationCompleted == null)
		{
			SaveFeedbackOperationCompleted = OnSaveFeedbackOperationCompleted;
		}
		InvokeAsync("SaveFeedback", new object[1] { csFeedbackResult }, SaveFeedbackOperationCompleted, userState);
	}

	private void OnSaveFeedbackOperationCompleted(object arg)
	{
		if (this.SaveFeedbackCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SaveFeedbackCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetLatestPublicCounterSketchVersionString", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetLatestPublicCounterSketchVersionString()
	{
		return (string)Invoke("GetLatestPublicCounterSketchVersionString", new object[0])[0];
	}

	public IAsyncResult BeginGetLatestPublicCounterSketchVersionString(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetLatestPublicCounterSketchVersionString", new object[0], callback, asyncState);
	}

	public string EndGetLatestPublicCounterSketchVersionString(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetLatestPublicCounterSketchVersionStringAsync()
	{
		GetLatestPublicCounterSketchVersionStringAsync(null);
	}

	public void GetLatestPublicCounterSketchVersionStringAsync(object userState)
	{
		if (GetLatestPublicCounterSketchVersionStringOperationCompleted == null)
		{
			GetLatestPublicCounterSketchVersionStringOperationCompleted = OnGetLatestPublicCounterSketchVersionStringOperationCompleted;
		}
		InvokeAsync("GetLatestPublicCounterSketchVersionString", new object[0], GetLatestPublicCounterSketchVersionStringOperationCompleted, userState);
	}

	private void OnGetLatestPublicCounterSketchVersionStringOperationCompleted(object arg)
	{
		if (this.GetLatestPublicCounterSketchVersionStringCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetLatestPublicCounterSketchVersionStringCompleted(this, new GetLatestPublicCounterSketchVersionStringCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetMyLibraryFiles", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string[] GetMyLibraryFiles(int KeyId)
	{
		return (string[])Invoke("GetMyLibraryFiles", new object[1] { KeyId })[0];
	}

	public IAsyncResult BeginGetMyLibraryFiles(int KeyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetMyLibraryFiles", new object[1] { KeyId }, callback, asyncState);
	}

	public string[] EndGetMyLibraryFiles(IAsyncResult asyncResult)
	{
		return (string[])EndInvoke(asyncResult)[0];
	}

	public void GetMyLibraryFilesAsync(int KeyId)
	{
		GetMyLibraryFilesAsync(KeyId, null);
	}

	public void GetMyLibraryFilesAsync(int KeyId, object userState)
	{
		if (GetMyLibraryFilesOperationCompleted == null)
		{
			GetMyLibraryFilesOperationCompleted = OnGetMyLibraryFilesOperationCompleted;
		}
		InvokeAsync("GetMyLibraryFiles", new object[1] { KeyId }, GetMyLibraryFilesOperationCompleted, userState);
	}

	private void OnGetMyLibraryFilesOperationCompleted(object arg)
	{
		if (this.GetMyLibraryFilesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetMyLibraryFilesCompleted(this, new GetMyLibraryFilesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetMyLibraryZBin", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] GetMyLibraryZBin(int KeyId)
	{
		return (byte[])Invoke("GetMyLibraryZBin", new object[1] { KeyId })[0];
	}

	public IAsyncResult BeginGetMyLibraryZBin(int KeyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetMyLibraryZBin", new object[1] { KeyId }, callback, asyncState);
	}

	public byte[] EndGetMyLibraryZBin(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void GetMyLibraryZBinAsync(int KeyId)
	{
		GetMyLibraryZBinAsync(KeyId, null);
	}

	public void GetMyLibraryZBinAsync(int KeyId, object userState)
	{
		if (GetMyLibraryZBinOperationCompleted == null)
		{
			GetMyLibraryZBinOperationCompleted = OnGetMyLibraryZBinOperationCompleted;
		}
		InvokeAsync("GetMyLibraryZBin", new object[1] { KeyId }, GetMyLibraryZBinOperationCompleted, userState);
	}

	private void OnGetMyLibraryZBinOperationCompleted(object arg)
	{
		if (this.GetMyLibraryZBinCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetMyLibraryZBinCompleted(this, new GetMyLibraryZBinCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetMyLibraryVersion", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetMyLibraryVersion(int KeyId)
	{
		return (int)Invoke("GetMyLibraryVersion", new object[1] { KeyId })[0];
	}

	public IAsyncResult BeginGetMyLibraryVersion(int KeyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetMyLibraryVersion", new object[1] { KeyId }, callback, asyncState);
	}

	public int EndGetMyLibraryVersion(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetMyLibraryVersionAsync(int KeyId)
	{
		GetMyLibraryVersionAsync(KeyId, null);
	}

	public void GetMyLibraryVersionAsync(int KeyId, object userState)
	{
		if (GetMyLibraryVersionOperationCompleted == null)
		{
			GetMyLibraryVersionOperationCompleted = OnGetMyLibraryVersionOperationCompleted;
		}
		InvokeAsync("GetMyLibraryVersion", new object[1] { KeyId }, GetMyLibraryVersionOperationCompleted, userState);
	}

	private void OnGetMyLibraryVersionOperationCompleted(object arg)
	{
		if (this.GetMyLibraryVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetMyLibraryVersionCompleted(this, new GetMyLibraryVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/TransmitDetailedDebuggingReport", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void TransmitDetailedDebuggingReport(string userName, int keyId, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode)
	{
		Invoke("TransmitDetailedDebuggingReport", new object[10] { userName, keyId, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode });
	}

	public IAsyncResult BeginTransmitDetailedDebuggingReport(string userName, int keyId, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("TransmitDetailedDebuggingReport", new object[10] { userName, keyId, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode }, callback, asyncState);
	}

	public void EndTransmitDetailedDebuggingReport(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void TransmitDetailedDebuggingReportAsync(string userName, int keyId, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode)
	{
		TransmitDetailedDebuggingReportAsync(userName, keyId, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode, null);
	}

	public void TransmitDetailedDebuggingReportAsync(string userName, int keyId, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode, object userState)
	{
		if (TransmitDetailedDebuggingReportOperationCompleted == null)
		{
			TransmitDetailedDebuggingReportOperationCompleted = OnTransmitDetailedDebuggingReportOperationCompleted;
		}
		InvokeAsync("TransmitDetailedDebuggingReport", new object[10] { userName, keyId, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode }, TransmitDetailedDebuggingReportOperationCompleted, userState);
	}

	private void OnTransmitDetailedDebuggingReportOperationCompleted(object arg)
	{
		if (this.TransmitDetailedDebuggingReportCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.TransmitDetailedDebuggingReportCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/TransmitDebuggingReport", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void TransmitDebuggingReport(string userName, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode)
	{
		Invoke("TransmitDebuggingReport", new object[9] { userName, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode });
	}

	public IAsyncResult BeginTransmitDebuggingReport(string userName, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("TransmitDebuggingReport", new object[9] { userName, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode }, callback, asyncState);
	}

	public void EndTransmitDebuggingReport(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void TransmitDebuggingReportAsync(string userName, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode)
	{
		TransmitDebuggingReportAsync(userName, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode, null);
	}

	public void TransmitDebuggingReportAsync(string userName, string usersEMailAddress, string usersPhoneNumber, string userComments, string counterSketchVersion, string debuggingLog, string eventLogEntries, string systemInfo, bool debugMode, object userState)
	{
		if (TransmitDebuggingReportOperationCompleted == null)
		{
			TransmitDebuggingReportOperationCompleted = OnTransmitDebuggingReportOperationCompleted;
		}
		InvokeAsync("TransmitDebuggingReport", new object[9] { userName, usersEMailAddress, usersPhoneNumber, userComments, counterSketchVersion, debuggingLog, eventLogEntries, systemInfo, debugMode }, TransmitDebuggingReportOperationCompleted, userState);
	}

	private void OnTransmitDebuggingReportOperationCompleted(object arg)
	{
		if (this.TransmitDebuggingReportCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.TransmitDebuggingReportCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GeoCodeAddress", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public double[] GeoCodeAddress(string address)
	{
		return (double[])Invoke("GeoCodeAddress", new object[1] { address })[0];
	}

	public IAsyncResult BeginGeoCodeAddress(string address, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GeoCodeAddress", new object[1] { address }, callback, asyncState);
	}

	public double[] EndGeoCodeAddress(IAsyncResult asyncResult)
	{
		return (double[])EndInvoke(asyncResult)[0];
	}

	public void GeoCodeAddressAsync(string address)
	{
		GeoCodeAddressAsync(address, null);
	}

	public void GeoCodeAddressAsync(string address, object userState)
	{
		if (GeoCodeAddressOperationCompleted == null)
		{
			GeoCodeAddressOperationCompleted = OnGeoCodeAddressOperationCompleted;
		}
		InvokeAsync("GeoCodeAddress", new object[1] { address }, GeoCodeAddressOperationCompleted, userState);
	}

	private void OnGeoCodeAddressOperationCompleted(object arg)
	{
		if (this.GeoCodeAddressCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GeoCodeAddressCompleted(this, new GeoCodeAddressCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateKeyInformation_2", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateKeyInformation_2(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, double latitude, double longitude)
	{
		Invoke("UpdateKeyInformation_2", new object[10] { keyId, name, company, accountNo, address, city, state, zip, latitude, longitude });
	}

	public IAsyncResult BeginUpdateKeyInformation_2(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, double latitude, double longitude, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateKeyInformation_2", new object[10] { keyId, name, company, accountNo, address, city, state, zip, latitude, longitude }, callback, asyncState);
	}

	public void EndUpdateKeyInformation_2(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateKeyInformation_2Async(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, double latitude, double longitude)
	{
		UpdateKeyInformation_2Async(keyId, name, company, accountNo, address, city, state, zip, latitude, longitude, null);
	}

	public void UpdateKeyInformation_2Async(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, double latitude, double longitude, object userState)
	{
		if (UpdateKeyInformation_2OperationCompleted == null)
		{
			UpdateKeyInformation_2OperationCompleted = OnUpdateKeyInformation_2OperationCompleted;
		}
		InvokeAsync("UpdateKeyInformation_2", new object[10] { keyId, name, company, accountNo, address, city, state, zip, latitude, longitude }, UpdateKeyInformation_2OperationCompleted, userState);
	}

	private void OnUpdateKeyInformation_2OperationCompleted(object arg)
	{
		if (this.UpdateKeyInformation_2Completed != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateKeyInformation_2Completed(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDebugVersionString", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetDebugVersionString(int keyId)
	{
		return (string)Invoke("GetDebugVersionString", new object[1] { keyId })[0];
	}

	public IAsyncResult BeginGetDebugVersionString(int keyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDebugVersionString", new object[1] { keyId }, callback, asyncState);
	}

	public string EndGetDebugVersionString(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetDebugVersionStringAsync(int keyId)
	{
		GetDebugVersionStringAsync(keyId, null);
	}

	public void GetDebugVersionStringAsync(int keyId, object userState)
	{
		if (GetDebugVersionStringOperationCompleted == null)
		{
			GetDebugVersionStringOperationCompleted = OnGetDebugVersionStringOperationCompleted;
		}
		InvokeAsync("GetDebugVersionString", new object[1] { keyId }, GetDebugVersionStringOperationCompleted, userState);
	}

	private void OnGetDebugVersionStringOperationCompleted(object arg)
	{
		if (this.GetDebugVersionStringCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDebugVersionStringCompleted(this, new GetDebugVersionStringCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetMyCounterSketchVersionString", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetMyCounterSketchVersionString(int KeyId)
	{
		return (string)Invoke("GetMyCounterSketchVersionString", new object[1] { KeyId })[0];
	}

	public IAsyncResult BeginGetMyCounterSketchVersionString(int KeyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetMyCounterSketchVersionString", new object[1] { KeyId }, callback, asyncState);
	}

	public string EndGetMyCounterSketchVersionString(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetMyCounterSketchVersionStringAsync(int KeyId)
	{
		GetMyCounterSketchVersionStringAsync(KeyId, null);
	}

	public void GetMyCounterSketchVersionStringAsync(int KeyId, object userState)
	{
		if (GetMyCounterSketchVersionStringOperationCompleted == null)
		{
			GetMyCounterSketchVersionStringOperationCompleted = OnGetMyCounterSketchVersionStringOperationCompleted;
		}
		InvokeAsync("GetMyCounterSketchVersionString", new object[1] { KeyId }, GetMyCounterSketchVersionStringOperationCompleted, userState);
	}

	private void OnGetMyCounterSketchVersionStringOperationCompleted(object arg)
	{
		if (this.GetMyCounterSketchVersionStringCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetMyCounterSketchVersionStringCompleted(this, new GetMyCounterSketchVersionStringCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/setVersion", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void setVersion(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount)
	{
		Invoke("setVersion", new object[5] { KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount });
	}

	public IAsyncResult BeginsetVersion(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("setVersion", new object[5] { KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount }, callback, asyncState);
	}

	public void EndsetVersion(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void setVersionAsync(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount)
	{
		setVersionAsync(KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount, null);
	}

	public void setVersionAsync(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, object userState)
	{
		if (setVersionOperationCompleted == null)
		{
			setVersionOperationCompleted = OnsetVersionOperationCompleted;
		}
		InvokeAsync("setVersion", new object[5] { KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount }, setVersionOperationCompleted, userState);
	}

	private void OnsetVersionOperationCompleted(object arg)
	{
		if (this.setVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.setVersionCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/setVersion1", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void setVersion1(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, string PrimaryEmail)
	{
		Invoke("setVersion1", new object[6] { KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount, PrimaryEmail });
	}

	public IAsyncResult BeginsetVersion1(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, string PrimaryEmail, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("setVersion1", new object[6] { KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount, PrimaryEmail }, callback, asyncState);
	}

	public void EndsetVersion1(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void setVersion1Async(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, string PrimaryEmail)
	{
		setVersion1Async(KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount, PrimaryEmail, null);
	}

	public void setVersion1Async(int KeyId, string CSVersion, string RhinoVersion, string LibraryVersion, string ModelCount, string PrimaryEmail, object userState)
	{
		if (setVersion1OperationCompleted == null)
		{
			setVersion1OperationCompleted = OnsetVersion1OperationCompleted;
		}
		InvokeAsync("setVersion1", new object[6] { KeyId, CSVersion, RhinoVersion, LibraryVersion, ModelCount, PrimaryEmail }, setVersion1OperationCompleted, userState);
	}

	private void OnsetVersion1OperationCompleted(object arg)
	{
		if (this.setVersion1Completed != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.setVersion1Completed(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetUserTypeId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetUserTypeId(int keyId)
	{
		return (int)Invoke("GetUserTypeId", new object[1] { keyId })[0];
	}

	public IAsyncResult BeginGetUserTypeId(int keyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetUserTypeId", new object[1] { keyId }, callback, asyncState);
	}

	public int EndGetUserTypeId(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetUserTypeIdAsync(int keyId)
	{
		GetUserTypeIdAsync(keyId, null);
	}

	public void GetUserTypeIdAsync(int keyId, object userState)
	{
		if (GetUserTypeIdOperationCompleted == null)
		{
			GetUserTypeIdOperationCompleted = OnGetUserTypeIdOperationCompleted;
		}
		InvokeAsync("GetUserTypeId", new object[1] { keyId }, GetUserTypeIdOperationCompleted, userState);
	}

	private void OnGetUserTypeIdOperationCompleted(object arg)
	{
		if (this.GetUserTypeIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetUserTypeIdCompleted(this, new GetUserTypeIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignVersionComponentId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetDesignVersionComponentId(int DesignVersionId, int Component)
	{
		return (int)Invoke("GetDesignVersionComponentId", new object[2] { DesignVersionId, Component })[0];
	}

	public IAsyncResult BeginGetDesignVersionComponentId(int DesignVersionId, int Component, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignVersionComponentId", new object[2] { DesignVersionId, Component }, callback, asyncState);
	}

	public int EndGetDesignVersionComponentId(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetDesignVersionComponentIdAsync(int DesignVersionId, int Component)
	{
		GetDesignVersionComponentIdAsync(DesignVersionId, Component, null);
	}

	public void GetDesignVersionComponentIdAsync(int DesignVersionId, int Component, object userState)
	{
		if (GetDesignVersionComponentIdOperationCompleted == null)
		{
			GetDesignVersionComponentIdOperationCompleted = OnGetDesignVersionComponentIdOperationCompleted;
		}
		InvokeAsync("GetDesignVersionComponentId", new object[2] { DesignVersionId, Component }, GetDesignVersionComponentIdOperationCompleted, userState);
	}

	private void OnGetDesignVersionComponentIdOperationCompleted(object arg)
	{
		if (this.GetDesignVersionComponentIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignVersionComponentIdCompleted(this, new GetDesignVersionComponentIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/AddAnswer", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int AddAnswer(string answer)
	{
		return (int)Invoke("AddAnswer", new object[1] { answer })[0];
	}

	public IAsyncResult BeginAddAnswer(string answer, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("AddAnswer", new object[1] { answer }, callback, asyncState);
	}

	public int EndAddAnswer(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void AddAnswerAsync(string answer)
	{
		AddAnswerAsync(answer, null);
	}

	public void AddAnswerAsync(string answer, object userState)
	{
		if (AddAnswerOperationCompleted == null)
		{
			AddAnswerOperationCompleted = OnAddAnswerOperationCompleted;
		}
		InvokeAsync("AddAnswer", new object[1] { answer }, AddAnswerOperationCompleted, userState);
	}

	private void OnAddAnswerOperationCompleted(object arg)
	{
		if (this.AddAnswerCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.AddAnswerCompleted(this, new AddAnswerCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/AddAssignment", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int AddAssignment(string name, string number, string description, string documentPath)
	{
		return (int)Invoke("AddAssignment", new object[4] { name, number, description, documentPath })[0];
	}

	public IAsyncResult BeginAddAssignment(string name, string number, string description, string documentPath, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("AddAssignment", new object[4] { name, number, description, documentPath }, callback, asyncState);
	}

	public int EndAddAssignment(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void AddAssignmentAsync(string name, string number, string description, string documentPath)
	{
		AddAssignmentAsync(name, number, description, documentPath, null);
	}

	public void AddAssignmentAsync(string name, string number, string description, string documentPath, object userState)
	{
		if (AddAssignmentOperationCompleted == null)
		{
			AddAssignmentOperationCompleted = OnAddAssignmentOperationCompleted;
		}
		InvokeAsync("AddAssignment", new object[4] { name, number, description, documentPath }, AddAssignmentOperationCompleted, userState);
	}

	private void OnAddAssignmentOperationCompleted(object arg)
	{
		if (this.AddAssignmentCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.AddAssignmentCompleted(this, new AddAssignmentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/AddAssignmentNew", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int AddAssignmentNew(string name, string number, string description)
	{
		return (int)Invoke("AddAssignmentNew", new object[3] { name, number, description })[0];
	}

	public IAsyncResult BeginAddAssignmentNew(string name, string number, string description, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("AddAssignmentNew", new object[3] { name, number, description }, callback, asyncState);
	}

	public int EndAddAssignmentNew(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void AddAssignmentNewAsync(string name, string number, string description)
	{
		AddAssignmentNewAsync(name, number, description, null);
	}

	public void AddAssignmentNewAsync(string name, string number, string description, object userState)
	{
		if (AddAssignmentNewOperationCompleted == null)
		{
			AddAssignmentNewOperationCompleted = OnAddAssignmentNewOperationCompleted;
		}
		InvokeAsync("AddAssignmentNew", new object[3] { name, number, description }, AddAssignmentNewOperationCompleted, userState);
	}

	private void OnAddAssignmentNewOperationCompleted(object arg)
	{
		if (this.AddAssignmentNewCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.AddAssignmentNewCompleted(this, new AddAssignmentNewCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/AddQuestion", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int AddQuestion(int questionTypeId, string question)
	{
		return (int)Invoke("AddQuestion", new object[2] { questionTypeId, question })[0];
	}

	public IAsyncResult BeginAddQuestion(int questionTypeId, string question, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("AddQuestion", new object[2] { questionTypeId, question }, callback, asyncState);
	}

	public int EndAddQuestion(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void AddQuestionAsync(int questionTypeId, string question)
	{
		AddQuestionAsync(questionTypeId, question, null);
	}

	public void AddQuestionAsync(int questionTypeId, string question, object userState)
	{
		if (AddQuestionOperationCompleted == null)
		{
			AddQuestionOperationCompleted = OnAddQuestionOperationCompleted;
		}
		InvokeAsync("AddQuestion", new object[2] { questionTypeId, question }, AddQuestionOperationCompleted, userState);
	}

	private void OnAddQuestionOperationCompleted(object arg)
	{
		if (this.AddQuestionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.AddQuestionCompleted(this, new AddQuestionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignCanBeStullerApproved", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool DesignCanBeStullerApproved(int designId)
	{
		return (bool)Invoke("DesignCanBeStullerApproved", new object[1] { designId })[0];
	}

	public IAsyncResult BeginDesignCanBeStullerApproved(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignCanBeStullerApproved", new object[1] { designId }, callback, asyncState);
	}

	public bool EndDesignCanBeStullerApproved(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void DesignCanBeStullerApprovedAsync(int designId)
	{
		DesignCanBeStullerApprovedAsync(designId, null);
	}

	public void DesignCanBeStullerApprovedAsync(int designId, object userState)
	{
		if (DesignCanBeStullerApprovedOperationCompleted == null)
		{
			DesignCanBeStullerApprovedOperationCompleted = OnDesignCanBeStullerApprovedOperationCompleted;
		}
		InvokeAsync("DesignCanBeStullerApproved", new object[1] { designId }, DesignCanBeStullerApprovedOperationCompleted, userState);
	}

	private void OnDesignCanBeStullerApprovedOperationCompleted(object arg)
	{
		if (this.DesignCanBeStullerApprovedCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignCanBeStullerApprovedCompleted(this, new DesignCanBeStullerApprovedCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignLock", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool DesignLock(int DesignId, string Guid)
	{
		return (bool)Invoke("DesignLock", new object[2] { DesignId, Guid })[0];
	}

	public IAsyncResult BeginDesignLock(int DesignId, string Guid, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignLock", new object[2] { DesignId, Guid }, callback, asyncState);
	}

	public bool EndDesignLock(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void DesignLockAsync(int DesignId, string Guid)
	{
		DesignLockAsync(DesignId, Guid, null);
	}

	public void DesignLockAsync(int DesignId, string Guid, object userState)
	{
		if (DesignLockOperationCompleted == null)
		{
			DesignLockOperationCompleted = OnDesignLockOperationCompleted;
		}
		InvokeAsync("DesignLock", new object[2] { DesignId, Guid }, DesignLockOperationCompleted, userState);
	}

	private void OnDesignLockOperationCompleted(object arg)
	{
		if (this.DesignLockCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignLockCompleted(this, new DesignLockCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignLockInfo", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool DesignLockInfo(int DesignId, string Guid, ref int status, ref DateTime dbTimeStamp, ref DateTime fileTimeStamp, ref int ComponentId)
	{
		object[] array = Invoke("DesignLockInfo", new object[6] { DesignId, Guid, status, dbTimeStamp, fileTimeStamp, ComponentId });
		status = (int)array[1];
		dbTimeStamp = (DateTime)array[2];
		fileTimeStamp = (DateTime)array[3];
		ComponentId = (int)array[4];
		return (bool)array[0];
	}

	public IAsyncResult BeginDesignLockInfo(int DesignId, string Guid, int status, DateTime dbTimeStamp, DateTime fileTimeStamp, int ComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignLockInfo", new object[6] { DesignId, Guid, status, dbTimeStamp, fileTimeStamp, ComponentId }, callback, asyncState);
	}

	public bool EndDesignLockInfo(IAsyncResult asyncResult, out int status, out DateTime dbTimeStamp, out DateTime fileTimeStamp, out int ComponentId)
	{
		object[] array = EndInvoke(asyncResult);
		status = (int)array[1];
		dbTimeStamp = (DateTime)array[2];
		fileTimeStamp = (DateTime)array[3];
		ComponentId = (int)array[4];
		return (bool)array[0];
	}

	public void DesignLockInfoAsync(int DesignId, string Guid, int status, DateTime dbTimeStamp, DateTime fileTimeStamp, int ComponentId)
	{
		DesignLockInfoAsync(DesignId, Guid, status, dbTimeStamp, fileTimeStamp, ComponentId, null);
	}

	public void DesignLockInfoAsync(int DesignId, string Guid, int status, DateTime dbTimeStamp, DateTime fileTimeStamp, int ComponentId, object userState)
	{
		if (DesignLockInfoOperationCompleted == null)
		{
			DesignLockInfoOperationCompleted = OnDesignLockInfoOperationCompleted;
		}
		InvokeAsync("DesignLockInfo", new object[6] { DesignId, Guid, status, dbTimeStamp, fileTimeStamp, ComponentId }, DesignLockInfoOperationCompleted, userState);
	}

	private void OnDesignLockInfoOperationCompleted(object arg)
	{
		if (this.DesignLockInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignLockInfoCompleted(this, new DesignLockInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignReleaseLock", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void DesignReleaseLock(int DesignId)
	{
		Invoke("DesignReleaseLock", new object[1] { DesignId });
	}

	public IAsyncResult BeginDesignReleaseLock(int DesignId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignReleaseLock", new object[1] { DesignId }, callback, asyncState);
	}

	public void EndDesignReleaseLock(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void DesignReleaseLockAsync(int DesignId)
	{
		DesignReleaseLockAsync(DesignId, null);
	}

	public void DesignReleaseLockAsync(int DesignId, object userState)
	{
		if (DesignReleaseLockOperationCompleted == null)
		{
			DesignReleaseLockOperationCompleted = OnDesignReleaseLockOperationCompleted;
		}
		InvokeAsync("DesignReleaseLock", new object[1] { DesignId }, DesignReleaseLockOperationCompleted, userState);
	}

	private void OnDesignReleaseLockOperationCompleted(object arg)
	{
		if (this.DesignReleaseLockCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignReleaseLockCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignsSetStatus", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int DesignsSetStatus(string DesignId, int StatusId)
	{
		return (int)Invoke("DesignsSetStatus", new object[2] { DesignId, StatusId })[0];
	}

	public IAsyncResult BeginDesignsSetStatus(string DesignId, int StatusId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignsSetStatus", new object[2] { DesignId, StatusId }, callback, asyncState);
	}

	public int EndDesignsSetStatus(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void DesignsSetStatusAsync(string DesignId, int StatusId)
	{
		DesignsSetStatusAsync(DesignId, StatusId, null);
	}

	public void DesignsSetStatusAsync(string DesignId, int StatusId, object userState)
	{
		if (DesignsSetStatusOperationCompleted == null)
		{
			DesignsSetStatusOperationCompleted = OnDesignsSetStatusOperationCompleted;
		}
		InvokeAsync("DesignsSetStatus", new object[2] { DesignId, StatusId }, DesignsSetStatusOperationCompleted, userState);
	}

	private void OnDesignsSetStatusOperationCompleted(object arg)
	{
		if (this.DesignsSetStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignsSetStatusCompleted(this, new DesignsSetStatusCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DownloadLatestCounterSketchVersion", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] DownloadLatestCounterSketchVersion()
	{
		return (byte[])Invoke("DownloadLatestCounterSketchVersion", new object[0])[0];
	}

	public IAsyncResult BeginDownloadLatestCounterSketchVersion(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DownloadLatestCounterSketchVersion", new object[0], callback, asyncState);
	}

	public byte[] EndDownloadLatestCounterSketchVersion(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void DownloadLatestCounterSketchVersionAsync()
	{
		DownloadLatestCounterSketchVersionAsync(null);
	}

	public void DownloadLatestCounterSketchVersionAsync(object userState)
	{
		if (DownloadLatestCounterSketchVersionOperationCompleted == null)
		{
			DownloadLatestCounterSketchVersionOperationCompleted = OnDownloadLatestCounterSketchVersionOperationCompleted;
		}
		InvokeAsync("DownloadLatestCounterSketchVersion", new object[0], DownloadLatestCounterSketchVersionOperationCompleted, userState);
	}

	private void OnDownloadLatestCounterSketchVersionOperationCompleted(object arg)
	{
		if (this.DownloadLatestCounterSketchVersionCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DownloadLatestCounterSketchVersionCompleted(this, new DownloadLatestCounterSketchVersionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/FindADesign", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int FindADesign(int AssignmentId, int ModelNumber, int DesignTypeId)
	{
		return (int)Invoke("FindADesign", new object[3] { AssignmentId, ModelNumber, DesignTypeId })[0];
	}

	public IAsyncResult BeginFindADesign(int AssignmentId, int ModelNumber, int DesignTypeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("FindADesign", new object[3] { AssignmentId, ModelNumber, DesignTypeId }, callback, asyncState);
	}

	public int EndFindADesign(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void FindADesignAsync(int AssignmentId, int ModelNumber, int DesignTypeId)
	{
		FindADesignAsync(AssignmentId, ModelNumber, DesignTypeId, null);
	}

	public void FindADesignAsync(int AssignmentId, int ModelNumber, int DesignTypeId, object userState)
	{
		if (FindADesignOperationCompleted == null)
		{
			FindADesignOperationCompleted = OnFindADesignOperationCompleted;
		}
		InvokeAsync("FindADesign", new object[3] { AssignmentId, ModelNumber, DesignTypeId }, FindADesignOperationCompleted, userState);
	}

	private void OnFindADesignOperationCompleted(object arg)
	{
		if (this.FindADesignCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.FindADesignCompleted(this, new FindADesignCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/FindADesign2", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int FindADesign2(int AssignmentId, int DesignerId, int ModelNumber, int DesignTypeId)
	{
		return (int)Invoke("FindADesign2", new object[4] { AssignmentId, DesignerId, ModelNumber, DesignTypeId })[0];
	}

	public IAsyncResult BeginFindADesign2(int AssignmentId, int DesignerId, int ModelNumber, int DesignTypeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("FindADesign2", new object[4] { AssignmentId, DesignerId, ModelNumber, DesignTypeId }, callback, asyncState);
	}

	public int EndFindADesign2(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void FindADesign2Async(int AssignmentId, int DesignerId, int ModelNumber, int DesignTypeId)
	{
		FindADesign2Async(AssignmentId, DesignerId, ModelNumber, DesignTypeId, null);
	}

	public void FindADesign2Async(int AssignmentId, int DesignerId, int ModelNumber, int DesignTypeId, object userState)
	{
		if (FindADesign2OperationCompleted == null)
		{
			FindADesign2OperationCompleted = OnFindADesign2OperationCompleted;
		}
		InvokeAsync("FindADesign2", new object[4] { AssignmentId, DesignerId, ModelNumber, DesignTypeId }, FindADesign2OperationCompleted, userState);
	}

	private void OnFindADesign2OperationCompleted(object arg)
	{
		if (this.FindADesign2Completed != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.FindADesign2Completed(this, new FindADesign2CompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllAttributes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllAttributes()
	{
		return (DataTable)Invoke("GetAllAttributes", new object[0])[0];
	}

	public IAsyncResult BeginGetAllAttributes(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllAttributes", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllAttributes(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllAttributesAsync()
	{
		GetAllAttributesAsync(null);
	}

	public void GetAllAttributesAsync(object userState)
	{
		if (GetAllAttributesOperationCompleted == null)
		{
			GetAllAttributesOperationCompleted = OnGetAllAttributesOperationCompleted;
		}
		InvokeAsync("GetAllAttributes", new object[0], GetAllAttributesOperationCompleted, userState);
	}

	private void OnGetAllAttributesOperationCompleted(object arg)
	{
		if (this.GetAllAttributesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllAttributesCompleted(this, new GetAllAttributesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllAttributeTypes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllAttributeTypes()
	{
		return (DataTable)Invoke("GetAllAttributeTypes", new object[0])[0];
	}

	public IAsyncResult BeginGetAllAttributeTypes(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllAttributeTypes", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllAttributeTypes(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllAttributeTypesAsync()
	{
		GetAllAttributeTypesAsync(null);
	}

	public void GetAllAttributeTypesAsync(object userState)
	{
		if (GetAllAttributeTypesOperationCompleted == null)
		{
			GetAllAttributeTypesOperationCompleted = OnGetAllAttributeTypesOperationCompleted;
		}
		InvokeAsync("GetAllAttributeTypes", new object[0], GetAllAttributeTypesOperationCompleted, userState);
	}

	private void OnGetAllAttributeTypesOperationCompleted(object arg)
	{
		if (this.GetAllAttributeTypesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllAttributeTypesCompleted(this, new GetAllAttributeTypesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesigns", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesigns()
	{
		return (DataTable)Invoke("GetAllDesigns", new object[0])[0];
	}

	public IAsyncResult BeginGetAllDesigns(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesigns", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllDesigns(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsAsync()
	{
		GetAllDesignsAsync(null);
	}

	public void GetAllDesignsAsync(object userState)
	{
		if (GetAllDesignsOperationCompleted == null)
		{
			GetAllDesignsOperationCompleted = OnGetAllDesignsOperationCompleted;
		}
		InvokeAsync("GetAllDesigns", new object[0], GetAllDesignsOperationCompleted, userState);
	}

	private void OnGetAllDesignsOperationCompleted(object arg)
	{
		if (this.GetAllDesignsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsCompleted(this, new GetAllDesignsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsAndRelatedInfo", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignsAndRelatedInfo()
	{
		return (DataTable)Invoke("GetAllDesignsAndRelatedInfo", new object[0])[0];
	}

	public IAsyncResult BeginGetAllDesignsAndRelatedInfo(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsAndRelatedInfo", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllDesignsAndRelatedInfo(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsAndRelatedInfoAsync()
	{
		GetAllDesignsAndRelatedInfoAsync(null);
	}

	public void GetAllDesignsAndRelatedInfoAsync(object userState)
	{
		if (GetAllDesignsAndRelatedInfoOperationCompleted == null)
		{
			GetAllDesignsAndRelatedInfoOperationCompleted = OnGetAllDesignsAndRelatedInfoOperationCompleted;
		}
		InvokeAsync("GetAllDesignsAndRelatedInfo", new object[0], GetAllDesignsAndRelatedInfoOperationCompleted, userState);
	}

	private void OnGetAllDesignsAndRelatedInfoOperationCompleted(object arg)
	{
		if (this.GetAllDesignsAndRelatedInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsAndRelatedInfoCompleted(this, new GetAllDesignsAndRelatedInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsAndRelatedInfoByKewordSearch", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignsAndRelatedInfoByKewordSearch(string keyword)
	{
		return (DataTable)Invoke("GetAllDesignsAndRelatedInfoByKewordSearch", new object[1] { keyword })[0];
	}

	public IAsyncResult BeginGetAllDesignsAndRelatedInfoByKewordSearch(string keyword, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsAndRelatedInfoByKewordSearch", new object[1] { keyword }, callback, asyncState);
	}

	public DataTable EndGetAllDesignsAndRelatedInfoByKewordSearch(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsAndRelatedInfoByKewordSearchAsync(string keyword)
	{
		GetAllDesignsAndRelatedInfoByKewordSearchAsync(keyword, null);
	}

	public void GetAllDesignsAndRelatedInfoByKewordSearchAsync(string keyword, object userState)
	{
		if (GetAllDesignsAndRelatedInfoByKewordSearchOperationCompleted == null)
		{
			GetAllDesignsAndRelatedInfoByKewordSearchOperationCompleted = OnGetAllDesignsAndRelatedInfoByKewordSearchOperationCompleted;
		}
		InvokeAsync("GetAllDesignsAndRelatedInfoByKewordSearch", new object[1] { keyword }, GetAllDesignsAndRelatedInfoByKewordSearchOperationCompleted, userState);
	}

	private void OnGetAllDesignsAndRelatedInfoByKewordSearchOperationCompleted(object arg)
	{
		if (this.GetAllDesignsAndRelatedInfoByKewordSearchCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsAndRelatedInfoByKewordSearchCompleted(this, new GetAllDesignsAndRelatedInfoByKewordSearchCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsByPaidStatus", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void GetAllDesignsByPaidStatus(bool hasBeenPaid)
	{
		Invoke("GetAllDesignsByPaidStatus", new object[1] { hasBeenPaid });
	}

	public IAsyncResult BeginGetAllDesignsByPaidStatus(bool hasBeenPaid, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsByPaidStatus", new object[1] { hasBeenPaid }, callback, asyncState);
	}

	public void EndGetAllDesignsByPaidStatus(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void GetAllDesignsByPaidStatusAsync(bool hasBeenPaid)
	{
		GetAllDesignsByPaidStatusAsync(hasBeenPaid, null);
	}

	public void GetAllDesignsByPaidStatusAsync(bool hasBeenPaid, object userState)
	{
		if (GetAllDesignsByPaidStatusOperationCompleted == null)
		{
			GetAllDesignsByPaidStatusOperationCompleted = OnGetAllDesignsByPaidStatusOperationCompleted;
		}
		InvokeAsync("GetAllDesignsByPaidStatus", new object[1] { hasBeenPaid }, GetAllDesignsByPaidStatusOperationCompleted, userState);
	}

	private void OnGetAllDesignsByPaidStatusOperationCompleted(object arg)
	{
		if (this.GetAllDesignsByPaidStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsByPaidStatusCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsByStatusesOrderByDesignId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignsByStatusesOrderByDesignId(string statusIds)
	{
		return (DataTable)Invoke("GetAllDesignsByStatusesOrderByDesignId", new object[1] { statusIds })[0];
	}

	public IAsyncResult BeginGetAllDesignsByStatusesOrderByDesignId(string statusIds, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsByStatusesOrderByDesignId", new object[1] { statusIds }, callback, asyncState);
	}

	public DataTable EndGetAllDesignsByStatusesOrderByDesignId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsByStatusesOrderByDesignIdAsync(string statusIds)
	{
		GetAllDesignsByStatusesOrderByDesignIdAsync(statusIds, null);
	}

	public void GetAllDesignsByStatusesOrderByDesignIdAsync(string statusIds, object userState)
	{
		if (GetAllDesignsByStatusesOrderByDesignIdOperationCompleted == null)
		{
			GetAllDesignsByStatusesOrderByDesignIdOperationCompleted = OnGetAllDesignsByStatusesOrderByDesignIdOperationCompleted;
		}
		InvokeAsync("GetAllDesignsByStatusesOrderByDesignId", new object[1] { statusIds }, GetAllDesignsByStatusesOrderByDesignIdOperationCompleted, userState);
	}

	private void OnGetAllDesignsByStatusesOrderByDesignIdOperationCompleted(object arg)
	{
		if (this.GetAllDesignsByStatusesOrderByDesignIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsByStatusesOrderByDesignIdCompleted(this, new GetAllDesignsByStatusesOrderByDesignIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsByStatusesOrderByDesignIdByKewordSearch", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignsByStatusesOrderByDesignIdByKewordSearch(string statusIds, string keyword)
	{
		return (DataTable)Invoke("GetAllDesignsByStatusesOrderByDesignIdByKewordSearch", new object[2] { statusIds, keyword })[0];
	}

	public IAsyncResult BeginGetAllDesignsByStatusesOrderByDesignIdByKewordSearch(string statusIds, string keyword, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsByStatusesOrderByDesignIdByKewordSearch", new object[2] { statusIds, keyword }, callback, asyncState);
	}

	public DataTable EndGetAllDesignsByStatusesOrderByDesignIdByKewordSearch(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsByStatusesOrderByDesignIdByKewordSearchAsync(string statusIds, string keyword)
	{
		GetAllDesignsByStatusesOrderByDesignIdByKewordSearchAsync(statusIds, keyword, null);
	}

	public void GetAllDesignsByStatusesOrderByDesignIdByKewordSearchAsync(string statusIds, string keyword, object userState)
	{
		if (GetAllDesignsByStatusesOrderByDesignIdByKewordSearchOperationCompleted == null)
		{
			GetAllDesignsByStatusesOrderByDesignIdByKewordSearchOperationCompleted = OnGetAllDesignsByStatusesOrderByDesignIdByKewordSearchOperationCompleted;
		}
		InvokeAsync("GetAllDesignsByStatusesOrderByDesignIdByKewordSearch", new object[2] { statusIds, keyword }, GetAllDesignsByStatusesOrderByDesignIdByKewordSearchOperationCompleted, userState);
	}

	private void OnGetAllDesignsByStatusesOrderByDesignIdByKewordSearchOperationCompleted(object arg)
	{
		if (this.GetAllDesignsByStatusesOrderByDesignIdByKewordSearchCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsByStatusesOrderByDesignIdByKewordSearchCompleted(this, new GetAllDesignsByStatusesOrderByDesignIdByKewordSearchCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsByStatusOrderByDesignId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignsByStatusOrderByDesignId(int StatusId)
	{
		return (DataTable)Invoke("GetAllDesignsByStatusOrderByDesignId", new object[1] { StatusId })[0];
	}

	public IAsyncResult BeginGetAllDesignsByStatusOrderByDesignId(int StatusId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsByStatusOrderByDesignId", new object[1] { StatusId }, callback, asyncState);
	}

	public DataTable EndGetAllDesignsByStatusOrderByDesignId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsByStatusOrderByDesignIdAsync(int StatusId)
	{
		GetAllDesignsByStatusOrderByDesignIdAsync(StatusId, null);
	}

	public void GetAllDesignsByStatusOrderByDesignIdAsync(int StatusId, object userState)
	{
		if (GetAllDesignsByStatusOrderByDesignIdOperationCompleted == null)
		{
			GetAllDesignsByStatusOrderByDesignIdOperationCompleted = OnGetAllDesignsByStatusOrderByDesignIdOperationCompleted;
		}
		InvokeAsync("GetAllDesignsByStatusOrderByDesignId", new object[1] { StatusId }, GetAllDesignsByStatusOrderByDesignIdOperationCompleted, userState);
	}

	private void OnGetAllDesignsByStatusOrderByDesignIdOperationCompleted(object arg)
	{
		if (this.GetAllDesignsByStatusOrderByDesignIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsByStatusOrderByDesignIdCompleted(this, new GetAllDesignsByStatusOrderByDesignIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsByStatusOrderByStatusDate", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignsByStatusOrderByStatusDate(int StatusId)
	{
		return (DataTable)Invoke("GetAllDesignsByStatusOrderByStatusDate", new object[1] { StatusId })[0];
	}

	public IAsyncResult BeginGetAllDesignsByStatusOrderByStatusDate(int StatusId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsByStatusOrderByStatusDate", new object[1] { StatusId }, callback, asyncState);
	}

	public DataTable EndGetAllDesignsByStatusOrderByStatusDate(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsByStatusOrderByStatusDateAsync(int StatusId)
	{
		GetAllDesignsByStatusOrderByStatusDateAsync(StatusId, null);
	}

	public void GetAllDesignsByStatusOrderByStatusDateAsync(int StatusId, object userState)
	{
		if (GetAllDesignsByStatusOrderByStatusDateOperationCompleted == null)
		{
			GetAllDesignsByStatusOrderByStatusDateOperationCompleted = OnGetAllDesignsByStatusOrderByStatusDateOperationCompleted;
		}
		InvokeAsync("GetAllDesignsByStatusOrderByStatusDate", new object[1] { StatusId }, GetAllDesignsByStatusOrderByStatusDateOperationCompleted, userState);
	}

	private void OnGetAllDesignsByStatusOrderByStatusDateOperationCompleted(object arg)
	{
		if (this.GetAllDesignsByStatusOrderByStatusDateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsByStatusOrderByStatusDateCompleted(this, new GetAllDesignsByStatusOrderByStatusDateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignsByStatusOrderByStatusDateByKewordSearch", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignsByStatusOrderByStatusDateByKewordSearch(int StatusId, string keyword)
	{
		return (DataTable)Invoke("GetAllDesignsByStatusOrderByStatusDateByKewordSearch", new object[2] { StatusId, keyword })[0];
	}

	public IAsyncResult BeginGetAllDesignsByStatusOrderByStatusDateByKewordSearch(int StatusId, string keyword, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignsByStatusOrderByStatusDateByKewordSearch", new object[2] { StatusId, keyword }, callback, asyncState);
	}

	public DataTable EndGetAllDesignsByStatusOrderByStatusDateByKewordSearch(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignsByStatusOrderByStatusDateByKewordSearchAsync(int StatusId, string keyword)
	{
		GetAllDesignsByStatusOrderByStatusDateByKewordSearchAsync(StatusId, keyword, null);
	}

	public void GetAllDesignsByStatusOrderByStatusDateByKewordSearchAsync(int StatusId, string keyword, object userState)
	{
		if (GetAllDesignsByStatusOrderByStatusDateByKewordSearchOperationCompleted == null)
		{
			GetAllDesignsByStatusOrderByStatusDateByKewordSearchOperationCompleted = OnGetAllDesignsByStatusOrderByStatusDateByKewordSearchOperationCompleted;
		}
		InvokeAsync("GetAllDesignsByStatusOrderByStatusDateByKewordSearch", new object[2] { StatusId, keyword }, GetAllDesignsByStatusOrderByStatusDateByKewordSearchOperationCompleted, userState);
	}

	private void OnGetAllDesignsByStatusOrderByStatusDateByKewordSearchOperationCompleted(object arg)
	{
		if (this.GetAllDesignsByStatusOrderByStatusDateByKewordSearchCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignsByStatusOrderByStatusDateByKewordSearchCompleted(this, new GetAllDesignsByStatusOrderByStatusDateByKewordSearchCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllQuestionTypes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllQuestionTypes()
	{
		return (DataTable)Invoke("GetAllQuestionTypes", new object[0])[0];
	}

	public IAsyncResult BeginGetAllQuestionTypes(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllQuestionTypes", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllQuestionTypes(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllQuestionTypesAsync()
	{
		GetAllQuestionTypesAsync(null);
	}

	public void GetAllQuestionTypesAsync(object userState)
	{
		if (GetAllQuestionTypesOperationCompleted == null)
		{
			GetAllQuestionTypesOperationCompleted = OnGetAllQuestionTypesOperationCompleted;
		}
		InvokeAsync("GetAllQuestionTypes", new object[0], GetAllQuestionTypesOperationCompleted, userState);
	}

	private void OnGetAllQuestionTypesOperationCompleted(object arg)
	{
		if (this.GetAllQuestionTypesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllQuestionTypesCompleted(this, new GetAllQuestionTypesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllStullerComponentAttributeTypes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllStullerComponentAttributeTypes()
	{
		return (DataTable)Invoke("GetAllStullerComponentAttributeTypes", new object[0])[0];
	}

	public IAsyncResult BeginGetAllStullerComponentAttributeTypes(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllStullerComponentAttributeTypes", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllStullerComponentAttributeTypes(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllStullerComponentAttributeTypesAsync()
	{
		GetAllStullerComponentAttributeTypesAsync(null);
	}

	public void GetAllStullerComponentAttributeTypesAsync(object userState)
	{
		if (GetAllStullerComponentAttributeTypesOperationCompleted == null)
		{
			GetAllStullerComponentAttributeTypesOperationCompleted = OnGetAllStullerComponentAttributeTypesOperationCompleted;
		}
		InvokeAsync("GetAllStullerComponentAttributeTypes", new object[0], GetAllStullerComponentAttributeTypesOperationCompleted, userState);
	}

	private void OnGetAllStullerComponentAttributeTypesOperationCompleted(object arg)
	{
		if (this.GetAllStullerComponentAttributeTypesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllStullerComponentAttributeTypesCompleted(this, new GetAllStullerComponentAttributeTypesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAssignmentGroupById", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAssignmentGroupById(int assignmentGroupId)
	{
		return (DataTable)Invoke("GetAssignmentGroupById", new object[1] { assignmentGroupId })[0];
	}

	public IAsyncResult BeginGetAssignmentGroupById(int assignmentGroupId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAssignmentGroupById", new object[1] { assignmentGroupId }, callback, asyncState);
	}

	public DataTable EndGetAssignmentGroupById(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAssignmentGroupByIdAsync(int assignmentGroupId)
	{
		GetAssignmentGroupByIdAsync(assignmentGroupId, null);
	}

	public void GetAssignmentGroupByIdAsync(int assignmentGroupId, object userState)
	{
		if (GetAssignmentGroupByIdOperationCompleted == null)
		{
			GetAssignmentGroupByIdOperationCompleted = OnGetAssignmentGroupByIdOperationCompleted;
		}
		InvokeAsync("GetAssignmentGroupById", new object[1] { assignmentGroupId }, GetAssignmentGroupByIdOperationCompleted, userState);
	}

	private void OnGetAssignmentGroupByIdOperationCompleted(object arg)
	{
		if (this.GetAssignmentGroupByIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAssignmentGroupByIdCompleted(this, new GetAssignmentGroupByIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignMetalSpecificationByDesignComponentId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignMetalSpecificationByDesignComponentId(int DesignComponentId)
	{
		return (DataTable)Invoke("GetDesignMetalSpecificationByDesignComponentId", new object[1] { DesignComponentId })[0];
	}

	public IAsyncResult BeginGetDesignMetalSpecificationByDesignComponentId(int DesignComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignMetalSpecificationByDesignComponentId", new object[1] { DesignComponentId }, callback, asyncState);
	}

	public DataTable EndGetDesignMetalSpecificationByDesignComponentId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignMetalSpecificationByDesignComponentIdAsync(int DesignComponentId)
	{
		GetDesignMetalSpecificationByDesignComponentIdAsync(DesignComponentId, null);
	}

	public void GetDesignMetalSpecificationByDesignComponentIdAsync(int DesignComponentId, object userState)
	{
		if (GetDesignMetalSpecificationByDesignComponentIdOperationCompleted == null)
		{
			GetDesignMetalSpecificationByDesignComponentIdOperationCompleted = OnGetDesignMetalSpecificationByDesignComponentIdOperationCompleted;
		}
		InvokeAsync("GetDesignMetalSpecificationByDesignComponentId", new object[1] { DesignComponentId }, GetDesignMetalSpecificationByDesignComponentIdOperationCompleted, userState);
	}

	private void OnGetDesignMetalSpecificationByDesignComponentIdOperationCompleted(object arg)
	{
		if (this.GetDesignMetalSpecificationByDesignComponentIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignMetalSpecificationByDesignComponentIdCompleted(this, new GetDesignMetalSpecificationByDesignComponentIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignRingRailSpecificationByDesignId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignRingRailSpecificationByDesignId(int designId)
	{
		return (DataTable)Invoke("GetDesignRingRailSpecificationByDesignId", new object[1] { designId })[0];
	}

	public IAsyncResult BeginGetDesignRingRailSpecificationByDesignId(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignRingRailSpecificationByDesignId", new object[1] { designId }, callback, asyncState);
	}

	public DataTable EndGetDesignRingRailSpecificationByDesignId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignRingRailSpecificationByDesignIdAsync(int designId)
	{
		GetDesignRingRailSpecificationByDesignIdAsync(designId, null);
	}

	public void GetDesignRingRailSpecificationByDesignIdAsync(int designId, object userState)
	{
		if (GetDesignRingRailSpecificationByDesignIdOperationCompleted == null)
		{
			GetDesignRingRailSpecificationByDesignIdOperationCompleted = OnGetDesignRingRailSpecificationByDesignIdOperationCompleted;
		}
		InvokeAsync("GetDesignRingRailSpecificationByDesignId", new object[1] { designId }, GetDesignRingRailSpecificationByDesignIdOperationCompleted, userState);
	}

	private void OnGetDesignRingRailSpecificationByDesignIdOperationCompleted(object arg)
	{
		if (this.GetDesignRingRailSpecificationByDesignIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignRingRailSpecificationByDesignIdCompleted(this, new GetDesignRingRailSpecificationByDesignIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetKeyCodeByKeyId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetKeyCodeByKeyId(int keyCodeId)
	{
		return (DataTable)Invoke("GetKeyCodeByKeyId", new object[1] { keyCodeId })[0];
	}

	public IAsyncResult BeginGetKeyCodeByKeyId(int keyCodeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetKeyCodeByKeyId", new object[1] { keyCodeId }, callback, asyncState);
	}

	public DataTable EndGetKeyCodeByKeyId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetKeyCodeByKeyIdAsync(int keyCodeId)
	{
		GetKeyCodeByKeyIdAsync(keyCodeId, null);
	}

	public void GetKeyCodeByKeyIdAsync(int keyCodeId, object userState)
	{
		if (GetKeyCodeByKeyIdOperationCompleted == null)
		{
			GetKeyCodeByKeyIdOperationCompleted = OnGetKeyCodeByKeyIdOperationCompleted;
		}
		InvokeAsync("GetKeyCodeByKeyId", new object[1] { keyCodeId }, GetKeyCodeByKeyIdOperationCompleted, userState);
	}

	private void OnGetKeyCodeByKeyIdOperationCompleted(object arg)
	{
		if (this.GetKeyCodeByKeyIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetKeyCodeByKeyIdCompleted(this, new GetKeyCodeByKeyIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetUserTypes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetUserTypes()
	{
		return (DataTable)Invoke("GetUserTypes", new object[0])[0];
	}

	public IAsyncResult BeginGetUserTypes(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetUserTypes", new object[0], callback, asyncState);
	}

	public DataTable EndGetUserTypes(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetUserTypesAsync()
	{
		GetUserTypesAsync(null);
	}

	public void GetUserTypesAsync(object userState)
	{
		if (GetUserTypesOperationCompleted == null)
		{
			GetUserTypesOperationCompleted = OnGetUserTypesOperationCompleted;
		}
		InvokeAsync("GetUserTypes", new object[0], GetUserTypesOperationCompleted, userState);
	}

	private void OnGetUserTypesOperationCompleted(object arg)
	{
		if (this.GetUserTypesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetUserTypesCompleted(this, new GetUserTypesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateUserTypeOnKeyCode", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateUserTypeOnKeyCode(int keyId, int userTypeId)
	{
		Invoke("UpdateUserTypeOnKeyCode", new object[2] { keyId, userTypeId });
	}

	public IAsyncResult BeginUpdateUserTypeOnKeyCode(int keyId, int userTypeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateUserTypeOnKeyCode", new object[2] { keyId, userTypeId }, callback, asyncState);
	}

	public void EndUpdateUserTypeOnKeyCode(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateUserTypeOnKeyCodeAsync(int keyId, int userTypeId)
	{
		UpdateUserTypeOnKeyCodeAsync(keyId, userTypeId, null);
	}

	public void UpdateUserTypeOnKeyCodeAsync(int keyId, int userTypeId, object userState)
	{
		if (UpdateUserTypeOnKeyCodeOperationCompleted == null)
		{
			UpdateUserTypeOnKeyCodeOperationCompleted = OnUpdateUserTypeOnKeyCodeOperationCompleted;
		}
		InvokeAsync("UpdateUserTypeOnKeyCode", new object[2] { keyId, userTypeId }, UpdateUserTypeOnKeyCodeOperationCompleted, userState);
	}

	private void OnUpdateUserTypeOnKeyCodeOperationCompleted(object arg)
	{
		if (this.UpdateUserTypeOnKeyCodeCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateUserTypeOnKeyCodeCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetLatestCounterSketchVersionString", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetLatestCounterSketchVersionString()
	{
		return (string)Invoke("GetLatestCounterSketchVersionString", new object[0])[0];
	}

	public IAsyncResult BeginGetLatestCounterSketchVersionString(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetLatestCounterSketchVersionString", new object[0], callback, asyncState);
	}

	public string EndGetLatestCounterSketchVersionString(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetLatestCounterSketchVersionStringAsync()
	{
		GetLatestCounterSketchVersionStringAsync(null);
	}

	public void GetLatestCounterSketchVersionStringAsync(object userState)
	{
		if (GetLatestCounterSketchVersionStringOperationCompleted == null)
		{
			GetLatestCounterSketchVersionStringOperationCompleted = OnGetLatestCounterSketchVersionStringOperationCompleted;
		}
		InvokeAsync("GetLatestCounterSketchVersionString", new object[0], GetLatestCounterSketchVersionStringOperationCompleted, userState);
	}

	private void OnGetLatestCounterSketchVersionStringOperationCompleted(object arg)
	{
		if (this.GetLatestCounterSketchVersionStringCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetLatestCounterSketchVersionStringCompleted(this, new GetLatestCounterSketchVersionStringCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetRequiredLibraryFiles", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string[] GetRequiredLibraryFiles(string CSVersionString)
	{
		return (string[])Invoke("GetRequiredLibraryFiles", new object[1] { CSVersionString })[0];
	}

	public IAsyncResult BeginGetRequiredLibraryFiles(string CSVersionString, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetRequiredLibraryFiles", new object[1] { CSVersionString }, callback, asyncState);
	}

	public string[] EndGetRequiredLibraryFiles(IAsyncResult asyncResult)
	{
		return (string[])EndInvoke(asyncResult)[0];
	}

	public void GetRequiredLibraryFilesAsync(string CSVersionString)
	{
		GetRequiredLibraryFilesAsync(CSVersionString, null);
	}

	public void GetRequiredLibraryFilesAsync(string CSVersionString, object userState)
	{
		if (GetRequiredLibraryFilesOperationCompleted == null)
		{
			GetRequiredLibraryFilesOperationCompleted = OnGetRequiredLibraryFilesOperationCompleted;
		}
		InvokeAsync("GetRequiredLibraryFiles", new object[1] { CSVersionString }, GetRequiredLibraryFilesOperationCompleted, userState);
	}

	private void OnGetRequiredLibraryFilesOperationCompleted(object arg)
	{
		if (this.GetRequiredLibraryFilesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetRequiredLibraryFilesCompleted(this, new GetRequiredLibraryFilesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetServiceExceptionHistory", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetServiceExceptionHistory()
	{
		return (string)Invoke("GetServiceExceptionHistory", new object[0])[0];
	}

	public IAsyncResult BeginGetServiceExceptionHistory(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetServiceExceptionHistory", new object[0], callback, asyncState);
	}

	public string EndGetServiceExceptionHistory(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetServiceExceptionHistoryAsync()
	{
		GetServiceExceptionHistoryAsync(null);
	}

	public void GetServiceExceptionHistoryAsync(object userState)
	{
		if (GetServiceExceptionHistoryOperationCompleted == null)
		{
			GetServiceExceptionHistoryOperationCompleted = OnGetServiceExceptionHistoryOperationCompleted;
		}
		InvokeAsync("GetServiceExceptionHistory", new object[0], GetServiceExceptionHistoryOperationCompleted, userState);
	}

	private void OnGetServiceExceptionHistoryOperationCompleted(object arg)
	{
		if (this.GetServiceExceptionHistoryCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetServiceExceptionHistoryCompleted(this, new GetServiceExceptionHistoryCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/HasActiveSurveys", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool HasActiveSurveys()
	{
		return (bool)Invoke("HasActiveSurveys", new object[0])[0];
	}

	public IAsyncResult BeginHasActiveSurveys(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("HasActiveSurveys", new object[0], callback, asyncState);
	}

	public bool EndHasActiveSurveys(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void HasActiveSurveysAsync()
	{
		HasActiveSurveysAsync(null);
	}

	public void HasActiveSurveysAsync(object userState)
	{
		if (HasActiveSurveysOperationCompleted == null)
		{
			HasActiveSurveysOperationCompleted = OnHasActiveSurveysOperationCompleted;
		}
		InvokeAsync("HasActiveSurveys", new object[0], HasActiveSurveysOperationCompleted, userState);
	}

	private void OnHasActiveSurveysOperationCompleted(object arg)
	{
		if (this.HasActiveSurveysCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.HasActiveSurveysCompleted(this, new HasActiveSurveysCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateAssignmentOnDesigner", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateAssignmentOnDesigner(int designerAssignmentId, int designerId, int assignmentId)
	{
		Invoke("UpdateAssignmentOnDesigner", new object[3] { designerAssignmentId, designerId, assignmentId });
	}

	public IAsyncResult BeginUpdateAssignmentOnDesigner(int designerAssignmentId, int designerId, int assignmentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateAssignmentOnDesigner", new object[3] { designerAssignmentId, designerId, assignmentId }, callback, asyncState);
	}

	public void EndUpdateAssignmentOnDesigner(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateAssignmentOnDesignerAsync(int designerAssignmentId, int designerId, int assignmentId)
	{
		UpdateAssignmentOnDesignerAsync(designerAssignmentId, designerId, assignmentId, null);
	}

	public void UpdateAssignmentOnDesignerAsync(int designerAssignmentId, int designerId, int assignmentId, object userState)
	{
		if (UpdateAssignmentOnDesignerOperationCompleted == null)
		{
			UpdateAssignmentOnDesignerOperationCompleted = OnUpdateAssignmentOnDesignerOperationCompleted;
		}
		InvokeAsync("UpdateAssignmentOnDesigner", new object[3] { designerAssignmentId, designerId, assignmentId }, UpdateAssignmentOnDesignerOperationCompleted, userState);
	}

	private void OnUpdateAssignmentOnDesignerOperationCompleted(object arg)
	{
		if (this.UpdateAssignmentOnDesignerCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateAssignmentOnDesignerCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateDesignPaidStatus", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateDesignPaidStatus(int designId, bool hasBeenPaid)
	{
		Invoke("UpdateDesignPaidStatus", new object[2] { designId, hasBeenPaid });
	}

	public IAsyncResult BeginUpdateDesignPaidStatus(int designId, bool hasBeenPaid, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateDesignPaidStatus", new object[2] { designId, hasBeenPaid }, callback, asyncState);
	}

	public void EndUpdateDesignPaidStatus(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateDesignPaidStatusAsync(int designId, bool hasBeenPaid)
	{
		UpdateDesignPaidStatusAsync(designId, hasBeenPaid, null);
	}

	public void UpdateDesignPaidStatusAsync(int designId, bool hasBeenPaid, object userState)
	{
		if (UpdateDesignPaidStatusOperationCompleted == null)
		{
			UpdateDesignPaidStatusOperationCompleted = OnUpdateDesignPaidStatusOperationCompleted;
		}
		InvokeAsync("UpdateDesignPaidStatus", new object[2] { designId, hasBeenPaid }, UpdateDesignPaidStatusOperationCompleted, userState);
	}

	private void OnUpdateDesignPaidStatusOperationCompleted(object arg)
	{
		if (this.UpdateDesignPaidStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateDesignPaidStatusCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateKeyInformation", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateKeyInformation(int keyId, string accountNo, string address, string city, string state, string zip, double latitude, double longitude)
	{
		Invoke("UpdateKeyInformation", new object[8] { keyId, accountNo, address, city, state, zip, latitude, longitude });
	}

	public IAsyncResult BeginUpdateKeyInformation(int keyId, string accountNo, string address, string city, string state, string zip, double latitude, double longitude, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateKeyInformation", new object[8] { keyId, accountNo, address, city, state, zip, latitude, longitude }, callback, asyncState);
	}

	public void EndUpdateKeyInformation(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateKeyInformationAsync(int keyId, string accountNo, string address, string city, string state, string zip, double latitude, double longitude)
	{
		UpdateKeyInformationAsync(keyId, accountNo, address, city, state, zip, latitude, longitude, null);
	}

	public void UpdateKeyInformationAsync(int keyId, string accountNo, string address, string city, string state, string zip, double latitude, double longitude, object userState)
	{
		if (UpdateKeyInformationOperationCompleted == null)
		{
			UpdateKeyInformationOperationCompleted = OnUpdateKeyInformationOperationCompleted;
		}
		InvokeAsync("UpdateKeyInformation", new object[8] { keyId, accountNo, address, city, state, zip, latitude, longitude }, UpdateKeyInformationOperationCompleted, userState);
	}

	private void OnUpdateKeyInformationOperationCompleted(object arg)
	{
		if (this.UpdateKeyInformationCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateKeyInformationCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/ValidateDesignerInfo", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool ValidateDesignerInfo(string guid, string designerFirstName, string designerLastName, string company)
	{
		return (bool)Invoke("ValidateDesignerInfo", new object[4] { guid, designerFirstName, designerLastName, company })[0];
	}

	public IAsyncResult BeginValidateDesignerInfo(string guid, string designerFirstName, string designerLastName, string company, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("ValidateDesignerInfo", new object[4] { guid, designerFirstName, designerLastName, company }, callback, asyncState);
	}

	public bool EndValidateDesignerInfo(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void ValidateDesignerInfoAsync(string guid, string designerFirstName, string designerLastName, string company)
	{
		ValidateDesignerInfoAsync(guid, designerFirstName, designerLastName, company, null);
	}

	public void ValidateDesignerInfoAsync(string guid, string designerFirstName, string designerLastName, string company, object userState)
	{
		if (ValidateDesignerInfoOperationCompleted == null)
		{
			ValidateDesignerInfoOperationCompleted = OnValidateDesignerInfoOperationCompleted;
		}
		InvokeAsync("ValidateDesignerInfo", new object[4] { guid, designerFirstName, designerLastName, company }, ValidateDesignerInfoOperationCompleted, userState);
	}

	private void OnValidateDesignerInfoOperationCompleted(object arg)
	{
		if (this.ValidateDesignerInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.ValidateDesignerInfoCompleted(this, new ValidateDesignerInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignComponentCountByDesignId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetDesignComponentCountByDesignId(int designId)
	{
		return (int)Invoke("GetDesignComponentCountByDesignId", new object[1] { designId })[0];
	}

	public IAsyncResult BeginGetDesignComponentCountByDesignId(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignComponentCountByDesignId", new object[1] { designId }, callback, asyncState);
	}

	public int EndGetDesignComponentCountByDesignId(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetDesignComponentCountByDesignIdAsync(int designId)
	{
		GetDesignComponentCountByDesignIdAsync(designId, null);
	}

	public void GetDesignComponentCountByDesignIdAsync(int designId, object userState)
	{
		if (GetDesignComponentCountByDesignIdOperationCompleted == null)
		{
			GetDesignComponentCountByDesignIdOperationCompleted = OnGetDesignComponentCountByDesignIdOperationCompleted;
		}
		InvokeAsync("GetDesignComponentCountByDesignId", new object[1] { designId }, GetDesignComponentCountByDesignIdOperationCompleted, userState);
	}

	private void OnGetDesignComponentCountByDesignIdOperationCompleted(object arg)
	{
		if (this.GetDesignComponentCountByDesignIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignComponentCountByDesignIdCompleted(this, new GetDesignComponentCountByDesignIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignComponentAttributesByDesignComponentId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignComponentAttributesByDesignComponentId(int designComponentId)
	{
		return (DataTable)Invoke("GetDesignComponentAttributesByDesignComponentId", new object[1] { designComponentId })[0];
	}

	public IAsyncResult BeginGetDesignComponentAttributesByDesignComponentId(int designComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignComponentAttributesByDesignComponentId", new object[1] { designComponentId }, callback, asyncState);
	}

	public DataTable EndGetDesignComponentAttributesByDesignComponentId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignComponentAttributesByDesignComponentIdAsync(int designComponentId)
	{
		GetDesignComponentAttributesByDesignComponentIdAsync(designComponentId, null);
	}

	public void GetDesignComponentAttributesByDesignComponentIdAsync(int designComponentId, object userState)
	{
		if (GetDesignComponentAttributesByDesignComponentIdOperationCompleted == null)
		{
			GetDesignComponentAttributesByDesignComponentIdOperationCompleted = OnGetDesignComponentAttributesByDesignComponentIdOperationCompleted;
		}
		InvokeAsync("GetDesignComponentAttributesByDesignComponentId", new object[1] { designComponentId }, GetDesignComponentAttributesByDesignComponentIdOperationCompleted, userState);
	}

	private void OnGetDesignComponentAttributesByDesignComponentIdOperationCompleted(object arg)
	{
		if (this.GetDesignComponentAttributesByDesignComponentIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignComponentAttributesByDesignComponentIdCompleted(this, new GetDesignComponentAttributesByDesignComponentIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetLastNoticeId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetLastNoticeId()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	public IAsyncResult BeginGetLastNoticeId(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetLastNoticeId", new object[0], callback, asyncState);
	}

	public int EndGetLastNoticeId(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetLastNoticeIdAsync()
	{
		GetLastNoticeIdAsync(null);
	}

	public void GetLastNoticeIdAsync(object userState)
	{
		if (GetLastNoticeIdOperationCompleted == null)
		{
			GetLastNoticeIdOperationCompleted = OnGetLastNoticeIdOperationCompleted;
		}
		InvokeAsync("GetLastNoticeId", new object[0], GetLastNoticeIdOperationCompleted, userState);
	}

	private void OnGetLastNoticeIdOperationCompleted(object arg)
	{
		if (this.GetLastNoticeIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetLastNoticeIdCompleted(this, new GetLastNoticeIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetNoticeTypeByNoticeId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetNoticeTypeByNoticeId(int id)
	{
		return (int)Invoke("GetNoticeTypeByNoticeId", new object[1] { id })[0];
	}

	public IAsyncResult BeginGetNoticeTypeByNoticeId(int id, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetNoticeTypeByNoticeId", new object[1] { id }, callback, asyncState);
	}

	public int EndGetNoticeTypeByNoticeId(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetNoticeTypeByNoticeIdAsync(int id)
	{
		GetNoticeTypeByNoticeIdAsync(id, null);
	}

	public void GetNoticeTypeByNoticeIdAsync(int id, object userState)
	{
		if (GetNoticeTypeByNoticeIdOperationCompleted == null)
		{
			GetNoticeTypeByNoticeIdOperationCompleted = OnGetNoticeTypeByNoticeIdOperationCompleted;
		}
		InvokeAsync("GetNoticeTypeByNoticeId", new object[1] { id }, GetNoticeTypeByNoticeIdOperationCompleted, userState);
	}

	private void OnGetNoticeTypeByNoticeIdOperationCompleted(object arg)
	{
		if (this.GetNoticeTypeByNoticeIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetNoticeTypeByNoticeIdCompleted(this, new GetNoticeTypeByNoticeIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetLatestNoticeByKeyId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetLatestNoticeByKeyId(int keyId)
	{
		return (int)Invoke("GetLatestNoticeByKeyId", new object[1] { keyId })[0];
	}

	public IAsyncResult BeginGetLatestNoticeByKeyId(int keyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetLatestNoticeByKeyId", new object[1] { keyId }, callback, asyncState);
	}

	public int EndGetLatestNoticeByKeyId(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void GetLatestNoticeByKeyIdAsync(int keyId)
	{
		GetLatestNoticeByKeyIdAsync(keyId, null);
	}

	public void GetLatestNoticeByKeyIdAsync(int keyId, object userState)
	{
		if (GetLatestNoticeByKeyIdOperationCompleted == null)
		{
			GetLatestNoticeByKeyIdOperationCompleted = OnGetLatestNoticeByKeyIdOperationCompleted;
		}
		InvokeAsync("GetLatestNoticeByKeyId", new object[1] { keyId }, GetLatestNoticeByKeyIdOperationCompleted, userState);
	}

	private void OnGetLatestNoticeByKeyIdOperationCompleted(object arg)
	{
		if (this.GetLatestNoticeByKeyIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetLatestNoticeByKeyIdCompleted(this, new GetLatestNoticeByKeyIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendApplicationEvents", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SendApplicationEvents(ApplicationEvent[] applicationEvents)
	{
		Invoke("SendApplicationEvents", new object[1] { applicationEvents });
	}

	public IAsyncResult BeginSendApplicationEvents(ApplicationEvent[] applicationEvents, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendApplicationEvents", new object[1] { applicationEvents }, callback, asyncState);
	}

	public void EndSendApplicationEvents(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SendApplicationEventsAsync(ApplicationEvent[] applicationEvents)
	{
		SendApplicationEventsAsync(applicationEvents, null);
	}

	public void SendApplicationEventsAsync(ApplicationEvent[] applicationEvents, object userState)
	{
		if (SendApplicationEventsOperationCompleted == null)
		{
			SendApplicationEventsOperationCompleted = OnSendApplicationEventsOperationCompleted;
		}
		InvokeAsync("SendApplicationEvents", new object[1] { applicationEvents }, SendApplicationEventsOperationCompleted, userState);
	}

	private void OnSendApplicationEventsOperationCompleted(object arg)
	{
		if (this.SendApplicationEventsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendApplicationEventsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendImmediateEvent", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SendImmediateEvent(int keyId, ApplicationEvent applicationEvent)
	{
		Invoke("SendImmediateEvent", new object[2] { keyId, applicationEvent });
	}

	public IAsyncResult BeginSendImmediateEvent(int keyId, ApplicationEvent applicationEvent, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendImmediateEvent", new object[2] { keyId, applicationEvent }, callback, asyncState);
	}

	public void EndSendImmediateEvent(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SendImmediateEventAsync(int keyId, ApplicationEvent applicationEvent)
	{
		SendImmediateEventAsync(keyId, applicationEvent, null);
	}

	public void SendImmediateEventAsync(int keyId, ApplicationEvent applicationEvent, object userState)
	{
		if (SendImmediateEventOperationCompleted == null)
		{
			SendImmediateEventOperationCompleted = OnSendImmediateEventOperationCompleted;
		}
		InvokeAsync("SendImmediateEvent", new object[2] { keyId, applicationEvent }, SendImmediateEventOperationCompleted, userState);
	}

	private void OnSendImmediateEventOperationCompleted(object arg)
	{
		if (this.SendImmediateEventCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendImmediateEventCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendInstallationSpecificApplicationEvents", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SendInstallationSpecificApplicationEvents(int keyId, ApplicationEvent[] applicationEvents)
	{
		Invoke("SendInstallationSpecificApplicationEvents", new object[2] { keyId, applicationEvents });
	}

	public IAsyncResult BeginSendInstallationSpecificApplicationEvents(int keyId, ApplicationEvent[] applicationEvents, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendInstallationSpecificApplicationEvents", new object[2] { keyId, applicationEvents }, callback, asyncState);
	}

	public void EndSendInstallationSpecificApplicationEvents(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SendInstallationSpecificApplicationEventsAsync(int keyId, ApplicationEvent[] applicationEvents)
	{
		SendInstallationSpecificApplicationEventsAsync(keyId, applicationEvents, null);
	}

	public void SendInstallationSpecificApplicationEventsAsync(int keyId, ApplicationEvent[] applicationEvents, object userState)
	{
		if (SendInstallationSpecificApplicationEventsOperationCompleted == null)
		{
			SendInstallationSpecificApplicationEventsOperationCompleted = OnSendInstallationSpecificApplicationEventsOperationCompleted;
		}
		InvokeAsync("SendInstallationSpecificApplicationEvents", new object[2] { keyId, applicationEvents }, SendInstallationSpecificApplicationEventsOperationCompleted, userState);
	}

	private void OnSendInstallationSpecificApplicationEventsOperationCompleted(object arg)
	{
		if (this.SendInstallationSpecificApplicationEventsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendInstallationSpecificApplicationEventsCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendMailFromUser", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool SendMailFromUser(string to, string from, string cc, string subject, string message, bool sendAsHtml)
	{
		return (bool)Invoke("SendMailFromUser", new object[6] { to, from, cc, subject, message, sendAsHtml })[0];
	}

	public IAsyncResult BeginSendMailFromUser(string to, string from, string cc, string subject, string message, bool sendAsHtml, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendMailFromUser", new object[6] { to, from, cc, subject, message, sendAsHtml }, callback, asyncState);
	}

	public bool EndSendMailFromUser(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void SendMailFromUserAsync(string to, string from, string cc, string subject, string message, bool sendAsHtml)
	{
		SendMailFromUserAsync(to, from, cc, subject, message, sendAsHtml, null);
	}

	public void SendMailFromUserAsync(string to, string from, string cc, string subject, string message, bool sendAsHtml, object userState)
	{
		if (SendMailFromUserOperationCompleted == null)
		{
			SendMailFromUserOperationCompleted = OnSendMailFromUserOperationCompleted;
		}
		InvokeAsync("SendMailFromUser", new object[6] { to, from, cc, subject, message, sendAsHtml }, SendMailFromUserOperationCompleted, userState);
	}

	private void OnSendMailFromUserOperationCompleted(object arg)
	{
		if (this.SendMailFromUserCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendMailFromUserCompleted(this, new SendMailFromUserCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendMail", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool SendMail(string to, string from, string cc, string subject, string message, bool sendAsHtml)
	{
		return (bool)Invoke("SendMail", new object[6] { to, from, cc, subject, message, sendAsHtml })[0];
	}

	public IAsyncResult BeginSendMail(string to, string from, string cc, string subject, string message, bool sendAsHtml, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendMail", new object[6] { to, from, cc, subject, message, sendAsHtml }, callback, asyncState);
	}

	public bool EndSendMail(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void SendMailAsync(string to, string from, string cc, string subject, string message, bool sendAsHtml)
	{
		SendMailAsync(to, from, cc, subject, message, sendAsHtml, null);
	}

	public void SendMailAsync(string to, string from, string cc, string subject, string message, bool sendAsHtml, object userState)
	{
		if (SendMailOperationCompleted == null)
		{
			SendMailOperationCompleted = OnSendMailOperationCompleted;
		}
		InvokeAsync("SendMail", new object[6] { to, from, cc, subject, message, sendAsHtml }, SendMailOperationCompleted, userState);
	}

	private void OnSendMailOperationCompleted(object arg)
	{
		if (this.SendMailCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendMailCompleted(this, new SendMailCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetExchangeRate", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public float GetExchangeRate(string countryCode, out string cultureCode)
	{
		object[] array = Invoke("GetExchangeRate", new object[1] { countryCode });
		cultureCode = (string)array[1];
		return (float)array[0];
	}

	public IAsyncResult BeginGetExchangeRate(string countryCode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetExchangeRate", new object[1] { countryCode }, callback, asyncState);
	}

	public float EndGetExchangeRate(IAsyncResult asyncResult, out string cultureCode)
	{
		object[] array = EndInvoke(asyncResult);
		cultureCode = (string)array[1];
		return (float)array[0];
	}

	public void GetExchangeRateAsync(string countryCode)
	{
		GetExchangeRateAsync(countryCode, null);
	}

	public void GetExchangeRateAsync(string countryCode, object userState)
	{
		if (GetExchangeRateOperationCompleted == null)
		{
			GetExchangeRateOperationCompleted = OnGetExchangeRateOperationCompleted;
		}
		InvokeAsync("GetExchangeRate", new object[1] { countryCode }, GetExchangeRateOperationCompleted, userState);
	}

	private void OnGetExchangeRateOperationCompleted(object arg)
	{
		if (this.GetExchangeRateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetExchangeRateCompleted(this, new GetExchangeRateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetQuickEstimate", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetQuickEstimate(int keyId, string company, string accountNumber, int designId, string SCSNumber, string stullerToken, CounterSketchCost counterSketchCost, double markupGems, double markupMountings, double markupServices, double userDefinedMarkup1, double userDefinedMarkup2, double userDefinedMarkup3, double taxPercentage, double[] pMetalWeight, ref double[] pTotalCost, ref double[] pCostMounting)
	{
		object[] array = Invoke("GetQuickEstimate", new object[17]
		{
			keyId, company, accountNumber, designId, SCSNumber, stullerToken, counterSketchCost, markupGems, markupMountings, markupServices,
			userDefinedMarkup1, userDefinedMarkup2, userDefinedMarkup3, taxPercentage, pMetalWeight, pTotalCost, pCostMounting
		});
		pTotalCost = (double[])array[1];
		pCostMounting = (double[])array[2];
		return (string)array[0];
	}

	public IAsyncResult BeginGetQuickEstimate(int keyId, string company, string accountNumber, int designId, string SCSNumber, string stullerToken, CounterSketchCost counterSketchCost, double markupGems, double markupMountings, double markupServices, double userDefinedMarkup1, double userDefinedMarkup2, double userDefinedMarkup3, double taxPercentage, double[] pMetalWeight, double[] pTotalCost, double[] pCostMounting, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetQuickEstimate", new object[17]
		{
			keyId, company, accountNumber, designId, SCSNumber, stullerToken, counterSketchCost, markupGems, markupMountings, markupServices,
			userDefinedMarkup1, userDefinedMarkup2, userDefinedMarkup3, taxPercentage, pMetalWeight, pTotalCost, pCostMounting
		}, callback, asyncState);
	}

	public string EndGetQuickEstimate(IAsyncResult asyncResult, out double[] pTotalCost, out double[] pCostMounting)
	{
		object[] array = EndInvoke(asyncResult);
		pTotalCost = (double[])array[1];
		pCostMounting = (double[])array[2];
		return (string)array[0];
	}

	public void GetQuickEstimateAsync(int keyId, string company, string accountNumber, int designId, string SCSNumber, string stullerToken, CounterSketchCost counterSketchCost, double markupGems, double markupMountings, double markupServices, double userDefinedMarkup1, double userDefinedMarkup2, double userDefinedMarkup3, double taxPercentage, double[] pMetalWeight, double[] pTotalCost, double[] pCostMounting)
	{
		GetQuickEstimateAsync(keyId, company, accountNumber, designId, SCSNumber, stullerToken, counterSketchCost, markupGems, markupMountings, markupServices, userDefinedMarkup1, userDefinedMarkup2, userDefinedMarkup3, taxPercentage, pMetalWeight, pTotalCost, pCostMounting, null);
	}

	public void GetQuickEstimateAsync(int keyId, string company, string accountNumber, int designId, string SCSNumber, string stullerToken, CounterSketchCost counterSketchCost, double markupGems, double markupMountings, double markupServices, double userDefinedMarkup1, double userDefinedMarkup2, double userDefinedMarkup3, double taxPercentage, double[] pMetalWeight, double[] pTotalCost, double[] pCostMounting, object userState)
	{
		if (GetQuickEstimateOperationCompleted == null)
		{
			GetQuickEstimateOperationCompleted = OnGetQuickEstimateOperationCompleted;
		}
		InvokeAsync("GetQuickEstimate", new object[17]
		{
			keyId, company, accountNumber, designId, SCSNumber, stullerToken, counterSketchCost, markupGems, markupMountings, markupServices,
			userDefinedMarkup1, userDefinedMarkup2, userDefinedMarkup3, taxPercentage, pMetalWeight, pTotalCost, pCostMounting
		}, GetQuickEstimateOperationCompleted, userState);
	}

	private void OnGetQuickEstimateOperationCompleted(object arg)
	{
		if (this.GetQuickEstimateCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetQuickEstimateCompleted(this, new GetQuickEstimateCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetQuickEstimateWeightCorrectionsByDesignId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public double[] GetQuickEstimateWeightCorrectionsByDesignId(int designId)
	{
		return (double[])Invoke("GetQuickEstimateWeightCorrectionsByDesignId", new object[1] { designId })[0];
	}

	public IAsyncResult BeginGetQuickEstimateWeightCorrectionsByDesignId(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetQuickEstimateWeightCorrectionsByDesignId", new object[1] { designId }, callback, asyncState);
	}

	public double[] EndGetQuickEstimateWeightCorrectionsByDesignId(IAsyncResult asyncResult)
	{
		return (double[])EndInvoke(asyncResult)[0];
	}

	public void GetQuickEstimateWeightCorrectionsByDesignIdAsync(int designId)
	{
		GetQuickEstimateWeightCorrectionsByDesignIdAsync(designId, null);
	}

	public void GetQuickEstimateWeightCorrectionsByDesignIdAsync(int designId, object userState)
	{
		if (GetQuickEstimateWeightCorrectionsByDesignIdOperationCompleted == null)
		{
			GetQuickEstimateWeightCorrectionsByDesignIdOperationCompleted = OnGetQuickEstimateWeightCorrectionsByDesignIdOperationCompleted;
		}
		InvokeAsync("GetQuickEstimateWeightCorrectionsByDesignId", new object[1] { designId }, GetQuickEstimateWeightCorrectionsByDesignIdOperationCompleted, userState);
	}

	private void OnGetQuickEstimateWeightCorrectionsByDesignIdOperationCompleted(object arg)
	{
		if (this.GetQuickEstimateWeightCorrectionsByDesignIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetQuickEstimateWeightCorrectionsByDesignIdCompleted(this, new GetQuickEstimateWeightCorrectionsByDesignIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetQuotesByKeyId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public QuoteStatusResult[] GetQuotesByKeyId(int keyId)
	{
		return (QuoteStatusResult[])Invoke("GetQuotesByKeyId", new object[1] { keyId })[0];
	}

	public IAsyncResult BeginGetQuotesByKeyId(int keyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetQuotesByKeyId", new object[1] { keyId }, callback, asyncState);
	}

	public QuoteStatusResult[] EndGetQuotesByKeyId(IAsyncResult asyncResult)
	{
		return (QuoteStatusResult[])EndInvoke(asyncResult)[0];
	}

	public void GetQuotesByKeyIdAsync(int keyId)
	{
		GetQuotesByKeyIdAsync(keyId, null);
	}

	public void GetQuotesByKeyIdAsync(int keyId, object userState)
	{
		if (GetQuotesByKeyIdOperationCompleted == null)
		{
			GetQuotesByKeyIdOperationCompleted = OnGetQuotesByKeyIdOperationCompleted;
		}
		InvokeAsync("GetQuotesByKeyId", new object[1] { keyId }, GetQuotesByKeyIdOperationCompleted, userState);
	}

	private void OnGetQuotesByKeyIdOperationCompleted(object arg)
	{
		if (this.GetQuotesByKeyIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetQuotesByKeyIdCompleted(this, new GetQuotesByKeyIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendPrototypeOrder", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string SendPrototypeOrder([XmlElement(DataType = "base64Binary")] byte[] DesignFile, int keyId, string StullerToken, [XmlElement(DataType = "base64Binary")] byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode)
	{
		return (string)Invoke("SendPrototypeOrder", new object[55]
		{
			DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName,
			ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial,
			CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo,
			GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone,
			RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage,
			EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode
		})[0];
	}

	public IAsyncResult BeginSendPrototypeOrder(byte[] DesignFile, int keyId, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendPrototypeOrder", new object[55]
		{
			DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName,
			ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial,
			CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo,
			GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone,
			RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage,
			EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode
		}, callback, asyncState);
	}

	public string EndSendPrototypeOrder(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void SendPrototypeOrderAsync(byte[] DesignFile, int keyId, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode)
	{
		SendPrototypeOrderAsync(DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone, RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode, null);
	}

	public void SendPrototypeOrderAsync(byte[] DesignFile, int keyId, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode, object userState)
	{
		if (SendPrototypeOrderOperationCompleted == null)
		{
			SendPrototypeOrderOperationCompleted = OnSendPrototypeOrderOperationCompleted;
		}
		InvokeAsync("SendPrototypeOrder", new object[55]
		{
			DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName,
			ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial,
			CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo,
			GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone,
			RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage,
			EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode
		}, SendPrototypeOrderOperationCompleted, userState);
	}

	private void OnSendPrototypeOrderOperationCompleted(object arg)
	{
		if (this.SendPrototypeOrderCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendPrototypeOrderCompleted(this, new SendPrototypeOrderCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendQuoteOrderRequest", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string SendQuoteOrderRequest([XmlElement(DataType = "base64Binary")] byte[] DesignFile, int keyId, string StullerToken, [XmlElement(DataType = "base64Binary")] byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode)
	{
		return (string)Invoke("SendQuoteOrderRequest", new object[55]
		{
			DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName,
			ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial,
			CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo,
			GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone,
			RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage,
			EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode
		})[0];
	}

	public IAsyncResult BeginSendQuoteOrderRequest(byte[] DesignFile, int keyId, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendQuoteOrderRequest", new object[55]
		{
			DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName,
			ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial,
			CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo,
			GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone,
			RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage,
			EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode
		}, callback, asyncState);
	}

	public string EndSendQuoteOrderRequest(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void SendQuoteOrderRequestAsync(byte[] DesignFile, int keyId, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode)
	{
		SendQuoteOrderRequestAsync(DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone, RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode, null);
	}

	public void SendQuoteOrderRequestAsync(byte[] DesignFile, int keyId, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, string ReferenceNumber, bool DebugMode, object userState)
	{
		if (SendQuoteOrderRequestOperationCompleted == null)
		{
			SendQuoteOrderRequestOperationCompleted = OnSendQuoteOrderRequestOperationCompleted;
		}
		InvokeAsync("SendQuoteOrderRequest", new object[55]
		{
			DesignFile, keyId, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName,
			ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial,
			CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo,
			GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone,
			RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage,
			EngraveFont, EngraveFontSize, EngraveFill, ReferenceNumber, DebugMode
		}, SendQuoteOrderRequestOperationCompleted, userState);
	}

	private void OnSendQuoteOrderRequestOperationCompleted(object arg)
	{
		if (this.SendQuoteOrderRequestCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendQuoteOrderRequestCompleted(this, new SendQuoteOrderRequestCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendQuoteRequest", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int SendQuoteRequest([XmlElement(DataType = "base64Binary")] byte[] DesignFile, string StullerToken, [XmlElement(DataType = "base64Binary")] byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode)
	{
		return (int)Invoke("SendQuoteRequest", new object[53]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone, RingSizeValue,
			RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont,
			EngraveFontSize, EngraveFill, DebugMode
		})[0];
	}

	public IAsyncResult BeginSendQuoteRequest(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendQuoteRequest", new object[53]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone, RingSizeValue,
			RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont,
			EngraveFontSize, EngraveFill, DebugMode
		}, callback, asyncState);
	}

	public int EndSendQuoteRequest(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void SendQuoteRequestAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode)
	{
		SendQuoteRequestAsync(DesignFile, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone, RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont, EngraveFontSize, EngraveFill, DebugMode, null);
	}

	public void SendQuoteRequestAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string MasterEMailAccount, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuoteEMail, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode, object userState)
	{
		if (SendQuoteRequestOperationCompleted == null)
		{
			SendQuoteRequestOperationCompleted = OnSendQuoteRequestOperationCompleted;
		}
		InvokeAsync("SendQuoteRequest", new object[53]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, MasterEMailAccount, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuoteEMail, QuotePhone, RingSizeValue,
			RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont,
			EngraveFontSize, EngraveFill, DebugMode
		}, SendQuoteRequestOperationCompleted, userState);
	}

	private void OnSendQuoteRequestOperationCompleted(object arg)
	{
		if (this.SendQuoteRequestCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendQuoteRequestCompleted(this, new SendQuoteRequestCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendRequestForQuote", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int SendRequestForQuote([XmlElement(DataType = "base64Binary")] byte[] DesignFile, string StullerToken, [XmlElement(DataType = "base64Binary")] byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality)
	{
		return (int)Invoke("SendRequestForQuote", new object[35]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems,
			MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality
		})[0];
	}

	public IAsyncResult BeginSendRequestForQuote(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendRequestForQuote", new object[35]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems,
			MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality
		}, callback, asyncState);
	}

	public int EndSendRequestForQuote(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void SendRequestForQuoteAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality)
	{
		SendRequestForQuoteAsync(DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, null);
	}

	public void SendRequestForQuoteAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, object userState)
	{
		if (SendRequestForQuoteOperationCompleted == null)
		{
			SendRequestForQuoteOperationCompleted = OnSendRequestForQuoteOperationCompleted;
		}
		InvokeAsync("SendRequestForQuote", new object[35]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems,
			MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality
		}, SendRequestForQuoteOperationCompleted, userState);
	}

	private void OnSendRequestForQuoteOperationCompleted(object arg)
	{
		if (this.SendRequestForQuoteCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendRequestForQuoteCompleted(this, new SendRequestForQuoteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendFullRequestForQuote", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int SendFullRequestForQuote([XmlElement(DataType = "base64Binary")] byte[] DesignFile, string StullerToken, [XmlElement(DataType = "base64Binary")] byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode)
	{
		return (int)Invoke("SendFullRequestForQuote", new object[52]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, RingSizeValue, RingSizeUnitOfMeasure,
			RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont, EngraveFontSize,
			EngraveFill, DebugMode
		})[0];
	}

	public IAsyncResult BeginSendFullRequestForQuote(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendFullRequestForQuote", new object[52]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, RingSizeValue, RingSizeUnitOfMeasure,
			RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont, EngraveFontSize,
			EngraveFill, DebugMode
		}, callback, asyncState);
	}

	public int EndSendFullRequestForQuote(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void SendFullRequestForQuoteAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode)
	{
		SendFullRequestForQuoteAsync(DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, RingSizeValue, RingSizeUnitOfMeasure, RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont, EngraveFontSize, EngraveFill, DebugMode, null);
	}

	public void SendFullRequestForQuoteAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, string RingSizeValue, string RingSizeUnitOfMeasure, double RingSizeMillimeters, string StullerIdNo, string BaseMetalCode, string Metal1Code, string Metal2Code, string Metal3Code, string Hallmarking, string EngraveMessage, string EngraveFont, string EngraveFontSize, string EngraveFill, bool DebugMode, object userState)
	{
		if (SendFullRequestForQuoteOperationCompleted == null)
		{
			SendFullRequestForQuoteOperationCompleted = OnSendFullRequestForQuoteOperationCompleted;
		}
		InvokeAsync("SendFullRequestForQuote", new object[52]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, RingSizeValue, RingSizeUnitOfMeasure,
			RingSizeMillimeters, StullerIdNo, BaseMetalCode, Metal1Code, Metal2Code, Metal3Code, Hallmarking, EngraveMessage, EngraveFont, EngraveFontSize,
			EngraveFill, DebugMode
		}, SendFullRequestForQuoteOperationCompleted, userState);
	}

	private void OnSendFullRequestForQuoteOperationCompleted(object arg)
	{
		if (this.SendFullRequestForQuoteCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendFullRequestForQuoteCompleted(this, new SendFullRequestForQuoteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SendDetailedRequestForQuote", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int SendDetailedRequestForQuote([XmlElement(DataType = "base64Binary")] byte[] DesignFile, string StullerToken, [XmlElement(DataType = "base64Binary")] byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, bool DebugMode)
	{
		return (int)Invoke("SendDetailedRequestForQuote", new object[39]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, DebugMode
		})[0];
	}

	public IAsyncResult BeginSendDetailedRequestForQuote(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, bool DebugMode, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SendDetailedRequestForQuote", new object[39]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, DebugMode
		}, callback, asyncState);
	}

	public int EndSendDetailedRequestForQuote(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void SendDetailedRequestForQuoteAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, bool DebugMode)
	{
		SendDetailedRequestForQuoteAsync(DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource, CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, DebugMode, null);
	}

	public void SendDetailedRequestForQuoteAsync(byte[] DesignFile, string StullerToken, byte[] SerialCode, string Username, string Company, string EMail, string BillToAccountNumber, string ShipToAccountNumber, string ShipToName, string ShipToAddress1, string ShipToAddress2, string ShipToCity, string ShipToState, string ShipToZip, string ShipToContactPhone, string ShipToContactEMail, string CustomerFirstName, string CustomerLastName, string CustomerMiddleInitial, string CustomerAddress1, string CustomerAddress2, string CustomerCity, string CustomerState, string CustomerZip, string CustomerContactPhone, string CustomerContactEMail, string CustomerNotes, string GemCenterSource, string GemCenterSourceInfo, string GemAccentSource, string[] CSAccentGems, string MountingFinishing, string MountingPrimaryMetal, string MountingPrimaryMetalQuality, string MountingSecondaryMetal, string MountingSecondaryMetalQuality, string QuoteNotes, string QuotePhone, bool DebugMode, object userState)
	{
		if (SendDetailedRequestForQuoteOperationCompleted == null)
		{
			SendDetailedRequestForQuoteOperationCompleted = OnSendDetailedRequestForQuoteOperationCompleted;
		}
		InvokeAsync("SendDetailedRequestForQuote", new object[39]
		{
			DesignFile, StullerToken, SerialCode, Username, Company, EMail, BillToAccountNumber, ShipToAccountNumber, ShipToName, ShipToAddress1,
			ShipToAddress2, ShipToCity, ShipToState, ShipToZip, ShipToContactPhone, ShipToContactEMail, CustomerFirstName, CustomerLastName, CustomerMiddleInitial, CustomerAddress1,
			CustomerAddress2, CustomerCity, CustomerState, CustomerZip, CustomerContactPhone, CustomerContactEMail, CustomerNotes, GemCenterSource, GemCenterSourceInfo, GemAccentSource,
			CSAccentGems, MountingFinishing, MountingPrimaryMetal, MountingPrimaryMetalQuality, MountingSecondaryMetal, MountingSecondaryMetalQuality, QuoteNotes, QuotePhone, DebugMode
		}, SendDetailedRequestForQuoteOperationCompleted, userState);
	}

	private void OnSendDetailedRequestForQuoteOperationCompleted(object arg)
	{
		if (this.SendDetailedRequestForQuoteCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SendDetailedRequestForQuoteCompleted(this, new SendDetailedRequestForQuoteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetCatalogOrders", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetCatalogOrders()
	{
		return (DataTable)Invoke("GetCatalogOrders", new object[0])[0];
	}

	public IAsyncResult BeginGetCatalogOrders(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCatalogOrders", new object[0], callback, asyncState);
	}

	public DataTable EndGetCatalogOrders(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetCatalogOrdersAsync()
	{
		GetCatalogOrdersAsync(null);
	}

	public void GetCatalogOrdersAsync(object userState)
	{
		if (GetCatalogOrdersOperationCompleted == null)
		{
			GetCatalogOrdersOperationCompleted = OnGetCatalogOrdersOperationCompleted;
		}
		InvokeAsync("GetCatalogOrders", new object[0], GetCatalogOrdersOperationCompleted, userState);
	}

	private void OnGetCatalogOrdersOperationCompleted(object arg)
	{
		if (this.GetCatalogOrdersCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCatalogOrdersCompleted(this, new GetCatalogOrdersCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetCatalogOrderById", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetCatalogOrderById(int catalogOrderId, int keyId)
	{
		return (DataTable)Invoke("GetCatalogOrderById", new object[2] { catalogOrderId, keyId })[0];
	}

	public IAsyncResult BeginGetCatalogOrderById(int catalogOrderId, int keyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCatalogOrderById", new object[2] { catalogOrderId, keyId }, callback, asyncState);
	}

	public DataTable EndGetCatalogOrderById(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetCatalogOrderByIdAsync(int catalogOrderId, int keyId)
	{
		GetCatalogOrderByIdAsync(catalogOrderId, keyId, null);
	}

	public void GetCatalogOrderByIdAsync(int catalogOrderId, int keyId, object userState)
	{
		if (GetCatalogOrderByIdOperationCompleted == null)
		{
			GetCatalogOrderByIdOperationCompleted = OnGetCatalogOrderByIdOperationCompleted;
		}
		InvokeAsync("GetCatalogOrderById", new object[2] { catalogOrderId, keyId }, GetCatalogOrderByIdOperationCompleted, userState);
	}

	private void OnGetCatalogOrderByIdOperationCompleted(object arg)
	{
		if (this.GetCatalogOrderByIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCatalogOrderByIdCompleted(this, new GetCatalogOrderByIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetCatalogOrdersByKeyId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetCatalogOrdersByKeyId(int keyId)
	{
		return (DataTable)Invoke("GetCatalogOrdersByKeyId", new object[1] { keyId })[0];
	}

	public IAsyncResult BeginGetCatalogOrdersByKeyId(int keyId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCatalogOrdersByKeyId", new object[1] { keyId }, callback, asyncState);
	}

	public DataTable EndGetCatalogOrdersByKeyId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetCatalogOrdersByKeyIdAsync(int keyId)
	{
		GetCatalogOrdersByKeyIdAsync(keyId, null);
	}

	public void GetCatalogOrdersByKeyIdAsync(int keyId, object userState)
	{
		if (GetCatalogOrdersByKeyIdOperationCompleted == null)
		{
			GetCatalogOrdersByKeyIdOperationCompleted = OnGetCatalogOrdersByKeyIdOperationCompleted;
		}
		InvokeAsync("GetCatalogOrdersByKeyId", new object[1] { keyId }, GetCatalogOrdersByKeyIdOperationCompleted, userState);
	}

	private void OnGetCatalogOrdersByKeyIdOperationCompleted(object arg)
	{
		if (this.GetCatalogOrdersByKeyIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCatalogOrdersByKeyIdCompleted(this, new GetCatalogOrdersByKeyIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CreateCatalogOrder", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CreateCatalogOrder(int keyId, int catalogOrderId, string arrangementTitle, [XmlElement(IsNullable = true)] DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle)
	{
		Invoke("CreateCatalogOrder", new object[30]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost,
			storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont,
			subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle
		});
	}

	public IAsyncResult BeginCreateCatalogOrder(int keyId, int catalogOrderId, string arrangementTitle, DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CreateCatalogOrder", new object[30]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost,
			storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont,
			subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle
		}, callback, asyncState);
	}

	public void EndCreateCatalogOrder(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CreateCatalogOrderAsync(int keyId, int catalogOrderId, string arrangementTitle, DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle)
	{
		CreateCatalogOrderAsync(keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost, storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont, subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle, null);
	}

	public void CreateCatalogOrderAsync(int keyId, int catalogOrderId, string arrangementTitle, DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, object userState)
	{
		if (CreateCatalogOrderOperationCompleted == null)
		{
			CreateCatalogOrderOperationCompleted = OnCreateCatalogOrderOperationCompleted;
		}
		InvokeAsync("CreateCatalogOrder", new object[30]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost,
			storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont,
			subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle
		}, CreateCatalogOrderOperationCompleted, userState);
	}

	private void OnCreateCatalogOrderOperationCompleted(object arg)
	{
		if (this.CreateCatalogOrderCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CreateCatalogOrderCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CreateCatalogOrderNew", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CreateCatalogOrderNew(int keyId, int catalogOrderId, string arrangementTitle, [XmlElement(IsNullable = true)] DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, string storeLogo, string openingPhoto)
	{
		Invoke("CreateCatalogOrderNew", new object[32]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost,
			storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont,
			subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle,
			storeLogo, openingPhoto
		});
	}

	public IAsyncResult BeginCreateCatalogOrderNew(int keyId, int catalogOrderId, string arrangementTitle, DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, string storeLogo, string openingPhoto, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CreateCatalogOrderNew", new object[32]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost,
			storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont,
			subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle,
			storeLogo, openingPhoto
		}, callback, asyncState);
	}

	public void EndCreateCatalogOrderNew(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CreateCatalogOrderNewAsync(int keyId, int catalogOrderId, string arrangementTitle, DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, string storeLogo, string openingPhoto)
	{
		CreateCatalogOrderNewAsync(keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost, storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont, subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle, storeLogo, openingPhoto, null);
	}

	public void CreateCatalogOrderNewAsync(int keyId, int catalogOrderId, string arrangementTitle, DateTime? createDate, int numberOfCopies, int numberOfPages, string orderStatus, string pathToSampleCover, string themeTitle, double totalCost, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, string storeLogo, string openingPhoto, object userState)
	{
		if (CreateCatalogOrderNewOperationCompleted == null)
		{
			CreateCatalogOrderNewOperationCompleted = OnCreateCatalogOrderNewOperationCompleted;
		}
		InvokeAsync("CreateCatalogOrderNew", new object[32]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderStatus, pathToSampleCover, themeTitle, totalCost,
			storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont,
			subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle,
			storeLogo, openingPhoto
		}, CreateCatalogOrderNewOperationCompleted, userState);
	}

	private void OnCreateCatalogOrderNewOperationCompleted(object arg)
	{
		if (this.CreateCatalogOrderNewCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CreateCatalogOrderNewCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateCatalogOrder", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateCatalogOrder(int keyId, int catalogOrderId, string arrangementTitle, DateTime createDate, int numberOfCopies, int numberOfPages, [XmlElement(IsNullable = true)] DateTime? orderDate, int orderNumber, string orderStatus, string pathToPDF, string pathToSampleCover, [XmlElement(IsNullable = true)] DateTime? shipDate, string shipToAddress1, string shipToAddress2, string shipToCity, string shipToCountry, string shipToName, string shipToState, string shipToZip, string themeTitle, double totalCost, string trackingNumber, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle)
	{
		Invoke("UpdateCatalogOrder", new object[42]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderDate, orderNumber, orderStatus, pathToPDF,
			pathToSampleCover, shipDate, shipToAddress1, shipToAddress2, shipToCity, shipToCountry, shipToName, shipToState, shipToZip, themeTitle,
			totalCost, trackingNumber, storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle,
			subTitleText, subTitleFont, subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont,
			descriptionFontSize, descriptionFontStyle
		});
	}

	public IAsyncResult BeginUpdateCatalogOrder(int keyId, int catalogOrderId, string arrangementTitle, DateTime createDate, int numberOfCopies, int numberOfPages, DateTime? orderDate, int orderNumber, string orderStatus, string pathToPDF, string pathToSampleCover, DateTime? shipDate, string shipToAddress1, string shipToAddress2, string shipToCity, string shipToCountry, string shipToName, string shipToState, string shipToZip, string themeTitle, double totalCost, string trackingNumber, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateCatalogOrder", new object[42]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderDate, orderNumber, orderStatus, pathToPDF,
			pathToSampleCover, shipDate, shipToAddress1, shipToAddress2, shipToCity, shipToCountry, shipToName, shipToState, shipToZip, themeTitle,
			totalCost, trackingNumber, storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle,
			subTitleText, subTitleFont, subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont,
			descriptionFontSize, descriptionFontStyle
		}, callback, asyncState);
	}

	public void EndUpdateCatalogOrder(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateCatalogOrderAsync(int keyId, int catalogOrderId, string arrangementTitle, DateTime createDate, int numberOfCopies, int numberOfPages, DateTime? orderDate, int orderNumber, string orderStatus, string pathToPDF, string pathToSampleCover, DateTime? shipDate, string shipToAddress1, string shipToAddress2, string shipToCity, string shipToCountry, string shipToName, string shipToState, string shipToZip, string themeTitle, double totalCost, string trackingNumber, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle)
	{
		UpdateCatalogOrderAsync(keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderDate, orderNumber, orderStatus, pathToPDF, pathToSampleCover, shipDate, shipToAddress1, shipToAddress2, shipToCity, shipToCountry, shipToName, shipToState, shipToZip, themeTitle, totalCost, trackingNumber, storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle, subTitleText, subTitleFont, subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont, descriptionFontSize, descriptionFontStyle, null);
	}

	public void UpdateCatalogOrderAsync(int keyId, int catalogOrderId, string arrangementTitle, DateTime createDate, int numberOfCopies, int numberOfPages, DateTime? orderDate, int orderNumber, string orderStatus, string pathToPDF, string pathToSampleCover, DateTime? shipDate, string shipToAddress1, string shipToAddress2, string shipToCity, string shipToCountry, string shipToName, string shipToState, string shipToZip, string themeTitle, double totalCost, string trackingNumber, string storeName, string storeNameFont, string storeNameFontSize, string storeNameFontStyle, string titleText, string titleFont, string titleFontSize, string titleFontStyle, string subTitleText, string subTitleFont, string subTitleFontSize, string subTitleFontStyle, string backCoverText, string backCoverFont, string backCoverFontSize, string backCoverFontStyle, string descriptionText, string descriptionFont, string descriptionFontSize, string descriptionFontStyle, object userState)
	{
		if (UpdateCatalogOrderOperationCompleted == null)
		{
			UpdateCatalogOrderOperationCompleted = OnUpdateCatalogOrderOperationCompleted;
		}
		InvokeAsync("UpdateCatalogOrder", new object[42]
		{
			keyId, catalogOrderId, arrangementTitle, createDate, numberOfCopies, numberOfPages, orderDate, orderNumber, orderStatus, pathToPDF,
			pathToSampleCover, shipDate, shipToAddress1, shipToAddress2, shipToCity, shipToCountry, shipToName, shipToState, shipToZip, themeTitle,
			totalCost, trackingNumber, storeName, storeNameFont, storeNameFontSize, storeNameFontStyle, titleText, titleFont, titleFontSize, titleFontStyle,
			subTitleText, subTitleFont, subTitleFontSize, subTitleFontStyle, backCoverText, backCoverFont, backCoverFontSize, backCoverFontStyle, descriptionText, descriptionFont,
			descriptionFontSize, descriptionFontStyle
		}, UpdateCatalogOrderOperationCompleted, userState);
	}

	private void OnUpdateCatalogOrderOperationCompleted(object arg)
	{
		if (this.UpdateCatalogOrderCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateCatalogOrderCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateCatalogOrderShipment", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateCatalogOrderShipment(int keyId, int catalogOrderId, string orderStatus, [XmlElement(IsNullable = true)] DateTime? shipDate, string trackingNumber, string creationStatus)
	{
		Invoke("UpdateCatalogOrderShipment", new object[6] { keyId, catalogOrderId, orderStatus, shipDate, trackingNumber, creationStatus });
	}

	public IAsyncResult BeginUpdateCatalogOrderShipment(int keyId, int catalogOrderId, string orderStatus, DateTime? shipDate, string trackingNumber, string creationStatus, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateCatalogOrderShipment", new object[6] { keyId, catalogOrderId, orderStatus, shipDate, trackingNumber, creationStatus }, callback, asyncState);
	}

	public void EndUpdateCatalogOrderShipment(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateCatalogOrderShipmentAsync(int keyId, int catalogOrderId, string orderStatus, DateTime? shipDate, string trackingNumber, string creationStatus)
	{
		UpdateCatalogOrderShipmentAsync(keyId, catalogOrderId, orderStatus, shipDate, trackingNumber, creationStatus, null);
	}

	public void UpdateCatalogOrderShipmentAsync(int keyId, int catalogOrderId, string orderStatus, DateTime? shipDate, string trackingNumber, string creationStatus, object userState)
	{
		if (UpdateCatalogOrderShipmentOperationCompleted == null)
		{
			UpdateCatalogOrderShipmentOperationCompleted = OnUpdateCatalogOrderShipmentOperationCompleted;
		}
		InvokeAsync("UpdateCatalogOrderShipment", new object[6] { keyId, catalogOrderId, orderStatus, shipDate, trackingNumber, creationStatus }, UpdateCatalogOrderShipmentOperationCompleted, userState);
	}

	private void OnUpdateCatalogOrderShipmentOperationCompleted(object arg)
	{
		if (this.UpdateCatalogOrderShipmentCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateCatalogOrderShipmentCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UpdateCatalogOrderCreationStatus", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void UpdateCatalogOrderCreationStatus(int keyId, int catalogOrderId, string creationStatus)
	{
		Invoke("UpdateCatalogOrderCreationStatus", new object[3] { keyId, catalogOrderId, creationStatus });
	}

	public IAsyncResult BeginUpdateCatalogOrderCreationStatus(int keyId, int catalogOrderId, string creationStatus, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UpdateCatalogOrderCreationStatus", new object[3] { keyId, catalogOrderId, creationStatus }, callback, asyncState);
	}

	public void EndUpdateCatalogOrderCreationStatus(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void UpdateCatalogOrderCreationStatusAsync(int keyId, int catalogOrderId, string creationStatus)
	{
		UpdateCatalogOrderCreationStatusAsync(keyId, catalogOrderId, creationStatus, null);
	}

	public void UpdateCatalogOrderCreationStatusAsync(int keyId, int catalogOrderId, string creationStatus, object userState)
	{
		if (UpdateCatalogOrderCreationStatusOperationCompleted == null)
		{
			UpdateCatalogOrderCreationStatusOperationCompleted = OnUpdateCatalogOrderCreationStatusOperationCompleted;
		}
		InvokeAsync("UpdateCatalogOrderCreationStatus", new object[3] { keyId, catalogOrderId, creationStatus }, UpdateCatalogOrderCreationStatusOperationCompleted, userState);
	}

	private void OnUpdateCatalogOrderCreationStatusOperationCompleted(object arg)
	{
		if (this.UpdateCatalogOrderCreationStatusCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UpdateCatalogOrderCreationStatusCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DeleteCatalogOrder", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void DeleteCatalogOrder(int keyId, int catalogOrderId)
	{
		Invoke("DeleteCatalogOrder", new object[2] { keyId, catalogOrderId });
	}

	public IAsyncResult BeginDeleteCatalogOrder(int keyId, int catalogOrderId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DeleteCatalogOrder", new object[2] { keyId, catalogOrderId }, callback, asyncState);
	}

	public void EndDeleteCatalogOrder(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void DeleteCatalogOrderAsync(int keyId, int catalogOrderId)
	{
		DeleteCatalogOrderAsync(keyId, catalogOrderId, null);
	}

	public void DeleteCatalogOrderAsync(int keyId, int catalogOrderId, object userState)
	{
		if (DeleteCatalogOrderOperationCompleted == null)
		{
			DeleteCatalogOrderOperationCompleted = OnDeleteCatalogOrderOperationCompleted;
		}
		InvokeAsync("DeleteCatalogOrder", new object[2] { keyId, catalogOrderId }, DeleteCatalogOrderOperationCompleted, userState);
	}

	private void OnDeleteCatalogOrderOperationCompleted(object arg)
	{
		if (this.DeleteCatalogOrderCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DeleteCatalogOrderCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetCatalogCostPerPage", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public double GetCatalogCostPerPage()
	{
		return (double)Invoke("GetCatalogCostPerPage", new object[0])[0];
	}

	public IAsyncResult BeginGetCatalogCostPerPage(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCatalogCostPerPage", new object[0], callback, asyncState);
	}

	public double EndGetCatalogCostPerPage(IAsyncResult asyncResult)
	{
		return (double)EndInvoke(asyncResult)[0];
	}

	public void GetCatalogCostPerPageAsync()
	{
		GetCatalogCostPerPageAsync(null);
	}

	public void GetCatalogCostPerPageAsync(object userState)
	{
		if (GetCatalogCostPerPageOperationCompleted == null)
		{
			GetCatalogCostPerPageOperationCompleted = OnGetCatalogCostPerPageOperationCompleted;
		}
		InvokeAsync("GetCatalogCostPerPage", new object[0], GetCatalogCostPerPageOperationCompleted, userState);
	}

	private void OnGetCatalogCostPerPageOperationCompleted(object arg)
	{
		if (this.GetCatalogCostPerPageCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCatalogCostPerPageCompleted(this, new GetCatalogCostPerPageCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetCatalogBaseShippingCost", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public double GetCatalogBaseShippingCost()
	{
		return (double)Invoke("GetCatalogBaseShippingCost", new object[0])[0];
	}

	public IAsyncResult BeginGetCatalogBaseShippingCost(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCatalogBaseShippingCost", new object[0], callback, asyncState);
	}

	public double EndGetCatalogBaseShippingCost(IAsyncResult asyncResult)
	{
		return (double)EndInvoke(asyncResult)[0];
	}

	public void GetCatalogBaseShippingCostAsync()
	{
		GetCatalogBaseShippingCostAsync(null);
	}

	public void GetCatalogBaseShippingCostAsync(object userState)
	{
		if (GetCatalogBaseShippingCostOperationCompleted == null)
		{
			GetCatalogBaseShippingCostOperationCompleted = OnGetCatalogBaseShippingCostOperationCompleted;
		}
		InvokeAsync("GetCatalogBaseShippingCost", new object[0], GetCatalogBaseShippingCostOperationCompleted, userState);
	}

	private void OnGetCatalogBaseShippingCostOperationCompleted(object arg)
	{
		if (this.GetCatalogBaseShippingCostCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCatalogBaseShippingCostCompleted(this, new GetCatalogBaseShippingCostCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetCatalogPerCopyShippingCost", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public double GetCatalogPerCopyShippingCost()
	{
		return (double)Invoke("GetCatalogPerCopyShippingCost", new object[0])[0];
	}

	public IAsyncResult BeginGetCatalogPerCopyShippingCost(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCatalogPerCopyShippingCost", new object[0], callback, asyncState);
	}

	public double EndGetCatalogPerCopyShippingCost(IAsyncResult asyncResult)
	{
		return (double)EndInvoke(asyncResult)[0];
	}

	public void GetCatalogPerCopyShippingCostAsync()
	{
		GetCatalogPerCopyShippingCostAsync(null);
	}

	public void GetCatalogPerCopyShippingCostAsync(object userState)
	{
		if (GetCatalogPerCopyShippingCostOperationCompleted == null)
		{
			GetCatalogPerCopyShippingCostOperationCompleted = OnGetCatalogPerCopyShippingCostOperationCompleted;
		}
		InvokeAsync("GetCatalogPerCopyShippingCost", new object[0], GetCatalogPerCopyShippingCostOperationCompleted, userState);
	}

	private void OnGetCatalogPerCopyShippingCostOperationCompleted(object arg)
	{
		if (this.GetCatalogPerCopyShippingCostCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCatalogPerCopyShippingCostCompleted(this, new GetCatalogPerCopyShippingCostCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignSignature", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetDesignSignature(int DesignId, int Version, out int BlockCount)
	{
		object[] array = Invoke("GetDesignSignature", new object[2] { DesignId, Version });
		BlockCount = (int)array[1];
		return (string)array[0];
	}

	public IAsyncResult BeginGetDesignSignature(int DesignId, int Version, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignSignature", new object[2] { DesignId, Version }, callback, asyncState);
	}

	public string EndGetDesignSignature(IAsyncResult asyncResult, out int BlockCount)
	{
		object[] array = EndInvoke(asyncResult);
		BlockCount = (int)array[1];
		return (string)array[0];
	}

	public void GetDesignSignatureAsync(int DesignId, int Version)
	{
		GetDesignSignatureAsync(DesignId, Version, null);
	}

	public void GetDesignSignatureAsync(int DesignId, int Version, object userState)
	{
		if (GetDesignSignatureOperationCompleted == null)
		{
			GetDesignSignatureOperationCompleted = OnGetDesignSignatureOperationCompleted;
		}
		InvokeAsync("GetDesignSignature", new object[2] { DesignId, Version }, GetDesignSignatureOperationCompleted, userState);
	}

	private void OnGetDesignSignatureOperationCompleted(object arg)
	{
		if (this.GetDesignSignatureCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignSignatureCompleted(this, new GetDesignSignatureCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignSignatures", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetDesignSignatures(int DesignId, out int[] Version, out int[] BlockCount, out string[] Signature)
	{
		object[] array = Invoke("GetDesignSignatures", new object[1] { DesignId });
		Version = (int[])array[1];
		BlockCount = (int[])array[2];
		Signature = (string[])array[3];
		return (int)array[0];
	}

	public IAsyncResult BeginGetDesignSignatures(int DesignId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignSignatures", new object[1] { DesignId }, callback, asyncState);
	}

	public int EndGetDesignSignatures(IAsyncResult asyncResult, out int[] Version, out int[] BlockCount, out string[] Signature)
	{
		object[] array = EndInvoke(asyncResult);
		Version = (int[])array[1];
		BlockCount = (int[])array[2];
		Signature = (string[])array[3];
		return (int)array[0];
	}

	public void GetDesignSignaturesAsync(int DesignId)
	{
		GetDesignSignaturesAsync(DesignId, null);
	}

	public void GetDesignSignaturesAsync(int DesignId, object userState)
	{
		if (GetDesignSignaturesOperationCompleted == null)
		{
			GetDesignSignaturesOperationCompleted = OnGetDesignSignaturesOperationCompleted;
		}
		InvokeAsync("GetDesignSignatures", new object[1] { DesignId }, GetDesignSignaturesOperationCompleted, userState);
	}

	private void OnGetDesignSignaturesOperationCompleted(object arg)
	{
		if (this.GetDesignSignaturesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignSignaturesCompleted(this, new GetDesignSignaturesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UploadCAMSettings", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string UploadCAMSettings([XmlElement(DataType = "base64Binary")] byte[] file, int designId, int version)
	{
		return (string)Invoke("UploadCAMSettings", new object[3] { file, designId, version })[0];
	}

	public IAsyncResult BeginUploadCAMSettings(byte[] file, int designId, int version, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UploadCAMSettings", new object[3] { file, designId, version }, callback, asyncState);
	}

	public string EndUploadCAMSettings(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void UploadCAMSettingsAsync(byte[] file, int designId, int version)
	{
		UploadCAMSettingsAsync(file, designId, version, null);
	}

	public void UploadCAMSettingsAsync(byte[] file, int designId, int version, object userState)
	{
		if (UploadCAMSettingsOperationCompleted == null)
		{
			UploadCAMSettingsOperationCompleted = OnUploadCAMSettingsOperationCompleted;
		}
		InvokeAsync("UploadCAMSettings", new object[3] { file, designId, version }, UploadCAMSettingsOperationCompleted, userState);
	}

	private void OnUploadCAMSettingsOperationCompleted(object arg)
	{
		if (this.UploadCAMSettingsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UploadCAMSettingsCompleted(this, new UploadCAMSettingsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/UploadCAMSettingsFile", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string UploadCAMSettingsFile([XmlElement(DataType = "base64Binary")] byte[] file, int designId, int version, string settingsXML)
	{
		return (string)Invoke("UploadCAMSettingsFile", new object[4] { file, designId, version, settingsXML })[0];
	}

	public IAsyncResult BeginUploadCAMSettingsFile(byte[] file, int designId, int version, string settingsXML, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("UploadCAMSettingsFile", new object[4] { file, designId, version, settingsXML }, callback, asyncState);
	}

	public string EndUploadCAMSettingsFile(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void UploadCAMSettingsFileAsync(byte[] file, int designId, int version, string settingsXML)
	{
		UploadCAMSettingsFileAsync(file, designId, version, settingsXML, null);
	}

	public void UploadCAMSettingsFileAsync(byte[] file, int designId, int version, string settingsXML, object userState)
	{
		if (UploadCAMSettingsFileOperationCompleted == null)
		{
			UploadCAMSettingsFileOperationCompleted = OnUploadCAMSettingsFileOperationCompleted;
		}
		InvokeAsync("UploadCAMSettingsFile", new object[4] { file, designId, version, settingsXML }, UploadCAMSettingsFileOperationCompleted, userState);
	}

	private void OnUploadCAMSettingsFileOperationCompleted(object arg)
	{
		if (this.UploadCAMSettingsFileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.UploadCAMSettingsFileCompleted(this, new UploadCAMSettingsFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetCAMSettingFiles", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int GetCAMSettingFiles(int designId, int version, out string[] settingXMLs)
	{
		object[] array = Invoke("GetCAMSettingFiles", new object[2] { designId, version });
		settingXMLs = (string[])array[1];
		return (int)array[0];
	}

	public IAsyncResult BeginGetCAMSettingFiles(int designId, int version, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetCAMSettingFiles", new object[2] { designId, version }, callback, asyncState);
	}

	public int EndGetCAMSettingFiles(IAsyncResult asyncResult, out string[] settingXMLs)
	{
		object[] array = EndInvoke(asyncResult);
		settingXMLs = (string[])array[1];
		return (int)array[0];
	}

	public void GetCAMSettingFilesAsync(int designId, int version)
	{
		GetCAMSettingFilesAsync(designId, version, null);
	}

	public void GetCAMSettingFilesAsync(int designId, int version, object userState)
	{
		if (GetCAMSettingFilesOperationCompleted == null)
		{
			GetCAMSettingFilesOperationCompleted = OnGetCAMSettingFilesOperationCompleted;
		}
		InvokeAsync("GetCAMSettingFiles", new object[2] { designId, version }, GetCAMSettingFilesOperationCompleted, userState);
	}

	private void OnGetCAMSettingFilesOperationCompleted(object arg)
	{
		if (this.GetCAMSettingFilesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetCAMSettingFilesCompleted(this, new GetCAMSettingFilesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DownloadCAMSettings", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] DownloadCAMSettings(int designId, int version)
	{
		return (byte[])Invoke("DownloadCAMSettings", new object[2] { designId, version })[0];
	}

	public IAsyncResult BeginDownloadCAMSettings(int designId, int version, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DownloadCAMSettings", new object[2] { designId, version }, callback, asyncState);
	}

	public byte[] EndDownloadCAMSettings(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void DownloadCAMSettingsAsync(int designId, int version)
	{
		DownloadCAMSettingsAsync(designId, version, null);
	}

	public void DownloadCAMSettingsAsync(int designId, int version, object userState)
	{
		if (DownloadCAMSettingsOperationCompleted == null)
		{
			DownloadCAMSettingsOperationCompleted = OnDownloadCAMSettingsOperationCompleted;
		}
		InvokeAsync("DownloadCAMSettings", new object[2] { designId, version }, DownloadCAMSettingsOperationCompleted, userState);
	}

	private void OnDownloadCAMSettingsOperationCompleted(object arg)
	{
		if (this.DownloadCAMSettingsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DownloadCAMSettingsCompleted(this, new DownloadCAMSettingsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DownloadCAMSettingsFile", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] DownloadCAMSettingsFile(int designId, int version, string settingsXML)
	{
		return (byte[])Invoke("DownloadCAMSettingsFile", new object[3] { designId, version, settingsXML })[0];
	}

	public IAsyncResult BeginDownloadCAMSettingsFile(int designId, int version, string settingsXML, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DownloadCAMSettingsFile", new object[3] { designId, version, settingsXML }, callback, asyncState);
	}

	public byte[] EndDownloadCAMSettingsFile(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void DownloadCAMSettingsFileAsync(int designId, int version, string settingsXML)
	{
		DownloadCAMSettingsFileAsync(designId, version, settingsXML, null);
	}

	public void DownloadCAMSettingsFileAsync(int designId, int version, string settingsXML, object userState)
	{
		if (DownloadCAMSettingsFileOperationCompleted == null)
		{
			DownloadCAMSettingsFileOperationCompleted = OnDownloadCAMSettingsFileOperationCompleted;
		}
		InvokeAsync("DownloadCAMSettingsFile", new object[3] { designId, version, settingsXML }, DownloadCAMSettingsFileOperationCompleted, userState);
	}

	private void OnDownloadCAMSettingsFileOperationCompleted(object arg)
	{
		if (this.DownloadCAMSettingsFileCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DownloadCAMSettingsFileCompleted(this, new DownloadCAMSettingsFileCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DownloadDesignForReview", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] DownloadDesignForReview(int designId)
	{
		return (byte[])Invoke("DownloadDesignForReview", new object[1] { designId })[0];
	}

	public IAsyncResult BeginDownloadDesignForReview(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DownloadDesignForReview", new object[1] { designId }, callback, asyncState);
	}

	public byte[] EndDownloadDesignForReview(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void DownloadDesignForReviewAsync(int designId)
	{
		DownloadDesignForReviewAsync(designId, null);
	}

	public void DownloadDesignForReviewAsync(int designId, object userState)
	{
		if (DownloadDesignForReviewOperationCompleted == null)
		{
			DownloadDesignForReviewOperationCompleted = OnDownloadDesignForReviewOperationCompleted;
		}
		InvokeAsync("DownloadDesignForReview", new object[1] { designId }, DownloadDesignForReviewOperationCompleted, userState);
	}

	private void OnDownloadDesignForReviewOperationCompleted(object arg)
	{
		if (this.DownloadDesignForReviewCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DownloadDesignForReviewCompleted(this, new DownloadDesignForReviewCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignIteraionSetNotFound", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void DesignIteraionSetNotFound(int DesignId)
	{
		Invoke("DesignIteraionSetNotFound", new object[1] { DesignId });
	}

	public IAsyncResult BeginDesignIteraionSetNotFound(int DesignId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignIteraionSetNotFound", new object[1] { DesignId }, callback, asyncState);
	}

	public void EndDesignIteraionSetNotFound(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void DesignIteraionSetNotFoundAsync(int DesignId)
	{
		DesignIteraionSetNotFoundAsync(DesignId, null);
	}

	public void DesignIteraionSetNotFoundAsync(int DesignId, object userState)
	{
		if (DesignIteraionSetNotFoundOperationCompleted == null)
		{
			DesignIteraionSetNotFoundOperationCompleted = OnDesignIteraionSetNotFoundOperationCompleted;
		}
		InvokeAsync("DesignIteraionSetNotFound", new object[1] { DesignId }, DesignIteraionSetNotFoundOperationCompleted, userState);
	}

	private void OnDesignIteraionSetNotFoundOperationCompleted(object arg)
	{
		if (this.DesignIteraionSetNotFoundCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignIteraionSetNotFoundCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignIterationValues", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignIterationValues(int DesignId)
	{
		return (DataTable)Invoke("GetDesignIterationValues", new object[1] { DesignId })[0];
	}

	public IAsyncResult BeginGetDesignIterationValues(int DesignId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignIterationValues", new object[1] { DesignId }, callback, asyncState);
	}

	public DataTable EndGetDesignIterationValues(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignIterationValuesAsync(int DesignId)
	{
		GetDesignIterationValuesAsync(DesignId, null);
	}

	public void GetDesignIterationValuesAsync(int DesignId, object userState)
	{
		if (GetDesignIterationValuesOperationCompleted == null)
		{
			GetDesignIterationValuesOperationCompleted = OnGetDesignIterationValuesOperationCompleted;
		}
		InvokeAsync("GetDesignIterationValues", new object[1] { DesignId }, GetDesignIterationValuesOperationCompleted, userState);
	}

	private void OnGetDesignIterationValuesOperationCompleted(object arg)
	{
		if (this.GetDesignIterationValuesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignIterationValuesCompleted(this, new GetDesignIterationValuesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignIteraionComponentAdd", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int DesignIteraionComponentAdd(int DesignId, int ComponentId, int IterationIndex)
	{
		return (int)Invoke("DesignIteraionComponentAdd", new object[3] { DesignId, ComponentId, IterationIndex })[0];
	}

	public IAsyncResult BeginDesignIteraionComponentAdd(int DesignId, int ComponentId, int IterationIndex, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignIteraionComponentAdd", new object[3] { DesignId, ComponentId, IterationIndex }, callback, asyncState);
	}

	public int EndDesignIteraionComponentAdd(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void DesignIteraionComponentAddAsync(int DesignId, int ComponentId, int IterationIndex)
	{
		DesignIteraionComponentAddAsync(DesignId, ComponentId, IterationIndex, null);
	}

	public void DesignIteraionComponentAddAsync(int DesignId, int ComponentId, int IterationIndex, object userState)
	{
		if (DesignIteraionComponentAddOperationCompleted == null)
		{
			DesignIteraionComponentAddOperationCompleted = OnDesignIteraionComponentAddOperationCompleted;
		}
		InvokeAsync("DesignIteraionComponentAdd", new object[3] { DesignId, ComponentId, IterationIndex }, DesignIteraionComponentAddOperationCompleted, userState);
	}

	private void OnDesignIteraionComponentAddOperationCompleted(object arg)
	{
		if (this.DesignIteraionComponentAddCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignIteraionComponentAddCompleted(this, new DesignIteraionComponentAddCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignIteraionComponentSetFoundAndDeleteAttributes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void DesignIteraionComponentSetFoundAndDeleteAttributes(int DesignIterationComponentId, int IterationIndex)
	{
		Invoke("DesignIteraionComponentSetFoundAndDeleteAttributes", new object[2] { DesignIterationComponentId, IterationIndex });
	}

	public IAsyncResult BeginDesignIteraionComponentSetFoundAndDeleteAttributes(int DesignIterationComponentId, int IterationIndex, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignIteraionComponentSetFoundAndDeleteAttributes", new object[2] { DesignIterationComponentId, IterationIndex }, callback, asyncState);
	}

	public void EndDesignIteraionComponentSetFoundAndDeleteAttributes(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void DesignIteraionComponentSetFoundAndDeleteAttributesAsync(int DesignIterationComponentId, int IterationIndex)
	{
		DesignIteraionComponentSetFoundAndDeleteAttributesAsync(DesignIterationComponentId, IterationIndex, null);
	}

	public void DesignIteraionComponentSetFoundAndDeleteAttributesAsync(int DesignIterationComponentId, int IterationIndex, object userState)
	{
		if (DesignIteraionComponentSetFoundAndDeleteAttributesOperationCompleted == null)
		{
			DesignIteraionComponentSetFoundAndDeleteAttributesOperationCompleted = OnDesignIteraionComponentSetFoundAndDeleteAttributesOperationCompleted;
		}
		InvokeAsync("DesignIteraionComponentSetFoundAndDeleteAttributes", new object[2] { DesignIterationComponentId, IterationIndex }, DesignIteraionComponentSetFoundAndDeleteAttributesOperationCompleted, userState);
	}

	private void OnDesignIteraionComponentSetFoundAndDeleteAttributesOperationCompleted(object arg)
	{
		if (this.DesignIteraionComponentSetFoundAndDeleteAttributesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignIteraionComponentSetFoundAndDeleteAttributesCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationAttributeSet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int IterationAttributeSet(int DesignIterationComponentId, int IterationAttributeId, string value)
	{
		return (int)Invoke("IterationAttributeSet", new object[3] { DesignIterationComponentId, IterationAttributeId, value })[0];
	}

	public IAsyncResult BeginIterationAttributeSet(int DesignIterationComponentId, int IterationAttributeId, string value, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationAttributeSet", new object[3] { DesignIterationComponentId, IterationAttributeId, value }, callback, asyncState);
	}

	public int EndIterationAttributeSet(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void IterationAttributeSetAsync(int DesignIterationComponentId, int IterationAttributeId, string value)
	{
		IterationAttributeSetAsync(DesignIterationComponentId, IterationAttributeId, value, null);
	}

	public void IterationAttributeSetAsync(int DesignIterationComponentId, int IterationAttributeId, string value, object userState)
	{
		if (IterationAttributeSetOperationCompleted == null)
		{
			IterationAttributeSetOperationCompleted = OnIterationAttributeSetOperationCompleted;
		}
		InvokeAsync("IterationAttributeSet", new object[3] { DesignIterationComponentId, IterationAttributeId, value }, IterationAttributeSetOperationCompleted, userState);
	}

	private void OnIterationAttributeSetOperationCompleted(object arg)
	{
		if (this.IterationAttributeSetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationAttributeSetCompleted(this, new IterationAttributeSetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationComponentAttributeSet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void IterationComponentAttributeSet(int DesignIterationComponentId, int IterationAttributeId, string value)
	{
		Invoke("IterationComponentAttributeSet", new object[3] { DesignIterationComponentId, IterationAttributeId, value });
	}

	public IAsyncResult BeginIterationComponentAttributeSet(int DesignIterationComponentId, int IterationAttributeId, string value, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationComponentAttributeSet", new object[3] { DesignIterationComponentId, IterationAttributeId, value }, callback, asyncState);
	}

	public void EndIterationComponentAttributeSet(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void IterationComponentAttributeSetAsync(int DesignIterationComponentId, int IterationAttributeId, string value)
	{
		IterationComponentAttributeSetAsync(DesignIterationComponentId, IterationAttributeId, value, null);
	}

	public void IterationComponentAttributeSetAsync(int DesignIterationComponentId, int IterationAttributeId, string value, object userState)
	{
		if (IterationComponentAttributeSetOperationCompleted == null)
		{
			IterationComponentAttributeSetOperationCompleted = OnIterationComponentAttributeSetOperationCompleted;
		}
		InvokeAsync("IterationComponentAttributeSet", new object[3] { DesignIterationComponentId, IterationAttributeId, value }, IterationComponentAttributeSetOperationCompleted, userState);
	}

	private void OnIterationComponentAttributeSetOperationCompleted(object arg)
	{
		if (this.IterationComponentAttributeSetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationComponentAttributeSetCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationComponentAttributeClear", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void IterationComponentAttributeClear(int DesignIterationComponentId, int IterationAttributeId)
	{
		Invoke("IterationComponentAttributeClear", new object[2] { DesignIterationComponentId, IterationAttributeId });
	}

	public IAsyncResult BeginIterationComponentAttributeClear(int DesignIterationComponentId, int IterationAttributeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationComponentAttributeClear", new object[2] { DesignIterationComponentId, IterationAttributeId }, callback, asyncState);
	}

	public void EndIterationComponentAttributeClear(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void IterationComponentAttributeClearAsync(int DesignIterationComponentId, int IterationAttributeId)
	{
		IterationComponentAttributeClearAsync(DesignIterationComponentId, IterationAttributeId, null);
	}

	public void IterationComponentAttributeClearAsync(int DesignIterationComponentId, int IterationAttributeId, object userState)
	{
		if (IterationComponentAttributeClearOperationCompleted == null)
		{
			IterationComponentAttributeClearOperationCompleted = OnIterationComponentAttributeClearOperationCompleted;
		}
		InvokeAsync("IterationComponentAttributeClear", new object[2] { DesignIterationComponentId, IterationAttributeId }, IterationComponentAttributeClearOperationCompleted, userState);
	}

	private void OnIterationComponentAttributeClearOperationCompleted(object arg)
	{
		if (this.IterationComponentAttributeClearCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationComponentAttributeClearCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationComponentAttributeIndexSetList", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void IterationComponentAttributeIndexSetList(int[] lstDesignIterationComponentId, int[] lstIterationAttributeId, int[] lstIndex, string[] lstValue)
	{
		Invoke("IterationComponentAttributeIndexSetList", new object[4] { lstDesignIterationComponentId, lstIterationAttributeId, lstIndex, lstValue });
	}

	public IAsyncResult BeginIterationComponentAttributeIndexSetList(int[] lstDesignIterationComponentId, int[] lstIterationAttributeId, int[] lstIndex, string[] lstValue, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationComponentAttributeIndexSetList", new object[4] { lstDesignIterationComponentId, lstIterationAttributeId, lstIndex, lstValue }, callback, asyncState);
	}

	public void EndIterationComponentAttributeIndexSetList(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void IterationComponentAttributeIndexSetListAsync(int[] lstDesignIterationComponentId, int[] lstIterationAttributeId, int[] lstIndex, string[] lstValue)
	{
		IterationComponentAttributeIndexSetListAsync(lstDesignIterationComponentId, lstIterationAttributeId, lstIndex, lstValue, null);
	}

	public void IterationComponentAttributeIndexSetListAsync(int[] lstDesignIterationComponentId, int[] lstIterationAttributeId, int[] lstIndex, string[] lstValue, object userState)
	{
		if (IterationComponentAttributeIndexSetListOperationCompleted == null)
		{
			IterationComponentAttributeIndexSetListOperationCompleted = OnIterationComponentAttributeIndexSetListOperationCompleted;
		}
		InvokeAsync("IterationComponentAttributeIndexSetList", new object[4] { lstDesignIterationComponentId, lstIterationAttributeId, lstIndex, lstValue }, IterationComponentAttributeIndexSetListOperationCompleted, userState);
	}

	private void OnIterationComponentAttributeIndexSetListOperationCompleted(object arg)
	{
		if (this.IterationComponentAttributeIndexSetListCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationComponentAttributeIndexSetListCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationComponentAttributeIndexSet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void IterationComponentAttributeIndexSet(int DesignIterationComponentId, int IterationAttributeId, int index, string value)
	{
		Invoke("IterationComponentAttributeIndexSet", new object[4] { DesignIterationComponentId, IterationAttributeId, index, value });
	}

	public IAsyncResult BeginIterationComponentAttributeIndexSet(int DesignIterationComponentId, int IterationAttributeId, int index, string value, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationComponentAttributeIndexSet", new object[4] { DesignIterationComponentId, IterationAttributeId, index, value }, callback, asyncState);
	}

	public void EndIterationComponentAttributeIndexSet(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void IterationComponentAttributeIndexSetAsync(int DesignIterationComponentId, int IterationAttributeId, int index, string value)
	{
		IterationComponentAttributeIndexSetAsync(DesignIterationComponentId, IterationAttributeId, index, value, null);
	}

	public void IterationComponentAttributeIndexSetAsync(int DesignIterationComponentId, int IterationAttributeId, int index, string value, object userState)
	{
		if (IterationComponentAttributeIndexSetOperationCompleted == null)
		{
			IterationComponentAttributeIndexSetOperationCompleted = OnIterationComponentAttributeIndexSetOperationCompleted;
		}
		InvokeAsync("IterationComponentAttributeIndexSet", new object[4] { DesignIterationComponentId, IterationAttributeId, index, value }, IterationComponentAttributeIndexSetOperationCompleted, userState);
	}

	private void OnIterationComponentAttributeIndexSetOperationCompleted(object arg)
	{
		if (this.IterationComponentAttributeIndexSetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationComponentAttributeIndexSetCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationComponentAttributeGet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string IterationComponentAttributeGet(int DesignIterationComponentId, int IterationAttributeId)
	{
		return (string)Invoke("IterationComponentAttributeGet", new object[2] { DesignIterationComponentId, IterationAttributeId })[0];
	}

	public IAsyncResult BeginIterationComponentAttributeGet(int DesignIterationComponentId, int IterationAttributeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationComponentAttributeGet", new object[2] { DesignIterationComponentId, IterationAttributeId }, callback, asyncState);
	}

	public string EndIterationComponentAttributeGet(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void IterationComponentAttributeGetAsync(int DesignIterationComponentId, int IterationAttributeId)
	{
		IterationComponentAttributeGetAsync(DesignIterationComponentId, IterationAttributeId, null);
	}

	public void IterationComponentAttributeGetAsync(int DesignIterationComponentId, int IterationAttributeId, object userState)
	{
		if (IterationComponentAttributeGetOperationCompleted == null)
		{
			IterationComponentAttributeGetOperationCompleted = OnIterationComponentAttributeGetOperationCompleted;
		}
		InvokeAsync("IterationComponentAttributeGet", new object[2] { DesignIterationComponentId, IterationAttributeId }, IterationComponentAttributeGetOperationCompleted, userState);
	}

	private void OnIterationComponentAttributeGetOperationCompleted(object arg)
	{
		if (this.IterationComponentAttributeGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationComponentAttributeGetCompleted(this, new IterationComponentAttributeGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignIterationComponantAttributesGet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable DesignIterationComponantAttributesGet(int designIterationComponentId)
	{
		return (DataTable)Invoke("DesignIterationComponantAttributesGet", new object[1] { designIterationComponentId })[0];
	}

	public IAsyncResult BeginDesignIterationComponantAttributesGet(int designIterationComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignIterationComponantAttributesGet", new object[1] { designIterationComponentId }, callback, asyncState);
	}

	public DataTable EndDesignIterationComponantAttributesGet(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void DesignIterationComponantAttributesGetAsync(int designIterationComponentId)
	{
		DesignIterationComponantAttributesGetAsync(designIterationComponentId, null);
	}

	public void DesignIterationComponantAttributesGetAsync(int designIterationComponentId, object userState)
	{
		if (DesignIterationComponantAttributesGetOperationCompleted == null)
		{
			DesignIterationComponantAttributesGetOperationCompleted = OnDesignIterationComponantAttributesGetOperationCompleted;
		}
		InvokeAsync("DesignIterationComponantAttributesGet", new object[1] { designIterationComponentId }, DesignIterationComponantAttributesGetOperationCompleted, userState);
	}

	private void OnDesignIterationComponantAttributesGetOperationCompleted(object arg)
	{
		if (this.DesignIterationComponantAttributesGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignIterationComponantAttributesGetCompleted(this, new DesignIterationComponantAttributesGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignInfo", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool GetDesignInfo(int DesignIterationComponentIDs, out int designId, out int iterationIndex, out int ComponentId, out int LinkedDICID)
	{
		object[] array = Invoke("GetDesignInfo", new object[1] { DesignIterationComponentIDs });
		designId = (int)array[1];
		iterationIndex = (int)array[2];
		ComponentId = (int)array[3];
		LinkedDICID = (int)array[4];
		return (bool)array[0];
	}

	public IAsyncResult BeginGetDesignInfo(int DesignIterationComponentIDs, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignInfo", new object[1] { DesignIterationComponentIDs }, callback, asyncState);
	}

	public bool EndGetDesignInfo(IAsyncResult asyncResult, out int designId, out int iterationIndex, out int ComponentId, out int LinkedDICID)
	{
		object[] array = EndInvoke(asyncResult);
		designId = (int)array[1];
		iterationIndex = (int)array[2];
		ComponentId = (int)array[3];
		LinkedDICID = (int)array[4];
		return (bool)array[0];
	}

	public void GetDesignInfoAsync(int DesignIterationComponentIDs)
	{
		GetDesignInfoAsync(DesignIterationComponentIDs, null);
	}

	public void GetDesignInfoAsync(int DesignIterationComponentIDs, object userState)
	{
		if (GetDesignInfoOperationCompleted == null)
		{
			GetDesignInfoOperationCompleted = OnGetDesignInfoOperationCompleted;
		}
		InvokeAsync("GetDesignInfo", new object[1] { DesignIterationComponentIDs }, GetDesignInfoOperationCompleted, userState);
	}

	private void OnGetDesignInfoOperationCompleted(object arg)
	{
		if (this.GetDesignInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignInfoCompleted(this, new GetDesignInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllBandSizes", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllBandSizes()
	{
		return (DataTable)Invoke("GetAllBandSizes", new object[0])[0];
	}

	public IAsyncResult BeginGetAllBandSizes(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllBandSizes", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllBandSizes(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllBandSizesAsync()
	{
		GetAllBandSizesAsync(null);
	}

	public void GetAllBandSizesAsync(object userState)
	{
		if (GetAllBandSizesOperationCompleted == null)
		{
			GetAllBandSizesOperationCompleted = OnGetAllBandSizesOperationCompleted;
		}
		InvokeAsync("GetAllBandSizes", new object[0], GetAllBandSizesOperationCompleted, userState);
	}

	private void OnGetAllBandSizesOperationCompleted(object arg)
	{
		if (this.GetAllBandSizesCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllBandSizesCompleted(this, new GetAllBandSizesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CreateNewBand", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int CreateNewBand(int ComponentId)
	{
		return (int)Invoke("CreateNewBand", new object[1] { ComponentId })[0];
	}

	public IAsyncResult BeginCreateNewBand(int ComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CreateNewBand", new object[1] { ComponentId }, callback, asyncState);
	}

	public int EndCreateNewBand(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void CreateNewBandAsync(int ComponentId)
	{
		CreateNewBandAsync(ComponentId, null);
	}

	public void CreateNewBandAsync(int ComponentId, object userState)
	{
		if (CreateNewBandOperationCompleted == null)
		{
			CreateNewBandOperationCompleted = OnCreateNewBandOperationCompleted;
		}
		InvokeAsync("CreateNewBand", new object[1] { ComponentId }, CreateNewBandOperationCompleted, userState);
	}

	private void OnCreateNewBandOperationCompleted(object arg)
	{
		if (this.CreateNewBandCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CreateNewBandCompleted(this, new CreateNewBandCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/CopyAttributesFromIDToLinkedID", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void CopyAttributesFromIDToLinkedID(int designIterationComponentId, int NewLinkedDICID)
	{
		Invoke("CopyAttributesFromIDToLinkedID", new object[2] { designIterationComponentId, NewLinkedDICID });
	}

	public IAsyncResult BeginCopyAttributesFromIDToLinkedID(int designIterationComponentId, int NewLinkedDICID, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("CopyAttributesFromIDToLinkedID", new object[2] { designIterationComponentId, NewLinkedDICID }, callback, asyncState);
	}

	public void EndCopyAttributesFromIDToLinkedID(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void CopyAttributesFromIDToLinkedIDAsync(int designIterationComponentId, int NewLinkedDICID)
	{
		CopyAttributesFromIDToLinkedIDAsync(designIterationComponentId, NewLinkedDICID, null);
	}

	public void CopyAttributesFromIDToLinkedIDAsync(int designIterationComponentId, int NewLinkedDICID, object userState)
	{
		if (CopyAttributesFromIDToLinkedIDOperationCompleted == null)
		{
			CopyAttributesFromIDToLinkedIDOperationCompleted = OnCopyAttributesFromIDToLinkedIDOperationCompleted;
		}
		InvokeAsync("CopyAttributesFromIDToLinkedID", new object[2] { designIterationComponentId, NewLinkedDICID }, CopyAttributesFromIDToLinkedIDOperationCompleted, userState);
	}

	private void OnCopyAttributesFromIDToLinkedIDOperationCompleted(object arg)
	{
		if (this.CopyAttributesFromIDToLinkedIDCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.CopyAttributesFromIDToLinkedIDCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/SetLinkedIDTo", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public void SetLinkedIDTo(int designIterationComponentId, int LinkedDICID)
	{
		Invoke("SetLinkedIDTo", new object[2] { designIterationComponentId, LinkedDICID });
	}

	public IAsyncResult BeginSetLinkedIDTo(int designIterationComponentId, int LinkedDICID, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("SetLinkedIDTo", new object[2] { designIterationComponentId, LinkedDICID }, callback, asyncState);
	}

	public void EndSetLinkedIDTo(IAsyncResult asyncResult)
	{
		EndInvoke(asyncResult);
	}

	public void SetLinkedIDToAsync(int designIterationComponentId, int LinkedDICID)
	{
		SetLinkedIDToAsync(designIterationComponentId, LinkedDICID, null);
	}

	public void SetLinkedIDToAsync(int designIterationComponentId, int LinkedDICID, object userState)
	{
		if (SetLinkedIDToOperationCompleted == null)
		{
			SetLinkedIDToOperationCompleted = OnSetLinkedIDToOperationCompleted;
		}
		InvokeAsync("SetLinkedIDTo", new object[2] { designIterationComponentId, LinkedDICID }, SetLinkedIDToOperationCompleted, userState);
	}

	private void OnSetLinkedIDToOperationCompleted(object arg)
	{
		if (this.SetLinkedIDToCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.SetLinkedIDToCompleted(this, new AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDICIDsFromLinkedDICID", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int[] GetDICIDsFromLinkedDICID(int LinkedDICID)
	{
		return (int[])Invoke("GetDICIDsFromLinkedDICID", new object[1] { LinkedDICID })[0];
	}

	public IAsyncResult BeginGetDICIDsFromLinkedDICID(int LinkedDICID, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDICIDsFromLinkedDICID", new object[1] { LinkedDICID }, callback, asyncState);
	}

	public int[] EndGetDICIDsFromLinkedDICID(IAsyncResult asyncResult)
	{
		return (int[])EndInvoke(asyncResult)[0];
	}

	public void GetDICIDsFromLinkedDICIDAsync(int LinkedDICID)
	{
		GetDICIDsFromLinkedDICIDAsync(LinkedDICID, null);
	}

	public void GetDICIDsFromLinkedDICIDAsync(int LinkedDICID, object userState)
	{
		if (GetDICIDsFromLinkedDICIDOperationCompleted == null)
		{
			GetDICIDsFromLinkedDICIDOperationCompleted = OnGetDICIDsFromLinkedDICIDOperationCompleted;
		}
		InvokeAsync("GetDICIDsFromLinkedDICID", new object[1] { LinkedDICID }, GetDICIDsFromLinkedDICIDOperationCompleted, userState);
	}

	private void OnGetDICIDsFromLinkedDICIDOperationCompleted(object arg)
	{
		if (this.GetDICIDsFromLinkedDICIDCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDICIDsFromLinkedDICIDCompleted(this, new GetDICIDsFromLinkedDICIDCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetValidIterationsList", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int[] GetValidIterationsList(int DesignId, bool justPrimary)
	{
		return (int[])Invoke("GetValidIterationsList", new object[2] { DesignId, justPrimary })[0];
	}

	public IAsyncResult BeginGetValidIterationsList(int DesignId, bool justPrimary, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetValidIterationsList", new object[2] { DesignId, justPrimary }, callback, asyncState);
	}

	public int[] EndGetValidIterationsList(IAsyncResult asyncResult)
	{
		return (int[])EndInvoke(asyncResult)[0];
	}

	public void GetValidIterationsListAsync(int DesignId, bool justPrimary)
	{
		GetValidIterationsListAsync(DesignId, justPrimary, null);
	}

	public void GetValidIterationsListAsync(int DesignId, bool justPrimary, object userState)
	{
		if (GetValidIterationsListOperationCompleted == null)
		{
			GetValidIterationsListOperationCompleted = OnGetValidIterationsListOperationCompleted;
		}
		InvokeAsync("GetValidIterationsList", new object[2] { DesignId, justPrimary }, GetValidIterationsListOperationCompleted, userState);
	}

	private void OnGetValidIterationsListOperationCompleted(object arg)
	{
		if (this.GetValidIterationsListCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetValidIterationsListCompleted(this, new GetValidIterationsListCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/getMatchingBands", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable getMatchingBands(int designIterationComponentId)
	{
		return (DataTable)Invoke("getMatchingBands", new object[1] { designIterationComponentId })[0];
	}

	public IAsyncResult BegingetMatchingBands(int designIterationComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("getMatchingBands", new object[1] { designIterationComponentId }, callback, asyncState);
	}

	public DataTable EndgetMatchingBands(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void getMatchingBandsAsync(int designIterationComponentId)
	{
		getMatchingBandsAsync(designIterationComponentId, null);
	}

	public void getMatchingBandsAsync(int designIterationComponentId, object userState)
	{
		if (getMatchingBandsOperationCompleted == null)
		{
			getMatchingBandsOperationCompleted = OngetMatchingBandsOperationCompleted;
		}
		InvokeAsync("getMatchingBands", new object[1] { designIterationComponentId }, getMatchingBandsOperationCompleted, userState);
	}

	private void OngetMatchingBandsOperationCompleted(object arg)
	{
		if (this.getMatchingBandsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.getMatchingBandsCompleted(this, new getMatchingBandsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignIteraionAdd", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int DesignIteraionAdd(int DesignId, int IterationIndex)
	{
		return (int)Invoke("DesignIteraionAdd", new object[2] { DesignId, IterationIndex })[0];
	}

	public IAsyncResult BeginDesignIteraionAdd(int DesignId, int IterationIndex, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignIteraionAdd", new object[2] { DesignId, IterationIndex }, callback, asyncState);
	}

	public int EndDesignIteraionAdd(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void DesignIteraionAddAsync(int DesignId, int IterationIndex)
	{
		DesignIteraionAddAsync(DesignId, IterationIndex, null);
	}

	public void DesignIteraionAddAsync(int DesignId, int IterationIndex, object userState)
	{
		if (DesignIteraionAddOperationCompleted == null)
		{
			DesignIteraionAddOperationCompleted = OnDesignIteraionAddOperationCompleted;
		}
		InvokeAsync("DesignIteraionAdd", new object[2] { DesignId, IterationIndex }, DesignIteraionAddOperationCompleted, userState);
	}

	private void OnDesignIteraionAddOperationCompleted(object arg)
	{
		if (this.DesignIteraionAddCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignIteraionAddCompleted(this, new DesignIteraionAddCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DesignNumbersGet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool DesignNumbersGet(int DesignIterationComponentId, out int DesignId, out int IterationIndex, out int ComponentId)
	{
		object[] array = Invoke("DesignNumbersGet", new object[1] { DesignIterationComponentId });
		DesignId = (int)array[1];
		IterationIndex = (int)array[2];
		ComponentId = (int)array[3];
		return (bool)array[0];
	}

	public IAsyncResult BeginDesignNumbersGet(int DesignIterationComponentId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DesignNumbersGet", new object[1] { DesignIterationComponentId }, callback, asyncState);
	}

	public bool EndDesignNumbersGet(IAsyncResult asyncResult, out int DesignId, out int IterationIndex, out int ComponentId)
	{
		object[] array = EndInvoke(asyncResult);
		DesignId = (int)array[1];
		IterationIndex = (int)array[2];
		ComponentId = (int)array[3];
		return (bool)array[0];
	}

	public void DesignNumbersGetAsync(int DesignIterationComponentId)
	{
		DesignNumbersGetAsync(DesignIterationComponentId, null);
	}

	public void DesignNumbersGetAsync(int DesignIterationComponentId, object userState)
	{
		if (DesignNumbersGetOperationCompleted == null)
		{
			DesignNumbersGetOperationCompleted = OnDesignNumbersGetOperationCompleted;
		}
		InvokeAsync("DesignNumbersGet", new object[1] { DesignIterationComponentId }, DesignNumbersGetOperationCompleted, userState);
	}

	private void OnDesignNumbersGetOperationCompleted(object arg)
	{
		if (this.DesignNumbersGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DesignNumbersGetCompleted(this, new DesignNumbersGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetIterationOptions", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string[] GetIterationOptions()
	{
		return (string[])Invoke("GetIterationOptions", new object[0])[0];
	}

	public IAsyncResult BeginGetIterationOptions(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetIterationOptions", new object[0], callback, asyncState);
	}

	public string[] EndGetIterationOptions(IAsyncResult asyncResult)
	{
		return (string[])EndInvoke(asyncResult)[0];
	}

	public void GetIterationOptionsAsync()
	{
		GetIterationOptionsAsync(null);
	}

	public void GetIterationOptionsAsync(object userState)
	{
		if (GetIterationOptionsOperationCompleted == null)
		{
			GetIterationOptionsOperationCompleted = OnGetIterationOptionsOperationCompleted;
		}
		InvokeAsync("GetIterationOptions", new object[0], GetIterationOptionsOperationCompleted, userState);
	}

	private void OnGetIterationOptionsOperationCompleted(object arg)
	{
		if (this.GetIterationOptionsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetIterationOptionsCompleted(this, new GetIterationOptionsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetIterationAttributesByTypeId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public IterationAttribute[] GetIterationAttributesByTypeId(int iterationAttributeTypeId)
	{
		return (IterationAttribute[])Invoke("GetIterationAttributesByTypeId", new object[1] { iterationAttributeTypeId })[0];
	}

	public IAsyncResult BeginGetIterationAttributesByTypeId(int iterationAttributeTypeId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetIterationAttributesByTypeId", new object[1] { iterationAttributeTypeId }, callback, asyncState);
	}

	public IterationAttribute[] EndGetIterationAttributesByTypeId(IAsyncResult asyncResult)
	{
		return (IterationAttribute[])EndInvoke(asyncResult)[0];
	}

	public void GetIterationAttributesByTypeIdAsync(int iterationAttributeTypeId)
	{
		GetIterationAttributesByTypeIdAsync(iterationAttributeTypeId, null);
	}

	public void GetIterationAttributesByTypeIdAsync(int iterationAttributeTypeId, object userState)
	{
		if (GetIterationAttributesByTypeIdOperationCompleted == null)
		{
			GetIterationAttributesByTypeIdOperationCompleted = OnGetIterationAttributesByTypeIdOperationCompleted;
		}
		InvokeAsync("GetIterationAttributesByTypeId", new object[1] { iterationAttributeTypeId }, GetIterationAttributesByTypeIdOperationCompleted, userState);
	}

	private void OnGetIterationAttributesByTypeIdOperationCompleted(object arg)
	{
		if (this.GetIterationAttributesByTypeIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetIterationAttributesByTypeIdCompleted(this, new GetIterationAttributesByTypeIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationAttributeIdGet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public int IterationAttributeIdGet(int AttributeTypeId, int IterationAttributeIndex)
	{
		return (int)Invoke("IterationAttributeIdGet", new object[2] { AttributeTypeId, IterationAttributeIndex })[0];
	}

	public IAsyncResult BeginIterationAttributeIdGet(int AttributeTypeId, int IterationAttributeIndex, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationAttributeIdGet", new object[2] { AttributeTypeId, IterationAttributeIndex }, callback, asyncState);
	}

	public int EndIterationAttributeIdGet(IAsyncResult asyncResult)
	{
		return (int)EndInvoke(asyncResult)[0];
	}

	public void IterationAttributeIdGetAsync(int AttributeTypeId, int IterationAttributeIndex)
	{
		IterationAttributeIdGetAsync(AttributeTypeId, IterationAttributeIndex, null);
	}

	public void IterationAttributeIdGetAsync(int AttributeTypeId, int IterationAttributeIndex, object userState)
	{
		if (IterationAttributeIdGetOperationCompleted == null)
		{
			IterationAttributeIdGetOperationCompleted = OnIterationAttributeIdGetOperationCompleted;
		}
		InvokeAsync("IterationAttributeIdGet", new object[2] { AttributeTypeId, IterationAttributeIndex }, IterationAttributeIdGetOperationCompleted, userState);
	}

	private void OnIterationAttributeIdGetOperationCompleted(object arg)
	{
		if (this.IterationAttributeIdGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationAttributeIdGetCompleted(this, new IterationAttributeIdGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetValidIterations", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetValidIterations(int designId)
	{
		return (DataTable)Invoke("GetValidIterations", new object[1] { designId })[0];
	}

	public IAsyncResult BeginGetValidIterations(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetValidIterations", new object[1] { designId }, callback, asyncState);
	}

	public DataTable EndGetValidIterations(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetValidIterationsAsync(int designId)
	{
		GetValidIterationsAsync(designId, null);
	}

	public void GetValidIterationsAsync(int designId, object userState)
	{
		if (GetValidIterationsOperationCompleted == null)
		{
			GetValidIterationsOperationCompleted = OnGetValidIterationsOperationCompleted;
		}
		InvokeAsync("GetValidIterations", new object[1] { designId }, GetValidIterationsOperationCompleted, userState);
	}

	private void OnGetValidIterationsOperationCompleted(object arg)
	{
		if (this.GetValidIterationsCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetValidIterationsCompleted(this, new GetValidIterationsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/IterationComponentAttributeIndexGet", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string IterationComponentAttributeIndexGet(int DesignIterationComponentId, int IterationAttributeId, int index)
	{
		return (string)Invoke("IterationComponentAttributeIndexGet", new object[3] { DesignIterationComponentId, IterationAttributeId, index })[0];
	}

	public IAsyncResult BeginIterationComponentAttributeIndexGet(int DesignIterationComponentId, int IterationAttributeId, int index, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("IterationComponentAttributeIndexGet", new object[3] { DesignIterationComponentId, IterationAttributeId, index }, callback, asyncState);
	}

	public string EndIterationComponentAttributeIndexGet(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void IterationComponentAttributeIndexGetAsync(int DesignIterationComponentId, int IterationAttributeId, int index)
	{
		IterationComponentAttributeIndexGetAsync(DesignIterationComponentId, IterationAttributeId, index, null);
	}

	public void IterationComponentAttributeIndexGetAsync(int DesignIterationComponentId, int IterationAttributeId, int index, object userState)
	{
		if (IterationComponentAttributeIndexGetOperationCompleted == null)
		{
			IterationComponentAttributeIndexGetOperationCompleted = OnIterationComponentAttributeIndexGetOperationCompleted;
		}
		InvokeAsync("IterationComponentAttributeIndexGet", new object[3] { DesignIterationComponentId, IterationAttributeId, index }, IterationComponentAttributeIndexGetOperationCompleted, userState);
	}

	private void OnIterationComponentAttributeIndexGetOperationCompleted(object arg)
	{
		if (this.IterationComponentAttributeIndexGetCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.IterationComponentAttributeIndexGetCompleted(this, new IterationComponentAttributeIndexGetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetIterationGemInfo", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool GetIterationGemInfo(int DesignIterationComponentId, int index, out string STONE_ID, out string STONE_NAME, out string STONE_SHAPE, out string STONE_GROUP, out int STONE_LOCATION_INDEX, out string SET_TYPE, out string sXYZ, out string cXYZ, out string xXYZ, out string yXYZ)
	{
		object[] array = Invoke("GetIterationGemInfo", new object[2] { DesignIterationComponentId, index });
		STONE_ID = (string)array[1];
		STONE_NAME = (string)array[2];
		STONE_SHAPE = (string)array[3];
		STONE_GROUP = (string)array[4];
		STONE_LOCATION_INDEX = (int)array[5];
		SET_TYPE = (string)array[6];
		sXYZ = (string)array[7];
		cXYZ = (string)array[8];
		xXYZ = (string)array[9];
		yXYZ = (string)array[10];
		return (bool)array[0];
	}

	public IAsyncResult BeginGetIterationGemInfo(int DesignIterationComponentId, int index, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetIterationGemInfo", new object[2] { DesignIterationComponentId, index }, callback, asyncState);
	}

	public bool EndGetIterationGemInfo(IAsyncResult asyncResult, out string STONE_ID, out string STONE_NAME, out string STONE_SHAPE, out string STONE_GROUP, out int STONE_LOCATION_INDEX, out string SET_TYPE, out string sXYZ, out string cXYZ, out string xXYZ, out string yXYZ)
	{
		object[] array = EndInvoke(asyncResult);
		STONE_ID = (string)array[1];
		STONE_NAME = (string)array[2];
		STONE_SHAPE = (string)array[3];
		STONE_GROUP = (string)array[4];
		STONE_LOCATION_INDEX = (int)array[5];
		SET_TYPE = (string)array[6];
		sXYZ = (string)array[7];
		cXYZ = (string)array[8];
		xXYZ = (string)array[9];
		yXYZ = (string)array[10];
		return (bool)array[0];
	}

	public void GetIterationGemInfoAsync(int DesignIterationComponentId, int index)
	{
		GetIterationGemInfoAsync(DesignIterationComponentId, index, null);
	}

	public void GetIterationGemInfoAsync(int DesignIterationComponentId, int index, object userState)
	{
		if (GetIterationGemInfoOperationCompleted == null)
		{
			GetIterationGemInfoOperationCompleted = OnGetIterationGemInfoOperationCompleted;
		}
		InvokeAsync("GetIterationGemInfo", new object[2] { DesignIterationComponentId, index }, GetIterationGemInfoOperationCompleted, userState);
	}

	private void OnGetIterationGemInfoOperationCompleted(object arg)
	{
		if (this.GetIterationGemInfoCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetIterationGemInfoCompleted(this, new GetIterationGemInfoCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignIterationComponentId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignIterationComponentId()
	{
		return (DataTable)Invoke("GetAllDesignIterationComponentId", new object[0])[0];
	}

	public IAsyncResult BeginGetAllDesignIterationComponentId(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignIterationComponentId", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllDesignIterationComponentId(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignIterationComponentIdAsync()
	{
		GetAllDesignIterationComponentIdAsync(null);
	}

	public void GetAllDesignIterationComponentIdAsync(object userState)
	{
		if (GetAllDesignIterationComponentIdOperationCompleted == null)
		{
			GetAllDesignIterationComponentIdOperationCompleted = OnGetAllDesignIterationComponentIdOperationCompleted;
		}
		InvokeAsync("GetAllDesignIterationComponentId", new object[0], GetAllDesignIterationComponentIdOperationCompleted, userState);
	}

	private void OnGetAllDesignIterationComponentIdOperationCompleted(object arg)
	{
		if (this.GetAllDesignIterationComponentIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignIterationComponentIdCompleted(this, new GetAllDesignIterationComponentIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetAllDesignIterationComponentIdFound", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetAllDesignIterationComponentIdFound()
	{
		return (DataTable)Invoke("GetAllDesignIterationComponentIdFound", new object[0])[0];
	}

	public IAsyncResult BeginGetAllDesignIterationComponentIdFound(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetAllDesignIterationComponentIdFound", new object[0], callback, asyncState);
	}

	public DataTable EndGetAllDesignIterationComponentIdFound(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetAllDesignIterationComponentIdFoundAsync()
	{
		GetAllDesignIterationComponentIdFoundAsync(null);
	}

	public void GetAllDesignIterationComponentIdFoundAsync(object userState)
	{
		if (GetAllDesignIterationComponentIdFoundOperationCompleted == null)
		{
			GetAllDesignIterationComponentIdFoundOperationCompleted = OnGetAllDesignIterationComponentIdFoundOperationCompleted;
		}
		InvokeAsync("GetAllDesignIterationComponentIdFound", new object[0], GetAllDesignIterationComponentIdFoundOperationCompleted, userState);
	}

	private void OnGetAllDesignIterationComponentIdFoundOperationCompleted(object arg)
	{
		if (this.GetAllDesignIterationComponentIdFoundCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetAllDesignIterationComponentIdFoundCompleted(this, new GetAllDesignIterationComponentIdFoundCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/StullerServicesAreActive", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public bool StullerServicesAreActive()
	{
		return (bool)Invoke("StullerServicesAreActive", new object[0])[0];
	}

	public IAsyncResult BeginStullerServicesAreActive(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("StullerServicesAreActive", new object[0], callback, asyncState);
	}

	public bool EndStullerServicesAreActive(IAsyncResult asyncResult)
	{
		return (bool)EndInvoke(asyncResult)[0];
	}

	public void StullerServicesAreActiveAsync()
	{
		StullerServicesAreActiveAsync(null);
	}

	public void StullerServicesAreActiveAsync(object userState)
	{
		if (StullerServicesAreActiveOperationCompleted == null)
		{
			StullerServicesAreActiveOperationCompleted = OnStullerServicesAreActiveOperationCompleted;
		}
		InvokeAsync("StullerServicesAreActive", new object[0], StullerServicesAreActiveOperationCompleted, userState);
	}

	private void OnStullerServicesAreActiveOperationCompleted(object arg)
	{
		if (this.StullerServicesAreActiveCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.StullerServicesAreActiveCompleted(this, new StullerServicesAreActiveCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetLatestDesignerVersionString", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetLatestDesignerVersionString()
	{
		return (string)Invoke("GetLatestDesignerVersionString", new object[0])[0];
	}

	public IAsyncResult BeginGetLatestDesignerVersionString(AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetLatestDesignerVersionString", new object[0], callback, asyncState);
	}

	public string EndGetLatestDesignerVersionString(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetLatestDesignerVersionStringAsync()
	{
		GetLatestDesignerVersionStringAsync(null);
	}

	public void GetLatestDesignerVersionStringAsync(object userState)
	{
		if (GetLatestDesignerVersionStringOperationCompleted == null)
		{
			GetLatestDesignerVersionStringOperationCompleted = OnGetLatestDesignerVersionStringOperationCompleted;
		}
		InvokeAsync("GetLatestDesignerVersionString", new object[0], GetLatestDesignerVersionStringOperationCompleted, userState);
	}

	private void OnGetLatestDesignerVersionStringOperationCompleted(object arg)
	{
		if (this.GetLatestDesignerVersionStringCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetLatestDesignerVersionStringCompleted(this, new GetLatestDesignerVersionStringCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignHistory", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public DataTable GetDesignHistory(int DesignId)
	{
		return (DataTable)Invoke("GetDesignHistory", new object[1] { DesignId })[0];
	}

	public IAsyncResult BeginGetDesignHistory(int DesignId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignHistory", new object[1] { DesignId }, callback, asyncState);
	}

	public DataTable EndGetDesignHistory(IAsyncResult asyncResult)
	{
		return (DataTable)EndInvoke(asyncResult)[0];
	}

	public void GetDesignHistoryAsync(int DesignId)
	{
		GetDesignHistoryAsync(DesignId, null);
	}

	public void GetDesignHistoryAsync(int DesignId, object userState)
	{
		if (GetDesignHistoryOperationCompleted == null)
		{
			GetDesignHistoryOperationCompleted = OnGetDesignHistoryOperationCompleted;
		}
		InvokeAsync("GetDesignHistory", new object[1] { DesignId }, GetDesignHistoryOperationCompleted, userState);
	}

	private void OnGetDesignHistoryOperationCompleted(object arg)
	{
		if (this.GetDesignHistoryCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignHistoryCompleted(this, new GetDesignHistoryCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/DownloadImage", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	[return: XmlElement(DataType = "base64Binary")]
	public byte[] DownloadImage(int designId)
	{
		return (byte[])Invoke("DownloadImage", new object[1] { designId })[0];
	}

	public IAsyncResult BeginDownloadImage(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("DownloadImage", new object[1] { designId }, callback, asyncState);
	}

	public byte[] EndDownloadImage(IAsyncResult asyncResult)
	{
		return (byte[])EndInvoke(asyncResult)[0];
	}

	public void DownloadImageAsync(int designId)
	{
		DownloadImageAsync(designId, null);
	}

	public void DownloadImageAsync(int designId, object userState)
	{
		if (DownloadImageOperationCompleted == null)
		{
			DownloadImageOperationCompleted = OnDownloadImageOperationCompleted;
		}
		InvokeAsync("DownloadImage", new object[1] { designId }, DownloadImageOperationCompleted, userState);
	}

	private void OnDownloadImageOperationCompleted(object arg)
	{
		if (this.DownloadImageCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.DownloadImageCompleted(this, new DownloadImageCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	[SoapDocumentMethod("http://CounterSketch.biz/GetDesignFilenameByDesignId", RequestNamespace = "http://CounterSketch.biz/", ResponseNamespace = "http://CounterSketch.biz/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
	public string GetDesignFilenameByDesignId(int designId)
	{
		return (string)Invoke("GetDesignFilenameByDesignId", new object[1] { designId })[0];
	}

	public IAsyncResult BeginGetDesignFilenameByDesignId(int designId, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("GetDesignFilenameByDesignId", new object[1] { designId }, callback, asyncState);
	}

	public string EndGetDesignFilenameByDesignId(IAsyncResult asyncResult)
	{
		return (string)EndInvoke(asyncResult)[0];
	}

	public void GetDesignFilenameByDesignIdAsync(int designId)
	{
		GetDesignFilenameByDesignIdAsync(designId, null);
	}

	public void GetDesignFilenameByDesignIdAsync(int designId, object userState)
	{
		if (GetDesignFilenameByDesignIdOperationCompleted == null)
		{
			GetDesignFilenameByDesignIdOperationCompleted = OnGetDesignFilenameByDesignIdOperationCompleted;
		}
		InvokeAsync("GetDesignFilenameByDesignId", new object[1] { designId }, GetDesignFilenameByDesignIdOperationCompleted, userState);
	}

	private void OnGetDesignFilenameByDesignIdOperationCompleted(object arg)
	{
		if (this.GetDesignFilenameByDesignIdCompleted != null)
		{
			InvokeCompletedEventArgs e = (InvokeCompletedEventArgs)arg;
			this.GetDesignFilenameByDesignIdCompleted(this, new GetDesignFilenameByDesignIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public new void CancelAsync(object userState)
	{
		base.CancelAsync(userState);
	}
}
