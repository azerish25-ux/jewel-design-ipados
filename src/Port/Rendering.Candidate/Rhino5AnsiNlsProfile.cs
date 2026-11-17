using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace JewelDesign.Rendering.Candidate;

public enum Rhino5AnsiCompareRoute { PointerGuard, CrtAscii, NlsBytes }
public readonly record struct Rhino5AnsiUtf8Prefix(byte[] Bytes, uint QueryErrors, uint WriteErrors, int ConsumedUtf16Units, int CPrefixUtf16Units);
public readonly record struct Rhino5AnsiLowerResult(string Value, uint QueryErrors, uint WriteErrors, uint DecodeErrors, bool NlsAttempted, bool UsedCrtFallback, int NlsError, int LegacyReplacementGroups = 0);
public readonly record struct Rhino5AnsiComparisonResult(int Value, Rhino5AnsiCompareRoute Route, uint FirstQueryErrors = 0, uint SecondQueryErrors = 0, uint FirstWriteErrors = 0, uint SecondWriteErrors = 0);

/// <summary>
/// Recorded original locale1 ANSI value operations: UTF16 C-prefix -> UTF8 -> CP1256 NLS.
/// Uses native-capture-derived byte tables and the original strict-prefix/masked-decoder policies.
/// This is an explicit versioned profile, not current runtime/locale selection or an errno emulator.
/// </summary>
public sealed class Rhino5AnsiNlsProfile
{
    public const string DataSha256 = "3a88c315e6ffe2092d9e4f923ecab1a9b8fdf4976bcc438c636c0a899da2324d";
    public static Rhino5AnsiNlsProfile Reference60502 { get; } = new();
    private readonly ushort[] decode = new ushort[256];
    private readonly byte[] lower = new byte[256];

