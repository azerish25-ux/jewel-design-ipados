using System;
using System.Collections.Generic;
using System.Linq;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilRhino
{
	public static Brep CapPlanarHoles(Brep input, double tolerance)
	{
		return input.CapPlanarHoles(tolerance);
	}

	public static OnBrep CapPlanarHoles(OnBrep input, double tolerance)
	{
		OnBrep onBrep = RhUtil.RhinoBrepCapPlanarHoles(input, tolerance);
		if (onBrep != null && !onBrep.IsValid())
		{
			RhUtil.RhinoRepairBrep(onBrep, tolerance);
		}
		return onBrep;
	}

	public static RhinoObject FindObjectInDocument(string name)
	{
		ObjectEnumeratorSettings settings = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = true,
			SelectedObjectsFilter = true
		};
		return RhinoDoc.ActiveDoc.Objects.GetObjectList(settings).FirstOrDefault((RhinoObject rhinoObject) => rhinoObject.Attributes.Name == name);
	}

	public static Brep ReJoinBrep(Brep bIn, double tolerance)
	{
		CSUtilGeometry.FixBrepSolidOrientation(ref bIn);
		List<Brep> list = new List<Brep>();
		for (int i = 0; i < bIn.Faces.Count; i++)
		{
			Brep item = bIn.Faces[i].DuplicateFace(duplicateMeshes: false);
			list.Add(item);
		}
		return Brep.JoinBreps(list, tolerance)[0];
	}

	public static OnBrep JoinAllBreps(OnBrep[] inputBreps, double tolerance)
	{
		OnBrep onBrep = RhUtil.RhinoJoinBreps(inputBreps, tolerance);
		if (onBrep == null)
		{
			return null;
		}
		if (!onBrep.IsValid())
		{
			RhUtil.RhinoRepairBrep(onBrep, tolerance);
		}
		return onBrep;
	}

	public static Brep JoinAllBreps(IEnumerable<Brep> inputBreps, double tolerance)
	{
		Brep[] array = Brep.JoinBreps(inputBreps.Where((Brep inputBrep) => inputBrep != null && inputBrep.IsValid && !inputBrep.IsSolid).ToList(), tolerance);
		if (array == null || array.Length != 1)
		{
			return null;
		}
		return array[0];
	}

	[Obsolete]
	public static Brep JoinAllBreps_BackwardCompatibility(Brep[] inputBreps, double tolerance)
	{
		if (inputBreps.Length == 0)
		{
			return null;
		}
		Brep brep = inputBreps[0].DuplicateBrep();
		List<Brep> list = new List<Brep>();
		for (int i = 1; i < inputBreps.Length; i++)
		{
			list.Add(inputBreps[i]);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (brep.Join(list[j], tolerance, compact: true))
			{
				list.RemoveAt(j);
				j = -1;
			}
		}
		if (!brep.IsValid)
		{
			brep = JoinAllBreps(inputBreps, tolerance);
		}
		return brep;
	}

	public static bool JoinBreps(OnBrep[] inputBreps, out OnBrep[] outputBreps, double tolerance)
	{
		if (!RhUtil.RhinoJoinBreps(inputBreps, out outputBreps, tolerance))
		{
			return false;
		}
		OnBrep[] array = outputBreps;
		foreach (OnBrep onBrep in array)
		{
			if (!onBrep.IsValid())
			{
				RhUtil.RhinoRepairBrep(onBrep, tolerance);
			}
		}
		return true;
	}

	public static Brep[] JoinBreps(IEnumerable<Brep> inputBreps, double tolerance)
	{
		return Brep.JoinBreps(inputBreps, tolerance);
	}

	public static Brep MergeBreps(IEnumerable<Brep> inputBreps, double tolerance)
	{
		Brep brep = Brep.MergeBreps(inputBreps, tolerance);
		if (brep == null)
		{
			return null;
		}
		if (brep.IsValid)
		{
			return brep;
		}
		OnBrep obj = (OnBrep)Interop.ToOnBrep(brep);
		RhUtil.RhinoRepairBrep(obj, tolerance);
		return Interop.FromOnBrep(obj);
	}
}
