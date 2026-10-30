using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.JSonDoc;

public static class CSJsonExtensions
{
	public static void Add(this JObject archive, string id, Transform transform)
	{
		float[] array = transform.ToFloatArray(rowDominant: true);
		JArray jArray = new JArray();
		float[] array2 = array;
		foreach (float num in array2)
		{
			jArray.Add(num);
		}
		archive.Add(id, jArray);
	}

	public static void Add(this JObject archive, string id, Point3d pt)
	{
		archive.Add(id, new JArray { pt.X, pt.Y, pt.Z });
	}

	public static void Add(this JObject archive, string id, GeometryBase geometry)
	{
		archive.Add(id, CSUtilIO.GeometryBaseToString(geometry));
	}

	public static void Add(this JObject archive, string id, On3dPoint pt)
	{
		archive.Add(id, new JArray { pt.x, pt.y, pt.z });
	}

	public static On3dPoint GetOn3dPoint(this JObject jBlock, string id)
	{
		if (!jBlock.ContainsKey(id))
		{
			return null;
		}
		JArray jArray = (JArray)jBlock[id];
		if (jArray.Count >= 3)
		{
			return new On3dPoint((float)jArray[0], (float)jArray[1], (float)jArray[2]);
		}
		return null;
	}

	public static bool GetMatrix(this JObject jBlock, string id, out Transform transform)
	{
		transform = Transform.Unset;
		if (!jBlock.ContainsKey(id))
		{
			return false;
		}
		JArray jArray = (JArray)jBlock[id];
		if (jArray == null)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				int num = i * 4 + j;
				if (num < jArray.Count)
				{
					transform[i, j] = (float)jArray[num];
				}
			}
		}
		return true;
	}

	public static Point3d GetPoint(this JObject jBlock, string id)
	{
		if (!jBlock.ContainsKey(id))
		{
			return default(Point3d);
		}
		JArray jArray = (JArray)jBlock[id];
		if (jArray.Count >= 3)
		{
			return new Point3d(jArray[0].ToObject<double>(), jArray[1].ToObject<double>(), jArray[2].ToObject<double>());
		}
		return default(Point3d);
	}
}
