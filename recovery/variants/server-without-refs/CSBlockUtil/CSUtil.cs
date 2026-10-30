using RMA.Rhino;

namespace CSBlockUtil;

public static class CSUtil
{
	public delegate bool DelegateGetBool();

	public delegate double DelegateGetDouble();

	public delegate string DelegateGetString();

	private const double _angleToleranceDefault = 0.001;

	private const double _toleranceDefault = 0.001;

	private static double _angleToleranceRadians = double.NaN;

	private static double _tolerance = double.NaN;

	public static string CSLibraryPath => GetCSLibraryPath?.Invoke();

	public static bool IsCounterSketch
	{
		get
		{
			if (GetIsCounterSketch != null)
			{
				return GetIsCounterSketch();
			}
			return false;
		}
	}

	public static bool IsPrintingAllowed
	{
		get
		{
			if (GetIsPrintingAllowed != null)
			{
				return GetIsPrintingAllowed();
			}
			return false;
		}
	}

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

	public static double AngleToleranceRadians
	{
		get
		{
			if (!double.IsNaN(_angleToleranceRadians))
			{
				return _angleToleranceRadians;
			}
			if (GetAngleToleranceRadians == null)
			{
				return 0.001;
			}
			_angleToleranceRadians = GetAngleToleranceRadians();
			return _angleToleranceRadians;
		}
	}

	public static DelegateGetString GetCSLibraryPath { get; set; }

	public static DelegateGetBool GetIsPrintingAllowed { get; set; }

	public static DelegateGetBool GetIsCounterSketch { get; set; }

	public static DelegateGetDouble GetTolerance { get; set; }

	public static DelegateGetDouble GetAngleToleranceRadians { get; set; }

	public static void Print(object toStringValue)
	{
		if (IsPrintingAllowed)
		{
			string text = toStringValue?.ToString() ?? "NULL Object";
			if (toStringValue == null)
			{
				RhUtil.RhinoApp().Print(text);
			}
			else
			{
				RhUtil.RhinoApp().Print(text);
			}
		}
	}

	public static void PrintLine(object toStringValue)
	{
		if (IsPrintingAllowed)
		{
			string text = ((toStringValue == null) ? string.Format("{0}\n", "NULL Object") : $"{toStringValue}\n");
			if (toStringValue == null)
			{
				RhUtil.RhinoApp().Print(text);
			}
			else
			{
				RhUtil.RhinoApp().Print(text);
			}
		}
	}
}
