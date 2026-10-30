using System;
using System.IO;
using RMA.OpenNURBS;
using Rhino;
using Rhino.DocObjects;
using Rhino.FileIO;

namespace CSGemInstance;

internal static class CSUtilGem
{
	public static InstanceDefinition GetGemInstanceDefinition(string type, string shape)
	{
		int num = shape.IndexOf(" Rotated", StringComparison.Ordinal);
		string arg = ((num != -1) ? shape.Substring(0, num) : shape);
		string blockName = $"{type}_{arg}";
		return GetGemInstanceDefinition(type, shape, blockName);
	}

	public static InstanceDefinition GetGemInstanceDefinition(string type, string shape, string blockName)
	{
		int num = shape.IndexOf(" Rotated", StringComparison.Ordinal);
		string arg = ((num != -1) ? shape.Substring(0, num) : shape);
		InstanceDefinition instanceDefinition = RhinoDoc.ActiveDoc.InstanceDefinitions.Find(blockName, ignoreDeletedInstanceDefinitions: false);
		if (instanceDefinition != null && instanceDefinition.IsDeleted && !RhinoDoc.ActiveDoc.InstanceDefinitions.Undelete(instanceDefinition.Index))
		{
			instanceDefinition = null;
		}
		if (instanceDefinition != null)
		{
			return instanceDefinition;
		}
		string path = Path.Combine(CSGemInstanceDefinition.GemDir, "ICGems", type, $"{arg}.3dm");
		if (!File.Exists(path))
		{
			return null;
		}
		FileReadOptions options = new FileReadOptions
		{
			ImportMode = true,
			ImportReferenceMode = false,
			ScaleGeometry = false
		};
		if (!RhinoDoc.ReadFile(path, options))
		{
			return null;
		}
		return RhinoDoc.ActiveDoc.InstanceDefinitions.Find(blockName, ignoreDeletedInstanceDefinitions: true);
	}

	public static bool Open3dm(string filePath)
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

	public static double ToRadians(double angleDegrees)
	{
		return OnUtil.On_DEGREES_TO_RADIANS * angleDegrees;
	}
}
