using System;
using System.Collections.Generic;
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
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		if (!File.Exists(filePath))
		{
			return false;
		}
		FileReadOptions val = new FileReadOptions
		{
			ImportMode = false,
			ImportReferenceMode = false,
			ScaleGeometry = false
		};
		return RhinoDoc.ReadFile(filePath, val);
	}

	public static bool ReadArray(ref int[] items, ref OnBinaryArchive archive)
	{
		int num = 0;
		if (!archive.ReadInt(ref num))
		{
			return false;
		}
		if (num == 0)
		{
			items = new int[0];
			return true;
		}
		if (num <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new int[num];
		}
		if (num != items.Length)
		{
			items = new int[num];
		}
		if (items == null)
		{
			return false;
		}
		for (int i = 0; i < items.Length; i++)
		{
			archive.ReadInt(ref items[i]);
		}
		return true;
	}

	public static bool ReadArray(ref double[] items, ref OnBinaryArchive archive)
	{
		int num = 0;
		archive.ReadInt(ref num);
		if (num <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new double[num];
		}
		if (num != items.Length)
		{
			items = new double[num];
		}
		if (items == null)
		{
			return false;
		}
		for (int i = 0; i < items.Length; i++)
		{
			archive.ReadDouble(ref items[i]);
		}
		return true;
	}

	public static bool ReadArray(ref string[] items, ref OnBinaryArchive archive)
	{
		int num = 0;
		if (!archive.ReadInt(ref num))
		{
			return false;
		}
		if (num <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new string[num];
		}
		if (num != items.Length)
		{
			items = new string[num];
		}
		if (items == null)
		{
			return false;
		}
		for (int i = 0; i < items.Length; i++)
		{
			if (!archive.ReadString(ref items[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool ReadArray(ref Guid[] items, ref OnBinaryArchive archive)
	{
		int num = 0;
		archive.ReadInt(ref num);
		if (num <= 0)
		{
			return false;
		}
		if (items == null)
		{
			items = new Guid[num];
		}
		if (num != items.Length)
		{
			items = new Guid[num];
		}
		if (items == null)
		{
			return false;
		}
		for (int i = 0; i < items.Length; i++)
		{
			archive.ReadUuid(ref items[i]);
		}
		return true;
	}

	[Obsolete]
	public static bool ReadArray(ref OnMesh[] items, ref OnBinaryArchive archive)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		int num = 0;
		archive.ReadInt(ref num);
		if (num < 0)
		{
			return false;
		}
		if (items == null)
		{
			items = (OnMesh[])(object)new OnMesh[num];
		}
		if (num != items.Length)
		{
			items = (OnMesh[])(object)new OnMesh[num];
		}
		if (items == null)
		{
			return false;
		}
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new OnMesh();
			uint num2 = 0u;
			archive.ReadInt(ref num2);
			((OnObject)items[i]).Read(ref archive);
			archive.SeekFromStart(num2);
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
		items = (from mesh in ((IEnumerable<OnMesh>)items2).Select((Func<OnMesh, Mesh>)Interop.FromOnMesh)
			where mesh != null
			select mesh).ToArray();
		return true;
	}

	[Obsolete]
	public static bool ReadArray(ref OnNurbsCurve[] items, ref OnBinaryArchive archive)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		int num = 0;
		archive.ReadInt(ref num);
		if (num < 0)
		{
			return false;
		}
		if (items == null)
		{
			items = (OnNurbsCurve[])(object)new OnNurbsCurve[num];
		}
		if (num != items.Length)
		{
			items = (OnNurbsCurve[])(object)new OnNurbsCurve[num];
		}
		if (items == null)
		{
			return false;
		}
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new OnNurbsCurve();
			((OnObject)items[i]).Read(ref archive);
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
			select Interop.FromOnCurve((object)onNurbsCurve) into curve
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
		foreach (int num in items)
		{
			archive.WriteInt(num);
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
		foreach (double num in items)
		{
			archive.WriteDouble(num);
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
		foreach (string text in items)
		{
			archive.WriteString(text);
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
			((OnObject)obj).Write(ref archive);
			uint num2 = archive.CurrentPosition();
			archive.SeekFromStart(num);
			archive.WriteInt(num2);
			archive.SeekFromStart(num2);
		}
		return true;
	}

	public static bool WriteArray(Mesh[] items, ref OnBinaryArchive archive)
	{
		return WriteArray((from onMesh in ((IEnumerable<Mesh>)items).Select((Func<Mesh, OnMesh>)((Mesh item) => (OnMesh)Interop.ToOnMesh(item)))
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
			((OnObject)items[i]).Write(ref archive);
		}
		return true;
	}

	public static bool WriteArray(NurbsCurve[] items, ref OnBinaryArchive archive)
	{
		return WriteArray((from item in items
			select ((OnCurve)Interop.ToOnCurve((Curve)(object)item)).NurbsCurve() into onNurbsCurve
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
			return (GeometryBase)((obj is GeometryBase) ? obj : null);
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
