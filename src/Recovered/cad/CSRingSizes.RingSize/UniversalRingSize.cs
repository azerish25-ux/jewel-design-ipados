namespace CSRingSizes.RingSize;

public struct UniversalRingSize
{
	public string RingSizeTypeAbrv;

	public string Size;

	public double diameterMM;

	public double radiusMM;

	public ERingSizeOption ringSizeType;

	public int index;

	public override string ToString()
	{
		if (RingSizeTypeAbrv == null || Size == null)
		{
			return "Unknown";
		}
		return RingSizeTypeAbrv + ":" + Size + ":" + diameterMM;
	}
}
