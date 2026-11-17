using System.Buffers.Binary;
using System.Security.Cryptography;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class NlsComparisonInputAssertions
{
    public static string Run()
    {
        var input = Rhino5NlsComparisonInputs.Reference60502;
        if (input.Locale != 1 || input.SortVersion != 0x60502 || input.CharacterCount != 65536 || input.ExpansionCount != 730) throw new Exception("Comparison input profile identity differs.");
        byte[] weights = new byte[65536 * 4];
        for (int unit = 0; unit < 65536; unit++) BinaryPrimitives.WriteUInt32LittleEndian(weights.AsSpan(unit * 4), input.GetPackedWeight((char)unit));
        if (Convert.ToHexStringLower(SHA256.HashData(weights)) != "42ef9f244f091136a6a94ddf747b3d13e4cb1bf7b8367956abcdf0bb42affba0") throw new Exception("Complete active native weight array mismatch.");
        byte[] expansions = new byte[730 * 4];
        for (int i = 0; i < 730; i++)
        {
            var pair = input.GetExpansion(i);
            BinaryPrimitives.WriteUInt16LittleEndian(expansions.AsSpan(i * 4), pair.First);
            BinaryPrimitives.WriteUInt16LittleEndian(expansions.AsSpan(i * 4 + 2), pair.Second);
        }
        if (Convert.ToHexStringLower(SHA256.HashData(expansions)) != "9d68be72650078da8ea7e72785b37abc8ffaaa87fcdd4f8ae507d0dcf258aa3e") throw new Exception("Complete native expansion array mismatch.");
        int guards = 0;
        try { input.GetExpansion(-1); throw new Exception("Negative expansion accepted."); } catch (ArgumentOutOfRangeException) { guards++; }
        try { input.GetExpansion(730); throw new Exception("Past-end expansion accepted."); } catch (ArgumentOutOfRangeException) { guards++; }
        return $"PASS NLS1 comparison inputs:65536 exact active native weights/730 exact ordered expansions/{guards} expansion-index guards; full direct comparison/Jamo/wrapper/CRT/ANSI/document/PC/physical remain open.";
    }
}
