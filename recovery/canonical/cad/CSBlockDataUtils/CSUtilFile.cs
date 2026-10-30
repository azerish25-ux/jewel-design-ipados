using System.Collections.Generic;
using System.Drawing;
using Rhino.FileIO;
using Rhino.Geometry;

namespace CSBlockDataUtils;

public static class CSUtilFile
{
	public static bool ReadOneObject(string fileName, out List<GeometryBase> objects, out Bitmap previewImage, out string notes)
	{
		File3dm file3dm = File3dm.Read(fileName);
		previewImage = File3dm.ReadPreviewImage(fileName);
		notes = file3dm.Notes.Notes;
		objects = new List<GeometryBase>();
		foreach (File3dmObject @object in file3dm.Objects)
		{
			if (@object.Attributes.IsInstanceDefinitionObject || @object.Geometry is InstanceReferenceGeometry)
			{
				continue;
			}
			GeometryBase geometryBase = @object.Geometry.Duplicate();
			if (geometryBase != null)
			{
				if (!string.IsNullOrWhiteSpace(@object.Name))
				{
					geometryBase.SetUserString("CSName", @object.Name);
				}
				if (@object.Attributes.LayerIndex >= 0 && @object.Attributes.LayerIndex < file3dm.Layers.Count)
				{
					string name = file3dm.Layers[@object.Attributes.LayerIndex].Name;
					geometryBase.SetUserString("CSLayer", name);
				}
				objects.Add(geometryBase);
			}
		}
		file3dm.Dispose();
		return true;
	}
}
