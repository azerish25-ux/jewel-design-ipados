using System;
using System.Xml.Serialization;

namespace CSBlocks.Core;

[Serializable]
public sealed class ScaleToGemShape
{
	[XmlAttribute]
	public string shape;

	[XmlAttribute]
	public double x;

	[XmlAttribute]
	public double y;

	public ScaleToGemShape(string s, double x, double y)
	{
		shape = s;
		this.x = x;
		this.y = y;
	}

	public ScaleToGemShape()
	{
	}
}
