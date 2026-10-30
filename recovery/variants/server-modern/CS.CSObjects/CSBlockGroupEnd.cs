using CS.Parametric;

namespace CS.CSObjects;

[CSBlock("BlockGroupEnd", "{D6CCAE41-B1BC-424a-BA96-0E52487B46CB}")]
public sealed class CSBlockGroupEnd : CSObject
{
	public CSBlockGroupEnd(string identifier)
		: base(identifier)
	{
	}

	public CSBlockGroupEnd()
		: base("EndGroup")
	{
	}
}
