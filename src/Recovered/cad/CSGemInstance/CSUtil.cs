namespace CSGemInstance;

public static class CSUtil
{
	public delegate double DelegateGetDouble();

	private const double _toleranceDefault = 0.001;

	private static double _tolerance = double.NaN;

	public static double Tolerance
	{
		get
		{
			if (!double.IsNaN(_tolerance))
			{
				return _tolerance;
			}
			if (GetTolerance == null)
			{
				return 0.001;
			}
			_tolerance = GetTolerance();
			return _tolerance;
		}
	}

	public static DelegateGetDouble GetTolerance { private get; set; }
}
