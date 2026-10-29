namespace CS.Parametric;

public sealed class CAMValue
{
	public string PatchWire { get; set; }

	public string Item { get; set; }

	public int Index { get; set; }

	public bool IsAbsolute { get; set; }

	public double Delta { get; set; }

	public double Absolute { get; set; }

	public string Note { get; set; }
}