    private Rhino5AnsiNlsProfile()
    {
        using var stream = typeof(Rhino5AnsiNlsProfile).Assembly.GetManifestResourceStream("AnsiNls1Profile60502") ?? throw new InvalidDataException("Recorded ANSI profile is missing.");
        using var memory = new MemoryStream(); stream.CopyTo(memory); byte[] bytes = memory.ToArray();
        if (bytes.Length != 800 || Convert.ToHexStringLower(SHA256.HashData(bytes)) != DataSha256 || !bytes.AsSpan(0, 8).SequenceEqual("JDANSI01"u8)) throw new InvalidDataException("Recorded ANSI profile identity changed.");
        uint[] expected = [1, 1, 1256, 0x100, 0x20001, 0x60502];
        for (int i = 0; i < expected.Length; i++) if (BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(8 + i * 4)) != expected[i]) throw new InvalidDataException("Unsupported ANSI profile header.");
        for (int i = 0; i < 256; i++) decode[i] = BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(32 + i * 2));
        bytes.AsSpan(544, 256).CopyTo(lower);
    }

    private static void ByteBudget(int length, int maximum)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximum);
        if (length > maximum) throw new InvalidDataException("ANSI input exceeds its explicit byte budget.");
    }
    private static void TextBudget(string? source, int maximumUtf16Units, int maximumUtf8Bytes)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximumUtf16Units); ArgumentOutOfRangeException.ThrowIfNegative(maximumUtf8Bytes);
        if (source is not null && source.Length > maximumUtf16Units) throw new InvalidDataException("ANSI input exceeds its explicit UTF16 budget.");
    }

    /// <summary>Explicit-count CP1256 decoding; unlike comparison, embedded zero bytes remain in the result.</summary>
    public string DecodeBytes(ReadOnlySpan<byte> source, int maximumBytes = int.MaxValue)
    {
        ByteBudget(source.Length, maximumBytes); char[] chars = new char[source.Length];
        for (int i = 0; i < chars.Length; i++) chars[i] = (char)decode[source[i]];
        return new(chars);
    }

    /// <summary>Actual byte lower value behavior. Empty explicit source count returns the recorded error87.</summary>
    public bool TryLowerBytes(ReadOnlySpan<byte> source, out byte[] result, out int win32Error, int maximumBytes = int.MaxValue)
    {
        ByteBudget(source.Length, maximumBytes);
        if (source.IsEmpty) { result = []; win32Error = 87; return false; }
        result = new byte[source.Length]; for (int i = 0; i < result.Length; i++) result[i] = lower[source[i]];
        win32Error = 0; return true;
    }

    public int CompareBytes(ReadOnlySpan<byte> first, ReadOnlySpan<byte> second, int maximumBytes = int.MaxValue)
    {
        ByteBudget(first.Length, maximumBytes); ByteBudget(second.Length, maximumBytes);
        int a = first.IndexOf((byte)0), b = second.IndexOf((byte)0);
        if (a >= 0) first = first[..a]; if (b >= 0) second = second[..b];
        return Rhino5NlsDirectComparer.Reference60502.Compare(DecodeBytes(first), DecodeBytes(second));
    }

    /// <summary>Models w2c_size followed by w2c at exactly the measured strict-prefix capacity.</summary>
    public Rhino5AnsiUtf8Prefix EncodePrefix(string source, int maximumUtf16Units = int.MaxValue, int maximumUtf8Bytes = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(source); TextBudget(source, maximumUtf16Units, maximumUtf8Bytes);
        int end = source.IndexOf('\0'); if (end < 0) end = source.Length;
        var bytes = new List<byte>(Math.Min(end, 256)); int i = 0; uint queryErrors = 0; Span<byte> encoded = stackalloc byte[4];
        while (i < end)
        {
            uint scalar = source[i]; int width = 1;
            if (char.IsHighSurrogate(source[i]))
            {
                if (i + 1 >= end || !char.IsLowSurrogate(source[i + 1])) { queryErrors = 16; break; }
                scalar = 0x10000u + (((uint)source[i] - 0xd800) << 10) + (uint)source[i + 1] - 0xdc00; width = 2;
            }
            else if (char.IsLowSurrogate(source[i])) { queryErrors = 16; break; }
            int size = EncodeScalar(scalar, encoded);
            if (size > maximumUtf8Bytes - bytes.Count) throw new InvalidDataException("ANSI encoded prefix exceeds its explicit byte budget.");
            for (int j = 0; j < size; j++) bytes.Add(encoded[j]);
            i += width;
        }
        // A masked replacement needs three further bytes, but strict measurement leaves no room for it.
        uint writeErrors = queryErrors != 0 && bytes.Count != 0 ? 18u : 0u;
        return new(bytes.ToArray(), queryErrors, writeErrors, i, end);
    }

    public string Lower(string source, Rhino5Crt100FilenameProfile crt, int maximumUtf16Units = int.MaxValue, int maximumUtf8Bytes = int.MaxValue) => LowerDetailed(source, crt, maximumUtf16Units, maximumUtf8Bytes).Value;
    public Rhino5AnsiLowerResult LowerDetailed(string source, Rhino5Crt100FilenameProfile crt, int maximumUtf16Units = int.MaxValue, int maximumUtf8Bytes = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(source); ArgumentNullException.ThrowIfNull(crt); TextBudget(source, maximumUtf16Units, maximumUtf8Bytes);
        if (source.Length == 0) return new(source, 0, 0, 0, false, false, 0);
        var prefix = EncodePrefix(source, maximumUtf16Units, maximumUtf8Bytes);
        if (!TryLowerBytes(prefix.Bytes, out byte[] mapped, out int error, maximumUtf8Bytes)) return new(crt.Lower(source, maximumUtf16Units), prefix.QueryErrors, prefix.WriteErrors, 0, true, true, error);
        var decoded = DecodeTransformedPrefix(mapped);
        return new(decoded.Value, prefix.QueryErrors, prefix.WriteErrors, 0, true, false, 0, decoded.Replacements);
    }

    public int CompareRaw(string? first, string? second, Rhino5Crt100FilenameProfile crt, int maximumUtf16Units = int.MaxValue, int maximumUtf8Bytes = int.MaxValue) => CompareDetailed(first, second, crt, maximumUtf16Units, maximumUtf8Bytes).Value;
    public Rhino5AnsiComparisonResult CompareDetailed(string? first, string? second, Rhino5Crt100FilenameProfile crt, int maximumUtf16Units = int.MaxValue, int maximumUtf8Bytes = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(crt); TextBudget(first, maximumUtf16Units, maximumUtf8Bytes); TextBudget(second, maximumUtf16Units, maximumUtf8Bytes);
        if (first is null) return new(second is null ? 0 : -1, Rhino5AnsiCompareRoute.PointerGuard);
        if (second is null) return new(1, Rhino5AnsiCompareRoute.PointerGuard);
        int i = 0;
        while (i < first.Length && i < second.Length && first[i] != 0 && second[i] != 0 && first[i] < 128 && second[i] < 128) i++;
        if (i == first.Length || i == second.Length || first[i] == 0 || second[i] == 0) return new(crt.CompareRaw(first, second, maximumUtf16Units), Rhino5AnsiCompareRoute.CrtAscii);
        var a = EncodePrefix(first, maximumUtf16Units, maximumUtf8Bytes); var b = EncodePrefix(second, maximumUtf16Units, maximumUtf8Bytes);
        return new(CompareBytes(a.Bytes, b.Bytes, maximumUtf8Bytes), Rhino5AnsiCompareRoute.NlsBytes, a.QueryErrors, b.QueryErrors, a.WriteErrors, b.WriteErrors);
    }

    public Rhino5FilenameSemantics CreateFilenameSemantics(Rhino5Crt100FilenameProfile crt)
    {
        ArgumentNullException.ThrowIfNull(crt);
        return new("original-ANSI/UTF8-CP1256/60502/CRT-" + crt.LocaleName, value => Lower(value, crt), (left, right) =>
        {
            if (string.IsNullOrEmpty(right) || right[0] == 0) return string.IsNullOrEmpty(left) ? 0 : 1;
            if (string.IsNullOrEmpty(left)) return -1;
            return CompareRaw(left, right, crt);
        });
    }

    private static int EncodeScalar(uint scalar, Span<byte> bytes)
    {
        if (scalar < 0x80) { bytes[0] = (byte)scalar; return 1; }
        if (scalar < 0x800) { bytes[0] = (byte)(0xc0 | (scalar >> 6)); bytes[1] = (byte)(0x80 | (scalar & 63)); return 2; }
        if (scalar < 0x10000) { bytes[0] = (byte)(0xe0 | (scalar >> 12)); bytes[1] = (byte)(0x80 | ((scalar >> 6) & 63)); bytes[2] = (byte)(0x80 | (scalar & 63)); return 3; }
        bytes[0] = (byte)(0xf0 | (scalar >> 18)); bytes[1] = (byte)(0x80 | ((scalar >> 12) & 63)); bytes[2] = (byte)(0x80 | ((scalar >> 6) & 63)); bytes[3] = (byte)(0x80 | (scalar & 63)); return 4;
    }

    // Private because this is the reachable mapped-prefix domain, not a replacement for the full legacy UTF8 API.
    private static (string Value, int Replacements) DecodeTransformedPrefix(ReadOnlySpan<byte> bytes)
    {
        var result = new StringBuilder(); int replacements = 0;
        for (int i = 0; i < bytes.Length;)
        {
            byte first = bytes[i]; uint scalar; int width;
            if (first < 128) { scalar = first; width = 1; }
            else if (first is >= 0xc2 and <= 0xdf) { scalar = (uint)(first & 31); width = 2; }
            else if (first is >= 0xe0 and <= 0xef) { scalar = (uint)(first & 15); width = 3; }
            else if (first is >= 0xf0 and <= 0xf4) { scalar = (uint)(first & 7); width = 4; }
            else throw new InvalidDataException("Unexpected mapped-prefix UTF8 lead byte.");
            if (width > bytes.Length - i) throw new InvalidDataException("Unexpected truncated mapped UTF8 prefix.");
            for (int j = 1; j < width; j++) { byte continuation = bytes[i + j]; if ((continuation & 0xc0) != 0x80) throw new InvalidDataException("Unexpected mapped-prefix continuation."); scalar = (scalar << 6) | (uint)(continuation & 63); }
            if (scalar > 0x10ffff)
            {
                if (width != 4 || first != 0xf4 || bytes[i + 1] != 0x9c) throw new InvalidDataException("Unexpected mapped-prefix invalid scalar.");
                // Original masked ON_DecodeUTF8 consumes the entire four-byte group and emits one replacement.
                result.Append('\ufffd'); replacements++;
                // The original structural helper returns4/status0 here; its outer decoder replaces without setting an error bit.
            }
            else
            {
                if (scalar is >= 0xd800 and <= 0xdfff || width == 2 && scalar < 128 || width == 3 && scalar < 0x800 || width == 4 && scalar < 0x10000) throw new InvalidDataException("Unexpected mapped-prefix scalar form.");
                if (scalar < 0x10000) result.Append((char)scalar);
                else { uint value = scalar - 0x10000; result.Append((char)(0xd800 + (value >> 10))); result.Append((char)(0xdc00 + (value & 1023))); }
            }
            i += width;
        }
        return (result.ToString(), replacements);
    }
}
