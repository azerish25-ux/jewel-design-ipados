#define TRACE
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.Material;
using CS.MaterialGroup;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Reporting;
using CS.Selection;
using CS.Utilities;
using CS.Utilities.Helpers;
using CS.Utilities.Travis;
using CSBlocks.Core;
using CSBlocks.Transforms;
using CSBlocks.Utilities;
using CSCompatibility;
using CSDatas;
using CSGemInstance.GemTableData;
using CSMaterials;
using CSRingSizes.RingSize;
using CounterSketchServer.Utilities.Helpers;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.CAD;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Pricing;
using GV.CounterSketch.Shared.Pricing.Enums;
using GV.CounterSketch.Shared.Utilities.Helpers;
using GalaSoft.MvvmLight.Threading;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.ApplicationSettings;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.FileIO;
using Rhino.Geometry;
using RhinoScript4;

namespace CounterSketchServer;

[ServiceBehavior(IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
public class RhinoServices : IRhinoServices
{
	private sealed class CachedMeshItem
	{
		public int VerticeCount { get; set; }

		public int FaceCount { get; set; }

		public BoundingBox Box { get; set; }
	}

	private static readonly MRhinoApp _rhinoApp = RhUtil.RhinoApp();

	private static readonly Dictionary<CachedMeshItem, double> _volumeCacheDictionary = new Dictionary<CachedMeshItem, double>();

	private static CSReport _csReport;

	private static IntPtr _intPtrRhino;

	private static bool _previousHandleStatus;

	private static ProjetHelper _projetPrinter;

	private static IStudioUICallback _studioUICallback;

	private readonly Dictionary<string, string> _dictionaryRingNameToHandFilePath = new Dictionary<string, string>
	{
		{ "Female_L0", "Hands\\Female\\f5.3dm" },
		{ "Female_L1", "Hands\\Female\\f4.3dm" },
		{ "Female_L2", "Hands\\Female\\f3.3dm" },
		{ "Female_L3", "Hands\\Female\\f2.3dm" },
		{ "Female_L4", "Hands\\Female\\f1.3dm" },
		{ "Female_R0", "Hands\\Female\\f5.3dm" },
		{ "Female_R1", "Hands\\Female\\f4.3dm" },
		{ "Female_R2", "Hands\\Female\\f3.3dm" },
		{ "Female_R3", "Hands\\Female\\f2.3dm" },
		{ "Female_R4", "Hands\\Female\\f1.3dm" },
		{ "Male_L0", "Hands\\Male\\f5.3dm" },
		{ "Male_L1", "Hands\\Male\\f4.3dm" },
		{ "Male_L2", "Hands\\Male\\f3.3dm" },
		{ "Male_L3", "Hands\\Male\\f2.3dm" },
		{ "Male_L4", "Hands\\Male\\f1.3dm" },
		{ "Male_R0", "Hands\\Male\\f5.3dm" },
		{ "Male_R1", "Hands\\Male\\f4.3dm" },
		{ "Male_R2", "Hands\\Male\\f3.3dm" },
		{ "Male_R3", "Hands\\Male\\f2.3dm" },
		{ "Male_R4", "Hands\\Male\\f1.3dm" }
	};

	private IRhinoScript _rhinoScript;

	private SelectionMode _previousVPSelectionMode;

	protected IRhinoScript RhinoScript
	{
		get
		{
			Trace.WriteLine("Accessing RhinoScript interface.");
			return _rhinoScript ?? (_rhinoScript = RhUtil.RhinoApp().GetRhinoScriptInterface() as IRhinoScript);
		}
	}

	public static IStudioUICallback StudioUICallback
	{
		get
		{
			if (OperationContext.Current != null)
			{
				_studioUICallback = OperationContext.Current.GetCallbackChannel<IStudioUICallback>();
			}
			return _studioUICallback;
		}
		set
		{
			_studioUICallback = value;
		}
	}

	private static CSPatchWireStart LastPatchwireWithHandle { get; set; }

	public RhinoServices()
	{
		Directory.SetCurrentDirectory(GetPathOfAssembly());
		UIControlHelper.RefreshUIControlList();
		CSApplicationHelper.Application.FreehandObjectSelectionChangedEvent += Application_FreehandObjectSelectionChanged;
		CSApplicationHelper.Application.RhinoWindowClickedEvent += Application_RhinoWindowClickedEvent;
		SetRhinoOptionsforCS();
	}

	public void PerformDistanceMeasurement()
	{
		CursorTooltipSettings.TooltipsEnabled = true;
		CursorTooltipSettings.DistancePane = true;
		if (CSApplicationHelper.Application.CurrentCameraViewName == CameraView.Perspective)
		{
			TopMostMessageBox.Show("In order to use the Measure Tool you will need to select a view other than Perspective.", "Please select another view.");
		}
		else
		{
			RhinoRunScript("!_Distance");
		}
	}

	public void SetIsBlockParallelProcessingEnabled(bool enabled)
	{
		CSApplicationHelper.Application.IsBlockParallelProcessingEnabled = enabled;
		CSUtil.PrintLine(CSApplicationHelper.Application.IsBlockParallelProcessingEnabled ? "IsBlockParallelProcessingEnabled is TRUE" : "IsBlockParallelProcessingEnabled is FALSE");
		CSUtil.ForceGraphToRedraw();
	}

	public void SetCAMModeState(bool CAMModeOn)
	{
		CSApplicationHelper.Application.IsCamMode = CAMModeOn;
		CSApplicationHelper.Application.IsCamValueMode = CAMModeOn;
		CSUtil.ForceGraphToRedraw();
		CSUtil.Redraw();
	}

	public void AddPropToDisplay(string renderPropSwatchCollectionItem)
	{
		RenderHelper.ImportFile(CSApplicationHelper.Application.CSLibraryPath + "\\Props\\" + renderPropSwatchCollectionItem + ".3dm");
		RhinoObject val = ((IEnumerable<RhinoObject>)RhinoDoc.ActiveDoc.Objects).FirstOrDefault((RhinoObject obj) => obj.Name == "PrimaryMaterial");
		if (val != null)
		{
			Material material = val.GetMaterial(true);
			material.SetBitmapTexture("C:\\Program Files (x86)\\CounterSketch Studio\\System\\RenderMaterials\\Groundplanes\\maps\\EliteCream_Diffuse.jpg");
			material.CommitChanges();
			RhinoObject val2 = ((IEnumerable<RhinoObject>)RhinoDoc.ActiveDoc.Objects).FirstOrDefault((RhinoObject obj) => obj.Name == "SecondaryMaterial");
			if (val2 != null)
			{
				material = val2.GetMaterial(true);
			}
			material.SetBitmapTexture("C:\\Program Files (x86)\\CounterSketch Studio\\System\\RenderMaterials\\Groundplanes\\maps\\EliteCream_Diffuse.jpg");
			material.CommitChanges();
		}
		CSUtil.Redraw();
	}

	public void ClearHighlightedGems()
	{
		CSReport.ClearGems();
	}

	public void ClearExpandedSliders()
	{
		foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.AllObjects())
		{
			item.UserData.RemoveKey("ExpandedSlider", "IsOn");
		}
	}

	public void ConduitEnabled(bool isEnabled)
	{
		if (isEnabled)
		{
			((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = true;
		}
		else
		{
			((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = false;
		}
		CSUtil.Redraw();
	}

	public void ConduitResetTryOn()
	{
		CSApplicationHelper.Application.Conduit.ResetTryOnMaterials();
	}

	public void ConduitSetFingernailObjectMaterial(string fingernailPolishMaterialName)
	{
		MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnFingernails).FirstOrDefault();
		if (materialGroup != null)
		{
			CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, fingernailPolishMaterialName);
		}
	}

	public void ConduitSetFlipRing(bool flipRing)
	{
		CSApplicationHelper.Application.Conduit._isRingFlipped = flipRing;
	}

	public void ConduitSetHandObjectMaterial(string handMaterialName)
	{
		MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnHand).FirstOrDefault();
		if (materialGroup != null)
		{
			CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, handMaterialName);
		}
	}

	public void ConduitSetScaleToRingRailObjectFlip(bool flipObject)
	{
		CSApplicationHelper.Application.Conduit._isHandFlipped = flipObject;
	}

	public void ConduitSetScaleToRingRailYOffset(double ringOffset)
	{
		CSApplicationHelper.Application.Conduit._handRingRailYOffset = ringOffset;
	}

	public bool ConnectionValid()
	{
		if (RhUtil.RhinoApp().SdkVersion() == 0)
		{
			return false;
		}
		return true;
	}

	public void DeselectViewportControls()
	{
		if (CSValueHandleHelper.CurrentHandle != null)
		{
			CSValueHandleHelper.TemporaryHandle = CSValueHandleHelper.CurrentHandle;
			CSValueHandleHelper.CurrentHandle = null;
			CSUtil.Redraw();
		}
	}

	public bool IsDisplayConduitEnabled()
	{
		return ((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled;
	}

	public void DisplayConduitSetEnabled(bool enabled)
	{
		if (enabled)
		{
			((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = true;
		}
		else
		{
			((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = false;
		}
	}

	public void DocumentCenterDesign()
	{
		CSUtil.CenterDesign();
	}

	public void DocumentClearAllAnyGemAnyColor()
	{
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((MaterialGroup materialGroup) => materialGroup.IsUserMaterialGroup).ToList().ForEach(delegate(MaterialGroup materialGroup)
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Remove(materialGroup);
		});
		CS.MaterialGroup.MaterialGroupHelper.ClearSelectedGroupItems();
		CSUtil.ForceGraphToRedraw();
	}

	public void DocumentClearUndo()
	{
		CSApplicationHelper.Application.Document.UndoTable.Reset();
	}

	public void DocumentResetCenterGemAnyGemAnyColorMaterialAssignment()
	{
		CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemUser).SelectMany((MaterialGroup _) => _.MaterialGroupItems).ToList()
			.ForEach(delegate(MaterialGroupItem materialGroupItem)
			{
				if (CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault().MaterialGroupItems.Select((MaterialGroupItem _) => _.Id.Split(':').FirstOrDefault()).Contains(materialGroupItem.Id.Split(':').FirstOrDefault()))
				{
					CS.MaterialGroup.MaterialGroupHelper.RemoveItemFromMaterialGroupByClassification(MaterialClassification.GemUser, materialGroupItem.Id);
				}
			});
		IEnumerable<CSBlock> csBlocksToUpdate = CSApplicationHelper.Application.Document.Graph.CSBlocks.Where((CSBlock csBlock) => CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault().MaterialGroupItems.Any((MaterialGroupItem materialGroupItem) => materialGroupItem.Id.Contains(csBlock.InstanceGuid.ToString())));
		csBlocksToUpdate.ToList().ForEach(delegate(CSBlock block)
		{
			block.UpdateMaterialAssignment();
		});
		CSUtil.Redraw();
		ImportGroupHelper.UpdateImportGroups(updateAllImportGroupImages: false);
		if (CSApplicationHelper.Application.Document.Graph.ImportedCSGParts == null)
		{
			return;
		}
		CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.Where((ImportGroup importGroup) => csBlocksToUpdate.Any((CSBlock block) => importGroup.BlockIdentifiers.Contains(block.InstanceGuid))).ToList().ForEach(ImportGroupHelper.UpdateImportGroupBitmapImage);
	}

	public bool DocumentCreateAnyGemAnyColorMaterialGroupForSelectedGems(GemMaterial gemMaterial, bool isCenterStoneAssigned = false)
	{
		bool result = false;
		if (isCenterStoneAssigned && CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault().MaterialGroupItems.Select((MaterialGroupItem _) => _.Id.Split(':').FirstOrDefault()).Intersect(CS.MaterialGroup.MaterialGroupHelper.GetSelectionMaterialGroup().MaterialGroupItems.Select((MaterialGroupItem _) => _.Id.Split(':').FirstOrDefault())).Any())
		{
			result = true;
		}
		MaterialGroup userMaterialGroup = new MaterialGroup
		{
			Classification = MaterialClassification.GemUser,
			MaterialName = gemMaterial.ToString(),
			Name = $"User Group - {Guid.NewGuid()}"
		};
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Add(userMaterialGroup);
		CS.MaterialGroup.MaterialGroupHelper.AddSelectedGroupItemsToMaterialGroup(userMaterialGroup);
		CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Where((MaterialGroup materialGroup) => materialGroup.IsUserMaterialGroup && !materialGroup.MaterialGroupItems.Any()).ToList().ForEach(delegate(MaterialGroup materialGroup)
		{
			CSMaterials.MaterialGroupHelper.CurrentDocumentMaterialGroups.Remove(materialGroup);
		});
		CS.MaterialGroup.MaterialGroupHelper.ClearSelectedGroupItems();
		IEnumerable<CSBlock> csBlocksToUpdate = CSApplicationHelper.Application.Document.Graph.CSBlocks.Where((CSBlock csBlock) => userMaterialGroup.MaterialGroupItems.Any((MaterialGroupItem materialGroupItem) => materialGroupItem.Id.Contains(csBlock.InstanceGuid.ToString())));
		csBlocksToUpdate.ToList().ForEach(delegate(CSBlock block)
		{
			block.UpdateMaterialAssignment();
		});
		CSUtil.Redraw();
		ImportGroupHelper.UpdateImportGroups(updateAllImportGroupImages: false);
		CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.Where((ImportGroup importGroup) => csBlocksToUpdate.Any((CSBlock block) => importGroup.BlockIdentifiers.Contains(block.InstanceGuid))).ToList().ForEach(ImportGroupHelper.UpdateImportGroupBitmapImage);
		return result;
	}

	public bool DocumentGetModified()
	{
		return RhUtil.RhinoApp().ActiveDoc().IsModified();
	}

	public double DocumentGetRingSizeIndexFromSizeText(string ringSizeText)
	{
		int index = 0;
		if (CSApplicationHelper.Application.RingSizeTranslator.GetIndexTxt(ringSizeText, ref index))
		{
			return index;
		}
		return 0.0;
	}

	public string DocumentGetRingSizeTextFromSizeIndex(double ringSizeIndex)
	{
		return CSApplicationHelper.Application.RingSizeTranslator.GetSizeIndex(ringSizeIndex).Size;
	}

	public double DocumentGetRingSizeTranslated(double ringSizeInMillimeters)
	{
		return CSApplicationHelper.Application.RingSizeTranslator.GetSizeRadius(ringSizeInMillimeters).index;
	}

	public void UpdateViewPortHandleDrawScale(double scaleFactor)
	{
		CSValueHandleHelper.ScreenScaleOverride = scaleFactor;
		CSUtil.Redraw();
	}

	public List<StudioGem> DocumentGetStudioGemList()
	{
		List<StudioGem> list = new List<StudioGem>();
		int num = 0;
		CSUtil.EnableDrawing(which: false);
		CS.MaterialGroup.MaterialGroupHelper.UpdateGemDisplayNumbers(isRedrawNeeded: false);
		_csReport = new CSReport();
		_csReport.PopulateReport();
		foreach (CSBlock item2 in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			int outputChannelCount = item2.OutputChannelCount;
			for (int i = 0; i < outputChannelCount; i++)
			{
				CSData outputChannelData = item2.GetOutputChannelData(i);
				if (outputChannelData == null)
				{
					continue;
				}
				CSDataArray flattenArray = CSDataHelper.GetFlattenArray(outputChannelData);
				for (int j = 0; j < flattenArray.GetArrayCount(); j++)
				{
					CSData arrayItem = flattenArray.GetArrayItem(j);
					if (CSReporting.GetDataType(arrayItem) == IOReportingTypes.Gems)
					{
						if (!(arrayItem is CSGemData cSGemData))
						{
							break;
						}
						bool isCenterStone = false;
						bool isSideStone = false;
						bool isLargeStone = false;
						bool isMeleeStone = false;
						if (_csReport.Gem(num).GemPlacement == GemPlace.Center)
						{
							isCenterStone = true;
						}
						else if (_csReport.Gem(num).GemPlacement == GemPlace.Side)
						{
							isSideStone = true;
						}
						Enum.TryParse<GemMaterial>(cSGemData.CurrentMaterialGroup.MaterialName, out var result);
						double specificGravityForMaterial = GemCaratHelper.GetSpecificGravityForMaterial(result);
						decimal num2 = Math.Round(Convert.ToDecimal(cSGemData.FFactor * specificGravityForMaterial * cSGemData.GemXSize * cSGemData.GemYSize * cSGemData.GemZSize), 4, MidpointRounding.ToEven);
						if (num2 < 0.30m)
						{
							isMeleeStone = true;
						}
						else
						{
							isLargeStone = true;
						}
						StudioGem item = new StudioGem
						{
							Quantity = 1,
							Length = Convert.ToDecimal(Math.Round(cSGemData.GemZSize, 3)),
							Width = Convert.ToDecimal(Math.Round(cSGemData.GemXSize, 3)),
							Height = Convert.ToDecimal(Math.Round(cSGemData.GemYSize, 3)),
							ItemNumber = null,
							GemNumber = cSGemData.GemNumber,
							GemIndex = num,
							IsCenterStone = isCenterStone,
							IsSideStone = isSideStone,
							IsLargeStone = isLargeStone,
							IsMeleeStone = isMeleeStone,
							GemTypeOption = StudioGemType.Genuine,
							GemShapeOption = UIControlHelper.ConvertGemStringToStudioGemShape(cSGemData.GemShape),
							GemMaterialOption = result,
							SetTypeOption = UIControlHelper.ConvertGemFlagsToStudioSetType(cSGemData),
							GemMountingType = cSGemData.SettingType.ToString(),
							Weight = num2
						};
						list.Add(item);
						num++;
					}
				}
			}
		}
		CSUtil.EnableDrawing(which: true);
		return list;
	}

	public void DocumentGraphGenerate()
	{
		CSApplicationHelper.Application.Document.Graph.Generate();
	}

	public void DocumentReset()
	{
		CSApplicationHelper.Application.Document.Reset();
	}

	public void DocumentSetCameraView(string cameraViewName)
	{
		CSUtil.DocumentSetCameraView(cameraViewName);
	}

	public void DocumentSetCurvesAndPointsVisibility(bool visible)
	{
		CSApplicationHelper.Application.Document.DrawingCurvesEnabled = visible;
		CSApplicationHelper.Application.Document.DrawingStartsEnabed = visible;
		CSApplicationHelper.Application.Document.DrawingPointsEnabed = visible;
		CSUtil.Redraw();
	}

	public void DocumentSetFilename(string filename)
	{
		CSApplicationHelper.Application.Document.Filename = filename;
	}

	public void DocumentSetModified(bool modified)
	{
		RhUtil.RhinoApp().ActiveDoc().SetModifiedFlag(modified);
	}

	public void DocumentSetRingSizeSetting(string ringSizeSetting)
	{
		CSApplicationHelper.Application.RingSizeTranslator = CSUtilRingSize.CreateRingSizeTranslator((ERingSizeOption)Enum.Parse(typeof(ERingSizeOption), ringSizeSetting));
		foreach (CSValueRingSize item in CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<CSValueRingSize>())
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			item.Change();
			item.SetValueFromUI(item.ValueIndex, mouseDown: false);
		}
		CSApplicationHelper.Application.Document.UndoTable.Reset();
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.RedrawUI();
	}

	public void DocumentSetShowCurves(bool visible)
	{
		CSApplicationHelper.Application.Document.DrawingCurvesEnabled = visible;
	}

	public void DocumentSetShowStarts(bool visible)
	{
		CSApplicationHelper.Application.Document.DrawingStartsEnabed = visible;
	}

	public void DocumentShowAxis(bool visible)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		MRhinoView[] array = null;
		RhUtil.RhinoApp().ActiveDoc().GetViewList(ref array);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MainViewport().SetShowConstructionAxes(visible);
			array[i].MainViewport().SetShowWorldAxes(visible);
		}
		MDisplayAttrsMgrList val = new MDisplayAttrsMgrList();
		MRhinoDisplayAttrsMgr.GetDisplayAttrsList(ref val);
		for (int j = 0; j < val.Count(); j++)
		{
			val[j].m_pAttrs.m_bDrawAxes = visible;
			val[j].m_pAttrs.m_bDrawWorldAxes = visible;
			MRhinoDisplayAttrsMgr.UpdateAttributes((IDisplayAttrsMgrListDesc)(object)val[j]);
		}
		CSUtil.Redraw();
	}

	public void DocumentShowGrid(bool visible)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		MRhinoView[] array = null;
		RhUtil.RhinoApp().ActiveDoc().GetViewList(ref array);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MainViewport().SetShowConstructionGrid(visible);
		}
		MDisplayAttrsMgrList val = new MDisplayAttrsMgrList();
		MRhinoDisplayAttrsMgr.GetDisplayAttrsList(ref val);
		for (int j = 0; j < val.Count(); j++)
		{
			val[j].m_pAttrs.m_bDrawGrid = visible;
			MRhinoDisplayAttrsMgr.UpdateAttributes((IDisplayAttrsMgrListDesc)(object)val[j]);
		}
		CSUtil.Redraw();
	}

	public void DocumentSpinDesign(bool isEnabled)
	{
		DispatcherHelper.CheckBeginInvokeOnUI(delegate
		{
			if (isEnabled)
			{
				RhUtil.RhinoApp().RunScript("! -_Turntable d 0 s 10 r 0 _Enter", 1);
			}
			else
			{
				RhUtil.RhinoApp().RunScript("!", 1);
				RhUtil.RhinoApp().RunScript("-_ClearUndo", 1);
			}
		});
	}

	public void DocumentToggleGemSelectionMode(bool isEnabled)
	{
		CS.MaterialGroup.MaterialGroupHelper.ClearSelectedGroupItems();
		CS.MaterialGroup.MaterialGroupHelper.ClearMouseOverGroupItems();
		CS.MaterialGroup.MaterialGroupHelper.ClearHighlightGroupItems();
		if (isEnabled)
		{
			_previousHandleStatus = CSValueHandleHelper.IsHandleMouseWatcherEnabled;
			_previousVPSelectionMode = SelectionHelper.CurrentSelectionMode;
			SelectionHelper.CurrentSelectionMode = SelectionMode.BlockOutputIndex;
			CSValueHandleHelper.IsHandleMouseWatcherEnabled = false;
		}
		else
		{
			SelectionHelper.CurrentSelectionMode = _previousVPSelectionMode;
			CSValueHandleHelper.IsHandleMouseWatcherEnabled = _previousHandleStatus;
		}
		SelectionHelper.EnableSelectionMode(isEnabled);
		CSUtil.Redraw();
	}

	public void DocumentUndoBeginRecord()
	{
	}

	public string DocumentUserDataGetOrderData()
	{
		IEnumerable<UserDataElement> enumerable = CSApplicationHelper.Application.Document.DocumentUserData.ElementsThatMatchType("OrderInfo");
		IEnumerable<UserDataElement> enumerable2 = (enumerable as IList<UserDataElement>) ?? enumerable.ToList();
		if (enumerable2.Any())
		{
			return string.Join("|", enumerable2);
		}
		return null;
	}

	public void DocumentUserDataAddValue(string typeName, string keyName, string value)
	{
		CSApplicationHelper.Application.Document.DocumentUserData.AddKey(typeName, keyName, value);
	}

	public string DocumentUserDataFindValue(string typeName, string keyName)
	{
		return CSApplicationHelper.Application.Document.DocumentUserData.FindValue(typeName, keyName);
	}

	public List<Tuple<string, string, string>> DocumentUserDataGetAllDataElements()
	{
		return CSApplicationHelper.Application.Document.DocumentUserData.AllDataElements;
	}

	public void DocumentWrite(string path, string licenseInfo)
	{
		try
		{
			if (!string.IsNullOrEmpty(licenseInfo))
			{
				CSApplicationHelper.Application.Document.DocumentUserData.AddKey("LICENSE", DateTime.Now.ToUniversalTime().ToString(), licenseInfo);
			}
			CSApplicationHelper.Application.Document.Write(path);
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
			throw;
		}
	}

	public void DocumentZoomCameraToBoundingBox()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		RhinoViewport mainViewport = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport;
		RhinoView val = RhinoDoc.ActiveDoc.Views.GetStandardRhinoViews().FirstOrDefault((RhinoView view) => view.MainViewport.Name.ToLower() == "perspective");
		if (val != null)
		{
			mainViewport = val.MainViewport;
		}
		BoundingBox blockBoundingBox = default(BoundingBox);
		(from csblock in CSApplicationHelper.Application.Document.Graph.AllObjects().OfType<CSBlock>()
			where csblock != null && csblock.DisplayVisible && csblock.InDisplay && (!csblock.ForceHide || csblock.ForceVisible)
			select csblock).ToList().ForEach(delegate(CSBlock csBlock)
		{
			csBlock.OutputChannelDatas.ForEach(delegate(CSData csData)
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				if (csData != null)
				{
					BoundingBox boundingBox = default(BoundingBox);
					csData.GetBoundingBox(ref boundingBox);
					((BoundingBox)(ref boundingBox)).Transform(CSApplicationHelper.Application.Conduit.GetDesignTransform());
					((BoundingBox)(ref blockBoundingBox)).Union(boundingBox);
				}
			});
		});
		mainViewport.ZoomBoundingBox(blockBoundingBox);
	}

	public void EditorGetPartsForCurveEditrFromGraphObject(Guid blockInstanceGuid, bool createAsCopy, out List<double> sweepPoints, out double chooserValue, out NurbsCurve curve)
	{
		chooserValue = 0.0;
		sweepPoints = new List<double>();
		curve = new NurbsCurve();
		if (UIControlHelper.GetCSObjectByInstanceGuid(blockInstanceGuid) is CSValueChooser cSValueChooser)
		{
			CSData chooserObject = cSValueChooser.GetChooserObject((int)cSValueChooser.Value);
			if (createAsCopy)
			{
				cSValueChooser.CopyCurrent();
			}
			if (chooserObject.HasNurbsCurve())
			{
				chooserValue = cSValueChooser.Value;
			}
			sweepPoints = new List<double>();
			for (int i = 0; i < chooserObject.GetCurveParameterCount(); i++)
			{
				sweepPoints.Add(chooserObject.GetCurveParameter(i));
			}
			if (chooserObject.HasNurbsCurve())
			{
				curve = NurbsHelper.SetVarFromCurve(chooserObject.GetNurbsCurve());
			}
		}
	}

	public void EnableDrawing(bool isEnabled)
	{
		CSUtil.EnableDrawing(isEnabled);
	}

	public IEnumerable<KeyValuePair<int, string>> GetAllRingSizesForCurrentRingSizeRegion()
	{
		int count = CSApplicationHelper.Application.RingSizeTranslator.Count;
		List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
		for (int i = 0; i < count - 1; i++)
		{
			KeyValuePair<int, string> item = new KeyValuePair<int, string>(i, DocumentGetRingSizeTextFromSizeIndex(i));
			list.Add(item);
		}
		return list;
	}

	public List<ComponentsEnum> GetComponents()
	{
		IEnumerable<Guid> enumerable = CSApplicationHelper.Application.Document.Graph.ImportedCSGParts?.SelectMany((ImportGroup _) => _.BlockIdentifiers);
		if (enumerable != null && !((CSApplicationHelper.Application.Document.Graph.CSBlocks?.Select((CSBlock _) => _.InstanceGuid))?.Except(enumerable)).Any())
		{
			return new List<ComponentsEnum>();
		}
		return CSUtil.DisplayComponentsToComponentsEnum(CSUtil.DisplayComponentsInFile());
	}

	public byte[] GetCurrentViewportImageBitmap()
	{
		MRhinoView obj = RhUtil.RhinoApp().ActiveView();
		int num = 0;
		int num2 = 1326;
		int num3 = 760;
		int num4 = 0;
		obj.MainViewport().VP().GetScreenPort(ref num, ref num2, ref num3, ref num4);
		Bitmap currentViewportImageBitmap = CSUtil.GetCurrentViewportImageBitmap(num2, ColorToUInt(System.Drawing.Color.FromArgb(255, 0, 255, 0)));
		using MemoryStream memoryStream = new MemoryStream();
		currentViewportImageBitmap.Save(memoryStream, ImageFormat.Bmp);
		return memoryStream.ToArray();
	}

	public string GetDesignerFromUserData()
	{
		return CSApplicationHelper.Application.Document.DocumentUserData.FindValue("DesignInfo", "ArtistName");
	}

	public int GetDesignIterationCount()
	{
		return IterationHelper.GetDesignIterationCount();
	}

	public bool AreSecondaryMetalObjectsVisible()
	{
		return GraphHelper.GetDisplayObjectsByMaterialClassification(MaterialClassification.MetalSecondary).Any();
	}

	public bool AreSecondaryMetalObjectsVisibleForComponent(ComponentsEnum whichComponent)
	{
		return GraphHelper.GetDisplayObjectsByComponentAndMaterialClassification(whichComponent, MaterialClassification.MetalSecondary).Any();
	}

	public string GetDocumentFilename()
	{
		return CSApplicationHelper.Application.Document.Filename;
	}

	public List<int> GetFeaturedDesignIterations()
	{
		return IterationHelper.GetFeaturedDesignIterations();
	}

	public GemMaterial[] GetGemMaterials()
	{
		MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault();
		MaterialGroup materialGroup2 = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemSide).FirstOrDefault();
		MaterialGroup materialGroup3 = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemAccent).FirstOrDefault();
		if (materialGroup != null && materialGroup2 != null && materialGroup3 != null)
		{
			return new GemMaterial[3]
			{
				(GemMaterial)Enum.Parse(typeof(GemMaterial), materialGroup.MaterialName),
				(GemMaterial)Enum.Parse(typeof(GemMaterial), materialGroup2.MaterialName),
				(GemMaterial)Enum.Parse(typeof(GemMaterial), materialGroup3.MaterialName)
			};
		}
		return null;
	}

	public MetalMaterial[] GetMetalMaterials()
	{
		MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalPrimary).FirstOrDefault();
		MaterialGroup materialGroup2 = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalSecondary).FirstOrDefault();
		if (materialGroup != null && materialGroup2 != null)
		{
			return new MetalMaterial[2]
			{
				(MetalMaterial)Enum.Parse(typeof(MetalMaterial), materialGroup.MaterialName),
				(MetalMaterial)Enum.Parse(typeof(MetalMaterial), materialGroup2.MaterialName)
			};
		}
		return null;
	}

	public int GetRhinoThreadId()
	{
		try
		{
			return AppDomain.GetCurrentThreadId();
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error getting Rhino Thread Id, Keystrokes for Rhino not be captured: " + ex);
			return -1;
		}
	}

	public string GetSPINumberFromUserData()
	{
		return CSApplicationHelper.Application.Document.DocumentUserData.FindValue("DesignInfo", "StullerIDNo");
	}

	public BitmapSourceByteArrayInfo GetThumbnailFromFile(string filename)
	{
		CSUtil.GetThumbnailFromFile(filename, out var previewImage, out var _, 255, 360);
		if (previewImage == null)
		{
			previewImage = new Bitmap(360, 255);
		}
		BitmapSource bitmapSource = CSUtil.GetBitmapSource(previewImage);
		BitmapSourceByteArrayInfo bitmapSourceByteArrayInfo = new BitmapSourceByteArrayInfo
		{
			ItemImageHeight = bitmapSource.PixelHeight,
			ItemImageWidth = bitmapSource.PixelWidth,
			ItemImageStride = bitmapSource.PixelWidth * ((bitmapSource.Format.BitsPerPixel + 7) / 8)
		};
		bitmapSourceByteArrayInfo.ItemImageSourceByteArray = new byte[bitmapSourceByteArrayInfo.ItemImageHeight * bitmapSourceByteArrayInfo.ItemImageStride];
		bitmapSource.CopyPixels(bitmapSourceByteArrayInfo.ItemImageSourceByteArray, bitmapSourceByteArrayInfo.ItemImageStride, 0);
		return bitmapSourceByteArrayInfo;
	}

	public List<ComponentsEnum> GetVisibleComponents()
	{
		return CSUtil.DisplayComponentsToComponentsEnum(CSUtil.DisplayComponentsDisplayed());
	}

	public void GraphItemRemoveUserDataValue(Guid blockInstanceGuid, string typeName, string keyName)
	{
		UIControlHelper.GetCSObjectByInstanceGuid(blockInstanceGuid)?.UserData.RemoveKey(typeName, keyName);
	}

	public void GraphItemAddUserDataValue(Guid blockInstanceGuid, string typeName, string keyName, string value, bool replaceCurrentValue)
	{
		UIControlHelper.GetCSObjectByInstanceGuid(blockInstanceGuid)?.UserData.AddKey(typeName, keyName, value);
	}

	public string GraphItemFindUserDataValue(Guid blockInstanceGuid, string typeName, string keyName)
	{
		return UIControlHelper.GetCSObjectByInstanceGuid(blockInstanceGuid)?.UserData.FindValue(typeName, keyName);
	}

	public List<UIControl> GraphRedoChange()
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.RedoLast();
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		List<UIControl> uIControlsFromGraphObjects = UIControlHelper.GetUIControlsFromGraphObjects();
		ImportGroupHelper.UpdateFreehandStackControls();
		return uIControlsFromGraphObjects;
	}

	public List<UIControl> GraphUndoChange()
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.UndoLast();
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		List<UIControl> uIControlsFromGraphObjects = UIControlHelper.GetUIControlsFromGraphObjects();
		ImportGroupHelper.UpdateFreehandStackControls();
		return uIControlsFromGraphObjects;
	}

	public void HighlightAccentGems(int gemId, bool highlightAll)
	{
		if (gemId == -1)
		{
			CSReport.ClearGems();
			return;
		}
		if (highlightAll)
		{
			if (gemId == 0)
			{
				for (int i = 0; i < _csReport.GemCount(); i++)
				{
					if (_csReport.Gem(i).GemPlacement != GemPlace.Center)
					{
						_csReport.ShowGemByGemIndex(i);
					}
				}
			}
		}
		else
		{
			_csReport.ShowGemByGemIndex(gemId);
		}
		CSUtil.Redraw();
	}

	public void HighlightGemFromGemReportByGemNumber(List<string> gemNumbers)
	{
		if (_csReport == null)
		{
			_csReport = new CSReport();
			_csReport.PopulateReport();
		}
		gemNumbers.ForEach(delegate(string gemNumber)
		{
			_csReport.ShowGemByGemNumber(gemNumber);
		});
		CSUtil.Redraw();
	}

	public void HighlightGemFromGemReportByIndex(List<int> gemIndexes)
	{
		if (_csReport == null)
		{
			_csReport = new CSReport();
			_csReport.PopulateReport();
		}
		gemIndexes.ForEach(delegate(int i)
		{
			_csReport.ShowGemByGemIndex(i);
		});
		CSUtil.Redraw();
	}

	public void HighlightGraphItem(Guid blockInstanceGuid, bool showHighlight)
	{
		CSPatchWireStart patchwireForUIControl = UIControlHelper.GetPatchwireForUIControl(blockInstanceGuid);
		if (patchwireForUIControl != null)
		{
			patchwireForUIControl.MouseOverStart = showHighlight;
		}
	}

	public void HighlightItemsByBlockInstanceGuid(IEnumerable<Guid> instanceGuids)
	{
		CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.Highlighted);
		if (instanceGuids == null)
		{
			return;
		}
		CS.MaterialGroup.MaterialGroupHelper.AddRangeToMaterialGroupByClassification(MaterialClassification.Highlighted, (from guid in instanceGuids
			where CSApplicationHelper.Application.Document.Graph.AllObjects().OfType<CSBlock>().Any((CSBlock csBlock) => csBlock.InstanceGuid == guid && csBlock.DisplayVisible && !csBlock.ForceHide)
			select guid.ToString()).ToList());
	}

	public void InitializeRhinoProperties(string csLibraryPath)
	{
		DispatcherHelper.CheckBeginInvokeOnUI(delegate
		{
			try
			{
				RenderHelper.LoadVrayMaterials("fullenv.visopt");
			}
			catch (Exception ex)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Error Initializing Rhino Properties: " + ex);
			}
		});
	}

	public bool IsComponentsEnumVisible(ComponentsEnum componentsEnum)
	{
		DisplayComponents displayComponents = CSUtil.ComponentsEnumToDisplayComponents(componentsEnum);
		if ((CSApplicationHelper.Application.Document.CurrentDisplayComponents & displayComponents) == 0)
		{
			return false;
		}
		return true;
	}

	public bool IsDocumentCanvasEmpty()
	{
		return !(from graphObject in CSApplicationHelper.Application.Document.Graph.ActiveObjects()
			where graphObject.DisplayVisible || graphObject.ForceVisible
			select graphObject).Any();
	}

	public int GetNumberofVisibleFreehandParts()
	{
		return CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.Count((ImportGroup _) => _.IsViewportVisible);
	}

	public bool IsUsbKeyBeingUsed()
	{
		return CSApplicationHelper.Application.IsKeyFound;
	}

	public BoundingBox NurbsGetBoundingBox(NurbsCurve nc)
	{
		return NurbsHelper.GetBoundingBox(nc);
	}

	public double NurbsGetClosestPoint(NurbsCurve nc, Point3D testHit)
	{
		return NurbsHelper.GetClosestPoint(nc, testHit);
	}

	public Point3D NurbsGetNormalizedPointAt(NurbsCurve nc, double editPointParam)
	{
		return NurbsHelper.GetNormalizedPointAt(nc, editPointParam);
	}

	public List<Point2D> NurbsGetPolyline(NurbsCurve nc)
	{
		return NurbsHelper.GetPolyline(nc);
	}

	public NurbsCurve NurbsRebuild(NurbsCurve nc, int i, int i1)
	{
		return NurbsHelper.Rebuild(nc, i, i1);
	}

	public NurbsCurve NurbsSetDomain(NurbsCurve nc, int i, int i1)
	{
		return NurbsHelper.SetDomain(nc, i, i1);
	}

	public List<UIControl> OpenDocument(string pathToFile, bool isQuarterCaratLargeStoneRoundingEnabled, double largeStoneCaratMax, Guid centerGemControlGuid = default(Guid), int? centerGemGraphIndex = null)
	{
		CSApplicationHelper.Application.IsQuarterCaratLargeStoneRoundingEnabled = isQuarterCaratLargeStoneRoundingEnabled;
		CSApplicationHelper.Application.LargeStoneCaratMax = largeStoneCaratMax;
		CSApplicationHelper.Application.Document.Read(pathToFile);
		DuplicateBand();
		if (!CSApplicationHelper.CheckSavedVersionAndPrompt() && StudioUICallback != null)
		{
			StudioUICallback.ResetApplicationState();
		}
		CSUtil.Redraw();
		for (int i = 0; i < CSApplicationHelper.Application.Document.Graph.AllObjectsList.Count; i++)
		{
			CSObject cSObject = CSApplicationHelper.Application.Document.Graph.AllObjectsList[i];
			if (cSObject.StudioVisible && cSObject is CSValueBase cSValueBase && cSValueBase.Categories.Any() && !cSValueBase.Deleted && cSObject is CSValueRange)
			{
				CSValueRange cSValueRange = cSObject as CSValueRange;
				GraphItemAddUserDataValue(cSValueRange.InstanceGuid, "ExpandedSlider", "OriginalMin", cSValueRange.MinValue.ToString(), replaceCurrentValue: false);
				GraphItemAddUserDataValue(cSValueRange.InstanceGuid, "ExpandedSlider", "OriginalMax", cSValueRange.MaxValue.ToString(), replaceCurrentValue: false);
			}
		}
		if (CSApplicationHelper.Application.Document.Graph.ImportedCSGParts != null)
		{
			DocumentInitializeFreehandStackControls();
		}
		UIControlHelper.UpdateCenterGemControlIdentifier(centerGemControlGuid, centerGemGraphIndex);
		return UIControlHelper.GetUIControlsFromGraphObjects();
	}

	public bool OrderCheckNonCenterGemsExist()
	{
		_csReport = new CSReport();
		_csReport.PopulateReport();
		for (int i = 0; i < _csReport.GemCount(); i++)
		{
			if (_csReport.Gem(i).GemPlacement != GemPlace.Center)
			{
				return true;
			}
		}
		return false;
	}

	public List<OrderGemData> OrderGetAccentGemData()
	{
		List<OrderGemData> list = new List<OrderGemData>();
		_csReport = new CSReport();
		_csReport.PopulateReport();
		for (int i = 0; i < _csReport.GemCount(); i++)
		{
			CSReportGem cSReportGem = _csReport.Gem(i);
			if (cSReportGem.GemPlacement != GemPlace.Center)
			{
				list.Add(new OrderGemData
				{
					Id = i,
					GemLength = cSReportGem.GemX,
					GemWidth = cSReportGem.GemY,
					GemHeight = cSReportGem.GemZ,
					GemList = i.ToString(),
					GemShape = cSReportGem.Shape,
					GemPlacement = (int)cSReportGem.GemPlacement,
					GemDescription = cSReportGem.GetReportString()
				});
			}
		}
		return list;
	}

	public List<OrderGemData> OrderGetAllGemData()
	{
		List<OrderGemData> list = new List<OrderGemData>();
		_csReport = new CSReport();
		_csReport.PopulateReport();
		for (int i = 0; i < _csReport.GemCount(); i++)
		{
			CSReportGem cSReportGem = _csReport.Gem(i);
			list.Add(new OrderGemData
			{
				Id = i,
				GemLength = cSReportGem.GemX,
				GemWidth = cSReportGem.GemY,
				GemHeight = cSReportGem.GemZ,
				GemList = i.ToString(),
				GemShape = cSReportGem.Shape,
				GemPlacement = (int)cSReportGem.GemPlacement,
				GemDescription = cSReportGem.GetGemInfoOrderString()
			});
		}
		return list;
	}

	public List<string> OrderGetCurrentRingSizes()
	{
		try
		{
			return (from _ in CSUtilRingSize.GetCurrentSizes()
				select _.Size).ToList();
		}
		catch (Exception)
		{
			return new List<string> { "Unknown" };
		}
	}

	public List<double> OrderGetCurrentRingSizesDiameter()
	{
		try
		{
			return (from _ in CSUtilRingSize.GetCurrentSizes()
				select _.diameterMM).ToList();
		}
		catch (Exception)
		{
			return new List<double> { 0.0 };
		}
	}

	public List<string> OrderGetCurrentRingSizesUom()
	{
		try
		{
			return (from _ in CSUtilRingSize.GetCurrentSizes()
				select _.RingSizeTypeAbrv).ToList();
		}
		catch (Exception)
		{
			return new List<string> { "Unknown" };
		}
	}

	public List<string> OrderGetExpandedSliderList()
	{
		return CSUtil.ListOfExpandedSliders();
	}

	public void RedrawRhino()
	{
		CSUtil.Redraw();
	}

	public void RemovePropFromDisplay()
	{
		RhinoDoc.ActiveDoc.Objects.Delete(((IEnumerable<RhinoObject>)RhinoDoc.ActiveDoc.Objects).Select((RhinoObject obj) => obj.Id), true);
	}

	public void RenderGenerateRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane)
	{
		DispatcherHelper.CheckBeginInvokeOnUI(delegate
		{
			RenderHelper.GenerateCustomRender(isRenderLayDesignFlat, isRenderUsePropsSelected, renderPropSwatchCollectionItem, renderPropMaterialSwatchCollectionItem, renderPropMaterialSwatchCollectionSecondaryItem, groundPlaneMaterial, renderResolution, renderEnvironment, saveRender, saveRenderLocation, alphaProps, alphaGroundPlane);
		});
	}

	public void RepaintRhino()
	{
		Win32Utility.RedrawWindow(FindRhino());
	}

	public Bitmap RequestScreenCapture(Size captureSize)
	{
		return CSDisplayCaptureHelper.CaptureViewportToBitmap(captureSize);
	}

	public string GetNotesToCustomerForCurrentDesign()
	{
		CSApplicationHelper.Application.Document.DocumentUserData.FindValue("EDIT", "NOTETOCUSTOMER", out var value);
		return value;
	}

	public Bitmap RequestScreenCaptureAndFreeze(Size captureSize)
	{
		((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = false;
		return CSDisplayCaptureHelper.CaptureViewportToBitmap(captureSize);
	}

	public Bitmap RequestScreenCaptureDefault(Size captureSize, bool zoom, double zoomPadding, bool isMainDesignOnly = false)
	{
		if (isMainDesignOnly)
		{
			return CSDisplayCaptureHelper.CaptureViewportToBitmapForTargetedBlocksAtDefaultPerspective((from _ in CSApplicationHelper.Application.Document.Graph.DisplayObjects().OfType<CSBlock>()
				where _.ImportGroup == null
				select _.InstanceGuid).ToList(), captureSize, zoom, zoomPadding);
		}
		return CSDisplayCaptureHelper.CaptureViewportToBitmapAtDefaultPerspective(captureSize, zoom, zoomPadding);
	}

	public Bitmap RequestScreenCaptureFront(Size captureSize)
	{
		return CSDisplayCaptureHelper.CaptureViewportToBitmapAtFront(captureSize, zoomToObjects: true);
	}

	public Bitmap RequestScreenCaptureSide(Size captureSize)
	{
		return CSDisplayCaptureHelper.CaptureViewportToBitmapAtSide(captureSize, zoomToObjects: true);
	}

	public Bitmap RequestScreenCaptureTop(Size captureSize)
	{
		return CSDisplayCaptureHelper.CaptureViewportToBitmapAtTop(captureSize, zoomToObjects: true);
	}

	public void ReselectViewportControls()
	{
		if (CSValueHandleHelper.TemporaryHandle != null && CSValueHandleHelper.CurrentHandle == null)
		{
			CSValueHandleHelper.CurrentHandle = CSValueHandleHelper.TemporaryHandle;
			CSUtil.Redraw();
		}
	}

	public void RhinoAbortScripts()
	{
		RhinoScript.Command("!", 0);
		RhinoScript.Command("!", 0);
	}

	public void RhinoCloseViewPort(string viewportName)
	{
		if (!(viewportName != "Perspective"))
		{
			return;
		}
		DispatcherHelper.UIDispatcher.Invoke(delegate
		{
			MRhinoView[] source = (MRhinoView[])(object)new MRhinoView[0];
			RhUtil.RhinoApp().ActiveDoc().GetViewList(ref source, true, false);
			MRhinoView val = (from mRhinoView in source
				where mRhinoView != null
				where string.Equals(mRhinoView.MainViewport().Name(), viewportName, StringComparison.OrdinalIgnoreCase)
				select mRhinoView).FirstOrDefault();
			if (val != null)
			{
				RhUtil.RhinoApp().ActiveDoc().CloseRhinoView(val);
			}
			SetPerspectiveViewActiveAndMaximize();
		});
	}

	public IntPtr RhinoCreateViewPort(string viewportName)
	{
		IntPtr handleToView = IntPtr.Zero;
		DispatcherHelper.UIDispatcher.Invoke(delegate
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			MRhinoView[] source = (MRhinoView[])(object)new MRhinoView[0];
			RhUtil.RhinoApp().ActiveDoc().GetViewList(ref source, true, false);
			MRhinoView val = (from mRhinoView in source
				where mRhinoView != null
				where string.Equals(mRhinoView.MainViewport().Name(), "Perspective", StringComparison.OrdinalIgnoreCase)
				select mRhinoView).FirstOrDefault();
			_rhinoApp.SetActiveView(val);
			if (val != null)
			{
				On3dmView val2 = new On3dmView(val.ActiveViewport().View())
				{
					m_name = viewportName
				};
				RhUtil.RhinoApp().ActiveDoc().NewView((IOn3dmView)(object)val2);
			}
			handleToView = RhUtil.RhinoApp().ActiveView().FloatRhinoView(true)
				.Handle;
		});
		return handleToView;
	}

	public IntPtr RhinoGetActiveViewWindowHandle()
	{
		return RhUtil.RhinoApp().ActiveView().m_hWnd.Handle;
	}

	public string RhinoGetVersion()
	{
		return string.Format("{0}:{1}:{2}-{3}-{4}", RhinoApp.ExeVersion, RhinoApp.ExeServiceRelease, RhinoApp.BuildDate.Year, RhinoApp.BuildDate.Month.ToString("D2", CultureInfo.InvariantCulture), RhinoApp.BuildDate.Day.ToString("D2", CultureInfo.InvariantCulture));
	}

	public IntPtr RhinoGetWindowHandle()
	{
		return RhUtil.RhinoApp().MainWnd().Handle;
	}

	public void RhinoLoadBlankDocument()
	{
		string text = Path.Combine(CSApplicationHelper.Application.CSRootPath, "System\\Default.3dm");
		RhinoScript.Command("-_Open \"" + text + "\"", 0);
	}

	public void RhinoRunScript(string scriptText)
	{
		RhinoScript.Command(scriptText, 0);
	}

	public void RhinoSetFocusTo()
	{
		Win32Utility.AllowSetForegroungWindowSetProcessId(-1);
		Win32Utility.SetWindowToForeground(FindRhino());
	}

	public void RhinoWindowReposition(int startPointX, int startPointY, int width, int height)
	{
		Win32Utility.PositionWindow(_intPtrRhino, startPointX, startPointY, width, height);
	}

	public void SaveScreen(string saveImagePath)
	{
		if (saveImagePath != null)
		{
			ScreenCaptureHelper.GrabScreenshot().Save(saveImagePath, ImageFormat.Jpeg);
		}
	}

	public bool SaveViewportToImage(string viewportName, string pathToSaveImage, double maxHeightWidth)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		CSUtil.EnableDrawing(which: false);
		bool isDisplayHandEnabled = CSApplicationHelper.Application.Conduit._isDisplayHandEnabled;
		CSApplicationHelper.Application.Conduit._isDisplayHandEnabled = false;
		CSApplicationHelper.Application.Conduit._isDisplayHighlightEnabled = false;
		DocumentSpinDesign(isEnabled: false);
		MRhinoView[] array = (MRhinoView[])(object)new MRhinoView[0];
		RhUtil.RhinoApp().ActiveDoc().GetViewList(ref array, true, false);
		MRhinoView val = null;
		MRhinoView[] array2 = array;
		foreach (MRhinoView val2 in array2)
		{
			if (val2.MainViewport().Name().Equals(viewportName, StringComparison.OrdinalIgnoreCase))
			{
				val = val2;
				break;
			}
		}
		if (val == null)
		{
			CSApplicationHelper.Application.Conduit._isDisplayHandEnabled = isDisplayHandEnabled;
			CSUtil.EnableDrawing(which: true);
			CSApplicationHelper.Application.Conduit._isDisplayHighlightEnabled = true;
			return false;
		}
		RhUtil.RhinoApp().SetActiveView(val);
		Transform transform = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport.GetTransform((CoordinateSystem)0, (CoordinateSystem)3);
		BoundingBox empty = BoundingBox.Empty;
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			if (item == null)
			{
				continue;
			}
			for (int j = 0; j < item.OutputChannelCount; j++)
			{
				Mesh val3 = item.GetOutputChannelData(j)?.GetMesh();
				if (val3 != null)
				{
					BoundingBox boundingBox = ((GeometryBase)val3).GetBoundingBox(transform);
					if (((BoundingBox)(ref boundingBox)).MakeValid())
					{
						((BoundingBox)(ref empty)).Union(boundingBox);
					}
				}
			}
		}
		On3dmView view = new On3dmView(val.MainViewport().View());
		val.MainViewport().DollyExtents((IOnBoundingBox)(object)CSConvert.BoundingBoxToOnBoundingBox(empty), (coordinate_system)3);
		MRhinoDisplayPipeline val4 = val.DisplayPipeline();
		int num = (int)maxHeightWidth;
		int num2 = (int)maxHeightWidth;
		System.Drawing.Color lightGreen = System.Drawing.Color.LightGreen;
		Bitmap bitmap = new Bitmap(num, num2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		bool flag;
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			MDisplayPipelineAttributes val5 = new MDisplayPipelineAttributes(val4.DisplayAttrs())
			{
				m_SolidColor = OnColor.op_Implicit(new OnColor(ref lightGreen)),
				m_bDisableTransparency = false,
				m_eFillMode = (FrameBufferFillMode)2,
				m_bDrawAxes = false,
				m_bDrawGrid = false,
				m_bDrawWorldAxes = false,
				m_bDrawZAxis = false,
				m_bDrawTransGridPlane = false,
				m_bShowTransGrid = false,
				m_bUseDefaultBg = false,
				m_bUseDocumentGrid = false,
				m_bPlaneUsesGridColor = false
			};
			flag = val4.DrawToDC((IDeviceContext)graphics, num, num2, val5);
		}
		if (flag)
		{
			bitmap.MakeTransparent(lightGreen);
			bitmap.Save(pathToSaveImage, ImageFormat.Png);
		}
		bitmap.Dispose();
		val.MainViewport().SetView((IOn3dmView)(object)view);
		CSUtil.EnableDrawing(which: true);
		array2 = array;
		foreach (MRhinoView val6 in array2)
		{
			if (val6.MainViewport().Name().Equals("Perspective", StringComparison.OrdinalIgnoreCase))
			{
				RhUtil.RhinoApp().SetActiveView(val6);
				break;
			}
		}
		CSApplicationHelper.Application.Conduit._isDisplayHandEnabled = isDisplayHandEnabled;
		CSApplicationHelper.Application.Conduit._isDisplayHighlightEnabled = true;
		return flag;
	}

	public void SelectImportedGroupByIdentifier(Guid importGroupIdentifier)
	{
		ImportGroupHelper.SelectImportedGroupByIdentifier(importGroupIdentifier);
	}

	public void SelectImportedGroupByIdentifierSecondary(List<Guid> importGroupIdentifiers)
	{
		ImportGroupHelper.SelectImportedGroupsByIdentifierSecondary(importGroupIdentifiers);
	}

	public void SetComponentVisibility(ComponentsEnum componentsEnum, bool visible)
	{
		if (visible)
		{
			CSApplicationHelper.Application.Document.CurrentDisplayComponents |= CSUtil.ComponentsEnumToDisplayComponents(componentsEnum);
		}
		else
		{
			CSApplicationHelper.Application.Document.CurrentDisplayComponents &= ~CSUtil.ComponentsEnumToDisplayComponents(componentsEnum);
		}
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
	}

	public void ToggleFreehandCommonCenterMode(bool isFreehandCommonCenterModeEnabled)
	{
		CSValueHandleHelper.SetFreehandCommonCenterModeEnabled(isFreehandCommonCenterModeEnabled);
		ImportGroupHelper.SelectImportedGroupsByIdentifierSecondary();
	}

	public void ToggleFreehandVisibility(bool isVisible)
	{
		CSApplicationHelper.Application.IsFreehandPartsVisible = isVisible;
		if (!isVisible)
		{
			DeselectViewportControls();
			ImportGroupHelper.UpdateActiveImportGroupUsingGroup(null);
		}
		else
		{
			ImportGroupHelper.UpdateImportGroups(updateAllImportGroupImages: true);
		}
		CSUtil.ForceGraphToRedraw();
	}

	public bool GetFreehandComponentVisibility()
	{
		return CSApplicationHelper.Application.IsFreehandPartsVisible;
	}

	public List<StudioGem> GetManualSizedStones()
	{
		List<StudioGem> list = new List<StudioGem>();
		int num = 0;
		CSUtil.EnableDrawing(which: false);
		CS.MaterialGroup.MaterialGroupHelper.UpdateGemDisplayNumbers(isRedrawNeeded: false);
		_csReport = new CSReport();
		_csReport.PopulateReport();
		foreach (CSBlock item2 in CSApplicationHelper.Application.Document.Graph.DisplayObjects())
		{
			if (item2 == null)
			{
				continue;
			}
			foreach (CSData outputChannelData in item2.OutputChannelDatas)
			{
				if (outputChannelData == null)
				{
					continue;
				}
				foreach (CSData item3 in CSDataHelper.GetFlattenArray(outputChannelData).Array)
				{
					CSGemData gemData = item3 as CSGemData;
					if (gemData != null && !(from gemSize in gemData.Sizes
						let isSameX = RhinoMath.EpsilonEquals(gemData.GemXSize, gemSize.X, CSApplicationHelper.Application.Tolerance)
						let isSameY = RhinoMath.EpsilonEquals(gemData.GemYSize, gemSize.Y, CSApplicationHelper.Application.Tolerance)
						where isSameX && isSameY
						select isSameX).Any())
					{
						bool isCenterStone = false;
						bool isSideStone = false;
						bool isLargeStone = false;
						bool isMeleeStone = false;
						switch (_csReport.Gem(num).GemPlacement)
						{
						case GemPlace.Center:
							isCenterStone = true;
							break;
						case GemPlace.Side:
							isSideStone = true;
							break;
						}
						if (!Enum.TryParse<GemMaterial>(gemData.CurrentMaterialGroup.MaterialName, out var result))
						{
							result = GemMaterial.Unassigned;
						}
						double specificGravityForMaterial = GemCaratHelper.GetSpecificGravityForMaterial(result);
						decimal num2 = Math.Round(Convert.ToDecimal(gemData.FFactor * specificGravityForMaterial * gemData.GemXSize * gemData.GemYSize * gemData.GemZSize), 4, MidpointRounding.ToEven);
						if (num2 < 0.30m)
						{
							isMeleeStone = true;
						}
						else
						{
							isLargeStone = true;
						}
						StudioGem item = new StudioGem
						{
							Quantity = 1,
							Length = Convert.ToDecimal(Math.Round(gemData.GemZSize, 3)),
							Width = Convert.ToDecimal(Math.Round(gemData.GemXSize, 3)),
							Height = Convert.ToDecimal(Math.Round(gemData.GemYSize, 3)),
							ItemNumber = null,
							GemNumber = gemData.GemNumber,
							GemIndex = num,
							IsCenterStone = isCenterStone,
							IsSideStone = isSideStone,
							IsLargeStone = isLargeStone,
							IsMeleeStone = isMeleeStone,
							GemTypeOption = StudioGemType.Genuine,
							GemShapeOption = UIControlHelper.ConvertGemStringToStudioGemShape(gemData.GemShape),
							GemMaterialOption = result,
							SetTypeOption = UIControlHelper.ConvertGemFlagsToStudioSetType(gemData),
							GemMountingType = gemData.SettingType.ToString(),
							Weight = num2
						};
						list.Add(item);
						num++;
					}
				}
			}
		}
		CSUtil.EnableDrawing(which: true);
		return list;
	}

	public bool IsManualSizingModeOn(bool excludeCenterStone)
	{
		List<CSBlock> list = CSApplicationHelper.Application.Document.Graph.CSBlocks.Where((CSBlock _) => _.StudioVisible && (_ is CSValueGem || _ is CSValueGemSize)).ToList();
		bool flag = false;
		foreach (CSBlock item in list)
		{
			if (!(item is CSValueGem cSValueGem))
			{
				if (item is CSValueGemSize cSValueGemSize && !(cSValueGemSize.IsAssignedCenter && excludeCenterStone))
				{
					flag |= cSValueGemSize.IsCustomGemSize;
				}
			}
			else if (!(cSValueGem.IsAssignedCenter && excludeCenterStone))
			{
				flag |= cSValueGem.IsCustomGemSize;
			}
		}
		return flag;
	}

	public List<UIControl> SetDesignIteration(int iterationId)
	{
		IterationHelper.SetDesignIteration(iterationId);
		return UIControlHelper.GetUIControlsFromGraphObjects();
	}

	public void SetGemMaterials(GemMaterial[] gemMaterials)
	{
		if (gemMaterials == null || !gemMaterials.Any())
		{
			return;
		}
		MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemCenter).FirstOrDefault();
		if (materialGroup != null)
		{
			CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, gemMaterials[0].ToString());
		}
		materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemSide).FirstOrDefault();
		if (materialGroup != null)
		{
			CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, gemMaterials[1].ToString());
		}
		if (gemMaterials.Length == 3)
		{
			materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.GemAccent).FirstOrDefault();
			if (materialGroup != null)
			{
				CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, gemMaterials[2].ToString());
			}
		}
		ImportGroupHelper.UpdateImportGroups(updateAllImportGroupImages: true);
	}

	public int SetKeepAliveProcessIds(int processId)
	{
		CSVRHelper.ParentProcessId = processId;
		return Process.GetCurrentProcess().Id;
	}

	public void SetMetalMaterials(MetalMaterial[] metalMaterials)
	{
		if (metalMaterials != null && metalMaterials.Any())
		{
			MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalPrimary).FirstOrDefault();
			if (materialGroup != null)
			{
				CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, metalMaterials[0].ToString());
			}
			materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.MetalSecondary).FirstOrDefault();
			if (materialGroup != null)
			{
				CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, metalMaterials[1].ToString());
			}
			ImportGroupHelper.UpdateImportGroups(updateAllImportGroupImages: true);
		}
	}

	public void ToggleViewportControls(bool isVisible)
	{
		CSValueHandleHelper.IsHandleMouseWatcherEnabled = isVisible;
		CSUtil.Redraw();
	}

	public void ToggleQuarterCaratGemRounding(bool isQuarterCaratRoundingEnabled)
	{
		CSApplicationHelper.Application.IsQuarterCaratLargeStoneRoundingEnabled = isQuarterCaratRoundingEnabled;
	}

	public void TurnHandOff()
	{
		CSApplicationHelper.Application.Conduit._handObjects = null;
		CSApplicationHelper.Application.Conduit._fingernailObjects = null;
		CSApplicationHelper.Application.Conduit._shirtObjects = null;
		CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.TryOnHand);
		CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.TryOnFingernails);
		CS.MaterialGroup.MaterialGroupHelper.ClearGroupItemsForMaterialGroupByClassification(MaterialClassification.TryOnShirt);
		CSApplicationHelper.Application.Conduit._necklaceChainObjects = null;
		DocumentSetCameraView("Perspective");
	}

	public void UpdateCurveObjectFromUI(Guid blockInstanceGuid, int whichCurveIndex, NurbsCurve curve, List<double> points, bool mouseDown)
	{
		if (UIControlHelper.GetCSObjectByInstanceGuid(blockInstanceGuid) is CSValueChooser cSValueChooser)
		{
			NurbsCurve varFromCurve = NurbsHelper.GetVarFromCurve(curve);
			cSValueChooser.UpdateGroupObjectFromUI(whichCurveIndex, varFromCurve, points, mouseDown);
		}
	}

	public void UpdateImportGroupSortOrder(List<Guid> importGroupGuids)
	{
		if (CSApplicationHelper.Application.Document.Graph.ImportedCSGParts == null)
		{
			return;
		}
		importGroupGuids.ForEach(delegate(Guid importGroupGuid)
		{
			List<ImportGroup> importedCSGParts = CSApplicationHelper.Application.Document.Graph.ImportedCSGParts;
			ImportGroup importGroup = importedCSGParts.FirstOrDefault((ImportGroup desiredGroup) => desiredGroup.Identifier == importGroupGuid);
			if (importGroup != null)
			{
				importedCSGParts.Remove(importGroup);
				importedCSGParts.Insert(importGroupGuids.IndexOf(importGroupGuid), importGroup);
			}
		});
	}

	public void ExportImportGroups(List<Guid> targetImportGroupIdentifiers, string path)
	{
		ImportGroupHelper.ExportImportGroups(targetImportGroupIdentifiers, path);
	}

	public void UpdateRhinoAppearanceMaterials(string handMaterialName, string polishMaterialName, bool isRenderLayDesignFlat)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(handMaterialName) && Enum.TryParse<HandMaterial>(handMaterialName, ignoreCase: true, out var result))
		{
			MaterialGroup materialGroup = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnHand).FirstOrDefault();
			if (materialGroup != null)
			{
				CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup, handMaterialName);
			}
		}
		if (!string.IsNullOrEmpty(polishMaterialName))
		{
			MaterialGroup materialGroup2 = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnFingernails).FirstOrDefault();
			if (materialGroup2 != null)
			{
				if (polishMaterialName == "None")
				{
					if (!string.IsNullOrEmpty(handMaterialName) && Enum.TryParse<HandMaterial>(handMaterialName, ignoreCase: true, out result))
					{
						CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup2, handMaterialName);
					}
				}
				else
				{
					CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup2, polishMaterialName);
				}
			}
		}
		string name = Enum.GetName(typeof(ShirtMaterial), ShirtMaterial.Fabric_White);
		if (!string.IsNullOrEmpty(name))
		{
			MaterialGroup materialGroup3 = CSMaterials.MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.TryOnShirt).FirstOrDefault();
			if (materialGroup3 != null)
			{
				CS.MaterialGroup.MaterialGroupHelper.AssignMaterialToMaterialGroup(materialGroup3, name);
			}
		}
		if (isRenderLayDesignFlat)
		{
			CSUtil.LayFlat();
		}
		else
		{
			CSApplicationHelper.Application.Conduit._layFlatForm = Transform.Unset;
		}
		CSUtil.Redraw();
	}

	public void UpdateRhinoHand(string selectedRing, bool isRingOnRightHand, bool isRingFlipped, double ringPositionOffset)
	{
		_dictionaryRingNameToHandFilePath.TryGetValue(selectedRing, out var value);
		if (!string.IsNullOrEmpty(value))
		{
			try
			{
				CSApplicationHelper.Application.Conduit._isDisplayHandEnabled = true;
				CSApplicationHelper.Application.Conduit._isHandFlipped = isRingOnRightHand;
				CSApplicationHelper.Application.Conduit._isRingFlipped = isRingFlipped;
				CSApplicationHelper.Application.Conduit._handRingRailYOffset = ringPositionOffset;
				CSBlockDescription cSBlockDescription = CSApplicationHelper.Application.BlockList.FirstOrDefault((CSBlockDescription b) => b.BlockClass == typeof(CSGroupOfRhinoObjects));
				if (cSBlockDescription != null)
				{
					CSGroupOfRhinoObjects cSGroupOfRhinoObjects = (CSGroupOfRhinoObjects)cSBlockDescription.GetNewTemp("Hand");
					CSGroupOfRhinoObjects cSGroupOfRhinoObjects2 = (CSGroupOfRhinoObjects)cSBlockDescription.GetNewTemp("Fingernail");
					CSGroupOfRhinoObjects cSGroupOfRhinoObjects3 = (CSGroupOfRhinoObjects)cSBlockDescription.GetNewTemp("Shirt");
					CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
					CSApplicationHelper.Application.Conduit._handObjects = cSGroupOfRhinoObjects;
					CSApplicationHelper.Application.Conduit._fingernailObjects = null;
					CSApplicationHelper.Application.Conduit._shirtObjects = null;
					if (cSGroupOfRhinoObjects.AddObjectFromFile(value, "Hand"))
					{
						if (cSGroupOfRhinoObjects2.AddObjectFromFile(value, "Fingernail"))
						{
							CSApplicationHelper.Application.Conduit._fingernailObjects = cSGroupOfRhinoObjects2;
						}
						if (cSGroupOfRhinoObjects3.AddObjectFromFile(value, "Shirt"))
						{
							CSApplicationHelper.Application.Conduit._shirtObjects = cSGroupOfRhinoObjects3;
						}
					}
					cSGroupOfRhinoObjects.Generate();
					cSGroupOfRhinoObjects2.Generate();
					cSGroupOfRhinoObjects3.Generate();
				}
			}
			catch (Exception)
			{
				CSApplicationHelper.Application.Conduit._handObjects = null;
				CSApplicationHelper.Application.Conduit._fingernailObjects = null;
				CSApplicationHelper.Application.Conduit._shirtObjects = null;
				CSApplicationHelper.Application.Conduit._necklaceChainObjects = null;
			}
		}
		CS.MaterialGroup.MaterialGroupHelper.UpdateTryOnMaterialGroups();
	}

	public void UpdateStudioUICallbackChannel()
	{
		StudioUICallback = OperationContext.Current.GetCallbackChannel<IStudioUICallback>();
	}

	public void ViewportSetBackgroundAndColor(string backgroundColor, string backgroundGraphicPath)
	{
		if (!(RhUtil.RhinoApp().GetRhinoScriptInterface() is IRhinoScript rhinoScript))
		{
			return;
		}
		if (!string.IsNullOrEmpty(backgroundColor))
		{
			System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(backgroundColor);
			AppearanceSettings.ViewportBackgroundColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
			CSUtil.Redraw();
		}
		else
		{
			AppearanceSettings.ViewportBackgroundColor = System.Drawing.Color.White;
			CSUtil.Redraw();
		}
		if (!string.IsNullOrEmpty(backgroundGraphicPath))
		{
			if (File.Exists(backgroundGraphicPath))
			{
				rhinoScript.Wallpaper(string.Empty, backgroundGraphicPath, false, false);
			}
		}
		else
		{
			rhinoScript.Wallpaper(string.Empty, string.Empty, false, false);
		}
	}

	public void ViewportSetHighlightColor(System.Drawing.Color color)
	{
		CSApplicationHelper.Application.Conduit.SetPreviewColor(color);
	}

	public void ViewportSetTargetAndCameraLocation(double x, double y, double z)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		RhUtil.RhinoApp().ActiveView().ActiveViewport()
			.SetTargetAndCameraLocation(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(x, y, z));
		CSUtil.Redraw();
	}

	public void WatcherSetEnabled(bool enabled)
	{
		((MRhinoEventWatcher)CSApplicationHelper.Application.Watcher).Enable(enabled);
	}

	public byte[] GetDisplayObjectBinObjByComponentAndMaterialClassification(ComponentsEnum whichComponent, MaterialClassification[] whichMaterialClassifications)
	{
		List<CSData> displayObjectsByComponentAndMaterialClassification = GraphHelper.GetDisplayObjectsByComponentAndMaterialClassification(whichComponent, whichMaterialClassifications);
		Mesh mesh = ((displayObjectsByComponentAndMaterialClassification.Count <= 0) ? null : CSDataHelper.ConvertToMesh(displayObjectsByComponentAndMaterialClassification));
		using MemoryStream memoryStream = new MemoryStream();
		CS.Utilities.Helpers.FileFormatHelper.WriteBinObj(mesh.Convert(), memoryStream);
		return memoryStream.ToArray();
	}

	public Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> GetMetalVolumesPerComponent(bool hasSingleMetalCastingCount, int singleMetalCastingCount)
	{
		MetalMaterial[] metalMaterials = GetMetalMaterials();
		if (metalMaterials == null)
		{
			return null;
		}
		MetalMaterial metalMaterial = metalMaterials[0];
		MetalMaterial metalMaterial2 = metalMaterials[1];
		Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> currentVolumeList = new Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double>();
		foreach (ComponentsEnum visibleComponent in GetVisibleComponents())
		{
			switch (visibleComponent)
			{
			case ComponentsEnum.Ring:
			case ComponentsEnum.Pendant:
				if (AreSecondaryMetalObjectsVisibleForComponent(visibleComponent))
				{
					if (metalMaterial == metalMaterial2)
					{
						if (hasSingleMetalCastingCount)
						{
							if (singleMetalCastingCount == 1)
							{
								GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalPrimary, MaterialClassification.MetalSecondary);
							}
							else
							{
								GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalPrimary);
								GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalSecondary);
							}
						}
						else
						{
							GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalPrimary);
							GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalSecondary);
						}
					}
					else
					{
						GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalPrimary);
						GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalSecondary);
					}
				}
				else
				{
					GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalPrimary);
				}
				break;
			case ComponentsEnum.Band1:
			case ComponentsEnum.Band2:
				if (AreSecondaryMetalObjectsVisibleForComponent(visibleComponent))
				{
					GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalPrimary);
					GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalSecondary);
				}
				else
				{
					GetComponentVolume(visibleComponent, ref currentVolumeList, MaterialClassification.MetalPrimary);
				}
				break;
			default:
				GlobalErrorHandler.WriteInfoToEventLog($"Invalid ComponentEnum type of {visibleComponent}");
				break;
			}
		}
		return currentVolumeList;
	}

	public bool ExportDesignAsSTL(string savePath, bool includeCenter, bool includeSide, bool includeAccent, bool repairMesh)
	{
		bool result = true;
		CSUtil.Print($"ExportDesignParts - Path: {savePath}\n");
		RhUtil.RhinoApp().RunScript("_ClearUndo", 0);
		RhUtil.RhinoApp().RunScript("_ClearAllMeshes", 0);
		CSUtil.EnableDrawing(which: false);
		CSApplicationHelper.Application.IsCenterGemAllowedForPrinting = includeCenter;
		CSApplicationHelper.Application.IsSideGemAllowedForPrinting = includeSide;
		CSApplicationHelper.Application.IsAccentGemAllowedForPrinting = includeAccent;
		CSApplicationHelper.Application.IsExportingSTL = true;
		CSApplicationHelper.Application.WhenExportingHonorGemPrintingFlags = true;
		CSUtil.AddModelToRhino();
		CSApplicationHelper.Application.WhenExportingHonorGemPrintingFlags = false;
		CSApplicationHelper.Application.IsExportingSTL = false;
		try
		{
			MeshRepair.CreateSTLFromRhinoObjects(repairMesh, saveToDisk: true, savePath);
		}
		catch (Exception)
		{
			result = false;
		}
		CSUtil.DeleteModelFromRhino();
		CSUtil.EnableDrawing(which: true);
		RhUtil.RhinoApp().RunScript("_ClearUndo", 0);
		RhUtil.RhinoApp().RunScript("_ClearAllMeshes", 0);
		RhUtil.RhinoApp().ActiveDoc().SetModifiedFlag(false);
		CSUtil.Print("ExportDesignParts - Completed.\n");
		CSUtil.Redraw();
		return result;
	}

	public bool ExportDesignAs3DM(string savePath, bool includeCenter, bool includeSide, bool includeAccent)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		bool result = true;
		try
		{
			GlobalErrorHandler.WriteInfoToEventLog($"ExportDesignParts - Path: {savePath}\n");
			RhUtil.RhinoApp().RunScript("_ClearUndo", 0);
			RhUtil.RhinoApp().RunScript("_ClearAllMeshes", 0);
			CSUtil.EnableDrawing(which: false);
			CSApplicationHelper.Application.IsCenterGemAllowedForPrinting = includeCenter;
			CSApplicationHelper.Application.IsSideGemAllowedForPrinting = includeSide;
			CSApplicationHelper.Application.IsAccentGemAllowedForPrinting = includeAccent;
			CSApplicationHelper.Application.DisableMeshObjectMeshing = true;
			CSUtil.ForceGraphToRedraw(((IEnumerable<CSBlock>)CSApplicationHelper.Application.Document.Graph.CSBlocks.OfType<CSMeshObject>()).ToList());
			CSApplicationHelper.Application.WhenExportingHonorGemPrintingFlags = true;
			CSUtil.AddModelToRhino();
			CSApplicationHelper.Application.WhenExportingHonorGemPrintingFlags = false;
			CSApplicationHelper.Application.DisableMeshObjectMeshing = false;
			string tempPath = FileSystemHelper.GetTempPath("CounterSketchStudio", Path.GetExtension(savePath));
			RhUtil.RhinoApp().Print("Starting saving local file\n");
			DateTime now = DateTime.Now;
			FileWriteOptions val = new FileWriteOptions
			{
				SuppressDialogBoxes = true,
				WriteSelectedObjectsOnly = false,
				IncludeRenderMeshes = false,
				IncludeBitmapTable = false,
				WriteUserData = true
			};
			if (!RhinoDoc.ActiveDoc.WriteFile(tempPath, val))
			{
				GlobalErrorHandler.WriteErrorToEventLog("Failed to write file " + tempPath + "\n");
				result = false;
			}
			else
			{
				DateTime now2 = DateTime.Now;
				GlobalErrorHandler.WriteInfoToEventLog("Saving local finished after " + (now2 - now).Seconds + " seconds\n");
				GlobalErrorHandler.WriteInfoToEventLog("Starting saving remote file\n");
				DateTime now3 = DateTime.Now;
				FileSystemHelper.CopyFile(tempPath, savePath);
				DateTime now4 = DateTime.Now;
				GlobalErrorHandler.WriteInfoToEventLog("Saving remote finished after " + (now4 - now3).Seconds + " seconds\n");
				if (!FileSystemHelper.DeleteFile(tempPath))
				{
					RhUtil.RhinoApp().Print("Error deleting file " + tempPath + "\n");
				}
			}
		}
		catch (Exception)
		{
			result = false;
		}
		CSUtil.DeleteModelFromRhino();
		CSUtil.EnableDrawing(which: true);
		RhUtil.RhinoApp().RunScript("_ClearUndo", 0);
		RhUtil.RhinoApp().RunScript("_ClearAllMeshes", 0);
		RhUtil.RhinoApp().ActiveDoc().SetModifiedFlag(false);
		CSUtil.Print("ExportDesignParts - Completed.\n");
		CSUtil.Redraw();
		return result;
	}

	public string GetRhinoSerialCode()
	{
		return RhinoApp.SerialNumber;
	}

	public bool IsRhino64Bit()
	{
		return System.Environment.Is64BitProcess;
	}

	public List<int> GetSavedJobs()
	{
		return _projetPrinter.GetSavedJobs();
	}

	public bool DeleteSavedJob(int whatjob)
	{
		return _projetPrinter.DeleteSavedJob(whatjob);
	}

	public bool GetActivationState()
	{
		return _projetPrinter.GetActivationState();
	}

	public bool CheckFirmwareVersion()
	{
		return _projetPrinter.CheckFirmwareVersion();
	}

	public bool UpdateFirmwareAndActivate(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany, out string sError)
	{
		return _projetPrinter.UpdateFirmwareAndActivate(bDoFirmware, bDoActivate, sEmail, sCompany, out sError);
	}

	public bool PrintMultipleJobs(List<int> lstJobsToPrint, out string sStatus)
	{
		return _projetPrinter.PrintMultipleJobs(lstJobsToPrint, out sStatus);
	}

	public bool SavePrintJob(int whatslot, out string sStatus)
	{
		return _projetPrinter.SaveCurrentJob(whatslot, out sStatus);
	}

	public bool ClearUnifiedMesh()
	{
		if (_projetPrinter != null)
		{
			_projetPrinter.ResetProjet();
		}
		return true;
	}

	public void PrintingSupportDisplayConduitEnabled(bool isEnabled)
	{
		if (_projetPrinter != null)
		{
			_projetPrinter.DrawingConduitEnabled = isEnabled;
		}
	}

	public bool SetSupportScales(double SupScale, int SupType)
	{
		_projetPrinter.SetSupportScales(SupScale, SupType);
		return true;
	}

	public bool SetMeshFromComponent(List<ComponentsEnum> whichComponents, bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed, out string sError)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		CSApplicationHelper.Application.IsCenterGemAllowedForPrinting = isCenterGemAllowed;
		CSApplicationHelper.Application.IsSideGemAllowedForPrinting = isSideGemAllowed;
		CSApplicationHelper.Application.IsAccentGemAllowedForPrinting = isAccentGemAllowed;
		ConcurrentBag<GeometryBase> geometryBases = new ConcurrentBag<GeometryBase>();
		new List<CSBlock>().Union(CSApplicationHelper.Application.Document.Graph.DisplayObjects()).ToList().ToList()
			.ForEach(delegate(CSBlock csBlock)
			{
				csBlock.GetMeshableObjects(ref geometryBases);
			});
		List<Mesh> meshList = MeshRepairHelper.GetMeshList(geometryBases.ToList());
		Mesh val = new Mesh();
		foreach (Mesh item in meshList)
		{
			val.Append(item);
		}
		if (_projetPrinter == null)
		{
			_projetPrinter = new ProjetHelper();
		}
		return _projetPrinter.SetCSItemToPrintPreview(val, out sError);
	}

	public bool IsProjetDialogLoaded()
	{
		if (_projetPrinter == null)
		{
			_projetPrinter = new ProjetHelper();
		}
		return _projetPrinter.IsProjetDialogLoaded();
	}

	public void SetLoadedDialogState(bool bState)
	{
		_projetPrinter.SetLoadedDialogState(bState);
	}

	public bool CSCreateSupportsForCSItem(out string ErrorMessage)
	{
		return _projetPrinter.CSCreateSupportsForCSItem(out ErrorMessage);
	}

	public void SetUnifiedMeshLocation(double dHeight)
	{
		_projetPrinter.UpdateMeshLocation(dHeight);
	}

	public void SetUnifiedMeshRotation(double angleX, double angleY, double angleZ)
	{
		string errorMessage = string.Empty;
		_projetPrinter.UpdateMeshRotation(angleX, angleY, angleZ, out errorMessage);
	}

	public bool FindProjetPrinter(out string FoundAddress)
	{
		if (_projetPrinter == null)
		{
			_projetPrinter = new ProjetHelper();
		}
		return _projetPrinter.FindProjetPrinter(out FoundAddress);
	}

	public bool CheckConnectionToProJet1200(string connectionAddress, out string sError, out string sStatus)
	{
		return _projetPrinter.ConnectToPrinter(connectionAddress, out sError, out sStatus);
	}

	public bool GetPrinterMaterial(out string message)
	{
		message = _projetPrinter.GetPrinterMaterial();
		return true;
	}

	public void SetSupportDensity(int iSupportDensity)
	{
		_projetPrinter.SetSupportDensity(iSupportDensity);
	}

	public bool PrintUnifiedMeshToProJet1200(out string sErr)
	{
		sErr = string.Empty;
		return _projetPrinter.PrintCSModel(out sErr);
	}

	public string AbortPrintJob()
	{
		return _projetPrinter.AbortBuildJob();
	}

	public void SetPerspectiveViewActiveAndMaximize()
	{
		CSDisplayCaptureHelper.CreateScreenCaptureView();
		RhinoView val = ((IEnumerable<RhinoView>)RhinoDoc.ActiveDoc.Views).FirstOrDefault((RhinoView _) => string.Equals(_.ActiveViewport.Name, "Perspective", StringComparison.OrdinalIgnoreCase));
		if (val != null)
		{
			val.TitleVisible = false;
			val.Maximized = true;
			RhinoDoc.ActiveDoc.Views.ActiveView = val;
		}
	}

	public void Shutdown()
	{
		RhinoDoc.ActiveDoc.Modified = false;
		RhinoApp.Exit();
	}

	public List<UIControl> DeleteImportedGroupFromGraph(List<Guid> importGroupIdentifiers)
	{
		return ImportGroupHelper.DeleteImportedGroupFromGraph(importGroupIdentifiers);
	}

	public void DocumentDuplicateCSGPart(List<Guid> importGroupIdentifiers)
	{
		ImportGroupHelper.DocumentDuplicateCSGPart(importGroupIdentifiers);
	}

	public void ImportGroupAddRingRailGuide()
	{
		ImportGroupHelper.DocumentImportCSGPart(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, "Parts\\RingRailGuide.csg"));
	}

	public void DocumentImportCSGPart(string pathToCSGPart, int? centerGemGraphIndex = null, bool doShowFontSourceErrors = true)
	{
		ImportGroupHelper.DocumentImportCSGPart(pathToCSGPart, centerGemGraphIndex, doShowFontSourceErrors);
	}

	public List<UIControl> InvokeAddAPartPatchwire(string path, string name)
	{
		return ImportGroupHelper.InvokeAddAPartPatchwire(path, name);
	}

	public List<UIControl> InvokeGemPatchWire(GemActions action)
	{
		return new List<UIControl>();
	}

	public void RequestScreenCaptureAtPerspectivePositionForImportGroup(Size captureSize, bool zoom, List<Guid> importGroupIdentifiers)
	{
		ImportGroupHelper.RequestScreenCaptureAtPerspectivePositionForImportGroup(captureSize, zoom, importGroupIdentifiers);
	}

	public List<UIControl> UpdateUIControl(UIControl uiControl)
	{
		return UIControlHelper.UpdateUIControl(uiControl);
	}

	public List<UIControl> UpdateUIControlQuick(UIControl uiControl)
	{
		return UIControlHelper.UpdateUIControlQuick(uiControl);
	}

	public void UpdateValueChooserFromUI(Guid blockInstanceGuid)
	{
		UIControlHelper.UpdateValueChooserFromUI(blockInstanceGuid);
	}

	public void SetImportGroupToPrimaryMetal(List<Guid> importGroupIdentifiers)
	{
		ImportGroupHelper.UpdateImportGroupMetalMaterial(importGroupIdentifiers, isPrimaryMetal: true);
	}

	public void SetImportGroupToSecondaryMetal(List<Guid> importGroupIdentifiers)
	{
		ImportGroupHelper.UpdateImportGroupMetalMaterial(importGroupIdentifiers, isPrimaryMetal: false);
	}

	public void SetImportGroupVisibilityByIdentifier(List<Guid> importGroupIdentifiers, bool isViewportVisible)
	{
		ImportGroupHelper.SetImportGroupVisibilityByIdentifier(importGroupIdentifiers, isViewportVisible);
	}

	public void UpdateImportGroupPartListGroupAssignment(List<Guid> importGroupIdentifiers, string groupName)
	{
	}

	public void ImportGroupAddToPartGroup(List<Guid> importGroupIdentifiers)
	{
		ImportGroupHelper.ImportGroupAddToPartGroup(importGroupIdentifiers);
	}

	public void ImportGroupBreakPartGroup(Guid importGroupIdentifier)
	{
		ImportGroupHelper.ImportGroupBreakPartGroup(importGroupIdentifier);
	}

	public void ImportGroupRename(Guid importGroupIdentifier, string updatedName)
	{
		ImportGroupHelper.ImportGroupRename(importGroupIdentifier, updatedName);
	}

	public IntPtr GetRhinoWindowHandle()
	{
		if (_intPtrRhino == IntPtr.Zero)
		{
			MRhinoApp val = RhUtil.RhinoApp();
			if (val != null)
			{
				_intPtrRhino = val.MainWnd().Handle;
			}
		}
		if (_intPtrRhino == IntPtr.Zero)
		{
			GlobalErrorHandler.WriteErrorToEventLog("ERROR *** GetRhinoWindowHandle() : Failed to Find Rhino Window Handle.");
		}
		return _intPtrRhino;
	}

	public void ResetHandleForImportGroup(Guid importGroupIdentifier)
	{
		ImportGroupHelper.ResetHandleForImportGroup(importGroupIdentifier);
	}

	public bool RhinoWindowReparent(IntPtr intPtrParent)
	{
		if (intPtrParent == IntPtr.Zero)
		{
			return false;
		}
		if (_intPtrRhino == IntPtr.Zero)
		{
			MRhinoApp val = RhUtil.RhinoApp();
			if (val != null)
			{
				_intPtrRhino = val.MainWnd().Handle;
			}
		}
		if (_intPtrRhino == IntPtr.Zero)
		{
			GlobalErrorHandler.WriteErrorToEventLog("ERROR *** RhinoWindowReparent() : Failed to Find Rhino Window Handle.");
			return false;
		}
		if (!Win32Utility.ReparentWindow(_intPtrRhino, intPtrParent))
		{
			GlobalErrorHandler.WriteErrorToEventLog("ERROR *** RhinoWindowReparent() : Failed to reparent Rhino Window.");
			int lastWin32Error = Marshal.GetLastWin32Error();
			GlobalErrorHandler.WriteErrorToEventLog($"ERROR # RETURNED WAS : {lastWin32Error}");
			Win32Exception ex = new Win32Exception(lastWin32Error);
			GlobalErrorHandler.WriteErrorToEventLog("ERROR MESSAGE RETURNED WAS : " + ex.Message);
			GlobalErrorHandler.WriteErrorToEventLog($"FULL EXCEPTION WAS : {ex}");
			return false;
		}
		Win32Utility.PositionWindow(_intPtrRhino, -10000, -10000, 10, 10);
		Win32Utility.SetWindowStyle(_intPtrRhino, Win32Utility.WindowStyles.WS_VISIBLE);
		if (!Win32Utility.ShowWindow(_intPtrRhino, 5u))
		{
			GlobalErrorHandler.WriteErrorToEventLog("Failed to Show Rhino Window.");
			return false;
		}
		SetPerspectiveViewActiveAndMaximize();
		return true;
	}

	public void DocumentDeleteDesignFromRhino()
	{
	}

	public void DocumentInitializeFreehandStackControls()
	{
		ImportGroupHelper.InitializeStackControls();
	}

	public void InitializeProjet()
	{
		if (_projetPrinter == null)
		{
			_projetPrinter = new ProjetHelper();
		}
	}

	private void AddCachedMeshItem(CachedMeshItem item, double volume)
	{
		_volumeCacheDictionary.Add(item, volume);
	}

	private void Application_FreehandObjectSelectionChanged(object sender, CSApplication.SelectedHandlesEventArgs e)
	{
		ImportGroupHelper.UpdateActiveImportGroupUsingHandleArgs(e);
	}

	private void Application_RhinoWindowClickedEvent(object sender, EventArgs e)
	{
		if (StudioUICallback != null)
		{
			try
			{
				StudioUICallback.RhinoWindowClickedCallback();
			}
			catch (Exception arg)
			{
				GlobalErrorHandler.WriteErrorToEventLog($"Callback Channel Error: {arg}");
			}
		}
	}

	private static uint ColorToUInt(System.Drawing.Color color)
	{
		return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
	}

	private bool CompareCachedMeshItem(CachedMeshItem meshItem1, CachedMeshItem meshItem2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox box = meshItem1.Box;
		Point3d min = ((BoundingBox)(ref box)).Min;
		box = meshItem2.Box;
		if (min == ((BoundingBox)(ref box)).Min)
		{
			box = meshItem1.Box;
			Point3d max = ((BoundingBox)(ref box)).Max;
			box = meshItem2.Box;
			if (max == ((BoundingBox)(ref box)).Max && meshItem1.VerticeCount == meshItem2.VerticeCount)
			{
				return meshItem1.FaceCount == meshItem2.FaceCount;
			}
		}
		return false;
	}

	private bool DoesCachedMeshItemExist(CachedMeshItem item)
	{
		return _volumeCacheDictionary.Any((KeyValuePair<CachedMeshItem, double> _) => CompareCachedMeshItem(_.Key, item));
	}

	private void DuplicateBand()
	{
		if (!CSApplicationHelper.Application.Document.Graph.PatchWireStartObjects().Any((CSPatchWireStart _) => _.DisplayComponent == DisplayComponents.Band2) && !CSApplicationHelper.Application.Document.Graph.PatchWireStartObjects().All((CSPatchWireStart _) => _.DisplayComponent != DisplayComponents.Band))
		{
			SetComponentVisibility(ComponentsEnum.Band1, visible: true);
			CSPatchWireStart cSPatchWireStart = CSUtil.CreatePatchWireStart("Duplicated Band", Guid.Parse("B2F543E6-61F1-4F29-89E7-65B34B514792"), 1, DisplayComponents.Band2);
			if (cSPatchWireStart != null)
			{
				cSPatchWireStart.UserData.AddKey("UI", "ControlColorBrush", "Utilities");
				cSPatchWireStart.UserData.AddKey("UI", "IconImagePath", "Images\\PatchWire\\NEW.png");
			}
			List<CSOutputChannel> outputChannelsToMirror = new List<CSOutputChannel>();
			(from _ in CSApplicationHelper.Application.Document.Graph.DisplayObjects()
				where _.ParentPatchWire.DisplayComponent == DisplayComponents.Band
				select _).ToList().ForEach(delegate(CSBlock _)
			{
				outputChannelsToMirror.AddRange(_.OutputChannels);
			});
			CSArray bandObjects = new CSArray("Band Objects");
			outputChannelsToMirror.ForEach(delegate(CSOutputChannel _)
			{
				bandObjects.AddLink(_);
			});
			CSValue cSValue = new CSValue("Mirror Y", 1.0);
			CSValue cSValue2 = new CSValue("Mirror X", 1.0);
			CSMirror cSMirror = new CSMirror("Band Mirror");
			cSMirror.InObj.SetLink(bandObjects.OutArray);
			cSMirror.InY.SetLink(cSValue.OutValue);
			cSMirror.InX.SetLink(cSValue2.OutValue);
			CSUtil.AddToDisplayAndGenerate(cSMirror);
			SetComponentVisibility(ComponentsEnum.Band1, visible: false);
		}
	}

	private static IntPtr FindRhino()
	{
		IntPtr result = default(IntPtr);
		if (RhUtil.RhinoApp() != null && RhUtil.RhinoApp().MainWnd() != null)
		{
			return RhUtil.RhinoApp().MainWnd().Handle;
		}
		return result;
	}

	private CachedMeshItem GetCachedMeshItemFromMesh(Mesh mesh)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		return new CachedMeshItem
		{
			VerticeCount = mesh.Vertices.Count,
			FaceCount = mesh.Faces.Count,
			Box = ((GeometryBase)mesh).GetBoundingBox(true)
		};
	}

	private void GetComponentVolume(ComponentsEnum visibleComponent, ref Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> currentVolumeList, params MaterialClassification[] materials)
	{
		double num = 0.0;
		DebugHelper.PrintTimedOutput("- GetDisplayObjectsByComponentAndMaterialClassification");
		List<CSData> displayObjectsByComponentAndMaterialClassification = GraphHelper.GetDisplayObjectsByComponentAndMaterialClassification(visibleComponent, materials);
		DebugHelper.PrintTimedOutput("- CSDataHelper.ConvertToVolumeableMesh");
		Mesh val = ((displayObjectsByComponentAndMaterialClassification.Count <= 0) ? null : CSDataHelper.ConvertToVolumeableMesh(displayObjectsByComponentAndMaterialClassification));
		if (val == null)
		{
			return;
		}
		DebugHelper.PrintTimedOutput("- GetCachedMeshItemFromMesh");
		CachedMeshItem cachedMeshItem = GetCachedMeshItemFromMesh(val);
		if (DoesCachedMeshItemExist(cachedMeshItem) && cachedMeshItem != null)
		{
			KeyValuePair<CachedMeshItem, double> keyValuePair = _volumeCacheDictionary.FirstOrDefault((KeyValuePair<CachedMeshItem, double> _) => CompareCachedMeshItem(_.Key, cachedMeshItem));
			if (keyValuePair.Value > 0.0)
			{
				currentVolumeList.Add(new Tuple<ComponentsEnum, MaterialClassification>(visibleComponent, materials[0]), keyValuePair.Value);
				return;
			}
		}
		DebugHelper.PrintTimedOutput("- MeshRepairHelper.GetVolumeViaOctTree");
		num = MeshRepairHelper.GetVolumeViaOctTree(new List<Mesh> { val });
		DebugHelper.PrintTimedOutput("- Added Volume to Cache");
		currentVolumeList.Add(new Tuple<ComponentsEnum, MaterialClassification>(visibleComponent, materials[0]), num);
		AddCachedMeshItem(cachedMeshItem, num);
	}

	private static string GetPathOfAssembly()
	{
		return Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
	}

	private void SetRhinoOptionsforCS()
	{
		ShortcutKeySettings.SetMacro((ShortcutKey)66, "");
		GeneralSettings.EnableContextMenu = false;
		AppearanceSettings.GridThinLineColor = System.Drawing.Color.FromArgb(255, 219, 219, 219);
		AppearanceSettings.GridThickLineColor = System.Drawing.Color.FromArgb(255, 168, 168, 168);
		AppearanceSettings.GridXAxisLineColor = System.Drawing.Color.FromArgb(255, 240, 90, 97);
		AppearanceSettings.GridYAxisLineColor = System.Drawing.Color.FromArgb(255, 121, 175, 235);
		AppearanceSettings.GridZAxisLineColor = System.Drawing.Color.GreenYellow;
		AppearanceSettings.WorldCoordIconXAxisColor = System.Drawing.Color.FromArgb(255, 240, 90, 97);
		AppearanceSettings.WorldCoordIconYAxisColor = System.Drawing.Color.FromArgb(255, 121, 175, 235);
		AppearanceSettings.WorldCoordIconZAxisColor = System.Drawing.Color.GreenYellow;
		AppearanceSettings.MenuVisible = false;
		AppearanceSettings.ViewportBackgroundColor = System.Drawing.Color.White;
		AppearanceSettings.FrameBackgroundColor = System.Drawing.Color.White;
		FileSettings.CreateBackupFiles = false;
		FileSettings.AutoSaveEnabled = false;
		RhinoViewport activeViewport = RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;
		ConstructionPlane constructionPlane = activeViewport.GetConstructionPlane();
		constructionPlane.GridLineCount = 20;
		constructionPlane.GridSpacing = 1.0;
		constructionPlane.ThickLineFrequency = 5;
		activeViewport.SetConstructionPlane(constructionPlane);
	}

	~RhinoServices()
	{
		CSApplicationHelper.Application.FreehandObjectSelectionChangedEvent -= Application_FreehandObjectSelectionChanged;
		CSApplicationHelper.Application.RhinoWindowClickedEvent -= Application_RhinoWindowClickedEvent;
	}
}
