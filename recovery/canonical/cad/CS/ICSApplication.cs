using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.Parametric;
using CSMaterials;
using CSRingSizes.RingSize;
using GV.CounterSketch.Shared;

namespace CS;

public interface ICSApplication
{
	string CSLibraryPath { get; }

	string CSMeshRepairPath { get; }

	string CSRootPath { get; }

	string CSSystemPath { get; }

	bool InRhinoCommand { get; }

	Collection<CSDisplayMaterial> Material { get; }

	int NumberOfStudioVisibleControls { get; }

	DisplayComponents PatchWireComponentDisplay { get; }

	double AngleToleranceDegrees { get; set; }

	bool IsFreehandPartsVisible { get; set; }

	double AngleToleranceRadians { get; set; }

	CSBlockList BlockList { get; set; }

	DateTime BuildDate { get; set; }

	string BuildDateString { get; set; }

	int BuildRevision { get; set; }

	string BuildRevisionString { get; set; }

	double CamProngExtensionHeight { get; set; }

	CSDisplayConduit Conduit { get; set; }

	string CSStyleSheetPath { get; set; }

	string CSIterationPresetsPath { get; set; }

	CameraView CurrentCameraViewName { get; set; }

	Guid DesignerGuid { get; set; }

	CSDocument Document { get; set; }

	CSUICategoryTable ImportCategories { get; set; }

	int ImportOffset { get; set; }

	bool InBenchmark { get; set; }

	bool InImport { get; set; }

	IInputChoosers InputChoosers { get; set; }

	bool InRenderCode { get; set; }

	bool InRunScriptBlock { get; set; }

	string InvokePartPath { get; set; }

	bool IsCamMode { get; set; }

	bool IsCamValueMode { get; set; }

	bool IsForceHideDisabled { get; set; }

	bool IsIterationMode { get; set; }

	bool IsKeyFound { get; set; }

	bool IsUIModified { get; set; }

	CSMouseCallback MouseCallback { get; set; }

	Assembly PatchwireAssembly { get; set; }

	ComponentsEnum PatchWireComponent { get; set; }

	bool PipeMode { get; set; }

	bool PrintTimingInformation { get; set; }

	bool ProductionMeshes { get; set; }

	string RegistryKeyPath { get; set; }

	RingSize RingSizeTranslator { get; set; }

	IScriptBlockEditor ScriptBlockEditor { get; set; }

	Assembly StudioPatchwireAssembly { get; set; }

	double Tolerance { get; set; }

	bool UseNativeCode { get; set; }

	CSEventWatcher Watcher { get; set; }

	bool DisableMeshObjectMeshing { get; set; }

	bool IsExportingSTL { get; set; }

	bool DesignerAppSettingAutoHideProjects { get; set; }

	bool DesignerAppSettingLibraryOpen { get; set; }

	bool DesignerAppSettingProjectBrowserOpen { get; set; }

	bool DesignerAppSettingSaveInsteadOfSaveAs { get; set; }

	bool DesignerAppSettingSortStackAZ { get; set; }

	bool IsQuarterCaratLargeStoneRoundingEnabled { get; set; }

	double LargeStoneCaratMax { get; set; }

	bool IsCenterGemAllowedForPrinting { get; set; }

	bool IsSideGemAllowedForPrinting { get; set; }

	bool IsAccentGemAllowedForPrinting { get; set; }

	bool WhenExportingHonorGemPrintingFlags { get; set; }

	bool IsBlockParallelProcessingEnabled { get; set; }

	event EventHandler<CSApplication.SelectedHandlesEventArgs> FreehandObjectSelectionChangedEvent;

	event EventHandler GraphGenerateCompleteEvent;

	event EventHandler<CSApplication.FilenameEventArgs> LoadDesignEvent;

	event EventHandler RedrawAllUINeeded;

	event EventHandler RedrawForceUINeeded;

	event EventHandler RedrawUINeeded;

	event EventHandler ResetUINeededEvent;

	event EventHandler RhinoWindowClickedEvent;

	event EventHandler RhinoWindowKeyPressedEvent;

	event EventHandler SaveNeededEvent;

	event EventHandler UnloadSuperviewEvent;

	bool OnLoad();

	void RaiseFreehandObjectSelectionChangedEvent(ICSVHandle blockSelected, List<ICSVHandle> csBlocksSecondarySelected, bool requiresUICallback = true);

	void RaiseGraphGenerateComplete();

	void RaiseLoadDesignEvent(string pathToDesign);

	void RaiseRedrawAllUINeededEvent();

	void RaiseRedrawForceUINeededEvent();

	void RaiseRedrawUINeededEvent();

	void RaiseResetUINeededEvent();

	void RaiseRhinoWindowClickedEvent();

	void RaiseRhinoWindowKeyPressedEvent(Key keyPressed);

	void RaiseSaveNeededEvent();

	void RaiseUnloadSuperviewEvent();

	void RegisterCSBlocks();

	void RegisterCSBlocks(Assembly asm);

	void ResetDocumentAllDisplaySettings();

	void ResetDocumentModelDisplaySettings();

	void UpdateDesignerAppSettings();
}
