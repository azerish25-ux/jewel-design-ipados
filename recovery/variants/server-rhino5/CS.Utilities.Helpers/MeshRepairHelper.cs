using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GemvisionGeometry.Geometry;
using OctTree;
using Rhino;
using Rhino.Collections;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CS.Utilities.Helpers;

public static class MeshRepairHelper
{
	public static bool CompareMeshBoundingBoxes(List<GeometryBase> geometryBases, Mesh mesh)
	{
		if (geometryBases == null)
		{
			return false;
		}
		if (mesh == null)
		{
			return false;
		}
		BoundingBox unset = BoundingBox.Unset;
		unset.MakeValid();
		foreach (BoundingBox item in geometryBases.Select((GeometryBase geometryBase) => geometryBase.GetBoundingBox(accurate: true)))
		{
			item.MakeValid();
			if (item.IsValid)
			{
				unset.Union(item);
			}
		}
		unset.MakeValid();
		BoundingBox boundingBox = mesh.GetBoundingBox(accurate: true);
		boundingBox.MakeValid();
		return CompareMeshBoundingBoxes(unset, boundingBox);
	}

	public static bool CompareMeshBoundingBoxes(Mesh meshLhs, Mesh meshRhs)
	{
		if (meshLhs == null)
		{
			return false;
		}
		if (meshRhs == null)
		{
			return false;
		}
		BoundingBox boundingBox = meshLhs.GetBoundingBox(accurate: true);
		boundingBox.MakeValid();
		BoundingBox boundingBox2 = meshRhs.GetBoundingBox(accurate: true);
		boundingBox2.MakeValid();
		return CompareMeshBoundingBoxes(boundingBox, boundingBox2);
	}

	public static List<Mesh> GetMeshList(List<GeometryBase> geometryBases)
	{
		MeshingParameters coarse = MeshingParameters.Coarse;
		return GetMeshList(geometryBases, coarse);
	}

