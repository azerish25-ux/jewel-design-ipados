using System.Buffers.Binary;
using System.IO.Compression;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class NlsDirectComparisonAssertions
{
    public static string Run()
    {
        using var stream = typeof(NlsDirectComparisonAssertions).Assembly.GetManifestResourceStream("NlsDirectStringReference") ?? throw new Exception("Direct comparison reference missing.");
        using var compressed = new BrotliStream(stream, CompressionMode.Decompress);
        using var fixture = JsonDocument.Parse(compressed);
        string[] strings = fixture.RootElement.GetProperty("Strings").EnumerateArray().Select(s => Decode(Convert.FromBase64String(s.GetProperty("InputUtf16Base64").GetString()!))).ToArray();
        var comparer = Rhino5NlsDirectComparer.Reference60502;
        int count = 0, disagreements = 0;
        foreach (var pair in fixture.RootElement.GetProperty("Pairs").EnumerateArray())
        {
            int expected = pair.GetProperty("NativeResult").GetInt32() - 2;
            int actual = comparer.Compare(strings[pair.GetProperty("Left").GetInt32()], strings[pair.GetProperty("Right").GetInt32()]);
            if (actual != expected) throw new Exception("Direct native comparison mismatch at " + count + ":" + actual + "/" + expected);
            if (expected != pair.GetProperty("SortKeySign").GetInt32()) disagreements++;
            count++;
        }
        if (count != 40663 || disagreements != 213) throw new Exception("Direct comparison reference coverage.");
        return $"PASS direct NLS1 strings:{count} exact native comparisons/{disagreements} preserved key-order disagreements; original wrapper/CRT/ANSI/state/document/PC/physical acceptance remain open.";
    }
    private static string Decode(byte[] bytes)
    {
        if ((bytes.Length & 1) != 0) throw new InvalidDataException();
        char[] output = new char[bytes.Length / 2];
        for (int i = 0; i < output.Length; i++) output[i] = (char)BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(i * 2));
        return new(output);
    }
}
