using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CS.Utilities.Helpers;
using Rhino;
using Rhino.Collections;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS.Utilities.Travis;

public sealed class MeshRepair
{
	private const string Lkey = "7XSjdv2dJaCXAW/2CsH6SbyO7pKb358QO1GWaDAGnbiXQVHivwgoZwFROVvDKgefFkhF/bjFTPgRXFm1VgTtfkQVT9dSJwuVpp2UgjFP3enBvpg5V0klW00OQ4bG3RpZ5EGQqiEXgfnf2pm5c5QxvdUKgY9QKoZZ91xOIXXVv+VBBkysTEExcJyd9wvaWovP+MgUIW2dwLlfZsk2kpxgTskb7ZeESlIQkQEG0+zEBvlYhtyiElG2romesw4RT44MM8BPeOURZ7SeQJzj5UVkIVWBxJrW8BtE10AFzPHdzSQeD1BhmhDpbVxCb59LCp5bbozLHD1T43hK0Vn1JVDr253RBMfdc9KLIEzAIa1RCDnEnEH7jcSl48RSeMqVmJ6WYMQWZBbW8qwUkgi4ZdD/HcxcvpMHvBhQVUEM+i0ay3QerEkojZR7WE1Wm1pCKBfdsgRDWv2OvC1KmJw/RQT1gFCQqxdRZk4c5dlRpOjR1D1XrhAzpFG8ExTGeEaRoocAE5Eadu1Vbb0WUoV/74xiKc0bfo9VdweXv02JlK2WEHDQTBUvjpj+bhDW9Fvb3ccUYYRSJ+BbOGCOdcQvp1hqpUmGgotE69Mbv1Fbgmzbn2CUMs22XVDwYAHZ30IZv4fH8RHJrHEQ+/HbMsDm0Zy8tBlCysqaNJ9bQhUG7mjTPy1V48wqkBx9NMATZtdE4spdDQQPVnnazKWVPZFrpZyn9dGFNA+FOIJEVJVCT+GAqTEdYl0tvxTpaMjLGEtBQE/BoMBFUWUeQbUYIV15tZE6bxEOC8pCUkMFNYB/oJGth6ouSAg128GvXB2ImXvzCN0/4rwT/egjXOBb0tE+vN7PFlFuUHwk7T1Z5a32yuWvzaabOqSF9XE5mzQHrPxkIdT3Szx/b82/pNWNo/18toJM/LxDkqdp5LcUeWmGkKf7VkyXf1OiWgKMl3ELSrt+K1ECwJq6g5Srf+SnFyZ2rpV5ykJBwmbIg+KdfLY//FROQoLh7KVT/Jy5fN4eUO7wxYYaSRFgF2UXjmvn1+ZrLCdtyAGASM9WDHrOShCbzrhUluvqkldEXDngNeHpnwTT0yffApeqx9RaSsbxiChJKbnoKOzCSEiy519bmMxodq5ZG1yRBD/dJbEjdXnUSeck7uIi7Tya9MX8K7op/HTqpdXikAAA";

	private static string _netfabbLastAction = string.Empty;

	private static int _netfabbLastProgress;

	public static bool CheckMeshValidity(Mesh mesh)
	{
		try
		{
			if (!((CommonObject)mesh).IsValid)
			{
				return false;
			}
			if (!mesh.IsClosed)
			{
				return false;
			}
			bool flag = default(bool);
			bool flag2 = default(bool);
			if (!mesh.IsManifold(true, ref flag, ref flag2))
			{
				return false;
			}
			if (!flag)
			{
				return false;
			}
			if (mesh.DisjointMeshCount > 1)
			{
				return false;
			}
			if (mesh.GetNakedEdges() != null)
			{
				return false;
			}
			if (mesh.Faces.CullDegenerateFaces() > 0)
			{
				return false;
			}
			if (mesh.Vertices.CullUnused() > 0)
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			RhinoApp.Write(ex.ToString());
			return false;
		}
	}

