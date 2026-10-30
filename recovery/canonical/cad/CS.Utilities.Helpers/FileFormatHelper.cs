using System;
using System.IO;
using System.Text;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.Geometry;

namespace CS.Utilities.Helpers;

public static class FileFormatHelper
{
	public static Mesh ReadObj(string filename)
	{
		Mesh mesh = new Mesh();
		using StreamReader streamReader = new StreamReader(filename);
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			string[] array = text.Split(' ');
			if (array[0] == "v")
			{
				double x = Convert.ToDouble(array[1]);
				double y = Convert.ToDouble(array[2]);
				double z = Convert.ToDouble(array[3]);
				mesh.Vertices.Add(x, y, z);
			}
			else if (array[0] == "f")
			{
				int vertex = Convert.ToInt32(array[1]) - 1;
				int vertex2 = Convert.ToInt32(array[2]) - 1;
				int vertex3 = Convert.ToInt32(array[3]) - 1;
				if (array.Length == 4)
				{
					mesh.Faces.AddFace(vertex, vertex2, vertex3);
					continue;
				}
				int vertex4 = Convert.ToInt32(array[4]) - 1;
				mesh.Faces.AddFace(vertex, vertex2, vertex3, vertex4);
			}
		}
		return mesh;
	}

	public static void WriteBinObj(OnMesh mesh, Stream sOut)
	{
		if (mesh == null)
		{
			return;
		}
		mesh.CombineIdenticalVertices(bIgnoreVertexNormals: true);
		for (int i = 0; i < mesh.m_F.Count(); i++)
		{
			IOnMeshFace onMeshFace = mesh.m_F[i];
			int num = (onMeshFace.IsQuad() ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				IOn3fPoint on3fPoint = mesh.m_V[onMeshFace.get_vi(j)];
				if (double.IsNaN(on3fPoint.x) || double.IsNaN(on3fPoint.y) || double.IsNaN(on3fPoint.z))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				mesh.DeleteFace(i);
				i--;
			}
		}
		mesh.CullUnusedVertices();
		DateTime now = DateTime.Now;
		RhUtil.RhinoApp().Print("Starting saving local Mesh file\n");
		using (BinaryWriter binaryWriter = new BinaryWriter(sOut, Encoding.UTF8, leaveOpen: true))
		{
			binaryWriter.Write((byte)1);
			int num2 = mesh.m_V.Count();
			binaryWriter.Write(num2);
			for (int k = 0; k < num2; k++)
			{
				On3fPoint on3fPoint2 = mesh.m_V[k];
				double.IsNaN(((IOn3fPoint)on3fPoint2).x);
				float num3 = ((IOn3fPoint)on3fPoint2).x;
				float num4 = ((IOn3fPoint)on3fPoint2).y;
				float num5 = ((IOn3fPoint)on3fPoint2).z;
				if (double.IsNaN(num3))
				{
					num3 = 0f;
				}
				if (double.IsNaN(num4))
				{
					num4 = 0f;
				}
				if (double.IsNaN(num5))
				{
					num5 = 0f;
				}
				binaryWriter.Write(num3);
				binaryWriter.Write(num4);
				binaryWriter.Write(num5);
			}
			int num6 = mesh.m_F.Count();
			binaryWriter.Write(num6);
			byte b = 1;
			byte b2 = 0;
			for (int l = 0; l < num6; l++)
			{
				if (((IOnMeshFace)mesh.m_F[l]).IsQuad())
				{
					b2 |= b;
				}
				b <<= 1;
				if (b == 0)
				{
					binaryWriter.Write(b2);
					b2 = 0;
					b = 1;
				}
			}
			if (b != 1)
			{
				binaryWriter.Write(b2);
			}
			for (int m = 0; m < num6; m++)
			{
				IOnMeshFace onMeshFace2 = mesh.m_F[m];
				if (onMeshFace2.IsQuad())
				{
					double num7 = 1.0;
					On3dVector quadNormal = GetQuadNormal(mesh.m_V[onMeshFace2.get_vi(0)], mesh.m_V[onMeshFace2.get_vi(1)], mesh.m_V[onMeshFace2.get_vi(2)], mesh.m_V[onMeshFace2.get_vi(3)]);
					if (quadNormal != null)
					{
						On3dVector on3dVector = new On3dVector();
						on3dVector.x = mesh.m_N[onMeshFace2.get_vi(0)].x + mesh.m_N[onMeshFace2.get_vi(1)].x + mesh.m_N[onMeshFace2.get_vi(2)].x + mesh.m_N[onMeshFace2.get_vi(3)].x;
						on3dVector.y = mesh.m_N[onMeshFace2.get_vi(0)].y + mesh.m_N[onMeshFace2.get_vi(1)].y + mesh.m_N[onMeshFace2.get_vi(2)].y + mesh.m_N[onMeshFace2.get_vi(3)].y;
						on3dVector.z = mesh.m_N[onMeshFace2.get_vi(0)].z + mesh.m_N[onMeshFace2.get_vi(1)].z + mesh.m_N[onMeshFace2.get_vi(2)].z + mesh.m_N[onMeshFace2.get_vi(3)].z;
						on3dVector.Unitize();
						num7 = OnUtil.ON_DotProduct(quadNormal, on3dVector);
					}
					if (num7 >= 0.0)
					{
						binaryWriter.Write(onMeshFace2.get_vi(0) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(1) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(2) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(3) + 1);
					}
					else
					{
						binaryWriter.Write(onMeshFace2.get_vi(3) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(2) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(1) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(0) + 1);
					}
				}
				else
				{
					double num8 = 1.0;
					On3dVector direction = GetDirection(mesh.m_V[onMeshFace2.get_vi(0)], mesh.m_V[onMeshFace2.get_vi(1)], mesh.m_V[onMeshFace2.get_vi(2)]);
					if (direction != null)
					{
						On3dVector on3dVector2 = new On3dVector();
						on3dVector2.x = mesh.m_N[onMeshFace2.get_vi(0)].x + mesh.m_N[onMeshFace2.get_vi(1)].x + mesh.m_N[onMeshFace2.get_vi(2)].x;
						on3dVector2.y = mesh.m_N[onMeshFace2.get_vi(0)].y + mesh.m_N[onMeshFace2.get_vi(1)].y + mesh.m_N[onMeshFace2.get_vi(2)].y;
						on3dVector2.z = mesh.m_N[onMeshFace2.get_vi(0)].z + mesh.m_N[onMeshFace2.get_vi(1)].z + mesh.m_N[onMeshFace2.get_vi(2)].z;
						on3dVector2.Unitize();
						num8 = OnUtil.ON_DotProduct(direction, on3dVector2);
					}
					if (num8 >= 0.0)
					{
						binaryWriter.Write(onMeshFace2.get_vi(0) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(1) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(2) + 1);
					}
					else
					{
						binaryWriter.Write(onMeshFace2.get_vi(2) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(1) + 1);
						binaryWriter.Write(onMeshFace2.get_vi(0) + 1);
					}
				}
			}
		}
		DateTime now2 = DateTime.Now;
		RhUtil.RhinoApp().Print("Saving local Mesh finished after " + (now2 - now).Seconds + " seconds\n");
		DateTime now3 = DateTime.Now;
		RhUtil.RhinoApp().Print("Saving remote Mesh finished after " + (now3 - now2).Seconds + " seconds\n");
	}

	public static void WriteBinObj(Mesh mesh, Stream sOut)
	{
		if (mesh == null)
		{
			return;
		}
		mesh.Vertices.CombineIdentical(ignoreNormals: true, ignoreAdditional: false);
		for (int i = 0; i < mesh.Faces.Count; i++)
		{
			MeshFace meshFace = mesh.Faces[i];
			int num = (meshFace.IsQuad ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				Point3f point3f = mesh.Vertices[meshFace[j]];
				if (double.IsNaN(point3f.X) || double.IsNaN(point3f.Y) || double.IsNaN(point3f.Z))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				mesh.Faces.RemoveAt(i);
				i--;
			}
		}
		mesh.Vertices.CullUnused();
		DateTime now = DateTime.Now;
		RhinoApp.WriteLine("Starting saving local Mesh file");
		using (BinaryWriter binaryWriter = new BinaryWriter(sOut, Encoding.UTF8, leaveOpen: true))
		{
			binaryWriter.Write((byte)1);
			int count = mesh.Vertices.Count;
			binaryWriter.Write(count);
			for (int k = 0; k < count; k++)
			{
				Point3f point3f2 = mesh.Vertices[k];
				double.IsNaN(point3f2.X);
				float num2 = point3f2.X;
				float num3 = point3f2.Y;
				float num4 = point3f2.Z;
				if (double.IsNaN(num2))
				{
					num2 = 0f;
				}
				if (double.IsNaN(num3))
				{
					num3 = 0f;
				}
				if (double.IsNaN(num4))
				{
					num4 = 0f;
				}
				binaryWriter.Write(num2);
				binaryWriter.Write(num3);
				binaryWriter.Write(num4);
			}
			int count2 = mesh.Faces.Count;
			binaryWriter.Write(count2);
			byte b = 1;
			byte b2 = 0;
			for (int l = 0; l < count2; l++)
			{
				if (mesh.Faces[l].IsQuad)
				{
					b2 |= b;
				}
				b <<= 1;
				if (b == 0)
				{
					binaryWriter.Write(b2);
					b2 = 0;
					b = 1;
				}
			}
			if (b != 1)
			{
				binaryWriter.Write(b2);
			}
			for (int m = 0; m < count2; m++)
			{
				MeshFace meshFace2 = mesh.Faces[m];
				if (meshFace2.IsQuad)
				{
					double num5 = 1.0;
					Vector3d quadNormal = GetQuadNormal(mesh.Vertices[meshFace2[0]], mesh.Vertices[meshFace2[1]], mesh.Vertices[meshFace2[2]], mesh.Vertices[meshFace2[3]]);
					if (quadNormal.IsValid)
					{
						Vector3d vector3d = new Vector3d
						{
							X = mesh.Normals[meshFace2[0]].X + mesh.Normals[meshFace2[1]].X + mesh.Normals[meshFace2[2]].X + mesh.Normals[meshFace2[3]].X,
							Y = mesh.Normals[meshFace2[0]].Y + mesh.Normals[meshFace2[1]].Y + mesh.Normals[meshFace2[2]].Y + mesh.Normals[meshFace2[3]].Y,
							Z = mesh.Normals[meshFace2[0]].Z + mesh.Normals[meshFace2[1]].Z + mesh.Normals[meshFace2[2]].Z + mesh.Normals[meshFace2[3]].Z
						};
						vector3d.Unitize();
						num5 = quadNormal * vector3d;
					}
					if (num5 >= 0.0)
					{
						binaryWriter.Write(meshFace2[0] + 1);
						binaryWriter.Write(meshFace2[1] + 1);
						binaryWriter.Write(meshFace2[2] + 1);
						binaryWriter.Write(meshFace2[3] + 1);
					}
					else
					{
						binaryWriter.Write(meshFace2[3] + 1);
						binaryWriter.Write(meshFace2[2] + 1);
						binaryWriter.Write(meshFace2[1] + 1);
						binaryWriter.Write(meshFace2[0] + 1);
					}
				}
				else
				{
					double num6 = 1.0;
					Vector3d direction = GetDirection(mesh.Vertices[meshFace2[0]], mesh.Vertices[meshFace2[1]], mesh.Vertices[meshFace2[2]]);
					if (direction.IsValid)
					{
						Vector3d vector3d2 = new Vector3d
						{
							X = mesh.Normals[meshFace2[0]].X + mesh.Normals[meshFace2[1]].X + mesh.Normals[meshFace2[2]].X,
							Y = mesh.Normals[meshFace2[0]].Y + mesh.Normals[meshFace2[1]].Y + mesh.Normals[meshFace2[2]].Y,
							Z = mesh.Normals[meshFace2[0]].Z + mesh.Normals[meshFace2[1]].Z + mesh.Normals[meshFace2[2]].Z
						};
						vector3d2.Unitize();
						num6 = direction * vector3d2;
					}
					if (num6 >= 0.0)
					{
						binaryWriter.Write(meshFace2[0] + 1);
						binaryWriter.Write(meshFace2[1] + 1);
						binaryWriter.Write(meshFace2[2] + 1);
					}
					else
					{
						binaryWriter.Write(meshFace2[2] + 1);
						binaryWriter.Write(meshFace2[1] + 1);
						binaryWriter.Write(meshFace2[0] + 1);
					}
				}
			}
		}
		DateTime now2 = DateTime.Now;
		RhinoApp.WriteLine("Saving local Mesh finished after " + (now2 - now).Seconds + " seconds");
		DateTime now3 = DateTime.Now;
		RhinoApp.WriteLine("Saving remote Mesh finished after " + (now3 - now2).Seconds + " seconds");
	}

	public static void WriteObj(OnMesh mesh, string filename)
	{
		if (mesh == null)
		{
			return;
		}
		for (int i = 0; i < mesh.m_F.Count(); i++)
		{
			IOnMeshFace onMeshFace = mesh.m_F[i];
			int num = (onMeshFace.IsQuad() ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				IOn3fPoint on3fPoint = mesh.m_V[onMeshFace.get_vi(j)];
				if (double.IsNaN(on3fPoint.x) || double.IsNaN(on3fPoint.y) || double.IsNaN(on3fPoint.z))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				mesh.DeleteFace(i);
				i--;
			}
		}
		mesh.CullUnusedVertices();
		string tempPath = FileSystemHelper.GetTempPath(Path.GetExtension(filename));
		RhUtil.RhinoApp().Print("Saving to file " + filename + "\n");
		RhUtil.RhinoApp().Print("Saving to temp file " + tempPath + "\n");
		DateTime now = DateTime.Now;
		RhUtil.RhinoApp().Print("Starting saving local Mesh file\n");
		using TextWriter textWriter = new StreamWriter(tempPath);
		int num2 = mesh.m_V.Count();
		for (int k = 0; k < num2; k++)
		{
			On3fPoint on3fPoint2 = mesh.m_V[k];
			double.IsNaN(((IOn3fPoint)on3fPoint2).x);
			double num3 = Math.Round(((IOn3fPoint)on3fPoint2).x, 6);
			double num4 = Math.Round(((IOn3fPoint)on3fPoint2).y, 6);
			double num5 = Math.Round(((IOn3fPoint)on3fPoint2).z, 6);
			string value = $"v {num3:0.######} {num4:0.######} {num5:0.######}";
			textWriter.WriteLine(value);
		}
		textWriter.WriteLine("g Designer_object");
		int num6 = mesh.m_F.Count();
		for (int l = 0; l < num6; l++)
		{
			IOnMeshFace onMeshFace2 = mesh.m_F[l];
			string value2;
			if (onMeshFace2.IsQuad())
			{
				double num7 = 1.0;
				On3dVector quadNormal = GetQuadNormal(mesh.m_V[onMeshFace2.get_vi(0)], mesh.m_V[onMeshFace2.get_vi(1)], mesh.m_V[onMeshFace2.get_vi(2)], mesh.m_V[onMeshFace2.get_vi(3)]);
				if (quadNormal != null)
				{
					On3dVector on3dVector = new On3dVector();
					on3dVector.x = mesh.m_N[onMeshFace2.get_vi(0)].x + mesh.m_N[onMeshFace2.get_vi(1)].x + mesh.m_N[onMeshFace2.get_vi(2)].x + mesh.m_N[onMeshFace2.get_vi(3)].x;
					on3dVector.y = mesh.m_N[onMeshFace2.get_vi(0)].y + mesh.m_N[onMeshFace2.get_vi(1)].y + mesh.m_N[onMeshFace2.get_vi(2)].y + mesh.m_N[onMeshFace2.get_vi(3)].y;
					on3dVector.z = mesh.m_N[onMeshFace2.get_vi(0)].z + mesh.m_N[onMeshFace2.get_vi(1)].z + mesh.m_N[onMeshFace2.get_vi(2)].z + mesh.m_N[onMeshFace2.get_vi(3)].z;
					on3dVector.Unitize();
					num7 = OnUtil.ON_DotProduct(quadNormal, on3dVector);
				}
				value2 = ((!(num7 >= 0.0)) ? $"f {onMeshFace2.get_vi(3) + 1} {onMeshFace2.get_vi(2) + 1} {onMeshFace2.get_vi(1) + 1} {onMeshFace2.get_vi(0) + 1}" : $"f {onMeshFace2.get_vi(0) + 1} {onMeshFace2.get_vi(1) + 1} {onMeshFace2.get_vi(2) + 1} {onMeshFace2.get_vi(3) + 1}");
			}
			else
			{
				double num8 = 1.0;
				On3dVector direction = GetDirection(mesh.m_V[onMeshFace2.get_vi(0)], mesh.m_V[onMeshFace2.get_vi(1)], mesh.m_V[onMeshFace2.get_vi(2)]);
				if (direction != null)
				{
					On3dVector on3dVector2 = new On3dVector();
					on3dVector2.x = mesh.m_N[onMeshFace2.get_vi(0)].x + mesh.m_N[onMeshFace2.get_vi(1)].x + mesh.m_N[onMeshFace2.get_vi(2)].x;
					on3dVector2.y = mesh.m_N[onMeshFace2.get_vi(0)].y + mesh.m_N[onMeshFace2.get_vi(1)].y + mesh.m_N[onMeshFace2.get_vi(2)].y;
					on3dVector2.z = mesh.m_N[onMeshFace2.get_vi(0)].z + mesh.m_N[onMeshFace2.get_vi(1)].z + mesh.m_N[onMeshFace2.get_vi(2)].z;
					on3dVector2.Unitize();
					num8 = OnUtil.ON_DotProduct(direction, on3dVector2);
				}
				value2 = ((!(num8 >= 0.0)) ? $"f {onMeshFace2.get_vi(2) + 1} {onMeshFace2.get_vi(1) + 1} {onMeshFace2.get_vi(0) + 1}" : $"f {onMeshFace2.get_vi(0) + 1} {onMeshFace2.get_vi(1) + 1} {onMeshFace2.get_vi(2) + 1}");
			}
			textWriter.WriteLine(value2);
		}
		textWriter.Flush();
		textWriter.Close();
		DateTime now2 = DateTime.Now;
		RhUtil.RhinoApp().Print("Saving local Mesh finished after " + (now2 - now).Seconds + " seconds\n");
		FileSystemHelper.CopyFile(tempPath, filename);
		DateTime now3 = DateTime.Now;
		RhUtil.RhinoApp().Print("Saving remote Mesh finished after " + (now3 - now2).Seconds + " seconds\n");
		if (!FileSystemHelper.DeleteFile(tempPath))
		{
			RhUtil.RhinoApp().Print("Error deleting file " + tempPath);
		}
	}

	public static void WriteObj(Mesh mesh, string filename)
	{
		if (mesh == null)
		{
			return;
		}
		for (int i = 0; i < mesh.Faces.Count; i++)
		{
			MeshFace meshFace = mesh.Faces[i];
			int num = (meshFace.IsQuad ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				Point3f point3f = mesh.Vertices[meshFace[j]];
				if (double.IsNaN(point3f.X) || double.IsNaN(point3f.Y) || double.IsNaN(point3f.Z))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				mesh.Faces.RemoveAt(i);
				i--;
			}
		}
		mesh.Vertices.CullUnused();
		string tempPath = FileSystemHelper.GetTempPath(Path.GetExtension(filename));
		RhinoApp.WriteLine("Saving to file " + filename);
		RhinoApp.WriteLine("Saving to temp file " + tempPath);
		DateTime now = DateTime.Now;
		RhinoApp.WriteLine("Starting saving local Mesh file");
		using TextWriter textWriter = new StreamWriter(tempPath);
		int count = mesh.Vertices.Count;
		for (int k = 0; k < count; k++)
		{
			Point3f point3f2 = mesh.Vertices[k];
			double.IsNaN(point3f2.X);
			double num2 = Math.Round(point3f2.X, 6);
			double num3 = Math.Round(point3f2.Y, 6);
			double num4 = Math.Round(point3f2.Z, 6);
			string value = $"v {num2:0.######} {num3:0.######} {num4:0.######}";
			textWriter.WriteLine(value);
		}
		textWriter.WriteLine("g Designer_object");
		int count2 = mesh.Faces.Count;
		for (int l = 0; l < count2; l++)
		{
			MeshFace meshFace2 = mesh.Faces[l];
			string value2;
			if (meshFace2.IsQuad)
			{
				double num5 = 1.0;
				Vector3d quadNormal = GetQuadNormal(mesh.Vertices[meshFace2[0]], mesh.Vertices[meshFace2[1]], mesh.Vertices[meshFace2[2]], mesh.Vertices[meshFace2[3]]);
				if (!quadNormal.IsValid)
				{
					Vector3d vector3d = new Vector3d
					{
						X = mesh.Normals[meshFace2[0]].X + mesh.Normals[meshFace2[1]].X + mesh.Normals[meshFace2[2]].X + mesh.Normals[meshFace2[3]].X,
						Y = mesh.Normals[meshFace2[0]].Y + mesh.Normals[meshFace2[1]].Y + mesh.Normals[meshFace2[2]].Y + mesh.Normals[meshFace2[3]].Y,
						Z = mesh.Normals[meshFace2[0]].Z + mesh.Normals[meshFace2[1]].Z + mesh.Normals[meshFace2[2]].Z + mesh.Normals[meshFace2[3]].Z
					};
					vector3d.Unitize();
					num5 = quadNormal * vector3d;
				}
				value2 = ((num5 >= 0.0) ? $"f {meshFace2[0] + 1} {meshFace2[1] + 1} {meshFace2[2] + 1} {meshFace2[3] + 1}" : $"f {meshFace2[3] + 1} {meshFace2[2] + 1} {meshFace2[1] + 1} {meshFace2[0] + 1}");
			}
			else
			{
				double num6 = 1.0;
				Vector3d direction = GetDirection(mesh.Vertices[meshFace2[0]], mesh.Vertices[meshFace2[1]], mesh.Vertices[meshFace2[2]]);
				if (!direction.IsValid)
				{
					Vector3d vector3d2 = new Vector3d
					{
						X = mesh.Normals[meshFace2[0]].X + mesh.Normals[meshFace2[1]].X + mesh.Normals[meshFace2[2]].X,
						Y = mesh.Normals[meshFace2[0]].Y + mesh.Normals[meshFace2[1]].Y + mesh.Normals[meshFace2[2]].Y,
						Z = mesh.Normals[meshFace2[0]].Z + mesh.Normals[meshFace2[1]].Z + mesh.Normals[meshFace2[2]].Z
					};
					vector3d2.Unitize();
					num6 = direction * vector3d2;
				}
				value2 = ((num6 >= 0.0) ? $"f {meshFace2[0] + 1} {meshFace2[1] + 1} {meshFace2[2] + 1}" : $"f {meshFace2[2] + 1} {meshFace2[1] + 1} {meshFace2[0] + 1}");
			}
			textWriter.WriteLine(value2);
		}
		textWriter.Flush();
		textWriter.Close();
		DateTime now2 = DateTime.Now;
		RhinoApp.WriteLine("Saving local Mesh finished after " + (now2 - now).Seconds + " seconds");
		FileSystemHelper.CopyFile(tempPath, filename);
		DateTime now3 = DateTime.Now;
		RhinoApp.WriteLine("Saving remote Mesh finished after " + (now3 - now2).Seconds + " seconds");
		if (!FileSystemHelper.DeleteFile(tempPath))
		{
			RhinoApp.WriteLine("Error deleting file " + tempPath);
		}
	}

	private static On3dVector GetDirection(IOn3fPoint p0, IOn3fPoint p1, IOn3fPoint p2)
	{
		On3dVector on3dVector = new On3dVector(p1.x - p0.x, p1.y - p0.y, p1.z - p0.z);
		if (on3dVector.Length() < 0.0001)
		{
			return null;
		}
		On3dVector on3dVector2 = new On3dVector(p2.x - p0.x, p2.y - p0.y, p2.z - p0.z);
		if (on3dVector2.Length() < 0.0001)
		{
			return null;
		}
		On3dVector on3dVector3 = OnUtil.ON_CrossProduct(on3dVector, on3dVector2);
		on3dVector3.Unitize();
		return on3dVector3;
	}

	private static Vector3d GetDirection(Point3f p0, Point3f p1, Point3f p2)
	{
		Vector3d a = new Vector3d(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
		if (a.Length < 0.0001)
		{
			return Vector3d.Unset;
		}
		Vector3d b = new Vector3d(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
		if (b.Length < 0.0001)
		{
			return Vector3d.Unset;
		}
		Vector3d result = Vector3d.CrossProduct(a, b);
		result.Unitize();
		return result;
	}

	private static On3dVector GetQuadNormal(IOn3fPoint p0, IOn3fPoint p1, IOn3fPoint p2, IOn3fPoint p3)
	{
		On3dVector direction = GetDirection(p0, p1, p2);
		if (direction != null)
		{
			return direction;
		}
		direction = GetDirection(p1, p2, p3);
		if (direction != null)
		{
			return direction;
		}
		direction = GetDirection(p2, p3, p0);
		if (direction != null)
		{
			return direction;
		}
		direction = GetDirection(p3, p0, p1);
		if (direction != null)
		{
			return direction;
		}
		return null;
	}

	private static Vector3d GetQuadNormal(Point3f p0, Point3f p1, Point3f p2, Point3f p3)
	{
		Vector3d direction = GetDirection(p0, p1, p2);
		if (!direction.IsValid)
		{
			return direction;
		}
		direction = GetDirection(p1, p2, p3);
		if (!direction.IsValid)
		{
			return direction;
		}
		direction = GetDirection(p2, p3, p0);
		if (!direction.IsValid)
		{
			return direction;
		}
		direction = GetDirection(p3, p0, p1);
		if (!direction.IsValid)
		{
			return direction;
		}
		return Vector3d.Unset;
	}
}
