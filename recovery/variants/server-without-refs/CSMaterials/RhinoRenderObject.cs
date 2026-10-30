using System;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.DocObjects;

namespace CSMaterials;

public sealed class RhinoRenderObject
{
	[Obsolete]
	public MRhinoObject MRhinoObject
	{
		get
		{
			if (!(RhinoObjectGuid == Guid.Empty))
			{
				return RhUtil.RhinoApp().ActiveDoc().LookupObject(RhinoObjectGuid);
			}
			return null;
		}
	}

	public RhinoObject RhinoObject
	{
		get
		{
			if (!(RhinoObjectGuid == Guid.Empty))
			{
				return RhinoDoc.ActiveDoc.Objects.Find(RhinoObjectGuid);
			}
			return null;
		}
	}

	public MaterialGroup AssignedMaterialGroup { get; set; }

	public Guid RhinoObjectGuid { get; set; }

	[Obsolete]
	public RhinoRenderObject(IOnObject rhinoObject, MaterialGroup materialGroup)
	{
		RhinoObjectGuid = rhinoObject.ModelObjectId();
		AssignedMaterialGroup = materialGroup;
	}

	public RhinoRenderObject(Guid rhinoObjectGuid, MaterialGroup materialGroup)
	{
		RhinoObjectGuid = rhinoObjectGuid;
		AssignedMaterialGroup = materialGroup;
	}
}
