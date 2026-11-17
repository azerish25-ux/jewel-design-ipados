using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class NlsDirectExhaustiveAssertions
{
    public static string RunExhaustive()
    {
        using var metadata = JsonDocument.Parse(Load("NlsDirectExhaustiveMetadata"));
        byte[] native = Load("NlsDirectExhaustiveResults");
        if (native.Length != 4456448 || Convert.ToHexStringLower(SHA256.HashData(native)) != metadata.RootElement.GetProperty("NativeRecordsSha256").GetString()) throw new Exception("Direct exhaustive native digest.");
        var comparer = Rhino5NlsDirectComparer.Reference60502;
        var lower = Rhino5UnicodeNlsLowercase.Reference60502;
        int at = 0;
        for (int key = 0; key <= 0x10ffff; key++)
        {
            string source = Text(key);
            Check(comparer, source, "", native, ref at);
            Check(comparer, source, "a", native, ref at);
            Check(comparer, source, Text(Math.Max(0, key - 1)), native, ref at);
            Check(comparer, source, lower.Lower(source), native, ref at);
        }
        if (at != native.Length) throw new Exception("Direct exhaustive coverage.");
        return $"PASS direct NLS1 exhaustive:{at} exact native comparisons/0 mismatches; recorded fixed profile, wrapper/CRT/ANSI/state/document/PC/physical remain open.";
    }
    public static string RunJamo()
    {
        using var metadata = JsonDocument.Parse(Load("NlsDirectJamoMetadata"));
        byte[] native = Load("NlsDirectJamoResults");
        var root = metadata.RootElement;
        if (native.Length != 6507500 || Convert.ToHexStringLower(SHA256.HashData(native)) != root.GetProperty("NativeRecordsSha256").GetString()) throw new Exception("Direct Jamo native digest.");
        int[] leading = root.GetProperty("Leading").EnumerateArray().Select(c => c.GetInt32()).ToArray(), vowels = root.GetProperty("Vowels").EnumerateArray().Select(c => c.GetInt32()).ToArray(), trailing = root.GetProperty("Trailing").EnumerateArray().Select(c => c.GetInt32()).ToArray();
        var comparer = Rhino5NlsDirectComparer.Reference60502;
        int at = 0;
        for (int l = 0; l < leading.Length; l++) for (int v = 0; v < vowels.Length; v++) for (int t = 0; t < trailing.Length; t++)
        {
            string source = Three(leading[l], vowels[v], trailing[t]);
            Check(comparer, source, new string([(char)leading[l], (char)vowels[v]]), native, ref at);
            Check(comparer, source, Three(leading[l], vowels[v], trailing[Math.Max(0, t - 1)]), native, ref at);
            Check(comparer, source, Three(leading[l], vowels[Math.Max(0, v - 1)], trailing[t]), native, ref at);
            Check(comparer, source, Three(leading[Math.Max(0, l - 1)], vowels[v], trailing[t]), native, ref at);
        }
        if (at != native.Length) throw new Exception("Direct Jamo coverage.");
        return $"PASS direct NLS1 Hangul:{at} exact native comparisons/0 mismatches; all supported triple inputs, wrapper/CRT/ANSI/state/document/PC/physical remain open.";
    }
    public static string RunStress()
    {
        using var fixture = JsonDocument.Parse(Load("NlsDirectStressReference"));
        var comparer = Rhino5NlsDirectComparer.Reference60502;
        int count = 0;
        foreach (var item in fixture.RootElement.GetProperty("Pairs").EnumerateArray())
        {
            string a = Decode(item.GetProperty("FirstUtf16Base64").GetString()!), b = Decode(item.GetProperty("SecondUtf16Base64").GetString()!);
            int expected = item.GetProperty("NativeResult").GetInt32() - 2;
            int actual = comparer.Compare(a, b);
            if (actual != expected) throw new Exception("Direct stress mismatch " + count + "/" + item.GetProperty("Group").GetString() + ":" + actual + "/" + expected);
            count++;
        }
        if (count != 103280) throw new Exception("Direct stress coverage.");
        int guards = 0;
        try { comparer.Compare(null!, "a"); throw new Exception("Null first accepted."); } catch (ArgumentNullException) { guards++; }
        try { comparer.Compare("a", null!); throw new Exception("Null second accepted."); } catch (ArgumentNullException) { guards++; }
        try { comparer.Compare("a", "b", -1); throw new Exception("Negative budget accepted."); } catch (ArgumentOutOfRangeException) { guards++; }
        try { comparer.Compare("aa", "b", 1); throw new Exception("First budget ignored."); } catch (InvalidDataException) { guards++; }
        try { comparer.Compare("a", "bb", 1); throw new Exception("Second budget ignored."); } catch (InvalidDataException) { guards++; }
        if (comparer.Compare("A", "a", 1) != 0) throw new Exception("Exact source budget rejected."); guards++;
        if (comparer.Compare("a\0z", "A\0y") != 0) throw new Exception("C terminator was not preserved."); guards++;
        return $"PASS direct NLS1 stress:{count} exact native comparisons/{guards} null/budget/terminator guards; original wrapper/CRT/ANSI/state/document/PC/physical remain open.";
    }
    private static void Check(Rhino5NlsDirectComparer comparer, string first, string second, byte[] native, ref int at)
    {
        int expected = native[at] - 2, actual = comparer.Compare(first, second);
        if (expected != actual) throw new Exception("Exhaustive direct mismatch " + at + ":" + actual + "/" + expected);
        at++;
    }
    private static string Text(int key) => key <= 65535 ? new string((char)key, 1) : char.ConvertFromUtf32(key);
    private static string Three(int a, int b, int c) => new([(char)a, (char)b, (char)c]);
    private static byte[] Load(string name)
    {
        using var stream = typeof(NlsDirectExhaustiveAssertions).Assembly.GetManifestResourceStream(name) ?? throw new Exception("Direct native fixture missing: " + name);
        using var compressed = new BrotliStream(stream, CompressionMode.Decompress);
        using var bytes = new MemoryStream(); compressed.CopyTo(bytes); return bytes.ToArray();
    }
    private static string Decode(string encoded)
    {
        byte[] bytes = Convert.FromBase64String(encoded);
        if ((bytes.Length & 1) != 0) throw new InvalidDataException();
        char[] output = new char[bytes.Length / 2];
        for (int i = 0; i < output.Length; i++) output[i] = (char)BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(i * 2));
        return new(output);
    }
}
