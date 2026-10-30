extern alias Legacy;
using System.Globalization;
using Current = CSRingSizes.RingSize;
using Baseline = Legacy::CSRingSizes.RingSize;

namespace DomainContractChecks;

public static class RingSizeAssertions
{
    public static string Run()
    {
        var culture = CultureInfo.CurrentCulture;
        int cases = 0;
        try
        {
            foreach (var cultureName in new[] { "en-US", "fr-FR" })
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
                var pairs = new (Current.RingSize Current, Baseline.RingSize Baseline)[]
                {
                    (new Current.RingSizeUSA(), new Baseline.RingSizeUSA()),
                    (new Current.RingSizeUK(), new Baseline.RingSizeUK()),
                    (new Current.RingSizeFrance(), new Baseline.RingSizeFrance()),
                    (new Current.RingSizeEur(), new Baseline.RingSizeEur()),
                    (new Current.RingSizeGerman(), new Baseline.RingSizeGerman()),
                    (new Current.RingSizeJapan(), new Baseline.RingSizeJapan()),
                    (new Current.RingSizeSwiss(), new Baseline.RingSizeSwiss())
                };
                foreach (var (current, baseline) in pairs)
                {
                    Equal(baseline.Count, current.Count);
                    Equal(baseline.RingSizeTypeAbrv, current.RingSizeTypeAbrv);
                    Equal((int)baseline.RingSizeType, (int)current.RingSizeType);
                    foreach (var index in Enumerable.Range(0, baseline.Count).Concat(new[] { -1, baseline.Count, baseline.Count + 1 }))
                    {
                        Equal(baseline.MmDiameter(index), current.MmDiameter(index));
                        Equal(baseline.TxtSize(index), current.TxtSize(index));
                        EqualSize(baseline.GetSizeIndex(index), current.GetSizeIndex(index));
                        var diameter = baseline.MmDiameter(index);
                        EqualSize(baseline.GetSize(diameter), current.GetSize(diameter));
                        EqualSize(baseline.GetSizeRadius(diameter / 2), current.GetSizeRadius(diameter / 2));
                        EqualSize(baseline.GetSizeTxt(baseline.TxtSize(index)), current.GetSizeTxt(current.TxtSize(index)));
                        cases++;
                    }
                    // Preserve truncation, rounding, invalid-text and out-of-range behavior; no added clamping.
                    foreach (var index in new[] { -1.75, -0.25, 0.25, 1.75, (double)baseline.Count + 0.75 })
                        EqualSize(baseline.GetSizeIndex(index), current.GetSizeIndex(index));
                    foreach (var diameter in new[] { -1.0, 0.0, 13.0, 17.33, 100.0, double.NaN, double.NegativeInfinity, double.PositiveInfinity })
                    {
                        SameOutcome(() => baseline.GetIndex(diameter), () => current.GetIndex(diameter));
                        SameOutcome(() => baseline.GetSize(diameter).ToString(), () => current.GetSize(diameter).ToString());
                    }
                    foreach (var text in new string?[] { null, "", "unrecognized", "7", "7.25", "7,25", " 7 " })
                    {
                        int expected = -123, actual = -123;
                        Equal(baseline.GetIndexTxt(text!, ref expected), current.GetIndexTxt(text!, ref actual));
                        Equal(expected, actual);
                        EqualSize(baseline.GetSizeTxt(text!), current.GetSizeTxt(text!));
                    }
                }
            }
            var us = new Current.RingSizeUSA();
            Equal(65, us.Count);
            Equal("7", us.TxtSize(28));
            Equal("7 1/4", us.TxtSize(29));
            Equal(17.33, us.MmDiameter(28));
            Equal(8.665, us.GetSizeIndex(28).radiusMM);
            Equal(28, us.GetSizeIndex(28.9).index);
            Equal("Z+1", new Current.RingSizeUK().TxtSize(52));
            Equal("Unknown", default(Current.UniversalRingSize).ToString());
            if (!Enum.GetNames<Current.ERingSizeOption>().SequenceEqual(Enum.GetNames<Baseline.ERingSizeOption>()))
                throw new Exception("Ring-size enum names changed");
            if (!Enum.GetValues<Current.ERingSizeOption>().Select(x => (int)x).SequenceEqual(Enum.GetValues<Baseline.ERingSizeOption>().Select(x => (int)x)))
                throw new Exception("Ring-size enum IDs changed");
        }
        finally { CultureInfo.CurrentCulture = culture; }
        return $"PASS production ring sizes: seven regions, {cases} catalog/boundary cases in two cultures, radius/diameter/index/text and invalid input parity; recovered-source baseline, no PC binary/geometry claim.";
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new Exception($"Ring size mismatch: expected {expected}, actual {actual}");
    }

    private static void EqualSize(Baseline.UniversalRingSize expected, Current.UniversalRingSize actual)
    {
        Equal(expected.RingSizeTypeAbrv, actual.RingSizeTypeAbrv);
        Equal(expected.Size, actual.Size);
        Equal(expected.diameterMM, actual.diameterMM);
        Equal(expected.radiusMM, actual.radiusMM);
        Equal((int)expected.ringSizeType, (int)actual.ringSizeType);
        Equal(expected.index, actual.index);
        Equal(expected.ToString(), actual.ToString());
    }

    private static void SameOutcome<T>(Func<T> baseline, Func<T> current)
    {
        T expected;
        try { expected = baseline(); }
        catch (Exception error)
        {
            try { current(); }
            catch (Exception actual) when (actual.GetType() == error.GetType()) { return; }
            throw new Exception("Ring size error behavior changed", error);
        }
        Equal(expected, current());
    }
}
