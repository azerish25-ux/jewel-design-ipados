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
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		if (mesh == null)
		{
			return;
		}
		mesh.CombineIdenticalVertices(true);
		for (int i = 0; i < mesh.m_F.Count(); i++)
		{
			IOnMeshFace val = (IOnMeshFace)(object)mesh.m_F[i];
			int num = (val.IsQuad() ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				IOn3fPoint val2 = (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val[j]];
				if (double.IsNaN(val2.x) || double.IsNaN(val2.y) || double.IsNaN(val2.z))
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
			int num2 = ((ArrayOn3fPoint)mesh.m_V).Count();
			binaryWriter.Write(num2);
			for (int k = 0; k < num2; k++)
			{
				On3fPoint obj = ((ArrayOn3fPoint)mesh.m_V)[k];
				double.IsNaN(((IOn3fPoint)obj).x);
				float num3 = ((IOn3fPoint)obj).x;
				float num4 = ((IOn3fPoint)obj).y;
				float num5 = ((IOn3fPoint)obj).z;
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
				IOnMeshFace val3 = (IOnMeshFace)(object)mesh.m_F[m];
				if (val3.IsQuad())
				{
					double num7 = 1.0;
					On3dVector quadNormal = GetQuadNormal((IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[0]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[1]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[2]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[3]]);
					if (quadNormal != (IOn3dVector)null)
					{
						On3dVector val4 = new On3dVector();
						val4.x = ((ArrayOn3fVector)mesh.m_N)[val3[0]].x + ((ArrayOn3fVector)mesh.m_N)[val3[1]].x + ((ArrayOn3fVector)mesh.m_N)[val3[2]].x + ((ArrayOn3fVector)mesh.m_N)[val3[3]].x;
						val4.y = ((ArrayOn3fVector)mesh.m_N)[val3[0]].y + ((ArrayOn3fVector)mesh.m_N)[val3[1]].y + ((ArrayOn3fVector)mesh.m_N)[val3[2]].y + ((ArrayOn3fVector)mesh.m_N)[val3[3]].y;
						val4.z = ((ArrayOn3fVector)mesh.m_N)[val3[0]].z + ((ArrayOn3fVector)mesh.m_N)[val3[1]].z + ((ArrayOn3fVector)mesh.m_N)[val3[2]].z + ((ArrayOn3fVector)mesh.m_N)[val3[3]].z;
						val4.Unitize();
						num7 = OnUtil.ON_DotProduct((IOn3dVector)(object)quadNormal, (IOn3dVector)(object)val4);
					}
					if (num7 >= 0.0)
					{
						binaryWriter.Write(val3[0] + 1);
						binaryWriter.Write(val3[1] + 1);
						binaryWriter.Write(val3[2] + 1);
						binaryWriter.Write(val3[3] + 1);
					}
					else
					{
						binaryWriter.Write(val3[3] + 1);
						binaryWriter.Write(val3[2] + 1);
						binaryWriter.Write(val3[1] + 1);
						binaryWriter.Write(val3[0] + 1);
					}
				}
				else
				{
					double num8 = 1.0;
					On3dVector direction = GetDirection((IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[0]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[1]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[2]]);
					if (direction != (IOn3dVector)null)
					{
						On3dVector val5 = new On3dVector();
						val5.x = ((ArrayOn3fVector)mesh.m_N)[val3[0]].x + ((ArrayOn3fVector)mesh.m_N)[val3[1]].x + ((ArrayOn3fVector)mesh.m_N)[val3[2]].x;
						val5.y = ((ArrayOn3fVector)mesh.m_N)[val3[0]].y + ((ArrayOn3fVector)mesh.m_N)[val3[1]].y + ((ArrayOn3fVector)mesh.m_N)[val3[2]].y;
						val5.z = ((ArrayOn3fVector)mesh.m_N)[val3[0]].z + ((ArrayOn3fVector)mesh.m_N)[val3[1]].z + ((ArrayOn3fVector)mesh.m_N)[val3[2]].z;
						val5.Unitize();
						num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)direction, (IOn3dVector)(object)val5);
					}
					if (num8 >= 0.0)
					{
						binaryWriter.Write(val3[0] + 1);
						binaryWriter.Write(val3[1] + 1);
						binaryWriter.Write(val3[2] + 1);
					}
					else
					{
						binaryWriter.Write(val3[2] + 1);
						binaryWriter.Write(val3[1] + 1);
						binaryWriter.Write(val3[0] + 1);
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
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		if (mesh == null)
		{
			return;
		}
		for (int i = 0; i < mesh.m_F.Count(); i++)
		{
			IOnMeshFace val = (IOnMeshFace)(object)mesh.m_F[i];
			int num = (val.IsQuad() ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				IOn3fPoint val2 = (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val[j]];
				if (double.IsNaN(val2.x) || double.IsNaN(val2.y) || double.IsNaN(val2.z))
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
		int num2 = ((ArrayOn3fPoint)mesh.m_V).Count();
		for (int k = 0; k < num2; k++)
		{
			On3fPoint obj = ((ArrayOn3fPoint)mesh.m_V)[k];
			double.IsNaN(((IOn3fPoint)obj).x);
			double num3 = Math.Round(((IOn3fPoint)obj).x, 6);
			double num4 = Math.Round(((IOn3fPoint)obj).y, 6);
			double num5 = Math.Round(((IOn3fPoint)obj).z, 6);
			string value = $"v {num3:0.######} {num4:0.######} {num5:0.######}";
			textWriter.WriteLine(value);
		}
		textWriter.WriteLine("g Designer_object");
		int num6 = mesh.m_F.Count();
		for (int l = 0; l < num6; l++)
		{
			IOnMeshFace val3 = (IOnMeshFace)(object)mesh.m_F[l];
			string value2;
			if (val3.IsQuad())
			{
				double num7 = 1.0;
				On3dVector quadNormal = GetQuadNormal((IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[0]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[1]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[2]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[3]]);
				if (quadNormal != (IOn3dVector)null)
				{
					On3dVector val4 = new On3dVector();
					val4.x = ((ArrayOn3fVector)mesh.m_N)[val3[0]].x + ((ArrayOn3fVector)mesh.m_N)[val3[1]].x + ((ArrayOn3fVector)mesh.m_N)[val3[2]].x + ((ArrayOn3fVector)mesh.m_N)[val3[3]].x;
					val4.y = ((ArrayOn3fVector)mesh.m_N)[val3[0]].y + ((ArrayOn3fVector)mesh.m_N)[val3[1]].y + ((ArrayOn3fVector)mesh.m_N)[val3[2]].y + ((ArrayOn3fVector)mesh.m_N)[val3[3]].y;
					val4.z = ((ArrayOn3fVector)mesh.m_N)[val3[0]].z + ((ArrayOn3fVector)mesh.m_N)[val3[1]].z + ((ArrayOn3fVector)mesh.m_N)[val3[2]].z + ((ArrayOn3fVector)mesh.m_N)[val3[3]].z;
					val4.Unitize();
					num7 = OnUtil.ON_DotProduct((IOn3dVector)(object)quadNormal, (IOn3dVector)(object)val4);
				}
				value2 = ((!(num7 >= 0.0)) ? $"f {val3[3] + 1} {val3[2] + 1} {val3[1] + 1} {val3[0] + 1}" : $"f {val3[0] + 1} {val3[1] + 1} {val3[2] + 1} {val3[3] + 1}");
			}
			else
			{
				double num8 = 1.0;
				On3dVector direction = GetDirection((IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[0]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[1]], (IOn3fPoint)(object)((ArrayOn3fPoint)mesh.m_V)[val3[2]]);
				if (direction != (IOn3dVector)null)
				{
					On3dVector val5 = new On3dVector();
					val5.x = ((ArrayOn3fVector)mesh.m_N)[val3[0]].x + ((ArrayOn3fVector)mesh.m_N)[val3[1]].x + ((ArrayOn3fVector)mesh.m_N)[val3[2]].x;
					val5.y = ((ArrayOn3fVector)mesh.m_N)[val3[0]].y + ((ArrayOn3fVector)mesh.m_N)[val3[1]].y + ((ArrayOn3fVector)mesh.m_N)[val3[2]].y;
					val5.z = ((ArrayOn3fVector)mesh.m_N)[val3[0]].z + ((ArrayOn3fVector)mesh.m_N)[val3[1]].z + ((ArrayOn3fVector)mesh.m_N)[val3[2]].z;
					val5.Unitize();
					num8 = OnUtil.ON_DotProduct((IOn3dVector)(object)direction, (IOn3dVector)(object)val5);
				}
				value2 = ((!(num8 >= 0.0)) ? $"f {val3[2] + 1} {val3[1] + 1} {val3[0] + 1}" : $"f {val3[0] + 1} {val3[1] + 1} {val3[2] + 1}");
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
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		On3dVector val = new On3dVector((double)(p1.x - p0.x), (double)(p1.y - p0.y), (double)(p1.z - p0.z));
		if (val.Length() < 0.0001)
		{
			return null;
		}
		On3dVector val2 = new On3dVector((double)(p2.x - p0.x), (double)(p2.y - p0.y), (double)(p2.z - p0.z));
		if (val2.Length() < 0.0001)
		{
			return null;
		}
		On3dVector obj = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		obj.Unitize();
		return obj;
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
		if (direction != (IOn3dVector)null)
		{
			return direction;
		}
		direction = GetDirection(p1, p2, p3);
		if (direction != (IOn3dVector)null)
		{
			return direction;
		}
		direction = GetDirection(p2, p3, p0);
		if (direction != (IOn3dVector)null)
		{
			return direction;
		}
		direction = GetDirection(p3, p0, p1);
		if (direction != (IOn3dVector)null)
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