	public static List<Mesh> GetMeshList(List<GeometryBase> geometryBases, MeshingParameters meshingParameters)
	{
		List<Brep> list = new List<Brep>();
		List<GeometryBase> list2 = new List<GeometryBase>();
		foreach (GeometryBase geometryBasis in geometryBases)
		{
			if (geometryBasis.ObjectType == ObjectType.Brep)
			{
				Brep brep = (Brep)geometryBasis;
				if (!brep.IsSolid)
				{
					Brep brep2 = brep.CapPlanarHoles(0.001);
					if (brep2 != null)
					{
						brep = brep2;
					}
				}
				list.Add(brep);
			}
			else
			{
				Mesh item = CreateMesh(geometryBasis, doCapPlanarHoles: true, meshingParameters, 10, doOptimizeMilgrain: true);
				list2.Add(item);
			}
		}
		if (list.Count > 0)
		{
			Brep[] collection = Brep.JoinBreps(list, 0.001);
			list2.AddRange(collection);
		}
		List<Mesh> list3 = new List<Mesh>();
		foreach (GeometryBase item2 in list2)
		{
			Mesh mesh = CreateMesh(item2, doCapPlanarHoles: true, meshingParameters, 10, doOptimizeMilgrain: true);
			if (mesh == null)
			{
				RhinoApp.WriteLine("Could not create Mesh Object");
				continue;
			}
			mesh.Faces.ConvertQuadsToTriangles();
			if (mesh.SolidOrientation() == -1)
			{
				mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
			}
			list3.Add(mesh);
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
		if (!mesh.IsValid)
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
		if (!mesh.IsManifold(topologicalTest: true, out var isOriented, out var _))
		{
			msg = "mesh is manifold";
			return false;
		}
		if (!isOriented)
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
		List<Brep> list = new List<Brep>();
		List<GeometryBase> list2 = new List<GeometryBase>();
		foreach (GeometryBase geometryBasis in geometryBases)
		{
			if (geometryBasis.ObjectType == ObjectType.Brep)
			{
				Brep brep = (Brep)geometryBasis;
				if (!brep.IsSolid)
				{
					Brep brep2 = brep.CapPlanarHoles(0.001);
					if (brep2 != null)
					{
						brep = brep2;
					}
				}
				list.Add(brep);
			}
			else
			{
				Mesh item = CreateMesh(geometryBasis, doCapPlanarHoles: true, meshingParameters, milgrainGridCount, doOptimizeMilgrain);
				list2.Add(item);
			}
		}
		if (list.Count > 0)
		{
			Brep[] collection = Brep.JoinBreps(list, 0.001);
			list2.AddRange(collection);
		}
		Mesh mesh = new Mesh();
		foreach (GeometryBase item2 in list2)
		{
			Mesh mesh2 = CreateMesh(item2, doCapPlanarHoles: true, meshingParameters, milgrainGridCount, doOptimizeMilgrain);
			if (mesh2 == null)
			{
				RhinoApp.WriteLine("Could not create Mesh Object");
				continue;
			}
			mesh2.Faces.ConvertQuadsToTriangles();
			if (mesh2.SolidOrientation() == -1)
			{
				mesh2.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
			}
			if (doPreRepairShells)
			{
				using Mesh mesh3 = mesh2.DuplicateMesh();
				if (!mesh2.IsClosed)
				{
					RhinoApp.CommandPrompt = "Prefixing Single Open Shell";
					mesh2 = NetfabbRepairMesh(mesh2, doExtendedRepair: false);
				}
				if (mesh2 == null)
				{
					mesh2 = mesh3.DuplicateMesh();
				}
			}
			mesh.Append(mesh2);
		}
		Mesh theRealSlimShady = NetfabbRepairMesh(mesh, doExtendedRepair);
		if (theRealSlimShady != null && !GetTheRealSlimShady(geometryBases, theRealSlimShady, out theRealSlimShady))
		{
			RhinoApp.WriteLine("The real Slim Shady failed to stand up");
		}
		mesh.Dispose();
		return theRealSlimShady;
	}

	public static bool SaveStlBinary(Mesh mesh, string filename, bool doVerbose)
	{
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
				int num = mesh.Faces.Count();
				binaryWriter.Write(num);
				if (doVerbose)
				{
					RhinoApp.CommandPrompt = $"Saving STL File {filename}";
					RhinoApp.WriteLine("Saving STL File: Face Count {0}", num);
				}
				for (i = 0; i < num; i++)
				{
					MeshFace meshFace = mesh.Faces[i];
					Point3f point3f = mesh.Vertices[meshFace.A];
					Point3f point3f2 = mesh.Vertices[meshFace.B];
					Point3f point3f3 = mesh.Vertices[meshFace.C];
					Vector3f vector3f = mesh.FaceNormals[i];
					if (!double.IsNaN(point3f.X) && !double.IsNaN(point3f.Y) && !double.IsNaN(point3f.Z) && !double.IsNaN(point3f2.X) && !double.IsNaN(point3f2.Y) && !double.IsNaN(point3f2.Z) && !double.IsNaN(point3f3.X) && !double.IsNaN(point3f3.Y) && !double.IsNaN(point3f3.Z))
					{
						binaryWriter.Write(vector3f.X);
						binaryWriter.Write(vector3f.Y);
						binaryWriter.Write(vector3f.Z);
						binaryWriter.Write(point3f.X);
						binaryWriter.Write(point3f.Y);
						binaryWriter.Write(point3f.Z);
						binaryWriter.Write(point3f2.X);
						binaryWriter.Write(point3f2.Y);
						binaryWriter.Write(point3f2.Z);
						binaryWriter.Write(point3f3.X);
						binaryWriter.Write(point3f3.Y);
						binaryWriter.Write(point3f3.Z);
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
			Mesh mesh = new Mesh();
			int num2 = 1;
			Mesh[] array2 = array;
			foreach (Mesh mesh2 in array2)
			{
				int num3 = (num2 * 200 + num) / (num * 2);
				RhinoApp.CommandPrompt = $"Slicing Shell {num2} of {num} : {num3}%";
				BoundingBox boundingBox = mesh2.GetBoundingBox(accurate: true);
				boundingBox.MakeValid();
				Point3d[] corners = boundingBox.GetCorners();
				Point3d[] array3 = new LineCurve(corners[0], corners[4]).DivideEquidistant(sliceThickness);
				foreach (Point3d origin in array3)
				{
					Plane sectionPlane = new Plane(origin, Vector3d.ZAxis);
					CurveList curveList = new CurveList();
					Curve[] array4 = Mesh.CreateContourCurves(mesh2, sectionPlane);
					if (!array4.Any())
					{
						continue;
					}
					Curve[] array5 = array4;
					foreach (Curve curve in array5)
					{
						NurbsCurve nurbsCurve = curve.Rebuild(50, 3, preserveTangents: false);
						if (nurbsCurve.IsValid)
						{
							if (curve.IsClosed)
							{
								curveList.Add(nurbsCurve);
							}
						}
						else if (curve.IsClosed)
						{
							curveList.Add(curve);
						}
					}
					if (!curveList.Any())
					{
						continue;
					}
					Brep[] array6 = Brep.CreatePlanarBreps(curveList);
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
						foreach (Mesh mesh3 in array9)
						{
							if (mesh3.IsValid)
							{
								Mesh other = mesh3.Offset(sliceThickness, solidify: true);
								mesh.Append(other);
							}
						}
					}
				}
				num2++;
			}
			return mesh;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private static bool CompareMeshBoundingBoxes(BoundingBox boundingBoxLhs, BoundingBox boundingBoxRhs, double tolerance = 0.05)
	{
		double num = boundingBoxLhs.Max.X - boundingBoxLhs.Min.X;
		double num2 = boundingBoxLhs.Max.Y - boundingBoxLhs.Min.Y;
		double num3 = boundingBoxLhs.Max.Z - boundingBoxLhs.Min.Z;
		double num4 = boundingBoxRhs.Max.X - boundingBoxRhs.Min.X;
		double num5 = boundingBoxRhs.Max.Y - boundingBoxRhs.Min.Y;
		double num6 = boundingBoxRhs.Max.Z - boundingBoxRhs.Min.Z;
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
		try
		{
			Mesh mesh = new Mesh();
			if (geometryBase != null)
			{
				if (geometryBase.ObjectType == ObjectType.Mesh && geometryBase is Mesh { IsValid: not false })
				{
					mesh.Append((Mesh)geometryBase);
					mesh.Faces.ConvertQuadsToTriangles();
					return mesh;
				}
				Brep brep = null;
				if (geometryBase.ObjectType == ObjectType.Extrusion && geometryBase is Extrusion extrusion)
				{
					Brep brep2 = extrusion.ToBrep(splitKinkyFaces: true);
					if (brep2 != null)
					{
						brep = brep2;
					}
				}
				if (geometryBase.ObjectType == ObjectType.Surface && geometryBase is Surface surface)
				{
					Brep brep3 = surface.ToBrep();
					if (brep3 != null)
					{
						brep = brep3;
					}
				}
				if (geometryBase.ObjectType == ObjectType.Brep && geometryBase is Brep brep4)
				{
					brep = brep4;
				}
				if (brep == null)
				{
					return null;
				}
				if (doOptimizeMilgrain && 1 == brep.Faces.Count && brep.Trims.Count == 0)
				{
					brep.Faces[0].TryGetSphere(out var sphere);
					if (sphere.Radius != 0.0 && sphere.Radius <= 1.5)
					{
						Mesh mesh3 = Mesh.CreateFromSphere(sphere, milgrainGridCount, milgrainGridCount);
						mesh3.Faces.ConvertQuadsToTriangles();
						return mesh3;
					}
				}
				Mesh[] array2;
				if (doCapPlanarHoles)
				{
					Brep brep5 = brep.CapPlanarHoles(0.001);
					if (brep5 != null)
					{
						Mesh[] array = Mesh.CreateFromBrep(brep5, meshingParameters);
						brep5.Dispose();
						array2 = array;
						foreach (Mesh other in array2)
						{
							mesh.Append(other);
						}
						mesh.Faces.ConvertQuadsToTriangles();
						return mesh;
					}
				}
				Mesh[] array3 = Mesh.CreateFromBrep(brep, meshingParameters);
				if (array3 == null)
				{
					return null;
				}
				array2 = array3;
				foreach (Mesh other2 in array2)
				{
					mesh.Append(other2);
				}
			}
			return mesh;
		}
		catch (Exception ex)
		{
			RhinoApp.WriteLine(ex.Message);
			return null;
		}
	}

	private static Mesh GetMeshFromObjects(IReadOnlyCollection<GeometryBase> geometryBases, double tolerance, int gridAspectRatio, bool doOptimizeMilgrain)
	{
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
			Mesh mesh = new Mesh();
			int num = 1;
			int count = geometryBases.Count;
			foreach (GeometryBase geometryBasis in geometryBases)
			{
				int num2 = (200 + count) / (count * 2);
				RhinoApp.CommandPrompt = $"Calculating Mesh Shell {num} of {count} : {num2}%";
				Mesh mesh2 = CreateMesh(geometryBasis, doCapPlanarHoles: true, meshingParameters, 10, doOptimizeMilgrain);
				if (mesh2.SolidOrientation() == -1)
				{
					mesh2.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
				}
				mesh.Append(mesh2);
			}
			mesh.Compact();
			return mesh;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private static MeshingParameters GetMeshingParameters(double meshingParameterGridAspectRatio = 5.0, double meshingParameterTolerance = 0.002)
	{
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
		BoundingBox boundingBox = inputMesh.GetBoundingBox(accurate: true);
		boundingBox.MakeValid();
		inputMesh.Transform(Transform.Scale(boundingBox.Center, 3.0));
		RhinoApp.WriteLine("Repairing and Unifying Mesh...Please Wait");
		if (!NetfabbHelper.NetfabbBuildMesh(inputMesh, doExtendedRepair, out var meshRepaired))
		{
			RhinoApp.WriteLine("Mesh Creation Failed.");
			RhinoApp.WriteLine(NetfabbHelper.LastNetFabError.ToString());
			RhinoApp.WriteLine("Mesh Repair failed - {0}", NetfabbHelper.LastNetFabError);
			return null;
		}
		if (meshRepaired == null)
		{
			RhinoApp.WriteLine("Mesh Creation Failed.");
			RhinoApp.WriteLine(NetfabbHelper.LastNetFabError.ToString());
			RhinoApp.WriteLine("Mesh Repair returned null output - {0}", NetfabbHelper.LastNetFabError);
			return null;
		}
		if (!meshRepaired.IsValid)
		{
			RhinoApp.WriteLine("Resulting Mesh Is Invalid");
			RhinoApp.WriteLine(NetfabbHelper.LastNetFabError.ToString());
			RhinoApp.WriteLine("Mesh Repair returned invalid output - {0}", NetfabbHelper.LastNetFabError);
			return null;
		}
		meshRepaired.GetBoundingBox(accurate: true).MakeValid();
		meshRepaired.Transform(Transform.Scale(boundingBox.Center, 1.0 / 3.0));
		meshRepaired.Unweld(CSCoreMath.ToRadians(22.5), modifyNormals: true);
		return meshRepaired;
	}

	private static void RandomScaleGeometryBase(GeometryBase geometryBase)
	{
		Random random = new Random();
		double scaleFactor = 1.001 + random.NextDouble() * 0.004;
		BoundingBox boundingBox = geometryBase.GetBoundingBox(accurate: true);
		boundingBox.MakeValid();
		geometryBase.Transform(Transform.Scale(boundingBox.Center, scaleFactor));
	}

	public static Mesh MeshFromOctTreeMeshes(GMesh oMesh, bool convertToTris = true)
	{
		Mesh mesh = new Mesh();
		int count = oMesh.Vertices.Count;
		for (int i = 0; i < count; i++)
		{
			GPoint3D gPoint3D = oMesh.Vertices[i];
			mesh.Vertices.Add(new Point3d(gPoint3D.X, gPoint3D.Y, gPoint3D.Z));
		}
		int count2 = oMesh.Faces.Count;
		for (int j = 0; j < count2; j++)
		{
			GMeshFace gMeshFace = oMesh.Faces[j];
			mesh.Faces.AddFace(gMeshFace.IsQuad ? new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C, gMeshFace.D) : new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C));
		}
		if (convertToTris)
		{
			mesh.Faces.ConvertQuadsToTriangles();
		}
		mesh.Normals.ComputeNormals();
		return mesh;
	}

	public static List<Mesh> MeshFromOctTreeMeshes(List<GMesh> lstoMeshes)
	{
		List<Mesh> list = new List<Mesh>();
		foreach (GMesh lstoMesh in lstoMeshes)
		{
			Mesh mesh = new Mesh();
			int count = lstoMesh.Vertices.Count;
			for (int i = 0; i < count; i++)
			{
				GPoint3D gPoint3D = lstoMesh.Vertices[i];
				mesh.Vertices.Add(new Point3d(gPoint3D.X, gPoint3D.Y, gPoint3D.Z));
			}
			int count2 = lstoMesh.Faces.Count;
			for (int j = 0; j < count2; j++)
			{
				GMeshFace gMeshFace = lstoMesh.Faces[j];
				mesh.Faces.AddFace(gMeshFace.IsQuad ? new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C, gMeshFace.D) : new MeshFace(gMeshFace.A, gMeshFace.B, gMeshFace.C));
			}
			list.Add(mesh);
		}
		return list;
	}

	public static GMesh OctTreeMeshFromMesh(Mesh mesh)
	{
		GMesh gMesh = new GMesh();
		int count = mesh.Vertices.Count;
		for (int i = 0; i < count; i++)
		{
			Point3f point3f = mesh.Vertices[i];
			gMesh.Vertices.Add(new GPoint3D(point3f.X, point3f.Y, point3f.Z));
		}
		int count2 = mesh.Faces.Count;
		for (int j = 0; j < count2; j++)
		{
			MeshFace meshFace = mesh.Faces[j];
			gMesh.Faces.Add(meshFace.IsQuad ? new GMeshFace(meshFace.A, meshFace.B, meshFace.C, meshFace.D) : new GMeshFace(meshFace.A, meshFace.B, meshFace.C));
		}
		return gMesh;
	}

	public static double GetVolumeViaOctTree(List<Mesh> lstMeshes)
	{
		List<GMesh> lstMesh = OctTreeMeshFromMesh(lstMeshes);
		double volume = 0.0;
		global::OctTree.OctTree.GetVolume(lstMesh, makeOutputMesh: false, out volume, out var _);
		return volume;
	}

	public static List<GMesh> OctTreeMeshFromMesh(IEnumerable<Mesh> meshes)
	{
		List<GMesh> list = new List<GMesh>();
		foreach (Mesh mesh in meshes)
		{
			if (mesh != null)
			{
				GMesh gMesh = new GMesh();
				int count = mesh.Vertices.Count;
				for (int i = 0; i < count; i++)
				{
					Point3f point3f = mesh.Vertices[i];
					gMesh.Vertices.Add(new GPoint3D(point3f.X, point3f.Y, point3f.Z));
				}
				int count2 = mesh.Faces.Count;
				for (int j = 0; j < count2; j++)
				{
					MeshFace meshFace = mesh.Faces[j];
					gMesh.Faces.Add(meshFace.IsQuad ? new GMeshFace(meshFace.A, meshFace.B, meshFace.C, meshFace.D) : new GMeshFace(meshFace.A, meshFace.B, meshFace.C));
				}
				list.Add(gMesh);
			}
		}
		return list;
	}
}
