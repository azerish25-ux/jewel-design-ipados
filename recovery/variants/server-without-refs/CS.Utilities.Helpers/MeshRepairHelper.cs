using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GemvisionGeometry.Geometry;
using OctTree;
using Rhino;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS.Utilities.Helpers;

public static class MeshRepairHelper
{
	public static bool CompareMeshBoundingBoxes(List<GeometryBase> geometryBases, Mesh mesh)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		if (geometryBases == null)
		{
			return false;
		}
		if (mesh == null)
		{
			return false;
		}
		BoundingBox unset = BoundingBox.Unset;
		((BoundingBox)(ref unset)).MakeValid();
		foreach (BoundingBox item in geometryBases.Select((GeometryBase geometryBase) => geometryBase.GetBoundingBox(true)))
		{
			BoundingBox current = item;
			((BoundingBox)(ref current)).MakeValid();
			if (((BoundingBox)(ref current)).IsValid)
			{
				((BoundingBox)(ref unset)).Union(current);
			}
		}
		((BoundingBox)(ref unset)).MakeValid();
		BoundingBox boundingBox = ((GeometryBase)mesh).GetBoundingBox(true);
		((BoundingBox)(ref boundingBox)).MakeValid();
		return CompareMeshBoundingBoxes(unset, boundingBox);
	}

	public static bool CompareMeshBoundingBoxes(Mesh meshLhs, Mesh meshRhs)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (meshLhs == null)
		{
			return false;
		}
		if (meshRhs == null)
		{
			return false;
		}
		BoundingBox boundingBox = ((GeometryBase)meshLhs).GetBoundingBox(true);
		((BoundingBox)(ref boundingBox)).MakeValid();
		BoundingBox boundingBox2 = ((GeometryBase)meshRhs).GetBoundingBox(true);
		((BoundingBox)(ref boundingBox2)).MakeValid();
		return CompareMeshBoundingBoxes(boundingBox, boundingBox2);
	}

	public static List<Mesh> GetMeshList(List<GeometryBase> geometryBases)
	{
		MeshingParameters coarse = MeshingParameters.Coarse;
		return GetMeshList(geometryBases, coarse);
	}

	public static List<Mesh> GetMeshList(List<GeometryBase> geometryBases, MeshingParameters meshingParameters)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		List<Brep> list = new List<Brep>();
		List<GeometryBase> list2 = new List<GeometryBase>();
		foreach (GeometryBase geometryBasis in geometryBases)
		{
			if ((int)geometryBasis.ObjectType == 16)
			{
				Brep val = (Brep)geometryBasis;
				if (!val.IsSolid)
				{
					Brep val2 = val.CapPlanarHoles(0.001);
					if (val2 != null)
					{
						val = val2;
					}
				}
				list.Add(val);
			}
			else
			{
				Mesh item = CreateMesh(geometryBasis, doCapPlanarHoles: true, meshingParameters, 10, doOptimizeMilgrain: true);
				list2.Add((GeometryBase)(object)item);
			}
		}
		if (list.Count > 0)
		{
			Brep[] collection = Brep.JoinBreps((IEnumerable<Brep>)list, 0.001);
			list2.AddRange((IEnumerable<GeometryBase>)(object)collection);
		}
		List<Mesh> list3 = new List<Mesh>();
		foreach (GeometryBase item2 in list2)
		{
			Mesh val3 = CreateMesh(item2, doCapPlanarHoles: true, meshingParameters, 10, doOptimizeMilgrain: true);
			if (val3 == null)
			{
				RhinoApp.WriteLine("Could not create Mesh Object");
				continue;
			}
			val3.Faces.ConvertQuadsToTriangles();
			if (val3.SolidOrientation() == -1)
			{
				val3.Flip(true, true, true);
			}
			list3.Add(val3);
		}
		return list3;
	}

	public static bool GetTheRealSlimShady(List<GeometryBase> geometryBases, Mesh mesh, out Mesh theRealSlimShady)
	{
		theRealSlimShady = null;
		if (geometryBases == null)
		{
			return false;
		}
		if (mesh == null)
		{
			return false;
		}
		if (mesh.DisjointMeshCount <= 1)
		{
			theRealSlimShady = mesh;
			return true;
		}
		List<int> list = new List<int>();
		Mesh[] array = mesh.SplitDisjointPieces();
		for (int i = 0; i < array.Length; i++)
		{
			if (CompareMeshBoundingBoxes(geometryBases, array[i]))
			{
				list.Add(i);
			}
		}
		if (list.Count != 1)
		{
			return false;
		}
		theRealSlimShady = array[list.First()];
		return true;
	}

	public static bool IsMeshPrintable(Mesh mesh, out string msg)
	{
		msg = string.Empty;
		if (mesh == null)
		{
			msg = "mesh is null";
			return false;
		}
		if (!((CommonObject)mesh).IsValid)
		{
			msg = "mesh is not valid";
			return false;
		}
		if (!mesh.IsClosed)
		{
			msg = "mesh is not closed";
			return false;
		}
		if (mesh.SolidOrientation() == -1)
		{
			msg = "mesh is not solid";
			return false;
		}
		bool flag = default(bool);
		bool flag2 = default(bool);
		if (!mesh.IsManifold(true, ref flag, ref flag2))
		{
			msg = "mesh is manifold";
			return false;
		}
		if (!flag)
		{
			msg = "mesh is not oriented";
			return false;
		}
		if (mesh.DisjointMeshCount > 1)
		{
			msg = $"mesh has {mesh.DisjointMeshCount} disjoint parts";
			return false;
		}
		Polyline[] nakedEdges = mesh.GetNakedEdges();
		if (nakedEdges != null && nakedEdges.Length != 0)
		{
			msg = "mesh has naked edges";
			return false;
		}
		if (mesh.Faces.CullDegenerateFaces() > 0)
		{
			msg = "mesh has degenerate faces";
			return false;
		}
		if (mesh.Vertices.CullUnused() > 0)
		{
			msg = "mesh has unused vertices";
			return false;
		}
		return true;
	}

	public static Mesh RepairMesh(List<GeometryBase> geometryBases, bool doExtendedRepair, bool doPreRepairShells, int milgrainGridCount = 10, bool doOptimizeMilgrain = true)
	{
		MeshingParameters meshingParameters = GetMeshingParameters();
		return RepairMesh(geometryBases, doExtendedRepair, doPreRepairShells, meshingParameters, milgrainGridCount, doOptimizeMilgrain);
	}

	public static Mesh RepairMesh(List<GeometryBase> geometryBases, bool doExtendedRepair, bool doPreRepairShells, MeshingParameters meshingParameters, int milgrainGridCount = 10, bool doOptimizeMilgrain = true)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		List<Brep> list = new List<Brep>();
		List<GeometryBase> list2 = new List<GeometryBase>();
		foreach (GeometryBase geometryBasis in geometryBases)
		{
			if ((int)geometryBasis.ObjectType == 16)
			{
				Brep val = (Brep)geometryBasis;
				if (!val.IsSolid)
				{
					Brep val2 = val.CapPlanarHoles(0.001);
					if (val2 != null)
					{
						val = val2;
					}
				}
				list.Add(val);
			}
			else
			{
				Mesh item = CreateMesh(geometryBasis, doCapPlanarHoles: true, meshingParameters, milgrainGridCount, doOptimizeMilgrain);
				list2.Add((GeometryBase)(object)item);
			}
		}
		if (list.Count > 0)
		{
			Brep[] collection = Brep.JoinBreps((IEnumerable<Brep>)list, 0.001);
			list2.AddRange((IEnumerable<GeometryBase>)(object)collection);
		}
		Mesh val3 = new Mesh();
		foreach (GeometryBase item2 in list2)
		{
			Mesh val4 = CreateMesh(item2, doCapPlanarHoles: true, meshingParameters, milgrainGridCount, doOptimizeMilgrain);
			if (val4 == null)
			{
				RhinoApp.WriteLine("Could not create Mesh Object");
				continue;
			}
			val4.Faces.ConvertQuadsToTriangles();
			if (val4.SolidOrientation() == -1)
			{
				val4.Flip(true, true, true);
			}
			if (doPreRepairShells)
			{
				Mesh val5 = val4.DuplicateMesh();
				try
				{
					if (!val4.IsClosed)
					{
						RhinoApp.CommandPrompt = "Prefixing Single Open Shell";
						val4 = NetfabbRepairMesh(val4, doExtendedRepair: false);
					}
					if (val4 == null)
					{
						val4 = val5.DuplicateMesh();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			val3.Append(val4);
		}
		Mesh theRealSlimShady = NetfabbRepairMesh(val3, doExtendedRepair);
		if (theRealSlimShady != null && !GetTheRealSlimShady(geometryBases, theRealSlimShady, out theRealSlimShady))
		{
			RhinoApp.WriteLine("The real Slim Shady failed to stand up");
		}
		((CommonObject)val3).Dispose();
		return theRealSlimShady;
	}

	public static bool SaveStlBinary(Mesh mesh, string filename, bool doVerbose)
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
				if (doVerbose)
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
				if (doVerbose)
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

	public static Mesh SliceMesh(List<GeometryBase> geometryBases, bool doRandomScale, bool doExtendedRepair, double sliceThickness)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<GeometryBase> list = new List<GeometryBase>();
			Mesh meshFromObjects;
			if (doRandomScale)
			{
				foreach (GeometryBase geometryBasis in geometryBases)
				{
					RandomScaleGeometryBase(geometryBasis);
					list.Add(geometryBasis);
				}
				meshFromObjects = GetMeshFromObjects(list, 0.002, 1, doOptimizeMilgrain: false);
			}
			else
			{
				meshFromObjects = GetMeshFromObjects(geometryBases, 0.002, 1, doOptimizeMilgrain: false);
			}
			meshFromObjects = NetfabbRepairMesh(meshFromObjects, doExtendedRepair);
			Mesh[] array = meshFromObjects.SplitDisjointPieces();
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
						}
						else if (val6.IsClosed)
						{
							((RhinoList<Curve>)(object)val5).Add(val6);
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
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private static bool CompareMeshBoundingBoxes(BoundingBox boundingBoxLhs, BoundingBox boundingBoxRhs, double tolerance = 0.05)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = ((BoundingBox)(ref boundingBoxLhs)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBoxLhs)).Min;
		double num = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBoxLhs)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBoxLhs)).Min;
		double num2 = y - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBoxLhs)).Max;
		double z = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBoxLhs)).Min;
		double num3 = z - ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBoxRhs)).Max;
		double x2 = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBoxRhs)).Min;
		double num4 = x2 - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBoxRhs)).Max;
		double y2 = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBoxRhs)).Min;
		double num5 = y2 - ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBoxRhs)).Max;
		double z2 = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBoxRhs)).Min;
		double num6 = z2 - ((Point3d)(ref val)).Z;
		double num7 = Math.Abs(num - num4);
		double num8 = Math.Abs(num2 - num5);
		double num9 = Math.Abs(num3 - num6);
		if (num7 <= tolerance && num8 <= tolerance)
		{
			return num9 <= tolerance;
		}
		return false;
	}

	private static Mesh CreateMesh(GeometryBase geometryBase, bool doCapPlanarHoles, MeshingParameters meshingParameters, int milgrainGridCount, bool doOptimizeMilgrain)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Mesh val = new Mesh();
			if (geometryBase != null)
			{
				if ((int)geometryBase.ObjectType == 32)
				{
					Mesh val2 = (Mesh)(object)((geometryBase is Mesh) ? geometryBase : null);
					if (val2 != null && ((CommonObject)val2).IsValid)
					{
						val.Append((Mesh)geometryBase);
						val.Faces.ConvertQuadsToTriangles();
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
						if (val5 != null)
						{
							val3 = val5;
						}
					}
				}
				if ((int)geometryBase.ObjectType == 8)
				{
					Surface val6 = (Surface)(object)((geometryBase is Surface) ? geometryBase : null);
					if (val6 != null)
					{
						Brep val7 = val6.ToBrep();
						if (val7 != null)
						{
							val3 = val7;
						}
					}
				}
				if ((int)geometryBase.ObjectType == 16)
				{
					Brep val8 = (Brep)(object)((geometryBase is Brep) ? geometryBase : null);
					if (val8 != null)
					{
						val3 = val8;
					}
				}
				if (val3 == null)
				{
					return null;
				}
				if (doOptimizeMilgrain && 1 == val3.Faces.Count && val3.Trims.Count == 0)
				{
					Sphere val9 = default(Sphere);
					((Surface)val3.Faces[0]).TryGetSphere(ref val9);
					if (((Sphere)(ref val9)).Radius != 0.0 && ((Sphere)(ref val9)).Radius <= 1.5)
					{
						Mesh obj = Mesh.CreateFromSphere(val9, milgrainGridCount, milgrainGridCount);
						obj.Faces.ConvertQuadsToTriangles();
						return obj;
					}
				}
				Mesh[] array2;
				if (doCapPlanarHoles)
				{
					Brep val10 = val3.CapPlanarHoles(0.001);
					if (val10 != null)
					{
						Mesh[] array = Mesh.CreateFromBrep(val10, meshingParameters);
						((CommonObject)val10).Dispose();
						array2 = array;
						foreach (Mesh val11 in array2)
						{
							val.Append(val11);
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
				foreach (Mesh val12 in array2)
				{
					val.Append(val12);
				}
			}
			return val;
		}
		catch (Exception ex)
		{
			RhinoApp.WriteLine(ex.Message);
			return null;
		}
	}

	private static Mesh GetMeshFromObjects(IReadOnlyCollection<GeometryBase> geometryBases, double tolerance, int gridAspectRatio, bool doOptimizeMilgrain)
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
			int count = geometryBases.Count;
			foreach (GeometryBase geometryBasis in geometryBases)
			{
				RhinoApp.CommandPrompt = string.Format(arg2: (200 + count) / (count * 2), format: "Calculating Mesh Shell {0} of {1} : {2}%", arg0: num, arg1: count);
				Mesh val2 = CreateMesh(geometryBasis, doCapPlanarHoles: true, meshingParameters, 10, doOptimizeMilgrain);
				if (val2.SolidOrientation() == -1)
				{
					val2.Flip(true, true, true);
				}
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

	private static MeshingParameters GetMeshingParameters(double meshingParameterGridAspectRatio = 5.0, double meshingParameterTolerance = 0.002)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		return new MeshingParameters
		{
			ComputeCurvature = false,
			GridAmplification = 0.0,
			GridAngle = 0.0,
			GridAspectRatio = meshingParameterGridAspectRatio,
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
			Tolerance = meshingParameterTolerance
		};
	}

	private static Mesh NetfabbRepairMesh(Mesh inputMesh, bool doExtendedRepair)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)inputMesh).GetBoundingBox(true);
		((BoundingBox)(ref boundingBox)).MakeValid();
		((GeometryBase)inputMesh).Transform(Transform.Scale(((BoundingBox)(ref boundingBox)).Center, 3.0));
		RhinoApp.WriteLine("Repairing and Unifying Mesh...Please Wait");
		if (!NetfabbHelper.NetfabbBuildMesh(inputMesh, doExtendedRepair, out var meshRepaired))
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
		((GeometryBase)meshRepaired).Transform(Transform.Scale(((BoundingBox)(ref boundingBox)).Center, 1.0 / 3.0));
		meshRepaired.Unweld(CSCoreMath.ToRadians(22.5), true);
		return meshRepaired;
	}

	private static void RandomScaleGeometryBase(GeometryBase geometryBase)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Random random = new Random();
		double num = 1.001 + random.NextDouble() * 0.004;
		BoundingBox boundingBox = geometryBase.GetBoundingBox(true);
		((BoundingBox)(ref boundingBox)).MakeValid();
		geometryBase.Transform(Transform.Scale(((BoundingBox)(ref boundingBox)).Center, num));
	}

	public static Mesh MeshFromOctTreeMeshes(GMesh oMesh, bool convertToTris = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = new Mesh();
		int count = oMesh.Vertices.Count;
		for (int i = 0; i < count; i++)
		{
			GPoint3D gPoint3D = oMesh.Vertices[i];
			val.Vertices.Add(new Point3d(gPoint3D.X, gPoint3D.Y, gPoint3D.Z));
		}
		int count2 = oMesh.Faces.Count;
		for (int j = 0; j < count2; j++)
		{
			GMeshFace gMeshFace = oMesh.Faces[j];
			val.Faces.AddFace(gMeshFace.IsQuad ? new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C, gMeshFace.D) : new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C));
		}
		if (convertToTris)
		{
			val.Faces.ConvertQuadsToTriangles();
		}
		val.Normals.ComputeNormals();
		return val;
	}

	public static List<Mesh> MeshFromOctTreeMeshes(List<GMesh> lstoMeshes)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		List<Mesh> list = new List<Mesh>();
		foreach (GMesh lstoMesh in lstoMeshes)
		{
			Mesh val = new Mesh();
			int count = lstoMesh.Vertices.Count;
			for (int i = 0; i < count; i++)
			{
				GPoint3D gPoint3D = lstoMesh.Vertices[i];
				val.Vertices.Add(new Point3d(gPoint3D.X, gPoint3D.Y, gPoint3D.Z));
			}
			int count2 = lstoMesh.Faces.Count;
			for (int j = 0; j < count2; j++)
			{
				GMeshFace gMeshFace = lstoMesh.Faces[j];
				val.Faces.AddFace(gMeshFace.IsQuad ? new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C, gMeshFace.D) : new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C));
			}
			list.Add(val);
		}
		return list;
	}

	public static GMesh OctTreeMeshFromMesh(Mesh mesh)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		GMesh gMesh = new GMesh();
		int count = mesh.Vertices.Count;
		for (int i = 0; i < count; i++)
		{
			Point3f val = mesh.Vertices[i];
			gMesh.Vertices.Add(new GPoint3D(((Point3f)(ref val)).X, ((Point3f)(ref val)).Y, ((Point3f)(ref val)).Z));
		}
		int count2 = mesh.Faces.Count;
		for (int j = 0; j < count2; j++)
		{
			MeshFace val2 = mesh.Faces[j];
			gMesh.Faces.Add(((MeshFace)(ref val2)).IsQuad ? new GMeshFace(((MeshFace)(ref val2)).A, ((MeshFace)(ref val2)).B, ((MeshFace)(ref val2)).C, ((MeshFace)(ref val2)).D) : new GMeshFace(((MeshFace)(ref val2)).A, ((MeshFace)(ref val2)).B, ((MeshFace)(ref val2)).C));
		}
		return gMesh;
	}

	public static double GetVolumeViaOctTree(List<Mesh> lstMeshes)
	{
		List<GMesh> lstMesh = OctTreeMeshFromMesh((IEnumerable<Mesh>)lstMeshes);
		double volume = 0.0;
		global::OctTree.OctTree.GetVolume(lstMesh, makeOutputMesh: false, out volume, out var _);
		return volume;
	}

	public static List<GMesh> OctTreeMeshFromMesh(IEnumerable<Mesh> meshes)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		List<GMesh> list = new List<GMesh>();
		foreach (Mesh mesh in meshes)
		{
			if (mesh != null)
			{
				GMesh gMesh = new GMesh();
				int count = mesh.Vertices.Count;
				for (int i = 0; i < count; i++)
				{
					Point3f val = mesh.Vertices[i];
					gMesh.Vertices.Add(new GPoint3D(((Point3f)(ref val)).X, ((Point3f)(ref val)).Y, ((Point3f)(ref val)).Z));
				}
				int count2 = mesh.Faces.Count;
				for (int j = 0; j < count2; j++)
				{
					MeshFace val2 = mesh.Faces[j];
					gMesh.Faces.Add(((MeshFace)(ref val2)).IsQuad ? new GMeshFace(((MeshFace)(ref val2)).A, ((MeshFace)(ref val2)).B, ((MeshFace)(ref val2)).C, ((MeshFace)(ref val2)).D) : new GMeshFace(((MeshFace)(ref val2)).A, ((MeshFace)(ref val2)).B, ((MeshFace)(ref val2)).C));
				}
				list.Add(gMesh);
			}
		}
		return list;
	}
}
