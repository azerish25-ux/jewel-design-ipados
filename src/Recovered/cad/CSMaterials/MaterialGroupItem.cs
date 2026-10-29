using System;

namespace CSMaterials;

[Serializable]
public sealed class MaterialGroupItem : ICloneable
{
	public string Id { get; set; }

	public string Name { get; set; }

	public bool IsReversed { get; set; }

	public bool IsInherited { get; set; }

	public object Clone()
	{
		return new MaterialGroupItem
		{
			Id = Id,
			IsReversed = IsReversed,
			IsInherited = IsInherited
		};
	}
}
