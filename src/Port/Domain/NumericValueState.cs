namespace JewelDesign.Domain;

/// <summary>Stored scalar state without inferred units, range limits or UI policy.</summary>
public readonly record struct NumericValueState(double RealValue, double PreviousValue)
{
    /// <summary>Plan only the scalar decisions in recovered CSValue.SetValue; execute no callbacks.</summary>
    public NumericValueEditPlan PlanSetValue(double newValue)
    {
        if (Math.Abs(newValue - RealValue) < 1E-05)
            return new NumericValueEditPlan(this, null, this);

        // The recovered method updates previous BEFORE recording PreChange.
        var atPreChange = new NumericValueState(RealValue, RealValue);
        var after = new NumericValueState(newValue, RealValue);
        return new NumericValueEditPlan(this, atPreChange, after);
    }
}

/// <summary>Immutable boundary states for a future graph command; not an applied edit or undo record.</summary>
public sealed class NumericValueEditPlan
{
    public NumericValueState Before { get; }
    public NumericValueState? AtPreChange { get; }
    public NumericValueState After { get; }
    public bool RequiresChange => AtPreChange.HasValue;

    internal NumericValueEditPlan(NumericValueState before, NumericValueState? atPreChange, NumericValueState after)
    {
        Before = before;
        AtPreChange = atPreChange;
        After = after;
    }
}
