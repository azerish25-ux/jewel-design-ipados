using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSMaterials;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CS;

public static class CSRenderHelper
{
	private static readonly string VrayDesignMaterialsPath = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "RenderMaterials\\Design Materials");

	private static readonly string VrayGroundPlaneMaterialsPath = Path.Combine(CSApplicationHelper.Application.CSSystemPath, "RenderMaterials\\Groundplanes");

	private static ConcurrentBag<RhinoRenderObject> _objects;

	private static string _visOpt = "";

	public static void Render()
	{
		LoadVray("fullenv.visopt");
		RhUtil.RhinoApp().RunScript("_ClearUndo", 0);
		RhUtil.RhinoApp().RunScript("_ClearAllMeshes", 0);
		AddModelToRhino();
		List<string> sceneMaterialNames = VRayInterface.GetSceneMaterialNames();
		foreach (var item in (from displayRenderObject in _objects.AsParallel().AsOrdered().WithDegreeOfParallelism(CSApplicationHelper.MaxNumberOfAvailableCores)
			where displayRenderObject.MRhinoObject != null
			select displayRenderObject).GroupBy((RhinoRenderObject renderObject) => renderObject.AssignedMaterialGroup.MaterialName, (string material, IEnumerable<RhinoRenderObject> renderItems) => new
		{
			Material = material,
			RenderItems = renderItems
		}))
		{
			string materialName = item.Material;
			string path = $"{materialName}.vismat";
			if (sceneMaterialNames != null && !sceneMaterialNames.Any((string v) => v.Contains("/" + materialName)))
			{
				VRayInterface.LoadVismat(Path.Combine(VrayDesignMaterialsPath, path));
				sceneMaterialNames = VRayInterface.GetSceneMaterialNames();
			}
			List<MRhinoObject> objects = item.RenderItems.Select((RhinoRenderObject renderObject) => renderObject.MRhinoObject).ToList();
			VRayInterface.ApplyVRayMaterialToObjects("/" + materialName, objects);
		}
		double z = GetModelBB().m_min.z;
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		On3dPoint arg = new On3dPoint(150.0, 150.0, z);
		On3dPoint arg2 = new On3dPoint(-150.0, 150.0, z);
		On3dPoint arg3 = new On3dPoint(150.0, -150.0, z);
		On3dPoint arg4 = new On3dPoint(-150.0, -150.0, z);
		onNurbsSurface.CreateRuledSurface(new OnLineCurve(arg, arg2), new OnLineCurve(arg3, arg4));
		MRhinoObject mRhinoObject = RhUtil.RhinoApp().ActiveDoc().AddSurfaceObject(onNurbsSurface);
		if (sceneMaterialNames != null && !sceneMaterialNames.Any((string v) => v.Contains("/GroundPlane")))
		{
			VRayInterface.LoadVismat(Path.Combine(VrayGroundPlaneMaterialsPath, "GroundPlane.vismat"));
		}
		VRayInterface.ApplyVRayMaterialToObjects("/GroundPlane", new List<MRhinoObject> { mRhinoObject });
		RhUtil.RhinoApp().RunScript("_Render", 0);
		RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(mRhinoObject), bQuiet: true, bIgnoreModes: true);
		DeleteModelFromRhino();
		RhUtil.RhinoApp().RunScript("_ClearUndo", 0);
		RhUtil.RhinoApp().RunScript("_ClearAllMeshes", 0);
	}

	private static void AddModelToRhino()
	{
		CSUtil.EnableDrawing(which: false);
		_objects = CSUtil.AddRenderObjectsToRhino();
	}

	private static void DeleteModelFromRhino()
	{
		foreach (RhinoRenderObject item in _objects.Where((RhinoRenderObject rhinoRenderObject) => rhinoRenderObject.MRhinoObject != null))
		{
			RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(item.MRhinoObject), bQuiet: true, bIgnoreModes: true);
		}
		MRhinoObjectIterator mRhinoObjectIterator = new MRhinoObjectIterator(IRhinoObjectIterator.object_state.undeleted_objects, IRhinoObjectIterator.object_category.active_and_reference_objects);
		mRhinoObjectIterator.IncludeLights(bIncludeLights: true);
		mRhinoObjectIterator.IncludeGrips(bIncludeGrips: false);
		for (MRhinoObject mRhinoObject = mRhinoObjectIterator.First(); mRhinoObject != null; mRhinoObject = mRhinoObjectIterator.Next())
		{
			RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(mRhinoObject), bQuiet: true, bIgnoreModes: true);
		}
		CSUtil.EnableDrawing(which: true);
	}

	private static OnBoundingBox GetModelBB()
	{
		OnBoundingBox onBoundingBox = new OnBoundingBox();
		foreach (OnBoundingBox item in from rhinoRenderObject in _objects
			where rhinoRenderObject.MRhinoObject != null
			select rhinoRenderObject.MRhinoObject.BoundingBox())
		{
			onBoundingBox.Union(item);
		}
		return onBoundingBox;
	}

	private static void LoadVray(string visThisOpt)
	{
		if (visThisOpt != _visOpt)
		{
			VRayInterface.LoadVisopt(Path.Combine(VrayDesignMaterialsPath, visThisOpt));
			_visOpt = visThisOpt;
		}
	}
}
