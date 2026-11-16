using System.Buffers.Binary;
using System.Security.Cryptography;

namespace JewelDesign.Rendering.Candidate;

/// <summary>
/// Versioned offline LCMapStringW(1,0x100) mapping captured from public Windows APIs.
/// Processes the stored UTF16 length, including embedded NUL and unpaired surrogates.
/// This is the lowercase operation only; it is not a Windows filename comparer or a CRT fallback.
/// </summary>
public sealed class Rhino5UnicodeNlsLowercase
{
    private readonly uint[] inputs;
    private readonly uint[] outputs;
    public uint Locale { get; }
    public uint Flags { get; }
    /// <summary>Captured NLS sorting version; the mapping's SHA256 separately identifies its exact contents.</summary>
    public uint SortVersion { get; }
    public uint EffectiveSortId { get; }
    public string MappingSha256 { get; }
    public static Rhino5UnicodeNlsLowercase Reference60502 { get; } = LoadReference();

    private Rhino5UnicodeNlsLowercase(byte[] bytes)
    {
        MappingSha256 = Convert.ToHexStringLower(SHA256.HashData(bytes));
        if (MappingSha256 != "06e58c3cbbd38bcce4af8e99bb4f0693148fb5144ca9db23ef65fc9711d3ab70" ||
            bytes.Length != 8136 || !bytes.AsSpan(0, 8).SequenceEqual("JDNLSL01"u8))
            throw new InvalidDataException("The approved native NLS1 lowercase mapping changed.");
        Locale = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(8));
        Flags = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(12));
        SortVersion = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(16));
        EffectiveSortId = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(20));
        int count = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(24)));
        if (Locale != 1 || Flags != 0x100 || SortVersion != 0x60502 || EffectiveSortId != 1025 ||
            count != 1013 || BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(28)) != 0)
            throw new InvalidDataException("Unsupported NLS lowercase profile identity.");
        inputs = new uint[count];
        outputs = new uint[count];
        for (int i = 0; i < count; i++)
        {
            uint input = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(32 + i * 8));
            uint output = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(36 + i * 8));
            if (input > 0x10ffff || output > 0x10ffff || input == output ||
                (input <= 65535) != (output <= 65535) ||
                input is >= 0xd800 and <= 0xdfff || output is >= 0xd800 and <= 0xdfff ||
                (i != 0 && input <= inputs[i - 1]))
                throw new InvalidDataException("Malformed native NLS mapping record.");
            inputs[i] = input;
            outputs[i] = output;
        }
    }

    private static Rhino5UnicodeNlsLowercase LoadReference()
    {
        using var stream = typeof(Rhino5UnicodeNlsLowercase).Assembly.GetManifestResourceStream("Rhino5Nls1Lower60502")
            ?? throw new InvalidDataException("Approved NLS lowercase mapping is missing.");
        using var bytes = new MemoryStream();
        stream.CopyTo(bytes);
        return new(bytes.ToArray());
    }

    /// <summary>Uses captured Windows mapping, independent of the current device's culture/Unicode casing tables.</summary>
    public string Lower(string source, int maximumUtf16Units = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumUtf16Units);
        if (source.Length > maximumUtf16Units)
            throw new InvalidDataException("NLS lowercase source exceeds the explicit UTF16 budget.");
        char[]? destination = null;
        for (int i = 0; i < source.Length; i++)
        {
            uint key = source[i];
            int width = 1;
            if (char.IsHighSurrogate(source[i]) && i + 1 < source.Length && char.IsLowSurrogate(source[i + 1]))
            {
                key = 0x10000u + (((uint)source[i] - 0xd800u) << 10) + (uint)source[i + 1] - 0xdc00u;
                width = 2;
            }
            int found = Array.BinarySearch(inputs, key);
            if (found >= 0)
            {
                destination ??= source.ToCharArray();
                uint value = outputs[found];
                if (width == 1) destination[i] = (char)value;
                else
                {
                    value -= 0x10000;
                    destination[i] = (char)(0xd800 + (value >> 10));
                    destination[i + 1] = (char)(0xdc00 + (value & 1023));
                }
            }
            i += width - 1;
        }
        return destination is null ? source : new(destination);
    }
}
