using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CS;
using CS.Parametric;
using CSMaterials;
using CounterSketchServer.Utilities.Extensions;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.FileIO;
using Rhino.PlugIns;
using Rhino.Render;

namespace CounterSketchServer.Utilities.Helpers;

public class RenderHelper
{
	private static readonly string VrayDesignMaterialsPath = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "RenderMaterials\\Design Materials");

	private static readonly string VraySkinMaterialsPath = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "RenderMaterials\\Skin");

	public static double TotalCustomDesignRenderTime;

	public static int TotalCustomDesignsRendered;

	private static string _renderOutputFile = string.Empty;

	private static string _visMatCurrentlyLoaded;

	private static bool isRenderPluginSet;

	public static void GenerateCustomRender(bool isRenderLayDesignFlat, bool isRenderUsePropsSelected, string renderPropSwatchCollectionItem, string renderPropMaterialSwatchCollectionItem, string renderPropMaterialSwatchCollectionSecondaryItem, string groundPlaneMaterial, string renderResolution, string renderEnvironment, bool saveRender, string saveRenderLocation, bool alphaProps, bool alphaGroundPlane)
	{
		Stopwatch stopwatch = new Stopwatch();
		Stopwatch stopwatch2 = new Stopwatch();
		stopwatch.Start();
		SetRendererToVray();
		List<string> sceneMaterialNames = VRayInterface.GetSceneMaterialNames();
		GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.GetSceneMaterialNames() - Found: {sceneMaterialNames.Count}");
		stopwatch2.Start();
		LoadVrayMaterials("fullenv.visopt");
		RhinoDoc.ActiveDoc.ClearUndoRecords(purgeDeletedObjects: true);
		RhinoDoc.ActiveDoc.ClearRedoRecords();
		stopwatch2.Stop();
		string text = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "RenderMaterials\\" + renderEnvironment + ".visopt");
		string text2 = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "RenderMaterials");
		VRayInterface.AddResourceSearchPath(text2);
		GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.AddResourceSearchPath({text2})");
		VRayInterface.LoadVisopt(text);
		GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.LoadVisopt({text})");
		if (saveRender)
		{
			string path = "Render.png".AppendTimeStampToFilename();
			if (!Directory.Exists(saveRenderLocation))
			{
				Directory.CreateDirectory(saveRenderLocation);
			}
			_renderOutputFile = Path.Combine(saveRenderLocation, path);
			File.Delete(_renderOutputFile);
			VRayInterface.SetRenderOutputSaveFile(_renderOutputFile);
			GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.SetRenderOutputSaveFile({_renderOutputFile})");
			VRayInterface.SetRenderOutputEnable(enable: true);
			GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.SetRenderOutputEnable({true})");
		}
		VRayInterface.SetBatchRender(enabled: true);
		GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.SetBatchRender({true})");
		VRayInterface.SetThreadPriorityOn(enabled: true);
		GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.SetThreadPriorityOn({true})");
		VRayInterface.SetProgressWindowOn(enabled: true);
		GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.SetProgressWindowOn({true})");
		CSUtil.EnableDrawing(which: false);
		ConcurrentBag<RhinoRenderObject> concurrentBag = CSUtil.AddRenderObjectsToRhino();
		try
		{
			double num = 0.0;
			double num2 = 0.0;
			foreach (RhinoRenderObject item in concurrentBag)
			{
				if (item.MRhinoObject != null)
				{
					OnBoundingBox onBoundingBox = item.MRhinoObject.BoundingBox();
					if (onBoundingBox.m_min.z < num)
					{
						num = onBoundingBox.m_min.z;
					}
					if (onBoundingBox.m_max.y < num2)
					{
						num2 = onBoundingBox.m_min.y;
					}
				}
			}
			if ((CSUtil.DisplayComponentsInFile() & DisplayComponents.Pendant) != DisplayComponents.Pendant && isRenderLayDesignFlat)
			{
				num = 0.0;
			}
			MRhinoObject rh_object = null;
			if (isRenderUsePropsSelected)
			{
				ImportFile(CSApplicationHelper.Application.CSLibraryPath + "\\Props\\" + renderPropSwatchCollectionItem + ".3dm");
			}
			else
			{
				ImportFile(CSApplicationHelper.Application.CSLibraryPath + "\\Props\\Groundplane.3dm");
				OnXform onXform = new OnXform();
				onXform.Translation(0.0, 0.0, num);
				rh_object = GetNamedObjects("GroundPlane").FirstOrDefault();
				MRhinoObjRef objref = new MRhinoObjRef(rh_object);
				RhUtil.RhinoApp().ActiveDoc().TransformObject(ref objref, onXform);
			}
			if (RhUtil.RhinoApp().ActiveView().ActiveViewport()
				.View()
				.m_vp.CameraLocation().z < num)
			{
				MessageBox.Show("Camera is below Ground Plane. Ground Plane will be turned off for this render");
				RhUtil.RhinoApp().RunScript("_-Selname GroundPlane _Delete");
			}
			if (isRenderUsePropsSelected)
			{
				List<MRhinoObject> namedObjects = GetNamedObjects("PrimaryMaterial");
				if (namedObjects.Count > 0)
				{
					string text3 = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "rendermaterials\\groundplanes\\" + renderPropMaterialSwatchCollectionItem + ".vismat");
					bool flag;
					if (sceneMaterialNames != null && !sceneMaterialNames.Any((string v) => v.Contains("/" + renderPropMaterialSwatchCollectionItem)))
					{
						flag = VRayInterface.LoadVismat(text3);
						GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.LoadVismat({0})", text3, flag));
						sceneMaterialNames = VRayInterface.GetSceneMaterialNames();
						GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.GetSceneMaterialNames() - Found: {sceneMaterialNames.Count}");
					}
					GlobalErrorHandler.WriteInfoToEventLog("Primary Prop AlphaSetting: " + VRayInterface.GetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionItem));
					if (alphaProps)
					{
						flag = VRayInterface.SetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionItem, 0f);
						GlobalErrorHandler.WriteInfoToEventLog($"SetAlphaContributionForMaterial Result: {flag}");
					}
					else
					{
						flag = VRayInterface.SetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionItem, 1f);
						GlobalErrorHandler.WriteInfoToEventLog($"SetAlphaContributionForMaterial Result: {flag}");
					}
					GlobalErrorHandler.WriteInfoToEventLog("Primary Prop New AlphaSetting: " + VRayInterface.GetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionItem));
					flag = VRayInterface.ApplyVRayMaterialToObjects("/" + renderPropMaterialSwatchCollectionItem, namedObjects);
					GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.ApplyVRayMaterialToObjects({0}, {2})", "/" + renderPropMaterialSwatchCollectionItem, flag, namedObjects));
				}
				namedObjects = GetNamedObjects("SecondaryMaterial");
				if (namedObjects.Count > 0)
				{
					string text4 = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "rendermaterials\\groundplanes\\" + renderPropMaterialSwatchCollectionSecondaryItem + ".vismat");
					bool flag;
					if (sceneMaterialNames != null && !sceneMaterialNames.Any((string v) => v.Contains("/" + renderPropMaterialSwatchCollectionSecondaryItem)))
					{
						flag = VRayInterface.LoadVismat(text4);
						GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.LoadVismat({0})", text4, flag));
						sceneMaterialNames = VRayInterface.GetSceneMaterialNames();
						GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.GetSceneMaterialNames() - Found: {sceneMaterialNames.Count}");
					}
					GlobalErrorHandler.WriteInfoToEventLog("Secondary Prop AlphaSetting: " + VRayInterface.GetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionSecondaryItem));
					if (alphaProps)
					{
						flag = VRayInterface.SetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionSecondaryItem, 0f);
						GlobalErrorHandler.WriteInfoToEventLog($"SetAlphaContributionForMaterial Result: {flag}");
					}
					else
					{
						flag = VRayInterface.SetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionSecondaryItem, 1f);
						GlobalErrorHandler.WriteInfoToEventLog($"SetAlphaContributionForMaterial Result: {flag}");
					}
					GlobalErrorHandler.WriteInfoToEventLog("Secondary Prop New AlphaSetting: " + VRayInterface.GetAlphaContributionForMaterial("/" + renderPropMaterialSwatchCollectionSecondaryItem));
					flag = VRayInterface.ApplyVRayMaterialToObjects("/" + renderPropMaterialSwatchCollectionSecondaryItem, namedObjects);
					GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.ApplyVRayMaterialToObjects({0}, {2})", "/" + renderPropMaterialSwatchCollectionItem, flag, namedObjects));
				}
			}
			List<MRhinoObject> namedObjects2 = GetNamedObjects("GroundPlane");
			if (namedObjects2.Count > 0)
			{
				string text5 = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "rendermaterials\\Groundplanes\\" + groundPlaneMaterial + ".vismat");
				bool flag2;
				if (sceneMaterialNames != null && !sceneMaterialNames.Any((string v) => v.Contains(groundPlaneMaterial)))
				{
					flag2 = VRayInterface.LoadVismat(text5);
					GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.LoadVismat({0})", text5, flag2));
				}
				GlobalErrorHandler.WriteInfoToEventLog("Ground Plane AlphaSetting: " + VRayInterface.GetAlphaContributionForMaterial("/" + groundPlaneMaterial));
				if (alphaGroundPlane)
				{
					flag2 = VRayInterface.SetAlphaContributionForMaterial("/" + groundPlaneMaterial, 0f);
					GlobalErrorHandler.WriteInfoToEventLog($"SetAlphaContributionForMaterial Result: {flag2}");
				}
				else
				{
					flag2 = VRayInterface.SetAlphaContributionForMaterial("/" + groundPlaneMaterial, 1f);
					GlobalErrorHandler.WriteInfoToEventLog($"SetAlphaContributionForMaterial Result: {flag2}");
				}
				GlobalErrorHandler.WriteInfoToEventLog("Ground Plane New AlphaSetting: " + VRayInterface.GetAlphaContributionForMaterial("/" + groundPlaneMaterial));
				flag2 = VRayInterface.ApplyVRayMaterialToObjects("/" + groundPlaneMaterial, namedObjects2);
				GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.ApplyVRayMaterialToObjects({0}, {2})", "/" + groundPlaneMaterial, flag2, namedObjects2));
			}
			foreach (var item2 in (from displayRenderObject in concurrentBag.AsParallel()
				where displayRenderObject.MRhinoObject != null
				select displayRenderObject).GroupBy((RhinoRenderObject renderObject) => renderObject.AssignedMaterialGroup.MaterialName, (string material, IEnumerable<RhinoRenderObject> renderItems) => new
			{
				Material = material,
				RenderItems = renderItems
			}).ToList())
			{
				string materialName = item2.Material;
				string text6 = $"{materialName}.vismat";
				string text7 = string.Empty;
				if (File.Exists(Path.Combine(VrayDesignMaterialsPath, text6)))
				{
					text7 = Path.Combine(VrayDesignMaterialsPath, text6);
				}
				else if (File.Exists(Path.Combine(VraySkinMaterialsPath, text6)))
				{
					text7 = Path.Combine(VraySkinMaterialsPath, text6);
				}
				bool flag3;
				if (sceneMaterialNames != null && !sceneMaterialNames.Any((string v) => v.Equals("/" + materialName)))
				{
					if (!string.IsNullOrWhiteSpace(text7))
					{
						flag3 = VRayInterface.LoadVismat(text7);
						GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.LoadVismat({0})", text7, flag3));
						sceneMaterialNames = VRayInterface.GetSceneMaterialNames();
						GlobalErrorHandler.WriteInfoToEventLog($"VRayInterface.GetSceneMaterialNames() - Found: {sceneMaterialNames.Count}");
					}
					else
					{
						GlobalErrorHandler.WriteErrorToEventLog($"ERROR: Material Missing - Name: {text6} - Path: {text7}");
					}
				}
				List<MRhinoObject> list = item2.RenderItems.Select((RhinoRenderObject renderObject) => renderObject.MRhinoObject).ToList();
				flag3 = VRayInterface.ApplyVRayMaterialToObjects("/" + materialName, list);
				GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.ApplyVRayMaterialToObjects({0}, {2})", "/" + materialName, flag3, list));
			}
			RhUtil.RhinoApp().ActiveDoc().UnselectAll();
			string text8 = renderResolution.Split('H')[0].Substring(1);
			string text9 = renderResolution.Split('H')[1];
			if (string.IsNullOrEmpty(text8) || string.IsNullOrEmpty(text9))
			{
				return;
			}
			RhUtil.RhinoApp().RunScript("-_DocumentProperties _Render _Resolution _Type=_Custom _Width=" + text8 + " _Height=" + text9 + " _Enter _Enter _Enter");
			RhUtil.RhinoApp().ActiveDoc().UnselectAll();
			RhUtil.RhinoApp().RunScript("_Seldup");
			RhUtil.RhinoApp().RunScript("_Delete !");
			if (isRenderUsePropsSelected)
			{
				RhUtil.RhinoApp().ActiveDoc().UnselectAll();
				OnXform onXform2 = new OnXform();
				IEnumerable<Guid> namedObjectsByLayer = GetNamedObjectsByLayer("Props");
				MRhinoObjRef objref2 = new MRhinoObjRef(rh_object);
				switch (renderPropSwatchCollectionItem.ToLower())
				{
				case "block":
					onXform2.Translation(0.0, 0.0, num);
					foreach (Guid item3 in namedObjectsByLayer)
					{
						MRhinoObjRef objref5 = new MRhinoObjRef(item3);
						RhUtil.RhinoApp().ActiveDoc().TransformObject(ref objref5, onXform2);
					}
					RhUtil.RhinoApp().ActiveDoc().TransformObject(ref objref2, onXform2);
					break;
				case "clothdrop":
				case "pedestal":
					onXform2.Translation(0.0, 0.0, num);
					foreach (Guid item4 in namedObjectsByLayer)
					{
						MRhinoObjRef objref4 = new MRhinoObjRef(item4);
						RhUtil.RhinoApp().ActiveDoc().TransformObject(ref objref4, onXform2);
					}
					RhUtil.RhinoApp().ActiveDoc().TransformObject(ref objref2, onXform2);
					break;
				case "ringfinger":
				{
					IEnumerable<Guid> namedObjectsInverseByLayer = GetNamedObjectsInverseByLayer("Props");
					onXform2.Rotation(Math.PI * 2.0 / 9.0, OnUtil.On_xaxis, OnUtil.On_origin);
					foreach (Guid item5 in namedObjectsInverseByLayer)
					{
						MRhinoObjRef objref3 = new MRhinoObjRef(item5);
						RhUtil.RhinoApp().ActiveDoc().TransformObject(ref objref3, onXform2);
					}
					break;
				}
				}
				RhUtil.RhinoApp().ActiveDoc().Redraw();
			}
			stopwatch.Stop();
			GlobalErrorHandler.WriteInfoToEventLog("Time To Prepare Render: " + (stopwatch.ElapsedMilliseconds - stopwatch2.ElapsedMilliseconds) + "\nTime To Load Materials: " + stopwatch2.ElapsedMilliseconds + "\nTotal Time: " + stopwatch.ElapsedMilliseconds + "\n");
			RhUtil.RhinoApp().RunScript("_Render");
		}
		finally
		{
			RhUtil.RhinoApp().RunScript("_selall _delete");
			RhinoDoc.ActiveDoc.ClearUndoRecords(purgeDeletedObjects: true);
			RhinoDoc.ActiveDoc.ClearRedoRecords();
			CSUtil.EnableDrawing(which: true);
		}
		CSUtil.Redraw();
	}

	public static void LoadVrayMaterials(string visThisOpt)
	{
		if (visThisOpt != _visMatCurrentlyLoaded)
		{
			string text = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "RenderMaterials\\" + visThisOpt);
			bool flag = VRayInterface.LoadVisopt(text);
			GlobalErrorHandler.WriteInfoToEventLog(string.Format("{1} :: VRayInterface.LoadVisopt({0})", text, flag));
			_visMatCurrentlyLoaded = visThisOpt;
		}
	}

	[DllImport("user32.dll")]
	public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

	private static List<MRhinoObject> GetNamedObjects(string objName)
	{
		MRhinoObjectIterator mRhinoObjectIterator = new MRhinoObjectIterator(IRhinoObjectIterator.object_state.undeleted_objects, IRhinoObjectIterator.object_category.active_and_reference_objects);
		mRhinoObjectIterator.IncludeLights(bIncludeLights: false);
		mRhinoObjectIterator.IncludeGrips(bIncludeGrips: false);
		List<MRhinoObject> list = new List<MRhinoObject>();
		for (MRhinoObject mRhinoObject = mRhinoObjectIterator.First(); mRhinoObject != null; mRhinoObject = mRhinoObjectIterator.Next())
		{
			if (mRhinoObject.Attributes().m_name == objName)
			{
				list.Add(mRhinoObject);
			}
		}
		return list;
	}

	private static IEnumerable<Guid> GetNamedObjectsByLayer(string objName)
	{
		int num = RhinoDoc.ActiveDoc.Layers.Find(objName, ignoreDeletedLayers: true);
		if (num == -1)
		{
			return null;
		}
		MRhinoObjectIterator mRhinoObjectIterator = new MRhinoObjectIterator(IRhinoObjectIterator.object_state.undeleted_objects, IRhinoObjectIterator.object_category.active_and_reference_objects);
		mRhinoObjectIterator.IncludeLights(bIncludeLights: false);
		mRhinoObjectIterator.IncludeGrips(bIncludeGrips: false);
		List<Guid> list = new List<Guid>();
		for (MRhinoObject mRhinoObject = mRhinoObjectIterator.First(); mRhinoObject != null; mRhinoObject = mRhinoObjectIterator.Next())
		{
			if (mRhinoObject.Attributes().m_layer_index == num)
			{
				list.Add(mRhinoObject.Attributes().m_uuid);
			}
		}
		return list;
	}

	private static IEnumerable<Guid> GetNamedObjectsInverseByLayer(string objName)
	{
		int num = RhinoDoc.ActiveDoc.Layers.Find(objName, ignoreDeletedLayers: true);
		if (num == -1)
		{
			return null;
		}
		MRhinoObjectIterator mRhinoObjectIterator = new MRhinoObjectIterator(IRhinoObjectIterator.object_state.undeleted_objects, IRhinoObjectIterator.object_category.active_and_reference_objects);
		mRhinoObjectIterator.IncludeLights(bIncludeLights: false);
		mRhinoObjectIterator.IncludeGrips(bIncludeGrips: false);
		List<Guid> list = new List<Guid>();
		for (MRhinoObject mRhinoObject = mRhinoObjectIterator.First(); mRhinoObject != null; mRhinoObject = mRhinoObjectIterator.Next())
		{
			if (mRhinoObject.Attributes().m_layer_index != num)
			{
				list.Add(mRhinoObject.Attributes().m_uuid);
			}
		}
		return list;
	}

	public static void ImportFile(string filename)
	{
		FileReadOptions options = new FileReadOptions
		{
			ImportMode = true,
			ImportReferenceMode = false,
			ScaleGeometry = false
		};
		RhinoDoc.ReadFile(filename, options);
	}

	private static void SetRendererToVray()
	{
		if (isRenderPluginSet)
		{
			return;
		}
		string text = string.Empty;
		if (File.Exists("C:\\ProgramData\\ASGVIS\\VRayForMatrix.rhp"))
		{
			text = "C:\\ProgramData\\ASGVIS\\VRayForMatrix.rhp";
		}
		if (File.Exists("C:\\Program Files\\Rhinoceros 5.0\\Plug-ins\\VRayForRhino\\VRayForRhino.rhp"))
		{
			text = "C:\\Program Files\\Rhinoceros 5.0\\Plug-ins\\VRayForRhino\\VRayForRhino.rhp";
		}
		if (File.Exists("C:\\Program Files (x86)\\Rhinoceros 5.0\\Plug-ins\\VRayForRhino\\VRayForRhino.rhp"))
		{
			text = "C:\\Program Files (x86)\\Rhinoceros 5.0\\Plug-ins\\VRayForRhino\\VRayForRhino.rhp";
		}
		Guid guid = new Guid("e8cfe179-b60c-411a-8416-62a893334519");
		PlugIn.PlugInExists(guid, out var loaded, out var loadProtected);
		if (!loaded)
		{
			if (PlugIn.LoadPlugIn(guid))
			{
				isRenderPluginSet = true;
			}
			else
			{
				if (RhinoApp.GetPlugInObject(guid) is PlugIn plugIn)
				{
					GlobalErrorHandler.WriteErrorToEventLog("Plugin Failed to Load: " + plugIn.Assembly.FullName);
				}
				GlobalErrorHandler.WriteErrorToEventLog("Unable to Load V-Ray Plugin while trying to Render.");
				GlobalErrorHandler.WriteErrorToEventLog("Attemnpting to load V-Ray Plugin via Rhinoscript.");
				RhUtil.RhinoApp().RunScript("-_Options _Plugins _Load _FileName \"" + text + "\" Enter Enter");
				PlugIn.PlugInExists(guid, out loaded, out loadProtected);
				if (loaded)
				{
					isRenderPluginSet = true;
				}
				else
				{
					GlobalErrorHandler.WriteErrorToEventLog("Unable to Load V-Ray Plugin via Rhinoscript. Path=" + text);
				}
			}
		}
		else
		{
			isRenderPluginSet = true;
		}
		Rhino.Render.Utilities.SetDefaultRenderPlugIn(guid);
	}
}
