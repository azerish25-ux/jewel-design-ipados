using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using RMA.OpenNURBS;
using Rhino;
using Rhino.FileIO;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS;

public static class CSUtilIO
{
	public static bool Open3DM(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return false;
		}
		FileReadOptions options = new FileReadOptions
		{
			ImportMode = false,
			ImportReferenceMode = false,
			ScaleGeometry = false
		};
		return RhinoDoc.ReadFile(filePath, options);
	}

	public static bool ReadArray(ref int[] items, ref OnBinaryArchive archive)
	{
		int i = 0;
		if (!archive.ReadInt(ref i))
		{
			return false;
		}
		if (i == 0)
		{
			items = new int[0];
			return true;
		}
		if (i <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new int[i];
		}
		if (i != items.Length)
		{
			items = new int[i];
		}
		if (items == null)
		{
			return false;
		}
		for (int j = 0; j < items.Length; j++)
		{
			archive.ReadInt(ref items[j]);
		}
		return true;
	}

	public static bool ReadArray(ref double[] items, ref OnBinaryArchive archive)
	{
		int i = 0;
		archive.ReadInt(ref i);
		if (i <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new double[i];
		}
		if (i != items.Length)
		{
			items = new double[i];
		}
		if (items == null)
		{
			return false;
		}
		for (int j = 0; j < items.Length; j++)
		{
			archive.ReadDouble(ref items[j]);
		}
		return true;
	}

	public static bool ReadArray(ref string[] items, ref OnBinaryArchive archive)
	{
		int i = 0;
		if (!archive.ReadInt(ref i))
		{
			return false;
		}
		if (i <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new string[i];
		}
		if (i != items.Length)
		{
			items = new string[i];
		}
		if (items == null)
		{
			return false;
		}
		for (int j = 0; j < items.Length; j++)
		{
			if (!archive.ReadString(ref items[j]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool ReadArray(ref Guid[] items, ref OnBinaryArchive archive)
	{
		int i = 0;
		archive.ReadInt(ref i);
		if (i <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new Guid[i];
		}
		if (i != items.Length)
		{
			items = new Guid[i];
		}
		if (items == null)
		{
			return false;
		}
		for (int j = 0; j < items.Length; j++)
		{
			archive.ReadUuid(ref items[j]);
		}
		return true;
	}

	[Obsolete]
	public static bool ReadArray(ref OnMesh[] items, ref OnBinaryArchive archive)
	{
		int i = 0;
		archive.ReadInt(ref i);
		if (i < 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new OnMesh[i];
		}
		if (i != items.Length)
		{
			items = new OnMesh[i];
		}
		if (items == null)
		{
			return false;
		}
		for (int j = 0; j < items.Length; j++)
		{
			items[j] = new OnMesh();
			uint i2 = 0u;
			archive.ReadInt(ref i2);
			items[j].Read(ref archive);
			archive.SeekFromStart(i2);
		}
		return true;
	}

	public static bool ReadArray(ref Mesh[] items, ref OnBinaryArchive archive)
	{
		OnMesh[] items2 = null;
		if (!ReadArray(ref items2, ref archive))
		{
			return false;
		}
		items = (from mesh in items2.Select(Interop.FromOnMesh)
			where mesh != null
			select mesh).ToArray();
		return true;
	}

	[Obsolete]
	public static bool ReadArray(ref OnNurbsCurve[] items, ref OnBinaryArchive archive)
	{
		int i = 0;
		archive.ReadInt(ref i);
		if (i < 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new OnNurbsCurve[i];
		}
		if (i != items.Length)
		{
			items = new OnNurbsCurve[i];
		}
		if (items == null)
		{
			return false;
		}
		for (int j = 0; j < items.Length; j++)
		{
			items[j] = new OnNurbsCurve();
			items[j].Read(ref archive);
		}
		return true;
	}

	public static bool ReadArray(ref NurbsCurve[] items, ref OnBinaryArchive archive)
	{
		OnNurbsCurve[] items2 = null;
		if (!ReadArray(ref items2, ref archive))
		{
			return false;
		}
		items = (from onNurbsCurve in items2
			select Interop.FromOnCurve(onNurbsCurve) into curve
			where curve != null
			where curve.HasNurbsForm() > 0
			select curve.ToNurbsCurve()).ToArray();
		return true;
	}

	public static bool WriteArray(int[] items, ref OnBinaryArchive archive)
	{
		if (items == null)
		{
			archive.WriteInt(0);
			return true;
		}
		archive.WriteInt(items.Length);
		foreach (int i2 in items)
		{
			archive.WriteInt(i2);
		}
		return true;
	}

	public static bool WriteArray(double[] items, ref OnBinaryArchive archive)
	{
		if (items == null)
		{
			archive.WriteInt(0);
			return true;
		}
		archive.WriteInt(items.Length);
		foreach (double d in items)
		{
			archive.WriteDouble(d);
		}
		return true;
	}

	public static bool WriteArray(string[] items, ref OnBinaryArchive archive)
	{
		if (items == null)
		{
			archive.WriteInt(0);
			return true;
		}
		archive.WriteInt(items.Length);
		foreach (string str in items)
		{
			archive.WriteString(str);
		}
		return true;
	}

	public static bool WriteArray(Guid[] items, ref OnBinaryArchive archive)
	{
		if (items == null)
		{
			archive.WriteInt(0);
			return true;
		}
		archive.WriteInt(items.Length);
		for (int i = 0; i < items.Length; i++)
		{
			archive.WriteUuid(ref items[i]);
		}
		return true;
	}

	[Obsolete]
	public static bool WriteArray(OnMesh[] items, ref OnBinaryArchive archive)
	{
		if (items == null)
		{
			archive.WriteInt(0);
			return true;
		}
		archive.WriteInt(items.Length);
		foreach (OnMesh obj in items)
		{
			uint num = archive.CurrentPosition();
			archive.WriteInt(num + 1);
			obj.Write(ref archive);
			uint num2 = archive.CurrentPosition();
			archive.SeekFromStart(num);
			archive.WriteInt(num2);
			archive.SeekFromStart(num2);
		}
		return true;
	}

	public static bool WriteArray(Mesh[] items, ref OnBinaryArchive archive)
	{
		return WriteArray((from item in items
			select (OnMesh)Interop.ToOnMesh(item) into onMesh
			where onMesh != null
			select onMesh).ToArray(), ref archive);
	}

	[Obsolete]
	public static bool WriteArray(OnNurbsCurve[] items, ref OnBinaryArchive archive)
	{
		if (items == null)
		{
			archive.WriteInt(0);
			return true;
		}
		archive.WriteInt(items.Length);
		for (int i = 0; i < items.Length; i++)
		{
			items[i].Write(ref archive);
		}
		return true;
	}

	public static bool WriteArray(NurbsCurve[] items, ref OnBinaryArchive archive)
	{
		return WriteArray((from item in items
			select ((OnCurve)Interop.ToOnCurve(item)).NurbsCurve() into onNurbsCurve
			where onNurbsCurve != null
			select onNurbsCurve).ToArray(), ref archive);
	}

	public static string GeometryBaseToString(GeometryBase gb)
	{
		try
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, gb);
			return Convert.ToBase64String(memoryStream.ToArray());
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static GeometryBase GeometryBaseFromString(string str)
	{
		try
		{
			byte[] array = Convert.FromBase64String(str);
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(array, 0, array.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			object obj = new BinaryFormatter().Deserialize(memoryStream);
			memoryStream.Dispose();
			return obj as GeometryBase;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static string BitmapToString(Bitmap bm)
	{
		using MemoryStream memoryStream = new MemoryStream();
		bm.Save(memoryStream, ImageFormat.Png);
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public static Bitmap BitmapFromString(string str)
	{
		using MemoryStream stream = new MemoryStream(Convert.FromBase64String(str));
		return new Bitmap(Image.FromStream(stream));
	}
}
