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
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		//IL_021d: Expected O, but got Unknown
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
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
		OnNurbsSurface val = new OnNurbsSurface();
		On3dPoint val2 = new On3dPoint(150.0, 150.0, z);
		On3dPoint val3 = new On3dPoint(-150.0, 150.0, z);
		On3dPoint val4 = new On3dPoint(150.0, -150.0, z);
		On3dPoint val5 = new On3dPoint(-150.0, -150.0, z);
		val.CreateRuledSurface((IOnCurve)new OnLineCurve((IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3), (IOnCurve)new OnLineCurve((IOn3dPoint)(object)val4, (IOn3dPoint)(object)val5));
		MRhinoObject val6 = (MRhinoObject)(object)RhUtil.RhinoApp().ActiveDoc().AddSurfaceObject((IOnSurface)(object)val);
		if (sceneMaterialNames != null && !sceneMaterialNames.Any((string v) => v.Contains("/GroundPlane")))
		{
			VRayInterface.LoadVismat(Path.Combine(VrayGroundPlaneMaterialsPath, "GroundPlane.vismat"));
		}
		VRayInterface.ApplyVRayMaterialToObjects("/GroundPlane", new List<MRhinoObject> { val6 });
		RhUtil.RhinoApp().RunScript("_Render", 0);
		RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)val6), true, true);
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		foreach (RhinoRenderObject item in _objects.Where((RhinoRenderObject rhinoRenderObject) => rhinoRenderObject.MRhinoObject != null))
		{
			RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)item.MRhinoObject), true, true);
		}
		MRhinoObjectIterator val = new MRhinoObjectIterator((object_state)7, (object_category)3);
		val.IncludeLights(true);
		val.IncludeGrips(false);
		for (MRhinoObject val2 = val.First(); val2 != null; val2 = val.Next())
		{
			RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)val2), true, true);
		}
		CSUtil.EnableDrawing(which: true);
	}

	private static OnBoundingBox GetModelBB()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		OnBoundingBox val = new OnBoundingBox();
		foreach (OnBoundingBox item in from rhinoRenderObject in _objects
			where rhinoRenderObject.MRhinoObject != null
			select rhinoRenderObject.MRhinoObject.BoundingBox())
		{
			val.Union((IOnBoundingBox)(object)item);
		}
		return val;
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