	public static Mesh CreateMesh(GeometryBase geometryBase, bool doCapPlanarHoles, MeshingParameters meshingParameters, int milGrainGridCount, bool optimizeMilgrain)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Mesh val = new Mesh();
			if ((int)geometryBase.ObjectType == 32)
			{
				Mesh val2 = (Mesh)(object)((geometryBase is Mesh) ? geometryBase : null);
				if (val2 != null && ((CommonObject)val2).IsValid)
				{
					val.Append((Mesh)geometryBase);
					val.Faces.ConvertQuadsToTriangles();
					if (val.SolidOrientation() == -1)
					{
						val.Flip(true, true, true);
					}
					return val;
				}
			}
			Brep val3 = null;
			if ((int)geometryBase.ObjectType == 1073741824)
			{
				Extrusion val4 = (Extrusion)(object)((geometryBase is Extrusion) ? geometryBase : null);
				if (val4 != null)
				{
					Brep val5 = val4.ToBrep(true);
					if (val5 != null && ((CommonObject)val5).IsValid)
					{
						val3 = val5;
					}
				}
			}
			if ((int)geometryBase.ObjectType == 8 && geometryBase is Surface)
			{
				Brep val6 = Brep.TryConvertBrep(geometryBase);
				if (((CommonObject)val6).IsValid)
				{
					val3 = val6;
				}
			}
			if ((int)geometryBase.ObjectType == 16)
			{
				Brep val7 = (Brep)(object)((geometryBase is Brep) ? geometryBase : null);
				if (val7 != null)
				{
					val3 = val7;
				}
			}
			if (val3 == null)
			{
				return null;
			}
			if (optimizeMilgrain && 1 == val3.Faces.Count && val3.Trims.Count == 0)
			{
				Sphere val8 = default(Sphere);
				((Surface)val3.Faces[0]).TryGetSphere(ref val8);
				if (((Sphere)(ref val8)).Radius != 0.0 && ((Sphere)(ref val8)).Radius <= 1.5)
				{
					Mesh obj = Mesh.CreateFromSphere(val8, milGrainGridCount, milGrainGridCount);
					obj.Faces.ConvertQuadsToTriangles();
					return obj;
				}
			}
			Mesh[] array2;
			if (doCapPlanarHoles)
			{
				Brep val9 = val3.CapPlanarHoles(0.001);
				if (val9 != null)
				{
					Mesh[] array = Mesh.CreateFromBrep(val9, meshingParameters);
					((CommonObject)val9).Dispose();
					array2 = array;
					foreach (Mesh val10 in array2)
					{
						val.Append(val10);
					}
					val.Faces.ConvertQuadsToTriangles();
					return val;
				}
			}
			Mesh[] array3 = Mesh.CreateFromBrep(val3, meshingParameters);
			if (array3 == null)
			{
				return null;
			}
			array2 = array3;
			foreach (Mesh val11 in array2)
			{
				val.Append(val11);
			}
			if (val.SolidOrientation() == -1)
			{
				val.Flip(true, true, true);
			}
			return val;
		}
		catch (Exception ex)
		{
			RhinoApp.WriteLine(ex.Message);
			return null;
		}
	}

	public static Mesh CreateMesh(RhinoObject rhinoObject, bool doCapPlanarHoles, MeshingParameters meshingParameters, int milGrainGridCount, bool optimizeMilgrain)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			return CreateMesh(rhinoObject.Geometry, doCapPlanarHoles, meshingParameters, milGrainGridCount, optimizeMilgrain);
		}
		catch (Exception ex)
		{
			RhinoApp.WriteLine(ex.Message);
			return new Mesh();
		}
	}

	public static void CreateSTLFromRhinoObjects(bool repairMesh, bool saveToDisk, string stlFileName)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		List<RhinoObject> normalObjects = GetNormalObjects();
		if (normalObjects.Count < 1)
		{
			return;
		}
		if (!repairMesh)
		{
			if (saveToDisk)
			{
				SaveStlBinary(GetMeshFromObjects(normalObjects, 0.002, 0, optimizeMilgrain: true), stlFileName, verbose: true);
			}
			return;
		}
		MeshingParameters mp = new MeshingParameters
		{
			ComputeCurvature = false,
			GridAmplification = 0.0,
			GridAngle = 0.0,
			GridAspectRatio = 0.0,
			GridMaxCount = 0,
			GridMinCount = 0,
			JaggedSeams = false,
			MaximumEdgeLength = 0.0,
			MinimumEdgeLength = 0.0,
			MinimumTolerance = 0.0,
			RefineAngle = 0.0,
			RefineGrid = true,
			RelativeTolerance = 0.0,
			SimplePlanes = true,
			Tolerance = 0.002
		};
		Mesh mesh = FilterNoiseShells(RepairMesh(normalObjects.Select((RhinoObject ro) => ro.Geometry).ToList(), extendedRepair: true, prefixshells: false, mp, 10, optimizeMilgrain: true));
		if (CheckMeshValidity(mesh))
		{
			RhinoApp.WriteLine("Mesh is repaired.");
		}
		else
		{
			RhinoApp.WriteLine("Mesh was not fixable.");
		}
		if (saveToDisk)
		{
			SaveStlBinary(mesh, stlFileName, verbose: true);
		}
	}

	public static Mesh FilterNoiseShells(Mesh mesh)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (mesh.DisjointMeshCount == 1)
		{
			return mesh;
		}
		Mesh val = new Mesh();
		Mesh[] array = mesh.SplitDisjointPieces();
		foreach (Mesh val2 in array)
		{
			double volume = VolumeMassProperties.Compute(val2).Volume;
			BoundingBox boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
			Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			if (!(((Vector3d)(ref diagonal)).Length < 0.2) && !(volume < 10.0))
			{
				val.Append(val2);
			}
		}
		return val;
	}

	public static Mesh GetMeshFromObjects(List<GeometryBase> lstGeometryBases, double tolerance, int gridAspectRatio, bool optimizeMilgrain)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		try
		{
			MeshingParameters meshingParameters = new MeshingParameters
			{
				ComputeCurvature = false,
				GridAmplification = 0.0,
				GridAngle = 0.0,
				GridAspectRatio = gridAspectRatio,
				GridMaxCount = 0,
				GridMinCount = 0,
				JaggedSeams = false,
				MaximumEdgeLength = 0.0,
				MinimumEdgeLength = 0.001,
				MinimumTolerance = 0.0,
				RefineAngle = 0.0,
				RefineGrid = true,
				RelativeTolerance = 0.0,
				SimplePlanes = false,
				Tolerance = tolerance
			};
			Mesh val = new Mesh();
			int num = 1;
			int count = lstGeometryBases.Count;
			foreach (GeometryBase lstGeometryBasis in lstGeometryBases)
			{
				_ = (num * 200 + count) / (count * 2);
				Mesh val2 = CreateMesh(lstGeometryBasis, doCapPlanarHoles: true, meshingParameters, 10, optimizeMilgrain);
				val.Append(val2);
			}
			val.Compact();
			return val;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh GetMeshFromObjects(List<RhinoObject> lstRhinoObjects, double tolerance, int gridAspectRatio, bool optimizeMilgrain)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		try
		{
			MeshingParameters meshingParameters = new MeshingParameters
			{
				ComputeCurvature = false,
				GridAmplification = 0.0,
				GridAngle = 0.0,
				GridAspectRatio = gridAspectRatio,
				GridMaxCount = 0,
				GridMinCount = 0,
				JaggedSeams = false,
				MaximumEdgeLength = 0.0,
				MinimumEdgeLength = 0.0,
				MinimumTolerance = 0.0,
				RefineAngle = 0.0,
				RefineGrid = true,
				RelativeTolerance = 0.0,
				SimplePlanes = false,
				Tolerance = tolerance
			};
			Mesh val = new Mesh();
			int num = 1;
			int count = lstRhinoObjects.Count;
			foreach (RhinoObject lstRhinoObject in lstRhinoObjects)
			{
				RhinoApp.CommandPrompt = string.Format(arg2: (num * 200 + count) / (count * 2), format: "Calculating Mesh Shell {0} of {1} : {2}%", arg0: num, arg1: count);
				Mesh val2 = CreateMesh(lstRhinoObject, doCapPlanarHoles: false, meshingParameters, 10, optimizeMilgrain);
				val.Append(val2);
			}
			val.Compact();
			return val;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<RhinoObject> GetNormalObjects()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ObjectEnumeratorSettings val = new ObjectEnumeratorSettings
		{
			ObjectTypeFilter = (ObjectType)2097208,
			DeletedObjects = false,
			NormalObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			IncludePhantoms = false,
			LockedObjects = false,
			HiddenObjects = false
		};
		return RhinoDoc.ActiveDoc.Objects.GetObjectList(val).ToList();
	}

	public static Mesh NetfabbRepairMesh(Mesh inputMesh, bool extendedRepair)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			BoundingBox boundingBox = ((GeometryBase)inputMesh).GetBoundingBox(true);
			((BoundingBox)(ref boundingBox)).MakeValid();
			Point3d center = ((BoundingBox)(ref boundingBox)).Center;
			((GeometryBase)inputMesh).Transform(Transform.Scale(center, 3.0));
			RhinoApp.WriteLine("Repairing and Unifying Mesh...Please Wait");
			if (!NetfabbHelper.NetfabbBuildMesh(inputMesh, extendedRepair, out var meshRepaired))
			{
				RhinoApp.WriteLine("Mesh Creation Failed.");
				RhinoApp.WriteLine(NetfabbHelper.LastNetFabError.ToString());
				RhinoApp.WriteLine("Mesh Repair failed - {0}", (object)NetfabbHelper.LastNetFabError);
				return null;
			}
			if (meshRepaired == null)
			{
				RhinoApp.WriteLine("Mesh Creation Failed.");
				RhinoApp.WriteLine(NetfabbHelper.LastNetFabError.ToString());
				RhinoApp.WriteLine("Mesh Repair returned null output - {0}", (object)NetfabbHelper.LastNetFabError);
				return null;
			}
			if (!((CommonObject)meshRepaired).IsValid)
			{
				RhinoApp.WriteLine("Resulting Mesh Is Invalid");
				RhinoApp.WriteLine(NetfabbHelper.LastNetFabError.ToString());
				RhinoApp.WriteLine("Mesh Repair returned invalid output - {0}", (object)NetfabbHelper.LastNetFabError);
				return null;
			}
			BoundingBox boundingBox2 = ((GeometryBase)meshRepaired).GetBoundingBox(true);
			((BoundingBox)(ref boundingBox2)).MakeValid();
			_ = ((BoundingBox)(ref boundingBox2)).Center;
			((GeometryBase)meshRepaired).Transform(Transform.Scale(center, 1.0 / 3.0));
			meshRepaired.Unweld(CSCoreMath.ToRadians(22.5), true);
			BoundingBox boundingBox3 = ((GeometryBase)meshRepaired).GetBoundingBox(true);
			Vector3d diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
			double length = ((Vector3d)(ref diagonal)).Length;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			RhinoApp.WriteLine((Math.Abs(length - ((Vector3d)(ref diagonal)).Length) < 0.001) ? "Bounding Boxes Match" : "Bounding Boxes DO NOT MATCH");
			return meshRepaired;
		}
		catch (Exception ex)
		{
			RhinoApp.WriteLine(ex.ToString());
			return null;
		}
	}

	public static bool RandomScaleRhinoObject(RhinoObject ro)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			double num = new Random().NextDouble() * Math.Abs(0.004) + 0.001;
			BoundingBox boundingBox = ro.Geometry.GetBoundingBox(true);
			((BoundingBox)(ref boundingBox)).MakeValid();
			return ro.Geometry.Transform(Transform.Scale(((BoundingBox)(ref boundingBox)).Center, 1.0 + num));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static bool RandomScaleRhinoObject(GeometryBase geo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			double num = new Random().NextDouble() * Math.Abs(0.004) + 0.001;
			BoundingBox boundingBox = geo.GetBoundingBox(true);
			((BoundingBox)(ref boundingBox)).MakeValid();
			return geo.Transform(Transform.Scale(((BoundingBox)(ref boundingBox)).Center, 1.0 + num));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh ReadBinarySTL(string filename)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			using BinaryReader binaryReader = new BinaryReader(File.Open(filename, FileMode.Open));
			binaryReader.BaseStream.Position = 0L;
			byte[] array = binaryReader.ReadBytes(80);
			_ = string.Empty;
			if (array.Length != 80)
			{
				return null;
			}
			Encoding.UTF8.GetString(array).TrimEnd();
			binaryReader.BaseStream.Position = 80L;
			binaryReader.ReadUInt32();
			int num = (int)binaryReader.BaseStream.Length;
			Mesh val = new Mesh();
			Vector3d val2 = default(Vector3d);
			for (int i = 84; i < num; i += 50)
			{
				float num2 = binaryReader.ReadSingle();
				float num3 = binaryReader.ReadSingle();
				float num4 = binaryReader.ReadSingle();
				float num5 = binaryReader.ReadSingle();
				float num6 = binaryReader.ReadSingle();
				float num7 = binaryReader.ReadSingle();
				float num8 = binaryReader.ReadSingle();
				float num9 = binaryReader.ReadSingle();
				float num10 = binaryReader.ReadSingle();
				float num11 = binaryReader.ReadSingle();
				float num12 = binaryReader.ReadSingle();
				float num13 = binaryReader.ReadSingle();
				binaryReader.ReadUInt16();
				int count = val.Vertices.Count;
				val.Vertices.SetVertex(count, num5, num6, num7);
				val.Vertices.SetVertex(count + 1, num8, num9, num10);
				val.Vertices.SetVertex(count + 2, num11, num12, num13);
				int count2 = val.Faces.Count;
				val.Faces.SetFace(count2, new MeshFace(count, count + 1, count + 2));
				((Vector3d)(ref val2))._002Ector((double)num2, (double)num3, (double)num4);
				val.FaceNormals.AddFaceNormal(val2);
			}
			binaryReader.Close();
			binaryReader.Dispose();
			val.Normals.ComputeNormals();
			val.Compact();
			val.Unweld(0.0, true);
			return val;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
			return null;
		}
	}

	public static Mesh RepairMesh(List<GeometryBase> lstGeometry, bool extendedRepair, bool prefixshells, MeshingParameters mp, int milgrainGridCount, bool optimizeMilgrain)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		Mesh val = new Mesh();
		List<Brep> list = new List<Brep>();
		List<GeometryBase> list2 = new List<GeometryBase>();
		foreach (GeometryBase item2 in lstGeometry)
		{
			if ((int)item2.ObjectType == 16)
			{
				Brep val2 = (Brep)item2;
				if (!val2.IsSolid)
				{
					Brep val3 = val2.CapPlanarHoles(0.001);
					if (val3 != null)
					{
						val2 = val3;
					}
				}
				list.Add(val2);
			}
			else
			{
				Mesh item = CreateMesh(item2, doCapPlanarHoles: true, mp, milgrainGridCount, optimizeMilgrain);
				list2.Add((GeometryBase)(object)item);
			}
		}
		if (list.Count > 0)
		{
			Brep[] collection = Brep.JoinBreps((IEnumerable<Brep>)list, 0.001);
			list2.AddRange((IEnumerable<GeometryBase>)(object)collection);
		}
		foreach (GeometryBase item3 in list2)
		{
			Mesh val4 = CreateMesh(item3, doCapPlanarHoles: true, mp, milgrainGridCount, optimizeMilgrain);
			if (val4 == null)
			{
				Guid guid = RhinoDoc.ActiveDoc.Objects.Add(item3);
				RhinoDoc.ActiveDoc.Objects.UnselectAll();
				RhinoDoc.ActiveDoc.Objects.Select(guid);
				RhinoApp.WriteLine("Could not create Mesh Object");
			}
			val4.Faces.ConvertQuadsToTriangles();
			if (val4.SolidOrientation() == -1)
			{
				val4.Flip(true, true, true);
			}
			if (prefixshells)
			{
				Mesh val5 = val4.DuplicateMesh();
				if (!val4.IsClosed)
				{
					RhinoApp.CommandPrompt = "Prefixing Single Open Shell";
					val4 = NetfabbRepairMesh(val4, extendedRepair: true);
				}
				if (val4 == null)
				{
					val4 = val5.DuplicateMesh();
				}
			}
			val.Append(val4);
		}
		return FilterNoiseShells(NetfabbRepairMesh(val, extendedRepair));
	}

	public static bool SaveStlBinary(Mesh mesh, string filename, bool verbose)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (mesh == null)
			{
				return false;
			}
			if (filename == null)
			{
				return false;
			}
			mesh.Faces.ConvertQuadsToTriangles();
			if (!mesh.Normals.ComputeNormals())
			{
				return false;
			}
			string text = "Gemvision STL Export";
			byte[] array = new byte[80];
			int i;
			for (i = 0; i < 80; i++)
			{
				array[i] = 32;
			}
			for (i = 0; i < text.Length && i < 80; i++)
			{
				array[i] = (byte)text[i];
			}
			if (i < 77)
			{
				array[i] = 13;
				array[i + 1] = 10;
			}
			array[78] = 13;
			array[79] = 10;
			using (BinaryWriter binaryWriter = new BinaryWriter(new FileInfo(filename).OpenWrite()))
			{
				binaryWriter.Write(array);
				int num = ((IEnumerable<MeshFace>)mesh.Faces).Count();
				binaryWriter.Write(num);
				if (verbose)
				{
					RhinoApp.CommandPrompt = $"Saving STL File {filename}";
					RhinoApp.WriteLine("Saving STL File: Face Count {0}", (object)num);
				}
				for (i = 0; i < num; i++)
				{
					MeshFace val = mesh.Faces[i];
					Point3f val2 = mesh.Vertices[((MeshFace)(ref val)).A];
					Point3f val3 = mesh.Vertices[((MeshFace)(ref val)).B];
					Point3f val4 = mesh.Vertices[((MeshFace)(ref val)).C];
					Vector3f val5 = mesh.FaceNormals[i];
					if (!double.IsNaN(((Point3f)(ref val2)).X) && !double.IsNaN(((Point3f)(ref val2)).Y) && !double.IsNaN(((Point3f)(ref val2)).Z) && !double.IsNaN(((Point3f)(ref val3)).X) && !double.IsNaN(((Point3f)(ref val3)).Y) && !double.IsNaN(((Point3f)(ref val3)).Z) && !double.IsNaN(((Point3f)(ref val4)).X) && !double.IsNaN(((Point3f)(ref val4)).Y) && !double.IsNaN(((Point3f)(ref val4)).Z))
					{
						binaryWriter.Write(((Vector3f)(ref val5)).X);
						binaryWriter.Write(((Vector3f)(ref val5)).Y);
						binaryWriter.Write(((Vector3f)(ref val5)).Z);
						binaryWriter.Write(((Point3f)(ref val2)).X);
						binaryWriter.Write(((Point3f)(ref val2)).Y);
						binaryWriter.Write(((Point3f)(ref val2)).Z);
						binaryWriter.Write(((Point3f)(ref val3)).X);
						binaryWriter.Write(((Point3f)(ref val3)).Y);
						binaryWriter.Write(((Point3f)(ref val3)).Z);
						binaryWriter.Write(((Point3f)(ref val4)).X);
						binaryWriter.Write(((Point3f)(ref val4)).Y);
						binaryWriter.Write(((Point3f)(ref val4)).Z);
						ushort value = 0;
						binaryWriter.Write(value);
					}
				}
				binaryWriter.Close();
				if (verbose)
				{
					RhinoApp.CommandPrompt = " ";
					RhinoApp.WriteLine($"STL File Written Successfully: {filename}");
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			RhinoApp.WriteLine(ex.Message);
			return false;
		}
	}

	public static Mesh SliceMesh(List<RhinoObject> lstRhinoObjects, bool randomScale, bool extendedRepairInput, double sliceThickness)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<RhinoObject> list = new List<RhinoObject>();
			Mesh val = new Mesh();
			if (randomScale)
			{
				foreach (RhinoObject lstRhinoObject in lstRhinoObjects)
				{
					RandomScaleRhinoObject(lstRhinoObject);
					list.Add(lstRhinoObject);
				}
				val = GetMeshFromObjects(list, 0.002, 1, optimizeMilgrain: false);
			}
			else
			{
				val = GetMeshFromObjects(lstRhinoObjects, 0.002, 1, optimizeMilgrain: false);
			}
			val = NetfabbRepairMesh(val, extendedRepairInput);
			Mesh[] array = val.SplitDisjointPieces();
			int num = array.Count();
			Mesh val2 = new Mesh();
			int num2 = 1;
			Mesh[] array2 = array;
			Plane val5 = default(Plane);
			foreach (Mesh val3 in array2)
			{
				int num3 = (num2 * 200 + num) / (num * 2);
				RhinoApp.CommandPrompt = $"Slicing Shell {num2} of {num} : {num3}%";
				BoundingBox boundingBox = ((GeometryBase)val3).GetBoundingBox(true);
				((BoundingBox)(ref boundingBox)).MakeValid();
				Point3d[] corners = ((BoundingBox)(ref boundingBox)).GetCorners();
				Point3d[] array3 = ((Curve)new LineCurve(corners[0], corners[4])).DivideEquidistant(sliceThickness);
				foreach (Point3d val4 in array3)
				{
					((Plane)(ref val5))._002Ector(val4, Vector3d.ZAxis);
					CurveList val6 = new CurveList();
					Curve[] array4 = Mesh.CreateContourCurves(val3, val5);
					if (!array4.Any())
					{
						continue;
					}
					Curve[] array5 = array4;
					foreach (Curve val7 in array5)
					{
						NurbsCurve val8 = val7.Rebuild(50, 3, false);
						if (((CommonObject)val8).IsValid)
						{
							if (val7.IsClosed)
							{
								((RhinoList<Curve>)(object)val6).Add((Curve)(object)val8);
							}
						}
						else if (val7.IsClosed)
						{
							((RhinoList<Curve>)(object)val6).Add(val7);
						}
					}
					if (!((IEnumerable<Curve>)val6).Any())
					{
						continue;
					}
					Brep[] array6 = Brep.CreatePlanarBreps(val6);
					if (array6 == null)
					{
						continue;
					}
					Brep[] array7 = array6;
					foreach (Brep obj in array7)
					{
						obj.Flip();
						Mesh[] array8 = Mesh.CreateFromBrep(obj, MeshingParameters.Coarse);
						if (array8 == null)
						{
							continue;
						}
						Mesh[] array9 = array8;
						foreach (Mesh val9 in array9)
						{
							if (((CommonObject)val9).IsValid)
							{
								Mesh val10 = val9.Offset(sliceThickness, true);
								val2.Append(val10);
							}
						}
					}
				}
				num2++;
			}
			return val2;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh SliceMesh(IEnumerable<GeometryBase> lstGeometryBases, bool extendedRepairInput, double sliceThickness)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Mesh val = new Mesh();
			foreach (GeometryBase lstGeometryBasis in lstGeometryBases)
			{
				Mesh val2 = CreateMesh(lstGeometryBasis, doCapPlanarHoles: true, MeshingParameters.Coarse, 10, optimizeMilgrain: true);
				if (((CommonObject)val2).IsValid)
				{
					val.Append(val2);
				}
			}
			val = NetfabbRepairMesh(val, extendedRepairInput);
			Mesh[] array = val.SplitDisjointPieces();
			int num = array.Count();
			Mesh val3 = new Mesh();
			int num2 = 1;
			Mesh[] array2 = array;
			Plane val6 = default(Plane);
			foreach (Mesh val4 in array2)
			{
				int num3 = (num2 * 200 + num) / (num * 2);
				RhinoApp.CommandPrompt = $"Slicing Shell {num2} of {num} : {num3}%";
				BoundingBox boundingBox = ((GeometryBase)val4).GetBoundingBox(true);
				((BoundingBox)(ref boundingBox)).MakeValid();
				Point3d[] corners = ((BoundingBox)(ref boundingBox)).GetCorners();
				Point3d[] array3 = ((Curve)new LineCurve(corners[0], corners[4])).DivideEquidistant(sliceThickness);
				foreach (Point3d val5 in array3)
				{
					((Plane)(ref val6))._002Ector(val5, Vector3d.ZAxis);
					CurveList val7 = new CurveList();
					Curve[] array4 = Mesh.CreateContourCurves(val4, val6);
					if (!array4.Any())
					{
						continue;
					}
					Curve[] array5 = array4;
					foreach (Curve val8 in array5)
					{
						NurbsCurve val9 = val8.Rebuild(50, 3, false);
						if (((CommonObject)val9).IsValid)
						{
							if (val8.IsClosed)
							{
								((RhinoList<Curve>)(object)val7).Add((Curve)(object)val9);
							}
						}
						else if (val8.IsClosed)
						{
							((RhinoList<Curve>)(object)val7).Add(val8);
						}
					}
					if (!((IEnumerable<Curve>)val7).Any())
					{
						continue;
					}
					Brep[] array6 = Brep.CreatePlanarBreps(val7);
					if (array6 == null)
					{
						continue;
					}
					Brep[] array7 = array6;
					foreach (Brep obj in array7)
					{
						obj.Flip();
						Mesh[] array8 = Mesh.CreateFromBrep(obj, MeshingParameters.Coarse);
						if (array8 == null)
						{
							continue;
						}
						Mesh[] array9 = array8;
						foreach (Mesh val10 in array9)
						{
							if (((CommonObject)val10).IsValid)
							{
								Mesh val11 = val10.Offset(sliceThickness, true);
								val3.Append(val11);
							}
						}
					}
				}
				num2++;
			}
			return val3;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh SliceMesh(Mesh inputMesh, double sliceThickness)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		Mesh[] array = inputMesh.SplitDisjointPieces();
		int num = array.Count();
		Mesh val = new Mesh();
		int num2 = 1;
		Mesh[] array2 = array;
		Plane val4 = default(Plane);
		foreach (Mesh val2 in array2)
		{
			int num3 = (num2 * 200 + num) / (num * 2);
			RhinoApp.CommandPrompt = $"Slicing Shell {num2} of {num} : {num3}%";
			BoundingBox boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
			((BoundingBox)(ref boundingBox)).MakeValid();
			Point3d[] corners = ((BoundingBox)(ref boundingBox)).GetCorners();
			Point3d[] array3 = ((Curve)new LineCurve(corners[0], corners[4])).DivideEquidistant(sliceThickness);
			foreach (Point3d val3 in array3)
			{
				((Plane)(ref val4))._002Ector(val3, Vector3d.ZAxis);
				CurveList val5 = new CurveList();
				Curve[] array4 = Mesh.CreateContourCurves(val2, val4);
				if (!array4.Any())
				{
					continue;
				}
				Curve[] array5 = array4;
				foreach (Curve val6 in array5)
				{
					NurbsCurve val7 = val6.Rebuild(50, 3, false);
					if (((CommonObject)val7).IsValid)
					{
						if (val6.IsClosed)
						{
							((RhinoList<Curve>)(object)val5).Add((Curve)(object)val7);
						}
						else if (val6.IsClosed)
						{
							((RhinoList<Curve>)(object)val5).Add(val6);
						}
					}
				}
				if (!((IEnumerable<Curve>)val5).Any())
				{
					continue;
				}
				Brep[] array6 = Brep.CreatePlanarBreps(val5);
				if (array6 == null)
				{
					continue;
				}
				Brep[] array7 = array6;
				foreach (Brep obj in array7)
				{
					obj.Flip();
					Mesh[] array8 = Mesh.CreateFromBrep(obj, MeshingParameters.Coarse);
					if (array8 == null)
					{
						continue;
					}
					Mesh[] array9 = array8;
					foreach (Mesh val8 in array9)
					{
						if (((CommonObject)val8).IsValid)
						{
							Mesh val9 = val8.Offset(sliceThickness, true);
							val.Append(val9);
						}
					}
				}
			}
			num2++;
		}
		return val;
	}

	private static void NetfabbHelper_NetfabbProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		if (e.UserState != null)
		{
			_netfabbLastAction = (string)e.UserState;
		}
		if (_netfabbLastProgress != e.ProgressPercentage)
		{
			_netfabbLastProgress = e.ProgressPercentage;
			RhinoApp.CommandPrompt = $"{_netfabbLastAction} Progress - {e.ProgressPercentage}%";
			Application.DoEvents();
		}
	}

	public static Mesh ReadEncryptedBinaryStl(string filename)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			byte[] array = Decrypt(File.ReadAllBytes(filename), "7XSjdv2dJaCXAW/2CsH6SbyO7pKb358QO1GWaDAGnbiXQVHivwgoZwFROVvDKgefFkhF/bjFTPgRXFm1VgTtfkQVT9dSJwuVpp2UgjFP3enBvpg5V0klW00OQ4bG3RpZ5EGQqiEXgfnf2pm5c5QxvdUKgY9QKoZZ91xOIXXVv+VBBkysTEExcJyd9wvaWovP+MgUIW2dwLlfZsk2kpxgTskb7ZeESlIQkQEG0+zEBvlYhtyiElG2romesw4RT44MM8BPeOURZ7SeQJzj5UVkIVWBxJrW8BtE10AFzPHdzSQeD1BhmhDpbVxCb59LCp5bbozLHD1T43hK0Vn1JVDr253RBMfdc9KLIEzAIa1RCDnEnEH7jcSl48RSeMqVmJ6WYMQWZBbW8qwUkgi4ZdD/HcxcvpMHvBhQVUEM+i0ay3QerEkojZR7WE1Wm1pCKBfdsgRDWv2OvC1KmJw/RQT1gFCQqxdRZk4c5dlRpOjR1D1XrhAzpFG8ExTGeEaRoocAE5Eadu1Vbb0WUoV/74xiKc0bfo9VdweXv02JlK2WEHDQTBUvjpj+bhDW9Fvb3ccUYYRSJ+BbOGCOdcQvp1hqpUmGgotE69Mbv1Fbgmzbn2CUMs22XVDwYAHZ30IZv4fH8RHJrHEQ+/HbMsDm0Zy8tBlCysqaNJ9bQhUG7mjTPy1V48wqkBx9NMATZtdE4spdDQQPVnnazKWVPZFrpZyn9dGFNA+FOIJEVJVCT+GAqTEdYl0tvxTpaMjLGEtBQE/BoMBFUWUeQbUYIV15tZE6bxEOC8pCUkMFNYB/oJGth6ouSAg128GvXB2ImXvzCN0/4rwT/egjXOBb0tE+vN7PFlFuUHwk7T1Z5a32yuWvzaabOqSF9XE5mzQHrPxkIdT3Szx/b82/pNWNo/18toJM/LxDkqdp5LcUeWmGkKf7VkyXf1OiWgKMl3ELSrt+K1ECwJq6g5Srf+SnFyZ2rpV5ykJBwmbIg+KdfLY//FROQoLh7KVT/Jy5fN4eUO7wxYYaSRFgF2UXjmvn1+ZrLCdtyAGASM9WDHrOShCbzrhUluvqkldEXDngNeHpnwTT0yffApeqx9RaSsbxiChJKbnoKOzCSEiy519bmMxodq5ZG1yRBD/dJbEjdXnUSeck7uIi7Tya9MX8K7op/HTqpdXikAAA");
			if (array == null)
			{
				return null;
			}
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream());
			binaryWriter.Write(array);
			using BinaryReader binaryReader = new BinaryReader(binaryWriter.BaseStream);
			binaryReader.BaseStream.Position = 0L;
			byte[] array2 = binaryReader.ReadBytes(80);
			_ = string.Empty;
			if (array2.Length != 80)
			{
				return null;
			}
			RhinoApp.WriteLine(Encoding.UTF8.GetString(array2).TrimEnd());
			binaryReader.BaseStream.Position = 80L;
			binaryReader.ReadUInt32();
			int num = (int)binaryReader.BaseStream.Length;
			Mesh val = new Mesh();
			Vector3d val2 = default(Vector3d);
			for (int i = 84; i < num; i += 50)
			{
				float num2 = binaryReader.ReadSingle();
				float num3 = binaryReader.ReadSingle();
				float num4 = binaryReader.ReadSingle();
				float num5 = binaryReader.ReadSingle();
				float num6 = binaryReader.ReadSingle();
				float num7 = binaryReader.ReadSingle();
				float num8 = binaryReader.ReadSingle();
				float num9 = binaryReader.ReadSingle();
				float num10 = binaryReader.ReadSingle();
				float num11 = binaryReader.ReadSingle();
				float num12 = binaryReader.ReadSingle();
				float num13 = binaryReader.ReadSingle();
				binaryReader.ReadUInt16();
				int count = val.Vertices.Count;
				val.Vertices.SetVertex(count, num5, num6, num7);
				val.Vertices.SetVertex(count + 1, num8, num9, num10);
				val.Vertices.SetVertex(count + 2, num11, num12, num13);
				int count2 = val.Faces.Count;
				val.Faces.SetFace(count2, new MeshFace(count, count + 1, count + 2));
				((Vector3d)(ref val2))._002Ector((double)num2, (double)num3, (double)num4);
				val.FaceNormals.AddFaceNormal(val2);
			}
			binaryWriter.Flush();
			binaryWriter.Close();
			val.Normals.ComputeNormals();
			val.Compact();
			val.Unweld(0.0, true);
			return val;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
			return null;
		}
	}

	public static bool WriteEncryptedBinaryStl(Mesh mesh, string filename, string headerText, bool verbose)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (mesh == null)
			{
				return false;
			}
			if (filename == null)
			{
				return false;
			}
			mesh.Faces.ConvertQuadsToTriangles();
			if (!mesh.Normals.ComputeNormals())
			{
				return false;
			}
			byte[] array = new byte[80];
			int i;
			for (i = 0; i < 80; i++)
			{
				array[i] = 32;
			}
			for (i = 0; i < headerText.Length && i < 80; i++)
			{
				array[i] = (byte)headerText[i];
			}
			if (i < 77)
			{
				array[i] = 13;
				array[i + 1] = 10;
			}
			array[78] = 13;
			array[79] = 10;
			using MemoryStream memoryStream = new MemoryStream();
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(array);
				int num = ((IEnumerable<MeshFace>)mesh.Faces).Count();
				binaryWriter.Write(num);
				if (verbose)
				{
					RhinoApp.WriteLine("Writing Encrypted STL File: Face Count {0}", (object)num);
				}
				for (i = 0; i < num; i++)
				{
					MeshFace val = mesh.Faces[i];
					Point3f val2 = mesh.Vertices[((MeshFace)(ref val)).A];
					Point3f val3 = mesh.Vertices[((MeshFace)(ref val)).B];
					Point3f val4 = mesh.Vertices[((MeshFace)(ref val)).C];
					Vector3f val5 = mesh.FaceNormals[i];
					if (!double.IsNaN(((Point3f)(ref val2)).X) && !double.IsNaN(((Point3f)(ref val2)).Y) && !double.IsNaN(((Point3f)(ref val2)).Z) && !double.IsNaN(((Point3f)(ref val3)).X) && !double.IsNaN(((Point3f)(ref val3)).Y) && !double.IsNaN(((Point3f)(ref val3)).Z) && !double.IsNaN(((Point3f)(ref val4)).X) && !double.IsNaN(((Point3f)(ref val4)).Y) && !double.IsNaN(((Point3f)(ref val4)).Z))
					{
						binaryWriter.Write(((Vector3f)(ref val5)).X);
						binaryWriter.Write(((Vector3f)(ref val5)).Y);
						binaryWriter.Write(((Vector3f)(ref val5)).Z);
						binaryWriter.Write(((Point3f)(ref val2)).X);
						binaryWriter.Write(((Point3f)(ref val2)).Y);
						binaryWriter.Write(((Point3f)(ref val2)).Z);
						binaryWriter.Write(((Point3f)(ref val3)).X);
						binaryWriter.Write(((Point3f)(ref val3)).Y);
						binaryWriter.Write(((Point3f)(ref val3)).Z);
						binaryWriter.Write(((Point3f)(ref val4)).X);
						binaryWriter.Write(((Point3f)(ref val4)).Y);
						binaryWriter.Write(((Point3f)(ref val4)).Z);
						ushort value = 0;
						binaryWriter.Write(value);
					}
				}
				binaryWriter.Close();
			}
			memoryStream.Flush();
			byte[] bytes = Encrypt(memoryStream.ToArray(), "7XSjdv2dJaCXAW/2CsH6SbyO7pKb358QO1GWaDAGnbiXQVHivwgoZwFROVvDKgefFkhF/bjFTPgRXFm1VgTtfkQVT9dSJwuVpp2UgjFP3enBvpg5V0klW00OQ4bG3RpZ5EGQqiEXgfnf2pm5c5QxvdUKgY9QKoZZ91xOIXXVv+VBBkysTEExcJyd9wvaWovP+MgUIW2dwLlfZsk2kpxgTskb7ZeESlIQkQEG0+zEBvlYhtyiElG2romesw4RT44MM8BPeOURZ7SeQJzj5UVkIVWBxJrW8BtE10AFzPHdzSQeD1BhmhDpbVxCb59LCp5bbozLHD1T43hK0Vn1JVDr253RBMfdc9KLIEzAIa1RCDnEnEH7jcSl48RSeMqVmJ6WYMQWZBbW8qwUkgi4ZdD/HcxcvpMHvBhQVUEM+i0ay3QerEkojZR7WE1Wm1pCKBfdsgRDWv2OvC1KmJw/RQT1gFCQqxdRZk4c5dlRpOjR1D1XrhAzpFG8ExTGeEaRoocAE5Eadu1Vbb0WUoV/74xiKc0bfo9VdweXv02JlK2WEHDQTBUvjpj+bhDW9Fvb3ccUYYRSJ+BbOGCOdcQvp1hqpUmGgotE69Mbv1Fbgmzbn2CUMs22XVDwYAHZ30IZv4fH8RHJrHEQ+/HbMsDm0Zy8tBlCysqaNJ9bQhUG7mjTPy1V48wqkBx9NMATZtdE4spdDQQPVnnazKWVPZFrpZyn9dGFNA+FOIJEVJVCT+GAqTEdYl0tvxTpaMjLGEtBQE/BoMBFUWUeQbUYIV15tZE6bxEOC8pCUkMFNYB/oJGth6ouSAg128GvXB2ImXvzCN0/4rwT/egjXOBb0tE+vN7PFlFuUHwk7T1Z5a32yuWvzaabOqSF9XE5mzQHrPxkIdT3Szx/b82/pNWNo/18toJM/LxDkqdp5LcUeWmGkKf7VkyXf1OiWgKMl3ELSrt+K1ECwJq6g5Srf+SnFyZ2rpV5ykJBwmbIg+KdfLY//FROQoLh7KVT/Jy5fN4eUO7wxYYaSRFgF2UXjmvn1+ZrLCdtyAGASM9WDHrOShCbzrhUluvqkldEXDngNeHpnwTT0yffApeqx9RaSsbxiChJKbnoKOzCSEiy519bmMxodq5ZG1yRBD/dJbEjdXnUSeck7uIi7Tya9MX8K7op/HTqpdXikAAA");
			File.WriteAllBytes(filename, bytes);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return false;
		}
		return true;
	}

	private static byte[] Encrypt(byte[] inputData, byte[] Key, byte[] IV)
	{
		MemoryStream memoryStream = new MemoryStream();
		Rijndael rijndael = Rijndael.Create();
		rijndael.Key = Key;
		rijndael.IV = IV;
		CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(inputData, 0, inputData.Length);
		cryptoStream.Close();
		return memoryStream.ToArray();
	}

	private static byte[] Encrypt(byte[] inputData, string Password)
	{
		PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 118
		});
		return Encrypt(inputData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
	}

	private static byte[] Decrypt(byte[] encryptedData, byte[] Key, byte[] IV)
	{
		MemoryStream memoryStream = new MemoryStream();
		Rijndael rijndael = Rijndael.Create();
		rijndael.Key = Key;
		rijndael.IV = IV;
		CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(encryptedData, 0, encryptedData.Length);
		cryptoStream.Close();
		return memoryStream.ToArray();
	}

	private static byte[] Decrypt(byte[] encryptedData, string Password)
	{
		try
		{
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[13]
			{
				73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
				100, 101, 118
			});
			return Decrypt(encryptedData, passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
		}
		catch (Exception)
		{
			return null;
		}
	}
}
