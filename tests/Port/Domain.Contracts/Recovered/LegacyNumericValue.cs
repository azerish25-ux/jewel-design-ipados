// Test-only extraction. SetValue body is verified against canonical CSValue.cs.
// Native PreChange/Change are replaced with observation-only callbacks.
namespace DomainContractChecks.Recovered;

internal sealed class LegacyNumericValue
{
    private double _doubleValue;
    private double _previousValue;
    public double RealValue => _doubleValue;
    public double PreviousValue => _previousValue;
    public List<(string Name, double Value, double Previous)> Calls { get; } = new();

    public LegacyNumericValue(double value, double previous)
    {
        _doubleValue = value;
        _previousValue = previous;
    }

    private void PreChange(string member) => Calls.Add(("PreChange:" + member, _doubleValue, _previousValue));
    private void Change() => Calls.Add(("Change", _doubleValue, _previousValue));

	public bool SetValue(double newValue)
	{
		if (Math.Abs(newValue - _doubleValue) < 1E-05)
		{
			return true;
		}
		_previousValue = _doubleValue;
		PreChange("SetValue");
		_doubleValue = newValue;
		Change();
		return true;
	}
}
