using System.Buffers.Binary;
using System.Security.Cryptography;
using JewelDesign.Documents;

namespace DocumentContractChecks;

public static class Assertions
{
    public static string Run(string? outputFolder = null)
    {
        using var stream = typeof(Assertions).Assembly.GetManifestResourceStream("LegacyCsg4Fixture")!;
        using var memory = new MemoryStream(); stream.CopyTo(memory);
        var original = memory.ToArray();
        Check(Convert.ToHexString(SHA256.HashData(original)).ToLowerInvariant() == "73e1e22f2f00346d0fee649cecc3b0633c03ee0dbe38ac88a5a2bdd2c40498cd", "Fixture changed");
        var input = (byte[])original.Clone();
        var document = LegacyCsg4Codec.Read(input);
        Check(document.Records.Count == 241 && document.NumericRecordCount == 74, "Fixture record counts");
        Check(document.Records.Count(r => r.Numeric?.Identifier == "Top Thickness") == 2, "Duplicate label fixture");
        ProjectionAssertions.Run(document);
        var editBridgeResult = EditBridgeAssertions.Run(document, original);
        var selected = document.Records[8];
        var selectedNumeric = selected.Numeric ?? throw new Exception("Missing fixture numeric schema");
        Check(selected.TypeGuid == LegacyCsg4Codec.RangeTypeGuid && selectedNumeric.Value == 4.38, "Selected record/schema");
        Check(selectedNumeric.Minimum == 3 && selectedNumeric.Maximum == 6 && selectedNumeric.Step == .01, "Numeric bounds lost");
        input[0] = 0;
        EqualBytes(original, LegacyCsg4Codec.Write(document), "Input aliasing/no-op rebuild");
        var returned = LegacyCsg4Codec.Write(document); returned[0] = 0;
        var recordCopy = selected.CopyOriginalRecordBytes(); recordCopy[0] = 0;
        EqualBytes(original, LegacyCsg4Codec.Write(document), "Output/record payload aliasing");
        var editedDocument = document.WithNumericValue(8, 4.48);
        var rebuilt = LegacyCsg4Codec.Write(document);
        var edited = LegacyCsg4Codec.Write(editedDocument);
        var reopened = LegacyCsg4Codec.Read(edited);
        Check(reopened.Records[8].Numeric!.Value == 4.48 && document.Records[8].Numeric!.Value == 4.38, "Edit/reopen/original mutation");
        for (int i = 0; i < 241; i++)
            if (i != 8) EqualBytes(document.Records[i].CopyOriginalRecordBytes(), reopened.Records[i].CopyOriginalRecordBytes(), "Unedited record lost");
        int valueOffset = selectedNumeric.OriginalValueOffset;
        int crcOffset = selected.OriginalFileOffset + selected.CopyOriginalRecordBytes().Length - 4;
        for (int i = 0; i < original.Length; i++)
            if (original[i] != edited[i] && !(i >= valueOffset && i < valueOffset + 8) && !(i >= crcOffset && i < crcOffset + 4))
                throw new Exception("Unrelated byte changed at " + i);
        var restored = LegacyCsg4Codec.Write(reopened.WithNumericValue(8, 4.38));
        EqualBytes(original, restored, "Reversal must restore metadata, arrays, raw strings and CRCs");
        Check(document.Records[2].Numeric!.Value == 8.667196228921199, "Unrounded ring radius lost");
        Throws<NotSupportedException>(() => document.WithNumericValue(0, 4));
        Throws<ArgumentOutOfRangeException>(() => document.WithNumericValue(-1, 4));
        Throws<ArgumentOutOfRangeException>(() => document.WithNumericValue(241, 4));

        foreach (var count in new[] { 0, 1, 31, 100, 122960, original.Length - 1 })
            Throws<InvalidDataException>(() => LegacyCsg4Codec.Read(original.AsSpan(0, count)));
        Throws<InvalidDataException>(() => LegacyCsg4Codec.Read(original, new LegacyCsg4Limits(MaxFileBytes: original.Length - 1)));
        Throws<ArgumentOutOfRangeException>(() => LegacyCsg4Codec.Read(original, new LegacyCsg4Limits(MaxArrayItems: -1)));
        var corrupt = (byte[])original.Clone(); corrupt[valueOffset] ^= 1;
        Throws<InvalidDataException>(() => LegacyCsg4Codec.Read(corrupt));
        var badEof = (byte[])original.Clone(); badEof[^1] ^= 1;
        Throws<InvalidDataException>(() => LegacyCsg4Codec.Read(badEof));
        var wrongHeader = (byte[])original.Clone(); wrongHeader[31] = (byte)'5';
        Throws<NotSupportedException>(() => LegacyCsg4Codec.Read(wrongHeader));
        const int graphOffset = 122946; // hash-locked fixture envelope, independently decoded in reference proof
        var hugeCount = (byte[])original.Clone(); BinaryPrimitives.WriteInt32LittleEndian(hugeCount.AsSpan(graphOffset + 16), int.MaxValue);
        Throws<InvalidDataException>(() => LegacyCsg4Codec.Read(hugeCount));
        var badText = (byte[])original.Clone(); BinaryPrimitives.WriteInt32LittleEndian(badText.AsSpan(selected.OriginalFileOffset + 16), int.MaxValue);
        LeafCrc(badText, selected);
        Throws<InvalidDataException>(() => LegacyCsg4Codec.Read(badText));

        var info11 = (byte[])original.Clone(); BinaryPrimitives.WriteInt32LittleEndian(info11.AsSpan(66613), 1);
        BinaryPrimitives.WriteUInt32LittleEndian(info11.AsSpan(67031), Crc(info11.AsSpan(66609, 422)));
        EqualBytes(info11, LegacyCsg4Codec.Write(LegacyCsg4Codec.Read(info11)), "Source-defined file-info 1.1 profile");

        var future = (byte[])original.Clone(); BinaryPrimitives.WriteInt32LittleEndian(future.AsSpan(selected.OriginalFileOffset + 8), 99);
        LeafCrc(future, selected);
        var futureDoc = LegacyCsg4Codec.Read(future);
        Check(futureDoc.Records[8].Numeric is null, "Future numeric schema was guessed");
        EqualBytes(future, LegacyCsg4Codec.Write(futureDoc), "Future record not retained opaque");
        Throws<NotSupportedException>(() => futureDoc.WithNumericValue(8, 4));
        var unknown = (byte[])original.Clone();
        Guid.Empty.ToByteArray().CopyTo(unknown, graphOffset + 20 + 8 * 16);
        BinaryPrimitives.WriteUInt32LittleEndian(unknown.AsSpan(original.Length - 16), Crc(unknown.AsSpan(graphOffset + 8, document.Records[0].OriginalFileOffset - graphOffset - 8)));
        var unknownDoc = LegacyCsg4Codec.Read(unknown);
        Check(unknownDoc.Records[8].Numeric is null && unknownDoc.Records[8].TypeGuid == Guid.Empty, "Unknown type coerced");
        EqualBytes(unknown, LegacyCsg4Codec.Write(unknownDoc), "Unknown type not retained");
        var futureGraph = (byte[])original.Clone(); BinaryPrimitives.WriteInt32LittleEndian(futureGraph.AsSpan(graphOffset + 8), 99);
        Throws<NotSupportedException>(() => LegacyCsg4Codec.Read(futureGraph));

        var rawUtf16 = (byte[])original.Clone(); BinaryPrimitives.WriteUInt16LittleEndian(rawUtf16.AsSpan(selected.OriginalFileOffset + 20), 0xd800);
        LeafCrc(rawUtf16, selected);
        var rawDoc = LegacyCsg4Codec.Read(rawUtf16);
        Check(rawDoc.Records[8].Numeric!.Identifier[0] == (char)0xd800, "UTF16 code unit normalized");
        EqualBytes(rawUtf16, LegacyCsg4Codec.Write(rawDoc.WithNumericValue(8, 4.48).WithNumericValue(8, 4.38)), "Raw UTF16 lost during edited record rebuild");
        foreach (var value in new[] { BitConverter.Int64BitsToDouble(long.MinValue), double.PositiveInfinity, BitConverter.Int64BitsToDouble(0x7ff8000000000042L) })
        {
            var data = LegacyCsg4Codec.Write(document.WithNumericValue(8, value));
            Check(BitConverter.DoubleToInt64Bits(LegacyCsg4Codec.Read(data).Records[8].Numeric!.Value) == BitConverter.DoubleToInt64Bits(value), "IEEE754 bits lost");
        }

        if (outputFolder is not null)
        {
            Directory.CreateDirectory(outputFolder);
            File.WriteAllBytes(Path.Combine(outputFolder, "source.csg"), original);
            File.WriteAllBytes(Path.Combine(outputFolder, "rebuilt.csg"), rebuilt);
            File.WriteAllBytes(Path.Combine(outputFolder, "edited.csg"), edited);
            File.WriteAllBytes(Path.Combine(outputFolder, "restored.csg"), restored);
        }
        return editBridgeResult + "; PASS production CSG4 codec: 241 records/74 numeric; structural no-op/edit/reopen/reversal, opaque and bit/text fidelity, CRC/bounds/version failures; no PC app or geometry regeneration claim.";
    }

    private static void LeafCrc(byte[] data, LegacyCsg4Record record)
    {
        int start = record.OriginalFileOffset + 8, length = record.CopyOriginalRecordBytes().Length - 12;
        BinaryPrimitives.WriteUInt32LittleEndian(data.AsSpan(start + length), Crc(data.AsSpan(start, length)));
    }
    private static uint Crc(ReadOnlySpan<byte> data)
    {
        uint crc = uint.MaxValue;
        foreach (var b in data) { crc ^= b; for (int i = 0; i < 8; i++) crc = (crc & 1) == 0 ? crc >> 1 : (crc >> 1) ^ 0xedb88320; }
        return ~crc;
    }
    private static void Check(bool value, string name) { if (!value) throw new Exception(name); }
    private static void EqualBytes(byte[] expected, byte[] actual, string name) => Check(expected.AsSpan().SequenceEqual(actual), name);
    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); } catch (T) { return; } throw new Exception("Expected " + typeof(T).Name);
    }
}
