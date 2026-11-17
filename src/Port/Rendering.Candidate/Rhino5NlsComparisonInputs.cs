using System.Buffers.Binary;
using System.Security.Cryptography;

namespace JewelDesign.Rendering.Candidate;

/// <summary>
/// Immutable fixed-profile input data for the direct NLS1 comparison engine.
/// Reconstructed from approved public weights and original expansion order, with complete
/// byte equality against the active owned-process native tables. This is not a comparer.
/// </summary>
public sealed class Rhino5NlsComparisonInputs
{
    private readonly uint[] weights;
    private readonly uint[] expansions;
    public uint Locale { get; }
    public uint SortVersion { get; }
    public int CharacterCount => weights.Length;
    public int ExpansionCount => expansions.Length;
    public static Rhino5NlsComparisonInputs Reference60502 { get; } = Load();

    private Rhino5NlsComparisonInputs(byte[] bytes)
    {
        if (bytes.Length != 265088 || Convert.ToHexStringLower(SHA256.HashData(bytes)) != "79d2e430fb8e0ee7b9558cdc3119dc609e2f1798392c50a63a4895bbc87533dd" ||
            !bytes.AsSpan(0, 8).SequenceEqual("JDNLSI01"u8))
            throw new InvalidDataException("Approved NLS comparison input resource differs.");
        Locale = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(8));
        SortVersion = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(12));
        int weightCount = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(16)));
        int expansionCount = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(20)));
        if (Locale != 1 || SortVersion != 0x60502 || weightCount != 65536 || expansionCount != 730)
            throw new InvalidDataException("NLS comparison input profile is unsupported.");
        weights = new uint[weightCount];
        expansions = new uint[expansionCount];
        for (int i = 0; i < weights.Length; i++) weights[i] = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(24 + i * 4));
        for (int i = 0; i < expansions.Length; i++) expansions[i] = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(24 + weights.Length * 4 + i * 4));
        foreach (uint weight in weights)
            if (((weight >> 8) & 255) == 2 && (weight >> 16) >= expansions.Length)
                throw new InvalidDataException("Native expansion marker escaped its table.");
    }

    private static Rhino5NlsComparisonInputs Load()
    {
        using var resource = typeof(Rhino5NlsComparisonInputs).Assembly.GetManifestResourceStream("NlsComparisonInputs60502")
            ?? throw new InvalidDataException("Native comparison inputs are missing.");
        using var bytes = new MemoryStream();
        resource.CopyTo(bytes);
        return new(bytes.ToArray());
    }

    /// <summary>Returns native packed AW,SM,DW,CW bytes, least significant first. Every UTF16 unit is defined.</summary>
    public uint GetPackedWeight(char unit) => weights[unit];

    /// <summary>Preserves native expansion indices rather than reordering pairs by source character.</summary>
    public (char First, char Second) GetExpansion(int index)
    {
        if ((uint)index >= (uint)expansions.Length) throw new ArgumentOutOfRangeException(nameof(index));
        uint pair = expansions[index];
        return ((char)(pair & 65535), (char)(pair >> 16));
    }
}
