using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using CS.CSObjects;
using CS.CSValues;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.MaterialGroup;
using CS.Parametric;
using CSBlockUtil;
using CSGemInstance;
using CSMaterials;
using CSRingSizes.RingSize;
using GV.CounterSketch.Shared;
using Microsoft.Win32;
using RMA.Rhino;

namespace CS;

public sealed class CSApplication : ICSApplication
{
	public sealed class FilenameEventArgs : EventArgs
	{
		public string PathToDesignFile { get; set; }
	}

	public sealed class SelectedHandlesEventArgs : EventArgs
	{
		public ICSVHandle PrimarySelectedHandle { get; set; }

		public List<ICSVHandle> SecondarySelectedHandles { get; set; }

		public bool RequiresUIUpdate { get; set; }
	}

	public readonly double VAT = 0.175;

	private string _csStyleSheetPath;

	private double _largeStoneCaratMax;

	public static int RhinoProcessId { get; set; }

	public bool DesignerAppSettingUseMatrixMaterials { get; set; }

	private DisplayComponents CurrentPatchwireComponent { get; set; }

	public bool IsFreehandPartsVisible { get; set; }

	public bool IsQuarterCaratLargeStoneRoundingEnabled { get; set; }

	public double LargeStoneCaratMax
	{
		get
		{
			if (!(Math.Abs(_largeStoneCaratMax) <= 0.0))
			{
				return _largeStoneCaratMax;
			}
			return 100.0;
		}
		set
		{
			_largeStoneCaratMax = value;
		}
	}

	public double CamProngExtensionHeight { get; set; }

	public bool IsCamMode { get; set; }

	public bool IsCamValueMode { get; set; }

	public bool IsForceHideDisabled { get; set; }

	public bool IsIterationMode { get; set; }

	public CSMouseCallback MouseCallback { get; set; }

	public bool IsCenterGemAllowedForPrinting { get; set; }

	public bool IsSideGemAllowedForPrinting { get; set; }

	public bool IsAccentGemAllowedForPrinting { get; set; }

	public bool WhenExportingHonorGemPrintingFlags { get; set; }

	public bool IsBlockParallelProcessingEnabled { get; set; } = true;

	public bool DisableMeshObjectMeshing { get; set; }

	public bool IsExportingSTL { get; set; }

	public double AngleToleranceDegrees { get; set; }

	public double AngleToleranceRadians { get; set; }

	public CSBlockList BlockList { get; set; }

	public DateTime BuildDate { get; set; }

	public string BuildDateString { get; set; }

	public string CSIterationPresetsPath { get; set; }

	public int BuildRevision { get; set; }

	public string BuildRevisionString { get; set; }

	public CSDisplayConduit Conduit { get; set; }

	public string CSLibraryPath => Path.Combine(CSRootPath, "Library");

	public string CSMeshRepairPath => Path.Combine(CSRootPath, "CounterSketchServer", "MeshRepair");

