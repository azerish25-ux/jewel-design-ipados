using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;

namespace CandidateGeometryContractChecks;
public static class UnicodeNlsLowerAssertions
{
    public static string Run()
    {
        using var resource = typeof(UnicodeNlsLowerAssertions).Assembly.GetManifestResourceStream("WindowsNls1LowerReference") ?? throw new Exception("NLS1 native fixture missing.");
        using var compressed = new BrotliStream(resource, CompressionMode.Decompress);
        using var fixture = JsonDocument.Parse(compressed);
        var data = fixture.RootElement;
        var lower = Rhino5UnicodeNlsLowercase.Reference60502;
        if (lower.SortVersion != 0x60502 || lower.Locale != 1 || lower.Flags != 0x100) throw new Exception("NLS1 profile mismatch.");
        using var totalHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        byte[] header = new byte[8];
        int cases = 0;
        foreach (var plane in data.GetProperty("Planes").EnumerateArray())
        {
            int index = plane.GetProperty("Index").GetInt32();
            using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            int first = index << 16;
            for (int key = first; key < first + 65536; key++)
            {
                string source = index == 0 ? new string((char)key, 1) : char.ConvertFromUtf32(key);
                string actual = lower.Lower(source);
                byte[] bytes = Utf16(actual);
                BinaryPrimitives.WriteInt32LittleEndian(header, actual.Length);
                BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(4), 0);
                hash.AppendData(header); hash.AppendData(bytes);
                totalHash.AppendData(header); totalHash.AppendData(bytes);
                cases++;
            }
            if (Convert.ToHexStringLower(hash.GetHashAndReset()) != plane.GetProperty("NativeSha256").GetString())
                throw new Exception("Complete native Unicode lower plane mismatch " + index);
        }
        if (cases != 1114112 || Convert.ToHexStringLower(totalHash.GetHashAndReset()) != data.GetProperty("NativeRecordsSha256").GetString())
            throw new Exception("Complete Unicode lower native digest mismatch.");
        int contexts = 0;
        foreach (var item in data.GetProperty("Controls").EnumerateArray())
        {
            string source = Decode(Convert.FromBase64String(item.GetProperty("InputUtf16Base64").GetString()!));
            byte[] expected = Convert.FromBase64String(item.GetProperty("OutputUtf16Base64").GetString()!);
            if (!Utf16(lower.Lower(source)).AsSpan().SequenceEqual(expected)) throw new Exception("Native context lower mismatch " + contexts);
            contexts++;
        }
        if (contexts != 3329) throw new Exception("Native context coverage mismatch.");
        int guards = 0;
        try { lower.Lower(null!); throw new Exception("Null lower accepted."); } catch (ArgumentNullException) { guards++; }
        try { lower.Lower("A", -1); throw new Exception("Negative budget accepted."); } catch (ArgumentOutOfRangeException) { guards++; }
        try { lower.Lower("AB", 1); throw new Exception("Lower source budget ignored."); } catch (InvalidDataException) { guards++; }
        if (lower.Lower("AB", 2) != "ab") throw new Exception("Exact lower budget rejected."); guards++;
        if (lower.Lower("", 0) != "") throw new Exception("Empty original lower failed."); guards++;
        if (lower.Lower("A\0B") != "a\0b") throw new Exception("Stored-length mapping stopped at embedded NUL."); guards++;
        if (lower.Lower("\ud800A\udc00") != "\ud800a\udc00") throw new Exception("Unpaired surrogate was replaced."); guards++;
        return $"PASS Unicode NLS1 lower:{cases} exact native unit/scalar mappings/{contexts} native contexts/{guards} null/budget/UTF16 guards; explicit captured profile, full NLS comparison/CRT/ANSI/document/PC/physical remain open.";
    }
    private static string Decode(byte[] bytes)
    {
        if ((bytes.Length & 1) != 0) throw new InvalidDataException("Odd native UTF16 size.");
        char[] text = new char[bytes.Length / 2];
        for (int i = 0; i < text.Length; i++) text[i] = (char)BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(i * 2));
        return new(text);
    }
    private static byte[] Utf16(string text)
    {
        byte[] bytes = new byte[text.Length * 2];
        for (int i = 0; i < text.Length; i++) BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(i * 2), text[i]);
        return bytes;
    }
}
