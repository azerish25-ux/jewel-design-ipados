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
			if (!mesh.IsValid)
			{
				return false;
			}
			if (!mesh.IsClosed)
			{
				return false;
			}
			if (!mesh.IsManifold(topologicalTest: true, out var isOriented, out var _))
			{
				return false;
			}
			if (!isOriented)
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
		try
		{
			Mesh mesh = new Mesh();
			if (geometryBase.ObjectType == ObjectType.Mesh && geometryBase is Mesh { IsValid: not false })
			{
				mesh.Append((Mesh)geometryBase);
				mesh.Faces.ConvertQuadsToTriangles();
				if (mesh.SolidOrientation() == -1)
				{
					mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
				}
				return mesh;
			}
			Brep brep = null;
			if (geometryBase.ObjectType == ObjectType.Extrusion && geometryBase is Extrusion extrusion)
			{
				Brep brep2 = extrusion.ToBrep(splitKinkyFaces: true);
				if (brep2 != null && brep2.IsValid)
				{
					brep = brep2;
				}
			}
			if (geometryBase.ObjectType == ObjectType.Surface && geometryBase is Surface)
			{
				Brep brep3 = Brep.TryConvertBrep(geometryBase);
				if (brep3.IsValid)
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
			if (optimizeMilgrain && 1 == brep.Faces.Count && brep.Trims.Count == 0)
			{
				brep.Faces[0].TryGetSphere(out var sphere);
				if (sphere.Radius != 0.0 && sphere.Radius <= 1.5)
				{
					Mesh mesh3 = Mesh.CreateFromSphere(sphere, milGrainGridCount, milGrainGridCount);
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
			if (mesh.SolidOrientation() == -1)
			{
				mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
			}
			return mesh;
		}
		catch (Exception ex)
		{
			RhinoApp.WriteLine(ex.Message);
			return null;
		}
	}

	public static Mesh CreateMesh(RhinoObject rhinoObject, bool doCapPlanarHoles, MeshingParameters meshingParameters, int milGrainGridCount, bool optimizeMilgrain)
	{
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
		if (mesh.DisjointMeshCount == 1)
		{
			return mesh;
		}
		Mesh mesh2 = new Mesh();
		Mesh[] array = mesh.SplitDisjointPieces();
		foreach (Mesh mesh3 in array)
		{
			double volume = VolumeMassProperties.Compute(mesh3).Volume;
			if (!(mesh3.GetBoundingBox(accurate: true).Diagonal.Length < 0.2) && !(volume < 10.0))
			{
				mesh2.Append(mesh3);
			}
		}
		return mesh2;
	}

	public static Mesh GetMeshFromObjects(List<GeometryBase> lstGeometryBases, double tolerance, int gridAspectRatio, bool optimizeMilgrain)
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
				MinimumEdgeLength = 0.001,
				MinimumTolerance = 0.0,
				RefineAngle = 0.0,
				RefineGrid = true,
				RelativeTolerance = 0.0,
				SimplePlanes = false,
				Tolerance = tolerance
			};
			Mesh mesh = new Mesh();
			int num = 1;
			int count = lstGeometryBases.Count;
			foreach (GeometryBase lstGeometryBasis in lstGeometryBases)
			{
				_ = (num * 200 + count) / (count * 2);
				Mesh other = CreateMesh(lstGeometryBasis, doCapPlanarHoles: true, meshingParameters, 10, optimizeMilgrain);
				mesh.Append(other);
			}
			mesh.Compact();
			return mesh;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh GetMeshFromObjects(List<RhinoObject> lstRhinoObjects, double tolerance, int gridAspectRatio, bool optimizeMilgrain)
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
			int count = lstRhinoObjects.Count;
			foreach (RhinoObject lstRhinoObject in lstRhinoObjects)
			{
				int num2 = (num * 200 + count) / (count * 2);
				RhinoApp.CommandPrompt = $"Calculating Mesh Shell {num} of {count} : {num2}%";
				Mesh other = CreateMesh(lstRhinoObject, doCapPlanarHoles: false, meshingParameters, 10, optimizeMilgrain);
				mesh.Append(other);
			}
			mesh.Compact();
			return mesh;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<RhinoObject> GetNormalObjects()
	{
		ObjectEnumeratorSettings settings = new ObjectEnumeratorSettings
		{
			ObjectTypeFilter = (ObjectType.Surface | ObjectType.Brep | ObjectType.Mesh | ObjectType.PolysrfFilter),
			DeletedObjects = false,
			NormalObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			IncludePhantoms = false,
			LockedObjects = false,
			HiddenObjects = false
		};
		return RhinoDoc.ActiveDoc.Objects.GetObjectList(settings).ToList();
	}

	public static Mesh NetfabbRepairMesh(Mesh inputMesh, bool extendedRepair)
	{
		try
		{
			BoundingBox boundingBox = inputMesh.GetBoundingBox(accurate: true);
			boundingBox.MakeValid();
			Point3d center = boundingBox.Center;
			inputMesh.Transform(Transform.Scale(center, 3.0));
			RhinoApp.WriteLine("Repairing and Unifying Mesh...Please Wait");
			if (!NetfabbHelper.NetfabbBuildMesh(inputMesh, extendedRepair, out var meshRepaired))
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
			BoundingBox boundingBox2 = meshRepaired.GetBoundingBox(accurate: true);
			boundingBox2.MakeValid();
			_ = boundingBox2.Center;
			meshRepaired.Transform(Transform.Scale(center, 1.0 / 3.0));
			meshRepaired.Unweld(CSCoreMath.ToRadians(22.5), modifyNormals: true);
			RhinoApp.WriteLine((Math.Abs(meshRepaired.GetBoundingBox(accurate: true).Diagonal.Length - boundingBox.Diagonal.Length) < 0.001) ? "Bounding Boxes Match" : "Bounding Boxes DO NOT MATCH");
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
		try
		{
			double num = new Random().NextDouble() * Math.Abs(0.004) + 0.001;
			BoundingBox boundingBox = ro.Geometry.GetBoundingBox(accurate: true);
			boundingBox.MakeValid();
			return ro.Geometry.Transform(Transform.Scale(boundingBox.Center, 1.0 + num));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static bool RandomScaleRhinoObject(GeometryBase geo)
	{
		try
		{
			double num = new Random().NextDouble() * Math.Abs(0.004) + 0.001;
			BoundingBox boundingBox = geo.GetBoundingBox(accurate: true);
			boundingBox.MakeValid();
			return geo.Transform(Transform.Scale(boundingBox.Center, 1.0 + num));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh ReadBinarySTL(string filename)
	{
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
			Mesh mesh = new Mesh();
			for (int i = 84; i < num; i += 50)
			{
				float num2 = binaryReader.ReadSingle();
				float num3 = binaryReader.ReadSingle();
				float num4 = binaryReader.ReadSingle();
				float x = binaryReader.ReadSingle();
				float y = binaryReader.ReadSingle();
				float z = binaryReader.ReadSingle();
				float x2 = binaryReader.ReadSingle();
				float y2 = binaryReader.ReadSingle();
				float z2 = binaryReader.ReadSingle();
				float x3 = binaryReader.ReadSingle();
				float y3 = binaryReader.ReadSingle();
				float z3 = binaryReader.ReadSingle();
				binaryReader.ReadUInt16();
				int count = mesh.Vertices.Count;
				mesh.Vertices.SetVertex(count, x, y, z);
				mesh.Vertices.SetVertex(count + 1, x2, y2, z2);
				mesh.Vertices.SetVertex(count + 2, x3, y3, z3);
				int count2 = mesh.Faces.Count;
				mesh.Faces.SetFace(count2, new MeshFace(count, count + 1, count + 2));
				Vector3d normal = new Vector3d(num2, num3, num4);
				mesh.FaceNormals.AddFaceNormal(normal);
			}
			binaryReader.Close();
			binaryReader.Dispose();
			mesh.Normals.ComputeNormals();
			mesh.Compact();
			mesh.Unweld(0.0, modifyNormals: true);
			return mesh;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
			return null;
		}
	}

	public static Mesh RepairMesh(List<GeometryBase> lstGeometry, bool extendedRepair, bool prefixshells, MeshingParameters mp, int milgrainGridCount, bool optimizeMilgrain)
	{
		Mesh mesh = new Mesh();
		List<Brep> list = new List<Brep>();
		List<GeometryBase> list2 = new List<GeometryBase>();
		foreach (GeometryBase item2 in lstGeometry)
		{
			if (item2.ObjectType == ObjectType.Brep)
			{
				Brep brep = (Brep)item2;
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
				Mesh item = CreateMesh(item2, doCapPlanarHoles: true, mp, milgrainGridCount, optimizeMilgrain);
				list2.Add(item);
			}
		}
		if (list.Count > 0)
		{
			Brep[] collection = Brep.JoinBreps(list, 0.001);
			list2.AddRange(collection);
		}
		foreach (GeometryBase item3 in list2)
		{
			Mesh mesh2 = CreateMesh(item3, doCapPlanarHoles: true, mp, milgrainGridCount, optimizeMilgrain);
			if (mesh2 == null)
			{
				Guid objectId = RhinoDoc.ActiveDoc.Objects.Add(item3);
				RhinoDoc.ActiveDoc.Objects.UnselectAll();
				RhinoDoc.ActiveDoc.Objects.Select(objectId);
				RhinoApp.WriteLine("Could not create Mesh Object");
			}
			mesh2.Faces.ConvertQuadsToTriangles();
			if (mesh2.SolidOrientation() == -1)
			{
				mesh2.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
			}
			if (prefixshells)
			{
				Mesh mesh3 = mesh2.DuplicateMesh();
				if (!mesh2.IsClosed)
				{
					RhinoApp.CommandPrompt = "Prefixing Single Open Shell";
					mesh2 = NetfabbRepairMesh(mesh2, extendedRepair: true);
				}
				if (mesh2 == null)
				{
					mesh2 = mesh3.DuplicateMesh();
				}
			}
			mesh.Append(mesh2);
		}
		return FilterNoiseShells(NetfabbRepairMesh(mesh, extendedRepair));
	}

	public static bool SaveStlBinary(Mesh mesh, string filename, bool verbose)
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
				if (verbose)
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
		try
		{
			List<RhinoObject> list = new List<RhinoObject>();
			Mesh mesh = new Mesh();
			if (randomScale)
			{
				foreach (RhinoObject lstRhinoObject in lstRhinoObjects)
				{
					RandomScaleRhinoObject(lstRhinoObject);
					list.Add(lstRhinoObject);
				}
				mesh = GetMeshFromObjects(list, 0.002, 1, optimizeMilgrain: false);
			}
			else
			{
				mesh = GetMeshFromObjects(lstRhinoObjects, 0.002, 1, optimizeMilgrain: false);
			}
			mesh = NetfabbRepairMesh(mesh, extendedRepairInput);
			Mesh[] array = mesh.SplitDisjointPieces();
			int num = array.Count();
			Mesh mesh2 = new Mesh();
			int num2 = 1;
			Mesh[] array2 = array;
			foreach (Mesh mesh3 in array2)
			{
				int num3 = (num2 * 200 + num) / (num * 2);
				RhinoApp.CommandPrompt = $"Slicing Shell {num2} of {num} : {num3}%";
				BoundingBox boundingBox = mesh3.GetBoundingBox(accurate: true);
				boundingBox.MakeValid();
				Point3d[] corners = boundingBox.GetCorners();
				Point3d[] array3 = new LineCurve(corners[0], corners[4]).DivideEquidistant(sliceThickness);
				foreach (Point3d origin in array3)
				{
					Plane sectionPlane = new Plane(origin, Vector3d.ZAxis);
					CurveList curveList = new CurveList();
					Curve[] array4 = Mesh.CreateContourCurves(mesh3, sectionPlane);
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
						foreach (Mesh mesh4 in array9)
						{
							if (mesh4.IsValid)
							{
								Mesh other = mesh4.Offset(sliceThickness, solidify: true);
								mesh2.Append(other);
							}
						}
					}
				}
				num2++;
			}
			return mesh2;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh SliceMesh(IEnumerable<GeometryBase> lstGeometryBases, bool extendedRepairInput, double sliceThickness)
	{
		try
		{
			Mesh mesh = new Mesh();
			foreach (GeometryBase lstGeometryBasis in lstGeometryBases)
			{
				Mesh mesh2 = CreateMesh(lstGeometryBasis, doCapPlanarHoles: true, MeshingParameters.Coarse, 10, optimizeMilgrain: true);
				if (mesh2.IsValid)
				{
					mesh.Append(mesh2);
				}
			}
			mesh = NetfabbRepairMesh(mesh, extendedRepairInput);
			Mesh[] array = mesh.SplitDisjointPieces();
			int num = array.Count();
			Mesh mesh3 = new Mesh();
			int num2 = 1;
			Mesh[] array2 = array;
			foreach (Mesh mesh4 in array2)
			{
				int num3 = (num2 * 200 + num) / (num * 2);
				RhinoApp.CommandPrompt = $"Slicing Shell {num2} of {num} : {num3}%";
				BoundingBox boundingBox = mesh4.GetBoundingBox(accurate: true);
				boundingBox.MakeValid();
				Point3d[] corners = boundingBox.GetCorners();
				Point3d[] array3 = new LineCurve(corners[0], corners[4]).DivideEquidistant(sliceThickness);
				foreach (Point3d origin in array3)
				{
					Plane sectionPlane = new Plane(origin, Vector3d.ZAxis);
					CurveList curveList = new CurveList();
					Curve[] array4 = Mesh.CreateContourCurves(mesh4, sectionPlane);
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
						foreach (Mesh mesh5 in array9)
						{
							if (mesh5.IsValid)
							{
								Mesh other = mesh5.Offset(sliceThickness, solidify: true);
								mesh3.Append(other);
							}
						}
					}
				}
				num2++;
			}
			return mesh3;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Mesh SliceMesh(Mesh inputMesh, double sliceThickness)
	{
		Mesh[] array = inputMesh.SplitDisjointPieces();
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
						else if (curve.IsClosed)
						{
							curveList.Add(curve);
						}
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
			Mesh mesh = new Mesh();
			for (int i = 84; i < num; i += 50)
			{
				float num2 = binaryReader.ReadSingle();
				float num3 = binaryReader.ReadSingle();
				float num4 = binaryReader.ReadSingle();
				float x = binaryReader.ReadSingle();
				float y = binaryReader.ReadSingle();
				float z = binaryReader.ReadSingle();
				float x2 = binaryReader.ReadSingle();
				float y2 = binaryReader.ReadSingle();
				float z2 = binaryReader.ReadSingle();
				float x3 = binaryReader.ReadSingle();
				float y3 = binaryReader.ReadSingle();
				float z3 = binaryReader.ReadSingle();
				binaryReader.ReadUInt16();
				int count = mesh.Vertices.Count;
				mesh.Vertices.SetVertex(count, x, y, z);
				mesh.Vertices.SetVertex(count + 1, x2, y2, z2);
				mesh.Vertices.SetVertex(count + 2, x3, y3, z3);
				int count2 = mesh.Faces.Count;
				mesh.Faces.SetFace(count2, new MeshFace(count, count + 1, count + 2));
				Vector3d normal = new Vector3d(num2, num3, num4);
				mesh.FaceNormals.AddFaceNormal(normal);
			}
			binaryWriter.Flush();
			binaryWriter.Close();
			mesh.Normals.ComputeNormals();
			mesh.Compact();
			mesh.Unweld(0.0, modifyNormals: true);
			return mesh;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
			return null;
		}
	}

	public static bool WriteEncryptedBinaryStl(Mesh mesh, string filename, string headerText, bool verbose)
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
				int num = mesh.Faces.Count();
				binaryWriter.Write(num);
				if (verbose)
				{
					RhinoApp.WriteLine("Writing Encrypted STL File: Face Count {0}", num);
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