	public string CSRootPath
	{
		get
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (directoryName != null)
			{
				if (!CSApplicationHelper.IsCounterSketchStudio)
				{
					return directoryName;
				}
				return Path.GetFullPath(Path.Combine(directoryName, "..\\"));
			}
			return string.Empty;
		}
	}

	public string CSStyleSheetPath
	{
		get
		{
			return _csStyleSheetPath ?? (_csStyleSheetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CounterSketch\\StyleSheets"));
		}
		set
		{
			_csStyleSheetPath = value;
		}
	}

	public string CSSystemPath => Path.Combine(CSRootPath, "System");

	public CameraView CurrentCameraViewName { get; set; }

	public bool DesignerAppSettingAutoHideProjects { get; set; }

	public bool DesignerAppSettingLibraryOpen { get; set; }

	public bool DesignerAppSettingProjectBrowserOpen { get; set; }

	public bool DesignerAppSettingSaveInsteadOfSaveAs { get; set; }

	public bool DesignerAppSettingSortStackAZ { get; set; }

	public Guid DesignerGuid { get; set; }

	public CSDocument Document { get; set; }

	public CSUICategoryTable ImportCategories { get; set; }

	public int ImportOffset { get; set; }

	public bool InBenchmark { get; set; }

	public bool InImport { get; set; }

	public IInputChoosers InputChoosers { get; set; }

	public bool InRenderCode { get; set; }

	public bool InRhinoCommand => RhUtil.RhinoApp().ActiveCommand() != null;

	public bool InRunScriptBlock { get; set; }

	public string InvokePartPath { get; set; }

	public bool IsKeyFound { get; set; }

	public bool IsUIModified { get; set; }

	public Collection<CSDisplayMaterial> Material => CSApplicationHelper.Application.Conduit.DisplayMaterials;

	public int NumberOfStudioVisibleControls
	{
		get
		{
			int num = 0;
			if (Document != null)
			{
				foreach (CSObject item in Document.Graph.AllObjects())
				{
					if (item is CSValueBase cSValueBase && (cSValueBase.StudioVisible || cSValueBase.StudioVisibleAdvanced || cSValueBase.StudioVisibleDesignOption) && cSValueBase.Categories.Count > 0)
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	public Assembly PatchwireAssembly { get; set; }

	public ComponentsEnum PatchWireComponent
	{
		get
		{
			return CSUtil.DisplayComponentsToComponentsEnum(CurrentPatchwireComponent)[0];
		}
		set
		{
			CurrentPatchwireComponent = CSUtil.ComponentsEnumToDisplayComponents(value);
		}
	}

	public DisplayComponents PatchWireComponentDisplay => CurrentPatchwireComponent;

	public bool PipeMode { get; set; }

	public bool PrintTimingInformation { get; set; }

	public bool ProductionMeshes { get; set; }

	public string RegistryKeyPath { get; set; }

	public RingSize RingSizeTranslator { get; set; }

	public IScriptBlockEditor ScriptBlockEditor { get; set; }

	public Assembly StudioPatchwireAssembly { get; set; }

	public double Tolerance { get; set; }

	public bool UseNativeCode { get; set; }

	public CSEventWatcher Watcher { get; set; }

	public event EventHandler<SelectedHandlesEventArgs> FreehandObjectSelectionChangedEvent;

	public event EventHandler<FilenameEventArgs> LoadDesignEvent;

	public event EventHandler RedrawAllUINeeded;

	public event EventHandler RedrawForceUINeeded;

	public event EventHandler RedrawUINeeded;

	public event EventHandler ResetUINeededEvent;

	public event EventHandler RhinoWindowClickedEvent;

	public event EventHandler RhinoWindowKeyPressedEvent;

	public event EventHandler SaveNeededEvent;

	public event EventHandler UnloadSuperviewEvent;

	public event EventHandler GraphGenerateCompleteEvent;

	public CSApplication()
	{
		Watcher = new CSEventWatcher();
		MouseCallback = new CSMouseCallback();
		RingSizeTranslator = new RingSizeUSA();
		CurrentPatchwireComponent = DisplayComponents.Ring;
		BuildRevision = 0;
		BuildRevisionString = "<<REV>>";
		BuildDateString = "<<NOW>>";
		Document = new CSDocument();
		BlockList = new CSBlockList();
		ImportOffset = 0;
		InImport = false;
		IsKeyFound = false;
		Conduit = new CSDisplayConduit
		{
			IsViewportHandlesVisible = true
		};
		PrintTimingInformation = false;
		ImportCategories = null;
		CamProngExtensionHeight = 0.25;
		CS.MaterialGroup.MaterialGroupHelper.CreateDefaultMaterialGroups();
		CSGemInstanceDefinition.GemDir = CSLibraryPath;
		CSGemInstance.CSUtil.GetTolerance = GetTolerance;
		CSBlockUtil.CSUtil.GetCSLibraryPath = GetCsLibraryPath;
		CSBlockUtil.CSUtil.GetIsPrintingAllowed = CSUtil.IsPrintingAllowed;
		CSBlockUtil.CSUtil.GetIsCounterSketch = CSApplicationHelper.GetIsCounterSketchStudio;
		CSBlockUtil.CSUtil.GetTolerance = GetTolerance;
		CSBlockUtil.CSUtil.GetAngleToleranceRadians = GetAngleToleranceRadians;
		if (CSApplicationHelper.IsCounterSketchStudio)
		{
			Document.DrawingPointsEnabed = false;
			Document.DrawingCurvesEnabled = false;
			Document.DrawingStartsEnabed = false;
		}
	}

	public bool OnLoad()
	{
		try
		{
			if (BuildRevisionString.IndexOf("REV") < 0)
			{
				BuildRevision = int.Parse(BuildRevisionString);
			}
			CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");
			if (BuildDateString.IndexOf("NOW") < 0)
			{
				BuildDate = DateTime.Parse(BuildDateString, provider);
			}
			else
			{
				BuildDate = DateTime.Now;
			}
		}
		catch (FormatException)
		{
		}
		CSUtil.PrintLine("Loading CSCore:" + BuildDate.Year + ":" + BuildDate.Month + ":" + BuildDate.Day + ":" + BuildRevisionString);
		Watcher.Register();
		Watcher.Enable();
		CSValueHandleHelper.IsHandleMouseWatcherEnabled = true;
		Conduit.Enabled = true;
		Conduit.LoadMaterials();
		PipeMode = false;
		ProductionMeshes = false;
		UseNativeCode = true;
		AngleToleranceDegrees = 1.0;
		AngleToleranceRadians = CSCoreMath.ToRadians(AngleToleranceDegrees);
		Tolerance = 0.001;
		return true;
	}

	public void RaiseFreehandObjectSelectionChangedEvent(ICSVHandle primaryHandle, List<ICSVHandle> secondaryHandles, bool requiresUICallback = true)
	{
		if (this.FreehandObjectSelectionChangedEvent != null)
		{
			SelectedHandlesEventArgs e = new SelectedHandlesEventArgs
			{
				PrimarySelectedHandle = primaryHandle,
				SecondarySelectedHandles = secondaryHandles,
				RequiresUIUpdate = requiresUICallback
			};
			this.FreehandObjectSelectionChangedEvent(primaryHandle, e);
		}
	}

	public void RaiseLoadDesignEvent(string pathToDesign)
	{
		FilenameEventArgs e = new FilenameEventArgs
		{
			PathToDesignFile = pathToDesign
		};
		this.LoadDesignEvent?.Invoke(null, e);
	}

	public void RaiseRedrawAllUINeededEvent()
	{
		this.RedrawAllUINeeded?.Invoke(null, new EventArgs());
	}

	public void RaiseRedrawForceUINeededEvent()
	{
		this.RedrawForceUINeeded?.Invoke(null, new EventArgs());
	}

	public void RaiseRedrawUINeededEvent()
	{
		this.RedrawUINeeded?.Invoke(null, new EventArgs());
	}

	public void RaiseResetUINeededEvent()
	{
		this.ResetUINeededEvent?.Invoke(null, new EventArgs());
	}

	public void RaiseRhinoWindowClickedEvent()
	{
		this.RhinoWindowClickedEvent?.Invoke(this, new EventArgs());
	}

	public void RaiseRhinoWindowKeyPressedEvent(Key keyPressed)
	{
		this.RhinoWindowKeyPressedEvent?.Invoke(keyPressed, new EventArgs());
	}

	public void RaiseSaveNeededEvent()
	{
		this.SaveNeededEvent?.Invoke(null, new EventArgs());
	}

	public void RaiseUnloadSuperviewEvent()
	{
		this.UnloadSuperviewEvent?.Invoke(null, new EventArgs());
	}

	public void RaiseGraphGenerateComplete()
	{
		this.GraphGenerateCompleteEvent?.Invoke(null, new EventArgs());
	}

	public void RegisterCSBlocks()
	{
		RegisterCSBlocks(Assembly.GetExecutingAssembly());
	}

	public void RegisterCSBlocks(Assembly assembly)
	{
		List<Guid> list = new List<Guid>();
		CSUtil.PrintLine("Registering Blocks");
		Type typeBlock = typeof(CSObject);
		foreach (Type item in from t in assembly.GetTypes()
			where !t.IsAbstract && t.IsClass && typeBlock.IsAssignableFrom(t)
			select t)
		{
			object[] customAttributes = item.GetCustomAttributes(inherit: false);
			for (int num = 0; num < customAttributes.Length; num++)
			{
				if (!(customAttributes[num] is CSBlockAttribute cSBlockAttribute))
				{
					continue;
				}
				if (list.IndexOf(cSBlockAttribute.Guid) >= 0)
				{
					CSUtil.PrintLine($"ERROR:  {cSBlockAttribute.BlockName} Invalid GUID");
					continue;
				}
				if (item.FullName.ToLower().Contains("obsolete"))
				{
					string blockName = cSBlockAttribute.BlockName;
					string name = item.Name;
					bool flag = false;
					object[] customAttributes2 = item.GetCustomAttributes(inherit: false);
					for (int num2 = 0; num2 < customAttributes2.Length; num2++)
					{
						if (customAttributes2[num2] is ObsoleteAttribute)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						CSUtil.PrintLine("No obsolete flag found in " + blockName);
					}
					int num3 = blockName.IndexOf("_V");
					if (num3 == -1)
					{
						CSUtil.PrintLine("No Version Found in attribName " + blockName);
					}
					else
					{
						try
						{
							int num4 = Convert.ToInt32(blockName.Substring(num3 + 2));
							blockName = blockName.Substring(0, num3);
							int num5 = Convert.ToInt32(name.Substring(num3 + 2));
							name = name.Substring(2, num3);
							if (num4 != num5 || blockName != name)
							{
								CSUtil.PrintLine("Version name do not match " + cSBlockAttribute.BlockName + " != " + item.Name);
							}
						}
						catch (Exception)
						{
							CSUtil.PrintLine("Fail to match " + cSBlockAttribute.BlockName + " != " + item.Name);
						}
					}
				}
				else
				{
					string blockName2 = cSBlockAttribute.BlockName;
					string name2 = item.Name;
					if ("CS" + blockName2 != name2)
					{
						CSUtil.PrintLine("Block Names do not match " + cSBlockAttribute.BlockName + " != " + item.Name + "   , " + item.FullName);
					}
				}
				if (item.GetConstructor(new Type[1] { typeof(string) }) == null)
				{
					CSUtil.PrintLine($"ERROR:  {cSBlockAttribute.BlockName} missing constructor");
				}
				list.Add(cSBlockAttribute.Guid);
				BlockList.Add(new CSBlockDescription(cSBlockAttribute.BlockName, item, cSBlockAttribute.Guid));
			}
		}
		CSUtil.PrintLine($"Registering Blocks Complete:  {BlockList.Count} Blocks Registered");
	}

	public void ResetDocumentModelDisplaySettings()
	{
		if (Document != null)
		{
			Document.DrawingObjectsEnabled = true;
		}
	}

	public void ResetDocumentAllDisplaySettings()
	{
		if (Document != null)
		{
			Document.DrawingCurvesEnabled = true;
			Document.DrawingPointsEnabed = true;
			Document.DrawingStartsEnabed = true;
			Document.DrawingObjectsEnabled = true;
		}
	}

	public void UpdateDesignerAppSettings()
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, writable: true);
		if (registryKey != null)
		{
			registryKey.SetValue("AutoHideProjects", DesignerAppSettingAutoHideProjects.ToString());
			registryKey.SetValue("SaveInsteadOfSaveAs", DesignerAppSettingSaveInsteadOfSaveAs.ToString());
			registryKey.SetValue("SortStackAZ", DesignerAppSettingSortStackAZ.ToString());
			registryKey.SetValue("LibraryOpen", DesignerAppSettingLibraryOpen.ToString());
			registryKey.SetValue("ProjectBrowserOpen", DesignerAppSettingProjectBrowserOpen.ToString());
		}
	}

	private double GetAngleToleranceRadians()
	{
		return AngleToleranceRadians;
	}

	private string GetCsLibraryPath()
	{
		return CSLibraryPath;
	}

	private double GetTolerance()
	{
		return Tolerance;
	}
}
