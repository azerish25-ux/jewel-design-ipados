using System.Collections.Generic;
using System.Drawing;
using Rhino.FileIO;
using Rhino.Geometry;

namespace CSBlockDataUtils;

public static class CSUtilFile
{
	public static bool ReadOneObject(string fileName, out List<GeometryBase> objects, out Bitmap previewImage, out string notes)
	{
		File3dm val = File3dm.Read(fileName);
		previewImage = File3dm.ReadPreviewImage(fileName);
		notes = val.Notes.Notes;
		objects = new List<GeometryBase>();
		foreach (File3dmObject @object in val.Objects)
		{
			if (@object.Attributes.IsInstanceDefinitionObject || @object.Geometry is InstanceReferenceGeometry)
			{
				continue;
			}
			GeometryBase val2 = @object.Geometry.Duplicate();
			if (val2 != null)
			{
				if (!string.IsNullOrWhiteSpace(@object.Name))
				{
					val2.SetUserString("CSName", @object.Name);
				}
				if (@object.Attributes.LayerIndex >= 0 && @object.Attributes.LayerIndex < val.Layers.Count)
				{
					string name = val.Layers[@object.Attributes.LayerIndex].Name;
					val2.SetUserString("CSLayer", name);
				}
				objects.Add(val2);
			}
		}
		val.Dispose();
		return true;
	}
}
