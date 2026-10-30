using CSMaterials;
using Rhino.Display;

namespace CSDatas.Interfaces;

public interface ICSDisplayObject
{
	bool AssignCurrentMaterialGroup(MaterialGroup materialGroup);

	bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial overrideMaterial = null);
}
