using JewelDesign.Domain;
using DomainContractChecks.Recovered;

namespace DomainContractChecks;

public static class NumericValueAssertions
{
    public static string Run()
    {
        int cases = 0;
        const double epsilon = 1E-05;
        var payloadNaN = BitConverter.Int64BitsToDouble(unchecked((long)0x7ff8000000000042UL));
        var otherNaN = BitConverter.Int64BitsToDouble(unchecked((long)0xfff8000000000055UL));
        var negativeZero = BitConverter.Int64BitsToDouble(long.MinValue);

        void Case(double current, double previous, double requested, bool? expectedChange = null)
        {
            var state = new NumericValueState(current, previous);
            var oracle = new LegacyNumericValue(current, previous);
            if (!oracle.SetValue(requested)) throw new Exception("Legacy normal return changed");
            var plan = state.PlanSetValue(requested);
            Bits(current, state.RealValue, "Input current mutated");
            Bits(previous, state.PreviousValue, "Input previous mutated");
            Bits(current, plan.Before.RealValue, "Before current");
            Bits(previous, plan.Before.PreviousValue, "Before previous");
            Bits(oracle.RealValue, plan.After.RealValue, "After current");
            Bits(oracle.PreviousValue, plan.After.PreviousValue, "After previous");
            if (expectedChange.HasValue && plan.RequiresChange != expectedChange.Value)
                throw new Exception("Explicit change/no-op boundary mismatch");
            if (oracle.Calls.Count == 0)
            {
                if (plan.RequiresChange || plan.AtPreChange.HasValue)
                    throw new Exception("No-op acquired change/undo work");
            }
            else
            {
                if (oracle.Calls.Count != 2 || oracle.Calls[0].Name != "PreChange:SetValue" || oracle.Calls[1].Name != "Change")
                    throw new Exception("Recovered callback trace changed");
                if (!plan.RequiresChange || plan.AtPreChange is not NumericValueState captured)
                    throw new Exception("Change lost pre-mutation capture");
                Bits(oracle.Calls[0].Value, captured.RealValue, "PreChange current");
                Bits(oracle.Calls[0].Previous, captured.PreviousValue, "PreChange previous");
                Bits(oracle.Calls[1].Value, plan.After.RealValue, "Change current");
                Bits(oracle.Calls[1].Previous, plan.After.PreviousValue, "Change previous");
            }
            cases++;
        }

        Case(4.38, 2, 4.48, true);
        Case(4.38, 2, 4.38, false);
        Case(0, 42, Math.BitDecrement(epsilon), false);
        Case(0, 42, epsilon, true);
        Case(0, 42, Math.BitIncrement(epsilon), true);
        Case(0, 42, -Math.BitDecrement(epsilon), false);
        Case(0, 42, -epsilon, true);
        Case(0, 42, -Math.BitIncrement(epsilon), true);
        Case(0, 42, double.Epsilon, false);
        Case(0, 42, negativeZero, false);
        Case(4.38, 2, 1000000.123456, true); // no range clamp or step snap in this setter
        Case(double.PositiveInfinity, 2, double.PositiveInfinity, true);
        Case(payloadNaN, 2, payloadNaN, true);

        var values = new[] { double.NegativeInfinity, -double.MaxValue, -1, -epsilon, -double.Epsilon,
            negativeZero, 0, double.Epsilon, Math.BitDecrement(epsilon), epsilon, Math.BitIncrement(epsilon),
            1, double.MaxValue, double.PositiveInfinity, payloadNaN, otherNaN };
        foreach (var current in values)
            foreach (var previous in new[] { -17.0, negativeZero, otherNaN })
                foreach (var requested in values) Case(current, previous, requested);

        ulong seed = 0x6172deadcafeUL;
        double Next()
        {
            seed ^= seed << 13; seed ^= seed >> 7; seed ^= seed << 17;
            return BitConverter.Int64BitsToDouble(unchecked((long)seed));
        }
        for (int i = 0; i < 128; i++) Case(Next(), Next(), Next());
        return $"PASS numeric value plans: {cases} bit-exact recovered-method cases; epsilon/no-op/prechange ordering, unclamped values and nonfinite payloads; no live callbacks, undo, codec or CAD claim.";
    }

    private static void Bits(double expected, double actual, string field)
    {
        if (BitConverter.DoubleToInt64Bits(expected) != BitConverter.DoubleToInt64Bits(actual))
            throw new Exception(field + " bits changed");
    }
}
