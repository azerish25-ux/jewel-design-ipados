using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CSMaterials;

public static class MaterialGroupHelper
{
	public static List<MaterialGroup> CurrentDocumentMaterialGroups { get; set; }

	public static bool IsVerificationMaterialGroupEnabled { get; set; }

	static MaterialGroupHelper()
	{
		CurrentDocumentMaterialGroups = new List<MaterialGroup>();
	}

	public static ObservableCollection<MaterialGroup> GetMaterialGroupByClassification(MaterialClassification materialClassification)
	{
		return new ObservableCollection<MaterialGroup>(CurrentDocumentMaterialGroups.Where((MaterialGroup materialGroup) => materialGroup.Classification == materialClassification));
	}
}
