using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;

namespace CS.CSObjects.ObsoleteGrouping;

[Obsolete]
[CSBlock("GroupStart", "{51D0AE08-06E7-495C-AD69-BBE4D64847E5}")]
public sealed class CSGroupStartObsolete : CSGroup
{
	public CSGroupStartObsolete(string identifier)
		: base(identifier)
	{
	}
}
