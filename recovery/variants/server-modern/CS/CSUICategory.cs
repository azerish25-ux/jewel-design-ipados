using System;
using System.Linq;
using GV.CounterSketch.Shared.Model.Objects;

namespace CS;

public sealed class CSUICategory
{
	public int fileIndex = -1;

	private Guid _importGroupIdentifier;

	public bool Deletable { get; }

	public string CategoryName { get; set; } = "Unnamed";

	public ImportGroup ImportGroup { get; set; }

	public Guid ImportGroupIdentifier
	{
		get
		{
			return _importGroupIdentifier;
		}
		set
		{
			_importGroupIdentifier = value;
			if (CSApplicationHelper.Application.Document.Graph.ImportedCSGParts != null)
			{
				ImportGroup = CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.FirstOrDefault((ImportGroup importGroup) => importGroup.Identifier == _importGroupIdentifier);
			}
		}
	}

	public int Index { get; set; }

	public CSUICategory(string name, Guid importGroupIdentifier = default(Guid))
	{
		CategoryName = name;
		Deletable = false;
		ImportGroupIdentifier = importGroupIdentifier;
	}

	public CSUICategory(string name, bool delete)
	{
		CategoryName = name;
		Deletable = delete;
	}

	public override string ToString()
	{
		return CategoryName;
	}
}
