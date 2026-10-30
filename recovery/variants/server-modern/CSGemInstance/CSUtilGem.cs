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
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		int num = shape.IndexOf(" Rotated", StringComparison.Ordinal);
		string arg = ((num != -1) ? shape.Substring(0, num) : shape);
		InstanceDefinition val = RhinoDoc.ActiveDoc.InstanceDefinitions.Find(blockName, false);
		if (val != null && val.IsDeleted && !RhinoDoc.ActiveDoc.InstanceDefinitions.Undelete(val.Index))
		{
			val = null;
		}
		if (val != null)
		{
			return val;
		}
		string text = Path.Combine(CSGemInstanceDefinition.GemDir, "ICGems", type, $"{arg}.3dm");
		if (!File.Exists(text))
		{
			return null;
		}
		FileReadOptions val2 = new FileReadOptions
		{
			ImportMode = true,
			ImportReferenceMode = false,
			ScaleGeometry = false
		};
		if (!RhinoDoc.ReadFile(text, val2))
		{
			return null;
		}
		return RhinoDoc.ActiveDoc.InstanceDefinitions.Find(blockName, true);
	}

	public static bool Open3dm(string filePath)
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

	public static double ToRadians(double angleDegrees)
	{
		return OnUtil.On_DEGREES_TO_RADIANS * angleDegrees;
	}
}
