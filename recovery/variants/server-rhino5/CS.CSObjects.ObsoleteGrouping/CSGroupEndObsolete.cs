using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;

namespace CS.CSObjects.ObsoleteGrouping;

[Obsolete]
[CSBlock("GroupEnd", "{4F6129F1-C193-448D-8A12-421B314463D1}")]
public sealed class CSGroupEndObsolete : CSGroup
{
	public CSGroupEndObsolete(string identifier)
		: base(identifier)
	{
	}
}
