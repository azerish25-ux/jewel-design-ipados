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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		Mesh val = new Mesh();
		using StreamReader streamReader = new StreamReader(filename);
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			string[] array = text.Split(' ');
			if (array[0] == "v")
			{
				double num = Convert.ToDouble(array[1]);
				double num2 = Convert.ToDouble(array[2]);
				double num3 = Convert.ToDouble(array[3]);
				val.Vertices.Add(num, num2, num3);
			}
			else if (array[0] == "f")
			{
				int num4 = Convert.ToInt32(array[1]) - 1;
				int num5 = Convert.ToInt32(array[2]) - 1;
				int num6 = Convert.ToInt32(array[3]) - 1;
				if (array.Length == 4)
				{
					val.Faces.AddFace(num4, num5, num6);
					continue;
				}
				int num7 = Convert.ToInt32(array[4]) - 1;
				val.Faces.AddFace(num4, num5, num6, num7);
			}
		}
		return val;
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
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		if (mesh == null)
		{
			return;
		}
		mesh.Vertices.CombineIdentical(true, false);
		for (int i = 0; i < mesh.Faces.Count; i++)
		{
			MeshFace val = mesh.Faces[i];
			int num = (((MeshFace)(ref val)).IsQuad ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				Point3f val2 = mesh.Vertices[((MeshFace)(ref val))[j]];
				if (double.IsNaN(((Point3f)(ref val2)).X) || double.IsNaN(((Point3f)(ref val2)).Y) || double.IsNaN(((Point3f)(ref val2)).Z))
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
				Point3f val3 = mesh.Vertices[k];
				double.IsNaN(((Point3f)(ref val3)).X);
				float num2 = ((Point3f)(ref val3)).X;
				float num3 = ((Point3f)(ref val3)).Y;
				float num4 = ((Point3f)(ref val3)).Z;
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
				MeshFace val4 = mesh.Faces[l];
				if (((MeshFace)(ref val4)).IsQuad)
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
				MeshFace val5 = mesh.Faces[m];
				Vector3d val6;
				Vector3f val7;
				if (((MeshFace)(ref val5)).IsQuad)
				{
					double num5 = 1.0;
					Vector3d quadNormal = GetQuadNormal(mesh.Vertices[((MeshFace)(ref val5))[0]], mesh.Vertices[((MeshFace)(ref val5))[1]], mesh.Vertices[((MeshFace)(ref val5))[2]], mesh.Vertices[((MeshFace)(ref val5))[3]]);
					if (((Vector3d)(ref quadNormal)).IsValid)
					{
						val6 = default(Vector3d);
						val7 = mesh.Normals[((MeshFace)(ref val5))[0]];
						float x = ((Vector3f)(ref val7)).X;
						val7 = mesh.Normals[((MeshFace)(ref val5))[1]];
						float num6 = x + ((Vector3f)(ref val7)).X;
						val7 = mesh.Normals[((MeshFace)(ref val5))[2]];
						float num7 = num6 + ((Vector3f)(ref val7)).X;
						val7 = mesh.Normals[((MeshFace)(ref val5))[3]];
						((Vector3d)(ref val6)).X = num7 + ((Vector3f)(ref val7)).X;
						val7 = mesh.Normals[((MeshFace)(ref val5))[0]];
						float y = ((Vector3f)(ref val7)).Y;
						val7 = mesh.Normals[((MeshFace)(ref val5))[1]];
						float num8 = y + ((Vector3f)(ref val7)).Y;
						val7 = mesh.Normals[((MeshFace)(ref val5))[2]];
						float num9 = num8 + ((Vector3f)(ref val7)).Y;
						val7 = mesh.Normals[((MeshFace)(ref val5))[3]];
						((Vector3d)(ref val6)).Y = num9 + ((Vector3f)(ref val7)).Y;
						val7 = mesh.Normals[((MeshFace)(ref val5))[0]];
						float z = ((Vector3f)(ref val7)).Z;
						val7 = mesh.Normals[((MeshFace)(ref val5))[1]];
						float num10 = z + ((Vector3f)(ref val7)).Z;
						val7 = mesh.Normals[((MeshFace)(ref val5))[2]];
						float num11 = num10 + ((Vector3f)(ref val7)).Z;
						val7 = mesh.Normals[((MeshFace)(ref val5))[3]];
						((Vector3d)(ref val6)).Z = num11 + ((Vector3f)(ref val7)).Z;
						Vector3d val8 = val6;
						((Vector3d)(ref val8)).Unitize();
						num5 = quadNormal * val8;
					}
					if (num5 >= 0.0)
					{
						binaryWriter.Write(((MeshFace)(ref val5))[0] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[1] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[2] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[3] + 1);
					}
					else
					{
						binaryWriter.Write(((MeshFace)(ref val5))[3] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[2] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[1] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[0] + 1);
					}
				}
				else
				{
					double num12 = 1.0;
					Vector3d direction = GetDirection(mesh.Vertices[((MeshFace)(ref val5))[0]], mesh.Vertices[((MeshFace)(ref val5))[1]], mesh.Vertices[((MeshFace)(ref val5))[2]]);
					if (((Vector3d)(ref direction)).IsValid)
					{
						val6 = default(Vector3d);
						val7 = mesh.Normals[((MeshFace)(ref val5))[0]];
						float x2 = ((Vector3f)(ref val7)).X;
						val7 = mesh.Normals[((MeshFace)(ref val5))[1]];
						float num13 = x2 + ((Vector3f)(ref val7)).X;
						val7 = mesh.Normals[((MeshFace)(ref val5))[2]];
						((Vector3d)(ref val6)).X = num13 + ((Vector3f)(ref val7)).X;
						val7 = mesh.Normals[((MeshFace)(ref val5))[0]];
						float y2 = ((Vector3f)(ref val7)).Y;
						val7 = mesh.Normals[((MeshFace)(ref val5))[1]];
						float num14 = y2 + ((Vector3f)(ref val7)).Y;
						val7 = mesh.Normals[((MeshFace)(ref val5))[2]];
						((Vector3d)(ref val6)).Y = num14 + ((Vector3f)(ref val7)).Y;
						val7 = mesh.Normals[((MeshFace)(ref val5))[0]];
						float z2 = ((Vector3f)(ref val7)).Z;
						val7 = mesh.Normals[((MeshFace)(ref val5))[1]];
						float num15 = z2 + ((Vector3f)(ref val7)).Z;
						val7 = mesh.Normals[((MeshFace)(ref val5))[2]];
						((Vector3d)(ref val6)).Z = num15 + ((Vector3f)(ref val7)).Z;
						Vector3d val9 = val6;
						((Vector3d)(ref val9)).Unitize();
						num12 = direction * val9;
					}
					if (num12 >= 0.0)
					{
						binaryWriter.Write(((MeshFace)(ref val5))[0] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[1] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[2] + 1);
					}
					else
					{
						binaryWriter.Write(((MeshFace)(ref val5))[2] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[1] + 1);
						binaryWriter.Write(((MeshFace)(ref val5))[0] + 1);
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		if (mesh == null)
		{
			return;
		}
		for (int i = 0; i < mesh.Faces.Count; i++)
		{
			MeshFace val = mesh.Faces[i];
			int num = (((MeshFace)(ref val)).IsQuad ? 4 : 3);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				Point3f val2 = mesh.Vertices[((MeshFace)(ref val))[j]];
				if (double.IsNaN(((Point3f)(ref val2)).X) || double.IsNaN(((Point3f)(ref val2)).Y) || double.IsNaN(((Point3f)(ref val2)).Z))
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
			Point3f val3 = mesh.Vertices[k];
			double.IsNaN(((Point3f)(ref val3)).X);
			double num2 = Math.Round(((Point3f)(ref val3)).X, 6);
			double num3 = Math.Round(((Point3f)(ref val3)).Y, 6);
			double num4 = Math.Round(((Point3f)(ref val3)).Z, 6);
			string value = $"v {num2:0.######} {num3:0.######} {num4:0.######}";
			textWriter.WriteLine(value);
		}
		textWriter.WriteLine("g Designer_object");
		int count2 = mesh.Faces.Count;
		for (int l = 0; l < count2; l++)
		{
			MeshFace val4 = mesh.Faces[l];
			Vector3d val5;
			Vector3f val6;
			string value2;
			if (((MeshFace)(ref val4)).IsQuad)
			{
				double num5 = 1.0;
				Vector3d quadNormal = GetQuadNormal(mesh.Vertices[((MeshFace)(ref val4))[0]], mesh.Vertices[((MeshFace)(ref val4))[1]], mesh.Vertices[((MeshFace)(ref val4))[2]], mesh.Vertices[((MeshFace)(ref val4))[3]]);
				if (!((Vector3d)(ref quadNormal)).IsValid)
				{
					val5 = default(Vector3d);
					val6 = mesh.Normals[((MeshFace)(ref val4))[0]];
					float x = ((Vector3f)(ref val6)).X;
					val6 = mesh.Normals[((MeshFace)(ref val4))[1]];
					float num6 = x + ((Vector3f)(ref val6)).X;
					val6 = mesh.Normals[((MeshFace)(ref val4))[2]];
					float num7 = num6 + ((Vector3f)(ref val6)).X;
					val6 = mesh.Normals[((MeshFace)(ref val4))[3]];
					((Vector3d)(ref val5)).X = num7 + ((Vector3f)(ref val6)).X;
					val6 = mesh.Normals[((MeshFace)(ref val4))[0]];
					float y = ((Vector3f)(ref val6)).Y;
					val6 = mesh.Normals[((MeshFace)(ref val4))[1]];
					float num8 = y + ((Vector3f)(ref val6)).Y;
					val6 = mesh.Normals[((MeshFace)(ref val4))[2]];
					float num9 = num8 + ((Vector3f)(ref val6)).Y;
					val6 = mesh.Normals[((MeshFace)(ref val4))[3]];
					((Vector3d)(ref val5)).Y = num9 + ((Vector3f)(ref val6)).Y;
					val6 = mesh.Normals[((MeshFace)(ref val4))[0]];
					float z = ((Vector3f)(ref val6)).Z;
					val6 = mesh.Normals[((MeshFace)(ref val4))[1]];
					float num10 = z + ((Vector3f)(ref val6)).Z;
					val6 = mesh.Normals[((MeshFace)(ref val4))[2]];
					float num11 = num10 + ((Vector3f)(ref val6)).Z;
					val6 = mesh.Normals[((MeshFace)(ref val4))[3]];
					((Vector3d)(ref val5)).Z = num11 + ((Vector3f)(ref val6)).Z;
					Vector3d val7 = val5;
					((Vector3d)(ref val7)).Unitize();
					num5 = quadNormal * val7;
				}
				value2 = ((num5 >= 0.0) ? $"f {((MeshFace)(ref val4))[0] + 1} {((MeshFace)(ref val4))[1] + 1} {((MeshFace)(ref val4))[2] + 1} {((MeshFace)(ref val4))[3] + 1}" : $"f {((MeshFace)(ref val4))[3] + 1} {((MeshFace)(ref val4))[2] + 1} {((MeshFace)(ref val4))[1] + 1} {((MeshFace)(ref val4))[0] + 1}");
			}
			else
			{
				double num12 = 1.0;
				Vector3d direction = GetDirection(mesh.Vertices[((MeshFace)(ref val4))[0]], mesh.Vertices[((MeshFace)(ref val4))[1]], mesh.Vertices[((MeshFace)(ref val4))[2]]);
				if (!((Vector3d)(ref direction)).IsValid)
				{
					val5 = default(Vector3d);
					val6 = mesh.Normals[((MeshFace)(ref val4))[0]];
					float x2 = ((Vector3f)(ref val6)).X;
					val6 = mesh.Normals[((MeshFace)(ref val4))[1]];
					float num13 = x2 + ((Vector3f)(ref val6)).X;
					val6 = mesh.Normals[((MeshFace)(ref val4))[2]];
					((Vector3d)(ref val5)).X = num13 + ((Vector3f)(ref val6)).X;
					val6 = mesh.Normals[((MeshFace)(ref val4))[0]];
					float y2 = ((Vector3f)(ref val6)).Y;
					val6 = mesh.Normals[((MeshFace)(ref val4))[1]];
					float num14 = y2 + ((Vector3f)(ref val6)).Y;
					val6 = mesh.Normals[((MeshFace)(ref val4))[2]];
					((Vector3d)(ref val5)).Y = num14 + ((Vector3f)(ref val6)).Y;
					val6 = mesh.Normals[((MeshFace)(ref val4))[0]];
					float z2 = ((Vector3f)(ref val6)).Z;
					val6 = mesh.Normals[((MeshFace)(ref val4))[1]];
					float num15 = z2 + ((Vector3f)(ref val6)).Z;
					val6 = mesh.Normals[((MeshFace)(ref val4))[2]];
					((Vector3d)(ref val5)).Z = num15 + ((Vector3f)(ref val6)).Z;
					Vector3d val8 = val5;
					((Vector3d)(ref val8)).Unitize();
					num12 = direction * val8;
				}
				value2 = ((num12 >= 0.0) ? $"f {((MeshFace)(ref val4))[0] + 1} {((MeshFace)(ref val4))[1] + 1} {((MeshFace)(ref val4))[2] + 1}" : $"f {((MeshFace)(ref val4))[2] + 1} {((MeshFace)(ref val4))[1] + 1} {((MeshFace)(ref val4))[0] + 1}");
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector((double)(((Point3f)(ref p1)).X - ((Point3f)(ref p0)).X), (double)(((Point3f)(ref p1)).Y - ((Point3f)(ref p0)).Y), (double)(((Point3f)(ref p1)).Z - ((Point3f)(ref p0)).Z));
		if (((Vector3d)(ref val)).Length < 0.0001)
		{
			return Vector3d.Unset;
		}
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector((double)(((Point3f)(ref p2)).X - ((Point3f)(ref p0)).X), (double)(((Point3f)(ref p2)).Y - ((Point3f)(ref p0)).Y), (double)(((Point3f)(ref p2)).Z - ((Point3f)(ref p0)).Z));
		if (((Vector3d)(ref val2)).Length < 0.0001)
		{
			return Vector3d.Unset;
		}
		Vector3d result = Vector3d.CrossProduct(val, val2);
		((Vector3d)(ref result)).Unitize();
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Vector3d direction = GetDirection(p0, p1, p2);
		if (!((Vector3d)(ref direction)).IsValid)
		{
			return direction;
		}
		direction = GetDirection(p1, p2, p3);
		if (!((Vector3d)(ref direction)).IsValid)
		{
			return direction;
		}
		direction = GetDirection(p2, p3, p0);
		if (!((Vector3d)(ref direction)).IsValid)
		{
			return direction;
		}
		direction = GetDirection(p3, p0, p1);
		if (!((Vector3d)(ref direction)).IsValid)
		{
			return direction;
		}
		return Vector3d.Unset;
	}
}
