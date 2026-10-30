using RMA.Rhino;

namespace CounterSketchServer;

public class CounterSketchPlugInAttributes : MRhinoPlugInAttributes
{
	public override string Address()
	{
		return "706 East River Drive\nDavenport\nIA\n52803";
	}

	public override string Country()
	{
		return "USA";
	}

	public override string Email()
	{
		return "support@gemvision.com";
	}

	public override string Fax()
	{
		return "563.884.8181";
	}

	public override string Organization()
	{
		return "Gemvision";
	}

	public override string Phone()
	{
		return "888.357.62762";
	}

	public override string UpdateURL()
	{
		return "http://gemvision.com";
	}

	public override string Website()
	{
		return "http://www.gemvision.com";
	}
}
