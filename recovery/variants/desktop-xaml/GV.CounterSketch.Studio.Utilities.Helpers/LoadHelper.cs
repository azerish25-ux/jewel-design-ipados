using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Controls.BLL;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.LibraryServices;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class LoadHelper
{
	public static GV.CounterSketch.Studio.Model.Objects.Design UrlProtocolRequestedDesign { get; private set; }

	public static string DoubleClickFileOpenRequest { get; set; }

	public static string RequestedFingerSizeUS { get; set; }

	public static void ExtractAndLoadDesignFromURL()
	{
		try
		{
			if (UrlProtocolRequestedDesign != null)
			{
				string path = $"{UrlProtocolRequestedDesign.BaseDesignInstance}_{UrlProtocolRequestedDesign.BaseDesignInstance}.csg";
				UrlProtocolRequestedDesign.Path = Path.Combine(Path.GetTempPath(), path);
				GlobalErrorHandler.WriteInfoToEventLog("ExtractAndLoadDesignFromURL: " + UrlProtocolRequestedDesign.Path, 2);
				GV.CounterSketch.Studio.Model.Objects.Design design = ProjectManagementHelper.ExtractDesignFromLibrary(UrlProtocolRequestedDesign);
				ProjectManagementHelper.AddDesignFromLibraryToActiveProject(design);
				design.IsLoadedFromServiceRequest = true;
				LoadDesign(design);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			UrlProtocolRequestedDesign = null;
		}
	}

	public static void LoadDesign(string tempFile)
	{
		LoadDesign(new GV.CounterSketch.Studio.Model.Objects.Design
		{
			Path = tempFile
		});
	}

	public static void LoadDesign(GV.CounterSketch.Studio.Model.Objects.Design design)
	{
		if (ProjectManagementHelper.ContinueAfterSaveDesignPrompt())
		{
			UIHelper.ShowWaitWindow(waitVisible: true);
			Stopwatch stopwatchOverAll = new Stopwatch();
			Stopwatch stopwatch = new Stopwatch();
			GlobalErrorHandler.WriteInfoToEventLog($"Loading Design: {design.DesignId}", 2);
			stopwatchOverAll.Start();
			stopwatch.Start();
			stopwatch.Reset();
			stopwatch.Start();
			Guid centerGemControlGuid = SearchHelper.GetCenterGemControlGuid(design.DesignId);
			int? centerGemControlGraphIndex = SearchHelper.GetCenterGemControlGraphIndex(design.DesignId);
			UIHelper.CoreDataService.OpenDocument(design.Path, delegate(IEnumerable<UIControl> uiControls, Exception exception)
			{
				LoadDesignCallback(design, stopwatch, exception, uiControls, stopwatchOverAll);
			}, centerGemControlGuid, centerGemControlGraphIndex);
		}
	}

	public static void SetUrlProtocolRequestedDesign(string initialModel, string metalMaterialOne, string metalMaterialTwo, string gemMaterialOne, string gemMaterialTwo, string gemMaterialThree, string iterationIndex = null, string iterationId = null, string designVersion = null, string fingerSizeUS = null)
	{
		GlobalErrorHandler.WriteInfoToEventLog("SetUrlProtocolRequestedDesign: Initializing Design Object - " + initialModel + ", " + metalMaterialOne + ", " + metalMaterialTwo + ", " + gemMaterialOne + ", " + gemMaterialTwo + ", " + gemMaterialThree + ", " + iterationIndex + ", " + iterationId + ", " + designVersion + ", " + fingerSizeUS, 2);
		int.TryParse(initialModel, out var designId);
		if (SearchHelper.GetDesignSearchByDesignId(designId) != null)
		{
			GV.CounterSketch.Studio.Model.Objects.Design designFromDesignSearch = ProjectManagementHelper.GetDesignFromDesignSearch(SearchHelper.GetDesignSearchByDesignId(designId));
			designFromDesignSearch.MaterialMetal1 = ((!int.TryParse(metalMaterialOne, out var result)) ? 1 : ((!Enum.IsDefined(typeof(MetalMaterial), result)) ? 1 : result));
			designFromDesignSearch.MaterialMetal2 = ((!int.TryParse(metalMaterialTwo, out result)) ? 1 : ((!Enum.IsDefined(typeof(MetalMaterial), result)) ? 1 : result));
			designFromDesignSearch.MaterialGem1 = ((!int.TryParse(gemMaterialOne, out result)) ? 105 : (Enum.IsDefined(typeof(GemMaterial), result) ? result : 105));
			designFromDesignSearch.MaterialGem2 = ((!int.TryParse(gemMaterialTwo, out result)) ? 105 : (Enum.IsDefined(typeof(GemMaterial), result) ? result : 105));
			designFromDesignSearch.MaterialGem3 = ((!int.TryParse(gemMaterialThree, out result)) ? 105 : (Enum.IsDefined(typeof(GemMaterial), result) ? result : 105));
			int result2;
			if (!string.IsNullOrWhiteSpace(iterationIndex))
			{
				if (int.TryParse(iterationIndex, out result2))
				{
					designFromDesignSearch.SelectedIterationId = result2;
				}
			}
			else
			{
				int.TryParse(designVersion, out var requestedVersion);
				if (requestedVersion != 0 && designFromDesignSearch.Version != requestedVersion)
				{
					Application.Current.Dispatcher.Invoke(delegate
					{
						WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.MessageOnly);
						windowMessage.Message = $"Requested Design [{designId}] Version [{requestedVersion}] is not available in your design library.";
						windowMessage.Owner = Application.Current.MainWindow;
						windowMessage.Title = "CounterSketch Design Request Error";
						windowMessage.Topmost = true;
						windowMessage.Show();
					});
				}
				int.TryParse(iterationId, out var result3);
				RequestedFingerSizeUS = fingerSizeUS;
				if (!UIHelper.IsInternetAccessible())
				{
					return;
				}
				using (LibraryServiceClient libraryServiceClient = new LibraryServiceClient())
				{
					result2 = libraryServiceClient.GetIterationIndexForIterationComponentIdAndVersionNumber(result3, requestedVersion);
				}
				designFromDesignSearch.SelectedIterationId = result2;
			}
			UrlProtocolRequestedDesign = designFromDesignSearch;
		}
		else
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.MessageOnly);
				windowMessage.Message = "Invalid Design ID";
				windowMessage.Owner = Application.Current.MainWindow;
				windowMessage.Title = "CounterSketch Design Request Error";
				windowMessage.Show();
			});
		}
	}

	private static void LoadDesignCallback(GV.CounterSketch.Studio.Model.Objects.Design design, Stopwatch stopwatch, Exception exception, IEnumerable<UIControl> uiControls, Stopwatch stopwatchOverAll)
	{
		GlobalErrorHandler.WriteInfoToEventLog($"\t Open Document Completed: {stopwatch.ElapsedMilliseconds}ms", 2);
		if (exception != null)
		{
			throw exception;
		}
		stopwatch.Reset();
		stopwatch.Start();
		UIHelper.DefaultRingSizeIndex = -1;
		GlobalErrorHandler.WriteInfoToEventLog($"\t Populating Design Stack controls Completed: {stopwatch.ElapsedMilliseconds}ms", 2);
		stopwatch.Reset();
		stopwatch.Start();
		if (!string.IsNullOrEmpty(UIHelper.LoadedDesign.Path) && Path.GetExtension(UIHelper.LoadedDesign.Path) != ".csg" && File.Exists(UIHelper.LoadedDesign.Path))
		{
			File.Delete(UIHelper.LoadedDesign.Path);
		}
		UIHelper.LoadedDesign = design;
		GlobalErrorHandler.WriteInfoToEventLog($"\t Cleaning temporary files Completed: {stopwatch.ElapsedMilliseconds}ms", 2);
		stopwatch.Reset();
		stopwatch.Start();
		UIHelper.LoadedDesign.SelectedIterationId = design.SelectedIterationId;
		ViewModelLocator.Current.DesignStackVM.UpdateStackFromCurrentIterationId();
		ViewModelLocator.Current.FreehandStackVM.Reset();
		ViewModelLocator.Current.CustomizePartsListVM.Reset();
		ViewModelLocator.Current.DesignStackVM.InitializeStack(uiControls);
		ViewModelLocator.Current.FreehandStackVM.IsFreehandCheckBoxActive = UIHelper.CoreDataService.GetFreehandComponentVisibility();
		if (design.IsLoadedFromLibrary)
		{
			UIHelper.CoreDataService.MaterialsSetGem(new GemMaterial[3]
			{
				(GemMaterial)design.MaterialGem1,
				(GemMaterial)design.MaterialGem2,
				(GemMaterial)design.MaterialGem3
			});
			GlobalErrorHandler.WriteInfoToEventLog($"\t Setting Gem Materials from Selections: Primary - {(GemMaterial)design.MaterialGem1}, Secondary - {(GemMaterial)design.MaterialGem2}, Tertiary - {(GemMaterial)design.MaterialGem3}", 2);
			UIHelper.CoreDataService.MaterialsSetMetal(new MetalMaterial[2]
			{
				(MetalMaterial)design.MaterialMetal1,
				(MetalMaterial)design.MaterialMetal2
			});
			GlobalErrorHandler.WriteInfoToEventLog($"\t Setting Metal Materials from Selections: Primary - {(MetalMaterial)design.MaterialMetal1}, Secondary - {(MetalMaterial)design.MaterialMetal2}", 2);
			UIHelper.CoreDataService.ClearExpandedSliders();
		}
		if (!design.IsLoadedFromLibrary || design.IsLoadedFromServiceRequest)
		{
			ViewModelLocator.Current.StudioMaterialSelectionVM.SetMaterialsFromDesign();
		}
		if (design.IsLoadedFromServiceRequest && !string.IsNullOrWhiteSpace(RequestedFingerSizeUS))
		{
			int num = (int)UIHelper.CoreDataService.DocumentGetRingSizeIndexFromSizeText(RequestedFingerSizeUS);
			if (num >= 0)
			{
				CSValueRingSizeControl cSValueRingSizeControl = ViewModelLocator.Current.DesignStackVM.DesignOptionsUICollection.OfType<CSValueRingSizeControl>()?.FirstOrDefault();
				if (cSValueRingSizeControl != null)
				{
					cSValueRingSizeControl.LocalUIControl.Value1 = num;
					cSValueRingSizeControl.SetGraphValue(isQuickSet: false);
				}
			}
		}
		UIHelper.CoreDataService.DocumentSetCurvesAndPointsVisibility(visible: false);
		GlobalErrorHandler.WriteInfoToEventLog($"\t Updating Render Materials Completed: {stopwatch.ElapsedMilliseconds}ms", 2);
		stopwatch.Reset();
		stopwatch.Start();
		ViewModelLocator.Current.GemReportVM.ResetPreviousQualityAssignments();
		UIHelper.IsOrderedDesignLoaded = OrderHelper.DeserializeOrderInformationFromDesignUserData();
		if (!UIHelper.IsOrderedDesignLoaded)
		{
			ViewModelLocator.Current.GemReportVM.Populate();
			ViewModelLocator.Current.OrderSummaryVM.Reset(skipVolume: true, skipGemShop: true);
		}
		GlobalErrorHandler.WriteInfoToEventLog($"\t Recreating Order Controls Completed: {stopwatch.ElapsedMilliseconds}ms", 2);
		stopwatch.Reset();
		stopwatch.Start();
		if (string.IsNullOrEmpty(UIHelper.LoadedDesign.SPINumber))
		{
			UIHelper.LoadedDesign.SPINumber = UIHelper.CoreDataService.GetSPINumberFromUserData();
		}
		if (string.IsNullOrEmpty(UIHelper.LoadedDesign.Designer))
		{
			UIHelper.LoadedDesign.Designer = UIHelper.CoreDataService.GetDesignerFromUserData();
		}
		int designId = UIHelper.LoadedDesign.DesignId;
		if (designId == 0)
		{
			string text = UIHelper.CoreDataService.DocumentUserDataFindValue("DesignInfo", "FileNumber");
			if (text != null)
			{
				int.TryParse(text, out designId);
				if (designId != 0)
				{
					using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
					GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Design design2 = counterSketchLibraryDataEntities.Designs.FirstOrDefault((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Design _) => _.DesignId == (long)designId);
					if (design2 != null)
					{
						UIHelper.LoadedDesign.SPINumber = design2.StullerNo;
					}
				}
			}
		}
		if (string.IsNullOrWhiteSpace(UIHelper.LoadedDesign.SPINumber) && !ViewModelLocator.Current.FreehandStackVM.HasFreehandParts)
		{
			UIHelper.LoadedDesign.SPINumber = "Unknown Design";
		}
		EventHelper.RecordEvent(EventHelper.EventTypes.Design_Opened, $"IsNew={DesignHelper.IsNewDesign(design.DesignId)}", UIHelper.LoadedDesign.SPINumber);
		GlobalErrorHandler.WriteInfoToEventLog($"\t Updating Design Display Info Completed: {stopwatch.ElapsedMilliseconds}ms", 2);
		ViewModelLocator.Current.MainVM.SPINumber = UIHelper.LoadedDesign.SPINumber;
		ProjectManagementHelper.IsFileModified = false;
		ViewModelLocator.Current.CustomizeVM.IsOrderSectionVisible = false;
		ViewModelLocator.Current.BreadCrumbVM.LabelEditing = UIHelper.LoadedDesign.SPINumber;
		ViewModelLocator.Current.BreadCrumbVM.IsDesignLoaded = !string.IsNullOrWhiteSpace(UIHelper.LoadedDesign.SPINumber);
		ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize);
		UIHelper.ShowWaitWindow(waitVisible: false);
		UIHelper.CoreDataService.DocumentCenterDesign();
		GlobalErrorHandler.WriteInfoToEventLog($"Loading Design - {design.DesignId} Completed: {stopwatchOverAll.ElapsedMilliseconds}ms", 2);
	}
}
