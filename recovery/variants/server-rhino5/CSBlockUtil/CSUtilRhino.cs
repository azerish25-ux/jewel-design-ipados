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
		OnBrep val = RhUtil.RhinoBrepCapPlanarHoles((IOnBrep)(object)input, tolerance);
		if (val != null && !((OnObject)val).IsValid())
		{
			RhUtil.RhinoRepairBrep(val, tolerance);
		}
		return val;
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
		OnBrep val = RhUtil.RhinoJoinBreps(inputBreps, tolerance);
		if (val == null)
		{
			return null;
		}
		if (!((OnObject)val).IsValid())
		{
			RhUtil.RhinoRepairBrep(val, tolerance);
		}
		return val;
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
		if (!RhUtil.RhinoJoinBreps(inputBreps, ref outputBreps, tolerance))
		{
			return false;
		}
		OnBrep[] array = outputBreps;
		foreach (OnBrep val in array)
		{
			if (!((OnObject)val).IsValid())
			{
				RhUtil.RhinoRepairBrep(val, tolerance);
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_002f: Expected O, but got Unknown
		Brep brep = Brep.MergeBreps(inputBreps, tolerance);
		if (brep == null)
		{
			return null;
		}
		if (brep.IsValid)
		{
			return brep;
		}
		OnBrep val = (OnBrep)Interop.ToOnBrep(brep);
		RhUtil.RhinoRepairBrep(val, tolerance);
		return Interop.FromOnBrep((object)val);
	}
}
