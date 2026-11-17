using System.Security.Cryptography;

namespace JewelDesign.Rendering.Candidate;

/// <summary>
/// Offline direct comparison for the captured original locale1/IGNORECASE|IGNOREWIDTH profile.
/// Uses UTF16 cursors and native ordering priorities, including behavior that differs from sort keys.
/// This layer models the successful public NLS comparison; original ON_wString/CRT/ANSI state is separate.
/// </summary>
public sealed class Rhino5NlsDirectComparer
{
    private static readonly Rhino5NlsComparisonInputs Inputs = Rhino5NlsComparisonInputs.Reference60502;
    private static readonly byte[] Jamo = LoadJamo();
    public static Rhino5NlsDirectComparer Reference60502 { get; } = new();
    private Rhino5NlsDirectComparer() { }

    public int Compare(string first, string second, int maximumInputUtf16Units = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumInputUtf16Units);
        if (first.Length > maximumInputUtf16Units || second.Length > maximumInputUtf16Units)
            throw new InvalidDataException("Direct NLS input exceeds the explicit UTF16 budget.");
        int start = 0;
        while (start < first.Length && start < second.Length && first[start] != 0 && first[start] == second[start]) start++;
        if ((start == first.Length || first[start] == 0) && (start == second.Length || second[start] == 0)) return 0;
        if (start > 0)
        {
            char previous = first[start - 1];
            if (IsJamo(previous)) start = 0;
            else if (previous == 0xdb40) start--;
        }
        var a = new Cursor(first, start);
        var b = new Cursor(second, start);
        uint extraMask = 0xf1;
        int secondaryResult = 0, punctuationResult = 0;
        bool ignorePunctuation = false;
        long steps = 0, stepLimit = (long)first.Length * 12 + (long)second.Length * 12 + 1024;
        while (!a.End && !b.End)
        {
            if (++steps > stepLimit) throw new InvalidDataException("Direct NLS cursor work exceeded its source-derived bound.");
            a.Load(); b.Load();
            int firstScript = Script(a.Weight), secondScript = Script(b.Weight);
            a.Primary = Primary(a.Weight); b.Primary = Primary(b.Weight);
            if (a.Weight == b.Weight && (firstScript < 2 || firstScript > 6))
            {
                a.SkipVariation(); b.SkipVariation(); a.Advance(); b.Advance(); continue;
            }
            bool ordinaryEqualPrimary = Primary(a.Weight) == Primary(b.Weight) && (firstScript <= 1 || firstScript > 12);
            if (!ordinaryEqualPrimary)
            {
                bool changed = Preprocess(a, ignorePunctuation);
                changed = Preprocess(b, ignorePunctuation) || changed;
                if (changed) continue;
                if (firstScript == 1)
                {
                    if ((extraMask & 3) != 0) Remember(1, 0x0a, ref secondaryResult, ref extraMask);
                    a.Advance();
                }
                else if (firstScript == 3)
                {
                    a.EastAsian();
                    if (secondScript == 6 && !ignorePunctuation) a.HasWeight = false;
                    else if (secondScript != 3 && a.Extra != 0 && (extraMask & 0xf0) != 0) Remember(1, 0x0f, ref secondaryResult, ref extraMask);
                }
                else if (firstScript == 5)
                {
                    if (a.Weight == b.Weight) { a.Advance(); b.Advance(); continue; }
                    a.Primary = secondScript == 5 ? ((Primary(a.Weight) << 8) | Diacritic(a.Weight)) & 65535 : 0xfdff;
                    if (secondScript == 5) b.Primary = ((Primary(b.Weight) << 8) | Diacritic(b.Weight)) & 65535;
                }
                else if (firstScript == 6 && secondScript != 6)
                {
                    ignorePunctuation = true; punctuationResult = 1; a.Advance();
                }
                if (secondScript == 1)
                {
                    if ((extraMask & 3) != 0) Remember(-1, 0x0a, ref secondaryResult, ref extraMask);
                    b.Advance();
                }
                else if (secondScript == 3)
                {
                    b.EastAsian();
                    if (firstScript == 6 && !a.HasWeight) b.HasWeight = false;
                    else if (firstScript == 3)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            uint bit = 0x10u << i;
                            uint left = (a.Extra >> (i * 8)) & 255, right = (b.Extra >> (i * 8)) & 255;
                            if ((extraMask & bit) != 0 && left != right) { Remember(Order(left, right), bit - 1, ref secondaryResult, ref extraMask); break; }
                        }
                    }
                    else if (b.Extra != 0 && (extraMask & 0xf0) != 0) Remember(-1, 0x0f, ref secondaryResult, ref extraMask);
                }
                else if (secondScript == 5 && firstScript != 5) b.Primary = 0xfdff;
                else if (secondScript == 6)
                {
                    if (firstScript == 6)
                    {
                        int difference = Order(a.Weight & 255, b.Weight & 255);
                        if (difference == 0) difference = Order((Diacritic(a.Weight) * 8 + (Case(a.Weight) & 63)) & 255, (Diacritic(b.Weight) * 8 + (Case(b.Weight) & 63)) & 255);
                        if (difference != 0) { ignorePunctuation = true; punctuationResult = difference; }
                        a.Advance();
                    }
                    else { ignorePunctuation = true; punctuationResult = -1; }
                    b.Advance();
                }
                if (!a.HasWeight || !b.HasWeight) continue;
            }
            int primaryDifference = Order(a.Primary, b.Primary);
            if (primaryDifference != 0) return primaryDifference;
            if (firstScript == 4 && secondScript == 4)
            {
                int difference = Order(a.Extra, b.Extra);
                if (difference != 0) return difference;
            }
            if (firstScript is >= 169 and <= 175)
            {
                int difference = Order(Diacritic(a.Weight), Diacritic(b.Weight));
                if (difference != 0) return difference;
            }
            if ((extraMask & 3) != 0 && Diacritic(a.Weight) != Diacritic(b.Weight))
            {
                uint left = Diacritic(a.Weight), right = Diacritic(b.Weight);
                while (!a.End && Script(Raw(a.Peek())) == 1) { left = (left + Diacritic(Raw(a.Peek()))) & 255; a.Advance(); }
                while (!b.End && Script(Raw(b.Peek())) == 1) { right = (right + Diacritic(Raw(b.Peek()))) & 255; b.Advance(); }
                if (left != right) Remember(Order(left, right), 0x0a, ref secondaryResult, ref extraMask);
            }
            a.Advance(); b.Advance();
        }
        int remainder = Remainder(a, 1, ignorePunctuation, secondaryResult, punctuationResult, extraMask);
        if (remainder != 0) return remainder;
        remainder = Remainder(b, -1, ignorePunctuation, secondaryResult, punctuationResult, extraMask);
        return remainder != 0 ? remainder : secondaryResult != 0 ? secondaryResult : punctuationResult;
    }

    private static bool Preprocess(Cursor value, bool ignorePunctuation)
    {
        switch (Script(value.Weight))
        {
            case 0: value.SkipVariation(); value.Advance(); return true;
            case 2: value.Expand(); return true;
            case 4: value.Hangul(); return false;
            case 6 when ignorePunctuation: value.Advance(); return true;
            default: return false;
        }
    }
    private static int Remainder(Cursor value, int side, bool ignorePunctuation, int secondary, int punctuation, uint mask)
    {
        while (!value.End)
        {
            int kind = Script(Raw(value.Current));
            if (kind == 6) { if (!ignorePunctuation && secondary == 0 && punctuation == 0) return side; }
            else if (kind == 0) value.SkipVariation();
            else if (kind == 1) { if ((mask & 3) != 0) return side; }
            else return side;
            value.Advance();
        }
        return 0;
    }
    private static void Remember(int sign, uint mask, ref int result, ref uint extraMask) { result = sign; extraMask &= mask; }
    private static uint Raw(char unit) => Inputs.GetPackedWeight(unit);
    private static int Script(uint weight) => (int)((weight >> 8) & 255);
    private static uint Diacritic(uint weight) => (weight >> 16) & 255;
    private static uint Case(uint weight) => weight >> 24;
    private static uint Primary(uint weight) => weight & 65535;
    private static int Order(uint first, uint second) => first < second ? -1 : first > second ? 1 : 0;
    private static uint Mask(uint value) => Script(value) == 5 || Script(value) is >= 169 and <= 175 ? value : value & 0xe6ffffff;
    private static bool IsJamo(char c) => c is >= '\u1100' and <= '\u11ff' or >= '\ua960' and <= '\ua97c' or >= '\ud7b0' and <= '\ud7c6' or >= '\ud7cb' and <= '\ud7fb';
    private static int JamoIndex(char c) => c is >= '\u1100' and <= '\u11ff' ? c - 0x1100 : c is >= '\ua960' and <= '\ua97c' ? c - 0xa860 : c is >= '\ud7b0' and <= '\ud7c6' ? c - 0xd693 : c - 0xd697;
    private static byte[] LoadJamo()
    {
        using var stream = typeof(Rhino5NlsDirectComparer).Assembly.GetManifestResourceStream("NlsJamo60502") ?? throw new InvalidDataException("Approved Jamo inputs missing.");
        using var buffer = new MemoryStream(); stream.CopyTo(buffer); byte[] bytes = buffer.ToArray();
        if (bytes.Length != 2856 || Convert.ToHexStringLower(SHA256.HashData(bytes)) != "4f8e7acefeeead6cd25f22cb801db55603813760bb9072c84cf2d31ed95a5f86") throw new InvalidDataException("Approved Jamo input data changed.");
        return bytes;
    }

    private sealed class Cursor(string text, int start)
    {
        private readonly string source = text;
        private int position = start, pending;
        private char firstExpansion, secondExpansion;
        public char Current = At(text, start);
        public uint Weight, Primary, Extra;
        public bool HasWeight;
        public bool End => pending == 0 && At(position) == 0;
        private static char At(string source, int index) => index >= 0 && index < source.Length ? source[index] : '\0';
        private char At(int index) => At(source, index);
        public void Load() { if (!HasWeight) { Weight = Mask(Raw(Current)); HasWeight = true; } }
        public void Advance()
        {
            if (pending > 0) { Current = pending == 2 ? firstExpansion : secondExpansion; pending--; }
            else Current = At(++position);
            HasWeight = false;
        }
        public char Peek() => pending > 0 ? pending == 2 ? firstExpansion : secondExpansion : At(position + 1);
        public void SkipVariation() { if (At(position) == 0xdb40 && At(position + 1) is >= '\udd00' and <= '\uddef') Advance(); }
        public void Expand()
        {
            var pair = Inputs.GetExpansion((int)(Raw(Current) >> 16));
            firstExpansion = pair.First; secondExpansion = pair.Second; pending = 2; Advance();
        }
        public void Hangul()
        {
            int next = position;
            char leading = At(next), vowel = '\u1160';
            bool filler = false;
            var state = new JamoAccumulator();
            if (Leading(leading)) { next++; if (leading == 0x115f) filler = true; }
            else { leading = '\u115f'; filler = true; }
            state.Merge(leading, 1);
            if (Vowel(At(next))) vowel = At(next++);
            if (vowel == 0x1160) filler = true;
            state.Merge(vowel, 2);
            if (Trailing(At(next))) state.Merge(At(next++), 3);
            if (!state.Old)
            {
                state = new(); state.Merge(At(position), 1); next = position + 1;
                state.Merge('\u1160', 2); filler = true;
            }
            uint syllable = ((state.LeadingIndex * 21 + state.VowelIndex) * 28 + state.TrailingIndex + 0xac00) & 65535;
            uint leadingExtra = state.LeadingExtra;
            if (filler) { syllable = (syllable - 1) & 65535; if (syllable < 0xac00) syllable = 0x3260; leadingExtra = (leadingExtra + 128) & 255; }
            Primary = Rhino5NlsDirectComparer.Primary(Raw((char)syllable)); Weight = Primary | 0x02020000;
            Extra = (Math.Max(2u, leadingExtra) << 16) | (Math.Max(2u, state.VowelExtra) << 8) | Math.Max(2u, state.TrailingExtra);
            position = next - 1;
        }
        public void EastAsian()
        {
            uint original = Weight, aw = original & 255, currentCase = Case(original); Extra = 0;
            if (aw >= 2) { Primary = 0x2200 | aw; Extra = (currentCase & 6) | (3u << 8) | ((currentCase & 0x24) << 16) | ((currentCase & 5) << 24); Weight = (original & 0x00ff0000) | Primary | 0x02000000; return; }
            int previous = position - 1;
            while (previous >= 0)
            {
                if (previous >= 1 && At(previous - 1) == 0xdb40 && At(previous) is >= '\udd00' and <= '\uddef') { previous -= 2; continue; }
                uint value = Mask(Raw(At(previous))); int kind = Script(value);
                if (kind == 2) break;
                if (kind < 3) { previous--; continue; }
                if (kind != 3) { Primary = Rhino5NlsDirectComparer.Primary(value); Weight = (original & 0x00ff0000) | Primary | 0x02000000; return; }
                if ((value & 255) >= 2)
                {
                    uint priorCase = Case(value);
                    Primary = 0x2200 | ((value & 255) & (aw == 0 ? 255u : 0x87u));
                    Extra = (priorCase & 6) | ((aw == 0 ? 4u : 5u) << 8) | ((priorCase & 0x24) << 16) | (((aw == 0 ? priorCase : currentCase) & 5) << 24);
                    Weight = (original & 0x00ff0000) | Primary | 0x02000000; return;
                }
                previous--;
            }
            Primary = 65535; Weight = 65535; Extra = 0;
        }
        private static bool Leading(char c) => c is >= '\u1100' and <= '\u115f' or >= '\ua960' and <= '\ua97c';
        private static bool Vowel(char c) => c is >= '\u1160' and <= '\u11a7' or >= '\ud7b0' and <= '\ud7c6';
        private static bool Trailing(char c) => c is >= '\u11a8' and <= '\u11ff' or >= '\ud7cb' and <= '\ud7fb';
    }
    private struct JamoAccumulator
    {
        public bool Old;
        public uint LeadingIndex, VowelIndex, TrailingIndex, LeadingExtra, VowelExtra, TrailingExtra;
        public void Merge(char c, int kind)
        {
            int offset = JamoIndex(c) * 8;
            if (offset < 0 || offset + 5 > Jamo.Length) throw new InvalidDataException("Jamo state escaped the approved input table.");
            Old |= Jamo[offset] != 0;
            LeadingIndex = Math.Max(LeadingIndex, Jamo[offset + 1]); VowelIndex = Math.Max(VowelIndex, Jamo[offset + 2]); TrailingIndex = Math.Max(TrailingIndex, Jamo[offset + 3]);
            if (kind == 1) LeadingExtra = Math.Max(LeadingExtra, Jamo[offset + 4]);
            else if (kind == 2) VowelExtra = Math.Max(VowelExtra, Jamo[offset + 4]);
            else TrailingExtra = Math.Max(TrailingExtra, Jamo[offset + 4]);
        }
    }
}
