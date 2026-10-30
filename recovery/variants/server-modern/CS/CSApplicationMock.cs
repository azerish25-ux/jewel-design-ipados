using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using CS.CSObjects.CSBlocks;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.Parametric;
using CSMaterials;
using CSRingSizes.RingSize;
using GV.CounterSketch.Shared;

namespace CS;

public sealed class CSApplicationMock : ICSApplication
{
	public string CSIterationPresetsPath { get; set; }

	public bool IsQuarterCaratLargeStoneRoundingEnabled { get; set; }

	public double LargeStoneCaratMax { get; set; }

	public bool IsBlockParallelProcessingEnabled { get; set; }

	public double AngleToleranceDegrees { get; set; }

	public double AngleToleranceRadians { get; set; }

	public CSBlockList BlockList { get; set; }

	public DateTime BuildDate { get; set; }

	public string BuildDateString { get; set; }

	public int BuildRevision { get; set; }

	public string BuildRevisionString { get; set; }

	public double CamProngExtensionHeight { get; set; }

	public bool IsFreehandPartsVisible { get; set; }

	public CSDisplayConduit Conduit { get; set; }

	public string CSApplicationDatabasePath { get; set; }

	public string CSLibraryPath { get; set; }

	public string CSModelLibrary { get; set; }

	public string CSProjectPath { get; set; }

	public string CSRootPath { get; set; }

	public string CSMeshRepairPath { get; set; }

	public string CSStyleSheetPath { get; set; }

	public string CSSystemPath { get; set; }

	public CameraView CurrentCameraViewName { get; set; }

	public bool DesignerAppSettingAutoHideProjects { get; set; }

	public bool DesignerAppSettingLibraryOpen { get; set; }

	public bool DesignerAppSettingProjectBrowserOpen { get; set; }

	public bool DesignerAppSettingSaveInsteadOfSaveAs { get; set; }

	public bool DesignerAppSettingSortStackAZ { get; set; }

	public bool DesignerAppSettingUseMatrixMaterials { get; set; }

	public Guid DesignerGuid { get; set; }

	public CSDocument Document { get; set; }

	public CSUICategoryTable ImportCategories { get; set; }

	public int ImportOffset { get; set; }

	public bool InBenchmark { get; set; }

	public bool InImport { get; set; }

	public IInputChoosers InputChoosers { get; set; }

	public bool InRenderCode { get; set; }

	public bool InRhinoCommand { get; private set; }

	public bool InRunScriptBlock { get; set; }

	public string InvokePartPath { get; set; }

	public bool IsCamMode { get; set; }

	public bool IsCamValueMode { get; set; }

	public bool IsCounterSketch { get; }

	public bool IsForceHideDisabled { get; set; }

	public bool IsIterationMode { get; set; }

	public bool IsKeyFound { get; set; }

	public bool IsUIModified { get; set; }

	public Version LatestVersion { get; set; }

	public Collection<CSDisplayMaterial> Material { get; private set; }

	public CSMouseCallback MouseCallback { get; set; }

	public int NumberOfStudioVisibleControls { get; private set; }

	public string Password { get; set; }

	public Assembly PatchwireAssembly { get; set; }

	public ComponentsEnum PatchWireComponent { get; set; }

	public DisplayComponents PatchWireComponentDisplay { get; private set; }

	public bool PipeMode { get; set; }

	public bool PrintTimingInformation { get; set; }

	public bool ProductionMeshes { get; set; }

	public string RegistryKeyPath { get; set; }

	public RingSize RingSizeTranslator { get; set; }

	public IScriptBlockEditor ScriptBlockEditor { get; set; }

	public Assembly StudioPatchwireAssembly { get; set; }

	public double Tolerance { get; set; }

	public bool UseNativeCode { get; set; }

	public string Username { get; set; }

	public CSEventWatcher Watcher { get; set; }

	public bool IsCenterGemAllowedForPrinting { get; set; }

	public bool IsSideGemAllowedForPrinting { get; set; }

	public bool IsAccentGemAllowedForPrinting { get; set; }

	public bool WhenExportingHonorGemPrintingFlags { get; set; }

	public bool DisableMeshObjectMeshing { get; set; }

	public bool IsExportingSTL { get; set; }

	event EventHandler<CSApplication.SelectedHandlesEventArgs> ICSApplication.FreehandObjectSelectionChangedEvent
	{
		add
		{
			throw new NotImplementedException();
		}
		remove
		{
			throw new NotImplementedException();
		}
	}

	public event EventHandler GraphGenerateCompleteEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<CSApplication.FilenameEventArgs> LoadDesignEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler MaterialsUpdatedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler RedrawAllUINeeded
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler RedrawForceUINeeded
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler RedrawUINeeded
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler ResetUINeededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler SaveNeededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler UnloadSuperviewEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler FreehandObjectSelectionChangedEvent;

	public event EventHandler RhinoWindowClickedEvent;

	public event EventHandler RhinoWindowKeyPressedEvent;

	public bool IsInternetAccessible(bool runCheckSilent)
	{
		return true;
	}

	public bool OnLoad()
	{
		return true;
	}

	public void RaiseFreehandObjectSelectionChangedEvent(ICSVHandle blockSelected, List<ICSVHandle> csBlocksSecondarySelected, bool requiresUICallback = true)
	{
		throw new NotImplementedException();
	}

	public void RaiseFreehandObjectSelectionChangedEvent(CSBlock blockSelected)
	{
	}

	public void RaiseLoadDesignEvent(string pathToDesign)
	{
	}

	public void RaiseMaterialsUpdatedEvent()
	{
	}

	public void RaiseRedrawAllUINeededEvent()
	{
	}

	public void RaiseRedrawForceUINeededEvent()
	{
	}

	public void RaiseRedrawUINeededEvent()
	{
	}

	public void RaiseResetUINeededEvent()
	{
	}

	public void RaiseRhinoWindowClickedEvent()
	{
	}

	public void RaiseRhinoWindowKeyPressedEvent(Key keyPressed)
	{
	}

	public void RaiseSaveNeededEvent()
	{
	}

	public void RaiseUnloadSuperviewEvent()
	{
	}

	public void RaiseGraphGenerateComplete()
	{
	}

	public void RegisterCSBlocks()
	{
	}

	public void RegisterCSBlocks(Assembly asm)
	{
	}

	public void ResetDocumentModelDisplaySettings()
	{
	}

	public void ResetDocumentAllDisplaySettings()
	{
	}

	public void UpdateDesignerAppSettings()
	{
	}
}
