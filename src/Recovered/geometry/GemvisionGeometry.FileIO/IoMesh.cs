using System;
using System.IO;
using GemvisionGeometry.Geometry;

namespace GemvisionGeometry.FileIO;

public class IoMesh
{
	public static GMesh ReadObj(string filename)
	{
		GMesh gMesh = new GMesh();
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
				gMesh.Vertices.Add(new GPoint3D(x, y, z));
			}
			else if (array[0] == "f")
			{
				int a = Convert.ToInt32(array[1]) - 1;
				int b = Convert.ToInt32(array[2]) - 1;
				int c = Convert.ToInt32(array[3]) - 1;
				if (array.Length == 4)
				{
					gMesh.Faces.Add(new GMeshFace(a, b, c));
					continue;
				}
				int d = Convert.ToInt32(array[4]) - 1;
				gMesh.Faces.Add(new GMeshFace(a, b, c, d));
			}
		}
		return gMesh;
	}

	public static bool SaveStlBinary(GMesh gMesh, string filename)
	{
		if (gMesh == null)
		{
			return false;
		}
		if (filename == null)
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
			int count = gMesh.Faces.Count;
			int num = count;
			for (i = 0; i < count; i++)
			{
				if (gMesh.Faces[i].IsQuad)
				{
					num++;
				}
			}
			binaryWriter.Write(num);
			for (i = 0; i < count; i++)
			{
				GMeshFace gMeshFace = gMesh.Faces[i];
				if (gMeshFace.IsQuad)
				{
					GPoint3F gPoint3F = new GPoint3F(gMesh.Vertices[gMeshFace.A]);
					GPoint3F p = new GPoint3F(gMesh.Vertices[gMeshFace.B]);
					GPoint3F gPoint3F2 = new GPoint3F(gMesh.Vertices[gMeshFace.C]);
					GPoint3F p2 = new GPoint3F(gMesh.Vertices[gMeshFace.D]);
					OutputTriFace(gPoint3F, p, gPoint3F2, binaryWriter);
					OutputTriFace(gPoint3F2, p2, gPoint3F, binaryWriter);
				}
				else
				{
					GPoint3F p3 = new GPoint3F(gMesh.Vertices[gMeshFace.A]);
					GPoint3F p4 = new GPoint3F(gMesh.Vertices[gMeshFace.B]);
					GPoint3F p5 = new GPoint3F(gMesh.Vertices[gMeshFace.C]);
					OutputTriFace(p3, p4, p5, binaryWriter);
				}
			}
			binaryWriter.Close();
		}
		return true;
	}

	private static void OutputTriFace(GPoint3F p1, GPoint3F p2, GPoint3F p3, BinaryWriter writer)
	{
		GVector3F v = p2 - p1;
		GVector3F v2 = p3 - p1;
		GVector3F gVector3F = GVector3F.CrossProduct(v, v2);
		gVector3F.Unitize();
		if (!double.IsNaN(p1.Mx) && !double.IsNaN(p1.My) && !double.IsNaN(p1.Mz) && !double.IsNaN(p2.Mx) && !double.IsNaN(p2.My) && !double.IsNaN(p2.Mz) && !double.IsNaN(p3.Mx) && !double.IsNaN(p3.My) && !double.IsNaN(p3.Mz))
		{
			writer.Write(gVector3F.Mx);
			writer.Write(gVector3F.My);
			writer.Write(gVector3F.Mz);
			writer.Write(p1.Mx);
			writer.Write(p1.My);
			writer.Write(p1.Mz);
			writer.Write(p2.Mx);
			writer.Write(p2.My);
			writer.Write(p2.Mz);
			writer.Write(p3.Mx);
			writer.Write(p3.My);
			writer.Write(p3.Mz);
			ushort value = 0;
			writer.Write(value);
		}
	}
}
