using System;
using System.Collections.Generic;

namespace CS.Parametric;

public sealed class PatchWirePos
{
	public string Name { get; set; }

	public List<PatchWirePosSetting> Children { get; set; } = new List<PatchWirePosSetting>();

	public int Width { get; set; }

	public int Height { get; set; }

	public int ExpandWidth { get; set; }

	public int ExpandHeight { get; set; }

	public Guid Guid { get; set; }
}
