using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using CSMaterials;

namespace CSDatas;

public sealed class CSOptionsDisplay
{
	public Collection<CSDisplayMaterial> DisplayMaterials { get; set; }

	public bool DoShowBackfaces { get; set; }

	public bool DoShowCurves { get; set; }

	public bool DoShowPoints { get; set; }

	public bool DoShowStarts { get; set; }

	public bool IsGemNumberDisplayEnabled { get; set; }

	public bool IsMouseDown { get; set; }

	public bool IsSelectionDisplayModeEnabled { get; set; }

	public bool IsWireframeModeEnabled { get; set; }

	public CSDisplayMaterial GetCSDisplayMaterialFromName(string materialName)
	{
		CSDisplayMaterial cSDisplayMaterial = DisplayMaterials.FirstOrDefault((CSDisplayMaterial material) => material.Name == materialName);
		if (cSDisplayMaterial != null)
		{
			if (!DoShowBackfaces || cSDisplayMaterial.Mat == null)
			{
				return cSDisplayMaterial;
			}
			CSDisplayMaterial cSDisplayMaterial2 = cSDisplayMaterial.Duplicate();
			cSDisplayMaterial2.Mat.BackDiffuse = Color.MediumVioletRed;
			cSDisplayMaterial2.Mat.IsTwoSided = true;
			return cSDisplayMaterial2;
		}
		return DisplayMaterials.FirstOrDefault((CSDisplayMaterial material) => material.Name == "Undefined");
	}
}
